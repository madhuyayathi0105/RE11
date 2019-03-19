using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;

public partial class Subjectschedularpage : System.Web.UI.Page
{
    //========Removed by sangeetha R on 31 Aug 2014=================
    // DataSet dsgo = new DataSet();
    // DataSet ds2 = new DataSet();
    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    // Hashtable hat = new Hashtable();
    // DataSet ds = new DataSet();
    // DataSet dsprint = new DataSet();

    DataTable newdt = new DataTable();
    static Hashtable htsubject = new Hashtable();
    DAccess2 obi_access = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    static bool forschoolsetting = false;
    bool flag_true = false;
    bool flagstudent = false;
    bool flagstaff = false;

    int icount = 0;
    int rc1 = 0;
    int subjecttype_no = 0;

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string group_code = string.Empty;
    string columnfield = string.Empty;
    string subjectno = string.Empty;
    string rollno = string.Empty;
    string subjectnumber = string.Empty;
    string ddlcollegevalue = string.Empty;
    string ddlbatchvalue = string.Empty;
    string ddlbatchitems = string.Empty;
    string ddlsemvalue = string.Empty;
    string ddlsecvalue = string.Empty;
    string ddlcollegecode = string.Empty;
    string ddldegreevalue = string.Empty;
    string ddlbranchvalue = string.Empty;
    string subjecttypeno = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string sql = string.Empty;
    string chile_index = string.Empty;
    string temp_sec = string.Empty;
    string RollorRegorAdmitNo = string.Empty;

    DataTable dtable = new DataTable();
    DataTable dtable1 = new DataTable();
    DataTable dtable3 = new DataTable();
    DataRow dtrow = null;
    DataRow dtrow2 = null;
    DataRow dtrow3 = null;

    static Hashtable subjs = new Hashtable();
    static Hashtable subjss = new Hashtable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    int height = 0;
    public int sub_count { get; set; }

    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
    FarPoint.Web.Spread.ButtonCellType staf_butt1 = new FarPoint.Web.Spread.ButtonCellType("OneCommand", FarPoint.Web.Spread.ButtonType.PushButton, "Remove");

    protected void Removesecall(object sender, EventArgs e)
    {
        DropDownList ddlsection = (DropDownList)this.usercontrol.FindControl("ddlSec");
        ddlsection.Items.Remove("All");
        DropDownList ddlsection1 = (DropDownList)this.usercontrol1.FindControl("ddlSec");
        ddlsection1.Items.Remove("All");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        RollorRegorAdmitNo = string.Empty;
        lblstustaferr.Visible = false;
        string grouporusercode = string.Empty;
        TabContainer1.Width = 1100;
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            grouporusercode = " group_code='" + group_user + "'";
        }
        else
        {
            grouporusercode = " usercode='" + Session["usercode"].ToString().Trim() + "'";
        }
        Labelerror.Visible = false;
        //Added by srinath 31/8/2013
        DropDownList ddlsection = ((DropDownList)this.usercontrol.FindControl("ddlSec"));
        ddlsection.Items.Remove("All");
        ddlsection.SelectedIndexChanged += new EventHandler(this.Removesecall);
        DropDownList ddlbatchparent = (DropDownList)this.usercontrol.FindControl("ddlBatch");
        DropDownList ddlDegree = (DropDownList)this.usercontrol.FindControl("ddlDegree");
        DropDownList ddlbranchparent = (DropDownList)this.usercontrol.FindControl("ddlBranch");
        DropDownList ddlsemparent = (DropDownList)this.usercontrol.FindControl("ddlSemYr");
        ddlbatchparent.SelectedIndexChanged += new EventHandler(this.Removesecall);
        ddlDegree.SelectedIndexChanged += new EventHandler(this.Removesecall);
        ddlbranchparent.SelectedIndexChanged += new EventHandler(this.Removesecall);
        ddlsemparent.SelectedIndexChanged += new EventHandler(this.Removesecall);
        DropDownList ddlsection1 = ((DropDownList)this.usercontrol1.FindControl("ddlSec"));
        ddlsection1.Items.Remove("All");
        ddlsection1.SelectedIndexChanged += new EventHandler(this.Removesecall);
        DropDownList ddlbatchparent1 = (DropDownList)this.usercontrol1.FindControl("ddlBatch");
        DropDownList ddlbranchparent1 = (DropDownList)this.usercontrol1.FindControl("ddlBranch");
        DropDownList ddlsemparent1 = (DropDownList)this.usercontrol1.FindControl("ddlSemYr");
        DropDownList ddlDegree1 = (DropDownList)this.usercontrol1.FindControl("ddlDegree");
        ddlbatchparent1.SelectedIndexChanged += new EventHandler(this.Removesecall);
        ddlDegree1.SelectedIndexChanged += new EventHandler(this.Removesecall);
        ddlbranchparent1.SelectedIndexChanged += new EventHandler(this.Removesecall);
        ddlsemparent1.SelectedIndexChanged += new EventHandler(this.Removesecall);
        //End
        try
        {
            if (!Page.IsPostBack)
            {

                rbsubacr.Checked = true;
                FpEntry.Visible = false;
                FpEntry.Sheets[0].ColumnCount = 0;
                FpEntry.Sheets[0].RowCount = 0;
                //Fpstaff.Sheets[0].ColumnCount = 0;
                //Fpstaff.Sheets[0].RowCount = 0;
                FindBtn.Enabled = false;
                Save.Enabled = false;
                //Fpstaff.SaveChanges();
                //fsstaff.SaveChanges();
                //Fpstaff.Visible = false;
                gviewstaff.Visible = false;
                FindBtn.Visible = false;
                Chkalterotherdept.Visible = false;
                subjtree.Visible = false;
                //MyStyle.Font.Bold = true;
                //MyStyle.Font.Size = FontUnit.Medium;
                //MyStyle.Font.Name = "Book Antiqua";
                //MyStyle.HorizontalAlign = HorizontalAlign.Center;
                //MyStyle.ForeColor = Color.Black;
                //MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //FpEntry.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                //Fpstaff.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                //FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                ////fsstaff.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                //Fpstaff.Sheets[0].AutoPostBack = true;
                //FpEntry.Sheets[0].AutoPostBack = true;
                Save.Visible = false;
                Button1.Visible = false;
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code='" + Session["group_code"].ToString().Trim().Split(';')[0] + "'";
                }
                else
                {
                    grouporusercode = " usercode='" + Session["usercode"].ToString().Trim() + "'";
                }
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet dssettings = obi_access.select_method_wo_parameter(Master, "Text");
                for (int i = 0; i < dssettings.Tables[0].Rows.Count; i++)
                {
                    if (dssettings.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && dssettings.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dssettings.Tables[0].Rows[i]["settings"].ToString() == "Register No" && dssettings.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dssettings.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && dssettings.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
                printcontrol.Visible = false;
                TabPanel3.Visible = false;   //Session["collegecode"] 
                string minimumabsentsms = obi_access.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString().Trim() + "'");
                string[] splitminimumabsentsms = minimumabsentsms.Split('-');
                if (splitminimumabsentsms.Length == 2)
                {
                    if (splitminimumabsentsms[0].ToString() == "1")
                    {
                        TabPanel3.Visible = true;
                    }
                }
                FpSpread2.Visible = false;
                btnstustaffsave.Visible = false;
                ucstuprint.Visible = false;
                btnstustaffprint.Visible = false;
                rbstusubcode.Checked = true;
                rbstcode.Checked = true;
                // Added By Sridharan 12 Mar 2015
                //{
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code='" + Session["group_code"].ToString().Trim().Split(';')[0] + "'";
                }
                else
                {
                    grouporusercode = " usercode='" + Session["usercode"].ToString().Trim() + "'";
                }
                DataSet schoolds = new DataSet();
                string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = obi_access.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                    if (schoolvalue.Trim() == "0")
                    {
                        Button2.Attributes.Add("style", " font-family: Book Antiqua; font-size: medium; font-weight: bold; height: 31px; width: 34px;");
                    }
                    else
                    {
                        // forschoolsetting = false;
                    }
                }
                //} Sridharan
            }
            lblerror.Visible = false;
            Labelerror.Visible = false;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        loadstudentsubjectchooser();
    }

    public void loadstudentsubjectchooser()
    {
        try
        {
            printcontrol.Visible = false;
            FpEntry.Visible = true;
            Dictionary<string, string> rolldic = new Dictionary<string, string>();
            FpEntry.Sheets[0].AutoPostBack = false;
            //Fpstaff.Sheets[0].AutoPostBack = false;
            ddlcollegevalue = ((DropDownList)this.usercontrol.FindControl("ddlcollege")).SelectedValue.ToString();
            ddlbatchvalue = ((DropDownList)this.usercontrol.FindControl("ddlBatch")).SelectedValue.ToString();
            ddldegreevalue = ((DropDownList)this.usercontrol.FindControl("ddlDegree")).SelectedValue.ToString();
            ddlsecvalue = ((DropDownList)this.usercontrol.FindControl("ddlSec")).SelectedValue.ToString();
            ddlsemvalue = ((DropDownList)this.usercontrol.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlbranchvalue = ((DropDownList)this.usercontrol.FindControl("ddlBranch")).SelectedValue.ToString();
            string sections = string.Empty;
            string strsec = string.Empty;
            if (ddlsecvalue.ToString() == "All")
            {
                Labelerror.Visible = true;
                Labelerror.Text = "Please Select any one section";
                FpEntry.Visible = false;
                Button1.Visible = false;
            }
            else
            {
                sections = ddlsecvalue.ToString();
                if (sections.ToString().ToLower() == "all" || sections.ToString() == string.Empty || sections.ToString() == "-1")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + sections.ToString() + "'";
                }
                FarPoint.Web.Spread.ComboBoxCellType cmbcell = new FarPoint.Web.Spread.ComboBoxCellType();
                FarPoint.Web.Spread.ComboBoxCellType cmbcell1 = new FarPoint.Web.Spread.ComboBoxCellType();
                DataSet ds_cssem = new DataSet();
                string cseme = "select distinct current_semester from registration where degree_code ='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' and cc=0 and delflag=0 and exam_flag!='debar' ";
                ds_cssem = obi_access.select_method_wo_parameter(cseme, "text");
                if (ds_cssem.Tables[0].Rows.Count > 0)
                {
                    string currentsem = ds_cssem.Tables[0].Rows[0]["current_semester"].ToString();
                    if (currentsem == ddlsemvalue.ToString())
                    {
                        DataSet ds_stu_names = new DataSet();
                        //added By Srinath 15/8/2013
                        string strorder = "ORDER BY len(registration.roll_no),registration.roll_no";
                        string serial = obi_access.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                        if (serial != null && serial != "" && serial != "0" && serial.ToLower() != "true")
                        {
                            strorder = "Order by registration.serialno";
                        }
                        else
                        {
                            string orderby_Setting = obi_access.GetFunction("select value from master_Settings where settings='order_by'");
                            if (orderby_Setting == "0")
                            {
                                strorder = "ORDER BY len(registration.roll_no),registration.roll_no";
                            }
                            else if (orderby_Setting == "1")
                            {
                                strorder = "ORDER BY registration.Reg_No";
                            }
                            else if (orderby_Setting == "2")
                            {
                                strorder = "ORDER BY registration.Stud_Name";
                            }
                            else if (orderby_Setting == "0,1,2")
                            {
                                strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Reg_No,registration.stud_name";
                            }
                            else if (orderby_Setting == "0,1")
                            {
                                strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Reg_No";
                            }
                            else if (orderby_Setting == "1,2")
                            {
                                strorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
                            }
                            else if (orderby_Setting == "0,2")
                            {
                                strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Stud_Name";
                            }
                        }
                        //  string stu_namequery = "select roll_no as rollno, stud_name as studentname from registration where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and current_semester='" + ddlsemvalue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by roll_no";
                        string stu_namequery = "select roll_no as rollno, stud_name as studentname,reg_no from registration where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and current_semester='" + ddlsemvalue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' " + strorder + "";//Modified By Srinath 6/3/2014
                        ds_stu_names = obi_access.select_method_wo_parameter(stu_namequery, "text");
                        if (ds_stu_names.Tables[0].Rows.Count > 0)
                        {
                            FpEntry.Visible = false;
                            FpEntry.Visible = true;
                            Button1.Visible = true;
                            FpEntry.Sheets[0].ColumnCount = 0;
                            FpEntry.Sheets[0].ColumnCount = 3;//Modified By Srinath 6/3/2014
                            FpEntry.Sheets[0].RowCount = 0;
                            btnprint.Visible = true;
                            FpEntry.Sheets[0].ColumnHeader.RowCount = 2;
                            FpEntry.Sheets[0].RowHeader.ColumnCount = 1;
                            FpEntry.Sheets[0].ColumnHeader.Visible = true;
                            FpEntry.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
                            //Color c = Fpstaff.ColumnHeader.DefaultStyle.BackColor;
                            FpEntry.ActiveSheetView.SheetCorner.DefaultStyle.BackColor = Color.LightCyan;
                            FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                            FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            FpEntry.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                            FpEntry.Sheets[0].DefaultStyle.Font.Bold = false;
                            FpEntry.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                            FpEntry.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                            FpEntry.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                            FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                            FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
                            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);//Modified By Srinath 6/3/2014
                            FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Roll No";
                            FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";//Modified By Srinath 6/3/2014
                            FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                            FpEntry.Sheets[0].Columns[0].Locked = true;
                            FpEntry.Sheets[0].Columns[1].Locked = true;
                            FpEntry.Sheets[0].Columns[2].Locked = true;
                            //Added By Srinath 6/3/2014
                            if (Session["Rollflag"].ToString() == "0")
                            {
                                FpEntry.Sheets[0].Columns[0].Visible = false;
                            }
                            else
                            {
                                FpEntry.Sheets[0].Columns[0].Visible = true;
                            }
                            if (Session["Regflag"].ToString() == "0")
                            {
                                FpEntry.Sheets[0].Columns[1].Visible = false;
                            }
                            else
                            {
                                FpEntry.Sheets[0].Columns[1].Visible = true;
                            }
                            //==========End
                            //=====Sangeetha R on 26 Aug 2014
                            FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                            FpEntry.Sheets[0].Columns[0].CellType = txtcell;
                            FpEntry.Sheets[0].Columns[1].CellType = txtcell;
                            FpEntry.Sheets[0].Columns[2].CellType = txtcell;
                            FpEntry.Sheets[0].RowCount++;
                            //Added By Srinath 13/8/2013
                            FpEntry.Sheets[0].RowHeader.Cells[0, 0].Text = " ";
                            for (int i = 0; i < ds_stu_names.Tables[0].Rows.Count; i++)
                            {
                                FpEntry.Sheets[0].Columns[0].Width = 100;
                                FpEntry.Sheets[0].Columns[1].Width = 100;
                                FpEntry.Sheets[0].Columns[2].Width = 200;
                                string studentname = ds_stu_names.Tables[0].Rows[i]["studentname"].ToString();
                                rollno = ds_stu_names.Tables[0].Rows[i]["rollno"].ToString();
                                string regno = ds_stu_names.Tables[0].Rows[i]["reg_no"].ToString();
                                FpEntry.Sheets[0].RowCount++;
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].CellType = txtcell;
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].CellType = txtcell;
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].CellType = txtcell;
                                FpEntry.Sheets[0].RowHeader.Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpEntry.Sheets[0].RowCount - 1);//added by Srinath 16/8/2013
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = rollno;
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = regno; //Modified By Srinath 6/3/2014
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = studentname;//Modified By Srinath 6/3/2014
                                if (!rolldic.ContainsKey((FpEntry.Sheets[0].RowCount - 1).ToString()))
                                {
                                    rolldic.Add((FpEntry.Sheets[0].RowCount - 1).ToString(), rollno);
                                }
                            }
                        }
                        //FpEntry.SaveChanges();
                        // saved students subject details
                        //Modified by srinath 16/10/2014
                        DataSet ds_totalsubjects = new DataSet();
                        string total_subjects = "select s.subject_code,s.acronym,s.subject_name,ss.subject_type ,s.subtype_no,sc.* from Subjectchooser sc,subject s,registration r,syllabus_master sy,sub_sem ss where s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and sy.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.subType_no=sc.subtype_no and sy.degree_code=r.degree_code and sy.Batch_Year=r.Batch_Year and sc.semester=r.Current_Semester and sc.roll_no=r.Roll_No and sc.semester=r.Current_Semester and r.degree_code='" + ddlbranchvalue.ToString() + "' and r.batch_year='" + ddlbatchvalue.ToString() + "' and r.current_Semester='" + ddlsemvalue.ToString() + "'";
                        ds_totalsubjects = obi_access.select_method_wo_parameter(total_subjects, "text");
                        //if (ds_totalsubjects.Tables[0].Rows.Count > 0)
                        //{
                        // all subjects 
                        //Modified by srinath 16/10/2014
                        DataSet ds_selectedsubjects = new DataSet();
                        string selected_subjects = "select s.subject_code,s.acronym,s.subject_no,s.subtype_no,s.subject_name from subject s,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and s.syll_code=ss.syll_code and s.syll_code=sy.syll_code and sy.degree_code='" + ddlbranchvalue.ToString() + "' and sy.semester='" + ddlsemvalue.ToString() + "' and sy.batch_year='" + ddlbatchvalue.ToString() + "'";
                        ds_selectedsubjects = obi_access.select_method_wo_parameter(selected_subjects, "text");
                        //column header 
                        DataSet ds_subjects = new DataSet();
                        string theroy_query = "select subject_type,no_of_papers,subType_no,ElectivePap from sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and sy.degree_code='" + ddlbranchvalue.ToString() + "' and sy.semester='" + ddlsemvalue.ToString() + "' and sy.batch_year='" + ddlbatchvalue.ToString() + "'";
                        ds_subjects = obi_access.select_method_wo_parameter(theroy_query, "text");
                        int numberofpapers = 0;
                        int startingcolumn = 3;
                        // bind the subjects based on the paper type
                        for (int subject = 0; subject < ds_subjects.Tables[0].Rows.Count; subject++)
                        {
                            numberofpapers = Convert.ToInt32(ds_subjects.Tables[0].Rows[subject]["no_of_papers"]);
                            string electivepaper = Convert.ToString(ds_subjects.Tables[0].Rows[subject]["ElectivePap"]);
                            for (int subtype = 1; subtype <= numberofpapers; subtype++)
                            {
                                FpEntry.Sheets[0].ColumnCount++;
                                FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 1].Visible = true;
                                if (chkeleective.Checked == true)
                                {
                                    if (electivepaper.Trim().ToLower() != "1" && electivepaper.Trim().ToLower() != "true")
                                    {
                                        FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 1].Visible = false;
                                    }
                                }
                                FpEntry.Sheets[0].ColumnHeader.Cells[1, FpEntry.Sheets[0].ColumnCount - 1].Text = subtype.ToString();
                                FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 1].CellType = cmbcell;
                                FpEntry.Sheets[0].ColumnHeader.Cells[1, FpEntry.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds_subjects.Tables[0].Rows[subject]["subType_no"]);
                                subjecttype_no = Convert.ToInt32(ds_subjects.Tables[0].Rows[subject]["subType_no"]);
                                DataTable dt_selectedsub = new DataTable();
                                dt_selectedsub = ds_selectedsubjects.Tables[0];
                                //rowfilter for to get the subject based on the subjecttype number (particular column)
                                dt_selectedsub.DefaultView.RowFilter = "subtype_no='" + subjecttype_no + "'";
                                DataView dv_selectedsub = new DataView();
                                dv_selectedsub = dt_selectedsub.DefaultView;
                                // convert the dataview values into datatable for get the values
                                newdt = dv_selectedsub.ToTable();
                                string[] sublist = new string[newdt.Rows.Count + 1];
                                for (icount = 0; icount < newdt.Rows.Count; icount++)
                                {
                                    //Modified by srinath 16/10/2014
                                    string snam = newdt.Rows[icount]["acronym"].ToString();
                                    if (rbsubname.Checked == true)
                                    {
                                        snam = newdt.Rows[icount]["subject_name"].ToString();
                                    }
                                    sublist[icount] = newdt.Rows[icount]["subject_code"].ToString() + "-" + snam;
                                    if (subjectno == "")
                                    {
                                        subjectno = newdt.Rows[icount]["subject_no"].ToString();
                                    }
                                    else
                                    {
                                        subjectno = subjectno + "/" + newdt.Rows[icount]["subject_no"].ToString();
                                    }
                                    //Modified by srinath 16/10/2014
                                    if (!htsubject.Contains(newdt.Rows[icount]["subject_code"].ToString() + "-" + snam))
                                    {
                                        htsubject.Add(newdt.Rows[icount]["subject_code"].ToString() + "-" + snam, newdt.Rows[icount]["subject_no"].ToString());
                                    }
                                }
                                if (sublist.GetUpperBound(0) > 0)
                                {
                                    sublist[icount] = string.Empty;
                                }
                                string[] sublist1 = new string[newdt.Rows.Count + 2];
                                sublist1[0] = "Select For ALL";
                                //sublist1[0] = "Select For ALL";
                                // sublist1=sublist1.("Select For ALL");
                                //string jh = "Select For ALL";
                                for (icount = 0; icount < newdt.Rows.Count; icount++)
                                {
                                    //Modified by srinath 16/10/2014
                                    string subname = newdt.Rows[icount]["acronym"].ToString();
                                    if (rbsubname.Checked == true)
                                    {
                                        subname = newdt.Rows[icount]["subject_name"].ToString();
                                    }
                                    sublist1[icount + 1] = newdt.Rows[icount]["subject_code"].ToString() + "-" + subname;
                                    if (subjectno == "")
                                    {
                                        subjectno = newdt.Rows[icount]["subject_no"].ToString();
                                    }
                                    else
                                    {
                                        subjectno = subjectno + "/" + newdt.Rows[icount]["subject_no"].ToString();
                                    }
                                    //Modified by srinath 16/10/2014
                                    if (!htsubject.Contains(newdt.Rows[icount]["subject_code"].ToString() + "-" + subname))
                                    {
                                        htsubject.Add(newdt.Rows[icount]["subject_code"].ToString() + "-" + subname, newdt.Rows[icount]["subject_no"].ToString());
                                    }
                                }
                                if (sublist1.GetUpperBound(0) > 0)
                                {
                                    sublist1[icount + 1] = string.Empty;
                                }
                                // bind into spread 
                                FarPoint.Web.Spread.ComboBoxCellType sub_combo = new FarPoint.Web.Spread.ComboBoxCellType(sublist);
                                FarPoint.Web.Spread.ComboBoxCellType sub_comboheader = new FarPoint.Web.Spread.ComboBoxCellType();
                                FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 1].CellType = sub_combo;
                                //indudival student bind
                                for (int stud_count = 1; stud_count < FpEntry.Sheets[0].RowCount; stud_count++)
                                {
                                    sub_comboheader = new FarPoint.Web.Spread.ComboBoxCellType(sublist1);
                                    sub_comboheader.ShowButton = true;
                                    sub_comboheader.AutoPostBack = true;
                                    sub_comboheader.UseValue = true;
                                    FpEntry.Sheets[0].Cells[0, FpEntry.Sheets[0].ColumnCount - 1].CellType = sub_comboheader;
                                    // FpEntry.SaveChanges();
                                    FpEntry.Sheets[0].Cells[stud_count, FpEntry.Sheets[0].ColumnCount - 1].CellType = sub_combo;
                                    sub_combo.AutoPostBack = true;
                                    rollno = FpEntry.Sheets[0].Cells[stud_count, 0].Text;
                                    if (rolldic.ContainsKey(stud_count.ToString()))
                                    {
                                        rollno = rolldic[stud_count.ToString()];
                                    }
                                    // dataset row filtter
                                    DataView dv = new DataView();
                                    DataTable dat = new DataTable();
                                    dat = ds_totalsubjects.Tables[0];
                                    dat.DefaultView.RowFilter = "roll_no= '" + rollno + "' and paper_order='" + subtype + "' and subtype_no='" + subjecttype_no + "' ";
                                    DataView dv1 = new DataView();
                                    dv1 = dat.DefaultView;
                                    foreach (DataRowView datarowviewUsers in dv1)
                                    {
                                        string subjectcode = datarowviewUsers["subject_code"].ToString();
                                        //Modified by srinath 16/10/2014
                                        string subjectacronym = datarowviewUsers["acronym"].ToString();
                                        if (rbsubname.Checked == true)
                                        {
                                            subjectacronym = datarowviewUsers["subject_name"].ToString();
                                        }
                                        string selected_sublist = subjectcode + "-" + subjectacronym;
                                        subjectnumber = datarowviewUsers["subject_no"].ToString();
                                        FpEntry.Sheets[0].Cells[stud_count, FpEntry.Sheets[0].ColumnCount - 1].Text = selected_sublist.ToString();
                                        FpEntry.Sheets[0].Cells[stud_count, FpEntry.Sheets[0].ColumnCount - 1].Tag = subjectnumber;
                                    }
                                }
                            }
                            if (numberofpapers > 0)
                            {
                                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, startingcolumn, 1, numberofpapers);
                                FpEntry.Sheets[0].ColumnHeader.Cells[0, startingcolumn].Text = Convert.ToString(ds_subjects.Tables[0].Rows[subject]["subject_type"]);
                            }
                            startingcolumn = FpEntry.Sheets[0].ColumnCount;
                        }
                        FpEntry.Sheets[0].DefaultStyle.Font.Name = "Book Antique";
                        FpEntry.Sheets[0].DefaultStyle.Font.Bold = false;
                        FpEntry.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                        FpEntry.Sheets[0].PageSize = FpEntry.Sheets[0].RowCount;
                        //}
                    }
                    else
                    {
                        Labelerror.Visible = true;
                        Labelerror.Text = "Students Not Available In This Semester";
                        FpEntry.Visible = false;
                        Button1.Visible = false;
                        btnprint.Visible = false;
                        printcontrol.Visible = false;
                    }
                }
                else
                {
                    Labelerror.Visible = true;
                    Labelerror.Text = "Students Not Available In This Semester";
                    FpEntry.Visible = false;
                    Button1.Visible = false;
                    btnprint.Visible = false;
                    printcontrol.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Labelerror.Visible = true;
            Labelerror.Text = ex.ToString();
        }
    }

    protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
    {
        FpEntry.Visible = false;
        FpSpread2.Visible = false;
        //Fpstaff.Visible = false;
        gview.Visible = false;
        btnprint.Visible = false;
        btnstustaffprint.Visible = false;
        btnstustaffsave.Visible = false;
        Button1.Visible = false;
        subjtree.Visible = false;
        FindBtn.Visible = false;

        Save.Visible = false;
        if (TabContainer1.ActiveTabIndex == 1 || TabContainer1.ActiveTabIndex == 2)
        {
            TabContainer1.Width = 1100;
        }
        else
        {
            TabContainer1.Width = 1100;
        }
    }

    protected void Savebtn_Click(object sender, EventArgs e)
    {
        try
        {
            FpEntry.SaveChanges();
            ddlsemvalue = ((DropDownList)this.usercontrol.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlcollegevalue = ((DropDownList)this.usercontrol.FindControl("ddlcollege")).SelectedValue.ToString();
            ddlbatchvalue = ((DropDownList)this.usercontrol.FindControl("ddlBatch")).SelectedValue.ToString();
            ddldegreevalue = ((DropDownList)this.usercontrol.FindControl("ddlDegree")).SelectedValue.ToString();
            ddlsecvalue = ((DropDownList)this.usercontrol.FindControl("ddlSec")).SelectedValue.ToString();
            ddlsemvalue = ((DropDownList)this.usercontrol.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlbranchvalue = ((DropDownList)this.usercontrol.FindControl("ddlBranch")).SelectedValue.ToString();
            DataSet ds_selectedsubjects = new DataSet();
            string sect = string.Empty;
            if (ddlsecvalue != null && Convert.ToString(ddlsecvalue).Trim() != "" && Convert.ToString(ddlsecvalue).Trim() != "-1" && Convert.ToString(ddlsecvalue).Trim().ToLower() != "all")
            {
                sect = " and r.sections='" + ddlsecvalue.ToString() + "'";
            }
            string strquery = "select r.Roll_No,s.subject_no,s.semester from subjectChooser s,Registration r,LabAlloc l where r.Roll_No=s.roll_no   and r.Current_Semester=s.semester and r.Batch_Year=l.Batch_Year and r.degree_code=l.Degree_Code and r.Current_Semester=l.Semester and r.Sections=l.Sections and s.semester=l.Semester and s.subject_no=l.Subject_No and s.Batch<>''  and r.CC= 0 and r.Exam_Flag<>'debar' and r.DelFlag=0 and r.degree_code='" + ddlbranchvalue.ToString() + "' and r.Batch_Year='" + ddlbatchvalue.ToString() + "' and r.Current_Semester='" + ddlsemvalue.ToString() + "' " + sect + " order by r.Roll_No";
            ds_selectedsubjects = obi_access.select_method_wo_parameter(strquery, "Text");
            if (ds_selectedsubjects.Tables[0].Rows.Count > 0)
            {
                mpesave.Show();
            }
            else
            {
                mpesave.Hide();
                savesubjectchooser();
            }
            //DataSet ds_selectedsubjects = new DataSet();
            //string selected_subjects = "select subject_code,acronym,subject_no,subtype_no,subject_name from subject where syll_code =(select syll_code from syllabus_master where degree_code='" + ddlbranchvalue.ToString() + "' and semester='" + ddlsemvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "') ";
            //ds_selectedsubjects = obi_access.select_method_wo_parameter(selected_subjects, "text");
            //htsubject.Clear();
            //for (int i = 0; i < ds_selectedsubjects.Tables[0].Rows.Count; i++)
            //{
            //    string subnam = ds_selectedsubjects.Tables[0].Rows[i]["acronym"].ToString();
            //    if (rbsubname.Checked == true)
            //    {
            //        subnam = ds_selectedsubjects.Tables[0].Rows[i]["subject_name"].ToString();
            //    }
            //    string subnumber = ds_selectedsubjects.Tables[0].Rows[i]["subject_no"].ToString();
            //    string subkeyval = ds_selectedsubjects.Tables[0].Rows[i]["subject_code"].ToString() + "-" + subnam;
            //    if (!htsubject.Contains(subkeyval))
            //    {
            //        htsubject.Add(subkeyval, subnumber);
            //    }
            //}
            //for (int studcount = 1; studcount <= Convert.ToInt32(FpEntry.Sheets[0].RowCount) - 1; studcount++)
            //{
            //    rollno = FpEntry.Sheets[0].Cells[studcount, 0].Text;
            //    string deletequery = "delete from subjectchooser where roll_no='" + rollno.ToString() + "' and semester='" + ddlsemvalue.ToString() + "'";
            //    int del = obi_access.update_method_wo_parameter(deletequery, "Text");
            //    //con.Close();
            //    //con.Open();
            //    //string cmd = new SqlCommand(deletequery, con);
            //    // cmd.ExecuteNonQuery();
            //    // for (int stcolcount = 2; stcolcount <= Convert.ToInt32(FpEntry.Sheets[0].ColumnCount) - 1; stcolcount++)//Mpdified by srinath 22/8/2013
            //    for (int stcolcount = 3; stcolcount <= Convert.ToInt32(FpEntry.Sheets[0].ColumnCount) - 1; stcolcount++)
            //    {
            //        if (FpEntry.Sheets[0].Cells[studcount, stcolcount].Text != "")
            //        {
            //            //using hashtable to get the subjectnumber
            //            subjectnumber = GetCorrespondingKey(Convert.ToString(FpEntry.Sheets[0].Cells[studcount, stcolcount].Text.ToString()), htsubject).ToString();
            //            int paper = Convert.ToInt32(FpEntry.Sheets[0].ColumnHeader.Cells[1, stcolcount].Text.ToString());
            //            subjecttype_no = Convert.ToInt32(FpEntry.Sheets[0].ColumnHeader.Cells[1, stcolcount].Tag);
            //            for (int i = stcolcount; i <= Convert.ToInt32(FpEntry.Sheets[0].ColumnCount - 1); i++)
            //            {
            //                if (i != stcolcount)
            //                {
            //                    if (subjecttype_no.ToString() == Convert.ToString(FpEntry.Sheets[0].ColumnHeader.Cells[1, i].Tag))
            //                    {
            //                        if (FpEntry.Sheets[0].Cells[studcount, stcolcount].Text == FpEntry.Sheets[0].Cells[studcount, i].Text)
            //                        {
            //                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student cannot select the same subjects more than once')", true);
            //                            return;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        goto l1;
            //                    }
            //                }
            //            }
            //        l1: string insertcmd = "insert into Subjectchooser(semester,roll_no,paper_order,subject_no,subtype_no,batch,grp_cell) values('" + ddlsemvalue.ToString() + "','" + rollno.ToString() + "','" + paper.ToString() + "','" + subjectnumber.ToString() + "','" + subjecttype_no.ToString() + "','',0)";
            //            int cmd1 = obi_access.update_method_wo_parameter(insertcmd, "Text");
            //            //cmd1.ExecuteNonQuery();
            //            flagstudent = true;
            //        }
            //    }
            //}
            //if (flagstudent == true)
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('saved successfully')", true);
            //    FpEntry.SaveChanges();
            //}
        }
        catch (Exception ex)
        {
            Labelerror.Text = ex.ToString();
            Labelerror.Visible = true;
        }
    }

    public void savesubjectchooser()
    {
        try
        {
            FpEntry.SaveChanges();
            ddlsemvalue = ((DropDownList)this.usercontrol.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlcollegevalue = ((DropDownList)this.usercontrol.FindControl("ddlcollege")).SelectedValue.ToString();
            ddlbatchvalue = ((DropDownList)this.usercontrol.FindControl("ddlBatch")).SelectedValue.ToString();
            ddldegreevalue = ((DropDownList)this.usercontrol.FindControl("ddlDegree")).SelectedValue.ToString();
            ddlsecvalue = ((DropDownList)this.usercontrol.FindControl("ddlSec")).SelectedValue.ToString();
            ddlsemvalue = ((DropDownList)this.usercontrol.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlbranchvalue = ((DropDownList)this.usercontrol.FindControl("ddlBranch")).SelectedValue.ToString();
            DataSet ds_selectedsubjects = new DataSet();
            string selected_subjects = "select s.subject_code,s.acronym,s.subject_no,s.subtype_no,s.subject_name from subject s,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and s.syll_code=ss.syll_code and s.syll_code=sy.syll_code and sy.degree_code='" + ddlbranchvalue.ToString() + "' and sy.semester='" + ddlsemvalue.ToString() + "' and sy.batch_year='" + ddlbatchvalue.ToString() + "'";
            ds_selectedsubjects = obi_access.select_method_wo_parameter(selected_subjects, "text");
            htsubject.Clear();
            for (int i = 0; i < ds_selectedsubjects.Tables[0].Rows.Count; i++)
            {
                string subnam = ds_selectedsubjects.Tables[0].Rows[i]["acronym"].ToString();
                if (rbsubname.Checked == true)
                {
                    subnam = ds_selectedsubjects.Tables[0].Rows[i]["subject_name"].ToString();
                }
                string subnumber = ds_selectedsubjects.Tables[0].Rows[i]["subject_no"].ToString();
                string subkeyval = ds_selectedsubjects.Tables[0].Rows[i]["subject_code"].ToString() + "-" + subnam;
                if (!htsubject.Contains(subkeyval))
                {
                    htsubject.Add(subkeyval, subnumber);
                }
            }
            Hashtable ht = new Hashtable();
            ht.Clear();
            for (int studcount = 1; studcount <= Convert.ToInt32(FpEntry.Sheets[0].RowCount) - 1; studcount++)
            {
                rollno = FpEntry.Sheets[0].Cells[studcount, 0].Text;
                for (int i = 3; i <= Convert.ToInt32(FpEntry.Sheets[0].ColumnCount - 1); i++)
                {
                    if (FpEntry.Sheets[0].Cells[studcount, i].Text.ToString().Trim() != "")
                    {
                        string getsubjectno = FpEntry.Sheets[0].Cells[studcount, i].Text.ToString().Trim();
                        if (!ht.Contains(rollno + '-' + getsubjectno))
                        {
                            ht.Add(rollno + '-' + getsubjectno, rollno + '-' + getsubjectno);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student cannot select the same subjects more than once')", true);
                            return;
                        }
                    }
                }
            }
            for (int studcount = 1; studcount <= Convert.ToInt32(FpEntry.Sheets[0].RowCount) - 1; studcount++)
            {
                rollno = FpEntry.Sheets[0].Cells[studcount, 0].Text;
                //ht.Clear();
                //ht.Add("roll_no", rollno);
                //ht.Add("semester", ddlsemvalue.ToString());
                //int del = obi_access.insert_method("sp_delete_Subjectchooser", ht, "sp");
                string subnovalu = string.Empty;
                for (int stcolcount = 3; stcolcount <= Convert.ToInt32(FpEntry.Sheets[0].ColumnCount) - 1; stcolcount++)
                {
                    if (FpEntry.Sheets[0].Cells[studcount, stcolcount].Text != "")
                    {
                        subjectnumber = GetCorrespondingKey(Convert.ToString(FpEntry.Sheets[0].Cells[studcount, stcolcount].Text.ToString()), htsubject).ToString();
                        int paper = Convert.ToInt32(FpEntry.Sheets[0].ColumnHeader.Cells[1, stcolcount].Text.ToString());
                        subjecttype_no = Convert.ToInt32(FpEntry.Sheets[0].ColumnHeader.Cells[1, stcolcount].Tag);
                        if (subnovalu.Trim() == "")
                        {
                            subnovalu = "'" + subjectnumber + "'";
                        }
                        else
                        {
                            subnovalu = subnovalu + ",'" + subjectnumber + "'";
                        }
                        ht.Clear();
                        ht.Add("roll_no", rollno);
                        ht.Add("semester", ddlsemvalue.ToString());
                        ht.Add("subject_no", subjectnumber.ToString());
                        ht.Add("subtype_no", subjecttype_no.ToString());
                        ht.Add("paper_order", paper.ToString());
                        ht.Add("batch", "");
                        ht.Add("grp_cell", 0);
                        int cmd1 = obi_access.insert_method("sp_ins_upd_Subjectchooser", ht, "sp");
                        flagstudent = true;
                    }
                }
                string subnoinval = string.Empty;
                if (subnovalu.Trim() != "")
                {
                    subnoinval = "  and subject_no not in(" + subnovalu + ")";
                }
                string subjectchooserdelquery = "delete from subjectChooser where roll_no='" + rollno + "' and semester='" + ddlsemvalue + "' " + subnoinval + "";
                int cmdinavskl = obi_access.update_method_wo_parameter(subjectchooserdelquery, "Text");
            }
            if (flagstudent == true)
            {
                loadstudentsubjectchooser();
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('saved successfully')", true);
            }
        }
        catch (Exception ex)
        {
            Labelerror.Text = ex.ToString();
            Labelerror.Visible = true;
        }
    }

    protected void FpEntry_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = e.SheetView.ActiveRow.ToString();
        string actcol = e.SheetView.ActiveColumn.ToString();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt32(FpEntry.Sheets[0].RowCount); j++)
            {
                actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt32(actcol)].ToString();
                FpEntry.Sheets[0].Cells[j, Convert.ToInt32(actcol)].Text = seltext.ToString();
            }
            flag_true = true;
        }
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }
        return null;
    }

    // staff selector code
    protected void btnGo1_Click(object sender, EventArgs e)
    {
        //Chkalterotherdept.Visible = false;
        gview.Visible = false;
        Save.Visible = false;
        ddlcollegevalue = ((DropDownList)this.usercontrol1.FindControl("ddlcollege")).SelectedValue.ToString();
        ddlbatchvalue = ((DropDownList)this.usercontrol1.FindControl("ddlBatch")).SelectedValue.ToString();
        ddldegreevalue = ((DropDownList)this.usercontrol1.FindControl("ddlDegree")).SelectedValue.ToString();
        ddlsecvalue = ((DropDownList)this.usercontrol1.FindControl("ddlSec")).SelectedValue.ToString();
        ddlsemvalue = ((DropDownList)this.usercontrol1.FindControl("ddlSemYr")).SelectedValue.ToString();
        ddlbranchvalue = ((DropDownList)this.usercontrol1.FindControl("ddlBranch")).SelectedValue.ToString();
        Session["SEC"] = Convert.ToString(ddlsecvalue);
        Session["Sem"] = Convert.ToString(ddlsemvalue);
        Session["ddlcollegevalue"] = Convert.ToString(ddlcollegevalue);
        Session["ddlbatchvalue"] = Convert.ToString(ddlbatchvalue);
        Session["ddldegreevalue"] = Convert.ToString(ddldegreevalue);
        string strsec = string.Empty;
        if (ddlsecvalue.ToString().Trim().ToLower() == "all")
        {
            lblerror.Visible = true;
            lblerror.Text = "Please Select any one section";
            FindBtn.Visible = false;
            Chkalterotherdept.Visible = false;
            Save.Visible = false;
            subjtree.Visible = false;
            //Fpstaff.Visible = false;
            gview.Visible = false;
        }
        else
        {
            if (ddlsecvalue.ToString() != "0" && ddlsecvalue.ToString() != "\0")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + ddlsecvalue.ToString() + "'";
            }
            DataSet ds_cssem = new DataSet();
            string cseme = "select distinct current_semester from registration where degree_code ='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' and cc=0 and delflag=0 and exam_flag!='debar' ";
            ds_cssem = obi_access.select_method_wo_parameter(cseme, "text");
            if (ds_cssem.Tables[0].Rows.Count > 0)
            {
                string currentsem = ds_cssem.Tables[0].Rows[0]["current_semester"].ToString();
                if (currentsem != ddlsemvalue.ToString())
                {
                    FindBtn.Visible = false;
                    Chkalterotherdept.Visible = false;
                    Save.Visible = false;
                    subjtree.Visible = false;
                    //Fpstaff.Visible = false;
                    gview.Visible = false;
                    lblerror.Visible = true;
                    lblerror.Text = "Students Not Available In This Semester";
                }
                else
                {
                    FindBtn.Visible = false;
                    subjtree.Visible = true;
                    FindBtn.Enabled = true;
                    Save.Enabled = true;
                    Chkalterotherdept.Visible = false;
                    treeload();
                }
            }
            else
            {
                FindBtn.Visible = false;
                Save.Visible = false;
                Chkalterotherdept.Visible = false;
                subjtree.Visible = false;
                //Fpstaff.Visible = false;
                gview.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Students Not Available In This Semester";
            }
        }
    }

    private string GetSyllabusYear(string degree_code, string batch_year, string sem)
    {
        string syl_year = string.Empty;
        // con.Close();
        // con.Open();
        // SqlCommand cmd2a;
        //SqlDataReader get_syl_year;
        string cmd2a = "select syllabus_year from syllabus_master where degree_code=" + ddlbranchvalue.ToString() + " and semester =" + ddlsemvalue.ToString() + " and batch_year=" + ddlbatchvalue.ToString() + " ";
        //get_syl_year = cmd2a.ExecuteReader();
        // get_syl_year.Read();
        DataSet get_syl_year = new DataSet();
        get_syl_year = obi_access.select_method_wo_parameter(cmd2a, "Text");
        if (get_syl_year.Tables[0].Rows.Count > 0)
        {
            if (get_syl_year.Tables[0].Rows[0][0].ToString() == "\0")
            {
                syl_year = "-1";
            }
            else
            {
                syl_year = get_syl_year.Tables[0].Rows[0][0].ToString();
            }
        }
        else
        {
            syl_year = "-1";
        }
        return syl_year;
    }

    protected void subjtree_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            try
            {
                ddlbatchvalue = ((DropDownList)this.usercontrol1.FindControl("ddlBatch")).SelectedValue.ToString();
                ddlsecvalue = ((DropDownList)this.usercontrol1.FindControl("ddlSec")).SelectedValue.ToString();
                //Fpstaff.Sheets[0].AutoPostBack = false;
                subjtree.Visible = true;
                //Fpstaff.Visible = false;
                gviewstaff.Visible = false;
                FindBtn.Visible = false;
                Chkalterotherdept.Visible = false;
                Save.Visible = false;
                string strsec;
                if (ddlsecvalue.ToString() != "0" && ddlsecvalue.ToString() != "\0")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + ddlsecvalue.ToString() + "'";
                }
                int parent_count = subjtree.Nodes.Count;//----------count parent node value
                for (int i = 0; i < parent_count; i++)
                {
                    for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                    {
                        if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                        {
                            subjtree.Visible = true;
                            //Fpstaff.Visible = true;
                            gviewstaff.Visible = false;
                            FindBtn.Visible = true;
                            Chkalterotherdept.Visible = true;
                            Save.Visible = true;
                            if (ddlsecvalue.ToString() == "")
                            {
                                temp_sec = string.Empty;
                            }
                            else
                            {
                                temp_sec = " and Sections='" + ddlsecvalue.ToString() + "'";
                            }
                            chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;

                            dtable.Columns.Add("Staff_Code");
                            dtable.Columns.Add("Staff_Name");

                            DataSet ds_stfbind = new DataSet();
                            string stffbd = "select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = '" + chile_index + "' and batch_year=" + ddlbatchvalue.ToString() + "  " + temp_sec + ")";
                            ds_stfbind = obi_access.select_method_wo_parameter(stffbd, "text");
                            for (int stcount = 0; stcount <= ds_stfbind.Tables[0].Rows.Count - 1; stcount++)
                            {
                                string staffname = ds_stfbind.Tables[0].Rows[stcount]["staff_name"].ToString();
                                string staffcode = ds_stfbind.Tables[0].Rows[stcount]["staff_code"].ToString();

                                dtrow = dtable.NewRow();
                                dtrow["Staff_Code"] = staffcode;
                                dtrow["Staff_Name"] = staffname;
                                dtable.Rows.Add(dtrow);
                            }

                        }
                    }
                }
                ViewState["dtadvisor"] = dtable;
                gview.DataSource = dtable;
                gview.DataBind();
                gview.Visible = true;
                Save.Visible = true;
                if (gview.Rows.Count == 0)
                {
                    Save.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    //protected void ddlr1_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    int getval = 0;

    //    Button grids = (Button)sender;
    //    string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
    //    int rowIndx = Convert.ToInt32(rowIndxSs) - 2;

    //    //   Label dayy = (GridView2.SelectedRow.FindControl("lblDayVal") as Label);
    //    // Rows[rowIndex].FindControl("lblDayVal");
    //    Label dayy = (Label)gview.Rows[rowIndx].FindControl("lblDayVal");
    //    string day = dayy.Text;
    //    int.TryParse(day, out getval);
    //    GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
    //    DropDownList duty = (DropDownList)gvr.FindControl("ddlr1");
    //    dropvalue = "ddlr1";
    //    string drop = duty.SelectedItem.Value;
    //    string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
    //    string dayval = Days[getval - 1];
    //    string selquer = "select " + dayval + "1 from Semester_Schedule_room";
    //    DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
    //    if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
    //    {
    //        for (int a = 0; a < ScheduleDataSet.Tables[0].Rows.Count; a++)
    //        {
    //            string getroom = Convert.ToString(ScheduleDataSet.Tables[0].Rows[a][dayval + "1"]);
    //            string[] spl = getroom.Split(';');
    //            if (spl.Length >= 1)
    //            {
    //                for (int cn = 0; cn < spl.Length; cn++)
    //                {
    //                    string[] splroom = spl[cn].Split('-');
    //                    if (splroom.Length == 3)
    //                    {
    //                        if (drop == splroom[2])
    //                        {
    //                            imgdiv2.Visible = true;
    //                            lbl_alerterror.Text = "Room Already Selected,Do You Want to Change Room";
    //                            //((DropDownList)GridView2.FindControl(dropvalue) as DropDownList).SelectedIndex = ((DropDownList)GridView2.FindControl(dropvalue) as DropDownList).Items.IndexOf(((DropDownList)GridView2.FindControl(dropvalue) as DropDownList).Items.FindByText(""));
    //                            // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Room Already Selected')", true );
    //                            return;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    #region Command
    //protected void btn_remove_click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ddlbatchvalue = ((DropDownList)this.usercontrol1.FindControl("ddlBatch")).SelectedValue.ToString();
    //        ddlsecvalue = ((DropDownList)this.usercontrol1.FindControl("ddlSec")).SelectedValue.ToString();

    //        subjtree.Visible = true;
    //        Fpstaff.Visible = true;
    //        string subjectNo = string.Empty;
    //        string staffCode = string.Empty;
    //        Fpstaff.SaveChanges();
    //        //string ar = e.SheetView.ActiveRow.ToString();
    //        //string ac = e.SheetView.ActiveColumn.ToString();

    //        int actrow = Convert.ToInt32(3);
    //        int actcol = Convert.ToInt32(5);
    //        if (staf_butt1 != null)
    //        {
    //            //if (Fpstaff.Sheets[0].Cells[actrow, 1].Text.Trim() != "" || Fpstaff.Sheets[0].Cells[actrow, 1].Text != null || Fpstaff.Sheets[0].Cells[actrow, 1].Text.Trim() != " ")
    //            if (gview.Rows[actrow].Cells[1].Text.Trim() != "" || gview.Rows[actrow].Cells[1].Text != null || gview.Rows[actrow].Cells[1].Text.Trim() != " ")
    //            {
    //                staffCode = Convert.ToString(gview.Rows[actrow].Cells[1].Text);
    //            }
    //        }
    //        int parent_count = subjtree.Nodes.Count;
    //        for (int i = 0; i < parent_count; i++)
    //        {
    //            for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
    //            {
    //                if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
    //                {
    //                    subjtree.Visible = true;
    //                    //Fpstaff.Visible = true;
    //                    gview.Visible = true;
    //                    FindBtn.Visible = true;
    //                    Chkalterotherdept.Visible = true;
    //                    Save.Visible = true;
    //                    if (ddlsecvalue.ToString() == "")
    //                    {
    //                        temp_sec = string.Empty;
    //                    }
    //                    else
    //                    {
    //                        temp_sec = " and Sections='" + ddlsecvalue.ToString() + "'";
    //                    }
    //                    subjectNo = subjtree.Nodes[i].ChildNodes[node_count].Value;
    //                }
    //            }
    //        }

    //        if (!string.IsNullOrEmpty(subjectNo) && !string.IsNullOrEmpty(staffCode))
    //        {
    //            DataTable dtSubjectChooser = dirAcc.selectDataTable("select * from subjectChooser where subject_no='" + subjectNo + "' and staffcode like '%" + staffCode + "%'");
    //            if (dtSubjectChooser.Rows.Count == 0)
    //            {
    //                string chkStaffSel = "select * from staff_selector where subject_no='" + subjectNo + "' and staff_code='" + staffCode + "' and sections='" + ddlsecvalue.ToString() + "'";
    //                DataTable dtStaffsel = dirAcc.selectDataTable(chkStaffSel);
    //                if (dtStaffsel.Rows.Count > 0)
    //                {
    //                    string deletequery = "delete from staff_selector where subject_no='" + subjectNo + "' and staff_code='" + staffCode + "' and batch_year='" + ddlbatchvalue.ToString() + "' and sections='" + ddlsecvalue.ToString() + "'";
    //                    int d = obi_access.update_method_wo_parameter(deletequery, "Text");
    //                    if (d != 0)
    //                    {
    //                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Removed successfully')", true);
    //                        //Fpstaff.Sheets[0].Rows[actrow].Remove();
    //                    }
    //                }
    //                else
    //                {
    //                    for (int i = 0; i < Fpstaff.Sheets[0].RowCount; i++)
    //                    {
    //                        //if (Fpstaff.Sheets[0].Cells[i, 1].Text.Trim() == "" || Fpstaff.Sheets[0].Cells[i, 1].Text == null || Fpstaff.Sheets[0].Cells[i, 1].Text.Trim() == " ")
    //                        if (gview.Rows[i].Cells[1].Text.Trim() != "" || gview.Rows[i].Cells[1].Text != null || gview.Rows[i].Cells[1].Text.Trim() != " ")
    //                        {
    //                            //Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount - 1;
    //                            //gview.Rows.Count = gview.Rows.Count - 1;
    //                        }
    //                    }
    //                    if (staf_butt1 != null)
    //                    {
    //                        if (actcol == 3)
    //                        {
    //                            // Fpstaff.Sheets[0].RemoveRows(actrow, 1);
    //                            //Fpstaff.Sheets[0].Rows[actrow].Remove();

    //                            // Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount - 1;
    //                        }
    //                    }
    //                    //Fpstaff.Sheets[0].PageSize = Fpstaff.Sheets[0].Rows.Count;
    //                    gview.PageSize = gview.Rows.Count;
    //                    //Fpstaff.SaveChanges();
    //                    rc1 = Fpstaff.Sheets[0].Rows.Count;
    //                    //added By Mullai
    //                    if (rc1 == 0)
    //                    {
    //                        Fpstaff.Sheets[0].ClearRowFilter();
    //                        Fpstaff.Visible = false;
    //                        Save.Visible = false;

    //                    }
    //                }
    //            }
    //            else
    //            {
    //                lblSubNo.Text = subjectNo;
    //                lblStaffCode.Text = staffCode;
    //                lblAlertMsg.Visible = true;
    //                lblAlertMsg.Text = ("Do You Want Delete Subject Chooser");
    //                divPopAlert.Visible = true;
    //                // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Remove Student staff selecter.!')", true);
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    #endregion

    //------------------remove the selected row from the spread

    //protected void Fpstaff_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    try
    //    {
    //        ddlbatchvalue = ((DropDownList)this.usercontrol1.FindControl("ddlBatch")).SelectedValue.ToString();
    //        ddlsecvalue = ((DropDownList)this.usercontrol1.FindControl("ddlSec")).SelectedValue.ToString();

    //        subjtree.Visible = true;
    //        //Fpstaff.Visible = true;
    //        gview.Visible = true;
    //        string subjectNo = string.Empty;
    //        string staffCode = string.Empty;
    //        //Fpstaff.SaveChanges();
    //        string ar = e.SheetView.ActiveRow.ToString();
    //        string ac = e.SheetView.ActiveColumn.ToString();
    //        int actrow = Convert.ToInt32(ar);
    //        int actcol = Convert.ToInt32(ac);
    //        if (staf_butt1 != null)
    //        {
    //            //if (Fpstaff.Sheets[0].Cells[actrow, 1].Text.Trim() != "" || Fpstaff.Sheets[0].Cells[actrow, 1].Text != null || Fpstaff.Sheets[0].Cells[actrow, 1].Text.Trim() != " ")
    //            //{
    //            //    staffCode = Convert.ToString(Fpstaff.Sheets[0].Cells[actrow, 1].Text);
    //            //}
    //        }
    //        int parent_count = subjtree.Nodes.Count;
    //        for (int i = 0; i < parent_count; i++)
    //        {
    //            for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
    //            {
    //                if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
    //                {
    //                    subjtree.Visible = true;
    //                    //Fpstaff.Visible = true;
    //                    gview.Visible = true;
    //                    FindBtn.Visible = true;
    //                    Chkalterotherdept.Visible = true;
    //                    Save.Visible = true;
    //                    if (ddlsecvalue.ToString() == "")
    //                    {
    //                        temp_sec = string.Empty;
    //                    }
    //                    else
    //                    {
    //                        temp_sec = " and Sections='" + ddlsecvalue.ToString() + "'";
    //                    }
    //                    subjectNo = subjtree.Nodes[i].ChildNodes[node_count].Value;
    //                }
    //            }
    //        }

    //        if (!string.IsNullOrEmpty(subjectNo) && !string.IsNullOrEmpty(staffCode))
    //        {
    //            DataTable dtSubjectChooser = dirAcc.selectDataTable("select * from subjectChooser where subject_no='" + subjectNo + "' and staffcode like '%" + staffCode + "%'");
    //            if (dtSubjectChooser.Rows.Count == 0)
    //            {
    //                string chkStaffSel = "select * from staff_selector where subject_no='" + subjectNo + "' and staff_code='" + staffCode + "' and sections='" + ddlsecvalue.ToString() + "'";
    //                DataTable dtStaffsel = dirAcc.selectDataTable(chkStaffSel);
    //                if (dtStaffsel.Rows.Count > 0)
    //                {
    //                    string deletequery = "delete from staff_selector where subject_no='" + subjectNo + "' and staff_code='" + staffCode + "' and batch_year='" + ddlbatchvalue.ToString() + "' and sections='" + ddlsecvalue.ToString() + "'";
    //                    int d = obi_access.update_method_wo_parameter(deletequery, "Text");
    //                    if (d != 0)
    //                    {
    //                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Removed successfully')", true);
    //                        //Fpstaff.Sheets[0].Rows[actrow].Remove();
    //                    }
    //                }
    //                else
    //                {
    //                    for (int i = 0; i < Fpstaff.Sheets[0].RowCount; i++)
    //                    {
    //                        if (Fpstaff.Sheets[0].Cells[i, 1].Text.Trim() == "" || Fpstaff.Sheets[0].Cells[i, 1].Text == null || Fpstaff.Sheets[0].Cells[i, 1].Text.Trim() == " ")
    //                        {
    //                            Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount - 1;
    //                        }
    //                    }
    //                    if (staf_butt1 != null)
    //                    {
    //                        if (actcol == 3)
    //                        {
    //                            // Fpstaff.Sheets[0].RemoveRows(actrow, 1);
    //                            Fpstaff.Sheets[0].Rows[actrow].Remove();

    //                            // Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount - 1;
    //                        }
    //                    }
    //                    Fpstaff.Sheets[0].PageSize = Fpstaff.Sheets[0].Rows.Count;
    //                    Fpstaff.SaveChanges();
    //                    rc1 = Fpstaff.Sheets[0].Rows.Count;
    //                    //added By Mullai
    //                    if (rc1 == 0)
    //                    {
    //                        Fpstaff.Sheets[0].ClearRowFilter();
    //                        Fpstaff.Visible = false;
    //                        Save.Visible = false;

    //                    }
    //                }
    //            }
    //            else
    //            {
    //                lblSubNo.Text = subjectNo;
    //                lblStaffCode.Text = staffCode;
    //                lblAlertMsg.Visible = true;
    //                lblAlertMsg.Text = ("Do You Want Delete Subject Chooser");
    //                divPopAlert.Visible = true;
    //               // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Remove Student staff selecter.!')", true);
    //            }
    //            Fpstaff.SaveChanges();

    //        }
    //    }
    //    catch
    //    {
    //    }
    //}



    //public void removing()
    //{
    //    FarPoint.Web.Spread.Model.BaseSheetDataModel dataModel;
    //    bool b;
    //    dataModel = (FarPoint.Web.Spread.Model.BaseSheetDataModel)Fpstaff
    //    b = dataModel.IsRowUsed(0);
    //    if (!b == true)
    //    {
    //        fpspread1.ActiveSheet.RemoveRows[0, 1];
    //    }
    //}

    private void treeload()
    {
        try
        {
            subjtree.Nodes.Clear();
            string subjname = string.Empty;
            string Syllabus_year = string.Empty;
            Syllabus_year = GetSyllabusYear(ddlbranchvalue.ToString(), ddlbatchvalue.ToString(), ddlsemvalue.ToString());
            if (Syllabus_year != "-1")
            {
                subjtree.Visible = true;
                FindBtn.Visible = false;
                Chkalterotherdept.Visible = false;
                DataSet ds_subjecttype = new DataSet();
                string subjectnames = "select distinct subject.subtype_no,subject_type from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + ddlbranchvalue.ToString() + " and semester=" + ddlsemvalue.ToString() + " and syllabus_year = " + Syllabus_year + " and batch_year = " + ddlbatchvalue.ToString() + ") order by subject.subtype_no";
                ds_subjecttype = obi_access.select_method_wo_parameter(subjectnames, "text");
                TreeNode node;
                int rec_count = 0;
                //node = new TreeNode(ddlbranchvalue.ToString(), rec_count.ToString());
                //rec_count++;
                if (ds_subjecttype.Tables.Count > 0 && ds_subjecttype.Tables[0].Rows.Count >= 0)
                {
                    for (int subject = 0; subject < ds_subjecttype.Tables[0].Rows.Count; subject++)
                    {
                        subjecttypeno = ds_subjecttype.Tables[0].Rows[subject]["subtype_no"].ToString();
                        DataSet ds_subjectnames = new DataSet();
                        string subjectnames1 = "select subject.subtype_no,subject_type,subject_no,subject_name,subject_code from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + ddlbranchvalue.ToString() + " and semester=" + ddlsemvalue.ToString() + " and syllabus_year = " + Syllabus_year + " and batch_year = " + ddlbatchvalue.ToString() + ") and subject.subtype_no=" + subjecttypeno.ToString() + " order by subject.subtype_no,subject.subject_no";
                        ds_subjectnames = obi_access.select_method_wo_parameter(subjectnames1, "text");
                        string sub_names = ds_subjecttype.Tables[0].Rows[subject]["subject_type"].ToString();
                        node = new TreeNode(sub_names, rec_count.ToString());
                        //-------------set to tree
                        for (int j = 0; j < ds_subjectnames.Tables[0].Rows.Count; j++)
                        {
                            string subj_name = ds_subjectnames.Tables[0].Rows[j]["subject_code"].ToString() + "-" + ds_subjectnames.Tables[0].Rows[j]["subject_name"].ToString();
                            string sub_no = ds_subjectnames.Tables[0].Rows[j]["subject_no"].ToString();
                            if (subj_name.ToString() != "0" && subj_name.ToString() != subjname)
                            {
                                node.ChildNodes.Add(new TreeNode(subj_name.ToString(), sub_no.ToString()));
                                rec_count = rec_count + 1;
                            }
                        }
                        subjtree.Nodes.Add(node);
                    }
                    FindBtn.Enabled = true;
                    Save.Enabled = true;
                    Chkalterotherdept.Visible = false;
                }
            }
            else
            {
                FindBtn.Enabled = false;
                Chkalterotherdept.Visible = false;
                Save.Enabled = false;
                lblerror.Visible = true;
                lblerror.Text = "There is No record Found";
            }
        }
        catch
        {
        }
    }

    protected void FindBtn_Click(object sender, EventArgs e)
    {
        panel3.Visible = true;
        // panelrollnopop.Visible = false;
        //fsstaff.Visible = true;
        gviewstaff.Visible = true;
        //fsstaff.Sheets[0].RowCount = 0;
        BindCollege();
        loadstaffdep(collegecode);
        bindstaffcata(collegecode);
        loadfsstaff();
        // loadallstaff();//Hidden By Srinath 9/5/2013
    }

    public void BindCollege()
    {
        // con.Open();
        string cmd = "select collname,college_code from collinfo";
        // SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        ds = obi_access.select_method_wo_parameter(cmd, "Text");
        //da.Fill(ds);
        ddlcollege.DataSource = ds;
        ddlcollege.DataTextField = "collname";
        ddlcollege.DataValueField = "college_code";
        ddlcollege.DataBind();
        //ddlcollege.SelectedIndex = ddlcollege.Items.Count - 1;
        //con.Close();
        bindstaffcata(Convert.ToString(ddlcollege.SelectedValue));
    }

    // ----Load staff department----
    public void loadstaffdep(string collegecode)
    {
        //con.Open();
        string cmd = "select distinct dept_name,dept_code from hrdept_master where college_code=" + Session["collegecode"] + "";
        // SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        ds = obi_access.select_method_wo_parameter(cmd, "Text");
        // da.Fill(ds);
        ddldepratstaff.DataSource = ds;
        ddldepratstaff.DataTextField = "dept_name";
        ddldepratstaff.DataValueField = "dept_code";
        ddldepratstaff.DataBind();
        ddldepratstaff.Items.Insert(0, "All");
        //con.Close();
        //  bindstaffcata(Convert.ToString(ddlcollege.SelectedValue));
    }

    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fsstaff.Sheets[0].RowCount = 0;
        loadfsstaff();
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindstaffcata(Convert.ToString(ddlcollege.SelectedValue));
        //fsstaff.Sheets[0].RowCount = 0;
        loadfsstaff();
    }

    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        //fsstaff.Sheets[0].RowCount = 0;
        loadfsstaff();
    }

    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fsstaff.Sheets[0].RowCount = 0;
        loadfsstaff();
    }

    //Hidden By Srinath 9/5/2013
    //protected void loadallstaff()
    //{
    //    sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0";
    //    fsstaff.Sheets[0].RowCount = 0;
    //    fsstaff.SaveChanges();
    //    FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
    //    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
    //    //fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
    //    //fsstaff.Sheets[0].SpanModel.Add(fsstaff.Sheets[0].RowCount - 1, 0, 1, 3);
    //    fsstaff.Sheets[0].AutoPostBack = false;
    //    string bindspread = sql;
    //    SqlDataAdapter dabindspread = new SqlDataAdapter(bindspread, con);
    //    DataSet dsbindspread = new DataSet();
    //    dabindspread.Fill(dsbindspread);
    //    if (dsbindspread.Tables[0].Rows.Count > 0)
    //    {
    //        int sno = 0;
    //        for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
    //        {
    //            sno++;
    //            string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
    //            string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();
    //            //fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
    //            fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Name";
    //            fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Code";
    //            fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "Select";
    //            fsstaff.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    //            //fsstaff.Sheets[0].Columns[0].Width = 50;
    //            //fsstaff.Sheets[0].Columns[3].Width = 320;
    //            fsstaff.Sheets[0].Columns[2].Width = 200;
    //            //fsstaff.Sheets[0].Columns[0].Width = 62;
    //            fsstaff.Sheets[0].ColumnCount = 3;
    //            fsstaff.Width = 401;
    //            fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
    //            fsstaff.Sheets[0].Rows[fsstaff.Sheets[0].RowCount - 1].Font.Bold = false;
    //            //fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
    //            //fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //            fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Text = name;
    //            fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
    //            fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Text = code;
    //            fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
    //            fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].CellType = chkcell1;
    //            fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //            fsstaff.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
    //            chkcell1.AutoPostBack = true;
    //        }
    //        int rowcount = fsstaff.Sheets[0].RowCount;
    //        fsstaff.Height = 300;
    //        fsstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
    //        fsstaff.SaveChanges();
    //        con.Close();
    //    }
    //}

    protected void loadfsstaff()
    {
        string Categorys = rs.GetSelectedItemsValueAsString(cbl_Category);
        if (ddldepratstaff.SelectedIndex != 0)
        {
            if (txt_search.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0)and  (staffmaster.settled = 0)  and (staff_name like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "' and  (stafftrans.category_code in('" + Categorys + "')) and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
                }
                else if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "' and  (stafftrans.category_code in('" + Categorys + "')) and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
                }
            }
            else
            {
                //sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_name = '" + ddldepratstaff.Text + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "' and (staffmaster.college_code =hrdept_master.college_code)";
                sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code  WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and  (stafftrans.category_code in('" + Categorys + "')) and StaffCategorizer.college_code=staffmaster.college_code";
            }
        }
        else if (txt_search.Text != "")
        {
            if (ddlstaff.SelectedIndex == 0)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '%" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "' and  (stafftrans.category_code in('" + Categorys + "')) and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
            }
            else if (ddlstaff.SelectedIndex == 1)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '%" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "' and  (stafftrans.category_code in('" + Categorys + "')) and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
            }
            else if (ddlcollege.SelectedIndex != -1)
            {
                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";//Modifed By Srinath 9/5/2013
            }
            else
            {
                sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster,hrdept_master.dept_name where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";//Modifed By Srinath 9/5/2013
            }
        }
        else
            if (ddldepratstaff.SelectedValue.ToString() == "All")
            {
                //magesh 17.8.18
                //sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";//Modifed By Srinath 9/5/2013
                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster,StaffCategorizer where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "' and StaffCategorizer.category_code= stafftrans.category_code and  stafftrans.category_code in ('" + Categorys + "') and StaffCategorizer.college_code=staffmaster.college_code";
            }

        DataSet dsbindspread = new DataSet();
        dsbindspread = obi_access.select_method_wo_parameter(sql, "Text");
        //con.Close();
        // con.Open();
        //FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        //fsstaff.Sheets[0].RowCount = 0;//
        if (dsbindspread.Tables[0].Rows.Count > 0)
        {
            int sno = 0;

            dtable1.Columns.Add("Staff_Code");
            dtable1.Columns.Add("Staff_Name");
            for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
            {
                sno++;
                string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();

                dtrow2 = dtable1.NewRow();
                dtrow2["Staff_Code"] = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();
                dtrow2["Staff_Name"] = name;
                dtable1.Rows.Add(dtrow2);
            }


            gviewstaff.DataSource = dtable1;
            gviewstaff.DataBind();
            gviewstaff.Visible = true;

            //int rowcountt = gviewstaff.Rows.Count;
            //gviewstaff.Height = 278;
            //gviewstaff.PageSize = 25 + (rowcountt * 20);
        }
    }

    protected void fsstaff_selectindexchanged(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
    }

    protected void fsstaff_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
    }

    protected void fsstaff_PreRender(object sender, EventArgs e)
    {
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        //Fpstaff.SaveChanges();
        //string activerow =string.Empty;
        //string activecol =string.Empty;
        //activerow = fsstaff.ActiveSheetView.ActiveRow.ToString();
        //activecol = fsstaff.ActiveSheetView.ActiveColumn.ToString();
        //for (int stafcount = 0; stafcount <= Convert.ToInt32(fsstaff.Sheets[0].RowCount) - 1; stafcount++)
        //{
        //    int isval = Convert.ToInt32(fsstaff.Sheets[0].Cells[stafcount, 0].Value);
        //    if (isval == 1)
        //    {
        //        string staffcode = fsstaff.Sheets[0].Cells[stafcount, 1].Text;
        //        string stname = fsstaff.Sheets[0].Cells[stafcount, 2].Text;
        //        Fpstaff.Sheets[0].RowCount++;
        //        Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = staffcode;
        //        Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = stname;
        //        FarPoint.Web.Spread.CheckBoxCellType ckbox = new FarPoint.Web.Spread.CheckBoxCellType();
        //        Fpstaff.Sheets[0].Columns[0].CellType = ckbox;
        //        FarPoint.Web.Spread.ButtonCellType staf_butt1 = new FarPoint.Web.Spread.ButtonCellType("OneCommand", FarPoint.Web.Spread.ButtonType.PushButton, "Remove");
        //        Fpstaff.Sheets[0].Columns[3].CellType = staf_butt1;
        //        staf_butt1.Text = "Remove";
        //    }
        //}
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        bool flag = false;
        ddlbatchvalue = ((DropDownList)this.usercontrol1.FindControl("ddlBatch")).SelectedValue.ToString();
        ddlsecvalue = ((DropDownList)this.usercontrol1.FindControl("ddlSec")).SelectedValue.ToString();

        for (int staffcount = 0; staffcount <= Convert.ToInt32(gview.Rows.Count) - 1; staffcount++)
        {
            CheckBox chkbox = (gview.Rows[staffcount].FindControl("selectchk") as CheckBox);

            if (chkbox.Checked)
            {
                flag = true;
                subjtree.Visible = true;

                string strsec;
                if (ddlsecvalue.ToString() != "0" && ddlsecvalue.ToString() != "\0")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + ddlsecvalue.ToString() + "'";
                }
                int parent_count = subjtree.Nodes.Count;//----------count parent node value
                if (Chkalterotherdept.Checked == false)
                {

                    for (int i = 0; i < parent_count; i++)
                    {
                        for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                        {
                            if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                            {
                                if (ddlsecvalue.ToString() == "")
                                {
                                    temp_sec = string.Empty;
                                }
                                else
                                {
                                    temp_sec = " and Sections='" + ddlsecvalue.ToString() + "'";
                                }
                                chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;
                                string deletequery = "delete from staff_selector where subject_no=" + Convert.ToInt32(chile_index).ToString() + " and batch_year='" + ddlbatchvalue.ToString() + "' and sections='" + ddlsecvalue.ToString() + "'";

                                int d = obi_access.update_method_wo_parameter(deletequery, "Text");
                            }
                        }
                    }

                    for (int stcolcount = 0; stcolcount <= Convert.ToInt32(gview.Rows.Count) - 1; stcolcount++)
                    {
                        CheckBox chkx = (gview.Rows[stcolcount].FindControl("selectchk") as CheckBox);
                        if (chkx.Checked)
                        {
                            Label code = (Label)gview.Rows[stcolcount].Cells[2].FindControl("lblcodee");
                            Label name = (Label)gview.Rows[stcolcount].Cells[3].FindControl("lblnamee");

                            string stf_code = code.Text;
                            string stf_name = name.Text;
                            if (gview.Rows[stcolcount].Visible == true)
                            {
                                string insertcmd = "insert into staff_selector(subject_no,staff_code,batch_year,sections,dailyflag) values('" + Convert.ToInt32(chile_index).ToString() + "','" + stf_code.ToString() + "','" + ddlbatchvalue.ToString() + "','" + ddlsecvalue.ToString() + "',0)";
                                int n = obi_access.update_method_wo_parameter(insertcmd, "Text");
                                flagstaff = true;

                            }
                        }
                    }
                    dtable.Clear();
                    dtable.Columns.Clear();
                    subjtree_SelectedNodeChanged(sender, e);
                }
                else
                {

                    if (Chkalterotherdept.Checked == true)
                    {
                        div5.Visible = true;
                        div6.Visible = true;
                        Label4.Text = "Do you want to Add  staff to other department for this subject";
                    }
                    else
                        div5.Visible = false;
                }
            }
        }

        if (flagstaff == true)
        {
            FindBtn.Visible = true;
            Chkalterotherdept.Visible = true;
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('saved successfully')", true);
        }
        if (flag == false)
        {
            //FindBtn.Visible = true;
            //Chkalterotherdept.Visible = true;
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Select Staff and proceed')", true);
        }
        gview.Visible = true;
        subjs.Clear();
    }

    protected void exit_Click(object sender, EventArgs e)
    {
        panel3.Visible = false;
    }

    protected void exitpop_Click(object sender, EventArgs e)
    {
        panel3.Visible = false;
        //treepanel.Visible = true;
    }

    //protected void btnstaffadd_Click1(object sender, EventArgs e)
    protected void btnstaffadd_Click1()
    {
        //try
        //{
        //    int isval = 0;
        //    int okflag = 0;
        //    Fpstaff.SaveChanges();
        //    Save.Visible = true;
        //    int count = 0;
        //    //==========Sangeetha  0n 3 Sep 2014
        //    // for removing empty column
        //    for (int k = 0; k < Fpstaff.Sheets[0].Rows.Count; k++)
        //    {
        //        if (Fpstaff.Sheets[0].Cells[k, 2].Text != "")
        //        {
        //            count++;
        //        }
        //    }
        //    Fpstaff.Sheets[0].Rows.Count = count;

        //    //==========================================
        //    for (int fprow = 0; fprow <= Convert.ToInt32(fsstaff.Sheets[0].RowCount) - 1; fprow++)
        //    {
        //        isval = Convert.ToInt32(fsstaff.Sheets[0].Cells[fprow, 0].Value);

        //        string v = string.Empty;
        //        v = fsstaff.Sheets[0].GetText(fprow, 1);
        //        if (isval == 1)
        //        {
        //            //Fpstaff.SaveChanges();
        //            Fpstaff.Visible = true;
        //            okflag = 1;
        //            Fpstaff.SaveChanges();
        //            Fpstaff.Sheets[0].RowCount++;//mani added july'01
        //            // Fpstaff.Sheets[0].RowCount = Convert.ToInt32(Fpstaff.Sheets[0].RowCount) + 1;
        //            int rc = Convert.ToInt32(Fpstaff.Sheets[0].RowCount) - 1;
        //            //Fpstaff.Sheets[0].Cells[rc, 0].Value = 1;
        //            Fpstaff.Sheets[0].Cells[rc, 1].CellType = txt;
        //            Fpstaff.Sheets[0].Cells[rc, 1].Text = fsstaff.Sheets[0].Cells[fprow, 1].Text;
        //            Fpstaff.Sheets[0].Cells[rc, 2].Text = fsstaff.Sheets[0].Cells[fprow, 2].Text;
        //            FarPoint.Web.Spread.CheckBoxCellType ckbox = new FarPoint.Web.Spread.CheckBoxCellType();
        //            Fpstaff.Sheets[0].Columns[0].CellType = ckbox;
        //            FarPoint.Web.Spread.ButtonCellType staf_butt1 = new FarPoint.Web.Spread.ButtonCellType("OneCommand", FarPoint.Web.Spread.ButtonType.PushButton, "Remove");
        //            Fpstaff.Sheets[0].Columns[3].CellType = staf_butt1;
        //            staf_butt1.Text = "Remove";
        //        }
        //        Fpstaff.Sheets[0].PageSize = Fpstaff.Sheets[0].RowCount;
        //        Fpstaff.SaveChanges();
        //    }
        //    if (okflag == 0)
        //    {
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Any Staff')", true);
        //    }
        //    Fpstaff.SaveChanges();
        //    panel3.Visible = false;
        //}
        //catch
        //{
        //    //throw ex;
        //}
    }

    protected void sprdselectrollno_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
    }

    protected void sprdselectrollno_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        // cellroll = true;
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        panel3.Visible = false;
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            if (FpEntry.Sheets[0].RowCount > 0)
            {
                ddlbatchvalue = ((DropDownList)this.usercontrol.FindControl("ddlBatch")).SelectedValue.ToString();
                ddldegreevalue = ((DropDownList)this.usercontrol.FindControl("ddlDegree")).SelectedItem.ToString();
                ddlsecvalue = ((DropDownList)this.usercontrol.FindControl("ddlSec")).SelectedValue.ToString();
                ddlsemvalue = ((DropDownList)this.usercontrol.FindControl("ddlSemYr")).SelectedItem.ToString();
                ddlbranchvalue = ((DropDownList)this.usercontrol.FindControl("ddlBranch")).SelectedItem.ToString();
                string section = string.Empty;
                if (ddlsecvalue.ToString() != null && ddlsecvalue.ToString().Trim() != "" && ddlsecvalue.ToString().Trim() != "-1")
                {
                    section = " Sec : " + ddlsecvalue + "";
                }
                string pagename = "SubjectSchedularepage.aspx";
                string pagedetails = "Subject Chooser Report @ Batch: " + ddlbatchvalue + " Degree : " + ddldegreevalue + "-" + ddlbranchvalue + " Sem : " + ddlsemvalue + " " + section + " ";
                printcontrol.loadspreaddetails(FpEntry, pagename, pagedetails);
                printcontrol.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void rbradio_CheckedChanged(object sender, EventArgs e)
    {
        FpEntry.Visible = false;
        Button1.Visible = false;
        printcontrol.Visible = false;
        btnprint.Visible = false;
    }

    protected void btnsaveok_Click(object sender, EventArgs e)
    {
        mpesave.Hide();
        savesubjectchooser();
    }

    protected void btnsaveCancel_Click(object sender, EventArgs e)
    {
        mpesave.Hide();
    }

    protected void btnstustafgo_Click(object sender, EventArgs e)
    {
        try
        {
            ucstuprint.Visible = false;
            btnstustaffprint.Visible = false;
            printcontrol.Visible = false;
            FpSpread2.Visible = false;
            Dictionary<string, string> rolldic = new Dictionary<string, string>();
            FpSpread2.Sheets[0].AutoPostBack = false;
            btnstustaffsave.Visible = false;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            FpSpread2.Height = 658;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 5;
            FpSpread2.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread2.Sheets[0].ColumnHeader.Rows[0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Rows[1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Columns[0].Width = 40;
            FpSpread2.Sheets[0].Columns[0].CellType = txt;
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread2.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Columns[1].Width = 100;
            FpSpread2.Sheets[0].Columns[1].CellType = txt;
            FpSpread2.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread2.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Columns[2].Width = 200;
            FpSpread2.Sheets[0].Columns[2].CellType = txt;
            FpSpread2.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread2.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Columns[3].Width = 200;
            FpSpread2.Sheets[0].Columns[3].CellType = txt;
            FpSpread2.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpSpread2.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Columns[4].Width = 100;
            FpSpread2.Sheets[0].Columns[4].CellType = txt;
            FpSpread2.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
            FpSpread2.Sheets[0].FrozenColumnCount = 4;
            //FpSpread2.Sheets[0].FrozenColumnCount = 4;

            dtable3.Columns.Add("sno");
            dtable3.Columns.Add("rollno");
            dtable3.Columns.Add("regno");
            dtable3.Columns.Add("stuname");
            dtable3.Columns.Add("stutype");

            if (Session["Rollflag"].ToString() == "0")
            {
                FpSpread2.Sheets[0].Columns[1].Visible = false;
            }
            else
            {
                FpSpread2.Sheets[0].Columns[1].Visible = true;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                FpSpread2.Sheets[0].Columns[2].Visible = false;
            }
            else
            {
                FpSpread2.Sheets[0].Columns[2].Visible = true;
            }
            if (Session["Studflag"].ToString() == "0")
            {
                FpSpread2.Sheets[0].Columns[4].Visible = false;
            }
            else
            {
                FpSpread2.Sheets[0].Columns[4].Visible = true;
            }

            ddlcollegevalue = ((DropDownList)this.usercontrol2.FindControl("ddlcollege")).SelectedValue.ToString();
            ddlbatchvalue = ((DropDownList)this.usercontrol2.FindControl("ddlBatch")).SelectedValue.ToString();
            ddldegreevalue = ((DropDownList)this.usercontrol2.FindControl("ddlDegree")).SelectedValue.ToString();
            ddlbranchvalue = ((DropDownList)this.usercontrol2.FindControl("ddlBranch")).SelectedValue.ToString();
            ddlsemvalue = ((DropDownList)this.usercontrol2.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlsecvalue = ((DropDownList)this.usercontrol2.FindControl("ddlSec")).SelectedValue.ToString();

            string sections = string.Empty;
            string strsec = string.Empty;
            sections = ddlsecvalue.ToString();
            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString().Trim() == "-1" || sections == null)
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + sections.ToString() + "'";
            }
            string cseme = "select distinct current_semester from registration where degree_code ='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and cc=0 and delflag=0 and exam_flag!='debar' ";
            DataSet ds_cssem = obi_access.select_method_wo_parameter(cseme, "text");
            if (ds_cssem.Tables[0].Rows.Count > 0)
            {
                string currentsem = ds_cssem.Tables[0].Rows[0]["current_semester"].ToString();
                if (currentsem == ddlsemvalue.ToString())
                {
                    string strorder = "ORDER BY len(registration.roll_no),registration.roll_no";
                    string serial = obi_access.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                    if (serial != null && serial != "" && serial != "0" && serial.ToLower() != "true")
                    {
                        strorder = "Order by registration.serialno";
                    }
                    else
                    {
                        string orderby_Setting = obi_access.GetFunction("select value from master_Settings where settings='order_by'");
                        if (orderby_Setting == "0")
                        {
                            strorder = "ORDER BY len(registration.roll_no),registration.roll_no";
                        }
                        else if (orderby_Setting == "1")
                        {
                            strorder = "ORDER BY registration.Reg_No";
                        }
                        else if (orderby_Setting == "2")
                        {
                            strorder = "ORDER BY registration.Stud_Name";
                        }
                        else if (orderby_Setting == "0,1,2")
                        {
                            strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Reg_No,registration.stud_name";
                        }
                        else if (orderby_Setting == "0,1")
                        {
                            strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Reg_No";
                        }
                        else if (orderby_Setting == "1,2")
                        {
                            strorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
                        }
                        else if (orderby_Setting == "0,2")
                        {
                            strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Stud_Name";
                        }
                    }
                    string theroy_query = "select subject_type,no_of_papers,subType_no from sub_sem where syll_code= (select syll_code from syllabus_master where  degree_code='" + ddlbranchvalue.ToString() + "' and semester ='" + ddlsemvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "')";
                    DataSet ds_subjects = obi_access.select_method_wo_parameter(theroy_query, "text");

                    string stu_namequery = "select roll_no as rollno, stud_name as studentname,reg_no,stud_type from registration where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and current_semester='" + ddlsemvalue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' " + strorder + "";//Modified By Srinath 6/3/2014
                    DataSet ds_stu_names = obi_access.select_method_wo_parameter(stu_namequery, "text");


                    string total_subjects = " select s.subject_code,s.acronym,s.subject_name ,s.subtype_no,sc.*,(select sm.staff_name from staffmaster sm where sm.staff_code=sc.staffcode ) as staff_name from Subjectchooser sc,subject s, registration r where s.subject_no=sc.subject_no and sc.semester = '" + ddlsemvalue.ToString() + "'and sc.roll_no=r.roll_no and r.degree_code='" + ddlbranchvalue.ToString() + "' and r.batch_year='" + ddlbatchvalue.ToString() + "' and r.current_Semester='" + ddlsemvalue.ToString() + "' " + strsec + "  and StaffCode<>'' order by s.subType_no,sc.paper_order";//barath 07.06.17
                    DataSet ds_totalsubjects = obi_access.select_method_wo_parameter(total_subjects, "text");


                    string selected_subjects = "select subject_code,acronym,subject_no,subtype_no,subject.subject_name from subject where syll_code =(select syll_code from syllabus_master where degree_code='" + ddlbranchvalue.ToString() + "' and semester='" + ddlsemvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "') ";
                    DataSet ds_selectedsubjects = obi_access.select_method_wo_parameter(selected_subjects, "text");
                    string strstaffquery = "select distinct s.subType_no,s.subject_no,s.acronym,s.subject_name,s.subject_code,st.staff_code,sm.staff_name,sections from syllabus_master sy,sub_sem ss,subject s,staff_selector st,staffmaster sm where sm.staff_code=st.staff_code and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and s.subject_no=st.subject_no and sy.Batch_Year=st.batch_year and sy.Batch_Year='" + ddlbatchvalue.ToString() + "' and sy.degree_code='" + ddlbranchvalue.ToString() + "' and sy.semester='" + ddlsemvalue.ToString() + "' " + strsec + " order by s.subType_no,s.subject_name,st.staff_code";
                    DataSet dsstaff = obi_access.select_method_wo_parameter(strstaffquery, "Text");
                    string staffquery = "Select Staff_name,Staff_code from staffmaster";

                    DataSet dsstaffval = obi_access.select_method_wo_parameter(staffquery, "text");

                    if (RollorRegorAdmitNo.Length > 0 && !String.IsNullOrEmpty(RollorRegorAdmitNo))
                    {
                        if (RollorRegorAdmitNo == "roll")
                        {
                            string[] rollnumarray = txtRollNo.Text.Split(',');
                            string rollno = string.Empty;
                            foreach (string item in rollnumarray)
                            {
                                if (string.IsNullOrEmpty(rollno))
                                {
                                    rollno = "'" + item + "'";
                                }
                                else
                                {
                                    rollno += ",'" + item + "'";
                                }
                            }

                            stu_namequery = "select roll_no as rollno, stud_name as studentname,reg_no,stud_type from registration where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and current_semester='" + ddlsemvalue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'  and roll_no in (" + rollno + ") " + strorder + "";
                            ds_stu_names = obi_access.select_method_wo_parameter(stu_namequery, "text");

                            total_subjects = " select s.subject_code,s.acronym,s.subject_name ,s.subtype_no,sc.*,(select sm.staff_name from staffmaster sm where sm.staff_code=sc.staffcode ) as staff_name from Subjectchooser sc,subject s, registration r where s.subject_no=sc.subject_no and sc.semester = '" + ddlsemvalue.ToString() + "'and sc.roll_no=r.roll_no and r.degree_code='" + ddlbranchvalue.ToString() + "' and r.batch_year='" + ddlbatchvalue.ToString() + "' and r.current_Semester='" + ddlsemvalue.ToString() + "' " + strsec + "and r.roll_no in (" + rollno + ")   and StaffCode<>'' order by s.subType_no,sc.paper_order";
                            ds_totalsubjects = obi_access.select_method_wo_parameter(total_subjects, "text");

                            RollorRegorAdmitNo = string.Empty;
                        }
                        else if (RollorRegorAdmitNo == "reg")
                        {
                            string[] Regnumtxtarray = txtRegNo.Text.Split(',');
                            string regno = string.Empty;
                            foreach (string item in Regnumtxtarray)
                            {
                                if (string.IsNullOrEmpty(regno))
                                {
                                    regno = "'" + item + "'";
                                }
                                else
                                {
                                    regno += ",'" + item + "'";
                                }
                            }

                            stu_namequery = "select roll_no as rollno, stud_name as studentname,reg_no,stud_type from registration where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and current_semester='" + ddlsemvalue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and reg_no in (" + regno + ")  " + strorder + "";
                            ds_stu_names = obi_access.select_method_wo_parameter(stu_namequery, "text");

                            total_subjects = " select s.subject_code,s.acronym,s.subject_name ,s.subtype_no,sc.*,(select sm.staff_name from staffmaster sm where sm.staff_code=sc.staffcode ) as staff_name from Subjectchooser sc,subject s, registration r where s.subject_no=sc.subject_no and sc.semester = '" + ddlsemvalue.ToString() + "'and sc.roll_no=r.roll_no and r.degree_code='" + ddlbranchvalue.ToString() + "' and r.batch_year='" + ddlbatchvalue.ToString() + "' and r.current_Semester='" + ddlsemvalue.ToString() + "' " + strsec + " and  reg_no in (" + regno + ")     and StaffCode<>'' order by s.subType_no,sc.paper_order";
                            ds_totalsubjects = obi_access.select_method_wo_parameter(total_subjects, "text");

                            RollorRegorAdmitNo = string.Empty;
                        }
                    }

                    FpSpread2.Sheets[0].RowCount++;
                    DataTable data = new DataTable();
                    FarPoint.Web.Spread.ComboBoxCellType objintcell = new FarPoint.Web.Spread.ComboBoxCellType();
                    FarPoint.Web.Spread.ComboBoxCellType objintcell1 = new FarPoint.Web.Spread.ComboBoxCellType();
                    ArrayList addcoundvlaue = new ArrayList();
                    if (ds_stu_names.Tables.Count > 0 && ds_stu_names.Tables[0].Rows.Count > 0)
                    {
                        if (ds_subjects.Tables.Count > 0 && ds_subjects.Tables[0].Rows.Count > 0 && dsstaff.Tables.Count > 0 && dsstaff.Tables[0].Rows.Count > 0)
                        {
                            for (int su = 0; su < ds_subjects.Tables[0].Rows.Count; su++)
                            {
                                int numberofpapers = Convert.ToInt32(ds_subjects.Tables[0].Rows[su]["no_of_papers"]);
                                FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + numberofpapers;
                                if (numberofpapers > 0)
                                {
                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - numberofpapers, 1, numberofpapers);
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - numberofpapers].Text = ds_subjects.Tables[0].Rows[su]["subject_type"].ToString();

                                    dtable3.Columns.Add("" + ds_subjects.Tables[0].Rows[su]["subject_type"].ToString() + "");

                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - numberofpapers].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - numberofpapers].Width = 100;
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - numberofpapers].CellType = txt;
                                    string subtypeno = ds_subjects.Tables[0].Rows[su]["subType_no"].ToString();
                                    ds_selectedsubjects.Tables[0].DefaultView.RowFilter = "subType_no='" + subtypeno + "'";
                                    DataView dvsub = ds_selectedsubjects.Tables[0].DefaultView;
                                    int srcolu = FpSpread2.Sheets[0].ColumnCount - (numberofpapers + 1);
                                    string[] strcomo1a = new string[dsstaff.Tables[0].Rows.Count + 2];
                                    string[] strcomo1 = new string[dsstaff.Tables[0].Rows.Count + 1];
                                    strcomo1a[0] = "Select For All";
                                    strcomo1a[1] = string.Empty;
                                    strcomo1[0] = string.Empty;
                                    dsstaff.Tables[0].DefaultView.RowFilter = "subType_no='" + subtypeno + "'";
                                    DataView dvbinsub = dsstaff.Tables[0].DefaultView;
                                    if (dvbinsub.Count > 0)
                                    {
                                        //dvbinsub.Sort = "";
                                        string orderBySubject = string.Empty;
                                        if (rbstusubacr.Checked == true)
                                        {
                                            orderBySubject = "acronym";
                                        }
                                        else
                                        {
                                            orderBySubject = "subject_name";
                                        }
                                        if (rbstname.Checked == true)
                                        {
                                            if (!string.IsNullOrEmpty(orderBySubject))
                                            {
                                                orderBySubject += ",staff_name";
                                            }
                                            else
                                            {
                                                orderBySubject = "staff_name";
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(orderBySubject))
                                            {
                                                orderBySubject += ",staff_code";
                                            }
                                            else
                                            {
                                                orderBySubject = "staff_code";
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(orderBySubject))
                                        {
                                            dvbinsub.Sort = orderBySubject;
                                        }
                                    }
                                    for (int ss = 0; ss < dvbinsub.Count; ss++)
                                    {
                                        string subjectno = dvbinsub[ss]["subject_no"].ToString();
                                        string sucode = dvbinsub[ss]["subject_name"].ToString();
                                        string suname = dvbinsub[ss]["acronym"].ToString();
                                        string stcode = dvbinsub[ss]["staff_code"].ToString();
                                        string stname = dvbinsub[ss]["staff_name"].ToString();
                                        //********************* Added by jairam 07-03-2015 *********************** Start
                                        data = dvbinsub.Table;
                                        DataView dv11 = new DataView(data);
                                        if (sections.Trim().ToLower() != "all" && sections.Trim().ToLower() != "-1" && sections.Trim().ToLower() != "" && sections.Trim().ToLower() != "0")
                                        {
                                            dv11.RowFilter = " subject_no='" + subjectno + "'";
                                            if (dv11.Count > 0)
                                            {
                                                if (!addcoundvlaue.Contains(dv11.Count))
                                                {
                                                    if (addcoundvlaue.Count > 0)
                                                    {
                                                        int val = Convert.ToInt32(addcoundvlaue[0]);
                                                        if (val < dv11.Count)
                                                        {
                                                            addcoundvlaue.RemoveAt(0);
                                                            addcoundvlaue.Add(dv11.Count);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        addcoundvlaue.Add(dv11.Count);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (((DropDownList)this.usercontrol2.FindControl("ddlSec")).Items.Count > 0)
                                            {
                                                for (int sec_value = 1; sec_value < ((DropDownList)this.usercontrol2.FindControl("ddlSec")).Items.Count; sec_value++)
                                                {
                                                    string sec_value1 = ((DropDownList)this.usercontrol2.FindControl("ddlSec")).Items[sec_value].Text;
                                                    dv11.RowFilter = " subject_no='" + subjectno + "' and sections='" + sec_value1 + "'";
                                                    if (dv11.Count > 0)
                                                    {
                                                        if (!addcoundvlaue.Contains(dv11.Count))
                                                        {
                                                            if (addcoundvlaue.Count > 0)
                                                            {
                                                                int val = Convert.ToInt32(addcoundvlaue[0]);
                                                                if (val < dv11.Count)
                                                                {
                                                                    addcoundvlaue.RemoveAt(0);
                                                                    addcoundvlaue.Add(dv11.Count);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                addcoundvlaue.Add(dv11.Count);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                dv11.RowFilter = " subject_no='" + subjectno + "'";
                                                if (dv11.Count > 0)
                                                {
                                                    if (!addcoundvlaue.Contains(dv11.Count))
                                                    {
                                                        if (addcoundvlaue.Count > 0)
                                                        {
                                                            int val = Convert.ToInt32(addcoundvlaue[0]);
                                                            if (val < dv11.Count)
                                                            {
                                                                addcoundvlaue.RemoveAt(0);
                                                                addcoundvlaue.Add(dv11.Count);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            addcoundvlaue.Add(dv11.Count);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //********************* Added by jairam 07-03-2015 *********************** End
                                        if (rbstusubacr.Checked == true)
                                        {
                                            sucode = dvbinsub[ss]["acronym"].ToString();
                                        }
                                        else
                                        {
                                            sucode = dvbinsub[ss]["subject_name"].ToString();
                                        }
                                        if (rbstname.Checked == true)
                                        {
                                            stcode = dvbinsub[ss]["staff_name"].ToString();
                                        }
                                        string setvals = sucode + '-' + stcode;
                                        strcomo1a[ss + 2] = setvals;
                                        strcomo1[ss + 1] = setvals;
                                    }
                                    objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1a);
                                    objintcell.ShowButton = true;
                                    objintcell.AutoPostBack = true;
                                    objintcell.UseValue = true;
                                    objintcell1 = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
                                    objintcell1.ShowButton = false;
                                    objintcell1.AutoPostBack = true;
                                    objintcell1.UseValue = true;
                                    int suno = 0;
                                    for (int sub = 0; sub < numberofpapers; sub++)
                                    {
                                        srcolu++;
                                        suno++;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, srcolu].Text = suno.ToString();
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, srcolu].Note = subtypeno;
                                        FpSpread2.Sheets[0].Columns[srcolu].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Columns[srcolu].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Columns[srcolu].Width = 150;
                                        FpSpread2.Sheets[0].Columns[srcolu].CellType = objintcell1;
                                        FpSpread2.Sheets[0].Cells[0, srcolu].CellType = objintcell;
                                    }
                                }
                            }
                            btnstustaffprint.Visible = true;
                            btnstustaffsave.Visible = true;
                            FpSpread2.Visible = true;
                            FpSpread2.SaveChanges();
                            FpSpread2.Sheets[0].SpanModel.Add(0, 0, 1, 5);
                            int srno = 0;
                            FarPoint.Web.Spread.IntegerCellType intc = new FarPoint.Web.Spread.IntegerCellType();
                            for (int i = 0; i < ds_stu_names.Tables[0].Rows.Count; i++)
                            {
                                string name = ds_stu_names.Tables[0].Rows[i]["studentname"].ToString();
                                string roll = ds_stu_names.Tables[0].Rows[i]["rollno"].ToString();
                                string reg = ds_stu_names.Tables[0].Rows[i]["reg_no"].ToString();
                                string type = ds_stu_names.Tables[0].Rows[i]["stud_type"].ToString();
                                srno++;
                                int value = 0;
                                if (addcoundvlaue.Count > 0)
                                    value = Convert.ToInt32(addcoundvlaue[0]);
                                Session["inc_value"] = Convert.ToString(value);
                                FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + value;  //********************* Added by jairam 07-03-2015 *********************** 
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - value, 0].CellType = intc;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - value, 0].Text = srno.ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - value, 1].Text = roll.ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - value, 2].Text = reg.ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - value, 3].Text = name.ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - value, 4].Text = type.ToString();
                                //for (int j = 0; j < value; j++)
                                //{
                                //    for (int col = 5; col < FpSpread2.Sheets[0].ColumnCount; col++)
                                //    {
                                //        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - value + j, col].CellType = objintcell1;
                                //    }
                                //}
                            }
                            int co = 0;
                            string tempno = string.Empty;
                            for (int col = 5; col < FpSpread2.Sheets[0].ColumnCount; col++)
                            {
                                string subty = FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Note;
                                if (tempno != subty)
                                {
                                    co = 1;
                                    tempno = subty;
                                }
                                else
                                {
                                    co++;
                                }
                                if (ds_totalsubjects.Tables.Count > 0 && ds_totalsubjects.Tables[0].Rows.Count > 0)
                                {
                                    int inc = Convert.ToInt32(Session["inc_value"]);  //********************* Added by jairam 07-03-2015 *********************** 
                                    for (int ro = 1; ro < FpSpread2.Sheets[0].RowCount; ro += inc)
                                    {
                                        string roll_no = FpSpread2.Sheets[0].Cells[ro, 1].Text.ToString();
                                        ds_totalsubjects.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_no + "' and subType_no='" + subty + "' and paper_order='" + co.ToString() + "'";
                                        DataView dvstustaf = ds_totalsubjects.Tables[0].DefaultView;
                                        if (dvstustaf.Count > 0)
                                        {
                                            string staffcode = dvstustaf[0]["Staffcode"].ToString();
                                            string staffneme = dvstustaf[0]["staff_name"].ToString();
                                            string subcode = dvstustaf[0]["subject_code"].ToString();
                                            string sacr = dvstustaf[0]["acronym"].ToString();
                                            string subname = dvstustaf[0]["subject_name"].ToString();
                                            if (staffcode.Contains(';') == true)  //********************* Added by jairam 07-03-2015 *********************** Start
                                            {
                                                string[] splitstaff = staffcode.Split(';');
                                                if (splitstaff.Length > 0)
                                                {
                                                    int val = 0;
                                                    for (int st = 0; st <= splitstaff.GetUpperBound(0) && splitstaff.Length <= inc; st++)
                                                    {
                                                        string staff_code_split = Convert.ToString(splitstaff[st]);
                                                        if (staff_code_split.Trim() != "")
                                                        {
                                                            string setstaff = string.Empty;
                                                            if (rbstusubacr.Checked == true)
                                                            {
                                                                subcode = sacr;
                                                            }
                                                            else
                                                            {
                                                                subcode = subname;
                                                            }
                                                            if (rbstname.Checked == true)
                                                            {
                                                                dsstaffval.Tables[0].DefaultView.RowFilter = "staff_code='" + staff_code_split + "'";
                                                                DataView dvstname = dsstaffval.Tables[0].DefaultView;
                                                                if (dvstname.Count > 0)
                                                                {
                                                                    staffneme = Convert.ToString(dvstname[0]["staff_name"]).Trim();
                                                                    staff_code_split = staffneme;
                                                                }
                                                            }
                                                            if (subcode.Trim() != "" && subcode != null && staff_code_split.Trim() != "" && staffcode != null)
                                                            {
                                                                setstaff = subcode + '-' + staff_code_split;
                                                            }
                                                            FpSpread2.Sheets[0].Cells[ro + val, col].Text = setstaff;
                                                            FpSpread2.Sheets[0].Cells[ro + val, col].Locked = false;
                                                            val++;
                                                        }
                                                    }
                                                }
                                            }  //********************* Added by jairam 07-03-2015 *********************** End
                                            else
                                            {
                                                string setstaff = string.Empty;
                                                if (rbstusubacr.Checked == true)
                                                {
                                                    subcode = sacr;
                                                }
                                                else
                                                {
                                                    subcode = subname;
                                                }
                                                if (rbstname.Checked == true)
                                                {
                                                    staffcode = staffneme;
                                                }
                                                if (subcode.Trim() != "" && subcode != null && staffcode.Trim() != "" && staffcode != null)
                                                {
                                                    setstaff = subcode + '-' + staffcode;
                                                }
                                                FpSpread2.Sheets[0].Cells[ro, col].Text = setstaff;
                                                FpSpread2.Sheets[0].Cells[ro, col].Locked = false;
                                            }
                                        }
                                        else
                                        {
                                            //FpSpread2.Sheets[0].Cells[ro, col].Text =string.Empty;
                                            //FpSpread2.Sheets[0].Cells[ro, col].Locked = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblstustaferr.Visible = true;
                            lblstustaferr.Text = "Please Allot Subject Information";
                            FpSpread2.Visible = false;
                        }
                    }
                    else
                    {
                        lblstustaferr.Visible = true;
                        lblstustaferr.Text = "Students Not Available In This Semester";
                        FpSpread2.Visible = false;
                    }
                }
                else
                {
                    lblstustaferr.Visible = true;
                    lblstustaferr.Text = "Students Not Available In This Semester";
                    FpSpread2.Visible = false;
                }
            }
            else
            {
                lblstustaferr.Visible = true;
                lblstustaferr.Text = "Students Not Available In This Semester";
                FpSpread2.Visible = false;
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.Height = 608;
            FpSpread2.Width = 950;
            FpSpread2.SaveChanges();
        }
        catch (Exception ex)
        {
            lblstustaferr.Visible = true;
            lblstustaferr.Text = ex.ToString();
        }
    }

    protected void btnstustaffsave_Click(object sender, EventArgs e)
    {
        try
        {
            Hashtable hat = new Hashtable();
            ddlcollegevalue = ((DropDownList)this.usercontrol2.FindControl("ddlcollege")).SelectedValue.ToString();
            ddlbatchvalue = ((DropDownList)this.usercontrol2.FindControl("ddlBatch")).SelectedValue.ToString();
            ddldegreevalue = ((DropDownList)this.usercontrol2.FindControl("ddlDegree")).SelectedValue.ToString();
            ddlbranchvalue = ((DropDownList)this.usercontrol2.FindControl("ddlBranch")).SelectedValue.ToString();
            ddlsemvalue = ((DropDownList)this.usercontrol2.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlsecvalue = ((DropDownList)this.usercontrol2.FindControl("ddlSec")).SelectedValue.ToString();
            string sections = string.Empty;
            string strsec = string.Empty;
            sections = ddlsecvalue.ToString();
            if (sections.ToString().ToLower().Trim() == "all" || sections.ToString().Trim() == string.Empty || sections.ToString().Trim() == "-1" || sections.Trim() == "" || sections == null)
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + sections.ToString() + "'";
            }
            bool saveflag = false;
            FpSpread2.SaveChanges();
            hat.Clear();
            string subvald = string.Empty;
            string straffcode = string.Empty;
            int incvalue = Convert.ToInt32(Session["inc_value"]);
            for (int r1 = 1; r1 < FpSpread2.Sheets[0].RowCount; r1 += incvalue)
            {
                string roll = Convert.ToString(FpSpread2.Sheets[0].Cells[r1, 1].Text).Trim();
                for (int c1 = 5; c1 < FpSpread2.Sheets[0].ColumnCount; c1++)
                {
                    string subtext = Convert.ToString(FpSpread2.Sheets[0].Cells[r1, c1].Text).Trim();
                    if (subtext.Trim() != "" && subtext != null)
                    {
                        string[] stm = subtext.Split('-');
                        subvald = string.Empty;
                        straffcode = stm[stm.GetUpperBound(0)].ToString();
                        for (int sts = 0; sts < stm.GetUpperBound(0); sts++)
                        {
                            if (subvald.Trim() != "")
                            {
                                subvald = subvald + '-' + stm[sts];
                            }
                            else
                            {
                                subvald = stm[sts];
                            }
                        }
                        string setbva = roll + '-' + subvald;
                        if (!hat.Contains(setbva))
                        {
                            hat.Add(setbva, setbva);
                        }
                        else
                        {
                            lblstustaferr.Visible = true;
                            lblstustaferr.Text = "Student Cannot Select The Same Subjects More Than Once";
                            return;
                        }
                    }
                    ArrayList addarray = new ArrayList();  //********************* Added by jairam 07-03-2015 *********************** Start
                    for (int inval = 0; inval < incvalue; inval++)
                    {
                        string subtext1 = FpSpread2.Sheets[0].Cells[r1 + inval, c1].Text.ToString();
                        if (subtext1.Trim() != "" && subtext1 != null)
                        {
                            string[] stm = subtext1.Split('-');
                            subvald = string.Empty;
                            straffcode = stm[stm.GetUpperBound(0)].ToString();
                            for (int sts = 0; sts < stm.GetUpperBound(0); sts++)
                            {
                                if (subvald.Trim() != "")
                                {
                                    subvald = subvald + '-' + stm[sts];
                                }
                                else
                                {
                                    subvald = stm[sts];
                                }
                            }
                            string setbva = roll + '-' + subvald;
                            string setbva1 = roll + '-' + subvald + '-' + straffcode;
                            if (addarray.Count == 0)
                            {
                                addarray.Add(setbva);
                            }
                            if (addarray.Contains(setbva))
                            {
                                if (!addarray.Contains(setbva1))
                                {
                                    addarray.Add(setbva1);
                                }
                                else
                                {
                                    lblstustaferr.Visible = true;
                                    lblstustaferr.Text = "Student Cannot Select The Same Subjects and Staff More Than Once";
                                    return;
                                }
                            }
                            else
                            {
                                lblstustaferr.Visible = true;
                                lblstustaferr.Text = "Student Cannot Select The Different Subjects More Than Once";
                                return;
                            }
                        }
                    }
                }
            }
            string strstaffquery = "select distinct s.subType_no,s.subject_no,s.acronym,s.subject_name,s.subject_code,st.staff_code,sm.staff_name from syllabus_master sy,sub_sem ss,subject s,staff_selector st,staffmaster sm where sm.staff_code=st.staff_code and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and s.subject_no=st.subject_no and sy.Batch_Year=st.batch_year and sy.Batch_Year='" + ddlbatchvalue.ToString() + "' and sy.degree_code='" + ddlbranchvalue.ToString() + "' and sy.semester='" + ddlsemvalue.ToString() + "' " + strsec + " order by s.subType_no,s.subject_name,st.staff_code";
            DataSet dssub = obi_access.select_method_wo_parameter(strstaffquery, "text");
            Hashtable h = new Hashtable();
            for (int r = 1; r < FpSpread2.Sheets[0].RowCount; r += incvalue)
            {
                string roll_no = FpSpread2.Sheets[0].Cells[r, 1].Text.ToString();
                h.Add(roll_no, roll_no);
                hat.Clear();
                hat.Add("roll_no", roll_no);
                hat.Add("semester", ddlsemvalue.ToString());
                int sav = obi_access.insert_method("sp_upd_student_staff_selector_Default", hat, "sp");
                if (sav != 0)
                {
                    saveflag = true;
                }
                //  int sav = 0;
                string tempno = string.Empty;
                int co = 0;
                for (int c = 5; c < FpSpread2.Sheets[0].ColumnCount; c++)
                {
                    string subty = FpSpread2.Sheets[0].ColumnHeader.Cells[1, c].Note;
                    string paperOrder = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[1, c].Text).Trim();
                    int paperOrderNo = 0;
                    int.TryParse(paperOrder, out paperOrderNo);
                    if (tempno != subty)
                    {
                        co = 1;
                        tempno = subty;
                    }
                    else
                    {
                        co++;
                    }
                    if (paperOrderNo == 0)
                    {
                        paperOrderNo = co;
                        paperOrder = co.ToString().Trim();
                    }
                    if (FpSpread2.Sheets[0].Cells[r, c].Locked == false)
                    {
                        //saveflag = true;
                        string staffcode = string.Empty;
                        string subno = string.Empty;
                        string subtypeno = string.Empty;
                        for (int inc = 0; inc < incvalue; inc++)  //********************* Added by jairam 07-03-2015 *********************** Start
                        {
                            string gettext = FpSpread2.Sheets[0].Cells[r + inc, c].Text.ToString();
                            string[] spt = gettext.Split('-');
                            string subjectfil = " ";
                            string staffil = " ";
                            if (spt.GetUpperBound(0) >= 1)
                            {
                                subvald = string.Empty;
                                straffcode = spt[spt.GetUpperBound(0)].ToString();
                                for (int sts = 0; sts < spt.GetUpperBound(0); sts++)
                                {
                                    if (subvald.Trim() != "")
                                    {
                                        subvald = subvald + '-' + spt[sts];
                                    }
                                    else
                                    {
                                        subvald = spt[sts];
                                    }
                                }
                                string gets = subvald;
                                string getstf = straffcode;
                                if (gets.Trim() != "" && gets != null && gets.Length >= 1)
                                {
                                    if (rbstusubcode.Checked == true)
                                    {
                                        subjectfil = " subject_name='" + gets + "'";
                                    }
                                    else
                                    {
                                        subjectfil = " acronym='" + gets + "'";
                                    }
                                }
                                if (getstf.Trim() != "" && getstf != null && getstf.Length >= 1)
                                {
                                    if (rbstname.Checked == true)
                                    {
                                        staffil = " staff_name='" + getstf + "'";
                                        if (subjectfil.Trim() != "")
                                        {
                                            staffil = " and " + staffil;
                                        }
                                    }
                                }
                                dssub.Tables[0].DefaultView.RowFilter = subjectfil + staffil;
                                DataView dvgetdate = dssub.Tables[0].DefaultView;
                                if (dvgetdate.Count > 0)
                                {
                                    if (rbstname.Checked == true)
                                    {
                                        if (staffcode == "")
                                        {
                                            staffcode = dvgetdate[0]["staff_code"].ToString();
                                        }
                                        else
                                        {
                                            staffcode = staffcode + ";" + dvgetdate[0]["staff_code"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (staffcode == "")
                                        {
                                            staffcode = getstf;
                                        }
                                        else
                                        {
                                            staffcode = staffcode + ";" + getstf;
                                        }
                                    }
                                    subno = dvgetdate[0]["subject_no"].ToString();
                                    subtypeno = dvgetdate[0]["subtype_no"].ToString();
                                }
                            }
                        }  //********************* Added by jairam 07-03-2015 *********************** End
                        if (staffcode != null && staffcode != "" && subno != null && subno.Trim() != "")
                        {
                            hat.Clear();
                            hat.Add("subject_no", subno);//4137
                            hat.Add("staffcode", staffcode);
                            hat.Add("roll_no", roll_no);
                            hat.Add("subtype_no", subtypeno);
                            hat.Add("semester", ddlsemvalue.ToString());
                            hat.Add("paperOrder", paperOrderNo);
                            sav = obi_access.insert_method("sp_upd_student_staff_selector", hat, "sp");
                            if (sav != 0)
                            {
                                saveflag = true;
                            }
                            //string update = "if exists(select * from subjectChooser where roll_no='" + roll_no + "' and subject_no=" + subno + " and semester=" + ddlsemvalue.ToString() + " and subtype_no=" + subtypeno + ") update subjectChooser set staffcode='" + staffcode + "' where roll_no='" + roll_no + "' and   subject_no=" + subno + " and semester=" + ddlsemvalue.ToString() + "  and subtype_no=" + subtypeno + "";
                            //sav = obi_access.update_method_wo_parameter(update, "Text");
                        }
                    }
                }
            }
            if (saveflag == true)
            {
                div8.Visible = true;
                div9.Visible = true;
                Label5.Visible = true;
                Label5.Text = "Saved Successfully";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('saved successfully')", true);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('saved successfully!')", true);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "customScript", "<script>alert('Saved Successfully');</script>", false);
            }
            else
            {
                div8.Visible = true;
                div9.Visible = true;
                Label5.Visible = true;
                Label5.Text = "Not Saved";
                // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "customScript", "<script>alert('Not Saved');</script>", false);
            }
        }
        catch (Exception ex)
        {
            lblstustaferr.Visible = true;
            lblstustaferr.Text = ex.ToString();
        }
    }

    protected void btnpopupalert_Click(object sender, EventArgs e)
    {
        div9.Visible = false;
        div8.Visible = false;
    }

    protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string spread = string.Empty;
            Control control = null;
            string ctrlname = Page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                control = Page.FindControl(ctrlname);
                spread = ctrlname.ToString();
            }
            string spreadname = string.Empty;
            if (spread != "")
            {
                string[] spiltspreadname = spread.Split('$');
                if (spiltspreadname.GetUpperBound(0) > 1)
                {
                    spreadname = spiltspreadname[4].ToString().Trim();
                    if (spreadname.ToString().Trim() != "fpspread2")
                    {
                        string actrow = FpSpread2.Sheets[0].ActiveRow.ToString();  //e.SheetView.ActiveRow.ToString();
                        string actcol = FpSpread2.Sheets[0].ActiveColumn.ToString();  //e.SheetView.ActiveColumn.ToString();
                        string last = e.CommandArgument.ToString();
                        if (actrow == "0")
                        {
                            if (last == "0")
                            {
                                flag_true = false;
                            }
                            else
                            {
                                flag_true = true;
                            }
                        }
                        if (flag_true == false && actrow == "0" && actcol.Trim() != "-1")
                        {
                            actcol = e.SheetView.ActiveColumn.ToString();
                            string seltext = e.EditValues[Convert.ToInt32(actcol)].ToString();
                            e.Handled = true;
                            if (seltext.Trim() == "Select For All" || seltext.Trim().ToLower() == "system.object")
                            {
                                seltext = string.Empty;
                            }
                            int inc = Convert.ToInt32(Session["inc_value"]);
                            for (int j = 1; j < Convert.ToInt32(FpSpread2.Sheets[0].RowCount); j += inc)
                            {
                                if (FpSpread2.Sheets[0].Cells[j, Convert.ToInt32(actcol)].Locked == false)
                                {
                                    FpSpread2.Sheets[0].Cells[j, Convert.ToInt32(actcol)].Text = seltext.ToString();
                                }
                                else
                                {
                                    FpSpread2.Sheets[0].Cells[j, Convert.ToInt32(actcol)].Text = string.Empty;
                                }
                            }
                            flag_true = true;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btnstustaffprint_Click(object sender, EventArgs e)
    {
        if (FpSpread2.Sheets[0].RowCount > 0)
        {
            ddlbatchvalue = ((DropDownList)this.usercontrol2.FindControl("ddlBatch")).SelectedValue.ToString();
            ddldegreevalue = ((DropDownList)this.usercontrol2.FindControl("ddlDegree")).SelectedItem.ToString();
            ddlsecvalue = ((DropDownList)this.usercontrol2.FindControl("ddlSec")).SelectedValue.ToString();
            ddlsemvalue = ((DropDownList)this.usercontrol2.FindControl("ddlSemYr")).SelectedItem.ToString();
            ddlbranchvalue = ((DropDownList)this.usercontrol2.FindControl("ddlBranch")).SelectedItem.ToString();
            string section = string.Empty;
            if (ddlsecvalue.ToString() != null && ddlsecvalue.ToString().Trim() != "" && ddlsecvalue.ToString().Trim() != "-1")
            {
                section = " Sec : " + ddlsecvalue + "";
            }
            string pagename = "SubjectSchedularepage.aspx";
            string pagedetails = "Student Staff Chooser Report @ Batch: " + ddlbatchvalue + " Degree : " + ddldegreevalue + "-" + ddlbranchvalue + " Sem : " + ddlsemvalue + " " + section + " ";
            ucstuprint.loadspreaddetails(FpSpread2, pagename, pagedetails);
            ucstuprint.Visible = true;
        }
        else
        {
            ucstuprint.Visible = false;
        }
    }

    protected void StudentStaffchanged(object sender, EventArgs e)
    {
        lblstustaferr.Visible = false;
        FpSpread2.Visible = false;
        btnstustaffsave.Visible = false;
        btnstustaffprint.Visible = false;
    }

    #region modified on nov 22 2017

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRollNo(string prefixText, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = string.Empty;

        string[] values = contextKey.Split('-');
        string degree = values[0];
        string branch = values[2];
        string section = values[3];
        string semester = values[4];
        string batchyr = values[1];

        if (section.Trim().ToLower() == "all" || section.Trim().ToLower() == "" || section.Trim().ToLower() == "0")
        {
            query = "select Roll_No from Registration r where  Roll_No Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' order by Roll_No";
        }
        else
        {
            query = "select Roll_No from Registration r where  Roll_No Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' and Sections='" + section + "' order by Roll_No";
        }

        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRegNo(string prefixText, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = string.Empty;

        string[] details = contextKey.Split('-');
        string degree = details[0];
        string branch = details[2];
        string section = details[3];
        string semester = details[4];
        string batchyr = details[1];

        if (section.Trim().ToLower() == "all")
        {
            query = "select Reg_No from Registration r where  Reg_No Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' order by Reg_No";
        }
        else
        {
            query = "select Reg_No from Registration r where  Reg_No Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' and Sections='" + section + "' order by Reg_No";
        }

        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetAdmitNo(string prefixText, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = string.Empty;
        //query = "select Roll_Admit from Registration where DelFlag=0 and Exam_Flag <>'Debar' and Roll_Admit Like '" + prefixText + "%'   order by Roll_Admit";

        string[] values = contextKey.Split('-');
        string degree = values[0];
        string branch = values[2];
        string section = values[3];
        string semester = values[4];
        string batchyr = values[1];

        if (section.Trim().ToLower() == "all")
        {
            query = "select Roll_Admit from Registration r where  Roll_Admit Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' order by Roll_Admit";
        }
        else
        {
            query = "select Roll_Admit from Registration r where  Roll_Admit Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' and Sections='" + section + "' order by Roll_Admit";
        }

        name = ws.Getname(query);
        return name;
    }

    public void getvaluefromddl()
    {
        lblValue.Text = string.Empty;
        string batch = ((DropDownList)this.usercontrol2.FindControl("ddlBatch")).SelectedValue.ToString();
        string degree = ((DropDownList)this.usercontrol2.FindControl("ddlDegree")).SelectedValue.ToString();
        string branch = ((DropDownList)this.usercontrol2.FindControl("ddlBranch")).SelectedValue.ToString();
        string sem = ((DropDownList)this.usercontrol2.FindControl("ddlSemYr")).SelectedValue.ToString();
        string Section = (((DropDownList)this.usercontrol2.FindControl("ddlSec")).SelectedValue.ToString() == "") ? "all" : ((DropDownList)this.usercontrol2.FindControl("ddlSec")).SelectedValue.ToString();

        lblValue.Text = degree + "-" + batch + "-" + branch + "-" + Section + "-" + sem + "";

    }

    protected void btnSearchBy_OnClick(object sender, EventArgs e)
    {
        SetStudentWiseSettings();
        getvaluefromddl();
        divPopSearchstudent.Visible = true;
    }

    protected void btnSearchbyrollorreg_Click(object sender, EventArgs e)
    {
        try
        {
            divPopSearchstudent.Visible = false;   //RollorRegorAdmitNo
            if (ddlSearchBy.SelectedItem.Text.ToLower() == "roll no")
            {
                RollorRegorAdmitNo = "roll";
            }
            else if (ddlSearchBy.SelectedItem.Text.ToLower() == "register no")
            {
                RollorRegorAdmitNo = "reg";
            }
            btnstustafgo_Click(sender, e);
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnsearchByClose_Click(object sender, EventArgs e)
    {
        divPopSearchstudent.Visible = false;
        txtRegNo.Text = string.Empty;
        txtRollNo.Text = string.Empty;
        txtAdmissionNo.Text = string.Empty;
        RollorRegorAdmitNo = string.Empty;
    }

    private DataSet GetSettings()
    {
        DataSet dsSettings = new DataSet();
        Hashtable ht = new Hashtable();

        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string groupCode = Convert.ToString(Session["group_code"]).Trim();
                string[] groupUser = Convert.ToString(groupCode).Trim().Split(';');
                if (groupUser.Length > 0)
                {
                    groupCode = groupUser[0].Trim();
                }
                if (!string.IsNullOrEmpty(groupCode.Trim()))
                {
                    grouporusercode = " and  group_code=" + Convert.ToString(groupCode).Trim() + "";
                }
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            if (!string.IsNullOrEmpty(grouporusercode))
            {
                string Master1 = "select distinct settings,value,ROW_NUMBER() over (ORDER BY settings DESC) as SetValue1,Case when settings='Admission No' then '1' when settings='Register No' then '2' when settings='Roll No' then '3' end as SetValue from Master_Settings where settings in('Roll No','Register No','Admission No') and value='1' " + grouporusercode + "";
                dsSettings = dirAcc.selectDataSet(Master1);
            }
            else
            {
                dsSettings.Clear();
                DataTable dt = new DataTable();
                dt.Columns.Clear();
                dt.Rows.Clear();
                dt.Columns.Add("settings");
                dt.Columns.Add("SetValue");
                dt.Rows.Add("Admission No", "1");
                dt.Rows.Add("Register No", "2");
                dt.Rows.Add("Roll No", "3");
                dsSettings.Tables.Add(dt);
            }
        }
        catch (Exception ex)
        {

        }
        return dsSettings;
    }

    private void SetStudentWiseSettings()
    {
        try
        {
            DataSet dsSearchBy = new DataSet();
            dsSearchBy = GetSettings();
            if (dsSearchBy.Tables.Count > 0 && dsSearchBy.Tables[0].Rows.Count > 0)
            {
                ddlSearchBy.DataSource = dsSearchBy;
                ddlSearchBy.DataTextField = "settings";
                ddlSearchBy.DataValueField = "SetValue";
                ddlSearchBy.DataBind();
                ddlSearchBy.SelectedIndex = 0;
                SelectSearchBy(ddlSearchBy);
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void SelectSearchBy(DropDownList ddlSearch)
    {
        try
        {
            txtAdmissionNo.Text = string.Empty;
            txtRollNo.Text = string.Empty;
            txtRegNo.Text = string.Empty;

            txtAdmissionNo.Visible = false;
            txtRollNo.Visible = false;
            txtRegNo.Visible = false;
            if (ddlSearch.Items.Count > 0)
            {
                string selectedValue = Convert.ToString(ddlSearch.SelectedValue).Trim();
                switch (selectedValue)
                {
                    case "1":
                        txtAdmissionNo.Visible = true;
                        break;
                    case "2":
                        txtRegNo.Visible = true;
                        break;
                    case "3":
                        txtRollNo.Visible = true;
                        break;
                }
            }
        }
        catch
        {
        }
    }

    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectSearchBy(ddlSearchBy);
    }

    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            lblStaffCode.Text = "";
            lblSubNo.Text = "";
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(lblStaffCode.Text) && !string.IsNullOrEmpty(lblSubNo.Text))
            {
                ddlcollegevalue = ((DropDownList)this.usercontrol.FindControl("ddlcollege")).SelectedValue.ToString();
                ddlbatchvalue = ((DropDownList)this.usercontrol.FindControl("ddlBatch")).SelectedValue.ToString();
                ddldegreevalue = ((DropDownList)this.usercontrol.FindControl("ddlDegree")).SelectedValue.ToString();
                ddlsecvalue = ((DropDownList)this.usercontrol.FindControl("ddlSec")).SelectedValue.ToString();
                ddlsemvalue = ((DropDownList)this.usercontrol.FindControl("ddlSemYr")).SelectedValue.ToString();
                ddlbranchvalue = ((DropDownList)this.usercontrol.FindControl("ddlBranch")).SelectedValue.ToString();
                //Session["SEC"] = Convert.ToString(ddlsecvalue);
                //Session["Sem"] = Convert.ToString(ddlsemvalue);
                //Session["ddlcollegevalue"] = Convert.ToString(ddlcollegevalue);
                //Session["ddlbatchvalue"] = Convert.ToString(ddlcollegevalue);
                //Session["ddldegreevalue"] = Convert.ToString(ddldegreevalue);
                if (!string.IsNullOrEmpty(Convert.ToString(Session["Sem"])) && !string.IsNullOrEmpty(Convert.ToString(Session["ddlcollegevalue"])) && !string.IsNullOrEmpty(Convert.ToString(Session["ddlbatchvalue"])) && !string.IsNullOrEmpty(Convert.ToString(Session["ddldegreevalue"])))
                {
                    ddlsemvalue = Convert.ToString(Session["Sem"]);
                    ddlcollegevalue = Convert.ToString(Session["ddlcollegevalue"]);
                    ddlbatchvalue = Convert.ToString(Session["ddlbatchvalue"]);
                    ddldegreevalue = Convert.ToString(Session["ddldegreevalue"]);
                }



                string temp_sec = string.Empty;
                if (Convert.ToString(Session["SEC"]) == "")
                {
                    temp_sec = string.Empty;
                }
                else
                {
                    temp_sec = "  and Sections='" + Convert.ToString(Session["SEC"]) + "'";
                }
                string SubjectCh = " select Roll_No from Registration where Batch_Year='" + ddlbatchvalue + "' and degree_code='" + ddlbranchvalue + "' and Current_Semester='" + ddlsemvalue + "'" + temp_sec;
                DataTable dtReg = dirAcc.selectDataTable(SubjectCh);
                if (dtReg.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtReg.Rows)
                    {
                        string RollNo = Convert.ToString(dt["Roll_No"]);
                        DataTable dtSubjectChooser = dirAcc.selectDataTable("select * from subjectChooser where subject_no='" + lblSubNo.Text + "' and staffcode like '%" + lblStaffCode.Text + "%' and roll_no='" + RollNo + "'");
                        if (dtSubjectChooser.Rows.Count > 0)
                        {
                            string staffCode = Convert.ToString(dtSubjectChooser.Rows[0]["staffcode"]);
                            string NEWstaff = string.Empty;
                            if (staffCode.Contains(';'))
                            {
                                string[] MultiStaff = staffCode.Split(';');
                                for (int i = 0; i < MultiStaff.Count(); i++)
                                {
                                    string staff = Convert.ToString(MultiStaff[i]);
                                    if (staff != lblStaffCode.Text)
                                    {
                                        if (string.IsNullOrEmpty(NEWstaff))
                                            NEWstaff = staff;
                                        else
                                            NEWstaff = NEWstaff + ";" + staff;
                                    }
                                }
                                if (!string.IsNullOrEmpty(NEWstaff))
                                {
                                    string updateQry = " update subjectChooser  SET   StaffCode='" + NEWstaff + "' where subject_no='" + lblSubNo.Text + "' and roll_no='" + RollNo + "'";
                                    int d = obi_access.update_method_wo_parameter(updateQry, "text");
                                }
                            }
                            else
                            {
                                NEWstaff = string.Empty;
                                string updateQry = " update subjectChooser  SET  StaffCode='" + NEWstaff + "' where subject_no='" + lblSubNo.Text + "' and roll_no='" + RollNo + "'";
                                int d = obi_access.update_method_wo_parameter(updateQry, "text");

                            }

                        }
                    }
                    string deletequery = "delete from staff_selector where subject_no='" + lblSubNo.Text + "' and staff_code='" + lblStaffCode.Text + "' and batch_year='" + ddlbatchvalue.ToString() + "'" + temp_sec;
                    int d1 = obi_access.update_method_wo_parameter(deletequery, "Text");
                    if (d1 != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Removed successfully')", true);
                        subjtree_SelectedNodeChanged(sender, e);
                    }
                }
            }
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblStaffCode.Text = "";
            lblSubNo.Text = "";
        }
        catch (Exception ex)
        {

        }
    }

    protected void cb_Category_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_Category.Checked == true)
        {
            for (int i = 0; i < cbl_Category.Items.Count; i++)
            {
                cbl_Category.Items[i].Selected = true;
                txt_Category.Text = "Category(" + (cbl_Category.Items.Count) + ")";
            }
            panel_Category.Focus();
        }
        else
        {
            for (int i = 0; i < cbl_Category.Items.Count; i++)
            {
                cbl_Category.Items[i].Selected = false;
                txt_Category.Text = "---Select---";
            }
        }
    }

    protected void cbl_Category_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel_Category.Focus();
        int desigcount = 0;
        for (int i = 0; i < cbl_Category.Items.Count; i++)
        {
            if (cbl_Category.Items[i].Selected == true)
            {
                desigcount = desigcount + 1;
                txt_Category.Text = "Category(" + desigcount.ToString() + ")";
            }
        }
        if (desigcount == 0)
        {
            txt_Category.Text = "---Select---";
        }
        cb_Category.Checked = false;
    }

    public void bindstaffcata(string college)
    {
        try
        {
            DataSet ds = new DataSet();
            txt_Category.Text = "---Select---";
            cb_Category.Checked = false;
            string collvalue = college;
            cbl_Category.Items.Clear();
            if (collvalue == "---Select---")
            {
                collvalue = Session["collegecode"].ToString();
            }
            height = 0;
            cbl_Category.Items.Clear();
            ds.Clear();
            ds = obi_access.loadcategory(collvalue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Category.DataSource = ds;
                cbl_Category.DataTextField = "category_name";
                cbl_Category.DataValueField = "Category_Code";
                cbl_Category.DataBind();
                for (int i = 0; i < cbl_Category.Items.Count; i++)
                {
                    cbl_Category.Items[i].Selected = true;
                    height++;
                }
                txt_Category.Text = "Category(" + cbl_Category.Items.Count + ")";
                cb_Category.Checked = true;
            }
            if (height > 10)
            {
                panel_Category.Height = 300;
            }
            else
            {
                panel_Category.Height = 150;
            }
        }
        catch (Exception)
        {
        }
    }

    protected void BtnCategory_Click(object sender, EventArgs e)
    {
        try
        {
            //fsstaff.Sheets[0].RowCount = 0; 
            loadfsstaff();
        }
        catch
        {
        }
    }

    protected void Chkalterotherdept_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //if (Chkalterotherdept.Checked == true)
            //{
            //    Label4.Text = "Do you want to alter the staff in other department";
            //    div5.Visible = true;
            //}
            //else
            //    div5.Visible = false;
            //loadfsstaff();

        }
        catch
        {
        }
    }

    public void athorsubj()
    {
        try
        {
            div5.Visible = false;
            ddlbatchvalue = ((DropDownList)this.usercontrol1.FindControl("ddlBatch")).SelectedValue.ToString();
            ddlsecvalue = ((DropDownList)this.usercontrol1.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlcollegecode = ((DropDownList)this.usercontrol1.FindControl("ddlcollege")).SelectedValue.ToString();
            int parent_count = subjtree.Nodes.Count;//----------count parent node value
            for (int i = 0; i < parent_count; i++)
            {
                for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                {
                    if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                    {
                        string[] child_node = subjtree.Nodes[i].ChildNodes[node_count].Text.Split('-');
                        chile_index = child_node[1]; //subjtree.Nodes[i].ChildNodes[node_count].Text;
                        DataTable gdvheaders = new DataTable();
                        gdvheaders.Columns.Add("S.No");
                        gdvheaders.Columns.Add("Course");
                        DataRow dr = null;
                        dr = gdvheaders.NewRow();
                        dr[0] = "S.No";
                        dr[1] = "Course";
                        dr[1] = "subject_no";
                        dr[1] = "degree_code";


                        gdvheaders.Rows.Add(dr);

                        string subj = "select  c.Course_Name +'-'+ d.Acronym as course,d.degree_code,s.subject_name,subject_no from subject s,syllabus_master sm,Degree d,course c  where Batch_Year='" + ddlbatchvalue + "' and semester='" + ddlsecvalue + "' and subject_name='" + chile_index + "' and d.college_code='" + ddlcollegecode + "' and s.syll_code =sm.syll_code and d.Degree_Code=sm.degree_code and d.Course_Id=c.Course_Id and c.college_code=d.college_code";
                        DataSet ds = obi_access.select_method_wo_parameter(subj, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                            //{
                            panel2.Visible = true;
                            div3.Visible = true;
                            gvatte.DataSource = ds.Tables[0];
                            gvatte.DataBind();
                            gvatte.Visible = true;
                            div4.Visible = true;
                            div6.Visible = false;
                            Chkalterotherdept.Checked = true;
                            div5.Visible = false;
                            //}
                        }
                    }
                }
            }
            if (gvatte.Rows.Count == 1)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('This Subject doesn't belong to others')", true);
            }
        }
        catch
        {
        }
    }

    protected void gvatte_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    protected void Btnok_Click(object sender, EventArgs e)
    {
        try
        {
            subjno.Text = string.Empty;
            string sub = string.Empty;
            int sno = 0;
            int snos = 0;
            subjs.Clear();
            subjss.Clear();
            foreach (GridViewRow row in gvatte.Rows)
            {
                CheckBox stud_rollno = (CheckBox)row.FindControl("chk11");
                Label lblsubj = (Label)row.FindControl("lblsubj");
                Label lbldeg = (Label)row.FindControl("lbldeg");


                if (stud_rollno.Checked == true)
                {

                    if (subjno.Text == "")
                        sub = lblsubj.Text;
                    else
                        sub = sub + "," + lblsubj.Text;
                    subjno.Text = sub;

                    subjs.Add(sno, lbldeg.Text);
                    subjss.Add(snos, lblsubj.Text);
                    sno++;
                    snos++;
                }
            }
            div3.Visible = false;
            panel2.Visible = false;
            alterstaff();
            subjtree_SelectedNodeChanged(sender, e);
            //Save.Visible = false;
            //FindBtn.Visible = false;
            //Chkalterotherdept.Visible = false;
        }
        catch
        {
        }
    }

    protected void Btncancle_Click(object sender, EventArgs e)
    {
        try
        {
            div3.Visible = false;
            panel2.Visible = false;
            subjno.Text = string.Empty;
            Chkalterotherdept.Checked = false;

        }
        catch
        {
        }
    }

    protected void btnPopAlertClose1_Click(object sender, EventArgs e)
    {
        try
        {
            div3.Visible = true;
            panel2.Visible = true;
            athorsubj();
        }
        catch
        {
        }
    }

    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        try
        {
            div3.Visible = false;
            panel2.Visible = false;
            Chkalterotherdept.Checked = false;
            div5.Visible = false;
            //alterstaff();
        }
        catch
        {
        }
    }

    public void alterstaff()
    {
        ddlbatchvalue = ((DropDownList)this.usercontrol1.FindControl("ddlBatch")).SelectedValue.ToString();
        ddlsecvalue = ((DropDownList)this.usercontrol1.FindControl("ddlSec")).SelectedValue.ToString();
        for (int staffcount = 0; staffcount <= Convert.ToInt32(gview.Rows.Count) - 1; staffcount++)
        {
            subjtree.Visible = true;
            string strsec;
            if (ddlsecvalue.ToString() != "0" && ddlsecvalue.ToString() != "\0")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + ddlsecvalue.ToString() + "'";
            }
            int parent_count = subjtree.Nodes.Count;//----------count parent node value
            if (Chkalterotherdept.Checked == false)
            {

                for (int i = 0; i < parent_count; i++)
                {
                    for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                    {
                        if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                        {
                            if (ddlsecvalue.ToString() == "")
                            {
                                temp_sec = string.Empty;
                            }
                            else
                            {
                                temp_sec = " and Sections='" + ddlsecvalue.ToString() + "'";
                            }
                            chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;
                            string deletequery = "delete from staff_selector where subject_no=" + Convert.ToInt32(chile_index).ToString() + " and batch_year='" + ddlbatchvalue.ToString() + "' and sections='" + ddlsecvalue.ToString() + "'";
                            int d = obi_access.update_method_wo_parameter(deletequery, "Text");
                            break;
                        }
                    }
                }

                for (int stcolcount = 0; stcolcount <= Convert.ToInt32(gview.Rows.Count) - 1; stcolcount++)
                {

                    string stf_code = (gview.Rows[stcolcount].FindControl("lblcodee") as Label).Text;
                    string stf_name = (gview.Rows[stcolcount].FindControl("lblnamee") as Label).Text;

                    string insertcmd = "insert into staff_selector(subject_no,staff_code,batch_year,sections,dailyflag) values('" + Convert.ToInt32(chile_index).ToString() + "','" + stf_code.ToString() + "','" + ddlbatchvalue.ToString() + "','" + ddlsecvalue.ToString() + "',0)";
                    int n = obi_access.update_method_wo_parameter(insertcmd, "Text");
                    flagstaff = true;

                }
            }
            else
            {
                string typ = string.Empty;
                string ddlsemval = ((DropDownList)this.usercontrol1.FindControl("ddlSemYr")).SelectedValue.ToString();

                for (int i = 0; i < parent_count; i++)
                {
                    for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                    {
                        if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                        {
                            if (ddlsecvalue.ToString() == "")
                            {
                                typ = string.Empty;
                            }

                            chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;
                            string deletequery = "delete from staff_selector where subject_no in(" + Convert.ToString(subjno.Text) + ") and batch_year in('" + ddlbatchvalue.ToString() + "') ";
                            int d = obi_access.update_method_wo_parameter(deletequery, "Text"); i = parent_count; break;
                        }
                    }
                }
                if (subjs.Count > 0)
                {
                    for (int has = 0; has < subjs.Count; has++)
                    {
                        for (int stcolcount = 0; stcolcount <= Convert.ToInt32(gview.Rows.Count) - 1; stcolcount++)
                        {
                            string stf_code = (gview.Rows[stcolcount].FindControl("lblcodee") as Label).Text;
                            string stf_name = (gview.Rows[stcolcount].FindControl("lblnamee") as Label).Text;
                            CheckBox chkd = (gview.Rows[stcolcount].FindControl("selectchk") as CheckBox);
                            if (chkd.Checked)
                            {
                                string secs = "select distinct(Sections) as Sections,degree_code from registration where degree_code='" + Convert.ToString(subjs[has]) + "' and batch_year='" + Convert.ToString(ddlbatchvalue) + "' and Current_Semester='" + ddlsemval + "'  and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' ";
                                DataSet sect = obi_access.select_method_wo_parameter(secs, "text");
                                string insertcmd = string.Empty;
                                if (sect.Tables.Count > 0 && sect.Tables[0].Rows.Count > 0)
                                {
                                    for (int section = 0; section < sect.Tables[0].Rows.Count; section++)
                                    {
                                        insertcmd = "insert into staff_selector(subject_no,staff_code,batch_year,sections,dailyflag) values('" + Convert.ToString(subjss[has]) + "','" + stf_code.ToString() + "','" + ddlbatchvalue.ToString() + "','" + sect.Tables[0].Rows[section]["Sections"] + "',0)";
                                        int n = obi_access.update_method_wo_parameter(insertcmd, "Text");
                                    }
                                }
                                else
                                {
                                    insertcmd = "insert into staff_selector(subject_no,staff_code,batch_year,sections,dailyflag) values('" + Convert.ToString(subjss[has]) + "','" + stf_code.ToString() + "','" + ddlbatchvalue.ToString() + "','',0)";
                                    int n = obi_access.update_method_wo_parameter(insertcmd, "Text");
                                }
                                string hass = Convert.ToString(subjss[has]);

                                flagstaff = true;
                            }
                        }
                    }
                }
            }
        }
        if (flagstaff == true)
        {
            gview.Visible = true;
            FindBtn.Visible = true;
            Chkalterotherdept.Visible = true;
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('saved successfully')", true);
        }

        subjs.Clear();
        Chkalterotherdept.Checked = false;
    }

    protected void SelectAll_Checked(object sender, EventArgs e)
    {
        CheckBox chckheader = (CheckBox)gvatte.HeaderRow.FindControl("chkselectall");
        foreach (GridViewRow row in gvatte.Rows)
        {
            CheckBox chckrw = (CheckBox)row.FindControl("chk11");
            chckrw.Checked = chckheader.Checked;
            if (chckheader.Checked == true)
            {
                chckrw.Checked = true;
            }
            else
            {
                chckrw.Checked = false;
            }
        }
    }

    #region DELETE
    protected void btndelete_Click(object sender, EventArgs e)
    {
        int selectCount = 0;
        foreach (GridViewRow gvrow in gview.Rows)
        {
            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
            if (chk.Checked == true)
            {
                selectCount++;
            }
        }
        if (selectCount > 0)
        {

        }
        else
        {
            //imgdiv2.Visible = true;
            //lbl_alert.Text = "Select the row to delete";
        }
    }

    protected void btn_DeleteYes_Click(object sender, EventArgs e)
    {
        try
        {
            string college = Convert.ToString(ddlcollege.SelectedItem.Value);
            foreach (GridViewRow gvrow in gview.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                if (chk.Checked == true)
                {
                    string staffname = string.Empty;
                    string staffcode = string.Empty;

                    staffcode = Convert.ToString(gview.Rows[RowCnt].Cells[2].Text);
                    staffname = Convert.ToString(gview.Rows[RowCnt].Cells[3].Text);
                }
            }
        }
        catch
        {
        }
    }

    protected void btn_remove(object sender, EventArgs e)
    {
        try
        {
            ddlbatchvalue = ((DropDownList)this.usercontrol1.FindControl("ddlBatch")).SelectedValue.ToString();
            ddlsecvalue = ((DropDownList)this.usercontrol1.FindControl("ddlSec")).SelectedValue.ToString();

            subjtree.Visible = true;
            gview.Visible = true;
            string subjectNo = string.Empty;
            string staffCode = string.Empty;
            string staffname = string.Empty;
            int parent_count = subjtree.Nodes.Count;
            for (int i = 0; i < parent_count; i++)
            {
                for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                {
                    if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                    {
                        subjtree.Visible = true;
                        gview.Visible = true;
                        FindBtn.Visible = true;
                        Chkalterotherdept.Visible = true;
                        Save.Visible = true;
                        if (ddlsecvalue.ToString() == "")
                        {
                            temp_sec = string.Empty;
                        }
                        else
                        {
                            temp_sec = " and Sections='" + ddlsecvalue.ToString() + "'";
                        }
                        subjectNo = subjtree.Nodes[i].ChildNodes[node_count].Value;
                        i = parent_count;
                        break;
                    }
                }
            }

            Button selectstaf = (Button)sender;
            string rowindex = selectstaf.UniqueID.ToString().Split('$')[5].Replace("ctl", string.Empty);
            int actrow = Convert.ToInt32(rowindex) - 2;

            if (gview.Rows.Count > 0)
            {
                string cod = (gview.Rows[actrow].FindControl("lblcodee") as Label).Text;
                if (cod.Trim() != "" || cod != null || cod.Trim() != " ")
                {
                    staffCode = cod;
                }
            }
            if (!string.IsNullOrEmpty(subjectNo) && !string.IsNullOrEmpty(staffCode))
            {
                DataTable dtSubjectChooser = dirAcc.selectDataTable("select * from subjectChooser where subject_no='" + subjectNo + "' and staffcode like '%" + staffCode + "%'");
                if (dtSubjectChooser.Rows.Count == 0)
                {
                    string chkStaffSel = "select * from staff_selector where subject_no='" + subjectNo + "' and staff_code='" + staffCode + "' and sections='" + ddlsecvalue.ToString() + "'";
                    DataTable dtStaffsel = dirAcc.selectDataTable(chkStaffSel);
                    if (dtStaffsel.Rows.Count > 0)
                    {
                        string deletequery = "delete from staff_selector where subject_no='" + subjectNo + "' and staff_code='" + staffCode + "' and batch_year='" + ddlbatchvalue.ToString() + "' and sections='" + ddlsecvalue.ToString() + "'";
                        int d = obi_access.update_method_wo_parameter(deletequery, "Text");
                        if (d != 0)
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Removed successfully')", true);
                            subjtree_SelectedNodeChanged(sender, e);//day
                        }
                    }
                    else
                    {
                        Button grids = (Button)sender;
                        string rowIndxSs = grids.UniqueID.ToString().Split('$')[5].Replace("ctl", string.Empty);
                        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
                        gview.Rows[rowIndx].Visible = false;
                        DataTable dt = (DataTable)ViewState["dtadvisor"];
                        dt.Rows.RemoveAt(rowIndx);
                        ViewState["dtadvisor"] = dt;
                    }
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblSubNo.Text = subjectNo;
                    lblStaffCode.Text = staffCode;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = ("Do You Want Delete Subject Chooser");

                }
            }
        }
        catch
        {
        }
    }

    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        try
        {
            int okflag = 0;
            string classadvisor = string.Empty;
            DataRow dradvisor = null;
            for (int i = 0; i < gview.Rows.Count; i++)
            {
                string[] spiltcheck = classadvisor.Split(',');
                Boolean chevalflag = false;
                for (int ch = 0; ch <= spiltcheck.GetUpperBound(0); ch++)
                {
                    string st = (gview.Rows[i].FindControl("lblcodee") as Label).Text;
                    if (st == spiltcheck[ch].ToString())
                    {
                        chevalflag = true;
                    }
                }
                if (chevalflag == false)
                {
                    if (classadvisor == "")
                    {

                        classadvisor = (gview.Rows[i].FindControl("lblcodee") as Label).Text;
                    }
                    else
                    {

                        classadvisor = classadvisor + ',' + (gview.Rows[i].FindControl("lblcodee") as Label).Text;
                    }
                }
            }
            string Staffcode = string.Empty;

            DataTable dtadv = (DataTable)ViewState["dtadvisor"];

            if (dtadv.Columns.Count > 0)
            { }
            else
            {
                dtadv.Columns.Add("Staff_Code");
                dtadv.Columns.Add("Staff_Name");
            }

            for (int rolcount = 0; rolcount < gviewstaff.Rows.Count; rolcount++)
            {
                string[] spiltcheck = classadvisor.Split(',');
                Boolean chevalflag = false;
                for (int ch = 0; ch <= spiltcheck.GetUpperBound(0); ch++)
                {

                    string str = (gviewstaff.Rows[rolcount].FindControl("lblstaff") as Label).Text;
                    if (str == spiltcheck[ch].ToString())
                    {
                        chevalflag = true;
                    }
                }
                if (chevalflag == false)
                {
                    CheckBox chk = (CheckBox)gviewstaff.Rows[rolcount].FindControl("selectchk1");

                    if (chk.Checked)
                    {
                        okflag = 1;
                        dradvisor = dtadv.NewRow();
                        dradvisor["Staff_Name"] = (gviewstaff.Rows[rolcount].FindControl("lblname") as Label).Text;
                        dradvisor["Staff_Code"] = (gviewstaff.Rows[rolcount].FindControl("lblstaff") as Label).Text;

                        dtadv.Rows.Add(dradvisor);
                    }
                }
            }
            ViewState["dtadvisor"] = dtadv;
            gview.DataSource = dtadv;
            gview.DataBind();
            gview.Visible = true;
            panel3.Visible = false;
            gviewstaff.Visible = false;
            Save.Visible = true;
            if (gview.Rows.Count == 0)
            {
                Save.Visible = false;
            }

            #region command
            //int isval = 0;
            //int okflag = 0;
            ////Fpstaff.SaveChanges();
            //Save.Visible = true;
            //int count = 0;
            //dtable.Columns.Add("Staff_Code");
            //dtable.Columns.Add("Staff_Name");
            ////==========Sangeetha  0n 3 Sep 2014
            //// for removing empty column
            //for (int k = 0; k < gview.Rows.Count; k++)
            //{
            //    Label code = (Label)gview.Rows[k].FindControl("lblcodee");
            //    string stafcode = code.Text;
            //    Label name = (Label)gview.Rows[k].FindControl("lblnamee");
            //    string stafname = name.Text;


            //    dtrow = dtable.NewRow();
            //    dtrow["Staff_Code"] = stafcode;
            //    dtrow["Staff_Name"] = stafname;
            //    dtable.Rows.Add(dtrow);
            //    if (stafcode != "")
            //    {
            //        count++;
            //    }
            //}
            ////==========================================
            ////subjtree_SelectedNodeChanged(sender, e);



            //for (int fprow = 0; fprow <= Convert.ToInt32(gviewstaff.Rows.Count) - 1; fprow++)
            //{
            //    //isval = Convert.ToInt32(fsstaff.Sheets[0].Cells[fprow, 0].Value);
            //    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gviewstaff.Rows[fprow].Cells[1].FindControl("selectchk1");
            //    string v = string.Empty;
            //    //v = fsstaff.Sheets[0].GetText(fprow, 1);
            //    Label v1 = (Label)gviewstaff.Rows[fprow].FindControl("lblstaff");
            //    v = v1.Text;
            //    //if (isval == 1)
            //    if (chk.Checked)
            //    {
            //        //Fpstaff.Visible = true;
            //        gview.Visible = true;
            //        okflag = 1;
            //        int rc = Convert.ToInt32(gview.Rows.Count) - 1;
            //        dtrow = dtable.NewRow();
            //        Label code = (Label)gviewstaff.Rows[fprow].Cells[2].FindControl("lblstaff");
            //        Label name = (Label)gviewstaff.Rows[fprow].Cells[3].FindControl("lblname");
            //        dtrow["Staff_Code"] = code.Text;
            //        dtrow["Staff_Name"] = name.Text;
            //        dtable.Rows.Add(dtrow);
            //    }
            //    //Fpstaff.Sheets[0].PageSize = Fpstaff.Sheets[0].RowCount;
            //    //Fpstaff.SaveChanges();
            //}
            //gview.DataSource = dtable;
            //gview.DataBind();
            //if (okflag == 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Any Staff')", true);
            //}
            #endregion
        }
        catch
        {
            //throw ex;
        }
    }
    #endregion
}
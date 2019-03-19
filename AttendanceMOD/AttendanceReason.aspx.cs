using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class AttendanceReason : System.Web.UI.Page
{
    #region "loadDetails"
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strdegree = string.Empty;
    string strbranch = string.Empty;
    string strsem = string.Empty;
    string strquery = string.Empty;
    static string grouporusercode = "";

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Dictionary<string, int> rollvalue = new Dictionary<string, int>();
    Hashtable hat = new Hashtable();

    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null) //Aruna For Back Button
            {
                Response.Redirect("~/Default.aspx");
            }
            errmsg.Visible = false;
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            if (!IsPostBack)
            {
                txtfromdate.Attributes.Add("readonly", "readonly");
                txttodate.Attributes.Add("readonly", "readonly");
                Fpreport.Sheets[0].AutoPostBack = true;
                Fpreport.CommandBar.Visible = true;
                txtfromdate.Text = DateTime.Today.ToString("d/MM/yyyy");
                txttodate.Text = DateTime.Today.ToString("d/MM/yyyy");

                Session["daywise"] = "0";
                Session["hourwise"] = "0";
                Fpreport.Sheets[0].SheetName = " ";
                Fpreport.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                Fpreport.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
                Fpreport.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                Fpreport.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                Fpreport.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                Fpreport.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.Font.Name = "Book Antiqua";
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = Color.Black;
                style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Fpreport.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                Fpreport.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                Fpreport.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                Fpreport.Sheets[0].AllowTableCorner = true;
                Fpreport.Sheets[0].SheetCorner.ColumnCount = 0;

                //---------------page number

                Fpreport.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                Fpreport.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                Fpreport.Pager.Align = HorizontalAlign.Right;
                Fpreport.Pager.Font.Bold = true;
                Fpreport.Pager.Font.Name = "Book Antiqua";
                Fpreport.Pager.ForeColor = Color.DarkGreen;
                Fpreport.Pager.BackColor = Color.Beige;
                Fpreport.Pager.BackColor = Color.AliceBlue;
                Fpreport.Pager.PageCount = 5;
                Fpreport.CommandBar.Visible = false;
                Fpreport.Sheets[0].AutoPostBack = true;
                //---------------------------
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                string Master1 = "select * from Master_Settings where " + grouporusercode + "";

                DataSet dsmaseter = d2.select_method_wo_parameter(Master1, "Text");
                if (dsmaseter.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsmaseter.Tables[0].Rows.Count; i++)
                    {
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Register No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                    }
                }
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                if (ddldegree.Items.Count > 0)
                {
                    ddldegree.Enabled = true;
                    ddlbranch.Enabled = true;
                    ddlsemester.Enabled = true;
                    ddlsection.Enabled = true;
                    btngo.Enabled = true;
                    txtfromdate.Enabled = true;
                    txttodate.Enabled = true;
                    BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                    BindSem(strbranch, strbatch, collegecode);
                    BindSectionDetail(strbatch, strbranch);
                    loadreason();
                    //loadattendance();
                }
                else
                {
                    ddldegree.Enabled = false;
                    ddlbranch.Enabled = false;
                    ddlsemester.Enabled = false;
                    ddlsection.Enabled = false;
                    btngo.Enabled = false;
                }
                rbhour.Checked = true;
                visiblecontrol();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void visiblecontrol()
    {
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        Fpreport.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        errmsg.Visible = false;
    }

    //Load Batch Details...,
    public void BindBatch()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
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
            errmsg.Text = ex.ToString();
        }
    }

    // Load Degree Details...
    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            ddldegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();

            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    // Load Branch Details...
    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = ddldegree.SelectedValue.ToString();
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Semester Details-----

    public void BindSem(string strbranch, string strbatchyear, string collegecode)
    {

        try
        {
            strbatch = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsemester.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSem(strbranch, strbatch, collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    //------Load Function for the Section Details-----

    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsection.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSectionDetail(strbatch, strbranch);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsection.DataSource = ds;
                ddlsection.DataTextField = "sections";
                ddlsection.DataBind();
                ddlsection.Items.Insert(0, "All");
                if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsection.Enabled = false;
                }
                else
                {
                    ddlsection.Enabled = true;
                }
            }
            else
            {
                ddlsection.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //========= Laod Attendance Reason================
    public void loadreason()
    {
        ddlreason.Items.Clear();
        collegecode = Session["collegecode"].ToString();
        string query = "select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code=" + collegecode + "";
        ds.Dispose(); ds.Reset();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlreason.DataSource = ds;
            ddlreason.DataTextField = "Textval";
            ddlreason.DataValueField = "TextCode";
            ddlreason.DataBind();
        }
        ddlreason.Items.Insert(0, "All");
    }


    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDegree(singleuser, group_user, collegecode, usercode);
        BindBranch(singleuser, group_user, course_id, collegecode, usercode);
        BindSem(strbranch, strbatch, collegecode);
        BindSectionDetail(strbatch, strbranch);
        visiblecontrol();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBranch(singleuser, group_user, course_id, collegecode, usercode);
        BindSem(strbranch, strbatch, collegecode);
        BindSectionDetail(strbatch, strbranch);
        visiblecontrol();
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSem(strbranch, strbatch, collegecode);
        visiblecontrol();

    }
    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSectionDetail(strbatch, strbranch);
        visiblecontrol();
    }
    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        visiblecontrol();
    }
    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            visiblecontrol();
            if (txtfromdate.Text != "")
            {
                string[] spitfrom = txtfromdate.Text.Split('/');
                DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);

                string[] spilttodate = txttodate.Text.Split('/');
                DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
                if (dtto < dtfrom)
                {
                    txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    errmsg.Visible = true;
                    errmsg.Text = "To Date Must Be Greater Than From Date";
                }

            }
            else
            {
                txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

        }
        catch (Exception ex)
        {
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            errmsg.Visible = true;
            errmsg.Text = "Please Enter Valid From Date";
        }
    }
    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            visiblecontrol();
            if (txttodate.Text != "")
            {
                string[] spitfrom = txtfromdate.Text.Split('/');
                DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);

                string[] spilttodate = txttodate.Text.Split('/');
                DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
                if (dtto < dtfrom)
                {
                    txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    errmsg.Visible = true;
                    errmsg.Text = "To Date Must Be Greater Than From Date";
                }
            }
            else
            {
                txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

        }
        catch (Exception ex)
        {
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            errmsg.Visible = true;
            errmsg.Text = "Please Enter Valid From Date";
        }
    }
    #endregion

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            visiblecontrol();
            if (txtfromdate.Text.Trim() != "" && txttodate.Text.Trim() != "")
            {
                string[] spitfrom = txtfromdate.Text.Split('/');
                DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);

                string[] spilttodate = txttodate.Text.Split('/');
                DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
                if (dtfrom <= dtto)
                {
                    if (dtfrom == dtto)
                    {
                        string day = dtfrom.DayOfWeek.ToString();
                        if (day.Trim().ToLower() == "sunday")
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "Selected Day is Sunday";
                            return;
                        }
                    }
                    strbatch = ddlbatch.SelectedValue.ToString();
                    strdegree = ddlbranch.SelectedValue.ToString();
                    strsem = ddlsemester.SelectedItem.ToString();
                    if (rbhour.Checked == true)
                    {
                        loadhourreport();
                    }
                    else if (rbday.Checked == true)
                    {

                        loaddayreport();
                        int vis = 0;
                        for (int i = 0; i < Fpreport.Sheets[0].RowCount; i++)
                        {
                            if (Fpreport.Sheets[0].Rows[i].Visible == true)
                            {
                                vis++;
                            }
                        }
                        if (vis == 0)
                        {
                            Fpreport.Sheets[0].RowCount = 0;
                        }
                    }
                    if (Fpreport.Sheets[0].RowCount > 0 && Fpreport.Sheets[0].ColumnCount > 4)
                    {
                        btnprintmaster.Visible = true;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnxl.Visible = true;
                        Fpreport.Visible = true;
                        if (Session["Rollflag"].ToString() != "0")
                        {
                            Fpreport.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                        }
                        else
                        {
                            Fpreport.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                        }
                        if (Session["Regflag"].ToString() != "0")
                        {
                            Fpreport.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                        }
                        else
                        {
                            Fpreport.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                        }
                    }
                    else
                    {
                        visiblecontrol();
                        errmsg.Text = "No Records Found";
                        errmsg.Visible = true;
                    }
                    Fpreport.Sheets[0].PageSize = Fpreport.Sheets[0].RowCount;
                    //Fpreport.Sheets[0].AutoPostBack = false;
                    Fpreport.SaveChanges();
                }
                else
                {
                    errmsg.Text = "To Date Must be Greater than From Date";
                    errmsg.Visible = true;
                }

            }
        }
        catch (Exception ex)
        {
        }
    }
    public void loadhourreport()
    {

        try
        {
            Boolean valueflag = false;
            Fpreport.Sheets[0].RowCount = 0;
            Fpreport.Sheets[0].ColumnCount = 4;
            Fpreport.Sheets[0].ColumnHeader.RowCount = 3;
            Fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            Fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            Fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";

            Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
            Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
            Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
            Fpreport.Sheets[0].Columns[0].Width = 50;
            Fpreport.Sheets[0].Columns[1].Width = 100;
            Fpreport.Sheets[0].Columns[2].Width = 100;
            Fpreport.Sheets[0].Columns[3].Width = 100;

            Fpreport.Sheets[0].Columns[2].CellType = txt;


            FarPoint.Web.Spread.SheetView svsort = new FarPoint.Web.Spread.SheetView();
            svsort = Fpreport.ActiveSheetView;
            svsort.AllowSort = true;

            string strreason = "";

            if (ddlreason.Text != "All") ;
            {
                strreason = ddlreason.SelectedItem.ToString();
            }
            DataSet dsholi = new DataSet();
            int noofhours = 0;
            int fsthlfhours = 0;
            int scdhlfhours = 0;
            string sections = ddlsection.SelectedValue.ToString();
            string strsec = "";
            if (sections.ToString() != "All" && sections.ToString() != string.Empty && sections.ToString() != "-1")
            {
                strsec = "and sections='" + sections + "'";
            }
            string strorder = orderby();

            strquery = "SELECT No_of_hrs_per_day,no_of_hrs_II_half_day,no_of_hrs_I_half_day FROM PeriodAttndSchedule WHERE degree_code=" + strdegree + " and Semester=" + strsem + "";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["No_of_hrs_per_day"] != null && ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString() != "")
                {
                    noofhours = int.Parse(ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString());
                }
                if (ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"] != null && ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString() != "")
                {
                    fsthlfhours = int.Parse(ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
                }
                if (ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"] != null && ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString() != "")
                {
                    scdhlfhours = int.Parse(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                }
            }

            strquery = "Select r.roll_no,r.reg_no,r.stud_name,r.serialno from registration r where cc=0 and delflag=0 and exam_flag<>'debar' and batch_year=" + strbatch + " and degree_code=" + strdegree + " and current_semester=" + strsem + " " + strsec + " " + strorder + "";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            int srno = 0;

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    rollvalue.Add(ds.Tables[0].Rows[i]["roll_no"].ToString(), i);
                    Fpreport.Sheets[0].RowCount++;
                    srno++;
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["roll_no"].ToString();
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["reg_no"].ToString();
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["stud_name"].ToString();
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }


                string[] spitfrom = txtfromdate.Text.Split('/');
                DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);

                string[] spilttodate = txttodate.Text.Split('/');
                DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);

                for (DateTime curdate = dtfrom; curdate <= dtto; curdate.AddDays(1))
                {
                    string[] spitdate = curdate.ToString().Split(' ');
                    string[] spiltcurdate = spitdate[0].ToString().Split('/');
                    int monthyear = int.Parse(spiltcurdate[2]) * 12 + int.Parse(spiltcurdate[0]);
                    string strholidayquery = "select holiday_desc,halforfull,morning,evening from holidayStudents where degree_code=" + strdegree + " and semester=" + strsem + " and holiday_date='" + curdate.ToString() + "'";
                    dsholi = d2.select_method_wo_parameter(strholidayquery, "Text");
                    int startcol = Fpreport.Sheets[0].ColumnCount;
                    int starhour = 1;
                    int totconhours = noofhours;
                    string half_full = "";
                    string morning_h = "";
                    string evening_h = "";
                    int tothrsday = noofhours;
                    if (dsholi.Tables[0].Rows.Count > 0)
                    {
                        half_full = dsholi.Tables[0].Rows[0]["halforfull"].ToString();
                        morning_h = dsholi.Tables[0].Rows[0]["morning"].ToString();
                        evening_h = dsholi.Tables[0].Rows[0]["evening"].ToString();
                    }
                    if (half_full.ToLower().Trim() == "false")
                    {
                        starhour = noofhours;
                        totconhours = 0;
                    }
                    else if (half_full.ToLower().Trim() == "true" && morning_h.ToLower().Trim() == "true")
                    {
                        starhour = totconhours - fsthlfhours;
                        starhour = starhour + 1;
                        tothrsday = scdhlfhours;
                    }
                    else if (half_full.ToLower().Trim() == "true" && evening_h.ToLower().Trim() == "true")
                    {
                        tothrsday = fsthlfhours;
                        totconhours = totconhours - scdhlfhours;
                    }
                    if (half_full == "" || half_full.ToLower().Trim() == "true")
                    {
                        string day = "";
                        string searreason = "";
                        for (int hr = starhour; hr <= totconhours; hr++)
                        {
                            Fpreport.Sheets[0].ColumnCount = Fpreport.Sheets[0].ColumnCount + 2;
                            Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(1, Fpreport.Sheets[0].ColumnCount - 2, 1, 2);
                            Fpreport.Sheets[0].ColumnHeader.Cells[1, Fpreport.Sheets[0].ColumnCount - 2].Text = "Period " + hr.ToString();
                            Fpreport.Sheets[0].ColumnHeader.Cells[2, Fpreport.Sheets[0].ColumnCount - 2].Text = "Attendance";
                            Fpreport.Sheets[0].ColumnHeader.Cells[2, Fpreport.Sheets[0].ColumnCount - 1].Text = "Reason";
                            if (day == "")
                            {
                                day = "d" + spiltcurdate[1].ToString() + "d" + hr;
                                if (strreason.ToLower().Trim() != "all")
                                {
                                    searreason = "and( " + day + " like '%" + strreason + "%'";
                                }
                            }
                            else
                            {
                                day = "" + day + ",d" + spiltcurdate[1].ToString() + "d" + hr;
                                if (strreason.ToLower().Trim() != "all")
                                {
                                    searreason = searreason + "or d" + spiltcurdate[1].ToString() + "d" + hr + " like '%" + strreason + "%'";
                                }
                            }
                        }

                        if (searreason.ToLower().Trim() != "")
                        {
                            searreason = searreason + ")";
                        }

                        //====================For set Reason================================
                        strquery = "select " + day + ",r.roll_no from  attendance_withreason a,Registration r where a.roll_no=r.Roll_No and a.month_year=" + monthyear + " and batch_year=" + strbatch + " and degree_code=" + strdegree + " and current_semester=" + strsem + " " + strsec + " " + searreason + "";
                        ds.Reset();
                        ds.Dispose();
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (rollvalue.ContainsKey(ds.Tables[0].Rows[i]["Roll_no"].ToString()))
                            {
                                int ro = rollvalue[ds.Tables[0].Rows[i]["Roll_no"].ToString()];
                                int setcol = startcol;
                                for (int hr = starhour; hr <= totconhours; hr++)
                                {
                                    setcol = setcol + 2;
                                    string getcol = "d" + spiltcurdate[1].ToString() + "d" + hr;
                                    string[] spitreason = ds.Tables[0].Rows[i]["" + getcol + ""].ToString().Split(';');
                                    string reason = "";
                                    if (spitreason.GetUpperBound(0) > 0)
                                    {
                                        reason = spitreason[1].ToString();
                                    }
                                    else
                                    {
                                        reason = spitreason[0].ToString();
                                    }
                                    Fpreport.Sheets[0].Cells[ro, setcol - 1].Text = reason;
                                    valueflag = true;
                                }
                            }
                        }
                        //====================For set Attendance================================
                        strquery = "select " + day + ",r.roll_no from  attendance a,Registration r where a.roll_no=r.Roll_No and a.month_year=" + monthyear + " and batch_year=" + strbatch + " and degree_code=" + strdegree + " and current_semester=" + strsem + " " + strsec + "";
                        ds.Reset();
                        ds.Dispose();
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (rollvalue.ContainsKey(ds.Tables[0].Rows[i]["Roll_no"].ToString()))
                            {
                                int ro = rollvalue[ds.Tables[0].Rows[i]["Roll_no"].ToString()];
                                int setcol = startcol;
                                for (int hr = starhour; hr <= totconhours; hr++)
                                {
                                    setcol = setcol + 2;
                                    string getcol = "d" + spiltcurdate[1].ToString() + "d" + hr;
                                    string attval = ds.Tables[0].Rows[i]["" + getcol + ""].ToString();
                                    attval = Attmark(attval);
                                    Fpreport.Sheets[0].Cells[ro, setcol - 2].Text = attval;
                                }
                                ro = Fpreport.Sheets[0].RowCount;
                            }
                        }

                        Fpreport.Sheets[0].ColumnHeader.Cells[0, startcol].Text = "" + spiltcurdate[1].ToString() + "/" + spiltcurdate[0].ToString() + "/" + spiltcurdate[2].ToString() + "";
                        Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(0, startcol, 1, tothrsday * 2);
                    }
                    curdate = curdate.AddDays(1);
                }
                if (valueflag == true)
                {
                    Fpreport.Visible = true;
                    btnprintmaster.Visible = true;

                }
            }
            else
            {
                errmsg.Text = "No Records Found";
                errmsg.Visible = true;
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    public void loaddayreport()
    {
        try
        {
            Boolean valueflag = false;
            Fpreport.Sheets[0].RowCount = 0;
            Fpreport.Sheets[0].ColumnCount = 4;
            Fpreport.Sheets[0].ColumnHeader.RowCount = 2;
            Fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            Fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            Fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";

            Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            Fpreport.Sheets[0].Columns[0].Width = 50;
            Fpreport.Sheets[0].Columns[1].Width = 100;
            Fpreport.Sheets[0].Columns[2].Width = 100;
            Fpreport.Sheets[0].Columns[3].Width = 100;
            FarPoint.Web.Spread.SheetView svsort = new FarPoint.Web.Spread.SheetView();
            svsort = Fpreport.ActiveSheetView;
            svsort.AllowSort = true;

            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            DataSet ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
            int count = ds1.Tables[0].Rows.Count;
            Dictionary<string, int> rollvalue = new Dictionary<string, int>();
            string strreason = "";

            if (ddlreason.Text != "All") ;
            {
                strreason = ddlreason.SelectedItem.ToString();
            }
            DataSet dsholi = new DataSet();
            int noofhours = 0;
            int fsthlfhours = 0;
            int scdhlfhours = 0;
            int minpreday = 0;
            int minpreIhlf = 0;
            int minpreIIhlf = 0;
            int ObtValue = -1;
            string tempvalue = string.Empty;
            string sections = ddlsection.SelectedValue.ToString();
            string strsec = "";

            if (sections.ToString().ToLower().Trim() != "all" && sections.ToString() != string.Empty && sections.ToString() != "-1")
            {
                strsec = "and sections='" + sections + "'";
            }
            string strorder = orderby();

            strquery = "SELECT No_of_hrs_per_day,no_of_hrs_II_half_day,no_of_hrs_I_half_day,min_hrs_per_day,min_pres_I_half_day,min_pres_II_half_day FROM PeriodAttndSchedule WHERE degree_code=" + strdegree + " and Semester=" + strsem + "";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["No_of_hrs_per_day"] != null && ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString() != "")
                {
                    noofhours = int.Parse(ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString());
                }
                if (ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"] != null && ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString() != "")
                {
                    fsthlfhours = int.Parse(ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
                }
                if (ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"] != null && ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString() != "")
                {
                    scdhlfhours = int.Parse(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                }
                if (ds.Tables[0].Rows[0]["min_hrs_per_day"] != null && ds.Tables[0].Rows[0]["min_hrs_per_day"].ToString() != "")
                {
                    minpreday = int.Parse(ds.Tables[0].Rows[0]["min_hrs_per_day"].ToString());
                }
                if (ds.Tables[0].Rows[0]["min_pres_I_half_day"] != null && ds.Tables[0].Rows[0]["min_pres_I_half_day"].ToString() != "")
                {
                    minpreIhlf = int.Parse(ds.Tables[0].Rows[0]["min_pres_I_half_day"].ToString());
                }
                if (ds.Tables[0].Rows[0]["min_pres_II_half_day"] != null && ds.Tables[0].Rows[0]["min_pres_II_half_day"].ToString() != "")
                {
                    minpreIIhlf = int.Parse(ds.Tables[0].Rows[0]["min_pres_II_half_day"].ToString());
                }
            }

            strquery = "Select r.roll_no,r.reg_no,r.stud_name,r.serialno from registration r where cc=0 and delflag=0 and exam_flag<>'debar' and batch_year=" + strbatch + " and degree_code=" + strdegree + " and current_semester=" + strsem + " " + strsec + " " + strorder + "";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            int srno = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    rollvalue.Add(ds.Tables[0].Rows[i]["roll_no"].ToString(), i);
                    Fpreport.Sheets[0].RowCount++;
                    srno++;
                    Fpreport.Sheets[0].Rows[Fpreport.Sheets[0].RowCount - 1].Visible = false;
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["roll_no"].ToString();
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["reg_no"].ToString();
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["stud_name"].ToString();
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpreport.Sheets[0].Cells[Fpreport.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                }
                Fpreport.Sheets[0].Columns[2].CellType = txt;
                string[] spitfrom = txtfromdate.Text.Split('/');
                DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);

                string[] spilttodate = txttodate.Text.Split('/');
                DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);

                for (DateTime curdate = dtfrom; curdate <= dtto; curdate.AddDays(1))
                {
                    string[] spitdate = curdate.ToString().Split(' ');
                    string[] spiltcurdate = spitdate[0].ToString().Split('/');
                    int monthyear = int.Parse(spiltcurdate[2]) * 12 + int.Parse(spiltcurdate[0]);
                    string strholidayquery = "select holiday_desc,halforfull,morning,evening from holidayStudents where degree_code=" + strdegree + " and semester=" + strsem + " and holiday_date='" + curdate.ToString() + "'";
                    dsholi = d2.select_method_wo_parameter(strholidayquery, "Text");
                    int startcol = Fpreport.Sheets[0].ColumnCount;
                    int starhour = 1;
                    int totconhours = noofhours;
                    string half_full = "";
                    string morning_h = "";
                    string evening_h = "";
                    int minatt = minpreday;
                    int mincon = noofhours;
                    if (dsholi.Tables[0].Rows.Count > 0)
                    {
                        half_full = dsholi.Tables[0].Rows[0]["halforfull"].ToString();
                        morning_h = dsholi.Tables[0].Rows[0]["morning"].ToString();
                        evening_h = dsholi.Tables[0].Rows[0]["evening"].ToString();

                    }
                    if (half_full.ToLower().Trim() == "false")
                    {
                        starhour = noofhours;
                        totconhours = 0;
                    }
                    else if (half_full.ToLower().Trim() == "true" && morning_h.ToLower().Trim() == "true")
                    {
                        starhour = totconhours - fsthlfhours;
                        starhour = starhour + 1;
                        minatt = minpreIhlf;
                        mincon = scdhlfhours;
                    }
                    else if (half_full.ToLower().Trim() == "true" && evening_h.ToLower().Trim() == "true")
                    {
                        totconhours = totconhours - scdhlfhours;
                        minatt = minpreIIhlf;
                        mincon = fsthlfhours;
                    }
                    if (half_full == "" || half_full.ToLower().Trim() == "true")
                    {
                        Fpreport.Sheets[0].ColumnCount = Fpreport.Sheets[0].ColumnCount + 2;
                        Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpreport.Sheets[0].ColumnCount - 2, 1, 2);
                        Fpreport.Sheets[0].ColumnHeader.Cells[0, startcol].Text = "" + spiltcurdate[1].ToString() + "/" + spiltcurdate[0].ToString() + "/" + spiltcurdate[2].ToString() + "";
                        Fpreport.Sheets[0].ColumnHeaderSpanModel.Add(0, startcol, 1, 2);
                        Fpreport.Sheets[0].ColumnHeader.Cells[1, Fpreport.Sheets[0].ColumnCount - 2].Text = "Attendance";
                        Fpreport.Sheets[0].ColumnHeader.Cells[1, Fpreport.Sheets[0].ColumnCount - 1].Text = "Reason";
                        string day = "";
                        string searreason = "";
                        for (int hr = starhour; hr <= totconhours; hr++)
                        {
                            if (day == "")
                            {
                                day = "d" + spiltcurdate[1].ToString() + "d" + hr;
                                if (strreason.ToLower().Trim() != "all")
                                {
                                    searreason = "and( " + day + " like '%" + strreason + "%'";
                                }
                            }
                            else
                            {
                                day = "" + day + ",d" + spiltcurdate[1].ToString() + "d" + hr;
                                if (strreason.ToLower().Trim() != "all")
                                {
                                    searreason = searreason + " and d" + spiltcurdate[1].ToString() + "d" + hr + " like '%" + strreason + "%'";
                                }
                            }
                        }
                        if (searreason.Trim() != "")
                        {
                            searreason = searreason + ")";
                        }
                        int prehrs = 0, abshrs = 0, njhr = 0;
                        //====================For set Attendance================================
                        strquery = "select " + day + ",r.roll_no from  attendance a,Registration r where a.roll_no=r.Roll_No and a.month_year=" + monthyear + " and batch_year=" + strbatch + " and degree_code=" + strdegree + " and current_semester=" + strsem + " " + strsec + "";
                        ds.Reset();
                        ds.Dispose();
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (rollvalue.ContainsKey(ds.Tables[0].Rows[i]["roll_no"].ToString()))
                            {
                                int ro = rollvalue[ds.Tables[0].Rows[i]["roll_no"].ToString()];

                                int setcol = startcol;
                                prehrs = 0;
                                abshrs = 0;
                                njhr = 0;
                                int conhrs = 0;
                                //============== For fullday=============
                                tempvalue = "";
                                for (int hr = starhour; hr <= totconhours; hr++)
                                {
                                    setcol = setcol + 2;
                                    string getcol = "d" + spiltcurdate[1].ToString() + "d" + hr;
                                    string attval = ds.Tables[0].Rows[i]["" + getcol + ""].ToString();
                                    if (attval != null && attval != "0" && attval != "7" && attval != "")
                                    {
                                        conhrs++;
                                        if (tempvalue != attval)
                                        {
                                            tempvalue = attval;
                                            for (int j = 0; j < count; j++)
                                            {

                                                if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == attval.ToString())
                                                {
                                                    ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                    j = count;
                                                }
                                            }
                                        }
                                        if (ObtValue == 1)
                                        {
                                            abshrs += 1;
                                        }
                                        else if (ObtValue == 2)
                                        {
                                            njhr += 1;
                                        }
                                        else if (ObtValue == 0)
                                        {
                                            prehrs += 1;
                                        }
                                    }
                                }
                                if (mincon <= conhrs)
                                {
                                    if (minatt <= prehrs)
                                    {
                                        Fpreport.Sheets[0].Cells[ro, startcol].Text = "P";
                                    }
                                    else
                                    {
                                        Fpreport.Sheets[0].Cells[ro, startcol].Text = "A";
                                    }
                                }
                                else
                                {
                                    Fpreport.Sheets[0].Cells[ro, startcol].Text = "";
                                }
                            }
                        }
                        //====================For set Reason================================
                        srno = 0;
                        strquery = "select " + day + ",r.roll_no from  attendance_withreason a,Registration r where a.roll_no=r.Roll_No and a.month_year=" + monthyear + " and batch_year=" + strbatch + " and degree_code=" + strdegree + " and current_semester=" + strsem + " " + strsec + " " + searreason + "";
                        ds.Reset();
                        ds.Dispose();
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (rollvalue.ContainsKey(ds.Tables[0].Rows[i]["Roll_no"].ToString()))
                            {
                                int ro = rollvalue[ds.Tables[0].Rows[i]["Roll_no"].ToString()];
                                string tempreason = "";
                                int reacount = 0;
                                for (int hr = starhour; hr <= totconhours; hr++)
                                {
                                    string getcol = "d" + spiltcurdate[1].ToString() + "d" + hr;
                                    string[] spitreason = ds.Tables[0].Rows[i]["" + getcol + ""].ToString().Split(';');
                                    string reason = "";
                                    if (spitreason.GetUpperBound(0) > 0)
                                    {
                                        reason = spitreason[1].ToString();
                                    }
                                    else
                                    {
                                        reason = spitreason[0].ToString();
                                    }
                                    if (reason.Trim() != "")
                                    {
                                        if (hr == starhour)
                                        {
                                            tempreason = reason;
                                            reacount++;
                                        }
                                        else if (tempreason == reason)
                                        {
                                            reacount++;
                                        }
                                        else
                                        {
                                            tempreason = reason;
                                        }
                                    }
                                }
                                if (reacount >= totconhours)
                                {
                                    srno++;
                                    Fpreport.Sheets[0].Cells[ro, 0].Text = srno.ToString();
                                    Fpreport.Sheets[0].Cells[ro, startcol + 1].Text = tempreason;
                                    Fpreport.Sheets[0].Rows[ro].Visible = true;
                                }
                                valueflag = true;
                                ro = Fpreport.Sheets[0].RowCount;
                            }
                        }
                    }
                    curdate = curdate.AddDays(1);
                }
            }
            else
            {
                errmsg.Text = "No Records Found";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string sections = ddlsection.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = "";
            Session["Sign"] = "" + ddlbatch.SelectedItem.ToString() + "," + ddlbranch.SelectedValue.ToString() + "," + ddlsemester.SelectedItem.ToString() + "";
        }
        else
        {
            Session["Sign"] = "" + ddlbatch.SelectedItem.ToString() + "," + ddlbranch.SelectedValue.ToString() + "," + ddlsemester.SelectedItem.ToString() + "," + sections + "";
            sections = "- Sec-" + sections;
        }

        string degreedetails = "Attendance Reason Report" + '@' + "Degree :" + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlsemester.SelectedItem.ToString() + sections + '@' + "Date :" + txtfromdate.Text.ToString() + " To " + txttodate.Text.ToString();
        string pagename = "AttendanceReason.aspx";
        Printcontrol.loadspreaddetails(Fpreport, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpreport, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    public string orderby()
    {
        string strorder = "ORDER BY r.Roll_No";
        string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
        if (serialno.Trim() == "1")
        {
            strorder = "ORDER BY r.serialno";
        }
        else
        {
            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY r.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY r.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY r.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY r.Reg_No,r.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Stud_Name";
            }
        }
        return strorder;
    }

    public string Attmark(string Attstr_mark)
    {
        string Att_mark = "";

        if (Attstr_mark == "1")
        {
            Att_mark = "P";

        }

        else if (Attstr_mark == "2")
        {
            Att_mark = "A";

        }
        else if (Attstr_mark == "3")
        {
            Att_mark = "OD";

        }
        else if (Attstr_mark == "4")
        {
            Att_mark = "ML";

        }
        else if (Attstr_mark == "5")
        {
            Att_mark = "SOD";

        }
        else if (Attstr_mark == "6")
        {
            Att_mark = "NSS";

        }
        else if (Attstr_mark == "7")
        {
            Att_mark = "H";

        }
        else if (Attstr_mark == "8")
        {
            Att_mark = "NJ";

        }
        else if (Attstr_mark == "9")
        {
            Att_mark = "S";

        }
        else if (Attstr_mark == "10")
        {
            Att_mark = "L";

        }
        else if (Attstr_mark == "11")
        {
            Att_mark = "NCC";

        }
        else if (Attstr_mark == "12")
        {
            Att_mark = "HS";

        }
        else if (Attstr_mark == "13")
        {
            Att_mark = "PP";
        }
        else if (Attstr_mark == "14")
        {
            Att_mark = "SYOD";
        }
        else if (Attstr_mark == "15")
        {
            Att_mark = "COD";
        }
        else if (Attstr_mark == "16")
        {
            Att_mark = "OOD";
        }
        else if (Attstr_mark == "17")
        {
            Att_mark = "LA";
        }
        else
        {
            Att_mark = "NE";
        }
        return Att_mark;
    }

}
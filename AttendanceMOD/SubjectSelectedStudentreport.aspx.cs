using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;



public partial class SubjectSelectedSTudentreport : System.Web.UI.Page
{

    #region "Load Details"
    DataSet ds = new DataSet();
    DAccess2 dacc = new DAccess2();
    string group_user = "", singleuser = "", usercode = "", collegecode = "";
    static string grouporusercode = "";
    Hashtable has = new Hashtable();
    Boolean cellclick = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        errmsg.Visible = false;
        lblnorec.Visible = false;
        if (!Page.IsPostBack)
        {
            rdb_subjectwise.Checked = true;
            chkelective.Checked = false;
            chkelective.Visible = false;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            ddlsubject.Width = 100;
            subject_spread.Sheets[0].SheetName = " ";
            subject_spread.Sheets[0].AutoPostBack = true;
            subject_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            subject_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            subject_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            subject_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
            subject_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            subject_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
            subject_spread.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
            subject_spread.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
            subject_spread.ActiveSheetView.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            subject_spread.Sheets[0].PageSize = 10;
            subject_spread.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            subject_spread.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            subject_spread.Pager.Align = HorizontalAlign.Right;
            subject_spread.Pager.Font.Bold = true;
            subject_spread.Pager.Font.Name = "Book Antiqua";
            subject_spread.Pager.ForeColor = Color.DarkGreen;
            subject_spread.Pager.BackColor = Color.Beige;
            subject_spread.Pager.BackColor = Color.AliceBlue;
            subject_spread.Pager.PageCount = 5;
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.HorizontalAlign = HorizontalAlign.Center;
            style.ForeColor = Color.Black;
            style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            subject_spread.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            subject_spread.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            subject_spread.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            subject_spread.Sheets[0].AllowTableCorner = true;
            subject_spread.Sheets[0].SheetCorner.ColumnCount = 0;


            Fpstucount.Sheets[0].SheetName = " ";
            Fpstucount.Sheets[0].AutoPostBack = true;
            Fpstucount.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            Fpstucount.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            Fpstucount.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            Fpstucount.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
            Fpstucount.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            Fpstucount.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
            Fpstucount.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
            Fpstucount.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
            Fpstucount.ActiveSheetView.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            Fpstucount.Sheets[0].PageSize = 10;
            Fpstucount.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            Fpstucount.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            Fpstucount.Pager.Align = HorizontalAlign.Right;
            Fpstucount.Pager.Font.Bold = true;
            Fpstucount.Pager.Font.Name = "Book Antiqua";
            Fpstucount.Pager.ForeColor = Color.DarkGreen;
            Fpstucount.Pager.BackColor = Color.Beige;
            Fpstucount.Pager.BackColor = Color.AliceBlue;
            Fpstucount.Pager.PageCount = 5;
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.HorizontalAlign = HorizontalAlign.Center;
            style.ForeColor = Color.Black;
            style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpstucount.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            Fpstucount.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            Fpstucount.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            Fpstucount.Sheets[0].AllowTableCorner = true;
            Fpstucount.Sheets[0].SheetCorner.ColumnCount = 0;
            psubject.Visible = false;
            txtsubject.Visible = false;
            clear();
            bindbatch();
            binddegree();
            if (ddldegree.Items.Count > 0)
            {
                ddlbatch.Enabled = true;
                ddldegree.Enabled = true;
                ddlbranch.Enabled = true;
                ddlduration.Enabled = true;
                btnGo.Enabled = true;
                bindbranch();
                bindsem();
                loadsec();
                load_subject();
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                string Master = "select * from Master_Settings where " + grouporusercode + "";

                ds.Dispose();
                ds.Reset();
                ds = dacc.select_method_wo_parameter(Master, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                }

            }
            else
            {
                ddlbatch.Enabled = false;
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
                ddlduration.Enabled = false;
                btnGo.Enabled = false;
            }
            rbname.Checked = true;
            rbname.Visible = false;
            rbacr.Visible = false;
            chkattfile.Visible = false;
        }

    }

    public void bindbatch()
    {
        ddlbatch.Items.Clear();
        ds = dacc.select_method_wo_parameter("bind_batch", "sp");
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlbatch.DataSource = ds;
            ddlbatch.DataTextField = "batch_year";
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();
        }
        int count1 = ds.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            ddlbatch.SelectedValue = max_bat.ToString();
        }
    }
    public void binddegree()
    {
        ddldegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        has.Clear();
        has.Add("single_user", singleuser);
        has.Add("group_code", group_user);
        has.Add("college_code", collegecode);
        has.Add("user_code", usercode);
        ds = dacc.select_method("bind_degree", has, "sp");
        int count1 = ds.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddldegree.DataSource = ds;
            ddldegree.DataTextField = "course_name";
            ddldegree.DataValueField = "course_id";
            ddldegree.DataBind();
        }
    }
    public void bindbranch()
    {

        ddlbranch.Items.Clear();
        has.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        has.Add("single_user", singleuser);
        has.Add("group_code", group_user);
        has.Add("course_id", ddldegree.SelectedValue);
        has.Add("college_code", collegecode);
        has.Add("user_code", usercode);

        ds = dacc.select_method("bind_branch", has, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }

    }
    public void bindsem()
    {
        ddlduration.Items.Clear();
        string duration = "";
        Boolean first_year = false;
        has.Clear();
        collegecode = Session["collegecode"].ToString();
        has.Add("degree_code", ddlbranch.SelectedValue.ToString());
        has.Add("batch_year", ddlbatch.SelectedValue.ToString());
        has.Add("college_code", collegecode);
        ds = dacc.select_method("bind_sem", has, "sp");
        int count3 = ds.Tables[0].Rows.Count;
        if (count3 > 0)
        {
            ddlduration.Enabled = true;
            duration = ds.Tables[0].Rows[0][0].ToString();
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
            {
                if (first_year == false)
                {
                    ddlduration.Items.Add(loop_val.ToString());
                }
                else if (first_year == true && loop_val != 2)
                {
                    ddlduration.Items.Add(loop_val.ToString());
                }

            }
        }
        else
        {
            count3 = ds.Tables[1].Rows.Count;
            if (count3 > 0)
            {
                ddlduration.Enabled = true;
                duration = ds.Tables[1].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlduration.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlduration.Items.Add(loop_val.ToString());
                    }

                }
            }
            else
            {
                ddlduration.Enabled = false;
            }
        }

    }

    public void loadsec()
    {
        try
        {
            ddlsec.Items.Clear();
            has.Clear();
            has.Add("batch_year", ddlbatch.SelectedValue.ToString());
            has.Add("degree_code", ddlbranch.SelectedValue);
            ds = dacc.select_method("bind_sec", has, "sp");
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataValueField = "sections";
                ddlsec.DataBind();
                ddlsec.Items.Insert(0, "All");
                ddlsec.Enabled = true;
            }
            else
            {
                ddlsec.Enabled = false;
            }
        }
        catch
        {
        }
    }

    public void load_subject()
    {

        clear();
        int count_subject = 0;

        string batchyear = ddlbatch.SelectedValue.ToString();
        string degreecode = ddlbranch.SelectedValue.ToString();
        string sem = ddlduration.SelectedItem.ToString();

        ds.Dispose();
        ds.Reset();
        string syllabusyear = dacc.GetFunction("select syllabus_year from syllabus_master where degree_code=" + degreecode + " and semester =" + sem + " and batch_year=" + batchyear + "");
        if (syllabusyear != null && syllabusyear.Trim() != "0" && syllabusyear.Trim() != "-1")
        {
            string strquery = "select distinct subject.subtype_no,subject_type from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + degreecode + " and semester=" + sem + " and syllabus_year = " + syllabusyear + " and batch_year = " + batchyear + ") order by subject.subtype_no";
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsubject.Enabled = true;
                ddlsubject.DataSource = ds;
                ddlsubject.DataTextField = "subject_type";
                ddlsubject.DataValueField = "subtype_no";
                ddlsubject.DataBind();
                ddlsubject.Items.Insert(0, "All");

            }
            else
            {
                ddlsubject.Enabled = false;
            }
        }
        else
        {
            ddlsubject.Enabled = false;
        }
    }
    public void loadallsubject()
    {
        string strquery = "select distinct subject_name+'-'+subject_code as subject,subject_name from subject s,subjectChooser sc,Registration r where r.Roll_No=sc.roll_no and r.Current_Semester=sc.semester and s.subject_no=sc.subject_no and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' order by subject_name";
        if (chkelective.Checked == true)
        {
            strquery = "select distinct subject_name+'-'+subject_code as subject,subject_name from subject s,subjectChooser sc,Registration r,sub_sem ss where r.Roll_No=sc.roll_no and s.subtype_no=ss.subtype_no and r.Current_Semester=sc.semester and s.subject_no=sc.subject_no and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and ss.electivepap=1 order by subject_name";
        }
        ds = dacc.select_method_wo_parameter(strquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlsubject.Enabled = true;
            ddlsubject.DataSource = ds;
            ddlsubject.DataTextField = "subject";
            ddlsubject.DataValueField = "subject";
            ddlsubject.DataBind();
        }
        else
        {
            ddlsubject.Enabled = false;
        }

    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {

        clear();
        loadsec();

        if (ddlreport.SelectedItem.ToString() == "Missing Student")
        {
            loadelesubject();
        }
        else
        {
            load_subject();
        }
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        bindbranch();
        bindsem();
        loadsec();

        if (ddlreport.SelectedItem.ToString() == "Missing Student")
        {
            loadelesubject();
        }
        else
        {
            load_subject();
        }
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        bindsem();
        loadsec();

        if (ddlreport.SelectedItem.ToString() == "Missing Student")
        {
            loadelesubject();
        }
        else
        {
            load_subject();
        }
    }
    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadsec();

        if (ddlreport.SelectedItem.ToString() == "Missing Student")
        {
            loadelesubject();
        }
        else
        {
            load_subject();
        }
    }
    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();

        if (ddlreport.SelectedItem.ToString() == "Missing Student")
        {
            loadelesubject();
        }
        else
        {
            load_subject();
        }
    }
    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chkelective.Checked = false;
            chkelective.Visible = false;
            clear();
            chkover.Checked = false;
            psubject.Visible = false;
            txtsubject.Visible = false;
            lblsubject.Width = 55;
            if (ddlreport.SelectedItem.ToString() == "Degree Wise")
            {
                rdb_subjectwise.Checked = true;
                rdb_subjecttypewise.Checked = false;
                ddlbatch.Visible = true;
                ddldegree.Visible = true;
                ddlbranch.Visible = true;
                ddlduration.Visible = true;
                lblbatch.Visible = true;
                lbldegree.Visible = true;
                lblbranch.Visible = true;
                lblduration.Visible = true;
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                loadsec();
                load_subject();
                ddlsubject.Width = 100;
                chkover.Visible = true;
                ddlsubject.Visible = true;
                lblsubject.Visible = true;
                ddlsec.Visible = true;
                lblsec.Visible = true;
                psubject.Visible = false;
                txtsubject.Visible = false;
            }
            else if (ddlreport.SelectedItem.ToString() == "Missing Student")
            {
                //btnGo.Attributes.Add("Style='top: 234px; left: 380px; position: absolute;'", "Style='top: 234px; left: 380px; position: absolute;'"); 
                rdb_subjectwise.Checked = true;
                rdb_subjecttypewise.Checked = false;
                ddlbatch.Visible = true;
                ddldegree.Visible = true;
                ddlbranch.Visible = true;
                ddlduration.Visible = true;
                lblbatch.Visible = true;
                lbldegree.Visible = true;
                lblbranch.Visible = true;
                lblduration.Visible = true;
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                loadsec();
                loadelesubject();
                chkover.Visible = false;
                ddlsubject.Visible = false;
                ddlsec.Visible = true;
                lblsec.Visible = true;
                lblsubject.Visible = true;
                psubject.Visible = true;
                txtsubject.Visible = true;
                lblsubject.Visible = true;
                lblsubject.Width = 160;
            }
            else if (ddlreport.SelectedItem.ToString() == "With staff")
            {
                rdb_subjectwise.Checked = true;
                rdb_subjecttypewise.Checked = false;
                ddlbatch.Visible = true;
                ddldegree.Visible = true;
                ddlbranch.Visible = true;
                ddlduration.Visible = true;
                lblbatch.Visible = true;
                lbldegree.Visible = true;
                lblbranch.Visible = true;
                lblduration.Visible = true;
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                loadsec();
                load_subject();
                ddlsubject.Width = 100;
                chkover.Visible = true;
                ddlsubject.Visible = true;
                lblsubject.Visible = true;
                ddlsec.Visible = true;
                lblsec.Visible = true;
                psubject.Visible = false;
                txtsubject.Visible = false;
            }
            else
            {
                rdb_subjectwise.Checked = true; rdb_subjecttypewise.Checked = false;
                //btnGo.Attributes.Add("Style='top: 173px; left: 445px; position: absolute;font-size:medium;font-family:Book Antiqua;font-weight: bold;'", "Style='top: 173px; left: 445px; position: absolute;font-size:medium;font-family:Book Antiqua;font-weight: bold;'");
                chkelective.Visible = true;
                ddlbatch.Visible = false;
                ddldegree.Visible = false;
                ddlbranch.Visible = false;
                ddlduration.Visible = false;
                lblbatch.Visible = false;
                lbldegree.Visible = false;
                lblbranch.Visible = false;
                lblduration.Visible = false;
                ddlsubject.Width = 200;
                chkover.Visible = false;
                ddlsubject.Visible = true;
                loadallsubject();
                lblsubject.Visible = true;
                ddlsec.Visible = false;
                lblsec.Visible = false;
                psubject.Visible = false;
                txtsubject.Visible = false;
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
            string sec = "";
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.ToString() == string.Empty || ddlsec.Text == "All")
                {
                    sec = "";
                }
                else
                {
                    sec = " -Sec-" + ddlsec.SelectedItem.ToString();

                }
            }
            else
            {
                sec = "";
            }
            if (ddlreport.SelectedItem.ToString() == "Missing Student")
            {
                string sem = ddlduration.SelectedItem.ToString();
                if (sem == "1")
                {
                    sem = "I";
                }
                else if (sem == "2")
                {
                    sem = "II";
                }
                else if (sem == "3")
                {
                    sem = "III";
                }
                else if (sem == "4")
                {
                    sem = "IV";
                }
                else if (sem == "5")
                {
                    sem = "V";
                }
                else if (sem == "6")
                {
                    sem = "VI";
                }
                else if (sem == "7")
                {
                    sem = "VII";
                }
                else if (sem == "8")
                {
                    sem = "VIII";
                }
                //string degreedetails = "Elective Interdisciplinary Selected For the Semester "+sem+" " + "@ Batch-Degree: " + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlduration.SelectedItem.ToString() + sec;
                string degreedetails = "";
                string pagename = "SubjectSelectedStudentReport.aspx";
                Printcontrol.loadspreaddetails(subject_spread, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            else
            {
                string degreedetails = "Subject Selected Student Report " + '@' + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlduration.SelectedItem.ToString();
                string pagename = "SubjectSelectedStudentReport.aspx";
                Printcontrol.loadspreaddetails(subject_spread, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
        }
        catch
        {
        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcel.Text;
            errmsg.Visible = false;
            if (reportname.ToString().Trim() != "")
            {
                dacc.printexcelreport(subject_spread, reportname);
                txtexcel.Text = "";
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch
        {
        }
    }
    protected void btnprint1_Click(object sender, EventArgs e)
    {
        try
        {
            string pagename = "SubjectSelectedStudentReport1.aspx";
            string strdegta = "";
            if (chkattfile.Checked == true)
            {
                strdegta = "ATTENDANCE SHEET $DEPARTMENT OF Common Papers@Day Order :@Date           :@Hour         :";
            }
            //PRINTPDF.loadspreaddetails(Fpstucount, pagename, "@Subject Wise Registered Student Count");
            PRINTPDF.loadspreaddetails(Fpstucount, pagename, strdegta);
            PRINTPDF.Visible = true;
        }
        catch
        {
        }
    }
    protected void btnxl1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtxl.Text;
            errmsg.Visible = false;
            if (reportname.ToString().Trim() != "")
            {
                dacc.printexcelreport(Fpstucount, reportname);
                txtexcel.Text = "";
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch
        {
        }
    }
    public void clear()
    {
        chkattfile.Visible = false;
        subject_spread.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        lblexcl.Visible = false;
        txtexcel.Visible = false;
        lblavailable.Visible = false;
        lblfiled.Visible = false;
        lblexceed.Visible = false;
        btnav.Visible = false;
        btnexc.Visible = false;
        btnfil.Visible = false;
        Fpstucount.Visible = false;
        lblxl.Visible = false;
        txtxl.Visible = false;
        btnxl1.Visible = false;
        btnprint1.Visible = false;
        PRINTPDF.Visible = false;
        lblnorec.Visible = false;
        // rbname.Checked = true;
        rbname.Visible = false;
        rbacr.Visible = false;
        //lbldename.Visible = false;
    }
    protected void chkover_CheckedChanged(object sender, EventArgs e)
    {
        clear();
    }
    #endregion

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            chkattfile.Visible = false;
            if (ddlreport.SelectedItem.ToString() == "Degree Wise" && chkover.Checked == true)
            {
                loadalldept();
            }
            else if (ddlreport.SelectedItem.ToString() == "Missing Student")
            {
                if (rdb_subjectwise.Checked == true)
                {
                    loadmissing();
                }
                if (rdb_subjecttypewise.Checked == true)
                {
                    loadmissingnew();
                }
            }
            else
            {
                subject_spread.Sheets[0].ColumnHeader.RowCount = 1;
                subject_spread.Sheets[0].ColumnCount = 0;
                subject_spread.Sheets[0].ColumnCount = 7;
                subject_spread.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                subject_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Type";
                subject_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Code";
                subject_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Name";
                subject_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Maximum Limit";
                subject_spread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Actual Selected";
                subject_spread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Available";

                subject_spread.Sheets[0].Columns[0].Width = 30;
                subject_spread.Sheets[0].Columns[1].Width = 80;
                subject_spread.Sheets[0].Columns[2].Width = 80;
                subject_spread.Sheets[0].Columns[3].Width = 300;
                subject_spread.Sheets[0].Columns[4].Width = 50;
                subject_spread.Sheets[0].Columns[5].Width = 50;
                subject_spread.Sheets[0].Columns[6].Width = 50;
                subject_spread.Width = 900;
                subject_spread.Sheets[0].RowCount = 0;

                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                subject_spread.Sheets[0].Columns[2].CellType = txt;
                subject_spread.Sheets[0].Columns[0].CellType = txt;

                clear();
                if (ddlreport.SelectedItem.ToString() == "Degree Wise" || ddlreport.SelectedItem.ToString() == "With staff")
                {
                    // subject_spread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    string batchyear = ddlbatch.SelectedValue.ToString();
                    string degreecode = ddlbranch.SelectedValue.ToString();
                    string sem = ddlduration.SelectedItem.ToString();
                    string subtypeno = "";
                    if (ddlsubject.Enabled == true && ddlsubject.Items.Count > 0)
                    {
                        if (ddlsubject.SelectedItem.ToString().Trim().ToLower() != "all")
                        {
                            subtypeno = " and subtype_no='" + ddlsubject.SelectedValue.ToString() + "'";
                        }

                        string query = "select distinct subtype_no,subject_no,subject_code,subject_name,isnull(maxstud,0) as maxstud,(select subject_type from sub_sem ss where ss.syll_code=s.syll_code and ss.subtype_no=s.subtype_no)as subject_type from subject s,syllabus_master sy where s.syll_code=sy.syll_code and sy.batch_year=" + batchyear + " and sy.degree_code=" + degreecode + " and sy.semester=" + sem + " " + subtypeno + " order by subtype_no,subject_no";
                        ds.Reset();
                        ds.Dispose();
                        ds = dacc.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            chkattfile.Visible = true;
                            subject_spread.Visible = true;
                            btnprintmaster.Visible = true;
                            lblexcl.Visible = true;
                            txtexcel.Visible = true;
                            btnxl.Visible = true;
                            lblavailable.Visible = true;
                            lblfiled.Visible = true;
                            lblexceed.Visible = true;
                            btnav.Visible = true;
                            btnexc.Visible = true;
                            btnfil.Visible = true;
                            rbname.Visible = true;
                            rbacr.Visible = true;
                            //lbldename.Visible = true;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                subject_spread.Sheets[0].RowCount++;
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Text = subject_spread.Sheets[0].RowCount.ToString();
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["subject_type"].ToString();
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].Note = ds.Tables[0].Rows[i]["subject_no"].ToString();
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["subject_name"].ToString();
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["maxstud"].ToString();

                                Double selecstud = 0, Staus = 0;
                                Double maxstud = Convert.ToDouble(ds.Tables[0].Rows[i]["maxstud"].ToString());

                                string getselectstude = dacc.GetFunction("select distinct isnull(count(s.roll_no),0) as selected from subjectchooser s,registration r where s.roll_no=r.roll_no and s.subject_no =" + ds.Tables[0].Rows[i]["subject_no"].ToString() + " and r.degree_code=" + degreecode + " and r.batch_year=" + batchyear + " and s.semester=" + sem + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar'");
                                int subcount = Convert.ToInt32(dacc.GetFunction("select isnull(COUNT(distinct sc.roll_no),'0') from subjectChooser sc,subject s,registration r where s.subject_no=sc.subject_no and sc.roll_no=r.roll_no and r.cc=0 and delflag=0 and r.exam_flag<>'Debar' and s.subject_name='" + ds.Tables[0].Rows[i]["subject_name"].ToString() + "' and subject_code='" + ds.Tables[0].Rows[i]["subject_code"].ToString() + "'  and r.batch_year=" + batchyear + " and sc.semester='" + sem + "' "));

                                if (getselectstude != null && getselectstude.Trim() != "")
                                {
                                    selecstud = Convert.ToDouble(getselectstude);
                                }

                                if (maxstud > 0)
                                {
                                    if (maxstud >= selecstud)
                                    //if (maxstud >= subcount)
                                    {
                                        Staus = maxstud - selecstud;
                                    }
                                    else
                                    {
                                        Staus = 0;
                                    }

                                }
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].Text = selecstud.ToString();
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].Font.Underline = true;
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].ForeColor = Color.Blue;
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 6].Text = Staus.ToString();

                                if (maxstud > 0)
                                {
                                    if (maxstud == subcount)
                                    {
                                        subject_spread.Sheets[0].Rows[subject_spread.Sheets[0].RowCount - 1].BackColor = Color.Orange;
                                    }
                                    else if (maxstud > subcount)
                                    {
                                        subject_spread.Sheets[0].Rows[subject_spread.Sheets[0].RowCount - 1].BackColor = Color.LightPink;
                                    }
                                    else if (maxstud < subcount)
                                    {
                                        subject_spread.Sheets[0].Rows[subject_spread.Sheets[0].RowCount - 1].BackColor = Color.LightSkyBlue;
                                    }
                                }


                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            }

                        }
                        else
                        {
                            clear();
                            errmsg.Visible = true;
                            errmsg.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        clear();
                        errmsg.Visible = true;
                        errmsg.Text = "No Records Found";
                    }
                }
                else
                {

                    string subcoe = ddlsubject.SelectedItem.ToString();

                    string[] spcode = subcoe.Split('-');
                    string subname = "";
                    string subcode = "";
                    for (int sn = 0; sn <= spcode.GetUpperBound(0); sn++)
                    {
                        if (sn == spcode.GetUpperBound(0))
                        {
                            subcode = spcode[sn].ToString();
                        }
                        else
                        {
                            if (subname == "")
                            {
                                subname = spcode[sn].ToString();
                            }
                            else
                            {
                                subname = subname + "-" + spcode[sn].ToString();
                            }
                        }
                    }

                    string query = "select distinct subject_type,s.subject_name,s.subject_code from subject s,sub_sem sm  where subject_name='" + subname + "' and subject_code='" + subcode + "' and s.subType_no=sm.subType_no";
                    ds.Reset();
                    ds.Dispose();
                    ds = dacc.select_method_wo_parameter(query, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        chkattfile.Visible = true;
                        subject_spread.Visible = true;
                        btnprintmaster.Visible = true;
                        lblexcl.Visible = true;
                        txtexcel.Visible = true;
                        btnxl.Visible = true;
                        lblavailable.Visible = true;
                        lblfiled.Visible = true;
                        lblexceed.Visible = true;
                        btnav.Visible = true;
                        btnexc.Visible = true;
                        btnfil.Visible = true;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            subject_spread.Sheets[0].RowCount++;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Text = subject_spread.Sheets[0].RowCount.ToString();
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["subject_type"].ToString();
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].Note = "";
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["subject_name"].ToString();
                            string subjecttype = ds.Tables[0].Rows[i]["subject_type"].ToString();
                            Double selecstud = 0, Staus = 0, maxstud = 0;
                            maxstud = Convert.ToDouble(dacc.GetFunction("select  isnull(max(maxstud),0) as maxstud from subject s,sub_sem sm where subject_name='" + subname + "' and subject_code='" + subcode + "' and s.subType_no=sm.subType_no and sm.subject_type='" + subjecttype + "'"));
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 4].Text = maxstud.ToString();
                            int subcount = Convert.ToInt32(dacc.GetFunction("select isnull(COUNT(distinct r.roll_no),'0') from subjectChooser s,Registration r,subject sc,sub_sem sm where r.roll_no=s.roll_no and sc.subType_no=sm.subType_no and sm.subject_type='" + subjecttype + "' and  sc.subject_no=s.subject_no and subject_name='" + subname + "' and subject_code='" + subcode + "' and r.Current_Semester=s.semester  and r.cc=0 and delflag=0 and r.exam_flag<>'Debar'"));

                            if (maxstud > 0)
                            {
                                if (maxstud >= subcount)
                                {
                                    Staus = maxstud - subcount;
                                }
                                else
                                {
                                    Staus = 0;
                                }

                            }
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].Text = subcount.ToString();
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].Font.Underline = true;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].ForeColor = Color.Blue;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 6].Text = Staus.ToString();

                            if (maxstud > 0)
                            {
                                if (maxstud == subcount)
                                {
                                    subject_spread.Sheets[0].Rows[subject_spread.Sheets[0].RowCount - 1].BackColor = Color.Orange;
                                }
                                else if (maxstud > subcount)
                                {
                                    subject_spread.Sheets[0].Rows[subject_spread.Sheets[0].RowCount - 1].BackColor = Color.LightPink;
                                }
                                else if (maxstud < subcount)
                                {
                                    subject_spread.Sheets[0].Rows[subject_spread.Sheets[0].RowCount - 1].BackColor = Color.LightSkyBlue;
                                }
                            }


                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else
                    {
                        clear();
                        errmsg.Visible = true;
                        errmsg.Text = "No Records Found";

                    }
                }
            }
            subject_spread.Sheets[0].PageSize = subject_spread.Sheets[0].RowCount;
        }
        catch
        {
        }
    }
    protected void subject_spread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellclick = true;
    }
    protected void subject_spread_Prerender(Object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                string activerow = subject_spread.ActiveSheetView.ActiveRow.ToString();
                string activecol = subject_spread.ActiveSheetView.ActiveColumn.ToString();
                Fpstucount.Visible = false;
                lblxl.Visible = false;
                txtxl.Visible = false;
                btnxl1.Visible = false;
                btnprint1.Visible = false;
                PRINTPDF.Visible = false;
                Fpstucount.CommandBar.Visible = false;
                if (activerow.Trim() != "-1" && activerow.Trim() != "" && activerow.Trim() != "System.Object" && activecol.Trim() == "5" && chkover.Checked == false)
                {
                    string Subjectno = subject_spread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Note;
                    string Subjectname = subject_spread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                    string Subjectcode = subject_spread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                    string subjecttype = subject_spread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                    Fpstucount.Sheets[0].ColumnCount = 0;
                    Fpstucount.Sheets[0].ColumnHeader.RowCount = 2;
                    Fpstucount.Sheets[0].ColumnCount = 9;
                    Fpstucount.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Fpstucount.Sheets[0].ColumnHeader.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Fpstucount.Sheets[0].ColumnHeader.Cells[0, 0].Text = " Subject :  " + Subjectcode + " - " + Subjectname + "";
                    Fpstucount.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Left;
                    Fpstucount.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 9);
                    Fpstucount.Sheets[0].ColumnHeader.Cells[1, 0].Text = "S.No";
                    Fpstucount.Sheets[0].ColumnHeader.Cells[1, 1].Text = "Roll No";
                    Fpstucount.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Reg No";
                    Fpstucount.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Student Name";
                    Fpstucount.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Degree Details";
                    Fpstucount.Sheets[0].ColumnHeader.Cells[1, 5].Text = ". ";
                    Fpstucount.Sheets[0].ColumnHeader.Cells[1, 6].Text = ". ";
                    Fpstucount.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Staff Code";
                    Fpstucount.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Staff Name";
                    if (chkattfile.Checked == true)
                    {
                        Fpstucount.Sheets[0].Columns[5].Visible = true;
                        Fpstucount.Sheets[0].Columns[6].Visible = true;
                    }
                    else
                    {
                        Fpstucount.Sheets[0].Columns[5].Visible = false;
                        Fpstucount.Sheets[0].Columns[6].Visible = false;
                    }
                    if (ddlreport.SelectedItem.ToString() == "With staff")
                    {
                        Fpstucount.Sheets[0].Columns[7].Visible = true;
                        Fpstucount.Sheets[0].Columns[8].Visible = true;
                    }
                    else
                    {
                        Fpstucount.Sheets[0].Columns[7].Visible = false;
                        Fpstucount.Sheets[0].Columns[8].Visible = false;
                    }
                    Fpstucount.Sheets[0].Columns[4].Visible = false;
                    Fpstucount.Width = 900;
                    Fpstucount.Sheets[0].RowCount = 0;
                    Fpstucount.Sheets[0].Columns[3].Width = 150;
                    Fpstucount.Sheets[0].Columns[3].Width = 200;
                    if (Session["Rollflag"].ToString() == "0")
                    {
                        Fpstucount.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                    }
                    else
                    {
                        Fpstucount.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                    }
                    if (Session["Regflag"].ToString() == "0")
                    {
                        Fpstucount.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                    }
                    else
                    {
                        Fpstucount.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                    }


                    string orderby_Setting = dacc.GetFunction("select value from master_Settings where settings='order_by'");
                    string strorder = ",r.roll_no";
                    string srialno = dacc.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                    if (srialno == "1")
                    {
                        strorder = ",r.serialno";
                    }
                    if (orderby_Setting == "1")
                    {
                        strorder = ",r.Reg_No";
                    }
                    else if (orderby_Setting == "2")
                    {
                        strorder = ",r.Stud_Name";
                    }
                    else if (orderby_Setting == "0,1,2")
                    {
                        strorder = ",r.roll_no,r.Reg_No,r.Stud_Name";
                    }
                    else if (orderby_Setting == "0,1")
                    {
                        strorder = ",r.roll_no,r.Reg_No";
                    }
                    else if (orderby_Setting == "1,2")
                    {
                        strorder = ",r.Reg_No,r.Stud_Name";
                    }
                    else if (orderby_Setting == "0,2")
                    {
                        strorder = ",r.roll_no,r.Stud_Name";
                    }
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();


                    string sec = "";
                    if (ddlreport.SelectedItem.ToString() == "Degree Wise" && chkover.Checked == false)
                    {
                        if (ddlsec.Enabled == true)
                        {
                            if (ddlsec.SelectedItem.ToString() == string.Empty || ddlsec.Text == "All")
                            {
                                sec = "";
                            }
                            else
                            {
                                sec = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";

                            }
                        }
                        else
                        {
                            sec = "";
                        }
                    }

                    string sqlqueries = "";
                    if (Subjectno.Trim() != "")
                    {
                        //sqlqueries = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,r.Batch_Year,de.dept_acronym,de.Dept_Name,c.Course_Name,s.semester,r.sections,r.degree_code from subjectChooser s,Registration r,Degree d,course c,Department de,subject sc,sub_sem sm where r.roll_no=s.roll_no and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and  sc.subject_no=s.subject_no and sc.subType_no=sm.subType_no and sm.subject_type='" + subjecttype + "' and subject_name='" + Subjectname + "' and subject_code='" + Subjectcode + "' and r.Current_Semester=s.semester and r.cc=0 and delflag=0 and r.exam_flag<>'Debar' and r.Batch_Year='"+ddlbatch.SelectedItem.ToString()+"' and r.degree_code='"+ddlbranch.SelectedValue.ToString()+"' and r.Current_Semester='"+ddlduration.SelectedItem.ToString()+"' "+sec+" order by r.degree_code,r.Batch_Year desc,s.semester, r.sections " + strorder + " asc";
                        sqlqueries = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,r.Batch_Year,de.dept_acronym,de.Dept_Name,c.Course_Name,s.semester,r.sections,r.degree_code from subjectChooser s,Registration r,Degree d,course c,Department de,subject sc,sub_sem sm where r.roll_no=s.roll_no and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and  sc.subject_no=s.subject_no and sc.subType_no=sm.subType_no and sm.subject_type='" + subjecttype + "' and subject_name='" + Subjectname + "' and subject_code='" + Subjectcode + "' and r.cc=0 and delflag=0 and r.exam_flag<>'Debar' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and s.Semester='" + ddlduration.SelectedItem.ToString() + "' " + sec + " order by r.degree_code,r.Batch_Year desc,s.semester, r.sections " + strorder + " asc";
                    }
                    else
                    {
                        //sqlqueries = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,r.Batch_Year,de.dept_acronym,de.Dept_Name,c.Course_Name,s.semester,r.sections,r.degree_code from subjectChooser s,Registration r,Degree d,course c,Department de where r.roll_no=s.roll_no and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.Current_Semester=s.semester and s.subject_no='" + Subjectno + "' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlduration.SelectedItem.ToString() + "' order by r.degree_code,r.Batch_Year desc,s.semester, r.sections " + strorder + " asc";
                        sqlqueries = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,r.Batch_Year,de.dept_acronym,de.Dept_Name,c.Course_Name,sc.semester,r.sections,r.degree_code from subjectChooser sc,Registration r,Degree d,course c,Department de,subject s where r.roll_no=sc.roll_no and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.Current_Semester=sc.semester and r.cc=0 and delflag=0 and r.exam_flag<>'Debar' and s.subject_name='" + Subjectname + "' and subject_code='" + Subjectcode + "'  " + sec + " and s.subject_no=sc.subject_no order by r.degree_code,r.Batch_Year desc,sc.semester, r.sections " + strorder + " asc";
                    }
                    if (ddlreport.SelectedItem.ToString() == "With staff")
                    {
                         if (Subjectno.Trim() != "")
                        {
                            //sqlqueries = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,r.Batch_Year,de.dept_acronym,de.Dept_Name,c.Course_Name,s.semester,r.sections,r.degree_code from subjectChooser s,Registration r,Degree d,course c,Department de,subject sc,sub_sem sm where r.roll_no=s.roll_no and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and  sc.subject_no=s.subject_no and sc.subType_no=sm.subType_no and sm.subject_type='" + subjecttype + "' and subject_name='" + Subjectname + "' and subject_code='" + Subjectcode + "' and r.Current_Semester=s.semester and r.cc=0 and delflag=0 and r.exam_flag<>'Debar' and r.Batch_Year='"+ddlbatch.SelectedItem.ToString()+"' and r.degree_code='"+ddlbranch.SelectedValue.ToString()+"' and r.Current_Semester='"+ddlduration.SelectedItem.ToString()+"' "+sec+" order by r.degree_code,r.Batch_Year desc,s.semester, r.sections " + strorder + " asc";
                            sqlqueries = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,r.Batch_Year,de.dept_acronym,de.Dept_Name,c.Course_Name,s.semester,r.sections,r.degree_code,s.staffcode,smt.staff_name from subjectChooser s,Registration r,Degree d,course c,Department de,subject sc,sub_sem sm,staffmaster smt where smt.staff_code=s.staffcode and  r.roll_no=s.roll_no and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and  sc.subject_no=s.subject_no and sc.subType_no=sm.subType_no and sm.subject_type='" + subjecttype + "' and subject_name='" + Subjectname + "' and subject_code='" + Subjectcode + "' and r.cc=0 and delflag=0 and r.exam_flag<>'Debar' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and s.Semester='" + ddlduration.SelectedItem.ToString() + "' " + sec + "  group by s.staffcode,smt.staff_name,r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,r.Batch_Year,de.dept_acronym,de.Dept_Name,c.Course_Name,s.semester,r.sections,r.degree_code  order by r.degree_code,r.Batch_Year desc,s.semester, r.sections " + strorder + " asc";
                        }
                        else
                        {
                            //sqlqueries = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,r.Batch_Year,de.dept_acronym,de.Dept_Name,c.Course_Name,s.semester,r.sections,r.degree_code from subjectChooser s,Registration r,Degree d,course c,Department de where r.roll_no=s.roll_no and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.Current_Semester=s.semester and s.subject_no='" + Subjectno + "' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlduration.SelectedItem.ToString() + "' order by r.degree_code,r.Batch_Year desc,s.semester, r.sections " + strorder + " asc";
                            sqlqueries = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,r.Batch_Year,de.dept_acronym,de.Dept_Name,c.Course_Name,sc.semester,r.sections,r.degree_code from subjectChooser sc,Registration r,Degree d,course c,Department de,subject s where r.roll_no=sc.roll_no and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.Current_Semester=sc.semester and r.cc=0 and delflag=0 and r.exam_flag<>'Debar' and s.subject_name='" + Subjectname + "' and subject_code='" + Subjectcode + "'  " + sec + " and s.subject_no=sc.subject_no order by r.degree_code,r.Batch_Year desc,sc.semester, r.sections " + strorder + " asc";
                        }
                    }

                    ds.Dispose();
                    ds.Reset();
                    ds = dacc.select_method_wo_parameter(sqlqueries, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblxl.Visible = true;
                        txtxl.Visible = true;
                        btnxl1.Visible = true;
                        btnprint1.Visible = true;
                        PRINTPDF.Visible = false;
                        Fpstucount.Visible = true;
                        string temdegree = "";
                        int srno = 0;
                        int count = 0;
                        string staffName = string.Empty;
                        string staffCode = string.Empty;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string roll = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                            string reg = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                            string name = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
                            string batch = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                            string degree = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                            string Dept = ds.Tables[0].Rows[i]["dept_acronym"].ToString();
                            string Sem = ds.Tables[0].Rows[i]["semester"].ToString();
                            string Sec = ds.Tables[0].Rows[i]["sections"].ToString();
                            string deptn = ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                            if (ddlreport.SelectedItem.ToString() == "With staff")
                            {
                                 staffName = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                                 staffCode = Convert.ToString(ds.Tables[0].Rows[i]["staffcode"]);
                            }

                            string degredeatils = batch + '-' + degree + '-' + Dept + '-' + Sem;

                            string secg = "";
                            if (Sec.Trim() != "" && Sec.Trim() != "-1" && Sec != null)
                            {
                                degredeatils = degredeatils + '-' + Sec;
                                secg = " - " + Sec;
                            }
                            string acdegree = degree;
                            if (rbacr.Checked == true)
                            {
                                acdegree = acdegree + '-' + Dept + secg;
                            }
                            else
                            {
                                acdegree = acdegree + '-' + deptn + secg;
                            }
                            if (temdegree != acdegree)
                            {
                                srno = 0;
                                temdegree = acdegree;
                                Fpstucount.Sheets[0].RowCount++;
                                Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].Text = acdegree;
                                Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                Fpstucount.Sheets[0].SpanModel.Add(Fpstucount.Sheets[0].RowCount - 1, 0, 1, 4);
                            }
                            srno++;
                            count++;
                            Fpstucount.Sheets[0].RowCount++;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                            // Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].Text = Fpstucount.Sheets[0].RowCount.ToString();
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 1].Text = roll;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 2].Text = reg;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 3].Text = name;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 7].Text = staffCode;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 8].Text = staffName;
                            //Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 4].Text = degredeatils;

                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].CellType = txt;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 1].CellType = txt;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 2].CellType = txt;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 3].CellType = txt;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 7].CellType = txt;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 8].CellType = txt;
                            //  Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 4].CellType = txt;

                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                            //Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        }
                        if (chkattfile.Checked == false)
                        {
                            DataTable dicstaff = ds.Tables[0].DefaultView.ToTable(true, "staffcode", "staff_name");

                            if (dicstaff.Rows.Count > 0)
                            {
                                foreach(DataRow dt in dicstaff.Rows)
                                {
                                    string staffCode1=Convert.ToString(dt["staffcode"]);
                                    string staffName1 = Convert.ToString(dt["staff_name"]);
                                    ds.Tables[0].DefaultView.RowFilter = "staffcode='" + staffCode1 + "'";
                                    DataTable dtstaff = ds.Tables[0].DefaultView.ToTable();
                                    if (dtstaff.Rows.Count > 0)
                                    {
                                        Fpstucount.Sheets[0].RowCount++;
                                        Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].Text = staffName1 + ": " + dtstaff.Rows.Count + "";
                                        Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue;
                                        Fpstucount.Sheets[0].SpanModel.Add(Fpstucount.Sheets[0].RowCount - 1, 0, 1, Fpstucount.Sheets[0].ColumnCount);
                                        Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                    }
                                }
                            }
                            Fpstucount.Sheets[0].RowCount++;
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].Text = "Total Registerd : " + count + "";
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            Fpstucount.Sheets[0].SpanModel.Add(Fpstucount.Sheets[0].RowCount - 1, 0, 1, Fpstucount.Sheets[0].ColumnCount);
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            //Fpstucount.Sheets[0].RowCount++;
                            //Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].Text = "Grand Total Registerd : " + count + "";
                            //Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].Text = "Available Seats  : " + count + "";
                            //Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].Font.Bold = true;

                          
                            Fpstucount.Sheets[0].SpanModel.Add(Fpstucount.Sheets[0].RowCount - 1, 0, 1, Fpstucount.Sheets[0].ColumnCount);
                            Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        }
                        //if (ddlreport.SelectedItem.ToString() == "With staff")
                        //{
                        //    Fpstucount.Sheets[0].SpanModel.Add(Fpstucount.Sheets[0].RowCount - 1, 0, 1, Fpstucount.Sheets[0].ColumnCount);
                        //    Fpstucount.Sheets[0].Cells[Fpstucount.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        //}
                        int wid = 0;
                        for (int i = 0; i < Fpstucount.Sheets[0].ColumnCount; i++)
                        {
                            if (Fpstucount.Sheets[0].Columns[i].Visible == true)
                            {

                                wid = wid + Fpstucount.Sheets[0].Columns[i].Width;
                            }
                        }
                        Fpstucount.Width = wid;
                    }
                    else
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Record Found";
                        Fpstucount.Visible = false;
                    }
                    Fpstucount.Sheets[0].PageSize = Fpstucount.Sheets[0].RowCount;
                    Fpstucount.SaveChanges();
                }
            }
        }
        catch
        {
        }
    }
    public void loadalldept()
    {
        try
        {
            subject_spread.Sheets[0].ColumnHeader.RowCount = 1;
            subject_spread.Sheets[0].ColumnCount = 0;
            subject_spread.Sheets[0].ColumnCount = 6;
            subject_spread.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree Details";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Title of Paper";


            subject_spread.Width = 900;
            subject_spread.Sheets[0].RowCount = 0;

            subject_spread.Sheets[0].Columns[0].Width = 30;
            subject_spread.Sheets[0].Columns[1].Width = 120;
            subject_spread.Sheets[0].Columns[2].Width = 120;
            subject_spread.Sheets[0].Columns[3].Width = 150;
            subject_spread.Sheets[0].Columns[4].Width = 180;
            subject_spread.Sheets[0].Columns[5].Width = 200;

            subject_spread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            subject_spread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            subject_spread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            subject_spread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            subject_spread.Sheets[0].Columns[2].CellType = txt;
            subject_spread.Sheets[0].Columns[1].CellType = txt;
            subject_spread.Sheets[0].Columns[3].CellType = txt;
            subject_spread.Sheets[0].Columns[0].CellType = txt;


            if (Session["Rollflag"].ToString() == "0")
            {
                Fpstucount.Sheets[0].ColumnHeader.Columns[1].Visible = false;
            }
            else
            {
                Fpstucount.Sheets[0].ColumnHeader.Columns[1].Visible = true;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                Fpstucount.Sheets[0].ColumnHeader.Columns[2].Visible = false;
            }
            else
            {
                Fpstucount.Sheets[0].ColumnHeader.Columns[2].Visible = true;
            }

            clear();


            string batchyear = ddlbatch.SelectedValue.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string sem = ddlduration.SelectedItem.ToString();
            string subtypeno = "";
            if (ddlsubject.Enabled == true && ddlsubject.Items.Count > 0)
            {
                if (ddlsubject.SelectedItem.ToString().Trim().ToLower() != "all")
                {
                    subtypeno = " and sm.subtype_no='" + ddlsubject.SelectedValue.ToString() + "'";
                }

                string orderby_Setting = dacc.GetFunction("select value from master_Settings where settings='order_by'");
                string strorder = "r.roll_no";
                string srialno = dacc.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                if (srialno == "1")
                {
                    strorder = "r.serialno";
                }
                if (orderby_Setting == "1")
                {
                    strorder = "r.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "r.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "r.roll_no,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "r.roll_no,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "r.roll_no,r.Stud_Name";
                }
                string sec = "";
                if (ddlsec.Enabled == true)
                {
                    if (ddlsec.SelectedItem.ToString() == string.Empty || ddlsec.Text == "All")
                    {
                        sec = "";
                    }
                    else
                    {
                        sec = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";

                    }
                }
                else
                {
                    sec = "";
                }
                // string sqlqueries = "select distinct r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,s.subject_name,de.Dept_Name,de.dept_acronym,c.Course_name,s.subType_no,sm.subject_type,r.sections,r.current_semester,r.batch_year,s.subject_no from Registration r,subjectChooser sc,subject s,syllabus_master sy,sub_sem sm,Degree d,Department de,course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.Current_Semester=sc.semester and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and r.Current_Semester=sy.semester and s.subType_no=sm.subType_no and sy.syll_code=sm.syll_code and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0 and r.DelFlag=0and r.Exam_Flag<>'debar' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' and r.Current_Semester='" + ddlduration.SelectedItem.ToString() + "' " + subtypeno + " "+sec+" order by " + strorder + ",s.subject_no";
                string sqlqueries = "select distinct r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,s.subject_name,de.Dept_Name,de.dept_acronym,c.Course_name,s.subType_no,sm.subject_type,r.sections,r.current_semester,r.batch_year,s.subject_no from Registration r,subjectChooser sc,subject s,syllabus_master sy,sub_sem sm,Degree d,Department de,course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sc.semester=sy.semester and s.subType_no=sm.subType_no and sy.syll_code=sm.syll_code and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0 and r.DelFlag=0and r.Exam_Flag<>'debar' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' and sc.semester='" + ddlduration.SelectedItem.ToString() + "' " + subtypeno + " " + sec + " order by " + strorder + ",s.subject_no";
                ds.Reset();
                ds.Dispose();
                ds = dacc.select_method_wo_parameter(sqlqueries, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chkattfile.Visible = true;
                    subject_spread.Visible = true;
                    btnprintmaster.Visible = true;
                    lblexcl.Visible = true;
                    txtexcel.Visible = true;
                    btnxl.Visible = true;
                    lblavailable.Visible = false;
                    lblfiled.Visible = false;
                    lblexceed.Visible = false;
                    btnav.Visible = false;
                    btnexc.Visible = false;
                    btnfil.Visible = false;
                    rbname.Visible = true;
                    rbacr.Visible = true;
                    //lbldename.Visible = true;
                    int srno = 0;
                    string temproll = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        string degreedetails = ds.Tables[0].Rows[i]["batch_year"].ToString() + '-' + ds.Tables[0].Rows[i]["Course_name"].ToString() + '-' + ds.Tables[0].Rows[i]["dept_acronym"].ToString() + '-' + ds.Tables[0].Rows[i]["current_semester"].ToString();
                        if (rbname.Checked == true)
                        {
                            degreedetails = ds.Tables[0].Rows[i]["batch_year"].ToString() + '-' + ds.Tables[0].Rows[i]["Course_name"].ToString() + '-' + ds.Tables[0].Rows[i]["Dept_Name"].ToString() + '-' + ds.Tables[0].Rows[i]["current_semester"].ToString();
                        }

                        string sections = ds.Tables[0].Rows[i]["sections"].ToString();
                        if (sections != null && sections.Trim() != "" && sections.Trim() != "-1")
                        {
                            degreedetails = degreedetails + '-' + sections;
                        }
                        if (temproll != ds.Tables[0].Rows[i]["Roll_No"].ToString())
                        {
                            temproll = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                            srno++;
                        }
                        subject_spread.Sheets[0].RowCount++;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["stud_name"].ToString();
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 4].Text = degreedetails;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["subject_name"].ToString();
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    }

                }
                else
                {
                    clear();
                    errmsg.Visible = true;
                    errmsg.Text = "No Records Found";
                }
            }
            else
            {
                clear();
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }

        }
        catch
        {
        }
    }
    public void loadmissing()
    {
        try
        {
            subject_spread.Sheets[0].ColumnHeader.RowCount = 1;
            subject_spread.Sheets[0].ColumnCount = 0;
            subject_spread.Sheets[0].ColumnCount = 6;
            subject_spread.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree Details";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Paper Name";


            subject_spread.Width = 900;
            subject_spread.Sheets[0].RowCount = 0;

            subject_spread.Sheets[0].Columns[0].Width = 30;
            subject_spread.Sheets[0].Columns[1].Width = 120;
            subject_spread.Sheets[0].Columns[2].Width = 120;
            subject_spread.Sheets[0].Columns[3].Width = 150;
            subject_spread.Sheets[0].Columns[4].Width = 180;
            subject_spread.Sheets[0].Columns[5].Width = 200;

            subject_spread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            subject_spread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            subject_spread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            subject_spread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            subject_spread.Sheets[0].Columns[2].CellType = txt;
            subject_spread.Sheets[0].Columns[1].CellType = txt;
            subject_spread.Sheets[0].Columns[3].CellType = txt;
            subject_spread.Sheets[0].Columns[0].CellType = txt;
            // subject_spread.Sheets[0].Columns[4].Visible = false;

            if (Session["Rollflag"].ToString() == "0")
            {
                subject_spread.Sheets[0].ColumnHeader.Columns[1].Visible = false;
            }
            else
            {
                subject_spread.Sheets[0].ColumnHeader.Columns[1].Visible = true;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                subject_spread.Sheets[0].ColumnHeader.Columns[2].Visible = false;
            }
            else
            {
                subject_spread.Sheets[0].ColumnHeader.Columns[2].Visible = true;
            }
            subject_spread.Sheets[0].ColumnHeader.Columns[0].Visible = true;
            subject_spread.Sheets[0].ColumnHeader.Columns[3].Visible = true;
            subject_spread.Sheets[0].ColumnHeader.Columns[4].Visible = true;
            subject_spread.Sheets[0].ColumnHeader.Columns[5].Visible = true;

            clear();

            string sec = "";
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.ToString() == string.Empty || ddlsec.Text == "All")
                {
                    sec = "";
                }
                else
                {
                    sec = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";

                }
            }
            else
            {
                sec = "";
            }
            string batchyear = ddlbatch.SelectedValue.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string sem = ddlduration.SelectedItem.ToString();
            //string subtypeno = "";
            if (chklssubject.Items.Count > 0)
            {
                string orderby_Setting = dacc.GetFunction("select value from master_Settings where settings='order_by'");
                string strorder = "r.roll_no";
                string srialno = dacc.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                if (srialno == "1")
                {
                    strorder = "r.serialno";
                }
                if (orderby_Setting == "1")
                {
                    strorder = "r.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "r.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "r.roll_no,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "r.roll_no,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "r.roll_no,r.Stud_Name";
                }
                string subjectnum = "";
                for (int i = 0; i < chklssubject.Items.Count; i++)
                {
                    if (chklssubject.Items[i].Selected == true)
                    {
                        if (subjectnum == "")
                        {
                            subjectnum = "'" + chklssubject.Items[i].Value + "'";
                        }
                        else
                        {
                            subjectnum = subjectnum + ",'" + chklssubject.Items[i].Value + "'";
                        }
                    }
                }

                if (rdb_subjectwise.Checked == true)
                {
                    if (subjectnum.Trim() != "")
                    {
                        subjectnum = " and sc.subject_no in (" + subjectnum + ")";
                    }
                }
                if (rdb_subjecttypewise.Checked == true)
                {
                    if (subjectnum.Trim() != "")
                    {
                        subjectnum = " and s.subType_no in (" + subjectnum + ")";
                    }
                }
                if (subjectnum.Trim() == "")
                {
                    clear();
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Subject And Then Proceed";
                    return;
                }
                string sqlqueries = " select distinct r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,de.Dept_Name,de.dept_acronym,c.Course_name,r.sections,r.current_semester,r.batch_year,s.subject_name from Registration r,Degree d,Department de,course c,subjectChooser sc,subject s where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and s.subType_no=sc.subtype_no and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' and sc.Semester='" + ddlduration.SelectedItem.ToString() + "' " + sec + " " + subjectnum + " order by " + strorder + "";
                ds.Reset();
                ds.Dispose();
                ds = dacc.select_method_wo_parameter(sqlqueries, "Text");


                string strstuquery = "select distinct r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,de.Dept_Name,de.dept_acronym,c.Course_name,r.sections,r.current_semester ,r.batch_year from Registration r,Degree d,Department de,course c where  r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0 and r.DelFlag=0  and r.Exam_Flag<>'debar' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' " + sec + " order by " + strorder + "";
                DataSet dsstulist = dacc.select_method_wo_parameter(strstuquery, "Text");
                if (dsstulist.Tables[0].Rows.Count > 0)
                {
                    subject_spread.Visible = true;
                    btnprintmaster.Visible = true;
                    lblexcl.Visible = true;
                    txtexcel.Visible = true;
                    btnxl.Visible = true;
                    lblavailable.Visible = false;
                    lblfiled.Visible = false;
                    lblexceed.Visible = false;
                    btnav.Visible = false;
                    btnexc.Visible = false;
                    btnfil.Visible = false;
                    rbname.Visible = true;
                    rbacr.Visible = true;
                    //lbldename.Visible = true;

                    int srno = 0;

                    for (int st = 0; st < dsstulist.Tables[0].Rows.Count; st++)
                    {
                        string degreedetails = dsstulist.Tables[0].Rows[st]["batch_year"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["Course_name"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["dept_acronym"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["current_semester"].ToString();
                        if (rbname.Checked == true)
                        {
                            degreedetails = dsstulist.Tables[0].Rows[st]["batch_year"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["Course_name"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["Dept_Name"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["current_semester"].ToString();
                        }

                        string sections = dsstulist.Tables[0].Rows[st]["sections"].ToString();
                        if (sections != null && sections.Trim() != "" && sections.Trim() != "-1")
                        {
                            degreedetails = degreedetails + '-' + sections;
                        }
                        string rollno = dsstulist.Tables[0].Rows[st]["Roll_No"].ToString();
                        srno++;
                        subject_spread.Sheets[0].RowCount++;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 1].Text = rollno;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].Text = dsstulist.Tables[0].Rows[st]["Reg_No"].ToString();
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 3].Text = dsstulist.Tables[0].Rows[st]["stud_name"].ToString();
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 4].Text = degreedetails;

                        string subject = "";
                        ds.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollno + "'";
                        DataView dvstusub = ds.Tables[0].DefaultView;
                        for (int subn = 0; subn < dvstusub.Count; subn++)
                        {
                            if (subject != "" && subject != null)
                            {
                                subject = subject + ',' + dvstusub[subn]["subject_name"].ToString();
                            }
                            else
                            {
                                subject = dvstusub[subn]["subject_name"].ToString();
                            }
                        }
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].Text = subject;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    }
                }
                else
                {
                    clear();
                    errmsg.Visible = true;
                    errmsg.Text = "No Records Found";
                }
            }
            else
            {
                clear();
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            subject_spread.SaveChanges();
        }
        catch
        {
        }
    }
    protected void chksubject_ChekedChange(object sender, EventArgs e)
    {
        if (chksubject.Checked == true)
        {
            for (int i = 0; i < chklssubject.Items.Count; i++)
            {
                chklssubject.Items[i].Selected = true;
            }
            txtsubject.Text = "Subject (" + chklssubject.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklssubject.Items.Count; i++)
            {
                chklssubject.Items[i].Selected = false;
            }
            txtsubject.Text = "--Select--";
        }
    }
    protected void chklssubject_SelectedIndexChanged(object sender, EventArgs e)
    {

        int commcount = 0;
        for (int i = 0; i < chklssubject.Items.Count; i++)
        {
            if (chklssubject.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        chksubject.Checked = false;
        txtsubject.Text = "Subject(" + commcount.ToString() + ")";
        if (commcount == chklssubject.Items.Count)
        {
            chksubject.Checked = true;
        }
        if (commcount == 0)
        {
            txtsubject.Text = "--Select--";
        }
    }
    public void loadelesubject()
    {
        try
        {
            clear();

            chklssubject.Items.Clear();
            chksubject.Checked = false;
            txtsubject.Text = "---Select---";
            string batchyear = ddlbatch.SelectedValue.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string sem = ddlduration.SelectedItem.ToString();

            ds.Dispose();
            ds.Reset();
            //string strquery = "select distinct subject.subject_name,subject.subject_no,subject.subtype_no,subject_type from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code='" + degreecode + "' and semester='" + sem + "' and batch_year = '" + batchyear + "') and sub_sem.ElectivePap='1' order by subject.subtype_no";

            string strquery = "select distinct s.subject_name,s.subject_no,s.subtype_no,ss.subject_type from subject s,sub_sem ss,syllabus_master sm where ss.subtype_no=s.subtype_no and sm.syll_code=s.syll_code and s.syll_code=ss.syll_code and sm.syll_code=ss.syll_code and sm.degree_code='" + degreecode + "' and sm.semester='" + sem + "' and sm.batch_year = '" + batchyear + "'  order by s.subtype_no";//and ISNULL(ss.ElectivePap,'0')='1'

            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklssubject.DataSource = ds;
                chklssubject.DataTextField = "subject_name";
                chklssubject.DataValueField = "subject_no";
                chklssubject.DataBind();

                for (int i = 0; i < chklssubject.Items.Count; i++)
                {
                    chklssubject.Items[i].Selected = true;
                }
                chksubject.Checked = true;
                txtsubject.Text = "Subject(" + chklssubject.Items.Count.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    public void loadsubtype()
    {
        try
        {
            clear();
            chklssubject.Items.Clear();
            chksubject.Checked = false;
            txtsubject.Text = "---Select---";
            string batchyear = ddlbatch.SelectedValue.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string sem = ddlduration.SelectedItem.ToString();
            ds.Dispose();
            ds.Reset();

            string strquery = "select distinct subject.subtype_no,subject_type from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + degreecode + " and semester=" + sem + " and batch_year = " + batchyear + ") and sub_sem.ElectivePap='1' order by subject.subtype_no";
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklssubject.DataSource = ds;
                chklssubject.DataTextField = "subject_type";
                chklssubject.DataValueField = "subtype_no";
                chklssubject.DataBind();

                for (int i = 0; i < chklssubject.Items.Count; i++)
                {
                    chklssubject.Items[i].Selected = true;
                }
                chksubject.Checked = true;
                txtsubject.Text = "Subject Type(" + chklssubject.Items.Count.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chkelective_CheckedChanged(object sender, EventArgs e)
    {
        loadallsubject();
    }
    protected void rdb_subjecttypewise_selectedchange(object sender, EventArgs e)
    {
        loadsubtype();
    }
    protected void rdb_subjectwise_selectedchange(object sender, EventArgs e)
    {
        loadelesubject();
    }

    public void loadmissingnew()
    {
        try
        {
            subject_spread.Sheets[0].ColumnHeader.RowCount = 1;
            subject_spread.Sheets[0].ColumnCount = 0;
            subject_spread.Sheets[0].ColumnCount = 6;
            subject_spread.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree Details";
            subject_spread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Paper Name";


            subject_spread.Width = 900;
            subject_spread.Sheets[0].RowCount = 0;

            subject_spread.Sheets[0].Columns[0].Width = 30;
            subject_spread.Sheets[0].Columns[1].Width = 120;
            subject_spread.Sheets[0].Columns[2].Width = 120;
            subject_spread.Sheets[0].Columns[3].Width = 150;
            subject_spread.Sheets[0].Columns[4].Width = 180;
            subject_spread.Sheets[0].Columns[5].Width = 200;

            subject_spread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            subject_spread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            subject_spread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            subject_spread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            subject_spread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            subject_spread.Sheets[0].Columns[2].CellType = txt;
            subject_spread.Sheets[0].Columns[1].CellType = txt;
            subject_spread.Sheets[0].Columns[3].CellType = txt;
            subject_spread.Sheets[0].Columns[0].CellType = txt;
            // subject_spread.Sheets[0].Columns[4].Visible = false;

            if (Session["Rollflag"].ToString() == "0")
            {
                subject_spread.Sheets[0].ColumnHeader.Columns[1].Visible = false;
            }
            else
            {
                subject_spread.Sheets[0].ColumnHeader.Columns[1].Visible = true;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                subject_spread.Sheets[0].ColumnHeader.Columns[2].Visible = false;
            }
            else
            {
                subject_spread.Sheets[0].ColumnHeader.Columns[2].Visible = true;
            }
            subject_spread.Sheets[0].ColumnHeader.Columns[0].Visible = true;
            subject_spread.Sheets[0].ColumnHeader.Columns[3].Visible = true;
            subject_spread.Sheets[0].ColumnHeader.Columns[4].Visible = true;
            subject_spread.Sheets[0].ColumnHeader.Columns[5].Visible = true;

            clear();

            string sec = "";
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.ToString() == string.Empty || ddlsec.Text == "All")
                {
                    sec = "";
                }
                else
                {
                    sec = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";

                }
            }
            else
            {
                sec = "";
            }
            string batchyear = ddlbatch.SelectedValue.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string sem = ddlduration.SelectedItem.ToString();
            if (chklssubject.Items.Count > 0)
            {
                string orderby_Setting = dacc.GetFunction("select value from master_Settings where settings='order_by'");
                string strorder = "r.roll_no";
                string srialno = dacc.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                if (srialno == "1")
                {
                    strorder = "r.serialno";
                }
                if (orderby_Setting == "1")
                {
                    strorder = "r.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "r.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "r.roll_no,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "r.roll_no,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "r.roll_no,r.Stud_Name";
                }
                string subjectnum = "";
                for (int i = 0; i < chklssubject.Items.Count; i++)
                {
                    if (chklssubject.Items[i].Selected == true)
                    {
                        if (subjectnum == "")
                        {
                            subjectnum = "'" + chklssubject.Items[i].Value + "'";
                        }
                        else
                        {
                            subjectnum = subjectnum + ",'" + chklssubject.Items[i].Value + "'";
                        }
                    }
                }

                if (rdb_subjectwise.Checked == true)
                {
                    if (subjectnum.Trim() != "")
                    {
                        subjectnum = " and sc.subject_no in (" + subjectnum + ")";
                    }
                }
                if (rdb_subjecttypewise.Checked == true)
                {
                    if (subjectnum.Trim() != "")
                    {
                        subjectnum = " and s.subType_no in (" + subjectnum + ")";
                    }
                }
                if (subjectnum.Trim() == "")
                {
                    clear();
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Subject And Then Proceed";
                    return;
                }

                string sqlqueries = " select distinct r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,de.Dept_Name,de.dept_acronym,c.Course_name,r.sections,r.current_semester,r.batch_year,s.subType_no from Registration r,Degree d,Department de,course c,subjectChooser sc,subject s where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and s.subType_no=sc.subtype_no and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' and sc.Semester='" + ddlduration.SelectedItem.ToString() + "' " + sec + " " + subjectnum + " order by " + strorder + "";
                ds.Reset();
                ds.Dispose();
                ds = dacc.select_method_wo_parameter(sqlqueries, "Text");
                string strstuquery = "select distinct r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,de.Dept_Name,de.dept_acronym,c.Course_name,r.sections,r.current_semester ,r.batch_year from Registration r,Degree d,Department de,course c where  r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0 and r.DelFlag=0  and r.Exam_Flag<>'debar' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' " + sec + " order by " + strorder + "";
                DataSet dsstulist = dacc.select_method_wo_parameter(strstuquery, "Text");
                if (dsstulist.Tables[0].Rows.Count > 0)
                {
                    subject_spread.Visible = true;
                    btnprintmaster.Visible = true;
                    lblexcl.Visible = true;
                    txtexcel.Visible = true;
                    btnxl.Visible = true;
                    lblavailable.Visible = false;
                    lblfiled.Visible = false;
                    lblexceed.Visible = false;
                    btnav.Visible = false;
                    btnexc.Visible = false;
                    btnfil.Visible = false;
                    rbname.Visible = true;
                    rbacr.Visible = true;
                    //lbldename.Visible = true;

                    int srno = 0;

                    for (int st = 0; st < dsstulist.Tables[0].Rows.Count; st++)
                    {
                        string degreedetails = dsstulist.Tables[0].Rows[st]["batch_year"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["Course_name"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["dept_acronym"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["current_semester"].ToString();
                        if (rbname.Checked == true)
                        {
                            degreedetails = dsstulist.Tables[0].Rows[st]["batch_year"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["Course_name"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["Dept_Name"].ToString() + '-' + dsstulist.Tables[0].Rows[st]["current_semester"].ToString();
                        }
                        string rollno = dsstulist.Tables[0].Rows[st]["Roll_No"].ToString();
                        string subject = "";
                        if (chklssubject.Items.Count > 0)
                        {
                            for (int sd = 0; sd < chklssubject.Items.Count; sd++)
                            {
                                if (chklssubject.Items[sd].Selected == true)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollno + "' and subtype_no='" + chklssubject.Items[sd].Value + "'";
                                    DataView dvstusub = ds.Tables[0].DefaultView;
                                    if (dvstusub.Count > 0)
                                    {

                                    }
                                    else
                                    {
                                        if (subject != "" && subject != null)
                                        {
                                            subject = subject + ',' + chklssubject.Items[sd].Text.ToString();
                                        }
                                        else
                                        {
                                            subject = chklssubject.Items[sd].Text.ToString();
                                        }
                                    }
                                }
                            }
                        }

                        string sections = dsstulist.Tables[0].Rows[st]["sections"].ToString();
                        if (sections != null && sections.Trim() != "" && sections.Trim() != "-1")
                        {
                            degreedetails = degreedetails + '-' + sections;
                        }

                        if (subject.Trim() != "")
                        {
                            srno++;
                            subject_spread.Sheets[0].RowCount++;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 1].Text = rollno;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].Text = dsstulist.Tables[0].Rows[st]["Reg_No"].ToString();
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 3].Text = dsstulist.Tables[0].Rows[st]["stud_name"].ToString();
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 4].Text = degreedetails;



                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].Text = subject;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        }
                    }
                }
                else
                {
                    clear();
                    errmsg.Visible = true;
                    errmsg.Text = "No Records Found";
                }
            }
            else
            {
                clear();
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            subject_spread.SaveChanges();
        }
        catch
        {
        }
    }
}
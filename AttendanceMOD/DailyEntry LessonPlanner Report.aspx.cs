using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;

public partial class DailyEntry_LessonPlanner_Report : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    String Day_Order = "";
    string schorder = "";
    Hashtable hat = new Hashtable();

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet dshr = new DataSet();
    DataSet dsstaff = new DataSet();
    DataSet dsalterschedule = new DataSet();
    DataSet dsholyday = new DataSet();
    static Boolean forschoolsetting = false;// Added by sridharan
    #region "Load Details"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        lblnorec.Visible = false;
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        if (!IsPostBack)
        {
            txtfrom.Attributes.Add("readonly", "readonly");
            txtto.Attributes.Add("readonly", "readonly");
            Fpdailyplanner.Width = 1000;
            Fpdailyplanner.Sheets[0].AutoPostBack = true;
            Fpdailyplanner.Sheets[0].SheetName = " ";
            Fpdailyplanner.Sheets[0].SheetCorner.Columns[0].Visible = false;
            Fpdailyplanner.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            Fpdailyplanner.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
            Fpdailyplanner.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            Fpdailyplanner.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fpdailyplanner.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Fpdailyplanner.Sheets[0].DefaultStyle.Font.Bold = false;

            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Name = "Book Antiqua";
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = System.Drawing.Color.Black;
            style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpdailyplanner.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fpdailyplanner.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fpdailyplanner.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            Fpdailyplanner.Sheets[0].AllowTableCorner = true;

            Fpdailyplanner.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            Fpdailyplanner.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            Fpdailyplanner.Pager.Align = HorizontalAlign.Right;
            Fpdailyplanner.Pager.Font.Bold = true;
            Fpdailyplanner.Pager.Font.Name = "Book Antiqua";
            Fpdailyplanner.Pager.ForeColor = Color.DarkGreen;
            Fpdailyplanner.Pager.BackColor = Color.Beige;
            Fpdailyplanner.Pager.BackColor = Color.AliceBlue;
            Fpdailyplanner.Pager.PageCount = 5;
            Fpdailyplanner.CommandBar.Visible = false;

            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Fpdailyplanner.CommandBar.Visible = false;
            Fpdailyplanner.Visible = false;
            btnxl.Visible = false;
            lblnorec.Visible = false;
            errmsg.Visible = false;
            btnprintmaster.Visible = false;

            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddldegree.Items.Count > 0)
            {
                ddldegree.Enabled = true;
                ddlbranch.Enabled = true;
                ddlsemseter.Enabled = true;
                ddlsection.Enabled = true;
                btngo.Enabled = true;
                txtfrom.Enabled = true;
                txtto.Enabled = true;
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSem(strbranch, strbatch, collegecode);
                BindSectionDetail(strbatch, strbranch);
                GetSubject();
                load_staffname();
                txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
                ddlsemseter.Enabled = false;
                ddlsection.Enabled = false;
                btngo.Enabled = false;
                txtfrom.Enabled = false;
                txtto.Enabled = false;
            }

            // Added By Sridharan 12 Mar 2015
            //{
            string grouporusercodeschool = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercodeschool = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercodeschool = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercodeschool + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                if (schoolvalue.Trim() == "0")
                {
                    forschoolsetting = true;
                    //lblcollege.Text = "School";
                    lblbatch.Text = "Year";
                    lbldegree.Text = "School Type";
                    lblbranch.Text = "Standard";
                    lblsemester.Text = "Term";
                    //Label1.Text = "Test Mark R11-Continuous Assessment Report";
                    //lbldeg.Attributes.Add("Style", "font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 229px;    position: absolute;    top: 210px;");
                    //tbdeg.Attributes.Add("Style", "   font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 20px;    left: 328px;    margin-right: 15px;    position: absolute;    top: 210px;    width: 100px;");
                    //lblbranch.Attributes.Add("Style", " color: Black;    display: inline-block;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 439px;    position: absolute;    top: 212px;    width: 90px;");
                    //txtbranch.Attributes.Add("Style", "font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 20px;    left: 509px;    position: absolute;    top: 210px;    width: 180px;");
                    //lblsection.Attributes.Add("Style", " color: Black;    display: inline-block;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 702px;    position: absolute;    top: 211px;    width: 100px;");


                }
                else
                {
                    forschoolsetting = false;
                }
            }
            else
            {
                forschoolsetting = false;
            }

            //} Sridharan
        }
    }
    // Batch load function
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
                ddlbatch.Items[0].Selected = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    // Degree load function
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
                ddldegree.Items[0].Selected = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    // Branch load function-------

    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            ddlbranch.Items.Clear();

            course_id = ddldegree.SelectedValue.ToString();

            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
                ddlbranch.Items[0].Selected = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }


    // Semsetr Load Function
    public void BindSem(string strbranch, string strbatchyear, string collegecode)
    {

        try
        {
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsemseter.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsemseter.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemseter.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    // section laod function

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
                ddlsection.Items.Insert(0, "All");
                ddlsection.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }



    //Subject Load Function

    public void GetSubject()
    {
        try
        {
            string subjectquery = string.Empty;
            ddlsubject.Items.Clear();
            string sections = ddlsection.SelectedValue.ToString();
            string strsec = "";
            if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == "")
            {
                strsec = "";
            }
            else
            {
                strsec = " and exam_type.Sections='" + sections.ToString() + "'";
            }

            string sems = "";
            if (ddlsemseter.SelectedValue != "")
            {
                if (ddlsemseter.SelectedValue == "")
                {
                    sems = "";
                }
                else
                {
                    sems = "and SM.semester=" + ddlsemseter.SelectedValue.ToString() + "";
                }


                if (Session["Staff_Code"].ToString() == "")
                {
                    subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' order by S.subject_no ";
                }
                else if (Session["Staff_Code"].ToString() != "")
                {
                    subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "'  and st.staff_code='" + Session["Staff_Code"].ToString() + "'  order by S.subject_no ";
                }
                if (subjectquery != "")
                {
                    ds.Dispose();
                    ds.Reset();
                    ds = d2.select_method(subjectquery, hat, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlsubject.Enabled = true;
                        ddlsubject.DataSource = ds;
                        ddlsubject.DataValueField = "Subject_No";
                        ddlsubject.DataTextField = "Subject_Name";
                        ddlsubject.DataBind();
                        ddlsubject.Items.Insert(0, "All");
                    }
                    else
                    {
                        ddlsubject.Enabled = false;
                    }
                }
            }
            else
            {
                //ddlsubject.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //Staff Load Function
    public void load_staffname()
    {
        if (ddlsubject.Enabled == true)
        {
            string staffquery = "";
            string subjectcode = "";
            string section = "";
            if (ddlsection.SelectedValue.ToString() != "All")
            {
                section = "and s.sections='" + ddlsection.SelectedValue.ToString() + "'";
            }
            //Modified by srinath 28/01/2014
            if (ddlsubject.Text == "All")
            // if (ddlsubject.Text == "--Select--")
            {

                string sems = "";

                if (ddlsemseter.SelectedValue == "")
                {
                    sems = "";
                }
                else
                {
                    sems = "and SM.semester=" + ddlsemseter.SelectedValue.ToString() + "";
                }

                staffquery = "select distinct S.subject_no,subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and st.subject_no=s.subject_no  and  SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and S.subtype_no = Sem.subtype_no and promote_count=1   order by subject_code ";
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method(staffquery, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (subjectcode == "")
                        {
                            subjectcode = "'" + ds.Tables[0].Rows[i]["Subject_No"].ToString() + "'";
                        }
                        else
                        {

                            subjectcode = subjectcode + ',' + "'" + ds.Tables[0].Rows[i]["Subject_No"].ToString() + "'";
                        }
                    }
                }
                //staffquery = "select distinct m.staff_name,m.staff_code from staffmaster m,stafftrans t,staff_selector s where m.staff_code=t.staff_code and m.staff_code=s.staff_code and t.latestrec = 1 and s.batch_year=" + ddlbatch.SelectedValue.ToString() + "  " + section + " and t.dept_code=" + ddlbranch.SelectedValue.ToString() + " and  s.subject_no in(" + subjectcode + ")";
                string getsubject = "";
                if (subjectcode.Trim() != "" && subjectcode != "")
                {
                    getsubject = " and  s.subject_no in(" + subjectcode + ")";
                }

                //staffquery = "select distinct m.staff_name,m.staff_code from staffmaster m,stafftrans t,staff_selector s where m.staff_code=t.staff_code and m.staff_code=s.staff_code and t.latestrec = 1 and m.resign = 0 and m.settled = 0 and s.batch_year=" + ddlbatch.SelectedValue.ToString() + " " + section + " and  s.subject_no in(" + subjectcode + ")";
                staffquery = "select distinct m.staff_name,m.staff_code from staffmaster m,stafftrans t,staff_selector s where m.staff_code=t.staff_code and m.staff_code=s.staff_code and t.latestrec = 1 and m.resign = 0 and m.settled = 0 and s.batch_year=" + ddlbatch.SelectedValue.ToString() + " " + section + " " + getsubject + "";
            }
            else
            {
                if (ddlsubject.Text != "")
                {
                    //staffquery = "select distinct m.staff_name,m.staff_code from staffmaster m,stafftrans t,staff_selector s where m.staff_code=t.staff_code and m.staff_code=s.staff_code and t.latestrec = 1 and s.batch_year=" + ddlbatch.SelectedValue.ToString() + " and t.dept_code=" + ddlbranch.SelectedValue.ToString() + " " + section + " and  s.subject_no='" + ddlsubject.SelectedValue.ToString() + "'";
                    staffquery = "select distinct m.staff_name,m.staff_code from staffmaster m,stafftrans t,staff_selector s where m.staff_code=t.staff_code and m.staff_code=s.staff_code and t.latestrec = 1 and m.resign = 0 and m.settled = 0 and s.batch_year=" + ddlbatch.SelectedValue.ToString() + " " + section + " and  s.subject_no='" + ddlsubject.SelectedValue.ToString() + "'";
                }
                else
                {
                    staffquery = "select distinct m.staff_name,m.staff_code from staffmaster m,stafftrans t,staff_selector s where m.staff_code=t.staff_code and m.staff_code=s.staff_code and t.latestrec = 1 and m.resign = 0 and m.settled = 0 and s.batch_year=" + ddlbatch.SelectedValue.ToString() + " " + section + " ";
                }
            }

            ddlstaff.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method(staffquery, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstaff.DataSource = ds.Tables[0];
                ddlstaff.DataTextField = "Staff_name";
                ddlstaff.DataValueField = "Staff_code";
                ddlstaff.DataBind();
                ddlstaff.Items.Insert(0, "All");
                ddlstaff.Enabled = true;
            }
            else
            {
                ddlstaff.Enabled = false;
            }
        }
        else
        {
            ddlstaff.Enabled = false;
        }

    }


    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubject();
        load_staffname();
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {

        BindBranch(singleuser, group_user, course_id, collegecode, usercode);
        BindSem(strbranch, strbatch, collegecode);
        BindSectionDetail(strbatch, strbranch);
        GetSubject();
        load_staffname();
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSem(strbranch, strbatch, collegecode);
        BindSectionDetail(strbatch, strbranch);
        load_staffname();
        GetSubject();
    }
    protected void ddlsemseter_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSectionDetail(strbatch, strbranch);
        GetSubject();
        load_staffname();
    }


    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_staffname();
    }
    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_staffname();
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        string reportname = txtexcelname.Text;
        if (reportname.ToString().Trim() != "")
        {
            d2.printexcelreport(Fpdailyplanner, reportname.ToString().Trim());
        }
        else
        {
            lblnorec.Text = "Please Enter Your Report Name";
            lblnorec.Visible = true;
        }

    }
    protected void txtto_TextChanged(object sender, EventArgs e)
    {
        string[] splitfrom = txtfrom.Text.Split(new Char[] { '/' });
        string[] splitto = txtto.Text.Split(new char[] { '/' });
        string fdate = splitfrom[1] + '/' + splitfrom[0] + '/' + splitfrom[2];
        string tdate = splitto[1] + '/' + splitto[0] + '/' + splitto[2];
        DateTime fromdate = Convert.ToDateTime(fdate);
        DateTime todate = Convert.ToDateTime(tdate);
        if (fromdate > todate)
        {
            Fpdailyplanner.Visible = false;
            btnxl.Visible = false;
            errmsg.Text = "Please Enter To Date Grater Than From Date";
            errmsg.Visible = true;
        }
        else
        {
            errmsg.Visible = false;
        }
    }
    protected void txtfrom_TextChanged(object sender, EventArgs e)
    {
        string[] splitfrom = txtfrom.Text.Split(new Char[] { '/' });
        string[] splitto = txtto.Text.Split(new char[] { '/' });
        string fdate = splitfrom[1] + '/' + splitfrom[0] + '/' + splitfrom[2];
        string tdate = splitto[1] + '/' + splitto[0] + '/' + splitto[2];
        DateTime fromdate = Convert.ToDateTime(fdate);
        DateTime todate = Convert.ToDateTime(tdate);
        if (fromdate > todate)
        {
            Fpdailyplanner.Visible = false;
            btnxl.Visible = false;
            errmsg.Text = "Please Enter To Date Grater Than From Date";
            errmsg.Visible = true;
        }
        else
        {
            errmsg.Visible = false;
        }
    }
    #endregion

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
          
            txtexcelname.Text = "";
            Boolean rowglag = false;
            string[] splitfromcheck = txtfrom.Text.Split(new Char[] { '/' });
            string[] splittocheck = txtto.Text.Split(new char[] { '/' });
            string fdate = splitfromcheck[1] + '/' + splitfromcheck[0] + '/' + splitfromcheck[2];
            string tdate = splittocheck[1] + '/' + splittocheck[0] + '/' + splittocheck[2];
            DateTime fromdatechech = Convert.ToDateTime(fdate);
            DateTime todatecheck = Convert.ToDateTime(tdate);
            string gdate = string.Empty;

            if (fromdatechech > todatecheck)
            {
                Fpdailyplanner.Visible = false;
                btnxl.Visible = false;
                errmsg.Text = "Please Enter To Date Grater Than From Date";
                errmsg.Visible = true;
            }
            else
            {
                if (ddlstaff.Enabled != false && ddlsemseter.Enabled != false && ddlsubject.Enabled != false)
                {
                    Fpdailyplanner.Sheets[0].ColumnCount = 8;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnxl.Visible = true;
                    lblnorec.Visible = false;
                    Fpdailyplanner.Visible = true;
                    btnprintmaster.Visible = true;
                    Fpdailyplanner.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpdailyplanner.Sheets[0].ColumnHeader.Visible = true;
                    Fpdailyplanner.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Fpdailyplanner.Sheets[0].ColumnHeader.Rows[0].Height = 50;
                    Fpdailyplanner.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpdailyplanner.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                    Fpdailyplanner.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject";
                    Fpdailyplanner.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff";
                    Fpdailyplanner.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hour";
                    Fpdailyplanner.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Topic to be Covered";
                    Fpdailyplanner.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Topic Covered";
                    Fpdailyplanner.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Remark";
                    if (ddldaily.SelectedIndex != 1)
                    {
                        Fpdailyplanner.Sheets[0].ColumnCount++;
                        Fpdailyplanner.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Other Topic to be Covered";
                    }
                    Fpdailyplanner.Sheets[0].Columns[0].Width = 50;
                    Fpdailyplanner.Sheets[0].Columns[1].Width = 100;
                    Fpdailyplanner.Sheets[0].Columns[2].Width = 150;
                    Fpdailyplanner.Sheets[0].Columns[3].Width = 100;
                    Fpdailyplanner.Sheets[0].Columns[4].Width = 100;
                    Fpdailyplanner.Sheets[0].Columns[5].Width = 150;
                    Fpdailyplanner.Sheets[0].Columns[6].Width = 150;
                    //Fpdailyplanner.Sheets[0].Columns[7].Width = 100;
                    //Fpdailyplanner.Sheets[0].Columns[0].BackColor = Color.AliceBlue;
                    //Fpdailyplanner.Sheets[0].Columns[1].BackColor = Color.AliceBlue;
                    //Fpdailyplanner.Sheets[0].Columns[2].BackColor = Color.AliceBlue;
                    //Fpdailyplanner.Sheets[0].Columns[3].BackColor = Color.AliceBlue;
                    //Fpdailyplanner.Sheets[0].Columns[4].BackColor = Color.AliceBlue;
                    //Fpdailyplanner.Sheets[0].Columns[5].BackColor = Color.AliceBlue;
                    //Fpdailyplanner.Sheets[0].Columns[6].BackColor = Color.AliceBlue;
                    //Fpdailyplanner.Sheets[0].Columns[7].BackColor = Color.AliceBlue;

                    Fpdailyplanner.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    Fpdailyplanner.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                    Fpdailyplanner.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                    Fpdailyplanner.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                    Fpdailyplanner.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                    Fpdailyplanner.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                    Fpdailyplanner.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
                    Fpdailyplanner.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;

                    Fpdailyplanner.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpdailyplanner.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpdailyplanner.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpdailyplanner.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);


                    if (ddldaily.SelectedIndex == 1)
                    {
                        Fpdailyplanner.Sheets[0].Columns[5].Visible = true;
                        Fpdailyplanner.Sheets[0].Columns[6].Visible = false;
                    }
                    else if (ddldaily.SelectedIndex == 2)
                    {
                        Fpdailyplanner.Sheets[0].Columns[5].Visible = false;
                        Fpdailyplanner.Sheets[0].Columns[6].Visible = true;
                    }
                    else if (ddldaily.SelectedIndex == 0)
                    {
                        Fpdailyplanner.Sheets[0].Columns[5].Visible = true;
                        Fpdailyplanner.Sheets[0].Columns[6].Visible = true;
                    }

                    Fpdailyplanner.Sheets[0].RowCount = 0;
                    Fpdailyplanner.SaveChanges();

                    string batchyear = ddlbatch.SelectedValue.ToString();
                    string degree_code = ddlbranch.SelectedValue.ToString();
                    string semester = ddlsemseter.SelectedValue.ToString();
                    string staffcode = ddlstaff.SelectedValue.ToString();
                    string subjectcode = ddlsubject.SelectedValue.ToString();
                    string section = ddlsection.SelectedValue.ToString();
                    string strsection = "";

                    if (section == "All")
                    {
                        DataSet dssection = d2.BindSectionDetail(batchyear, degree_code);
                        if (dssection.Tables[0].Rows.Count > 0)
                        {
                            for (int sec = 0; sec < dssection.Tables[0].Rows.Count; sec++)
                            {
                                if (strsection == "")
                                {
                                    strsection = dssection.Tables[0].Rows[sec]["sections"].ToString();
                                }
                                else
                                {
                                    strsection = strsection + '\\' + dssection.Tables[0].Rows[sec]["sections"].ToString();
                                }
                            }
                        }
                        else
                        {
                            strsection = "";
                        }


                    }
                    else
                    {
                        strsection = "" + ddlsection.SelectedValue.ToString() + "";
                    }

                    string holydayquery = "select * from holidaystudents where degree_code=" + degree_code + " and semester=" + semester + "";
                    dsholyday.Dispose();
                    dsholyday.Reset();
                    dsholyday = d2.select_method(holydayquery, hat, "Text");

                    string totalhour = "select max(No_of_hrs_per_day) from PeriodAttndSchedule where  degree_code=" + degree_code + " and semester=" + semester + "";
                    int maxhour = Convert.ToInt32(d2.GetFunctionv(totalhour));

                    string[] from = txtfrom.Text.Split(new char[] { '/' });
                    string fromdate = from[1] + '/' + from[0] + '/' + from[2];

                    string[] to = txtto.Text.Split(new char[] { '/' });
                    string todate = to[1] + '/' + to[0] + '/' + to[2];

                    DateTime fromday1 = Convert.ToDateTime(fromdate);
                    DateTime today = Convert.ToDateTime(todate);
                    string classhour = "";
                    int sno = 0;
                    // int rowscheck = 0;
                    string sectionvalue = "";
                    for (DateTime caldate = fromday1; caldate <= today; caldate.AddDays(1))
                    {
                       
                        string[] caldtesplit = Convert.ToString(caldate).Split(' ');
                        string[] datesplit = Convert.ToString(caldtesplit[0]).Split('/');
                        string date = datesplit[1] + '/' + datesplit[0] + '/' + datesplit[2];
                        gdate = datesplit[2] + '-' + datesplit[0] + '-' + datesplit[1];
                        string querydate = Convert.ToString(caldtesplit[0]);
                        DataRow drholyday = dsholyday.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<DateTime>("holiday_date") == caldate);
                        if (drholyday == null)
                        {
                            string noofdays = "";
                            string start_datesem = "";
                            string start_dayorder = "";
                            string dayorderquery = "select s.start_date,s.starting_dayorder,p.nodays,p.schorder from periodattndschedule p,seminfo s where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code=" + degree_code + " and s.semester=" + semester + " and batch_year=" + batchyear + "";
                            DataSet dsdayorder = d2.select_method(dayorderquery, hat, "Text");
                            if (dsdayorder.Tables[0].Rows.Count > 0)
                            {
                                schorder = dsdayorder.Tables[0].Rows[0]["SchOrder"].ToString();
                                noofdays = dsdayorder.Tables[0].Rows[0]["nodays"].ToString();
                                start_datesem = dsdayorder.Tables[0].Rows[0]["start_date"].ToString();
                                start_dayorder = dsdayorder.Tables[0].Rows[0]["starting_dayorder"].ToString();
                            }
                            string dayget = "";
                            if (schorder == "1")
                            {
                                dayget = Convert.ToString(caldate.ToString("ddd"));
                            }
                            else
                            {
                                string[] startdatspilt = start_datesem.Split(' ');
                                start_datesem = startdatspilt[0].ToString();
                                dayget = findday(querydate.ToString(), degree_code, semester, batchyear, start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                            }
                            for (int i = 1; i <= maxhour; i++)
                            {
                                if (i == maxhour)
                                {
                                    classhour = classhour + dayget + i;
                                }
                                else
                                {
                                    if (i == 1)
                                    {
                                        classhour = dayget + i + ',';
                                    }
                                    else
                                    {
                                        classhour = classhour + dayget + i + ',';
                                    }
                                }
                            }
                            string[] sectionspilt = strsection.Split('\\');
                            for (int scet = 0; scet <= sectionspilt.GetUpperBound(0); scet++)
                            {
                                string chksectionvalue = sectionspilt[scet].ToString();

                                if (sectionspilt.GetUpperBound(0) > 0)
                                {
                                    Fpdailyplanner.Sheets[0].RowCount++;
                                    Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 1].Text = date.ToString();
                                    Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                    Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Large;
                                    Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 2].Text = "Batch : " + batchyear + " " + '-' + " Branch : " + ddlbranch.SelectedItem.ToString() + " - Sem : " + ddlsemseter.Text.ToString() + " " + '-' + " Section " + '-' + " " + chksectionvalue + " ";
                                    Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                    Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Large;
                                    Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 2].ForeColor = Color.Blue;
                                    Fpdailyplanner.Sheets[0].SpanModel.Add(Fpdailyplanner.Sheets[0].RowCount - 1, 2, 1, 6);
                                }
                                if (chksectionvalue == "")
                                {
                                    sectionvalue = "";
                                }
                                else
                                {
                                    sectionvalue = " and Sections='" + chksectionvalue.ToString() + "'";
                                }

                                //string shedulequery = "select top 1 " + classhour + ",batch_year,degree_code,semester,sections from semester_schedule where sections<>'-1' and batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " " + sectionvalue + " and fromdate <= '" + querydate.ToString() + "'  order by fromdate desc";
                                string shedulequery = "select top 1 " + classhour + ",batch_year,degree_code,semester,sections from semester_schedule where  batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " " + sectionvalue + " and fromdate <= '" + querydate.ToString() + "'  order by fromdate desc";
                                ds.Dispose();
                                ds.Reset();
                                ds = d2.select_method(shedulequery, hat, "Text");


                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        string[] classhourspilt = classhour.Split(new char[] { ',' });
                                        for (int colu = 0; colu <= classhourspilt.GetUpperBound(0); colu++)
                                        {
                                            string othertop = string.Empty;
                                            string othertops = string.Empty;
                                            string columnvalue = classhourspilt[colu].ToString();
                                            string classhour1 = "";
                                            string altershedulequery = "select " + columnvalue + " from Alternate_schedule where batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " " + sectionvalue + " and fromdate= '" + querydate.ToString() + "'";
                                            DataSet dsaltershudel = d2.select_method(altershedulequery, hat, "Text");
                                            if (dsaltershudel.Tables[0].Rows.Count > 0)
                                            {
                                                classhour1 = dsaltershudel.Tables[0].Rows[0]["" + columnvalue + ""].ToString();
                                            }
                                            if (classhour1 == "")
                                            {
                                                classhour1 = ds.Tables[0].Rows[i]["" + columnvalue + ""].ToString();
                                            }
                                            if (classhour1.ToString().Trim() != "")
                                            {
                                                string[] splitcode = classhour1.Split(';');
                                                for (int k = 0; k <= splitcode.GetUpperBound(0); k++)
                                                {
                                                    string staffcodecheck = splitcode[k].ToString();
                                                    string[] staffcodesubject = staffcodecheck.Split('-');
                                                    string tempstaffcode = staffcodesubject[1].ToString();
                                                    string tempsubject = staffcodesubject[0].ToString();

                                                    string staffquery = "";
                                                    string sectionstraff = "";

                                                    if (sectionvalue == "")
                                                    {
                                                        sectionstraff = "";
                                                    }
                                                    else
                                                    {
                                                        sectionstraff = "and st.sections='" + chksectionvalue.ToString() + "'";
                                                    }
                                                    // if (ddlsubject.Text == "--Select--" && ddlstaff.Text == "All")
                                                    if (ddlsubject.Text == "All" && ddlstaff.Text == "All")
                                                    {

                                                        staffquery = "select distinct S.subject_no,subject_code,subject_name,st.staff_code from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code  and st.subject_no=s.subject_no   and S.subtype_no = Sem.subtype_no and promote_count=1 and SM.degree_code=" + degree_code + " and  SM.batch_year=" + batchyear + " and SM.semester=" + semester + " " + sectionstraff + "  order by subject_code";
                                                    }
                                                    else if (ddlsubject.Text != "All" && ddlstaff.Text == "All")
                                                    {

                                                        staffquery = "select distinct S.subject_no,subject_code,subject_name,st.staff_code from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code  and st.subject_no=s.subject_no   and S.subtype_no = Sem.subtype_no and promote_count=1 and SM.degree_code=" + degree_code + " and  SM.batch_year=" + batchyear + " and SM.semester=" + semester + " " + sectionstraff + " and s.subject_no='" + ddlsubject.SelectedValue.ToString() + "' order by subject_code";
                                                    }
                                                    else if (ddlsubject.Text == "All" && ddlstaff.Text != "All")
                                                    {
                                                        staffquery = "select distinct S.subject_no,subject_code,subject_name,st.staff_code from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code  and st.subject_no=s.subject_no   and S.subtype_no = Sem.subtype_no and promote_count=1 and SM.degree_code=" + degree_code + " and  SM.batch_year=" + batchyear + " and SM.semester=" + semester + " " + sectionstraff + "  and st.staff_code='" + staffcode + "' order by subject_code";
                                                    }
                                                    else if (ddlsubject.Text != "All" && ddlstaff.Text != "All")
                                                    {
                                                        staffquery = "select distinct S.subject_no,subject_code,subject_name,st.staff_code from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code  and st.subject_no=s.subject_no   and S.subtype_no = Sem.subtype_no and promote_count=1 and SM.degree_code=" + degree_code + " and  SM.batch_year=" + batchyear + " and SM.semester=" + semester + " " + sectionstraff + " and s.subject_no='" + ddlsubject.SelectedValue.ToString() + "' and st.staff_code='" + staffcode + "' order by subject_code";
                                                    }
                                                    dsstaff = d2.select_method(staffquery, hat, "Text");
                                                    if (dsstaff.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int staff = 0; staff < dsstaff.Tables[0].Rows.Count; staff++)
                                                        {
                                                            staffcode = dsstaff.Tables[0].Rows[staff]["Staff_code"].ToString();
                                                            subjectcode = dsstaff.Tables[0].Rows[staff]["Subject_no"].ToString();

                                                            string subjstaff = subjstaff = staffcode + '/' + subjectcode;
                                                            string tempsubjstaff = tempstaffcode + '/' + tempsubject;
                                                            if (subjstaff == tempsubjstaff)
                                                            {
                                                                string subjectname = "";
                                                                string staffname = "";
                                                                ds2.Reset();
                                                                ds2.Dispose();
                                                                ds2 = d2.select_method("select Subject_name from subject where subject_no='" + tempsubject + "'", hat, "Text");
                                                                if (ds2.Tables[0].Rows.Count > 0)
                                                                {

                                                                    subjectname = ds2.Tables[0].Rows[0]["Subject_name"].ToString();
                                                                }
                                                                ds2.Reset();
                                                                ds2.Dispose();

                                                                ds2 = d2.select_method("select staff_name from staffmaster where staff_code='" + tempstaffcode + "'", hat, "Text");
                                                                if (ds2.Tables[0].Rows.Count > 0)
                                                                {
                                                                    staffname = ds2.Tables[0].Rows[0]["staff_name"].ToString();
                                                                }

                                                                string hr = "";
                                                                for (int spilt = 3; spilt < columnvalue.Length; spilt++)
                                                                {
                                                                    hr = columnvalue[spilt].ToString();
                                                                }
                                                                string dailytopicname = "";
                                                                string topicname = "";
                                                                string sectiontopic = ddlsection.Text;
                                                                string plannersec = "";
                                                                if (sectiontopic != "All")
                                                                {
                                                                    plannersec = "and p.sections='" + ddlsection.SelectedValue.ToString() + "'";
                                                                }

                                                                if (ddldaily.SelectedIndex != 2)
                                                                {

                                                                    ds2.Reset();
                                                                    ds2.Dispose();
                                                                    string unitname = "";
                                                                    string topicquery = "select topics from lesson_plan p,lessonplantopics l where l.lp_code=p.lp_code  and  sch_date='" + querydate.ToString() + "'  and l.staff_code='" + tempstaffcode + "' and l.hr=" + hr + " " + plannersec + "";
                                                                    ds2 = d2.select_method(topicquery, hat, "Text");
                                                                    if (ds2.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        unitname = ds2.Tables[0].Rows[0]["topics"].ToString();
                                                                    }
                                                                    if (unitname != "")
                                                                    {
                                                                        string[] unitname1;
                                                                        string unitnamespilt;
                                                                        unitname1 = unitname.Split('/');
                                                                        for (int j = 0; j <= unitname1.GetUpperBound(0); j++)
                                                                        {
                                                                            rowglag = true;
                                                                            unitnamespilt = unitname1[j];
                                                                            string unitquery = "select unit_name from sub_unit_details where topic_no='" + unitnamespilt + "'";
                                                                            ds2.Dispose();
                                                                            ds2.Reset();
                                                                            ds2 = d2.select_method(unitquery, hat, "Text");
                                                                            if (ds2.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                if (topicname == "")
                                                                                {
                                                                                    topicname = ds2.Tables[0].Rows[0]["unit_name"].ToString();
                                                                                }
                                                                                else
                                                                                {
                                                                                    topicname = topicname + " / " + ds2.Tables[0].Rows[0]["unit_name"].ToString();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (ddldaily.SelectedIndex != 1)
                                                                {
                                                                    string dailysec = "";
                                                                    if (sectiontopic != "All")
                                                                    {
                                                                        dailysec = "and ds.sections='" + ddlsection.SelectedValue.ToString() + "'";
                                                                    }

                                                                    ds2.Reset();
                                                                    ds2.Dispose();
                                                                    string dailyunitname = "";
                                                                    int hrvalue = colu + 1;

                                                                    string dailytopicquery = "select topics from dailyEntdet de,dailyStaffEntry ds where de.lp_code=ds.lp_code and ds.sch_date='" + querydate.ToString() + "'  and de.staff_code='" + tempstaffcode + "' " + dailysec + " and hr=" + hrvalue + "";
                                                                    ds2 = d2.select_method(dailytopicquery, hat, "Text");
                                                                    if (ds2.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        dailyunitname = ds2.Tables[0].Rows[0]["topics"].ToString();
                                                                    }
                                                                    if (dailyunitname != "")
                                                                    {
                                                                        string[] dailyunitname1;
                                                                        string dailyunitnamespilt;
                                                                        dailyunitname1 = dailyunitname.Split('/');
                                                                        for (int j = 0; j <= dailyunitname1.GetUpperBound(0); j++)
                                                                        {
                                                                            rowglag = true;
                                                                            dailyunitnamespilt = dailyunitname1[j];
                                                                            string unitquery = "select unit_name from sub_unit_details where topic_no='" + dailyunitnamespilt + "'";
                                                                            ds2.Dispose();
                                                                            ds2.Reset();
                                                                            ds2 = d2.select_method(unitquery, hat, "Text");
                                                                            if (ds2.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                if (dailytopicname == "")
                                                                                {
                                                                                    dailytopicname = ds2.Tables[0].Rows[0]["unit_name"].ToString();
                                                                                }
                                                                                else
                                                                                {
                                                                                    dailytopicname = dailytopicname + " / " + ds2.Tables[0].Rows[0]["unit_name"].ToString();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                 
                                                                   string  othertopic = "select topic_name from dailyEntdet de,dailyStaffEntry ds,dailyEntryother do where de.lp_code=ds.lp_code and ds.sch_date='" + querydate.ToString() + "'  and de.staff_code='" + tempstaffcode + "' " + dailysec + " and hr=" + hrvalue + " and do.subpk=ds.othersub";

                                                                   DataSet otherds2 = d2.select_method(othertopic, hat, "Text");
                                                                  
                                                                   if (otherds2.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        if (othertop=="")
                                                                          othertop = otherds2.Tables[0].Rows[0]["topic_name"].ToString();
                                                                        else
                                                                            othertop = othertop + " / " + otherds2.Tables[0].Rows[0]["topic_name"].ToString();
                                                                       
                                                                    }
                                                                   
                                                                   

                                                                }
                                                                sno++;
                                                                string[] datespilt = Convert.ToString(caldate).Split(' ');
                                                                string[] date1 = datespilt[0].Split('/');
                                                                string arrangedate = date1[1] + '/' + date1[0] + '/' + date1[2];

                                                                Fpdailyplanner.Sheets[0].RowCount++;
                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 1].Text = arrangedate;
                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Large;
                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 2].Text = subjectname;
                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 3].Text = staffname;
                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 4].Text = hr.ToString();
                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 5].Text = topicname.ToString();
                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 6].Text = dailytopicname.ToString();

                                                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 8].Text = othertop.ToString();

                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }

                                    }

                                }
                            }
                            classhour = "";
                        }
                        else
                        {
                            string holydayresonquery = "select holiday_date,holiday_desc from holidaystudents where degree_code=" + degree_code + " and semester=" + semester + " AND holiday_date='" + caldate.ToString() + "'";
                            DataSet dsholydayres = new DataSet();
                            dsholydayres = d2.select_method(holydayresonquery, hat, "Text");
                            if (dsholydayres.Tables[0].Rows.Count > 0)
                            {
                                string holudayreson = dsholydayres.Tables[0].Rows[0]["holiday_desc"].ToString();
                                Fpdailyplanner.Sheets[0].RowCount++;
                                string[] datespilt = Convert.ToString(caldate).Split(' ');
                                string[] date1 = datespilt[0].Split('/');
                                string arrangedate = date1[1] + '/' + date1[0] + '/' + date1[2];
                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 0].Text = " " + arrangedate + " is " + holudayreson + "";
                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpdailyplanner.Sheets[0].Cells[Fpdailyplanner.Sheets[0].RowCount - 1, 0].ForeColor = Color.Red;
                                Fpdailyplanner.Sheets[0].SpanModel.Add(Fpdailyplanner.Sheets[0].RowCount - 1, 0, 1, 8);
                            }
                        }
                        caldate = caldate.AddDays(1);
                    }

                    if (sno == 0)
                    {
                        Fpdailyplanner.Visible = false;
                        btnxl.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Records Found";
                        btnprintmaster.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                    }

                    int rowcount = Fpdailyplanner.Sheets[0].RowCount;
                    Fpdailyplanner.Height = 300;
                    Fpdailyplanner.Sheets[0].PageSize = 25 + (rowcount * 20);
                    Fpdailyplanner.SaveChanges();
                }
                else
                {
                    Fpdailyplanner.Visible = false;
                    btnxl.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Records Found";
                }

            }
            if (rowglag == false)
            {
                Fpdailyplanner.Visible = false;
                btnxl.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "No Records Found";
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }
    public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    {
        int holiday = 0;
        if (no_days == "")
            return "";
        if (sdate != "")
        {
            string[] sp_date = sdate.Split(new Char[] { '/' });
            string start_date = sp_date[1].ToString() + "-" + sp_date[2].ToString() + "-" + sp_date[0].ToString();
            DateTime dt1 = Convert.ToDateTime(sdate);
            DateTime dt2 = Convert.ToDateTime(curday);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*)as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";
            string holday = d2.GetFunction(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);
            int dif_days = ts.Days;
            int nodays = Convert.ToInt32(no_days);
            int order = (dif_days - holiday) % nodays;
            order = order + 1;

            //-----------------------------------------------------------     

            if (stastdayorder.ToString().Trim() != "")
            {
                if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
                {
                    order = order + (Convert.ToInt16(stastdayorder) - 1);
                    if (order == (nodays + 1))
                        order = 1;
                    else if (order > nodays)
                        order = order % nodays;
                }
            }
            //-----------------------------------------------------------


            string findday = "";
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";

            Day_Order = Convert.ToString(order) + "-" + Convert.ToString(findday);
            return findday;
        }
        else
            return "";

    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = Convert.ToString(Fpdailyplanner.ColumnHeader.RowCount);
        string degreedetails = "Daily Entry and Lesson Planner Report@Degree : " + ddlbatch.SelectedItem.Text.ToString() + "-" + ddldegree.SelectedItem.Text.ToString() + "- " + ddlbranch.SelectedItem.Text.ToString() + "-" + ddlsemseter.SelectedItem.Text.ToString() + "-" + ddlsection.SelectedItem.Text.ToString() + "@" + "Date : " + txtfrom.Text.ToString() + " - " + txtto.Text.ToString();
        string pagename = "StudentTestReport.aspx";
        Printcontrol.loadspreaddetails(Fpdailyplanner, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    
}
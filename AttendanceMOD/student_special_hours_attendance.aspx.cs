using System;//=============modified on 28/2/12(remov "select all"), 28/2/12(tot P&A), 29/2/12(SlipList),(spread width)
using System.Collections;
using System.Configuration;
//--------------07/3/12(select all for NE values ly)
//-----------21.05.12 modified(add link button in source) mythili,27/7/12(group user added (prabha)),28/7/12(attnd for subject alloted student only, subject filter(ddl))
//=================30/7/12(back link,attnd rights)
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;

public partial class student_special_hours_attendance : System.Web.UI.Page
{
    #region Initialization
    SqlConnection dc_con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection dc_con1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection mysql1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    InsproDirectAccess dir = new InsproDirectAccess();
    SqlCommand cmd = new SqlCommand();
    SqlCommand cmd_sem_shed;

    Hashtable hat = new Hashtable();
    Hashtable present_calcflag = new Hashtable();
    Hashtable absent_calcflag = new Hashtable();

    DataSet ds_attndmaster = new DataSet();
    DataSet ds = new DataSet();

    DAccess2 d2 = new DAccess2();
    DAccess2 dacces2 = new DAccess2();

    int count_master = 0;
    int no_hrs = 0, nodays = 0, temp_hr = 0, strdate = 0;
    int present_count = 0, absent_count = 0, colcnt = 0;
    int flag;
    int Att_mark_row;
    int Att_mark_column;

    string strsec = string.Empty;
    string single_user = string.Empty;
    string group_code = string.Empty;
    string no_of_hrs = string.Empty;
    string sch_order = string.Empty;
    string srt_day = string.Empty;
    string startdate = string.Empty;
    string no_days = string.Empty;
    string date_txt = string.Empty;
    string sem_sched = string.Empty;
    string subject_no = string.Empty;
    string Att_dcolumn = string.Empty;
    string Att_strqueryst = string.Empty;
    string strdayflag;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string staffcode = string.Empty;
    string Att_mark = string.Empty;
    string roll_indiv = string.Empty;

    bool flag_true = false;
    bool cellclick = false;
    bool cellclick1 = false;
    bool colhead = false;
    bool serialflag = false;
    bool update_flag = false;
    bool nullflag = false;
    #endregion

    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();

    public DataSet Bind_Degree(string college_code, string user_code)
    {
        DataSet ds = new DataSet();
        single_user = GetFunction("select singleuser from usermaster where user_code='" + user_code + "'");
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        if (single_user == "1" || single_user == "true" || single_user == "TRUE" || single_user == "True")
        {
            SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }
        else
        {
            // group_code = GetFunction("select group_code from usermaster where user_code="+user_code+"");
            if (group_code.Trim() != "")
            {
                SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_code + "", dcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
        }
        return ds;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        group_code = Session["group_code"].ToString();
        if (group_code.Contains(';'))
        {
            string[] group_semi = group_code.Split(';');
            group_code = group_semi[0].ToString();
        }
        if (!IsPostBack)
        {
            txtFromDate.Attributes.Add("readonly", "readonly");
            TxtToDate.Attributes.Add("readonly", "readonly");
            ddl_select_subj.Items.Clear();
            ddl_select_subj.Visible = false;
            lbl_subj_select.Visible = false;
            ddl_select_hour.Items.Clear();
            ddl_select_hour.Visible = false;
            lblSelectHour.Visible = false;

            txtFromDate.Text = DateTime.Today.ToString("d-MM-yyyy");
            TxtToDate.Text = DateTime.Today.ToString("d-MM-yyyy");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["Sex"] = "0";
            Session["flag"] = "-1";
            FpSpread2.Sheets[0].FrozenRowCount = 1;
            FpSpread2.Sheets[0].Columns.Default.Font.Name = "Book Antiqua";
            // FpSpread2.Sheets[0].Columns.Default.Font.Bold = true;
            FpSpread2.Sheets[0].Columns.Default.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Rows.Default.Font.Name = "Book Antiqua";
            // FpSpread2.Sheets[0].Rows.Default.Font.Bold = true;
            FpSpread2.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
            MyStyle.Font.Size = 12;
            MyStyle.Font.Bold = true;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            FpSpread2.SheetCorner.Rows.Default.Font.Size = FontUnit.Medium;
            FpSpread2.SheetCorner.Rows.Default.Font.Name = "Book Antiqua";
            FpSpread2.SheetCorner.Rows.Default.Font.Bold = true;
            FpSpread2.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(MyStyle);
            FpSpread2.Sheets[0].AllowTableCorner = true;
            FpSpread2.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns.Default.HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpSpread2.Sheets[0].Columns.Default.HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread2.ActiveSheetView.SheetCorner.DefaultStyle.BackColor = FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.BackColor;
            FpSpread2.RowHeader.Width = 50;
            FpSpread2.CommandBar.Visible = false;
            /// pnl_sliplist.Visible = false;     
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
            }
            else
            {
                grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string Master = string.Empty;
            SqlDataReader mtrdr;
            SqlCommand mtcmd;
            Master = "select * from Master_Settings where " + grouporusercode + "";
            mysql.Open();
            mtcmd = new SqlCommand(Master, mysql);
            mtrdr = mtcmd.ExecuteReader();
            string regularflag = string.Empty;

            while (mtrdr.Read())
            {
                if (mtrdr.HasRows == true)
                {
                    if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (mtrdr["settings"].ToString() == "Student_Type" && mtrdr["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                    if (mtrdr["settings"].ToString() == "sex" && mtrdr["value"].ToString() == "1")
                    {
                        Session["Sex"] = "1";
                    }
                    if (mtrdr["settings"].ToString() == "General attend" && mtrdr["value"].ToString() == "1")
                    {
                        //  option.SelectedValue = "1";
                    }
                    if (mtrdr["settings"].ToString() == "Absentees" && mtrdr["value"].ToString() == "1")
                    {
                        ///option.SelectedValue = "2";
                        //PanelindBody.Visible = true;
                    }
                    if (mtrdr["settings"].ToString() == "RollNo" && mtrdr["value"].ToString() == "1")
                    {
                        ///RadioButtonList1.SelectedValue = "1";
                    }
                    if (mtrdr["settings"].ToString() == "RegisterNo" && mtrdr["value"].ToString() == "1")
                    {
                        // RadioButtonList1.SelectedValue = "2";
                    }
                    if (mtrdr["settings"].ToString() == "Admission No" && mtrdr["value"].ToString() == "1")
                    {
                        ///RadioButtonList1.SelectedValue = "3";
                    }
                    if (mtrdr["settings"].ToString() == "General" && mtrdr["value"].ToString() == "1")
                    {
                        Session["flag"] = 0;
                    }
                    if (mtrdr["settings"].ToString() == "As Per Lesson" && mtrdr["value"].ToString() == "1")
                    {
                        Session["flag"] = 1;
                    }
                    if (mtrdr["settings"].ToString() == "Male" && mtrdr["value"].ToString() == "1")
                    {
                        genderflag = " and (a.sex='0'";
                    }
                    if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                    {
                        if (genderflag != "" && genderflag != "\0")
                        {
                            genderflag = genderflag + " or a.sex='1'";
                        }
                        else
                        {
                            genderflag = " and (a.sex='1'";
                        }
                    }
                    if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                    {
                        strdayflag = " and (registration.Stud_Type='Day Scholar'";
                    }
                    if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                    {
                        if (strdayflag != null && strdayflag != "\0")
                        {
                            strdayflag = strdayflag + " or registration.Stud_Type='Hostler'";
                        }
                        else
                        {
                            strdayflag = " and (registration.Stud_Type='Hostler'";
                        }
                    }
                    if (mtrdr["settings"].ToString() == "Regular")
                    {
                        regularflag = "and ((registration.mode=1)";
                        // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                    }
                    if (mtrdr["settings"].ToString() == "Lateral")
                    {
                        if (regularflag != "")
                        {
                            regularflag = regularflag + " or (registration.mode=3)";
                        }
                        else
                        {
                            regularflag = regularflag + " and ((registration.mode=3)";
                        }
                        //Session["strvar"] = Session["strvar"] + " and (mode=3)";
                    }
                    if (mtrdr["settings"].ToString() == "Transfer")
                    {
                        if (regularflag != "")
                        {
                            regularflag = regularflag + " or (registration.mode=2)";
                        }
                        else
                        {
                            regularflag = regularflag + " and ((registration.mode=2)";
                        }
                        //Session["strvar"] = Session["strvar"] + " and (mode=2)";
                    }
                }
            }
            mtrdr.Close();
            mysql.Close();
            if (strdayflag != null && strdayflag != "")
            {
                strdayflag = strdayflag + ")";
            }
            Session["strvar"] = strdayflag;
            if (regularflag != "")
            {
                regularflag = regularflag + ")";
            }
            if (genderflag != "")
            {
                genderflag = genderflag + ")";
            }
            Session["strvar"] = Session["strvar"] + regularflag + genderflag;
            FpSpread2.Sheets[0].FrozenRowCount = 1;
            //
            Printcontrol.Visible = false;
            //batch
            cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''order by batch_year", con);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            ddlbatch.DataSource = ds1;
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();
            //ddlBatch.Items.Insert(0, new ListItem("--Select--", "-1"));
            string batchcount = ddlbatch.Items.Count.ToString();
            int batch = 0;
            if (int.TryParse(batchcount, out batch))
                batch = batch - 1;
            ddlbatch.SelectedIndex = batch;
            //course
            con.Open();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            DataSet ds = Bind_Degree(collegecode.ToString(), usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataValueField = "course_id";
                ddldegree.DataTextField = "course_name";
                ddldegree.DataBind();
                //ddlDegree.Items.Insert(0, new ListItem("--Select--", "-1"));
                con.Close();
            }
            con.Close();
            con.Open();
            //cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddldegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
            //SqlDataAdapter daBRANCH = new SqlDataAdapter(cmd);
            //DataSet dsbranch = new DataSet();
            //daBRANCH.Fill(dsbranch);
            string course_id = ddldegree.SelectedValue.ToString();
            if (course_id != null && course_id != "")
            {
                DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode);
                ddlbranch.DataSource = dsbranch;
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataBind();
                con.Close();
                Btngo.Enabled = true;
                //bind semester
                bindsem();
                //bind section
                // BindSectionDetail();
            }
            else
            {
                Btngo.Enabled = false;
                lblset.Visible = true;
                ddl_select_subj.Visible = false;
                lbl_subj_select.Visible = false;
                ddl_select_hour.Visible = false;
                lblSelectHour.Visible = false;
                lblset.Text = "No Degree Rights For This User";
            }
        }
    }

    public void bindsem()
    {
        //--------------------semester load
        ddlsem.Items.Clear();
        bool first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            first_year = Convert.ToBoolean(dr[1].ToString());
            duration = Convert.ToInt16(dr[0].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlsem.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlsem.Items.Add(i.ToString());
                }
            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            con.Close();
            con.Open();
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlsem.Items.Clear();
            dr1 = cmd.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr1[1].ToString());
                duration = Convert.ToInt16(dr1[0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }
            dr1.Close();
        }
        if (ddlsem.Items.Count > 0)
        {
            ddlsem.SelectedIndex = 0;
            BindSectionDetail();
        }
        //FpMarkEntry.Visible = false;
        con.Close();
    }

    public void BindSectionDetail()
    {
        //string branch = ddlbranch.SelectedValue.ToString();
        //string batch = ddlbatch.SelectedValue.ToString();
        //DataSet ds = ClsAttendanceAccess.GetsectionDetail(batch.ToString(), branch.ToString());
        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        //    ddlsec.DataSource = ds;
        //    ddlsec.DataTextField = "sections";
        //    ddlsec.DataValueField = "sections";
        //    ddlsec.DataBind();
        //    if (ddlsec.Items.Count > 0)
        //    {
        //        if (ddlsec.Items[0].Text != "")
        //        {
        //            ddlsec.Items.Insert(0, new ListItem("--Select--", "-1"));
        //            ddlsec.Enabled = true;
        //        }
        //        else
        //            ddlsec.Enabled = false;BindSectionDetailBindSectionDetailBindSectionDetail
        //    }
        //}
        string branch = ddlbranch.SelectedValue.ToString();
        string batch = ddlbatch.SelectedValue.ToString();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        //added by annyutha*3rd sep 2014***//
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlsec.DataSource = ds;
            ddlsec.DataTextField = "sections";
            ddlsec.DataValueField = "sections";
            ddlsec.DataBind();
        }
        ddlsec.Items.Insert(0, "All");
        //end*//
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
            {
                ddlsec.Enabled = false;
            }
            else
            {
                ddlsec.Enabled = true;
            }
        }
        else
        {
            ddlsec.Enabled = false;
        }
    }

    public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    {
        //SqlCommand cmd;
        //DataSet ds = new DataSet();
        //SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        //dcon.Close();
        //dcon.Open();
        //if (single_user == "1" || single_user == "true" || single_user == "TRUE" || single_user == "True")
        //{
        //     cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        //     SqlDataAdapter da = new SqlDataAdapter(cmd);           
        //     da.Fill(ds);
        //}
        //else
        //{
        //    if (group_code.Trim() != "")
        //    {
        //        cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_code + "", dcon);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);             
        //        da.Fill(ds);
        //    }
        //}
        //return ds;
        hat.Clear();
        string usercode = Session["usercode"].ToString();
        string collegecode = Session["collegecode"].ToString();
        string singleuser = Session["single_user"].ToString();
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_code);
        hat.Add("course_id", ddldegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds = d2.select_method("bind_branch", hat, "sp");
        return ds;
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  load_spread();
        string course_id = ddldegree.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["UserCode"].ToString();
        //if (ddldegree.SelectedIndex > 0)
        //{
        //    DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        ddlbranch.DataSource = ds;
        //        ddlbranch.DataTextField = "Dept_Name";
        //        ddlbranch.DataValueField = "degree_code";
        //        ddlbranch.DataBind();
        //       // ddlbranch.Items.Insert(0, new ListItem("--Select--", "-1"));
        //    }
        //}
        con.Open();
        //cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddldegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
        //SqlDataAdapter daBRANCH = new SqlDataAdapter(cmd);
        //DataSet dsbranch = new DataSet();
        //daBRANCH.Fill(dsbranch);
        //string course_id = ddlDegree.SelectedValue.ToString();
        ddlbranch.Items.Clear();
        if (course_id != null && course_id != "")
        {
            DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode);
            ddlbranch.DataSource = dsbranch;
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataBind();
            con.Close();
            Btngo.Enabled = true;
            bindsem();
            //bind section
            BindSectionDetail();
        }
        else
        {
            Btngo.Enabled = false;
            lblset.Visible = true;
            ddl_select_subj.Visible = false;
            lbl_subj_select.Visible = false;
            ddl_select_hour.Visible = false;
            lblSelectHour.Visible = false;
            lblset.Text = "No Degree Rights For This User";
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //lblnorec.Visible = false;
        //  load_spread();
        bindsem();
        if (!Page.IsPostBack == false)
        {
            ddlsem.Items.Clear();
        }
        try
        {
            //if ((ddlBranch.SelectedIndex != 0) && (ddlBranch.SelectedIndex > 0))
            //{
            bindsem();
            //}
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!Page.IsPostBack == false)
        {
            ddlsec.Items.Clear();
        }
        Btngo.Visible = true;
        //btnok.Visible = false;
        BindSectionDetail();
        string collegecode = Convert.ToString(Session["collegecode"]).Trim();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        getsql.Close();
        getsql.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, getsql);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = getsql;
        drnew = cmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }

    protected void Btngo_Click(object sender, EventArgs e)
    {
        if (ddl_select_subj.Items.Count > 0)
        {
            if (ddl_select_subj.SelectedValue.Trim().ToLower() == "all")
                go_click("0");
            else
                go_click("1");
        }
        else
        {
            go_click("0");
        }
        FpSpread2.Sheets[0].AutoPostBack = false;
    }

    public void go_click(string tag_filter)
    {
        try
        {
            Printcontrol.Visible = false;
            Hashtable has_stud_list = new Hashtable();
            Hashtable has_subj = new Hashtable();
            ArrayList Arrlist_subjno = new ArrayList();
            lblset.Text = string.Empty;
            datelbl.Text = string.Empty;
            FpSpread2.Visible = false;
            pHeaderatendence.Visible = true;
            pBodyatendence.Visible = true;
            lblfromdate.Visible = false;
            lbltodate.Visible = false;
            lblset.Visible = false;
            lblother.Visible = false;
            ddl_select_subj.Visible = false;
            lbl_subj_select.Visible = false;
            ddl_select_hour.Visible = false;
            lblSelectHour.Visible = false;
            LabelE.Visible = false;
            serialflag = false;
            string sec = "", subject_name = "", subject_filter_tag = string.Empty;
            bool flag_save = false;
            bool flag_update = false;
            lblspecial.Visible = false; ;
            if (tag_filter == "1")
            {
                subject_filter_tag = " and subject_no=" + ddl_select_subj.SelectedValue.ToString();
            }
            else
            {
                ddl_select_subj.Items.Clear();
                subject_filter_tag = string.Empty;
            }
            if (staffcode == "" || staffcode == null)
            {
                FpSpread2.Visible = true;
                if (txtFromDate.Text != "")
                {
                    if (TxtToDate.Text != "")
                    {
                        lblset.Visible = false;
                        datelbl.Visible = false;
                        Buttonsave.Visible = true;
                        Buttonupdate.Visible = false;
                        string strsec = string.Empty;
                        FpSpread2.Sheets[0].Visible = true;
                        if (ddlsec.Text.ToString().Trim().ToLower() != "all" && ddlsec.Text.ToString().Trim() != "" && ddlsec.Text.ToString().Trim() != null && ddlsec.Enabled == true)
                        {
                            strsec = " and registration.sections='" + ddlsec.SelectedValue.ToString() + "'";
                            sec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                        }
                        else
                        {
                            strsec = string.Empty;
                            sec = string.Empty;
                        }
                        string date1 = string.Empty;
                        string date2 = string.Empty;
                        string datefrom;
                        string dateto = string.Empty;
                        date1 = txtFromDate.Text.ToString();
                        string[] split = date1.Split(new Char[] { '-' });
                        datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
                        date2 = TxtToDate.Text.ToString();
                        string[] split1 = date2.Split(new Char[] { '-' });
                        dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
                        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                        DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                        TimeSpan t = dt2.Subtract(dt1);
                        long days = t.Days;
                        if (days < 0)
                        {
                            datelbl.Visible = true;
                            datelbl.Text = "From date should be less than To date";
                            return;
                        }
                        if (dt1 > DateTime.Today)
                        {
                            datelbl.Visible = true;
                            datelbl.Text = "You can not mark attendance for the date greater than today";
                            txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                            return;
                        }
                        else
                        {
                            datelbl.Visible = false;
                        }
                        if (dt2 > DateTime.Today)
                        {
                            datelbl.Visible = true;
                            datelbl.Text = "You can not mark attendance for the date greater than today";
                            TxtToDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                            return;
                        }
                        else
                        {
                            datelbl.Visible = false;
                        }
                        string SelhrdetNo = string.Empty;
                        DataTable dthrDetNo = new DataTable();

                        staffcode = Convert.ToString(Session["staff_code"]).Trim();
                        if (!string.IsNullOrEmpty(Convert.ToString(ddlbatch.SelectedItem.Value)) && !string.IsNullOrEmpty(Convert.ToString(ddlbranch.SelectedItem.Value)) && !string.IsNullOrEmpty(Convert.ToString(ddlsem.SelectedItem.Value)))
                        {
                            SelhrdetNo = "select sd.hrdet_no from specialhr_master sm,specialhr_details sd where sm.hrentry_no=sd.hrentry_no and sm.date  between '" + dt1 + "' and '" + dt1 + "' and sd.staff_code='" + staffcode + "'";
                            dthrDetNo = dir.selectDataTable(SelhrdetNo);
                        }
                        //if (dthrDetNo.Rows.Count > 0)
                        //{
                        //    foreach(DataRow dr in dthrDetNo.Rows)
                        //    {


                        //    }
                        //}
                        datelbl.Visible = false;
                        lblset.Visible = false;
                        FpSpread2.Sheets[0].ColumnCount = 5;
                        FpSpread2.Sheets[0].Columns.Default.Width = 0;
                        //FpSpread2.Sheets[0].Rows.Default.Height = 50;
                        FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();//Added by Manikandan 24/07/2013
                        //FpSpread2.Sheets[0].SheetCornerSpanModel.Add(0, 0, 2, 1);//Added by Manikandan on 24/07/2013
                        // FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 2);
                        FpSpread2.SheetCorner.Cells[0, 0].Text = "S.No";
                        FpSpread2.SheetCorner.Cells[0, 0].RowSpan = 3;//added by srinath 4/10/2013
                        FpSpread2.Sheets[0].SheetCorner.Columns[0].CellType = tb;//Added by Manikandan
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                        //Added  by srinath 21/8/2013
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[1].Locked = true;
                        FpSpread2.Sheets[0].Columns[2].Locked = true;
                        FpSpread2.Sheets[0].Columns[3].Locked = true;
                        FpSpread2.Sheets[0].Columns[4].Locked = true;
                        FpSpread2.Sheets[0].Columns[4].Visible = false;
                        FpSpread2.Sheets[0].FrozenColumnCount = 5;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Columns[0].Width = 50;
                        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                        FpSpread2.Sheets[0].Columns[1].CellType = textcel_type;
                        FpSpread2.Sheets[0].Columns[2].CellType = textcel_type;
                        FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                        int sno = 0;
                        //   string[] strcomo1;
                        FarPoint.Web.Spread.ComboBoxCellType objintcell1 = new FarPoint.Web.Spread.ComboBoxCellType();
                        //  strcomo1 = new string[] { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NJ", "S", "NCC", "HS" };
                        //---------------------------------load rights
                        string[] strcomo1 = new string[20];
                        string[] attnd_rights1 = new string[100];
                        int i = 0;
                        string grouporusercode = string.Empty;
                        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                        {
                            grouporusercode = " group_code='" + Convert.ToString(Convert.ToString(Session["group_code"]).Trim().Split(';')[0]).Trim() + "'";
                        }
                        else
                        {
                            grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                        }
                        cmd.CommandText = "select distinct rights from  OD_Master_Setting where " + grouporusercode + "";//--usercode='" + Session["UserCode"].ToString() + "'
                        cmd.Connection = con;
                        con.Close();
                        con.Open();
                        SqlDataReader dr_rights_od = cmd.ExecuteReader();
                        if (dr_rights_od.HasRows)
                        {
                            while (dr_rights_od.Read())
                            {
                                string od_rights = string.Empty;
                                Hashtable od_has = new Hashtable();
                                od_rights = dr_rights_od["rights"].ToString();
                                if (od_rights != string.Empty)
                                {
                                    string[] split_od_rights = od_rights.Split(',');
                                    strcomo1 = new string[split_od_rights.GetUpperBound(0) + 2];
                                    strcomo1[i++] = string.Empty;
                                    for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                                    {
                                        strcomo1[i++] = split_od_rights[od_temp].ToString();
                                    }
                                }
                                else
                                {
                                    // "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" 
                                    strcomo1[0] = string.Empty;
                                    //strcomo1[1] = "P";
                                    //strcomo1[2] = "A";
                                    //strcomo1[3] = "OD";
                                    //strcomo1[4] = "SOD";
                                    //strcomo1[5] = "ML";
                                    //strcomo1[6] = "NSS";
                                    //strcomo1[7] = "L";
                                    //strcomo1[8] = "NCC";
                                    //strcomo1[9] = "HS";
                                    //strcomo1[10] = "PP";
                                    //strcomo1[11] = "SYOD";
                                    //strcomo1[12] = "COD";
                                    //strcomo1[13] = "OOD";
                                }
                            }
                        }
                        else
                        {
                            // "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" 
                            strcomo1[0] = string.Empty;
                            //strcomo1[1] = "P";
                            //strcomo1[2] = "A";
                            //strcomo1[3] = "OD";
                            //strcomo1[4] = "SOD";
                            //strcomo1[5] = "ML";
                            //strcomo1[6] = "NSS";
                            //strcomo1[7] = "L";
                            //strcomo1[8] = "NCC";
                            //strcomo1[9] = "HS";
                            //strcomo1[10] = "PP";
                            //strcomo1[11] = "SYOD";
                            //strcomo1[12] = "COD";
                            //strcomo1[13] = "OOD";
                        }
                        //---------------------------
                        objintcell1 = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
                        objintcell1.ShowButton = true;
                        objintcell1.AutoPostBack = true;
                        objintcell1.UseValue = true;
                        FpSpread2.ActiveSheetView.Columns[0].CellType = objintcell1;
                        FpSpread2.SaveChanges();
                        FpSpread2.Sheets[0].Columns[0].BackColor = Color.MistyRose;
                        FpSpread2.Sheets[0].Cells[0, 0].Locked = true;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[4].Visible = false;
                        if (Session["Rollflag"].ToString() != "0")
                        {
                            FpSpread2.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                            FpSpread2.Sheets[0].Columns[1].Width = 100;
                        }
                        if (Session["Regflag"].ToString() != "0")
                        {
                            FpSpread2.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                            FpSpread2.Sheets[0].Columns[2].Width = 100;
                        }
                        if (Session["Studflag"].ToString() != "0")
                        {
                            FpSpread2.Sheets[0].ColumnHeader.Columns[4].Visible = true;
                            FpSpread2.Sheets[0].Columns[4].Width = 100;
                        }
                        FpSpread2.Sheets[0].Columns[3].Width = 200;
                        FpSpread2.SaveChanges();
                        FpSpread2.ActiveSheetView.AutoPostBack = false;
                        FpSpread2.Sheets[0].RowCount = 1;
                        FpSpread2.Sheets[0].Cells[0, 0].CellType = textcel_type;
                        FpSpread2.Sheets[0].Cells[0, 0].BackColor = Color.White;
                        FarPoint.Web.Spread.ComboBoxCellType objintcell = new FarPoint.Web.Spread.ComboBoxCellType();
                        FpSpread2.Sheets[0].SpanModel.Add(0, 0, 1, 5);
                        //string[] strcomo1a = new string[] { "Select for All ", " ", "P", "A", "OD", "ML", "SOD", "NSS", "L", "NJ", "S", "NCC", "HS" };
                        //strcomo = new string[] { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NJ", "S", "NCC", "HS" };
                        //---------------------------------load rights
                        string[] strcomo = new string[20];
                        string[] strcomo1a = new string[20];
                        int j = 0;
                        i = 0;
                        cmd.CommandText = "select rights from OD_Master_Setting where " + grouporusercode + "";// usercode=" + Session["UserCode"].ToString() + "";
                        cmd.Connection = con;
                        con.Close();
                        con.Open();
                        SqlDataReader dr_rights_od_2 = cmd.ExecuteReader();
                        if (dr_rights_od_2.HasRows)
                        {
                            while (dr_rights_od_2.Read())
                            {
                                string od_rights = string.Empty;
                                Hashtable od_has = new Hashtable();
                                od_rights = dr_rights_od_2["rights"].ToString();
                                if (od_rights != string.Empty)
                                {
                                    string[] split_od_rights = od_rights.Split(',');
                                    strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
                                    strcomo1a = new string[split_od_rights.GetUpperBound(0) + 3];
                                    strcomo1a[j++] = "Select for All";
                                    strcomo1a[j++] = string.Empty;
                                    strcomo[i++] = string.Empty;
                                    for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                                    {
                                        strcomo[i++] = split_od_rights[od_temp].ToString();
                                        strcomo1a[j++] = split_od_rights[od_temp].ToString();
                                    }
                                }
                                else
                                {
                                    // "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" 
                                    strcomo[0] = string.Empty;
                                    //strcomo[1] = "P";
                                    //strcomo[2] = "A";
                                    //strcomo[3] = "OD";
                                    //strcomo[4] = "SOD";
                                    //strcomo[5] = "ML";
                                    //strcomo[6] = "NSS";
                                    //strcomo[7] = "L";
                                    //strcomo[8] = "NCC";
                                    //strcomo[9] = "HS";
                                    //strcomo[10] = "PP";
                                    //strcomo[11] = "SYOD";
                                    //strcomo[12] = "COD";
                                    //strcomo[13] = "OOD";
                                    // "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" 
                                    strcomo1a[0] = "Select for All";
                                    strcomo1a[1] = string.Empty;
                                    //strcomo1a[2] = "P";
                                    //strcomo1a[3] = "A";
                                    //strcomo1a[4] = "OD";
                                    //strcomo1a[5] = "SOD";
                                    //strcomo1a[6] = "ML";
                                    //strcomo1a[7] = "NSS";
                                    //strcomo1a[8] = "L";
                                    //strcomo1a[9] = "NCC";
                                    //strcomo1a[10] = "HS";
                                    //strcomo1a[11] = "PP";
                                    //strcomo1a[12] = "SYOD";
                                    //strcomo1a[13] = "COD";
                                    //strcomo1a[14] = "OOD";
                                }
                            }
                        }
                        else
                        {
                            // "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" 
                            strcomo[0] = string.Empty;
                            //strcomo[1] = "P";
                            //strcomo[2] = "A";
                            //strcomo[3] = "OD";
                            //strcomo[4] = "SOD";
                            //strcomo[5] = "ML";
                            //strcomo[6] = "NSS";
                            //strcomo[7] = "L";
                            //strcomo[8] = "NCC";
                            //strcomo[9] = "HS";
                            //strcomo[10] = "PP";
                            //strcomo[11] = "SYOD";
                            //strcomo[12] = "COD";
                            //strcomo[13] = "OOD";
                            // "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" 
                            strcomo1a[0] = "Select for All";
                            strcomo1a[1] = string.Empty;
                            //strcomo1a[2] = "P";
                            //strcomo1a[3] = "A";
                            //strcomo1a[4] = "OD";
                            //strcomo1a[5] = "SOD";
                            //strcomo1a[6] = "ML";
                            //strcomo1a[7] = "NSS";
                            //strcomo1a[8] = "L";
                            //strcomo1a[9] = "NCC";
                            //strcomo1a[10] = "HS";
                            //strcomo1a[11] = "PP";
                            //strcomo1a[12] = "SYOD";
                            //strcomo1a[13] = "COD";
                            //strcomo1a[14] = "OOD";
                        }
                        //---------------------------
                        objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1a);
                        objintcell.ShowButton = true;
                        objintcell.AutoPostBack = true;
                        objintcell.UseValue = true;
                        // FpSpread2.ActiveSheetView.Cells[0, 5].CellType = objintcell;
                        FpSpread2.SaveChanges();
                        FarPoint.Web.Spread.ComboBoxCellType objcom = new FarPoint.Web.Spread.ComboBoxCellType();
                        objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);
                        objcom.AutoPostBack = true;
                        FpSpread2.Visible = true;
                        //added By Srinath 15/8/2013
                        string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                        string strorder = "ORDER BY registration.roll_no";
                        if (orderby_Setting == "0")
                        {
                            strorder = "ORDER BY registration.roll_no";
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
                            strorder = "ORDER BY registration.roll_no,registration.Reg_No,registration.Stud_Name";
                        }
                        else if (orderby_Setting == "0,1")
                        {
                            strorder = "ORDER BY registration.roll_no,registration.Reg_No";
                        }
                        else if (orderby_Setting == "1,2")
                        {
                            strorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
                        }
                        else if (orderby_Setting == "0,2")
                        {
                            strorder = "ORDER BY registration.roll_no,registration.Stud_Name";
                        }
                        //string sqlstr =string.Empty;
                        //if (tag_filter == "1")
                        //{
                        //    string islab = d2.GetFunction("select sm.Lab  from subject s,sub_sem sm where s.subType_no =sm.subType_no and s.subject_no ='" + ddl_select_subj.SelectedValue + "'");
                        //    if (islab.Trim() == "1" || islab.Trim() == "True")
                        //    {
                        //        string getbatch = dacces2.GetFunction(" select Stu_Batch  from LabAlloc_New_Spl where Subject_No ='" + ddl_select_subj.SelectedValue + "' and Degree_Code ='" + Convert.ToString(ddlbranch.SelectedValue) + "' and Semester ='" + Convert.ToString(ddlsem.SelectedValue) + "' and Batch_Year ='" + Convert.ToString(ddlbatch.SelectedValue) + "' and fdate ='" + datefrom + "' " + sec + "");
                        //        string bat =string.Empty;
                        //        if (getbatch.Trim() != "")
                        //        {
                        //            if (getbatch.Contains(',') == true)
                        //            {
                        //                string[] splitbatch = getbatch.Split(',');
                        //                if (splitbatch.Length > 0)
                        //                {
                        //                    for (int row = 0; row < splitbatch.Length; row++)
                        //                    {
                        //                        if (bat == "")
                        //                        {
                        //                            bat = Convert.ToString(splitbatch[row]);
                        //                        }
                        //                        else
                        //                        {
                        //                            bat = bat + "'" + "," + "'" + Convert.ToString(splitbatch[row]);
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                bat = getbatch;
                        //            }
                        //            sqlstr = "select registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,subjectChooser_New_Spl sp where sp.roll_no =Registration.Roll_No and sp.semester=Registration.Current_Semester and  a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + " and sp.Batch in('" + bat + "')  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and a.degree_code = registration.degree_code " + strsec + " " + Session["strvar"] + " " + strorder + " ";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        sqlstr = "select registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and a.degree_code = registration.degree_code " + strsec + " " + Session["strvar"] + " " + strorder + " ";
                        //    }
                        //}
                        //else
                        //{
                        string strsub = string.Empty;
                        if (ddl_select_subj.Items.Count > 0)
                        {
                            strsub = Convert.ToString(ddl_select_subj.SelectedItem).ToLower();
                        }
                        string detnoval = string.Empty;
                        string detnovalues = string.Empty;

                        if (ddl_select_hour.Items.Count > 0 && strsub != "all")//&& ddl_select_subj.SelectedItem.ToString().ToLower() != "all"
                        {
                            if (!chkAllstudent.Checked)
                            {
                                detnoval = "and shs.hrdet_no='" + ddl_select_hour.SelectedValue.ToString() + "'";
                            }
                            else
                            {
                                detnoval = "and shd.hrdet_no='" + ddl_select_hour.SelectedValue.ToString() + "'";
                            }
                        }
                        //else
                        //{
                        //    if (ddl_select_hour.Items.Count > 1)
                        //    {
                        //        for (int s = 1; s < ddl_select_hour.Items.Count; s++)
                        //        {
                        //            string hedet = Convert.ToString(ddl_select_hour.SelectedValue);
                        //            if (detnovalues == "")
                        //                detnovalues = hedet;
                        //            else
                        //                detnovalues = detnovalues + "','" + hedet;

                        //        }
                        //    }
                        //    if (detnovalues != "")
                        //    {
                        //        if (!chkAllstudent.Checked)
                        //        {
                        //            detnoval = "and shs.hrdet_no='" + detnovalues + "'";
                        //        }
                        //        else
                        //        {
                        //            detnoval = "and shd.hrdet_no='" + detnovalues + "'";
                        //        }
                        //    }

                        //}



                        string sqlstr = string.Empty;
                        staffcode = Convert.ToString(Session["staff_code"]).Trim();
                        string qrySubjectNoList = string.Empty;

                        //Added By Saranyadevi 16.8.2018
                        if (!chkAllstudent.Checked)
                        {
                            if (ddl_select_subj.Items.Count > 0)
                            {
                                string selectedSubject = Convert.ToString(ddl_select_subj.SelectedValue).Trim();
                                if (!string.IsNullOrEmpty(selectedSubject) && selectedSubject.Trim().ToLower() != "all")
                                {
                                    qrySubjectNoList = "select distinct registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,specialhr_master shm,specialhr_details shd,specialHourStudents shs,subject s where shd.subject_no=s.subject_no and a.app_no=registration.app_no   and shm.hrentry_no=shd.hrentry_no and shd.hrdet_no=shs.hrdet_no and a.app_no=shs.appNo and Registration.App_No=shs.appNo and  shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and shd.subject_no='" + selectedSubject + "' and shd.staff_code='" + Convert.ToString(staffcode).Trim() + "'" + detnoval + " and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "'" + strsec + " and shm.date between '" + datefrom + "' and '" + dateto + "' " + Session["strvar"] + " " + strorder + "";

                                    //and a.degree_code = registration.degree_code cmd saranyadevi16.8.2018


                                    //qrySubjectNoList = "select distinct registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,specialhr_master shm,specialhr_details shd,specialHourStudents shs,subject s where shd.subject_no=s.subject_no and a.app_no=registration.app_no  and a.degree_code = registration.degree_code and shm.hrentry_no=shd.hrentry_no and shd.hrdet_no=shs.hrdet_no and a.app_no=shs.appNo and Registration.App_No=shs.appNo and  shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and shd.subject_no='" + selectedSubject + "' and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "'" + strsec + " " + Session["strvar"] + " " + strorder + "";

                                }
                            }

                            if (staffcode.Trim() != "")
                            {
                                #region Modified By Prabha

                                // sqlstr = "select distinct registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,subjectChooser sc where a.app_no=registration.app_no and registration.roll_no=sc.roll_no and registration.Current_Semester =sc.semester and  staffcode like '%" + staffcode + "%'  and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and a.degree_code = registration.degree_code " + strsec + " " + Session["strvar"] + " " + strorder + "";

                                sqlstr = "select distinct registration.roll_no,registration.reg_no,registration.stud_name,registration.stud_type,adm_date from specialhr_master shm,specialhr_details shd,specialHourStudents shs, registration, applyn a where shm.hrentry_no=shd.hrentry_no and shd.hrdet_no=shs.hrdet_no and a.app_no=shs.appNo and Registration.App_No=shs.appNo and a.app_no=registration.App_No and shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  and ((registration.mode=1) or (registration.mode=3) or (registration.mode=2)) and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "'" + detnoval + " and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and shd.staff_code='" + Convert.ToString(staffcode).Trim() + "' and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "'" + strsec + " " + Session["strvar"] + " and shd.hrdet_no in(" + SelhrdetNo + ") " + strorder + "";
                                //and a.degree_code = registration.degree_code cmd saranyadevi16.8.2018
                                #endregion

                                sqlstr = sqlstr + "  select distinct sc.subject_no from specialhr_master shm,specialhr_details shd,specialHourStudents shs,registration, applyn a,subjectChooser sc where shm.hrentry_no=shd.hrentry_no and shd.hrdet_no=shs.hrdet_no and a.app_no=shs.appNo and sc.subject_no=shd.subject_no and a.app_no=registration.app_no and registration.roll_no=sc.roll_no and registration.Current_Semester =sc.semester and shd.staff_code like '%" + staffcode + "%'  and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + "" + detnoval + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR'  and delflag=0 " + strsec + " and shm.date between '" + datefrom + "' and '" + dateto + "'  and shd.hrdet_no in(" + SelhrdetNo + ") " + Session["strvar"] + "";
                                //and a.degree_code = registration.degree_code cmd saranyadevi16.8.2018

                                //sqlstr = sqlstr + "select distinct registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,specialhr_master shm,specialhr_details shd,specialHourStudents shs,subject s,subjectChooser sc where shd.subject_no=s.subject_no and sc.subject_no=s.subject_no and a.app_no=registration.app_no  and a.degree_code = registration.degree_code and shm.hrentry_no=shd.hrentry_no and shd.hrdet_no=shs.hrdet_no and a.app_no=shs.appNo and Registration.App_No=shs.appNo and  shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and  staffcode like '%" + staffcode + "%' and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and shd.subject_no='" + ddl_select_subj.SelectedValue.ToString() + "' and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "'" + strsec + " " + Session["strvar"] + " " + strorder + "";
                            }
                            else
                            {
                                sqlstr = "select distinct registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,specialhr_master shm,specialhr_details shd,specialHourStudents shs where a.app_no=registration.app_no  and shm.hrentry_no=shd.hrentry_no and shd.hrdet_no=shs.hrdet_no and a.app_no=shs.appNo and Registration.App_No=shs.appNo and shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' " + detnoval + "and delflag=0 and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "'" + strsec + " and shm.date between '" + datefrom + "' and '" + dateto + "' " + Session["strvar"] + " " + strorder + " ";

                                //and a.degree_code = registration.degree_code cmd saranyadevi16.8.2018
                            }
                        }
                        //Added By Saranyadevi 16.8.2018
                        else
                        {

                            if (ddl_select_subj.Items.Count > 0)
                            {
                                string selectedSubject = Convert.ToString(ddl_select_subj.SelectedValue).Trim();
                                if (!string.IsNullOrEmpty(selectedSubject) && selectedSubject.Trim().ToLower() != "all")
                                {
                                    qrySubjectNoList = "select distinct registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,specialhr_master shm,specialhr_details shd,subject s where shd.subject_no=s.subject_no and a.app_no=registration.app_no  and shm.hrentry_no=shd.hrentry_no  and  shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and shd.subject_no='" + selectedSubject + "' and shd.staff_code='" + Convert.ToString(staffcode).Trim() + "'" + detnoval + " and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "'" + strsec + " and shm.date between '" + datefrom + "' and '" + dateto + "' " + Session["strvar"] + " " + strorder + "";

                                }
                            }

                            if (staffcode.Trim() != "")
                            {
                                sqlstr = "select distinct registration.roll_no,registration.reg_no,registration.stud_name,registration.stud_type,adm_date from specialhr_master shm,specialhr_details shd, registration, applyn a where shm.hrentry_no=shd.hrentry_no  and a.app_no=registration.App_No and shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  and ((registration.mode=1) or (registration.mode=3) or (registration.mode=2)) and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "'" + detnoval + " and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and shd.staff_code='" + Convert.ToString(staffcode).Trim() + "' and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "'" + strsec + " and shm.date between '" + datefrom + "' and '" + dateto + "' " + Session["strvar"] + " and shd.hrdet_no in(" + SelhrdetNo + ") " + strorder + "";


                                sqlstr = sqlstr + "  select distinct sc.subject_no from specialhr_master shm,specialhr_details shd,registration, applyn a,subjectChooser sc where shm.hrentry_no=shd.hrentry_no  and sc.subject_no=shd.subject_no and a.app_no=registration.app_no and registration.roll_no=sc.roll_no and registration.Current_Semester =sc.semester and shd.staff_code like '%" + staffcode + "%'  and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + "" + detnoval + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR'  and delflag=0  " + strsec + "  and shd.hrdet_no in(" + SelhrdetNo + ") and shm.date between '" + datefrom + "' and '" + dateto + "' " + Session["strvar"] + "";

                            }
                            else
                            {
                                sqlstr = "select distinct registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,specialhr_master shm,specialhr_details shd where a.app_no=registration.app_no  and shm.hrentry_no=shd.hrentry_no and shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' " + detnoval + "and delflag=0 and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "'" + strsec + " and shm.date between '" + datefrom + "' and '" + dateto + "'  " + Session["strvar"] + "  " + strorder + " ";

                            }


                        }
                        //}
                        // string sqlstr = "select roll_no,reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and a.degree_code = registration.degree_code " + strsec + " " + Session["strvar"] + " order by  roll_no ";
                        string subjectnostudentAllocate = string.Empty;
                        SqlDataAdapter da_reg_student = new SqlDataAdapter(sqlstr, con);
                        con.Close();
                        con.Open();
                        DataSet ds_reg_stud = new DataSet();
                        da_reg_student.Fill(ds_reg_stud);
                        DataTable dtDistinctStudent = new DataTable();
                        if (!string.IsNullOrEmpty(qrySubjectNoList))
                        {
                            dtDistinctStudent = dir.selectDataTable(qrySubjectNoList);
                        }
                        if (ds_reg_stud.Tables.Count > 0 && ds_reg_stud.Tables[0].Rows.Count > 0)
                        {
                            if (dtDistinctStudent.Rows.Count <= 0)
                            {
                                dtDistinctStudent = ds_reg_stud.Tables[0].DefaultView.ToTable(true, "roll_no", "reg_no", "stud_name", "adm_date", "stud_type");
                            }
                            if (dtDistinctStudent.Rows.Count > 0)
                            {
                                for (int stucont = 0; stucont < dtDistinctStudent.Rows.Count; stucont++)
                                {
                                    sno++;
                                    int rowcnt = FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].RowHeader.Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread2.Sheets[0].RowCount - 1);
                                    FpSpread2.Sheets[0].Cells[rowcnt, 1].Text = dtDistinctStudent.Rows[stucont]["roll_no"].ToString();
                                    FpSpread2.Sheets[0].Cells[rowcnt, 2].Text = dtDistinctStudent.Rows[stucont]["reg_no"].ToString();
                                    FpSpread2.Sheets[0].Cells[rowcnt, 3].Text = dtDistinctStudent.Rows[stucont]["stud_name"].ToString();
                                    FpSpread2.Sheets[0].Cells[rowcnt, 3].Tag = dtDistinctStudent.Rows[stucont]["adm_date"].ToString();
                                    FpSpread2.Sheets[0].Cells[rowcnt, 4].Text = dtDistinctStudent.Rows[stucont]["stud_type"].ToString();
                                    FpSpread2.Sheets[0].Cells[rowcnt, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].Cells[rowcnt, 1].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread2.Sheets[0].Cells[rowcnt, 2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread2.Sheets[0].Cells[rowcnt, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread2.Sheets[0].Cells[rowcnt, 4].HorizontalAlign = HorizontalAlign.Left;
                                    if (dtDistinctStudent.Rows[stucont]["stud_type"].ToString() == "Hostler")
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.LightYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.LightYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.LightYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].BackColor = Color.LightYellow;
                                    }
                                    else
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.MediumSeaGreen;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.MediumSeaGreen;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.MediumSeaGreen;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].BackColor = Color.MediumSeaGreen;
                                    }
                                }
                            }
                            if (staffcode.Trim() != "")
                            {
                                if (ds_reg_stud.Tables.Count > 1 && ds_reg_stud.Tables[1].Rows.Count > 0)
                                {
                                    for (int row = 0; row < ds_reg_stud.Tables[1].Rows.Count; row++)
                                    {
                                        if (subjectnostudentAllocate == "")
                                        {
                                            subjectnostudentAllocate = Convert.ToString(ds_reg_stud.Tables[1].Rows[row]["subject_no"]);
                                        }
                                        else
                                        {
                                            subjectnostudentAllocate = subjectnostudentAllocate + "'" + ", " + "'" + Convert.ToString(ds_reg_stud.Tables[1].Rows[row]["subject_no"]);
                                        }
                                    }
                                }
                            }
                        }
                        FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 2;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].CellType = tb;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].CellType = tb;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = "No Of Student(s) Present:";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "No Of Student(s) Absent:";
                        //added by annyutha 3nd sep 14*//
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Locked = true;
                        //*end*****//
                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, 0, 1, 4);
                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 4);
                        Arrlist_subjno.Clear();
                        int splcnt = 0;
                        string slag_code = GetFunction("select isnull(staff_code,'') as slogcode from usermaster where user_code=" + Session["usercode"].ToString() + " and is_staff=1");
                        //   if (slag_code == "")
                        {
                            string sql_special_hr_master = "Select degree_code,semester,batch_year,convert(nvarchar(25),date,103) as date,date as spl_date ,hrentry_no from specialhr_master where degree_code='" + ddlbranch.SelectedItem.Value.ToString() + "' and semester='" + ddlsem.SelectedItem.Text + "' and batch_year='" + ddlbatch.SelectedItem.Text + "' and date between '" + datefrom + "' and '" + dateto + "' " + sec + "";

                            SqlDataAdapter da_spl_hr_master = new SqlDataAdapter(sql_special_hr_master, mysql1);
                            mysql1.Close();
                            mysql1.Open();
                            DataSet ds_spl_hr_master = new DataSet();
                            da_spl_hr_master.Fill(ds_spl_hr_master);
                            if (ds_spl_hr_master.Tables.Count > 0 && ds_spl_hr_master.Tables[0].Rows.Count > 0)
                            {
                                for (int hr_mas_count = 0; hr_mas_count < ds_spl_hr_master.Tables[0].Rows.Count; hr_mas_count++)
                                {
                                    string sql_hr_details = string.Empty;
                                    if (slag_code == "")
                                    {
                                        sql_hr_details = "Select * from specialhr_details where hrentry_no='" + ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["hrentry_no"].ToString() + "' " + subject_filter_tag + " order by hrdet_no ";
                                    }
                                    else
                                    {
                                        if (subjectnostudentAllocate.Trim() != "")
                                        {
                                            if (subject_filter_tag.Trim() == "")
                                            {
                                                sql_hr_details = "Select * from specialhr_details where hrentry_no='" + ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["hrentry_no"].ToString() + "' and subject_no in ('" + subjectnostudentAllocate + "') and staff_code='" + slag_code + "' order by hrdet_no ";
                                            }
                                            else
                                            {
                                                sql_hr_details = "Select * from specialhr_details where hrentry_no='" + ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["hrentry_no"].ToString() + "' " + subject_filter_tag + " and staff_code='" + slag_code + "' order by hrdet_no ";
                                            }
                                        }
                                        else
                                        {
                                            sql_hr_details = "Select * from specialhr_details where hrentry_no='" + ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["hrentry_no"].ToString() + "' and staff_code='" + slag_code + "' order by hrdet_no ";
                                        }
                                    }
                                    SqlDataAdapter da_hr_details = new SqlDataAdapter(sql_hr_details, mysql1);
                                    DataSet ds_hr_details = new DataSet();
                                    da_hr_details.Fill(ds_hr_details);
                                    colcnt = 5;
                                    string Staffsubject = string.Empty;
                                    DataTable dtStaffSub = new DataTable();
                                    if (ds_hr_details.Tables.Count > 0 && ds_hr_details.Tables[0].Rows.Count > 0)
                                    {

                                        colcnt = FpSpread2.Sheets[0].ColumnCount;
                                        int spancnt = 0;
                                        int periodcnt = 0;
                                        for (int no_of_period = 0; no_of_period < ds_hr_details.Tables[0].Rows.Count; no_of_period++)
                                        {
                                            DataTable dtStucount = new DataTable();
                                            int count = 0;
                                            string studentCount = string.Empty;
                                            if (!chkAllstudent.Checked)
                                            {
                                                studentCount = "select COUNT(appNo) as stucount from specialHourStudents where hrdet_no='" + ds_hr_details.Tables[0].Rows[no_of_period]["hrdet_no"].ToString() + "'";
                                                dtStucount = dir.selectDataTable(studentCount);
                                                if (dtStucount.Rows.Count > 0)
                                                {
                                                    count = Convert.ToInt32(dtStucount.Rows[0]["stucount"]);
                                                }
                                                if (count > 0)
                                                {
                                                    has_stud_list.Clear();
                                                    splcnt++;
                                                    spancnt++;
                                                    FpSpread2.Sheets[0].SheetCorner.RowCount = 2;
                                                    int colcunt = FpSpread2.Sheets[0].ColumnCount++;
                                                    periodcnt++;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, colcunt].Text = ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["date"].ToString();
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcunt].Tag = ds_hr_details.Tables[0].Rows[no_of_period]["hrdet_no"].ToString();
                                                    subject_name = GetFunction("select subject_name from subject where subject_no='" + ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString() + "'");
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcunt].Text = subject_name;
                                                    Arrlist_subjno.Add(ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString());
                                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcunt].Note = ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["date"].ToString();
                                                    FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                                    FpSpread2.Sheets[0].Cells[0, colcunt].Tag = ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["spl_date"].ToString();
                                                    SqlDataReader dr2;
                                                    con.Close();
                                                    con.Open();
                                                    //-----------------------load subject to ddl

                                                    if (tag_filter == "0")
                                                    {
                                                        int ddl_subject_item_count = 0;
                                                        if (ddl_select_subj.Items.Count == 0)
                                                        {
                                                            ddl_select_subj.Items.Add("All");
                                                        }
                                                        ddl_subject_item_count = ddl_select_subj.Items.Count;
                                                        if (!has_subj.ContainsKey(ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString()))
                                                        {
                                                            ddl_select_subj.Items.Add(subject_name);
                                                            ddl_select_subj.Items[ddl_subject_item_count].Value = ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString();
                                                            has_subj.Add(ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString(), subject_name);
                                                        }
                                                    }
                                                    else if (!string.IsNullOrEmpty(staffcode))
                                                    {
                                                        //ddl_select_subj.Items.Clear();
                                                        if (ds_reg_stud.Tables.Count > 0)
                                                            Staffsubject = Convert.ToString(ds_reg_stud.Tables[1].Rows[0]);
                                                        string sqlSub = "select * from subject where subject_no='" + Staffsubject + "'";
                                                        dtStaffSub = dir.selectDataTable(sqlSub);
                                                        if (dtStaffSub.Rows.Count > 0)
                                                        {
                                                            ddl_select_subj.DataSource = dtStaffSub;
                                                            ddl_select_subj.DataTextField = "subject_name";
                                                            ddl_select_subj.DataValueField = "subject_no";
                                                            ddl_select_subj.DataBind();
                                                            //ddl_select_subj.Visible = false;
                                                        }
                                                    }
                                                    //--------------------------On 28/7/12 PRABHA lock stud attnd those who dont have tz subject
                                                    //
                                                    //SqlCommand cmd_attnd1 = new SqlCommand("select distinct s.roll_no From subjectchooser s, registration r where r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and s.semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedItem.ToString() + " and s.roll_no=r.roll_no  and subject_no=" + ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString() + "  " + sec + " order  by s.roll_no", con);


                                                    SqlCommand cmd_attnd1 = new SqlCommand("select distinct registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,specialhr_master shm,specialhr_details shd,specialHourStudents shs where a.app_no=registration.app_no   and shm.hrentry_no=shd.hrentry_no and shd.hrdet_no=shs.hrdet_no and a.app_no=shs.appNo and Registration.App_No=shs.appNo and  shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no=" + ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString() + " " + strsec + "", con);


                                                    dr2 = cmd_attnd1.ExecuteReader();
                                                    while (dr2.Read())
                                                    {
                                                        if (dr2.HasRows == true)
                                                        {
                                                            if (!has_stud_list.ContainsKey(dr2[0].ToString().Trim().ToLower()))
                                                            {
                                                                has_stud_list.Add(dr2[0].ToString().Trim().ToLower(), dr2[0].ToString().Trim());
                                                            }
                                                        }
                                                    }
                                                    for (int rw_cnt = 1; rw_cnt < FpSpread2.Sheets[0].RowCount; rw_cnt++)
                                                    {
                                                        string roll_no = FpSpread2.Sheets[0].Cells[rw_cnt, 1].Text.Trim();
                                                        if (!has_stud_list.ContainsKey(roll_no.Trim().ToLower()))
                                                        {
                                                            FpSpread2.Sheets[0].Cells[rw_cnt, (FpSpread2.Sheets[0].ColumnCount - 1)].Text = "-";
                                                            FpSpread2.Sheets[0].Cells[rw_cnt, (FpSpread2.Sheets[0].ColumnCount - 1)].Locked = true;
                                                        }
                                                    }
                                                    ArrayList addnewlist = new ArrayList();
                                                    DateTime dt1_new = Convert.ToDateTime(ds_hr_details.Tables[0].Rows[no_of_period]["start_time"]);
                                                    string start = dt1_new.ToString("hh:mm");
                                                    DateTime dt2_new = Convert.ToDateTime(ds_hr_details.Tables[0].Rows[no_of_period]["end_time"]);
                                                    string end = dt2_new.ToString("hh:mm");
                                                    string hour = start + "-" + end;
                                                    string date_newvalue = ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["date"].ToString();
                                                    string[] splitdatenew = date_newvalue.Split('/');
                                                    date_newvalue = Convert.ToString(splitdatenew[1] + "/" + splitdatenew[0] + "/" + splitdatenew[2]);
                                                    string islab = d2.GetFunction("select sm.Lab  from subject s,sub_sem sm where s.subType_no =sm.subType_no and s.subject_no ='" + ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString() + "'");
                                                    if (islab.Trim() == "1" || islab.Trim() == "True")
                                                    {
                                                        string getbatch = dacces2.GetFunction(" select Stu_Batch from LabAlloc_New_Spl where Subject_No ='" + ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString() + "' and Degree_Code ='" + Convert.ToString(ddlbranch.SelectedValue) + "' and Semester ='" + Convert.ToString(ddlsem.SelectedValue) + "' and Batch_Year ='" + Convert.ToString(ddlbatch.SelectedValue) + "' and fdate ='" + date_newvalue + "' " + sec + " and Hour_Value='" + hour + "'");
                                                        string bat = string.Empty;
                                                        if (getbatch.Trim() != "")
                                                        {
                                                            if (getbatch.Contains(',') == true)
                                                            {
                                                                string[] splitbatch = getbatch.Split(',');
                                                                if (splitbatch.Length > 0)
                                                                {
                                                                    for (int row = 0; row < splitbatch.Length; row++)
                                                                    {
                                                                        if (bat == "")
                                                                        {
                                                                            bat = Convert.ToString(splitbatch[row]);
                                                                        }
                                                                        else
                                                                        {
                                                                            bat = bat + "'" + "," + "'" + Convert.ToString(splitbatch[row]);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                bat = getbatch;
                                                            }
                                                            sqlstr = "select distinct s.roll_no From subjectChooser_New_Spl s, registration r where  r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and s.semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedItem.ToString() + " and s.roll_no=r.roll_no and s.Batch in('" + bat + "')  and subject_no=" + ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString() + "  " + sec + " order  by s.roll_no";
                                                            ds.Clear();
                                                            ds = d2.select_method_wo_parameter(sqlstr, "Text");
                                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                            {
                                                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                                                {
                                                                    if (!addnewlist.Contains(ds.Tables[0].Rows[row]["roll_no"]))
                                                                    {
                                                                        addnewlist.Add(ds.Tables[0].Rows[row]["roll_no"]);
                                                                    }
                                                                }
                                                            }
                                                            if (addnewlist.Count > 0)
                                                            {
                                                                for (int rw_cnt = 1; rw_cnt < FpSpread2.Sheets[0].RowCount; rw_cnt++)
                                                                {
                                                                    string roll_no = FpSpread2.Sheets[0].Cells[rw_cnt, 1].Text;
                                                                    if (!addnewlist.Contains(roll_no))
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[rw_cnt, (FpSpread2.Sheets[0].ColumnCount - 1)].Text = "-";
                                                                        FpSpread2.Sheets[0].Cells[rw_cnt, (FpSpread2.Sheets[0].ColumnCount - 1)].Locked = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                                                }
                                            }
                                            //Added By Saranyadevi 16.8.2018
                                            else
                                            {
                                                has_stud_list.Clear();
                                                splcnt++;
                                                spancnt++;
                                                FpSpread2.Sheets[0].SheetCorner.RowCount = 2;
                                                int colcunt = FpSpread2.Sheets[0].ColumnCount++;
                                                periodcnt++;
                                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, colcunt].Text = ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["date"].ToString();
                                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcunt].Tag = ds_hr_details.Tables[0].Rows[no_of_period]["hrdet_no"].ToString();
                                                subject_name = GetFunction("select subject_name from subject where subject_no='" + ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString() + "'");
                                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcunt].Text = subject_name;
                                                Arrlist_subjno.Add(ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString());
                                                FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcunt].Note = ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["date"].ToString();
                                                FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                                FpSpread2.Sheets[0].Cells[0, colcunt].Tag = ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["spl_date"].ToString();
                                                SqlDataReader dr2;
                                                con.Close();
                                                con.Open();
                                                //-----------------------load subject to ddl

                                                if (tag_filter == "0")
                                                {
                                                    int ddl_subject_item_count = 0;
                                                    if (ddl_select_subj.Items.Count == 0)
                                                    {
                                                        ddl_select_subj.Items.Add("All");
                                                    }
                                                    ddl_subject_item_count = ddl_select_subj.Items.Count;
                                                    if (!has_subj.ContainsKey(ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString()))
                                                    {
                                                        ddl_select_subj.Items.Add(subject_name);
                                                        ddl_select_subj.Items[ddl_subject_item_count].Value = ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString();
                                                        has_subj.Add(ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString(), subject_name);
                                                    }
                                                }
                                                else if (!string.IsNullOrEmpty(staffcode))
                                                {
                                                    //ddl_select_subj.Items.Clear();
                                                    if (ds_reg_stud.Tables.Count > 0)
                                                        Staffsubject = Convert.ToString(ds_reg_stud.Tables[1].Rows[0]);
                                                    string sqlSub = "select * from subject where subject_no='" + Staffsubject + "'";
                                                    dtStaffSub = dir.selectDataTable(sqlSub);
                                                    if (dtStaffSub.Rows.Count > 0)
                                                    {
                                                        ddl_select_subj.DataSource = dtStaffSub;
                                                        ddl_select_subj.DataTextField = "subject_name";
                                                        ddl_select_subj.DataValueField = "subject_no";
                                                        ddl_select_subj.DataBind();
                                                        //ddl_select_subj.Visible = false;
                                                    }
                                                }
                                                SqlCommand cmd_attnd1 = new SqlCommand("select distinct registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,specialhr_master shm,specialhr_details shd where a.app_no=registration.app_no   and shm.hrentry_no=shd.hrentry_no  and  shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no=" + ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString() + " " + strsec + "", con);


                                                dr2 = cmd_attnd1.ExecuteReader();
                                                while (dr2.Read())
                                                {
                                                    if (dr2.HasRows == true)
                                                    {
                                                        if (!has_stud_list.ContainsKey(dr2[0].ToString().Trim().ToLower()))
                                                        {
                                                            has_stud_list.Add(dr2[0].ToString().Trim().ToLower(), dr2[0].ToString().Trim());
                                                        }
                                                    }
                                                }
                                                for (int rw_cnt = 1; rw_cnt < FpSpread2.Sheets[0].RowCount; rw_cnt++)
                                                {
                                                    string roll_no = FpSpread2.Sheets[0].Cells[rw_cnt, 1].Text.Trim();
                                                    if (!has_stud_list.ContainsKey(roll_no.Trim().ToLower()))
                                                    {
                                                        FpSpread2.Sheets[0].Cells[rw_cnt, (FpSpread2.Sheets[0].ColumnCount - 1)].Text = "-";
                                                        FpSpread2.Sheets[0].Cells[rw_cnt, (FpSpread2.Sheets[0].ColumnCount - 1)].Locked = true;
                                                    }
                                                }
                                                ArrayList addnewlist = new ArrayList();
                                                DateTime dt1_new = Convert.ToDateTime(ds_hr_details.Tables[0].Rows[no_of_period]["start_time"]);
                                                string start = dt1_new.ToString("hh:mm");
                                                DateTime dt2_new = Convert.ToDateTime(ds_hr_details.Tables[0].Rows[no_of_period]["end_time"]);
                                                string end = dt2_new.ToString("hh:mm");
                                                string hour = start + "-" + end;
                                                string date_newvalue = ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["date"].ToString();
                                                string[] splitdatenew = date_newvalue.Split('/');
                                                date_newvalue = Convert.ToString(splitdatenew[1] + "/" + splitdatenew[0] + "/" + splitdatenew[2]);
                                                string islab = d2.GetFunction("select sm.Lab  from subject s,sub_sem sm where s.subType_no =sm.subType_no and s.subject_no ='" + ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString() + "'");
                                                if (islab.Trim() == "1" || islab.Trim() == "True")
                                                {
                                                    string getbatch = dacces2.GetFunction(" select Stu_Batch from LabAlloc_New_Spl where Subject_No ='" + ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString() + "' and Degree_Code ='" + Convert.ToString(ddlbranch.SelectedValue) + "' and Semester ='" + Convert.ToString(ddlsem.SelectedValue) + "' and Batch_Year ='" + Convert.ToString(ddlbatch.SelectedValue) + "' and fdate ='" + date_newvalue + "' " + sec + " and Hour_Value='" + hour + "'");
                                                    string bat = string.Empty;
                                                    if (getbatch.Trim() != "")
                                                    {
                                                        if (getbatch.Contains(',') == true)
                                                        {
                                                            string[] splitbatch = getbatch.Split(',');
                                                            if (splitbatch.Length > 0)
                                                            {
                                                                for (int row = 0; row < splitbatch.Length; row++)
                                                                {
                                                                    if (bat == "")
                                                                    {
                                                                        bat = Convert.ToString(splitbatch[row]);
                                                                    }
                                                                    else
                                                                    {
                                                                        bat = bat + "'" + "," + "'" + Convert.ToString(splitbatch[row]);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            bat = getbatch;
                                                        }
                                                        sqlstr = "select distinct s.roll_no From subjectChooser_New_Spl s, registration r where  r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and s.semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedItem.ToString() + " and s.roll_no=r.roll_no and s.Batch in('" + bat + "')  and subject_no=" + ds_hr_details.Tables[0].Rows[no_of_period]["subject_no"].ToString() + "  " + sec + " order  by s.roll_no";
                                                        ds.Clear();
                                                        ds = d2.select_method_wo_parameter(sqlstr, "Text");
                                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                        {
                                                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                                            {
                                                                if (!addnewlist.Contains(ds.Tables[0].Rows[row]["roll_no"]))
                                                                {
                                                                    addnewlist.Add(ds.Tables[0].Rows[row]["roll_no"]);
                                                                }
                                                            }
                                                        }
                                                        if (addnewlist.Count > 0)
                                                        {
                                                            for (int rw_cnt = 1; rw_cnt < FpSpread2.Sheets[0].RowCount; rw_cnt++)
                                                            {
                                                                string roll_no = FpSpread2.Sheets[0].Cells[rw_cnt, 1].Text;
                                                                if (!addnewlist.Contains(roll_no))
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[rw_cnt, (FpSpread2.Sheets[0].ColumnCount - 1)].Text = "-";
                                                                    FpSpread2.Sheets[0].Cells[rw_cnt, (FpSpread2.Sheets[0].ColumnCount - 1)].Locked = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);


                                            }
                                        }
                                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, colcnt, 1, spancnt);
                                    }
                                    string date = ds_spl_hr_master.Tables[0].Rows[hr_mas_count]["date"].ToString();
                                    string[] splitdate = date.Split('/');
                                    int temp = 0;
                                    int monthyear = Convert.ToInt32(splitdate[2]) * 12 + Convert.ToInt32(splitdate[1]);
                                    for (temp = 5; temp < FpSpread2.Sheets[0].ColumnCount; temp++)
                                        FpSpread2.ActiveSheetView.Cells[0, temp].CellType = objintcell;
                                    FpSpread2.Sheets[0].RowHeader.Cells[0, 0].Text = " ";
                                    for (temp = 5; temp < FpSpread2.Sheets[0].ColumnCount; temp++)
                                        FpSpread2.ActiveSheetView.Columns[temp].CellType = objcom;
                                    for (int col = 5; col < FpSpread2.Sheets[0].ColumnCount; col++)
                                    {
                                        has_stud_list.Clear();
                                        SqlDataReader dr1;
                                        con.Close();
                                        con.Open();
                                        if (!chkAllstudent.Checked)
                                        {
                                            SqlCommand cmd_attnd1 = new SqlCommand("select distinct registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,specialhr_master shm,specialhr_details shd,specialHourStudents shs where a.app_no=registration.app_no  and a.degree_code = registration.degree_code and shm.hrentry_no=shd.hrentry_no and shd.hrdet_no=shs.hrdet_no and a.app_no=shs.appNo and Registration.App_No=shs.appNo and  shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no=" + Arrlist_subjno[col - 5].ToString() + " " + strsec + "", con);
                                            dr1 = cmd_attnd1.ExecuteReader();

                                            
                                        }
                                        else 
                                        {
                                            SqlCommand cmd_attnd1 = new SqlCommand("select distinct registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type,adm_date from registration, applyn a,specialhr_master shm,specialhr_details shd where a.app_no=registration.app_no  and shm.hrentry_no=shd.hrentry_no and  shm.degree_code=Registration.degree_code and shm.batch_year=Registration.Batch_Year  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and registration.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and registration.current_semester='" + ddlsem.SelectedValue.ToString() + "' and registration.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no=" + Arrlist_subjno[col - 5].ToString() + " " + strsec + "", con);

                                            dr1 = cmd_attnd1.ExecuteReader();
                                        }

                                        
                                        while (dr1.Read())
                                        {
                                            if (dr1.HasRows == true)
                                            {
                                                if (!has_stud_list.ContainsKey(dr1[0].ToString().Trim().ToLower()))
                                                {
                                                    has_stud_list.Add(dr1[0].ToString().Trim().ToLower(), dr1[0].ToString().Trim());
                                                }
                                            }
                                        }
                                        string sql_attendance = "Select * from specialhr_attendance where month_year=" + monthyear + " and hrdet_no=" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + " order by roll_no";
                                        SqlDataAdapter da_attnd = new SqlDataAdapter(sql_attendance, dc_con);
                                        dc_con.Close();
                                        dc_con.Open();
                                        DataSet ds_attnd = new DataSet();
                                        da_attnd.Fill(ds_attnd);
                                        if (ds_attnd.Tables[0].Rows.Count > 0)
                                        {
                                            for (int attnd = 0; attnd < ds_attnd.Tables[0].Rows.Count; attnd++)
                                            {
                                                for (int rowcnt = 1; rowcnt < FpSpread2.Sheets[0].RowCount; rowcnt++)
                                                {
                                                    string roll_no = ds_attnd.Tables[0].Rows[attnd]["roll_no"].ToString().Trim();
                                                    if (roll_no.ToLower() == FpSpread2.Sheets[0].Cells[rowcnt, 1].Text.Trim().ToLower())
                                                    {
                                                        string currdate = FpSpread2.Sheets[0].ColumnHeader.Cells[0, col].Text;
                                                        string[] spiltcurdate = currdate.Split('/');
                                                        DateTime dtcurdate = Convert.ToDateTime(spiltcurdate[1] + '/' + spiltcurdate[0] + '/' + spiltcurdate[2]);
                                                        string strquery = "select  convert(varchar(15),dateadd(day,tot_days-1,ack_date),1) as action_days,ack_date,tot_days from stucon where ack_susp=1 and tot_days>0 and roll_no='" + roll_no + "' and ack_date<= '" + dtcurdate + "'";
                                                        DataSet dssuspen = d2.select_method(strquery, hat, "Text");
                                                        if (dssuspen.Tables.Count > 0 && dssuspen.Tables[0].Rows.Count > 0)
                                                        {
                                                            DateTime dt_act = Convert.ToDateTime(dssuspen.Tables[0].Rows[0]["action_days"].ToString());
                                                            TimeSpan t_con = dt_act.Subtract(dtcurdate);
                                                            long daycon = t_con.Days;
                                                            DateTime dt_curr1 = Convert.ToDateTime(dssuspen.Tables[0].Rows[0]["ack_date"].ToString());
                                                            TimeSpan t_con1 = dtcurdate.Subtract(dt_curr1);
                                                            long daycon1 = t_con1.Days;
                                                            long totalactdays = Convert.ToInt32(dssuspen.Tables[0].Rows[0]["tot_days"]);
                                                            if ((Convert.ToInt32(daycon + daycon1) == totalactdays - 1) && (daycon >= 0))
                                                            {
                                                                FarPoint.Web.Spread.TextCellType tc = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread2.Sheets[0].Cells[rowcnt, col].CellType = tc;
                                                                string att_mark1 = Attmark("9");
                                                                FpSpread2.Sheets[0].SetValue(rowcnt, col, "9");
                                                                FpSpread2.Sheets[0].SetText(rowcnt, col, att_mark1);
                                                                // FpSpread2.Sheets[0].Cells[rowcnt, col].Text = "S";
                                                                FpSpread2.Sheets[0].Cells[rowcnt, col].Locked = true;
                                                                //FpSpread2.Sheets[0].Cells[rowcnt, col].Tag = "9";
                                                            }
                                                            else
                                                            {
                                                                if (has_stud_list.ContainsKey(roll_no.Trim().ToLower()))
                                                                {
                                                                    string att_mark = ds_attnd.Tables[0].Rows[attnd]["attendance"].ToString();
                                                                    string att_mark1 = Attmark(att_mark);
                                                                    FpSpread2.Sheets[0].SetValue(rowcnt, col, att_mark.ToString());
                                                                    FpSpread2.Sheets[0].SetText(rowcnt, col, att_mark1.ToString());
                                                                    FpSpread2.Sheets[0].AutoPostBack = false;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[rowcnt, col].Text = "-";
                                                                    FpSpread2.Sheets[0].Cells[rowcnt, col].BackColor = Color.LightGreen;
                                                                    FpSpread2.Sheets[0].Cells[rowcnt, col].Locked = true;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (has_stud_list.ContainsKey(roll_no.ToLower().Trim()))
                                                            {
                                                                string att_mark = ds_attnd.Tables[0].Rows[attnd]["attendance"].ToString();
                                                                if (att_mark != "")
                                                                {
                                                                    flag_save = true;
                                                                }
                                                                string att_mark1 = Attmark(att_mark);
                                                                FpSpread2.Sheets[0].SetValue(rowcnt, col, att_mark.ToString());
                                                                FpSpread2.Sheets[0].SetText(rowcnt, col, att_mark1.ToString());
                                                                FpSpread2.Sheets[0].AutoPostBack = false;
                                                            }
                                                            else
                                                            {
                                                                FpSpread2.Sheets[0].Cells[rowcnt, col].Text = "-";
                                                                FpSpread2.Sheets[0].Cells[rowcnt, col].BackColor = Color.LightGreen;
                                                                FpSpread2.Sheets[0].Cells[rowcnt, col].Locked = true;
                                                            }
                                                        }
                                                        //rowcnt = FpSpread2.Sheets[0].RowCount - 1;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int rowcnt = 1; rowcnt < FpSpread2.Sheets[0].RowCount; rowcnt++)
                                            {
                                                string roll_no = FpSpread2.Sheets[0].Cells[rowcnt, 1].Text;
                                                string currdate = FpSpread2.Sheets[0].ColumnHeader.Cells[0, col].Text;
                                                string[] spiltcurdate = currdate.Split('/');
                                                DateTime dtcurdate = Convert.ToDateTime(spiltcurdate[1] + '/' + spiltcurdate[0] + '/' + spiltcurdate[2]);
                                                string strquery = "select  convert(varchar(15),dateadd(day,tot_days-1,ack_date),1) as action_days,ack_date,tot_days from stucon where ack_susp=1 and tot_days>0 and roll_no='" + roll_no + "' and ack_date<= '" + dtcurdate + "'";
                                                DataSet dssuspen = d2.select_method(strquery, hat, "Text");
                                                if (dssuspen.Tables[0].Rows.Count > 0)
                                                {
                                                    DateTime dt_act = Convert.ToDateTime(dssuspen.Tables[0].Rows[0]["action_days"].ToString());
                                                    TimeSpan t_con = dt_act.Subtract(dtcurdate);
                                                    long daycon = t_con.Days;
                                                    DateTime dt_curr1 = Convert.ToDateTime(dssuspen.Tables[0].Rows[0]["ack_date"].ToString());
                                                    TimeSpan t_con1 = dtcurdate.Subtract(dt_curr1);
                                                    long daycon1 = t_con1.Days;
                                                    long totalactdays = Convert.ToInt32(dssuspen.Tables[0].Rows[0]["tot_days"]);
                                                    if ((Convert.ToInt32(daycon + daycon1) == totalactdays - 1) && (daycon >= 0))
                                                    {
                                                        FarPoint.Web.Spread.TextCellType tc = new FarPoint.Web.Spread.TextCellType();
                                                        FpSpread2.Sheets[0].Cells[rowcnt, col].CellType = tc;
                                                        string att_mark1 = Attmark("9");
                                                        FpSpread2.Sheets[0].SetValue(rowcnt, col, "9");
                                                        FpSpread2.Sheets[0].SetText(rowcnt, col, att_mark1);
                                                        FpSpread2.Sheets[0].Cells[rowcnt, col].Locked = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // modified by annyutha 01/09/14 //
                        if (sno == 0)
                        {
                            pHeaderatendence.Visible = false;
                            pBodyatendence.Visible = false;
                            Panelpage.Visible = false;
                            Panel3.Visible = false;
                            lblspecial.Visible = true;
                            lblspecial.Text = "There are no students available";
                        }
                        // ********** end*******//
                        else
                        {
                            presentabsentcount();
                        }
                        if (splcnt == 0)
                        {
                            pHeaderatendence.Visible = false;
                            pBodyatendence.Visible = false;
                            Panelpage.Visible = false;
                            Panel3.Visible = false;
                            lblspecial.Visible = true;
                        }
                    }
                }
            }
            Panel3.Visible = true;
            FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpSpread2.Sheets[0].RowCount);
            if (totalRows > 1)
            {
                ddl_select_subj.Visible = true;
                lbl_subj_select.Visible = true;
                ddl_select_hour.Visible = true;
                lblSelectHour.Visible = true;
            }
            //Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread2.Sheets[0].PageSize);
            //Buttontotal.Text = "Records: " + totalRows + "  Pages:1 ";
            //DropDownListpage.Items.Clear();
            //if (totalRows >= 10)
            //{
            //    FpSpread2.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            //    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
            //    {
            //        DropDownListpage.Items.Add((k + 10).ToString());
            //    }
            //    DropDownListpage.Items.Add("Others");
            //    FpSpread2.Height = 400;
            //    FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            //    FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            //}
            //else if (totalRows == 0)
            //{
            //    DropDownListpage.Items.Add("0");
            //    // FpSpread2.Height = 200;
            //}
            //else
            //{
            //    FpSpread2.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            //    DropDownListpage.Items.Add(FpSpread2.Sheets[0].PageSize.ToString());
            //    FpSpread2.Height = 200 + (25 * Convert.ToInt32(totalRows));
            //}
            //DropDownListpage.SelectedIndex = DropDownListpage.Items.Count - 2;
            FpSpread2.Height = 700;
            FpSpread2.Sheets[0].AutoPostBack = false;
            int widt = 0;
            for (int col = 0; col < FpSpread2.Sheets[0].ColumnCount; col++)
                widt = widt + FpSpread2.Sheets[0].Columns[col].Width;
            widt = widt + FpSpread2.Sheets[0].RowHeader.Width + 18;
            if (widt > 900)
            {
                FpSpread2.Width = 900;
            }
            else
                FpSpread2.Width = widt;
            FpSpread2.SaveChanges();
            //added by annyutha
            if (flag_save == true)
            {
                Buttonsave.Text = "Update";
            }
            else
            {
                Buttonsave.Text = "Save";
            }
        }
        catch
        {
        }
    }

    //else
    //{
    //    lbltodate .Visible = true;
    //    lbltodate.Text = "Selece From Date";
    //}
    // FpSpread2.Height = 

    protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //if (update_flag == false)
        //{
        //    update_flag = true;
        //    FpSpread2.SaveChanges();
        //}
        // e.Handled = true;
        string actrow = e.SheetView.ActiveRow.ToString();
        string actcol = e.SheetView.ActiveColumn.ToString();
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
        if (actcol == "0")
        {
            if (actrow == last)
            {
                flag_true = false;
            }
            else
            {
                flag_true = true;
            }
        }
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(FpSpread2.Sheets[0].RowCount - 2); j++)
            {
                string admdate = Convert.ToString(FpSpread2.Sheets[0].Cells[j, 3].Tag);
                if (Convert.ToInt32(actcol) > 4)
                {
                    string spl = Convert.ToString(FpSpread2.Sheets[0].Cells[0, Convert.ToInt32(actcol)].Tag);
                    DateTime joindtae = Convert.ToDateTime(admdate);
                    DateTime spl_date = Convert.ToDateTime(spl);
                    TimeSpan t = spl_date.Subtract(joindtae);
                    long days = t.Days;
                    int daydiff = Convert.ToInt32(days);
                    actcol = e.SheetView.ActiveColumn.ToString();
                    e.Handled = true;
                    string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                    if (seltext != "System.Object" && seltext != "Select for All")
                    {
                        if (daydiff > 0)
                        {
                            if (FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Locked == false)
                            {
                                if ((FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "OD") && (FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "S"))
                                {
                                    FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Locked == false)
                            {
                                if ((FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "OD") && (FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "S"))
                                {
                                    FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = "NJ";
                                }
                            }
                        }
                    }
                }
                else
                {
                    actcol = e.SheetView.ActiveColumn.ToString();
                    //  e.Handled = true;
                    string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                    if (seltext != "System.Object")
                    {
                        if (FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Locked == false)
                        {
                            //  if (FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Locked == false)
                            if ((FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "OD") && (FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "S"))
                            {
                                FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Locked == false)
                        {
                            if ((FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "OD") && (FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "S"))
                            {
                                FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = string.Empty;
                            }
                        }
                    }
                }
            }
            flag_true = true;
            // presentabsentcount();
        }
        if (flag_true == false && actcol == "0")
        {
            for (int j = 5; j <= Convert.ToInt16(FpSpread2.Sheets[0].ColumnCount - 1); j++)
            {
                actcol = e.SheetView.ActiveColumn.ToString();
                string value = e.EditValues[0].ToString();
                e.Handled = true;
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                if (seltext != "Select for All")
                {
                    if (seltext != "System.Object")
                    {
                        if (FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), j].Locked == false)
                        {
                            if ((FpSpread2.Sheets[0].GetText(Convert.ToInt16(actrow), j) != "OD") && (FpSpread2.Sheets[0].GetText(Convert.ToInt16(actrow), j) != "S"))
                            {
                                FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), j].Text = seltext.ToString();
                            }
                        }
                    }
                }
            }
            //  presentabsentcount();
            flag_true = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //con.Open();
        //string collegecode = Session["collegecode"].ToString();
        //string usercode = Session["usercode"].ToString();
        //DataSet ds = Bind_Degree(collegecode.ToString(), usercode);
        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        //    ddldegree.DataSource = ds;
        //    ddldegree.DataValueField = "course_id";
        //    ddldegree.DataTextField = "course_name";
        //    ddldegree.DataBind();
        //    con.Close();
        //}
        //con.Open();
        //string course_id = ddldegree.SelectedValue.ToString();
        //if (course_id != null && course_id != "")
        //{
        //    DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode);
        //    ddlbranch.DataSource = dsbranch;
        //    ddlbranch.DataValueField = "degree_code";
        //    ddlbranch.DataTextField = "dept_name";
        //    ddlbranch.DataBind();
        //    con.Close();
        //    Btngo.Enabled = true;
        //    //bind semester
        //    bindsem();
        //    //bind section
        //    BindSectionDetail();
        //}
        //else
        //{
        //    Btngo.Enabled = false;
        //    lblset.Visible = true;
        //    ddl_select_subj.Visible = false;
        //    lbl_subj_select.Visible = false;
        //    lblset.Text = "No Degree Rights For This User";
        //}
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        pHeaderatendence.Visible = false;
        pBodyatendence.Visible = false;
        string date1 = string.Empty;
        string datefrom = string.Empty;
        lblfromdate.Visible = false;
        lbltodate.Visible = false;
        if (txtFromDate.Text == "")
        {
            lblfromdate.Text = "Select From Date";
            lblfromdate.Visible = true;
            return;
        }
        date1 = txtFromDate.Text.ToString();
        string[] split = date1.Split(new Char[] { '-' });
        datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
        if (dt1 > DateTime.Today)
        {
            lblset.Visible = true;
            ddl_select_subj.Visible = false;
            lbl_subj_select.Visible = false;
            ddl_select_hour.Visible = false;
            lblSelectHour.Visible = false;
            lblset.Text = "You can not mark attendance for the date greater than today";
            //txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            return;
        }
        else
        {
            lblset.Visible = false;
        }
    }

    protected void TxtToDate_TextChanged(object sender, EventArgs e)
    {
        lbltodate.Visible = false;
        if (TxtToDate.Text == "")
        {
            lbltodate.Text = "Select To Date";
            lbltodate.Visible = true;
            return;
        }
        string date2 = string.Empty;
        string dateto = string.Empty;
        //int noofhours;
        lblset.Visible = false;
        date2 = TxtToDate.Text.ToString();
        string[] split1 = date2.Split(new Char[] { '-' });
        dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
        DateTime dt2 = Convert.ToDateTime(dateto.ToString());
        if (dt2 > DateTime.Today)
        {
            lblset.Visible = true;
            ddl_select_subj.Visible = false;
            lbl_subj_select.Visible = false;
            ddl_select_hour.Visible = false;
            lblSelectHour.Visible = false;
            lblset.Text = "You can not mark attendance for the date greater than today";
            // TxtToDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            return;
        }
        pHeaderatendence.Visible = false;
        pBodyatendence.Visible = false;
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBoxother.Text = string.Empty;
        lblother.Visible = false;
        LabelE.Visible = false;
        if (DropDownListpage.Text == "Others")
        {
            TextBoxother.Visible = true;
            TextBoxother.Focus();
        }
        else
        {
            TextBoxother.Visible = false;
            FpSpread2.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
    }

    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpSpread2.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread2.Sheets[0].PageSize);
        Buttontotal.Text = "Records: " + totalRows + "  Pages: " + Session["totalPages"];
        Buttontotal.Visible = true;
    }

    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        try
        {
            if (FpSpread2.Sheets[0].RowCount > 0)
            {
                if (TextBoxother.Text != "")
                {
                    FpSpread2.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                    CalculateTotalPages();
                    lblother.Visible = false;
                }
            }
        }
        catch
        {
            lblother.Text = "Enter the Valid Page";
            TextBoxother.Text = string.Empty;
            lblother.Visible = true;
        }
    }

    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        lblother.Visible = false;
        try
        {
            if (FpSpread2.Sheets[0].RowCount > 0)
            {
                if (TextBoxpage.Text.Trim() != "")
                {
                    if (Convert.ToInt32(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                    {
                        LabelE.Visible = true;
                        LabelE.Text = "Exceed The Page Limit";
                        TextBoxpage.Text = string.Empty;
                        FpSpread2.Visible = true;
                    }
                    else if ((Convert.ToInt32(TextBoxpage.Text) == 0))
                    {
                        LabelE.Text = "Should be Greater than Zero";
                        LabelE.Visible = true;
                        TextBoxpage.Text = string.Empty;
                        FpSpread2.Visible = true;
                    }
                    else
                    {
                        LabelE.Visible = false;
                        FpSpread2.CurrentPage = Convert.ToInt32(TextBoxpage.Text) - 1;
                        FpSpread2.Visible = true;
                    }
                }
            }
        }
        catch
        {
            LabelE.Text = "Exceed The Page Limit";
            TextBoxpage.Text = string.Empty;
            LabelE.Visible = true;
        }
    }

    protected void Buttondeselect_Click(object sender, EventArgs e)
    {
        if (FpSpread2.Sheets[0].RowCount > 1)
        {
            for (int row = 1; row < FpSpread2.Sheets[0].RowCount - 2; row++)
            {
                for (int col = 5; col < FpSpread2.Sheets[0].ColumnCount; col++)
                    if (FpSpread2.Sheets[0].Cells[row, col].Text.ToString() != "OD" && FpSpread2.Sheets[0].Cells[row, col].Text.ToString() != "S")
                    {
                        FpSpread2.Sheets[0].Cells[row, col].Text = string.Empty;
                    }
                FpSpread2.SaveChanges();
            }
        }
        presentabsentcount();
    }

    protected void Buttonselectall_Click(object sender, EventArgs e)
    {
        if (FpSpread2.Sheets[0].RowCount > 1)
        {
            for (int row = 1; row < FpSpread2.Sheets[0].RowCount - 2; row++)
            {
                for (int col = 5; col < FpSpread2.Sheets[0].ColumnCount; col++)
                {
                    string admdate = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 3].Tag);
                    string spl = Convert.ToString(FpSpread2.Sheets[0].Cells[0, col].Tag);
                    DateTime joindtae = Convert.ToDateTime(admdate);
                    DateTime spl_date = Convert.ToDateTime(spl);
                    TimeSpan t = spl_date.Subtract(joindtae);
                    long days = t.Days;
                    int daydiff = Convert.ToInt32(days);
                    if (FpSpread2.Sheets[0].Cells[row, col].Locked != true)// Added by jairam 30-06-2015
                    {
                        if (daydiff > 0)
                        {
                            if (FpSpread2.Sheets[0].Cells[row, col].Text.ToString() != "OD" && FpSpread2.Sheets[0].Cells[row, col].Text.ToString() != "S")
                            {
                                FpSpread2.Sheets[0].Cells[row, col].Text = "P";
                            }
                        }
                        else
                        {
                            if (FpSpread2.Sheets[0].Cells[row, col].Text.ToString() != "OD" && FpSpread2.Sheets[0].Cells[row, col].Text.ToString() != "S")
                            {
                                FpSpread2.Sheets[0].Cells[row, col].Text = "NJ";
                            }
                        }
                    }
                    FpSpread2.SaveChanges();
                }
            }
        }
        presentabsentcount();
    }

    protected void Buttonupdate_Click(object sender, EventArgs e)
    {
    }

    protected void Buttonexit_Click(object sender, EventArgs e)
    {
    }

    protected void Buttonsave_Click(object sender, EventArgs e)
    {
        btn_save();
        presentabsentcount();
    }

    public void btn_save()
    {
        DataSet ds_attend = new DataSet();
        for (int row_Cnt = 1; row_Cnt < FpSpread2.Sheets[0].RowCount - 2; row_Cnt++)
        {
            for (int col = 5; col < FpSpread2.Sheets[0].ColumnCount; col++)
            {
                ds_attend.Clear();
                FpSpread2.SaveChanges();
                string str_rollno = FpSpread2.Sheets[0].GetText(row_Cnt, 1).ToString();
                string date = FpSpread2.Sheets[0].ColumnHeader.Cells[0, col].Text;
                Att_mark = Convert.ToString(FpSpread2.GetEditValue(row_Cnt, col).ToString());
                if (Att_mark == "System.Object")
                {
                    Att_mark = FpSpread2.Sheets[0].Cells[row_Cnt, col].Text.ToString();
                }
                string Att_value = Attvalues(Att_mark);
                if (Att_value == "")
                    Att_value = "0";
                if (Att_value != "0")
                {
                    nullflag = true;
                }
                string[] splitdt = date.Split('/');
                int month_year = Convert.ToInt32(splitdt[2]) * 12 + Convert.ToInt32(splitdt[1]);
                string sql_attendace = "Select * from specialhr_attendance where month_year=" + month_year + " and hrdet_no=" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + " and roll_no='" + str_rollno + "'";
                SqlDataAdapter da_attend = new SqlDataAdapter(sql_attendace, mysql);
                mysql.Open();
                mysql.Close();
                da_attend.Fill(ds_attend);
                string attendance = FpSpread2.Sheets[0].Cells[row_Cnt, col].Text.ToString();
                string appno = d2.GetFunction("select distinct App_No from Registration where Roll_No ='" + str_rollno + "' ");
                if (ds_attend.Tables[0].Rows.Count > 0)
                {

                    if (chkAllstudent.Checked == true)
                    {
                        string qry = "if not exists ( select * from  specialHourStudents where hrdet_no='" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + "' and appNo='" + appno + "' )  insert into specialHourStudents(hrdet_no,appNo) values('" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + "','" + appno + "') else update specialHourStudents set hrdet_no='" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + "',appNo='" + appno + "' where hrdet_no='" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + "' and appNo='" + appno + "' ";
                        int ins = d2.update_method_wo_parameter(qry, "Text");
                    }
                    string updatequery = "update specialhr_attendance set  attendance='" + Att_value + "' where  Roll_no='" + str_rollno.ToString() + "' and month_year=" + month_year + " and hrdet_no=" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + " ";
                    SqlCommand cmd = new SqlCommand(updatequery);
                    mysql1.Open();
                    cmd.Connection = mysql1;
                    cmd.ExecuteNonQuery();
                    mysql1.Close();
                }
                else
                {
                    if (chkAllstudent.Checked == true)
                    {
                        string qry = "if not exists ( select * from  specialHourStudents where hrdet_no='" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + "' and appNo='" + appno + "' )  insert into specialHourStudents(hrdet_no,appNo) values('" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + "','" + appno + "') else update specialHourStudents set hrdet_no='" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + "',appNo='" + appno + "' where hrdet_no='" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + "' and appNo='" + appno + "' ";
                        int ins = d2.update_method_wo_parameter(qry, "Text");
                    }
                    string insert_query = "Insert into specialhr_attendance(roll_no,hrdet_no,attendance,month_year)values('" + str_rollno + "','" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag + "'," + Att_value + "," + month_year + ")";
                    SqlCommand cmd = new SqlCommand(insert_query);
                    mysql1.Open();
                    cmd.Connection = mysql1;
                    cmd.ExecuteNonQuery();
                    mysql1.Close();
                }
            }
        }
        // added and modified by annyutha* 2nd sep 2014****/ 
        if (ds_attend.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated successfully')", true);
        }
        else
        {
            Buttonsave.Text = "Update";
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
        }
        //****end*//
    }

    public string Attmark(string Attstr_mark)
    {
        Att_mark = string.Empty;
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
        else
        {
            Att_mark = "NE";
        }
        //return Convert.ToInt32(Att_mark);
        return Att_mark;
    }

    public string Attvalues(string Att_str1)
    {
        string Attvalue;
        Attvalue = string.Empty;
        if (Att_str1 == "P")
        {
            Attvalue = "1";
        }
        else if (Att_str1 == "A")
        {
            Attvalue = "2";
        }
        else if (Att_str1 == "OD")
        {
            Attvalue = "3";
        }
        else if (Att_str1 == "ML")
        {
            Attvalue = "4";
        }
        else if (Att_str1 == "SOD")
        {
            Attvalue = "5";
        }
        else if (Att_str1 == "NSS")
        {
            Attvalue = "6";
        }
        else if (Att_str1 == "H")
        {
            Attvalue = "7";
        }
        else if (Att_str1 == "NJ")
        {
            Attvalue = "8";
        }
        else if (Att_str1 == "S")
        {
            Attvalue = "9";
        }
        else if (Att_str1 == "L")
        {
            Attvalue = "10";
        }
        else if (Att_str1 == "NCC")
        {
            Attvalue = "11";
        }
        else if (Att_str1 == "HS")
        {
            Attvalue = "12";
        }
        else
        {
            Attvalue = string.Empty;
        }
        return Attvalue;
    }

    protected void ddl_select_subj_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Btngo_Click(sender, e);
        ddl_select_hour.Items.Clear();
        if (ddl_select_subj.SelectedItem.ToString().ToLower() == "all")
        {
            go_click("0");
        }
        else
        {
            bindHour();
            Btngo_Click(sender, e);
        }
        //else
        //{
        //    bindHour();

        //}
    }

    public void presentabsentcount()
    {
        present_calcflag.Clear();
        absent_calcflag.Clear();
        hat.Clear();
        hat.Add("colege_code", Session["collegecode"].ToString());
        ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
        count_master = (ds_attndmaster.Tables[0].Rows.Count);
        if (count_master > 0)
        {
            for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
            {
                if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                {
                    present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                }
                if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                {
                    absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                }
            }
        }
        for (int column = 5; column < FpSpread2.Sheets[0].ColumnCount; column++)
        {
            absent_count = 0;
            present_count = 0;
            for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
            {
                if (FpSpread2.Sheets[0].Cells[Att_mark_row, column].Text.ToString() != "")
                {
                    string attmaktext = FpSpread2.Sheets[0].Cells[Att_mark_row, column].Text.ToString();
                    string attval = Attvalues(attmaktext);
                    FpSpread2.Sheets[0].Cells[Att_mark_row, column].Note = attval;
                    if (present_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, column].Note.ToString()))
                    {
                        present_count++;
                    }
                    if (absent_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, column].Note.ToString()))
                    {
                        absent_count++;
                    }
                }
            }
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, column].CellType = txt;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, column].CellType = txt;
            FpSpread2.Sheets[0].RowHeader.Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = " ";
            FpSpread2.Sheets[0].RowHeader.Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = " ";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, column].Text = present_count.ToString();
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, column].Text = absent_count.ToString();
            Att_mark_column++;
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 2;
        string sections = ddlsec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = string.Empty;
            Session["Sign"] = "" + ddlbatch.SelectedItem.ToString() + "," + ddlbranch.SelectedValue.ToString() + "," + ddlsem.SelectedItem.ToString() + "";
        }
        else
        {
            Session["Sign"] = "" + ddlbatch.SelectedItem.ToString() + "," + ddlbranch.SelectedValue.ToString() + "," + ddlsem.SelectedItem.ToString() + "," + sections + "";
            sections = "- Sec-" + sections;
        }
        string degreedetails = "Student Special Hour Attendance" + '@' + "Degree :" + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlsem.SelectedItem.ToString() + sections + '@' + "Date :" + txtFromDate.Text.ToString() + " To " + TxtToDate.Text.ToString();
        string pagename = "student_special_hours_attendance.aspx";
        Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void ddl_select_hour_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_select_subj.SelectedItem.ToString().ToLower() == "all")
        {
            go_click("0");
        }
        else
        {
            go_click("1");
        }
    }

    protected void bindHour()
    {
        string fromdate = txtFromDate.Text;
        string toDate = TxtToDate.Text;
        string[] fromdatearr = fromdate.Split('-');
        string[] todatearr = toDate.Split('-');
        string sectionqry = string.Empty;
        string subj = string.Empty;
        if (ddlsec.SelectedItem.Text.ToLower().Trim() != "all")
        {
            sectionqry = " and shm.sections='" + ddlsec.SelectedItem.Text + "' ";
        }
        if (ddl_select_subj.SelectedValue.ToString().ToLower().Trim() != "all")
        {
            subj = "and shd.subject_no='" + ddl_select_subj.SelectedValue.ToString().ToLower().Trim() + "'";
        }
       
        string sqlqry = "select  shd.hrdet_no,CONVERT(varchar,shd.start_time,108) + ' - ' +CONVERT(varchar,shd.end_time,108) as Time from  specialhr_master shm,specialhr_details shd where shm.hrentry_no=shd.hrentry_no and  shm.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and shm.semester='" + ddlsem.SelectedItem.Text + "' " + sectionqry + " " + subj + " and shm.date between '" + fromdatearr[2] + "-" + fromdatearr[1] + "-" + fromdatearr[0] + "' and '" + todatearr[2] + "-" + todatearr[1] + "-" + todatearr[0] + "'";

        DataTable dt = dir.selectDataTable(sqlqry);
        if (dt.Rows.Count > 0)
        {
            ddl_select_hour.DataSource = dt;
            ddl_select_hour.DataValueField = "hrdet_no";
            ddl_select_hour.DataTextField = "Time";
            ddl_select_hour.DataBind();
        }
    }
   
}
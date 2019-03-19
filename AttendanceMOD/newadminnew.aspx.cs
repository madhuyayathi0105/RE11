using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;

public partial class newadminnew : System.Web.UI.Page
{
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlCommand cmd = new SqlCommand();
    DataSet ds = new DataSet();
    DataSet ds_attndmaster = new DataSet();
    DAccess2 dacces2 = new DAccess2();
    Hashtable present_calcflag = new Hashtable();
    Hashtable absent_calcflag = new Hashtable();
    Hashtable hat = new Hashtable();
    string no_of_hrs = string.Empty;
    string sch_order = string.Empty;
    string srt_day = string.Empty;
    string startdate = string.Empty;
    string no_days = string.Empty;
    string starting_dayorder = string.Empty;
    string staffcode = string.Empty;
    string strdayflag;
    string genderflag = string.Empty;
    string Att_mark = string.Empty;
    string strorder;
    string groupcode = string.Empty;
    string qry = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;
    static string collegename = string.Empty;
    static string collacronym = string.Empty;
    static string coursename = string.Empty;
    static string minimum_day = string.Empty;
    ArrayList staticarrhourss = new ArrayList();
    static ArrayList countarray = new ArrayList();
    Boolean update_flag = false;
    Boolean nullflag = false;
    Boolean serialflag = false;
    int Att_mark_row;
    int Att_mark_column;
    int no_hrs = 0, nodays = 0;
    int present_count = 0, absent_count = 0;
    int count_master = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!IsPostBack)
            {
                rbincludeonduty.Checked = true;
                rbexcludeonduty.Checked = false;
                rbcommon.Checked = true;
                btngvsave.Visible = false;
                chkIncludeRedoStudent.Checked = false;
                chkIncludeRedoStudent.Visible = false;
                string grouporusercode = string.Empty;
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                DataSet schoolds = new DataSet();
                string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = dacces2.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                    if (schoolvalue.Trim() == "0")
                    {
                        lblbatch.Text = "Year";
                        lbldegree.Text = "School Type";
                        lblbranch.Text = "Standard";
                        lblsem.Text = "Term";
                    }
                }
                txtFromDate.Attributes.Add("Readonly", "Readonly");
                update_flag = false;
                txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["AdmissionNo"] = "0";
                Session["Sex"] = "0";
                Session["flag"] = "-1";
                string Master1 = string.Empty;
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    Master1 = "select * from Master_Settings where group_code=" + Session["group_code"] + "";
                }
                else
                {
                    Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                }
                DataSet dsmaseter = dacces2.select_method(Master1, hat, "Text");
                string regularflag = string.Empty;
                if (dsmaseter.Tables.Count > 0 && dsmaseter.Tables[0].Rows.Count > 0)
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
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["AdmissionNo"] = "1";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "sex" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Sex"] = "1";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "General" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["flag"] = 0;
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "As Per Lesson" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["flag"] = 1;
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Male" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            genderflag = " and (a.sex='0'";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Female" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
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
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Days Scholor" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (strdayflag != null && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or registration.Stud_Type='Day Scholar'";
                            }
                            else
                            {
                                strdayflag = " and (registration.Stud_Type='Day Scholar'";
                            }
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Hostel" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
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
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Regular")
                        {
                            regularflag = "and ((registration.mode=1)";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Lateral")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=3)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=3)";
                            }
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Transfer")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=2)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=2)";
                            }
                        }
                    }
                }
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
                bindstram();
                bindeducation();
                BindBatch();
                Boolean lockflag = false;
                if (ddlbatch.Items.Count > 0)
                {
                    lockflag = true;
                }
                string collegecode = Session["collegecode"].ToString();
                string usercode = Session["usercode"].ToString();
                groupcode = Session["group_code"].ToString();
                Session["witotttname"] = "0";
                string getshedulockva = dacces2.GetFunctionv("select value from Master_Settings where  settings='Attendance with out timetable'");
                if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                {
                    Session["witotttname"] = "1";
                }
                Session["Deptwisesubject"] = "0";
                getshedulockva = dacces2.GetFunctionv("select value from Master_Settings where  settings='Subject Based on Department Rights'");
                if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                {
                    Session["Deptwisesubject"] = "1";
                }
                Bind_Degree();
                if (ddldegree.Items.Count > 0 && lockflag == true)
                {
                    ddldegree.Enabled = true;
                    ddlbranch.Enabled = true;
                    ddlsem.Enabled = true;
                    txtFromDate.Enabled = true;
                    ddlsec.Enabled = true;
                    Btngo.Enabled = true;
                    groupcode = Session["group_code"].ToString();
                    Bind_Dept();
                    bindsem();
                }
                else
                {
                    ddldegree.Enabled = false;
                    ddlbranch.Enabled = false;
                    ddlsem.Enabled = false;
                    txtFromDate.Enabled = false;
                    ddlsec.Enabled = false;
                    Btngo.Enabled = false;
                }
                Radio_CheckedChanged(sender, e);
                ChangeDefaultOnDuty();
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void bindstram()
    {
        try
        {
            string collegecode = Session["collegecode"].ToString();
            DataSet ds = dacces2.select_method_wo_parameter("select distinct type from Course where isnull(type,'')<>'' and college_code='" + collegecode + "'", "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
            }
            else
            {
                ddlstream.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void BindBatch()
    {
        try
        {
            string Master1 = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string collegecode = Session["collegecode"].ToString();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' order by batch_year desc";
            DataSet ds = dacces2.select_method_wo_parameter(strbinddegree, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void bindeducation()
    {
        try
        {
            ddlcourse.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            string typeval = string.Empty;
            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            {
                typeval = " and course.type='" + ddlstream.SelectedItem.ToString() + "'";
            }
            string query = string.Empty;
            if ((group_code.ToString().Trim() != "") && (group_code.Trim() != "0") && (group_code.ToString().Trim() != "-1"))
            {
                query = "select distinct course.Edu_Level from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_code + "' " + typeval + "";
            }
            else
            {
                query = "select distinct course.Edu_Level from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' " + typeval + "";
            }
            DataSet ds = new DataSet();
            ds = dacces2.select_method(query, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcourse.DataSource = ds;
                ddlcourse.DataValueField = "Edu_Level";
                ddlcourse.DataTextField = "Edu_Level";
                ddlcourse.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void Bind_Degree()
    {
        try
        {
            ddldegree.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            string typeval = string.Empty;
            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            {
                typeval = " and course.type='" + ddlstream.SelectedItem.ToString() + "'";
            }
            string query = string.Empty;
            if (ddlcourse.Items.Count > 0)
            {
                if ((group_code.ToString().Trim() != "") && (group_code.Trim() != "0") && (group_code.ToString().Trim() != "-1"))
                {
                    query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and course.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and group_code=" + group_code + " " + typeval + "";
                }
                else
                {
                    query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and course.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and user_code=" + usercode + " " + typeval + "";
                }
                DataSet ds = new DataSet();
                ds = dacces2.select_method(query, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddldegree.DataSource = ds;
                    ddldegree.DataValueField = "course_id";
                    ddldegree.DataTextField = "course_name";
                    ddldegree.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void Bind_Dept()
    {
        try
        {
            ddlbranch.Items.Clear();
            if (ddldegree.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string usercode = Session["usercode"].ToString();
                string group_code = Session["group_code"].ToString();
                if (group_code.Contains(';'))
                {
                    string[] group_semi = group_code.Split(';');
                    group_code = group_semi[0].ToString();
                }
                string degree_code = ddldegree.SelectedValue.ToString();
                string query = string.Empty;
                if ((group_code.ToString().Trim() != "") && (group_code.Trim() != "0") && (group_code.ToString().Trim() != "-1"))
                {
                    query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_code + "";
                }
                else
                {
                    query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "";
                }
                DataSet ds = new DataSet();
                ds = dacces2.select_method(query, hat, "Text");
                ddlbranch.DataSource = ds;
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void bindsem()
    {
        try
        {
            ddlsem.Items.Clear();
            if (ddlbranch.Items.Count > 0 && ddlbatch.Items.Count > 0)
            {
                Boolean first_year;
                first_year = false;
                int duration = 0;
                int i = 0;
                con.Close();
                con.Open();
                SqlDataReader dr;
                cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.Text.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
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
                con.Close();
                bindhours();
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            ddlsec.Items.Clear();
            string branch = ddlbranch.SelectedValue.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            if (ddlbranch.Items.Count > 0 && ddlbatch.Items.Count > 0)
            {
                string sctsre = "select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and isnull(ProLongAbsent,0)<>'1' and isnull(ProLongAbsent,0)<>'1' and exam_flag<>'Debar' order by sections";
                DataSet ds = dacces2.select_method_wo_parameter(sctsre, "Text");
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataValueField = "sections";
                ddlsec.DataBind();
                ddlsec.Items.Insert(0, "All");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["sections"].ToString() == "")
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
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void bindhours()
    {
        try
        {
            txtcopyto.Text = "---Select---";
            chkcopyto.Checked = false;
            chklscopyto.Items.Clear();
            ddlcopyfrom.Items.Clear();
            chkhour.Enabled = true;
            txthour.Text = "---Select---";
            chkhour.Checked = false;
            chklshour.Items.Clear();
            string date1 = txtFromDate.Text.ToString();
            string[] split = date1.Split(new Char[] { '-' });
            string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            if (rbcommon.Checked == true)
            {
                string strquery = dacces2.GetFunction("select No_of_hrs_per_day from seminfo s,periodattndschedule p where s.degree_code=p.degree_code and s.semester=p.semester and batch_year=" + ddlbatch.Text.ToString() + " and s.degree_code=" + ddlbranch.SelectedValue.ToString() + " and s.semester=" + ddlsem.SelectedValue.ToString() + "");
                int minhrsckl = 0;
                if (strquery.Trim() != "" && strquery != null)
                {
                    minhrsckl = Convert.ToInt32(strquery);
                    for (int i = 0; i < minhrsckl; i++)
                    {
                        chklshour.Items.Add(Convert.ToString(i + 1));
                        chklshour.Items[i].Selected = true;
                        chklshour.Items[i].Enabled = true;
                    }
                }
                chkhour.Checked = true;
                txthour.Text = "Hours(" + chklshour.Items.Count + ")";
            }
            else
            {
                chkhour.Enabled = false;
                string typeval = string.Empty;
                if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
                {
                    typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
                }
                //and Degree_code='"+Convert.ToString(ddl)+"'
                string qryConsiderDayOrder = "SELECT tbc.DayOrder,tbc.Degree_code,tbc.Batch_year,tbc.Semester,tbc.From_Date,c.Edu_Level FROM tbl_consider_day_order tbc,Degree dg,Course c where dg.Degree_Code=tbc.Degree_code and dg.Course_Id=c.Course_Id and c.Edu_Level='" + Convert.ToString(ddlcourse.SelectedItem.Text).Trim() + "' and  tbc.From_Date='" + datefrom + "' and tbc.Batch_year='" + Convert.ToString(ddlbatch.Text) + "'  and tbc.Semester='" + Convert.ToString(ddlsem.SelectedValue).Trim() + "' and isnull(tbc.DayOrder,'0')<>'0' " + typeval;
                DataSet dsConsiderDayOrder = new DataSet();
                dsConsiderDayOrder = dacces2.select_method_wo_parameter(qryConsiderDayOrder, "Text");
                string strquery = "select s.degree_code,start_date,isnull(starting_dayorder,1) as starting_dayorder,schorder,nodays,No_of_hrs_per_day,min_hrs_per_day from seminfo s,periodattndschedule p,Degree d,Course c where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and batch_year=" + ddlbatch.Text.ToString() + " and s.semester=" + ddlsem.SelectedValue.ToString() + "";
                int minhrsckl = 0;
                ds = dacces2.select_method(strquery, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    minhrsckl = Convert.ToInt32(ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString());
                    for (int i = 0; i < minhrsckl; i++)
                    {
                        chklshour.Items.Add(Convert.ToString(i + 1));
                        chklshour.Items[i].Selected = false;
                        if (Session["witotttname"] == "0")
                        {
                            chklshour.Items[i].Enabled = false;
                        }
                        else
                        {
                            chklshour.Items[i].Enabled = true;
                            chkhour.Enabled = true;
                        }
                    }
                    if (ddlsubject.Items.Count > 0)
                    {
                        string scheduloredr = ds.Tables[0].Rows[0]["schorder"].ToString();
                        string start_datesem = ds.Tables[0].Rows[0]["start_date"].ToString();
                        string noofdays = ds.Tables[0].Rows[0]["nodays"].ToString();
                        string start_dayorder = ds.Tables[0].Rows[0]["starting_dayorder"].ToString();
                        string degreecode = ds.Tables[0].Rows[0]["degree_code"].ToString();
                        string datestr = txtFromDate.Text.ToString();
                        string[] spdt = datestr.Split('-');
                        DateTime dtf = Convert.ToDateTime(spdt[1] + '/' + spdt[0] + '/' + spdt[2]);
                        string strday = dtf.ToString("ddd");
                        if (scheduloredr == "0")
                        {
                            strday = dacces2.findday(dtf.ToString(), degreecode, ddlsem.SelectedValue.ToString(), ddlbatch.Text.ToString(), start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                            DataView dvConsiderDayOrder = new DataView();
                            if (dsConsiderDayOrder.Tables.Count > 0 && dsConsiderDayOrder.Tables[0].Rows.Count > 0)
                            {
                                dsConsiderDayOrder.Tables[0].DefaultView.RowFilter = "Degree_code='" + degreecode + "'";
                                dvConsiderDayOrder = dsConsiderDayOrder.Tables[0].DefaultView;
                            }
                            if (dvConsiderDayOrder.Count > 0)
                            {
                                byte dayOrderConsider = 0;
                                string considerDayOrder = string.Empty;
                                considerDayOrder = Convert.ToString(dvConsiderDayOrder[0]["DayOrder"]).Trim();
                                byte.TryParse(considerDayOrder.Trim(), out dayOrderConsider);
                                strday = findDayName(dayOrderConsider);
                            }
                            if (string.IsNullOrEmpty(strday))
                            {
                                strday = dacces2.findday(dtf.ToString(), degreecode, ddlsem.SelectedValue.ToString(), ddlbatch.Text.ToString(), start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                            }
                        }
                        string strgetsubno = "select s.subject_code,s.subject_name,s.subject_no from subject s,syllabus_master sy,Degree d,Course c where s.syll_code=sy.syll_code and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and sy.batch_year=" + ddlbatch.Text.ToString() + " and sy.semester=" + ddlsem.SelectedValue.ToString() + " and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' ";
                        DataSet dssub = dacces2.select_method_wo_parameter(strgetsubno, "Text");
                        Hashtable hatsub = new Hashtable();
                        for (int sy = 0; sy < dssub.Tables[0].Rows.Count; sy++)
                        {
                            string suno = dssub.Tables[0].Rows[sy]["subject_no"].ToString();
                            if (!hatsub.Contains(suno))
                            {
                                hatsub.Add(suno, suno);
                            }
                        }
                        if (Session["witotttname"] == "0")
                        {
                            Hashtable hathour = new Hashtable();
                            int noofhrsubject = 0;
                            string strquerysemsche = "Select s.* from Semester_Schedule s,Degree d,Course c where s.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and s.batch_year='" + ddlbatch.Text.ToString() + "' and s.semester='" + ddlsem.SelectedValue.ToString() + "'";
                            DataSet dssem = dacces2.select_method_wo_parameter(strquerysemsche, "Text");
                            if (dssem.Tables[0].Rows.Count > 0)
                            {
                                for (int s = 0; s < dssem.Tables[0].Rows.Count; s++)
                                {
                                    for (int i = 0; i < minhrsckl; i++)
                                    {
                                        string dayval = strday + (i + 1);
                                        string getpwer = dssem.Tables[0].Rows[s][dayval].ToString();
                                        if (getpwer.Trim() != "")
                                        {
                                            string[] sph = getpwer.Split(';');
                                            for (int sp = 0; sp <= sph.GetUpperBound(0); sp++)
                                            {
                                                if (sph[sp].Trim() != "")
                                                {
                                                    string[] gteval = sph[sp].Split('-');
                                                    if (gteval.GetUpperBound(0) > 0)
                                                    {
                                                        string subno = gteval[0].ToString();
                                                        if (hatsub.Contains(subno))
                                                        {
                                                            chklshour.Items[i].Selected = true;
                                                            chklshour.Items[i].Enabled = true;
                                                            if (!hathour.Contains(chklshour.Items[i].Value.ToString()))
                                                            {
                                                                noofhrsubject++;
                                                                hathour.Add(chklshour.Items[i].Value.ToString(), chklshour.Items[i].Value.ToString());
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (noofhrsubject > 0)
                                {
                                    txthour.Text = "Hours(" + noofhrsubject + ")";
                                    if (noofhrsubject == chklshour.Items.Count)
                                    {
                                        chkhour.Checked = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Hashtable hathour = new Hashtable();
                            int noofhrsubject = 0;
                            string strquerysemsche = "Select s.* from Alternate_Schedule s,Degree d,Course c where s.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and s.batch_year='" + ddlbatch.Text.ToString() + "' and s.semester='" + ddlsem.SelectedValue.ToString() + "' and FromDate='" + datefrom + "' ";
                            DataSet dssem = dacces2.select_method_wo_parameter(strquerysemsche, "Text");
                            if (dssem.Tables[0].Rows.Count > 0)
                            {
                                for (int s = 0; s < dssem.Tables[0].Rows.Count; s++)
                                {
                                    for (int i = 0; i < minhrsckl; i++)
                                    {
                                        string dayval = strday + (i + 1);
                                        string getpwer = dssem.Tables[0].Rows[s][dayval].ToString();
                                        if (getpwer.Trim() != "")
                                        {
                                            string[] sph = getpwer.Split(';');
                                            for (int sp = 0; sp <= sph.GetUpperBound(0); sp++)
                                            {
                                                if (sph[sp].Trim() != "")
                                                {
                                                    string[] gteval = sph[sp].Split('-');
                                                    if (gteval.GetUpperBound(0) > 0)
                                                    {
                                                        string subno = gteval[0].ToString();
                                                        if (hatsub.Contains(subno))
                                                        {
                                                            chklshour.Items[i].Selected = true;
                                                            chklshour.Items[i].Enabled = true;
                                                            if (!hathour.Contains(chklshour.Items[i].Value.ToString()))
                                                            {
                                                                noofhrsubject++;
                                                                hathour.Add(chklshour.Items[i].Value.ToString(), chklshour.Items[i].Value.ToString());
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (noofhrsubject > 0)
                                {
                                    txthour.Text = "Hours(" + noofhrsubject + ")";
                                    if (noofhrsubject == chklshour.Items.Count)
                                    {
                                        chkhour.Checked = true;
                                    }
                                }
                            }
                            if (hathour.Count == 0)
                            {
                                hathour.Clear();
                                noofhrsubject = 0; strquerysemsche = string.Empty;
                                strquerysemsche = "Select s.* from Semester_Schedule s,Degree d,Course c where s.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and s.batch_year='" + ddlbatch.Text.ToString() + "' and s.semester='" + ddlsem.SelectedValue.ToString() + "'";
                                dssem.Clear();
                                dssem = dacces2.select_method_wo_parameter(strquerysemsche, "Text");
                                if (dssem.Tables[0].Rows.Count > 0)
                                {
                                    for (int s = 0; s < dssem.Tables[0].Rows.Count; s++)
                                    {
                                        for (int i = 0; i < minhrsckl; i++)
                                        {
                                            string dayval = strday + (i + 1);
                                            string getpwer = dssem.Tables[0].Rows[s][dayval].ToString();
                                            if (getpwer.Trim() != "")
                                            {
                                                string[] sph = getpwer.Split(';');
                                                for (int sp = 0; sp <= sph.GetUpperBound(0); sp++)
                                                {
                                                    if (sph[sp].Trim() != "")
                                                    {
                                                        string[] gteval = sph[sp].Split('-');
                                                        if (gteval.GetUpperBound(0) > 0)
                                                        {
                                                            string subno = gteval[0].ToString();
                                                            if (hatsub.Contains(subno))
                                                            {
                                                                chklshour.Items[i].Selected = true;
                                                                chklshour.Items[i].Enabled = true;
                                                                if (!hathour.Contains(chklshour.Items[i].Value.ToString()))
                                                                {
                                                                    noofhrsubject++;
                                                                    hathour.Add(chklshour.Items[i].Value.ToString(), chklshour.Items[i].Value.ToString());
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (noofhrsubject > 0)
                                    {
                                        txthour.Text = "Hours(" + noofhrsubject + ")";
                                        if (noofhrsubject == chklshour.Items.Count)
                                        {
                                            chkhour.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            txtcopyto.Text = "---Select---";
            string hourval = string.Empty;
            int commcount = 0;
            if (chklshour.Items.Count > 0)
            {
                int noh = 0;
                for (int sj = 0; sj < chklshour.Items.Count; sj++)
                {
                    if (chklshour.Items[sj].Selected == true)
                    {
                        ddlcopyfrom.Items.Add(chklshour.Items[sj].Value.ToString());
                        chklscopyto.Items.Add(chklshour.Items[sj].Value.ToString());
                        chklscopyto.Items[noh].Selected = true;
                        commcount = commcount + 1;
                        if (hourval.Trim() == "")
                        {
                            hourval = ddlcopyfrom.Items[noh].Text.ToString();
                        }
                        else
                        {
                            hourval = hourval + ", " + ddlcopyfrom.Items[noh].Text.ToString();
                        }
                        noh++;
                    }
                }
                if (noh > 0)
                {
                    //txtcopyto.Text = "Hour (" + chklshour.Items.Count.ToString() + ")";
                    txtcopyto.Text = hourval;
                    chkcopyto.Checked = true;
                }
            }
            hourval = string.Empty;
            commcount = 0;
            for (int i = 0; i < chklshour.Items.Count; i++)
            {
                if (chklshour.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    if (hourval.Trim() == "")
                    {
                        hourval = chklshour.Items[i].Text.ToString();
                    }
                    else
                    {
                        hourval = hourval + ", " + chklshour.Items[i].Text.ToString();
                    }
                }
            }
            if (commcount > 0)
            {
                //txthour.Text = "Hours(" + commcount.ToString() + ")";
                txthour.Text = hourval;
                if (commcount == chklshour.Items.Count)
                {
                    chkhour.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void bindsubtype()
    {
        try
        {
            ddlsubtype.Items.Clear();
            if (rbelective.Checked == true)
            {
                string grouporusercode = string.Empty;
                string typeval = string.Empty;
                if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
                {
                    typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
                }
                string usercode = Session["usercode"].ToString();
                string group_code = Session["group_code"].ToString();
                if (group_code.Contains(';'))
                {
                    string[] group_semi = group_code.Split(';');
                    group_code = group_semi[0].ToString();
                }
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " and dp.group_code='" + group_code + "'";
                }
                else
                {
                    grouporusercode = " and dp.user_code='" + usercode + "'";
                }
                string sctsre = "select distinct ss.subject_type from syllabus_master sy,sub_sem ss,Degree d,Course c where sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and sy.syll_code=ss.syll_code and sy.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and sy.semester='" + ddlsem.SelectedValue.ToString() + "'";
                if (Session["Deptwisesubject"].ToString() == "1")
                {
                    sctsre = "select distinct ss.subject_type from syllabus_master sy,sub_sem ss,Degree d,Course c,Department de,DeptPrivilages dp,subject s where sy.degree_code=d.Degree_Code and dp.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and ss.subType_no=s.subType_no and s.dept_code=de.Dept_Code and d.Course_Id=c.Course_Id and sy.syll_code=ss.syll_code and sy.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and sy.semester='" + ddlsem.SelectedValue.ToString() + "' " + grouporusercode + "";
                }
                DataSet ds = dacces2.select_method_wo_parameter(sctsre, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlsubtype.DataSource = ds;
                    ddlsubtype.DataTextField = "subject_type";
                    ddlsubtype.DataValueField = "subject_type";
                    ddlsubtype.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void bindsubject()
    {
        try
        {
            ddlsubject.Items.Clear();
            if (rbelective.Checked == true && ddlsubtype.Items.Count > 0)
            {
                string grouporusercode = string.Empty;
                string usercode = Session["usercode"].ToString();
                string group_code = Session["group_code"].ToString();
                if (group_code.Contains(';'))
                {
                    string[] group_semi = group_code.Split(';');
                    group_code = group_semi[0].ToString();
                }
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " and dp.group_code='" + group_code + "'";
                }
                else
                {
                    grouporusercode = " and dp.user_code='" + usercode + "'";
                }
                string typeval = string.Empty;
                if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
                {
                    typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
                }
                string sctsre = "select distinct s.subject_name,s.subject_code from syllabus_master sy,sub_sem ss,Degree d,Course c,subject s  where sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and sy.semester='" + ddlsem.SelectedValue.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "'";
                if (Session["Deptwisesubject"].ToString() == "1")
                {
                    sctsre = "select distinct s.subject_name,s.subject_code from syllabus_master sy,sub_sem ss,Degree d,Course c,Department de,DeptPrivilages dp,subject s where sy.degree_code=d.Degree_Code and dp.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and ss.subType_no=s.subType_no and s.dept_code=de.Dept_Code and d.Course_Id=c.Course_Id and sy.syll_code=ss.syll_code and sy.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' and sy.semester='" + ddlsem.SelectedValue.ToString() + "' " + grouporusercode + "";
                }
                DataSet ds = dacces2.select_method_wo_parameter(sctsre, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlsubject.DataSource = ds;
                    ddlsubject.DataTextField = "subject_name";
                    ddlsubject.DataValueField = "subject_code";
                    ddlsubject.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void loadstubatch()
    {
        try
        {
            ddlstubatch.Items.Clear();
            ddlstubatch.Enabled = false;
            if (rbelective.Checked == true && ddlsubject.Items.Count > 0)
            {
                string typeval = string.Empty;
                if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
                {
                    typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
                }
                string sqlstr = "select distinct sc.batch from Registration r,subject s,subjectChooser sc,Degree d,course c,syllabus_master sy where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and s.syll_code=sy.syll_code and r.Current_Semester=sc.semester " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "' and r.cc=0 and r.delflag=0 and isnull(r.ProLongAbsent,0)<>'1' and r.Exam_Flag<>'debar' and isnull(sc.batch,'')<>'' order by sc.batch";
                DataSet gvds = dacces2.select_method_wo_parameter(sqlstr, "text");
                if (gvds.Tables.Count > 0 && gvds.Tables[0].Rows.Count > 0)
                {
                    ddlstubatch.DataSource = gvds;
                    ddlstubatch.DataTextField = "batch";
                    ddlstubatch.DataValueField = "batch";
                    ddlstubatch.DataBind();
                    ddlstubatch.Enabled = true;
                    ddlstubatch.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void load_spread()
    {
        lblset.Visible = false;
    }

    protected void ddlstream_SelectedIndexChanged(object sender, EventArgs e)
    {
        hiddenfields();
        bindeducation();
        Bind_Degree();
        if (ddldegree.Items.Count > 0)
        {
            Bind_Dept();
            bindsem();
            BindSectionDetail();
            bindhours();
            bindsubtype();
            bindsubject();
            loadstubatch();
            ddldegree.Enabled = true;
            ddlbranch.Enabled = true;
            ddlsem.Enabled = true;
            txtFromDate.Enabled = true;
            ddlsec.Enabled = true;
            Btngo.Enabled = true;
        }
        else
        {
            ddldegree.Enabled = false;
            ddlbranch.Enabled = false;
            ddlsem.Enabled = false;
            txtFromDate.Enabled = false;
            ddlsec.Enabled = false;
            Btngo.Enabled = false;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        hiddenfields();
        Bind_Degree();
        if (ddldegree.Items.Count > 0)
        {
            Bind_Dept();
            bindsem();
            BindSectionDetail();
            bindhours();
            bindsubtype();
            bindsubject();
            loadstubatch();
        }
    }

    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        hiddenfields();
        Bind_Degree();
        if (ddldegree.Items.Count > 0)
        {
            Bind_Dept();
            bindsem();
            BindSectionDetail();
            bindhours();
            bindsubtype();
            bindsubject();
            loadstubatch();
            ddldegree.Enabled = true;
            ddlbranch.Enabled = true;
            ddlsem.Enabled = true;
            txtFromDate.Enabled = true;
            ddlsec.Enabled = true;
            Btngo.Enabled = true;
        }
        else
        {
            ddldegree.Enabled = false;
            ddlbranch.Enabled = false;
            ddlsem.Enabled = false;
            txtFromDate.Enabled = false;
            ddlsec.Enabled = false;
            Btngo.Enabled = false;
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_spread();
        hiddenfields();
        Bind_Dept();
        bindsem();
        BindSectionDetail();
        bindhours();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_spread();
        hiddenfields();
        bindsem();
        BindSectionDetail();
        bindhours();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        hiddenfields();
        load_spread();
        BindSectionDetail();
        bindhours();
        bindsubtype();
        bindsubject();
        loadstubatch();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        hiddenfields();
    }

    protected void ddlsubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        hiddenfields();
        bindsubject();
        bindhours();
        loadstubatch();
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        hiddenfields();
        bindhours();
        loadstubatch();
    }

    protected void ddlstubatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        hiddenfields();
    }

    protected void Radio_CheckedChanged(object sender, EventArgs e)
    {
        hiddenfields();
        chkIncludeRedoStudent.Visible = false;
        if (rbcommon.Checked == true)
        {
            chkIncludeRedoStudent.Checked = false;
            chkIncludeRedoStudent.Visible = true;
            lblsec.Visible = true;
            ddlsec.Visible = true;
            lbldegree.Visible = true;
            ddldegree.Visible = true;
            lblbranch.Visible = true;
            ddlbranch.Visible = true;
            lblsubtype.Visible = false;
            ddlsubtype.Visible = false;
            lblsubjcet.Visible = false;
            ddlsubject.Visible = false;
            lblstubatch.Visible = false;
            ddlstubatch.Visible = false;
            Bind_Degree();
            if (ddldegree.Items.Count > 0)
            {
                Bind_Dept();
                bindsem();
                BindSectionDetail();
                bindhours();
            }
        }
        else
        {
            lblsec.Visible = false;
            ddlsec.Visible = false;
            lbldegree.Visible = false;
            ddldegree.Visible = false;
            lblbranch.Visible = false;
            ddlbranch.Visible = false;
            lblsubtype.Visible = true;
            ddlsubtype.Visible = true;
            lblsubjcet.Visible = true;
            ddlsubject.Visible = true;
            lblstubatch.Visible = true;
            ddlstubatch.Visible = true;
            bindsubtype();
            bindsubject();
            bindhours();
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        hiddenfields();
        string date1 = string.Empty;
        string datefrom = string.Empty;
        lblset.Visible = false;
        lblset.Visible = false;
        if (txtFromDate.Text == "")
        {
            lblset.Text = "Select From Date";
            lblset.Visible = true;
            return;
        }
        date1 = txtFromDate.Text.ToString();
        string[] split = date1.Split(new Char[] { '-' });
        datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
        if (dt1 > DateTime.Today)
        {
            lblset.Visible = true;
            lblset.Text = "You can not mark attendance for the date greater than today";
            return;
        }
        else
        {
            lblset.Visible = false;
        }
        bindhours();
    }

    protected void Ckhour_SelectedIndexChanged(object sender, EventArgs e)
    {
        hiddenfields();
    }

    protected void Btngo_Click(object sender, EventArgs e)
    {
        try
        {
            SaveDefaultOnDuty();
            if (rbcommon.Checked == true)
            {
                loadcommstudent();
            }
            else
            {
                loadSubjectwisestudent();
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void loadcommstudent()
    {
        try
        {
            string stodstudetails = string.Empty;
            gvatte.Visible = false;
            GVhead.Visible = false;
            btngvsave.Visible = false;
            lblset.Text = string.Empty;
            lblset.Visible = false;
            serialflag = false;
            bool isRoll = false;
            bool isRegNo = false;
            bool isStudName = false;
            DataSet dsholiday = new DataSet();
            loadcollegename();
            int chlhourscount = 0;
            ArrayList arrhours = new ArrayList();
            string hourdetails = string.Empty;
            string qryIncludeRedo = string.Empty;
            if (!chkIncludeRedoStudent.Checked)
                qryIncludeRedo = " and r.cc='0' ";
            for (int i = 0; i < chklshour.Items.Count; i++)
            {
                if (chklshour.Items[i].Selected == true)
                {
                    arrhours.Add(chklshour.Items[i].Text);
                    chlhourscount++;
                    if (hourdetails.Trim() == "")
                    {
                        hourdetails = " s.hourse like '%" + chklshour.Items[i].Text + "%'";
                    }
                    else
                    {
                        hourdetails = hourdetails + " or s.hourse like '%" + chklshour.Items[i].Text + "%'";
                    }
                }
            }
            if (hourdetails.Trim() != "")
            {
                hourdetails = " and (" + hourdetails + ")";
            }
            if (staffcode == "" || staffcode == null)
            {
                if (txtFromDate.Text != "")
                {
                    lblset.Visible = false;
                    string strsec = string.Empty;
                    string sec = string.Empty;
                    string secrights = string.Empty;
                    if (ddlsec.Enabled == true)
                    {
                        if (ddlsec.Text.ToString().ToLower().Trim() == "all" || ddlsec.Text.ToString().ToLower().Trim() == "" || ddlsec.Text.ToString().ToLower().Trim() == "-1")
                        {
                            strsec = string.Empty;
                        }
                        else
                        {
                            strsec = " and r.sections='" + ddlsec.SelectedValue.ToString() + "'";
                            sec = " and r.sections='" + ddlsec.SelectedValue.ToString() + "'";
                            secrights = ddlsec.SelectedValue.ToString();
                        }
                    }
                    Boolean secrightsflag = false;
                    string collegecode = Session["collegecode"].ToString();
                    string ucode = string.Empty;
                    string code = string.Empty;
                    string group_code = Session["group_code"].ToString();
                    if (group_code.Contains(';'))
                    {
                        string[] group_semi = group_code.Split(';');
                        group_code = group_semi[0].ToString();
                    }
                    if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString().Trim().ToLower() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                    {
                        ucode = group_code;
                        code = "group_code=" + ucode + "";
                    }
                    else
                    {
                        ucode = Session["usercode"].ToString();
                        code = "usercode=" + ucode + "";
                    }
                    string strgetsec = dacces2.GetFunction("select sections from tbl_attendance_rights where batch_year='" + ddlbatch.SelectedItem.ToString() + "' and user_id='" + ucode + "'");
                    if (strgetsec.Trim() != null && strgetsec.Trim() != "0")
                    {
                        string[] spsec = strgetsec.Split(',');
                        for (int sp = 0; sp <= spsec.GetUpperBound(0); sp++)
                        {
                            string valu = spsec[sp].ToString();
                            if (secrights.Trim().ToLower() == valu.Trim().ToLower())
                            {
                                secrightsflag = true;
                            }
                        }
                    }
                    if (secrightsflag == false)
                    {
                        lblset.Visible = true;
                        lblset.Text = "Please Set Rights For The User";
                        return;
                    }
                    string date1 = string.Empty;
                    string date2 = string.Empty;
                    string datefrom;
                    string dateto = string.Empty;
                    date1 = txtFromDate.Text.ToString();
                    string[] split = date1.Split(new Char[] { '-' });
                    datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
                    date2 = txtFromDate.Text.ToString();
                    string[] split1 = date2.Split(new Char[] { '-' });
                    dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
                    Dictionary<string, DateTime[]> dicFeeOfRollStudents = new Dictionary<string, DateTime[]>();
                    Dictionary<string, byte> dicFeeOnRollStudents = new Dictionary<string, byte>();
                    GetFeeOfRollStudent(ref dicFeeOfRollStudents, ref dicFeeOnRollStudents);
                    DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                    DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                    TimeSpan t = dt2.Subtract(dt1);
                    long days = t.Days;
                    if (days < 0)
                    {
                        lblset.Visible = true;
                        lblset.Text = "From date should be less than To date";
                        return;
                    }
                    string strholiday = "select * from holidaystudents where holiday_date='" + dt2.ToString() + "'and degree_code=" + 
ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "";
                    DataSet dsholidays = dacces2.select_method(strholiday, hat, "Text");
                    if (dsholidays.Tables.Count > 0 && dsholidays.Tables[0].Rows.Count > 0)
                    {
                        lblset.Visible = true;
                        lblset.Text = "Selected Day is Sunday";
                        return;
                    }
                    //if (days == 0 && dt1.ToString("dddd") == "Sunday")//magesh 9.10.18
                    //{
                    //    lblset.Visible = true;
                    //    lblset.Text = "Selected Day is Sunday";
                    //    return;
                    //}
                    if (dt1 > DateTime.Today)
                    {
                        lblset.Visible = true;
                        lblset.Text = "You can not mark attendance for the date greater than today";
                        txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                        return;
                    }
                    else
                    {
                        lblset.Visible = false;
                    }
                    ds.Reset();
                    ds.Dispose();
                    string strquery = "select start_date,isnull(starting_dayorder,1) as starting_dayorder,schorder,nodays,No_of_hrs_per_day,min_hrs_per_day from seminfo s,periodattndschedule p where s.degree_code=p.degree_code and s.semester=p.semester and batch_year=" + ddlbatch.Text.ToString() + " and s.degree_code=" + ddlbranch.SelectedValue.ToString() + " and s.semester=" + ddlsem.SelectedValue.ToString() + "";
                    ds = dacces2.select_method(strquery, hat, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        sch_order = ds.Tables[0].Rows[0]["schorder"].ToString();
                        no_days = ds.Tables[0].Rows[0]["nodays"].ToString();
                        startdate = ds.Tables[0].Rows[0]["start_date"].ToString();
                        starting_dayorder = ds.Tables[0].Rows[0]["starting_dayorder"].ToString();
                        no_of_hrs = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                    }
                    if (no_of_hrs.Trim() != "")
                    {
                        no_hrs = Convert.ToInt16(no_of_hrs);
                    }
                    else
                    {
                        no_hrs = 0;
                    }
                    nodays = Convert.ToInt16(nodays);
                    if (starting_dayorder == "")
                    {
                        starting_dayorder = "1";
                    }
                    lblset.Visible = false;
                    strorder = " ORDER BY r.Roll_No";
                    string serialno = dacces2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                    if (serialno.Trim() == "1")
                    {
                        serialflag = true;
                        strorder = "ORDER BY r.serialno";
                    }
                    else
                    {
                        serialflag = false;
                        string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");
                        if (orderby_Setting == "0")
                        {
                            strorder = "ORDER BY r.Roll_No";
                            isRoll = true;
                            isRegNo = false;
                            isStudName = false;
                        }
                        else if (orderby_Setting == "1")
                        {
                            strorder = "ORDER BY r.Reg_No";
                            isRegNo = true;
                            isRoll = false;
                            isStudName = false;
                        }
                        else if (orderby_Setting == "2")
                        {
                            strorder = "ORDER BY r.Stud_Name";
                            isRegNo = false;
                            isRoll = false;
                            isStudName = true;
                        }
                        else if (orderby_Setting == "0,1,2")
                        {
                            strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                            isRegNo = true;
                            isRoll = true;
                            isStudName = true;
                        }
                        else if (orderby_Setting == "0,1")
                        {
                            strorder = "ORDER BY r.Roll_No,r.Reg_No";
                            isRegNo = true;
                            isRoll = true;
                            isStudName = false;
                        }
                        else if (orderby_Setting == "1,2")
                        {
                            strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                            isRegNo = true;
                            isRoll = false;
                            isStudName = true;
                        }
                        else if (orderby_Setting == "0,2")
                        {
                            strorder = "ORDER BY r.Roll_No,r.Stud_Name";
                            isRegNo = false;
                            isRoll = true;
                            isStudName = true;
                        }
                    }
                    int hourschkcount = 0;
                    for (int sj = 0; sj < chklshour.Items.Count; sj++)
                    {
                        if (chklshour.Items[sj].Selected == true)
                        {
                            hourschkcount++;
                        }
                    }
                    if (hourschkcount == 0)
                    {
                        lblset.Text = "Please Select Hours";
                        lblset.Visible = true;
                        gvatte.Visible = false;
                        GVhead.Visible = false;
                        btngvsave.Visible = false;
                        return;
                    }
                    if (days >= 0)
                    {
                        string[] spitdate = txtFromDate.Text.Split('-');
                        Boolean daychek = daycheck(Convert.ToDateTime(spitdate[1] + '/' + spitdate[0] + '/' + spitdate[2]));
                        if (daychek == true)
                        {
                        }
                        else
                        {
                            lblset.Visible = true;
                            lblset.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                            return;
                        }
                        DataTable gdvheaders = new DataTable();
                        gdvheaders.Columns.Add("S.No");
                        gdvheaders.Columns.Add("Roll_no");
                        gdvheaders.Columns.Add("Reg_no");
                        gdvheaders.Columns.Add("Roll_Admit");
                        gdvheaders.Columns.Add("stud_name");
                        gdvheaders.Columns.Add("stud_type");
                        gdvheaders.Columns.Add("1");
                        gdvheaders.Columns.Add("2");
                        gdvheaders.Columns.Add("3");
                        gdvheaders.Columns.Add("4");
                        gdvheaders.Columns.Add("5");
                        gdvheaders.Columns.Add("6");
                        gdvheaders.Columns.Add("7");
                        gdvheaders.Columns.Add("8");
                        gdvheaders.Columns.Add("9");
                        DataRow dr = null;
                        dr = gdvheaders.NewRow();
                        dr[0] = "S.No";
                        dr[1] = "Roll No";
                        dr[2] = "Reg No";
                        dr[3] = "Admission No";
                        dr[4] = "Student Name";
                        dr[5] = "Type";
                        dr[6] = "1";
                        dr[7] = "2";
                        dr[8] = "3";
                        dr[9] = "4";
                        dr[10] = "5";
                        dr[11] = "6";
                        dr[12] = "7";
                        dr[13] = "8";
                        dr[14] = "9";
                        gdvheaders.Rows.Add(dr);
                        string includediscon = " and delflag='0' and isnull(ProLongAbsent,0)<>'1'";
                        string getshedulockva = dacces2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount'");
                        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                        {
                            includediscon = string.Empty;
                        }
                        string includedebar = " and exam_flag <> 'DEBAR'";
                        getshedulockva = dacces2.GetFunctionv("select value from Master_Settings where settings='Attendance Debar'");
                        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                        {
                            includedebar = string.Empty;
                        }
                        string feeofrolls = string.Empty;
                        string strfeeofrollquery = string.Empty;
                        DataSet dsfeerol = new DataSet();

                        if (rbexcludeonduty.Checked == true)
                        {
                            strfeeofrollquery = "select r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,s.Purpose,s.hourse from Onduty_Stud s,Registration r where s.roll_no=r.Roll_No and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "'   " + includediscon + includedebar + qryIncludeRedo + " and '" + dateto + "' between s.Fromdate and s.Todate " + sec + " " + hourdetails + " order by r.Stud_Name";//and r.Exam_Flag<>'debar' and r.cc=0 and r.delflag=0
                            dsfeerol = dacces2.select_method_wo_parameter(strfeeofrollquery, "text");
                            for (int fs = 0; fs < dsfeerol.Tables[0].Rows.Count; fs++)
                            {
                                if (feeofrolls == "")
                                {
                                    feeofrolls = "'" + dsfeerol.Tables[0].Rows[fs]["Roll_No"].ToString() + "'";
                                    stodstudetails = " Following Student's Having OD :<br/>" + ((isRoll) ? "Roll No. : " + Convert.ToString(dsfeerol.Tables[0].Rows[fs]["Roll_No"]) : (isRegNo) ? "Reg no : " + dsfeerol.Tables[0].Rows[fs]["Reg_No"].ToString() : "Roll No. : " + Convert.ToString(dsfeerol.Tables[0].Rows[fs]["Roll_No"])) + " Purpose : " + dsfeerol.Tables[0].Rows[fs]["Purpose"].ToString() + " Hours : " + dsfeerol.Tables[0].Rows[fs]["hourse"].ToString();
                                }
                                else
                                {
                                    feeofrolls = feeofrolls + ",'" + dsfeerol.Tables[0].Rows[fs]["Roll_No"].ToString() + "'";
                                    stodstudetails = stodstudetails + "<br/>" + ((isRoll) ? "Roll No. : " + Convert.ToString(dsfeerol.Tables[0].Rows[fs]["Roll_No"]) : (isRegNo) ? "Reg no : " + dsfeerol.Tables[0].Rows[fs]["Reg_No"].ToString() : "Roll No. : " + Convert.ToString(dsfeerol.Tables[0].Rows[fs]["Roll_No"])) + " Purpose : " + dsfeerol.Tables[0].Rows[fs]["Purpose"].ToString() + " Hours : " + dsfeerol.Tables[0].Rows[fs]["hourse"].ToString();
                                }
                            }
                        }
                        #region Commented By Malang Raja On Feb 03 2017
                        //strfeeofrollquery = "select r.Roll_No from stucon s,Registration r where s.roll_no=r.Roll_No and r.Current_Semester=s.semester and s.ack_fee_of_roll=1  and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "' and r.cc=0 and r.delflag=0 and r.Exam_Flag<>'debar' " + sec + " ";
                        //dsfeerol = dacces2.select_method_wo_parameter(strfeeofrollquery, "text");
                        //if (dsfeerol.Tables.Count > 0 && dsfeerol.Tables[0].Rows.Count > 0)
                        //{
                        //    for (int fs = 0; fs < dsfeerol.Tables[0].Rows.Count; fs++)
                        //    {
                        //        if (feeofrolls == "")
                        //        {
                        //            feeofrolls = "'" + dsfeerol.Tables[0].Rows[fs]["Roll_No"].ToString() + "'";
                        //        }
                        //        else
                        //        {
                        //            feeofrolls = feeofrolls + ",'" + dsfeerol.Tables[0].Rows[fs]["Roll_No"].ToString() + "'";
                        //        }
                        //    }
                        //}                       
                        #endregion Commented By Malang Raja On Feb 03 2017
                        if (feeofrolls.Trim() != "")
                        {
                            feeofrolls = " and r.roll_no not in(" + feeofrolls + ")";
                        }
                        GVhead.DataSource = gdvheaders;
                        GVhead.DataBind();
                        hoursvisiblity();
                        string sqlstr;
                        //sqlstr = "select Registration.roll_no,Registration.reg_no, Registration.stud_name,Registration.stud_type,registration.serialno,Registration.Adm_Date from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 " + strsec + " " + Session["strvar"] + " and adm_date<='" + dateto + "' " + strorder + "";
                        #region Commented By Malang Raja On Feb 03 2017
                        //sqlstr = "select r.roll_no,r.reg_no, r.stud_name,r.stud_type,r.serialno,r.Adm_Date,delflag,exam_flag from registration r, applyn a where a.app_no=r.app_no and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.current_semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 " + includedebar + " " + includediscon + " " + strsec + " " + feeofrolls + " and adm_date<='" + dateto + "' " + strorder + "";
                        #endregion Commented By Malang Raja On Feb 03 2017
                        sqlstr = "select r.roll_no,r.reg_no,r.Roll_Admit, r.stud_name,r.stud_type,r.serialno,r.Adm_Date,delflag,exam_flag from registration r, applyn a where a.app_no=r.app_no  and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.current_semester='" + ddlsem.SelectedValue.ToString() + "' and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "'  and RollNo_Flag<>'0'  " + qryIncludeRedo + includedebar + " " + includediscon + " " + strsec + " " + feeofrolls + " and adm_date<='" + dateto + "' " + strorder + "";//and cc='0'
                        DataSet gvds = new DataSet();
                        gvds.Clear();
                        gvds = dacces2.select_method_wo_parameter(sqlstr, "text");
                        if (gvds.Tables.Count > 0 && gvds.Tables[0].Rows.Count > 0)
                        {
                            gvatte.Visible = true;
                            GVhead.Visible = true;
                            btngvsave.Visible = true;
                            btnprint.Visible = true;
                            DataRow dr1 = null;
                            dr1 = gvds.Tables[0].NewRow();
                            dr1[2] = "No Of Student(s) Present:";
                            gvds.Tables[0].Rows.Add(dr1);
                            DataRow dr2 = null;
                            dr2 = gvds.Tables[0].NewRow();
                            dr2[2] = "No Of Student(s) Absent:";
                            gvds.Tables[0].Rows.Add(dr2);
                            gvatte.DataSource = gvds.Tables[0];
                            gvatte.DataBind();
                            gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].Text = " ";
                            gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].Text = " ";
                        }
                        else
                        {
                            gvatte.Visible = false;
                            GVhead.Visible = false;
                            btngvsave.Visible = false;
                        }
                        if (gvatte.Rows.Count == 0)
                        {
                            GVhead.Visible = false;
                        }
                        else
                        {
                            GVhead.Visible = true;
                        }
                        if (gvatte.Rows.Count > 0)
                        {
                            for (int gvcol = 6; gvcol < gvatte.Columns.Count; gvcol++)
                            {
                                string timageids = "chk" + gvcol;
                                string lblpres = "p" + gvcol;
                                //string rollNo = string.Empty;
                                //rollNo = (gvatte.Rows[gvatte.Rows.Count - 2].Cells[gvcol].FindControl("lblroll_no") as Label).Text;
                                //if (dicFeeOfRollStudents.ContainsKey(rollNo.Trim()))
                                //{
                                //    DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollNo.Trim()];
                                //    if (dt1 >= dtFeeOfRoll[0])
                                //    {
                                //        (gvatte.Rows[gvatte.Rows.Count - 2].Cells[gvcol].FindControl(timageids) as CheckBox).Checked = false;
                                //        (gvatte.Rows[gvatte.Rows.Count - 2].Cells[gvcol].FindControl(timageids) as CheckBox).Visible = false;
                                //        (gvatte.Rows[gvatte.Rows.Count - 2].Cells[gvcol].FindControl(lblpres) as Label).Visible = true;
                                //        (gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked = false;
                                //        (gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Visible = false;
                                //        (gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(lblpres) as Label).Visible = true;
                                //    }
                                //}
                                //else
                                //{
                                (gvatte.Rows[gvatte.Rows.Count - 2].Cells[gvcol].FindControl(timageids) as CheckBox).Checked = false;
                                (gvatte.Rows[gvatte.Rows.Count - 2].Cells[gvcol].FindControl(timageids) as CheckBox).Visible = false;
                                (gvatte.Rows[gvatte.Rows.Count - 2].Cells[gvcol].FindControl(lblpres) as Label).Visible = true;
                                (gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked = false;
                                (gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Visible = false;
                                (gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(lblpres) as Label).Visible = true;
                                //}
                            }
                        }
                        DataSet dsstudent = dacces2.select_method(sqlstr, hat, "Text");
                        if (dsstudent.Tables[0].Rows.Count > 0)
                        {
                        }
                        else
                        {
                            lblset.Visible = true;
                            lblset.Text = "There are no students available";
                            gvatte.Visible = false;
                            GVhead.Visible = false;
                            btngvsave.Visible = false;
                            return;
                        }
                        ds.Reset();
                        ds.Dispose();
                        strquery = "select * from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "";
                        ds = dacces2.select_method(strquery, hat, "Text");
                        string noofhours = no_hrs.ToString();
                        string str = string.Empty;
                        str = txtFromDate.Text;
                        lblset.Text = string.Empty;
                    //HOLDAY://magesh 9.10.18
                        if (dsholidays.Tables.Count > 0 && dsholidays.Tables[0].Rows.Count > 0)
                        {

                            lblset.Visible = true;
                            lblset.Text = lblset.Text + dt1.ToString("d-MM-yyyy") + "-holiday" + " Sunday  ";
                            if (dt2 != dt1)
                            {
                                dt1 = dt1.AddDays(1);
                            }
                            else
                            {
                                return;
                            }
                        }
                    //    if (dt1.ToString("dddd") == "Sunday")
                    //    {
                    //        lblset.Visible = true;
                    //        lblset.Text = lblset.Text + dt1.ToString("d-MM-yyyy") + "-holiday" + " Sunday  ";
                    //        if (dt2 != dt1)
                    //        {
                    //            dt1 = dt1.AddDays(1);
                    //        }
                    //        else
                    //        {
                    //            return;
                    //        }
                    //    }
                        int spancolumn = Convert.ToInt32(noofhours);
                        string half_full = string.Empty;
                        string morning_h = string.Empty;
                        string evening_h = string.Empty;
                        int starthour = 1;
                        if (noofhours.ToString().Trim() != "" && noofhours != "0" && noofhours.ToString() != null)
                        {
                            string[] differdays = new string[500];
                            DateTime temp_date = dt1;
                            int date_loop = 0;
                            while (temp_date <= dt2)
                            {
                                temp_date = dt1.AddDays(date_loop);
                                starthour = 1;
                                spancolumn = 1;
                                string Day_Order = string.Empty;
                                if (temp_date <= dt2)
                                {
                                    if (no_hrs > 0)
                                    {
                                        if (sch_order != "0")
                                        {
                                            srt_day = temp_date.ToString("ddd");
                                        }
                                        else
                                        {
                                            string[] tmpdate = temp_date.ToString().Split(new char[] { ' ' });
                                            string currdate = tmpdate[0].ToString();
                                            string[] tmpdate1 = startdate.ToString().Split(new char[] { ' ' });
                                            string startdate1 = tmpdate1[0].ToString();
                                            srt_day = dacces2.findday(currdate.ToString(), ddlbranch.SelectedValue.ToString(), ddlsem.SelectedItem.ToString(), ddlbatch.Text, startdate1.ToString(), no_days.ToString(), starting_dayorder.ToString());
                                        }
                                    }
                                    strquery = "select * from holidaystudents where holiday_date='" + temp_date + "'and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "";
                                    dsholiday.Reset();
                                    dsholiday.Dispose();
                                    dsholiday = dacces2.select_method_wo_parameter(strquery, "Text");
                                    string holi_des = string.Empty;
                                    string holi_value = string.Empty;
                                    DateTime holidate;
                                    if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                                    {
                                        holi_des = dsholiday.Tables[0].Rows[0][2].ToString();
                                        holi_value = dsholiday.Tables[0].Rows[0][4].ToString();
                                    }
                                    if (holi_des != "" && holi_value.Trim().ToLower() == "false")
                                    {
                                        lblset.Visible = true;
                                        holidate = Convert.ToDateTime(dsholiday.Tables[0].Rows[0][1].ToString());//Added by Manikandan 30/07/2013
                                        if (temp_date.ToString("dddd").ToLower().Trim() == "sunday")
                                        {
                                            lblset.Text = "    " + lblset.Text + holidate.ToString("d/MM/yyyy") + "-holiday" + " Sunday  ";//Modified by Manikandan 30/07/2013
                                        }
                                        else
                                        {
                                            lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday";
                                        }
                                        dbhoursvisiblity(0, 0);
                                    }
                                    else
                                    {
                                        if (temp_date > dt2) break;
                                        if (temp_date.ToString("dddd").ToLower().Trim() == "sunday")
                                        {
                                            lblset.Visible = true;
                                            lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday" + " Sunday  ";
                                            date_loop++;
                                            continue;
                                        }
                                        if (half_full == "False")
                                        {
                                            lblset.Visible = true;
                                            lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday";
                                            date_loop++;
                                            continue;
                                        }
                                        else
                                        {
                                            if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                                            {
                                                lblset.Visible = true;
                                                Boolean morse = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["morning"]);
                                                Boolean evese = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["evening"]);
                                                if (morse == true)
                                                {
                                                    lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "- is Morning holiday";
                                                }
                                                else
                                                {
                                                    lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "- is Evening holiday";
                                                }
                                            }
                                        }
                                        differdays[date_loop] = temp_date.ToString("d-MM-yyyy");
                                        int i = 0;
                                        string dateformat;
                                        half_full = string.Empty;
                                        morning_h = string.Empty;
                                        evening_h = string.Empty;
                                        if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                                        {
                                            half_full = dsholiday.Tables[0].Rows[0]["halforfull"].ToString();
                                            morning_h = dsholiday.Tables[0].Rows[0]["morning"].ToString();
                                            evening_h = dsholiday.Tables[0].Rows[0]["evening"].ToString();
                                        }
                                        spancolumn = Convert.ToInt32(noofhours);
                                        if (half_full.Trim().ToLower() == "true" && morning_h.Trim().ToLower() == "true")
                                        {
                                            starthour = Convert.ToInt32(noofhours) - Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                                            starthour = starthour + 1;
                                            spancolumn = Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString()); //aruna 25feb2013
                                        }
                                        else if (half_full.Trim().ToLower() == "true" && evening_h.Trim().ToLower() == "true")
                                        {
                                            noofhours = ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                                            if (noofhours == "" && noofhours == null)
                                            {
                                                noofhours = "0";
                                            }
                                            spancolumn = Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());  //aruna 25feb2013
                                        }
                                        else
                                        {
                                            noofhours = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                                            if (noofhours == "" && noofhours == null)
                                            {
                                                noofhours = "0";
                                            }
                                        }
                                        dbhoursvisiblity(starthour, Convert.ToInt32(noofhours.ToString()));
                                    }
                                    date_loop++;
                                }
                            }
                            int temp = 0;
                            string str_Date;
                            string str_day;
                            string Atmonth;
                            string Atyear;
                            long strdate;
                            string rollno_Att = string.Empty;
                            string Att_dcolumn = string.Empty;
                            string Att_Markvalue;
                            string Att_Mark1;
                            temp = 0;
                            Hashtable hatsuspent = new Hashtable();
                            //string strsuspen = "select s.roll_no from stucon s,Registration r where r.Roll_No=s.roll_no and ack_susp=1 and tot_days>0 and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.current_semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 " + sec + "";
                            string strsuspen = "select s.roll_no from stucon s,Registration r where r.Roll_No=s.roll_no and ack_susp=1 and tot_days>0 and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.current_semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 " + qryIncludeRedo + sec + "";
                            ds_attndmaster.Reset();
                            ds_attndmaster.Dispose();
                            ds_attndmaster = dacces2.select_method(strsuspen, hatsuspent, "Text");
                            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                            {
                                for (int su = 0; su < ds_attndmaster.Tables[0].Rows.Count; su++)
                                {
                                    rollno_Att = ds_attndmaster.Tables[0].Rows[su]["ROll_no"].ToString();
                                    if (!hatsuspent.Contains(rollno_Att))
                                    {
                                        hatsuspent.Add(rollno_Att, rollno_Att);
                                    }
                                }
                            }
                            //sqlstr = "select a.* from attendance a,Registration r where r.Roll_No=a.roll_no and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.current_semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 " + sec + " and adm_date<='" + dateto + "'";
                            sqlstr = "select a.* from attendance a,Registration r where r.Roll_No=a.roll_no and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.current_semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0  " + qryIncludeRedo + sec + " and adm_date<='" + dateto + "'";//and cc=0
                            ds_attndmaster = dacces2.select_method(sqlstr, hat, "Text");
                            DataView dvatt = new DataView();
                            string monthyear = string.Empty;
                            for (Att_mark_row = 0; Att_mark_row < gvatte.Rows.Count - 2; Att_mark_row++)
                            {
                                gvatte.Rows[Att_mark_row].Enabled = true;
                                for (Att_mark_column = 6; Att_mark_column < gvatte.Columns.Count; Att_mark_column++)
                                {
                                    string timageids = "chk" + Att_mark_column;
                                    string lblpres = "p" + Att_mark_column;
                                    str_Date = txtFromDate.Text;
                                    string[] tmpdate = str_Date.ToString().Split(new char[] { ' ' });
                                    str_Date = tmpdate[0].ToString();
                                    rollno_Att = (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl("lblroll_no") as Label).Text;
                                    string[] sp = str_Date.Split(new Char[] { '-' });
                                    str_day = sp[0].ToString();
                                    Atmonth = sp[1].ToString();
                                    Atyear = sp[2].ToString();
                                    strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                    int Att_hour;
                                    Att_hour = Convert.ToInt32(GVhead.Rows[0].Cells[Att_mark_column].Text.ToString());
                                    Att_dcolumn = "d" + Convert.ToInt16(str_day) + "d" + Att_hour;
                                    DateTime dtcurdate = Convert.ToDateTime(sp[1] + '/' + sp[0] + '/' + sp[2]);
                                    string rollNo = string.Empty;
                                    bool checkedFeeOfRoll = false;
                                    rollNo = Convert.ToString((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl("lblroll_no") as Label).Text).Trim().ToLower();
                                    if (dicFeeOfRollStudents.ContainsKey(rollNo.Trim()) && dicFeeOnRollStudents.ContainsKey(rollNo.Trim()))
                                    {
                                        DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollNo.Trim()];
                                        string dtadntdate = dacces2.GetFunction("select adm_date from registration where Roll_No ='" + rollno_Att + "'");
                                        DateTime dtadm = Convert.ToDateTime(dtadntdate);
                                        if (dtadm <= dtcurdate)
                                        {
                                            if (dtcurdate >= dtFeeOfRoll[0])
                                            {
                                                DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtcurdate < dtFeeOfRoll[1])
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Enabled = false;
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].Enabled = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "a";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Red;
                                                    gvatte.Rows[Att_mark_row].Enabled = false;
                                                    checkedFeeOfRoll = true;
                                                }
                                                else if (dicFeeOnRollStudents[rollNo.Trim()] == 1)
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Enabled = false;
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].Enabled = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "a";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Red;
                                                    gvatte.Rows[Att_mark_row].Enabled = false;
                                                    checkedFeeOfRoll = true;
                                                }
                                                else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Enabled = false;
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].Enabled = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "a";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Red;
                                                    gvatte.Rows[Att_mark_row].Enabled = false;
                                                    checkedFeeOfRoll = true;
                                                }
                                                //else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate >= dtFeeOfRoll[1])
                                                //{
                                                //    checkedFeeOfRoll = false;
                                                //}
                                                else
                                                {
                                                    checkedFeeOfRoll = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Visible = true;
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "NJ";
                                        }
                                    }
                                    if (hatsuspent.Contains(rollno_Att) && !checkedFeeOfRoll)
                                    {
                                        int splitackdate = Convert.ToInt32(sp[0]);
                                        int splitackmonth = Convert.ToInt32(sp[1]);
                                        int splitackyear = Convert.ToInt32(sp[2]);
                                        string concat_susdate = splitackmonth + "/" + splitackdate + "/" + splitackyear;
                                        string suspend_qry = "select  convert(varchar(15),dateadd(day,tot_days-1,ack_date),1) as action_days,ack_date,tot_days from stucon where ack_susp=1 and tot_days>0 and roll_no='" + rollno_Att.ToString() + "' and ack_date<= '" + concat_susdate.ToString() + "'";
                                        DataSet ds_suspend = new DataSet();
                                        ds_suspend = dacces2.select_method(suspend_qry, hat, "Text");
                                        if (ds_suspend.Tables.Count > 0 && ds_suspend.Tables[0].Rows.Count > 0)
                                        {
                                            DateTime dt_curr = Convert.ToDateTime(concat_susdate.ToString());
                                            DateTime dt_act = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["action_days"].ToString());
                                            TimeSpan t_con = dt_act.Subtract(dt_curr);
                                            long daycon = t_con.Days;
                                            DateTime dt_curr1 = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["ack_date"].ToString());
                                            DateTime dt_act1 = Convert.ToDateTime(concat_susdate.ToString());
                                            TimeSpan t_con1 = dt_act1.Subtract(dt_curr1);
                                            long daycon1 = t_con1.Days;
                                            long totalactdays = Convert.ToInt32(ds_suspend.Tables[0].Rows[0]["tot_days"]);
                                            if ((Convert.ToInt32(daycon + daycon1) == totalactdays - 1) && (daycon >= 0))
                                            {
                                                (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Visible = true;
                                                (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "S";
                                            }
                                            else
                                            {
                                                string dtadntdate = dacces2.GetFunction("select adm_date from registration where Roll_No ='" + rollno_Att + "'");
                                                DateTime dtadm = Convert.ToDateTime(dtadntdate);
                                                if (dtadm <= dtcurdate)
                                                {
                                                    ds_attndmaster.Tables[0].DefaultView.RowFilter = " Roll_no='" + rollno_Att + "' and month_year='" + strdate + "'";
                                                    dvatt = ds_attndmaster.Tables[0].DefaultView;
                                                    if (dvatt.Count > 0)
                                                    {
                                                        Att_Markvalue = dvatt[0]["" + Att_dcolumn + ""].ToString();
                                                        Att_Mark1 = Attmark(Att_Markvalue);
                                                        if (Att_Mark1.ToString().ToLower() == "a")
                                                        {
                                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "a";
                                                            gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Red;
                                                        }
                                                        else
                                                        {
                                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = true;
                                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "p";
                                                            gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Green;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = true;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "p";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Green;
                                                }
                                            }
                                        }
                                    }
                                    else if (!checkedFeeOfRoll)
                                    {
                                        string dtadntdate = dacces2.GetFunction("select adm_date from registration where Roll_No ='" + rollno_Att + "'");
                                        DateTime dtadm = Convert.ToDateTime(dtadntdate);
                                        if (dtadm <= dtcurdate)
                                        {
                                            ds_attndmaster.Tables[0].DefaultView.RowFilter = " Roll_no='" + rollno_Att + "' and month_year='" + strdate + "'";
                                            dvatt = ds_attndmaster.Tables[0].DefaultView;
                                            if (dvatt.Count > 0)
                                            {
                                                Att_Markvalue = dvatt[0]["" + Att_dcolumn + ""].ToString();
                                                Att_Mark1 = Attmark(Att_Markvalue);
                                                if (Att_Mark1.ToString().ToLower() == "a")
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "a";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Red;
                                                }
                                                else
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = true;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "p";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Green;
                                                }
                                            }
                                            else
                                            {
                                                (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = true;
                                                (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "p";
                                                gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Green;
                                            }
                                        }
                                        else
                                        {
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Visible = true;
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "NJ";
                                        }
                                    }
                                }
                            }
                            string strqueryunactive = "select * from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and current_semester=" + ddlsem.SelectedValue.ToString() + " and (delflag<>0 and isnull(ProLongAbsent,0)<>'1' or exam_flag='debar')";
                            DataSet dsunactive = dacces2.select_method_wo_parameter(strqueryunactive, "Text");
                            if (dsunactive.Tables.Count > 0 && dsunactive.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsunactive.Tables[0].Rows.Count; i++)
                                {
                                    for (int gvcol = 0; gvcol < gvatte.Rows.Count; gvcol++)
                                    {
                                        if (dsunactive.Tables[0].Rows[i]["Roll_no"].ToString().Trim() == (gvatte.Rows[gvcol].FindControl("lblroll_no") as Label).Text.Trim())
                                        {
                                            (gvatte.Rows[gvcol].FindControl("lblroll_no") as Label).ForeColor = Color.Red;
                                            (gvatte.Rows[gvcol].FindControl("lblReg_no") as Label).ForeColor = Color.Red;
                                            (gvatte.Rows[gvcol].FindControl("lblstud_name") as Label).ForeColor = Color.Red;
                                            for (int j = 6; j < gvatte.Columns.Count; j++)
                                            {
                                                gvatte.Rows[gvcol].Cells[j].BackColor = Color.DarkViolet;
                                                gvatte.Rows[gvcol].Cells[j].Enabled = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblset.Visible = true;
                            lblset.Text = "Please Update Attendance Parameters!!!";
                            return;
                        }
                    }
                    if (Convert.ToInt32(gvatte.Rows.Count) == 0)
                    {
                    }
                    else
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
                        for (Att_mark_column = 6; Att_mark_column < gvatte.Columns.Count; Att_mark_column++)
                        {
                            absent_count = 0;
                            present_count = 0;
                            string timageids = "chk" + Att_mark_column;
                            string lblpres = "p" + Att_mark_column;
                            for (Att_mark_row = 0; Att_mark_row < gvatte.Rows.Count - 2; Att_mark_row++)
                            {
                                if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text != "")
                                {
                                    if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text == "p")
                                    {
                                        if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                                        {
                                            present_count++;
                                        }
                                    }
                                    else if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text == "a")
                                    {
                                        if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                                        {
                                            absent_count++;
                                        }
                                    }
                                }
                            }
                            (gvatte.Rows[gvatte.Rows.Count - 2].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = present_count.ToString();
                            (gvatte.Rows[gvatte.Rows.Count - 1].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = absent_count.ToString();
                        }
                    }
                }
                else
                {
                    lblset.Visible = true;
                    lblset.Text = "Selece From Date";
                }
            }
            if (gvatte.Rows.Count > 0)
            {
                gvatte.Visible = true;
                GVhead.Visible = true;
                int nofospan = 6;
                gvatte.Columns[1].Visible = true;
                gvatte.Columns[2].Visible = true;
                gvatte.Columns[5].Visible = true;
                gvatte.Columns[3].Visible = true;
                if (Session["Rollflag"].ToString() == "0")
                {
                    nofospan--;
                    gvatte.Columns[1].Visible = false;
                }
                if (Session["Regflag"].ToString() == "0")
                {
                    nofospan--;
                    gvatte.Columns[2].Visible = false;
                }
                if (Session["AdmissionNo"].ToString() == "0")
                {
                    nofospan--;
                    gvatte.Columns[3].Visible = false;
                }
                if (Session["Studflag"].ToString() == "0")
                {
                    nofospan--;
                    gvatte.Columns[5].Visible = false;
                }

                // nofospan = 3;
                gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].ColumnSpan = nofospan;
                gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].ColumnSpan = nofospan;
                //nofospan = nofospan + 1;
                for (int i = 1; i < 6; i++)
                {
                    gvatte.Rows[gvatte.Rows.Count - 2].Cells[i].Visible = false;
                    gvatte.Rows[gvatte.Rows.Count - 1].Cells[i].Visible = false;
                }
                gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].Text = "No Of Student(s) Present:";
                gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].Text = "No Of Student(s) Absent:";
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "SyncTableColumns();", true);
            if (rbexcludeonduty.Checked == true)
            {
                lblset.Visible = true;
                lblset.Text = stodstudetails.ToString();
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void loadSubjectwisestudent()
    {
        try
        {
            string stodstudetails = string.Empty;
            gvatte.Visible = false;
            GVhead.Visible = false;
            btngvsave.Visible = false;
            lblset.Text = string.Empty;
            lblset.Visible = false;
            serialflag = false;
            DataSet dsholiday = new DataSet();
            int chlhourscount = 0;
            string hourdetails = string.Empty;
            ArrayList arrhours = new ArrayList();
            for (int i = 0; i < chklshour.Items.Count; i++)
            {
                if (chklshour.Items[i].Selected == true)
                {
                    arrhours.Add(chklshour.Items[i].Text);
                    chlhourscount++;
                    if (hourdetails.Trim() == "")
                    {
                        hourdetails = " s.hourse like '%" + chklshour.Items[i].Text + "%'";
                    }
                    else
                    {
                        hourdetails = hourdetails + " or s.hourse like '%" + chklshour.Items[i].Text + "%'";
                    }
                }
            }
            if (hourdetails.Trim() != "")
            {
                hourdetails = " and (" + hourdetails + ")";
            }
            if (staffcode == "" || staffcode == null)
            {
                if (txtFromDate.Text != "")
                {
                    lblset.Visible = false;
                    string strsec = string.Empty;
                    string sec = string.Empty;
                    string secrights = string.Empty;
                    if (ddlsec.Enabled == true)
                    {
                        if (ddlsec.Text.ToString().Trim().ToLower() == "all" || ddlsec.Text.ToString().Trim() == "")
                        {
                            strsec = string.Empty;
                        }
                        else
                        {
                            strsec = " and r.sections='" + ddlsec.SelectedValue.ToString() + "'";
                            sec = " and r.sections='" + ddlsec.SelectedValue.ToString() + "'";
                            secrights = ddlsec.SelectedValue.ToString();
                        }
                    }
                    string typeval = string.Empty;
                    if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
                    {
                        typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
                    }
                    Boolean secrightsflag = false;
                    string collegecode = Session["collegecode"].ToString();
                    string ucode = string.Empty;
                    string code = string.Empty;
                    string group_code = Session["group_code"].ToString();
                    if (group_code.Contains(';'))
                    {
                        string[] group_semi = group_code.Split(';');
                        group_code = group_semi[0].ToString();
                    }
                    if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                    {
                        ucode = group_code;
                        code = "group_code=" + ucode + "";
                    }
                    else
                    {
                        ucode = Session["usercode"].ToString();
                        code = "usercode=" + ucode + "";
                    }
                    string strgetsec = dacces2.GetFunction("select sections from tbl_attendance_rights where batch_year='" + ddlbatch.SelectedItem.ToString() + "' and user_id='" + ucode + "' ");
                    if (strgetsec.Trim() != null && strgetsec.Trim() != "0")
                    {
                        string[] spsec = strgetsec.Split(',');
                        for (int sp = 0; sp <= spsec.GetUpperBound(0); sp++)
                        {
                            string valu = spsec[sp].ToString();
                            if (secrights.Trim().ToLower() == valu.Trim().ToLower())
                            {
                                secrightsflag = true;
                            }
                        }
                    }
                    if (secrightsflag == false)
                    {
                        lblset.Visible = true;
                        lblset.Text = "Please Set Rights For The User";
                        return;
                    }
                    string date1 = string.Empty;
                    string date2 = string.Empty;
                    string datefrom;
                    string dateto = string.Empty;
                    date1 = txtFromDate.Text.ToString();
                    string[] split = date1.Split(new Char[] { '-' });
                    datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
                    date2 = txtFromDate.Text.ToString();
                    string[] split1 = date2.Split(new Char[] { '-' });
                    dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
                    DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                    DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                    TimeSpan t = dt2.Subtract(dt1);
                    long days = t.Days;
                    Dictionary<string, DateTime[]> dicFeeOfRollStudents = new Dictionary<string, DateTime[]>();
                    Dictionary<string, byte> dicFeeOnRollStudents = new Dictionary<string, byte>();
                    GetFeeOfRollStudent(ref dicFeeOfRollStudents, ref dicFeeOnRollStudents);
                    if (days < 0)
                    {
                        lblset.Visible = true;
                        lblset.Text = "From date should be less than To date";
                        return;
                    }
                    if (days == 0 && dt1.ToString("dddd") == "Sunday")
                    {
                        lblset.Visible = true;
                        lblset.Text = "Selected Day is Sunday";
                        return;
                    }
                    if (dt1 > DateTime.Today)
                    {
                        lblset.Visible = true;
                        lblset.Text = "You can not mark attendance for the date greater than today";
                        txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                        return;
                    }
                    else
                    {
                        lblset.Visible = false;
                    }
                    ds.Reset();
                    ds.Dispose();
                    string strquery = "select distinct start_date,isnull(starting_dayorder,1) as starting_dayorder,schorder,nodays,No_of_hrs_per_day,min_hrs_per_day from seminfo s,periodattndschedule p,Degree d,Course c where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and batch_year=" + ddlbatch.Text.ToString() + " and s.semester=" + ddlsem.SelectedValue.ToString() + "";
                    ds = dacces2.select_method(strquery, hat, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        sch_order = ds.Tables[0].Rows[0]["schorder"].ToString();
                        no_days = ds.Tables[0].Rows[0]["nodays"].ToString();
                        startdate = ds.Tables[0].Rows[0]["start_date"].ToString();
                        starting_dayorder = ds.Tables[0].Rows[0]["starting_dayorder"].ToString();
                        no_of_hrs = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                    }
                    if (no_of_hrs.Trim() != "")
                    {
                        //no_hrs = Convert.ToInt16(no_of_hrs);
                        int.TryParse(no_of_hrs, out no_hrs);
                    }
                    else
                    {
                        no_hrs = 0;
                    }
                    nodays = Convert.ToInt16(nodays);
                    if (starting_dayorder == "")
                    {
                        starting_dayorder = "1";
                    }
                    lblset.Visible = false;
                    strorder = "ORDER BY r.Roll_No";
                    string serialno = dacces2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                    if (serialno.Trim() == "1")
                    {
                        serialflag = true;
                        strorder = "ORDER BY r.serialno";
                    }
                    else
                    {
                        serialflag = false;
                        string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");
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
                    int hourschkcount = 0;
                    for (int sj = 0; sj < chklshour.Items.Count; sj++)
                    {
                        if (chklshour.Items[sj].Selected == true)
                        {
                            hourschkcount++;
                        }
                    }
                    if (hourschkcount == 0)
                    {
                        lblset.Text = "Please Select Hours";
                        lblset.Visible = true;
                        gvatte.Visible = false;
                        GVhead.Visible = false;
                        btngvsave.Visible = false;
                        return;
                    }
                    if (days >= 0)
                    {
                        string[] spitdate = txtFromDate.Text.Split('-');
                        Boolean daychek = daycheck(Convert.ToDateTime(spitdate[1] + '/' + spitdate[0] + '/' + spitdate[2]));
                        if (daychek == true)
                        {
                        }
                        else
                        {
                            lblset.Visible = true;
                            lblset.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                            return;
                        }
                        DataTable gdvheaders = new DataTable();
                        gdvheaders.Columns.Add("S.No");
                        gdvheaders.Columns.Add("Roll_no");
                        gdvheaders.Columns.Add("Reg_no");
                        gdvheaders.Columns.Add("Roll_Admit");
                        gdvheaders.Columns.Add("stud_name");
                        gdvheaders.Columns.Add("stud_type");
                        gdvheaders.Columns.Add("1");
                        gdvheaders.Columns.Add("2");
                        gdvheaders.Columns.Add("3");
                        gdvheaders.Columns.Add("4");
                        gdvheaders.Columns.Add("5");
                        gdvheaders.Columns.Add("6");
                        gdvheaders.Columns.Add("7");
                        gdvheaders.Columns.Add("8");
                        gdvheaders.Columns.Add("9");
                        DataRow dr = null;
                        dr = gdvheaders.NewRow();
                        dr[0] = "S.No";
                        dr[1] = "Roll No";
                        dr[2] = "Reg No";
                        dr[3] = "Admission No";
                        dr[4] = "Student Name";
                        dr[5] = "Type";
                        dr[6] = "1";
                        dr[7] = "2";
                        dr[8] = "3";
                        dr[9] = "4";
                        dr[10] = "5";
                        dr[11] = "6";
                        dr[12] = "7";
                        dr[13] = "8";
                        dr[14] = "9";
                        gdvheaders.Rows.Add(dr);
                        string strfeeofrollquery = string.Empty;
                        DataSet dsfeerol = new DataSet();
                        string feeofrolls = string.Empty;
                        if (rbexcludeonduty.Checked == true)
                        {
                            strfeeofrollquery = "select r.Roll_No,r.Reg_No,r.Stud_Name,s.Purpose,s.hourse from Onduty_Stud s,Registration r,Degree d,Course c where s.roll_no=r.Roll_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id  and r.Current_Semester=s.semester " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "' and r.cc=0 and r.delflag=0 and isnull(r.ProLongAbsent,0)<>'1' and r.Exam_Flag<>'debar' and '" + dateto + "' between s.Fromdate and s.Todate " + sec + " " + hourdetails + " order by r.Stud_Name";
                            dsfeerol = dacces2.select_method_wo_parameter(strfeeofrollquery, "text");
                            if (dsfeerol.Tables.Count > 0 && dsfeerol.Tables[0].Rows.Count > 0)
                            {
                                for (int fs = 0; fs < dsfeerol.Tables[0].Rows.Count; fs++)
                                {
                                    if (feeofrolls == "")
                                    {
                                        feeofrolls = "'" + dsfeerol.Tables[0].Rows[fs]["Roll_No"].ToString() + "'";
                                        stodstudetails = " Following Student's Having OD :<br/>Reg no : " + dsfeerol.Tables[0].Rows[fs]["Reg_No"].ToString() + " Purpose : " + dsfeerol.Tables[0].Rows[fs]["Purpose"].ToString() + " Hours : " + dsfeerol.Tables[0].Rows[fs]["hourse"].ToString();
                                    }
                                    else
                                    {
                                        feeofrolls = feeofrolls + ",'" + dsfeerol.Tables[0].Rows[fs]["Roll_No"].ToString() + "'";
                                        stodstudetails = stodstudetails + "<br/>Reg no  : " + dsfeerol.Tables[0].Rows[fs]["Reg_No"].ToString() + " Purpose : " + dsfeerol.Tables[0].Rows[fs]["Purpose"].ToString() + " Hours : " + dsfeerol.Tables[0].Rows[fs]["hourse"].ToString();
                                    }
                                }
                            }
                        }

                        #region Commented By Malang Raja On Feb 03 2017

                        //strfeeofrollquery = "select r.Roll_No from stucon s,Registration r,Degree d,Course c where s.roll_no=r.Roll_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id  and r.Current_Semester=s.semester  and s.ack_fee_of_roll=1 " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "' and r.cc=0 and r.delflag=0 and r.Exam_Flag<>'debar' ";
                        //dsfeerol = dacces2.select_method_wo_parameter(strfeeofrollquery, "text");
                        //if (dsfeerol.Tables.Count > 0 && dsfeerol.Tables[0].Rows.Count > 0)
                        //{
                        //    for (int fs = 0; fs < dsfeerol.Tables[0].Rows.Count; fs++)
                        //    {
                        //        if (feeofrolls == "")
                        //        {
                        //            feeofrolls = "'" + dsfeerol.Tables[0].Rows[fs]["Roll_No"].ToString() + "'";
                        //        }
                        //        else
                        //        {
                        //            feeofrolls = feeofrolls + ",'" + dsfeerol.Tables[0].Rows[fs]["Roll_No"].ToString() + "'";
                        //        }
                        //    }
                        //}

                        #endregion Commented By Malang Raja On Feb 03 2017

                        if (feeofrolls.Trim() != "")
                        {
                            feeofrolls = " and sc.Roll_No not in(" + feeofrolls + ")";
                        }
                        GVhead.DataSource = gdvheaders;
                        GVhead.DataBind();
                        hoursvisiblity();
                        string stubatch = string.Empty;
                        if (ddlstubatch.Enabled == true && ddlstubatch.Items.Count > 0)
                        {
                            if (ddlstubatch.Text.ToString().Trim().ToLower() != "all")
                            {
                                stubatch = " and sc.batch='" + ddlstubatch.Text.ToString() + "'";
                            }
                        }
                        string includediscon = " and r.delflag=0 and isnull(r.ProLongAbsent,0)<>'1'";
                        string getshedulockva = dacces2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount'");
                        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                        {
                            includediscon = string.Empty;
                        }
                        string includedebar = " and r.exam_flag <> 'DEBAR'";
                        getshedulockva = dacces2.GetFunctionv("select value from Master_Settings where settings='Attendance Debar'");
                        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                        {
                            includedebar = string.Empty;
                        }
                        string degreecodevalues = string.Empty;
                        string strquerydegerr = "select d.Degree_Code from degree d,Course c where d.Course_Id=c.Course_Id and c.college_code=d.college_code " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and c.college_code='" + collegecode + "'";
                        DataSet dsdegree = dacces2.select_method_wo_parameter(strquerydegerr, "Text");
                        if (dsdegree.Tables.Count > 0 && dsdegree.Tables[0].Rows.Count > 0)
                        {
                            for (int d = 0; d < dsdegree.Tables[0].Rows.Count; d++)
                            {
                                if (degreecodevalues.Trim() == "")
                                {
                                    degreecodevalues = dsdegree.Tables[0].Rows[d]["Degree_Code"].ToString();
                                }
                                else
                                {
                                    degreecodevalues = degreecodevalues + "," + dsdegree.Tables[0].Rows[d]["Degree_Code"].ToString();
                                }
                            }
                        }
                        if (degreecodevalues.Trim() != "")
                        {
                            degreecodevalues = "and r.degree_code in(" + degreecodevalues + ")";
                        }
                        //string sqlstr = "select r.roll_no,r.reg_no, r.stud_name,r.stud_type,r.serialno,r.Adm_Date,s.subject_no from Registration r,subject s,subjectChooser sc,Degree d,course c,syllabus_master sy where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and r.Current_Semester=sc.semester and sy.syll_code=s.syll_code and sy.degree_code=d.Degree_Code and sy.semester=r.Current_Semester and sy.degree_code=r.degree_code " + feeofrolls + " " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "' " + stubatch + " and r.cc=0 " + includediscon + " " + includedebar + " " + strorder + "";
                        string sqlstr = "select r.roll_no,r.reg_no,r.Roll_Admit, r.stud_name,r.stud_type,r.serialno,r.Adm_Date,s.subject_no from Registration r,subject s,subjectChooser sc,syllabus_master sy where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.Current_Semester=sc.semester and sy.syll_code=s.syll_code and sy.semester=r.Current_Semester and sy.degree_code=r.degree_code " + feeofrolls + " and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "' " + stubatch + " " + degreecodevalues + " and r.cc=0 " + includediscon + " " + includedebar + " " + strorder + "";
                        DataSet gvds = dacces2.select_method_wo_parameter(sqlstr, "text");
                        if (gvds.Tables.Count > 0 && gvds.Tables[0].Rows.Count > 0)
                        {
                            gvatte.Visible = true;
                            GVhead.Visible = true;
                            btngvsave.Visible = true;
                            btnprint.Visible = true;
                            DataRow dr1 = null;
                            dr1 = gvds.Tables[0].NewRow();
                            dr1[2] = "No Of Student(s) Present:";
                            gvds.Tables[0].Rows.Add(dr1);
                            DataRow dr2 = null;
                            dr2 = gvds.Tables[0].NewRow();
                            dr2[2] = "No Of Student(s) Absent:";
                            gvds.Tables[0].Rows.Add(dr2);
                            gvatte.DataSource = gvds.Tables[0];
                            gvatte.DataBind();
                            gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].Text = " ";
                            gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].Text = " ";
                        }
                        else
                        {
                            gvatte.Visible = false;
                            GVhead.Visible = false;
                            btngvsave.Visible = false;
                        }
                        if (gvatte.Rows.Count == 0)
                        {
                            GVhead.Visible = false;
                        }
                        else
                        {
                            GVhead.Visible = true;
                        }
                        if (gvatte.Rows.Count > 0)
                        {
                            for (int gvcol = 6; gvcol < gvatte.Columns.Count; gvcol++)
                            {
                                string timageids = "chk" + gvcol;
                                string lblpres = "p" + gvcol;
                                (gvatte.Rows[gvatte.Rows.Count - 2].Cells[gvcol].FindControl(timageids) as CheckBox).Checked = false;
                                (gvatte.Rows[gvatte.Rows.Count - 2].Cells[gvcol].FindControl(timageids) as CheckBox).Visible = false;
                                (gvatte.Rows[gvatte.Rows.Count - 2].Cells[gvcol].FindControl(lblpres) as Label).Visible = true;
                                (gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked = false;
                                (gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Visible = false;
                                (gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(lblpres) as Label).Visible = true;
                            }
                        }
                        DataSet dsstudent = dacces2.select_method(sqlstr, hat, "Text");
                        if (dsstudent.Tables.Count > 0 && dsstudent.Tables[0].Rows.Count > 0)
                        {
                        }
                        else
                        {
                            lblset.Visible = true;
                            lblset.Text = "There are no students available";
                            gvatte.Visible = false;
                            GVhead.Visible = false;
                            btngvsave.Visible = false;
                            return;
                        }
                        string noofhours = no_of_hrs.ToString();
                        string str = string.Empty;
                        str = txtFromDate.Text;
                    HOLDAY:
                        if (dt1.ToString("dddd") == "Sunday")
                        {
                            lblset.Visible = true;
                            lblset.Text = lblset.Text + dt1.ToString("d-MM-yyyy") + "-holiday" + " Sunday  ";
                            if (dt2 != dt1)
                            {
                                dt1 = dt1.AddDays(1);
                            }
                            else
                            {
                                return;
                            }
                        }
                        int spancolumn = Convert.ToInt32(noofhours);
                        string half_full = string.Empty;
                        string morning_h = string.Empty;
                        string evening_h = string.Empty;
                        int starthour = 1;
                        if (noofhours.ToString().Trim() != "" && noofhours != "0" && noofhours.ToString() != null)
                        {
                            string[] differdays = new string[500];
                            DateTime temp_date = dt1;
                            int date_loop = 0;
                            while (temp_date <= dt2)
                            {
                                temp_date = dt1.AddDays(date_loop);
                                starthour = 1;
                                spancolumn = 1;
                                string Day_Order = string.Empty;
                                if (temp_date <= dt2)
                                {
                                    if (no_hrs > 0)
                                    {
                                        if (sch_order != "0")
                                        {
                                            srt_day = temp_date.ToString("ddd");
                                        }
                                        else
                                        {
                                            string[] tmpdate = temp_date.ToString().Split(new char[] { ' ' });
                                            string currdate = tmpdate[0].ToString();
                                            string[] tmpdate1 = startdate.ToString().Split(new char[] { ' ' });
                                            string startdate1 = tmpdate1[0].ToString();
                                            srt_day = dacces2.findday(currdate.ToString(), ddlbranch.SelectedValue.ToString(), ddlsem.SelectedItem.ToString(), ddlbatch.Text, startdate1.ToString(), no_days.ToString(), starting_dayorder.ToString());
                                        }
                                    }
                                    strquery = "select distinct h.degree_code,h.holiday_date,h.holiday_desc,h.semester,h.halforfull,h.morning,h.evening from holidaystudents h,Degree d,Course c where d.Degree_Code=h.degree_code and d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and holiday_date='" + temp_date + "' and semester=" + ddlsem.SelectedValue.ToString() + "";
                                    dsholiday.Reset();
                                    dsholiday.Dispose();
                                    dsholiday = dacces2.select_method_wo_parameter(strquery, "Text");
                                    string holi_des = string.Empty;
                                    string holi_value = string.Empty;
                                    DateTime holidate;
                                    if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                                    {
                                        holi_des = dsholiday.Tables[0].Rows[0][2].ToString();
                                        holi_value = dsholiday.Tables[0].Rows[0][4].ToString();
                                    }
                                    if (holi_des != "" && holi_value.ToLower().Trim() == "false")
                                    {
                                        lblset.Visible = true;
                                        holidate = Convert.ToDateTime(dsholiday.Tables[0].Rows[0][1].ToString());//Added by Manikandan 30/07/2013
                                        if (temp_date.ToString("dddd").ToLower().Trim() == "sunday")
                                        {
                                            lblset.Text = "    " + lblset.Text + holidate.ToString("d/MM/yyyy") + "-holiday" + " Sunday  ";//Modified by Manikandan 30/07/2013
                                        }
                                        else
                                        {
                                            lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday";
                                        }
                                        dbhoursvisiblity(0, 0);
                                    }
                                    else
                                    {
                                        if (temp_date > dt2) break;
                                        if (temp_date.ToString("dddd").ToLower().Trim() == "sunday")
                                        {
                                            lblset.Visible = true;
                                            lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday" + " Sunday  ";
                                            date_loop++;
                                            continue;
                                        }
                                        if (half_full.Trim().ToLower() == "false" || half_full.Trim().ToLower() == "0")
                                        {
                                            lblset.Visible = true;
                                            lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday";
                                            date_loop++;
                                            continue;
                                        }
                                        else
                                        {
                                            if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                                            {
                                                lblset.Visible = true;
                                                Boolean morse = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["morning"]);
                                                Boolean evese = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["evening"]);
                                                if (morse == true)
                                                {
                                                    lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "- is Morning holiday";
                                                }
                                                else
                                                {
                                                    lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "- is Evening holiday";
                                                }
                                            }
                                        }
                                        differdays[date_loop] = temp_date.ToString("d-MM-yyyy");
                                        string dateformat;
                                        half_full = string.Empty;
                                        morning_h = string.Empty;
                                        evening_h = string.Empty;
                                        if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                                        {
                                            half_full = dsholiday.Tables[0].Rows[0]["halforfull"].ToString();
                                            morning_h = dsholiday.Tables[0].Rows[0]["morning"].ToString();
                                            evening_h = dsholiday.Tables[0].Rows[0]["evening"].ToString();
                                        }
                                        spancolumn = Convert.ToInt32(noofhours);
                                        if (half_full == "True" && morning_h == "True")
                                        {
                                            starthour = Convert.ToInt32(noofhours) - Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                                            starthour = starthour + 1;
                                            spancolumn = Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString()); //aruna 25feb2013
                                        }
                                        else if (half_full == "True" && evening_h == "True")
                                        {
                                            noofhours = ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                                            if (noofhours == "" && noofhours == null)
                                            {
                                                noofhours = "0";
                                            }
                                            spancolumn = Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());  //aruna 25feb2013
                                        }
                                        else
                                        {
                                            noofhours = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                                            if (noofhours == "" && noofhours == null)
                                            {
                                                noofhours = "0";
                                            }
                                        }
                                        dbhoursvisiblity(starthour, Convert.ToInt32(noofhours.ToString()));
                                    }
                                    date_loop++;
                                }
                            }
                            int temp = 0;
                            string str_Date;
                            string str_day;
                            string Atmonth;
                            string Atyear;
                            long strdate;
                            string rollno_Att = string.Empty;
                            string Att_dcolumn = string.Empty;
                            string Att_Markvalue;
                            string Att_Mark1;
                            temp = 0;
                            Hashtable hatsuspent = new Hashtable();
                            string strsuspen = "select s.roll_no from stucon s,Registration r,Degree d,Course c where r.Roll_No=s.roll_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ack_susp=1 and tot_days>0 and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and isnull(ProLongAbsent,0)<>'1' " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and r.current_semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "  " + sec + "";
                            ds_attndmaster.Reset();
                            ds_attndmaster.Dispose();
                            ds_attndmaster = dacces2.select_method(strsuspen, hatsuspent, "Text");
                            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                            {
                                for (int su = 0; su < ds_attndmaster.Tables[0].Rows.Count; su++)
                                {
                                    rollno_Att = ds_attndmaster.Tables[0].Rows[su]["ROll_no"].ToString();
                                    if (!hatsuspent.Contains(rollno_Att))
                                    {
                                        hatsuspent.Add(rollno_Att, rollno_Att);
                                    }
                                }
                            }
                            sqlstr = "select a.* from attendance a,Registration r,Degree d,course c where r.Roll_No=a.roll_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and r.current_semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + " " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and delflag=0 and isnull(ProLongAbsent,0)<>'1' " + sec + " and adm_date<='" + dateto + "'";
                            ds_attndmaster = dacces2.select_method(sqlstr, hat, "Text");
                            DataView dvatt = new DataView();
                            string monthyear = string.Empty;
                            for (Att_mark_row = 0; Att_mark_row < gvatte.Rows.Count - 2; Att_mark_row++)
                            {
                                for (Att_mark_column = 6; Att_mark_column < gvatte.Columns.Count; Att_mark_column++)
                                {
                                    string timageids = "chk" + Att_mark_column;
                                    string lblpres = "p" + Att_mark_column;
                                    str_Date = txtFromDate.Text;
                                    string[] tmpdate = str_Date.ToString().Split(new char[] { ' ' });
                                    str_Date = tmpdate[0].ToString();
                                    rollno_Att = (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl("lblroll_no") as Label).Text;
                                    string[] sp = str_Date.Split(new Char[] { '-' });
                                    str_day = sp[0].ToString();
                                    Atmonth = sp[1].ToString();
                                    Atyear = sp[2].ToString();
                                    strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                    int Att_hour;
                                    Att_hour = Convert.ToInt32(GVhead.Rows[0].Cells[Att_mark_column].Text.ToString());
                                    Att_dcolumn = "d" + Convert.ToInt16(str_day) + "d" + Att_hour;
                                    DateTime dtcurdate = Convert.ToDateTime(sp[1] + '/' + sp[0] + '/' + sp[2]);
                                    string rollNo = string.Empty;
                                    bool checkedFeeOfRoll = false;
                                    rollNo = Convert.ToString((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl("lblroll_no") as Label).Text).Trim().ToLower();
                                    if (dicFeeOfRollStudents.ContainsKey(rollNo.Trim()) && dicFeeOnRollStudents.ContainsKey(rollNo.Trim()))
                                    {
                                        DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollNo.Trim()];
                                        string dtadntdate = dacces2.GetFunction("select adm_date from registration where Roll_No ='" + rollno_Att + "'");
                                        DateTime dtadm = Convert.ToDateTime(dtadntdate);
                                        if (dtadm <= dtcurdate)
                                        {
                                            if (dtcurdate >= dtFeeOfRoll[0])
                                            {
                                                DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtcurdate < dtFeeOfRoll[1])
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Enabled = false;
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].Enabled = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "a";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Red;
                                                    gvatte.Rows[Att_mark_row].Enabled = false;
                                                    checkedFeeOfRoll = true;
                                                }
                                                else if (dicFeeOnRollStudents[rollNo.Trim()] == 1)
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Enabled = false;
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].Enabled = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "a";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Red;
                                                    gvatte.Rows[Att_mark_row].Enabled = false;
                                                    checkedFeeOfRoll = true;
                                                }
                                                else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Enabled = false;
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].Enabled = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "a";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Red;
                                                    gvatte.Rows[Att_mark_row].Enabled = false;
                                                    checkedFeeOfRoll = true;
                                                }
                                                //else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate >= dtFeeOfRoll[1])
                                                //{
                                                //    checkedFeeOfRoll = false;
                                                //}
                                                else
                                                {
                                                    checkedFeeOfRoll = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Visible = true;
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "NJ";
                                        }
                                    }
                                    if (hatsuspent.Contains(rollno_Att) && !checkedFeeOfRoll)
                                    {
                                        int splitackdate = Convert.ToInt32(sp[0]);
                                        int splitackmonth = Convert.ToInt32(sp[1]);
                                        int splitackyear = Convert.ToInt32(sp[2]);
                                        string concat_susdate = splitackmonth + "/" + splitackdate + "/" + splitackyear;
                                        string suspend_qry = "select  convert(varchar(15),dateadd(day,tot_days-1,ack_date),1) as action_days,ack_date,tot_days from stucon where ack_susp=1 and tot_days>0 and roll_no='" + rollno_Att.ToString() + "' and ack_date<= '" + concat_susdate.ToString() + "'";
                                        DataSet ds_suspend = new DataSet();
                                        ds_suspend = dacces2.select_method(suspend_qry, hat, "Text");
                                        if (ds_suspend.Tables.Count > 0 && ds_suspend.Tables[0].Rows.Count > 0)
                                        {
                                            DateTime dt_curr = Convert.ToDateTime(concat_susdate.ToString());
                                            DateTime dt_act = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["action_days"].ToString());
                                            TimeSpan t_con = dt_act.Subtract(dt_curr);
                                            long daycon = t_con.Days;
                                            DateTime dt_curr1 = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["ack_date"].ToString());
                                            DateTime dt_act1 = Convert.ToDateTime(concat_susdate.ToString());
                                            TimeSpan t_con1 = dt_act1.Subtract(dt_curr1);
                                            long daycon1 = t_con1.Days;
                                            long totalactdays = Convert.ToInt32(ds_suspend.Tables[0].Rows[0]["tot_days"]);
                                            if ((Convert.ToInt32(daycon + daycon1) == totalactdays - 1) && (daycon >= 0))
                                            {
                                                (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Visible = true;
                                                (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "S";
                                            }
                                            else
                                            {
                                                string dtadntdate = dacces2.GetFunction("select adm_date from registration where Roll_No ='" + rollno_Att + "'");
                                                DateTime dtadm = Convert.ToDateTime(dtadntdate);
                                                if (dtadm <= dtcurdate)
                                                {
                                                    if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                                                    {
                                                        ds_attndmaster.Tables[0].DefaultView.RowFilter = " Roll_no='" + rollno_Att + "' and month_year='" + strdate + "'";
                                                        dvatt = ds_attndmaster.Tables[0].DefaultView;
                                                    }
                                                    if (dvatt.Count > 0)
                                                    {
                                                        Att_Markvalue = dvatt[0]["" + Att_dcolumn + ""].ToString();
                                                        Att_Mark1 = Attmark(Att_Markvalue);
                                                        if (Att_Mark1.ToString().ToLower() == "a")
                                                        {
                                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "a";
                                                            gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Red;
                                                        }
                                                        else
                                                        {
                                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = true;
                                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "p";
                                                            gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Green;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = true;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "p";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Green;
                                                }
                                            }
                                        }
                                    }
                                    else if (!checkedFeeOfRoll)
                                    {
                                        string dtadntdate = dacces2.GetFunction("select adm_date from registration where Roll_No ='" + rollno_Att + "'");
                                        DateTime dtadm = Convert.ToDateTime(dtadntdate);
                                        if (dtadm <= dtcurdate)
                                        {
                                            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                                            {
                                                ds_attndmaster.Tables[0].DefaultView.RowFilter = " Roll_no='" + rollno_Att + "' and month_year='" + strdate + "'";
                                                dvatt = ds_attndmaster.Tables[0].DefaultView;
                                            }
                                            if (dvatt.Count > 0)
                                            {
                                                Att_Markvalue = dvatt[0]["" + Att_dcolumn + ""].ToString();
                                                Att_Mark1 = Attmark(Att_Markvalue);
                                                if (Att_Mark1.ToString().ToLower() == "a")
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "a";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Red;
                                                }
                                                else
                                                {
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = true;
                                                    (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "p";
                                                    gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Green;
                                                }
                                            }
                                            else
                                            {
                                                (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = true;
                                                (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "p";
                                                gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor = Color.Green;
                                            }
                                        }
                                        else
                                        {
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(timageids) as CheckBox).Checked = false;
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Visible = true;
                                            (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = "NJ";
                                        }
                                    }
                                }
                            }
                            string strqueryunactive = "select r.roll_no,r.reg_no, r.stud_name,r.stud_type,r.serialno,r.Adm_Date,s.subject_no from Registration r,subject s,subjectChooser sc, Degree d,course c,syllabus_master sy where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=d.Degree_Code  and d.Course_Id=c.Course_Id and r.Current_Semester=sc.semester and sy.syll_code=s.syll_code  and sy.degree_code=d.Degree_Code and sy.semester=r.Current_Semester and sy.degree_code=r.degree_code  " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "' " + stubatch + " and r.cc=0 and (r.delflag<>'0' and isnull(r.ProLongAbsent,0)<>'1' or r.exam_flag='debar')  ";
                            DataSet dsunactive = dacces2.select_method_wo_parameter(strqueryunactive, "Text");
                            //if (dsunactive.Tables[0].Rows.Count > 0)
                            if (dsunactive.Tables.Count > 0 && dsunactive.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsunactive.Tables[0].Rows.Count; i++)
                                {
                                    for (int gvcol = 0; gvcol < gvatte.Rows.Count; gvcol++)
                                    {
                                        if (dsunactive.Tables[0].Rows[i]["Roll_no"].ToString().Trim() == (gvatte.Rows[gvcol].FindControl("lblroll_no") as Label).Text.Trim())
                                        {
                                            (gvatte.Rows[gvcol].FindControl("lblroll_no") as Label).ForeColor = Color.Red;
                                            (gvatte.Rows[gvcol].FindControl("lblReg_no") as Label).ForeColor = Color.Red;
                                            (gvatte.Rows[gvcol].FindControl("lblstud_name") as Label).ForeColor = Color.Red;
                                            for (int j = 6; j < gvatte.Columns.Count; j++)
                                            {
                                                gvatte.Rows[gvcol].Cells[j].BackColor = Color.DarkViolet;
                                                gvatte.Rows[gvcol].Cells[j].Enabled = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblset.Visible = true;
                            lblset.Text = "Please Update Attendance Parameters!!!";
                            return;
                        }
                    }
                    if (Convert.ToInt32(gvatte.Rows.Count) == 0)
                    {
                    }
                    else
                    {
                        present_calcflag.Clear();
                        absent_calcflag.Clear();
                        hat.Clear();
                        hat.Add("colege_code", Session["collegecode"].ToString());
                        ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                        if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                        {
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
                        }
                        for (Att_mark_column = 6; Att_mark_column < gvatte.Columns.Count; Att_mark_column++)
                        {
                            absent_count = 0;
                            present_count = 0;
                            string timageids = "chk" + Att_mark_column;
                            string lblpres = "p" + Att_mark_column;
                            for (Att_mark_row = 0; Att_mark_row < gvatte.Rows.Count - 2; Att_mark_row++)
                            {
                                if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text.Trim().ToLower() != "")
                                {
                                    if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text.Trim().ToLower() == "p")
                                    {
                                        if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                                        {
                                            present_count++;
                                        }
                                    }
                                    else if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text.Trim().ToLower() == "a")
                                    {
                                        if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                                        {
                                            absent_count++;
                                        }
                                    }
                                }
                            }
                            (gvatte.Rows[gvatte.Rows.Count - 2].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = present_count.ToString();
                            (gvatte.Rows[gvatte.Rows.Count - 1].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = absent_count.ToString();
                        }
                    }
                }
                else
                {
                    lblset.Visible = true;
                    lblset.Text = "Selece From Date";
                }
            }
            if (gvatte.Rows.Count > 0)
            {
                gvatte.Visible = true;
                GVhead.Visible = true;
                int nofospan = 6;
                gvatte.Columns[1].Visible = true;
                gvatte.Columns[2].Visible = true;
                gvatte.Columns[3].Visible = true;
                gvatte.Columns[5].Visible = true;
                if (Session["Rollflag"].ToString() == "0")
                {
                    nofospan--;
                    gvatte.Columns[1].Visible = false;
                }
                if (Session["Regflag"].ToString() == "0")
                {
                    nofospan--;
                    gvatte.Columns[2].Visible = false;
                }
                if (Session["AdmissionNo"].ToString() == "0")
                {
                    nofospan--;
                    gvatte.Columns[3].Visible = false;
                }
                if (Session["Studflag"].ToString() == "0")
                {
                    nofospan--;
                    gvatte.Columns[5].Visible = false;
                }
                gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].ColumnSpan = nofospan;
                gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].ColumnSpan = nofospan;
                for (int i = 1; i < 6; i++)
                {
                    gvatte.Rows[gvatte.Rows.Count - 2].Cells[i].Visible = false;
                    gvatte.Rows[gvatte.Rows.Count - 1].Cells[i].Visible = false;
                }
                gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].Text = "No Of Student(s) Present:";
                gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].Text = "No Of Student(s) Absent:";
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "SyncTableColumns();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "Synchecked();", true);
            if (rbexcludeonduty.Checked == true)
            {
                lblset.Visible = true;
                lblset.Text = stodstudetails.ToString();
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void parentsmeet(string regno01, DateTime datectr, string reson)
    {
        DataSet dstemp = new DataSet();
        string srisql = "if not exists (select * from parents_meet where roll_no='" + regno01 + "' and send_date='" + datectr + "')  begin  insert into parents_meet  values ('" + regno01 + "','" + datectr + "','" + reson + "','','','','') end";
        dstemp.Clear();
        dstemp = dacces2.select_method_wo_parameter(srisql, "Text");
    }

    protected void Buttonexit_Click(object sender, EventArgs e)
    {
        string intime = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
        int a = dacces2.update_method_wo_parameter("update UserEELog  set Out_Time='" + intime + "',LogOff='1' where entry_code='" + Session["Entry_Code"] + "'", "Text");
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
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
        }
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
        else if (Att_str1 == "PP")
        {
            Attvalue = "13";
        }
        else if (Att_str1 == "SYOD")
        {
            Attvalue = "14";
        }
        else if (Att_str1 == "COD")
        {
            Attvalue = "15";
        }
        else if (Att_str1 == "OOD")
        {
            Attvalue = "16";
        }
        else if (Att_str1 == "LA")
        {
            Attvalue = "17";
        }
        else
        {
            Attvalue = string.Empty;
        }
        return Attvalue;
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

    public void SendingSms(string rollno, string date, string hour, string college, string course, string setting, int total, int absent)
    {
        try
        {
            string Gender = string.Empty;
            string stude_name = string.Empty;
            string collegename1 = string.Empty;
            string Hour = hour;
            string hour_check = string.Empty;
            string admno = string.Empty;
            string app_no = string.Empty;
            string regno = string.Empty;
            string MsgText = string.Empty;
            string RecepientNo = string.Empty;
            int check = 0;
            string user_id = string.Empty;
            collegename1 = college;
            string coursename1 = course;
            string[] split = date.Split(new Char[] { '-' });
            string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date = datefrom;
            if (Convert.ToInt16(hour) == 1)
            {
                Hour = hour + "st ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 2)
            {
                Hour = hour + "nd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 3)
            {
                Hour = hour + "rd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) > 3)
            {
                Hour = hour + "th ";
                hour_check = hour;
            }
            string str1 = string.Empty;
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + group_code + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and group_code='" + group_code + "'and value='1'";
                str1 = str1 + "  select Sections,Roll_Admit,Reg_No,App_No from Registration where Roll_No='" + rollno + "'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and usercode='" + Session["usercode"].ToString() + "'and value='1'";
                str1 = str1 + "  select Sections,Roll_Admit,Reg_No,App_No,stud_name from Registration where Roll_No='" + rollno + "'";
            }
            Boolean flage = false;
            DataSet ds1;
            ds1 = dacces2.select_method_wo_parameter(str1, "txt");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Attendance Sms for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }
                if (flage == true)
                {
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if (ds1.Tables[0].Rows[k]["TextName"].ToString() == "Period" && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (hour_check == final_Hours)
                                {
                                    check = check + 1;
                                }
                            }
                        }
                    }
                }
            }
            if (ds1.Tables[2].Rows.Count > 0)
            {
                regno = Convert.ToString(ds1.Tables[2].Rows[0]["Reg_No"]);
                admno = Convert.ToString(ds1.Tables[2].Rows[0]["Roll_Admit"]);
                app_no = Convert.ToString(ds1.Tables[2].Rows[0]["App_No"]);
                stude_name = Convert.ToString(ds1.Tables[2].Rows[0]["stud_name"]);
            }
            if (check > 0)
            {
                check = 0;
                string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dstrack;
                dstrack = dacces2.select_method_wo_parameter(ssr, "txt");
                if (dstrack.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);
                    string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                    DataSet dsMobile;
                    dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");
                    DataSet ds;
                    if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                        {
                            Gender = "Your Son ";
                        }
                        else
                        {
                            Gender = "Your Daughter ";
                        }
                        DateTime dt = Convert.ToDateTime(date);
                        string section = string.Empty;
                        if (ddlsec.Enabled == true)
                        {
                            section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                            if (section.Trim().ToLower() == "all")
                            {
                                if (ds1.Tables.Count > 0 && ds1.Tables[1].Rows.Count > 0)
                                {
                                    string sectvalue = ds1.Tables[2].Rows[0][0].ToString();
                                    if (sectvalue.Trim() != "" && sectvalue.Trim() != null)
                                    {
                                        section = sectvalue.ToString();
                                    }
                                }
                            }
                        }
                        if (ds1.Tables.Count > 0 && ds1.Tables[1].Rows.Count > 0)
                        {
                            if (setting == "Hour")
                            {
                                string templatevlaue = Convert.ToString(ds1.Tables[1].Rows[0]["template"]);
                                if (templatevlaue.Trim() != "")
                                {
                                    string[] splittemplate = templatevlaue.Split('$');
                                    if (splittemplate.Length > 0)
                                    {
                                        for (int j = 0; j <= splittemplate.GetUpperBound(0); j++)
                                        {
                                            if (splittemplate[j].ToString() != "")
                                            {
                                                if (splittemplate[j].ToString() == "College Name")
                                                {
                                                    MsgText = MsgText + " " + collegename1;
                                                }
                                                else if (splittemplate[j].ToString() == "Student Name")
                                                {
                                                    MsgText = MsgText + " " + dsMobile.Tables[0].Rows[0]["StudName"].ToString();
                                                }
                                                else if (splittemplate[j].ToString().ToLower() == "degree")
                                                {
                                                    MsgText = MsgText + " " + coursename1;
                                                }
                                                else if (splittemplate[j].ToString() == "Section")
                                                {
                                                    if (section.Trim() != "")
                                                    {
                                                        MsgText = MsgText + " " + "" + section + " Section";
                                                    }
                                                }
                                                else if (splittemplate[j].ToString() == "Thank You")
                                                {
                                                    MsgText = MsgText + " " + splittemplate[j].ToString();
                                                }
                                                else if (splittemplate[j].ToString() == "Absent")
                                                {
                                                    //commented by prabha on jan 24 2018
                                                    //MsgText = MsgText + " " + Hour + " hour Absent";
                                                    MsgText = MsgText + " " + Hour + " hour";
                                                }
                                                else if (splittemplate[j].ToString() == "Conducted Hours")
                                                {
                                                    MsgText = MsgText + " Conducted hours:" + total + "";
                                                }
                                                else if (splittemplate[j].ToString() == "Absent hours")
                                                {
                                                    MsgText = MsgText + " Absent hours:" + absent + "";
                                                }
                                                else if (splittemplate[j].ToString() == "Roll No")
                                                {
                                                    MsgText = MsgText + " " + rollno;
                                                }
                                                else if (splittemplate[j].ToString() == "Register No")
                                                {
                                                    MsgText = MsgText + " " + regno;
                                                }
                                                else if (splittemplate[j].ToString() == "Application No")
                                                {
                                                    MsgText = MsgText + " " + app_no;
                                                }
                                                else if (splittemplate[j].ToString() == "Admission No")
                                                {
                                                    MsgText = MsgText + " " + admno;
                                                }
                                                else if (splittemplate[j].ToString() == "Date") //added by prabha 
                                                {
                                                    MsgText = MsgText + " " + txtFromDate.Text.Trim();
                                                }
                                                else
                                                {
                                                    if (MsgText == "")
                                                    {
                                                        MsgText = splittemplate[j].ToString();
                                                    }
                                                    else
                                                    {
                                                        MsgText = MsgText + " " + splittemplate[j].ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (setting == "Hour")
                            {
                                MsgText = "Dear Parent, Good Morning. This Message from" + " " + collegename1 + ". Your ward " + dsMobile.Tables[0].Rows[0]["StudName"].ToString() + " of " + coursename1 + "-" + section + " is found absent  " + Hour + " hour. Conducted hour:" + total + " Absent hour:" + absent + ". Thank you";
                            }
                        }
                        for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                        {
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                    string getval = dacces2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    string strpath = string.Empty;
                                    //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    int nofosmssend = dacces2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                    string getval = dacces2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    string strpath = string.Empty;
                                    //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    int nofosmssend = dacces2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                    string getval = dacces2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //string strpath =string.Empty;
                                    //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    int nofosmssend = dacces2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (flage == true)
                {
                    if (setting == "Day")
                    {
                        string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dstrack;
                        dstrack = dacces2.select_method_wo_parameter(ssr, "txt");
                        if (dstrack.Tables[0].Rows.Count > 0)
                        {
                            user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);
                            string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                            DataSet dsMobile;
                            dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");
                            DataSet ds;
                            if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                            {
                                if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                                {
                                    Gender = "Your Son ";
                                }
                                else
                                {
                                    Gender = "Your Daughter ";
                                }
                                DateTime dt = Convert.ToDateTime(date);
                                string section = string.Empty;
                                if (ddlsec.Enabled == true)
                                {
                                    section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                                }
                                if (ds1.Tables.Count > 0 && ds1.Tables[1].Rows.Count > 0)
                                {
                                    if (setting == "Day")
                                    {
                                        string templatevlaue = Convert.ToString(ds1.Tables[1].Rows[0]["template"]);
                                        if (templatevlaue.Trim() != "")
                                        {
                                            string[] splittemplate = templatevlaue.Split('$');
                                            if (splittemplate.Length > 0)
                                            {
                                                for (int j = 0; j <= splittemplate.GetUpperBound(0); j++)
                                                {
                                                    if (splittemplate[j].ToString() != "")
                                                    {
                                                        if (splittemplate[j].ToString() == "College Name")
                                                        {
                                                            MsgText = MsgText + " " + collegename1;
                                                        }
                                                        else if (splittemplate[j].ToString() == "Student Name")
                                                        {
                                                            MsgText = MsgText + " " + dsMobile.Tables[0].Rows[0]["StudName"].ToString();
                                                        }
                                                        else if (splittemplate[j].ToString() == "Degree")
                                                        {
                                                            MsgText = MsgText + " " + coursename1;
                                                        }
                                                        else if (splittemplate[j].ToString() == "Section")
                                                        {
                                                            MsgText = MsgText + " " + "" + section + " Section";
                                                        }
                                                        else if (splittemplate[j].ToString() == "Thank You")
                                                        {
                                                            MsgText = MsgText + " " + splittemplate[j].ToString();
                                                        }
                                                        else if (splittemplate[j].ToString() == "Absent")
                                                        {
                                                            //commented by prabha on jan 24 2018
                                                            //MsgText = MsgText + " " + "absent";
                                                            MsgText = MsgText + " " + "";
                                                        }
                                                        else if (splittemplate[j].ToString() == "Conducted Days")
                                                        {
                                                            MsgText = MsgText + " Conducted Days: " + total + "";
                                                        }
                                                        else if (splittemplate[j].ToString() == "Absent Days")
                                                        {
                                                            MsgText = MsgText + " Absent Days: " + absent + "";
                                                        }
                                                        else if (splittemplate[j].ToString() == "Roll No")
                                                        {
                                                            MsgText = MsgText + " " + rollno;
                                                        }
                                                        else if (splittemplate[j].ToString() == "Register No")
                                                        {
                                                            MsgText = MsgText + " " + regno;
                                                        }
                                                        else if (splittemplate[j].ToString() == "Application No")
                                                        {
                                                            MsgText = MsgText + " " + app_no;
                                                        }
                                                        else if (splittemplate[j].ToString() == "Admission No")
                                                        {
                                                            MsgText = MsgText + " " + admno;
                                                        }
                                                        else if (splittemplate[j].ToString() == "Date")
                                                        {
                                                            MsgText = MsgText + " " + txtFromDate.Text.Trim(); //modified by prabha  jan 25 2018
                                                        }
                                                        else
                                                        {
                                                            if (MsgText == "")
                                                            {
                                                                MsgText = splittemplate[j].ToString();
                                                            }
                                                            else
                                                            {
                                                                MsgText = MsgText + " " + splittemplate[j].ToString();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (setting == "Day")
                                    {
                                        MsgText = "Dear Parent, Good Morning. This Message from " + " " + collegename1 + ". Your ward " + dsMobile.Tables[0].Rows[0]["StudName"].ToString() + " of " + coursename1 + "-" + section + " is found absent today. Conducted Days:" + total + " Absent Days:" + absent + ". Thank you";
                                    }
                                }
                                for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                                {
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {
                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            string strpath = string.Empty;
                                            //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //string isst = "0";
                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                           int nofosmssend = dacces2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }
                                    }
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {
                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            //string strpath =string.Empty;
                                            //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //string isst = "0";
                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = dacces2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }
                                    }
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {
                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            //string strpath =string.Empty;
                                            //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //string isst = "0";
                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                           int nofosmssend = dacces2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (setting == "Minimun Absent Day")
                    {
                        string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dstrack;
                        dstrack = dacces2.select_method_wo_parameter(ssr, "txt");
                        if (dstrack.Tables[0].Rows.Count > 0)
                        {
                            user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);
                            string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                            DataSet dsMobile;
                            dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");
                            string days = string.Empty;
                            if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                            {
                                if (minimum_day == "1")
                                {
                                    days = "Day";
                                }
                                if (minimum_day != "1")
                                {
                                    days = "Days";
                                }
                                DateTime dt = Convert.ToDateTime(date);
                                string date1 = txtFromDate.Text;
                                string[] splitdate = date1.Split('-');
                                DateTime statrtdate = Convert.ToDateTime(splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString());
                                statrtdate = statrtdate.AddDays(7);
                                string seconddate = txtFromDate.Text;
                                string finalseconddate = seconddate.Replace("-", "/");
                                MsgText = "Dear Parent, your ward is absent for " + minimum_day + " " + days + " (" + countarray[countarray.Count - 1].ToString() + " to " + finalseconddate.ToString() + "). You are requested to meet Principal/Hod on or before a week " + statrtdate.ToString("dd/MM/yyyy") + ". Thank you!.";
                                for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                                {
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {
                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            //string strpath =string.Empty;
                                            //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //string isst = "0";
                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = dacces2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }
                                    }
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {
                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            //string strpath =string.Empty;
                                            //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //string isst = "0";
                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = dacces2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }
                                    }
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {
                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            //string strpath =string.Empty;
                                            //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //string isst = "0";
                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = dacces2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText,"0");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void smsreport(string uril, string isstaff, DateTime dt, string phone, string msg)
    {
        try
        {
            string phoneno = phone;
            string message = msg;
            string date = dt.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = string.Empty;
            groupmsgid = strvel;
            string[] splitvalue = groupmsgid.Split(' ');
            int sms = 0;
            string smsreportinsert = string.Empty;
            string[] split_mobileno = phoneno.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + split_mobileno[icount] + "','" + splitvalue[0] + "','" + message + "','" + Session["collegecode"].ToString() + "','" + isstaff + "','" + date + "','" + Session["UserCode"].ToString() + "')";// Added by jairam 21-11-2014
                sms = dacces2.insert_method(smsreportinsert, hat, "Text");
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void loadcollegename()
    {
        try
        {
            string collquery = "Select collname,Coll_acronymn from collinfo where college_code=" + Session["collegecode"].ToString() + "";
            DataSet datacol = new DataSet();
            datacol.Clear();
            datacol = dacces2.select_method_wo_parameter(collquery, "Text");
            if (datacol.Tables[0].Rows.Count > 0)
            {
                collacronym = datacol.Tables[0].Rows[0]["Coll_acronymn"].ToString();
                collegename = datacol.Tables[0].Rows[0]["collname"].ToString();
            }
            string degreequery = "select distinct Course_Name,Dept_Name from Department dep, Degree deg, course c where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and deg.college_code =" + Session["collegecode"].ToString() + " and Degree_Code=" + ddlbranch.SelectedItem.Value + "";
            DataSet dscode = new DataSet();
            dscode = dacces2.select_method_wo_parameter(degreequery, "Text");
            if (dscode.Tables[0].Rows.Count > 0)
            {
                string course = dscode.Tables[0].Rows[0]["Course_Name"].ToString();
                string deptname = dscode.Tables[0].Rows[0]["Dept_Name"].ToString();
                coursename = course + "-" + deptname;
            }
        }
        catch
        {
        }
    }

    public void sendvoicecall(string rollno, string date, string hour, string batch, string degree, string college, string course, string setting)
    {
        try
        {
            string Hour = hour;
            string hour_check = string.Empty;
            string roll = rollno;
            string batchyear = batch;
            string coursename = course;
            string voicelanguage = string.Empty;
            string collegename = college;
            string MsgText = string.Empty;
            string RecepientNo = string.Empty;
            int check = 0;
            string[] split = date.Split(new Char[] { '-' });
            string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date = datefrom;
            if (Convert.ToInt16(hour) == 1)
            {
                Hour = hour + "st ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 2)
            {
                Hour = hour + "nd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 3)
            {
                Hour = hour + "rd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) > 3)
            {
                Hour = hour + "th ";
                hour_check = hour;
            }
            string str1 = string.Empty;
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + group_code + "'";
                str1 = str1 + "  select Sections  from Registration where Roll_No='" + rollno + "'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
                str1 = str1 + "  select Sections  from Registration where Roll_No='" + rollno + "'";
            }
            Boolean flage = false;
            DataSet ds1;
            ds1 = dacces2.select_method_wo_parameter(str1, "txt");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Voice Call for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }
                if (flage == true)
                {
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if (ds1.Tables[0].Rows[k]["TextName"].ToString() == "Period" && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (hour_check == final_Hours)
                                {
                                    check = check + 1;
                                }
                            }
                        }
                    }
                }
            }
            string section_voice = string.Empty;
            if (ddlsec.Enabled == true)
            {
                section_voice = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                if (section_voice.Trim().ToLower() == "all")
                {
                    if (ds1.Tables.Count > 0 && ds1.Tables[1].Rows.Count > 0)
                    {
                        string sectvalue = ds1.Tables[2].Rows[0][0].ToString();
                        if (sectvalue.Trim() != "" && sectvalue.Trim() != null)
                        {
                            section_voice = sectvalue.ToString();
                        }
                    }
                }
            }
            if (check > 0)
            {
                check = 0;
                string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dsMobile;
                dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");
                string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                if (voicelang != "")
                {
                    string langquery = string.Empty;
                    langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                    DataSet datalang = new DataSet();
                    datalang = dacces2.select_method_wo_parameter(langquery, "Text");
                    if (datalang.Tables[0].Rows.Count > 0)
                    {
                        voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                    }
                }
                // voicelanguage = "English";
                if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                {
                    string gender = string.Empty;
                    if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                    {
                        gender = "MALE";
                    }
                    else
                    {
                        gender = "FEMALE";
                    }
                    string orginalname = string.Empty;
                    string student_name = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudName"]);
                    if (student_name.Contains(".") == true)
                    {
                        string[] splitname = student_name.Split('.');
                        for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                        {
                            string lengthname = splitname[i].ToString();
                            if (lengthname.Trim().Length > 2)
                            {
                                orginalname = splitname[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        string[] split2ndname = student_name.Split(' ');
                        if (split2ndname.Length > 0)
                        {
                            for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                            {
                                string firstname = split2ndname[k].ToString();
                                if (firstname.Trim().Length > 2)
                                {
                                    if (orginalname == "")
                                    {
                                        orginalname = firstname.ToString();
                                    }
                                    else
                                    {
                                        orginalname = orginalname + " " + firstname.ToString();
                                    }
                                }
                            }
                        }
                    }
                    DateTime dt = Convert.ToDateTime(date);
                    biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                    MsgText = "ABSETN AT ";
                    for (int jj1 = 0; jj1 < ds.Tables[0].Rows.Count; jj1++)
                    {
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                            {
                                RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                            }
                        }
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                            {
                                RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                            }
                        }
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                            {
                                RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                            }
                        }
                    }
                }
            }
            else
            {
                if (flage == true)
                {
                    if (setting == "Day")
                    {
                        string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dsMobile;
                        dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");
                        string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                        if (voicelang != "")
                        {
                            string langquery = string.Empty;
                            langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                            DataSet datalang = new DataSet();
                            datalang = dacces2.select_method_wo_parameter(langquery, "Text");
                            if (datalang.Tables[0].Rows.Count > 0)
                            {
                                voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                            }
                        }
                        if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                        {
                            string gender = string.Empty;
                            if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                            {
                                gender = "MALE";
                            }
                            else
                            {
                                gender = "FEMALE";
                            }
                            string orginalname = string.Empty;
                            string student_name = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudName"]);
                            if (student_name.Contains(".") == true)
                            {
                                string[] splitname = student_name.Split('.');
                                for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                                {
                                    string lengthname = splitname[i].ToString();
                                    if (lengthname.Trim().Length > 2)
                                    {
                                        orginalname = splitname[i].ToString();
                                    }
                                }
                            }
                            else
                            {
                                string[] split2ndname = student_name.Split(' ');
                                if (split2ndname.Length > 0)
                                {
                                    for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                                    {
                                        string firstname = split2ndname[k].ToString();
                                        if (firstname.Trim().Length > 2)
                                        {
                                            if (orginalname == "")
                                            {
                                                orginalname = firstname.ToString();
                                            }
                                            else
                                            {
                                                orginalname = orginalname + " " + firstname.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            DateTime dt = Convert.ToDateTime(date);
                            biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                            MsgText = "ABSETN AT ";
                            for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                            {
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                            }
                        }
                    }
                    else if (setting == "Minimun Absent Day")
                    {
                        string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dsMobile;
                        dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");
                        string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                        if (voicelang != "")
                        {
                            string langquery = string.Empty;
                            langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                            DataSet datalang = new DataSet();
                            datalang = dacces2.select_method_wo_parameter(langquery, "Text");
                            if (datalang.Tables[0].Rows.Count > 0)
                            {
                                voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                            }
                        }
                        if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                        {
                            string gender = string.Empty;
                            if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                            {
                                gender = "MALE";
                            }
                            else
                            {
                                gender = "FEMALE";
                            }
                            string orginalname = string.Empty;
                            string student_name = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudName"]);
                            if (student_name.Contains(".") == true)
                            {
                                string[] splitname = student_name.Split('.');
                                for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                                {
                                    string lengthname = splitname[i].ToString();
                                    if (lengthname.Trim().Length > 2)
                                    {
                                        orginalname = splitname[i].ToString();
                                    }
                                }
                            }
                            else
                            {
                                string[] split2ndname = student_name.Split(' ');
                                if (split2ndname.Length > 0)
                                {
                                    for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                                    {
                                        string firstname = split2ndname[k].ToString();
                                        if (firstname.Trim().Length > 2)
                                        {
                                            if (orginalname == "")
                                            {
                                                orginalname = firstname.ToString();
                                            }
                                            else
                                            {
                                                orginalname = orginalname + " " + firstname.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            DateTime dt = Convert.ToDateTime(date);
                            biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                            MsgText = "ABSETN AT ";
                            for (int jj1 = 0; jj1 < ds.Tables[0].Rows.Count; jj1++)
                            {
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void attendacesavefunction()
    {
        try
        {
            staticarrhourss.Clear();
            for (int colvis = 4; colvis < gvatte.Columns.Count; colvis++)
            {
                if (gvatte.Columns[colvis].Visible == true)
                {
                    staticarrhourss.Add(colvis);
                }
            }
            Boolean savefalg = false;
            int savevalue = 0;
            int insert = 0;
            string insertvalues = string.Empty;
            string updatevalues = string.Empty;
            string monthandyear = string.Empty;
            loadcollegename();
            DataSet data1 = new DataSet();
            ArrayList notarray = new ArrayList();
            Hashtable holiday = new Hashtable();
            WebService web = new WebService();
            if (txtFromDate.Text != "")
            {
                if (gvatte.Columns.Count > 1)
                {
                    string typeval = string.Empty;
                    if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
                    {
                        typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
                    }
                    string fdtae = txtFromDate.Text.ToString();
                    string[] spd = fdtae.Split('-');
                    string fmon = spd[1].ToString();
                    string fyear = spd[2].ToString();
                    string fdate = spd[0].ToString();
                    int monval = Convert.ToInt32((Convert.ToInt32(fyear) * 12) + Convert.ToInt32(fmon));
                    string fdateval = spd[1].ToString() + '/' + spd[0].ToString() + '/' + spd[2].ToString();
                    string savehoursqlstrq;
                    int totalhor;
                    string noofhours_save = string.Empty;
                    string no_firsthalf = string.Empty;
                    string no_secondhalf = string.Empty;
                    string no_minpresent_firsthalf = string.Empty;
                    string no_minpresent_secondhalf = string.Empty;
                    string min_per_day = string.Empty;
                    savehoursqlstrq = "select distinct No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_II_half_day ,min_pres_I_half_day,min_hrs_per_day  from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "";
                    if (rbelective.Checked == true)
                    {
                        savehoursqlstrq = "select distinct No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_II_half_day ,min_pres_I_half_day,min_hrs_per_day  from PeriodAttndSchedule p,Degree d,Course c where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and p.semester=" + ddlsem.SelectedValue.ToString() + "";
                    }
                    ds.Clear();
                    ds = dacces2.select_method_wo_parameter(savehoursqlstrq, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        noofhours_save = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                        no_firsthalf = ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                        no_secondhalf = ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                        no_minpresent_firsthalf = ds.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
                        no_minpresent_secondhalf = ds.Tables[0].Rows[0]["min_pres_II_half_day"].ToString();
                        min_per_day = ds.Tables[0].Rows[0]["min_hrs_per_day"].ToString();
                    }
                    totalhor = Convert.ToInt32(noofhours_save);
                    string str_Date;
                    string appNo = string.Empty;
                    string str_rollno;
                    string str_hour;
                    string Atyear;
                    string Atmonth;
                    long strdate;
                    string str_day;
                    string Att_mark;
                    string Att_value;
                    string dcolumn;
                    string Splitmondate;
                    str_Date = string.Empty;
                    str_rollno = string.Empty;
                    str_hour = string.Empty;
                    str_day = string.Empty;
                    Att_mark = string.Empty;
                    Att_value = string.Empty;
                    dcolumn = string.Empty;
                    Splitmondate = string.Empty;
                    string hourwise = string.Empty;
                    string daywise = string.Empty;
                    string hourwisedata = string.Empty;
                    string daywisedata = string.Empty;
                    string minimum = string.Empty;
                    string minimun_data = string.Empty;
                    string settingquery = string.Empty;
                    settingquery = "select TextName,Taxtval from Attendance_Settings where  College_Code ='" + Session["collegecode"].ToString() + "'and user_id='" + Session["usercode"].ToString() + "'";
                    settingquery = settingquery + " ;select a.* from attendance a,Registration r where a.roll_no=r.roll_no and a.month_year='" + monval + "' and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "'";
                    if (rbelective.Checked == true)
                    {
                        settingquery = "select TextName,Taxtval from Attendance_Settings where  College_Code ='" + Session["collegecode"].ToString() + "'and user_id='" + Session["usercode"].ToString() + "'";
                        settingquery = settingquery + " ;select a.* from attendance a,Registration r,Degree d,Course c where a.roll_no=r.roll_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and a.month_year='" + monval + "' " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "'";
                    }
                    ds.Clear();
                    ds = dacces2.select_method_wo_parameter(settingquery, "Text");
                    DataTable dtattenda = ds.Tables[1];
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ArrayList addarray = new ArrayList();
                        DataView dv_demand_data = new DataView();
                        ds.Tables[0].DefaultView.RowFilter = "TextName in ('Hour','Day','Minimun Absent Day','Minimun Days')";
                        dv_demand_data = ds.Tables[0].DefaultView;
                        if (dv_demand_data.Count > 0)
                        {
                            for (int i = 0; i < dv_demand_data.Count; i++)
                            {
                                if (dv_demand_data[i]["TextName"].ToString() == "Hour" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 1)
                                {
                                    hourwise = "1";
                                    hourwisedata = "Hour";
                                }
                                else if (dv_demand_data[i]["TextName"].ToString() == "Hour" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 0)
                                {
                                    hourwise = "0";
                                }
                                if (dv_demand_data[i]["TextName"].ToString() == "Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 1)
                                {
                                    daywise = "1";
                                    daywisedata = "Day";
                                }
                                else if (dv_demand_data[i]["TextName"].ToString() == "Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 0)
                                {
                                    daywise = "0";
                                }
                                if (dv_demand_data[i]["TextName"].ToString() == "Minimun Absent Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 1)
                                {
                                    minimum = "1";
                                    minimun_data = "Minimun Absent Day";
                                }
                                else if (dv_demand_data[i]["TextName"].ToString() == "Minimun Absent Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 0)
                                {
                                    minimum = "0";
                                }
                                if (dv_demand_data[i]["TextName"].ToString() == "Minimun Days" && Convert.ToString(dv_demand_data[i]["Taxtval"]) != "")
                                {
                                    minimum_day = Convert.ToString(dv_demand_data[i]["Taxtval"]);
                                }
                                else if (dv_demand_data[i]["TextName"].ToString() == "Minimun Days" && Convert.ToString(dv_demand_data[i]["Taxtval"]) == "")
                                {
                                    minimum_day = string.Empty;
                                }
                            }
                        }
                    }
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
                    int startsem_date = 0;
                    string start_Date = string.Empty;
                    string startdatequery = string.Empty;
                    startdatequery = "select leavecode from AttMasterSetting where calcflag='2' and collegecode=" + Session["collegecode"].ToString() + "";
                    startdatequery = startdatequery + " select convert(varchar(10),start_date,103) as start_date from seminfo where  degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlsem.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "'";
                    startdatequery = startdatequery + " select convert(varchar(10),holiday_date,103)as holiday_date ,halforfull,morning,evening from holidayStudents where holiday_date='" + fdateval + "' and degree_code ='" + ddlbranch.SelectedValue.ToString() + "' and semester ='" + ddlsem.SelectedValue.ToString() + "'";
                    if (rbelective.Checked == true)
                    {
                        startdatequery = "select leavecode from AttMasterSetting where calcflag='2' and collegecode=" + Session["collegecode"].ToString() + "";
                        startdatequery = startdatequery + " select distinct convert(varchar(10),s.start_date,103) as start_date from seminfo s,Degree d,Course c where s.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and s.semester='" + ddlsem.SelectedValue.ToString() + "' and s.batch_year='" + ddlbatch.SelectedValue.ToString() + "'";
                        startdatequery = startdatequery + " select distinct convert(varchar(10),holiday_date,103)as holiday_date ,halforfull,morning,evening from holidayStudents h,Degree d,Course c where h.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and h.holiday_date='" + fdateval + "' " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and h.semester ='" + ddlsem.SelectedValue.ToString() + "'";
                    }
                    data1.Clear();
                    data1 = dacces2.select_method_wo_parameter(startdatequery, "Text");
                    if (data1.Tables[0].Rows.Count > 0)
                    {
                        for (int val = 0; val < data1.Tables[0].Rows.Count; val++)
                        {
                            notarray.Add(data1.Tables[0].Rows[val]["leavecode"].ToString());
                        }
                    }
                    if (data1.Tables[1].Rows.Count > 0)
                    {
                        start_Date = data1.Tables[1].Rows[0]["start_date"].ToString();
                        string[] split = start_Date.Split(new Char[] { '/' });
                        string str_day1 = split[0].ToString();
                        string Atmonth1 = split[1].ToString();
                        string Atyear1 = split[2].ToString();
                        startsem_date = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
                    }
                    if (data1.Tables[2].Rows.Count > 0)
                    {
                        for (int hol = 0; hol < data1.Tables[2].Rows.Count; hol++)
                        {
                            string date = data1.Tables[2].Rows[hol]["holiday_date"].ToString();
                            string conncetion = data1.Tables[2].Rows[hol]["holiday_date"].ToString() + "*" + data1.Tables[2].Rows[hol]["halforfull"].ToString() + "*" + data1.Tables[2].Rows[hol]["morning"].ToString() + "*" + data1.Tables[2].Rows[hol]["evening"].ToString();
                            if (!holiday.ContainsKey(date))
                            {
                                holiday.Add(date, conncetion);
                            }
                        }
                    }
                    int total_conduct_hour = 0;
                    int absent_hour = 0;
                    Boolean noentryflag = false;
                    for (int Att_row = 0; Att_row <= gvatte.Rows.Count - 3; Att_row++)
                    {
                        str_rollno = (gvatte.Rows[Att_row].Cells[1].FindControl("lblroll_no") as Label).Text;
                        appNo = dacces2.GetFunction("select app_no from registration where roll_no='" + str_rollno + "' and college_code='" + Session["collegecode"].ToString() + "'");
                        if ((gvatte.Rows[Att_row].Cells[1].FindControl("lblroll_no") as Label).ForeColor != Color.Red)
                        {
                            insertvalues = string.Empty;
                            updatevalues = string.Empty;
                            monthandyear = string.Empty;
                            string values = string.Empty;
                            string existattndval = string.Empty;
                            int colcount1 = 0;
                            string getvalue = string.Empty;
                            for (int Att_column = 6; Att_column <= gvatte.Columns.Count - 1; Att_column++)
                            {
                                if (staticarrhourss.Contains(Att_column))
                                {
                                    //string cimageids = "cimg" + Att_column;
                                    string timageids = "chk" + Att_column;
                                    string lblpres = "p" + Att_column;
                                    colcount1++;
                                    str_Date = txtFromDate.Text;
                                    string[] tmpdate = str_Date.ToString().Split(new char[] { ' ' });
                                    str_Date = tmpdate[0].ToString();
                                    Splitmondate = str_Date.ToString();
                                    string[] split = Splitmondate.Split(new Char[] { '-' });
                                    str_day = split[0].ToString().TrimStart('0');
                                    Atmonth = split[1].ToString();
                                    Atyear = split[2].ToString();
                                    strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                    str_hour = GVhead.Rows[0].Cells[Att_column].Text.ToString();
                                    string[] split_hr = str_hour.Split(new Char[] { '-' });
                                    str_hour = str_hour[0].ToString();
                                    dcolumn = "d" + str_day + "d" + str_hour;
                                    Att_mark = (gvatte.Rows[Att_row].Cells[Att_column].FindControl(lblpres) as Label).Text.ToUpper();
                                    Att_value = Attvalues(Att_mark);
                                    getvalue = Att_value;
                                    if (Att_value == "")
                                    {
                                        Att_value = "0";
                                    }
                                    if (Att_value != "0")
                                    {
                                        nullflag = true;
                                    }
                                    if (minimum != "1")
                                    {
                                        if (hourwise == "1")
                                        {
                                            if (absent_calcflag.Count > 0)
                                            {
                                                if (absent_calcflag.Contains(getvalue) == true)
                                                {
                                                    string value_return = web.coundected_hour(strdate, startsem_date, str_rollno, absent_calcflag, notarray);
                                                    if (value_return == "Empty")
                                                    {
                                                        total_conduct_hour = 1;
                                                        absent_hour = 1;
                                                    }
                                                    else
                                                    {
                                                        string[] splitvalue = value_return.Split('-');
                                                        if (splitvalue.Length > 0)
                                                        {
                                                            if (splitvalue[0].ToString() != "")
                                                            {
                                                                total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                                                total_conduct_hour++;
                                                            }
                                                            else
                                                            {
                                                                total_conduct_hour++;
                                                            }
                                                            if (splitvalue[1].ToString() != "")
                                                            {
                                                                absent_hour = Convert.ToInt32(splitvalue[1]);
                                                                absent_hour++;
                                                            }
                                                            else
                                                            {
                                                                absent_hour++;
                                                            }
                                                        }
                                                    }
                                                    SendingSms(str_rollno, Splitmondate, str_hour, collacronym, coursename, hourwisedata, total_conduct_hour, absent_hour);
                                                }
                                            }
                                        }
                                    }
                                    if (insertvalues == "")
                                    {
                                        insertvalues = dcolumn;
                                        values = Att_value;
                                        updatevalues = dcolumn + "=" + Att_value;
                                    }
                                    else
                                    {
                                        insertvalues = insertvalues + ',' + dcolumn;
                                        values = values + ',' + Att_value;
                                        updatevalues = updatevalues + ',' + dcolumn + "=" + Att_value;
                                    }
                                    dtattenda.DefaultView.RowFilter = " roll_no='" + str_rollno + "' and month_year='" + strdate + "'";
                                    DataView dvstuattmon = dtattenda.DefaultView;
                                    if (dvstuattmon.Count > 0)
                                    {
                                        string setval = dvstuattmon[0][dcolumn].ToString();
                                        if (existattndval == "")
                                        {
                                            existattndval = dcolumn + "=" + setval;
                                        }
                                        else
                                        {
                                            existattndval = existattndval + ',' + dcolumn + "=" + setval;
                                        }
                                    }
                                    else
                                    {
                                        string setval = "0";
                                        if (existattndval == "")
                                        {
                                            existattndval = dcolumn + "=" + setval;
                                        }
                                        else
                                        {
                                            existattndval = existattndval + ',' + dcolumn + "=" + setval;
                                        }
                                    }
                                    if (monthandyear == "")
                                    {
                                        monthandyear = strdate.ToString();
                                    }
                                    if (monthandyear != strdate.ToString() || Att_column == Convert.ToInt32(staticarrhourss[staticarrhourss.Count - 1]))
                                    {
                                        if (existattndval != updatevalues)
                                        {
                                            hat.Clear();
                                            hat.Add("Att_App_no", appNo);
                                            hat.Add("Att_CollegeCode", Session["collegecode"].ToString());
                                            hat.Add("rollno", str_rollno);
                                            hat.Add("monthyear", monthandyear);
                                            hat.Add("columnname", insertvalues);
                                            hat.Add("colvalues", values);
                                            hat.Add("coulmnvalue", updatevalues);
                                            insert = dacces2.insert_method("sp_ins_upd_student_attendance_Dead", hat, "sp");
                                            savevalue = 1;
                                            #region Added by Idhris 29-12-2016
                                            string[] split1 = txtFromDate.Text.Split(new Char[] { '-' });
                                            str_day = split1[0].ToString().TrimStart('0');
                                            Atmonth = split1[1].ToString();
                                            Atyear = split1[2].ToString();
                                            string dtFrom = split1[1] + "/" + split1[0] + "/" + split1[2];
                                            strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                            StringBuilder sb_aattddaayy = new StringBuilder();
                                            for (int hrsI = 1; hrsI <= Convert.ToInt32(noofhours_save); hrsI++)
                                            {
                                                sb_aattddaayy.Append("d" + str_day + "d" + hrsI + ",");
                                            }
                                            if (sb_aattddaayy.Length > 1)
                                            {
                                                sb_aattddaayy.Remove(sb_aattddaayy.Length - 1, 1);
                                            }
                                            attendanceMark(appNo, (int)strdate, sb_aattddaayy.ToString(), Convert.ToInt32(noofhours_save), Convert.ToInt32(no_firsthalf), Convert.ToInt32(no_secondhalf), Convert.ToInt32(no_minpresent_firsthalf), Convert.ToInt32(no_minpresent_secondhalf), dtFrom, Session["collegecode"].ToString());
                                            #endregion
                                        }
                                        insertvalues = string.Empty;
                                        updatevalues = string.Empty;
                                        monthandyear = string.Empty;
                                        values = string.Empty;
                                        existattndval = string.Empty;
                                        if (monthandyear != strdate.ToString())
                                        {
                                            monthandyear = strdate.ToString();
                                            insertvalues = dcolumn;
                                            values = Att_value;
                                            updatevalues = dcolumn + "=" + Att_value;
                                        }
                                        savefalg = true;
                                        noentryflag = true;
                                    }
                                }
                            }
                            if (minimum != "1")
                            {
                                if (daywise == "1")
                                {
                                    string fromdate = txtFromDate.Text;
                                    string todate = txtFromDate.Text;
                                    string[] fromdatesplit = fromdate.Split('-');
                                    string[] todatesplit = todate.Split('-');
                                    DateTime newfromdate = Convert.ToDateTime(fromdatesplit[1].ToString() + "/" + fromdatesplit[0].ToString() + "/" + fromdatesplit[2].ToString());
                                    DateTime newtodate = Convert.ToDateTime(todatesplit[1].ToString() + "/" + todatesplit[0].ToString() + "/" + todatesplit[2].ToString());
                                    TimeSpan dt = new TimeSpan();
                                    dt = newtodate.Subtract(newfromdate);
                                    int days = dt.Days;
                                    if (days == 0)
                                    {
                                        string newdate = newtodate.ToString("dd/MM/yyyy");
                                        string[] newdatesplit = newdate.Split('/');
                                        string date_value = newdatesplit[0].ToString();
                                        date_value = date_value.TrimStart('0');
                                        string date_value_table = "d" + date_value;
                                        string month_value = newdatesplit[1].ToString();
                                        string year_value = newdatesplit[2].ToString();
                                        string monty_year_value = Convert.ToString((Convert.ToInt32(year_value) * 12 + Convert.ToInt32(month_value)));
                                        string date_value_table_day = string.Empty;
                                        for (int k = 1; k <= totalhor; k++)
                                        {
                                            if (date_value_table_day == "")
                                            {
                                                date_value_table_day = date_value_table + "d" + k;
                                            }
                                            else
                                            {
                                                date_value_table_day = date_value_table_day + "," + date_value_table + "d" + k;
                                            }
                                        }
                                        int split_day_hour = 0;
                                        int first_split_present = 0;
                                        int second_split_absent = 0;
                                        int notconsider = 0;
                                        int first_split_absent = 0;
                                        int second_split_present = 0;
                                        int firstempty_count = 0;
                                        int secondempty_count = 0;
                                        bool attendflag = false;
                                        string absent_count_query = string.Empty;
                                        absent_count_query = "Select " + date_value_table_day + " from attendance where roll_no ='" + str_rollno + "'and month_year in ('" + monty_year_value + "')";
                                        ds.Clear();
                                        ds = dacces2.select_method_wo_parameter(absent_count_query, "Text");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                            {
                                                split_day_hour++;
                                                string attendvalue = Convert.ToString(ds.Tables[0].Rows[0][j]);
                                                if (attendvalue != "")
                                                {
                                                    if (present_calcflag.Count > 0)
                                                    {
                                                        if (split_day_hour <= Convert.ToInt32(no_firsthalf))
                                                        {
                                                            if (present_calcflag.Contains(attendvalue) == true)
                                                            {
                                                                first_split_present++;
                                                            }
                                                            else if (absent_calcflag.Contains(attendvalue) == true)
                                                            {
                                                                first_split_absent++;
                                                            }
                                                            else if (attendvalue == "" || attendvalue == "0" || attendvalue == null || attendvalue == "H")
                                                            {
                                                                firstempty_count++;
                                                            }
                                                            else
                                                            {
                                                                notconsider++;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (present_calcflag.Contains(attendvalue) == true)
                                                            {
                                                                second_split_present++;
                                                            }
                                                            else if (absent_calcflag.Contains(attendvalue) == true)
                                                            {
                                                                second_split_absent++;
                                                            }
                                                            else if (attendvalue == "" || attendvalue == null || attendvalue == "0" || attendvalue == "H")
                                                            {
                                                                secondempty_count++;
                                                            }
                                                            else
                                                            {
                                                                notconsider++;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        attendflag = true;
                                                    }
                                                }
                                            }
                                            if (attendflag == false)
                                            {
                                                if (firstempty_count < Convert.ToInt32(no_minpresent_firsthalf))
                                                {
                                                    if (secondempty_count < Convert.ToInt32(no_minpresent_secondhalf))
                                                    {
                                                        if (first_split_present < Convert.ToInt32(no_minpresent_firsthalf) && second_split_present < Convert.ToInt32(no_minpresent_secondhalf))
                                                        {
                                                            if (first_split_absent != 0 && second_split_absent != 0)
                                                            {
                                                                string days_count = web.condected_days(start_Date, fromdate, str_rollno, absent_calcflag, notarray, totalhor, no_firsthalf, no_secondhalf, present_calcflag, no_minpresent_firsthalf, no_minpresent_secondhalf, holiday);
                                                                if (days_count == "Empty")
                                                                {
                                                                    total_conduct_hour = 1;
                                                                    absent_hour = 1;
                                                                }
                                                                else
                                                                {
                                                                    string[] splitvalue = days_count.Split('-');
                                                                    if (splitvalue.Length > 0)
                                                                    {
                                                                        if (splitvalue[0].ToString() != "")
                                                                        {
                                                                            total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                                                        }
                                                                        else
                                                                        {
                                                                            total_conduct_hour++;
                                                                        }
                                                                        if (splitvalue[1].ToString() != "")
                                                                        {
                                                                            absent_hour = Convert.ToInt32(splitvalue[1]);
                                                                        }
                                                                        else
                                                                        {
                                                                            absent_hour++;
                                                                        }
                                                                    }
                                                                }
                                                                SendingSms(str_rollno, Splitmondate, str_hour, collacronym, coursename, daywisedata, total_conduct_hour, absent_hour);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (minimum == "1")
                            {
                                if (minimum_day != "")
                                {
                                    int parentsmeat = 0;
                                    int firsthalf = 0;
                                    int secondhalf = 0;
                                    int firsthalfabsent = 0;
                                    int secondhalfabent = 0;
                                    int firstemptycount = 0;
                                    int secondemptycount = 0;
                                    int emptycount = 0;
                                    string mini_absent_days = minimum_day.ToString();
                                    string date = txtFromDate.Text;
                                    string[] splitdate = date.Split('-');
                                    DateTime statrtdate = Convert.ToDateTime(splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString());
                                    ArrayList addmonthyear = new ArrayList();
                                    ArrayList adddate = new ArrayList();
                                    Hashtable addmonthdate = new Hashtable();
                                    DataSet dsholidy = new DataSet();
                                    DataView dv = new DataView();
                                    string hollydayquery = string.Empty;
                                    hollydayquery = "select * from holidayStudents where degree_code=" + ddldegree.SelectedItem.Value + " and semester=" + ddlsem.SelectedItem.Value + "";
                                    dsholidy.Clear();
                                    dsholidy = dacces2.select_method_wo_parameter(hollydayquery, "Text");
                                    for (int i = 0; i < Convert.ToInt32(mini_absent_days); i++)
                                    {
                                        if (dsholidy.Tables[0].Rows.Count > 0)
                                        {
                                            dv.Table.DefaultView.RowFilter = "holiday_date='" + statrtdate.ToString("MM/dd/yyyy") + "'";
                                            if (dv.Count > 0)
                                            {
                                                string holidayvalue = Convert.ToString(dv[0]["halforfull"]);
                                                if (holidayvalue != "")
                                                {
                                                    if (holidayvalue == "0" || holidayvalue == "1")
                                                    {
                                                        i--;
                                                    }
                                                }
                                            }
                                            if (statrtdate.ToString("dddd") != "Sunday")
                                            {
                                                string date1 = statrtdate.ToString("dd/MM/yyyy");
                                                countarray.Add(date1);
                                                string[] splitdate1 = date1.Split('/');
                                                string firstdate = splitdate1[0].ToString();
                                                firstdate = firstdate.TrimStart('0');
                                                string finaldate = "d" + firstdate;
                                                adddate.Add(finaldate);
                                                string firstmonth = splitdate1[1].ToString();
                                                string firstyear = splitdate1[2].ToString();
                                                string datevalue = Convert.ToString((Convert.ToInt32(firstyear) * 12 + Convert.ToInt32(firstmonth)));
                                                addmonthyear.Add((datevalue));
                                            }
                                            else
                                            {
                                                i--;
                                            }
                                        }
                                        else
                                        {
                                            if (statrtdate.ToString("dddd") != "Sunday")
                                            {
                                                string date1 = statrtdate.ToString("dd/MM/yyyy");
                                                countarray.Add(date1);
                                                string[] splitdate1 = date1.Split('/');
                                                string firstdate = splitdate1[0].ToString();
                                                firstdate = firstdate.TrimStart('0');
                                                string finaldate = "d" + firstdate;
                                                adddate.Add(finaldate);
                                                string firstmonth = splitdate1[1].ToString();
                                                string firstyear = splitdate1[2].ToString();
                                                string datevalue = Convert.ToString((Convert.ToInt32(firstyear) * 12 + Convert.ToInt32(firstmonth)));
                                                addmonthyear.Add((datevalue));
                                            }
                                            else
                                            {
                                                i--;
                                            }
                                        }
                                        statrtdate = statrtdate.AddDays(-1);
                                    }
                                    if (adddate.Count > 0)
                                    {
                                        for (int i = 0; i < adddate.Count; i++)
                                        {
                                            string value = adddate[i].ToString();
                                            string dayvalue = string.Empty;
                                            if (totalhor > 0)
                                            {
                                                for (int k = 1; k <= totalhor; k++)
                                                {
                                                    if (dayvalue == "")
                                                    {
                                                        dayvalue = value + "d" + k;
                                                    }
                                                    else
                                                    {
                                                        dayvalue = dayvalue + "," + value + "d" + k;
                                                    }
                                                }
                                                bool attendflag = false;
                                                int dscolcount = 0;
                                                string attendancequery = string.Empty;
                                                attendancequery = "Select " + dayvalue + " from attendance where roll_no ='" + str_rollno + "'and month_year in ('" + addmonthyear[i].ToString() + "')";
                                                ds.Clear();
                                                ds = dacces2.select_method_wo_parameter(attendancequery, "Text");
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                                    {
                                                        dscolcount++;
                                                        string attendvalue = Convert.ToString(ds.Tables[0].Rows[0][j]);
                                                        if (attendvalue != "")
                                                        {
                                                            if (present_calcflag.Count > 0)
                                                            {
                                                                if (dscolcount <= Convert.ToInt32(no_firsthalf))
                                                                {
                                                                    if (present_calcflag.Contains(attendvalue) == true)
                                                                    {
                                                                        firsthalf++;
                                                                    }
                                                                    else if (absent_calcflag.Contains(attendvalue) == true)
                                                                    {
                                                                        firsthalfabsent++;
                                                                    }
                                                                    else if (attendvalue == "" || attendvalue == "0" || attendvalue == null || attendvalue == "H")
                                                                    {
                                                                        firstemptycount++;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (present_calcflag.Contains(attendvalue) == true)
                                                                    {
                                                                        secondhalf++;
                                                                    }
                                                                    else if (absent_calcflag.Contains(attendvalue) == true)
                                                                    {
                                                                        secondhalfabent++;
                                                                    }
                                                                    else if (attendvalue == "" || attendvalue == null || attendvalue == "0" || attendvalue == "H")
                                                                    {
                                                                        secondemptycount++;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                attendflag = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            emptycount++;
                                                        }
                                                    }
                                                    if (attendflag == false)
                                                    {
                                                        if (emptycount < Convert.ToInt32(min_per_day)) ;
                                                        if (firstemptycount < Convert.ToInt32(no_minpresent_firsthalf) && secondemptycount < Convert.ToInt32(no_minpresent_secondhalf))
                                                        {
                                                            if (firsthalf < Convert.ToInt32(no_minpresent_firsthalf) && secondhalf < Convert.ToInt32(no_minpresent_secondhalf))
                                                            {
                                                                if (firsthalfabsent != 0 && secondhalfabent != 0)
                                                                {
                                                                    parentsmeat++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (Convert.ToInt32(mini_absent_days) == parentsmeat)
                                    {
                                        DateTime datesnowcc = DateTime.Now;
                                        string purpose01 = "" + mini_absent_days + "  Days Absent Parents Meet ";
                                        string days_count = web.condected_days(start_Date, txtFromDate.Text, str_rollno, absent_calcflag, notarray, totalhor, no_firsthalf, no_secondhalf, present_calcflag, no_minpresent_firsthalf, no_minpresent_secondhalf, holiday);
                                        if (days_count == "Empty")
                                        {
                                            total_conduct_hour = 0;
                                            absent_hour = 0;
                                        }
                                        else
                                        {
                                            string[] splitvalue = days_count.Split('-');
                                            if (splitvalue.Length > 0)
                                            {
                                                if (splitvalue[0].ToString() != "")
                                                {
                                                    total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                                }
                                                else
                                                {
                                                    total_conduct_hour++;
                                                }
                                                if (splitvalue[1].ToString() != "")
                                                {
                                                    absent_hour = Convert.ToInt32(splitvalue[1]);
                                                }
                                                else
                                                {
                                                    absent_hour++;
                                                }
                                            }
                                        }
                                        SendingSms(str_rollno, Splitmondate, str_hour, collacronym, coursename, daywisedata, total_conduct_hour, absent_hour);
                                        parentsmeet(str_rollno, datesnowcc, purpose01);//added by sridhar 13 aug 2014
                                    }
                                }
                            }
                        }
                    }
                    if (nullflag == true)
                    {
                        lblset.Visible = false;
                    }
                    else
                    {
                        lblset.Visible = true;
                        lblset.Text = "Mark Attendance For Save";
                    }
                    for (Att_mark_column = 6; Att_mark_column < gvatte.Columns.Count; Att_mark_column++)
                    {
                        if (staticarrhourss.Contains(Att_mark_column))
                        {
                            absent_count = 0;
                            present_count = 0;
                            string timageids = "chk" + Att_mark_column;
                            string lblpres = "p" + Att_mark_column;
                            for (Att_mark_row = 0; Att_mark_row < gvatte.Rows.Count - 2; Att_mark_row++)
                            {
                                if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text != "" && (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text != null) //condn 09.08.12 mythili
                                {
                                    string getvalue = Attvalues((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text.ToUpper());
                                    if (present_calcflag.ContainsKey(getvalue))
                                    {
                                        if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                                        {
                                            present_count++;
                                        }
                                    }
                                    if (absent_calcflag.ContainsKey(getvalue))
                                    {
                                        if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                                        {
                                            absent_count++;
                                        }
                                    }
                                }
                            }
                            (gvatte.Rows[gvatte.Rows.Count - 2].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = present_count.ToString();
                            (gvatte.Rows[gvatte.Rows.Count - 1].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = absent_count.ToString();
                        }
                    }
                    if (savefalg == true)
                    {
                        if (rbelective.Checked == true && Session["witotttname"] == "1")
                        {
                            string stubatch = string.Empty;
                            if (ddlstubatch.Enabled == true && ddlstubatch.Items.Count > 0)
                            {
                                if (ddlstubatch.Text.ToString() != "All")
                                {
                                    stubatch = " and sc.batch='" + ddlstubatch.Text.ToString() + "'";
                                }
                            }
                            string includediscon = " and r.delflag=0 and isnull(r.ProLongAbsent,0)<>'1'";
                            string getshedulockva = dacces2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount'");
                            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                            {
                                includediscon = string.Empty;
                            }
                            string includedebar = " and r.exam_flag <> 'DEBAR'";
                            getshedulockva = dacces2.GetFunctionv("select value from Master_Settings where settings='Attendance Debar'");
                            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                            {
                                includedebar = string.Empty;
                            }
                            string sqlstr = "select distinct r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.subject_no from Registration r,subject s,subjectChooser sc,Degree d,course c,syllabus_master sy where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and r.Current_Semester=sc.semester and sy.syll_code=s.syll_code and sy.degree_code=d.Degree_Code and sy.semester=r.Current_Semester and sy.degree_code=r.degree_code " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "' " + stubatch + " and r.cc=0 " + includediscon + " " + includedebar + "";
                            DataSet dsdegree = dacces2.select_method_wo_parameter(sqlstr, "Text");
                            string strquery = "select s.degree_code,start_date,isnull(starting_dayorder,1) as starting_dayorder,schorder,nodays,No_of_hrs_per_day,min_hrs_per_day,batch_year,s.degree_code,s.semester from seminfo s,periodattndschedule p,Degree d,Course c where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and batch_year=" + ddlbatch.Text.ToString() + " and s.semester=" + ddlsem.SelectedValue.ToString() + "";
                            DataSet dssemin = dacces2.select_method(strquery, hat, "Text");
                            for (int de = 0; de < dsdegree.Tables[0].Rows.Count; de++)
                            {
                                string degreecode = dsdegree.Tables[0].Rows[0]["degree_code"].ToString();
                                string byeard = dsdegree.Tables[0].Rows[0]["Batch_Year"].ToString();
                                string seme = dsdegree.Tables[0].Rows[0]["Current_Semester"].ToString();
                                string scet = dsdegree.Tables[0].Rows[0]["Sections"].ToString();
                                string subjectno = dsdegree.Tables[0].Rows[0]["subject_no"].ToString();
                                dssemin.Tables[0].DefaultView.RowFilter = "batch_year='" + ddlbatch.Text.ToString() + "' and degree_code='" + degreecode + "' and semester='" + ddlsem.Text.ToString() + "'";
                                DataView dvsemin = dssemin.Tables[0].DefaultView;
                                if (dvsemin.Count > 0)
                                {
                                    string scheduloredr = dvsemin[0]["schorder"].ToString();
                                    string start_datesem = dvsemin[0]["start_date"].ToString();
                                    string noofdays = dvsemin[0]["nodays"].ToString();
                                    string start_dayorder = dvsemin[0]["starting_dayorder"].ToString();
                                    string sectvakl = string.Empty;
                                    if (scet.Trim() != "" && scet != "0" && scet.Trim() != "-1")
                                    {
                                        sectvakl = " and sections='" + scet + "'";
                                    }
                                    else
                                    {
                                        scet = string.Empty;
                                    }
                                    string datestr = txtFromDate.Text.ToString();
                                    string[] spdt = datestr.Split('-');
                                    DateTime dtf = Convert.ToDateTime(spdt[1] + '/' + spdt[0] + '/' + spdt[2]);
                                    string strday = dtf.ToString("ddd");
                                    if (scheduloredr == "0")
                                    {
                                        strday = dacces2.findday(dtf.ToString(), degreecode, ddlsem.SelectedValue.ToString(), ddlbatch.Text.ToString(), start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                                    }
                                    for (int Att_column = 6; Att_column <= gvatte.Columns.Count - 1; Att_column++)
                                    {
                                        Boolean subexflag = false;
                                        if (staticarrhourss.Contains(Att_column))
                                        {
                                            string temp = GVhead.Rows[0].Cells[Att_column].Text.ToString();
                                            string[] split_hr = temp.Split(new Char[] { '-' });
                                            temp = split_hr[0].ToString();
                                            string setalte = dacces2.GetFunction("select " + strday + temp + " from Alternate_Schedule where batch_year='" + ddlbatch.Text.ToString() + "' and degree_code='" + degreecode + "' and semester='" + ddlsem.Text.ToString() + "' " + sectvakl + " and FromDate= '" + dtf.ToString() + "'");
                                            string[] spba = setalte.Split(';');
                                            for (int li = 0; li <= spba.GetUpperBound(0); li++)
                                            {
                                                string[] sphb = spba[li].Split('-');
                                                if (sphb.GetUpperBound(0) >= 1)
                                                {
                                                    if (sphb[0].ToString().Trim().ToLower() == subjectno.Trim().ToLower())
                                                    {
                                                        subexflag = true;
                                                    }
                                                }
                                            }
                                            if (subexflag == false)
                                            {
                                                setalte = setalte + ";" + subjectno + "--s";
                                                string strquery1 = "if exists(select * from Alternate_Schedule where batch_year='" + ddlbatch.Text.ToString() + "' and degree_code='" + degreecode + "' and semester='" + ddlsem.Text.ToString() + "' " + sectvakl + " and FromDate= '" + dtf.ToString() + "')";
                                                strquery1 = strquery1 + " Update Alternate_Schedule set " + strday + temp + "='" + setalte + "' where batch_year='" + ddlbatch.Text.ToString() + "' and degree_code='" + degreecode + "' and semester='" + ddlsem.Text.ToString() + "' " + sectvakl + " and FromDate= '" + dtf.ToString() + "'";
                                                strquery1 = strquery1 + " ELse insert into Alternate_Schedule(batch_year,degree_code,semester,Sections,FromDate," + strday + temp + ") values('" + ddlbatch.Text.ToString() + "','" + degreecode + "','" + ddlsem.Text.ToString() + "','" + scet + "','" + dtf.ToString() + "','" + setalte + "')";
                                                int insert1 = dacces2.update_method_wo_parameter(strquery1, "Text");
                                            }
                                        }
                                    }
                                    string inserquery = "if not exists(select * from Semester_Schedule where batch_year='" + ddlbatch.Text.ToString() + "' and degree_code='" + degreecode + "' and semester='" + ddlsem.Text.ToString() + "' " + sectvakl + " ) insert into Semester_Schedule (degree_code,batch_year,semester,sections,TTName,FromDate,lastrec) values(" + degreecode + "," + ddlbatch.Text.ToString() + "," + ddlsem.Text.ToString() + ",'" + scet + "','" + ddlbatch.Text.ToString() + "','" + dtf + "',1)";
                                    int sfsafa = dacces2.insert_method(inserquery, hat, "Text");
                                }
                            }
                        }
                        string entrycode = Session["Entry_Code"].ToString();
                        string formname = "Student Attendance Entry";
                        string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                        string doa = DateTime.Now.ToString("MM/dd/yyy");
                        string section = string.Empty;
                        if (ddlsec.SelectedValue.ToString() != "" && ddlsec.SelectedValue.ToString().Trim().ToLower() != "all" && ddlsec.SelectedValue.ToString() != null && ddlsec.SelectedValue.ToString() != "0")
                        {
                            section = ":Sections -" + ddlsec.SelectedValue.ToString();
                        }
                        string details = "" + ddlbranch.SelectedValue.ToString() + ":Sem - " + ddlsem.SelectedValue.ToString() + ":Batch Year -" + ddlbatch.SelectedValue.ToString() + " " + section + "";
                        string modules = "0";
                        string act_diff = " ";
                        string ctsname = "Update Attendance Information";
                        if (noentryflag == true)
                        {
                            if (savevalue == 1)
                            {
                                ctsname = "Save the Attendance Inforamtion";
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved Successfully')", true);
                            }
                            else
                            {
                                ctsname = "Update the Attendance Information";
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved Successfully')", true);//updated
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Update Attendance And Save')", true);
                        }
                        string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                        int a = dacces2.update_method_wo_parameter(strlogdetails, "Text");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    protected void chkhour_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            hiddenfields();
            gvatte.Visible = false;
            GVhead.Visible = false;
            btngvsave.Visible = false;
            string hourval = string.Empty;
            if (chkhour.Checked == true)
            {
                for (int i = 0; i < chklshour.Items.Count; i++)
                {
                    chklshour.Items[i].Selected = true;
                    if (hourval.Trim() == "")
                    {
                        hourval = chklshour.Items[i].Text.ToString();
                    }
                    else
                    {
                        hourval = hourval + ", " + chklshour.Items[i].Text.ToString();
                    }
                }
                if (chklshour.Items.Count > 0)
                {
                    // txthour.Text = "Hours(" + (chklshour.Items.Count) + ")";
                    txthour.Text = hourval;
                }
            }
            else
            {
                for (int i = 0; i < chklshour.Items.Count; i++)
                {
                    chklshour.Items[i].Selected = false;
                }
                txthour.Text = "---Select---";
            }
            ddlcopyfrom.Items.Clear();
            for (int sj = 0; sj < chklshour.Items.Count; sj++)
            {
                if (chklshour.Items[sj].Selected == true)
                {
                    ddlcopyfrom.Items.Add(chklshour.Items[sj].Value.ToString());
                }
            }
        }
        catch
        {
        }
    }

    protected void chklshour_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hiddenfields();
            gvatte.Visible = false;
            GVhead.Visible = false;
            btngvsave.Visible = false;
            int commcount = 0;
            chkhour.Checked = false;
            txthour.Text = "---Select---";
            string hourval = string.Empty;
            for (int i = 0; i < chklshour.Items.Count; i++)
            {
                if (chklshour.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    if (hourval.Trim() == "")
                    {
                        hourval = chklshour.Items[i].Text.ToString();
                    }
                    else
                    {
                        hourval = hourval + ", " + chklshour.Items[i].Text.ToString();
                    }
                }
            }
            if (commcount > 0)
            {
                //txthour.Text = "Hours(" + commcount.ToString() + ")";
                txthour.Text = hourval;
                if (commcount == chklshour.Items.Count)
                {
                    chkhour.Checked = true;
                }
            }
            ddlcopyfrom.Items.Clear();
            for (int sj = 0; sj < chklshour.Items.Count; sj++)
            {
                if (chklshour.Items[sj].Selected == true)
                {
                    ddlcopyfrom.Items.Add(chklshour.Items[sj].Value.ToString());
                }
            }
        }
        catch
        {
        }
    }

    protected void gvatte_SelectedIndexChanege(object sender, EventArgs e)
    {
    }

    protected void OnDataBound(object sender, EventArgs e)
    {
    }

    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        //e.Row.Cells[0].Width = 42;
        //e.Row.Cells[1].Width = 70;
        //e.Row.Cells[2].Width = 70;
        //e.Row.Cells[3].Width = 190;
        //e.Row.Cells[4].Width = 100;
        //e.Row.Cells[5].Width = 75;
        //e.Row.Cells[6].Width = 75;
        //e.Row.Cells[7].Width = 75;
        //e.Row.Cells[8].Width = 75;
        //e.Row.Cells[9].Width = 75;
        //e.Row.Cells[10].Width = 75;
        //e.Row.Cells[11].Width = 75;
        //e.Row.Cells[12].Width = 75;
        //gvatte.Width = 1000;
        //GVhead.Width = 1000;
        int nofospan = 6;
        if (Session["Rollflag"].ToString() == "0")
        {
            nofospan--;
            gvatte.Columns[1].Visible = false;
        }
        if (Session["Regflag"].ToString() == "0")
        {
            nofospan--;
            gvatte.Columns[2].Visible = false;
        }
        if (Session["AdmissionNo"].ToString() == "0")
        {
            nofospan--;
            gvatte.Columns[3].Visible = false;
        }
        if (Session["Studflag"].ToString() == "0")
        {
            nofospan--;
            gvatte.Columns[5].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 4; i < e.Row.Cells.Count; i++)
            {
                //e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvatte, "Type-" + 0 + "$" + e.Row.RowIndex);
                if (e.Row.Enabled == true)
                    e.Row.Cells[0].Attributes["onclick"] = "checkvalue('" + (e.Row.RowIndex) + "')";
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "onMouseOver('" + (e.Row.RowIndex) + "')";
            e.Row.Attributes["onmouseout"] = "onMouseOut('" + (e.Row.RowIndex) + "')";
        }
    }

    protected void gv_OnRowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        string cmdnae = e.CommandName;
        int row = Convert.ToInt32(e.CommandArgument);
        string[] split = cmdnae.Split('-');
        string columnname = string.Empty;
        string timageids = string.Empty;
        string nexttimageids = string.Empty;
        string prevtimageids = string.Empty;
        string lblpres = string.Empty;
        string nextlblpres = string.Empty;
        string prevlblpres = string.Empty;
        string checkgvtextpresent = "0";
        if (split.Length > 0)
        {
            columnname = Convert.ToString(split[1]);
            timageids = "chk" + columnname;
            nexttimageids = "chk" + Convert.ToString((Convert.ToInt32(columnname) + 1));
            lblpres = "p" + columnname;
            nextlblpres = "p" + Convert.ToString((Convert.ToInt32(columnname) + 1));
            prevlblpres = "p" + Convert.ToString((Convert.ToInt32(columnname) - 1));
            prevtimageids = "chk" + Convert.ToString((Convert.ToInt32(columnname) - 1));
        }
        string lastrows = Convert.ToString(gvatte.Rows[row].Cells[0].Text);
        if (gvatte.Rows.Count > 0)
        {
            if (lastrows.Trim() != "No Of Student(s) Present:" && lastrows.Trim() != "No Of Student(s) Absent:")
            {
                if (Convert.ToInt32(columnname) == 0)
                {
                    checkgvtextpresent = Convert.ToString((gvatte.Rows[row].Cells[4].FindControl("p5") as Label).Text);
                    for (int d = 6; d < gvatte.Columns.Count; d++)
                    {
                        timageids = "chk" + d;
                        lblpres = "p" + d;
                        if (checkgvtextpresent == "p")
                        {
                            (gvatte.Rows[row].Cells[d].FindControl(timageids) as CheckBox).Checked = false;
                            (gvatte.Rows[row].Cells[d].FindControl(lblpres) as Label).Text = "a";
                            gvatte.Rows[row].Cells[d].BackColor = Color.Red;
                        }
                        else
                        {
                            (gvatte.Rows[row].Cells[d].FindControl(timageids) as CheckBox).Checked = true;
                            (gvatte.Rows[row].Cells[d].FindControl(lblpres) as Label).Text = "p";
                            gvatte.Rows[row].Cells[d].BackColor = Color.Green;
                        }
                    }
                }
            }
        }
        if (gvatte.Rows.Count > 0)
        {
            for (Att_mark_column = 6; Att_mark_column < gvatte.Columns.Count; Att_mark_column++)
            {
                absent_count = 0;
                present_count = 0;
                timageids = "chk" + Att_mark_column;
                lblpres = "p" + Att_mark_column;
                for (Att_mark_row = 0; Att_mark_row < gvatte.Rows.Count - 3; Att_mark_row++)
                {
                    if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text != "")
                    {
                        if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text == "p")
                        {
                            if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                            {
                                present_count++;
                            }
                        }
                        else if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text == "a")
                        {
                            if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                            {
                                absent_count++;
                            }
                        }
                    }
                }
                (gvatte.Rows[gvatte.Rows.Count - 2].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = present_count.ToString();
                (gvatte.Rows[gvatte.Rows.Count - 1].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = absent_count.ToString();
            }
            gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].Text = "No Of Student(s) Present:";
            gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].Text = "No Of Student(s) Absent:";
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "SyncTableColumns();", true);
    }

    protected void GVhead_OnRowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        string cmdnae = e.CommandName;
        int row = Convert.ToInt32(e.CommandArgument);
        string[] split = cmdnae.Split('-');
        string columnname = string.Empty;
        string timageids = string.Empty;
        string nexttimageids = string.Empty;
        string prevtimageids = string.Empty;
        string lblpres = string.Empty;
        string nextlblpres = string.Empty;
        string prevlblpres = string.Empty;
        int checkpresentattence = 0;
        if (split.Length > 0)
        {
            columnname = Convert.ToString(split[1]);
            timageids = "chk" + columnname;
            nexttimageids = "chk" + Convert.ToString((Convert.ToInt32(columnname) + 1));
            lblpres = "p" + columnname;
            nextlblpres = "p" + Convert.ToString((Convert.ToInt32(columnname) + 1));
            prevlblpres = "p" + Convert.ToString((Convert.ToInt32(columnname) - 1));
            prevtimageids = "chk" + Convert.ToString((Convert.ToInt32(columnname) - 1));
        }
        if (gvatte.Rows.Count > 0)
        {
            if (Convert.ToString((gvatte.Rows[row].Cells[4].FindControl(lblpres) as Label).Text.ToLower()) == "p")
            {
                checkpresentattence = 0;
            }
            else
            {
                checkpresentattence = 1;
            }
            for (int d = 0; d < gvatte.Rows.Count - 2; d++)
            {
                if (checkpresentattence == 1)
                {
                    (gvatte.Rows[d].FindControl(timageids) as CheckBox).Checked = true;
                    (gvatte.Rows[d].FindControl(lblpres) as Label).Visible = false;
                    (gvatte.Rows[d].FindControl(lblpres) as Label).Text = "p";
                    //gvatte.Rows[d].Cells[0].BackColor = Color.Green;
                    gvatte.Rows[d].Cells[Convert.ToInt32(columnname)].BackColor = Color.Green;
                }
                else
                {
                    (gvatte.Rows[d].FindControl(timageids) as CheckBox).Checked = false;
                    (gvatte.Rows[d].FindControl(lblpres) as Label).Visible = false;
                    (gvatte.Rows[d].FindControl(lblpres) as Label).Text = "a";
                    gvatte.Rows[d].Cells[Convert.ToInt32(columnname)].BackColor = Color.Red;
                }
            }
        }
        if (gvatte.Rows.Count > 0)
        {
            for (Att_mark_column = 6; Att_mark_column < gvatte.Columns.Count; Att_mark_column++)
            {
                absent_count = 0;
                present_count = 0;
                timageids = "chk" + Att_mark_column;
                lblpres = "p" + Att_mark_column;
                for (Att_mark_row = 0; Att_mark_row < gvatte.Rows.Count - 3; Att_mark_row++)
                {
                    if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text != "")
                    {
                        string sss = (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text;
                        if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text.ToLower() == "p")
                        {
                            if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                            {
                                present_count++;
                            }
                        }
                        else if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text.ToLower() == "a")
                        {
                            if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                            {
                                absent_count++;
                            }
                        }
                    }
                }
                (gvatte.Rows[gvatte.Rows.Count - 2].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = present_count.ToString();
                (gvatte.Rows[gvatte.Rows.Count - 1].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = absent_count.ToString();
            }
            gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].Text = "No Of Student(s) Present:";
            gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].Text = "No Of Student(s) Absent:";
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "SyncTableColumns();", true);
    }

    protected void GVhead_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Width = 50;
        e.Row.Cells[1].Width = 70;
        e.Row.Cells[2].Width = 70;
        e.Row.Cells[3].Width = 70;
        e.Row.Cells[4].Width = 190;
        e.Row.Cells[5].Width = 100;
        e.Row.Cells[6].Width = 75;
        e.Row.Cells[7].Width = 75;
        e.Row.Cells[8].Width = 75;
        e.Row.Cells[9].Width = 75;
        e.Row.Cells[10].Width = 75;
        e.Row.Cells[11].Width = 75;
        e.Row.Cells[12].Width = 75;
        e.Row.Cells[13].Width = 75;
        gvatte.Width = 1000;
        GVhead.Width = 1000;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 4; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Attributes["onclick"] = "checkvaluecolumn('" + (i) + "')";
            }
        }
    }

    protected void imgbtnpresentclick(object sender, EventArgs e)
    {
        string timageids = string.Empty;
        string lblpres = string.Empty;
        if (gvatte.Rows.Count > 0)
        {
            for (int i = 6; i < gvatte.Columns.Count; i++)
            {
                for (int j = 0; j < gvatte.Rows.Count - 2; j++)
                {
                    if (gvatte.Rows[j].Enabled == true)
                    {
                        timageids = "chk" + i;
                        lblpres = "p" + i;
                        (gvatte.Rows[j].Cells[i].FindControl(timageids) as CheckBox).Checked = true;
                        (gvatte.Rows[j].Cells[i].FindControl(lblpres) as Label).Visible = false;
                        (gvatte.Rows[j].Cells[i].FindControl(lblpres) as Label).Text = "p";
                        if (gvatte.Rows[j].Cells[i].BackColor != Color.DarkViolet)
                        {
                            gvatte.Rows[j].Cells[i].BackColor = Color.Green;
                        }
                    }
                }
            }
        }
        if (gvatte.Rows.Count > 0)
        {
            for (Att_mark_column = 6; Att_mark_column < gvatte.Columns.Count; Att_mark_column++)
            {
                absent_count = 0;
                present_count = 0;
                timageids = "chk" + Att_mark_column;
                lblpres = "p" + Att_mark_column;
                for (Att_mark_row = 0; Att_mark_row < gvatte.Rows.Count - 2; Att_mark_row++)
                {
                    if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text != "")
                    {
                        if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text == "p")
                        {
                            if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                            {
                                present_count++;
                            }
                        }
                        else if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text == "a")
                        {
                            if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                            {
                                absent_count++;
                            }
                        }
                    }
                }
                (gvatte.Rows[gvatte.Rows.Count - 2].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = present_count.ToString();
                (gvatte.Rows[gvatte.Rows.Count - 1].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = absent_count.ToString();
            }
            gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].Text = "No Of Student(s) Present:";
            gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].Text = "No Of Student(s) Absent:";
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "SyncTableColumns();", true);
    }

    protected void imgbtnabstclick(object sender, EventArgs e)
    {
        string timageids = string.Empty;
        string lblpres = string.Empty;
        if (gvatte.Rows.Count > 0)
        {
            for (int i = 6; i < gvatte.Columns.Count; i++)
            {
                for (int j = 0; j < gvatte.Rows.Count - 2; j++)
                {
                    if (gvatte.Rows[j].Enabled == true)
                    {
                        timageids = "chk" + i;
                        lblpres = "p" + i;
                        (gvatte.Rows[j].Cells[i].FindControl(timageids) as CheckBox).Checked = false;
                        (gvatte.Rows[j].Cells[i].FindControl(lblpres) as Label).Visible = false;
                        (gvatte.Rows[j].Cells[i].FindControl(lblpres) as Label).Text = "a";
                        if (gvatte.Rows[j].Cells[i].BackColor != Color.DarkViolet)
                        {
                            gvatte.Rows[j].Cells[i].BackColor = Color.Red;
                        }
                    }
                }
            }
        }
        if (gvatte.Rows.Count > 0)
        {
            for (Att_mark_column = 6; Att_mark_column < gvatte.Columns.Count; Att_mark_column++)
            {
                absent_count = 0;
                present_count = 0;
                timageids = "chk" + Att_mark_column;
                lblpres = "p" + Att_mark_column;
                for (Att_mark_row = 0; Att_mark_row < gvatte.Rows.Count - 2; Att_mark_row++)
                {
                    if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text != "")
                    {
                        if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text == "p")
                        {
                            if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                            {
                                present_count++;
                            }
                        }
                        else if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text == "a")
                        {
                            if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                            {
                                absent_count++;
                            }
                        }
                    }
                }
                (gvatte.Rows[gvatte.Rows.Count - 2].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = present_count.ToString();
                (gvatte.Rows[gvatte.Rows.Count - 1].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = absent_count.ToString();
            }
            gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].Text = "No Of Student(s) Present:";
            gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].Text = "No Of Student(s) Absent:";
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "SyncTableColumns();", true);
    }

    protected void btngvsave_click(object sender, EventArgs e)
    {
        beforesavefn();
        attendacesavefunction();
        if (gvatte.Rows.Count > 0)
        {
            gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].Text = "No Of Student(s) Present:";
            gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].Text = "No Of Student(s) Absent:";
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "SyncTableColumns();", true);
    }

    public void hoursvisiblity()
    {
        ArrayList arrhourss = new ArrayList();
        arrhourss.Clear();
        if (Session["Rollflag"].ToString() == "0")
        {
            GVhead.Rows[0].Cells[1].Visible = false;
        }
        if (Session["Regflag"].ToString() == "0")
        {
            GVhead.Rows[0].Cells[2].Visible = false;
        }
        if (Session["AdmissionNo"].ToString() == "0")
        {
            GVhead.Rows[0].Cells[3].Visible = false;
        }
        if (Session["Studflag"].ToString() == "0")
        {
            GVhead.Rows[0].Cells[5].Visible = false;
        }
        for (int i = 1; i < 10; i++)
        {
            int iii = i + 5;
            gvatte.Columns[iii].Visible = false;
            GVhead.Rows[0].Cells[iii].Visible = false;
        }
        for (int i = 0; i < chklshour.Items.Count; i++)
        {
            if (chklshour.Items[i].Selected == true)
            {
                arrhourss.Add(chklshour.Items[i].Text);
            }
        }
        for (int i = 0; i < arrhourss.Count; i++)
        {
            int num = Convert.ToInt32(arrhourss[i].ToString());
            num = num + 5;
            gvatte.Columns[num].Visible = true;
            GVhead.Rows[0].Cells[num].Visible = true;
        }
    }

    public void dbhoursvisiblity(int starhour, int endhour)
    {
        staticarrhourss.Clear();
        for (int i = 1; i < 10; i++)
        {
            int iii = i + 5;
            gvatte.Columns[iii].Visible = false;
            GVhead.Rows[0].Cells[iii].Visible = false;
        }
        for (int i = starhour; i <= endhour; i++)
        {
            for (int j = 0; j < chklshour.Items.Count; j++)
            {
                if (Convert.ToInt32(chklshour.Items[j].Text) == i && chklshour.Items[j].Selected == true)
                {
                    int num = i;
                    num = num + 5;
                    staticarrhourss.Add(num);
                    gvatte.Columns[num].Visible = true;
                    GVhead.Rows[0].Cells[num].Visible = true;
                    btngvsave.Visible = true;
                }
            }
        }
        if (btngvsave.Visible != true)
        {
            gvatte.Rows[gvatte.Rows.Count - 2].Visible = false;
            gvatte.Rows[gvatte.Rows.Count - 1].Visible = false;
        }
    }

    public void hiddenfields()
    {
        gvatte.Visible = false;
        GVhead.Visible = false;
        btngvsave.Visible = false;
        lblset.Text = string.Empty;
        lblset.Visible = false;
        btnprint.Visible = false;
    }

    public bool daycheck(DateTime seldate)
    {
        DAccess2 da = new DAccess2();
        string collegecode = Session["collegecode"].ToString();
        bool daycheck = false;
        DateTime curdate;//, prevdate;
        long total, k, s;
        string[] ddate = new string[500];
        string c_date = DateTime.Today.ToString();
        DateTime todate_day = Convert.ToDateTime(DateTime.Today.ToString());
        curdate = DateTime.Today;
        if (seldate.ToString() == c_date)
        {
            daycheck = true;
            return daycheck;
        }
        else
        {
            string lockdayvalue = "select lockdays,lflag from collinfo where college_code=" + collegecode + "";
            DataSet ds = new DataSet();
            ds = da.select_method(lockdayvalue, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][1].ToString() == "True")
                    {
                        if (ds.Tables[0].Rows[i][0].ToString() != null && int.Parse(ds.Tables[0].Rows[i][0].ToString()) >= 0)
                        {
                            total = int.Parse(ds.Tables[0].Rows[i][0].ToString());
                            total = total + 1;
                            String strholidasquery = "select holiday_date from holidaystudents where degree_code=" + ddlbranch.SelectedItem.Value.ToString() + "  and semester=" + ddlsem.SelectedItem.Text.ToString() + "";
                            DataSet ds1 = new DataSet();
                            ds1 = da.select_method(strholidasquery, hat, "Text");
                            if (ds1.Tables[0].Rows.Count <= 0)
                            {
                                for (int i1 = 1; i1 < total; i1++)
                                {
                                    string temp_date = todate_day.AddDays(-i1).ToString();
                                    string temp2 = todate_day.AddDays(i1).ToString();
                                    if (temp_date == seldate.ToString())
                                    {
                                        daycheck = true;
                                        return daycheck;
                                    }
                                    if (temp2 == seldate.ToString())
                                    {
                                        daycheck = true;
                                        return daycheck;
                                    }
                                }
                            }
                            else
                            {
                                k = 0;
                                for (int i1 = 1; i1 < ds1.Tables[0].Rows.Count; i1++)
                                {
                                    ddate[k] = ds1.Tables[0].Rows[i1][0].ToString();
                                    k++;
                                }
                                i = 0;
                                while (i <= total - 1)
                                {
                                    string temp_date = curdate.AddDays(-i).ToString();
                                    for (s = 0; s < k - 1; s++)
                                    {
                                        if (temp_date == ddate[s].ToString())
                                        {
                                            total = total + 1;
                                            goto lab;
                                        }
                                    }
                                lab:
                                    i = i + 1;
                                    if (temp_date == seldate.ToString())
                                    {
                                        daycheck = true;
                                        return daycheck;
                                    }
                                }
                            }
                        }
                        else
                        {
                            daycheck = true;
                        }
                    }
                    else
                    {
                        daycheck = true;
                    }
                }
            }
        }
        return daycheck;
    }

    public void beforesavefn()
    {
        string timageids = string.Empty;
        string lblpres = string.Empty;
        if (gvatte.Rows.Count > 0)
        {
            for (int i = 6; i < gvatte.Columns.Count; i++)
            {
                for (int j = 0; j < gvatte.Rows.Count - 2; j++)
                {
                    timageids = "chk" + i;
                    lblpres = "p" + i;
                    if ((gvatte.Rows[j].Cells[i].FindControl(timageids) as CheckBox).Checked == false)
                    {
                        (gvatte.Rows[j].Cells[i].FindControl(lblpres) as Label).Text = "a";
                        if (gvatte.Rows[j].Cells[i].BackColor != Color.DarkViolet)
                        {
                            gvatte.Rows[j].Cells[i].BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        (gvatte.Rows[j].Cells[i].FindControl(lblpres) as Label).Text = "p";
                        if (gvatte.Rows[j].Cells[i].BackColor != Color.DarkViolet)
                        {
                            gvatte.Rows[j].Cells[i].BackColor = Color.Green;
                        }
                    }
                }
            }
        }
        if (gvatte.Rows.Count > 0)
        {
            for (Att_mark_column = 6; Att_mark_column < gvatte.Columns.Count; Att_mark_column++)
            {
                absent_count = 0;
                present_count = 0;
                timageids = "chk" + Att_mark_column;
                lblpres = "p" + Att_mark_column;
                for (Att_mark_row = 0; Att_mark_row < gvatte.Rows.Count - 2; Att_mark_row++)
                {
                    if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text != "")
                    {
                        if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text.ToLower() == "p")
                        {
                            if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                            {
                                present_count++;
                            }
                        }
                        else if ((gvatte.Rows[Att_mark_row].Cells[Att_mark_column].FindControl(lblpres) as Label).Text.ToLower() == "a")
                        {
                            if (gvatte.Rows[Att_mark_row].Cells[Att_mark_column].BackColor != Color.DarkViolet)
                            {
                                absent_count++;
                            }
                        }
                    }
                }
                (gvatte.Rows[gvatte.Rows.Count - 2].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = present_count.ToString();
                (gvatte.Rows[gvatte.Rows.Count - 1].Cells[Att_mark_column].FindControl(lblpres) as Label).Text = absent_count.ToString();
            }
            gvatte.Rows[gvatte.Rows.Count - 2].Cells[0].Text = "No Of Student(s) Present:";
            gvatte.Rows[gvatte.Rows.Count - 1].Cells[0].Text = "No Of Student(s) Absent:";
        }
    }

    protected void btnprint_click(object sender, EventArgs e)
    {
        try
        {
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfPage mypdfpage;
            Font Fontboldbig = new Font("Book Antiqua", 21, FontStyle.Bold);
            Font Fontbold = new Font("Book Antiqua", 17, FontStyle.Bold);
            Font Fontbold2 = new Font("Book Antiqua", 15, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 15, FontStyle.Regular);
            string collname = string.Empty;
            string address = string.Empty;
            string university = string.Empty;
            string strcoll = "select * from collinfo where  college_code='" + Session["collegecode"].ToString() + "'";
            DataSet dshall = dacces2.select_method_wo_parameter(strcoll, "Text");
            if (dshall.Tables[0].Rows.Count > 0)
            {
                collname = dshall.Tables[0].Rows[0]["collname"].ToString();
                string add1 = dshall.Tables[0].Rows[0]["address1"].ToString();
                string add2 = dshall.Tables[0].Rows[0]["address2"].ToString();
                string add3 = dshall.Tables[0].Rows[0]["address3"].ToString();
                string pincode = dshall.Tables[0].Rows[0]["pincode"].ToString();
                university = dshall.Tables[0].Rows[0]["university"].ToString();
                string category = dshall.Tables[0].Rows[0]["category"].ToString();
                if (category.Trim() != "")
                {
                    collname = collname + " (" + category + ")";
                }
                if (add1.Trim() != "")
                {
                    address = add1;
                }
                if (add2.Trim() != "")
                {
                    if (address == "")
                    {
                        address = add2;
                    }
                    else
                    {
                        address = address + ", " + add2;
                    }
                }
                if (add3.Trim() != "")
                {
                    if (address == "")
                    {
                        address = add3;
                    }
                    else
                    {
                        address = address + ", " + add3;
                    }
                }
                if (pincode.Trim() != "")
                {
                    if (address == "")
                    {
                        address = pincode;
                    }
                    else
                    {
                        address = address + " - " + pincode;
                    }
                }
            }
            if (gvatte.Rows.Count > 0)
            {
                string typeval = string.Empty;
                if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
                {
                    typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
                }
                string qryIncludeRedo = string.Empty;
                string rolldegreequery = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,r.Current_Semester,d.Degree_Code from Registration r,Degree d,Course c,Department de where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code  and r.DelFlag=0 and isnull(r.ProLongAbsent,0)<>'1' and r.Exam_Flag<>'debar'";
                if (rbcommon.Checked == true)
                {
                    if (!chkIncludeRedoStudent.Checked)
                        qryIncludeRedo = " and r.cc='0' ";
                    rolldegreequery = rolldegreequery + " " + qryIncludeRedo + " and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "'";
                }
                else
                {
                    rolldegreequery = rolldegreequery + " " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.cc='0' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "'";
                }
                DataSet dsrolldegree = dacces2.select_method_wo_parameter(rolldegreequery, "Text");
                int saveval = 0;
                int rowval = 0;
                string degreedetails = string.Empty;
            StudDet:
                mypdfpage = mydocument.NewPage();
                int coltop = 10;
                coltop = coltop + 10;
                PdfTextArea ptccol = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                coltop = coltop + 30;
                PdfTextArea ptcaddres = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                PdfTextArea ptcdate = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                         new PdfArea(mydocument, 650, 90, 100, 50), System.Drawing.ContentAlignment.MiddleRight, "Day Order :");
                mypdfpage.Add(ptcdate);
                PdfTextArea ptcdayorder = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                         new PdfArea(mydocument, 650, 110, 100, 50), System.Drawing.ContentAlignment.MiddleRight, "Date :");
                mypdfpage.Add(ptcdayorder);
                PdfTextArea ptchr = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                         new PdfArea(mydocument, 650, 130, 100, 50), System.Drawing.ContentAlignment.MiddleRight, "Hour :");
                mypdfpage.Add(ptchr);
                PdfTextArea ptincial = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 10, 1020, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, "Initials");
                mypdfpage.Add(ptincial);
                PdfTextArea ptsign = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 10, 1050, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, "Sign");
                mypdfpage.Add(ptsign);
                PdfTextArea ptsignhod = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 10, 1080, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Hod");
                mypdfpage.Add(ptsignhod);
                coltop = coltop + 60;
                if (rbelective.Checked == true)
                {
                    coltop = coltop + 30;
                }
                int left1 = 10;
                int left2 = 50;
                int left3 = 150;
                int srno = 0;
                coltop = coltop + 20;
                int basetop = coltop;
                int rowCnt = gvatte.Rows.Count - 2;
                int rowrem = rowCnt - rowval;
                if (rowrem > 30)
                    rowrem = 30;
                Font heading = new Font("Book Antiqua", 11, FontStyle.Regular);
                PdfTable tbldet = mydocument.NewTable(heading, rowrem + 1, 8, 6);
                tbldet.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                tbldet.VisibleHeaders = false;
                //  tbldet.HeadersRow.Cells[0] = "Sno";
                tbldet.Cell(0, 0).SetContent("Sno");
                tbldet.Cell(0, 0).SetFont(heading);
                tbldet.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tbldet.Columns[0].SetWidth(30);
                //tbldet.HeadersRow.Cells[1] = "Name";
                tbldet.Cell(0, 1).SetContent("Name");
                tbldet.Cell(0, 1).SetFont(heading);
                tbldet.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                tbldet.Columns[1].SetWidth(240);
                tbldet.Cell(0, 2).SetContent("");
                tbldet.Cell(0, 2).SetFont(heading);
                tbldet.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                tbldet.Columns[2].SetWidth(40);
                tbldet.Cell(0, 3).SetContent("");
                tbldet.Cell(0, 3).SetFont(heading);
                tbldet.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                tbldet.Columns[3].SetWidth(40);
                //  tbldet.HeadersRow.Cells[4] = "Sno";
                tbldet.Cell(0, 4).SetContent("Sno");
                tbldet.Cell(0, 4).SetFont(heading);
                tbldet.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                tbldet.Columns[4].SetWidth(30);
                // tbldet.HeadersRow.Cells[5] = "Name";
                tbldet.Cell(0, 5).SetContent("Name");
                tbldet.Cell(0, 5).SetFont(heading);
                tbldet.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                tbldet.Columns[5].SetWidth(240);
                //tbldet.Cell(0, 6).SetContent("");
                tbldet.Cell(0, 6).SetFont(heading);
                tbldet.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                tbldet.Columns[6].SetWidth(40);
                // tbldet.Cell(0, 7).SetContent("");
                tbldet.Cell(0, 7).SetFont(heading);
                tbldet.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleLeft);
                tbldet.Columns[7].SetWidth(40);
                int colindex = 0;
                int rowindex = 0;
                int degindex = 0;
                for (int r = rowval; r < gvatte.Rows.Count - 2; r++)
                {
                    string regno = (gvatte.Rows[r].Cells[2].FindControl("lblReg_no") as Label).Text;
                    string rollno = (gvatte.Rows[r].Cells[1].FindControl("lblroll_no") as Label).Text;
                    string name = (gvatte.Rows[r].Cells[4].FindControl("lblstud_name") as Label).Text;
                    regno += "-" + name;
                    #region old
                    //if (r == 60)
                    //{
                    //    coltop = basetop;
                    //    if (left1 == 10)
                    //    {
                    //        left1 = 450;
                    //        left2 = 500;
                    //        left3 = 600;
                    //    }
                    //    else
                    //    {
                    //        left1 = 10;
                    //        left2 = 50;
                    //        left3 = 150;
                    //    }
                    //    if (left1 == 10)
                    //    {
                    //        mypdfpage.Add(ptccol);
                    //        if (rbelective.Checked == true)
                    //        {
                    //            string subdetails = ddlsubject.SelectedItem.ToString() + " - " + ddlsubject.SelectedValue.ToString();
                    //            PdfTextArea ptcsubject = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                    //                                                     new PdfArea(mydocument, 0, 70, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, subdetails);
                    //            mypdfpage.Add(ptcsubject);
                    //        }
                    //        mypdfpage.Add(ptcaddres);
                    //        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    //        {
                    //            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    //            mypdfpage.Add(LogoImage, 30, 10, 400);
                    //        }
                    //        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    //        {
                    //            PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                    //            mypdfpage.Add(leftimage, 740, 10, 400);
                    //        }
                    //        mypdfpage.Add(ptcdate);
                    //        mypdfpage.Add(ptcdayorder);
                    //        mypdfpage.Add(ptchr);
                    //        mypdfpage.SaveToDocument();
                    //        mypdfpage = mydocument.NewPage();
                    //    }
                    //}
                    #endregion
                    dsrolldegree.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollno + "'";
                    DataView dvdegeedatils = dsrolldegree.Tables[0].DefaultView;
                    string tempdegree = dvdegeedatils[0]["Batch_Year"].ToString() + " - " + dvdegeedatils[0]["Course_Name"].ToString() + " - " + dvdegeedatils[0]["Dept_Name"].ToString() + " - " + dvdegeedatils[0]["Current_Semester"].ToString();
                    rowindex++;
                    if (rowindex > rowrem)
                    {
                        colindex = 4;
                        rowindex = 0;
                        rowindex++;
                    }
                    if (tempdegree != degreedetails)
                    {
                        coltop = coltop + 25;
                        degreedetails = tempdegree;
                        //PdfTextArea ptadegree = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                        //                                      new PdfArea(mydocument, left1, coltop, 450, 50), System.Drawing.ContentAlignment.MiddleLeft, degreedetails.ToString());
                        //mypdfpage.Add(ptadegree);
                        coltop = coltop + 5;
                        tbldet.Cell(rowindex, colindex).SetContent(degreedetails.ToString());
                        tbldet.Cell(rowindex, colindex).SetFont(heading);
                        tbldet.Cell(rowindex, colindex).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tbldet.Cell(rowindex, colindex).ColSpan = 4;
                        degindex++;
                        // srno++;                  
                        rowindex++;
                        if (rowindex > rowrem)
                        {
                            colindex = 4;
                            rowindex = 0;
                            rowindex++;
                            //   srno++;
                            degindex++;
                            //rowval++;
                            if (degindex >= 60)
                                break;
                        }
                    }
                    coltop = coltop + 20;
                    tbldet.Cell(rowindex, colindex).SetContent(r + 1);
                    tbldet.Cell(rowindex, colindex).SetFont(heading);
                    tbldet.Cell(rowindex, colindex).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tbldet.Cell(rowindex, colindex + 1).SetContent(regno);
                    tbldet.Cell(rowindex, colindex + 1).SetFont(heading);
                    tbldet.Cell(rowindex, colindex + 1).SetContentAlignment(ContentAlignment.BottomLeft);
                    tbldet.Cell(rowindex, colindex + 2).SetContent("");
                    tbldet.Cell(rowindex, colindex + 2).SetFont(heading);
                    tbldet.Cell(rowindex, colindex + 2).SetContentAlignment(ContentAlignment.BottomLeft);
                    tbldet.Cell(rowindex, colindex + 3).SetContent("");
                    tbldet.Cell(rowindex, colindex + 3).SetFont(heading);
                    tbldet.Cell(rowindex, colindex + 3).SetContentAlignment(ContentAlignment.BottomLeft);
                    srno++;
                    degindex++;
                    rowval++;
                    if (degindex >= 60)
                        break;
                }
                PdfTablePage tbldetailss = tbldet.CreateTablePage(new PdfArea(mydocument, 50, 200, 750, 850));
                mypdfpage.Add(tbldetailss);
                mypdfpage.Add(ptccol);
                if (rbelective.Checked == true)
                {
                    string subdetails = ddlsubject.SelectedItem.ToString() + " - " + ddlsubject.SelectedValue.ToString();
                    PdfTextArea ptcsubject = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 0, 70, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, subdetails);
                    mypdfpage.Add(ptcsubject);
                }
                mypdfpage.Add(ptcaddres);
                mypdfpage.Add(ptcdate);
                mypdfpage.Add(ptcdayorder);
                mypdfpage.Add(ptchr);
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 30, 10, 400);
                }
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                {
                    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                    mypdfpage.Add(leftimage, 740, 10, 400);
                }
                mypdfpage.SaveToDocument();
                if (rowCnt > rowval)
                    goto StudDet;
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "AttendancePrint.pdf";
                    mydocument.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            else
            {
                lblset.Visible = true;
                lblset.Text = "No Records Found To Print";
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    //protected void btnprint_click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
    //        Gios.Pdf.PdfPage mypdfpage;
    //        Font Fontboldbig = new Font("Book Antiqua", 21, FontStyle.Bold);
    //        Font Fontbold = new Font("Book Antiqua", 17, FontStyle.Bold);
    //        Font Fontbold2 = new Font("Book Antiqua", 15, FontStyle.Bold);
    //        Font Fontsmall = new Font("Book Antiqua", 15, FontStyle.Regular);
    //        string collname =string.Empty;
    //        string address =string.Empty;
    //        string university =string.Empty;
    //        string strcoll = "select * from collinfo where  college_code='" + Session["collegecode"].ToString() + "'";
    //        DataSet dshall = dacces2.select_method_wo_parameter(strcoll, "Text");
    //        if (dshall.Tables[0].Rows.Count > 0)
    //        {
    //            collname = dshall.Tables[0].Rows[0]["collname"].ToString();
    //            string add1 = dshall.Tables[0].Rows[0]["address1"].ToString();
    //            string add2 = dshall.Tables[0].Rows[0]["address2"].ToString();
    //            string add3 = dshall.Tables[0].Rows[0]["address3"].ToString();
    //            string pincode = dshall.Tables[0].Rows[0]["pincode"].ToString();
    //            university = dshall.Tables[0].Rows[0]["university"].ToString();
    //            string category = dshall.Tables[0].Rows[0]["category"].ToString();
    //            if (category.Trim() != "")
    //            {
    //                collname = collname + " (" + category + ")";
    //            }
    //            if (add1.Trim() != "")
    //            {
    //                address = add1;
    //            }
    //            if (add2.Trim() != "")
    //            {
    //                if (address == "")
    //                {
    //                    address = add2;
    //                }
    //                else
    //                {
    //                    address = address + ", " + add2;
    //                }
    //            }
    //            if (add3.Trim() != "")
    //            {
    //                if (address == "")
    //                {
    //                    address = add3;
    //                }
    //                else
    //                {
    //                    address = address + ", " + add3;
    //                }
    //            }
    //            if (pincode.Trim() != "")
    //            {
    //                if (address == "")
    //                {
    //                    address = pincode;
    //                }
    //                else
    //                {
    //                    address = address + " - " + pincode;
    //                }
    //            }
    //        }
    //        if (gvatte.Rows.Count > 0)
    //        {
    //            string typeval =string.Empty;
    //            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
    //            {
    //                typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
    //            }
    //            string rolldegreequery = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,r.Current_Semester,d.Degree_Code from Registration r,Degree d,Course c,Department de where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code  and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'";
    //            if (rbcommon.Checked == true)
    //            {
    //                rolldegreequery = rolldegreequery + " and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "'";
    //            }
    //            else
    //            {
    //                rolldegreequery = rolldegreequery + " " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "'";
    //            }
    //            DataSet dsrolldegree = dacces2.select_method_wo_parameter(rolldegreequery, "Text");
    //            int saveval = 0;
    //            int rowval = 0;
    //        StudDet:
    //            mypdfpage = mydocument.NewPage();
    //            int coltop = 10;
    //            coltop = coltop + 10;
    //            PdfTextArea ptccol = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
    //                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
    //            coltop = coltop + 30;
    //            PdfTextArea ptcaddres = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
    //            PdfTextArea ptcdate = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydocument, 650, 90, 100, 50), System.Drawing.ContentAlignment.MiddleRight, "Day Order :");
    //            mypdfpage.Add(ptcdate);
    //            PdfTextArea ptcdayorder = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydocument, 650, 110, 100, 50), System.Drawing.ContentAlignment.MiddleRight, "Date :");
    //            mypdfpage.Add(ptcdayorder);
    //            PdfTextArea ptchr = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydocument, 650, 130, 100, 50), System.Drawing.ContentAlignment.MiddleRight, "Hour :");
    //            mypdfpage.Add(ptchr);
    //            PdfTextArea ptincial = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydocument, 10, 1020, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, "Initials");
    //            mypdfpage.Add(ptincial);
    //            PdfTextArea ptsign = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydocument, 10, 1050, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, "Sign");
    //            mypdfpage.Add(ptsign);
    //            PdfTextArea ptsignhod = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydocument, 10, 1080, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Hod");
    //            mypdfpage.Add(ptsignhod);
    //            coltop = coltop + 60;
    //            if (rbelective.Checked == true)
    //            {
    //                coltop = coltop + 30;
    //            }
    //            int left1 = 10;
    //            int left2 = 50;
    //            int left3 = 150;
    //            int srno = 0;
    //            coltop = coltop + 20;
    //            int basetop = coltop;
    //            string degreedetails =string.Empty;
    //            int rowCnt = gvatte.Rows.Count - 2;
    //            int rowrem = rowCnt - rowval;
    //            if (rowrem > 30)
    //                rowrem = 30;
    //            Font heading = new Font("Book Antiqua", 11, FontStyle.Regular);
    //            PdfTable tbldet = mydocument.NewTable(heading, rowrem + 1, 8, 6);
    //            tbldet.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //            tbldet.VisibleHeaders = false;
    //            //  tbldet.HeadersRow.Cells[0] = "Sno";
    //            tbldet.Cell(0, 0).SetContent("Sno");
    //            tbldet.Cell(0, 0).SetFont(heading);
    //            tbldet.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //            tbldet.Columns[0].SetWidth(40);
    //            //tbldet.HeadersRow.Cells[1] = "Name";
    //            tbldet.Cell(0, 1).SetContent("Name");
    //            tbldet.Cell(0, 1).SetFont(heading);
    //            tbldet.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //            tbldet.Columns[1].SetWidth(210);
    //            tbldet.Cell(0, 2).SetContent("");
    //            tbldet.Cell(0, 2).SetFont(heading);
    //            tbldet.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //            tbldet.Columns[2].SetWidth(50);
    //            tbldet.Cell(0, 3).SetContent("");
    //            tbldet.Cell(0, 3).SetFont(heading);
    //            tbldet.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //            tbldet.Columns[3].SetWidth(50);
    //            //  tbldet.HeadersRow.Cells[4] = "Sno";
    //            tbldet.Cell(0, 4).SetContent("Sno");
    //            tbldet.Cell(0, 4).SetFont(heading);
    //            tbldet.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //            tbldet.Columns[4].SetWidth(40);
    //            // tbldet.HeadersRow.Cells[5] = "Name";
    //            tbldet.Cell(0, 5).SetContent("Name");
    //            tbldet.Cell(0, 5).SetFont(heading);
    //            tbldet.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
    //            tbldet.Columns[5].SetWidth(210);
    //            //tbldet.Cell(0, 6).SetContent("");
    //            tbldet.Cell(0, 6).SetFont(heading);
    //            tbldet.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
    //            tbldet.Columns[6].SetWidth(50);
    //            // tbldet.Cell(0, 7).SetContent("");
    //            tbldet.Cell(0, 7).SetFont(heading);
    //            tbldet.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleLeft);
    //            tbldet.Columns[7].SetWidth(50);
    //            int colindex = 0;
    //            int rowindex = 1;
    //            for (int r = rowval; r < gvatte.Rows.Count - 2; r++)
    //            {
    //                string regno = (gvatte.Rows[r].Cells[2].FindControl("lblReg_no") as Label).Text;
    //                string rollno = (gvatte.Rows[r].Cells[2].FindControl("lblroll_no") as Label).Text;
    //                string name = (gvatte.Rows[r].Cells[2].FindControl("lblstud_name") as Label).Text;
    //                regno += "-" + name;
    //                #region old
    //                //if (r == 60)
    //                //{
    //                //    coltop = basetop;
    //                //    if (left1 == 10)
    //                //    {
    //                //        left1 = 450;
    //                //        left2 = 500;
    //                //        left3 = 600;
    //                //    }
    //                //    else
    //                //    {
    //                //        left1 = 10;
    //                //        left2 = 50;
    //                //        left3 = 150;
    //                //    }
    //                //    if (left1 == 10)
    //                //    {
    //                //        mypdfpage.Add(ptccol);
    //                //        if (rbelective.Checked == true)
    //                //        {
    //                //            string subdetails = ddlsubject.SelectedItem.ToString() + " - " + ddlsubject.SelectedValue.ToString();
    //                //            PdfTextArea ptcsubject = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                //                                                     new PdfArea(mydocument, 0, 70, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, subdetails);
    //                //            mypdfpage.Add(ptcsubject);
    //                //        }
    //                //        mypdfpage.Add(ptcaddres);
    //                //        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
    //                //        {
    //                //            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
    //                //            mypdfpage.Add(LogoImage, 30, 10, 400);
    //                //        }
    //                //        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
    //                //        {
    //                //            PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
    //                //            mypdfpage.Add(leftimage, 740, 10, 400);
    //                //        }
    //                //        mypdfpage.Add(ptcdate);
    //                //        mypdfpage.Add(ptcdayorder);
    //                //        mypdfpage.Add(ptchr);
    //                //        mypdfpage.SaveToDocument();
    //                //        mypdfpage = mydocument.NewPage();
    //                //    }
    //                //}
    //                #endregion
    //                dsrolldegree.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollno + "'";
    //                DataView dvdegeedatils = dsrolldegree.Tables[0].DefaultView;
    //                string tempdegree = dvdegeedatils[0]["Batch_Year"].ToString() + " - " + dvdegeedatils[0]["Course_Name"].ToString() + " - " + dvdegeedatils[0]["Dept_Name"].ToString() + " - " + dvdegeedatils[0]["Current_Semester"].ToString();
    //                if (tempdegree != degreedetails)
    //                {
    //                    coltop = coltop + 25;
    //                    degreedetails = tempdegree;
    //                    PdfTextArea ptadegree = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydocument, left1, coltop, 450, 50), System.Drawing.ContentAlignment.MiddleLeft, degreedetails.ToString());
    //                    mypdfpage.Add(ptadegree);
    //                    coltop = coltop + 5;
    //                }
    //                coltop = coltop + 20;
    //                tbldet.Cell(rowindex, colindex).SetContent(r + 1);
    //                tbldet.Cell(rowindex, colindex).SetFont(heading);
    //                tbldet.Cell(rowindex, colindex).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                tbldet.Cell(rowindex, colindex + 1).SetContent(regno);
    //                tbldet.Cell(rowindex, colindex + 1).SetFont(heading);
    //                tbldet.Cell(rowindex, colindex + 1).SetContentAlignment(ContentAlignment.BottomLeft);
    //                tbldet.Cell(rowindex, colindex + 2).SetContent("");
    //                tbldet.Cell(rowindex, colindex + 2).SetFont(heading);
    //                tbldet.Cell(rowindex, colindex + 2).SetContentAlignment(ContentAlignment.BottomLeft);
    //                tbldet.Cell(rowindex, colindex + 3).SetContent("");
    //                tbldet.Cell(rowindex, colindex + 3).SetFont(heading);
    //                tbldet.Cell(rowindex, colindex + 3).SetContentAlignment(ContentAlignment.BottomLeft);
    //                srno++;
    //                rowval++;
    //                if (srno >= 60)
    //                    break;
    //                if (rowindex == 30)
    //                {
    //                    colindex = 4;
    //                    rowindex = 0;
    //                }
    //                rowindex++;
    //            }
    //            PdfTablePage tbldetailss = tbldet.CreateTablePage(new PdfArea(mydocument, 50, 200, 700, 850));
    //            mypdfpage.Add(tbldetailss);
    //            mypdfpage.Add(ptccol);
    //            if (rbelective.Checked == true)
    //            {
    //                string subdetails = ddlsubject.SelectedItem.ToString() + " - " + ddlsubject.SelectedValue.ToString();
    //                PdfTextArea ptcsubject = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydocument, 0, 70, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, subdetails);
    //                mypdfpage.Add(ptcsubject);
    //            }
    //            mypdfpage.Add(ptcaddres);
    //            mypdfpage.Add(ptcdate);
    //            mypdfpage.Add(ptcdayorder);
    //            mypdfpage.Add(ptchr);
    //            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
    //            {
    //                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
    //                mypdfpage.Add(LogoImage, 30, 10, 400);
    //            }
    //            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
    //            {
    //                PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
    //                mypdfpage.Add(leftimage, 740, 10, 400);
    //            }
    //            mypdfpage.SaveToDocument();
    //            if (rowCnt > rowval)
    //                goto StudDet;
    //            string appPath = HttpContext.Current.Server.MapPath("~");
    //            if (appPath != "")
    //            {
    //                string szPath = appPath + "/Report/";
    //                string szFile = "AttendancePrint.pdf";
    //                mydocument.SaveToFile(szPath + szFile);
    //                Response.ClearHeaders();
    //                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
    //                Response.ContentType = "application/pdf";
    //                Response.WriteFile(szPath + szFile);
    //            }
    //        }
    //        else
    //        {
    //            lblset.Visible = true;
    //            lblset.Text = "No Records Found To Print";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblset.Visible = true;
    //        lblset.Text = ex.ToString();
    //    }
    //}

    protected void chkcopyto_ChekedChange(object sender, EventArgs e)
    {
        string hourval = string.Empty;
        if (chkcopyto.Checked == true)
        {
            for (int i = 0; i < chklscopyto.Items.Count; i++)
            {
                chklscopyto.Items[i].Selected = true;
                if (hourval.Trim() == "")
                {
                    hourval = chklscopyto.Items[i].Text.ToString();
                }
                else
                {
                    hourval = hourval + ", " + chklscopyto.Items[i].Text.ToString();
                }
            }
            txtcopyto.Text = hourval;
        }
        else
        {
            for (int i = 0; i < chklscopyto.Items.Count; i++)
            {
                chklscopyto.Items[i].Selected = false;
            }
            txtcopyto.Text = "--Select--";
        }
    }

    protected void chklscopyto_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtcopyto.Text = "--Select--";
        chkcopyto.Checked = false;
        int commcount = 0;
        string hourval = string.Empty;
        for (int i = 0; i < chklscopyto.Items.Count; i++)
        {
            if (chklscopyto.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                if (hourval.Trim() == "")
                {
                    hourval = chklscopyto.Items[i].Text.ToString();
                }
                else
                {
                    hourval = hourval + ", " + chklscopyto.Items[i].Text.ToString();
                }
            }
        }
        if (commcount > 0)
        {
            //txtcopyto.Text = "Hour (" + commcount.ToString() + ")";
            txtcopyto.Text = hourval;
            if (chklscopyto.Items.Count == commcount)
            {
                chkcopyto.Checked = true;
            }
        }
    }

    protected void btncopy_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvatte.Rows.Count > 0 && gvatte.Visible == true)
            {
                string timageids = "", lblpres = string.Empty;
                string hrval = ddlcopyfrom.Text.ToString();
                if (hrval.Trim() != "")
                {
                    int fromhr = Convert.ToInt32(hrval);
                    fromhr = fromhr + 5;
                    beforesavefn();
                    for (int Att_row = 0; Att_row <= gvatte.Rows.Count - 3; Att_row++)
                    {
                        timageids = "chk" + fromhr;
                        lblpres = "p" + fromhr;
                        string Att_mark = (gvatte.Rows[Att_row].Cells[fromhr].FindControl(lblpres) as Label).Text.ToUpper();
                        for (int i = 0; i < chklscopyto.Items.Count; i++)
                        {
                            if (chklscopyto.Items[i].Selected == true)
                            {
                                int clon = Convert.ToInt32(chklscopyto.Items[i].Value.ToString());
                                clon = clon + 5;
                                string timageids1 = "chk" + clon;
                                string lblpres1 = "p" + clon;
                                if (Att_mark == "P")
                                {
                                    (gvatte.Rows[Att_row].Cells[clon].FindControl(timageids1) as CheckBox).Checked = true;
                                    (gvatte.Rows[Att_row].Cells[clon].FindControl(lblpres1) as Label).Text = "p";
                                    gvatte.Rows[Att_row].Cells[clon].BackColor = Color.Green;
                                }
                                else
                                {
                                    (gvatte.Rows[Att_row].Cells[clon].FindControl(timageids1) as CheckBox).Checked = false;
                                    (gvatte.Rows[Att_row].Cells[clon].FindControl(lblpres1) as Label).Text = "a";
                                    gvatte.Rows[Att_row].Cells[clon].BackColor = Color.Red;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                lblset.Text = "Not Copy Attendace Because Invalid Entry";
                lblset.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    private void SaveDefaultOnDuty()
    {
        try
        {
            string value = string.Empty;
            string coll_code = string.Empty;
            if (isLogin())
            {
                coll_code = Convert.ToString(Session["collegecode"]);
            }
            if (rbincludeonduty.Checked == true)
            {
                value = "1";
            }
            else if (rbexcludeonduty.Checked == true)
            {
                value = "2";
            }
            if (coll_code != "" && value != "")
            {
                qry = "if exists (select * from New_InsSettings where college_code='" + coll_code + "' and LinkName='DefaultOnDuty') update New_InsSettings set LinkValue='" + value + "' where college_code='" + coll_code + "' and LinkName='DefaultOnDuty' else insert into New_InsSettings (LinkName,LinkValue,college_code) values ('DefaultOnDuty','" + value + "','" + coll_code + "')";
                int a = dacces2.update_method_wo_parameter(qry, "Text");
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void ChangeDefaultOnDuty()
    {
        try
        {
            string value = string.Empty;
            string coll_code = string.Empty;
            if (isLogin())
            {
                coll_code = Convert.ToString(Session["collegecode"]);
            }
            //if (rbincludeonduty.Checked == true)
            //{
            //    value = "1";
            //}
            //else if (rbexcludeonduty.Checked == true)
            //{
            //    value = "2";
            //}
            if (coll_code != "")
            {
                qry = "select LinkValue from New_InsSettings where college_code='" + coll_code + "' and LinkName='DefaultOnDuty'";
                value = dacces2.GetFunctionv(qry);
            }
            if (value == "1")
            {
                rbincludeonduty.Checked = true;
                rbexcludeonduty.Checked = false;
            }
            else if (value == "2")
            {
                rbincludeonduty.Checked = false;
                rbexcludeonduty.Checked = true;
            }
            else
            {
                rbincludeonduty.Checked = true;
                rbexcludeonduty.Checked = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public bool isLogin()
    {
        bool islog = false;
        try
        {
            if (Session["collegecode"] == null)
            {
                islog = false;
            }
            else
            {
                islog = true;
            }
            return islog;
        }
        catch (Exception ex)
        {
            return islog;
        }
    }

    //Added by Idhris 29-12-2016
    #region allstudentattendancereport new table

    protected void attendanceMark(string appno, int mnthYear, string attDay, int noMaxHrsDay, int noFstHrsDay, int noSndHrsDay, int noMinFstHrsDay, int noMinSndHrsDay, string DateVal, string collegecode)
    {
        try
        {
            DataSet dsload = new DataSet();
            Dictionary<int, int> AttValueMrng = new Dictionary<int, int>();
            Dictionary<int, int> AttvalueEve = new Dictionary<int, int>();
            double attVal = 0;
            int MPCnt = 0;
            int EPCnt = 0;
            int MnullCnt = 0;
            int EnullCnt = 0;
            string SelQ = " select " + attDay + ",A.ROLL_NO,r.app_no from attendance a,registration r where r.roll_no =a.roll_no  and r.college_code=Att_CollegeCode and  r.college_code='" + collegecode + "' AND month_year='" + mnthYear + "' and Att_App_no='" + appno + "' ";
            //d1d1,d1d2,d1d3,d1d4,d1d5,d1d6,d1d7,d1d8,d1d9,d1d10,d2d1,d2d2,d2d3,d2d4,d2d5,d2d6,d2d7,d2d8,d2d9,d2d10,d3d1,d3d2,d3d3,d3d4,d3d5,d3d6,d3d7,d3d8,d3d9,d3d10,d4d1,d4d2,d4d3,d4d4,d4d5,d4d6,d4d7,d4d8,d4d9,d4d10,d5d1,d5d2,d5d3,d5d4,d5d5,d5d6,d5d7,d5d8,d5d9,d5d10,d6d1,d6d2,d6d3,d6d4,d6d5,d6d6,d6d7,d6d8,d6d9,d6d10,d7d1,d7d2,d7d3,d7d4,d7d5,d7d6,d7d7,d7d8,d7d9,d7d10,d8d1,d8d2,d8d3,d8d4,d8d5,d8d6,d8d7,d8d8,d8d9,d8d10,d9d1,d9d2,d9d3,d9d4,d9d5,d9d6,d9d7,d9d8,d9d9,d9d10,d10d1,d10d2,d10d3,d10d4,d10d5,d10d6,d10d7,d10d8,d10d9,d10d10,d11d1,d11d2,d11d3,d11d4,d11d5,d11d6,d11d7,d11d8,d11d9,d11d10,d12d1,d12d2,d12d3,d12d4,d12d5,d12d6,d12d7,d12d8,d12d9,d12d10,d13d1,d13d2,d13d3,d13d4,d13d5,d13d6,d13d7,d13d8,d13d9,d13d10,d14d1,d14d2,d14d3,d14d4,d14d5,d14d6,d14d7,d14d8,d14d9,d14d10,d15d1,d15d2,d15d3,d15d4,d15d5,d15d6,d15d7,d15d8,d15d9,d15d10,d16d1,d16d2,d16d3,d16d4,d16d5,d16d6,d16d7,d16d8,d16d9,d16d10,d17d1,d17d2,d17d3,d17d4,d17d5,d17d6,d17d7,d17d8,d17d9,d17d10,d18d1,d18d2,d18d3,d18d4,d18d5,d18d6,d18d7,d18d8,d18d9,d18d10,d19d1,d19d2,d19d3,d19d4,d19d5,d19d6,d19d7,d19d8,d19d9,d19d10,d20d1,d20d2,d20d3,d20d4,d20d5,d20d6,d20d7,d20d8,d20d9,d20d10,d21d1,d21d2,d21d3,d21d4,d21d5,d21d6,d21d7,d21d8,d21d9,d21d10,d22d1,d22d2,d22d3,d22d4,d22d5,d22d6,d22d7,d22d8,d22d9,d22d10,d23d1,d23d2,d23d3,d23d4,d23d5,d23d6,d23d7,d23d8,d23d9,d23d10,d24d1,d24d2,d24d3,d24d4,d24d5,d24d6,d24d7,d24d8,d24d9,d24d10,d25d1,d25d2,d25d3,d25d4,d25d5,d25d6,d25d7,d25d8,d25d9,d25d10,d26d1,d26d2,d26d3,d26d4,d26d5,d26d6,d26d7,d26d8,d26d9,d26d10,d27d1,d27d2,d27d3,d27d4,d27d5,d27d6,d27d7,d27d8,d27d9,d27d10,d28d1,d28d2,d28d3,d28d4,d28d5,d28d6,d28d7,d28d8,d28d9,d28d10,d29d1,d29d2,d29d3,d29d4,d29d5,d29d6,d29d7,d29d8,d29d9,d29d10,d30d1,d30d2,d30d3,d30d4,d30d5,d30d6,d30d7,d30d8,d30d9,d30d10,d31d1,d31d2,d31d3,d31d4,d31d5,d31d6,d31d7,d31d8,d31d9,d31d10
            dsload.Clear();
            dsload = dacces2.select_method_wo_parameter(SelQ, "Text");
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            {
                for (int sel = 0; sel < noMaxHrsDay; sel++)
                {
                    if (sel < noFstHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                MPCnt++;
                            else
                            {
                                //  MOCnt = attVal;
                                if (!AttValueMrng.ContainsKey(Convert.ToInt32(attVal)))
                                    AttValueMrng.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttValueMrng[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttValueMrng.Remove(Convert.ToInt32(attVal));
                                    AttValueMrng.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }
                        }
                        else
                            MnullCnt++;
                    }
                    else if (sel >= noSndHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                EPCnt++;
                            else
                            {
                                // EOCnt = attVal;
                                if (!AttvalueEve.ContainsKey(Convert.ToInt32(attVal)))
                                    AttvalueEve.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttvalueEve[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttvalueEve.Remove(Convert.ToInt32(attVal));
                                    AttvalueEve.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }
                        }
                        else
                            EnullCnt++;
                    }
                }
                int matt = attendanceSet(MPCnt, MnullCnt, noMinFstHrsDay, AttValueMrng);
                int eatt = attendanceSet(EPCnt, EnullCnt, noMinSndHrsDay, AttvalueEve);
                if (matt != null && eatt != null)
                {
                    string InsQ = " if exists (select AppNo from AllStudentAttendanceReport where dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "')update AllStudentAttendanceReport set mleavecode='" + matt + "',eleavecode='" + eatt + "' where  dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "' else insert into AllStudentAttendanceReport(AppNo, DateofAttendance,MLeaveCode,ELeaveCode) values('" + dsload.Tables[0].Rows[0]["app_no"] + "','" + DateVal + "','" + matt + "','" + eatt + "')";
                    int save = dacces2.update_method_wo_parameter(InsQ, "Text");
                }
            }
        }
        catch { }
    }

    protected int attendanceSet(int attCnt, int nullCnt, int hrCntCheck, Dictionary<int, int> val)
    {
        int attVal = 0;
        try
        {
            //if (attCnt >= hrCntCheck)
            //    attVal = Convert.ToInt32(leave);
            //else if (nullCnt > 0)
            //    attVal = 0;
            //else
            //    attVal = Convert.ToInt32(leave);
            if (attCnt >= hrCntCheck)
                attVal = 1;
            else if (nullCnt > 0)
                attVal = 0;
            else
            {
                val = val.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                foreach (KeyValuePair<int, int> txt in val)
                {
                    attVal = Convert.ToInt32(txt.Key);
                    break;
                }
            }
        }
        catch { }
        return attVal;
    }

    #endregion

    private string findDayName(byte dayOrder)
    {
        string dayName = string.Empty;
        switch (dayOrder)
        {
            case 0:
                dayName = string.Empty;
                break;
            case 1:
                dayName = "mon";
                break;
            case 2:
                dayName = "tue";
                break;
            case 3:
                dayName = "wed";
                break;
            case 4:
                dayName = "thu";
                break;
            case 5:
                dayName = "fri";
                break;
            case 6:
                dayName = "sat";
                break;
            case 7:
                dayName = "sun";
                break;
            default:
                break;
        }
        return dayName;
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetAllODStudents()
    {
        DataSet dsODStudents = new DataSet();
        Dictionary<string, int[]> dic = new Dictionary<string, int[]>();
        try
        {
            qry = "select * from ondudy_students";
        }
        catch
        {
        }
    }

    /// <summary>
    /// This Is Used to Get All Fee of Students 
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="dicFeeOfRoll">referenced type Dictionary To Hold Fee of Roll Student</param>
    /// <param name="dtFromDate">dd-MM-yyyy</param>
    /// <param name="dtToDate">dd-MM-yyyy</param>
    private void GetFeeOfRollStudent(ref Dictionary<string, DateTime[]> dicFeeOfRollStudents, ref Dictionary<string, byte> dicFeeOnRoll, string fromDate = null, string toDate = null)
    {
        try
        {
            DataSet dsFeeOfRollDate = new DataSet();
            DateTime dtFromDate = new DateTime();
            DateTime dtToDate = new DateTime();
            bool isFromSuccess = false;
            bool isToSuccess = false;
            fromDate = txtFromDate.Text;
            if (!string.IsNullOrEmpty(fromDate))
            {
                isFromSuccess = DateTime.TryParseExact(fromDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                isToSuccess = DateTime.TryParseExact(toDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtToDate);
            }
            string qryFeeOfRollDate = string.Empty;
            if (isFromSuccess && isToSuccess)
            {
                qryFeeOfRollDate = " and curr_date between '" + dtFromDate.ToString("mm/dd/yyyy") + "' and '" + dtToDate.ToString("mm/dd/yyyy") + "'";
            }
            else if (isFromSuccess)
            {
                qryFeeOfRollDate = " and curr_date='" + dtFromDate.ToString("mm/dd/yyyy") + "'";
            }
            else if (isToSuccess)
            {
                qryFeeOfRollDate = " and curr_date='" + dtToDate.ToString("mm/dd/yyyy") + "'";
            }
            else
            {
                qryFeeOfRollDate = string.Empty;
            }
            //qry = "select roll_no, Convert(varchar(50),curr_date,103) as curr_date,infr_type,Convert(varchar(50),CAST(ack_date as DateTime),103) as ack_date,Convert(varchar(50),feeOnRollDate,103) as feeOnRollDate,ack_diss,ack_fine,ack_remarks,ack_susp,ack_warn,tot_days,fine_amo,semester,ack_fee_of_roll,Remark,Convert(varchar(50),suspendFromDate,103) as suspendFromDate,Convert(varchar(50),suspendToDate,103) as suspendToDate from stucon where (ack_fee_of_roll=1 or feeOnRollDate is not null) " + qryFeeOfRollDate;
            qry = "select roll_no,Convert(varchar(50),curr_date,103) as curr_date,Convert(varchar(50),CAST(ack_date as DateTime),103) as ack_date,Convert(varchar(50),feeOnRollDate,103) as feeOnRollDate,semester,ack_fee_of_roll from stucon where (ack_fee_of_roll=1 or feeOnRollDate is not null) and  CAST(ack_date as DateTime) <='" + dtFromDate.ToString("MM/dd/yyyy") + "' order by CAST(ack_date as DateTime) desc";
            dsFeeOfRollDate = dacces2.select_method_wo_parameter(qry, "text");
            if (dsFeeOfRollDate.Tables.Count > 0 && dsFeeOfRollDate.Tables[0].Rows.Count > 0)
            {
                dicFeeOfRollStudents.Clear();
                foreach (DataRow drFeeOfRoll in dsFeeOfRollDate.Tables[0].Rows)
                {
                    string rollNo = Convert.ToString(drFeeOfRoll["roll_no"]).Trim().ToLower();
                    string feeOffRollDate = Convert.ToString(drFeeOfRoll["curr_date"]).Trim();
                    string feeOffRollDate1 = Convert.ToString(drFeeOfRoll["ack_date"]).Trim();
                    string feeOnRollDate = Convert.ToString(drFeeOfRoll["feeOnRollDate"]).Trim();
                    string isFeeOfRoll = Convert.ToString(drFeeOfRoll["ack_fee_of_roll"]).Trim();
                    byte FeeOnRoll = 0;
                    byte.TryParse(isFeeOfRoll.Trim(), out FeeOnRoll);
                    DateTime dtFeeOffRollDate = new DateTime();
                    DateTime dtFeeOnRollDate = new DateTime();
                    bool isFeeOff = DateTime.TryParseExact(feeOffRollDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFeeOffRollDate);
                    bool isFeeOn = DateTime.TryParseExact(feeOnRollDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFeeOnRollDate);
                    DateTime[] dtFeeRoll = new DateTime[2];
                    dtFeeRoll[0] = dtFeeOffRollDate;
                    dtFeeRoll[1] = dtFeeOnRollDate;
                    if (!isFeeOn)
                    {
                        //dtFeeOnRollDate = ;
                    }
                    if (!dicFeeOfRollStudents.ContainsKey(rollNo.Trim()))
                    {
                        dicFeeOfRollStudents.Add(rollNo.Trim(), dtFeeRoll);
                    }
                    if (!dicFeeOnRoll.ContainsKey(rollNo.Trim()))
                    {
                        dicFeeOnRoll.Add(rollNo.Trim(), FeeOnRoll);
                    }
                    //string rollNo = Convert.ToString(drFeeOfRoll[""]).Trim();
                }
            }
        }
        catch
        {
        }
    }

}
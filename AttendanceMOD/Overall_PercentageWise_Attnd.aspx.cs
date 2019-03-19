using System;//--------------------started On 01/08/12, complete on 2/8/12(PRABHA)
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;//.Specialized;
using FarPoint.Web.Spread;
using System.Drawing;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using BalAccess;
using DalConnection;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;


public partial class Overall_PercentageWise_Attnd : System.Web.UI.Page
{


    [Serializable()]
    public class MyImg : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  

            img.Width = Unit.Percentage(90);
            return img;
        }
    }


    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    static Boolean forschoolsetting = false;// Added by sridharan
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    //---------------------dataset
    DataSet ds = new DataSet();
    DataTable dt1 = new DataTable();
    DataSet ds_stud = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 dass = new DAccess2();
    Hashtable has = new Hashtable();
    DateTime per_from_date = new DateTime();
    Hashtable hat = new Hashtable();
    DAccess2 dacces2 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    Hashtable has_degree_heading = new Hashtable();
    DateTime per_to_date = new DateTime();
    DateTime dumm_from_date = new DateTime();
    DataSet ds2 = new DataSet();
    Boolean check_print_row = false;
    static DataSet dsprint = new DataSet();
    string usercode = "", collegecode = "", singleuser = "", group_user = "";
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;
    static string view_footer = "", view_header = "", view_footer_text = "";
    string degree_code = "", curr_sem = "", stud_roll_no = "", stud_sem = "", frdate = "", todate = "", stud_batch_year = "", temp_sem = "";
    int curr_yr = 0, stu_count = 0, rows_count = 0, count = 0;
    int cal_from_date = 0, cal_to_date = 0, mmyycount = 0, moncount = 0;

    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0;
    double per_leave_true = 0;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string value_holi_status = "";
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int minpresII = 0;
    string value, date;
    string tempvalue = "-1";
    int ObtValue = -1;
    double per_perhrs, per_abshrs, cum_perhrs, cum_abshrs;
    double per_ondu, per_leave, per_hhday, cum_ondu, cum_leave, cum_hhday;
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    double per_con_hrs, cum_con_hrs;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double cum_present_date, cum_ondu_date, cum_leave_date, cum_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double cum_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;

    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double halfday = 0;
    double cum_tot_point, per_holidate, cum_per_holidate;
    int per_dum_unmark, cum_dum_unmark, dum_unmark, unmark = 0, left_logo_clm = 0;
    string dum_tage_date, dum_tage_hrs;
    double per_tage_hrs;

    string leftlogo = "", rightlogo = "", leftlength = "", rightlength = "", multi_iso = "", new_header_name = "";
    string coll_name = "", address1 = "", header_alignment = "", address2 = "", phoneno = "", faxno = "", email = "", address3 = "", website = "", form_name = "";
    int temp_count = 0, final_print_col_cnt = 0, col_count = 0, start_column = 0, end_column = 0, sub_col_val = 0;
    string phone = "", fax = "", email_id = "", web_add = "";
    int temp_count_temp = 0;
    string[] new_header_string_split;
    string new_header_string_index = "", column_field = "";

    int cnt_0_30 = 0, cnt_31_40 = 0, cnt_41_50 = 0, cnt_51_60 = 0, cnt_61_70 = 0, cnt_71_74 = 0, cnt_75 = 0, cnt_more_75 = 0, cnt_total = 0;


    string check_value_graduate = "", check_value_degree = "", check_value_branch = "", clmnheadrname = "";
    string[] check_value_graduate_splt, check_value_degree_splt, check_value_branch_splt;

    string group_code = "", columnfield = "";
    //added by Srinath 18/2/2013
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();
    DataSet ds3 = new DataSet();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();

    Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
    Dictionary<string, int> holiday_table2 = new Dictionary<string, int>();
    Dictionary<string, int> holiday_table3 = new Dictionary<string, int>();
    int callfromdatesample = 0;
    string frdatesample = "";
    string todatesample = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        lblerr.Visible = false;
        if (!Page.IsPostBack)
        {
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            btnprintmaster.Visible = false;
            Session["QueryString"] = "";
            group_code = Session["group_code"].ToString();
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
            dsprint = dacces2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                btngo.Enabled = true;
                txtbranch.Enabled = true;
                txtdegree.Enabled = true;
                ddlyear.Enabled = true;
                txtfromdate.Enabled = true;
                txttodate.Enabled = true;
                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                ddlcollege_SelectedIndexChanged(sender, e);
            }
            else
            {
                ddlcollege.Enabled = false;
                btngo.Enabled = false;
                txtbranch.Enabled = false;
                txtdegree.Enabled = false;
                ddlyear.Enabled = false;
                txtfromdate.Enabled = false;
                txttodate.Enabled = false;
            }


            Pageload(sender, e);
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
                    Label4.Text = "School";
                    lblgraduate.Text = "Education Level";
                    lbldegree.Text = "School Type";
                    lblbranch.Text = "Standard";
                    //lblDuration.Text = "Term";
                    //Label1.Text = "Test Mark R11-Continuous Assessment Report";
                    lbldegree.Attributes.Add("Style", "left: 631px;    position: absolute;    top: 180px;");
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

    public void bindyear()
    {

        ddlyear.Items.Clear();
        string get_sem_for_year_str = "";
        double year_val = 0, get_sem_for_year = 0;


        get_sem_for_year_str = GetFunction("select max(duration) from degree where  college_code=" + Session["InternalCollegeCode"].ToString() + "");


        if (get_sem_for_year_str != "" && get_sem_for_year_str != null)
        {
            get_sem_for_year = Convert.ToDouble(get_sem_for_year_str);
            ddlyear.Enabled = true;
            year_val = (Math.Round(get_sem_for_year, 0, MidpointRounding.AwayFromZero)) / 2;
            for (int load_yr = 1; load_yr <= year_val; load_yr++)
            {
                ddlyear.Items.Add(sem_roman(load_yr) + " Year");
                ddlyear.Items[ddlyear.Items.Count - 1].Value = load_yr.ToString();
            }

            Bind_Graduation();
        }
        else
        {
            ddlyear.Enabled = false;
            chkbxlistDegree.Enabled = false;
            chkbxlistbranch.Enabled = false;
            chkbxlist_graduate.Enabled = false;
        }


    }

    public void Bind_Graduation()
    {
        chkbxlist_graduate.Items.Clear();

        ds.Reset();
        ds.Clear();
        string get_graduate = "select distinct edu_level from course where (edu_level<>'' and edu_level<>' ') and college_code=" + Session["InternalCollegeCode"].ToString() + "";
        cmd = new SqlCommand(get_graduate, con);
        con.Close();
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            chkbxlist_graduate.DataTextField = "edu_level";
            chkbxlist_graduate.DataSource = ds;
            chkbxlist_graduate.DataBind();
            chkbxlist_graduate.Enabled = true;

            chkgraduate.Checked = true;
            for (int item = 0; item < chkbxlist_graduate.Items.Count; item++)
            {
                chkbxlist_graduate.Items[item].Selected = true;
            }
            txtgraduate.Text = "Graduation(" + chkbxlist_graduate.Items.Count + ")";
            Bind_Degree();
        }
        else
        {
            chkbxlist_graduate.Enabled = false;
        }

    }

    public void Bind_Degree()
    {

        for (int clear = 0; clear < ds.Tables.Count; clear++)
        {
            ds.Tables[clear].Columns.Clear();
            ds.Tables[clear].Rows.Clear();
        }

        chkbxlistDegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["InternalCollegeCode"].ToString();// +" and edu_level in" + get_items_fm_graduate;
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
        ds.Clear();
        ds.Reset();
        ds = dass.select_method("bind_degree", has, "sp");
        int count1 = ds.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            chkbxlistDegree.Enabled = true;
            chkbxlistDegree.DataSource = ds;
            chkbxlistDegree.DataTextField = "course_name";
            chkbxlistDegree.DataValueField = "course_id";
            chkbxlistDegree.DataBind();

            chkdegree.Checked = true;
            for (int item = 0; item < chkbxlistDegree.Items.Count; item++)
            {
                chkbxlistDegree.Items[item].Selected = true;
            }
            txtdegree.Text = "Degree(" + chkbxlistDegree.Items.Count + ")";

            bindbranch_degree();
        }
        else
        {
            chkbxlistDegree.Enabled = false;
        }
    }

    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        getcon.Close();
        getcon.Open();
        SqlDataReader drnew;
        cmd = new SqlCommand(sqlstr, getcon);
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

    protected void chkgraduate_CheckedChanged(object sender, EventArgs e)
    {
        if (chkgraduate.Checked == true)
        {
            for (int item = 0; item < chkbxlist_graduate.Items.Count; item++)
            {
                chkbxlist_graduate.Items[item].Selected = true;
            }
            txtgraduate.Text = "Graduation(" + chkbxlist_graduate.Items.Count + ")";
            Bind_Degree();
        }
        else
        {
            for (int item = 0; item < chkbxlist_graduate.Items.Count; item++)
            {
                chkbxlist_graduate.Items[item].Selected = false;
            }
            txtgraduate.Text = "Graduation(0)";
        }
    }

    protected void chkbxlist_graduate_SelectedIndexChanged(object sender, EventArgs e)
    {

        bool flag_allcheck = false;
        int get_count = 0;

        for (int item = 0; item < chkbxlist_graduate.Items.Count; item++)
        {
            if (chkbxlist_graduate.Items[item].Selected == false)
            {
                flag_allcheck = true;
            }
            else
            {
                get_count++;
            }
        }

        if (flag_allcheck == true)
        {
            chkgraduate.Checked = false;
        }
        else
        {
            chkgraduate.Checked = true;
        }
        txtgraduate.Text = "Graduation(" + get_count + ")";

        if (get_count > 0)
        {
            Bind_Degree_for_graduate();
            txtbranch.Enabled = true;
            txtdegree.Enabled = true;
        }
        else
        {
            chkbxlistbranch.Items.Clear();
            txtbranch.Enabled = false;
            txtbranch.Text = " ";
            chkbxlistDegree.Items.Clear();
            txtdegree.Enabled = false;
            txtdegree.Text = " ";
        }
    }

    protected void ddlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerr.Visible = false;
        Bind_Degree_for_graduate();
    }

    public void Bind_Degree_for_graduate()
    {
        for (int clear = 0; clear < ds.Tables.Count; clear++)
        {
            ds.Tables[clear].Columns.Clear();
            ds.Tables[clear].Rows.Clear();
        }

        string get_items_fm_graduate = "", str_get_val = "";
        for (int item = 0; item < chkbxlist_graduate.Items.Count; item++)
        {
            if (chkbxlist_graduate.Items[item].Selected == true)
            {
                if (get_items_fm_graduate == "")
                {
                    get_items_fm_graduate = "('" + chkbxlist_graduate.Items[item].Text + "'";
                }
                else
                {
                    get_items_fm_graduate = get_items_fm_graduate + ",'" + chkbxlist_graduate.Items[item].Text + "'";
                }
            }
        }
        if (get_items_fm_graduate.Trim() != "")
        {
            get_items_fm_graduate = get_items_fm_graduate + ")";
        }


        chkbxlistDegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["InternalCollegeCode"].ToString();// +" and edu_level in" + get_items_fm_graduate;
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }

        if (singleuser == "true" || singleuser == "True" || singleuser == "TRUE" || singleuser == "1")
        {
            str_get_val = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + " and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " and edu_level in" + get_items_fm_graduate + "";
        }
        else
        {
            str_get_val = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + " and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + "  and edu_level in" + get_items_fm_graduate + "";
        }
        SqlDataAdapter da = new SqlDataAdapter(str_get_val, con);
        con.Close();
        con.Open();
        da.Fill(ds);

        int count1 = ds.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            chkbxlistDegree.Enabled = true;
            chkbxlistDegree.DataSource = ds;
            chkbxlistDegree.DataTextField = "course_name";
            chkbxlistDegree.DataValueField = "course_id";
            chkbxlistDegree.DataBind();

            for (int item = 0; item < chkbxlistDegree.Items.Count; item++)
            {
                chkbxlistDegree.Items[item].Selected = true;
            }
            txtdegree.Text = "Degree(" + chkbxlistDegree.Items.Count + ")";

            bindbranch_degree();
        }
        else
        {
            chkbxlistDegree.Enabled = false;
        }
    }

    public void bindbranch_degree()
    {

        for (int clear = 0; clear < ds.Tables.Count; clear++)
        {
            ds.Tables[clear].Columns.Clear();
            ds.Tables[clear].Rows.Clear();
        }


        string get_items_fm_graduate = "", str_get_val = "";
        for (int item = 0; item < chkbxlistDegree.Items.Count; item++)
        {
            if (chkbxlistDegree.Items[item].Selected == true)
            {
                if (get_items_fm_graduate == "")
                {
                    get_items_fm_graduate = "('" + chkbxlistDegree.Items[item].Value + "'";
                }
                else
                {
                    get_items_fm_graduate = get_items_fm_graduate + ",'" + chkbxlistDegree.Items[item].Value + "'";
                }
            }
        }
        if (get_items_fm_graduate.Trim() != "")
        {
            get_items_fm_graduate = get_items_fm_graduate + ")";
        }



        str_get_val = "";
        chkbxlistbranch.Items.Clear();
        has.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["InternalCollegeCode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if (singleuser == "true" || singleuser == "True" || singleuser == "TRUE" || singleuser == "1")
        {
            str_get_val = " select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in" + get_items_fm_graduate + " and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " order by  degree.degree_code";
        }
        else
        {
            str_get_val = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in" + get_items_fm_graduate + " and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + " order by  degree.degree_code";
        }
        ds.Clear();
        ds.Reset();
        SqlDataAdapter da = new SqlDataAdapter(str_get_val, con);
        con.Close();
        con.Open();
        da.Fill(ds);

        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            chkbxlistbranch.Enabled = true;
            chkbxlistbranch.DataSource = ds;
            chkbxlistbranch.DataTextField = "dept_name";
            chkbxlistbranch.DataValueField = "degree_code";
            chkbxlistbranch.DataBind();

            chkbranch.Checked = true;
            for (int item = 0; item < chkbxlistbranch.Items.Count; item++)
            {
                chkbxlistbranch.Items[item].Selected = true;
            }
            txtbranch.Text = " Branch(" + chkbxlistbranch.Items.Count + ")";
        }
        else
        {
            chkbxlistbranch.Enabled = false;
        }
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdegree.Checked == true)
        {
            for (int item = 0; item < chkbxlistDegree.Items.Count; item++)
            {
                chkbxlistDegree.Items[item].Selected = true;
            }
            txtdegree.Text = "Degree(" + chkbxlistDegree.Items.Count + ")";
            bindbranch_degree();
        }
        else
        {
            for (int item = 0; item < chkbxlistDegree.Items.Count; item++)
            {
                chkbxlistDegree.Items[item].Selected = false;
            }
            txtdegree.Text = "Degree(0)";
        }
    }

    protected void chkbxlistDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool flag_allcheck = false;
        int get_count = 0;

        for (int item = 0; item < chkbxlistDegree.Items.Count; item++)
        {
            if (chkbxlistDegree.Items[item].Selected == false)
            {
                flag_allcheck = true;
            }
            else
            {
                get_count++;
            }
        }

        if (flag_allcheck == true)
        {
            chkdegree.Checked = false;
        }
        else
        {
            chkdegree.Checked = true;
        }
        txtdegree.Text = "Degree(" + get_count + ")";

        if (get_count > 0)
        {
            txtbranch.Enabled = true;
            bindbranch_degree();
        }
        else
        {
            chkbxlistbranch.Items.Clear();
            txtbranch.Text = " ";
            txtbranch.Enabled = false;
        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbranch.Checked == true)
        {
            for (int item = 0; item < chkbxlistbranch.Items.Count; item++)
            {
                chkbxlistbranch.Items[item].Selected = true;
            }
            txtbranch.Text = "Branch(" + chkbxlistbranch.Items.Count + ")";
            //  bindbranch_degree();
        }
        else
        {
            for (int item = 0; item < chkbxlistbranch.Items.Count; item++)
            {
                chkbxlistbranch.Items[item].Selected = false;
            }
            txtbranch.Text = "Branch(0)";
        }
    }

    protected void chkbxlistbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool flag_allcheck = false;
        int get_count = 0;

        for (int item = 0; item < chkbxlistbranch.Items.Count; item++)
        {
            if (chkbxlistbranch.Items[item].Selected == false)
            {
                flag_allcheck = true;
            }
            else
            {
                get_count++;
            }
        }

        if (flag_allcheck == true)
        {
            chkbranch.Checked = false;
        }
        else
        {
            chkbranch.Checked = true;
        }
        txtbranch.Text = "Branch(" + get_count + ")";

        if (get_count > 0)
        {
            //   bindbranch_degree ();
        }
    }

    public string sem_roman(int sem)
    {

        string sem_roman = "";


        switch (sem)
        {
            case 1:
                sem_roman = "I";
                break;
            case 2:
                sem_roman = "II";
                break;
            case 3:
                sem_roman = "III";
                break;
            case 4:
                sem_roman = "IV";
                break;
            case 5:
                sem_roman = "V";
                break;
            case 6:
                sem_roman = "VI";
                break;
            case 7:
                sem_roman = "VII";
                break;
            case 8:
                sem_roman = "VIII";
                break;
            case 9:
                sem_roman = "IX";
                break;
            case 10:
                sem_roman = "X";
                break;
            default:
                sem_roman = " ";
                break;

        }
        return sem_roman;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        txtexcelname.Text = "";
        lblerr.Visible = false;
        lblerr.Text = "";
        if (ddlyear.Text.Trim() == "")
        {
            lblerr.Visible = true;
            lblerr.Text = "Please Select The Year";
            return;
        }
        if (txtgraduate.Enabled == true && txtdegree.Enabled == true && txtbranch.Enabled == true)
        {
            if ((txtfromdate.Text != string.Empty) && (txttodate.Text != string.Empty))
            {
                string valfromdate = "", frmconcat = "", valtodate = "";
                valfromdate = txtfromdate.Text.ToString();
                string[] split1 = valfromdate.Split(new char[] { '/' });
                frmconcat = split1[1].ToString() + '/' + split1[0].ToString() + '/' + split1[2].ToString();
                DateTime dtfromdate = Convert.ToDateTime(frmconcat.ToString());

                valtodate = txttodate.Text.ToString();
                string[] split2 = valtodate.Split(new char[] { '/' });
                frmconcat = split2[1].ToString() + '/' + split2[0].ToString() + '/' + split2[2].ToString();
                DateTime dttodate = Convert.ToDateTime(frmconcat.ToString());
                TimeSpan ts = dttodate.Subtract(dtfromdate);
                int days = ts.Days;
                if (days >= 0)
                {
                    btngoclick_function();//------------------------load function
                    
                    //if (Sprd_attendance.Sheets[0].RowCount > 0)//
                    if (gview.Rows.Count > 0)
                    {
                        //header_text();//-----------------get values
                        //setheader_print();//-------------header setting

                        pageset_pnl.Visible = false;
                        //Sprd_attendance.Visible = true;//
                        gview.Visible = true;
                        btnprintmaster.Visible = true;
                        btnxl.Visible = true;
                        //Added By Srinath 27/2/2013
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;
                        //lblpages.Visible = true;
                        // ddlpage.Visible = true;
                    }
                    else
                    {
                        lblerr.Visible = true;
                        pageset_pnl.Visible = false;
                        //Sprd_attendance.Visible = false;//
                        gview.Visible = false;
                        btnprintmaster.Visible = false;

                        btnxl.Visible = false;
                        //Added By Srinath 27/2/2013
                        txtexcelname.Visible = false;
                        lblrptname.Visible = false;
                        //lblpages.Visible = false;
                        //ddlpage.Visible = false;
                        lblerr.Text = "No Record(s) Availabla";

                    }
                }
                else
                {
                    lblerr.Visible = true;
                    pageset_pnl.Visible = false;
                    //Sprd_attendance.Visible = false;//
                    gview.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                    //Added By Srinath 27/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    //lblpages.Visible = false;
                    //ddlpage.Visible = false;
                    lblerr.Text = "Fromdate Should Be Less Than Todate";
                }
            }
        }
    }

    public void btngoclick_function()
    {
            DataRow dtrow = null;
            string test = "";            
            

            // added By Srinath 20/2/2013 ==Start
            int demfcal, demtcal;
            string monthcal;
            frdate = txtfromdate.Text;
            todate = txttodate.Text;
            string dt = frdate;
            string[] dsplit = dt.Split(new Char[] { '/' });
            frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            demfcal = int.Parse(dsplit[2].ToString());
            demfcal = demfcal * 12;
            cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
            monthcal = cal_from_date.ToString();
            dt = todate;
            dsplit = dt.Split(new Char[] { '/' });
            todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            demtcal = int.Parse(dsplit[2].ToString());
            demtcal = demtcal * 12;
            cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
            string tempchecksphr = "";
            callfromdatesample = cal_from_date;
            frdatesample = frdate;
            todatesample = todate;
            has.Clear();
            has.Add("colege_code", Session["InternalCollegeCode"].ToString());
            ds1 = dass.select_method("ATT_MASTER_SETTING", has, "sp");
            count = ds1.Tables[0].Rows.Count;
            //End 
            //-------------------------------------------------------get semester
            ////int row_cnt = 0;
            string temp_degree_code = "";

            ds_stud.Clear();

            curr_yr = Convert.ToInt32(ddlyear.SelectedValue.ToString());
            curr_sem = "(" + ((curr_yr * 2) - 1) + "," + (curr_yr * 2) + ")";            

            dt1.Columns.Add("Dept");
            dt1.Columns.Add("0-30");
            dt1.Columns.Add("31-40");
            dt1.Columns.Add("41-50");
            dt1.Columns.Add("51-60");
            dt1.Columns.Add("61-70");
            dt1.Columns.Add("71-74");
            dt1.Columns.Add("75");
            dt1.Columns.Add("Above 75");
            dt1.Columns.Add("Total");

            dtrow = dt1.NewRow();
            dtrow["Dept"] = "Dept";
            dtrow["0-30"] = "0-30";
            dtrow["31-40"] = "31-40";
            dtrow["41-50"] = "41-50";
            dtrow["51-60"] = "51-60";
            dtrow["61-70"] = "61-70";
            dtrow["71-74"] = "71-74";
            dtrow["75"] = "75";
            dtrow["Above 75"] = "Above 75";
            dtrow["Total"] = "Total";
            dt1.Rows.Add(dtrow);
            

            for (int degree_count = 0; degree_count < chkbxlistbranch.Items.Count; degree_count++)
            {
                if (chkbxlistbranch.Items[degree_count].Selected == true)
                {
                    degree_code = chkbxlistbranch.Items[degree_count].Value;

                    if (temp_degree_code != GetFunction("select course_id from degree where  degree_code=" + degree_code + ""))
                    {
                        temp_degree_code = GetFunction("select course_id from degree where  degree_code=" + degree_code + "");

                        //=============================Hided by Manikandan 28/05/2013
                        
                        
                        dtrow = dt1.NewRow();
                        dtrow["Dept"] = ddlyear.SelectedItem.ToString() + "(" + GetFunction("select distinct(course_name) from degree,course where degree_code=" + degree_code + " and course.course_id=degree.course_id ") + ")";
                        dt1.Rows.Add(dtrow);
                        //------------------load in has
                        
                        if (!has_degree_heading.ContainsKey(dt1.Rows.Count - 1))
                        {
                            has_degree_heading.Add((dt1.Rows.Count - 1), ddlyear.SelectedItem.ToString() + "(" + GetFunction("select distinct(course_name) from degree,course where degree_code=" + degree_code + " and course.course_id=degree.course_id ") + ")");
                        }                       
                        
                        
                    }
                    //-----------------------------getstudents

                    string get_student = " select distinct roll_no,current_semester,batch_year from registration where cc=0 and exam_flag<>'debar' and delflag=0  and degree_code=" + degree_code + " and current_semester in" + curr_sem + "";
                    cmd = new SqlCommand(get_student, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    con.Close();
                    con.Open();
                    da.Fill(ds_stud);
                    stu_count = ds_stud.Tables[0].Rows.Count;
                    //-----------------------------find count

                    //Hided By Srinath 20/2/2013
                    //has.Clear();
                    //has.Add("colege_code", Session["InternalCollegeCode"].ToString());
                    //ds1 = dass.select_method("ATT_MASTER_SETTING", has, "sp");
                    //count = ds1.Tables[0].Rows.Count;

                    for (rows_count = 0; rows_count < stu_count; rows_count++)
                    {
                        stud_roll_no = ds_stud.Tables[0].Rows[rows_count]["roll_no"].ToString();
                        stud_sem = ds_stud.Tables[0].Rows[rows_count]["current_semester"].ToString();
                        stud_batch_year = ds_stud.Tables[0].Rows[rows_count]["batch_year"].ToString();

                        string checkspecialhr = ds_stud.Tables[0].Rows[rows_count]["batch_year"].ToString() + '/' + degree_code + '/' + ds_stud.Tables[0].Rows[rows_count]["current_semester"].ToString();
                        if (tempchecksphr != checkspecialhr)
                        {
                            //added By srinath 18/2/2013 ==start
                            string[] fromdatespit = txtfromdate.Text.Split('/');
                            string[] todatespit = txttodate.Text.Split('/');
                            DateTime spfromdate = Convert.ToDateTime(fromdatespit[1] + '/' + fromdatespit[0] + '/' + fromdatespit[2]);
                            DateTime sptodate = Convert.ToDateTime(todatespit[1] + '/' + todatespit[0] + '/' + todatespit[2]);
                            ht_sphr.Clear();
                            string hrdetno = "";
                            string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + degree_code + " and batch_year=" + ds_stud.Tables[0].Rows[rows_count]["batch_year"].ToString() + " and semester=" + ds_stud.Tables[0].Rows[rows_count]["current_semester"].ToString() + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "'";
                            ds_sphr = d2.select_method(getsphr, hat, "Text");
                            if (ds_sphr.Tables[0].Rows.Count > 0)
                            {
                                for (int sphr = 0; sphr < ds_sphr.Tables[0].Rows.Count; sphr++)
                                {
                                    if (ht_sphr.Contains(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])))
                                    {
                                        hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), ht_sphr));
                                        hrdetno = hrdetno + "," + Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]);
                                        ht_sphr[Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])] = hrdetno;
                                    }
                                    else
                                    {
                                        ht_sphr.Add(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]));
                                    }
                                }
                            }
                            frdate = frdatesample;
                            todate = todatesample;
                            string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
                            holiday_table1.Clear();
                            holiday_table2.Clear();
                            holiday_table3.Clear();

                            ds3.Dispose();
                            ds3.Clear();
                            has.Clear();
                            has.Add("degree_code", degree_code);
                            has.Add("sem", stud_sem);
                            has.Add("from_date", frdate.ToString());
                            has.Add("to_date", todate.ToString());
                            has.Add("coll_code", int.Parse(Session["InternalCollegeCode"].ToString()));


                            //------------------------------------------------------------------
                            int iscount = 0;
                            holidaycon.Close();
                            holidaycon.Open();
                            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + degree_code + " and semester=" + stud_sem + "";
                            SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
                            SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
                            DataSet dsholiday = new DataSet();
                            daholiday.Fill(dsholiday);
                            if (dsholiday.Tables[0].Rows.Count > 0)
                            {
                                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                            }
                            has.Add("iscount", iscount);

                            ds3 = dass.select_method("ALL_HOLIDATE_DETAILS", has, "sp");

                            holiday_table11.Clear();
                            holiday_table21.Clear();
                            holiday_table31.Clear();
                            if (ds3.Tables[0].Rows.Count != 0)
                            {
                                for (int k = 0; k < ds3.Tables[0].Rows.Count; k++)
                                {
                                    if (ds3.Tables[0].Rows[0]["halforfull"].ToString() == "False")
                                    {
                                        halforfull = "0";
                                    }
                                    else
                                    {
                                        halforfull = "1";
                                    }
                                    if (ds3.Tables[0].Rows[0]["morning"].ToString() == "False")
                                    {
                                        mng = "0";
                                    }
                                    else
                                    {
                                        mng = "1";
                                    }
                                    if (ds3.Tables[0].Rows[0]["evening"].ToString() == "False")
                                    {
                                        evng = "0";
                                    }
                                    else
                                    {
                                        evng = "1";
                                    }

                                    holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                                    string[] split_date_time1 = ds3.Tables[0].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                                    string[] dummy_split = split_date_time1[0].Split('/');
                                    if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                                    {
                                        holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                                    }
                                    // holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
                                }
                            }

                            if (ds3.Tables[1].Rows.Count != 0)
                            {
                                for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                                {
                                    string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                                    string[] dummy_split = split_date_time1[0].Split('/');
                                    if (!holiday_table21.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                                    {
                                        holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                                    }
                                    if (ds3.Tables[1].Rows[k]["halforfull"].ToString() == "False")
                                    {
                                        halforfull = "0";
                                    }
                                    else
                                    {
                                        halforfull = "1";
                                    }
                                    if (ds3.Tables[1].Rows[k]["morning"].ToString() == "False")
                                    {
                                        mng = "0";
                                    }
                                    else
                                    {
                                        mng = "1";
                                    }
                                    if (ds3.Tables[1].Rows[k]["evening"].ToString() == "False")
                                    {
                                        evng = "0";
                                    }
                                    else
                                    {
                                        evng = "1";
                                    }

                                    holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                                    if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                                    {
                                        holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                                    }
                                    if (!holiday_table2.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                                    {
                                        holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                                    }
                                }
                            }

                            if (ds3.Tables[2].Rows.Count != 0)
                            {
                                for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                                {
                                    string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                                    string[] dummy_split = split_date_time1[0].Split('/');
                                    if (!holiday_table31.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                                    {
                                        holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                                    }
                                    if (ds3.Tables[2].Rows[k]["halforfull"].ToString() == "False")
                                    {
                                        halforfull = "0";
                                    }
                                    else
                                    {
                                        halforfull = "1";
                                    }
                                    if (ds3.Tables[2].Rows[k]["morning"].ToString() == "False")
                                    {
                                        mng = "0";
                                    }
                                    else
                                    {
                                        mng = "1";
                                    }
                                    if (ds3.Tables[2].Rows[k]["evening"].ToString() == "False")
                                    {
                                        evng = "0";
                                    }
                                    else
                                    {
                                        evng = "1";
                                    }

                                    holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                                    if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                                    {
                                        holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                                    }
                                    if (!holiday_table3.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                                    {
                                        holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                                    }
                                }
                            }



                            // End 
                            tempchecksphr = ds_stud.Tables[0].Rows[rows_count]["batch_year"].ToString() + '/' + degree_code + '/' + ds_stud.Tables[0].Rows[rows_count]["current_semester"].ToString();
                        }

                        if (temp_sem != stud_sem)
                        {
                            has.Clear();
                            has.Add("degree_code", degree_code);
                            has.Add("sem_ester", stud_sem);
                            ds = dass.select_method("period_attnd_schedule", has, "sp");
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                            }
                            temp_sem = stud_sem;
                        }

                        //---------------------start finding   
                        //diden by Srinath 20/2/2013 ==Start
                        // frdate = txtfromdate.Text;
                        //todate = txttodate.Text;
                        //==End

                        per_abshrs_spl = 0;
                        tot_per_hrs_spl = 0;
                        tot_ondu_spl = 0;
                        per_hhday_spl = 0;
                        unmark_spl = 0;
                        tot_conduct_hr_spl = 0;
                        per_workingdays1 = 0;
                        cum_per_workingdays1 = 0;
                        persentmonthcal();//-----------------------function

                        per_con_hrs = (per_workingdays1 - cum_dum_unmark) + tot_conduct_hr_spl_fals;
                        per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100);

                        if (per_tage_hrs > 100)
                        {
                            per_tage_hrs = 100;
                        }
                        else if (per_tage_hrs.ToString() == "NaN")
                        {
                            per_tage_hrs = 0;
                        }
                        else if (per_tage_hrs.ToString() == "Infinity")
                        {
                            per_tage_hrs = 0;
                        }
                        else
                        {
                            per_tage_hrs = Math.Round(per_tage_hrs, 2, MidpointRounding.AwayFromZero);
                        }

                        if (per_tage_hrs >= 0 && per_tage_hrs <= 30)
                        {
                            cnt_0_30++;
                        }
                        else if (per_tage_hrs > 30 && per_tage_hrs <= 40)
                        {
                            cnt_31_40++;
                        }
                        else if (per_tage_hrs > 40 && per_tage_hrs <= 50)
                        {
                            cnt_41_50++;
                        }
                        else if (per_tage_hrs > 50 && per_tage_hrs <= 60)
                        {
                            cnt_51_60++;
                        }
                        else if (per_tage_hrs > 60 && per_tage_hrs <= 70)
                        {
                            cnt_61_70++;
                        }
                        else if (per_tage_hrs > 70 && per_tage_hrs <= 75)
                        {
                            cnt_71_74++;
                        }
                        else if (per_tage_hrs == 75)
                        {
                            cnt_75++;
                        }
                        else if (per_tage_hrs > 75)
                        {
                            cnt_more_75++;
                        }
                        //-----------------------------
                    }

                    //-------------------row increment

                    
                    
                    dtrow = dt1.NewRow();

                    dtrow["Dept"] = GetFunction("select distinct(acronym) from degree where degree_code=" + chkbxlistbranch.Items[degree_count].Value + "");
                    dtrow["0-30"] = cnt_0_30.ToString();
                    dtrow["31-40"] = cnt_31_40.ToString();
                    dtrow["41-50"] = cnt_41_50.ToString();
                    dtrow["51-60"] = cnt_51_60.ToString();
                    dtrow["61-70"] = cnt_61_70.ToString();
                    dtrow["71-74"] = cnt_71_74.ToString();
                    dtrow["75"] = cnt_75.ToString();
                    dtrow["Above 75"] = cnt_more_75.ToString();
                    dtrow["Total"] = Convert.ToString((Convert.ToInt32(cnt_0_30)) + (Convert.ToInt32(cnt_31_40)) + (Convert.ToInt32(cnt_41_50)) + (Convert.ToInt32(cnt_51_60)) + (Convert.ToInt32(cnt_61_70)) + (Convert.ToInt32(cnt_71_74)) + (Convert.ToInt32(cnt_75)) + (Convert.ToInt32(cnt_more_75)));
                    
                    dt1.Rows.Add(dtrow);

                    cnt_0_30 = 0;
                    cnt_31_40 = 0;
                    cnt_41_50 = 0;
                    cnt_51_60 = 0;
                    cnt_61_70 = 0;
                    cnt_71_74 = 0;
                    cnt_75 = 0;
                    cnt_more_75 = 0;
                    stu_count = 0;

                    ds_stud.Tables[0].Rows.Clear();
                    ds_stud.Tables[0].Columns.Clear();
                }
            }
            gview.DataSource = dt1;
            gview.DataBind();
            gview.Visible = true;
            RowHead(gview, 1);
            MergeColumns(gview);

            
            if (gview.Rows.Count > 0)
            {
                ////Sprd_attendance.Visible = true;
                gview.Visible = true;
                btnprintmaster.Visible = true;
                pageset_pnl.Visible = false;
                ////Sprd_attendance.Sheets[0].PageSize = Sprd_attendance.Sheets[0].RowCount;
                gview.PageSize = gview.Rows.Count;
                //--------------------------------------load in page setup ddl
                Double totalRows = 0;
                ////totalRows = Convert.ToInt32(Sprd_attendance.Sheets[0].RowCount);
                totalRows = Convert.ToInt32(gview.Rows.Count);
                DropDownListpage.Items.Clear();
                if (totalRows >= 10)
                {
                    ////Sprd_attendance.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    DropDownListpage.SelectedIndex = DropDownListpage.Items.Count - 2;
                    ////Sprd_attendance.Height = 350;

                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    ////Sprd_attendance.Height = 200;
                }
                else
                {
                    ////Sprd_attendance.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    ////DropDownListpage.Items.Add(Sprd_attendance.Sheets[0].PageSize.ToString());
                    ////Sprd_attendance.Height = 200 + (10 * Convert.ToInt32(totalRows));
                    DropDownListpage.Items.Add(gview.PageSize.ToString());
                }
                ////totalRows = Convert.ToInt32(Sprd_attendance.Sheets[0].RowCount);
                ////Session["totalPages"] = (int)Math.Ceiling(totalRows / Sprd_attendance.Sheets[0].PageSize);
                totalRows = Convert.ToInt32(gview.Rows.Count);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);
                Buttontotal.Text = "Records : " + totalRows + "          Pages : " + 1;
            }
        
    }

    protected void RowHead(GridView gview, int count)
    {
        for (int head = 0; head < count; head++)
        {
            gview.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }

    protected void MergeColumns(GridView gridview)
    {
        string dep = gridview.HeaderRow.Cells[0].Text;

        for (int i = 1; i < gridview.Rows.Count; i++)
        {
            string sf = gridview.Rows[i].Cells[0].Text;
            if (sf.Contains("Year"))
            {
                int cont = 1;
                for (int j = 1; j < gridview.Rows[i].Cells.Count; j++)
                {
                    gridview.Rows[i].Cells[cont].Visible = false;
                    cont++;
                }
                gridview.Rows[i].Cells[0].ColumnSpan = cont;
                gridview.Rows[i].Cells[0].Font.Bold = true;
                gridview.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            for (int cell = 1; cell < gridview.Rows[i].Cells.Count;cell++ )
            {
                gridview.Rows[i].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    void CalculateTotalPages()
    {
        Double totalRows = 0;
        ////totalRows = Convert.ToInt32(Sprd_attendance.Sheets[0].RowCount);
        ////Session["totalPages"] = (int)Math.Ceiling(totalRows / Sprd_attendance.Sheets[0].PageSize);
        totalRows = Convert.ToInt32(gview.Rows.Count);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];


        Buttontotal.Visible = true;
    }

    //===========Hided by Manikandan 15/05/2013

    //public void setheader_print()
    //{


    //    //  try
    //    {
    //        final_print_col_cnt = 0;
    //        for (col_count = 0; col_count < Sprd_attendance.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (Sprd_attendance.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                final_print_col_cnt++;
    //            }
    //        }

    //        
    //        //Sprd_attendance.Sheets[0].ColumnHeader.RowCount = 0;
    //        //Sprd_attendance.Sheets[0].ColumnHeader.RowCount = 6;



    //        temp_count =0;


    //        //MyImg mi = new MyImg();
    //        //mi.ImageUrl = "~/images/10BIT001.jpeg";
    //        //mi.ImageUrl = "Handler/Handler2.ashx?";
    //        //MyImg mi2 = new MyImg();
    //        //mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //        //mi2.ImageUrl = "Handler/Handler5.ashx?";

    //        //=================

    //        if (final_print_col_cnt == 1)
    //        {
    //            for (col_count = 0; col_count < Sprd_attendance.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (Sprd_attendance.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    more_column();
    //                    break;
    //                }
    //            }

    //        }

    //        else if (final_print_col_cnt == 2)
    //        {
    //            for (col_count = 0; col_count < Sprd_attendance.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (Sprd_attendance.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 1)
    //                    {
    //                        start_column = col_count;
    //                        //   Sprd_attendance.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                        Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (6), 1);
    //                        if (leftlogo == "1" && leftlength != "")
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        }
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else
    //                    {
    //                        //  one_column();
    //                        more_column();
    //                        for (int row_cnt = 0; row_cnt < 7; row_cnt++)
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        }
    //                    }
    //                    temp_count++;
    //                    if (temp_count == 3)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        else if (final_print_col_cnt == 3)
    //        {
    //            for (col_count = 0; col_count < Sprd_attendance.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (Sprd_attendance.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 1)
    //                    {
    //                        start_column = col_count;
    //                        //   Sprd_attendance.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                        Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (6), 1);
    //                        if (leftlogo == "1" && leftlength != "")
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        }
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else if (temp_count == 2)
    //                    {
    //                        // one_column();
    //                        more_column();
    //                        for (int row_cnt = 0; row_cnt < 7; row_cnt++)
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                        }
    //                    }
    //                    else if (temp_count == 3)
    //                    {

    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (6), 2);
    //                            if (rightlogo == "1" && rightlength != "")
    //                            {
    //                                Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                            }
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                            // Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.Black;
    //                        }
    //                    }
    //                    temp_count++;
    //                    if (temp_count == 4)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }

    //        }
    //        else//-----------column count more than 3
    //        {
    //            for (col_count = 0; col_count < Sprd_attendance.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (Sprd_attendance.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 1)
    //                    {
    //                        start_column = col_count;

    //                    }
    //                    if (temp_count == 0)
    //                    {
    //                        left_logo_clm = col_count;
    //                        Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (6), 1);
    //                        if (leftlogo == "1" && leftlength != "")
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        }
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;

    //                    }

    //                    end_column = col_count;

    //                    temp_count++;
    //                    if (final_print_col_cnt == temp_count - 1)
    //                    {
    //                        break;
    //                    }

    //                }
    //            }


    //            {
    //                Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (6), 2);
    //                if (rightlogo == "1" && rightlength != "")
    //                {
    //                    Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //                }
    //                Sprd_attendance.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //            }

    //            temp_count = 1;
    //            for (col_count = 0; col_count < Sprd_attendance.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (Sprd_attendance.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 2)
    //                    {
    //                        col_count = sub_col_val;
    //                        more_column();
    //                        for (int row_cnt = 0; row_cnt < 6; row_cnt++)
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_cnt, start_column ].Border.BorderColorLeft = Color.White;
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_cnt, start_column].Border.BorderColorRight = Color.White;
    //                        }
    //                    }
    //                    temp_count++;
    //                }
    //            }
    //        }
    //    }
    //    // catch
    //    {
    //    }
    //}
    //=======================

    //==============Hided by Manikandan 15/05/2013
    //public void more_column()
    //{


    //    //  try
    //    {

    //           int row_val = 0;


    //        if (multi_iso.Trim() == "")
    //        {
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(0, start_column , 1, (end_column - col_count-1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(1, start_column, 1, (end_column - col_count-1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(2, start_column, 1, (end_column - col_count-1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(3, start_column, 1, (end_column - col_count-1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(4, start_column, 1, (end_column - col_count-1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(5, start_column, 1, (end_column - col_count-1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, (end_column - col_count-1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(7, start_column, 1, (end_column - col_count-1));
    //        }
    //        else
    //        {
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(0, start_column, 1, (end_column - start_column - 1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(1, start_column, 1, (end_column - start_column - 1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(2, start_column, 1, (end_column - start_column - 1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(3, start_column, 1, (end_column - start_column - 1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(4, start_column, 1, (end_column - start_column - 1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(5, start_column, 1, (end_column - start_column - 1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, (end_column - start_column - 1));
    //            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(7, start_column, 1, (end_column - start_column - 1));
    //        }


    //        if (coll_name.Trim() != "")
    //        {
    //            Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, start_column].Text = coll_name;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[0].Tag = 1;
    //            row_val++;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[0].Visible = true;
    //        }
    //        else
    //        {
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[0].Tag = 0;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[0].Visible = false;
    //        }
    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[0, start_column].Border.BorderColorBottom = Color.White;



    //        if (address1.Trim() != "" && address2.Trim() != "" && address3.Trim() != "")
    //        {
    //            Sprd_attendance.Sheets[0].ColumnHeader.Cells[1, start_column].Text = address1 + "-" + address2 + "-" + address3;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[1].Tag = 1;
    //              row_val++;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[1].Visible = true;
    //        }
    //        else
    //        {
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[1].Tag = 0;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[1].Visible = false;
    //        }

    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[1, start_column].Border.BorderColorTop = Color.White;
    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[1, start_column].Border.BorderColorBottom = Color.White;

    //        if (phoneno != "" && phoneno != null)
    //        {
    //            phone = "Phone:" + phoneno;
    //        }
    //        else
    //        {
    //            phone = "";
    //        }

    //        if (faxno != "" && faxno != null)
    //        {
    //            fax = "  Fax:" + faxno;
    //        }
    //        else
    //        {
    //            fax = "";
    //        }


    //        if (phone.Trim() == "" && fax.Trim() == "")
    //        {
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[2].Tag = 0;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[2].Visible = false;
    //        }
    //        else
    //        {

    //            Sprd_attendance.Sheets[0].ColumnHeader.Cells[2, start_column].Text = phone + fax;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[2].Tag = 1;
    //              row_val++;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[2].Visible = true;
    //        }
    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[2, start_column].Border.BorderColorTop = Color.White;
    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[2, start_column].Border.BorderColorBottom = Color.White;

    //        if (email != "" && faxno != null)
    //        {
    //            email_id = "Email:" + email;
    //        }
    //        else
    //        {
    //            email_id = "";
    //        }


    //        if (website != "" && website != null)
    //        {
    //            web_add = "  Web Site:" + website;
    //        }
    //        else
    //        {
    //            web_add = "";
    //        }


    //        if (email_id.Trim() == "" && web_add.Trim() == "")
    //        {

    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[3].Tag = 0;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[3].Visible = false;
    //        }

    //        else
    //        {
    //            Sprd_attendance.Sheets[0].ColumnHeader.Cells[3, start_column].Text = email_id + web_add;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[3].Tag = 1;
    //              row_val++;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[3].Visible = true;
    //        }

    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[3, start_column].Border.BorderColorTop = Color.White;
    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[3, start_column].Border.BorderColorBottom = Color.White;

    //        if (form_name != "" && form_name != null)
    //        {
    //            Sprd_attendance.Sheets[0].ColumnHeader.Cells[4, start_column].Text = "Overall Percentagewise Attendance Report";

    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[4].Tag = 1;
    //              row_val++;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[4].Visible = true;
    //        }
    //        else
    //        {
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[4].Tag = 0;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Rows[4].Visible = false;
    //        }

    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[4, start_column].Border.BorderColorTop = Color.White;
    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[4, start_column].Border.BorderColorBottom = Color.White;

    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[5, start_column].Text = "From: " + txtfromdate.Text + "      To: " + txttodate.Text + "       Date: " + DateTime.Now.ToString("dd/MM/yyyy");

    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    //        Sprd_attendance.Sheets[0].ColumnHeader.Rows[5].Tag = 1;
    //          row_val++;
    //        Sprd_attendance.Sheets[0].ColumnHeader.Rows[5].Visible = true;
    //        //------------------------------multi iso set
    //        int col_val_iso = 0, iso_start_col = 0, iso_upper_bound = 0;

    //        if (multi_iso.Trim() != "")
    //        {
    //            for (col_val_iso = (Sprd_attendance.Sheets[0].ColumnCount - 1); col_val_iso >= 0; col_val_iso--)
    //            {
    //                if (Sprd_attendance.Sheets[0].Columns[col_val_iso].Visible == true)
    //                {
    //                    iso_start_col++;
    //                    if (iso_start_col == 2)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }


    //            //--------------------------------ISO Set
    //            row_val = 0;
    //            if (multi_iso.Trim() != "")
    //            {
    //                string[] multi_iso_spt = multi_iso.Split(',');

    //                for (int iso = 0; iso <= multi_iso_spt.GetUpperBound(0); iso++)
    //                {
    //                    if (row_val > 5)
    //                    {
    //                        Sprd_attendance.Sheets[0].ColumnHeader.RowCount++;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Rows[Sprd_attendance.Sheets[0].ColumnHeader.RowCount-1].Tag = 1;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1),left_logo_clm ].Text = multi_iso_spt[iso];
    //                        Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add((Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), left_logo_clm, 1, final_print_col_cnt - 1);
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), left_logo_clm].HorizontalAlign = HorizontalAlign.Right;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), left_logo_clm].Border.BorderColorRight = Color.White;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 2), left_logo_clm].Border.BorderColorBottom = Color.White;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), left_logo_clm].Border.BorderColorTop = Color.White;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 2), start_column].Border.BorderColorBottom = Color.White;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), start_column].Border.BorderColorTop = Color.White;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), left_logo_clm].Border.BorderColorBottom = Color.White;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Rows[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1)].Tag = "1";

    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 2), end_column ].Border.BorderColorTop = Color.White;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), end_column].Border.BorderColorTop = Color.White;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), end_column].Border.BorderColorLeft  = Color.White;
    //                        //if (rightlogo == "")
    //                        //{
    //                        //    Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), (Sprd_attendance.Sheets[0].ColumnCount - 1)].Text = multi_iso_spt[iso];
    //                        //    Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), (Sprd_attendance.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
    //                        //    Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), (Sprd_attendance.Sheets[0].ColumnCount - 1)].Border.BorderColorTop = Color.White;
    //                        //    Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), (Sprd_attendance.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
    //                        //}
    //                    }
    //                    if (Sprd_attendance.Sheets[0].ColumnHeader.Rows[row_val].Tag.ToString() == "1")
    //                    {
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].Text = multi_iso_spt[iso];
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].HorizontalAlign = HorizontalAlign.Right;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].Border.BorderColorLeft = Color.White;
    //                        if (iso != 0)
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].Border.BorderColorTop = Color.White;
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].Border.BorderColorBottom = Color.White;

    //                        if (rightlogo == "1")
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(row_val, col_val_iso, 1, 1);
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].Border.BorderColorRight = Color.White;
    //                        }
    //                        else
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(row_val, col_val_iso, 1, 2);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        Sprd_attendance.Sheets[0].ColumnHeader.Rows[row_val].Visible = false;
    //                        iso--;
    //                    }


    //                    row_val++;


    //                }

    //                for (int yy = multi_iso_spt.GetUpperBound(0) + 1; yy <= 5; yy++)
    //                {
    //                    if (Sprd_attendance.Sheets[0].ColumnHeader.Cells[yy, col_val_iso].Text == "")
    //                    {
    //                        if (rightlogo == "1")
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(yy, col_val_iso, 1, 2);
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[yy, col_val_iso].Border.BorderColorRight = Color.White;
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[yy, col_val_iso].Border.BorderColorTop = Color.White;
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[yy, col_val_iso].Border.BorderColorBottom = Color.White;
    //                        }
    //                        else
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(yy, col_val_iso, 1, 3);
    //                        }
    //                    }

    //                }

    //            }
    //        }
    //        //-------------------------------------------

    //        temp_count_temp = 0;
    //        int row_cnt_after_iso = 0;
    //        row_cnt_after_iso = Sprd_attendance.Sheets[0].ColumnHeader.RowCount;
    //        if (new_header_name != null && new_header_name != "")
    //        {
    //            new_header_string_split = new_header_name.Split(',');

    //            Sprd_attendance.Sheets[0].ColumnHeader.RowCount = Sprd_attendance.Sheets[0].ColumnHeader.RowCount + new_header_string_split.GetUpperBound(0) + 1;

    //            string[] new_header_string_index_split = new_header_string_index.Split(',');
    //            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_cnt_after_iso - 1, start_column].Border.BorderColorBottom = Color.White;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_cnt_after_iso - 1, left_logo_clm ].Border.BorderColorBottom = Color.White;
    //            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_cnt_after_iso - 1, end_column].Border.BorderColorBottom = Color.White;

    //            for (int row_head_count = row_cnt_after_iso; row_head_count < (row_cnt_after_iso + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //            {
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Text = new_header_string_split[temp_count_temp].ToString();
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, left_logo_clm].Text = " ";
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, end_column].Text = " ";

    //                Sprd_attendance.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, start_column, 1, (end_column - start_column));

    //                Sprd_attendance.Sheets[0].ColumnHeader.Rows[row_head_count].Tag = 1;

    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, end_column].Border.BorderColorTop = Color.White;
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorLeft = Color.White;
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorRight = Color.White;
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorRight = Color.White;
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, end_column].Border.BorderColorLeft = Color.White;
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;
    //                Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, end_column].Border.BorderColorTop = Color.White;




    //                if (row_head_count != (row_cnt_after_iso + new_header_string_split.GetUpperBound(0)))
    //                {
    //                    Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorBottom = Color.White;
    //                    Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorBottom = Color.White;
    //                    Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, end_column].Border.BorderColorBottom = Color.White;
    //                    Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, left_logo_clm].Border.BorderColorBottom = Color.White;
    //                }

    //                if (temp_count_temp <= new_header_string_index_split.GetUpperBound(0))
    //                {
    //                    header_alignment = new_header_string_index_split[temp_count_temp].ToString();

    //                    Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count-1, left_logo_clm].Border.BorderColorBottom = Color.White;
    //                    Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, left_logo_clm ].Border.BorderColorTop = Color.White;                       
    //                    Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, left_logo_clm].Border.BorderColorRight = Color.White;

    //                    Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count - 1, end_column ].Border.BorderColorBottom = Color.White;
    //                    Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, end_column].Border.BorderColorTop = Color.White;
    //                    Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, end_column].Border.BorderColorRight = Color.White;

    //                    if (header_alignment != string.Empty)
    //                    {
    //                        if (header_alignment == "2")
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Center;
    //                        }
    //                        else if (header_alignment == "1")
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Left;
    //                        }
    //                        else
    //                        {
    //                            Sprd_attendance.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Right;
    //                        }
    //                    }
    //                }
    //                temp_count_temp++;
    //            }
    //        }

    //        //------------------header
    //        Sprd_attendance.Sheets[0].ColumnHeader.Cells[(Sprd_attendance.Sheets[0].ColumnHeader.RowCount - 1), left_logo_clm].Border.BorderColorBottom = Color.Black;
    //        foreach (DictionaryEntry parameter2 in has_degree_heading )
    //        {
    //            int row =Convert.ToInt32( (parameter2.Key));
    //            string text_val = (parameter2.Value).ToString();
    //            if (Sprd_attendance.Sheets[0].RowCount > row)
    //            {
    //                Sprd_attendance.Sheets[0].Cells[row, left_logo_clm].Text = text_val;
    //                Sprd_attendance.Sheets[0].Cells[row, left_logo_clm].Font.Bold = true;
    //                Sprd_attendance.Sheets[0].SpanModel.Add(row, left_logo_clm, 1, 1);
    //                Sprd_attendance.Sheets[0].SpanModel.Add(row, left_logo_clm, 1, (end_column - left_logo_clm + 1));
    //            }
    //        }


    //    }
    //    //  catch
    //    {
    //    }


    //}

    //==================


    //==============Hided by Manikandan 15/05/2013
    //public void header_text()
    //{

    //    SqlDataReader dr_collinfo;
    //    con.Close();
    //    con.Open();
    //   // cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header,isnull(state,'')  as state,isnull(pincode,'') as pincode,affliated,leftlogo,rightlogo,datalength(leftlogo) as leftlength,datalength(rightlogo) as rightlength,MultiISOCode,new_header_name,header_align_index,column_fields  from print_master_setting  where form_name='Overall_PercentageWise_Attnd.aspx'", con);
    //    cmd = new SqlCommand("select *From print_master_setting  where form_name='Overall_PercentageWise_Attnd.aspx' and college_code=" + Session["InternalCollegeCode"].ToString() + "", con);
    //    dr_collinfo = cmd.ExecuteReader();
    //    while (dr_collinfo.Read())
    //    {
    //        if (dr_collinfo.HasRows == true)
    //        {
    //            check_print_row = true;

    //            coll_name = dr_collinfo["college_name"].ToString();
    //            address1 = dr_collinfo["address1"].ToString();
    //            address2 = dr_collinfo["address2"].ToString();
    //            address3 = dr_collinfo["address3"].ToString();
    //            phoneno = dr_collinfo["phoneno"].ToString();
    //            faxno = dr_collinfo["faxno"].ToString();
    //            email = dr_collinfo["email"].ToString();
    //            website = dr_collinfo["website"].ToString();
    //            form_name = dr_collinfo["form_name"].ToString();
    //            header_alignment = dr_collinfo["header_align"].ToString();
    //            view_header = dr_collinfo["header_flag_value"].ToString();
    //            new_header_name = dr_collinfo["new_header_name"].ToString();
    //            leftlogo = dr_collinfo["leftlogo"].ToString();
    //            rightlogo = dr_collinfo["rightlogo"].ToString();


    //            leftlength = GetFunction("select isnull(datalength(leftlogo),0) From print_master_setting  where form_name='Overall_PercentageWise_Attnd.aspx' and college_code=" + Session["InternalCollegeCode"].ToString() + "");
    //            rightlength = GetFunction("select isnull(datalength(rightlogo),0) From print_master_setting  where form_name='Overall_PercentageWise_Attnd.aspx' and college_code=" + Session["InternalCollegeCode"].ToString() + "");

    //            multi_iso = dr_collinfo["MultiISOCode"].ToString();
    //            new_header_string_index = dr_collinfo["header_align_index"].ToString();
    //            view_footer = dr_collinfo["footer_flag_value"].ToString();
    //            view_header = dr_collinfo["header_flag_value"].ToString();
    //            view_footer_text = dr_collinfo["footer_name"].ToString();
    //            column_field = dr_collinfo["column_fields"].ToString();

    //            ddlpage.Visible = true;
    //            lblpages.Visible = true;
    //        }
    //    }
    //    if (check_print_row == false)
    //    {
    //        con.Close();
    //        con.Open();
    //        cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["InternalCollegeCode"] + "", con);
    //        dr_collinfo = cmd.ExecuteReader();
    //        while (dr_collinfo.Read())
    //        {
    //            if (dr_collinfo.HasRows == true)
    //            {                  
    //                check_print_row = true;
    //                coll_name = dr_collinfo["collname"].ToString();
    //                address1 = dr_collinfo["address1"].ToString();
    //                address2 = dr_collinfo["address2"].ToString();
    //                address3 = dr_collinfo["address3"].ToString();
    //                phoneno = dr_collinfo["phoneno"].ToString();
    //                faxno = dr_collinfo["faxno"].ToString();
    //                email = dr_collinfo["email"].ToString();
    //                website = dr_collinfo["website"].ToString();
    //                form_name = "Overall Percentagewise Attendance Report ";
    //                view_footer = "";
    //                view_header = "";
    //                view_footer_text = "";
    //                leftlogo = "";
    //                rightlogo = "";
    //                leftlength = "";
    //                rightlength = "";
    //                multi_iso = "";

    //                ddlpage.Visible = false ;
    //                lblpages.Visible = false;
    //            }

    //        }
    //    }
    //}
    //=====================

    public void persentmonthcal()
    {
        // try


        // string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
        // DataSet ds3 = new DataSet();
        //Hashtable holiday_table11 = new Hashtable();
        //Hashtable holiday_table21 = new Hashtable();
        //Hashtable holiday_table31 = new Hashtable();
        bool splhr_flag = false;
        TimeSpan ts;
        string diff_date = "";
        double dif_date1 = 0;
        int next = 0;
        {
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;
            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;
            notconsider_value = 0;
            //Hiden By Srinath 20/2/2013 ==Start
            //int demfcal, demtcal;
            //string monthcal;
            //==End
            conduct_hour_new = 0;
            frdate = frdatesample;
            todate = todatesample;
            cal_from_date = callfromdatesample;
            //Hiden By Srinath 20/2/2013 ==Start
            //  if (rows_count == 0)
            //{
            //string dt = txtfromdate.Text;
            // dt = frdate;
            //string[] dsplit = dt.Split(new Char[] { '/' });
            //frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            //demfcal = int.Parse(dsplit[2].ToString());
            //demfcal = demfcal * 12;
            //cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
            //monthcal = cal_from_date.ToString();
            //todate = txttodate.Text;
            //    dt = todate;
            //    dsplit = dt.Split(new Char[] { '/' });
            //    todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            //    demtcal = int.Parse(dsplit[2].ToString());
            //    demtcal = demtcal * 12;
            //    cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
            per_from_date = Convert.ToDateTime(frdate);
            per_to_date = Convert.ToDateTime(todate);
            //}
            dumm_from_date = per_from_date;




            has.Clear();
            has.Add("std_rollno", stud_roll_no);
            has.Add("from_month", cal_from_date);
            has.Add("to_month", cal_to_date);
            ds2 = dass.select_method("STUD_ATTENDANCE", has, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            //Hiden By Srinath 20/2/13 ==start 

            // if (rows_count == 0)
            //{ 

            //has.Clear();
            //has.Add("degree_code", degree_code);
            //has.Add("sem", stud_sem);
            //has.Add("from_date", frdate.ToString());
            //has.Add("to_date", todate.ToString());
            //has.Add("coll_code", int.Parse(Session["InternalCollegeCode"].ToString()));


            ////------------------------------------------------------------------
            //int iscount = 0;
            //holidaycon.Close();
            //holidaycon.Open();
            //string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + degree_code + " and semester=" + stud_sem + "";
            //SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
            //SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
            //DataSet dsholiday = new DataSet();
            //daholiday.Fill(dsholiday);
            //if (dsholiday.Tables[0].Rows.Count > 0)
            //{
            //    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            //}
            //has.Add("iscount", iscount);

            //ds3 = dass.select_method("ALL_HOLIDATE_DETAILS", has, "sp");
            //==End

            //Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
            //Dictionary<string, int> holiday_table2 = new Dictionary<string, int>();
            //Dictionary<string, int> holiday_table3 = new Dictionary<string, int>();

            //holiday_table11.Clear();
            //holiday_table21.Clear();
            //holiday_table31.Clear();
            //if (ds3.Tables[0].Rows.Count != 0)
            //{
            //    for (int k = 0; k < ds3.Tables[0].Rows.Count; k++)
            //    {
            //        if (ds3.Tables[0].Rows[0]["halforfull"].ToString() == "False")
            //        {
            //            halforfull = "0";
            //        }
            //        else
            //        {
            //            halforfull = "1";
            //        }
            //        if (ds3.Tables[0].Rows[0]["morning"].ToString() == "False")
            //        {
            //            mng = "0";
            //        }
            //        else
            //        {
            //            mng = "1";
            //        }
            //        if (ds3.Tables[0].Rows[0]["evening"].ToString() == "False")
            //        {
            //            evng = "0";
            //        }
            //        else
            //        {
            //            evng = "1";
            //        }

            //        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

            //        string[] split_date_time1 = ds3.Tables[0].Rows[k]["HOLI_DATE"].ToString().Split(' ');
            //        string[] dummy_split = split_date_time1[0].Split('/');
            //        holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
            //        // holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
            //    }
            //}

            //if (ds3.Tables[1].Rows.Count != 0)
            //{
            //    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
            //    {
            //        string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
            //        string[] dummy_split = split_date_time1[0].Split('/');
            //        holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

            //        if (ds3.Tables[1].Rows[k]["halforfull"].ToString() == "False")
            //        {
            //            halforfull = "0";
            //        }
            //        else
            //        {
            //            halforfull = "1";
            //        }
            //        if (ds3.Tables[1].Rows[k]["morning"].ToString() == "False")
            //        {
            //            mng = "0";
            //        }
            //        else
            //        {
            //            mng = "1";
            //        }
            //        if (ds3.Tables[1].Rows[k]["evening"].ToString() == "False")
            //        {
            //            evng = "0";
            //        }
            //        else
            //        {
            //            evng = "1";
            //        }

            //        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

            //        if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
            //        {
            //            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
            //        }
            //        holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
            //    }
            //}

            //if (ds3.Tables[2].Rows.Count != 0)
            //{
            //    for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
            //    {
            //        string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
            //        string[] dummy_split = split_date_time1[0].Split('/');
            //        holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

            //        if (ds3.Tables[2].Rows[k]["halforfull"].ToString() == "False")
            //        {
            //            halforfull = "0";
            //        }
            //        else
            //        {
            //            halforfull = "1";
            //        }
            //        if (ds3.Tables[2].Rows[k]["morning"].ToString() == "False")
            //        {
            //            mng = "0";
            //        }
            //        else
            //        {
            //            mng = "1";
            //        }
            //        if (ds3.Tables[2].Rows[k]["evening"].ToString() == "False")
            //        {
            //            evng = "0";
            //        }
            //        else
            //        {
            //            evng = "1";
            //        }

            //        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

            //        if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
            //        {
            //            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
            //        }

            //        holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
            //    }
            //}

            //==End

            //=====================================14/5/12 PRABHA
            con.Close();
            cmd.CommandText = "select rights from  special_hr_rights where usercode=" + Session["usercode"].ToString() + "";
            cmd.Connection = con;
            con.Open();
            SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
            if (dr_rights_spl_hr.HasRows)
            {
                while (dr_rights_spl_hr.Read())
                {
                    string spl_hr_rights = "";
                    Hashtable od_has = new Hashtable();

                    spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                    if (spl_hr_rights == "True" || spl_hr_rights == "true")
                    {
                        splhr_flag = true;

                    }
                }
            }
            // } //hidden By Srinath 20/2/2013
            //===================================

            //------------------------------------------------------------------
            if (ds3.Tables[0].Rows.Count != 0)
            {
                ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
            }
            next = 0;

            if (ds2.Tables[0].Rows.Count != 0)
            {
                int rowcount = 0;
                int ccount;
                ccount = ds3.Tables[1].Rows.Count;
                ccount = ccount - 1;
                //if ( == ds2.Tables [0].Rows [mmyycount].["Month_year"])
                while (dumm_from_date <= (per_to_date))
                {

                    if (splhr_flag == true)
                    {
                        //added By srinath 13/2/2013 ===start
                        if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
                        {
                            getspecial_hr();
                        }
                        //==End
                    }

                    for (int i = 1; i <= mmyycount; i++)
                    {
                        if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
                        {
                            string[] split_date_time1 = dumm_from_date.ToString().Split(' ');
                            string[] dummy_split = split_date_time1[0].Split('/');


                            if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                            {
                                holiday_table11.Add(((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()), "3*0*0");
                            }

                            if (holiday_table11.Contains((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                            {
                                value_holi_status = GetCorrespondingKey(dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString(), holiday_table11).ToString();
                                split_holiday_status = value_holi_status.Split('*');

                                if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                                {
                                    split_holiday_status_1 = "1";
                                    split_holiday_status_2 = "1";
                                }
                                else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                                {
                                    if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                                    {
                                        split_holiday_status_1 = "0";
                                        split_holiday_status_2 = "1";
                                    }

                                    if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                                    {
                                        split_holiday_status_1 = "1";
                                        split_holiday_status_2 = "0";
                                    }
                                }
                                else if (split_holiday_status[0].ToString() == "0")
                                {
                                    dumm_from_date = dumm_from_date.AddDays(1);
                                    if (dumm_from_date.Day == 1)
                                    {
                                        cal_from_date++;
                                        if (moncount > next)
                                        {
                                            next++;

                                        }
                                    }
                                    break;
                                }

                                //if (ds3.Tables[1].Rows.Count != 0)
                                //{
                                //    ts = DateTime.Parse(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                //    diff_date = Convert.ToString(ts.Days);
                                //    dif_date = double.Parse(diff_date.ToString());
                                //}
                                //else
                                //{
                                //    dif_date = 0;
                                //}
                                //if (dif_date == 1)
                                //{
                                //    leave_pointer = holi_leav;
                                //    absent_pointer = holi_absent;
                                //}
                                //else if (dif_date == -1)
                                //{
                                //    leave_pointer = holi_leav;
                                //    absent_pointer = holi_absent;
                                //    if (ccount > rowcount)
                                //    {
                                //        rowcount += 1;
                                //    }
                                //}
                                //else
                                //{
                                //    leave_pointer = leav_pt;
                                //    absent_pointer = absent_pt;

                                //}

                                //if (ds3.Tables[2].Rows.Count != 0)
                                //{
                                //    ts = DateTime.Parse(ds3.Tables[2].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                //    diff_date = Convert.ToString(ts.Days);
                                //    dif_date = double.Parse(diff_date.ToString());
                                //    if (dif_date == 1)
                                //    {
                                //        leave_pointer = holi_leav;
                                //        absent_pointer = holi_absent;
                                //    }

                                //}
                                //if (dif_date1 == -1)
                                //{
                                //    leave_pointer = holi_leav;
                                //    absent_pointer = holi_absent;
                                //}
                                //dif_date1 = 0;
                                if (split_holiday_status_1 == "1")
                                {

                                    for (i = 1; i <= fnhrs; i++)
                                    {
                                        date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                        value = ds2.Tables[0].Rows[next][date].ToString();

                                        if (value != null && value != "0" && value != "7" && value != "")
                                        {
                                            if (tempvalue != value)
                                            {
                                                tempvalue = value;
                                                for (int j = 0; j < count; j++)
                                                {

                                                    if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                    {
                                                        ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                        j = count;
                                                    }
                                                }
                                            }
                                            if (ObtValue == 1)
                                            {
                                                per_abshrs += 1;
                                            }
                                            else if (ObtValue == 2)
                                            {
                                                notconsider_value += 1;
                                                njhr += 1;
                                            }
                                            else if (ObtValue == 0)
                                            {
                                                per_perhrs += 1;
                                                tot_per_hrs += 1;
                                            }
                                            if (value == "3")
                                            {
                                                per_ondu += 1;
                                                tot_ondu += 1;
                                            }
                                            else if (value == "10")
                                            {
                                                per_leave += 1;
                                            }
                                        }
                                        else if (value == "7")
                                        {
                                            per_hhday += 1;

                                        }
                                        else
                                        {
                                            unmark += 1;
                                        }
                                    }

                                    //  if (per_perhrs >= minpresI)
                                    if (per_perhrs + njhr >= minpresI)
                                    {
                                        Present += 0.5;
                                    }

                                    else if (per_leave >= 1)
                                    {
                                        leave_point += leave_pointer / 2;
                                        Leave += 0.5;
                                    }

                                    else if (per_abshrs >= 1)
                                    {
                                        Absent += 0.5;
                                        absent_point += absent_pointer / 2;
                                    }
                                    if (njhr >= minpresI)
                                    {
                                        njdate += 0.5;
                                        njdate_mng += 1;
                                    }
                                    if (per_ondu >= 1)
                                    {
                                        Onduty += 0.5;
                                    }

                                    if (unmark == fnhrs)
                                    {
                                        per_holidate_mng += 1;
                                        per_holidate += 0.5;
                                        unmark = 0;
                                    }

                                    workingdays += 0.5;
                                    mng_conducted_half_days += 1;

                                }
                                per_perhrs = 0;
                                per_ondu = 0;
                                per_leave = 0;
                                per_abshrs = 0;
                                // unmark = 0;
                                njhr = 0;
                                int temp_unmark = 0;
                                int k = fnhrs + 1;

                                if (split_holiday_status_2 == "1")
                                {
                                    for (i = k; i <= NoHrs; i++)
                                    {
                                        date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                        value = ds2.Tables[0].Rows[next][date].ToString();

                                        if (value != null && value != "0" && value != "7" && value != "")
                                        {
                                            if (tempvalue != value)
                                            {
                                                tempvalue = value;
                                                for (int j = 0; j < count; j++)
                                                {

                                                    if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                    {
                                                        ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                        j = count;
                                                    }
                                                }
                                            }
                                            if (ObtValue == 1)
                                            {
                                                per_abshrs += 1;
                                            }
                                            else if (ObtValue == 2)
                                            {
                                                notconsider_value += 1;
                                                njhr += 1;
                                            }
                                            else if (ObtValue == 0)
                                            {
                                                per_perhrs += 1;
                                                tot_per_hrs += 1;
                                            }
                                            if (value == "3")
                                            {
                                                per_ondu += 1;
                                                tot_ondu += 1;
                                            }
                                            else if (value == "10")

                                                per_leave += 1;
                                        }
                                        else if (value == "7")
                                        {
                                            per_hhday += 1;
                                        }
                                        else
                                        {
                                            unmark += 1;
                                        }
                                    }
                                    //   if (per_perhrs >= minpresII)
                                    if (per_perhrs + njhr >= minpresII)
                                    {
                                        Present += 0.5;
                                    }

                                    else if (per_leave >= 1)
                                    {

                                        leave_point += leave_pointer / 2;
                                        Leave += 0.5;
                                    }

                                    else if (per_abshrs >= 1)
                                    {
                                        Absent += 0.5;
                                        absent_point += absent_pointer / 2;
                                    }
                                    if (njhr >= minpresII)
                                    {
                                        njdate_evng += 1;
                                        njdate += 0.5;
                                    }
                                    if (per_ondu >= 1)
                                    {
                                        Onduty += 0.5;
                                    }



                                    if (unmark == NoHrs - fnhrs)
                                    {
                                        per_holidate_evng += 1;
                                        per_holidate += 0.5;
                                        unmark = 0;
                                    }
                                    else
                                    {
                                        dum_unmark += unmark;
                                    }



                                    workingdays += 0.5;
                                    evng_conducted_half_days += 1;

                                }

                                per_perhrs = 0;
                                per_ondu = 0;
                                per_leave = 0;
                                per_abshrs = 0;
                                unmark = 0;
                                njhr = 0;


                                dumm_from_date = dumm_from_date.AddDays(1);
                                if (dumm_from_date.Day == 1)
                                {
                                    cal_from_date++;
                                    if (moncount > next)
                                    {
                                        next++;

                                    }
                                }


                                per_perhrs = 0;

                            }

                        }
                        else
                        {

                            DateTime dumm_fdate = dumm_from_date.AddDays(1 - Convert.ToInt16(dumm_from_date.Day.ToString()));
                            dumm_fdate = dumm_fdate.AddMonths(1);
                            dumm_from_date = dumm_fdate;

                            if (dumm_from_date.Day == 1)
                            {

                                cal_from_date++;


                                if (moncount > next)
                                {
                                    //  next++;
                                }

                            }

                            if (moncount > next)
                            {
                                i--;
                            }
                        }

                    }
                }
                int diff_Date = per_from_date.Day - dumm_from_date.Day;
            }

            per_tot_ondu = tot_ondu;
            per_njdate = njdate;
            pre_present_date = Present - njdate;
            per_per_hrs = tot_per_hrs;
            per_absent_date = Absent;
            pre_ondu_date = Onduty;
            pre_leave_date = Leave;
            per_workingdays = workingdays - per_holidate - per_njdate;
            per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value;// ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));
            per_dum_unmark = dum_unmark;

            Present = 0;
            tot_per_hrs = 0;
            Absent = 0;
            Onduty = 0;
            Leave = 0;
            workingdays = 0;
            per_holidate = 0;
            dum_unmark = 0;
            absent_point = 0;
            leave_point = 0;
            njdate = 0;
            tot_ondu = 0;
        }
        //   catch
        {
        }
    }

    public void getspecial_hr()
    {
        //  try
        {
            //added By Srinath 22/2/2013 ==Start
            string hrdetno = "";
            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dumm_from_date), ht_sphr));

            }
            if (hrdetno != "")
            {
                //========End
                con_splhr_query_master.Close();
                con_splhr_query_master.Open();
                DataSet ds_splhr_query_master = new DataSet();
                //  no_stud_flag = false;
                //string splhr_query_master = "select attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no=(select hrentry_no from  specialhr_master where batch_year=" + stud_batch_year + " and semester=" + stud_sem + " and degree_code=" + degree_code + " and date='" + dumm_from_date + "')  ) and r.roll_no=sa.roll_no and batch_year=" + stud_batch_year + " and current_semester=" + stud_sem + " and degree_code=" + degree_code + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar') and sa.roll_no='" + stud_roll_no + "'  order by r.roll_no asc";
                string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + stud_roll_no + "'  and hrdet_no in(" + hrdetno + ")";
                SqlDataReader dr_splhr_query_master;
                cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
                dr_splhr_query_master = cmd.ExecuteReader();

                while (dr_splhr_query_master.Read())
                {
                    if (dr_splhr_query_master.HasRows)
                    {
                        value = dr_splhr_query_master[0].ToString();

                        if (value != null && value != "0" && value != "7" && value != "")
                        {
                            if (tempvalue != value)
                            {
                                tempvalue = value;
                                for (int j = 0; j < count; j++)
                                {

                                    if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                    {
                                        ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                        j = count;
                                    }
                                }
                            }
                            if (ObtValue == 1)
                            {
                                per_abshrs_spl += 1;
                            }
                            else if (ObtValue == 2)
                            {
                                notconsider_value += 1;
                                njhr += 1;
                            }
                            else if (ObtValue == 0)
                            {
                                tot_per_hrs_spl += 1;
                            }
                            if (value == "3")
                            {
                                tot_ondu_spl += 1;
                            }
                            else if (value == "10")
                            {
                                per_leave += 1;
                            }
                            tot_conduct_hr_spl++;
                        }
                        else if (value == "7")
                        {
                            per_hhday_spl += 1;
                            tot_conduct_hr_spl--;
                        }
                        else
                        {
                            unmark_spl += 1;
                            tot_conduct_hr_spl--;
                        }
                    }
                }


                per_abshrs_spl_fals = per_abshrs_spl;
                tot_per_hrs_spl_fals = tot_per_hrs_spl;
                per_leave_fals = per_leave;
                tot_conduct_hr_spl_fals = tot_conduct_hr_spl;
                tot_ondu_spl_fals = tot_ondu_spl;
            }//Added By Srinath 22/2/2013
        }
        //  catch
        {
        }
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }

        return null;
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        lblother.Visible = false;
        TextBoxother.Text = "";
        ////Sprd_attendance.CurrentPage = 0;
        if (DropDownListpage.Text == "Others")
        {

            TextBoxother.Visible = true;
            TextBoxother.Focus();

        }
        else
        {
            TextBoxother.Visible = false;
            ////Sprd_attendance.Sheets[0].PageSize = Convert.ToInt32(DropDownListpage.Text.ToString());
            gview.PageSize = Convert.ToInt32(DropDownListpage.Text.ToString());
            
            CalculateTotalPages();
        }
        ////Sprd_attendance.CurrentPage = 0;
    }

    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        ////Sprd_attendance.CurrentPage = 0;
        LabelE.Visible = false;
        lblother.Visible = false;
        try
        {
            ////if (Sprd_attendance.Sheets[0].RowCount > 0)
            if (gview.Rows.Count > 0)
            {
                if (TextBoxother.Text != string.Empty)
                {
                    ////Sprd_attendance.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                    gview.PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                    CalculateTotalPages();
                    lblother.Visible = false;
                }
            }
        }
        catch
        {
            lblother.Text = "Enter the Valid Page";
            TextBoxother.Text = "";
            lblother.Visible = true;
        }
    }

    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        ////Sprd_attendance.CurrentPage = 0;
        LabelE.Visible = false;
        lblother.Visible = false;
        try
        {
            ////if (Sprd_attendance.Sheets[0].RowCount > 0)
            if (gview.Rows.Count > 0)
            {
                if (TextBoxpage.Text.Trim() != string.Empty)
                {
                    if (Convert.ToInt32(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                    {
                        LabelE.Visible = true;
                        LabelE.Text = "Exceed The Page Limit";
                        TextBoxpage.Text = "";
                        ////Sprd_attendance.Visible = true;
                        gview.Visible = true;
                        btnprintmaster.Visible = true;
                        btnxl.Visible = true;
                        //Added By Srinath 27/2/2013
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;
                    }
                    else if ((Convert.ToInt32(TextBoxpage.Text) == 0))
                    {
                        LabelE.Text = "Page search should be more than 0";
                        LabelE.Visible = true;
                        TextBoxpage.Text = "";
                        ////Sprd_attendance.Visible = true;
                        gview.Visible = true;
                        btnprintmaster.Visible = true;
                        btnxl.Visible = true;
                        //Added By Srinath 27/2/2013
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;
                    }
                    else
                    {
                        LabelE.Visible = false;
                        ////Sprd_attendance.CurrentPage = Convert.ToInt32(TextBoxpage.Text) - 1;                        
                        ////Sprd_attendance.Visible = true;
                        gview.Visible = true;
                        btnprintmaster.Visible = true;
                        btnxl.Visible = true;
                        //Added By Srinath 27/2/2013
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;
                    }
                }
            }
        }
        catch
        {
            LabelE.Text = "Exceed The Page Limit";
            TextBoxpage.Text = "";
            LabelE.Visible = true;
        }
    }

    public void print_btngo()
    {
        try
        {
            int col_count_all = 0;
            if (column_field != "" && column_field != null)
            {


                btngoclick_function();//---------------load function


                ////for (col_count_all = 0; col_count_all < Sprd_attendance.Sheets[0].ColumnCount; col_count_all++)
                ////{
                ////    Sprd_attendance.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column                                
                ////}


                string[] split_printvar = column_field.Split(',');
                for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
                {

                    string[] split_star = split_printvar[splval].Split('*');
                    //if(split_star.GetUpperBound(0)>0)
                    {

                        ////for (col_count = 0; col_count < Sprd_attendance.Sheets[0].ColumnCount; col_count++)
                        ////{
                        ////    if (Sprd_attendance.Sheets[0].Cells[1, col_count].Text == split_printvar[splval])
                        ////    {
                        ////        Sprd_attendance.Sheets[0].Columns[col_count].Visible = true;

                        ////        final_print_col_cnt++;
                        ////        break;
                        ////    }
                        ////}
                    }
                }
                //1 end.set visible columns
            }
        }
        catch
        {
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        //Modified by Srinath 27/2/2013
        string reportname = txtexcelname.Text;

        if (reportname.ToString().Trim() != "")
        {
            d2.printexcelreportgrid(gview, reportname);
            txtexcelname.Text = "";
        }
        else
        {
            lblerr.Text = "Please Enter Your Report Name";
            lblerr.Visible = true;
        }
        //string appPath = HttpContext.Current.Server.MapPath("~");
        //string print = "";
        //if (appPath != "")
        //{
        //    int i = 1;
        //    appPath = appPath.Replace("\\", "/");
        //e:
        //    try
        //    {
        //        print = "Percentagewise Attnedance Report" + i;
        //        //Sprd_attendance.SaveExcel(appPath + "/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
        //        //Aruna on 26feb2013============================
        //        string szPath = appPath + "/Report/";
        //        string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

        //        Sprd_attendance.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
        //        Response.Clear();
        //        Response.ClearHeaders();
        //        Response.ClearContent();
        //        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
        //        Response.ContentType = "application/vnd.ms-excel";
        //        Response.Flush();
        //        Response.WriteFile(szPath + szFile);
        //        //=============================================

        //    }
        //    catch
        //    {
        //        i++;
        //        goto e;

        //    }
        //}
        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        //Control cntUpdateBtn = Sprd_attendance.FindControl("Update");
        //Control cntCancelBtn = Sprd_attendance.FindControl("Cancel");
        //Control cntCopyBtn = Sprd_attendance.FindControl("Copy");
        //Control cntCutBtn = Sprd_attendance.FindControl("Clear");
        //Control cntPasteBtn = Sprd_attendance.FindControl("Paste");
        ////Control cntPageNextBtn = Sprd_attendance.FindControl("Next");
        ////Control cntPagePreviousBtn = Sprd_attendance.FindControl("Prev");
        Control cntPageNextBtn = gview.FindControl("Next");
        Control cntPagePreviousBtn = gview.FindControl("Prev");
        // Control cntPagePrintBtn = Sprd_attendance.FindControl("Print");

        if ((cntPageNextBtn != null))
        {

            TableCell tc = (TableCell)cntPageNextBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            //tc = (TableCell)cntCancelBtn.Parent;
            //tr.Cells.Remove(tc);


            //tc = (TableCell)cntCopyBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntCutBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPasteBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);

            ////tc = (TableCell)cntPagePrintBtn.Parent;
            ////tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["QueryString"]) != "")
        {

            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove(Convert.ToString(Session["QueryString"]));
            Request.QueryString.Clear();

        }

        Session["current_college_code"] = ddlcollege.SelectedValue.ToString();
        Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
        Pageload(sender, e);
    }

    public void Pageload(object sender, EventArgs e)
    {
        
        lblerr.Visible = false;
        ////Sprd_attendance.Visible = false;
        gview.Visible = false;
        btnprintmaster.Visible = false;
        //ddlpage.Visible = false;
        //lblpages.Visible = false;
        pageset_pnl.Visible = false;
        btnxl.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

        //bindyear();

        DateTime currdate = DateTime.Now;
        txtfromdate.Text = currdate.ToString("dd") + "/" + currdate.ToString("MM") + "/" + currdate.ToString("yyyy");
        txttodate.Text = currdate.ToString("dd") + "/" + currdate.ToString("MM") + "/" + currdate.ToString("yyyy");

        ////Sprd_attendance.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        ////Sprd_attendance.Sheets[0].DefaultStyle.Font.Name = "Book Antique";
        ////Sprd_attendance.Sheets[0].AutoPostBack = true;
        ////Sprd_attendance.Sheets[0].RowHeader.Visible = false;

        ////FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
        ////style1.Font.Size = 12;
        ////style1.Font.Bold = true;
        ////style1.HorizontalAlign = HorizontalAlign.Center;
        ////style1.ForeColor = Color.Black;
        ////style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        ////Sprd_attendance.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
        ////Sprd_attendance.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
        ////Sprd_attendance.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
        ////Sprd_attendance.Sheets[0].AllowTableCorner = true;
        ////Sprd_attendance.CommandBar.Visible = false;

        //if (Session["prntvissble"].ToString() == "true")
        //{
        //    btnPrint.Visible = true;
        //}
        //else
        //{
        //    btnPrint.Visible = false;
        //}

        if (Request.QueryString["val"] == null)
        {
            bindyear();
        }
        else if (Request.QueryString["val"] != null)
        {
            Session["QueryString"] = Request.QueryString["val"];
            string[] string_session_values = Request.QueryString["val"].Split(',');
            if (string_session_values.GetUpperBound(0) >= 5)
            {
                ddlcollege.SelectedIndex = Convert.ToInt16(string_session_values[6].ToString());
                Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();

                bindyear();

                ddlyear.SelectedIndex = Convert.ToInt32(string_session_values[0]);
                ddlyear_SelectedIndexChanged(sender, e);

                //-------------------load graduation



                temp_count = 0;

                for (temp_count = 0; temp_count < chkbxlist_graduate.Items.Count; temp_count++)
                {
                    chkbxlist_graduate.Items[temp_count].Selected = false;
                }

                temp_count = 0;
                check_value_graduate_splt = string_session_values[1].Split('@');


                for (temp_count = 0; temp_count <= check_value_graduate_splt.GetUpperBound(0); temp_count++)
                {
                    chkbxlist_graduate.Items[Convert.ToInt32(check_value_graduate_splt[temp_count])].Selected = true;
                }
                //chkbxlist_graduate_SelectedIndexChanged(sender, e);

                //-------------------load degree

                temp_count = 0;

                for (temp_count = 0; temp_count < chkbxlistDegree.Items.Count; temp_count++)
                {
                    chkbxlistDegree.Items[temp_count].Selected = false;
                }

                temp_count = 0;
                check_value_degree_splt = string_session_values[2].Split('@');
                for (temp_count = 0; temp_count <= check_value_degree_splt.GetUpperBound(0); temp_count++)
                {
                    chkbxlistDegree.Items[Convert.ToInt32(check_value_degree_splt[temp_count])].Selected = true;
                }
                //chkbxlistDegree_SelectedIndexChanged(sender, e);

                //-------------------load branch
                temp_count = 0;

                for (temp_count = 0; temp_count < chkbxlistbranch.Items.Count; temp_count++)
                {
                    chkbxlistbranch.Items[temp_count].Selected = false;
                }

                temp_count = 0;
                check_value_branch_splt = string_session_values[3].Split('@');
                for (temp_count = 0; temp_count <= check_value_branch_splt.GetUpperBound(0); temp_count++)
                {
                    chkbxlistbranch.Items[Convert.ToInt32(check_value_branch_splt[temp_count])].Selected = true;
                }

                //chkbxlistbranch_SelectedIndexChanged(sender, e);


                txtfromdate.Text = string_session_values[4];

                txttodate.Text = string_session_values[5];




                //header_text();
                print_btngo();
                //setheader_print();


            }
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 1;
        string graduate = string.Empty;
        string deg = string.Empty;
        string brnch = string.Empty;
        string date_pdf = string.Empty;
        string header = string.Empty;
        string degreedetails = string.Empty;

        for (int get_graduate = 0; get_graduate < chkbxlist_graduate.Items.Count; get_graduate++)
        {
            if (chkbxlist_graduate.Items[get_graduate].Selected == true)
            {
                if (graduate == "")
                {
                    graduate = chkbxlist_graduate.Items[get_graduate].Text.ToString();
                }
                else
                {
                    graduate = graduate + "," + chkbxlist_graduate.Items[get_graduate].Text.ToString();
                }
            }
        }

        for (int get_degree = 0; get_degree < chkbxlistDegree.Items.Count; get_degree++)
        {
            if (chkbxlistDegree.Items[get_degree].Selected == true)
            {
                if (deg == "")
                {
                    deg = chkbxlistDegree.Items[get_degree].Text.ToString();
                }
                else
                {
                    deg = deg + "," + chkbxlistDegree.Items[get_degree].Text.ToString();
                }
            }
        }

        for (int get_brnch = 0; get_brnch < chkbxlistbranch.Items.Count; get_brnch++)
        {
            if (chkbxlistbranch.Items[get_brnch].Selected == true)
            {
                if (brnch == "")
                {
                    brnch = chkbxlistbranch.Items[get_brnch].Text.ToString();
                }
                else
                {
                    brnch = brnch + "," + chkbxlistbranch.Items[get_brnch].Text.ToString();
                }
            }
        }
        degreedetails = "Overall PercentageWise Attendance Report " + "@Date: " + txtfromdate.Text.ToString() + " - " + txttodate.Text.ToString();

        string pagename = "Overall_PercentageWise_Attnd.aspx";
        string ss = null;
        NEWPrintMater1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
        NEWPrintMater1.Visible = true;
    }
}
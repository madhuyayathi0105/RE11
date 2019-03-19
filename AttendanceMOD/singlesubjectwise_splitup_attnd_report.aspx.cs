//==========MANIPRABHA A.
using System;//-------------------------15/03/12, 16/3/12(XL), 29/3/12(NaN value, cc delflag in attnd query,spread visible)
//===============29/3/12(halfholiday,30/3/12(more than mon staff in a same hour(extra tot hrs issue)),30/3/12(len(r_no))
//================4/4/12(getfun, day_val), 12/4/12(complete print setting),23/4/12(txt cell type , 0->holi),25/4/12(2 batch for a same period)
//=============, 26/4/12(trim() in text), 27/4/12(tot counts change, remove all columns,add row in header for tot hr), 
//================3/5/12(change header setting function for more span col cnt val), 11/05/12( halforfull='0'),
//================14/5/12(special hour included,header span prob), 29/5/12(modification in practical query), 30/5/12(include HS)
//=====31/5/12(HS not consider condition),9/6/12(page load try, p_m_s_n,header_index),15/6/12(col visible)
//----------------16/6/12(spl issue),22/6/12(header caption ),30/6/12 (change single qurey into 2 query),3/7/12(remove : fm header)
//----------------3/7/12(add HS condition,change single qurey into 2 query) , modified on 04.07.12 by mythili(header_alignment)
//====================26/7/12(find lab or not)
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;

public partial class singlesubjectwise_splitup_attnd_report : System.Web.UI.Page
{
    [Serializable()]
    public class MyImg : ImageCellType
    {
        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, bool upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(80);
            return img;
        }
    }

    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sem_roman = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_attnd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_attnd1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    SqlCommand cmd1;
    SqlCommand cmd;
    SqlCommand cmd_attnd;
    SqlCommand cmd_sem_shed;
    SqlCommand cmd_alt_shed;

    Hashtable has = new Hashtable();
    Hashtable has_load_rollno = new Hashtable();
    Hashtable has_total_attnd_hour = new Hashtable();
    Hashtable result_has = new Hashtable();
    Hashtable hat_holy = new Hashtable();
    Hashtable has_attnd_masterset = new Hashtable();
    Hashtable has_od = new Hashtable();

    DataSet ds_getvalues = new DataSet();
    DataSet ds = new DataSet();
    DataSet ds_subject = new DataSet();
    DataSet ds_holi = new DataSet();
    DataSet ds_student = new DataSet();
    DataSet ds_alter = new DataSet();

    DAccess2 dacc = new DAccess2();
    DataSet ds_attndmaster = new DataSet();

    int count_master = 0;
    string present_calcflag = string.Empty;
    static Hashtable has_subtype = new Hashtable();
    Hashtable temp_has_subj_code = new Hashtable();
    double max_tot = 0;
    double attnd_hr = 0, tot_hr = 0, atndsplhr, abssplhr, totsplhr, odspl;
    //------------------------------------------16/6/12
    string new_header_string_index = string.Empty;
    string isonumber = string.Empty;
    //==============0n 11/4/12 PRABHA
    string[] string_session_values = new string[100];
    int temp_count = 0, final_print_col_cnt = 0, split_col_for_footer = 0, col_count = 0, footer_balanc_col = 0, footer_count = 0;
    int col_count_all = 0, span_cnt = 0, child_span_count = 0;
    bool check_col_count_flag = false;
    static DataSet dsprint = new DataSet();
    string new_header_string = "", column_field = "", printvar = string.Empty;
    string view_footer = "", view_header = "", view_footer_text = string.Empty;
    int start_column = 0, end_column = 0;
    string coll_name = "", address1 = "", address2 = "", address3 = "", form_name = "", phoneno = "", faxno = "", email = "", website = "", degree_val = string.Empty;
    string footer_text = "", header_alignment = string.Empty;
    string degree_deatil = string.Empty;
    int new_header_count = 0;
    string[] new_header_string_split;
    string phone = "", fax = "", email_id = "", web_add = string.Empty;
    bool btnclick_or_print = false;
    int between_visible_col_cnt = 0, between_visible_col_cnt_bal = 0;
    int x = 0;
    int visi_col = 0, visi_col1 = 0;
    //---------------------------
    //--------
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    int split_holiday_status_1 = 0, split_holiday_status_2 = 0;
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string get_date_holiday = string.Empty;
    Hashtable holiday = new Hashtable();
    int mng_hrs = 0, evng_hrs = 0;
    //----------
    int od_count = 0;
    string subj_type = string.Empty;
    string group_user = "", singleuser = "", usercode = "", collegecode = string.Empty;
    string date1 = "", datefrom = "", date2 = "", dateto = string.Empty;
    DateTime dt1 = new DateTime();
    DateTime dt2 = new DateTime();
    int temp_stud_count = 0;
    string strsec = string.Empty;
    int row_count = 0;
    int no_of_hrs = 0;
    string order = string.Empty;
    string roll_no = string.Empty;
    string sem_start_date = string.Empty;
    string strDay = "", dummy_date = "", temp_hr_field = "", subject_no = string.Empty;
    string full_hour = string.Empty;
    string single_hour = string.Empty;
    bool recflag = false;
    DateTime temp_date = new DateTime();
    int stud_count = 0;
    string Att_mark;
    bool check_alter = false;
    int span_count = 0;
    string date_temp_field = "", month_year = string.Empty;
    int present_count = 0;
    int roll_count = 0;
    DateTime Admission_date;
    string section_lab = string.Empty;
    static string grouporusercode = string.Empty;
    static string grouporusercode1 = string.Empty;
    //added By Srinath 22/2/2013 
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();
    Hashtable hat = new Hashtable();
    bool chkflag = false;
    bool splhr_flag = false;
    Hashtable has_attnd_masterset_notconsider = new Hashtable();
    Hashtable hatsplhrattendance = new Hashtable();
    string attMinShortagePercentage = string.Empty;
    double attMinShorage = 0;
    DataTable data = new DataTable();
    DataRow drow;
    Dictionary<string, string> dicstd = new Dictionary<string, string>();
    Dictionary<int, string> dicstdroll = new Dictionary<int, string>();
    Dictionary<int, string> dicdisCon = new Dictionary<int, string>();
    int colcount = 0;
    string includediscon = "";
    string includedebar = "";
    ArrayList arrColHdrNames1 = new ArrayList();
    int colHdrIndx = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        errmsg.Visible = false;
        if (!Page.IsPostBack)
        {
            //txtAttShoratgePecentage.Attributes.Add("type", "number");
            txtAttShoratgePecentage.Attributes.Add("autocomplete", "off");
            txtAttShoratgePecentage.Attributes.Add("min", "0");
            txtAttShoratgePecentage.Attributes.Add("max", "100");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                grouporusercode1 = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                grouporusercode1 = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            //if (Convert.ToString(Session["value"]) == "1")//==========back button visible
            //{
            //    LinkButton3.Visible = false;
            //    LinkButton2.Visible = true;
            //}
            //else
            //{
            //    LinkButton3.Visible = true;
            //    LinkButton2.Visible = false;
            //}


            //==================visibility
            Showgrid.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            btnprintdirect.Visible = false;
            lblexcl.Visible = false;//added By Srinath 11/2/2013
            txtexcel.Visible = false;
            pnl_pagesetting.Visible = false;
            errlbl.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            //   pageddltxt.Visible = false;
            ddlpage.Visible = false;
            lblpages.Visible = false;
            string date = Convert.ToString(DateTime.Today.ToShortDateString());
            string[] split = date.Split(new Char[] { '/' });
            string date_disp = split[1] + "/" + split[0] + "/" + split[2];
            txtFromDate.Text = date_disp.ToString();
            Session["curr_year"] = split[2].ToString();
            //------------initial date picker value
            string dt = DateTime.Today.ToShortDateString();
            string[] dsplit = dt.Split(new Char[] { '/' });
            txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            //=======================on 11/4/12
            if (Request.QueryString["val"] == null)
            {
                bindbatch();//-----------------call bind functions
                binddegree();
                if (ddldegree.Items.Count > 0)
                {
                    ddldegree.Enabled = true;
                    ddlbranch.Enabled = true;
                    ddlduration.Enabled = true;
                    ddlsec.Enabled = true;
                    btnGo.Enabled = true;
                    txtFromDate.Enabled = true;
                    txtToDate.Enabled = true;
                    bindbranch();
                    bindsem();
                    bindsec();
                    load_subject();
                }
                else
                {
                    ddldegree.Enabled = false;
                    ddlbranch.Enabled = false;
                    ddlduration.Enabled = false;
                    ddlsec.Enabled = false;
                    btnGo.Enabled = false;
                    txtFromDate.Enabled = false;
                    txtToDate.Enabled = false;
                }
            }
            else
            {
                //=======================page redirect from master print setting
                try
                {
                    string_session_values = Request.QueryString["val"].Split(',');
                    if (string_session_values.GetUpperBound(0) == 8)
                    {
                        bindbatch();
                        ddlbatch.SelectedIndex = Convert.ToInt16(string_session_values[0]);
                        binddegree();
                        if (ddldegree.Items.Count > 0)
                        {
                            ddldegree.Enabled = true;
                            ddlbranch.Enabled = true;
                            ddlduration.Enabled = true;
                            ddlsec.Enabled = true;
                            btnGo.Enabled = true;
                            txtFromDate.Enabled = true;
                            txtToDate.Enabled = true;
                            ddldegree.SelectedIndex = Convert.ToInt16(string_session_values[1]);
                            bindbranch();
                            if (ddlbranch.Enabled == true)
                            {
                                ddlbranch.SelectedIndex = Convert.ToInt16(string_session_values[2].ToString());
                            }
                            bindsem();
                            if (ddlduration.Enabled == true)
                            {
                                ddlduration.SelectedIndex = Convert.ToInt16(string_session_values[3].ToString());
                            }
                            bindsec();
                            if (ddlsec.Enabled == true)
                            {
                                ddlsec.SelectedIndex = Convert.ToInt16(string_session_values[4].ToString());
                            }
                            txtFromDate.Text = string_session_values[5].ToString();
                            txtToDate.Text = string_session_values[6].ToString();
                            if (Convert.ToBoolean(string_session_values[7].ToString() == "True"))
                            {
                                ddlsubject.SelectedIndex = Convert.ToInt16(string_session_values[8].ToString());
                            }
                            else
                            {
                                ddlsubject.Enabled = false;
                            }
                            load_subject();
                            Showgrid.Width = final_print_col_cnt * 100;
                        }
                        else
                        {
                            ddldegree.Enabled = false;
                            ddlbranch.Enabled = false;
                            ddlduration.Enabled = false;
                            ddlsec.Enabled = false;
                            btnGo.Enabled = false;
                            txtFromDate.Enabled = false;
                            txtToDate.Enabled = false;
                        }
                    }
                    //===================================
                }
                catch
                {
                }
            }
            //======================
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["Sex"] = "0";
            Session["flag"] = "-1";
            ViewState["strvar"] = string.Empty;
            string Master = string.Empty;
            string strdayflag = string.Empty;
            //  string regularflag = string.Empty;
            string genderflag = string.Empty;
            Master = "select * from Master_Settings where " + grouporusercode + "";
            mysql.Open();
            SqlDataReader mtrdr;
            SqlCommand mtcmd = new SqlCommand(Master, mysql);
            string regularflag = string.Empty;
            mtrdr = mtcmd.ExecuteReader();
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
                    //if (mtrdr["settings"].ToString() == "General attend" && mtrdr["value"].ToString() == "1")
                    //{
                    //    option.SelectedValue = "1";
                    //}
                    //if (mtrdr["settings"].ToString() == "Absentees" && mtrdr["value"].ToString() == "1")
                    //{
                    //    option.SelectedValue = "2";
                    //    //PanelindBody.Visible = true;
                    //}
                    //if (mtrdr["settings"].ToString() == "RollNo" && mtrdr["value"].ToString() == "1")
                    //{
                    //    RadioButtonList1.SelectedValue = "1";
                    //}
                    //if (mtrdr["settings"].ToString() == "RegisterNo" && mtrdr["value"].ToString() == "1")
                    //{
                    //    RadioButtonList1.SelectedValue = "2";
                    //}
                    //if (mtrdr["settings"].ToString() == "Admission No" && mtrdr["value"].ToString() == "1")
                    //{
                    //    RadioButtonList1.SelectedValue = "3";
                    //}
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
                        genderflag = " and (app.sex='0'";
                    }
                    if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                    {
                        if (genderflag != "" && genderflag != "\0")
                        {
                            genderflag = genderflag + " or app.sex='1'";
                        }
                        else
                        {
                            genderflag = " and (app.sex='1'";
                        }
                    }
                    if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                    {
                        strdayflag = " and (r.Stud_Type='Day Scholar'";
                    }
                    if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                    {
                        if (strdayflag != null && strdayflag != "\0")
                        {
                            strdayflag = strdayflag + " or r.Stud_Type='Hostler'";
                        }
                        else
                        {
                            strdayflag = " and (r.Stud_Type='Hostler'";
                        }
                    }
                    if (mtrdr["settings"].ToString() == "Regular")
                    {
                        regularflag = "and ((r.mode=1)";
                        // ViewState["strvar"] = ViewState["strvar"] + " and (mode=1)";
                    }
                    if (mtrdr["settings"].ToString() == "Lateral")
                    {
                        if (regularflag != "")
                        {
                            regularflag = regularflag + " or (r.mode=3)";
                        }
                        else
                        {
                            regularflag = regularflag + " and ((r.mode=3)";
                        }
                        //ViewState["strvar"] = ViewState["strvar"] + " and (mode=3)";
                    }
                    if (mtrdr["settings"].ToString() == "Transfer")
                    {
                        if (regularflag != "")
                        {
                            regularflag = regularflag + " or (r.mode=2)";
                        }
                        else
                        {
                            regularflag = regularflag + " and ((r.mode=2)";
                        }
                        //ViewState["strvar"] = ViewState["strvar"] + " and (mode=2)";
                    }
                }
            }
            mtrdr.Close();
            mysql.Close();
            if (strdayflag != null && strdayflag != "")
            {
                strdayflag = strdayflag + ")";
            }
            ViewState["strvar"] = strdayflag;
            if (regularflag != "")
            {
                regularflag = regularflag + ")";
            }
            if (genderflag != "")
            {
                genderflag = genderflag + ")";
            }
            ViewState["strvar"] = ViewState["strvar"] + regularflag + genderflag;

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
            con.Close();
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
        string duration = string.Empty;
        bool first_year = false;
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

    public void bindsec()
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
            ddlsec.Enabled = true;
        }
        else
        {
            ddlsec.Enabled = false;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnprintdirect.Visible = false;
        lblexcl.Visible = false;//added By Srinath 11/2/2013
        txtexcel.Visible = false;
        //  pnl_head_pageset.Visible = false;
        pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
        //bindbranch();
        //bindsem();
        //bindsec();
        load_subject();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnprintdirect.Visible = false;
        lblexcl.Visible = false;//added By Srinath 11/2/2013
        txtexcel.Visible = false;
        // pnl_head_pageset.Visible = false;
        pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        load_subject();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnprintdirect.Visible = false;
        lblexcl.Visible = false;//added By Srinath 11/2/2013
        txtexcel.Visible = false;
        //  pnl_head_pageset.Visible = false;
        pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
        bindsem();
        bindsec();
        load_subject();
    }

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnprintdirect.Visible = false;
        lblexcl.Visible = false;//added By Srinath 11/2/2013
        txtexcel.Visible = false;
        //   pnl_head_pageset.Visible = false;
        pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
        bindsec();
        load_subject();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnprintdirect.Visible = false;
        lblexcl.Visible = false;//added By Srinath 11/2/2013
        txtexcel.Visible = false;
        //   pnl_head_pageset.Visible = false;
        pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
        load_subject();
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnprintdirect.Visible = false;
        lblexcl.Visible = false;//added By Srinath 11/2/2013
        txtexcel.Visible = false;
        //   pnl_head_pageset.Visible = false;
        pnl_pagesetting.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        errlbl.Visible = false;
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnprintdirect.Visible = false;
        lblexcl.Visible = false;//added By Srinath 11/2/2013
        txtexcel.Visible = false;
        //  pnl_head_pageset.Visible = false;
        pnl_pagesetting.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        errlbl.Visible = false;
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnprintdirect.Visible = false;
        lblexcl.Visible = false;//added By Srinath 11/2/2013
        txtexcel.Visible = false;
        //  pnl_head_pageset.Visible = false;
        pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
    }

    public void load_subject()
    {
        string staff_code = string.Empty;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnprintdirect.Visible = false;
        lblexcl.Visible = false;//added By Srinath 11/2/2013
        txtexcel.Visible = false;
        //  pnl_head_pageset.Visible = false;
        pnl_pagesetting.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        errlbl.Visible = false;
        int count_subject = 0;
        has.Clear();
        has.Add("Batch_Year", ddlbatch.SelectedValue.ToString());
        has.Add("DegCode", ddlbranch.SelectedValue.ToString());
        has.Add("Sems", ddlduration.SelectedItem.ToString());
        has.Add("staffcode", Session["Staff_Code"].ToString());
        if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1" || ddlsec.Enabled == false)
        {
            has.Add("sec", "");
        }
        else
        {
            has.Add("sec", ddlsec.SelectedValue.ToString());
        }
        ds_subject = dacc.select_method("single_subjectwise_attnd", has, "sp");
        count_subject = (ds_subject.Tables[0].Rows.Count);
        if (count_subject > 0)
        {
            ddlsubject.Enabled = true;
            ddlsubject.DataSource = ds_subject;
            ddlsubject.DataTextField = "subject_name";
            ddlsubject.DataValueField = "subject_no";
            ddlsubject.DataBind();
            ddlsubject.Items.Insert(0, "--Select--");
            //   ddlsubject.Items.Insert(0, " ");
            has_subtype.Clear();
            for (int i = 0; i < ds_subject.Tables[0].Rows.Count; i++)
            {
                if (!has_subtype.ContainsKey(ds_subject.Tables[0].Rows[i]["subject_no"].ToString()))
                {
                    has_subtype.Add(ds_subject.Tables[0].Rows[i]["subject_no"].ToString(), ds_subject.Tables[0].Rows[i]["subject_type"].ToString());
                }
            }
        }
        else
        {
            ddlsubject.Enabled = false;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            attMinShortagePercentage = txtAttShoratgePecentage.Text.Trim();
            attMinShorage = 0;
            if (!double.TryParse(attMinShortagePercentage, out attMinShorage))
            {
                attMinShorage = 100;
            }

            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1")
            {
                section_lab = string.Empty;
            }
            else
            {
                section_lab = " and l.sections='" + ddlsec.SelectedItem.ToString() + "'";
            }
            btnclick();
            int temp_col = 0;



        }
        catch
        {
        }
    }

    public void btnclick()
    {
        //  
        {
            //=============================0n 9/4/12
            has.Clear();
            has.Add("college_code", Session["collegecode"].ToString());
            has.Add("form_name", "singlesubjectwise_splitup_attnd_report.aspx");
            dsprint = dacc.select_method("PROC_PRINT_MASTER_SETTINGS", has, "sp");
            //===========================================
            Showgrid.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            btnprintdirect.Visible = false;
            lblexcl.Visible = false;//added By Srinath 11/2/2013
            txtexcel.Visible = false;
            //   pnl_head_pageset.Visible = false;
            pnl_pagesetting.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            errlbl.Visible = false;
            date1 = txtFromDate.Text;
            if (date1.Trim() != "")
            {
                string[] split = date1.Split(new Char[] { '/' });
                if (split.GetUpperBound(0) == 2)//-------date valid
                {
                    if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                    {
                        datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                        date2 = txtToDate.Text.ToString();
                        if (date2.Trim() != "")
                        {
                            string[] split1 = date2.Split(new Char[] { '/' });
                            if (split1.GetUpperBound(0) == 2)//--date valid
                            {
                                if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                                {
                                    dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                    dt1 = Convert.ToDateTime(datefrom.ToString());
                                    Session["from_date_time"] = dt1;
                                    dt2 = Convert.ToDateTime(dateto.ToString());
                                    Session["to_date_time"] = dt2;
                                    TimeSpan t = dt2.Subtract(dt1);
                                    long days = t.Days;
                                    Session["days"] = days;
                                    if (days >= 0)//-----check date difference
                                    {
                                        if (ddlsubject.Text != "" && ddlsubject.Text != " " && ddlsubject.Text != "--Select--")
                                        {
                                            load_student();
                                        }
                                        else
                                        {
                                            errlbl.Visible = true;
                                            errlbl.Text = "Select Subject And Then Click Go";
                                        }
                                    }
                                    else
                                    {
                                        tofromlbl.Visible = true;
                                    }
                                }
                                else
                                {
                                    tolbl.Visible = true;
                                    tolbl.Text = "Select Valid To Date";
                                }
                            }
                            else
                            {
                                tolbl.Visible = true;
                                tolbl.Text = "Select Valid To Date";
                            }
                        }
                        else
                        {
                            tolbl.Visible = true;
                            tolbl.Text = "Select To Date";
                        }
                    }
                    else
                    {
                        frmlbl.Visible = true;
                        frmlbl.Text = "Select Valid From Date";
                    }
                }
                else
                {
                    frmlbl.Visible = true;
                    frmlbl.Text = "Select Valid From Date";
                }
            }
            else
            {
                frmlbl.Visible = true;
                frmlbl.Text = "Select From Date";
            }
        }
        //  catch
        {
        }
    }

    public void load_student()
    {
        try
        {

            //======================0n 11/4/12 PRABHA
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();
                if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                {
                    // subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorBottom = Color.White;
                    new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                    new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
                    new_header_string_split = new_header_string.Split(',');
                    //subject_spread.Sheets[0].SheetCorner.RowCount = subject_spread.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
                }
            }
            //=====================================
            //========find holiday
            has.Clear();
            has.Add("from_date", dt1);
            has.Add("to_date", dt2);
            has.Add("degree_code", ddlbranch.SelectedValue.ToString());
            has.Add("sem", ddlduration.SelectedValue.ToString());
            has.Add("coll_code", Session["collegecode"].ToString());
            int iscount = 0;
            holidaycon.Close();
            holidaycon.Open();
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + dt1.ToString() + "' and '" + dt2.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedItem.ToString() + "";
            SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
            SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
            DataSet dsholiday = new DataSet();
            daholiday.Fill(dsholiday);
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            has.Add("iscount", iscount);
            ds_holi = dacc.select_method("HOLIDATE_DETAILS_FINE", has, "sp");
            if (ds_holi.Tables[0].Rows.Count > 0)
            {
                for (int holi = 0; holi < ds_holi.Tables[0].Rows.Count; holi++)
                {
                    if (ds_holi.Tables[0].Rows[holi]["halforfull"].ToString() == "False")
                    {
                        halforfull = "0";
                    }
                    else
                    {
                        halforfull = "1";
                    }
                    if (ds_holi.Tables[0].Rows[holi]["morning"].ToString() == "False")
                    {
                        mng = "0";
                    }
                    else
                    {
                        mng = "1";
                    }
                    if (ds_holi.Tables[0].Rows[holi]["evening"].ToString() == "False")
                    {
                        evng = "0";
                    }
                    else
                    {
                        evng = "1";
                    }
                    holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                    if (!hat_holy.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString())))
                    {
                        hat_holy.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString()), holiday_sched_details);
                    }
                }
            }
            //===========================
            //============================section
            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";
            }
            //==================================

            string orderby_Setting = dacc.GetFunction("select value from master_Settings where settings='order_by'");
            string strorder = "ORDER BY r.roll_no";
            string serialno = dacc.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno == "1")
            {
                strorder = "ORDER BY r.serialno";
            }
            else
            {
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY r.roll_no";
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
                    strorder = "ORDER BY r.roll_no,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY r.roll_no,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY r.roll_no,r.Stud_Name";
                }
            }
            con.Close();
            con.Open();
            //   cmd = new SqlCommand(" select distinct a.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(a.roll_no) FROM attendance a , registration r , Department d ,  PeriodAttndSchedule p  ,seminfo s,applyn app WHERE a.roll_no=r.roll_no and   r.degree_code=p.degree_code and  r.Batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and  s.batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and r.degree_code= " + ddlbranch.SelectedValue.ToString() + " and s.degree_code= " + ddlbranch.SelectedValue.ToString() + " and  s.semester=" + ddlduration.SelectedValue.ToString() + " and p.semester=" + ddlduration.SelectedValue.ToString() + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL) and  r.app_no=app.app_no " + strsec + "  " + ViewState["strvar"] + " order by len(a.roll_no)  ", con);//=0n 11/4/12
            //  cmd = new SqlCommand(" select distinct r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(a.roll_no), convert(varchar(15),adm_date,103) as adm_date FROM attendance a , registration r , Department d ,  PeriodAttndSchedule p  ,seminfo s,applyn app WHERE a.roll_no=r.roll_no and   r.degree_code=p.degree_code and  r.Batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and  s.batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and r.degree_code= " + ddlbranch.SelectedValue.ToString() + " and s.degree_code= " + ddlbranch.SelectedValue.ToString() + " and  s.semester=" + ddlduration.SelectedValue.ToString() + " and p.semester=" + ddlduration.SelectedValue.ToString() + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL) and  r.app_no=app.app_no " + strsec + "  " + ViewState["strvar"] + " order by r.roll_no  ", con);//=0n 11/4/12


            includediscon = " and r.delflag=0";
            includedebar = " and r.exam_flag <> 'DEBAR'";
            string getshedulockva = dacc.GetFunctionv("select value from Master_Settings where  settings='Attendance Discount'  " + grouporusercode1 + " ");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                includediscon = string.Empty;
            getshedulockva = dacc.GetFunctionv("select value from Master_Settings where   settings='Attendance Debar'  " + grouporusercode1 + "");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                includedebar = string.Empty;

            cmd = new SqlCommand(" select distinct r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(a.roll_no), convert(varchar(15),adm_date,103) as adm_date,r.serialno,r.delflag,r.Exam_Flag FROM attendance a , registration r , Department d ,  PeriodAttndSchedule p  ,seminfo s,applyn app,subjectchooser sc WHERE a.roll_no=r.roll_no and sc.roll_no=r.roll_no and sc.subject_no='" + ddlsubject.SelectedValue.ToString() + "' and r.degree_code=p.degree_code and  r.Batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and  s.batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and r.degree_code= " + ddlbranch.SelectedValue.ToString() + " and s.degree_code= " + ddlbranch.SelectedValue.ToString() + " and  s.semester=" + ddlduration.SelectedValue.ToString() + " and p.semester=" + ddlduration.SelectedValue.ToString() + "  and (r.CC = 0)  " + includediscon + "  " + includedebar + " AND (r.Current_Semester IS NOT NULL) and  r.app_no=app.app_no " + strsec + "  " + ViewState["strvar"] + " " + strorder + "  ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds_student);
            stud_count = ds_student.Tables[0].Rows.Count;
            if (stud_count > 0)
            {
                Showgrid.Visible = true;
                btnxl.Visible = true;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = true;
                btnprintdirect.Visible = false;
                lblexcl.Visible = true;//added By Srinath 11/2/2013
                txtexcel.Visible = true;
                //   pnl_head_pageset.Visible = true;
                pnl_pagesetting.Visible = true;
                // subject_spread.Sheets[0].ColumnHeader.RowCount = subject_spread.Sheets[0].ColumnHeader.RowCount + 2;//Hidden By SRinath 15/5/2013
                colcount = 0;
                arrColHdrNames1.Add("S.No");
                data.Columns.Add("col0");

                if (Session["Rollflag"].ToString() == "1")
                {

                    colcount++;
                    arrColHdrNames1.Add("Roll No");
                    data.Columns.Add("col" + colcount);
                }

                if (Session["Regflag"].ToString() == "1")
                {
                    colcount++;
                    arrColHdrNames1.Add("Reg No");
                    data.Columns.Add("col" + colcount);
                }
                colcount++;
                arrColHdrNames1.Add("Name of The Students");
                data.Columns.Add("col" + colcount);

                colcount = colcount + 1;


                colHdrIndx = colcount;
                DataRow drHdr1 = data.NewRow();

                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    drHdr1["col" + grCol] = arrColHdrNames1[grCol];

                data.Rows.Add(drHdr1);


                dicstd.Clear();
                dicstdroll.Clear();
                no_of_hrs = int.Parse(ds_student.Tables[0].Rows[0]["PER DAY"].ToString());
                order = ds_student.Tables[0].Rows[0]["order"].ToString();
                sem_start_date = ds_student.Tables[0].Rows[0]["start_date"].ToString();
                mng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
                evng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                if (no_of_hrs > 0)
                {
                    int snocnt = 0;
                    for (temp_stud_count = 0; temp_stud_count < stud_count; temp_stud_count++)
                    {
                        snocnt++;
                        drow = data.NewRow();
                        data.Rows.Add(drow);
                        for (int c = 0; c < colcount; c++)
                        {
                            //data.Rows[data.Rows.Count - 1][c] = Convert.ToString(snocnt);
                            if (Convert.ToString(Session["Rollflag"]) == "1")
                            {
                                c++;
                                data.Rows[data.Rows.Count - 1][c] = Convert.ToString(ds_student.Tables[0].Rows[temp_stud_count]["ROLL NO"]);


                            }
                            if (Convert.ToString(Session["Regflag"]) == "1")
                            {
                                c++;
                                data.Rows[data.Rows.Count - 1][c] = Convert.ToString(ds_student.Tables[0].Rows[temp_stud_count]["REG NO"]);

                            }

                            c++;
                            data.Rows[data.Rows.Count - 1][c] = Convert.ToString(ds_student.Tables[0].Rows[temp_stud_count]["STUD NAME"]);
                            string del = ds_student.Tables[0].Rows[temp_stud_count]["delflag"].ToString();
                            string examf = ds_student.Tables[0].Rows[temp_stud_count]["Exam_Flag"].ToString();
                            if (del == "1" || examf.ToUpper() == "DEBAR")
                            {
                                dicdisCon.Add(data.Rows.Count - 1, ds_student.Tables[0].Rows[temp_stud_count]["ROLL NO"].ToString());
                            }


                        }




                        string admdate = ds_student.Tables[0].Rows[temp_stud_count]["adm_date"].ToString();
                        string[] admdatesp = admdate.Split(new Char[] { '/' });
                        admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();



                        dicstd.Add(ds_student.Tables[0].Rows[temp_stud_count]["ROLL NO"].ToString(), admdate.ToString());
                        has_load_rollno.Add(ds_student.Tables[0].Rows[temp_stud_count]["ROLL NO"].ToString(), 0);
                        has_total_attnd_hour.Add(ds_student.Tables[0].Rows[temp_stud_count]["ROLL NO"].ToString(), 0);

                        if (!hatsplhrattendance.Contains(ds_student.Tables[0].Rows[temp_stud_count]["ROLL NO"].ToString()))
                        {
                            hatsplhrattendance.Add(ds_student.Tables[0].Rows[temp_stud_count]["ROLL NO"].ToString(), temp_stud_count);
                        }

                        dicstdroll.Add(snocnt, ds_student.Tables[0].Rows[temp_stud_count]["ROLL NO"].ToString());
                    }
                    load_attendance();
                }
                else
                {
                    errlbl.Visible = true;
                    errlbl.Text = "Update Master Setting";
                }
            }
            else
            {
                errlbl.Visible = true;
                errlbl.Text = "No Student(s) Available";
            }
        }
        catch
        {


        }
    }

    public void load_attendance()
    {
        try
        {

            //added By Srinath 14/8/2013
            Hashtable hatattendance = new Hashtable();
            Dictionary<DateTime, byte> dicAlternateDayOrder = new Dictionary<DateTime, byte>();//magesh 3.9.18
            string orderby_Setting = dacc.GetFunction("select value from master_Settings where settings='order_by'");
            string strorder = "ORDER BY r.roll_no";
            string serialno = dacc.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno == "1")
            {
                strorder = "ORDER BY r.serialno";
            }
            else
            {
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY r.roll_no";
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
                    strorder = "ORDER BY r.roll_no,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY r.roll_no,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY r.roll_no,r.Stud_Name";
                }
            }
            string splhrsec = string.Empty;
            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1")
            {
                splhrsec = string.Empty;
            }
            else
            {
                splhrsec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
            }
            //added By srinath 18/2/2013 ==STart
            string[] fromdatespit = txtFromDate.Text.Split('/');
            string[] todatespit = txtToDate.Text.Split('/');
            DateTime spfromdate = Convert.ToDateTime(fromdatespit[1] + '/' + fromdatespit[0] + '/' + fromdatespit[2]);
            DateTime sptodate = Convert.ToDateTime(todatespit[1] + '/' + todatespit[0] + '/' + todatespit[2]);
            ht_sphr.Clear();
            string hrdetno = string.Empty;
            string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "' " + splhrsec + "";
            ds_sphr = dacc.select_method(getsphr, hat, "Text");
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
            //== End
            string temp_tag = "", rstrsec = string.Empty;
            // string[] final_date_string=new string[2];
            temp_date = dt1;
            subject_no = ddlsubject.SelectedValue.ToString();
            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1")
            {
                strsec = string.Empty;
                rstrsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                rstrsec = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";
            }
            string currlabsub = "select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester from syllabus_master sy,sub_sem sm,subject s where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and sm.Lab=1 and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sy.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sy.semester='" + ddlduration.SelectedValue.ToString() + "' order by sy.Batch_Year,sy.degree_code,sy.semester";
            DataSet dscurrlab = dacc.select_method_wo_parameter(currlabsub, "Text");
            Hashtable hatlab = new Hashtable();
            for (int l = 0; l < dscurrlab.Tables[0].Rows.Count; l++)
            {
                string strsub = dscurrlab.Tables[0].Rows[l]["subject_no"].ToString();
                if (!hatlab.Contains(strsub))
                {
                    hatlab.Add(strsub, strsub);
                }
            }
            //==================================
            //Added by Srinath 5/9/2014=========Start==========================
            string getdeteails = "select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester='" + ddlduration.SelectedItem.ToString() + "' and s.batch_year='" + ddlbatch.Text.ToString() + "'  and s.degree_code='" + ddlbranch.SelectedValue.ToString() + "'";
            getdeteails = getdeteails + " ; select * from tbl_consider_day_order where semester='" + ddlduration.SelectedItem.ToString() + "' and batch_year='" + ddlbatch.Text.ToString() + "'  and degree_code='" + ddlbranch.SelectedValue.ToString() + "'";
            DataSet dssem = dacc.select_method_wo_parameter(getdeteails, "Text");
            string semstartdate = string.Empty;
            string noofdays = string.Empty;
            string startday = string.Empty;
            if (dssem.Tables[0].Rows.Count > 0)
            {
                semstartdate = dssem.Tables[0].Rows[0]["start_date"].ToString();
                noofdays = dssem.Tables[0].Rows[0]["nodays"].ToString();
                startday = dssem.Tables[0].Rows[0]["starting_dayorder"].ToString();
            }

            Boolean staffSelector = false;
            string minimumabsentsms = dacc.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + collegecode + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(ddlbatch.Text.ToString()) >= batchyearsetting)
                    {
                        staffSelector = true;
                    }
                }
            }
            else if (splitminimumabsentsms.Length > 0)
            {
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }

            Hashtable hatdc = new Hashtable();
            try
            {
                for (int dc = 0; dc < dssem.Tables[1].Rows.Count; dc++)
                {
                    DateTime dtdcf = Convert.ToDateTime(dssem.Tables[1].Rows[dc]["from_date"].ToString());
                    DateTime dtdct = Convert.ToDateTime(dssem.Tables[1].Rows[dc]["to_date"].ToString());
                    string asperday = Convert.ToString(dssem.Tables[1].Rows[dc]["include_attendance"].ToString());
                    string alternateDayOrder = Convert.ToString(dssem.Tables[1].Rows[dc]["DayOrder"]).Trim();
                    byte alternateDay = 0;
                    byte.TryParse(alternateDayOrder, out alternateDay);
                    for (DateTime dtc = dtdcf; dtc <= dtdct; dtc = dtc.AddDays(1))
                    {
                        if (asperday != "1") //magesh 3.9.18
                        {
                            if (!hatdc.Contains(dtc))
                            {
                                hatdc.Add(dtc, dtc);
                            }
                        }
                        else
                        {
                            //magesh 3.9.18
                            if (!dicAlternateDayOrder.ContainsKey(dtc))
                            {
                                dicAlternateDayOrder.Add(dtc, alternateDay);
                            } //magesh 3.9.18
                        }
                    }
                }
            }
            catch
            {
            }
            //====================End=====================================================
            //modified By Srinath 22/2/2013 
            if (chkflag == false)
            {
                chkflag = true;
                //Hashtable has_attnd_masterset_notconsider = new Hashtable();
                //---------------get calcflag
                has.Clear();
                has.Add("colege_code", Session["collegecode"].ToString());
                ds_attndmaster = dacc.select_method("ATT_MASTER_SETTING", has, "sp");
                count_master = (ds_attndmaster.Tables[0].Rows.Count);
                if (count_master > 0)
                {
                    for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                    {
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                        {
                            has_attnd_masterset.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "2")
                        {
                            if (!has_attnd_masterset_notconsider.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                            {
                                has_attnd_masterset_notconsider.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                            }
                        }
                    }
                }
                //Added By Srinath 21/8/2013
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                //bool splhr_flag = false;
                //=====================================14/5/12 PRABHA
                con.Close();
                cmd.CommandText = "select rights from  special_hr_rights where " + grouporusercode + "";
                cmd.Connection = con;
                con.Open();
                SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
                if (dr_rights_spl_hr.HasRows)
                {
                    while (dr_rights_spl_hr.Read())
                    {
                        string spl_hr_rights = string.Empty;
                        Hashtable od_has = new Hashtable();
                        spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                        if (spl_hr_rights == "True" || spl_hr_rights == "true")
                        {
                            splhr_flag = true;
                            //getspecial_hr();
                        }
                    }
                }
            }
            //===================================
            string subj_type = GetFunction("select subject_type From sub_sem where subtype_no=(select subtype_no from subject where  subject_no='" + subject_no + "')");
            while (temp_date <= dt2)
            {
                if (!hatdc.Contains(temp_date))//Added by srinath 5/9/2014 for day order change
                {
                    if (splhr_flag == true)
                    {    //modified By Srinath 18/2/2013 ==start
                        if (ht_sphr.Contains(Convert.ToString(temp_date)))
                        {
                            getspecial_hr();
                        }
                    }
                    span_count = 0;
                    //--------------------------
                    if (!hat_holy.ContainsKey(temp_date))
                    {
                        if (!hat_holy.ContainsKey(temp_date))
                        {
                            hat_holy.Add(temp_date, "3*0*0");
                        }
                    }
                    value_holi_status = GetCorrespondingKey(temp_date, hat_holy).ToString();
                    split_holiday_status = value_holi_status.Split('*');
                    if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                    {
                        split_holiday_status_1 = 1;
                        split_holiday_status_2 = no_of_hrs;
                    }
                    else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                    {
                        if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                        {
                            split_holiday_status_1 = mng_hrs + 1;
                            split_holiday_status_2 = no_of_hrs;
                        }
                        if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                        {
                            split_holiday_status_1 = 1;
                            split_holiday_status_2 = mng_hrs;
                        }
                    }
                    else if (split_holiday_status[0].ToString() == "0")//=================fulday holiday
                    {
                        split_holiday_status_1 = 0;
                        split_holiday_status_2 = 0;
                    }
                    //------------------------------
                    if (split_holiday_status_1 == 0 && split_holiday_status_2 == 0)
                    {
                        //  temp_date = temp_date.AddDays(1);//Hidden by srinath 11/9/2014
                    }
                    else
                    {
                        DataSet dsalterdet = new DataSet();
                        ds_alter.Clear();
                        //---------------alternate schedule
                        con.Close();
                        con.Open();
                        cmd_alt_shed = new SqlCommand("select  * from alternate_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and FromDate ='" + temp_date + "' " + strsec + " order by FromDate Desc", con);
                        SqlDataAdapter da_alter = new SqlDataAdapter(cmd_alt_shed);
                        ds_alter.Clear();
                        da_alter.Fill(ds_alter);

                        string alterquery1 = "select  * from AlternateDetails where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and AlternateDate ='" + temp_date + "' " + strsec + " order by AlternateDate Desc";
                        dsalterdet = dacc.select_method(alterquery1, hat, "Text");


                        //---------------------------------------------
                        ds.Clear();
                        con.Close();
                        con.Open();
                        cmd_sem_shed = new SqlCommand("select top 1 * from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc", con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd_sem_shed);
                        da.Fill(ds);
                        hatattendance.Clear();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (no_of_hrs > 0)
                            {
                                dummy_date = temp_date.ToString();
                                string[] dummy_date_split = dummy_date.Split(' ');
                                string[] final_date_string = dummy_date_split[0].Split('/');
                                dummy_date = final_date_string[1].ToString() + "/" + final_date_string[0].ToString() + "/" + final_date_string[2].ToString();
                                month_year = ((Convert.ToInt16(final_date_string[2].ToString()) * 12) + (Convert.ToInt16(final_date_string[0].ToString()))).ToString();
                                if (order != "0")
                                {
                                    strDay = temp_date.ToString("ddd");
                                }
                                else
                                {
                                    //Modified b y srinath 5/9/2014
                                    // strDay = findday(no_of_hrs, sem_start_date.ToString(), dummy_date);
                                    string[] sp = dummy_date.Split('/');
                                    string curdate = sp[1] + '/' + sp[0] + '/' + sp[2];
                                    strDay = dacc.findday(curdate, ddlbranch.SelectedValue.ToString(), ddlduration.SelectedItem.ToString(), ddlbatch.Text.ToString(), semstartdate, noofdays, startday);
                                    //magesh 3.9.18
                                    if (dicAlternateDayOrder.ContainsKey(temp_date))
                                    {
                                        strDay = dacc.findDayName(dicAlternateDayOrder[temp_date]);
                                        string Day_Order = Convert.ToString(dicAlternateDayOrder[temp_date]).Trim();
                                    } //magesh 3.9.18
                                }
                                for (int temp_hr = split_holiday_status_1; temp_hr <= split_holiday_status_2; temp_hr++)
                                {
                                    bool samehr_flag = false;
                                    roll_count = 0;
                                    present_count = 0;
                                    temp_hr_field = strDay + temp_hr;
                                    date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;
                                    hatattendance.Clear();
                                    string hour = ds.Tables[0].Rows[0][temp_hr_field].ToString();
                                    DataView dvstaffdetails = new DataView();
                                    string single_hour2 = "";
                                    Boolean checalter = false;
                                    if (hour != "")
                                    {
                                        string[] split_full_hour1 = hour.Split(';');
                                        for (int g1 = 0; g1 <= split_full_hour1.GetUpperBound(0); g1++)
                                        {

                                            string single_hour1 = split_full_hour1[g1].ToString();
                                            string[] split_single_hour1 = single_hour1.Split('-');

                                            if (split_single_hour1.GetUpperBound(0) >= 1)
                                            {
                                                dsalterdet.Tables[0].DefaultView.RowFilter = "ActSubNo='" + split_single_hour1[0].ToString() + "' and AlterHour='" + temp_hr + "' ";
                                                DataView dvstaffdetails1 = dsalterdet.Tables[0].DefaultView;

                                                //ds_alter.Tables[0].DefaultView.RowFilter=
                                                if (!checalter)
                                                {
                                                    if (dvstaffdetails1.Count > 0)
                                                    {
                                                        if (single_hour2 == "")
                                                            single_hour2 = ds_alter.Tables[0].Rows[0][temp_hr_field].ToString();
                                                        else
                                                            single_hour2 = single_hour2 + ";" + ds_alter.Tables[0].Rows[0][temp_hr_field].ToString();

                                                        // single_hour2 = ds_alter.Tables[0].Rows[0][temp_hr_field].ToString();
                                                        check_alter = false;
                                                        checalter = true;
                                                    }
                                                    else
                                                    {
                                                        check_alter = false;

                                                        if (single_hour2 == "")
                                                            single_hour2 = single_hour1;
                                                        else
                                                            single_hour2 = single_hour2 + ";" + single_hour1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (check_alter)
                                    {

                                        if (ds_alter.Tables[0].Rows.Count > 0)
                                        {
                                            for (int hasrow = 0; hasrow < ds_alter.Tables[0].Rows.Count; hasrow++)
                                            {
                                                full_hour = ds_alter.Tables[0].Rows[hasrow][temp_hr_field].ToString();
                                                if (full_hour.Trim() != "")
                                                {
                                                    temp_has_subj_code.Clear();
                                                    string[] split_full_hour = full_hour.Split(';');
                                                    //===============================================
                                                    bool batchflag = false;
                                                    for (int g = 0; g <= split_full_hour.GetUpperBound(0); g++)
                                                    {
                                                        string[] valhr = split_full_hour[g].ToString().Split('-');
                                                        if (valhr.GetUpperBound(0) > 1)
                                                        {
                                                            string lsub = valhr[0].ToString();
                                                            if (hatlab.Contains(lsub))
                                                            {
                                                                batchflag = true;
                                                            }
                                                        }
                                                    }
                                                    //===============================================
                                                    for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                                    {
                                                        roll_count = 0;
                                                        single_hour = split_full_hour[semi_colon].ToString();
                                                        string[] split_single_hour = single_hour.Split('-');
                                                        //Modified By Srinath 1/6/2013
                                                        // if (split_single_hour.GetUpperBound(0) == 2 || split_single_hour.GetUpperBound(0) == 3)

                                                        if (split_single_hour.GetUpperBound(0) >= 1)
                                                        {

                                                            check_alter = true;
                                                            if (split_single_hour[0].ToString() == subject_no)
                                                            {
                                                                if (!temp_has_subj_code.ContainsKey(subject_no))
                                                                {
                                                                    temp_has_subj_code.Add(subject_no, subject_no);
                                                                    //----------------------check lab allocation
                                                                    recflag = true;
                                                                    roll_count = 0;
                                                                    if (samehr_flag == false)
                                                                    {
                                                                        span_count++;
                                                                        //  subject_spread.Sheets[0].ColumnCount++;
                                                                        // subject_spread.Sheets[0].Columns[subject_spread.Sheets[0].ColumnCount - 1].Visible = false;
                                                                        samehr_flag = true;
                                                                    }

                                                                    //------------------------attendance
                                                                    con_attnd.Close();
                                                                    con_attnd.Open();
                                                                    con_attnd1.Close();
                                                                    con_attnd1.Open();
                                                                    SqlDataReader dr;
                                                                    SqlDataReader dr1;
                                                                    Hashtable has_stud_list = new Hashtable();
                                                                    //------------------find subject type
                                                                    //==========================Modified by srinath 30/5/2014===========
                                                                    // subj_type = GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                                                                    if (batchflag == false)
                                                                    {
                                                                        subj_type = "0";
                                                                    }
                                                                    else
                                                                    {
                                                                        subj_type = "1";
                                                                    }

                                                                    includediscon = " and r.delflag=0";
                                                                    includedebar = " and r.exam_flag <> 'DEBAR'";
                                                                    string getshedulockva = dacc.GetFunctionv("select value from Master_Settings where  settings='Attendance Discount'  " + grouporusercode1 + " ");
                                                                    if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                                                                        includediscon = string.Empty;
                                                                    getshedulockva = dacc.GetFunctionv("select value from Master_Settings where   settings='Attendance Debar'  " + grouporusercode1 + "");
                                                                    if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                                                                        includedebar = string.Empty;

                                                                    //====================
                                                                    if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type != "true")
                                                                    {

                                                                        string strquery = "select distinct r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a  where r.roll_no=a.roll_no and s.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0  " + includediscon + "  " + includedebar + " and r.roll_no=s.roll_no and s.semester= " + ddlduration.SelectedItem.ToString() + " " + strsec + " and  subject_no=" + subject_no + " " + strorder + "";
                                                                        DataSet dsquery = dacc.select_method(strquery, hat, "Text");
                                                                        for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                        {
                                                                            string rollno = dsquery.Tables[0].Rows[i]["roll_no"].ToString().Trim();
                                                                            if (!hatattendance.Contains(rollno))
                                                                            {
                                                                                hatattendance.Add(rollno, dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {

                                                                        string strquery = "select distinct  r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser_New s,laballoc_new l,attendance a  where a.roll_no=s.roll_no and r.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedItem.ToString() + " and cc=0  " + includediscon + "  " + includedebar + "  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no=" + subject_no + " and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester and s.subject_no =l.subject_no and hour_value=" + temp_hr + "  and day_value='" + strDay + "' and l.subject_no=" + subject_no + " and s.batch=l.stu_batch " + section_lab + " and FromDate ='" + temp_date + "' and l.fdate=s.fromdate " + strorder + "";
                                                                        DataSet dsquery = dacc.select_method(strquery, hat, "Text");
                                                                        for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                        {
                                                                            string rollno = dsquery.Tables[0].Rows[i]["roll_no"].ToString().Trim();
                                                                            if (!hatattendance.Contains(rollno))
                                                                            {
                                                                                hatattendance.Add(rollno, dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                            }
                                                                        }
                                                                    }
                                                                    if (hatattendance.Count > 0)
                                                                    {

                                                                        if (dicstd.Count > 0)
                                                                        {
                                                                            foreach (KeyValuePair<string, string> drval in dicstd)
                                                                            {
                                                                                string rollno = drval.Key;
                                                                                string stdadmdate = drval.Value;

                                                                                if (hatattendance.Contains(rollno.ToString()))
                                                                                {
                                                                                    Admission_date = Convert.ToDateTime(stdadmdate.Trim());
                                                                                    string attvalue = GetCorrespondingKey(rollno, hatattendance).ToString();
                                                                                    string value = Attmark(attvalue.ToString());
                                                                                    if (temp_date >= Admission_date)
                                                                                    {
                                                                                        if (attvalue == "3")
                                                                                        {
                                                                                            // subject_spread.Sheets[0].Cells[roll_count, (subject_spread.Sheets[0].ColumnCount - 1)].Tag = "3";
                                                                                            temp_tag = "3";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //   subject_spread.Sheets[0].Cells[roll_count, (subject_spread.Sheets[0].ColumnCount - 1)].Tag = "0";
                                                                                            temp_tag = "0";
                                                                                        }

                                                                                        if ((attvalue.ToString()) != "8")
                                                                                        {
                                                                                            if (value != "HS") //'Aruna 21may20123/7/12 PRABHA
                                                                                            {
                                                                                                if (!has_attnd_masterset_notconsider.ContainsKey(attvalue.ToString()))//==31/5/12 PRABHA
                                                                                                {
                                                                                                    if (temp_tag == "0")
                                                                                                    {
                                                                                                        if (has_attnd_masterset.ContainsKey(attvalue))
                                                                                                        {
                                                                                                            present_count = Convert.ToInt16(GetCorrespondingKey(rollno, has_load_rollno));
                                                                                                            present_count++;
                                                                                                            has_load_rollno[rollno] = present_count;
                                                                                                        }
                                                                                                        if (value != "NE")
                                                                                                        {
                                                                                                            present_count = Convert.ToInt16(GetCorrespondingKey(rollno, has_total_attnd_hour));
                                                                                                            present_count++;
                                                                                                            has_total_attnd_hour[rollno] = present_count;
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (!has_od.ContainsKey(rollno))
                                                                                                        {
                                                                                                            has_od.Add(rollno, 1);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            od_count = Convert.ToInt16(GetCorrespondingKey(rollno, has_od));
                                                                                                            od_count++;
                                                                                                            has_od[rollno] = od_count;
                                                                                                        }
                                                                                                        if (value != "NE")
                                                                                                        {
                                                                                                            // if (subject_spread.Sheets[0].Cells[roll_count, (subject_spread.Sheets[0].ColumnCount - 1)].Tag == "0")
                                                                                                            {
                                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(rollno, has_total_attnd_hour));
                                                                                                                present_count++;
                                                                                                                has_total_attnd_hour[rollno] = present_count;
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
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }


                                    samehr_flag = false;
                                    roll_count = 0;
                                    present_count = 0;
                                    if (check_alter == false)
                                    {
                                        full_hour = single_hour2;
                                        if (full_hour.Trim() != "")
                                        {
                                            temp_has_subj_code.Clear();

                                            string[] split_full_hour_sem = full_hour.Split(';');
                                            //===============================================
                                            bool batchflag = false;
                                            for (int g = 0; g <= split_full_hour_sem.GetUpperBound(0); g++)
                                            {
                                                string[] valhr = split_full_hour_sem[g].ToString().Split('-');
                                                if (valhr.GetUpperBound(0) > 1)
                                                {
                                                    string lsub = valhr[0].ToString();
                                                    if (hatlab.Contains(lsub))
                                                    {
                                                        batchflag = true;
                                                    }
                                                }
                                            }
                                            //===============================================
                                            for (int semi_colon = 0; semi_colon <= split_full_hour_sem.GetUpperBound(0); semi_colon++)
                                            {
                                                roll_count = 0;
                                                single_hour = split_full_hour_sem[semi_colon].ToString();
                                                string[] split_single_hour = single_hour.Split('-');
                                                //Modified By Srinath 1/6/2013
                                                // if (split_single_hour.GetUpperBound(0) == 2 || split_single_hour.GetUpperBound(0) == 3)
                                                if (split_single_hour.GetUpperBound(0) >= 1)
                                                {
                                                    if (split_single_hour[0].ToString() == subject_no)
                                                    {
                                                        if (!temp_has_subj_code.ContainsKey(subject_no))
                                                        {
                                                            temp_has_subj_code.Add(subject_no, subject_no);
                                                            recflag = true;
                                                            if (samehr_flag == false)
                                                            {
                                                                span_count++;
                                                                //  subject_spread.Sheets[0].ColumnCount++;
                                                                //  subject_spread.Sheets[0].Columns[subject_spread.Sheets[0].ColumnCount - 1].Visible = false;
                                                                samehr_flag = true;
                                                            }

                                                            //------------------------attendance
                                                            con_attnd.Close();
                                                            con_attnd.Open();
                                                            con_attnd1.Close();//---------30/6/12 PRABAH
                                                            con_attnd1.Open();
                                                            SqlDataReader dr;
                                                            SqlDataReader dr1;//---------30/6/12 PRABAH
                                                            Hashtable has_stud_list = new Hashtable();
                                                            //------------------find subject type
                                                            //  subj_type = GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                                                            if (batchflag == true)
                                                            {
                                                                subj_type = "1";
                                                            }
                                                            else
                                                            {
                                                                subj_type = "0";
                                                            }
                                                            //====================
                                                            includediscon = " and r.delflag=0";
                                                            includedebar = " and r.exam_flag <> 'DEBAR'";
                                                            string getshedulockva = dacc.GetFunctionv("select value from Master_Settings where  settings='Attendance Discount'  " + grouporusercode1 + " ");
                                                            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                                                                includediscon = string.Empty;
                                                            getshedulockva = dacc.GetFunctionv("select value from Master_Settings where   settings='Attendance Debar'  " + grouporusercode1 + "");
                                                            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                                                                includedebar = string.Empty;
                                                            if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type != "true")
                                                            {

                                                                string strquery = "select distinct r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0   " + includediscon + "  " + includedebar + " and r.roll_no=s.roll_no and s.semester= " + ddlduration.SelectedItem.ToString() + " " + strsec + " and  subject_no=" + subject_no + " " + strorder + "";
                                                                DataSet dsquery = dacc.select_method(strquery, hat, "Text");
                                                                for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                {
                                                                    string rollno = dsquery.Tables[0].Rows[i]["Roll_no"].ToString().Trim();
                                                                    if (!hatattendance.Contains(rollno))
                                                                    {
                                                                        hatattendance.Add(rollno, dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {

                                                                string strquery = "select distinct r.roll_no,a." + date_temp_field + " AS attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser s,laballoc l,attendance a  where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedItem.ToString() + " and cc=0  " + includediscon + "  " + includedebar + " " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no=" + subject_no + " and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester and s.subject_no =l.subject_no and s.batch=l.stu_batch and hour_value=" + temp_hr + "  and day_value='" + strDay + "' and l.subject_no=" + subject_no + " " + section_lab + " " + strorder + "";
                                                                //and s.batch=l.stu_batch 
                                                                DataSet dsquery = dacc.select_method(strquery, hat, "Text");
                                                                for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                {
                                                                    string rollno = dsquery.Tables[0].Rows[i]["Roll_no"].ToString().Trim();
                                                                    if (!hatattendance.Contains(rollno.ToString()))
                                                                    {
                                                                        hatattendance.Add(rollno.ToString(), dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                    }
                                                                }
                                                            }
                                                            if (hatattendance.Count > 0)
                                                            {

                                                                if (dicstd.Count > 0)
                                                                {
                                                                    foreach (KeyValuePair<string, string> drval in dicstd)
                                                                    {
                                                                        string rollno = drval.Key;
                                                                        string stdadmdate = drval.Value;

                                                                        if (hatattendance.Contains(rollno.ToString()))
                                                                        {
                                                                            Admission_date = Convert.ToDateTime(stdadmdate.Trim());
                                                                            string attvalue = GetCorrespondingKey(rollno, hatattendance).ToString();
                                                                            string value = Attmark(attvalue.ToString());
                                                                            if (temp_date >= Admission_date)
                                                                            {
                                                                                //subject_spread.Sheets[0].Rows[i].Visible = true;
                                                                                if (attvalue == "3")
                                                                                {
                                                                                    temp_tag = "3";
                                                                                }
                                                                                else
                                                                                {
                                                                                    temp_tag = "0";
                                                                                }
                                                                                if ((attvalue.ToString()) != "8")
                                                                                {
                                                                                    if (value != "HS") //'Aruna 21may20123/7/12 PRABHA
                                                                                    {
                                                                                        if (temp_tag == "0")
                                                                                        {
                                                                                            if (has_attnd_masterset.ContainsKey(attvalue))
                                                                                            {
                                                                                                //  if (subject_spread.Sheets[0].Cells[roll_count, (subject_spread.Sheets[0].ColumnCount - 1)].Tag == "0")
                                                                                                if (temp_tag == "0")
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(GetCorrespondingKey(rollno, has_load_rollno));
                                                                                                    present_count++;
                                                                                                    has_load_rollno[rollno] = present_count;
                                                                                                }
                                                                                            }
                                                                                            if (value != "NE")
                                                                                            {
                                                                                                // if (subject_spread.Sheets[0].Cells[roll_count, (subject_spread.Sheets[0].ColumnCount - 1)].Tag == "0")
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(GetCorrespondingKey(rollno, has_total_attnd_hour));
                                                                                                    present_count++;
                                                                                                    has_total_attnd_hour[rollno] = present_count;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (!has_od.ContainsKey(rollno))
                                                                                            {
                                                                                                has_od.Add(rollno, 1);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                od_count = Convert.ToInt16(GetCorrespondingKey(rollno, has_od));
                                                                                                od_count++;
                                                                                                has_od[rollno] = od_count;
                                                                                            }
                                                                                            if (value != "NE")
                                                                                            {
                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(rollno, has_total_attnd_hour));
                                                                                                present_count++;
                                                                                                has_total_attnd_hour[rollno] = present_count;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    check_alter = false;
                                }
                            }
                        }
                    }
                }//Added by srinath for day order change
                temp_date = temp_date.AddDays(1);

            }
            if (recflag == true)
            {
                max_tot = 0;
                attnd_hr = 0;
                tot_hr = 0;

                int colmcnt = colcount;
                data.Columns.Add("col" + colmcnt);
                data.Rows[0][colmcnt] = "Conducted Periods";

                colmcnt++;
                data.Columns.Add("col" + colmcnt);
                data.Rows[0][colmcnt] = "Periods Present";

                colmcnt++;
                data.Columns.Add("col" + colmcnt);
                data.Rows[0][colmcnt] = "OD Periods";

                colmcnt++;
                data.Columns.Add("col" + colmcnt);
                data.Rows[0][colmcnt] = "Total Periods";

                colmcnt++;
                data.Columns.Add("col" + colmcnt);
                data.Rows[0][colmcnt] = "Periods Absent";

                colmcnt++;
                data.Columns.Add("col" + colmcnt);
                data.Rows[0][colmcnt] = "% of Attendance";



                int sno = 0;//Added by Srinath 21/8/2013
                int countInvisible = 0;
                bool check_row_visible = false;
                int rowValue = 0;
                if (dicstd.Count > 0)
                {
                    int row_cnt = 1;
                    bool rowvis = true;
                    foreach (KeyValuePair<string, string> drval in dicstd)
                    {
                        data.Rows[row_cnt][0] = row_cnt;
                        int c = colcount;
                        string stdadmdate = drval.Value;


                        bool check_flag = false;
                        attnd_hr = 0;
                        roll_no = drval.Key;
                        temp_date = dt1;
                        totsplhr = 0;
                        atndsplhr = 0;
                        abssplhr = 0;
                        odspl = 0;
                        while (temp_date <= dt2)
                        {
                            if (splhr_flag == true)
                            {    //modified By Srinath 18/2/2013 ==start
                                if (ht_sphr.Contains(Convert.ToString(temp_date)))
                                {
                                    getspecial_hr1();
                                }
                            }
                            temp_date = temp_date.AddDays(1);
                        }
                        if (has_load_rollno.Contains(roll_no))
                        {
                            attnd_hr = Convert.ToDouble(GetCorrespondingKey(roll_no, has_load_rollno));
                            attnd_hr = attnd_hr + atndsplhr;
                            data.Rows[row_cnt][c + 1] = attnd_hr.ToString();

                        }

                        tot_hr = 0;
                        if (has_total_attnd_hour.Contains(roll_no))
                        {
                            tot_hr = Convert.ToDouble(GetCorrespondingKey(roll_no, has_total_attnd_hour));
                            if (row_cnt == 0)
                            {
                                if (max_tot < tot_hr)
                                {
                                    max_tot = tot_hr;
                                }
                                Session["max_tot_hour"] = tot_hr.ToString();
                            }
                        }
                        tot_hr = tot_hr + totsplhr;
                        data.Rows[row_cnt][c] = tot_hr.ToString();

                        od_count = 0;
                        if (has_od.Contains(roll_no) || odspl != 0)
                        {
                            od_count = Convert.ToInt16(GetCorrespondingKey(roll_no, has_od));
                            check_flag = true;
                            od_count = od_count + Convert.ToInt32(odspl);
                            data.Rows[row_cnt][c + 2] = od_count.ToString();
                            // drow["OD Periods"] = od_count.ToString();

                        }

                        if (check_flag == false)
                        {
                            data.Rows[row_cnt][c + 2] = "0";
                            // drow["OD Periods"] = "0";

                        }
                        string totper = "";
                        if (attnd_hr == 0 && od_count == 0)
                        {
                            data.Rows[row_cnt][c + 3] = "-";
                            //drow["Total Periods"] = "-";

                        }
                        else
                        {
                            data.Rows[row_cnt][c + 3] = (attnd_hr + od_count).ToString();
                            //drow["Total Periods"] = (attnd_hr + od_count).ToString();
                            totper = (attnd_hr + od_count).ToString();
                        }
                        if (attnd_hr == 0 && tot_hr == 0)
                        {
                            data.Rows[row_cnt][c + 4] = "-";
                            //drow["Periods Absent"] = "-";

                        }
                        else
                        {
                            data.Rows[row_cnt][c + 4] = (tot_hr - (attnd_hr + od_count)).ToString();
                            //drow["Periods Absent"] = (tot_hr - (attnd_hr + od_count)).ToString();

                        }
                        //if (subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].Note != "")
                        //{
                        //    if (Convert.ToInt32(subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].Note) < tot_hr)
                        //    {
                        //        subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].Note = tot_hr.ToString();
                        //    }
                        //}
                        //  data.Rows[row_cnt][c + 4] = tot_hr.ToString();


                        if (totper == "0" && tot_hr == 0)
                        {
                            data.Rows[row_cnt][c + 5] = "-";
                            //drow["% of Attendance"] = "-";

                        }
                        else
                        {
                            double avg_val = 0, avgstudent3 = 0;
                            decimal avgstudent1 = 0, avgstudent2 = 0;
                            double attnd_perc = 0;
                            avg_val = (((attnd_hr + od_count) / tot_hr) * 100);
                            if (avg_val.ToString() != "NaN")
                            {
                                //avgstudent1 = Convert.ToDecimal(avg_val);
                                //avgstudent2 = Math.Round(avgstudent1);
                                //avgstudent3 = Convert.ToDouble(avgstudent2);
                                //attnd_perc = Convert.ToString(avgstudent3);
                                attnd_perc = Math.Round(avg_val, 2);
                            }
                            else
                            {
                                attnd_perc = 0;
                            }
                            data.Rows[row_cnt][c + 5] = attnd_perc.ToString();

                            //drow["% of Attendance"] = attnd_perc.ToString();
                            //data.Rows.Add(drow);
                            if (attMinShorage < attnd_perc)
                            {
                                countInvisible++;
                                rowvis = false;
                                data.Rows.RemoveAt(row_cnt);
                                row_cnt--;
                                //subject_spread.Sheets[0].Rows[row_cnt].Visible = false;

                            }
                        }

                        row_cnt++;
                        check_row_visible = true;

                    }

                }



                //for (int row_visible = 0; row_visible < subject_spread.Sheets[0].RowCount; row_visible++)
                //{
                //    if (subject_spread.Sheets[0].Rows[row_visible].Visible == true)
                //    {
                //        rowValue++;
                //        check_row_visible = true;
                //        subject_spread.Sheets[0].Cells[row_visible, 0].Text = rowValue.ToString();
                //    }
                //}

                if (!check_row_visible || countInvisible == data.Rows.Count)
                {
                    pnl_pagesetting.Visible = false;
                    Showgrid.Visible = false;
                    btnxl.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    btnprintdirect.Visible = false;
                    lblexcl.Visible = false;//added By Srinath 11/2/2013
                    txtexcel.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                    return;
                }
                else if (data.Columns.Count > 0 && data.Rows.Count > 1)
                {

                    Showgrid.DataSource = data;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;


                    foreach (KeyValuePair<int, string> dr in dicdisCon)
                    {
                        int key = dr.Key;
                        Showgrid.Rows[key].BackColor = ColorTranslator.FromHtml("RED");


                    }

                    Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[0].Font.Bold = true;
                    btnxl.Visible = true;
                    btnprintmaster.Visible = true;
                    btnprintdirect.Visible = true;
                    lblexcl.Visible = true;//added By Srinath 11/2/2013
                    txtexcel.Visible = true;
                }
                else
                {
                    pnl_pagesetting.Visible = false;
                    Showgrid.Visible = false;
                    btnxl.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    btnprintdirect.Visible = false;
                    lblexcl.Visible = false;//added By Srinath 11/2/2013
                    txtexcel.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                    return;

                }
            }
            else
            {
                //    pnl_head_pageset.Visible = false;
                pnl_pagesetting.Visible = false;
                Showgrid.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                btnprintdirect.Visible = false;
                lblexcl.Visible = false;//added By Srinath 11/2/2013
                txtexcel.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            }
        }
        catch (Exception ex)
        {
            //dacc.sendErrorMail(ex, Convert.ToString("13"), Request.Path);
        }
    }

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                for (int j = colcount; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }



        }
        catch
        {
        }
    }

    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Subject Wise Attendance Details – Splitup Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


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
        else
        {
            Attvalue = "NE";
        }
        return Attvalue;
    }

    void CalculateTotalPages()
    {
        double totalRows = 0;
        totalRows = Convert.ToInt32(data.Rows.Count);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / Showgrid.PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }

    public string sem_roman(int sem)
    {
        string sql = string.Empty;
        string sem_roman = string.Empty;
        SqlDataReader rsChkSet;
        con_sem_roman.Close();
        con_sem_roman.Open();
        sql = "select * from inssettings where college_code=" + Session["collegecode"] + " and LinkName ='Semester Display'";
        SqlCommand cmd1 = new SqlCommand(sql, con_sem_roman);
        rsChkSet = cmd1.ExecuteReader();
        rsChkSet.Read();
        if (rsChkSet.HasRows == true)
        {
            if (rsChkSet["linkvalue"].ToString() == "1")
            {
                switch (sem)
                {
                    case 1:
                        sem_roman = "1";
                        break;
                    case 2:
                        sem_roman = "1-II";
                        break;
                    case 3:
                        sem_roman = "2-I";
                        break;
                    case 4:
                        sem_roman = "2-II";
                        break;
                    case 5:
                        sem_roman = "3-I";
                        break;
                    case 6:
                        sem_roman = "3-II";
                        break;
                    case 7:
                        sem_roman = "4-I";
                        break;
                    case 8:
                        sem_roman = "4-II";
                        break;
                    default:
                        sem_roman = " ";
                        break;
                }
            }
            else
            {
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
            }
        }
        return sem_roman;
    }

    //public void setheader()
    //{
    //    string coll_name = "", address1 = "", address2 = "", address3 = "", phoneno = "", faxno = "", email = "", website = "", degree_val = string.Empty;
    //    MyImg mi = new MyImg();
    //    mi.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi.ImageUrl = "Handler/Handler2.ashx?";
    //    MyImg mi2 = new MyImg();
    //    mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi2.ImageUrl = "Handler/Handler5.ashx?";
    //    if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
    //    {
    //        SqlDataReader dr_collinfo;//=new SqlDataReader();
    //        con.Close();
    //        con.Open();
    //        cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["collegecode"] + "", con);
    //        dr_collinfo = cmd.ExecuteReader();
    //        while (dr_collinfo.Read())
    //        {
    //            if (dr_collinfo.HasRows == true)
    //            {
    //                coll_name = dr_collinfo["collname"].ToString();
    //                address1 = dr_collinfo["address1"].ToString();
    //                address2 = dr_collinfo["address2"].ToString();
    //                address3 = dr_collinfo["address3"].ToString();
    //                phoneno = dr_collinfo["phoneno"].ToString();
    //                faxno = dr_collinfo["faxno"].ToString();
    //                email = dr_collinfo["email"].ToString();
    //                website = dr_collinfo["website"].ToString();
    //            }
    //        }
    //        subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
    //        if (subject_spread.Sheets[0].Columns[1].Visible == true)
    //        {
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, (subject_spread.Sheets[0].ColumnCount - 2));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = coll_name;
    //            subject_spread.Sheets[0].SheetCornerSpanModel.Add(0, 0, 8, 1);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 1].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 1].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, subject_spread.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, (subject_spread.Sheets[0].ColumnCount - 2));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 1].Text = address1 + "-" + address2 + "-" + address3;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, (subject_spread.Sheets[0].ColumnCount - 2));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 1].Text = "Phone:" + phoneno + "  Fax:" + faxno;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, (subject_spread.Sheets[0].ColumnCount - 2));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 1].Text = "Email:" + email + "  Web Site:" + website;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, (subject_spread.Sheets[0].ColumnCount - 2));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 1].Text = "Subject Wise Attendance Details – Splitup Report";
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, (subject_spread.Sheets[0].ColumnCount - 2));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 1].Text = "----------------------------------------------------";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 1].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 1].Border.BorderColorTop = Color.White;
    //            string sec_val = string.Empty;
    //            if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null)
    //            {
    //                sec_val = "Section: " + ddlsec.SelectedItem.ToString();
    //            }
    //            else
    //            {
    //                sec_val = string.Empty;
    //            }
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6,0].Text="Department:";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 1].Text = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString();
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7,0].Text="Academic Year:";
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7, 1].Text = Session["curr_year"].ToString();
    //              subject_spread.Sheets[0].ColumnHeader.Cells[8, 0].Text = "Subject Code & Name:";
    //              subject_spread.Sheets[0].ColumnHeader.Cells[8, 1].Text = GetFunction("select *From subject where subject_no="+ddlsubject.SelectedValue.ToString()+"") + " & " + ddlsubject.SelectedItem.ToString();
    //              subject_spread.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorTop  = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorBottom = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorTop = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorBottom = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorTop = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorTop = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorTop = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorBottom = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorTop = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorBottom = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorRight = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorRight = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[8, 0].Border.BorderColorRight = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorRight = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorRight = Color.White;
    //              subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Text = "Regulation:";
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Text = "Semester:";
    //              subject_spread.Sheets[0].ColumnHeader.Cells[8, (subject_spread.Sheets[0].ColumnCount - 2)].Text = "Total Number Of Hour(s) Conducted:" + Session["max_tot_hour"].ToString();
    //              subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 1)].Text =GetFunction(" select regulation from degree  where degree_code="+ddlbranch.SelectedValue.ToString()+"");
    //              subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 1)].Text = ddlduration.SelectedValue.ToString();
    //             // subject_spread.Sheets[0].ColumnHeader.Cells[8, (subject_spread.Sheets[0].ColumnCount - 1)].Text = Session["max_tot_hour"].ToString();
    //             subject_spread.Sheets[0].ColumnHeader.Rows[6].HorizontalAlign=HorizontalAlign.Left;
    //             subject_spread.Sheets[0].ColumnHeader.Rows[7].HorizontalAlign = HorizontalAlign.Left;
    //             subject_spread.Sheets[0].ColumnHeader.Rows[8].HorizontalAlign = HorizontalAlign.Left;
    //             subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, (subject_spread.Sheets[0].ColumnCount - 2), 1,2);
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, 1, 1, (subject_spread.Sheets[0].ColumnCount - 3));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, 1, 1, (subject_spread.Sheets[0].ColumnCount - 3));
    //       //     subject_spread.Sheets[0].ColumnHeader.Cells[6, 1].Text = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
    //            string dt = DateTime.Today.ToShortDateString();
    //            string[] dsplit = dt.Split(new Char[] { '/' });
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, 1, 1, (subject_spread.Sheets[0].ColumnCount - 3));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 1);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, ((subject_spread.Sheets[0].ColumnCount - 1)), 6, 1);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].CellType = mi2;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
    //            //--------set footer
    //            if (subject_spread.Sheets[0].RowCount > 0)
    //            {
    //                subject_spread.Sheets[0].RowCount=subject_spread.Sheets[0].RowCount+3;                   
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount-3,0,1,subject_spread.Sheets[0].ColumnCount );
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount-2,0,1,subject_spread.Sheets[0].ColumnCount );
    //                  subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount-3,0].Border.BorderColorBottom=Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount-2,0].Border.BorderColorTop =Color.White;
    //                  subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount-2,0].Border.BorderColorBottom=Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount-1,0].Border.BorderColorTop =Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1,0].Text = "Signature of Subject Incharge(s)";
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 1, 0, 1,4);
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5)].Text = "Signature of Class Teacher(s)";
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5), 1, 2);
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5)].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5)].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3)].Text = "HOD";
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3), 1, 2);
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3)].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3)].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 1)].Text = "DEAN";
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
    //            }
    //            //------------------------
    //        }
    //        if (subject_spread.Sheets[0].Columns[1].Visible == false && subject_spread.Sheets[0].Columns[2].Visible == true)
    //        {
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, (subject_spread.Sheets[0].ColumnCount - 3));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = coll_name;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 1);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.White;
    //            //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 8, 1);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 2].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, (subject_spread.Sheets[0].ColumnCount - 3));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 2].Text = address1 + "-" + address2 + "-" + address3;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, (subject_spread.Sheets[0].ColumnCount - 3));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 2].Text = "Phone:" + phoneno + "  Fax:" + faxno;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(3, 2, 1, (subject_spread.Sheets[0].ColumnCount - 3));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 2].Text = "Email:" + email + "  Web Site:" + website;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, (subject_spread.Sheets[0].ColumnCount - 3));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 2].Text = "Subject Wise Attendance Details – Splitup Report";
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(5, 2, 1, (subject_spread.Sheets[0].ColumnCount - 3));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 2].Text = "----------------------------------------------------";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorTop = Color.White;
    //            string sec_val = string.Empty;
    //            if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null)
    //            {
    //                sec_val = "Section: " + ddlsec.SelectedItem.ToString();
    //            }
    //            else
    //            {
    //                sec_val = string.Empty;
    //            }
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 0].Text = "Department:";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 2].Text = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString();
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6,2, 1, (subject_spread.Sheets[0].ColumnCount - 4));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, 2, 1, (subject_spread.Sheets[0].ColumnCount - 4));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, 2, 1, (subject_spread.Sheets[0].ColumnCount - 4));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Text = "Academic Year:";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 1].Text = Session["curr_year"].ToString();
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 0].Text = "Subject Code & Name:";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 2].Text = GetFunction("select *From subject where subject_no=" + ddlsubject.SelectedValue.ToString() + "") + " & " + ddlsubject.SelectedItem.ToString();
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 0].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Text = "Regulation:";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Text = "Semester:";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, (subject_spread.Sheets[0].ColumnCount - 2)].Text = "Total Number Of Hour(s) Conducted:" + Session["max_tot_hour"].ToString();
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, (subject_spread.Sheets[0].ColumnCount - 2),1,2);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 1)].Text = GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 1)].Text = ddlduration.SelectedValue.ToString();
    //          //  subject_spread.Sheets[0].ColumnHeader.Cells[8, (subject_spread.Sheets[0].ColumnCount - 1)].Text = Session["max_tot_hour"].ToString();
    //            subject_spread.Sheets[0].ColumnHeader.Rows[6].HorizontalAlign = HorizontalAlign.Left;
    //            subject_spread.Sheets[0].ColumnHeader.Rows[7].HorizontalAlign = HorizontalAlign.Left;
    //            subject_spread.Sheets[0].ColumnHeader.Rows[8].HorizontalAlign = HorizontalAlign.Left;
    //            subject_spread.Sheets[0].SheetCornerSpanModel.Add(0, 0, 6, 1);
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, ((subject_spread.Sheets[0].ColumnCount - 1)), 6, 1);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].CellType = mi2;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
    //            //--------set footerdef
    //            if (subject_spread.Sheets[0].RowCount > 0)
    //            {
    //                subject_spread.Sheets[0].RowCount = subject_spread.Sheets[0].RowCount + 3;
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 3, 0, 1, subject_spread.Sheets[0].ColumnCount);
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 2, 0, 1, subject_spread.Sheets[0].ColumnCount);
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 2, 0].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Text = "Signature of Subject Incharge(s)";
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 1, 0, 1, 4);
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5)].Text = "Signature of Class Teacher(s)";
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5), 1, 2);
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5)].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5)].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3)].Text = "HOD";
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3), 1, 2);
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3)].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3)].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 1)].Text = "DEAN";
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
    //            }
    //            //------------------------
    //        }
    //        if (subject_spread.Sheets[0].Columns[1].Visible == false && subject_spread.Sheets[0].Columns[2].Visible == false)
    //        {
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 1);
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, (subject_spread.Sheets[0].ColumnCount - 4));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = coll_name;
    //          //  subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 5, 1);
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 5, 1);
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 5, 1);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 3].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 3].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 3].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 3].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 3].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 3].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 3].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 3].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 3].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 3].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 3].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 3].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 3].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 3].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 3].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(1,3, 1, (subject_spread.Sheets[0].ColumnCount - 4));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 3].Text = address1 + "-" + address2 + "-" + address3;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(2, 3, 1, (subject_spread.Sheets[0].ColumnCount - 4));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 3].Text = "Phone:" + phoneno + "  Fax:" + faxno;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(3, 3, 1, (subject_spread.Sheets[0].ColumnCount - 4));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 3].Text = "Email:" + email + "  Web Site:" + website;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(4, 3, 1, (subject_spread.Sheets[0].ColumnCount - 4));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 3].Text = "Individual Subject Wise Attendance Report";
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(5, 3, 1, (subject_spread.Sheets[0].ColumnCount - 4));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 3].Text = "----------------------------------------------------";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, 3].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, 3].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, 3].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, 3].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 3].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 3].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 3].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 3].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 3].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 3].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 3].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 3].Border.BorderColorTop = Color.White;
    //            string sec_val = string.Empty;
    //            if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null)
    //            {
    //                sec_val = "Section: " + ddlsec.SelectedItem.ToString();
    //            }
    //            else
    //            {
    //                sec_val = string.Empty;
    //            }
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 0].Text = "Department:";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 3].Text = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString();
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, 3, 1, (subject_spread.Sheets[0].ColumnCount - 5));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, 3, 1, (subject_spread.Sheets[0].ColumnCount - 5));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, 3, 1, (subject_spread.Sheets[0].ColumnCount - 5));
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Text = "Academic Year:";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 3].Text = Session["curr_year"].ToString();
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 0].Text = "Subject Code & Name:";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 3].Text = GetFunction("select *From subject where subject_no=" + ddlsubject.SelectedValue.ToString() + "") + " & " + ddlsubject.SelectedItem.ToString();
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, 0].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 2)].Text = "Regulation:";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 2)].Text = "Semester:";
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, (subject_spread.Sheets[0].ColumnCount - 2)].Text = "Total Number Of Hour(s) Conducted:" + Session["max_tot_hour"].ToString();
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, (subject_spread.Sheets[0].ColumnCount - 2), 1, 2);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, (subject_spread.Sheets[0].ColumnCount - 1)].Text = GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, (subject_spread.Sheets[0].ColumnCount - 1)].Text = ddlduration.SelectedValue.ToString();
    //            //subject_spread.Sheets[0].ColumnHeader.Cells[8, (subject_spread.Sheets[0].ColumnCount - 1)].Text = Session["max_tot_hour"].ToString();
    //            subject_spread.Sheets[0].ColumnHeader.Rows[6].HorizontalAlign = HorizontalAlign.Left;
    //            subject_spread.Sheets[0].ColumnHeader.Rows[7].HorizontalAlign = HorizontalAlign.Left;
    //            subject_spread.Sheets[0].ColumnHeader.Rows[8].HorizontalAlign = HorizontalAlign.Left;
    //            subject_spread.Sheets[0].SheetCornerSpanModel.Add(0, 0, 6, 1);
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, ((subject_spread.Sheets[0].ColumnCount - 1)), 6, 1);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].CellType = mi2;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
    //            //--------set footerdef
    //            if (subject_spread.Sheets[0].RowCount > 0)
    //            {
    //                subject_spread.Sheets[0].RowCount = subject_spread.Sheets[0].RowCount + 3;
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 3, 0, 1, subject_spread.Sheets[0].ColumnCount);
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 2, 0, 1, subject_spread.Sheets[0].ColumnCount);
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 2, 0].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Text = "Signature of Subject Incharge(s)";
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 1, 0, 1, 4);
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, 0].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5)].Text = "Signature of Class Teacher(s)";
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5), 1, 2);
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5)].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 5)].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3)].Text = "HOD";
    //                subject_spread.Sheets[0].SpanModel.Add(subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3), 1, 2);
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3)].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 3)].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 1)].Text = "DEAN";
    //                subject_spread.Sheets[0].Cells[subject_spread.Sheets[0].RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
    //            }
    //            //------------------------
    //        }
    //    }
    //    int overall_colcount = 0;
    //    overall_colcount = subject_spread.Sheets[0].ColumnCount;
    //  //  subject_spread.Width = overall_colcount * 60;
    //}

    private string findday(int no, string sdate, string todate)
    {
        int order, holino;
        holino = 0;
        string day_order = string.Empty;
        string from_date = string.Empty;
        string fdate = string.Empty;
        int diff_work_day = 0;
        from_date = todate.ToString();
        string[] fm_date = from_date.Split(new Char[] { '/' });
        fdate = fm_date[1].ToString() + "/" + fm_date[0].ToString() + "/" + fm_date[2].ToString();
        SqlDataReader dr;
        con.Close();
        con.Open();
        cmd = new SqlCommand("select count(*) from holidaystudents where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and holiday_date between '" + sdate.ToString() + "' and  '" + fdate.ToString() + "' and halforfull='0' and isnull(Not_include_dayorder,0)<>'1'", con);//01.03.17 barath";
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            holino = Convert.ToInt16(dr[0].ToString());
        }
        DateTime dt1 = Convert.ToDateTime(fdate.ToString());
        DateTime dt2 = Convert.ToDateTime(sdate.ToString());
        TimeSpan t = dt1.Subtract(dt2);
        int days = t.Days;
        string quer = "select nodays from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString();
        string nodays = GetFunction(quer);
        int no_days = Convert.ToInt32(nodays);
        diff_work_day = days - holino;
        //  order = Convert.ToInt16(diff_work_day.ToString()) % no;
        order = Convert.ToInt16(diff_work_day.ToString()) % no_days;
        order = order + 1;
        string stastdayorder = string.Empty;
        stastdayorder = GetFunction("select starting_dayorder from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + "");
        if (stastdayorder.ToString().Trim() != "")
        {
            if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
            {
                order = order + (Convert.ToInt16(stastdayorder) - 1);
                if (order == (no_days + 1))
                    order = 1;
                else if (order > no_days)
                    order = order % no_days;
            }
        }
        if (order.ToString() == "0")
        {
            order = no;
        }
        if (order.ToString() == "1")
        {
            day_order = "mon";
        }
        else if (order.ToString() == "2")
        {
            day_order = "tue";
        }
        else if (order.ToString() == "3")
        {
            day_order = "wed";
        }
        else if (order.ToString() == "4")
        {
            day_order = "thu";
        }
        else if (order.ToString() == "5")
        {
            day_order = "fri";
        }
        else if (order.ToString() == "6")
        {
            day_order = "sat";
        }
        else if (order.ToString() == "7")
        {
            day_order = "sun";
        }
        return (day_order);
        con.Close();
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //errmsg.Visible = false;
        //subject_spread.CurrentPage = 0;
        //pagesearch_txt.Text = string.Empty;
        //errmsg.Visible = false;
        //pagesearch_txt.Text = string.Empty;
        //pageddltxt.Text = string.Empty;
        //pageddltxt.Text = string.Empty;
        //if (DropDownListpage.Text == "Others")
        //{
        //    pageddltxt.Visible = true;
        //    pageddltxt.Focus();
        //}
        //else
        //{
        //    pageddltxt.Visible = false;
        //    subject_spread.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
        //    subject_spread.Height = 40 + (25 * Convert.ToInt32(DropDownListpage.Text.ToString()));
        //    subject_spread.Sheets[0].Columns[3].Width = 200;
        //    CalculateTotalPages();
        //}
    }

    protected void pageddltxt_TextChanged(object sender, EventArgs e)
    {
        //   errmsg.Visible = false;
        //   subject_spread.CurrentPage = 0;
        //   pagesearch_txt.Text = string.Empty;
        //  try
        //   {
        //       if (pageddltxt.Text != string.Empty)
        //       {
        //           if (subject_spread.Sheets[0].RowCount >= Convert.ToInt16(pageddltxt.Text.ToString()) && Convert.ToInt16(pageddltxt.Text.ToString()) != 0)
        //           {
        //               subject_spread.Sheets[0].PageSize = Convert.ToInt16(pageddltxt.Text.ToString());
        //               subject_spread.Height = 30 + (25 * Convert.ToInt32(pageddltxt.Text.ToString()));
        //               CalculateTotalPages();
        //           }
        //           else
        //           {
        //               errmsg.Visible = true;
        //               errmsg.Text = "Enter valid Record count";
        //               pageddltxt.Text = string.Empty;
        //           }
        //       }
        //   }
        //catch
        //   {
        //       errmsg.Visible = true;
        //       errmsg.Text = "Enter valid Record count";
        //       pageddltxt.Text = string.Empty;
        //   }
    }

    protected void pagesearch_txt_TextChanged(object sender, EventArgs e)
    {
        //errmsg.Visible = false;
        //if (pagesearch_txt.Text.Trim() != string.Empty)
        //{
        //    if (Convert.ToInt64(pagesearch_txt.Text) > Convert.ToInt64(Session["totalPages"]))
        //    {
        //        errmsg.Visible = true;
        //        errmsg.Text = "Exceed The Page Limit";
        //        pagesearch_txt.Text = string.Empty;
        //        subject_spread.Visible = true;
        //        btnxl.Visible = true;
        //        Printcontrol.Visible = false;
        //        btnprintmaster.Visible = true;
        //        lblexcl.Visible = true;//added By Srinath 11/2/2013
        //        txtexcel.Visible = true;
        //    }
        //    else if (Convert.ToInt64(pagesearch_txt.Text) == 0)
        //    {
        //        errmsg.Visible = true;
        //        errmsg.Text = " Search Should Be Greater Than '0'";
        //        pagesearch_txt.Text = string.Empty;
        //        subject_spread.Visible = true;
        //        btnxl.Visible = true;
        //        Printcontrol.Visible = false;
        //        btnprintmaster.Visible = true;
        //        lblexcl.Visible = true;//added By Srinath 11/2/2013
        //        txtexcel.Visible = true;
        //    }
        //    else
        //    {
        //        errmsg.Visible = false;
        //        subject_spread.CurrentPage = Convert.ToInt16(pagesearch_txt.Text) - 1;
        //        subject_spread.Visible = true;
        //        btnxl.Visible = true;
        //        Printcontrol.Visible = false;
        //        btnprintmaster.Visible = true;
        //        lblexcl.Visible = true;//added By Srinath 11/2/2013
        //        txtexcel.Visible = true;
        //    }
        //}
    }

    //Saran
    #region unused_Function
    //protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    //{
    //    errlbl.Visible = false;
    //    subject_spread.Sheets[0].ColumnHeader.Rows[0].Visible = true;
    //    subject_spread.Sheets[0].ColumnHeader.Rows[1].Visible = true;
    //    subject_spread.Sheets[0].ColumnHeader.Rows[2].Visible = true;
    //    subject_spread.Sheets[0].ColumnHeader.Rows[3].Visible = true;
    //    subject_spread.Sheets[0].ColumnHeader.Rows[4].Visible = true;
    //    subject_spread.Sheets[0].ColumnHeader.Rows[5].Visible = true;
    //    subject_spread.Sheets[0].ColumnHeader.Rows[6].Visible = true;
    //    subject_spread.Sheets[0].ColumnHeader.Rows[7].Visible = true;
    //    int i = 0;
    //    ddlpage.Items.Clear();
    //    int totrowcount = subject_spread.Sheets[0].RowCount;
    //    int pages = totrowcount / 25;
    //    int intialrow = 1;
    //    int remainrows = totrowcount % 25;
    //    if (subject_spread.Sheets[0].RowCount > 0)
    //    {
    //        int i5 = 0;
    //        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    //        for (i = 1; i <= pages; i++)
    //        {
    //            i5 = i;
    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //            intialrow = intialrow + 25;
    //        }
    //        if (remainrows > 0)
    //        {
    //            i = i5 + 1;
    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //        }
    //    }
    //    //if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //    //{
    //    //    for (i = 0; i < subject_spread.Sheets[0].RowCount; i++)
    //    //    {
    //    //        subject_spread.Sheets[0].Rows[i].Visible = true;
    //    //    }
    //    //    double totalRows = 0;
    //    //    totalRows = Convert.ToInt32(subject_spread.Sheets[0].RowCount);
    //    //    Session["totalPages"] = (int)Math.Ceiling(totalRows / subject_spread.Sheets[0].PageSize);
    //    //    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //    //    DropDownListpage.Items.Clear();
    //    //    if (totalRows >= 10)
    //    //    {
    //    //        subject_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //    //        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //    //        {
    //    //            DropDownListpage.Items.Add((k + 10).ToString());
    //    //        }
    //    //        DropDownListpage.Items.Add("Others");
    //    //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //    //        subject_spread.Height = 335;
    //    //    }
    //    //    else if (totalRows == 0)
    //    //    {
    //    //        DropDownListpage.Items.Add("0");
    //    //        subject_spread.Height = 100;
    //    //    }
    //    //    else
    //    //    {
    //    //        subject_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //    //        DropDownListpage.Items.Add(subject_spread.Sheets[0].PageSize.ToString());
    //    //        subject_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //    //    }
    //    //    if (Convert.ToInt32(subject_spread.Sheets[0].RowCount) > 10)
    //    //    {
    //    //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //    //        subject_spread.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //    //        //   subject_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //    //        CalculateTotalPages();
    //    //    }
    //    //    pnl_pagesetting.Visible = true;
    //    //}
    //    //else
    //    //{
    //    //    errlbl.Visible = false;
    //    //    pnl_pagesetting.Visible = false;
    //    //}
    //}

    //protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    //{
    //    errlbl.Visible = false;
    //    int i = 0;
    //    ddlpage.Items.Clear();
    //    int totrowcount = subject_spread.Sheets[0].RowCount;
    //    int pages = totrowcount / 25;
    //    int intialrow = 1;
    //    int remainrows = totrowcount % 25;
    //    if (subject_spread.Sheets[0].RowCount > 0)
    //    {
    //        int i5 = 0;
    //        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    //        for (i = 1; i <= pages; i++)
    //        {
    //            i5 = i;
    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //            intialrow = intialrow + 25;
    //        }
    //        if (remainrows > 0)
    //        {
    //            i = i5 + 1;
    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //        }
    //    }
    //    //if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //    //{
    //    //    for (i = 0; i < subject_spread.Sheets[0].RowCount; i++)
    //    //    {
    //    //        subject_spread.Sheets[0].Rows[i].Visible = true;
    //    //    }
    //    //    double totalRows = 0;
    //    //    totalRows = Convert.ToInt32(subject_spread.Sheets[0].RowCount);
    //    //    Session["totalPages"] = (int)Math.Ceiling(totalRows / subject_spread.Sheets[0].PageSize);
    //    //    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //    //    DropDownListpage.Items.Clear();
    //    //    if (totalRows >= 10)
    //    //    {
    //    //        subject_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //    //        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //    //        {
    //    //            DropDownListpage.Items.Add((k + 10).ToString());
    //    //        }
    //    //        DropDownListpage.Items.Add("Others");
    //    //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //    //        subject_spread.Height = 335;
    //    //    }
    //    //    else if (totalRows == 0)
    //    //    {
    //    //        DropDownListpage.Items.Add("0");
    //    //        subject_spread.Height = 100;
    //    //    }
    //    //    else
    //    //    {
    //    //        subject_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //    //        DropDownListpage.Items.Add(subject_spread.Sheets[0].PageSize.ToString());
    //    //        subject_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //    //    }
    //    //    if (Convert.ToInt32(subject_spread.Sheets[0].RowCount) > 10)
    //    //    {
    //    //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //    //        subject_spread.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //    //        //  subject_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //    //        CalculateTotalPages();
    //    //    }
    //    //    pnl_pagesetting.Visible = true;
    //    //}
    //    //else
    //    //{
    //    //    pnl_pagesetting.Visible = false;
    //    //}
    //}
    #endregion


    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Saran
        //has.Clear();
        //has.Add("college_code", Session["collegecode"].ToString());
        //has.Add("form_name", "singlesubjectwise_splitup_attnd_report.aspx");
        //dsprint = dacc.select_method("PROC_PRINT_MASTER_SETTINGS", has, "sp");
        //if (dsprint.Tables[0].Rows.Count > 0)
        //{
        //    view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
        //    view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
        //    view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
        //    errlbl.Visible = false;
        //    if (view_header == "0")
        //    {
        //        for (int i = 0; i < subject_spread.Sheets[0].RowCount; i++)
        //        {
        //            subject_spread.Sheets[0].Rows[i].Visible = false;
        //        }
        //        int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //        int end = start + 24;
        //        if (end >= subject_spread.Sheets[0].RowCount)
        //        {
        //            end = subject_spread.Sheets[0].RowCount;
        //        }
        //        int rowstart = subject_spread.Sheets[0].RowCount - Convert.ToInt32(start);
        //        int rowend = subject_spread.Sheets[0].RowCount - Convert.ToInt32(end);
        //        for (int i = start - 1; i < end; i++)
        //        {
        //            subject_spread.Sheets[0].Rows[i].Visible = true;
        //        }
        //        for (int row_cnt = 0; row_cnt < subject_spread.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //        {
        //            subject_spread.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
        //        }
        //    }
        //    else if (view_header == "1")
        //    {
        //        for (int i = 0; i < subject_spread.Sheets[0].RowCount; i++)
        //        {
        //            subject_spread.Sheets[0].Rows[i].Visible = false;
        //        }
        //        int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //        int end = start + 24;
        //        if (end >= subject_spread.Sheets[0].RowCount)
        //        {
        //            end = subject_spread.Sheets[0].RowCount;
        //        }
        //        int rowstart = subject_spread.Sheets[0].RowCount - Convert.ToInt32(start);
        //        int rowend = subject_spread.Sheets[0].RowCount - Convert.ToInt32(end);
        //        for (int i = start - 1; i < end; i++)
        //        {
        //            subject_spread.Sheets[0].Rows[i].Visible = true;
        //        }
        //        if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
        //        {
        //            for (int row_cnt = 0; row_cnt < subject_spread.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //            {
        //                subject_spread.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
        //            }
        //        }
        //        else
        //        {
        //            for (int row_cnt = 0; row_cnt < subject_spread.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //            {
        //                subject_spread.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < subject_spread.Sheets[0].RowCount; i++)
        //        {
        //            subject_spread.Sheets[0].Rows[i].Visible = false;
        //        }
        //        int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //        int end = start + 24;
        //        if (end >= subject_spread.Sheets[0].RowCount)
        //        {
        //            end = subject_spread.Sheets[0].RowCount;
        //        }
        //        int rowstart = subject_spread.Sheets[0].RowCount - Convert.ToInt32(start);
        //        int rowend = subject_spread.Sheets[0].RowCount - Convert.ToInt32(end);
        //        for (int i = start - 1; i < end; i++)
        //        {
        //            subject_spread.Sheets[0].Rows[i].Visible = true;
        //        }
        //        {
        //            for (int row_cnt = 0; row_cnt < subject_spread.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //            {
        //                subject_spread.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
        //            }
        //        }
        //    }

        //    if (view_footer_text != "")
        //    {
        //        if (view_footer == "0")
        //        {
        //            subject_spread.Sheets[0].Rows[(subject_spread.Sheets[0].RowCount - 1)].Visible = true;
        //            subject_spread.Sheets[0].Rows[(subject_spread.Sheets[0].RowCount - 2)].Visible = true;
        //            subject_spread.Sheets[0].Rows[(subject_spread.Sheets[0].RowCount - 3)].Visible = true;
        //        }
        //        else
        //        {
        //            if (ddlpage.Text != "")
        //            {
        //                if (ddlpage.SelectedIndex != ddlpage.Items.Count - 1)
        //                {
        //                    subject_spread.Sheets[0].Rows[(subject_spread.Sheets[0].RowCount - 1)].Visible = false;
        //                    subject_spread.Sheets[0].Rows[(subject_spread.Sheets[0].RowCount - 2)].Visible = false;
        //                    subject_spread.Sheets[0].Rows[(subject_spread.Sheets[0].RowCount - 3)].Visible = false;
        //                }
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    errlbl.Visible = false;
        //    errlbl.Text = "No Header and Footer setting Assigned";
        //}
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

    protected void btnxl_Click(object sender, EventArgs e)
    {
        //Modified by Srinath 27/2/2013
        string reportname = txtexcel.Text;
        errmsg.Visible = false;
        if (reportname.ToString().Trim() != "")
        {

            dacc.printexcelreportgrid(Showgrid, reportname);
            txtexcel.Text = string.Empty;
        }
        else
        {
            errmsg.Text = "Please Enter Your Report Name";
            errmsg.Visible = true;
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }


    protected void btnPrint_Click(object sender, EventArgs e)
    {

    }

    //Saran
    #region unused_function
    //public void print_btngo()
    //{
    //    final_print_col_cnt = 0;
    //    errmsg.Visible = false;
    //    check_col_count_flag = false;
    //    subject_spread.Sheets[0].SheetCorner.RowCount = 0;
    //    subject_spread.Sheets[0].ColumnCount = 0;
    //    subject_spread.Sheets[0].RowCount = 0;
    //    subject_spread.Sheets[0].SheetCorner.RowCount = 8;
    //    subject_spread.Sheets[0].ColumnCount = 5;
    //    has.Clear();
    //    has.Add("college_code", Session["collegecode"].ToString());
    //    has.Add("form_name", "singlesubjectwise_splitup_attnd_report.aspx");
    //    dsprint = dacc.select_method("PROC_PRINT_MASTER_SETTINGS", has, "sp");
    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        lblpages.Visible = true;
    //        ddlpage.Visible = true;
    //        // 3. header add
    //        if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //        {
    //            new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
    //        }
    //        //   3. end header add
    //        btnclick();
    //        isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();
    //        //1.set visible columns
    //        column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
    //        if (column_field != "" && column_field != null)
    //        {
    //            //  check_col_count_flag = true;
    //            for (col_count_all = 0; col_count_all < subject_spread.Sheets[0].ColumnCount; col_count_all++)
    //            {
    //                subject_spread.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column                                
    //            }
    //            printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
    //            string[] split_printvar = printvar.Split(',');
    //            for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
    //            {
    //                span_cnt = 0;
    //                string[] split_star = split_printvar[splval].Split('*');
    //                if (split_star.GetUpperBound(0) > 0)
    //                {
    //                    for (col_count = 0; col_count < subject_spread.Sheets[0].ColumnCount - 1; col_count++)
    //                    {
    //                        if (subject_spread.Sheets[0].ColumnHeader.Cells[(subject_spread.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_star[0])
    //                        {
    //                            child_span_count = 0;
    //                            string[] split_star_doller = split_star[1].Split('$');
    //                            for (int doller_count = 1; doller_count < split_star_doller.GetUpperBound(0); doller_count++)
    //                            {
    //                                for (int child_node = col_count; child_node <= col_count + split_star_doller.GetUpperBound(0); child_node++)
    //                                {
    //                                    if (subject_spread.Sheets[0].ColumnHeader.Cells[(subject_spread.Sheets[0].ColumnHeader.RowCount - 1), child_node].Text == split_star_doller[doller_count])
    //                                    {
    //                                        span_cnt++;
    //                                        if (span_cnt == 1 && child_node == col_count + 1)
    //                                        {
    //                                            subject_spread.Sheets[0].ColumnHeader.Cells[(subject_spread.Sheets[0].ColumnHeader.RowCount - 2), col_count + 1].Text = split_star[0].ToString();
    //                                            col_count++;
    //                                        }
    //                                        if (child_node != col_count)
    //                                        {
    //                                            span_cnt = child_node - (child_span_count - 1);
    //                                        }
    //                                        else
    //                                        {
    //                                            child_span_count = col_count;
    //                                        }
    //                                        subject_spread.Sheets[0].ColumnHeaderSpanModel.Add((subject_spread.Sheets[0].ColumnHeader.RowCount - 2), col_count, 1, span_cnt);
    //                                        subject_spread.Sheets[0].Columns[child_node].Visible = true;
    //                                        final_print_col_cnt++;
    //                                        if (span_cnt == split_star_doller.GetUpperBound(0) - 1)
    //                                        {
    //                                            break;
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    for (col_count = 0; col_count < subject_spread.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (subject_spread.Sheets[0].ColumnHeader.Cells[(subject_spread.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_printvar[splval])
    //                        {
    //                            subject_spread.Sheets[0].Columns[col_count].Visible = true;
    //                            final_print_col_cnt++;
    //                            break;
    //                        }
    //                    }
    //                }
    //            }
    //            //1 end.set visible columns
    //        }
    //        else
    //        {
    //            Showgrid.Visible = false;
    //            btnxl.Visible = false;
    //            Printcontrol.Visible = false;
    //            btnprintmaster.Visible = false;
    //            lblexcl.Visible = true;//added By Srinath 11/2/2013
    //            txtexcel.Visible = true;
    //            pnl_pagesetting.Visible = false;
    //            lblpages.Visible = false;
    //            ddlpage.Visible = false;
    //            errmsg.Visible = true;
    //            errmsg.Text = "Select Atleast One Column Field From The Treeview";
    //        }
    //    }
    //    // subject_spread.Width = final_print_col_cnt * 100;
    //}

    ////Hiiden By Srinath 15/5/2013
    //public void setheader_print()
    //{
    //    // subject_spread.Sheets[0].RemoveSpanCell
    //    //================header
    //    temp_count = 0;
    //    MyImg mi = new MyImg();
    //    mi.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi.ImageUrl = "Handler/Handler2.ashx?";
    //    MyImg mi2 = new MyImg();
    //    mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi2.ImageUrl = "Handler/Handler5.ashx?";
    //    if (final_print_col_cnt == 1)
    //    {
    //        for (col_count = 0; col_count < subject_spread.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (subject_spread.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                // one_column();
    //                more_column();
    //                break;
    //            }
    //        }
    //    }
    //    else if (final_print_col_cnt == 2)
    //    {
    //        for (col_count = 0; col_count < subject_spread.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (subject_spread.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    //   subject_spread.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                    subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (subject_spread.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                else
    //                {
    //                    //  one_column();
    //                    more_column();
    //                    for (int row_cnt = 0; row_cnt < subject_spread.Sheets[0].ColumnHeader.RowCount - 1; row_cnt++)
    //                    {
    //                        subject_spread.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                    }
    //                }
    //                temp_count++;
    //                if (temp_count == 2)
    //                {
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    else if (final_print_col_cnt == 3)
    //    {
    //        for (col_count = 0; col_count < subject_spread.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (subject_spread.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    //   subject_spread.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                    subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (subject_spread.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                else if (temp_count == 1)
    //                {
    //                    // one_column();
    //                    more_column();
    //                    for (int row_cnt = 0; row_cnt < subject_spread.Sheets[0].ColumnHeader.RowCount - 1; row_cnt++)
    //                    {
    //                        subject_spread.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        subject_spread.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                }
    //                else if (temp_count == 2)
    //                {
    //                    if (isonumber != string.Empty)
    //                    {
    //                        subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].Text = "ISO CODE:" + isonumber;
    //                        subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, (subject_spread.Sheets[0].ColumnHeader.RowCount - 3), 1);
    //                        subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count].CellType = mi2;
    //                        subject_spread.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                        subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorRight = Color.Black;
    //                    }
    //                    else
    //                    {
    //                        subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (subject_spread.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                        subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                        subject_spread.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                        subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.Black;
    //                    }
    //                }
    //                temp_count++;
    //                if (temp_count == 3)
    //                {
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    else//-----------column count more than 3
    //    {
    //        for (col_count = 0; col_count < subject_spread.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (subject_spread.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (6), 1);
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //                }
    //                end_column = col_count;
    //                temp_count++;
    //                if (final_print_col_cnt == temp_count)
    //                {
    //                    break;
    //                }
    //            }
    //        }
    //        if (isonumber != string.Empty)
    //        {
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Text = "ISO CODE:";// +isonumber;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, end_column].Text = isonumber;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, end_column].HorizontalAlign = HorizontalAlign.Left;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(1, end_column, (5), 1);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, end_column].CellType = mi2;
    //            subject_spread.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorRight = Color.Black;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, end_column - 1].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, end_column - 1].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //        }
    //        else
    //        {
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (6), 1);
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //            subject_spread.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorRight = Color.Black;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //        }
    //        temp_count = 0;
    //        for (col_count = 0; col_count < subject_spread.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (subject_spread.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 1)
    //                {
    //                    more_column();
    //                    for (int row_cnt = 0; row_cnt < subject_spread.Sheets[0].ColumnHeader.RowCount - 1; row_cnt++)
    //                    {
    //                        subject_spread.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        subject_spread.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                }
    //                temp_count++;
    //            }
    //        }
    //    }
    //    //=========================
    //    //2.Footer setting
    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
    //        {
    //            footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
    //            subject_spread.Sheets[0].RowCount = subject_spread.Sheets[0].RowCount + 3;
    //            subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 3), start_column].ColumnSpan = subject_spread.Sheets[0].ColumnCount - start_column;
    //            subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 2), start_column].ColumnSpan = subject_spread.Sheets[0].ColumnCount - start_column;
    //            subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 3), start_column].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 2), start_column].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 2), start_column].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), start_column].Border.BorderColorTop = Color.White;
    //            footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //            string[] footer_text_split = footer_text.Split(',');
    //            footer_text = string.Empty;
    //            if (final_print_col_cnt < footer_count)
    //            {
    //                for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
    //                {
    //                    if (footer_text == "")
    //                    {
    //                        footer_text = footer_text_split[concod_footer].ToString();
    //                    }
    //                    else
    //                    {
    //                        footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
    //                    }
    //                }
    //                for (col_count = 0; col_count < subject_spread.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (subject_spread.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        subject_spread.Sheets[0].SpanModel.Add((subject_spread.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                        subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                        subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        break;
    //                    }
    //                }
    //            }
    //            else if (final_print_col_cnt == footer_count)
    //            {
    //                for (col_count = 0; col_count < subject_spread.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (subject_spread.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                        subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                        subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                        subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        temp_count++;
    //                        if (temp_count == footer_count)
    //                        {
    //                            break;
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                temp_count = 0;
    //                split_col_for_footer = final_print_col_cnt / footer_count;
    //                footer_balanc_col = final_print_col_cnt % footer_count;
    //                for (col_count = 0; col_count < subject_spread.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (subject_spread.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        if (temp_count == 0)
    //                        {
    //                            subject_spread.Sheets[0].SpanModel.Add((subject_spread.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                        }
    //                        else
    //                        {
    //                            subject_spread.Sheets[0].SpanModel.Add((subject_spread.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);
    //                        }
    //                        subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                        subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        if (col_count - 1 >= 0)
    //                        {
    //                            subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                            subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                        }
    //                        subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                        subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                        if (col_count + 1 < subject_spread.Sheets[0].ColumnCount)
    //                        {
    //                            subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                            subject_spread.Sheets[0].Cells[(subject_spread.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
    //                        }
    //                        temp_count++;
    //                        if (temp_count == 0)
    //                        {
    //                            col_count = col_count + split_col_for_footer + footer_balanc_col;
    //                        }
    //                        else
    //                        {
    //                            col_count = col_count + split_col_for_footer;
    //                        }
    //                        if (temp_count == footer_count)
    //                        {
    //                            break;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    //2 end.Footer setting
    //}

    //public void more_column()
    //{
    //    header_text();
    //    double max_tot = 0;
    //    foreach (DictionaryEntry parameter2 in has_total_attnd_hour)
    //    {
    //        max_tot = Convert.ToDouble((parameter2.Value).ToString());
    //        if (tot_hr < max_tot)
    //        {
    //            tot_hr = max_tot;
    //        }
    //    }
    //    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
    //    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //    //  subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, final_print_col_cnt - 2);
    //    if (final_print_col_cnt > 3)
    //    {
    //        int dd = end_column - col_count;
    //        int span_col = 0;
    //        if (dd >= 100)
    //        {
    //            int span_col_count = 0, span_balanc = 0;
    //            span_col_count = dd / 100;
    //            span_balanc = dd % 100;
    //            for (span_col = 0; span_col <= dd - 100; span_col += 100)
    //            {
    //                subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].ColumnSpan = 100;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].ColumnSpan = 100;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].ColumnSpan = 100;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].ColumnSpan = 100;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].ColumnSpan = 100;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].ColumnSpan = 100;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].ColumnSpan = 100;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].ColumnSpan = 100;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorTop = Color.White;
    //            }
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].ColumnSpan = span_balanc;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].ColumnSpan = span_balanc;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].ColumnSpan = span_balanc;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].ColumnSpan = span_balanc;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].ColumnSpan = span_balanc;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].ColumnSpan = span_balanc;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].ColumnSpan = span_balanc;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].ColumnSpan = span_balanc;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorRight = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorLeft = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorTop = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorTop = Color.White;
    //        }
    //        else
    //        {
    //            if (isonumber != string.Empty)
    //            {
    //                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count - 1));
    //            }
    //            else
    //            {
    //                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count));
    //            }
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, col_count, 1, (end_column - col_count));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, col_count, 1, (end_column - col_count));
    //        }
    //    }
    //    subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
    //    subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //    subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;
    //    if (phoneno != "" && phoneno != null)
    //    {
    //        phone = "Phone:" + phoneno;
    //    }
    //    else
    //    {
    //        phone = string.Empty;
    //    }
    //    if (faxno != "" && faxno != null)
    //    {
    //        fax = "  Fax:" + faxno;
    //    }
    //    else
    //    {
    //        fax = string.Empty;
    //    }
    //    subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
    //    subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
    //    subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;
    //    if (email != "" && faxno != null)
    //    {
    //        email_id = "Email:" + email;
    //    }
    //    else
    //    {
    //        email_id = string.Empty;
    //    }
    //    if (website != "" && website != null)
    //    {
    //        web_add = "  Web Site:" + website;
    //    }
    //    else
    //    {
    //        web_add = string.Empty;
    //    }
    //    subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
    //    subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
    //    subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;
    //    if (form_name != "" && form_name != null)
    //    {
    //        subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;
    //        //   subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------"; //hided on 04.07.12
    //    }
    //    if (final_print_col_cnt <= 3)
    //    {
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count].Text = "Degree & Branch:" + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "     Regulation:" + GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");//"Name of the Program & Branch:"
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count].Text = "Academic Year:" + Session["curr_year"].ToString() + "Semester Number:" + ddlduration.SelectedValue.ToString();
    //        subject_spread.Sheets[0].ColumnHeader.Cells[8, col_count].Text = "Course Code & Name=" + GetFunction("select *From subject where subject_no='" + ddlsubject.SelectedValue.ToString() + "'") + "&" + ddlsubject.SelectedItem.ToString() + "   Total Number Of Hour(s) Conducted:" + tot_hr;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorTop = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorBottom = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[8, col_count].Border.BorderColorTop = Color.White;
    //    }
    //    else
    //    {
    //        // between_visible_col_cnt = (end_column - col_count)/2;
    //        between_visible_col_cnt = (final_print_col_cnt) / 2; //= (final_print_col_cnt - 1) / 2;
    //        between_visible_col_cnt_bal = (final_print_col_cnt) % 2;// (final_print_col_cnt - 1 ) % 2;
    //        visi_col = 0;
    //        for (x = start_column; x < subject_spread.Sheets[0].ColumnCount - 1; x++)//==============find first half column count
    //        {
    //            if (subject_spread.Sheets[0].Columns[x].Visible == true)
    //            {
    //                visi_col++;
    //                if (visi_col == between_visible_col_cnt)
    //                {
    //                    visi_col = x;
    //                    break;
    //                }
    //            }
    //        }
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].Text = "Degree & Branch: " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString(); //"Name of the Program & Branch:"
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].HorizontalAlign = HorizontalAlign.Left;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorBottom = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorRight = Color.White;
    //        int xx = 0;
    //        for (xx = visi_col; xx < subject_spread.Sheets[0].ColumnCount - 1; xx++)
    //        {
    //            if (subject_spread.Sheets[0].Columns[xx].Visible == true)
    //            {
    //                visi_col1 = xx;
    //                break;
    //            }
    //        }
    //        //xx = 0;
    //        //for (x = visi_col1; x < subject_spread.Sheets[0].ColumnCount - 1; x++)
    //        //{
    //        //    if (subject_spread.Sheets[0].Columns[x].Visible == true)
    //        //    {
    //        //        xx++;
    //        //        //if (visi_col1 == between_visible_col_cnt + between_visible_col_cnt_bal)
    //        //        //{
    //        //        //    break;
    //        //        //}
    //        //    }
    //        //}
    //        //subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col].Text = "Regulation:";
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + 1].Border.BorderColorLeft = Color.White;//here also changed from visi_col to visi_col+1
    //        //subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col+1].Border.BorderColorRight = Color.White; //hihded
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + 1].Border.BorderColorBottom = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + 1].HorizontalAlign = HorizontalAlign.Left;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, end_column].HorizontalAlign = HorizontalAlign.Left;
    //        //   subject_spread.Sheets[0].ColumnHeader.Cells[6, end_column].Text = GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");//hided on 04.07.12
    //        int visi_col3 = 0, last_col = 0;
    //        //for (int y = visi_col; y < end_column; y++)
    //        //{
    //        //    if (subject_spread.Sheets[0].Columns[y].Visible == true)
    //        //    {
    //        //        visi_col3++;
    //        //        last_col = y;
    //        //    }
    //        //}
    //        subject_spread.Sheets[0].ColumnHeader.Cells[6, end_column].Border.BorderColorBottom = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].Text = "Academic Year:" + Session["curr_year"].ToString();
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].HorizontalAlign = HorizontalAlign.Left;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorRight = Color.White;
    //        //subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col].Text = "Regulation:";
    //        //subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col].Text = "Semester Number:";
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + 1].Border.BorderColorTop = Color.White;//here also changed from visi_col to visi_Col+1
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + 1].Border.BorderColorLeft = Color.White;
    //        //  subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + 1].Border.BorderColorRight = Color.White;//hided
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + 1].HorizontalAlign = HorizontalAlign.Left;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + 1].Border.BorderColorBottom = Color.White;//added
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].HorizontalAlign = HorizontalAlign.Left;
    //        //   subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].Text = ddlduration.SelectedValue.ToString();//modified on 04.07.12
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].Border.BorderColorTop = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].Border.BorderColorBottom = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorBottom = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorBottom = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].Text = "Course Code & Name:" + GetFunction("select *From subject where subject_no='" + ddlsubject.SelectedValue.ToString() + "'") + "&" + ddlsubject.SelectedItem;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].HorizontalAlign = HorizontalAlign.Left;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].Border.BorderColorRight = Color.White;
    //        if (visi_col1 < 100)
    //        {
    //            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col1 + 1].Text = "Regulation: " + GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + ""); ; //modified here below 7 lines from the col count visi_col to visi_col+1
    //            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col1 + 1].Text = "Semester Number: " + ddlduration.SelectedValue.ToString();
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col1 + 1].Text = "Total number of hours conducted (a):" + tot_hr.ToString();
    //        }
    //        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col1 + 1].Border.BorderColorTop = Color.White;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col1 + 1].Border.BorderColorLeft = Color.White;
    //        //subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col1+1].Border.BorderColorRight = Color.White; //hided
    //        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col1 + 1].HorizontalAlign = HorizontalAlign.Left;
    //        subject_spread.Sheets[0].ColumnHeader.Cells[8, end_column].HorizontalAlign = HorizontalAlign.Left;
    //        //    subject_spread.Sheets[0].ColumnHeader.Cells[8, end_column].Text = tot_hr.ToString();
    //        subject_spread.Sheets[0].ColumnHeader.Cells[8, end_column].Border.BorderColorTop = Color.White;
    //        //=================================================3/5/12
    //        if (visi_col1 < 100)
    //        {
    //            //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, visi_col );//from visi_col to visi_col+1 modified on 04.07.12
    //            //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, start_column, 1, visi_col);
    //            //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, start_column, 1, visi_col);
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, visi_col1 + 1);//from visi_col to visi_col+1 modified on 04.07.12
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, start_column, 1, visi_col1 + 1);
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, start_column, 1, visi_col1 + 1);
    //            //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, visi_col1 , 1, (subject_spread.Sheets[0].ColumnCount-1-visi_col1));
    //            //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, visi_col1, 1, (subject_spread.Sheets[0].ColumnCount - 1 - visi_col1));
    //            //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, visi_col1, 1, (subject_spread.Sheets[0].ColumnCount - 1 - visi_col1));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, visi_col1 + 1, 1, (subject_spread.Sheets[0].ColumnCount - 1 - visi_col1));//changed on 04.07.12
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, visi_col1 + 1, 1, (subject_spread.Sheets[0].ColumnCount - 1 - visi_col1));
    //            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, visi_col1 + 1, 1, (subject_spread.Sheets[0].ColumnCount - 1 - visi_col1));
    //        }
    //        {
    //            int dd = visi_col;
    //            int span_col = 0;
    //            if (dd >= 100)
    //            {
    //                int span_col_count = 0, span_balanc = 0;
    //                span_col_count = dd / 100;
    //                span_balanc = dd % 100;
    //                for (span_col = 0; span_col <= dd - 100; span_col += 100)
    //                {
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].ColumnSpan = 100;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].ColumnSpan = 100;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].ColumnSpan = 100;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorTop = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorBottom = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorTop = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorBottom = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorTop = Color.White;
    //                }
    //                subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].ColumnSpan = span_balanc;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].ColumnSpan = span_balanc;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].ColumnSpan = span_balanc;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorTop = Color.White;
    //            }
    //            else
    //            {
    //                //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, visi_col1 + 1);
    //                //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, start_column, 1, visi_col1 + 1);
    //                //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, start_column, 1, visi_col1 + 1);
    //            }
    //        }
    //        {
    //            int dd = (subject_spread.Sheets[0].ColumnCount - 2 - visi_col1);
    //            int span_col = 0;
    //            if (dd >= 100)
    //            {
    //                int span_col_count = 0, span_balanc = 0;
    //                span_col_count = dd / 100;
    //                span_balanc = dd % 100;
    //                for (span_col = 0; span_col <= dd - 100; span_col += 100)
    //                {
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].ColumnSpan = 100;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].ColumnSpan = 100;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].ColumnSpan = 100;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].Border.BorderColorRight = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].Border.BorderColorLeft = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].Border.BorderColorTop = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].Border.BorderColorBottom = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].Border.BorderColorTop = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].Border.BorderColorBottom = Color.White;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].Border.BorderColorTop = Color.White;
    //                }
    //                if (span_balanc == 0)
    //                {
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col - 100].Text = "Regulation:";
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col - 100].Text = "Semester Number:";
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col - 100].Text = "Total Conducted Hours:";
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col - 100].HorizontalAlign = HorizontalAlign.Right;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col - 100].HorizontalAlign = HorizontalAlign.Right;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col - 100].HorizontalAlign = HorizontalAlign.Right;
    //                }
    //                else
    //                {
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].Text = "Regulation:";
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].Text = "Semester Number:";
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].Text = "Total Conducted Hours:";
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].HorizontalAlign = HorizontalAlign.Right;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].HorizontalAlign = HorizontalAlign.Right;
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].HorizontalAlign = HorizontalAlign.Right;
    //                }
    //                subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].ColumnSpan = span_balanc;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].ColumnSpan = span_balanc;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].ColumnSpan = span_balanc;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].Border.BorderColorRight = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].Border.BorderColorLeft = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + span_col].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].Border.BorderColorTop = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + span_col].Border.BorderColorBottom = Color.White;
    //                subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + span_col].Border.BorderColorTop = Color.White;
    //            }
    //            else
    //            {
    //                //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, visi_col, 1, visi_col3-1);
    //                //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, visi_col, 1, visi_col3-1);
    //                //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, visi_col, 1, visi_col3-1);
    //                //subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + visi_col3-1].Text = "Regulation:";
    //                //subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + visi_col3-1].Text = "Semester Number:";
    //                //subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + visi_col3-1].Text = "Total Conducted Hours:";
    //                //subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col + visi_col3].HorizontalAlign = HorizontalAlign.Right;
    //                //subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col + visi_col3].HorizontalAlign = HorizontalAlign.Right;
    //                //subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col + visi_col3].HorizontalAlign = HorizontalAlign.Right;
    //            }
    //        }
    //        //=====================================================
    //    }
    //    subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
    //    subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
    //    subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    //    subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;
    //    int temp_count_temp = 0;
    //    string[] header_align_index;
    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //        {
    //            header_align_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString().Split(',');
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, end_column].Border.BorderColorBottom = Color.White;
    //            subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col].Border.BorderColorBottom = Color.White;
    //            for (int row_head_count = 9; row_head_count < (9 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //            {
    //                subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Text = new_header_string_split[temp_count_temp].ToString();
    //                if (final_print_col_cnt > 3)
    //                {
    //                    //  subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, start_column, 1, (subject_spread.Sheets[0].ColumnCount - start_column + 1));
    //                    int dd = (subject_spread.Sheets[0].ColumnCount - start_column + 1);
    //                    int span_col = 0;
    //                    if (dd >= 100)
    //                    {
    //                        int span_col_count = 0, span_balanc = 0;
    //                        span_col_count = dd / 100;
    //                        span_balanc = dd % 100;
    //                        for (span_col = 0; span_col <= dd - 100; span_col += 100)
    //                        {
    //                            subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column + span_col].ColumnSpan = 100;
    //                        }
    //                        subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column + span_col].ColumnSpan = span_balanc;
    //                    }
    //                    else
    //                    {
    //                        subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, start_column, 1, (subject_spread.Sheets[0].ColumnCount - start_column + 1));
    //                    }
    //                }
    //                subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;
    //                if (row_head_count != (9 + new_header_string_split.GetUpperBound(0)))
    //                {
    //                    subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorBottom = Color.White;
    //                }
    //                if (temp_count_temp <= header_align_index.GetUpperBound(0))
    //                {
    //                    if (header_align_index[temp_count_temp].ToString() != string.Empty)
    //                    {
    //                        header_alignment = header_align_index[temp_count_temp].ToString();
    //                        if (header_alignment == "2")
    //                        {
    //                            subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Center;
    //                        }
    //                        else if (header_alignment == "1")
    //                        {
    //                            subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Left;
    //                        }
    //                        else
    //                        {
    //                            subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Right;
    //                        }
    //                    }
    //                }
    //                temp_count_temp++;
    //            }
    //        }
    //    }
    //}

    //public void header_text()
    //{
    //    bool check_print_row = false;
    //    SqlDataReader dr_collinfo;
    //    con.Close();
    //    con.Open();
    //    cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header from print_master_setting  where form_name='singlesubjectwise_splitup_attnd_report.aspx'", con);
    //    dr_collinfo = cmd.ExecuteReader();
    //    while (dr_collinfo.Read())
    //    {
    //        if (dr_collinfo.HasRows == true)
    //        {
    //            check_print_row = true;
    //            coll_name = dr_collinfo["collname"].ToString();
    //            address1 = dr_collinfo["address1"].ToString();
    //            address2 = dr_collinfo["address2"].ToString();
    //            address3 = dr_collinfo["address3"].ToString();
    //            phoneno = dr_collinfo["phoneno"].ToString();
    //            faxno = dr_collinfo["faxno"].ToString();
    //            email = dr_collinfo["email"].ToString();
    //            website = dr_collinfo["website"].ToString();
    //            form_name = dr_collinfo["form_name"].ToString();
    //            degree_deatil = dr_collinfo["degree_deatil"].ToString();
    //            header_alignment = dr_collinfo["header_alignment"].ToString();
    //            view_header = dr_collinfo["view_header"].ToString();
    //        }
    //    }
    //    if (check_print_row == false)
    //    {
    //        con.Close();
    //        con.Open();
    //        cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["collegecode"] + "", con);
    //        dr_collinfo = cmd.ExecuteReader();
    //        while (dr_collinfo.Read())
    //        {
    //            if (dr_collinfo.HasRows == true)
    //            {
    //                string sec_val = string.Empty;
    //                if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null)
    //                {
    //                    sec_val = "Section: " + ddlsec.SelectedItem.ToString();
    //                }
    //                else
    //                {
    //                    sec_val = string.Empty;
    //                }
    //                check_print_row = true;
    //                coll_name = dr_collinfo["collname"].ToString();
    //                address1 = dr_collinfo["address1"].ToString();
    //                address2 = dr_collinfo["address2"].ToString();
    //                address3 = dr_collinfo["address3"].ToString();
    //                phoneno = dr_collinfo["phoneno"].ToString();
    //                faxno = dr_collinfo["faxno"].ToString();
    //                email = dr_collinfo["email"].ToString();
    //                website = dr_collinfo["website"].ToString();
    //                form_name = "Subject Wise Attendance Details – Splitup Report ";
    //                degree_deatil = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
    //                // header_alignment = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
    //                // view_header = dr_collinfo["view_header"].ToString();
    //            }
    //        }
    //    }
    //}

    //public void view_header_setting()
    //{
    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        ddlpage.Visible = true;
    //        lblpages.Visible = true;
    //        view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
    //        view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
    //        view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //        if (view_header == "0" || view_header == "1")
    //        {
    //            errmsg.Visible = false;
    //            for (int row_cnt = 0; row_cnt < subject_spread.Sheets[0].ColumnHeader.RowCount; row_cnt++)
    //            {
    //                subject_spread.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
    //            }
    //            int i = 0;
    //            ddlpage.Items.Clear();
    //            int totrowcount = subject_spread.Sheets[0].RowCount;
    //            int pages = totrowcount / 25;
    //            int intialrow = 1;
    //            int remainrows = totrowcount % 25;
    //            if (subject_spread.Sheets[0].RowCount > 0)
    //            {
    //                int i5 = 0;
    //                ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    //                for (i = 1; i <= pages; i++)
    //                {
    //                    i5 = i;
    //                    ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //                    intialrow = intialrow + 25;
    //                }
    //                if (remainrows > 0)
    //                {
    //                    i = i5 + 1;
    //                    ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //                }
    //            }
    //            // if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //            //{
    //            //    for (i = 0; i < subject_spread.Sheets[0].RowCount; i++)
    //            //    {
    //            //        subject_spread.Sheets[0].Rows[i].Visible = true;
    //            //    }
    //            //    double totalRows = 0;
    //            //    totalRows = Convert.ToInt32(subject_spread.Sheets[0].RowCount);
    //            //    Session["totalPages"] = (int)Math.Ceiling(totalRows / subject_spread.Sheets[0].PageSize);
    //            //    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //            //    DropDownListpage.Items.Clear();
    //            //    if (totalRows >= 10)
    //            //    {
    //            //        subject_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            //        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //            //        {
    //            //            DropDownListpage.Items.Add((k + 10).ToString());
    //            //        }
    //            //        DropDownListpage.Items.Add("Others");
    //            //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            //        subject_spread.Height = 335;
    //            //    }
    //            //    else if (totalRows == 0)
    //            //    {
    //            //        DropDownListpage.Items.Add("0");
    //            //        subject_spread.Height = 100;
    //            //    }
    //            //    else
    //            //    {
    //            //        subject_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            //        DropDownListpage.Items.Add(subject_spread.Sheets[0].PageSize.ToString());
    //            //        subject_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //            //    }
    //            //    if (Convert.ToInt32(subject_spread.Sheets[0].RowCount) > 10)
    //            //    {
    //            //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            //        subject_spread.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //            //        subject_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //            //        CalculateTotalPages();
    //            //    }
    //            //    pnl_pagesetting.Visible = true;
    //            //}
    //            //else
    //            //{
    //            //    errmsg.Visible = false;
    //            //    pnl_pagesetting.Visible = false;
    //            //}
    //        }
    //        else if (view_header == "2")
    //        {
    //            for (int row_cnt = 0; row_cnt < subject_spread.Sheets[0].ColumnHeader.RowCount; row_cnt++)
    //            {
    //                subject_spread.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
    //            }
    //            errmsg.Visible = false;
    //            int i = 0;
    //            ddlpage.Items.Clear();
    //            int totrowcount = subject_spread.Sheets[0].RowCount;
    //            int pages = totrowcount / 25;
    //            int intialrow = 1;
    //            int remainrows = totrowcount % 25;
    //            if (subject_spread.Sheets[0].RowCount > 0)
    //            {
    //                int i5 = 0;
    //                ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    //                for (i = 1; i <= pages; i++)
    //                {
    //                    i5 = i;
    //                    ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //                    intialrow = intialrow + 25;
    //                }
    //                if (remainrows > 0)
    //                {
    //                    i = i5 + 1;
    //                    ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //                }
    //            }
    //            // if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //            //{
    //            //    for (i = 0; i < subject_spread.Sheets[0].RowCount; i++)
    //            //    {
    //            //        subject_spread.Sheets[0].Rows[i].Visible = true;
    //            //    }
    //            //    double totalRows = 0;
    //            //    totalRows = Convert.ToInt32(subject_spread.Sheets[0].RowCount);
    //            //    Session["totalPages"] = (int)Math.Ceiling(totalRows / subject_spread.Sheets[0].PageSize);
    //            //    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //            //    DropDownListpage.Items.Clear();
    //            //    if (totalRows >= 10)
    //            //    {
    //            //        subject_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            //        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //            //        {
    //            //            DropDownListpage.Items.Add((k + 10).ToString());
    //            //        }
    //            //        DropDownListpage.Items.Add("Others");
    //            //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            //        subject_spread.Height = 335;
    //            //    }
    //            //    else if (totalRows == 0)
    //            //    {
    //            //        DropDownListpage.Items.Add("0");
    //            //        subject_spread.Height = 100;
    //            //    }
    //            //    else
    //            //    {
    //            //        subject_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            //        DropDownListpage.Items.Add(subject_spread.Sheets[0].PageSize.ToString());
    //            //        subject_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //            //    }
    //            //    if (Convert.ToInt32(subject_spread.Sheets[0].RowCount) > 10)
    //            //    {
    //            //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            //        subject_spread.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //            //        //  subject_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //            //        CalculateTotalPages();
    //            //    }
    //            //    pnl_pagesetting.Visible = true;
    //            //}
    //            //else
    //            //{
    //            //    pnl_pagesetting.Visible = false;
    //            //}
    //        }
    //        else
    //        {
    //        }
    //    }
    //    else
    //    {
    //        lblpages.Visible = false;
    //        ddlpage.Visible = false;
    //    }
    //}

    #endregion


    public void getspecial_hr()
    {
        try
        {
            string hrdetno = string.Empty;
            if (ht_sphr.Contains(Convert.ToString(temp_date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(temp_date), ht_sphr));
            }
            if (hrdetno != "")
            {
                // bool spl_hr_flag = false;
                //  string spl_hr_attnd_value = string.Empty;
                con_splhr_query_master.Close();
                con_splhr_query_master.Open();
                DataSet ds_splhr_query_master = new DataSet();
                //  no_stud_flag = false;
                // string splhr_query_master = "select r.roll_no , attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no=(select hrentry_no from  specialhr_master where batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and date='" + temp_date + "') and subject_no='" + subject_no + "') and r.roll_no=sa.roll_no and batch_year=" + ddlbatch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar')  order by r.roll_no asc";
                string splhr_query_master = "select spa.roll_no,spa.attendance from  specialhr_attendance spa,specialhr_details spd where spa.hrdet_no=spd.hrdet_no and spd.subject_no='" + subject_no + "'  and spd.hrdet_no in(" + hrdetno + ") order by spa.roll_no asc";
                SqlDataReader dr_splhr_query_master;
                cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
                dr_splhr_query_master = cmd.ExecuteReader();
                while (dr_splhr_query_master.Read())
                {
                    if (dr_splhr_query_master.HasRows)
                    {
                        //{
                        //gotolable:
                        //if (roll_count < subject_spread.Sheets[0].RowCount)
                        //{
                        if (hatsplhrattendance.Contains(dr_splhr_query_master[0].ToString()))
                        {
                            roll_count = Convert.ToInt32(GetCorrespondingKey(dr_splhr_query_master[0].ToString(), hatsplhrattendance));
                            recflag = true;
                            string stdroll_no = Convert.ToString(dicstdroll[roll_count]);

                            if (stdroll_no.Trim() == dr_splhr_query_master[0].ToString().Trim())
                            {
                                //subject_spread.Sheets[0].Rows[roll_count].Visible = true;
                                if ((dr_splhr_query_master[1].ToString()) != "8")
                                {
                                    if (Attmark(dr_splhr_query_master[1].ToString()) != "HS")
                                    {
                                        if (has_attnd_masterset.ContainsKey((dr_splhr_query_master[1].ToString())))
                                        {
                                            present_count = Convert.ToInt16(has_load_rollno[dr_splhr_query_master[0].ToString()].ToString());
                                            present_count++;
                                            has_load_rollno[stdroll_no] = present_count;
                                            atndsplhr++;
                                        }
                                        if (Attmark(dr_splhr_query_master[1].ToString()) != "NE")
                                        {
                                            present_count = Convert.ToInt16(has_total_attnd_hour[dr_splhr_query_master[0].ToString()].ToString());
                                            present_count++;
                                            has_total_attnd_hour[stdroll_no] = present_count;
                                            atndsplhr++;
                                        }
                                    }
                                }
                            }
                            //else if (subject_spread.Sheets[0].Cells[roll_count - 1, 1].Text.Trim() == dr_splhr_query_master[0].ToString().Trim())
                            //{
                            //    if ((dr_splhr_query_master[1].ToString()) != "8")
                            //    {
                            //        if (Attmark(dr_splhr_query_master[1].ToString()) != "HS")
                            //        {
                            //            if (has_attnd_masterset.ContainsKey((dr_splhr_query_master[1].ToString())))
                            //            {
                            //                present_count = Convert.ToInt16(has_load_rollno[dr_splhr_query_master[0].ToString()].ToString());
                            //                present_count++;
                            //                has_load_rollno[subject_spread.Sheets[0].Cells[roll_count - 1, 1].Text] = present_count;
                            //            }
                            //            if (Attmark(dr_splhr_query_master[1].ToString()) != "NE")
                            //            {
                            //                present_count = Convert.ToInt16(has_total_attnd_hour[dr_splhr_query_master[0].ToString()].ToString());
                            //                present_count++;
                            //                has_total_attnd_hour[subject_spread.Sheets[0].Cells[roll_count - 1, 1].Text] = present_count;
                            //            }
                            //        }
                            //    }
                            //    roll_count = roll_count - 1;
                            //}
                            //else
                            //{
                            //    roll_count++;
                            //    if (roll_count < subject_spread.Sheets[0].RowCount)
                            //    {
                            //        goto gotolable;
                            //    }
                            //    else
                            //    {
                            //        break;
                            //    }
                            //}
                        }
                        //else
                        //{
                        //    break;
                        //}
                        //}
                        //roll_count++;
                    }
                }
            }
        }
        catch
        {
        }
    }


    public void getspecial_hr1()
    {
        try
        {
            string hrdetno = string.Empty;
            if (ht_sphr.Contains(Convert.ToString(temp_date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(temp_date), ht_sphr));
            }
            if (hrdetno != "")
            {

                con_splhr_query_master.Close();
                con_splhr_query_master.Open();
                DataSet ds_splhr_query_master = new DataSet();

                // string splhr_query_master = "select spa.roll_no,spa.attendance from  specialhr_attendance spa,specialhr_details spd where spa.hrdet_no=spd.hrdet_no and spd.subject_no='" + subject_no + "'  and spd.hrdet_no in(" + hrdetno + ") order by spa.roll_no asc";
                string splhr_query_master = "select attendance,sa.hrdet_no,CONVERT(VARCHAR(5),start_time,108) as start_time, CONVERT(VARCHAR(5),end_time,108) as end_time from specialhr_attendance sa,specialhr_details sd where roll_no='" + roll_no + "' and  sd.subject_no='" + subject_no + "'  and sa.hrdet_no in(" + hrdetno + ") and sd.hrdet_no=sa.hrdet_no";
                SqlDataReader dr_splhr_query_master;
                cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
                dr_splhr_query_master = cmd.ExecuteReader();
                ds_splhr_query_master = dacc.select_method_wo_parameter(splhr_query_master, "text");
                while (dr_splhr_query_master.Read())
                {
                    if (dr_splhr_query_master.HasRows)
                    {
                        for (int j = 0; j < ds_splhr_query_master.Tables[0].Rows.Count; j++)
                        {
                            totsplhr++;
                            string atnd = Convert.ToString(ds_splhr_query_master.Tables[0].Rows[j]["attendance"]);
                            if (atnd == "1")
                            {
                                atndsplhr++;
                            }
                            if (atnd == "2")
                            {
                                abssplhr++;
                            }
                            if (atnd == "3")
                            {
                                odspl++;
                            }



                        }

                        //if (hatsplhrattendance.Contains(dr_splhr_query_master[0].ToString()))
                        //{
                        //    recflag = true;
                        //    if (roll_no.Trim() == dr_splhr_query_master[0].ToString().Trim())
                        //    {
                        //        //subject_spread.Sheets[0].Rows[roll_count].Visible = true;
                        //        if ((dr_splhr_query_master[1].ToString()) != "8")
                        //        {
                        //            if (Attmark(dr_splhr_query_master[1].ToString()) != "HS")
                        //            {
                        //                if (has_attnd_masterset.ContainsKey((dr_splhr_query_master[1].ToString())))
                        //                {
                        //                   // present_count = Convert.ToInt16(has_load_rollno[dr_splhr_query_master[0].ToString()].ToString());
                        //                    present_count++;
                        //                   // has_load_rollno[stdroll_no] = present_count;
                        //                    atndsplhr++;
                        //                }
                        //                if (Attmark(dr_splhr_query_master[1].ToString()) != "NE")
                        //                {
                        //                    //present_count = Convert.ToInt16(has_total_attnd_hour[dr_splhr_query_master[0].ToString()].ToString());
                        //                    present_count++;
                        //                  //  has_total_attnd_hour[stdroll_no] = present_count;
                        //                    atndsplhr++;
                        //                }
                        //            }
                        //        }
                        //    }

                        //}

                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        string sections = ddlsec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = string.Empty;
        }
        else
        {
            sections = "- Sec-" + sections;
        }
        //  string Conduct = subject_spread.Sheets[0].ColumnHeader.Cells[0, (subject_spread.Sheets[0].ColumnCount - 1)].Note;
        //string value = "@Conducted Periods : " + Conduct + "";
        string ss = null;
        string subname_ptn = ddlsubject.SelectedItem.Text;
        string subcode_ptn = dacc.GetFunction("Select Subject_code from subject where subject_no='" + ddlsubject.SelectedItem.Value + "'");
        subname_ptn = "@Subject Name : " + subname_ptn + "," + "@Subject code : " + subcode_ptn;
        // subcode_ptn = "@subject_no:" + subcode_ptn;
        string degreedetails = "SUBJECT WISE ATTENDANCE DETAILS - SPLITUP REPORT" + '@' + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlduration.SelectedItem.ToString() + sections + '@' + "Period :" + txtFromDate.Text.ToString() + " to " + txtToDate.Text.ToString() + " " + subname_ptn + "";
        string pagename = "singlesubjectwise_splitup_attnd_report.aspx";
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }



}
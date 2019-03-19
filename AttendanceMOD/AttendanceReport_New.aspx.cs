using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;

public partial class AttendanceReport_New : System.Web.UI.Page
{

    string regularflag = "", new_header_string = "", new_header_string_index = "";
    string genderflag = "";

    //saravana strat 
    int mmyycount;
    int days1 = 0;
    string dd = "";
    static Boolean splhr_flag = false;

    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    static Hashtable ht_sphr = new Hashtable();
    static Hashtable hasdaywise = new Hashtable();
    static Hashtable hashrwise = new Hashtable();
    Hashtable hat = new Hashtable();


    ArrayList al_pdf = new ArrayList();
    ArrayList al_header = new ArrayList();
    ArrayList al_startdate = new ArrayList();
    ArrayList al_examdate = new ArrayList();

    DataTable dt_stud_details = new DataTable();
    DataTable dt_criteria = new DataTable();

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet dsprint = new DataSet();
    DataSet ds_attnd_pts = new DataSet();
    DataSet ds_sphr = new DataSet();
    DataSet dsholiday = new DataSet();
    DataSet ds_splhr_query_master = new DataSet();

    //===================12/6/12 PRABHA
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0, tot_ml_spl_true = 0;
    double per_leave_true = 0;
    //============================

    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0;//, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;

    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;

    int unmark;

    int check;

    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;

    DateTime dumm_from_date;
    DateTime Admission_date;
    string frdate, todate;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";

    TimeSpan ts;

    string diff_date;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;

    int count;
    int next = 0;
    int minpresII = 0;
    string value, date;
    int rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    //Opt------------
    int cal_from_date, cal_from_date_tmp;
    int cal_to_date, cal_to_date_tmp;
    //---------------
    double per_perhrs, per_abshrs;
    double per_ondu, per_leave, per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;

    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;

    double workingdays = 0;
    double per_workingdays = 0;

    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_tage_date, per_tage_hrs;
    double per_holidate;
    int per_dum_unmark, dum_unmark;
    int tot_per_hrs;
    double per_con_hrs;
    double njhr, njdate, per_njdate;
    double per_per_hrs;
    double tot_ondu, per_tot_ondu, tot_ml, per_tot_ml, totabsent;
    string usercode = "", collegecode = "", singleuser = "", group_user = "";


    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = "";

    static string grouporusercode = "";
    //opt----
    int demfcal, demtcal;
    string monthcal;
    string dum_tage_date, dum_tage_hrs;
    int stud;

    double from_percentage;
    double to_percentage;
    double dum_tage_hrs_1;
    //-----------------------------------------

    string batch_yr = string.Empty;
    string deg_code = string.Empty;
    string course_name = string.Empty;
    string section = string.Empty;
    string acronym = string.Empty;
    string rollno = string.Empty;
    string regno = string.Empty;
    string stud_name = string.Empty;
    string curr_sem = string.Empty;
    string syll_code = string.Empty;
    string start_date = string.Empty;
    string columnheader_pdf = string.Empty;
    string student = string.Empty;
    string student_1 = string.Empty;
    string degree_code = string.Empty;
    string batch_year = string.Empty;

    string pdf_date = string.Empty;
    string pef_examdate = string.Empty;

    string group_code = string.Empty;
    string columnfield = string.Empty;
    string course_id = string.Empty;

    string selected_degree = string.Empty;

    string start_date_1 = "";
    string get_todate = "";
    int col_cnt;
    DataTable data = new DataTable();
    DataRow drow;
    int colcnt = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        lbl_error.Visible = false;
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lblexcelerror.Visible = false;

        if (!IsPostBack)
        {

            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            string Strquery = "select * from Master_Settings where " + grouporusercode + "";
            DataSet dsset = d2.select_method_wo_parameter(Strquery, "Text");
            for (int i = 0; i < dsset.Tables[0].Rows.Count; i++)
            {
                if (dsset.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && dsset.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Rollflag"] = "1";
                }
                if (dsset.Tables[0].Rows[i]["settings"].ToString() == "Register No" && dsset.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Regflag"] = "1";
                }
            }

            lblnote.Visible = false;
            BindBatch();

            BindDegree(singleuser, group_user, collegecode, usercode);

            txtfdate.Attributes.Add("readonly", "readonly");
            txttdate.Attributes.Add("readonly", "readonly");
            txtfdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            rbtest.Checked = true;

            txtfdate.Visible = false;
            txttdate.Visible = false;
            lblfdate.Visible = false;
            lbltdate.Visible = false;

            if (chklst_degree.Items.Count > 0)
            {
                txt_degree.Enabled = true;
                txt_branch.Enabled = true;
                Button1.Enabled = true;
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);


            }
            else
            {
                txt_degree.Enabled = false;
                txt_branch.Enabled = false;
                Button1.Enabled = false;

            }


            Showgrid.Visible = false;
            lbl_error.ForeColor = System.Drawing.Color.Red;
            lbl_error.Visible = false;
            lblxl.Visible = false;
            txtxl.Visible = false;
            btnxl.Visible = false;

            btnprintmaster.Visible = false;
            btnPrint.Visible = false;


            string strspquery = "select rights from  special_hr_rights where " + grouporusercode + "";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strspquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string spl_hr_rights = "";
                spl_hr_rights = ds.Tables[0].Rows[0]["rights"].ToString();
                if (spl_hr_rights == "True" || spl_hr_rights == "true")
                {
                    splhr_flag = true;

                }
            }
        }
    }

    public void BindBatch()
    {
        try
        {
            chk_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_batch.DataSource = ds;
                chklst_batch.DataTextField = "Batch_year";
                chklst_batch.DataValueField = "Batch_year";
                chklst_batch.DataBind();

                for (int checkall = 0; checkall < chklst_batch.Items.Count; checkall++)
                {
                    chklst_batch.Items[checkall].Selected = true;
                }
                chk_batch.Checked = true;
                txt_batch.Text = "Batch (" + chklst_batch.Items.Count + ")";
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = ex.ToString();
            lbl_error.Visible = true;
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            chk_degree.Checked = false;
            txt_degree.Text = "---Select---";
            chklst_degree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            //ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_degree.DataSource = ds;
                chklst_degree.DataTextField = "course_name";
                chklst_degree.DataValueField = "course_id";
                chklst_degree.DataBind();
                for (int checkall = 0; checkall < chklst_degree.Items.Count; checkall++)
                {
                    chklst_degree.Items[checkall].Selected = true;
                }
                chk_degree.Checked = true;
                txt_degree.Text = "Degree (" + chklst_degree.Items.Count + ")";
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = ex.ToString();
            lbl_error.Visible = true;
        }
    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            for (int i = 0; i < chklst_degree.Items.Count; i++)
            {
                if (chklst_degree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "'" + chklst_degree.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        course_id = course_id + "," + "'" + chklst_degree.Items[i].Value.ToString() + "'";
                    }
                }
            }
            chk_branch.Checked = false;
            txt_branch.Text = "---Select---";
            chklst_branch.Items.Clear();
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
                chklst_branch.DataSource = ds;
                chklst_branch.DataTextField = "dept_name";
                chklst_branch.DataValueField = "degree_code";
                chklst_branch.DataBind();
                chklst_branch.Items[0].Selected = true;
                txt_branch.Text = "Branch (1)";
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = ex.ToString();
            lbl_error.Visible = true;
        }
    }

    protected void chk_branch_ChekedChanged(object sender, EventArgs e)
    {
        clear();
        txt_branch.Text = "---Select---";
        if (chk_branch.Checked == true)
        {
            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                chklst_branch.Items[i].Selected = true;
            }
            if (chklst_branch.Items.Count > 0)
            {
                txt_branch.Text = "Branch(" + (chklst_branch.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                chklst_branch.Items[i].Selected = false;
            }
        }
    }

    protected void chklst_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        int branchcount = 0;
        txt_branch.Text = "---Select---";
        chk_branch.Checked = false;

        for (int i = 0; i < chklst_branch.Items.Count; i++)
        {
            if (chklst_branch.Items[i].Selected == true)
            {
                branchcount = branchcount + 1;
            }
        }
        if (branchcount > 0)
        {
            txt_branch.Text = "Branch(" + branchcount.ToString() + ")";
            if (branchcount == chklst_branch.Items.Count)
            {
                chk_branch.Checked = true;
            }
        }
    }

    protected void chk_degree_ChekedChanged(object sender, EventArgs e)
    {
        clear();
        if (chk_degree.Checked == true)
        {
            for (int i = 0; i < chklst_degree.Items.Count; i++)
            {
                chklst_degree.Items[i].Selected = true;
            }
            txt_degree.Text = "Degree(" + (chklst_degree.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklst_degree.Items.Count; i++)
            {
                chklst_degree.Items[i].Selected = false;
            }
            txt_degree.Text = "---Select---";
        }
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }

    protected void chklst_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        int degreecount = 0;
        txt_degree.Text = "---Select---";
        chk_degree.Checked = false;
        for (int i = 0; i < chklst_degree.Items.Count; i++)
        {
            if (chklst_degree.Items[i].Selected == true)
            {
                degreecount = degreecount + 1;
            }
        }
        if (degreecount > 0)
        {
            txt_degree.Text = "Degree(" + degreecount + ")";
            if (degreecount == chklst_degree.Items.Count)
            {
                chk_degree.Checked = true;
            }
        }

        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }

    protected void chk_batch_ChekedChanged(object sender, EventArgs e)
    {
        clear();
        if (chk_batch.Checked == true)
        {
            for (int i = 0; i < chklst_batch.Items.Count; i++)
            {
                chklst_batch.Items[i].Selected = true;
            }
            txt_batch.Text = "Batch(" + (chklst_batch.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklst_batch.Items.Count; i++)
            {
                chklst_batch.Items[i].Selected = false;
            }
            txt_batch.Text = "---Select---";
        }
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }


    protected void chklst_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        chk_batch.Checked = false;
        txt_batch.Text = "---Select---";
        clear();
        for (int i = 0; i < chklst_batch.Items.Count; i++)
        {
            if (chklst_batch.Items[i].Selected == true)
            {
                batchcount = batchcount + 1;
            }
        }
        if (batchcount > 0)
        {
            txt_batch.Text = "Batch(" + batchcount.ToString() + ")";
            if (batchcount == chklst_batch.Items.Count)
            {
                chk_batch.Checked = true;
            }
        }
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {


            clear();
            btnPrint11();
            Double frange = 0;
            Double trange = 0;
            string fr = txtfrom.Text.ToString();
            string tr = txtto.Text.ToString();
            if (fr.Trim() != "" && fr != null)
            {
                frange = Convert.ToDouble(fr);
                if (frange > 100)
                {
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Enter The From Range Less Than or Equal To 100";
                    return;
                }
            }
            if (tr.Trim() != "" && tr != null)
            {
                trange = Convert.ToDouble(tr);
                if (trange > 100)
                {
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Enter The To Range Less Than or Equal To 100";
                    return;
                }
            }

            if (fr.Trim() != "" && fr != null && tr.Trim() == "")
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Please Enter The To Range";
                return;
            }
            if (tr.Trim() != "" && tr != null && fr.Trim() == "")
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Please Enter The From Range";
                return;
            }
            if (tr.Trim() == "" || tr == null || fr.Trim() == "" || fr == null)
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Please Enter The From And To Range";
                return;
            }
            if (trange < frange)
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Please Enter The From Range Must Be Lesser Than or Equal To Range";
                return;
            }
            if (rboverall.Checked == true)
            {
                loadoverallattendance();
            }
            else
            {
                int col_count = 4;

                ArrayList arrColHdrNames1 = new ArrayList();
                arrColHdrNames1.Add("S.No");
                arrColHdrNames1.Add("Branch/Section");
                data.Columns.Add("S.No", typeof(string));
                data.Columns.Add("Branch/Section", typeof(string));
                if (Session["Rollflag"].ToString() == "1")
                {
                    arrColHdrNames1.Add("RollNo");
                    data.Columns.Add("Roll No", typeof(string));
                    colcnt++;
                }
                if (Session["Regflag"].ToString() == "1")
                {
                    arrColHdrNames1.Add("Reg No");
                    data.Columns.Add("Reg No", typeof(string));
                    colcnt++;
                }
                arrColHdrNames1.Add("Name of the Students");
                data.Columns.Add("Name of the Students", typeof(string));

                colcnt = colcnt + 3;

                for (int cnt_getbatch = 0; cnt_getbatch < chklst_batch.Items.Count; cnt_getbatch++)
                {
                    if (chklst_batch.Items[cnt_getbatch].Selected == true)
                    {
                        if (batch_year == "")
                        {
                            batch_year = chklst_batch.Items[cnt_getbatch].Value;
                        }
                        else
                        {
                            batch_year = batch_year + "," + chklst_batch.Items[cnt_getbatch].Value;
                        }
                    }
                }

                if (batch_year.Trim() == "")
                {
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select The Batch And then Proceed";
                    return;
                }

                for (int cnt_getdegree = 0; cnt_getdegree < chklst_branch.Items.Count; cnt_getdegree++)
                {
                    if (chklst_branch.Items[cnt_getdegree].Selected == true)
                    {
                        if (degree_code == "")
                        {
                            degree_code = chklst_branch.Items[cnt_getdegree].Value;
                        }
                        else
                        {
                            degree_code = degree_code + "," + chklst_branch.Items[cnt_getdegree].Value;
                        }
                    }
                }

                string strdegree = "";
                if (degree_code.Trim() == "")
                {
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select The Degree And Branch And then Proceed";
                    return;
                }



                dt_criteria = d2.select_method_wop_table("select distinct criteria from registration r ,syllabus_master s,criteriaforinternal c,exam_type e where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.current_semester=s.semester and cc=0 and exam_flag<>'debar' and delflag=0 and s.syll_code=c.syll_code and c.criteria_no=e.criteria_no and r.degree_code in(" + degree_code + ") and r.batch_year in(" + batch_year + ") order by criteria", "Text");

                //added By Srinath 15/8/2013
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                string strorder = ",r.roll_no";
                if (orderby_Setting == "0")
                {
                    strorder = ",r.roll_no";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = ",r.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = ",r.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = ",r.roll_no,r.Reg_No,r.stud_name";
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
                    strorder = " ,r.roll_no,r.Stud_Name";
                }

                dt_stud_details = d2.select_method_wop_table("select distinct r.batch_year,r.degree_code,course_name,r.sections,acronym,roll_no,reg_no,stud_name,r.adm_date,current_semester,syll_code,start_date from registration r,course c,department dp,degree d,syllabus_master s,seminfo si where c.course_id=d.course_id and dp.dept_code=d.dept_code and r.degree_code=d.degree_code and r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.current_semester=s.semester and cc=0 and exam_flag<>'debar' and delflag=0 and r.degree_code=si.degree_code and r.batch_year=si.batch_year and r.current_semester=si.semester and r.degree_code in(" + degree_code + ") and r.batch_year in(" + batch_year + ") order by r.batch_year,r.degree_code,r.sections " + strorder + "", "Text");
                int dtcriteria_rowcount = dt_criteria.Rows.Count;



                for (int load_header = 0; load_header < dt_criteria.Rows.Count; load_header++)
                {
                    col_count++;

                    string header = dt_criteria.Rows[load_header]["criteria"].ToString();

                    data.Columns.Add(header, typeof(string));
                    arrColHdrNames1.Add(header);
                    al_header.Add(dt_criteria.Rows[load_header]["criteria"].ToString());
                }



                //modified by srinath 6/1/2014
                arrColHdrNames1.Add("Cumulative Percentage");
                data.Columns.Add("Cumulative Percentage", typeof(string));

                arrColHdrNames1.Add("No Of Students");
                data.Columns.Add("No Of Students", typeof(string));

                arrColHdrNames1.Add("Remarks");
                data.Columns.Add("Remarks", typeof(string));



                DataRow drHdr1 = data.NewRow();
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    drHdr1[grCol] = arrColHdrNames1[grCol];

                data.Rows.Add(drHdr1);


                string stud_count_value = string.Empty;

                int totstudent = 0;

                DataTable dt = new DataTable();
                DataColumn dc;

                dc = new DataColumn();
                dc.ColumnName = "Dept";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.ColumnName = "Count";
                dt.Columns.Add(dc);

                DataRow dr;



                string student_dept = string.Empty;
                int sprd_cnt = 0;
                int sno = 0;
                for (stud = 0; stud < dt_stud_details.Rows.Count; stud++)
                {
                    sno++;
                    Boolean studvisable = false;
                    batch_yr = dt_stud_details.Rows[stud]["batch_year"].ToString();
                    deg_code = dt_stud_details.Rows[stud]["degree_code"].ToString();
                    course_name = dt_stud_details.Rows[stud]["course_name"].ToString();
                    section = dt_stud_details.Rows[stud]["sections"].ToString();
                    acronym = dt_stud_details.Rows[stud]["acronym"].ToString();
                    rollno = dt_stud_details.Rows[stud]["roll_no"].ToString();
                    regno = dt_stud_details.Rows[stud]["reg_no"].ToString();
                    stud_name = dt_stud_details.Rows[stud]["stud_name"].ToString();
                    curr_sem = dt_stud_details.Rows[stud]["current_semester"].ToString();
                    syll_code = dt_stud_details.Rows[stud]["syll_code"].ToString();
                    start_date = dt_stud_details.Rows[stud]["start_date"].ToString();




                    if (al_startdate.Contains(dt_stud_details.Rows[stud]["start_date"].ToString()) == false)
                    {
                        al_startdate.Add(dt_stud_details.Rows[stud]["start_date"].ToString());
                    }

                    if (section != "")
                    {
                        student = batch_yr + "-" + course_name + "[" + acronym + "]" + "-" + curr_sem + "-" + section;
                    }
                    else
                    {
                        student = batch_yr + "-" + course_name + "[" + acronym + "]" + "-" + curr_sem;
                    }
                    drow = data.NewRow();
                    drow["S.No"] = sno.ToString();
                    drow["Branch/Section"] = student.ToString();
                    if (Session["Rollflag"].ToString() == "1")
                        drow["Roll No"] = rollno.ToString();
                    if (Session["Regflag"].ToString() == "1")
                        drow["Reg No"] = regno.ToString();
                    drow["Name of the Students"] = stud_name.ToString();

                    data.Rows.Add(drow);

                    if (student_1 == "")
                    {
                        if (al_pdf.Contains(student_1) == false)
                        {
                            if (section != "")
                            {
                                student_1 = batch_yr + "-" + course_name + "[" + acronym + "]" + "-" + curr_sem + "-" + section;
                                al_pdf.Add(student_1);
                            }
                            else
                            {
                                student_1 = batch_yr + "-" + course_name + "[" + acronym + "]" + "-" + curr_sem;
                                al_pdf.Add(student_1);
                            }
                        }
                    }
                    else
                    {
                        if (al_pdf.Contains(student_1) == false)
                        {
                            if (section != "")
                            {
                                student_1 = batch_yr + "-" + course_name + "[" + acronym + "]" + "-" + curr_sem + "-" + section;

                                al_pdf.Add(student_1);
                            }

                            else
                            {
                                student_1 = batch_yr + "-" + course_name + "[" + acronym + "]" + "-" + curr_sem;
                                al_pdf.Add(student_1);
                            }
                        }

                    }




                    btnprintmaster.Visible = true;
                    btnPrint.Visible = true;

                    string startdate = dt_stud_details.Rows[stud]["start_date"].ToString();
                    string[] split_date = start_date.Split(new char[] { '/' });
                    //string time = split_date[2];
                    string[] rjct_time = split_date[2].Split(new char[] { ' ' });
                    string day = rjct_time[0];
                    start_date_1 = split_date[1] + "/" + split_date[0] + "/" + day;
                    int i = 0;
                    int i1 = 0;
                    int col_incre = 4;
                    Double attpercentage = 0;
                    Double contegtest = 0;
                    int cnt = colcnt;
                    for (col_cnt = colcnt; col_cnt < data.Columns.Count - 3; col_cnt++)
                    {
                        i1++;
                        col_incre++;
                        string test_name = "";
                        test_name = data.Columns[col_cnt].ColumnName;

                        DataTable dt_examdate = d2.select_method_wop_table("select min(exam_date) as exam_date from criteriaforinternal c,exam_type e where criteria='" + test_name + "' and syll_code='" + syll_code + "' and c.criteria_no=e.criteria_no", "Text");

                        if (dt_examdate.Rows[0]["exam_date"].ToString() != "")
                        {
                            contegtest = contegtest + 100;
                            DateTime exam_mindate = Convert.ToDateTime(dt_examdate.Rows[0]["exam_date"].ToString());
                            DateTime todate = exam_mindate.AddDays(-1);


                            string todate_1 = todate.ToString();
                            string[] split_todate = todate_1.Split(new char[] { '/' });
                            string[] rjct_todate = split_todate[2].Split(new char[] { ' ' });
                            string day_to = rjct_todate[0];
                            get_todate = split_todate[1] + "/" + split_todate[0] + "/" + day_to;

                            if (al_examdate.Contains(dt_examdate.Rows[0]["exam_date"].ToString()) == false)
                            {
                                al_examdate.Add(dt_examdate.Rows[0]["exam_date"].ToString());
                            }

                            persentmonthcal();


                            //=============taken from Load_students()
                            per_tage_date = ((pre_present_date / per_workingdays) * 100);
                            if (per_tage_date > 100)
                            {
                                per_tage_date = 100;
                            }

                            per_con_hrs = per_workingdays1; //added on 08.08.12//my

                            per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100);
                            if (per_tage_hrs > 100)
                            {
                                per_tage_hrs = 100;
                            }
                            dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
                            dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));

                            if (dum_tage_hrs == "NaN")
                            {
                                dum_tage_hrs = "0";
                            }
                            else if (dum_tage_hrs == "Infinity")
                            {
                                dum_tage_hrs = "0";
                            }
                            if (dum_tage_date == "NaN")
                            {
                                dum_tage_date = "0";
                            }
                            else if (dum_tage_date == "Infinity")
                            {
                                dum_tage_date = "0";
                            }
                            //==========================================
                            if (txtfrom.Text != "" && txtto.Text != "")
                            {
                                dum_tage_hrs_1 = Convert.ToDouble(dum_tage_hrs);
                                from_percentage = Convert.ToDouble(txtfrom.Text.ToString());
                                to_percentage = Convert.ToDouble(txtto.Text.ToString());

                                if (from_percentage < to_percentage)
                                {

                                    if (stud == dt_stud_details.Rows.Count - 1)
                                    {
                                        //saran
                                        int student_cnt = data.Rows.Count - 1;
                                        student_cnt = student_cnt - sprd_cnt;
                                        dr = dt.NewRow();
                                        dr["Dept"] = student_dept;
                                        //if (student_cnt > 7)
                                        //{
                                        //    student_cnt++;
                                        //}
                                        dr["Count"] = student_cnt.ToString();
                                        dt.Rows.Add(dr);
                                        sprd_cnt = sprd_cnt + student_cnt;
                                    }
                                    else if (student_dept != student && student_dept != "")
                                    {
                                        //saran
                                        int student_cnt = data.Rows.Count - 2;
                                        student_cnt = student_cnt - sprd_cnt;
                                        dr = dt.NewRow();
                                        dr["Dept"] = student_dept;

                                        dr["Count"] = student_cnt.ToString();
                                        dt.Rows.Add(dr);
                                        sprd_cnt = sprd_cnt + student_cnt;

                                    }
                                    student_dept = student;

                                    if (col_cnt == cnt && dum_tage_hrs_1 >= from_percentage && dum_tage_hrs_1 <= to_percentage)
                                    {
                                        i++;
                                    }
                                    else if (col_cnt == col_incre && dum_tage_hrs_1 >= from_percentage && dum_tage_hrs_1 <= to_percentage && i != 1)
                                    {
                                        i++;
                                    }

                                    if (dum_tage_hrs_1 >= from_percentage && dum_tage_hrs_1 <= to_percentage)
                                    {
                                        studvisable = true;

                                        data.Rows[data.Rows.Count - 1][col_cnt] = dum_tage_hrs.ToString();

                                        lbl_error.Visible = false;
                                        lblxl.Visible = true;
                                        txtxl.Visible = true;
                                        btnxl.Visible = true;
                                    }
                                }
                                else
                                {
                                    lbl_error.Text = "From Percentage Must be Lesser than To Percentage";
                                    lbl_error.Visible = true;
                                    Showgrid.Visible = false;
                                    lblnote.Visible = false;
                                }
                            }
                            else
                            {
                                if (i1 == 1)
                                {
                                    if (student_dept == student || student_dept == "")
                                    {
                                        totstudent++;
                                        student_dept = student;


                                        if (stud == dt_stud_details.Rows.Count - 1)
                                        {
                                            dr = dt.NewRow();
                                            dr["Dept"] = student_dept;

                                            dr["Count"] = totstudent;
                                            dt.Rows.Add(dr);

                                        }
                                    }

                                    else
                                    {
                                        dr = dt.NewRow();
                                        dr["Dept"] = student_dept;

                                        dr["Count"] = totstudent;
                                        dt.Rows.Add(dr);

                                        totstudent = 0;

                                        totstudent++;
                                        student_dept = student;
                                    }
                                }

                                if (col_cnt == 5 && dum_tage_hrs_1 >= from_percentage && dum_tage_hrs_1 <= to_percentage)
                                {

                                    i++;

                                }
                                else if (col_cnt == col_incre && dum_tage_hrs_1 >= from_percentage && dum_tage_hrs_1 <= to_percentage && i != 1)
                                {

                                    i++;
                                }
                                studvisable = true;

                                data.Rows[data.Rows.Count - 1][col_cnt] = dum_tage_hrs.ToString();


                                lblxl.Visible = true;
                                txtxl.Visible = true;
                                btnxl.Visible = true;
                                lbl_error.Visible = false;
                                //}
                            }
                            if (dum_tage_hrs.Trim() != "" && dum_tage_hrs != "0" && dum_tage_hrs != null)
                            {
                                attpercentage = attpercentage + Convert.ToDouble(dum_tage_hrs);
                            }
                        }

                    }
                    if (!studvisable)
                    {
                        data.Rows.Remove(drow);
                        sno--;
                    }
                    //*****************added by srinath**************
                    if (data.Rows.Count > 0)
                    {
                        if (attpercentage >= 0 && contegtest >= 100)
                        {
                            Double cumpercentage = 0;
                            if (attpercentage > 0)
                            {
                                cumpercentage = attpercentage / contegtest * 100;
                            }
                            if (cumpercentage > 100)
                            {
                                cumpercentage = 100;
                            }
                            cumpercentage = Math.Round(cumpercentage, 2, MidpointRounding.AwayFromZero);
                            dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(cumpercentage.ToString()));

                            data.Rows[data.Rows.Count - 1][data.Columns.Count - 3] = dum_tage_hrs.ToString();


                        }
                    }
                    attpercentage = 0;
                    contegtest = 0;
                }

                for (int a = 1; a < data.Rows.Count; a++)
                {
                    string dept_fp = data.Rows[a][1].ToString().Trim();

                    DataView dv_get_count = new DataView();

                    dt.DefaultView.RowFilter = "Dept='" + dept_fp + "'";
                    dv_get_count = dt.DefaultView;

                    int st_count = 0;

                    if (dv_get_count.Count > 0)
                    {
                        st_count = Convert.ToInt32(dv_get_count[0][1].ToString());

                    }
                    data.Rows[a][data.Columns.Count - 2] = st_count.ToString();


                }

                drow = data.NewRow();
                data.Rows.Add(drow);

                data.Rows[data.Rows.Count - 1][data.Columns.Count - 3] = "Total";

                data.Rows[data.Rows.Count - 1][data.Columns.Count - 2] = sno.ToString();

                lbl_error.Visible = false;
                lblnote.Visible = true;
                if (data.Rows.Count <= 1)
                {
                    Showgrid.Visible = false;
                    lblxl.Visible = false;
                    txtxl.Visible = false;
                    btnxl.Visible = false;
                    btnPrint.Visible = false;
                    btnprintmaster.Visible = false;
                    lbl_error.Text = "No Records Found";
                    lbl_error.Visible = true;
                    lblnote.Visible = false;

                }

                if (data.Columns.Count > 0 && data.Rows.Count > 1)
                {
                    Showgrid.DataSource = data;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;

                    Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[0].Font.Bold = true;
                    Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                    //Rowspan
                    int col = data.Columns.Count;
                    for (int t = Showgrid.Rows.Count - 1; t > 0; t--)
                    {
                        GridViewRow row = Showgrid.Rows[t];
                        GridViewRow previousRow = Showgrid.Rows[t - 1];
                        if (row.Cells[1].Text == previousRow.Cells[1].Text)
                        {
                            if (previousRow.Cells[1].RowSpan == 0)
                            {
                                if (row.Cells[1].RowSpan == 0)
                                {
                                    previousRow.Cells[1].RowSpan += 2;
                                    previousRow.Cells[col - 2].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[1].RowSpan = row.Cells[1].RowSpan + 1;
                                    previousRow.Cells[col - 2].RowSpan = row.Cells[col - 2].RowSpan + 1;
                                }
                                row.Cells[1].Visible = false;
                                row.Cells[col - 2].Visible = false;
                            }
                        }
                        //if (row.Cells[col - 2].Text == previousRow.Cells[col - 2].Text)
                        //{
                        //    string value = row.Cells[1].Text;
                        //    if (value != "0")
                        //    {
                        //        if (previousRow.Cells[col - 2].RowSpan == 0)
                        //        {
                        //            if (row.Cells[col - 2].RowSpan == 0)
                        //            {
                        //                previousRow.Cells[col - 2].RowSpan += 2;
                        //            }
                        //            else
                        //            {
                        //                previousRow.Cells[col - 2].RowSpan = row.Cells[col - 2].RowSpan + 1;
                        //            }
                        //            row.Cells[col - 2].Visible = false;
                        //        }
                        //    }
                        //}
                    }

                }

            }


            if (Showgrid.Visible == true)
            {
                lbl_error.Visible = false;
            }



            al_pdf.Clear();
            al_pdf = null;
            al_header.Clear();
            al_header = null;
            al_startdate.Clear();
            al_startdate = null;
            al_examdate.Clear();
            al_examdate = null;

            dt_stud_details.Dispose();
            dt_stud_details.Clear();
            dt_stud_details = null;
            dt_criteria.Dispose();
            dt_criteria.Clear();
            dt_criteria = null;

            ds_splhr_query_master.Dispose();
            ds_splhr_query_master.Clear();
            ds_splhr_query_master = null;
            dsholiday.Dispose();
            dsholiday.Clear();
            dsholiday = null;
            ds_sphr.Dispose();
            ds_sphr.Clear();
            ds_sphr = null;
            ds.Dispose();
            ds.Clear();
            ds = null;
            ds1.Dispose();
            ds1.Clear();
            ds1 = null;
            ds2.Dispose();
            ds2.Clear();
            ds2 = null;
            ds3.Dispose();
            ds3.Clear();
            ds3 = null;
            ds4.Dispose();
            ds4.Clear();
            ds4 = null;
            dsprint.Dispose();
            dsprint.Clear();
            dsprint = null;
            ds_attnd_pts.Dispose();
            ds_attnd_pts.Clear();
            ds_attnd_pts = null;


            hat.Clear();
            hat = null;
            holiday_table11.Clear();
            holiday_table11 = null;
            holiday_table21.Clear();
            holiday_table21 = null;
            holiday_table31.Clear();
            holiday_table31 = null;
            //ht_sphr.Clear();
            //ht_sphr = null;
            //hasdaywise.Clear();
            //hasdaywise = null;
            //hashrwise.Clear();
            //hashrwise = null;

            MyClass ms = new MyClass();
            ms.Dispose();
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }
        catch (Exception ex)
        {
            lbl_error.Text = ex.ToString();
            lbl_error.Visible = true;
        }
    }
    public class MyClass : IDisposable
    {
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // called via myClass.Dispose(). 
                    // OK to use any private object references
                }

                disposed = true;
            }
            disposed = true;
        }

        public void Dispose() // Implement IDisposable
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MyClass() // the finalizer
        {
            Dispose(false);
        }
    }

    public void persentmonthcal()
    {

        Boolean isadm = false;
        //try
        {
            totabsent = 0;
            frdate = start_date_1;
            todate = get_todate;
            string dt = frdate;
            string[] dsplit = dt.Split(new Char[] { '/' });
            frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            demfcal = int.Parse(dsplit[2].ToString());
            demfcal = demfcal * 12;
            cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
            cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());

            monthcal = cal_from_date.ToString();
            dt = todate;
            dsplit = dt.Split(new Char[] { '/' });
            todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            demtcal = int.Parse(dsplit[2].ToString());
            demtcal = demtcal * 12;
            cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
            cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());

            per_from_gendate = Convert.ToDateTime(frdate);
            per_to_gendate = Convert.ToDateTime(todate);


            int my_un_mark = 0;
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;

            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;

            notconsider_value = 0;
            conduct_hour_new = 0;

            cal_from_date = cal_from_date_tmp;
            cal_to_date = cal_to_date_tmp;
            per_from_date = per_from_gendate;
            per_to_date = per_to_gendate;
            dumm_from_date = per_from_date;

            hat.Clear();
            hat.Add("degree_code", deg_code);
            hat.Add("sem_ester", int.Parse(curr_sem));
            ds = d2.select_method("period_attnd_schedule", hat, "sp");
            if (ds.Tables[0].Rows.Count != 0)
            {
                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
            }
            hat.Clear();

            hat.Add("colege_code", Session["collegecode"].ToString());
            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
            count = ds1.Tables[0].Rows.Count;


            string admdate = dt_stud_details.Rows[stud]["adm_date"].ToString();
            string[] split_date_time = admdate.Split(new Char[] { ' ' });
            string[] admdatesp = split_date_time[0].Split(new Char[] { '/' });
            if (admdatesp.GetUpperBound(0) >= 2)
            {
                admdate = admdatesp[2].ToString() + "/" + admdatesp[0].ToString() + "/" + admdatesp[1].ToString();
                Admission_date = Convert.ToDateTime(admdate.ToString());
            }

            dd = dt_stud_details.Rows[stud]["ROLL_NO"].ToString();
            hat.Clear();
            hat.Add("std_rollno", dt_stud_details.Rows[stud]["ROLL_NO"].ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            if (rows_count == 0)
            {
                hat.Clear();
                hat.Add("degree_code", int.Parse(deg_code));
                hat.Add("sem", int.Parse(curr_sem));
                hat.Add("from_date", frdate.ToString());
                hat.Add("to_date", todate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));


                //------------------------------------------------------------------
                int iscount = 0;
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + deg_code + " and semester=" + curr_sem + "";

                dsholiday = d2.select_method_wo_parameter(sqlstr_holiday, "text");
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);

                ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

                Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table2 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table3 = new Dictionary<string, int>();

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
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);

                    }
                }

                if (ds3.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');

                        if (!holiday_table21.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                            holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

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
                            holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                    }
                }

                if (ds3.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');

                        if (!holiday_table31.Contains(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                        {
                            holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

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

                            holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }



            }

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

                while (dumm_from_date <= (per_to_date))
                {
                    isadm = false;
                    if (dumm_from_date >= Admission_date)
                    {
                        isadm = true;
                        int temp_unmark = 0;
                        if (splhr_flag == true)
                        {
                            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
                            {
                                getspecial_hr();
                            }
                        }

                        for (int i = 1; i <= mmyycount; i++)
                        {
                            //Modified By Srinath 22/Jan2014
                            ds2.Tables[0].DefaultView.RowFilter = " month_year='" + cal_from_date + "'";
                            DataView dvatt = ds2.Tables[0].DefaultView;
                            // if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
                            if (dvatt.Count > 0)
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

                                    if (ds3.Tables[1].Rows.Count != 0)
                                    {
                                        ts = DateTime.Parse(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                        diff_date = Convert.ToString(ts.Days);
                                        dif_date = double.Parse(diff_date.ToString());
                                    }
                                    else
                                    {
                                        dif_date = 0;
                                    }
                                    if (dif_date == 1)
                                    {
                                        leave_pointer = holi_leav;
                                        absent_pointer = holi_absent;
                                    }
                                    else if (dif_date == -1)
                                    {
                                        leave_pointer = holi_leav;
                                        absent_pointer = holi_absent;
                                        if (ccount > rowcount)
                                        {
                                            rowcount += 1;
                                        }
                                    }
                                    else
                                    {
                                        leave_pointer = leav_pt;
                                        absent_pointer = absent_pt;

                                    }

                                    if (ds3.Tables[2].Rows.Count != 0)
                                    {
                                        ts = DateTime.Parse(ds3.Tables[2].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                        diff_date = Convert.ToString(ts.Days);
                                        dif_date = double.Parse(diff_date.ToString());
                                        if (dif_date == 1)
                                        {
                                            leave_pointer = holi_leav;
                                            absent_pointer = holi_absent;
                                        }

                                    }
                                    if (dif_date1 == -1)
                                    {
                                        leave_pointer = holi_leav;
                                        absent_pointer = holi_absent;
                                    }
                                    dif_date1 = 0;
                                    if (split_holiday_status_1 == "1")
                                    {

                                        for (i = 1; i <= fnhrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            // value = ds2.Tables[0].Rows[next][date].ToString();
                                            value = dvatt[0][date].ToString(); //Modified By Srinath 22/Jan2014
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
                                                    totabsent++;
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
                                                else if (value == "4")
                                                {
                                                    tot_ml += 1;
                                                }

                                            }
                                            else if (value == "7")
                                            {
                                                per_hhday += 1;

                                            }
                                            else
                                            {
                                                unmark += 1;
                                                temp_unmark++;

                                                my_un_mark++;//added 080812
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

                                        if (temp_unmark == fnhrs)
                                        {
                                            per_holidate_mng += 1;
                                            per_holidate += 0.5;
                                            unmark = 0;
                                        }
                                        else
                                        {
                                            dum_unmark = temp_unmark;
                                        }
                                        if (fnhrs - temp_unmark >= minpresI)
                                        {
                                            workingdays += 0.5;
                                        }
                                        mng_conducted_half_days += 1;


                                    }
                                    per_perhrs = 0;
                                    per_ondu = 0;
                                    per_leave = 0;
                                    per_abshrs = 0;
                                    //   unmark = 0;
                                    temp_unmark = 0;
                                    njhr = 0;

                                    int k = fnhrs + 1;

                                    if (split_holiday_status_2 == "1")
                                    {
                                        for (i = k; i <= NoHrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            // value = ds2.Tables[0].Rows[next][date].ToString();
                                            value = dvatt[0][date].ToString(); //Modified By Srinath 22/Jan2014
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
                                                    totabsent++;
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
                                                if (value == "4")
                                                {
                                                    tot_ml += 1;
                                                }
                                            }
                                            else if (value == "7")
                                            {
                                                per_hhday += 1;
                                            }
                                            else
                                            {
                                                unmark += 1;
                                                temp_unmark++;

                                                my_un_mark++; //added 080812
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

                                        if (temp_unmark == NoHrs - fnhrs)
                                        {
                                            per_holidate_evng += 1;
                                            per_holidate += 0.5;

                                            unmark = 0;
                                        }
                                        else
                                        {
                                            dum_unmark += unmark;
                                        }


                                        if ((NoHrs - fnhrs) - temp_unmark >= minpresII)
                                        {
                                            workingdays += 0.5;
                                        }
                                        evng_conducted_half_days += 1;


                                    }

                                    per_perhrs = 0;
                                    per_ondu = 0;
                                    per_leave = 0;
                                    per_abshrs = 0;
                                    unmark = 0; //hided
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
                                dumm_from_date = dumm_from_date.AddDays(1);
                                if (dumm_from_date.Day == 1)
                                {

                                    cal_from_date++;


                                    if (moncount > next)
                                    {
                                        next++; //  next++;
                                    }
                                }
                            }
                        }
                    }
                    if (isadm == false)
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
                    }
                }
                int diff_Date = per_from_date.Day - dumm_from_date.Day;
            }

            per_tot_ondu = tot_ondu;
            per_tot_ml = tot_ml;
            per_njdate = njdate;
            pre_present_date = Present - njdate;
            per_per_hrs = tot_per_hrs;
            per_absent_date = Absent;
            pre_ondu_date = Onduty;
            pre_leave_date = Leave;

            per_workingdays = workingdays - per_njdate;
            per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark;

            per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value;

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
            tot_ml = 0;
        }
        //  catch { }

    }

    public void getspecial_hr()
    {
        //  try
        {
            string hrdetno = "";
            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dumm_from_date), ht_sphr));

            }
            if (hrdetno != "")
            {

                string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + rollno + "'  and hrdet_no in(" + hrdetno + ")";
                ds_splhr_query_master.Dispose();
                ds_splhr_query_master.Reset();
                ds_splhr_query_master = d2.select_method_wo_parameter(splhr_query_master, "text");
                if (ds_splhr_query_master.Tables[0].Rows.Count > 0)
                {
                    for (int g = 0; g < ds_splhr_query_master.Tables[0].Rows.Count; g++)
                    {
                        value = ds_splhr_query_master.Tables[0].Rows[g][0].ToString();

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
                            if (value == "4")
                            {
                                tot_ml_spl += 1;
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
                if (check == 1)
                {

                    per_abshrs_spl_fals = per_abshrs_spl;
                    tot_per_hrs_spl_fals = tot_per_hrs_spl;
                    per_leave_fals = per_leave;
                    tot_conduct_hr_spl_fals = tot_conduct_hr_spl;
                    tot_ondu_spl_fals = tot_ondu_spl;
                    tot_ml_spl_fals = tot_ml_spl;
                }
                else if (check == 2)
                {
                    per_abshrs_spl_true = per_abshrs_spl;
                    tot_per_hrs_spl_true = tot_per_hrs_spl;
                    per_leave_true = per_leave;
                    tot_conduct_hr_spl_true = tot_conduct_hr_spl;
                    tot_ondu_spl_true = tot_ondu_spl;
                    tot_ml_spl_true = tot_ml_spl;
                }
            }
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
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            //Modified by Srinath 27/2/2013
            string reportname = txtxl.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(Showgrid, reportname);
            }
            else
            {
                lblexcelerror.Text = "Please Enter Your Report Name";
                lblexcelerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblexcelerror.Text = ex.ToString();
            lblexcelerror.Visible = true;
        }
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    { }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "Attendance Report";
        if (rboverall.Checked == true)
        {
            degreedetails = "Over All Attendance Report @From Date : " + txtfdate.Text.ToString() + "   To Date : " + txttdate.Text.ToString();
        }
        string ss = null;
        Printcontrol.loadspreaddetails(Showgrid, "AttendanceReport_New.aspx", degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }
    protected void radiocheched(object sender, EventArgs e)
    {
        clear();
        if (rbtest.Checked == true)
        {
            txtfdate.Visible = false;
            txttdate.Visible = false;
            lbltdate.Visible = false;
            lblfdate.Visible = false;

        }
        else
        {
            lbltdate.Visible = true;
            lblfdate.Visible = true;
            txtfdate.Visible = true;
            txttdate.Visible = true;
            txtfdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        }
    }
    public void clear()
    {
        lblexcelerror.Visible = false;
        Showgrid.Visible = false;
        txtxl.Visible = false;
        lblxl.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        lbl_error.Visible = false;
        txtxl.Text = "";
        lblnote.Visible = false;
    }
    protected void rangecheck(object sender, EventArgs e)
    {
        //try
        //{
        clear();
        //    Double frange = 0;
        //    Double trange = 0;

        //    string fr = txtfrom.Text.ToString();
        //    string tr = txtto.Text.ToString();
        //    if (fr.Trim() == "" && tr.Trim() == "")
        //    {
        //        return;
        //    }
        //    if (fr.Trim() != "" && fr != null)
        //    {
        //        frange = Convert.ToDouble(fr);
        //        if (trange > 100)
        //        {
        //            txtfrom.Text = "";
        //            lbl_error.Visible = true;
        //            lbl_error.Text = "Please Enter The From Range Less Than or Equal To 100";
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        lbl_error.Visible = true;
        //        lbl_error.Text = "Please Enter The From Range";
        //        return;
        //    }
        //    if (tr.Trim() != "" && tr != null)
        //    {
        //        trange = Convert.ToDouble(tr);
        //        if (trange > 100)
        //        {
        //            txtto.Text = "";
        //            lbl_error.Visible = true;
        //            lbl_error.Text = "Please Enter The To Range Less Than or Equal To 100";
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        lbl_error.Visible = true;
        //        lbl_error.Text = "Please Enter The To Range";
        //        return;
        //    }

        //    if (trange < frange)
        //    {
        //        lbl_error.Visible = true;
        //        lbl_error.Text = "Please Enter The From Range Must Be Lesser Than or Equal To Range";
        //        return;
        //    }

        //}
        //catch
        //{
        //}

    }
    public void loadoverallattendance()
    {
        try
        {
            ArrayList arrColHdrNames1 = new ArrayList();
            ArrayList arrColHdrNames2 = new ArrayList();
            arrColHdrNames1.Add("S.No");
            arrColHdrNames1.Add("Branch/Section");
            arrColHdrNames2.Add("S.No");
            arrColHdrNames2.Add("Branch/Section");

            data.Columns.Add("S.No", typeof(string));
            data.Columns.Add("Branch/Section", typeof(string));
            if (Session["Rollflag"].ToString() == "1")
            {
                arrColHdrNames1.Add("RollNo");
                arrColHdrNames2.Add("RollNo");
                data.Columns.Add("Roll No", typeof(string));
                colcnt++;
            }
            if (Session["Regflag"].ToString() == "1")
            {
                arrColHdrNames1.Add("RegNo");
                arrColHdrNames2.Add("RegNo");
                data.Columns.Add("Reg No", typeof(string));
                colcnt++;
            }
            arrColHdrNames1.Add("Name of the Students");
            arrColHdrNames2.Add("Name of the Students");
            data.Columns.Add("Name of the Students", typeof(string));
            colcnt = colcnt + 3;



            arrColHdrNames1.Add("Period Wise Attendance");
            arrColHdrNames1.Add("Period Wise Attendance");
            arrColHdrNames1.Add("Period Wise Attendance");
            arrColHdrNames1.Add("Period Wise Attendance");
            arrColHdrNames1.Add("Period Wise Attendance");
            arrColHdrNames2.Add("Conducted Periods");
            arrColHdrNames2.Add("Attend Periods");
            arrColHdrNames2.Add("Absent Periods");
            arrColHdrNames2.Add("On Duty Periods");
            arrColHdrNames2.Add("Attendance Percentage");

            data.Columns.Add("Conducted Periods", typeof(string));
            data.Columns.Add("Attend Periods", typeof(string));
            data.Columns.Add("Absent Periods", typeof(string));
            data.Columns.Add("On Duty Periods", typeof(string));
            data.Columns.Add("Attendance Percentage", typeof(string));

            DataRow drHdr1 = data.NewRow();
            DataRow drHdr2 = data.NewRow();
            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames1[grCol];
                drHdr2[grCol] = arrColHdrNames2[grCol];
            }
            data.Rows.Add(drHdr1);
            data.Rows.Add(drHdr2);

            Double frange = 0;
            Double trange = 100;

            if (txtfrom.Text.ToString().Trim() != "")
            {
                frange = Convert.ToDouble(txtfrom.Text.ToString());
            }

            if (txtto.Text.ToString().Trim() != "")
            {
                trange = Convert.ToDouble(txtto.Text.ToString());
            }

            for (int cnt_getbatch = 0; cnt_getbatch < chklst_batch.Items.Count; cnt_getbatch++)
            {
                if (chklst_batch.Items[cnt_getbatch].Selected == true)
                {
                    if (batch_year == "")
                    {
                        batch_year = chklst_batch.Items[cnt_getbatch].Value;
                    }
                    else
                    {
                        batch_year = batch_year + "," + chklst_batch.Items[cnt_getbatch].Value;
                    }
                }
            }

            string strbatchyear = "";
            if (batch_year.Trim() != "")
            {
                strbatchyear = " and r.batch_year in(" + batch_year + ")";
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select The Batch And then Proceed";
                return;
            }

            for (int cnt_getdegree = 0; cnt_getdegree < chklst_branch.Items.Count; cnt_getdegree++)
            {
                if (chklst_branch.Items[cnt_getdegree].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = chklst_branch.Items[cnt_getdegree].Value;
                    }
                    else
                    {
                        degree_code = degree_code + "," + chklst_branch.Items[cnt_getdegree].Value;
                    }
                }
            }
            string strdegree = "";
            if (degree_code.Trim() != "")
            {
                strdegree = " and r.degree_code in(" + degree_code + ")";
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select The Degree And Branch And then Proceed";
                return;
            }



            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
            string strorder = ",r.roll_no";
            if (orderby_Setting == "0")
            {
                strorder = ",r.roll_no";
            }
            else if (orderby_Setting == "1")
            {
                strorder = ",r.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = ",r.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = ",r.roll_no,r.Reg_No,r.stud_name";
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
                strorder = " ,r.roll_no,r.Stud_Name";
            }
            string startdate = txtfdate.Text.ToString();
            start_date_1 = startdate;

            string todate_1 = txttdate.Text.ToString();
            get_todate = todate_1;

            int sno = 0;
            dt_stud_details = d2.select_method_wop_table("select distinct r.batch_year,r.degree_code,course_name,r.sections,acronym,roll_no,reg_no,stud_name,r.adm_date,current_semester from registration r,course c,department dp,degree d where c.course_id=d.course_id and dp.dept_code=d.dept_code and r.degree_code=d.degree_code and cc=0 and exam_flag<>'debar' and delflag=0 " + strdegree + " " + strbatchyear + " order by r.batch_year,r.degree_code,r.sections " + strorder + "", "Text");
            string student_dept = string.Empty;
            for (stud = 0; stud < dt_stud_details.Rows.Count; stud++)
            {

                batch_yr = dt_stud_details.Rows[stud]["batch_year"].ToString();
                deg_code = dt_stud_details.Rows[stud]["degree_code"].ToString();
                course_name = dt_stud_details.Rows[stud]["course_name"].ToString();
                section = dt_stud_details.Rows[stud]["sections"].ToString();
                acronym = dt_stud_details.Rows[stud]["acronym"].ToString();
                rollno = dt_stud_details.Rows[stud]["roll_no"].ToString();
                regno = dt_stud_details.Rows[stud]["reg_no"].ToString();
                stud_name = dt_stud_details.Rows[stud]["stud_name"].ToString();
                curr_sem = dt_stud_details.Rows[stud]["current_semester"].ToString();

                student_dept = batch_yr + '-' + course_name + '-' + acronym + '-' + curr_sem;
                if (section.Trim() != "" && section != null && section.Trim() != "-1-")
                {
                    student_dept = student_dept + '-' + section;
                }


                persentmonthcal();

                per_tage_date = ((pre_present_date / per_workingdays) * 100);
                if (per_tage_date > 100)
                {
                    per_tage_date = 100;
                }

                per_con_hrs = per_workingdays1;

                per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100);
                if (per_tage_hrs > 100)
                {
                    per_tage_hrs = 100;
                }
                per_tage_hrs = Math.Round(per_tage_hrs, 2, MidpointRounding.AwayFromZero);
                per_tage_date = Math.Round(per_tage_date, 2, MidpointRounding.AwayFromZero);
                dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
                dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));

                if (dum_tage_hrs == "NaN")
                {
                    dum_tage_hrs = "0";
                }
                else if (dum_tage_hrs == "Infinity")
                {
                    dum_tage_hrs = "0";
                }
                if (dum_tage_date == "NaN")
                {
                    dum_tage_date = "0";
                }
                else if (dum_tage_date == "Infinity")
                {
                    dum_tage_date = "0";
                }

                Boolean sturowflag = false;
                if (frange <= Convert.ToDouble(dum_tage_hrs) && trange >= Convert.ToDouble(dum_tage_hrs))
                {
                    sturowflag = true;
                }
                if (sturowflag == true)
                {
                    sno++;

                    drow = data.NewRow();
                    drow["S.No"] = sno.ToString();
                    drow["Branch/Section"] = student_dept.ToString();
                    if (Session["Rollflag"].ToString() == "1")
                        drow["Roll No"] = rollno.ToString();
                    if (Session["Regflag"].ToString() == "1")
                        drow["Reg No"] = regno.ToString();
                    drow["Name of the Students"] = stud_name.ToString();
                    drow["Conducted Periods"] = per_con_hrs.ToString();
                    drow["Attend Periods"] = (per_per_hrs + tot_per_hrs_spl_fals).ToString();
                    drow["Absent Periods"] = per_tot_ondu.ToString();
                    drow["On Duty Periods"] = dum_tage_hrs.ToString();
                    drow["Attendance Percentage"] = per_workingdays.ToString();

                    data.Rows.Add(drow);


                }

            }
            if (data.Rows.Count > 0 && data.Columns.Count > 0)
            {
                Showgrid.DataSource = data;
                Showgrid.DataBind();
                Showgrid.Visible = true;
                lblxl.Visible = true;
                txtxl.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;

                int rowcnt = Showgrid.Rows.Count - 2;

                Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Showgrid.Rows[0].Font.Bold = true;
                Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                Showgrid.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Showgrid.Rows[1].Font.Bold = true;
                Showgrid.Rows[1].HorizontalAlign = HorizontalAlign.Center;

                for (int i = 0; i < data.Columns.Count; i++)
                {
                    if (Showgrid.Rows[0].Cells[i].Text == Showgrid.Rows[1].Cells[i].Text)
                    {

                        Showgrid.Rows[0].Cells[i].RowSpan = Showgrid.Rows[1].Cells[i].RowSpan < 2 ? 2 :
                                               Showgrid.Rows[1].Cells[i].RowSpan + 1;
                        Showgrid.Rows[1].Cells[i].Visible = false;
                    }
                }
                for (int t = Showgrid.Rows.Count - 1; t > 0; t--)
                {
                    GridViewRow row = Showgrid.Rows[t];
                    GridViewRow previousRow = Showgrid.Rows[t - 1];
                    if (row.Cells[1].Text == previousRow.Cells[1].Text)
                    {
                        if (previousRow.Cells[1].RowSpan == 0)
                        {
                            if (row.Cells[1].RowSpan == 0)
                            {
                                previousRow.Cells[1].RowSpan += 2;

                            }
                            else
                            {
                                previousRow.Cells[1].RowSpan = row.Cells[1].RowSpan + 1;

                            }
                            row.Cells[1].Visible = false;

                        }
                    }
                }

                //ColumnSpan
                for (int rowIndex = Showgrid.Rows.Count - rowcnt - 2; rowIndex >= 0; rowIndex--)
                {


                    for (int cell = Showgrid.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                    {
                        TableCell colum = Showgrid.Rows[rowIndex].Cells[cell];
                        TableCell previouscol = Showgrid.Rows[rowIndex].Cells[cell - 1];
                        if (colum.Text == previouscol.Text)
                        {
                            if (previouscol.ColumnSpan == 0)
                            {
                                if (colum.ColumnSpan == 0)
                                {
                                    previouscol.ColumnSpan += 2;

                                }
                                else
                                {
                                    previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                }
                                colum.Visible = false;

                            }
                        }
                    }

                }

            }
            else
            {
                lbl_error.Text = "No Records Found";
                lbl_error.Visible = true;
            }

        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }
    protected void datechange(object sender, EventArgs e)
    {
        try
        {
            clear();
            string[] spf = txtfdate.Text.ToString().Split('/');
            string[] spt = txttdate.Text.ToString().Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            if (dtf > DateTime.Now)
            {
                txtfdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbl_error.Visible = true;
                lbl_error.Text = "Please Enter From Date Must Be Lesser Than or Equal To Current Date";
            }
            if (dtt > DateTime.Now)
            {
                txttdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbl_error.Visible = true;
                lbl_error.Text = "Please Enter To Date Must Be Lesser Than or Equal To Current Date";
            }
            if (dtt < dtf)
            {
                txtfdate.Text = txttdate.Text;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Enter From Date Must Be Lesser Than or Equal To Date";
            }
        }
        catch
        {
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
                for (int j = colcnt; j < data.Columns.Count; j++)
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
        spReportName.InnerHtml = "Attendance Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
}
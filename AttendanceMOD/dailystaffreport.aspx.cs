using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.IO;
using Gios.Pdf;



public partial class CITreport : System.Web.UI.Page
{

    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds_attndmaster = new DataSet();
    DataSet dsstuatt = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable hpdf = new Hashtable();
    Hashtable hatvalue = new Hashtable();
    static Hashtable ht_sch = new Hashtable();
    static Hashtable ht_sdate = new Hashtable();
    static Hashtable ht_bell = new Hashtable();
    static Hashtable ht_period = new Hashtable();
    static Boolean hr_lock = false;
    Boolean attendanceentryflag = false;
    Boolean dailyentryflag = false;
    Boolean flag_true = false;
    Boolean resultflag = false;
    Boolean saveflag = false;
    Boolean rowglag = false;
    Boolean selflag = false;
    string query = "";
    string collegecode = "";
    string degree_var = "";
    string strday = "";
    string tmp_camprevar = "";
    string cur_camprevar = "";
    string tmp_datevalue = "";
    string noofdays = "";
    string start_datesem = "";
    string start_dayorder = "";
    string Att_strqueryst = "";
    string staff_code = "";
    //string findday = "";
    string SqlFinal1 = "";
    string strquerytext = "";
    String Day_Order = "";
    int rowcnt = 0;
    List<string> list = new List<string>();
    static Hashtable hatpdf = new Hashtable();
    Hashtable hatfinal = new Hashtable();


    //added by rajasekar 05/10/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;

    //============================//



    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        collegecode = Session["Collegecode"].ToString();

        if (!IsPostBack)
        {
            txtdate.Attributes.Add("readonly", "readonly");
            txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            loaddept();
            loadstaffcode();
            Session["curr_year"] = DateTime.Now.ToString("yyyy");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["Sex"] = "0";
            Session["flag"] = "-1";
            Showgrid.Visible = false;
            divgrid.Visible = false;
            btngenerate.Visible = false;
        }
    }

    public void loaddept()
    {
        try
        {
            txtdept.Text = "---Select---";
            chkdept.Checked = false;
            query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + collegecode + "'";
            ds = da.select_method_wo_parameter(query, "Text");
            chklstdept.DataSource = ds;
            chklstdept.DataTextField = "dept_name";
            chklstdept.DataValueField = "dept_code";
            chklstdept.DataBind();
            chkdept.Checked = true;
            if (chklstdept.Items.Count > 0)
            {
                for (int i = 0; i < chklstdept.Items.Count; i++)
                {
                    chklstdept.Items[i].Selected = true;
                }
                txtdept.Text = "Dept(" + (chklstdept.Items.Count) + ")";
                chkdept.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void loadstaffcode()
    {
        try
        {
            txtstaff.Text = "---Select---";
            chkstaff.Checked = false;
            chklststaff.Items.Clear();
            query = "select distinct staff_code,dept_code from stafftrans where stftype='Teaching' and latestrec = 1";
            ds = da.select_method_wo_parameter(query, "Text");
            chklststaff.DataSource = ds;
            chklststaff.DataTextField = "staff_code";
            chklststaff.DataValueField = "dept_code";
            chklststaff.DataBind();
            chkstaff.Checked = true;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < chklststaff.Items.Count; i++)
                {
                    chklststaff.Items[i].Selected = true;
                }
                chkstaff.Checked = true;
                txtstaff.Text = "Code(" + (chklststaff.Items.Count) + ")";
            }
        }
        catch
        {
        }
    }

    protected void chkdept_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdept.Checked == true)
            {
                for (int i = 0; i < chklstdept.Items.Count; i++)
                {
                    chklstdept.Items[i].Selected = true;
                    txtdept.Text = "Dept(" + (chklstdept.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstdept.Items.Count; i++)
                {
                    chklstdept.Items[i].Selected = false;
                    txtdept.Text = "---Select---";
                }
            }

            loadstaffcode();
            txtstaff.Text = "---Select---";
            chkstaff.Checked = false;
        }
        catch
        {
        }
    }
    protected void chklstdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtstaff.Text = "---Select---";
            chkstaff.Checked = false;
            string dept = "";
            int commcount = 0;
            chkdept.Checked = false;
            for (int i = 0; i < chklstdept.Items.Count; i++)
            {
                if (chklstdept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;

                    if (dept == "")
                    {
                        dept = chklstdept.Items[i].Value.ToString();
                    }
                    else
                    {
                        dept = dept + "," + chklstdept.Items[i].Value;
                    }
                }
            }
            if (commcount == chklstdept.Items.Count)
            {
                txtdept.Text = "Dept(" + commcount.ToString() + ")";
                chkdept.Checked = true;
            }
            else if (commcount == 0)
            {
                txtdept.Text = "--Select--";
            }
            else
            {
                txtdept.Text = "Dept(" + commcount.ToString() + ")";
            }
            chklststaff.Items.Clear();
            string query = "select distinct staff_code,dept_code from stafftrans where stftype='Teaching' and latestrec = 1 and dept_code in(" + dept + ")";
            ds = da.select_method_wo_parameter(query, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklststaff.DataSource = ds;
                chklststaff.DataTextField = "staff_code";
                chklststaff.DataValueField = "dept_code";
                chklststaff.DataBind();
                for (int i = 0; i < chklststaff.Items.Count; i++)
                {
                    chklststaff.Items[i].Selected = true;
                }
                chkstaff.Checked = true;
                txtstaff.Text = "Code(" + (chklststaff.Items.Count) + ")";
            }

        }
        catch
        {

        }
    }
    protected void chkstaff_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkstaff.Checked == true)
            {
                for (int i = 0; i < chklststaff.Items.Count; i++)
                {
                    chklststaff.Items[i].Selected = true;
                }
                txtstaff.Text = "Code(" + (chklststaff.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklststaff.Items.Count; i++)
                {
                    chklststaff.Items[i].Selected = false;
                    txtstaff.Text = "---Select---";
                }
            }
        }
        catch
        {
        }
    }
    protected void chklststaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string code = "";
            int commcount = 0;
            chkstaff.Checked = false;
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                if (chklststaff.Items[i].Selected == true)
                {
                    commcount = commcount + 1;

                    if (code == "")
                    {
                        code = chklststaff.Items[i].Value.ToString();
                    }
                    else
                    {
                        code = code + "," + chklststaff.Items[i].Value;
                    }
                }
            }
            if (commcount == chklststaff.Items.Count)
            {
                txtstaff.Text = "Code(" + commcount.ToString() + ")";
                chkstaff.Checked = true;
            }
            else if (commcount == 0)
            {
                txtstaff.Text = "--Select--";
            }
            else
            {
                txtstaff.Text = "Code(" + commcount.ToString() + ")";
            }

            //string query = "select fee_type,fee_code from fee_info where header_id in(" + clg + ") and fee_type not in ('Cash','Income & Expenditure','Misc') and fee_type not in (select bankname from bank_master1) order by fee_code";
            //ds = da.select_method_wo_parameter(query, "text");
            //chklstcategory.DataSource = ds;
            //chklstcategory.DataTextField = "fee_type";
            //chklstcategory.DataValueField = "fee_code";
            //chklstcategory.DataBind();

        }
        catch
        {

        }
    }


    protected void btnGo_Click(object sender, EventArgs e)
    {

        string curdt = System.DateTime.Now.ToString("dd/MM/yyyy");
        string da = txtdate.Text;
        string[] d1 = curdt.Split('/');
        string[] d2 = da.Split('/');

        DateTime das = Convert.ToDateTime(d1[1] + '/' + d1[0] + '/' + d1[2]);
        DateTime d2s = Convert.ToDateTime(d2[1] + '/' + d2[0] + '/' + d2[2]);

        if (d2s > das)
        {
            lblerr.Visible = true;
            lblerr.Text = "Please Select Current Date";
            Showgrid.Visible = false;
            divgrid.Visible = false;
            btngenerate.Visible = false;
            return;
        }
        int a = 0;
        for (int j = 0; j < chklststaff.Items.Count; j++)
        {
            if (chklststaff.Items[j].Selected == true)
            {
                a++;
            }
        }
        if (a == 0)
        {
            lblerr.Visible = true;
            lblerr.Text = "Please Select Staff Code";
            Showgrid.Visible = false;
            divgrid.Visible = false;
            btngenerate.Visible = false;
            return;
        }
        hatpdf.Clear();
        binddpread();

    }

    public void binddpread()
    {
        try
        {

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            int colu = 0;


            
            


            dtl.Columns.Add("S.No", typeof(string));

            dtl.Rows[0][colu] = "S.No";
            colu++;

            dtl.Columns.Add("Staff Code", typeof(string));

            dtl.Rows[0][colu] = "Staff Code";
            colu++;

            dtl.Columns.Add("Staff Name", typeof(string));

            dtl.Rows[0][colu] = "Staff Name";
            colu++;

            dtl.Columns.Add("Degree", typeof(string));
            dtl.Rows[0][colu] = "Degree";
            colu++;

            dtl.Columns.Add("Subject Name", typeof(string));
            dtl.Rows[0][colu] = "Subject Name";
            colu++;

            dtl.Columns.Add("Hour", typeof(string));
            dtl.Rows[0][colu] = "Hour";
            colu++;

            



            int cn = 0;
            int noofhrs = 0;
            string sql_s = "";
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string tmp_varstr = "";
            string SqlBatchYear = "";
            Boolean finalfalg = false;
            Hashtable hatvalue = new Hashtable();
            string date1 = txtdate.Text.ToString();
            string[] split = date1.Split(new Char[] { '/' });
            string datefrom = split[2].ToString() + "-" + split[1].ToString() + "-" + split[0].ToString();
            string date2 = txtdate.Text.ToString();
            string[] split1 = date2.Split(new Char[] { '/' });
            string dateto = split[2].ToString() + "-" + split[1].ToString() + "-" + split[0].ToString();
            string ddf = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            string ddt = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();

            //query = "select holiday_date from holidayStudents where holiday_date='" + datefrom + "'";



            for (int k = 0; k < 1; k++)
            {
                
                FarPoint.Web.Spread.CheckBoxCellType cbx = new FarPoint.Web.Spread.CheckBoxCellType();
                
                cbx.AutoPostBack = true;
                for (int j = 0; j < chklststaff.Items.Count; j++)
                {
                    int a = 0;
                    if (chklststaff.Items[j].Selected == true)
                    {
                        selflag = true;
                        finalfalg = false;
                        string stafcode = chklststaff.Items[j].Text.ToString();
                        staff_code = stafcode;
                        string vari = "";
                        ht_sch.Clear();
                        hat.Clear();
                        hat.Add("college_code", Session["collegecode"].ToString());
                        string sql_stringvar = "sp_select_details_staff";
                        ds_attndmaster.Dispose();
                        ds_attndmaster.Reset();
                        ds_attndmaster = da.select_method(sql_stringvar, hat, "sp");
                        if (ds_attndmaster.Tables[0].Rows.Count > 0)
                        {
                            for (int pcont = 0; pcont < ds_attndmaster.Tables[0].Rows.Count; pcont++)
                            {
                                degree_var = Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["semester"]);

                                if (!ht_sch.Contains(Convert.ToString(degree_var)))
                                {
                                    vari = ds_attndmaster.Tables[0].Rows[pcont]["SchOrder"] + "," + ds_attndmaster.Tables[0].Rows[pcont]["nodays"];
                                    ht_sch.Add(degree_var, Convert.ToString(vari));
                                }

                            }
                        }

                        ht_sdate.Clear();

                        //sql_stringvar = "select distinct s.batch_year,s.degree_code,s.semester,CONVERT(VARCHAR(10),s.start_date,23)as sdate,starting_dayorder from seminfo s,registration r where r.degree_code=s.degree_code  and r.current_semester=s.semester and r.batch_year=s.batch_year";
                        //ds_attndmaster = da.select_method(sql_stringvar, hat, "Text");
                        if (ds_attndmaster.Tables[1].Rows.Count > 0)
                        {
                            for (int pcont = 0; pcont < ds_attndmaster.Tables[1].Rows.Count; pcont++)
                            {
                                degree_var = Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["semester"]);

                                if (!ht_sdate.Contains(Convert.ToString(degree_var)))
                                {
                                    vari = ds_attndmaster.Tables[1].Rows[pcont]["sdate"] + "," + ds_attndmaster.Tables[1].Rows[pcont]["starting_dayorder"];
                                    ht_sdate.Add(degree_var, Convert.ToString(vari));
                                }

                            }
                        }

                        ht_bell.Clear();

                        //sql_stringvar = "select distinct b.batch_year, b.degree_code,b.semester,b.period1,LTRIM(RIGHT(CONVERT(VARCHAR(20), b.start_time, 100), 7))as start_time ,LTRIM(RIGHT(CONVERT(VARCHAR(20), b.end_time, 100), 7))as end_time  from BellSchedule b,degree d where  b.degree_code=d.degree_code and b.batch_year is not null and d.college_code=" + Session["collegecode"].ToString() + " order by b.batch_year, b.degree_code,b.semester,b.period1";
                        //ds_attndmaster = da.select_method(sql_stringvar, hat, "Text");
                        if (ds_attndmaster.Tables[2].Rows.Count > 0)
                        {
                            for (int pcont = 0; pcont < ds_attndmaster.Tables[2].Rows.Count; pcont++)
                            {
                                degree_var = Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["semester"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["period1"]);

                                if (!ht_bell.Contains(Convert.ToString(degree_var)))
                                {
                                    vari = ds_attndmaster.Tables[2].Rows[pcont]["start_time"] + "," + ds_attndmaster.Tables[2].Rows[pcont]["end_time"];
                                    ht_bell.Add(degree_var, Convert.ToString(vari));
                                }

                            }
                        }

                        ht_period.Clear();

                        //sql_stringvar = "select * from attendance_hrlock where college_code=" + Session["collegecode"].ToString() + " order by lock_hr";
                        //ds_attndmaster = da.select_method(sql_stringvar, hat, "Text");
                        if (ds_attndmaster.Tables[3].Rows.Count > 0)
                        {
                            for (int pcont = 0; pcont < ds_attndmaster.Tables[3].Rows.Count; pcont++)
                            {
                                degree_var = Convert.ToString(ds_attndmaster.Tables[3].Rows[pcont]["lock_hr"]);

                                if (!ht_period.Contains(Convert.ToString(degree_var)))
                                {
                                    vari = ds_attndmaster.Tables[3].Rows[pcont]["markatt_from"] + "," + ds_attndmaster.Tables[3].Rows[pcont]["markatt_to"];
                                    ht_period.Add(degree_var, Convert.ToString(vari));
                                }
                            }
                        }

                        hr_lock = false;


                        if (ds_attndmaster.Tables[4].Rows.Count > 0)
                        {
                            string locktrue = ds_attndmaster.Tables[4].Rows[0]["hrlock"].ToString();
                            if (locktrue == "1")
                            {
                                hr_lock = true;
                            }
                        }

                        string degreename = "";
                        Hashtable hatdegreename = new Hashtable();

                        for (int i = 0; i < ds_attndmaster.Tables[5].Rows.Count; i++)
                        {
                            if (!hatdegreename.Contains(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString()))
                            {
                                hatdegreename.Add(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString(), ds_attndmaster.Tables[5].Rows[i]["course"].ToString() + '-' + ds_attndmaster.Tables[5].Rows[i]["dept_acronym"].ToString());
                            }
                        }
                        string currlabsub = "select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester from syllabus_master sy,sub_sem sm,subject s,Registration r where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and sm.Lab=1 and r.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and  sy.semester=r.Current_Semester and r.college_code='" + Session["collegecode"].ToString() + "' order by sy.Batch_Year,sy.degree_code,sy.semester";
                        DataSet dscurrlab = da.select_method_wo_parameter(currlabsub, "Text");
                        DataTable dtcurrlab = dscurrlab.Tables[0];



                        if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[2].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                        {

                            if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[2].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                            {
                                long days = -1;
                                DateTime dt1 = DateTime.Now.AddDays(-6);
                                DateTime dt2 = DateTime.Now;
                                try
                                {
                                    dt1 = Convert.ToDateTime(ddf);
                                    dt2 = Convert.ToDateTime(ddt);
                                    TimeSpan t = dt2.Subtract(dt1);
                                    days = t.Days;
                                }
                                catch
                                {
                                    try
                                    {
                                        dt1 = Convert.ToDateTime(date1);
                                        dt2 = Convert.ToDateTime(date2);
                                        TimeSpan t = dt2.Subtract(dt1);
                                        days = t.Days;
                                    }
                                    catch
                                    {
                                    }
                                }

                                if (days >= 0)
                                {
                                    //load_attnd_spread();

                                    string[] differdays = new string[days];
                                    //sqlstr = da.GetFunction("select max(No_of_hrs_per_day) from PeriodAttndSchedule");
                                    noofhrs = 0;
                                    if (ds_attndmaster.Tables[6].Rows.Count > 0)
                                    {
                                        if (ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != "" && ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != null && ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != "0")
                                        {
                                            noofhrs = Convert.ToInt32(ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString());
                                        }
                                    }

                                    if (noofhrs != 0)
                                    {
                                        string sql1 = "";
                                        string Strsql = "";
                                        string SqlFinal = "";

                                        for (int day_lp = 0; day_lp < 7; day_lp++)
                                        {
                                            strday = Days[day_lp].ToString();
                                            sql1 = sql1 + "(";
                                            tmp_varstr = "";
                                            for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                            {
                                                Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                                if (tmp_varstr == "")
                                                {
                                                    tmp_varstr = tmp_varstr + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";

                                                }
                                                else
                                                {
                                                    tmp_varstr = tmp_varstr + " or " + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                                }
                                            }
                                            if (day_lp != 6)
                                                tmp_varstr = tmp_varstr + ") or ";
                                            else
                                                tmp_varstr = tmp_varstr + ")";

                                            sql1 = sql1 + tmp_varstr.ToString();
                                        }


                                        string SqlPrefinal1 = "";
                                        string SqlPrefinal2 = "";
                                        string SqlPrefinal3 = "";
                                        string SqlPrefinal4 = "";

                                        sql_s = "select semester_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=semester_schedule.degree_code and semester=semester_schedule.semester), ";
                                        sql_s = sql_s + Strsql + "";
                                        SqlBatchYear = "(select distinct(registration.batch_year) from registration,semester_schedule where registration.degree_code=semester_schedule.degree_code and registration.cc=0 and delflag=0 and registration.exam_flag<>'DEBAR' AND registration.current_Semester = semester_schedule.semester)";
                                        SqlPrefinal1 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and semester=1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                        SqlPrefinal2 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Semester' and First_Year_Nonsemester=0)";
                                        SqlPrefinal3 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and  batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Non Semester') ";
                                        SqlPrefinal4 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and  batch_year in " + SqlBatchYear + " and " + sql1 + " and semester<>1 and semester<>-1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                        SqlFinal = "(" + SqlPrefinal1 + ") union all (" + SqlPrefinal4 + ") union all (" + SqlPrefinal2 + ") union all (" + SqlPrefinal3 + ")";
                                        SqlFinal = SqlFinal + " order by batch_year,degree_code,semester,sections,FromDate";


                                        //Start Added By Aruna on 13feb2013=====================================================================
                                        SqlFinal = "";
                                        //  SqlFinal = " select distinct  r.degree_code,r.batch_year,s.semester,r.sections,s.fromdate,";
                                        SqlFinal = " select distinct  r.degree_code,r.batch_year,s.semester,r.sections ,";
                                        //  SqlFinal = SqlFinal + Strsql;
                                        SqlFinal = SqlFinal + " (select distinct  (c.course_name+'-'+ dt.dept_acronym) from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and degree_code=s.degree_code) as degree";
                                        SqlFinal = SqlFinal + ", (select distinct si.end_date from seminfo si where si.degree_code=s.degree_code and si.batch_year=s.batch_year and si.semester=s.semester) as end_date";
                                        SqlFinal = SqlFinal + " from semester_schedule s,registration r where s.semester=r.current_semester and s.batch_year=r.batch_year and s.degree_code=r.degree_code and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and s.sections=r.sections and ";
                                        SqlFinal = SqlFinal + "(" + sql1 + ")";
                                        //SqlFinal = SqlFinal + " and FromDate in (select top 1 FromDate from semester_schedule where degree_code =r.degree_code  and semester = s.semester  and batch_year = r.batch_year and FromDate <='" + datefrom + "'  order by FromDate Desc)";
                                        //  SqlFinal = SqlFinal + " order by r.degree_code,r.batch_year,s.semester,r.sections,FromDate";
                                        SqlFinal = SqlFinal + " order by r.degree_code,r.batch_year,s.semester,r.sections";
                                        //End==================================================================================================

                                        //STart Srinath 15/4/2014==================

                                        SqlFinal = " select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from staff_selector ss,Registration r,";
                                        SqlFinal = SqlFinal + " subject s,sub_sem sm,syllabus_master sy,seminfo si where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
                                        SqlFinal = SqlFinal + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
                                        SqlFinal = SqlFinal + " and s.subject_no=ss.subject_no and r.sections=ss.sections and ss.batch_year=r.Batch_Year";
                                        SqlFinal = SqlFinal + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
                                        SqlFinal = SqlFinal + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
                                        SqlFinal = SqlFinal + " and r.DelFlag=0 and ss.staff_code='" + staff_code + "'";
                                        //==========================End====================

                                        Hashtable hatholiday = new Hashtable();
                                        DataSet dsholiday = new DataSet();
                                        DataSet dsperiod = da.select_method(SqlFinal, hat, "Text");
                                        if (dsperiod.Tables[0].Rows.Count > 0)
                                        {
                                            for (int pre = 0; pre < dsperiod.Tables[0].Rows.Count; pre++)
                                            {
                                                // cur_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["FromDate"]);
                                                cur_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);
                                                string getdate = "";
                                                if (Convert.ToString(tmp_camprevar.Trim()) != Convert.ToString(cur_camprevar.Trim()))
                                                {
                                                    //=================================================================================================================================================================
                                                    dtcurrlab.DefaultView.RowFilter = " degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"] + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                    DataView dtcurlab = dtcurrlab.DefaultView;
                                                    Hashtable hatcurlab = new Hashtable();
                                                    for (int cula = 0; cula < dtcurlab.Count; cula++)
                                                    {
                                                        string lasubno = dtcurlab[cula]["subject_no"].ToString();
                                                        if (!hatcurlab.Contains(lasubno))
                                                        {
                                                            hatcurlab.Add(lasubno, lasubno);
                                                        }
                                                    }

                                                    hatholiday.Clear();
                                                    dsholiday.Dispose();
                                                    dsholiday.Reset();
                                                    string strholidayquery1 = "select holiday_date,degree_code,holiday_desc from holidaystudents where degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and holiday_date between '" + dt1 + "' and '" + dt2 + "'";
                                                    dsholiday = da.select_method(strholidayquery1, hat, "Text");
                                                    for (int i = 0; i < dsholiday.Tables[0].Rows.Count; i++)
                                                    {
                                                        if (!hatholiday.Contains(dsholiday.Tables[0].Rows[i]["holiday_date"].ToString()))
                                                        {
                                                            hatholiday.Add(dsholiday.Tables[0].Rows[i]["holiday_date"].ToString(), dsholiday.Tables[0].Rows[i]["holiday_desc"].ToString());
                                                        }
                                                    }
                                                    int frshlf = 0, schlf = 0;

                                                    string gethoursquery = "select no_of_hrs_I_half_day as mor,no_of_hrs_I_half_day as eve from periodattndschedule where degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                    DataSet dssemhour = da.select_method_wo_parameter(gethoursquery, "Text");
                                                    if (dssemhour.Tables[0].Rows.Count > 0)
                                                    {
                                                        string morhr = dssemhour.Tables[0].Rows[0]["mor"].ToString();
                                                        string evehr = dssemhour.Tables[0].Rows[0]["mor"].ToString();
                                                        if (morhr != null && morhr.Trim() != "")
                                                        {
                                                            frshlf = Convert.ToInt32(morhr);
                                                        }
                                                        if (evehr != null && evehr.Trim() != "")
                                                        {
                                                            schlf = Convert.ToInt32(evehr);
                                                        }
                                                    }
                                                    string getcurrent_sem = da.GetFunction("select distinct current_semester from registration where degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar'");
                                                    if (Convert.ToString(getcurrent_sem) == Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]))
                                                    {
                                                        string semenddate = "";
                                                        semenddate = da.GetFunction("select end_date from seminfo where degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'");

                                                        for (int row_inc = 0; row_inc <= days; row_inc++) //Date Loop
                                                        {
                                                            if (hatdegreename.Contains(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString()))
                                                            {
                                                                degreename = GetCorrespondingKey(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), hatdegreename).ToString();
                                                            }
                                                            DateTime cur_day = new DateTime();
                                                            cur_day = dt2.AddDays(-row_inc);

                                                            tmp_datevalue = Convert.ToString(cur_day);
                                                            degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                            string SchOrder = "";
                                                            string day_from = cur_day.ToString("yyyy-MM-dd");
                                                            //DateTime schfromdate = Convert.ToDateTime(dsperiod.Tables[0].Rows[pre]["FromDate"]);
                                                            DateTime schfromdate = cur_day;
                                                            string strsction = "";
                                                            if ((Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "") && (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1"))
                                                            {
                                                                strsction = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                            }
                                                            getdate = Convert.ToString(da.GetFunction("select top 1 FromDate from semester_schedule where degree_code =" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "  and semester = " + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "  and batch_year = " + Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + " and FromDate <='" + day_from + "' " + strsction + " order by FromDate Desc  "));

                                                            if (Convert.ToString(getdate) != "" && Convert.ToString(getdate).Trim() != "0" && Convert.ToString(getdate).Trim() != null)
                                                            {
                                                                DateTime getsche = Convert.ToDateTime(getdate);

                                                                if (Convert.ToDateTime(schfromdate) == Convert.ToDateTime(getsche) || Convert.ToDateTime(schfromdate) != Convert.ToDateTime(getsche))
                                                                {
                                                                    if (ht_sch.Contains(Convert.ToString(degree_var)))
                                                                    {
                                                                        string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sch));
                                                                        string[] sp_rd_semi = contvar.Split(',');
                                                                        if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                                        {
                                                                            SchOrder = sp_rd_semi[0].ToString();
                                                                            noofdays = sp_rd_semi[1].ToString();
                                                                        }
                                                                    }

                                                                    degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);

                                                                    if (ht_sdate.Contains(Convert.ToString(degree_var)))
                                                                    {
                                                                        string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sdate));
                                                                        string[] sp_rd_semi = contvar.Split(',');
                                                                        if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                                        {
                                                                            start_datesem = sp_rd_semi[0].ToString();
                                                                            start_dayorder = sp_rd_semi[1].ToString();
                                                                        }

                                                                    }
                                                                    if (noofdays.ToString().Trim() == "")
                                                                    {
                                                                        // goto lb1;
                                                                    }
                                                                    string Day_Order = "";
                                                                    if (SchOrder == "1")
                                                                    {
                                                                        strday = cur_day.ToString("ddd"); //Week Dayorder
                                                                        Day_Order = "0-" + Convert.ToString(strday);
                                                                    }
                                                                    else
                                                                    {
                                                                        strday = findday(cur_day.ToString(), dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), dsperiod.Tables[0].Rows[pre]["semester"].ToString(), dsperiod.Tables[0].Rows[pre]["batch_year"].ToString(), start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                                                                    }
                                                                    if (strday.ToString().Trim() == "")
                                                                    {
                                                                        // goto lb1;
                                                                    }
                                                                    //==check holiday
                                                                    string reasonsun = "";

                                                                    if (!hatholiday.Contains(cur_day.ToString()) || reasonsun.Trim().ToLower() != "sunday")
                                                                    {
                                                                        string str_day = strday;
                                                                        string Atmonth = cur_day.Month.ToString();
                                                                        string Atyear = cur_day.Year.ToString();
                                                                        long strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);

                                                                        sql1 = "";
                                                                        Strsql = "";

                                                                        for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                                                        {
                                                                            Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                                                            if (sql1 == "")
                                                                            {
                                                                                sql1 = sql1 + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line

                                                                            }
                                                                            else
                                                                            {
                                                                                sql1 = sql1 + " or " + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                                                            }
                                                                        }

                                                                        string day_aten = cur_day.Day.ToString();
                                                                        Boolean check_hour = false;

                                                                        //aruna 19dec2012================================================     
                                                                        string strsectionvar = "";
                                                                        string labsection = "";
                                                                        if (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "" && Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1")
                                                                        {
                                                                            strsectionvar = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                            labsection = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                        }
                                                                        sql1 = " and (" + sql1 + ")";
                                                                        string sqlalter = "select " + Strsql + " degree_code,batch_year,semester,sections";
                                                                        string alterqueryfinal = sqlalter + " from Alternate_schedule where fromdate='" + day_from + "' ";
                                                                        sqlalter = sqlalter + " from Alternate_schedule where fromdate='" + day_from + "'  " + sql1 + " ";
                                                                        SqlFinal1 = sqlalter;

                                                                        string altersetion = "";
                                                                        if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null && dsperiod.Tables[0].Rows[pre]["sections"].ToString().Trim() != "")
                                                                        {
                                                                            altersetion = "and Sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                                                        }



                                                                        DataSet dsaltertable = new DataSet();
                                                                        alterqueryfinal = alterqueryfinal + "and degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + "";
                                                                        dsaltertable = da.select_method(alterqueryfinal, hat, "Text");

                                                                        string semequery = "select top 1 * from semester_schedule where degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate <='" + cur_day.ToString() + "' order by FromDate Desc";
                                                                        DataSet dssem = da.select_method_wo_parameter(semequery, "Text");



                                                                        //===============================================================
                                                                        //sem sedule
                                                                        string text_temp = "";
                                                                        int temp = 0;
                                                                        text_temp = "";
                                                                        string getcolumnfield = "";
                                                                        string getcolumnfield_alter = "";

                                                                        DataSet dsholidayval = new DataSet();
                                                                        Boolean moringleav = false;
                                                                        Boolean evenleave = false;

                                                                        string strholyquery = "select * from holidaystudents where holiday_date='" + cur_day.ToString("MM/dd/yyyy") + "' and degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                                        dsholidayval = da.select_method_wo_parameter(strholyquery, "Text");
                                                                        if (dsholidayval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            if (!hatholiday.Contains(cur_day.ToString()))
                                                                            {
                                                                                hatholiday.Add(cur_day.ToString(), dsholidayval.Tables[0].Rows[0]["holiday_desc"].ToString());
                                                                            }
                                                                            if (dsholidayval.Tables[0].Rows[0]["morning"].ToString() == "1" || dsholidayval.Tables[0].Rows[0]["morning"].ToString().Trim().ToLower() == "true")
                                                                            {
                                                                                moringleav = true;
                                                                            }
                                                                            if (dsholidayval.Tables[0].Rows[0]["evening"].ToString() == "1" || dsholidayval.Tables[0].Rows[0]["evening"].ToString().Trim().ToLower() == "true")
                                                                            {
                                                                                evenleave = true;
                                                                            }
                                                                            if (dsholidayval.Tables[0].Rows[0]["halforfull"].ToString() == "0" || dsholidayval.Tables[0].Rows[0]["halforfull"].ToString().Trim().ToLower() == "false")
                                                                            {
                                                                                evenleave = true;
                                                                                moringleav = true;
                                                                            }
                                                                        }
                                                                        for (temp = 1; temp <= noofhrs; temp++)
                                                                        {
                                                                            string sp_rd = "";
                                                                            Boolean altfalg = false;
                                                                            if (dsaltertable.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                sp_rd = dsaltertable.Tables[0].Rows[0]["" + strday.Trim() + temp + ""].ToString();

                                                                                if (hatdegreename.Contains(dsaltertable.Tables[0].Rows[0]["degree_code"].ToString()))
                                                                                {
                                                                                    degreename = GetCorrespondingKey(dsaltertable.Tables[0].Rows[0]["degree_code"].ToString(), hatdegreename).ToString();
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                sp_rd = "";
                                                                            }
                                                                            if (sp_rd.Trim() != "" && sp_rd.Trim() != "0" && sp_rd != null)
                                                                            {
                                                                                altfalg = true;
                                                                                string[] sp_rd_split = sp_rd.Split(';');
                                                                                for (int index = 0; index <= sp_rd_split.GetUpperBound(0); index++)
                                                                                {
                                                                                    string[] sp2 = sp_rd_split[index].Split(new Char[] { '-' });

                                                                                    if (sp2.GetUpperBound(0) >= 1)
                                                                                    {

                                                                                        int upperbound = sp2.GetUpperBound(0);


                                                                                        for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                                        {
                                                                                            if (sp2[multi_staff] == stafcode)
                                                                                            {
                                                                                                resultflag = true;
                                                                                                //==============================theroy batch=======================================
                                                                                                Boolean checklabhr = false;
                                                                                                for (int sr = 0; sr <= sp_rd_split.GetUpperBound(0); sr++)
                                                                                                {
                                                                                                    string[] getlasub = sp_rd_split[sr].ToString().Split('-');
                                                                                                    if (getlasub.GetUpperBound(0) > 1)
                                                                                                    {
                                                                                                        string srllab = getlasub[0].ToString();
                                                                                                        if (hatcurlab.Contains(srllab))
                                                                                                        {
                                                                                                            checklabhr = true;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                //======================================================================

                                                                                                string sect = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                if (sect != "-1" && sect != null && sect.Trim() != "")
                                                                                                {
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    sect = "";
                                                                                                }

                                                                                                if (semenddate.Trim() != "" && semenddate.Trim() != null && semenddate.Trim() != "0")
                                                                                                {
                                                                                                    if (cur_day <= (Convert.ToDateTime(semenddate)))
                                                                                                    {
                                                                                                        check_hour = true;
                                                                                                        double Num;
                                                                                                        bool isNum = double.TryParse(sp2[0].ToString(), out Num);
                                                                                                        if (isNum)
                                                                                                        {
                                                                                                            if (checklabhr == false)
                                                                                                            {
                                                                                                                text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-S";
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-L";
                                                                                                            }
                                                                                                            // text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-" + sp2[upperbound];
                                                                                                            string Schedule_string = "";

                                                                                                            if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "-1" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == null)
                                                                                                            {
                                                                                                                if (checklabhr == false)
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-0"; //+ sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-0"; //+ sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (checklabhr == false)
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-0";// +sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-0";// +sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                            }
                                                                                                            Boolean allowleave = false;
                                                                                                            if (hatholiday.Contains(cur_day.ToString()))
                                                                                                            {
                                                                                                                if (moringleav == true)
                                                                                                                {
                                                                                                                    if (frshlf >= temp)
                                                                                                                    {
                                                                                                                        allowleave = true;
                                                                                                                    }
                                                                                                                }
                                                                                                                if (evenleave == true)
                                                                                                                {
                                                                                                                    if (temp > frshlf)
                                                                                                                    {
                                                                                                                        allowleave = true;
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                            if (allowleave == true)
                                                                                                            {

                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                //if (Fpspread.Sheets[0].Cells[row_inc, temp - 1].Text.Trim() == "")
                                                                                                                //{
                                                                                                                //    Fpspread.Sheets[0].Cells[row_inc, temp - 1].Text = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                //    Fpspread.Sheets[0].Cells[row_inc, temp - 1].Tag = Schedule_string.ToString() + "-alter";
                                                                                                                //    if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                //    {
                                                                                                                //        Fpspread.Sheets[0].Cells[row_inc, temp - 1].Note = Convert.ToString(Day_Order);
                                                                                                                //    }
                                                                                                                //}
                                                                                                                //else
                                                                                                                //{
                                                                                                                //    string tmpvar = "";
                                                                                                                //    string istemp = "";
                                                                                                                //    istemp = Fpspread.Sheets[0].Cells[row_inc, temp - 1].Text.ToString();
                                                                                                                //    tmpvar = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                //    if (Convert.ToString(istemp) != Convert.ToString(tmpvar))
                                                                                                                //    {
                                                                                                                //        Fpspread.Sheets[0].Cells[row_inc, temp - 1].Text = Fpspread.Sheets[0].Cells[row_inc, temp - 1].Text + " * " + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + da.GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "") + "-" + da.GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "") + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                //        Fpspread.Sheets[0].Cells[row_inc, temp - 1].Tag = Fpspread.Sheets[0].Cells[row_inc, temp - 1].Tag + " * " + Schedule_string.ToString() + "-alter";
                                                                                                                //        if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                //        {
                                                                                                                //            Fpspread.Sheets[0].Cells[row_inc, temp - 1].Note = Convert.ToString(Day_Order);
                                                                                                                //        }
                                                                                                                //    }

                                                                                                                //}
                                                                                                                if (true)
                                                                                                                {

                                                                                                                    string daystring = dt2.AddDays(-row_inc).ToString("dd");
                                                                                                                    string daystring1 = dt2.AddDays(-row_inc).ToString("ddd");
                                                                                                                    string Att_dcolumn = "d" + Convert.ToInt16(daystring) + "d" + temp;
                                                                                                                    string check_lab = "";
                                                                                                                    hatvalue.Clear();
                                                                                                                    if (checklabhr == false)
                                                                                                                    {
                                                                                                                        check_lab = "0";
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        check_lab = "1";
                                                                                                                    }

                                                                                                                    string sectionvar = "";
                                                                                                                    string sectionsvalue = "";
                                                                                                                    if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null)
                                                                                                                    {
                                                                                                                        sectionvar = " and sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                                                                                                        sectionsvalue = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                                    }
                                                                                                                    if (check_lab == "1" || check_lab.Trim().ToLower() == "true")
                                                                                                                    {
                                                                                                                        hatvalue.Clear();
                                                                                                                        hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                        hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                        hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                        hatvalue.Add("sections", sectionsvalue);
                                                                                                                        hatvalue.Add("month_year", strdate);
                                                                                                                        hatvalue.Add("date", cur_day);
                                                                                                                        hatvalue.Add("subject_no", sp2[0]);
                                                                                                                        hatvalue.Add("day", strday);
                                                                                                                        hatvalue.Add("hour", temp);
                                                                                                                        dsstuatt.Reset();
                                                                                                                        dsstuatt.Dispose();
                                                                                                                        dsstuatt = da.select_method("sp_stu_atten_month_check_lab_alter", hatvalue, "sp");

                                                                                                                        if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                        {
                                                                                                                            Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                                                                                                            if (int.Parse(Att_strqueryst) > 0)
                                                                                                                            {
                                                                                                                                hatvalue.Clear();
                                                                                                                                hatvalue.Add("columnname", Att_dcolumn);
                                                                                                                                hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                                hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                                hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                                hatvalue.Add("sections", sectionsvalue);
                                                                                                                                hatvalue.Add("month_year", strdate);
                                                                                                                                hatvalue.Add("date", cur_day);
                                                                                                                                hatvalue.Add("subject_no", sp2[0]);
                                                                                                                                hatvalue.Add("day", strday);
                                                                                                                                hatvalue.Add("hour", temp);
                                                                                                                                dsstuatt.Reset();
                                                                                                                                dsstuatt.Dispose();
                                                                                                                                //  dsstuatt = da.select_method("sp_stu_atten_day_check_lab_alter", hatvalue, "sp");
                                                                                                                                string strgetatt = "select count(distinct r.Roll_No) as stucount from registration r,attendance a,subjectchooser_new s where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and  exam_flag<>'debar' and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";
                                                                                                                                strgetatt = strgetatt + " r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + sectionvar + " and(" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and fromdate='" + cur_day + "' and batch    in(select stu_batch from laballoc_new where subject_no='" + sp2[0].ToString() + "'  and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'  and hour_value='" + temp + "'  and    degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' ";
                                                                                                                                strgetatt = strgetatt + " and day_value='" + strday + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + sectionvar + " and fdate='" + cur_day + "') and adm_date<='" + cur_day + "'";

                                                                                                                                dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                                                if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                                {
                                                                                                                                    if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                                    {
                                                                                                                                        Att_strqueryst = "0";
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        Att_strqueryst = "1";
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    Att_strqueryst = "1";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                Att_strqueryst = "1";
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            Att_strqueryst = "1";
                                                                                                                        }

                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        hatvalue.Clear();
                                                                                                                        hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                        hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                        hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                        hatvalue.Add("sections", sectionsvalue);
                                                                                                                        hatvalue.Add("month_year", strdate);
                                                                                                                        hatvalue.Add("date", cur_day);
                                                                                                                        hatvalue.Add("subject_no", sp2[0]);
                                                                                                                        dsstuatt.Reset();
                                                                                                                        dsstuatt.Dispose();
                                                                                                                        dsstuatt = da.select_method("sp_stu_atten_month_check", hatvalue, "sp");
                                                                                                                        if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                        {
                                                                                                                            Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                                                                                                            if (int.Parse(Att_strqueryst) > 0)
                                                                                                                            {
                                                                                                                                hatvalue.Clear();
                                                                                                                                hatvalue.Add("columnname ", Att_dcolumn);
                                                                                                                                hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                                hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                                hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                                hatvalue.Add("sections", sectionsvalue);
                                                                                                                                hatvalue.Add("month_year", strdate);
                                                                                                                                hatvalue.Add("date", cur_day);
                                                                                                                                hatvalue.Add("subject_no", sp2[0]);
                                                                                                                                dsstuatt.Reset();
                                                                                                                                dsstuatt.Dispose();
                                                                                                                                //              dsstuatt = da.select_method("sp_stu_atten_day_check", hatvalue, "sp");
                                                                                                                                string strgetatt = "select count(distinct registration.roll_no) as stucount  from registration,attendance,subjectchooser where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and  registration.roll_no=subjectchooser.roll_no ";
                                                                                                                                strgetatt = strgetatt + " and registration.current_semester=subjectchooser.semester and subject_no='" + sp2[0] + "' " + sectionvar + "";
                                                                                                                                strgetatt = strgetatt + " and (" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and adm_date<='" + cur_day + "' ";
                                                                                                                                dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                                                if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                                {
                                                                                                                                    if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                                    {
                                                                                                                                        Att_strqueryst = "0";
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        Att_strqueryst = "1";
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    Att_strqueryst = "1";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                Att_strqueryst = "1";
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            Att_strqueryst = "1";
                                                                                                                        }
                                                                                                                    }
                                                                                                                    if (int.Parse(Att_strqueryst) > 0)
                                                                                                                    {
                                                                                                                        attendanceentryflag = false;
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        attendanceentryflag = true;
                                                                                                                    }
                                                                                                                    //Fpspread.Sheets[0].Cells[row_inc, temp - 1].Font.Underline = true;
                                                                                                                    // }

                                                                                                                    strquerytext = "select de.lp_code from dailyentdet de,dailystaffentry ds where de.lp_code=ds.lp_code  and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sectionvar + " and subject_no='" + sp2[0] + "' and  staff_code='" + stafcode + "' and sch_date='" + cur_day + "' and hr=" + temp + "";
                                                                                                                    dsstuatt.Reset();
                                                                                                                    dsstuatt.Dispose();
                                                                                                                    dsstuatt = da.select_method(strquerytext, hatvalue, "Text");
                                                                                                                    if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                    {
                                                                                                                        dailyentryflag = true;
                                                                                                                    }

                                                                                                                }
                                                                                                                dailyentryflag = false;
                                                                                                                attendanceentryflag = false;
                                                                                                                //Fpspread.Sheets[0].Cells[row_inc, temp - 1].Font.Bold = true;

                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            if (altfalg == false)
                                                                            {
                                                                                getcolumnfield = Convert.ToString(strday + temp);
                                                                                attendanceentryflag = false;
                                                                                dailyentryflag = false;
                                                                                // if (dsperiod.Tables[0].Rows[pre][getcolumnfield].ToString() != "" && dsperiod.Tables[0].Rows[pre][getcolumnfield].ToString() != null && dsperiod.Tables[0].Rows[pre][getcolumnfield].ToString() != "\0")
                                                                                if (dssem.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    if (dssem.Tables[0].Rows[0][getcolumnfield].ToString() != "" && dssem.Tables[0].Rows[0][getcolumnfield].ToString() != null && dssem.Tables[0].Rows[0][getcolumnfield].ToString() != "\0")
                                                                                    {
                                                                                        string timetable = "";
                                                                                        string name = dssem.Tables[0].Rows[0]["ttname"].ToString();
                                                                                        if (name != null && name.Trim() != "")
                                                                                        {
                                                                                            timetable = name;
                                                                                        }
                                                                                        sp_rd = dssem.Tables[0].Rows[0][getcolumnfield].ToString();
                                                                                        string[] sp_rd_semi = sp_rd.Split(';');

                                                                                        for (int semi = 0; semi <= sp_rd_semi.GetUpperBound(0); semi++)
                                                                                        {
                                                                                            string[] sp2 = sp_rd_semi[semi].Split(new Char[] { '-' });
                                                                                            if (sp2.GetUpperBound(0) >= 1)
                                                                                            {
                                                                                                int upperbound = sp2.GetUpperBound(0);
                                                                                                for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                                                {
                                                                                                    if (sp2[multi_staff] == stafcode)
                                                                                                    {

                                                                                                        //==============================theroy batch=======================================
                                                                                                        Boolean checklabhr = false;
                                                                                                        for (int sr = 0; sr <= sp_rd_semi.GetUpperBound(0); sr++)
                                                                                                        {
                                                                                                            string[] getlasub = sp_rd_semi[sr].ToString().Split('-');
                                                                                                            if (getlasub.GetUpperBound(0) > 1)
                                                                                                            {
                                                                                                                string srllab = getlasub[0].ToString();
                                                                                                                if (hatcurlab.Contains(srllab))
                                                                                                                {
                                                                                                                    checklabhr = true;
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        //======================================================================

                                                                                                        string sect = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                        if (sect == "-1" || sect == null || sect.Trim() == "")
                                                                                                        {
                                                                                                            sect = "";
                                                                                                        }
                                                                                                        if (semenddate.Trim() != "" && semenddate.Trim() != null && semenddate.Trim() != "0")
                                                                                                        {
                                                                                                            if (cur_day <= (Convert.ToDateTime(semenddate)))
                                                                                                            {
                                                                                                                check_hour = true;
                                                                                                                double Num;
                                                                                                                bool isNum = double.TryParse(sp2[0].ToString(), out Num);
                                                                                                                if (isNum)
                                                                                                                {
                                                                                                                    // text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-" + sp2[upperbound];
                                                                                                                    if (checklabhr == false)
                                                                                                                    {
                                                                                                                        text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-S";
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-L";
                                                                                                                    }
                                                                                                                    string Schedule_string = "";
                                                                                                                    if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "-1" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == null)
                                                                                                                    {
                                                                                                                        if (checklabhr == false)
                                                                                                                        {
                                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        if (checklabhr == false)
                                                                                                                        {
                                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                        }
                                                                                                                    }

                                                                                                                    Boolean allowleave = false;
                                                                                                                    if (hatholiday.Contains(cur_day.ToString()))
                                                                                                                    {
                                                                                                                        if (moringleav == true)
                                                                                                                        {
                                                                                                                            if (frshlf >= temp)
                                                                                                                            {
                                                                                                                                allowleave = true;
                                                                                                                            }
                                                                                                                        }
                                                                                                                        if (evenleave == true)
                                                                                                                        {
                                                                                                                            if (temp > frshlf)
                                                                                                                            {
                                                                                                                                allowleave = true;
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }

                                                                                                                    if (allowleave == true)
                                                                                                                    {
                                                                                                                        if (hatholiday.Contains(cur_day.ToString()))
                                                                                                                        {
                                                                                                                            string holidayreason = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {

                                                                                                                        dtrow = dtl.NewRow();
                                                                                                                        dtl.Rows.Add(dtrow);
                                                                                                                        if (dtl.Rows[dtl.Rows.Count-1][5].ToString().Trim() == "")
                                                                                                                        {

                                                                                                                            resultflag = true;
                                                                                                                            if (finalfalg == false)
                                                                                                                            {
                                                                                                                                cn++;
                                                                                                                                finalfalg = true;
                                                                                                                               
                                                                                                                                rowcnt = dtl.Rows.Count;
                                                                                                                            }
                                                                                                                            



                                    int col = 0;

                                    dtrow[col] = cn.ToString();
                                    col++;
                                                                                                                            
                                                                                                                            dtrow[col] = stafcode;
                                                                                                                            col++;
                                                                                                                            string stnm = "select staff_name from staffmaster where staff_code='" + stafcode + "'";
                                                                                                                            ds = da.select_method_wo_parameter(stnm, "Text");
                                                                                                                            


                                                                                                                            dtrow[col] = ds.Tables[0].Rows[0]["staff_name"].ToString();

                                                                                                                            col++;
                                                                                                                            

                                                                                                                            dtrow[col] = dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sect + "";
                                                                                                                            col++;

                                                                                                                            

                                                                                                                            dtrow[col] = text_temp;
                                                                                                                            col++;

                                                                                                                            

                                                                                                                            dtrow[col] = temp.ToString();
                                                                                                                            col++;

                                                                                                                            
                                                                                                                           
                                                                                                                            a = a + 1;
                                                                                                                            
                                                                                                                            
                                                                                                                            list.Add(text_temp + "-" + temp.ToString());

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
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                                //lb1: tmp_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["FromDate"]);
                                                // lb1: tmp_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);

                                            }
                                        }
                                    }
                                }



                                hatpdf.Add(stafcode, a);
                                lblerr.Visible = false;
                                divgrid.Visible = true;
                                Showgrid.Visible = true;
                                btngenerate.Visible = true;
                                //  Labelstaf.Visible = false;
                                


                               
                            }
                        }

                    }

                }

            }
             if (dtl.Rows.Count > 0)
                                {
                                    Showgrid.DataSource = dtl;
                                    Showgrid.DataBind();
                                    Showgrid.Visible = true;
                                    Showgrid.HeaderRow.Visible = false;
                                    divgrid.Visible = true;
                                   

                                    
                                    int dtrowcount = dtl.Rows.Count;
                                 int rowspanstart = 0;
                                 int rowspandegree = 0;
                                 int rowspansubname = 0;
                                 

                for (int i = 0; i < Showgrid.Rows.Count; i++)
                {
                    int rowspancount = 0;
                    int rowspancountdegree = 0;
                    int rowspancountsubname = 0;

                    if (i != dtrowcount - 1)
                    {
                        
                        if (rowspanstart == i)
                        {
                            for (int k = rowspanstart + 1; Showgrid.Rows[i].Cells[1].Text == Showgrid.Rows[k].Cells[1].Text; k++)
                            {
                                rowspancount++;
                                if (k == dtrowcount - 1)
                                    break;
                            }
                            rowspanstart++;
                        }
                        if (rowspandegree == i)
                        {
                            for (int k = rowspandegree + 1; Showgrid.Rows[i].Cells[4].Text == Showgrid.Rows[k].Cells[4].Text; k++)
                            {
                                rowspancountdegree++;
                                if (k == dtrowcount - 1)
                                    break;
                            }
                            rowspandegree++;
                        }
                        if (rowspansubname == i)
                        {
                            for (int k = rowspansubname + 1; Showgrid.Rows[i].Cells[5].Text == Showgrid.Rows[k].Cells[5].Text; k++)
                            {
                                rowspancountsubname++;
                                if (k == dtrowcount - 1)
                                    break;
                            }
                            rowspansubname++;
                        }


                        if (rowspancount != 0)
                        {
                            rowspanstart = rowspanstart + rowspancount;
                            Showgrid.Rows[i].Cells[0].RowSpan = rowspancount + 1;
                            for (int a = i; a < rowspanstart - 1; a++)
                                Showgrid.Rows[a + 1].Cells[0].Visible = false;

                            
                            Showgrid.Rows[i].Cells[1].RowSpan = rowspancount + 1;
                            for (int a = i; a < rowspanstart - 1; a++)
                                Showgrid.Rows[a + 1].Cells[1].Visible = false;


                            
                            Showgrid.Rows[i].Cells[2].RowSpan = rowspancount + 1;
                            for (int a = i; a < rowspanstart - 1; a++)
                                Showgrid.Rows[a + 1].Cells[2].Visible = false;


                            
                            Showgrid.Rows[i].Cells[3].RowSpan = rowspancount + 1;
                            for (int a = i; a < rowspanstart - 1; a++)
                                Showgrid.Rows[a + 1].Cells[3].Visible = false;


                           
                        }

                        if (rowspancountdegree != 0)
                        {
                            rowspandegree = rowspandegree + rowspancountdegree;
                            Showgrid.Rows[i].Cells[4].RowSpan = rowspancountdegree + 1;
                            for (int a = i; a < rowspandegree - 1; a++)
                                Showgrid.Rows[a + 1].Cells[4].Visible = false;
                        }

                        if (rowspancountsubname != 0)
                        {

                            rowspansubname = rowspansubname + rowspancountsubname;
                            Showgrid.Rows[i].Cells[5].RowSpan = rowspancountsubname + 1;
                            for (int a = i; a < rowspansubname - 1; a++)
                                Showgrid.Rows[a + 1].Cells[5].Visible = false;
                        }
                        
                       


                    }

                    for (int j = 0; j < Showgrid.HeaderRow.Cells.Count; j++)
                    {

                        if (i == 0)
                        {
                            Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                            Showgrid.Rows[i].Cells[j].Font.Bold = true;


                            if (j == 0)
                            {
                                var checkbox1 = Showgrid.Rows[i].Cells[0].FindControl("lbl_cb") as CheckBox;
                                checkbox1.Visible = false;

                            }
                        }
                        else
                        {


                            if (j == 0)
                            {
                                var checkbox1 = Showgrid.Rows[i].Cells[0].FindControl("chkselectall") as CheckBox;
                                checkbox1.Visible = false;

                            }
                            if (j == 1 || j == 6)
                            {
                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                            }

                        }

                    }





                }

                                    
                                }
            if (selflag == false)
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Select Staff Code";
                Showgrid.Visible = false;
                divgrid.Visible = false;
                btngenerate.Visible = false;
            }
            else if (resultflag == false)
            {
                lblerr.Visible = true;
                lblerr.Text = "No Records Found";
                Showgrid.Visible = false;
                divgrid.Visible = false;
                btngenerate.Visible = false;
            }
        }
        catch(Exception ex)
        {
            lblerr.Text = ex.ToString();
        }
    }



    public string find_day_order()
    {
        int holiday = 0;
        string query = "select CONVERT(VARCHAR(10),start_date,23) from seminfo where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + "  and batch_year=" + Session["batch_year"].ToString();
        string sdate = da.GetFunction(query);
        string quer = "select nodays from PeriodAttndSchedule where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString();
        string no_days = da.GetFunction(quer);
        if (sdate != "")
        {
            string curday = Session["sch_date"].ToString();
            string[] sp_date = sdate.Split(new Char[] { '-' });
            string start_date = sp_date[1].ToString() + "-" + sp_date[2].ToString() + "-" + sp_date[0].ToString();
            DateTime dt1 = Convert.ToDateTime(start_date);
            DateTime dt2 = Convert.ToDateTime(curday);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*) from holidaystudents  where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and halforfull='0'";
            string holday = da.GetFunction(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);

            int dif_days = ts.Days;
            int nodays = Convert.ToInt32(no_days);
            int order = (dif_days - holiday) % nodays;
            order = order + 1;
            string findday = "";
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";
            return findday;
        }
        else
            return "";

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
    public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    {
        int holiday = 0;
        if (no_days == "")
            return "";
        if (sdate != "")
        {
            string[] sp_date = sdate.Split(new Char[] { '-' });
            string start_date = sp_date[1].ToString() + "-" + sp_date[2].ToString() + "-" + sp_date[0].ToString();
            DateTime dt1 = Convert.ToDateTime(start_date);
            DateTime dt2 = Convert.ToDateTime(curday);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*)as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";
            string holday = da.GetFunction(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);
            int dif_days = ts.Days;
            //Added by Srinath 10/9/2013
            string leave = da.GetFunction(" select Holiday_desc from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date='" + dt2.ToString("yyyy-MM-dd") + "' ");
            if (leave != null && leave != "0")
            {
                dif_days = dif_days + 1;
            }
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
            if (order.ToString() == "0")
            {
                order = Convert.ToInt32(no_days);
            }

            string findday = "";
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";
            if (order >= 1)
            {
                Day_Order = Convert.ToString(order) + "-" + Convert.ToString(findday);
            }
            else
            {
                Day_Order = "";
            }
            return findday;
        }
        else
            return "";

    }



    //protected void Fpspread_command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    try
    //    {
    //        //string actrow = e.SheetView.ActiveRow.ToString();
    //        string actrow = e.CommandArgument.ToString();
    //        if (flag_true == false && actrow == "0")
    //        {
    //            for (int j = 0; j < Convert.ToInt16(Fpspread.Sheets[0].RowCount); j++)
    //            {
    //                string actcol = e.SheetView.ActiveColumn.ToString();
    //                string seltext = e.EditValues[6].ToString();
    //                if (seltext != "System.Object" && seltext != "Selector For All")
    //                {
    //                    Fpspread.Sheets[0].Cells[j, 6].Text = seltext.ToString();
    //                }
    //            }
    //            flag_true = true;
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    protected void btngenerate_click(object sender, EventArgs e)
    {
        bindpdf();
    }
    public void bindpdf()
    {
        try
        {
            
            Font Fontbold = new Font("Times New Roman", 12, FontStyle.Bold);
            Font Fontbold2 = new Font("Times New Roman", 16, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
            Gios.Pdf.PdfDocument myprovdoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            for (int i = 1; i < Showgrid.Rows.Count + 1; i++)
            {
                int isval = 0;
                

                var checkbox = Showgrid.Rows[i - 1].Cells[0].FindControl("lbl_cb") as CheckBox;
                if (checkbox.Checked)
                    isval = 1;
                if (isval == 1)
                {
                    saveflag = true;
                    hatfinal.Clear();
                    string staffcode = Showgrid.Rows[i - 1].Cells[2].Text;



                    string staffname = Showgrid.Rows[i - 1].Cells[3].Text;



                    string batch = Showgrid.Rows[i - 1].Cells[4].Text;

                    


                    string[] from = txtdate.Text.Split(new char[] { '/' });
                    string fromdate = from[0] + '-' + from[1] + '-' + from[2];
                    string from1 = from[2] + '-' + from[1] + '-' + from[0];
                    string clgcode = Session["collegecode"].ToString();
                    string[] s = batch.Split('-');
                    query = "select value from master_settings where settings='Academic year'";
                    ds = da.select_method_wo_parameter(query, "Text");
                    string[] yearset = ds.Tables[0].Rows[0]["value"].ToString().Split(',');
                    string finyr = yearset[0] + "-" + yearset[1];
                    int value = Convert.ToInt32(hatpdf[staffcode]);
                    string fromdate1 = from[0] + '/' + from[1] + '/' + from[2];

                    Gios.Pdf.PdfPage myprov_pdfpage = myprovdoc.NewPage();
                    string staffdept = "select distinct h.dept_name,s.dept_code from stafftrans s,hrdept_master h where s.dept_code=h.dept_code and s.staff_code='" + staffcode + "'";
                    ds = da.select_method_wo_parameter(staffdept, "Text");
                    string dept = ds.Tables[0].Rows[0]["dept_name"].ToString();
                    string deptcode = ds.Tables[0].Rows[0]["dept_code"].ToString();
                    string batchyear = s[0];
                    string acr = "";
                    string sbnm = "";
                    if (s[2] == "AUTO")
                    {
                        acr = "AME";
                    }
                    else if (s[2] == "CIVIL")
                    {
                        acr = "DCIVIL";
                    }
                    else if (s[2] == "EEE")
                    {
                        acr = "DEEE";
                    }
                    else
                    {
                        acr = s[2];
                    }

                    string degree_code = "";
                    if (dept == "Mathematics")
                    {
                        degree_code = "45";
                    }
                    else if (dept == "English")
                    {
                        degree_code = "45";
                    }
                    else if (dept == "Computer Maintenance Cell")
                    {
                        degree_code = "45";
                    }
                    else
                    {
                        string dq = "select Degree_Code from Degree where Dept_Code='" + deptcode + "'";
                        ds = da.select_method_wo_parameter(dq, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            degree_code = ds.Tables[0].Rows[0]["Degree_Code"].ToString();

                        }
                        else
                        {
                            degree_code = "45";
                        }
                    }

                    string sem = s[4];
                    string info = "";
                    if (sem == "1" || sem == "3" || sem == "5" || sem == "7" || sem == "9")
                    {
                        info = "Odd Semester";
                    }
                    else
                    {
                        info = "Even Semester";
                    }
                    Hashtable hatsub = new Hashtable();
                    for (int sub = 0; sub < value; sub++)
                    {
                        if (Showgrid.Rows.Count + 1 > (i + sub))
                        {
                            string x = Showgrid.Rows[((i-1) + sub)].Cells[6].Text;




                            string z = Showgrid.Rows[((i-1) + sub)].Cells[5].Text;


                            

                            if (!hatsub.Contains(x + z))
                            {
                                if (!hatfinal.Contains(x))
                                {
                                    hatfinal.Add(x, z);
                                }
                                else
                                {
                                    string gatval = hatfinal[x].ToString();
                                    gatval = gatval + " , " + z;
                                    hatfinal[x] = gatval;
                                }
                                hatsub.Add(x + z, x + z);
                            }
                        }
                        //  i++;

                    }
                    // i--;


                    // PdfArea tete = new PdfArea(myprovdoc, 490, 40, 80, 70);
                    int y = 40;
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        PdfImage LogoImage = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        myprov_pdfpage.Add(LogoImage, 20, 30, 350);
                    }

                    string clgquery = "select collname,address1,address2,address3,pincode from collinfo where college_code='" + clgcode + "'";
                    ds = da.select_method_wo_parameter(clgquery, "Text");
                    string name = ds.Tables[0].Rows[0]["collname"].ToString();
                    string add = ds.Tables[0].Rows[0]["address1"].ToString() + "," + ds.Tables[0].Rows[0]["address2"].ToString() + "," + ds.Tables[0].Rows[0]["address3"].ToString() + "-" + ds.Tables[0].Rows[0]["pincode"].ToString();

                    PdfTextArea ptc1 = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                      new PdfArea(myprovdoc, 20, y, 650, 30), System.Drawing.ContentAlignment.MiddleCenter, name);


                    PdfTextArea ptc2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                      new PdfArea(myprovdoc, 30, y + 20, 650, 30), System.Drawing.ContentAlignment.MiddleCenter, add);

                    PdfTextArea ptc3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                     new PdfArea(myprovdoc, 110, y + 35, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, finyr + " Academic Year-" + info);

                    PdfTextArea ptc4 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                    new PdfArea(myprovdoc, 160, y + 48, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, "Daywise Staff Report                               Date:");

                    PdfTextArea ptc5 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                   new PdfArea(myprovdoc, 355, y + 48, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, fromdate1);

                    string periodtimequery = "select distinct Period1,RIGHT(CONVERT(VARCHAR, start_time, 100),7) as STime,RIGHT(CONVERT(VARCHAR, end_time, 100),7) as ETime from BellSchedule where batch_year='" + batchyear + "' and Degree_Code='" + degree_code + "' and semester='" + sem + "' order by Period1";
                    ds = da.select_method_wo_parameter(periodtimequery, "Text");
                    int rowcount = ds.Tables[0].Rows.Count;

                    Gios.Pdf.PdfTable table = myprovdoc.NewTable(Fontsmall, rowcount + 3, 6, 1);
                    table.VisibleHeaders = false;
                    table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    table.Columns[0].SetWidth(20);
                    table.Columns[1].SetWidth(40);
                    table.Columns[2].SetWidth(60);
                    table.Columns[3].SetWidth(80);
                    table.Columns[4].SetWidth(80);
                    table.Columns[5].SetWidth(60);
                    table.Cell(0, 0).SetContent("Faculty Name             ");
                    table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 0).SetFont(Fontbold1);
                    table.Cell(0, 2).SetContent(staffname);
                    table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table.Cell(0, 2).SetFont(Fontsmall);
                    table.Cell(0, 5).SetContent("Faculty ID:" + staffcode);
                    table.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 5).SetFont(Fontbold1);
                    table.Cell(1, 0).SetContent("Department               ");
                    table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(1, 0).SetFont(Fontbold1);
                    table.Cell(1, 2).SetContent(dept);
                    table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table.Cell(1, 2).SetFont(Fontsmall);

                    foreach (PdfCell pr in table.CellRange(0, 0, 0, 0).Cells)
                    {
                        pr.ColSpan = 2;
                    }
                    foreach (PdfCell pr in table.CellRange(0, 2, 0, 2).Cells)
                    {
                        pr.ColSpan = 3;
                    }
                    foreach (PdfCell pr in table.CellRange(1, 0, 1, 0).Cells)
                    {
                        pr.ColSpan = 2;
                    }
                    foreach (PdfCell pr in table.CellRange(1, 2, 1, 2).Cells)
                    {
                        pr.ColSpan = 4;
                    }

                    table.Cell(2, 0).SetContent("           S.No       ");
                    table.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                    table.Cell(2, 0).SetFont(Fontbold1);
                    table.Cell(2, 1).SetContent("Hour");
                    table.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(2, 1).SetFont(Fontbold1);
                    table.Cell(2, 2).SetContent("Timing");
                    table.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(2, 2).SetFont(Fontbold1);
                    table.Cell(2, 3).SetContent("Subject");
                    table.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(2, 3).SetFont(Fontbold1);
                    table.Cell(2, 4).SetContent("Task Performed");
                    table.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(2, 4).SetFont(Fontbold1);
                    table.Cell(2, 5).SetContent("Remarks");
                    table.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(2, 5).SetFont(Fontbold1);
                    int cn = 0;
                    int rw = 0;
                    // hatpdf.ContainsKey(staffcode);

                    for (int c = 3; c < rowcount + 3; c++)
                    {
                        string prd = ds.Tables[0].Rows[rw]["Period1"].ToString();
                        string st = ds.Tables[0].Rows[rw]["STime"].ToString();
                        string et = ds.Tables[0].Rows[rw]["ETime"].ToString();
                        string finval = st + "-" + et;
                        cn++;
                        table.Cell(c, 0).SetContent(cn);
                        table.Cell(c, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(c, 0).SetFont(Fontbold1);
                        table.Cell(c, 1).SetContent(prd);
                        table.Cell(c, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(c, 1).SetFont(Fontbold1);
                        table.Cell(c, 2).SetContent(finval);
                        table.Cell(c, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(c, 2).SetFont(Fontbold1);
                        string dailytopicname = "";
                        string subno = "";

                        if (prd != "Break1" && prd != "Break2" && prd != "Lunch" && prd != "Break3")
                        //if (prd == "1" || prd == "2" || prd == "3" || prd == "4" || prd == "5" || prd == "6" || prd == "7" || prd == "8" || prd == "9" || prd == "10" || prd == "11" || prd == "12" || prd == "13")
                        {
                            string taskquery = "select a.topics,a.hr,a.subject_no from dailyentdet a,dailyStaffEntry b where a.lp_code=b.lp_code and b.sch_date='" + from1 + "' and a.staff_code='" + staffcode + "' and b.semester='" + sem + "' and b.batch_year='" + batchyear + "' and a.hr='" + prd + "'";
                            ds1 = da.select_method_wo_parameter(taskquery, "Text");
                            string dailyunitname = "";

                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                subno = ds1.Tables[0].Rows[0]["subject_no"].ToString();
                                dailyunitname = ds1.Tables[0].Rows[0]["topics"].ToString();
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
                                    ds2 = da.select_method(unitquery, hat, "Text");
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
                        }

                        if (hatfinal.ContainsKey(Convert.ToString(cn)) == true)
                        {
                            sbnm = Convert.ToString(hatfinal[Convert.ToString(cn)]);

                        }
                        else
                        {
                            sbnm = "";
                        }



                        table.Cell(c, 3).SetContent(sbnm);
                        table.Cell(c, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(c, 3).SetFont(Fontbold1);


                        table.Cell(c, 4).SetContent(dailytopicname);
                        table.Cell(c, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(c, 4).SetFont(Fontbold1);

                        table.Cell(c, 5).SetContent("                                                                               ");
                        table.Cell(c, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(c, 5).SetFont(Fontbold1);

                        rw++;
                    }
                    Gios.Pdf.PdfTablePage myprov_pdfpage1 = table.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, 120, 557, 900));

                    PdfArea tete = new PdfArea(myprovdoc, 25, y + 420, 550, 150);
                    PdfRectangle pr1 = new PdfRectangle(myprovdoc, tete, Color.Black);
                    myprov_pdfpage.Add(pr1);

                    PdfTextArea ptc6 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                   new PdfArea(myprovdoc, 5, y + 425, 125, 30), System.Drawing.ContentAlignment.MiddleCenter, "HOD Remarks");

                    PdfArea tete1 = new PdfArea(myprovdoc, 25, y + 580, 550, 150);
                    PdfRectangle pr2 = new PdfRectangle(myprovdoc, tete1, Color.Black);
                    myprov_pdfpage.Add(pr2);

                    PdfTextArea ptc7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                 new PdfArea(myprovdoc, 5, y + 585, 145, 30), System.Drawing.ContentAlignment.MiddleCenter, "Principal Remarks");

                    myprov_pdfpage.Add(myprov_pdfpage1);
                    myprov_pdfpage.Add(ptc1);
                    myprov_pdfpage.Add(ptc2);
                    myprov_pdfpage.Add(ptc3);
                    myprov_pdfpage.Add(ptc4);
                    myprov_pdfpage.Add(ptc5);
                    myprov_pdfpage.Add(ptc6);
                    myprov_pdfpage.Add(ptc7);
                    myprov_pdfpage.SaveToDocument();

                }

            }
            if (saveflag == false)
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Select Atleast Anyone Detail";
            }
            else
            {

                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    Response.Buffer = true;
                    Response.Clear();
                    string szPath = appPath + "/Report/";
                    string szFile = "Form.pdf";
                    myprovdoc.SaveToFile(szPath + szFile);

                    Response.ClearHeaders();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }

            }
        }
        catch (Exception ex)
        {
            lblerr.Visible = true;
            lblerr.Text = ex.ToString();
        }
    }


    protected void chkselectall_CheckedChanged(object sender, EventArgs e)
    
    
    {

        var checkbox = Showgrid.Rows[0].Cells[0].FindControl("chkselectall") as CheckBox;
     
        for (int i = 1; i < Showgrid.Rows.Count; i++)
        {


            if (checkbox.Checked == true)
            {
                var checkbox1 = Showgrid.Rows[i].Cells[0].FindControl("lbl_cb") as CheckBox;
                checkbox1.Checked = true;

            }
            else
            {
                var checkbox1 = Showgrid.Rows[i].Cells[0].FindControl("lbl_cb") as CheckBox;
                checkbox1.Checked = false;
            }
        }

        
    }
}
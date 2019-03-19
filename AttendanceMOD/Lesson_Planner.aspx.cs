using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;

public partial class Lesson_Planner : System.Web.UI.Page
{
    string schorder = "";
    string Day_Order = "";
    string subcode_tot = "";
    Boolean chk = false;
    static Boolean forschoolsetting = false;
    DataSet ds = new DataSet();
    DataSet dsholyday = new DataSet();
    DataSet dsstaff = new DataSet();

    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable htlessonplan = new Hashtable();
    Hashtable htdailyentdet = new Hashtable();
    DataSet ds2 = new DataSet();
    DataSet ds_lpcode = new DataSet();

    //---------------------------Added by Mani
    DataTable dt = new DataTable();
    DataTable dt_dailyentdet1 = new DataTable();


    DataTable dt_1 = new DataTable();
    DataTable dt_det = new DataTable();

    int ar;//= 0;
    string subj = "";// string.Empty;
    string[] subj_split;
    string col_value1 = "";// string.Empty;

    string ht_value = string.Empty;
    string[] ht_split;
    string ht_degreecode = "";// string.Empty;
    int ht_num_degreecode = 0;
    string sem = "";//string.Empty;
    string date_6 = "";//string.Empty;
    string[] getdate_2;
    string getdate_3 = "";// string.Empty;
    DateTime ht_datetime;
    DateTime ht_datetime1;
    DateTime set_date;

    string cls_hour = "";// string.Empty;
    string[] split_value_1;
    string ht_hour = "";// string.Empty;
    int ht_num_hr = 0;

    string ht_staff_code = "";//string.Empty;
    string get_node_code = "";
    string get_node_text = "";

    int c;// = 0;
    int nu_batchyear;//= 0;    
    int nu_semester;//= 0;
    string sections = "";

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string group_code = string.Empty;
    string columnfield = string.Empty;
    //------------

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null) //Aruna For Back Button
            {
                Response.Redirect("~/Default.aspx");
            }
            usercode = Session["UserCode"].ToString();
            group_code = Session["group_code"].ToString();
            lblerror.Visible = false;
            if (!IsPostBack)
            {
                FpSpread1.ActiveSheetView.AutoPostBack = true;
                Session["chk_bool"] = chk;
                panel3.Visible = false;
                FpSpread1.Width = 950;

                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.CommandBar.Visible = true;
                FpSpread1.Sheets[0].SheetName = " ";
                FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = System.Drawing.Color.Black;
                style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].AllowTableCorner = true;

                txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtfrom.Attributes.Add("readonly", "readonly");
                txtto.Attributes.Add("readonly", "readonly");

                FpSpread1.Visible = false;
                lblerror.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                collegecode = ddlcollege.SelectedValue.ToString();
                usercode = Session["UserCode"].ToString();
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
                ds = d2.select_method("bind_college", hat, "sp");
                ddlcollege.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.DataSource = ds;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    ddlcollege_SelectedIndexChanged(sender, e);
                }
                else
                {
                    ddlcollege.Enabled = false;
                    ddlBatch.Enabled = false;
                    ddlBranch.Enabled = false;
                    ddlSemYr.Enabled = false;
                    ddlSec.Enabled = false;
                    GO.Enabled = false;
                    lblerror.Visible = true;
                    lblerror.Text = "Please Set College's Rights And Proceed";
                }
                string grouporusercodeschool = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercodeschool = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercodeschool = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                // Added By Sridharan 12 Mar 2015
                //{
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
                        Label5.Text = "School";
                        lblYear.Text = "Year";
                        lblDegree.Text = "School Type";
                        lblBranch.Text = "Standard";
                        lblDuration.Text = "Term";
                        //lblDegree.Attributes.Add("style", " width: 95px;");
                        //lblBranch.Attributes.Add("style", " width: 67px;");
                        //ddlBranch.Attributes.Add("style", " width: 241px;");
                    }
                    else
                    {
                        forschoolsetting = false;
                    }
                }
                //} Sridharan
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void chklstsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            psubject.Focus();
            txtsubject.Text = "--Select--";
            chksubject.Checked = false;
            int commcount = 0;
            for (int i = 0; i < chklstsubject.Items.Count; i++)
            {
                if (chklstsubject.Items[i].Selected == true)
                {
                    commcount = commcount + 1;

                    if (subcode_tot == "")
                    {
                        subcode_tot = chklstsubject.Items[i].Value.ToString();
                    }
                    else
                    {
                        subcode_tot = subcode_tot + "," + chklstsubject.Items[i].Value;
                    }
                }
            }
            if (commcount > 0)
            {
                txtsubject.Text = "Subject(" + commcount.ToString() + ")";
                if (commcount == chklstsubject.Items.Count)
                {
                    chksubject.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }

    }
    protected void chksubject_CheckedChanged(object sender, EventArgs e)
    {

        try
        {
            if (chklstsubject.Items.Count > 0)
            {
                if (chksubject.Checked == true)
                {
                    for (int i = 0; i < chklstsubject.Items.Count; i++)
                    {
                        chklstsubject.Items[i].Selected = true;
                        txtsubject.Text = "Subject(" + (chklstsubject.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chklstsubject.Items.Count; i++)
                    {
                        chklstsubject.Items[i].Selected = false;
                        txtsubject.Text = "---Select---";
                    }
                }

            }

        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();

        }
    }
    protected void GO_Click(object sender, EventArgs e)   // **************************** start Modify by jairam 21-11-2014************
    {
        try
        {

            string topicname = "";
            string sectiontopic = ddlSec.Text;

            string plannersec = "";
            string hr = "";
            string subjectname = "";


            string[] splitfromcheck = txtfrom.Text.Split(new Char[] { '/' });
            string[] splittocheck = txtto.Text.Split(new char[] { '/' });
            string fdate = splitfromcheck[1] + '/' + splitfromcheck[0] + '/' + splitfromcheck[2];
            string tdate = splittocheck[1] + '/' + splittocheck[0] + '/' + splittocheck[2];
            DateTime fromdatechech = Convert.ToDateTime(fdate);
            DateTime todatecheck = Convert.ToDateTime(tdate);

            if (fromdatechech > todatecheck)
            {
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                lblerror.Text = "Please Enter To Date Greater Than From Date";// Sangeetha on 01 Sep 2013
                lblerror.Visible = true;
            }
            else
            {
                if (ddlSemYr.Enabled != false && chklstsubject.Enabled != false && txtsubject.Enabled != false)
                {

                    FpSpread1.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnxl.Visible = true;
                    btnprintmaster.Visible = true;
                    Printcontrol.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 5;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].ColumnHeader.Visible = true;
                    FpSpread1.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    FpSpread1.Sheets[0].ColumnHeader.Rows[0].Height = 50;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Hour";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Topic";
                    FpSpread1.Sheets[0].Columns[0].Width = 50;
                    FpSpread1.Sheets[0].Columns[1].Width = 80;
                    FpSpread1.Sheets[0].Columns[2].Width = 350;
                    FpSpread1.Sheets[0].Columns[3].Width = 150;
                    FpSpread1.Sheets[0].Columns[4].Width = 320;

                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    FpSpread1.Sheets[0].Columns[0].CellType = txt;
                    FpSpread1.Sheets[0].Columns[1].CellType = txt;
                    FpSpread1.Sheets[0].Columns[2].CellType = txt;
                    FpSpread1.Sheets[0].Columns[3].CellType = txt;
                    FpSpread1.Sheets[0].Columns[4].CellType = txt;

                    FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);


                    FpSpread1.Sheets[0].RowCount = 0;

                    string batchyear = ddlBatch.SelectedValue.ToString();
                    string degree_code = ddlBranch.SelectedValue.ToString();
                    string semester = ddlSemYr.SelectedValue.ToString();
                    string section = ddlSec.SelectedValue.ToString();//Added by M.SakthiPriya 19/12/2014
                    string subjectcode = "";

                    string strsection = "";
                    string secval = "";
                    string secval1 = "";
                    if (section.Trim().ToString() == "All")
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
                    else if (section.Trim().ToString() == "")
                    {
                        strsection = "";
                    }
                    else
                    {
                        strsection = "" + ddlSec.SelectedValue.ToString() + "";
                        secval = " and st.Sections='" + section + "'";
                        secval1 = " and sections='" + section + "'"; //added by Mullai

                    }



                    // Modify by jairam 21-11-2014
                    string noofdays = "";
                    string start_datesem = "";
                    string start_dayorder = "";
                    string end_datesem = "";
                    int maxhour = 0;
                    string dayorderquery = "select s.start_date,s.end_date,s.starting_dayorder,p.nodays,p.schorder,p.No_of_hrs_per_day from periodattndschedule p,seminfo s where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code=" + degree_code + " and s.semester=" + semester + " and batch_year=" + batchyear + "";
                    DataSet dsdayorder = d2.select_method(dayorderquery, hat, "Text");
                    if (dsdayorder.Tables[0].Rows.Count > 0)
                    {
                        schorder = dsdayorder.Tables[0].Rows[0]["SchOrder"].ToString();
                        noofdays = dsdayorder.Tables[0].Rows[0]["nodays"].ToString();
                        start_datesem = dsdayorder.Tables[0].Rows[0]["start_date"].ToString();
                        end_datesem = dsdayorder.Tables[0].Rows[0]["end_date"].ToString();
                        start_dayorder = dsdayorder.Tables[0].Rows[0]["starting_dayorder"].ToString();
                        maxhour = Convert.ToInt32(dsdayorder.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                    }

                    DataView dvmain = new DataView();
                    DataSet alterdata = new DataSet();
                    string alterquery = " select * from Alternate_Schedule where batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " and Fromdate between '" + fromdatechech.ToString("MM/dd/yyyy") + "' and '" + todatecheck.ToString("MM/dd/yyyy") + "'";
                    alterquery = alterquery + " select s.subject_code,s.subType_no,s.subject_name,s.subject_no from subject s,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and sy.degree_code=" + degree_code + " and sy.Batch_Year=" + batchyear + " and sy.semester=" + semester + "";
                    alterquery = alterquery + " select s.subject_code,s.subType_no,s.subject_name,s.subject_no,su.unit_name,su.topic_no,su.parent_code from subject s,sub_sem ss,syllabus_master sy,sub_unit_details su where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.subject_no=su.subject_no and sy.degree_code=" + degree_code + " and sy.Batch_Year=" + batchyear + " and sy.semester=" + semester + " order by s.subject_no,su.parent_code,su.topic_no";
                    alterquery = alterquery + "  select * from Semester_Schedule where batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " order by fromdate desc";
                    alterquery = alterquery + " select distinct S.subject_no,subject_code,subject_name,st.staff_code,sections from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code  and st.subject_no=s.subject_no   and S.subtype_no = Sem.subtype_no and promote_count=1 and SM.degree_code=" + degree_code + " and  SM.batch_year=" + batchyear + " and SM.semester=" + semester + " " + secval + " ";
                    alterquery = alterquery + " select * from lesson_plan p,lessonplantopics l where l.lp_code=p.lp_code and p.degree_code=" + degree_code + " and p.Batch_Year=" + batchyear + " and p.semester=" + semester + " and sch_date between '" + fromdatechech.ToString("MM/dd/yyyy") + "' and '" + todatecheck.ToString("MM/dd/yyyy") + "' " + secval1 + " ";  //modified by Mullai
                    alterquery = alterquery + "   select * from lessonplantopics as lpt,lesson_plan as lp where lp.lp_code=lpt.lp_code and lp.degree_code=" + degree_code + " and lp.semester=" + semester + " " + secval1 + " order by lpt.lp_code desc";  //modified by Mullai
                    alterdata = d2.select_method_wo_parameter(alterquery, "Text");

                    // string totalhour = "select isnull(max(No_of_hrs_per_day),0) from PeriodAttndSchedule where  degree_code=" + degree_code + " and semester=" + semester + "";
                    if (maxhour > 0)//Added By srinath 23/8/2013
                    {
                        DataView holidayview = new DataView();
                        string holydayquery = "select * from holidaystudents where degree_code=" + degree_code + " and semester=" + semester + " and holiday_date between '" + fromdatechech.ToString("MM/dd/yyyy") + "' and '" + todatecheck.ToString("MM/dd/yyyy") + "'";
                        dsholyday.Dispose();
                        dsholyday.Reset();
                        dsholyday = d2.select_method(holydayquery, hat, "Text");

                        string[] from = txtfrom.Text.Split(new char[] { '/' });
                        string fromdate = from[1] + '/' + from[0] + '/' + from[2];

                        string[] to = txtto.Text.Split(new char[] { '/' });
                        string todate = to[1] + '/' + to[0] + '/' + to[2];

                        DateTime fromday1 = Convert.ToDateTime(fromdate);
                        DateTime today = Convert.ToDateTime(todate);
                        string classhour = "";
                        int sno = 0;

                        //SqlCommand cmd_findSem = new SqlCommand("select start_date,end_date from seminfo where semester=" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + "", con);
                        //DataTable dt_findsem = new DataTable();
                        //SqlDataAdapter da_findsem = new SqlDataAdapter(cmd_findSem);
                        //da_findsem.Fill(dt_findsem);
                        if (start_datesem.Trim() != "" && end_datesem.Trim() != "" && start_datesem != null && end_datesem != null)
                        {
                            DateTime s_date = Convert.ToDateTime(start_datesem);
                            DateTime e_date = Convert.ToDateTime(end_datesem);


                            string sectionvalue = "";
                            for (DateTime caldate = fromday1; caldate <= today; caldate = caldate.AddDays(1))
                            {
                                if (caldate >= s_date && caldate <= e_date)
                                {
                                    //string find_sem = dt_findsem.Rows[0][0].ToString();

                                    string[] caldtesplit = Convert.ToString(caldate).Split(' ');
                                    string[] datesplit = Convert.ToString(caldtesplit[0]).Split('/');
                                    string date = datesplit[1] + '/' + datesplit[0] + '/' + datesplit[2];
                                    string querydate = Convert.ToString(caldtesplit[0]);

                                    // DataRow drholyday = dsholyday.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<DateTime>("holiday_date") == caldate);
                                    dsholyday.Tables[0].DefaultView.RowFilter = "degree_code=" + degree_code + " and semester=" + semester + " AND holiday_date='" + caldate.ToString("MM/dd/yyy") + "'"; // Modify by Jairam 21-11-2014
                                    holidayview = dsholyday.Tables[0].DefaultView;
                                    //  if (drholyday == null)
                                    if (holidayview.Count == 0)
                                    {
                                        string dayget = "";
                                        if (schorder == "1")
                                        {
                                            dayget = Convert.ToString(caldate.ToString("ddd"));
                                        }
                                        else
                                        {
                                            string[] startdatspilt = start_datesem.Split(' ');
                                            start_datesem = startdatspilt[0].ToString();
                                            dayget = d2.findday(querydate.ToString(), degree_code, semester, batchyear, start_datesem.ToString(), noofdays.ToString(), start_dayorder);
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

                                            Boolean headrflga = false;

                                            if (chksectionvalue == "")
                                            {
                                                sectionvalue = "";
                                            }
                                            else
                                            {
                                                sectionvalue = " and Sections='" + chksectionvalue.ToString() + "'";
                                            }

                                            alterdata.Tables[3].DefaultView.RowFilter = "fromdate <= '" + querydate.ToString() + "' " + sectionvalue + " "; //modified by Mullai
                                            dvmain = alterdata.Tables[3].DefaultView;
                                            //string shedulequery = "select top 1 " + classhour + ",batch_year,degree_code,semester,sections from semester_schedule where sections<>'-1' and batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " " + sectionvalue + " and fromdate <= '" + querydate.ToString() + "'  order by fromdate desc";
                                            //ds.Dispose();
                                            //ds.Reset();
                                            //ds = d2.select_method(shedulequery, hat, "Text");

                                            DataView dvalter = new DataView();
                                            DataView dvalter1 = new DataView();
                                            DataView dvalter2 = new DataView();
                                            DataView dvsub1 = new DataView();
                                            DataView dvsub2 = new DataView();
                                            DataView dvplan = new DataView();
                                            if (dvmain.Count > 0)
                                            {
                                                for (int i = 0; i < dvmain.Count; i++)
                                                {
                                                    string[] classhourspilt = classhour.Split(new char[] { ',' });
                                                    for (int colu = 0; colu <= classhourspilt.GetUpperBound(0); colu++)
                                                    {
                                                        string columnvalue = classhourspilt[colu].ToString();
                                                        string classhour1 = "";
                                                        if (chkwithoutalter.Checked == false) // Added by sridharan april 30 2015
                                                        {
                                                            alterdata.Tables[0].DefaultView.RowFilter = " batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " " + sectionvalue + " and fromdate= '" + querydate.ToString() + "'";
                                                            dvalter = alterdata.Tables[0].DefaultView;
                                                        }
                                                        //string altershedulequery = "select " + columnvalue + " from Alternate_schedule where batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " " + sectionvalue + " and fromdate= '" + querydate.ToString() + "'";
                                                        //DataSet dsaltershudel = d2.select_method(altershedulequery, hat, "Text");
                                                        if (dvalter.Count > 0)
                                                        {
                                                            classhour1 = dvalter[0][columnvalue].ToString();
                                                        }

                                                        if (classhour1 == "" || classhour1 == null)
                                                        {
                                                            classhour1 = dvmain[i][columnvalue].ToString();
                                                        }

                                                        if (classhour1.ToString().Trim() != "")
                                                        {
                                                            string[] splitcode = classhour1.Split(';');

                                                            string[] split_sub_code = subcode_tot.Split(',');

                                                            for (int k = 0; k <= splitcode.GetUpperBound(0); k++)
                                                            {
                                                                string staffcodecheck = splitcode[k].ToString();
                                                                string[] staffsubject = staffcodecheck.Split('-');
                                                                for (int l = 1; l < staffsubject.GetUpperBound(0); l++)
                                                                {
                                                                    string tempstaffcode = staffsubject[l].ToString();
                                                                    string tempsubject_no = staffsubject[0].ToString();
                                                                    string staffquery = "";
                                                                    string sectionstraff = "";


                                                                    string strstaffcode = staffsubject[l].ToString();
                                                                    if (Session["Staff_Code"].ToString() != null && Session["Staff_Code"].ToString() != "")
                                                                    {
                                                                        strstaffcode = Session["Staff_Code"].ToString();
                                                                    }

                                                                    if (tempstaffcode == strstaffcode)
                                                                    {

                                                                        if (sectionvalue == "")
                                                                        {
                                                                            sectionstraff = "";
                                                                        }
                                                                        else
                                                                        {
                                                                            sectionstraff = "and sections='" + chksectionvalue.ToString() + "'";
                                                                        }

                                                                        if (tempstaffcode.Trim().ToString() == "" || tempsubject_no.Trim().ToString() == "")
                                                                        {
                                                                            goto lb;
                                                                        }


                                                                        if (chklstsubject.Text != "--Select--")
                                                                        {
                                                                            alterdata.Tables[4].DefaultView.RowFilter = "subject_no='" + tempsubject_no.ToString() + "' " + sectionstraff + " and staff_code='" + tempstaffcode.ToString() + "'";
                                                                            // staffquery = "select distinct S.subject_no,subject_code,subject_name,st.staff_code from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code  and st.subject_no=s.subject_no   and S.subtype_no = Sem.subtype_no and promote_count=1 and SM.degree_code=" + degree_code + " and  SM.batch_year=" + batchyear + " and SM.semester=" + semester + " " + sectionstraff + " and s.subject_no='" + tempsubject_no.ToString() + "' and staff_code='" + tempstaffcode.ToString() + "' order by subject_code";
                                                                        }

                                                                        else if (chklstsubject.Text != "--Select--")
                                                                        {
                                                                            alterdata.Tables[4].DefaultView.RowFilter = "subject_no='" + chklstsubject.SelectedValue.ToString() + "' " + sectionstraff + " and staff_code='" + tempstaffcode.ToString() + "'";
                                                                            // staffquery = "select distinct S.subject_no,subject_code,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code  and st.subject_no=s.subject_no   and S.subtype_no = Sem.subtype_no and promote_count=1 and SM.degree_code=" + degree_code + " and  SM.batch_year=" + batchyear + " and SM.semester=" + semester + " " + sectionstraff + " and s.subject_no='" + chklstsubject.SelectedValue.ToString() + "' and staff_code='" + tempstaffcode.ToString() + "' order by subject_code";
                                                                        }
                                                                        dvsub1 = alterdata.Tables[4].DefaultView;
                                                                        if (dvsub1.Count > 0)
                                                                        {
                                                                            //for (int staff = 0; staff < dvsub1.Count; staff++)
                                                                            //{

                                                                            subjectcode = dvsub1[0]["Subject_no"].ToString();
                                                                            string subjstaff = subjectcode;
                                                                            string tempsubjstaff = tempstaffcode + '/' + tempsubject_no;
                                                                            for (int m = 0; m < chklstsubject.Items.Count; m++)
                                                                            {
                                                                                if (chklstsubject.Items[m].Selected == true)
                                                                                {
                                                                                    string subject = chklstsubject.Items[m].Value;
                                                                                    if (subjstaff == subject)
                                                                                    {

                                                                                        string staffcode = tempstaffcode;
                                                                                        alterdata.Tables[1].DefaultView.RowFilter = "subject_no='" + tempsubject_no + "'";
                                                                                        dvalter1 = alterdata.Tables[1].DefaultView;
                                                                                        //ds2.Reset();
                                                                                        //ds2.Dispose();

                                                                                        //ds2 = d2.select_method("select Subject_name from subject where subject_no='" + tempsubject_no + "'", hat, "Text");
                                                                                        if (dvalter1.Count > 0)
                                                                                        {

                                                                                            subjectname = dvalter1[0]["Subject_name"].ToString();

                                                                                        }

                                                                                        //ds2.Reset();
                                                                                        //ds2.Dispose();

                                                                                        for (int spilt = 3; spilt < columnvalue.Length; spilt++)
                                                                                        {
                                                                                            hr = columnvalue[spilt].ToString();
                                                                                        }

                                                                                        if (sectiontopic.Trim().ToString() != "All" && sectiontopic.Trim().ToString() != "")
                                                                                        {
                                                                                            plannersec = "and sections='" + ddlSec.SelectedValue.ToString() + "'";
                                                                                        }

                                                                                        alterdata.Tables[5].DefaultView.RowFilter = " sch_date='" + querydate.ToString() + "'  and staff_code='" + tempstaffcode + "' and hr=" + hr + " " + plannersec + "";
                                                                                        dvsub2 = alterdata.Tables[5].DefaultView;
                                                                                        //ds2.Reset();
                                                                                        //ds2.Dispose();
                                                                                        string unitname = "";
                                                                                        //string topicquery = "select topics from lesson_plan p,lessonplantopics l where l.lp_code=p.lp_code  and  sch_date='" + querydate.ToString() + "'  and l.staff_code='" + tempstaffcode + "' and l.hr=" + hr + " " + plannersec + "";
                                                                                        //ds2 = d2.select_method(topicquery, hat, "Text");
                                                                                        if (dvsub2.Count > 0)
                                                                                        {
                                                                                            unitname = dvsub2[0]["topics"].ToString();
                                                                                        }
                                                                                        if (unitname != "")
                                                                                        {
                                                                                            string[] unitname1;
                                                                                            string unitnamespilt;
                                                                                            unitname1 = unitname.Split('/');
                                                                                            for (int j = 0; j <= unitname1.GetUpperBound(0); j++)
                                                                                            {
                                                                                                unitnamespilt = unitname1[j];
                                                                                                alterdata.Tables[2].DefaultView.RowFilter = "topic_no='" + unitnamespilt + "'";
                                                                                                dvalter2 = alterdata.Tables[2].DefaultView;
                                                                                                //string unitquery = "select unit_name from sub_unit_details where topic_no='" + unitnamespilt + "'";
                                                                                                //ds2.Dispose();
                                                                                                //ds2.Reset();
                                                                                                //ds2 = d2.select_method(unitquery, hat, "Text");
                                                                                                if (dvalter2.Count > 0)
                                                                                                {
                                                                                                    if (topicname == "")
                                                                                                    {
                                                                                                        topicname = dvalter2[0]["unit_name"].ToString();
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        topicname = topicname + " / " + dvalter2[0]["unit_name"].ToString();
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }


                                                                                        string dailysec = "";

                                                                                        if (sectiontopic.Trim().ToString() != "All" && sectiontopic.Trim().ToString() != "")
                                                                                        {
                                                                                            dailysec = "and ds.sections='" + ddlSec.SelectedValue.ToString() + "'";
                                                                                        }

                                                                                        if (headrflga == false)
                                                                                        {
                                                                                            if (sectionspilt.GetUpperBound(0) > 0)
                                                                                            {
                                                                                                headrflga = true;
                                                                                                FpSpread1.Sheets[0].RowCount++;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = date.ToString();
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Large;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Batch : " + batchyear + " " + '-' + " Branch : " + ddlBranch.SelectedItem.ToString() + " - Sem : " + ddlSemYr.Text.ToString() + " " + '-' + " Section " + '-' + " " + chksectionvalue + " ";
                                                                                                if (forschoolsetting == true)
                                                                                                {
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Year : " + batchyear + " " + '-' + " Standard : " + ddlBranch.SelectedItem.ToString() + " - Term : " + ddlSemYr.Text.ToString() + " " + '-' + " Section " + '-' + " " + chksectionvalue + " ";
                                                                                                }
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Large;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.Blue;
                                                                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 4);
                                                                                            }
                                                                                        }

                                                                                        sno++;
                                                                                        string[] datespilt = Convert.ToString(caldate).Split(' ');
                                                                                        string[] date1 = datespilt[0].Split('/');
                                                                                        string arrangedate = date1[1] + '/' + date1[0] + '/' + date1[2];

                                                                                        FpSpread1.Sheets[0].RowCount++;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                                        string values = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = degree_code + "/" + batchyear + "/" + schorder + "/" + semester + "/" + section;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = arrangedate;

                                                                                        string date_1 = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = arrangedate;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Large;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = subjectname;
                                                                                        string col_value = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = tempsubject_no + "/" + subjectname;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = hr.ToString();
                                                                                        string hour_staff = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = hr + "/" + staffcode;
                                                                                        lblerror.Visible = false;

                                                                                        ar = FpSpread1.ActiveSheetView.ActiveRow;

                                                                                        string[] split_value = values.Split('/');
                                                                                        string deg_code = split_value[0];

                                                                                        int select_degreecode = Convert.ToInt32(deg_code);
                                                                                        string[] split_hour_staff = hour_staff.Split('/');
                                                                                        string hour1 = split_hour_staff[0];

                                                                                        int select_hour = Convert.ToInt32(hour1);
                                                                                        string select_staff_code = split_hour_staff[1];

                                                                                        string batch_yr = split_value[1];

                                                                                        string schedule_ord = split_value[2];

                                                                                        string sem = split_value[3];

                                                                                        int sem_1 = Convert.ToInt32(sem);
                                                                                        string sec = split_value[4];

                                                                                        string[] get_subj_no = col_value.Split('/');
                                                                                        string get_subj_value = get_subj_no[0];
                                                                                        int select_subjno = Convert.ToInt32(get_subj_value);

                                                                                        string subj_name = "";

                                                                                        string[] getdate = date_1.Split(new Char[] { '/' });
                                                                                        string getdate_1 = getdate[1] + '/' + getdate[0] + '/' + getdate[2];
                                                                                        set_date = Convert.ToDateTime(getdate_1);
                                                                                        //Session["session_getdate_1"] = getdate_1;

                                                                                        alterdata.Tables[6].DefaultView.RowFilter = "sch_date='" + set_date + "' and hr=" + select_hour + " and staff_code='" + select_staff_code + "' and subject_no=" + select_subjno + "";
                                                                                        dvplan = alterdata.Tables[6].DefaultView;
                                                                                        //string query11 = "select top 1 lpt.topics from lessonplantopics as lpt,lesson_plan as lp where lp.degree_code=" + select_degreecode + " and lp.sch_date='" + set_date + "' and lp.lp_code=lpt.lp_code and lpt.hr=" + select_hour + " and lpt.staff_code='" + select_staff_code + "' and lpt.subject_no=" + select_subjno + " and lp.semester=" + sem_1 + " order by lpt.lp_code desc";
                                                                                        //SqlCommand ret_topics = new SqlCommand(query11,con);
                                                                                        //SqlDataAdapter da = new SqlDataAdapter(ret_topics);
                                                                                        //DataTable dt = new DataTable();
                                                                                        //da.Fill(dt);

                                                                                        //SqlCommand ret_topics2 = new SqlCommand("select top 1 det.topics from dailyentdet as det,dailystaffentry as dse where dse.degree_code=" + select_degreecode + " and dse.sch_date='" + set_date + "' and dse.lp_code=det.lp_code and det.hr=" + select_hour + " and det.staff_code='" + select_staff_code + "' and det.subject_no=" + select_subjno + " and dse.semester=" + sem_1 + " order by det.lp_code desc", con);
                                                                                        //SqlDataAdapter datadpt = new SqlDataAdapter(ret_topics2);
                                                                                        //datadpt.Fill(dt_dailyentdet1);

                                                                                        string dt_topics = "";
                                                                                        if (dvplan.Count > 0)
                                                                                        {

                                                                                            dt_topics = dvplan[0]["topics"].ToString();
                                                                                            string[] split_topics = dt_topics.Split('/');
                                                                                            for (int value_count = 0; split_topics.GetUpperBound(0) >= value_count; value_count++)
                                                                                            {
                                                                                                string topics = split_topics[value_count];
                                                                                                DataTable dt1_topics = new DataTable();
                                                                                                if (Convert.ToString(topics) != "")
                                                                                                {
                                                                                                    int subject_topics = Convert.ToInt32(topics);

                                                                                                    alterdata.Tables[2].DefaultView.RowFilter = "topic_no='" + subject_topics + "'";
                                                                                                    dvalter2 = alterdata.Tables[2].DefaultView;
                                                                                                    //SqlCommand ret_sub_topics1 = new SqlCommand("select unit_name from sub_unit_details where topic_no=" + subject_topics + "", con);
                                                                                                    //SqlDataAdapter da_topics = new SqlDataAdapter(ret_sub_topics1);

                                                                                                    //da_topics.Fill(dt1_topics);
                                                                                                }
                                                                                                if (dvalter2.Count > 0)
                                                                                                {
                                                                                                    subj_name = subj_name + dvalter2[0]["unit_name"].ToString() + "/";

                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = subj_name;
                                                                                                }
                                                                                                FpSpread1.SaveChanges();
                                                                                               // FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            // }
                                                                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                                                        }
                                                                    lb: int test = 0;
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
                                        //string holydayresonquery = "select holiday_date,holiday_desc from holidaystudents where degree_code=" + degree_code + " and semester=" + semester + " AND holiday_date='" + caldate.ToString() + "'";
                                        //DataSet dsholydayres = new DataSet();
                                        //dsholydayres = d2.select_method(holydayresonquery, hat, "Text");
                                        if (holidayview.Count > 0)
                                        {
                                            string holudayreson = holidayview[0]["holiday_desc"].ToString();
                                            FpSpread1.Sheets[0].RowCount++;
                                            //string[] datespilt = Convert.ToString(caldate).Split(' ');
                                            //string[] get_split_date = datespilt[0].Split('/');
                                            //string arrangedate = get_split_date[1] + '/' + get_split_date[0] + '/' + get_split_date[2];
                                            string arrangedate = caldate.ToString("d/M/yyyy");
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = arrangedate + " is " + holudayreson;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Red;
                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                        }
                                    }
                                    //caldate = caldate.AddDays(1);
                                }
                            }

                        }
                        if (sno == 0)
                        {
                            FpSpread1.Visible = false;
                            lblerror.Visible = true;
                            lblerror.Text = "No Records Found";
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnprintmaster.Visible = false;
                            btnxl.Visible = false;
                            Printcontrol.Visible = false;
                        }
                    }
                    else
                    {
                        FpSpread1.Visible = false;
                        lblerror.Visible = true;
                        lblerror.Text = "Please Update Semester Information";
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnprintmaster.Visible = false;
                        btnxl.Visible = false;
                        Printcontrol.Visible = false;
                    }

                    int rowcount = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Height = 300;
                    FpSpread1.Sheets[0].PageSize = 25 + (rowcount * 20);
                    FpSpread1.SaveChanges();

                }
                else
                {
                    FpSpread1.Visible = false;
                    lblerror.Visible = true;
                    lblerror.Text = "No Record Found";
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                    Printcontrol.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    // **************************** End  by jairam 21-11-2014 ******************
    protected void FpSpread1_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        chk = true;

    }
    protected void FpSpread1_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (chk == true)
            {
                TreeView1.Nodes.Clear();
                ar = FpSpread1.ActiveSheetView.ActiveRow;
                subj = FpSpread1.Sheets[0].Cells[ar, 2].Note;
                subj_split = subj.Split('/');
                col_value1 = subj_split[0];

                ht_value = FpSpread1.Sheets[0].Cells[ar, 0].Note.ToString();
                ht_split = ht_value.Split('/');
                ht_degreecode = ht_split[0].ToString();
                if (ht_degreecode != "")// Sangeetha 0n 01 Sep 2014
                {
                    ht_num_degreecode = Convert.ToInt32(ht_degreecode);

                    sem = ht_split[3].ToString();
                    nu_semester = Convert.ToInt32(sem);
                    nu_batchyear = Convert.ToInt32(ht_split[1].ToString());
                    date_6 = FpSpread1.Sheets[0].Cells[ar, 1].Note.ToString();
                    getdate_2 = date_6.Split(new Char[] { '/' });
                    getdate_3 = getdate_2[1] + '/' + getdate_2[0] + '/' + getdate_2[2];
                    ht_datetime = Convert.ToDateTime(getdate_3);


                    cls_hour = FpSpread1.Sheets[0].Cells[ar, 3].Note.ToString();
                    split_value_1 = cls_hour.Split('/');
                    ht_hour = split_value_1[0];
                    ht_num_hr = Convert.ToInt32(ht_hour);

                    ht_staff_code = split_value_1[1].ToString();
                    c = Convert.ToInt32(col_value1);


                    string parent_topic = "select unit_name,topic_no from sub_unit_details where subject_no='" + c + "' and parent_code='0' order by parent_code,topic_no";
                    DataSet ds_2 = new DataSet();
                    ds_2 = d2.select_method(parent_topic, hat, "text");

                    string child_topic = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + c + "' and parent_code<>'0' order by parent_code,topic_no";
                    DataSet ds_3 = new DataSet();
                    ds_3 = d2.select_method(child_topic, hat, "text");

                    string top_parent = subj_split[1];
                    int sub_no1 = Convert.ToInt32(col_value1);
                    CheckBox1.Text = top_parent;

                    string lpt_value = FpSpread1.Sheets[0].Cells[ar, 0].Note;
                    string[] lpt_split = lpt_value.Split('/');

                    string lpt_sections = lpt_split[4].ToString();
                    sections = lpt_sections.ToString();
                    if (sections.Trim().ToString() == "All" || sections.Trim().ToString() == "")
                    {
                        sections = "";
                    }

                    string date = ht_datetime.ToString();

                    //con.Close();
                    //con.Open();

                    //SqlCommand ret_topics;
                    //SqlDataAdapter da;
                    //SqlCommand ret_topics_dailyentdet1;
                    //SqlDataAdapter da_dailyentdet1;

                    if (chklstexcl.Items[0].Selected == true)
                    {
                        //ret_topics = new SqlCommand("select lpt.topics from lessonplantopics as lpt,lesson_plan as lp where lp.degree_code=" + ht_num_degreecode + "  and lp.lp_code=lpt.lp_code   and lpt.subject_no=" + c + " and topics<>'' order by lpt.lp_code desc", con);
                        //da = new SqlDataAdapter(ret_topics);
                        //da.Fill(dt);

                        dt.Reset();
                        dt.Dispose();
                        string sricheck = "select lpt.topics from lessonplantopics as lpt,lesson_plan as lp where lp.degree_code=" + ht_num_degreecode + "  and lp.lp_code=lpt.lp_code   and lpt.subject_no=" + c + " and topics<>'' order by lpt.lp_code desc";
                        dt = d2.select_method_wop_table("select lpt.topics from lessonplantopics as lpt,lesson_plan as lp where lp.degree_code=" + ht_num_degreecode + "  and lp.lp_code=lpt.lp_code   and lpt.subject_no=" + c + " and topics<>'' order by lpt.lp_code desc", "Text");

                        //dt.Reset();
                        //dt.Dispose();
                        //dt = d2.select_method_wop_table("select lpt.topics from lessonplantopics as lpt,lesson_plan as lp where lp.degree_code=" + ht_num_degreecode + " and lp.sch_date='" + ht_datetime + "' and lp.lp_code=lpt.lp_code and lpt.hr=" + ht_hour + " and lpt.staff_code='" + ht_staff_code + "' and lpt.subject_no=" + c + " and topics<>'' order by lpt.lp_code desc", "Text");

                    }
                    else
                    {
                        //ret_topics = new SqlCommand("select lpt.topics from lessonplantopics as lpt,lesson_plan as lp where lp.degree_code=" + ht_num_degreecode + " and lp.sch_date='" + ht_datetime + "' and lp.lp_code=lpt.lp_code and lpt.hr=" + ht_hour + " and lpt.staff_code='" + ht_staff_code + "' and lpt.subject_no=" + c + " and topics<>'' order by lpt.lp_code desc", con);
                        //da = new SqlDataAdapter(ret_topics);
                        //da.Fill(dt);

                        dt.Reset();
                        dt.Dispose();
                        dt = d2.select_method_wop_table("select lpt.topics from lessonplantopics as lpt,lesson_plan as lp where lp.degree_code=" + ht_num_degreecode + " and lp.sch_date='" + ht_datetime + "' and lp.lp_code=lpt.lp_code and lpt.hr=" + ht_hour + " and lpt.staff_code='" + ht_staff_code + "' and lpt.subject_no=" + c + " and topics<>'' order by lpt.lp_code desc", "Text");
                    }


                    dt_det.Clear();
                    dt_det.Reset();
                    dt_det.Dispose();

                    //ret_topics_dailyentdet1 = new SqlCommand("select det.topics from dailyentdet as det,dailystaffentry as dse where dse.degree_code=" + ht_num_degreecode + "  and dse.lp_code=det.lp_code   and det.subject_no=" + c + " and topics<>'' and batch_year=" + nu_batchyear + " order by det.lp_code desc", con);
                    //da_dailyentdet1 = new SqlDataAdapter(ret_topics_dailyentdet1);
                    //da_dailyentdet1.Fill(dt_det);
                    dt_det = d2.select_method_wop_table("select det.topics from dailyentdet as det,dailystaffentry as dse where dse.degree_code=" + ht_num_degreecode + "  and dse.lp_code=det.lp_code   and det.subject_no=" + c + " and topics<>'' and batch_year=" + nu_batchyear + " order by det.lp_code desc", "Text");

                    PopulateTreeview();

                    if (dt.Rows.Count > 0)
                    {
                        for (int dt_row_cnt = 0; dt_row_cnt < dt.Rows.Count; dt_row_cnt++)
                        {
                            string split_lessonplantopics = dt.Rows[dt_row_cnt][0].ToString();
                            string[] splited_lessonplantopics = split_lessonplantopics.Split('/');
                            for (int cnt_lesplantop = 0; splited_lessonplantopics.GetUpperBound(0) >= cnt_lesplantop; cnt_lesplantop++)
                            {
                                for (int cnt_lpt = 0; cnt_lpt < TreeView1.Nodes.Count; cnt_lpt++)
                                {
                                    if (splited_lessonplantopics[cnt_lesplantop] == TreeView1.Nodes[cnt_lpt].Value)
                                    {
                                        TreeView1.Nodes[cnt_lpt].Checked = true;
                                    }
                                    for (int cnt_lpt1 = 0; cnt_lpt1 < TreeView1.Nodes[cnt_lpt].ChildNodes.Count; cnt_lpt1++)
                                    {
                                        if (splited_lessonplantopics[cnt_lesplantop] == TreeView1.Nodes[cnt_lpt].ChildNodes[cnt_lpt1].Value)
                                        {
                                            TreeView1.Nodes[cnt_lpt].ChildNodes[cnt_lpt1].Checked = true;
                                        }

                                        for (int cnt_lpt2 = 0; cnt_lpt2 < TreeView1.Nodes[cnt_lpt].ChildNodes[cnt_lpt1].ChildNodes.Count; cnt_lpt2++)
                                        {
                                            if (splited_lessonplantopics[cnt_lesplantop] == TreeView1.Nodes[cnt_lpt].ChildNodes[cnt_lpt1].ChildNodes[cnt_lpt2].Value)
                                            {
                                                TreeView1.Nodes[cnt_lpt].ChildNodes[cnt_lpt1].ChildNodes[cnt_lpt2].Checked = true;
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
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    private void PopulateTreeview()
    {
        try
        {
            string dt_topics = "";
            string dt_topics1 = "";

            //con.Close();
            //con.Open();

            this.TreeView1.Nodes.Clear();
            HierarchyTrees hierarchyTrees = new HierarchyTrees();
            HierarchyTrees.HTree objHTree = null;

            //start=======common tree load
            string strquery = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + c + "' order by parent_code,topic_no";
            DataSet dsload = d2.select_method_wo_parameter(strquery, "Text");
            this.TreeView1.Nodes.Clear();
            hierarchyTrees.Clear();
            for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
            {
                objHTree = new HierarchyTrees.HTree();
                objHTree.topic_no = int.Parse(dsload.Tables[0].Rows[i]["Topic_no"].ToString());
                objHTree.parent_code = int.Parse(dsload.Tables[0].Rows[i]["parent_code"].ToString());
                objHTree.unit_name = dsload.Tables[0].Rows[i]["unit_name"].ToString();
                hierarchyTrees.Add(objHTree);
            }

            //using (SqlCommand command = new SqlCommand("select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + c + "' order by parent_code,topic_no", con)) // order by added by jairam 2014/11/03
            //{
            //    this.TreeView1.Nodes.Clear();
            //    hierarchyTrees.Clear();
            //    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            //    while (reader.Read())
            //    {
            //        objHTree = new HierarchyTrees.HTree();
            //        objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
            //        objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
            //        objHTree.unit_name = reader["unit_name"].ToString();
            //        hierarchyTrees.Add(objHTree);

            //    }

            //}
            panel3.Visible = true;
            //end==========
            {
                //con.Close();
                //con.Open();
                if (chklstexcl.Items[0].Selected == false && chklstexcl.Items[1].Selected == false)
                {
                    //using (SqlCommand command = new SqlCommand("select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + c + "' order by parent_code,topic_no", con)) // order by added by jairam 2014/11/03
                    //{
                    //    hierarchyTrees.Clear();
                    //    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    //    while (reader.Read())
                    //    {
                    //        objHTree = new HierarchyTrees.HTree();
                    //        objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                    //        objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                    //        objHTree.unit_name = reader["unit_name"].ToString();
                    //        hierarchyTrees.Add(objHTree);
                    //    }
                    //}

                    strquery = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + c + "' order by parent_code,topic_no";
                    DataSet dstopic = d2.select_method_wo_parameter(strquery, "Text");
                    hierarchyTrees.Clear();
                    for (int t = 0; t < dstopic.Tables[0].Rows.Count; t++)
                    {
                        objHTree = new HierarchyTrees.HTree();
                        objHTree.topic_no = int.Parse(dstopic.Tables[0].Rows[t]["Topic_no"].ToString());
                        objHTree.parent_code = int.Parse(dstopic.Tables[0].Rows[t]["parent_code"].ToString());
                        objHTree.unit_name = dstopic.Tables[0].Rows[t]["unit_name"].ToString();
                        hierarchyTrees.Add(objHTree);
                    }
                    panel3.Visible = true;
                }
                else if (chklstexcl.Items[0].Selected == true && chklstexcl.Items[1].Selected == true)
                {

                    if (dt.Rows.Count > 0)
                    {
                        string get_topic_no = "";
                        string get_topic_no1 = "";
                        string get_topic_no2 = "";

                        for (int dt_row_cnt = 0; dt_row_cnt < dt.Rows.Count; dt_row_cnt++)
                        {
                            dt_topics = dt.Rows[dt_row_cnt][0].ToString();
                            string[] split_topics2 = dt_topics.Split('/');
                            for (int i = 0; split_topics2.GetUpperBound(0) >= i; i++)
                            {
                                if (get_topic_no == "")
                                {
                                    get_topic_no = "'" + split_topics2[i] + "'";
                                }
                                else
                                {
                                    get_topic_no = get_topic_no + ',' + "'" + split_topics2[i] + "'";
                                }
                            }
                        }

                        if (dt_det.Rows.Count > 0)
                        {
                            for (int dt_dailyentdet1_row_cnt = 0; dt_dailyentdet1_row_cnt < dt_det.Rows.Count; dt_dailyentdet1_row_cnt++)
                            {
                                dt_topics1 = dt_det.Rows[dt_dailyentdet1_row_cnt][0].ToString();
                                string[] split_topics3 = dt_topics1.Split('/');
                                for (int i = 0; split_topics3.GetUpperBound(0) >= i; i++)
                                {
                                    if (get_topic_no1 == "")
                                    {
                                        get_topic_no1 = "'" + split_topics3[i] + "'";
                                    }
                                    else
                                    {
                                        get_topic_no1 = get_topic_no1 + ',' + "'" + split_topics3[i] + "'";
                                    }
                                }
                            }

                        }
                        if (get_topic_no1 != "")
                        {
                            get_topic_no2 = get_topic_no + "," + get_topic_no1;
                        }
                        else
                        {
                            get_topic_no2 = get_topic_no;
                        }

                        //using (SqlCommand command = new SqlCommand("select topic_no,parent_code,unit_name from sub_unit_details where topic_no not in(" + get_topic_no2 + ") and subject_no='" + c + "' order by parent_code,topic_no", con)) // order by added by jairam 2014/11/03
                        //{
                        //    hierarchyTrees.Clear();
                        //    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        //    while (reader.Read())
                        //    {
                        //        string sqlquery = "select isnull(count(*),0) as ischild from sub_unit_details where parent_code=" + reader["Topic_no"].ToString() + "";
                        //        string ischild = d2.GetFunction(sqlquery);
                        //        string sqlquery1 = "select isnull(count(*),0) as isavailable from sub_unit_details where topic_no not in(" + get_topic_no2 + ") and subject_no='" + c + "' and parent_code=" + reader["Topic_no"].ToString() + "";
                        //        string isavailable = d2.GetFunction(sqlquery1);

                        //        if (Convert.ToInt16(ischild) == 0)
                        //        {
                        //            objHTree = new HierarchyTrees.HTree();
                        //            objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                        //            objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                        //            objHTree.unit_name = reader["unit_name"].ToString();
                        //            hierarchyTrees.Add(objHTree);
                        //        }
                        //        else if (Convert.ToInt16(ischild) > 0 && Convert.ToInt16(isavailable) > 0)
                        //        {
                        //            objHTree = new HierarchyTrees.HTree();
                        //            objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                        //            objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                        //            objHTree.unit_name = reader["unit_name"].ToString();
                        //            hierarchyTrees.Add(objHTree);
                        //        }
                        //    }
                        //}

                        strquery = "select topic_no,parent_code,unit_name from sub_unit_details where topic_no not in(" + get_topic_no2 + ") and subject_no='" + c + "' order by parent_code,topic_no";
                        DataSet dsloadtopic = d2.select_method_wo_parameter(strquery, "Text");
                        if (dsloadtopic.Tables[0].Rows.Count > 0)
                        {
                            hierarchyTrees.Clear();

                            for (int at = 0; at < dsloadtopic.Tables[0].Rows.Count; at++)
                            {
                                string sqlquery = "select isnull(count(*),0) as ischild from sub_unit_details where parent_code=" + dsloadtopic.Tables[0].Rows[at]["Topic_no"].ToString() + "";
                                string ischild = d2.GetFunction(sqlquery);
                                string sqlquery1 = "select isnull(count(*),0) as isavailable from sub_unit_details where topic_no not in(" + get_topic_no2 + ") and subject_no='" + c + "' and parent_code=" + dsloadtopic.Tables[0].Rows[at]["Topic_no"].ToString() + "";
                                string isavailable = d2.GetFunction(sqlquery1);

                                if (Convert.ToInt16(ischild) == 0)
                                {
                                    objHTree = new HierarchyTrees.HTree();
                                    objHTree.topic_no = int.Parse(dsloadtopic.Tables[0].Rows[at]["Topic_no"].ToString());
                                    objHTree.parent_code = int.Parse(dsloadtopic.Tables[0].Rows[at]["parent_code"].ToString());
                                    objHTree.unit_name = dsloadtopic.Tables[0].Rows[at]["unit_name"].ToString();
                                    hierarchyTrees.Add(objHTree);
                                }
                                else if (Convert.ToInt16(ischild) > 0 && Convert.ToInt16(isavailable) > 0)
                                {
                                    objHTree = new HierarchyTrees.HTree();
                                    objHTree.topic_no = int.Parse(dsloadtopic.Tables[0].Rows[at]["Topic_no"].ToString());
                                    objHTree.parent_code = int.Parse(dsloadtopic.Tables[0].Rows[at]["parent_code"].ToString());
                                    objHTree.unit_name = dsloadtopic.Tables[0].Rows[at]["unit_name"].ToString();
                                    hierarchyTrees.Add(objHTree);
                                }

                            }
                        }

                        panel3.Visible = true;
                    }
                }



                else if (chklstexcl.Items[0].Selected == true && chklstexcl.Items[1].Selected == false)
                {
                    if (dt.Rows.Count > 0)
                    {
                        string get_topic_no = "";

                        for (int dt_row_cnt = 0; dt_row_cnt < dt.Rows.Count; dt_row_cnt++)
                        {
                            dt_topics = dt.Rows[dt_row_cnt][0].ToString();
                            string[] split_topics2 = dt_topics.Split('/');
                            for (int i = 0; split_topics2.GetUpperBound(0) >= i; i++)
                            {
                                if (get_topic_no == "")
                                {
                                    get_topic_no = "'" + split_topics2[i] + "'";
                                }
                                else
                                {
                                    get_topic_no = get_topic_no + ',' + "'" + split_topics2[i] + "'";
                                }
                            }
                        }

                        //using (SqlCommand command = new SqlCommand("select topic_no,parent_code,unit_name from sub_unit_details where topic_no not in(" + get_topic_no + ") and subject_no='" + c + "' order by parent_code,topic_no ", con)) // order by added by jairam 2014/11/03
                        //{
                        //    hierarchyTrees.Clear();
                        //    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        //    while (reader.Read())
                        //    {
                        //        string sqlquery = "select isnull(count(*),0) as ischild from sub_unit_details where parent_code=" + reader["Topic_no"].ToString() + "";
                        //        string ischild = d2.GetFunction(sqlquery);
                        //        string sqlquery1 = "select isnull(count(*),0) as isavailable from sub_unit_details where topic_no not in(" + get_topic_no + ") and subject_no='" + c + "' and parent_code=" + reader["Topic_no"].ToString() + "";
                        //        string isavailable = d2.GetFunction(sqlquery1);

                        //        if (Convert.ToInt16(ischild) == 0)
                        //        {
                        //            objHTree = new HierarchyTrees.HTree();
                        //            objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                        //            objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                        //            objHTree.unit_name = reader["unit_name"].ToString();
                        //            hierarchyTrees.Add(objHTree);
                        //        }
                        //        else if (Convert.ToInt16(ischild) > 0 && Convert.ToInt16(isavailable) > 0)
                        //        {
                        //            objHTree = new HierarchyTrees.HTree();
                        //            objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                        //            objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                        //            objHTree.unit_name = reader["unit_name"].ToString();
                        //            hierarchyTrees.Add(objHTree);
                        //        }

                        //    }
                        //}

                        strquery = "select topic_no,parent_code,unit_name from sub_unit_details where topic_no not in(" + get_topic_no + ") and subject_no='" + c + "' order by parent_code,topic_no ";
                        DataSet dsloat = d2.select_method_wo_parameter(strquery, "Text");
                        if (dsloat.Tables[0].Rows.Count > 0)
                        {
                            hierarchyTrees.Clear();
                            for (int lt = 0; lt < dsloat.Tables[0].Rows.Count; lt++)
                            {
                                string sqlquery = "select isnull(count(*),0) as ischild from sub_unit_details where parent_code=" + dsloat.Tables[0].Rows[lt]["Topic_no"].ToString() + "";
                                string ischild = d2.GetFunction(sqlquery);
                                string sqlquery1 = "select isnull(count(*),0) as isavailable from sub_unit_details where topic_no not in(" + get_topic_no + ") and subject_no='" + c + "' and parent_code=" + dsloat.Tables[0].Rows[lt]["Topic_no"].ToString() + "";
                                string isavailable = d2.GetFunction(sqlquery1);

                                if (Convert.ToInt16(ischild) == 0)
                                {
                                    objHTree = new HierarchyTrees.HTree();
                                    objHTree.topic_no = int.Parse(dsloat.Tables[0].Rows[lt]["Topic_no"].ToString());
                                    objHTree.parent_code = int.Parse(dsloat.Tables[0].Rows[lt]["parent_code"].ToString());
                                    objHTree.unit_name = dsloat.Tables[0].Rows[lt]["unit_name"].ToString();
                                    hierarchyTrees.Add(objHTree);
                                }
                                else if (Convert.ToInt16(ischild) > 0 && Convert.ToInt16(isavailable) > 0)
                                {
                                    objHTree = new HierarchyTrees.HTree();
                                    objHTree.topic_no = int.Parse(dsloat.Tables[0].Rows[lt]["Topic_no"].ToString());
                                    objHTree.parent_code = int.Parse(dsloat.Tables[0].Rows[lt]["parent_code"].ToString());
                                    objHTree.unit_name = dsloat.Tables[0].Rows[lt]["unit_name"].ToString();
                                    hierarchyTrees.Add(objHTree);
                                }
                            }
                        }
                        panel3.Visible = true;

                    }
                }

                else if (chklstexcl.Items[0].Selected == false && chklstexcl.Items[1].Selected == true)
                {
                    if (dt_det.Rows.Count > 0)
                    {
                        string get_topic_no1 = "";
                        for (int dt_dailyentdet1_row_cnt = 0; dt_dailyentdet1_row_cnt < dt_det.Rows.Count; dt_dailyentdet1_row_cnt++)
                        {
                            dt_topics1 = dt_det.Rows[dt_dailyentdet1_row_cnt][0].ToString();
                            string[] split_topics3 = dt_topics1.Split('/');
                            for (int i = 0; split_topics3.GetUpperBound(0) >= i; i++)
                            {
                                if (get_topic_no1 == "")
                                {
                                    get_topic_no1 = "'" + split_topics3[i] + "'";
                                }
                                else
                                {
                                    get_topic_no1 = get_topic_no1 + ',' + "'" + split_topics3[i] + "'";
                                }
                            }
                        }
                        //using (SqlCommand command = new SqlCommand("select topic_no,parent_code,unit_name from sub_unit_details where topic_no not in(" + get_topic_no1 + ") and subject_no='" + c + "' order by parent_code,topic_no", con)) // order by added by jairam 2014/11/03
                        //{
                        //    hierarchyTrees.Clear();
                        //    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        //    while (reader.Read())
                        //    {
                        //        string sqlquery = "select isnull(count(*),0) as ischild from sub_unit_details where parent_code=" + reader["Topic_no"].ToString() + "";
                        //        string ischild = d2.GetFunction(sqlquery);
                        //        string sqlquery1 = "select isnull(count(*),0) as isavailable from sub_unit_details where topic_no not in(" + get_topic_no1 + ") and subject_no='" + c + "' and parent_code=" + reader["Topic_no"].ToString() + "";
                        //        string isavailable = d2.GetFunction(sqlquery1);
                        //        if (Convert.ToInt16(ischild) == 0)
                        //        {
                        //            objHTree = new HierarchyTrees.HTree();
                        //            objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                        //            objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                        //            objHTree.unit_name = reader["unit_name"].ToString();
                        //            hierarchyTrees.Add(objHTree);
                        //        }
                        //        else if (Convert.ToInt16(ischild) > 0 && Convert.ToInt16(isavailable) > 0)
                        //        {
                        //            objHTree = new HierarchyTrees.HTree();
                        //            objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                        //            objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                        //            objHTree.unit_name = reader["unit_name"].ToString();
                        //            hierarchyTrees.Add(objHTree);
                        //        }
                        //    }
                        //}


                        strquery = "select topic_no,parent_code,unit_name from sub_unit_details where topic_no not in(" + get_topic_no1 + ") and subject_no='" + c + "' order by parent_code,topic_no";
                        DataSet dsloatg = d2.select_method_wo_parameter(strquery, "Text");
                        if (dsloatg.Tables[0].Rows.Count > 0)
                        {
                            hierarchyTrees.Clear();
                            for (int pc = 0; pc < dsloatg.Tables[0].Rows.Count; pc++)
                            {
                                string sqlquery = "select isnull(count(*),0) as ischild from sub_unit_details where parent_code=" + dsloatg.Tables[0].Rows[pc]["Topic_no"].ToString() + "";
                                string ischild = d2.GetFunction(sqlquery);
                                string sqlquery1 = "select isnull(count(*),0) as isavailable from sub_unit_details where topic_no not in(" + get_topic_no1 + ") and subject_no='" + c + "' and parent_code=" + dsloatg.Tables[0].Rows[pc]["Topic_no"].ToString() + "";
                                string isavailable = d2.GetFunction(sqlquery1);
                                if (Convert.ToInt16(ischild) == 0)
                                {
                                    objHTree = new HierarchyTrees.HTree();
                                    objHTree.topic_no = int.Parse(dsloatg.Tables[0].Rows[pc]["Topic_no"].ToString());
                                    objHTree.parent_code = int.Parse(dsloatg.Tables[0].Rows[pc]["parent_code"].ToString());
                                    objHTree.unit_name = dsloatg.Tables[0].Rows[pc]["unit_name"].ToString();
                                    hierarchyTrees.Add(objHTree);
                                }
                                else if (Convert.ToInt16(ischild) > 0 && Convert.ToInt16(isavailable) > 0)
                                {
                                    objHTree = new HierarchyTrees.HTree();
                                    objHTree.topic_no = int.Parse(dsloatg.Tables[0].Rows[pc]["Topic_no"].ToString());
                                    objHTree.parent_code = int.Parse(dsloatg.Tables[0].Rows[pc]["parent_code"].ToString());
                                    objHTree.unit_name = dsloatg.Tables[0].Rows[pc]["unit_name"].ToString();
                                    hierarchyTrees.Add(objHTree);
                                }
                            }
                        }

                        panel3.Visible = true;
                        //    }
                        //}

                    }
                    else
                    {
                        //========
                        //using (SqlCommand command = new SqlCommand("select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + c + "' order by parent_code,topic_no", con)) // order by added by jairam 2014/11/03
                        //{
                        //    hierarchyTrees.Clear();
                        //    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        //    while (reader.Read())
                        //    {
                        //        objHTree = new HierarchyTrees.HTree();
                        //        objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                        //        objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                        //        objHTree.unit_name = reader["unit_name"].ToString();
                        //        hierarchyTrees.Add(objHTree);
                        //    }
                        //}

                        strquery = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + c + "' order by parent_code,topic_no";
                        DataSet dsloatg1 = d2.select_method_wo_parameter(strquery, "Text");
                        if (dsloatg1.Tables[0].Rows.Count > 0)
                        {
                            hierarchyTrees.Clear();
                            for (int tpc = 0; tpc < dsloatg1.Tables[0].Rows.Count; tpc++)
                            {
                                objHTree = new HierarchyTrees.HTree();
                                objHTree.topic_no = int.Parse(dsloatg1.Tables[0].Rows[tpc]["Topic_no"].ToString());
                                objHTree.parent_code = int.Parse(dsloatg1.Tables[0].Rows[tpc]["parent_code"].ToString());
                                objHTree.unit_name = dsloatg1.Tables[0].Rows[tpc]["unit_name"].ToString();
                                hierarchyTrees.Add(objHTree);
                            }
                        }

                    }
                    //========
                }
            }
            foreach (HierarchyTrees.HTree hTree in hierarchyTrees)
            {
                HierarchyTrees.HTree parentNode = hierarchyTrees.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
                if (parentNode != null)
                {
                    foreach (TreeNode tn in TreeView1.Nodes)
                    {
                        if (tn.Value == parentNode.topic_no.ToString())
                        {
                            tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                            //Session["session_tnValue"] = tn;
                        }
                        if (tn.ChildNodes.Count > 0)
                        {
                            foreach (TreeNode ctn in tn.ChildNodes)
                            {
                                RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);

                            }
                        }

                    }


                }
                else
                {
                    TreeView1.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
                }

                TreeView1.ExpandAll();


            }

            if (TreeView1.Nodes.Count < 1)
            {

                BtnSaveTree.Enabled = false;
            }
            else
            {
                BtnSaveTree.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void RecursiveChild(TreeNode tn, string searchValue, HierarchyTrees.HTree hTree)
    {
        try
        {
            if (tn.Value == searchValue)
            {
                tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
            }
            if (tn.ChildNodes.Count > 0)
            {
                foreach (TreeNode ctn in tn.ChildNodes)
                {
                    RecursiveChild(ctn, searchValue, hTree);
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }


    public class HierarchyTrees : List<HierarchyTrees.HTree>
    {
        public class HTree
        {
            private int m_topic_no;
            private int m_parent_code;
            private string m_unit_name;

            public int topic_no
            {
                get { return m_topic_no; }
                set { m_topic_no = value; }
            }

            public int parent_code
            {
                get { return m_parent_code; }
                set { m_parent_code = value; }
            }
            public string unit_name
            {
                get { return m_unit_name; }
                set { m_unit_name = value; }
            }

        }
    }


    protected void txtfrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
            string fromdate = txtfrom.Text;
            string todate = txtto.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Enter To Date Greater Than From Date";// Sangeetha on 01 Sep 2013
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void txtto_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
            string fromdate = txtfrom.Text;
            string todate = txtto.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Enter To Date Grater Than From Date";
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    //public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    //{
    //    int holiday = 0;
    //    if (no_days == "")
    //        return "";
    //    if (sdate != "")
    //    {
    //        string[] sp_date = sdate.Split(new Char[] { '/' });
    //        string start_date = sp_date[1].ToString() + "-" + sp_date[2].ToString() + "-" + sp_date[0].ToString();
    //        DateTime dt1 = Convert.ToDateTime(sdate);
    //        DateTime dt2 = Convert.ToDateTime(curday);
    //        TimeSpan ts = dt2 - dt1;
    //        string query1 = "select count(*)as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "'";
    //        string holday = d2.GetFunction(query1);
    //        if (holday != "")
    //            holiday = Convert.ToInt32(holday);
    //        int dif_days = ts.Days;
    //        int nodays = Convert.ToInt32(no_days);
    //        int order = (dif_days - holiday) % nodays;
    //        order = order + 1;

    //        //----------------------------------------------------------     

    //        if (stastdayorder.ToString().Trim() != "")
    //        {
    //            if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
    //            {
    //                order = order + (Convert.ToInt16(stastdayorder) - 1);
    //                if (order == (nodays + 1))
    //                    order = 1;
    //                else if (order > nodays)
    //                    order = order % nodays;
    //            }
    //        }
    //        //-----------------------------------------------------------


    //        string findday = "";
    //        if (order == 1)
    //            findday = "mon";
    //        else if (order == 2) findday = "tue";
    //        else if (order == 3) findday = "wed";
    //        else if (order == 4) findday = "thu";
    //        else if (order == 5) findday = "fri";
    //        else if (order == 6) findday = "sat";
    //        else if (order == 7) findday = "sun";

    //        Day_Order = Convert.ToString(order) + "-" + Convert.ToString(findday);
    //        return findday;
    //    }
    //    else
    //        return "";

    //}
    protected void BtnSaveTree_Click(object sender, EventArgs e)
    {
        try
        {
            string temp_lpcode = "";
            string strsec = "";
            ar = FpSpread1.ActiveSheetView.ActiveRow;
            subj = FpSpread1.Sheets[0].Cells[ar, 2].Note;
            subj_split = subj.Split('/');
            col_value1 = subj_split[0];

            ht_value = FpSpread1.Sheets[0].Cells[ar, 0].Note.ToString();
            ht_split = ht_value.Split('/');
            ht_degreecode = ht_split[0].ToString();
            ht_num_degreecode = Convert.ToInt32(ht_degreecode);
            sem = ht_split[3].ToString();
            nu_semester = Convert.ToInt32(sem);
            string batch = ht_split[1].ToString();
            int batch_year = Convert.ToInt32(batch);
            string schorder = ht_split[2].ToString();
            int sch_order = Convert.ToInt32(schorder);
            date_6 = FpSpread1.Sheets[0].Cells[ar, 1].Note.ToString();
            getdate_2 = date_6.Split(new Char[] { '/' });
            getdate_3 = getdate_2[1] + '/' + getdate_2[0] + '/' + getdate_2[2];
            ht_datetime = Convert.ToDateTime(getdate_3);


            cls_hour = FpSpread1.Sheets[0].Cells[ar, 3].Note.ToString();
            split_value_1 = cls_hour.Split('/');
            ht_hour = split_value_1[0];
            ht_num_hr = Convert.ToInt32(ht_hour);

            ht_staff_code = split_value_1[1].ToString();
            c = Convert.ToInt32(col_value1);

            string lpt_sections = ht_split[4].ToString();
            sections = lpt_sections.ToString();
            if (sections.Trim().ToString() == "All" || sections.Trim().ToString() == "")
            {
               sections = "";
               

            }
            else
            {
                strsec = " and sections='" + sections.Trim().ToString() + "'";
            }

            if (IsPostBack)
            {
                for (int a = 0; a < TreeView1.CheckedNodes.Count; a++)
                {
                    if (get_node_code == "")
                    {
                        get_node_code = TreeView1.CheckedNodes[a].Value;
                        get_node_text = TreeView1.CheckedNodes[a].Text;
                    }
                    else
                    {
                        get_node_code = get_node_code + "/" + TreeView1.CheckedNodes[a].Value;
                        get_node_text = get_node_text + "," + TreeView1.CheckedNodes[a].Text;
                        //string unfocus = Session["session_ar"].ToString();
                        //int cell_unfocus = 0;
                        //cell_unfocus = Convert.ToInt32(unfocus);
                        //FpSpread1.Sheets[0].Cells[ar, 2].CanFocus = false;
                    }
                }


                FpSpread1.Sheets[0].Cells[ar, 4].Note = get_node_code;
                FpSpread1.Sheets[0].Cells[ar, 4].Text = get_node_text;
                //panel3.Visible = false;

                string date = ht_datetime.ToString("MM/dd/yyyy");

                //con.Close();
                //con.Open();
                ds_lpcode.Clear();
                ds_lpcode.Dispose();
                ds_lpcode.Reset();
                ds_lpcode = d2.select_method_wo_parameter("select * from lesson_plan where degree_code=" + ht_num_degreecode + " and batch_year=" + batch_year + " and sch_date='" + date + "' and semester =" + nu_semester + "  " + strsec + "", "Text");
                //SqlCommand lesson_plan_topics = new SqlCommand("select * from lesson_plan where degree_code=" + ht_num_degreecode + " and batch_year=" + batch_year + " and sch_date='" + date + "' and semester =" + nu_semester + "  " + strsec + "", con);
                //SqlDataAdapter da_lpcode = new SqlDataAdapter(lesson_plan_topics);
                //da_lpcode.Fill(ds_lpcode);

                if (ds_lpcode.Tables[0].Rows.Count > 0)
                {
                    temp_lpcode = ds_lpcode.Tables[0].Rows[0]["lp_code"].ToString();
                }
                else
                {
                    //added by Mullai
                    if(sections == "")
                    {
                        
                        for (int i1 = 1; i1 < ddlSec.Items.Count; i1++)
                        {
                            sections = ddlSec.Items[i1].Text.ToString();

                            int insert = d2.update_method_wo_parameter("insert into lesson_plan (degree_code,batch_year,sch_order,sch_date,semester,sections)values(" + ht_num_degreecode + ", " + batch_year + ",'" + sch_order + "','" + date + "'," + nu_semester + ",'" + sections + "')", "Text");
                            sections = "";
                        }

                    }
                    else
                    {

                        int insert = d2.update_method_wo_parameter("insert into lesson_plan (degree_code,batch_year,sch_order,sch_date,semester,sections)values(" + ht_num_degreecode + ", " + batch_year + ",'" + sch_order + "','" + date + "'," + nu_semester + ",'" + sections + "')", "Text");
                    }
                    //con.Close();
                    //con.Open();
                    //SqlCommand lesson_plan1 = new SqlCommand("insert into lesson_plan (degree_code,batch_year,sch_order,sch_date,semester,sections)values(" + ht_num_degreecode + ", " + batch_year + ",'" + sch_order + "','" + date + "'," + nu_semester + ",'" + sections + "')", con);
                    //lesson_plan1.ExecuteNonQuery();

                    //con.Close();
                    //con.Open();
                    ds_lpcode.Clear();
                    ds_lpcode.Dispose();
                    ds_lpcode.Reset();

                    ds_lpcode = d2.select_method_wo_parameter("select * from lesson_plan where degree_code=" + ht_num_degreecode + " and batch_year=" + batch_year + " and sch_date='" + date + "' and semester =" + nu_semester + "  " + strsec + "", "Text");
                    //SqlCommand lesson_plan_topics1 = new SqlCommand("select * from lesson_plan where degree_code=" + ht_num_degreecode + " and batch_year=" + batch_year + " and sch_date='" + date + "' and semester =" + nu_semester + "  " + strsec + "", con);
                    //SqlDataAdapter da_lpcode1 = new SqlDataAdapter(lesson_plan_topics1);
                    //da_lpcode1.Fill(ds_lpcode);
                    if (ds_lpcode.Tables[0].Rows.Count > 0)
                    {
                        temp_lpcode = ds_lpcode.Tables[0].Rows[0]["lp_code"].ToString();
                    }
                }
                if (temp_lpcode.Trim().ToString() != "")
                {
                    int temp1_lpcode = 0;
                    temp1_lpcode = Convert.ToInt32(temp_lpcode);
                    //con.Close();
                    //con.Open();
                    //SqlCommand lesson_topicdel = new SqlCommand("delete from lessonplantopics where lp_code=" + temp1_lpcode + " and subject_no=" + c + " and hr=" + ht_num_hr + " ", con);
                    //lesson_topicdel.ExecuteNonQuery();

                    int delete = d2.update_method_wo_parameter("delete from lessonplantopics where lp_code=" + temp1_lpcode + " and subject_no=" + c + " and hr=" + ht_num_hr + " ", "Text");

                    //con.Close();
                    //con.Open();
                    //SqlCommand lessonplantopics_insert = new SqlCommand("insert into lessonplantopics (lp_code,subject_no,staff_code,hr,topics)values(" + temp1_lpcode + ", " + c + ",'" + ht_staff_code + "'," + ht_num_hr + ",'" + get_node_code + "')", con);
                    //lessonplantopics_insert.ExecuteNonQuery();
                    delete = d2.update_method_wo_parameter("insert into lessonplantopics (lp_code,subject_no,staff_code,hr,topics)values(" + temp1_lpcode + ", " + c + ",'" + ht_staff_code + "'," + ht_num_hr + ",'" + get_node_code + "')", "Text");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Successfully Saved')", true);

                }

                panel3.Visible = false;
                panel3.Visible = false;
                FpSpread1.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {

    }
    protected void TreeView1_TreeNodeCheckChanged(object sender, EventArgs e)
    {
        try
        {
            for (int iCnt = 0; iCnt < TreeView1.Nodes.Count; iCnt++)
            {
                if (TreeView1.Nodes[iCnt].Checked == true)
                {
                    if (TreeView1.Nodes[iCnt].ChildNodes.Count > 0)
                    {
                        for (int jCnt = 0; jCnt < TreeView1.Nodes[iCnt].ChildNodes.Count; jCnt++)
                        {
                            TreeView1.Nodes[iCnt].ChildNodes[jCnt].Checked = true;
                            for (int kcnt = 0; kcnt < TreeView1.Nodes[iCnt].ChildNodes[jCnt].ChildNodes.Count; kcnt++)
                            {
                                TreeView1.Nodes[iCnt].ChildNodes[jCnt].ChildNodes[kcnt].Checked = true;
                            }
                        }
                    }

                    else
                    {
                        for (int jCnt = 0; jCnt < TreeView1.Nodes[iCnt].ChildNodes.Count; jCnt++)
                        {
                            TreeView1.Nodes[iCnt].ChildNodes[jCnt].Checked = false;
                            for (int kcnt = 0; kcnt < TreeView1.Nodes[iCnt].ChildNodes[jCnt].ChildNodes.Count; kcnt++)
                            {
                                TreeView1.Nodes[iCnt].ChildNodes[jCnt].ChildNodes[kcnt].Checked = false;
                            }
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void chklstexcl_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chklstexcl.Focus();
            pexclude.Focus();
            int excldcnt = 0;
            txtexclude.Text = "---Select---";
            for (int i = 0; i < chklstexcl.Items.Count; i++)
            {
                if (chklstexcl.Items[i].Selected == true)
                {
                    excldcnt = excldcnt + 1;
                }
            }
            if (excldcnt > 0)
            {
                txtexclude.Text = "Excluded(" + excldcnt.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void BtnExitTree_Click(object sender, EventArgs e)
    {
        panel3.Visible = false;
        FpSpread1.SaveChanges();
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            for (int iCnt = 0; iCnt < TreeView1.Nodes.Count; iCnt++)
            {
                if ((TreeView1.Nodes[iCnt].ChildNodes.Count > 0) && (CheckBox1.Checked == true))
                {
                    TreeView1.Nodes[iCnt].Checked = true;
                    for (int jcnt = 0; jcnt < TreeView1.Nodes[iCnt].ChildNodes.Count; jcnt++)
                    {

                        TreeView1.Nodes[iCnt].ChildNodes[jcnt].Checked = true;
                        if (TreeView1.Nodes[iCnt].ChildNodes[jcnt].ChildNodes.Count > 0)
                        {
                            for (int kcnt = 0; kcnt < TreeView1.Nodes[jcnt].ChildNodes[jcnt].ChildNodes.Count; kcnt++)
                            {
                                TreeView1.Nodes[iCnt].ChildNodes[jcnt].ChildNodes[kcnt].Checked = true;
                            }
                        }
                    }
                }
                else
                {
                    TreeView1.Nodes[iCnt].Checked = false;
                    for (int jcnt = 0; jcnt < TreeView1.Nodes[iCnt].ChildNodes.Count; jcnt++)
                    {
                        if (TreeView1.Nodes[iCnt].ChildNodes[jcnt].ChildNodes.Count > 0)
                        {
                            TreeView1.Nodes[iCnt].ChildNodes[jcnt].Checked = false;

                            for (int kcnt = 0; kcnt < TreeView1.Nodes[iCnt].ChildNodes[jcnt].ChildNodes.Count; kcnt++)
                            {
                                TreeView1.Nodes[iCnt].ChildNodes[jcnt].ChildNodes[kcnt].Checked = false;
                            }

                        }
                        else
                        {
                            TreeView1.Nodes[iCnt].ChildNodes[jcnt].Checked = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string batchyear = ddlBatch.SelectedValue.ToString();
            string degree_code = ddlBranch.SelectedValue.ToString();
            string semester = ddlSemYr.SelectedValue.ToString();
            string subjectcode = chklstsubject.SelectedValue.ToString();
            string section = ddlSec.SelectedValue.ToString();
            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
            {
                sections = "";
                Session["Sign"] = "" + batchyear + "," + degree_code + "," + semester + "";
            }
            else
            {
                Session["Sign"] = "" + batchyear + "," + degree_code + "," + semester + "," + sections + "";
                sections = "- Sec-" + sections;

            }
            string degreedetails = "Lesson Planner Report" + '@' + "Degree :" + batchyear + '-' + ddlDegree.SelectedItem.ToString() + '-' + ddlBranch.SelectedItem.ToString() + '-' + "Sem-" + ddlSemYr.SelectedItem.ToString() + sections + '@' + "Date :" + txtfrom.Text.ToString() + " To " + txtto.Text.ToString();
            string pagename = "Lesson_planner.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Enter Report Name";
            }

        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    public void bindbranch()
    {
        try
        {
            ddlBranch.Items.Clear();
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlcollege.SelectedValue.ToString();// Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddlDegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

            ds = d2.select_method("bind_branch", hat, "sp");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void BindDegree()
    {
        try
        {
            ddlDegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlcollege.SelectedValue.ToString();// Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_degree", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
                ddlBranch.Enabled = true;
                ddlSemYr.Enabled = true;
                ddlSec.Enabled = true;
                GO.Enabled = true;
            }
            else
            {
                ddlBranch.Enabled = false;
                ddlSemYr.Enabled = false;
                ddlSec.Enabled = false;
                GO.Enabled = false;
                lblerror.Visible = true;
                lblerror.Text = "Please Set Degree Rights And Proceed";
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void BindBatch()
    {
        try
        {
            ddlBatch.Items.Clear();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "batch_year";
                ddlBatch.DataValueField = "batch_year";
                ddlBatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlBatch.SelectedValue = max_bat.ToString();
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void bindsem()
    {
        try
        {
            ddlSemYr.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            if (!string.IsNullOrEmpty(Convert.ToString(ddlBranch.SelectedValue)))
            {
            DataSet ds = d2.select_method_wo_parameter("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }

                }
            }
           
            else
            {
                ds.Reset();
                ds.Dispose();
                if (!string.IsNullOrEmpty(Convert.ToString(ddlBranch.Text)))
                {
                    ds = d2.select_method_wo_parameter("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "", "Text");
                    ddlSemYr.Items.Clear();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                        duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                        for (i = 1; i <= duration; i++)
                        {
                            if (first_year == false)
                            {
                                ddlSemYr.Items.Add(i.ToString());
                            }
                            else if (first_year == true && i != 2)
                            {
                                ddlSemYr.Items.Add(i.ToString());
                            }
                        }
                    }
                }
            }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            ddlSec.Items.Clear();
            ddlSec.Enabled = false;
            if (!string.IsNullOrEmpty(Convert.ToString(ddlBranch.SelectedValue)))
            {
                DataSet ds = d2.select_method_wo_parameter("select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + Convert.ToString(ddlBranch.SelectedValue) + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", "Text");
                if (ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlSec.DataSource = ds;
                    ddlSec.DataTextField = "sections";
                    ddlSec.DataBind();
                    ddlSec.Items.Insert(0, "All");
                    ddlSec.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            BindSectionDetail();
            GetSubject();
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindSectionDetail();
            GetSubject();
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetSubject();
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    public void GetSubject()
    {
        try
        {
            string sd =Convert.ToString(Session["Staff_Code"]);
            chklstsubject.Items.Clear();
            chksubject.Checked = false;
            txtsubject.Text = "---Select---";
            hat.Clear();
            hat.Add("Batch_Year",Convert.ToString(ddlBatch.SelectedValue));
            hat.Add("DegCode",Convert.ToString(ddlBranch.SelectedValue));
            hat.Add("Sems",Convert.ToString(ddlSemYr.SelectedItem));
            hat.Add("staffcode",Convert.ToString(Session["Staff_Code"]));
            if (ddlSec.Text != "All" && ddlSec.Text != "" && ddlSec.Text != "-1" && ddlSec.Enabled != false)
            {
                hat.Add("sec",Convert.ToString(ddlSec.SelectedValue));
            }
            else
            {
                hat.Add("sec", "");
            }
            ds.Reset();
            ds.Dispose();
            string vl = "";
            //if (sd == "")
            //{
            //    vl = "select distinct S.subject_no,subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as   SM, subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and    s.syll_code=SM.syll_code and SM.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and SM.Semester='" + ddlSemYr.SelectedItem.ToString() + "' and st.subject_no=s.subject_no     and  SM.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and S.subtype_no = Sem.subtype_no and promote_count=1  order by    subject_code";
            //}
            //else
            //{
            //    if (ddlSec.SelectedValue.ToString() == "")
            //    {
            //        vl = "select distinct S.subject_no,subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as SM, subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and    s.syll_code=SM.syll_code and SM.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and SM.Semester='" + ddlSemYr.SelectedItem.ToString() + "' and st.subject_no=s.subject_no  and  SM.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and S.subtype_no = Sem.subtype_no and promote_count=1 and  st.staff_code='" + Session["Staff_Code"].ToString() + "'  order by subject_code ";
            //    }
            //    else
            //    {
            //        vl = "select distinct S.subject_no,subject_code, subject_name,sem.subject_type from subject as S,syllabus_master  as SM, subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  SM.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and SM.Semester='" + ddlSemYr.SelectedItem.ToString() + "' and st.subject_no=s.subject_no  and SM.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and S.subtype_no = Sem.subtype_no and promote_count=1 and st.staff_code='" + Session["Staff_Code"].ToString() + "' and st.Sections='" + ddlSec.SelectedValue.ToString() + "'  order by subject_code";
            //    }
            //}
            //ds = d2.select_method_wo_parameter(vl, "text");
            ds = d2.select_method("single_subjectwise_attnd", hat, "sp");
            if (ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0)
            {
                chklstsubject.DataSource = ds;
                chklstsubject.DataTextField = "subject_name";
                chklstsubject.DataValueField = "subject_no";
                chklstsubject.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
}


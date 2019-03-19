using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

public partial class ConsoliatedCumulative_AttnReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string college_code = "";
    string college = "";
    string course_id = string.Empty;
    static string Hostelcode = "";
    string selectQuery = string.Empty;

    DataSet ds2 = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable Boys_hostel_present = new Hashtable();
    Hashtable Girls_hostel_present = new Hashtable();
    Hashtable Boys_hostel_absent = new Hashtable();
    Hashtable Girls_hostel_absent = new Hashtable();
    int period = 0;

    string boyshst_present = "";
    string boyshst_absent = "";
    string girlshst_present = "";
    string girlshst_absent = "";

    double boyshst_present1 = 0;
    double boyshst_absent1 = 0;
    double girlshst_present1 = 0;
    double girlshst_absent1 = 0;

    string todaydate = "";
    string date = "";
    string mng_present = "";
    string mng_proj = "";
    string mng_od = "";
    string mng_sus = "";
    string mng_leav = "";
    string mng_absent = "";
    string eng_present = "";
    string eng_proj = "";
    string eng_od = "";
    string eng_sus = "";
    string eng_leav = "";
    string eng_absent = "";
    string temp1 = "", temp2 = "", temp3 = "", temp4 = "", temp5 = "", temp6 = "";
    int first_hrs = 0;
    int sec_hrs = 0;
    int min1halfprs = 0;
    int min2halfprs = 0;
    string Atmnth = "";
    string Atyr = "";
    string Atday = "";
    int noofhrs = 0;
    int MthYear = 0;
    string sections = "";
    string deg_code = "";
    string acronym = "";
    string current_sem = "";
    string roman_val = "";
    string batch_year = "";
    string date_concat = "";
    double temp_val = 0;
    double absent = 0;
    string tot_stn = "";
    double tot_prsnt = 0;
    string year_value = "";

    double firstabsent1 = 0;
    double secondabsent1 = 0;
    DataTable data = new DataTable();
    DataTable datadept = new DataTable();
    DataTable datahtabs = new DataTable();
    DataRow drow;
    DataRow drow1;
    DataRow drow2;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            bindclg();
            loadperiods();
            txt_date.Text = DateTime.Now.ToString("d/MM/yyyy");  //dd/MM/yyyy modified by Deepali on 9.4.18
            txt_date.Attributes.Add("readonly", "readonly");
            Upp1.Visible = false;
            Showgrid.Visible = false;
            div_report.Visible = false;
            lblerr.Visible = false;
        }
    }
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        divMainContents.Visible = false;
        Showgrid.Visible = false;
        lbl_reportname.Visible = false;
        txt_excelname.Visible = false;
        btn_Excel.Visible = false;
        btn_printmaster.Visible = false;
        btnPrint.Visible = false;
    }
    public void cb_hour_checkedchange(object sender, EventArgs e)
    {
        if (cb_hour.Checked == true)
        {
            for (int i = 0; i < cbl_hour.Items.Count; i++)
            {
                cbl_hour.Items[i].Selected = true;
            }
            txt_hour.Text = "Hour(" + cbl_hour.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hour.Items.Count; i++)
            {
                cbl_hour.Items[i].Selected = false;
            }
            txt_hour.Text = "--Select--";
        }
        divMainContents.Visible = false;
        Showgrid.Visible = false;
        lbl_reportname.Visible = false;
        txt_excelname.Visible = false;
        btn_Excel.Visible = false;
        btn_printmaster.Visible = false;
        btnPrint.Visible = false;
    }
    public void cbl_hour_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_hour.Text = "--Select--";
        cb_hour.Checked = false;
        int ccount = 0;
        for (int i = 0; i < cbl_hour.Items.Count; i++)
        {
            if (cbl_hour.Items[i].Selected == true)
            {
                ccount = ccount + 1;
                cb_hour.Checked = false;
            }
        }
        if (ccount > 0)
        {
            txt_hour.Text = "Hour(" + ccount.ToString() + ")";
            if (ccount == cbl_hour.Items.Count)
            {
                cb_hour.Checked = true;
            }

        }
        divMainContents.Visible = false;
        Showgrid.Visible = false;
        lbl_reportname.Visible = false;
        txt_excelname.Visible = false;
        btn_Excel.Visible = false;
        btn_printmaster.Visible = false;
        btnPrint.Visible = false;
    }
    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            if (chk_hour.Checked == true)
            {
                if (txt_hour.Text == "--Select--")
                {
                    lblerr.Visible = true;
                    Showgrid.Visible = false;
                    divMainContents.Visible = false;
                    lbl_reportname.Visible = false;
                    txt_excelname.Visible = false;
                    btn_Excel.Visible = false;
                    btn_printmaster.Visible = false;
                    btnPrint.Visible = false;
                    div_report.Visible = false;

                }
                else
                {
                    go();
                }
            }
            else if (chk_hour.Checked == false)
            {
                go();
            }

        }
        catch
        {
        }
    }
    public void chk_hour_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_hour.Checked == true)
        {
            Upp1.Visible = true;

        }
        else
        {
            Upp1.Visible = false;

        }
        divMainContents.Visible = false;
        Showgrid.Visible = false;
        lbl_reportname.Visible = false;
        txt_excelname.Visible = false;
        btn_Excel.Visible = false;
        btn_printmaster.Visible = false;
        btnPrint.Visible = false;
        div_report.Visible = false;

    }


    public void loadperiods()
    {
        int hour = int.Parse(d2.GetFunction("select MAX(no_of_hrs_per_day) from PeriodAttndSchedule"));

        if (hour > 0)
        {
            for (int i = 1; i <= hour; i++)
            {
                cbl_hour.Items.Add(i.ToString());
            }

        }


    }

    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
            btnPrint.Visible = true;
        }
        catch (Exception ex)
        {

        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {

            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {

                d2.printexcelreportgrid(Showgrid, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }

        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }

    }
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = null;
            string attendance = "Consolidated Cumulative Attendance Report";
            string pagename = "ConsoliatedCumulative_AttnReport.aspx";
            Printcontrol.loadspreaddetails(Showgrid, pagename, attendance, 0, ss);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    public void go()
    {
        try
        {
            btnPrint11();
            lblerr.Visible = false;

            int i;
            string year = "";
            string batch = "";
            string degree = "";
            string sem = "";
            string fnlquery = "";
            string daytotboys = "";
            string daytotgirls = "";
            string hstltotboys = "";
            string hstltotgirls = "";


            date = txt_date.Text.ToString();
            string[] split_date = date.Split(new char[] { '/' });
            Atday = split_date[0].ToString();
            Atmnth = split_date[1].ToString();
            Atyr = split_date[2].ToString();
            todaydate = Atmnth + "/" + Atday + "/" + Atyr;
            DateTime input_date = Convert.ToDateTime(todaydate.ToString());
            date_concat = "'" + date + "'";
            MthYear = (Convert.ToInt32(Atyr) * 12) + Convert.ToInt32(Atmnth);//((year*12)+month)

            string clgcode = Convert.ToString(ddl_college.SelectedItem.Value);
            string query = "SELECT Dept_Name,R.Current_Semester,R.Stud_Type,CASE WHEN ISNULL(Sex,0) = 0 THEN 'Boys' ELSE 'Girls' END Sex ,COUNT(*) Tot,G.degree_Code FROM Registration R ,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code and G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND CC = 0 AND DelFlag = 0  AND Exam_Flag = 'OK' and G.college_code ='" + clgcode + "'  GROUP BY Dept_Name,R.Current_Semester,R.Stud_Type,Sex ,G.degree_Code  ORDER BY Dept_Name,R.Current_Semester,R.Stud_Type,Sex ";
            fnlquery = query + " SELECT distinct G.degree_Code,R.Current_Semester,R.Batch_Year FROM Registration R ,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code  AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code and G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and G.college_code ='" + clgcode + "'ORDER BY  G.degree_Code ";
            fnlquery = fnlquery + " select distinct Stud_Type  from Registration where Stud_Type<>'' and college_code ='" + clgcode + "'";
            fnlquery = fnlquery + " SELECT distinct G.degree_Code FROM Registration R ,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code  AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code and G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and G.college_code ='" + clgcode + "'ORDER BY  G.degree_Code";
            fnlquery = fnlquery + " SELECT No_of_hrs_per_day , no_of_hrs_II_half_day, no_of_hrs_I_half_day, min_pres_II_half_day, min_pres_I_half_day, degree_code, semester  from PeriodAttndSchedule";
            ds = d2.select_method_wo_parameter(fnlquery, "Text");

            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                divMainContents.Visible = true;
                Showgrid.Visible = true;
                lbl_reportname.Visible = true;
                txt_excelname.Visible = true;
                btn_Excel.Visible = true;
                btn_printmaster.Visible = true;
                btnPrint.Visible = true;
                div_report.Visible = true;

                data.Columns.Add("S.No", typeof(string));
                data.Columns.Add("Department", typeof(string));
                data.Columns.Add("Year", typeof(string));
                data.Columns.Add("Total", typeof(string));
                data.Columns.Add("Boys", typeof(string));
                data.Columns.Add("Girls", typeof(string));
                data.Columns.Add("Present", typeof(string));
                data.Columns.Add("Absent", typeof(string));
                data.Columns.Add("Boys1", typeof(string));
                data.Columns.Add("Girls1", typeof(string));
                data.Columns.Add("Boys2", typeof(string));
                data.Columns.Add("Girls2", typeof(string));
                ArrayList arrColHdrNames1 = new ArrayList();
                ArrayList arrColHdrNames2 = new ArrayList();
                ArrayList arrColHdrNames3 = new ArrayList();

                //1st student details
                arrColHdrNames1.Add("S.No");
                arrColHdrNames2.Add("S.No");
                arrColHdrNames1.Add("Department");
                arrColHdrNames2.Add("Department");
                arrColHdrNames1.Add("Year");
                arrColHdrNames2.Add("Year");
                arrColHdrNames1.Add("Total");
                arrColHdrNames2.Add("Total");
                arrColHdrNames1.Add("Total");
                arrColHdrNames2.Add("Boys");
                arrColHdrNames1.Add("Total");
                arrColHdrNames2.Add("Girls");
                arrColHdrNames1.Add("Present");
                arrColHdrNames2.Add("Present");
                arrColHdrNames1.Add("Absent");
                arrColHdrNames2.Add("Absent");
                arrColHdrNames1.Add("Days Scholar");
                arrColHdrNames2.Add("Boys");
                arrColHdrNames1.Add("Days Scholar");
                arrColHdrNames2.Add("Girls");
                arrColHdrNames1.Add("Hostler");
                arrColHdrNames2.Add("Boys");
                arrColHdrNames1.Add("Hostler");
                arrColHdrNames2.Add("Girls");
                //End

                DataRow drHdr1 = data.NewRow();
                DataRow drHdr2 = data.NewRow();


                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                {
                    drHdr1[grCol] = arrColHdrNames1[grCol];
                    drHdr2[grCol] = arrColHdrNames2[grCol];

                }

                data.Rows.Add(drHdr1);
                data.Rows.Add(drHdr2);

                DataView dv = new DataView();
                Hashtable hat_deptTotal = new Hashtable();
                Hashtable hostelerboys = new Hashtable();
                Hashtable hostelergirls = new Hashtable();
                Dictionary<int, string> dicstdet = new Dictionary<int, string>();
                Dictionary<int, string> dicrowcolspan = new Dictionary<int, string>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    double deptTotal = 0;

                    for (i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {

                        degree = Convert.ToString(ds.Tables[1].Rows[i]["degree_Code"]);
                        sem = Convert.ToString(ds.Tables[1].Rows[i]["Current_Semester"]);
                        batch = Convert.ToString(ds.Tables[1].Rows[i]["Batch_Year"]);
                        string stdet = "";
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            drow = data.NewRow();
                            drow["S.No"] = Convert.ToString(i + 1);

                            if (sem == "1" || sem == "2")// Deepali on 9.4.18
                            {
                                year = "1st year";
                                drow["Year"] = year;


                            }
                            //else if (sem == "3")
                            else if (sem == "3" || sem == "4")// Deepali on 9.4.18
                            {
                                year = "2nd year";
                                drow["Year"] = year;


                            }
                            //else if (sem == "5")
                            else if (sem == "5" || sem == "6")// Deepali on 9.4.18
                            {
                                year = "3rd year";
                                drow["Year"] = year;


                            }
                            else
                            {
                                year = "4th year";
                                drow["Year"] = year;

                            }
                            int boyscount = 0;
                            int girlscount = 0;
                            string degree_Code = "";
                            ds.Tables[0].DefaultView.RowFilter = "degree_Code='" + degree + "' and Current_Semester='" + sem + "'";
                            dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                drow["Department"] = Convert.ToString(dv[0]["Dept_Name"]);
                                degree_Code = dv[0]["degree_Code"].ToString();
                            }

                            for (int s = 0; s < ds.Tables[2].Rows.Count; s++)
                            {
                                string type = Convert.ToString(ds.Tables[2].Rows[s]["Stud_Type"]);
                                ds.Tables[0].DefaultView.RowFilter = "degree_Code='" + degree + "' and Current_Semester='" + sem + "' and Stud_Type='" + type + "'";
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    for (int j = 0; j < dv.Count; j++)
                                    {
                                        string gender = Convert.ToString(dv[j]["Sex"]);
                                        if (type == "Day Scholar")
                                        {
                                            if (Convert.ToString(gender).Trim() == "Boys")
                                            {
                                                daytotboys = Convert.ToString(dv[j]["Tot"]);
                                                if (daytotboys.Trim() != "")
                                                {
                                                    boyscount = boyscount + Convert.ToInt32(daytotboys);
                                                }
                                                drow["Boys1"] = Convert.ToString(dv[j]["Tot"]);

                                            }
                                            else
                                            {
                                                daytotgirls = Convert.ToString(dv[j]["Tot"]);
                                                if (daytotgirls.Trim() != "")
                                                {
                                                    girlscount = girlscount + Convert.ToInt32(daytotgirls);
                                                }
                                                drow["Girls1"] = Convert.ToString(dv[j]["Tot"]);

                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToString(gender).Trim() == "Boys")
                                            {
                                                hstltotboys = Convert.ToString(dv[j]["Tot"]);
                                                if (hstltotboys.Trim() != "")
                                                {
                                                    boyscount = boyscount + Convert.ToInt32(hstltotboys);
                                                    if (!hostelerboys.Contains(Convert.ToString(year)))
                                                    {
                                                        hostelerboys.Add(Convert.ToString(year), Convert.ToInt32(hstltotboys));
                                                    }
                                                    else
                                                    {
                                                        string getvlaue = Convert.ToString(hostelerboys[Convert.ToString(year)]);
                                                        if (getvlaue.Trim() != "")
                                                        {
                                                            int totalcount = Convert.ToInt32(hstltotboys) + Convert.ToInt16(getvlaue);
                                                            hostelerboys.Remove(Convert.ToString(Convert.ToString(year)));

                                                            hostelerboys.Add(Convert.ToString(year), Convert.ToInt32(totalcount));
                                                        }

                                                    }
                                                }
                                                drow["Boys2"] = Convert.ToString(dv[j]["Tot"]);


                                            }
                                            else
                                            {
                                                hstltotgirls = Convert.ToString(dv[j]["Tot"]);
                                                if (hstltotgirls.Trim() != "")
                                                {
                                                    girlscount = girlscount + Convert.ToInt32(hstltotgirls);

                                                    if (!hostelergirls.Contains(Convert.ToString(year)))
                                                    {
                                                        hostelergirls.Add(Convert.ToString(year), Convert.ToInt32(hstltotgirls));
                                                    }
                                                    else
                                                    {
                                                        string getvlaue = Convert.ToString(hostelergirls[Convert.ToString(year)]);
                                                        if (getvlaue.Trim() != "")
                                                        {
                                                            int totalcount = Convert.ToInt32(hstltotgirls) + Convert.ToInt16(getvlaue);
                                                            hostelergirls.Remove(Convert.ToString(Convert.ToString(year)));
                                                            hostelergirls.Add(Convert.ToString(year), Convert.ToInt32(totalcount));
                                                        }

                                                    }

                                                    // hostelergirls.Add(Convert.ToString(year), Convert.ToInt32(hstltotgirls));
                                                }
                                                drow["Girls2"] = Convert.ToString(dv[j]["Tot"]);
                                            }
                                        }
                                    }

                                }
                            }
                            drow["Boys"] = Convert.ToString(boyscount);
                            drow["Girls"] = Convert.ToString(girlscount);

                            drow["Total"] = Convert.ToString(Convert.ToInt32(boyscount + girlscount));

                            data.Rows.Add(drow);
                            stdet = batch + "," + year + "," + sem + "," + degree_Code + "," + Convert.ToString(Convert.ToInt32(boyscount + girlscount));
                            dicstdet.Add(data.Rows.Count - 1, stdet);



                            deptTotal = 0;
                            deptTotal += boyscount + girlscount;

                            if (!hat_deptTotal.Contains(Convert.ToString(degree)))
                            {
                                hat_deptTotal.Add(Convert.ToString(degree), deptTotal);
                            }
                            else
                            {
                                string getvlaue = Convert.ToString(hat_deptTotal[Convert.ToString(degree)]);
                                if (getvlaue.Trim() != "")
                                {
                                    deptTotal = deptTotal + Convert.ToDouble(getvlaue);
                                    hat_deptTotal.Remove(Convert.ToString(Convert.ToString(degree)));
                                    hat_deptTotal.Add(Convert.ToString(degree), deptTotal);
                                }
                            }
                        }

                    }

                    DataView dv4 = new DataView();
                    Hashtable hat_present = new Hashtable();
                    Hashtable hat_Absent = new Hashtable();
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        double TotalPresent = 0;
                        double TotalAbsent = 0;

                        if (dicstdet.Count > 0)
                        {

                            foreach (KeyValuePair<int, string> dr in dicstdet)
                            {
                                int k = dr.Key;
                                string studet = dr.Value;
                                string[] spilt = studet.Split(',');

                                degree = Convert.ToString(spilt[3]);
                                batch = Convert.ToString(spilt[0]);
                                sem = Convert.ToString(spilt[2]);
                                tot_stn = Convert.ToString(spilt[4]);
                                year_value = Convert.ToString(spilt[1]);
                                deg_code = degree;
                                batch_year = batch;
                                current_sem = sem;
                                date_concat = Convert.ToString(txt_date.Text);
                                date_concat = "'" + date_concat + "'";

                                ds.Tables[4].DefaultView.RowFilter = "degree_Code='" + degree + "' and semester='" + sem + "'";
                                year = sem;
                                dv4 = ds.Tables[4].DefaultView;
                                if (dv4.Count > 0)
                                {

                                    noofhrs = int.Parse(dv4[0]["No_of_hrs_per_day"].ToString());
                                    first_hrs = int.Parse(dv4[0]["no_of_hrs_I_half_day"].ToString());
                                    sec_hrs = int.Parse(dv4[0]["no_of_hrs_II_half_day"].ToString());
                                    min1halfprs = int.Parse(dv4[0]["min_pres_I_half_day"].ToString());
                                    min2halfprs = int.Parse(dv4[0]["min_pres_II_half_day"].ToString());


                                    findhours();

                                    data.Rows[k]["Present"] = Convert.ToString(secondabsent1);
                                    data.Rows[k]["Absent"] = Convert.ToString(absent);
                                    TotalPresent = 0;
                                    TotalPresent = secondabsent1;

                                    TotalAbsent = 0;
                                    TotalAbsent = absent;


                                    if (!hat_present.Contains(Convert.ToString(degree)))
                                    {
                                        hat_present.Add(Convert.ToString(degree), TotalPresent);
                                    }
                                    else
                                    {
                                        string getvlaue = Convert.ToString(hat_present[Convert.ToString(degree)]);
                                        if (getvlaue.Trim() != "")
                                        {
                                            TotalPresent = TotalPresent + Convert.ToDouble(getvlaue);
                                            hat_present.Remove(Convert.ToString(Convert.ToString(degree)));
                                            hat_present.Add(Convert.ToString(degree), TotalPresent);
                                        }

                                    }

                                    if (!hat_Absent.Contains(Convert.ToString(degree)))
                                    {
                                        hat_Absent.Add(Convert.ToString(degree), TotalAbsent);
                                    }
                                    else
                                    {
                                        string getvlaue1 = Convert.ToString(hat_Absent[Convert.ToString(degree)]);
                                        if (getvlaue1.Trim() != "")
                                        {
                                            TotalAbsent = TotalAbsent + Convert.ToDouble(getvlaue1);
                                            hat_Absent.Remove(Convert.ToString(Convert.ToString(degree)));
                                            hat_Absent.Add(Convert.ToString(degree), TotalAbsent);
                                        }

                                    }

                                }

                            }
                        }
                    }
                    DataView dv1 = new DataView();
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        drow = data.NewRow();
                        data.Rows.Add(drow);
                        data.Rows[data.Rows.Count - 1][0] = "DEPARTMENT WISE ABSENT";
                        data.Rows[data.Rows.Count - 1][4] = "TOTAL";
                        data.Rows[data.Rows.Count - 1][6] = "PRESENT";
                        data.Rows[data.Rows.Count - 1][8] = "ABSENT";
                        data.Rows[data.Rows.Count - 1][10] = "PERSENTAGE";
                        dicrowcolspan.Add(data.Rows.Count - 1, "1");
                        for (i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            degree = Convert.ToString(ds.Tables[3].Rows[i]["degree_Code"]);
                            double newtotal = Convert.ToDouble(hat_deptTotal[Convert.ToString(degree)]);
                            double present_tot = Convert.ToDouble(hat_present[Convert.ToString(degree)]);
                            double absent_tot = Convert.ToDouble(hat_Absent[Convert.ToString(degree)]);
                            double persentage = (present_tot / newtotal) * 100;

                            ds.Tables[0].DefaultView.RowFilter = "degree_Code='" + degree + "'";
                            dv1 = ds.Tables[0].DefaultView;
                            if (dv1.Count > 0)
                            {
                                drow = data.NewRow();
                                data.Rows.Add(drow);

                                data.Rows[data.Rows.Count - 1][0] = Convert.ToString(dv1[0]["Dept_Name"]);
                                data.Rows[data.Rows.Count - 1][4] = Convert.ToString(newtotal);
                                data.Rows[data.Rows.Count - 1][6] = Convert.ToString(present_tot);
                                data.Rows[data.Rows.Count - 1][8] = Convert.ToString(absent_tot);
                                data.Rows[data.Rows.Count - 1][10] = Convert.ToString(Math.Round(persentage, 2));
                                dicrowcolspan.Add(data.Rows.Count - 1, "2");
                            }

                        }
                    }

                    drow = data.NewRow();
                    data.Rows.Add(drow);
                    data.Rows[data.Rows.Count - 1][0] = "HOSTEL ABSENT REPORT";
                    dicrowcolspan.Add(data.Rows.Count - 1, "3");
                    drow = data.NewRow();
                    data.Rows.Add(drow);
                    data.Rows[data.Rows.Count - 1][0] = "BOYS HOSTEL";
                    data.Rows[data.Rows.Count - 1][6] = "GIRLS HOSTEL";
                    dicrowcolspan.Add(data.Rows.Count - 1, "4");
                    drow = data.NewRow();
                    data.Rows.Add(drow);
                    data.Rows[data.Rows.Count - 1][0] = "Year";
                    data.Rows[data.Rows.Count - 1][3] = "Boys Total";
                    data.Rows[data.Rows.Count - 1][4] = "Present";
                    data.Rows[data.Rows.Count - 1][5] = "Absent";
                    data.Rows[data.Rows.Count - 1][6] = "Year";
                    data.Rows[data.Rows.Count - 1][9] = "Boys Total";
                    data.Rows[data.Rows.Count - 1][10] = "Present";
                    data.Rows[data.Rows.Count - 1][11] = "Absent";
                    dicrowcolspan.Add(data.Rows.Count - 1, "5");


                    foreach (DictionaryEntry pr in hostelerboys)
                    {
                        string key = Convert.ToString(pr.Key);
                        string value = Convert.ToString(pr.Value);
                        string getvlaue = Convert.ToString(hostelergirls[Convert.ToString(key)]);

                        string getprecentvalue = Convert.ToString(Girls_hostel_present[Convert.ToString(key)]);
                        string[] splitnewprecent = getprecentvalue.Split('-');

                        string getprecentvalueboys = Convert.ToString(Boys_hostel_present[Convert.ToString(key)]);
                        string[] splitnewprecentboys = getprecentvalueboys.Split('-');


                        drow = data.NewRow();
                        data.Rows.Add(drow);

                        data.Rows[data.Rows.Count - 1][0] = Convert.ToString(key);
                        data.Rows[data.Rows.Count - 1][3] = Convert.ToString(value);
                        data.Rows[data.Rows.Count - 1][4] = Convert.ToString(splitnewprecentboys[0]);
                        data.Rows[data.Rows.Count - 1][5] = Convert.ToString(splitnewprecentboys[1]);
                        data.Rows[data.Rows.Count - 1][6] = Convert.ToString(key);
                        data.Rows[data.Rows.Count - 1][9] = Convert.ToString(getvlaue);
                        data.Rows[data.Rows.Count - 1][10] = Convert.ToString(splitnewprecent[0]);
                        data.Rows[data.Rows.Count - 1][11] = Convert.ToString(splitnewprecent[1]);

                        dicrowcolspan.Add(data.Rows.Count - 1, "6");
                    }

                }
                //1 Grid

                if (data.Columns.Count > 0 && data.Rows.Count > 2)
                {
                    Showgrid.DataSource = data;
                    Showgrid.DataBind();
                    divMainContents.Visible = true;
                    Showgrid.Visible = true;


                    int rowcnt = Showgrid.Rows.Count - 2;
                    //Rowspan
                    for (int rowIndex = Showgrid.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow row = Showgrid.Rows[rowIndex];
                        GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                        Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid.Rows[rowIndex].Font.Bold = true;
                        Showgrid.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;

                        for (int j = 0; j < row.Cells.Count; j++)
                        {
                            if (row.Cells[j].Text == previousRow.Cells[j].Text)
                            {
                                row.Cells[j].RowSpan = previousRow.Cells[j].RowSpan < 2 ? 2 :
                                                       previousRow.Cells[j].RowSpan + 1;
                                previousRow.Cells[j].Visible = false;
                            }

                        }

                    }

                    //Rowspan
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

                    //ColSpan
                    foreach (KeyValuePair<int, string> pr in dicrowcolspan)
                    {
                        int key = pr.Key;
                        string value = Convert.ToString(pr.Value);
                        if (value == "1" || value == "3" || value == "4" || value == "5")
                        {
                            Showgrid.Rows[key].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            Showgrid.Rows[key].Font.Bold = true;
                            Showgrid.Rows[key].BorderColor = Color.Black;
                        }
                        if (value == "2" || value == "1")
                        {
                            Showgrid.Rows[key].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[key].Cells[0].ColumnSpan = 4;

                            Showgrid.Rows[key].Cells[1].Visible = false;
                            Showgrid.Rows[key].Cells[2].Visible = false;
                            Showgrid.Rows[key].Cells[3].Visible = false;



                            Showgrid.Rows[key].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[key].Cells[4].ColumnSpan = 2;
                            Showgrid.Rows[key].Cells[5].Visible = false;

                            Showgrid.Rows[key].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[key].Cells[6].ColumnSpan = 2;
                            Showgrid.Rows[key].Cells[7].Visible = false;

                            Showgrid.Rows[key].Cells[8].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[key].Cells[8].ColumnSpan = 2;
                            Showgrid.Rows[key].Cells[9].Visible = false;


                            Showgrid.Rows[key].Cells[10].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[key].Cells[10].ColumnSpan = 2;
                            Showgrid.Rows[key].Cells[11].Visible = false;
                        }
                        if (value == "3")
                        {
                            Showgrid.Rows[key].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[key].Cells[0].ColumnSpan = 12;

                            for (int g = 1; g < data.Columns.Count; g++)
                                Showgrid.Rows[key].Cells[g].Visible = false;

                        }
                        if (value == "4")
                        {
                            Showgrid.Rows[key].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[key].Cells[0].ColumnSpan = 6;

                            for (int g = 1; g < 6; g++)
                                Showgrid.Rows[key].Cells[g].Visible = false;

                            Showgrid.Rows[key].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[key].Cells[6].ColumnSpan = 6;

                            for (int g = 7; g < 12; g++)
                                Showgrid.Rows[key].Cells[g].Visible = false;

                        }
                        if (value == "5" || value == "6")
                        {
                            Showgrid.Rows[key].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[key].Cells[0].ColumnSpan = 3;

                            Showgrid.Rows[key].Cells[1].Visible = false;
                            Showgrid.Rows[key].Cells[2].Visible = false;


                            Showgrid.Rows[key].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[key].Cells[6].ColumnSpan = 3;

                            Showgrid.Rows[key].Cells[7].Visible = false;
                            Showgrid.Rows[key].Cells[8].Visible = false;

                        }
                    }


                }


            }
            else
            {
                divMainContents.Visible = false;
                Showgrid.Visible = false;
                lbl_reportname.Visible = false;
                txt_excelname.Visible = false;
                btn_Excel.Visible = false;
                btn_printmaster.Visible = false;
                btnPrint.Visible = false;

            }
        }
        catch { }
    }


    public void findhours()
    {
        eng_present = "";
        eng_leav = "";
        eng_absent = "";
        eng_sus = "";
        eng_od = "";
        eng_proj = "";

        mng_present = "";
        mng_leav = "";
        mng_absent = "";
        mng_sus = "";
        mng_od = "";
        mng_proj = "";



        temp1 = "";
        temp2 = "";
        temp3 = "";
        temp4 = "";
        temp5 = "";
        temp6 = "";




        if (chk_hour.Checked == false)
        {

            for (int mng_hr = 1; mng_hr <= noofhrs; mng_hr++)
            {
                temp1 = "d" + Atday + "d" + mng_hr + "=1";
                temp2 = "d" + Atday + "d" + mng_hr + "=10";
                temp3 = "d" + Atday + "d" + mng_hr + "=2";
                temp4 = "d" + Atday + "d" + mng_hr + "=9";
                temp5 = "d" + Atday + "d" + mng_hr + "=3";
                temp6 = "d" + Atday + "d" + mng_hr + "=5";
                if (mng_present == "")
                {
                    mng_present = temp1;
                    mng_leav = temp2;
                    mng_absent = temp3;
                    mng_sus = temp4;
                    mng_od = temp5;
                    mng_proj = temp6;
                }
                else
                {
                    mng_present = mng_present + " and " + temp1;
                    mng_leav = mng_leav + " or " + temp2;
                    mng_absent = mng_absent + " or " + temp3;
                    mng_sus = mng_sus + " or " + temp4;
                    mng_od = mng_od + " or " + temp5;
                    mng_proj = mng_proj + " or " + temp6;
                }
            }
            if (mng_present != "")
            {
                mng_present = " ( " + mng_present + " ) ";
            }
            else
            {
                mng_present = "";
            }
            if (mng_leav != "")
            {
                mng_leav = " ( " + mng_leav + " ) ";
            }
            else
            {
                mng_leav = "";
            }
            if (mng_absent != "")
            {
                mng_absent = " ( " + mng_absent + " ) ";
            }
            else
            {
                mng_absent = "";
            }
            if (mng_sus != "")
            {
                mng_sus = " ( " + mng_sus + " ) ";
            }
            else
            {
                mng_sus = "";
            }
            if (mng_od != "")
            {
                mng_od = " ( " + mng_od + " ) ";
            }
            else
            {
                mng_od = "";
            }
            if (mng_proj != "")
            {
                mng_proj = " ( " + mng_proj + " ) ";
            }
            else
            {
                mng_proj = "";
            }
            temp1 = "";
            temp2 = "";
            temp3 = "";
            temp4 = "";
            temp5 = "";
            temp6 = "";

            if (eng_present != "")
            {
                eng_present = " ( " + eng_present + " ) ";
            }
            else
            {
                eng_present = "";
            }
            if (eng_leav != "")
            {
                eng_leav = " ( " + eng_leav + " ) ";
            }
            else
            {
                eng_leav = "";
            }

            if (eng_absent != "")
            {
                eng_absent = " ( " + eng_absent + " ) ";
            }
            else
            {
                eng_absent = "";
            }
            if (eng_sus != "")
            {
                eng_sus = " ( " + eng_sus + " ) ";
            }
            else
            {
                eng_sus = "";
            }
            if (eng_od != "")
            {
                eng_od = " ( " + eng_od + " ) ";
            }
            else
            {
                eng_od = "";
            }
            if (eng_proj != "")
            {
                eng_proj = " ( " + eng_proj + " ) ";
            }
            else
            {
                eng_proj = "";
            }

        }
        else if (chk_hour.Checked == true)
        {
            for (int i = 0; i < cbl_hour.Items.Count; i++)
            {
                if (cbl_hour.Items[i].Selected == true)
                {
                    period = int.Parse(cbl_hour.Items[i].Value.ToString());
                    temp1 = "";
                    temp2 = "";
                    temp3 = "";
                    temp4 = "";
                    temp5 = "";
                    temp6 = "";

                    temp1 = "d" + Atday + "d" + period + "=1";
                    temp2 = "d" + Atday + "d" + period + "=10";
                    temp3 = "d" + Atday + "d" + period + "=2";
                    temp4 = "d" + Atday + "d" + period + "=9";
                    temp5 = "d" + Atday + "d" + period + "=3";
                    temp6 = "d" + Atday + "d" + period + "=5";
                    if (mng_present == "")
                    {
                        mng_present = temp1;
                        mng_leav = temp2;
                        mng_absent = temp3;
                        mng_sus = temp4;
                        mng_od = temp5;
                        mng_proj = temp6;
                    }
                    else
                    {
                        mng_present = mng_present + " and " + temp1;
                        mng_leav = mng_leav + " or " + temp2;
                        mng_absent = mng_absent + " or " + temp3;
                        mng_sus = mng_sus + " or " + temp4;
                        mng_od = mng_od + " or " + temp5;
                        mng_proj = mng_proj + " or " + temp6;
                    }
                }
            }
            if (mng_present != "")
            {
                mng_present = " ( " + mng_present + " ) ";
            }
            else
            {
                mng_present = "";
            }
            if (mng_leav != "")
            {
                mng_leav = " ( " + mng_leav + " ) ";
            }
            else
            {
                mng_leav = "";
            }
            if (mng_absent != "")
            {
                mng_absent = " ( " + mng_absent + " ) ";
            }
            else
            {
                mng_absent = "";
            }
            if (mng_sus != "")
            {
                mng_sus = " ( " + mng_sus + " ) ";
            }
            else
            {
                mng_sus = "";
            }
            if (mng_od != "")
            {
                mng_od = " ( " + mng_od + " ) ";
            }
            else
            {
                mng_od = "";
            }
            if (mng_proj != "")
            {
                mng_proj = " ( " + mng_proj + " ) ";
            }
            else
            {
                mng_proj = "";
            }

            if (eng_present != "")
            {
                eng_present = " ( " + eng_present + " ) ";
            }
            else
            {
                eng_present = "";
            }
            if (eng_leav != "")
            {
                eng_leav = " ( " + eng_leav + " ) ";
            }
            else
            {
                eng_leav = "";
            }

            if (eng_absent != "")
            {
                eng_absent = " ( " + eng_absent + " ) ";
            }
            else
            {
                eng_absent = "";
            }
            if (eng_sus != "")
            {
                eng_sus = " ( " + eng_sus + " ) ";
            }
            else
            {
                eng_sus = "";
            }
            if (eng_od != "")
            {
                eng_od = " ( " + eng_od + " ) ";
            }
            else
            {
                eng_od = "";
            }
            if (eng_proj != "")
            {
                eng_proj = " ( " + eng_proj + " ) ";
            }
            else
            {
                eng_proj = "";
            }
        }

        if (sections.Trim() != "")
        {
            sections = "'" + sections + "'";
        }

        hat.Clear();
        hat.Add("monthyear", MthYear);
        hat.Add("degree_code", deg_code);
        hat.Add("curr_sem", current_sem);
        hat.Add("strsec", sections);
        hat.Add("date", date_concat);
        hat.Add("batch_year", batch_year);
        hat.Add("field_val_mng1", mng_present);
        hat.Add("field_val_mng2", mng_leav);
        hat.Add("field_val_mng3", mng_absent);
        hat.Add("field_val_mng4", mng_sus);
        hat.Add("field_val_mng5", mng_od);
        hat.Add("field_val_mng6", mng_proj);
        hat.Add("field_val_eng1", eng_present);
        hat.Add("field_val_eng2", eng_leav);
        hat.Add("field_val_eng3", eng_absent);
        hat.Add("field_val_eng4", eng_sus);
        hat.Add("field_val_eng5", eng_od);
        hat.Add("field_val_eng6", eng_proj);
        ds2 = d2.select_method("find_value_overall", hat, "sp");
        string fistabsent = "";
        string presentcount = "";
        firstabsent1 = 0;
        secondabsent1 = 0;
        if (ds2.Tables[0].Rows.Count > 0)
        {
            if (ds2.Tables[0].Rows.Count > 0)
            {
                presentcount = (Convert.ToString(ds2.Tables[0].Rows[0]["Count"]));
                if (presentcount.Trim() != "")
                {
                    secondabsent1 = Convert.ToDouble(presentcount);
                }
            }

            if (ds2.Tables[2].Rows.Count > 0)
            {
                fistabsent = (Convert.ToString(ds2.Tables[2].Rows[0]["Count"]));
                if (fistabsent.Trim() != "")
                {
                    firstabsent1 = Convert.ToDouble(fistabsent);
                }
            }

            if (firstabsent1 > 0)
            {
                absent = firstabsent1;
            }
            else
            {
                absent = 0;
            }
        }
        string mrgpresent = "";
        string mrgabsent = "";

        if (mng_present != "")
            mrgpresent = "and " + mng_present;
        if (mng_absent != "")
            mrgabsent = "and " + mng_absent;

        string hostelgetquery = "Select count(*) as Count from attendance,registration,applyn a where registration.roll_no=attendance.roll_no and attendance.month_year ='" + MthYear + "' and registration.degree_code = '" + deg_code + "' and registration.current_semester ='" + current_sem + "' and  cc =0 and delflag = 0 and exam_flag <>'debar' and registration.Stud_Type ='Hostler'  and a.app_no =registration.App_No and a.sex ='0' " + mrgpresent + "";
        hostelgetquery = hostelgetquery + " Select count(*) as Count from attendance,registration,applyn a where registration.roll_no=attendance.roll_no and attendance.month_year ='" + MthYear + "' and registration.degree_code = '" + deg_code + "' and registration.current_semester ='" + current_sem + "' and  cc =0 and delflag = 0 and exam_flag <>'debar' and registration.Stud_Type ='Hostler'  and a.app_no =registration.App_No and a.sex ='0' " + mrgabsent + "";
        hostelgetquery = hostelgetquery + " Select count(*) as Count from attendance,registration,applyn a where registration.roll_no=attendance.roll_no and attendance.month_year ='" + MthYear + "' and registration.degree_code = '" + deg_code + "' and registration.current_semester ='" + current_sem + "' and  cc =0 and delflag = 0 and exam_flag <>'debar' and registration.Stud_Type ='Hostler'  and a.app_no =registration.App_No and a.sex ='1' " + mrgpresent + " ";
        hostelgetquery = hostelgetquery + " Select count(*) as Count from attendance,registration,applyn a where registration.roll_no=attendance.roll_no and attendance.month_year ='" + MthYear + "' and registration.degree_code = '" + deg_code + "' and registration.current_semester ='" + current_sem + "' and  cc =0 and delflag = 0 and exam_flag <>'debar' and registration.Stud_Type ='Hostler'  and a.app_no =registration.App_No and a.sex ='1' " + mrgabsent + " ";
        DataSet dNew = d2.select_method_wo_parameter(hostelgetquery, "Text");

        if (dNew.Tables[0].Rows.Count > 0)
        {
            boyshst_present = (Convert.ToString(dNew.Tables[0].Rows[0]["Count"]));
            if (boyshst_present.Trim() != "")
            {
                boyshst_present1 = Convert.ToDouble(boyshst_present);
            }
        }

        if (dNew.Tables[1].Rows.Count > 0)
        {
            boyshst_absent = (Convert.ToString(dNew.Tables[1].Rows[0]["Count"]));
            if (boyshst_absent.Trim() != "")
            {
                boyshst_absent1 = Convert.ToDouble(boyshst_absent);
            }
        }

        if (dNew.Tables[2].Rows.Count > 0)
        {
            girlshst_present = (Convert.ToString(dNew.Tables[2].Rows[0]["Count"]));
            if (girlshst_present.Trim() != "")
            {
                girlshst_present1 = Convert.ToDouble(girlshst_present);
            }
        }

        if (dNew.Tables[3].Rows.Count > 0)
        {
            girlshst_absent = (Convert.ToString(dNew.Tables[3].Rows[0]["Count"]));
            if (girlshst_absent.Trim() != "")
            {
                girlshst_absent1 = Convert.ToDouble(girlshst_absent);
            }
        }

        if (!Girls_hostel_present.Contains(Convert.ToString(year_value)))
        {
            Girls_hostel_present.Add(Convert.ToString(year_value), Convert.ToString(girlshst_present1) + '-' + Convert.ToString(girlshst_absent1));
        }
        else
        {
            string values1 = Convert.ToString(Girls_hostel_present[Convert.ToString(year_value)]);
            double newprecentvalue = 0;
            double absentvalue = 0;
            if (values1.Trim() != "")
            {
                string[] splitnew = values1.Split('-');
                if (splitnew.Length > 0)
                {
                    string presentnew = Convert.ToString(splitnew[0]);
                    string absent = Convert.ToString(splitnew[1]);
                    if (presentnew.Trim() != "")
                    {
                        newprecentvalue = Convert.ToDouble(presentnew) + Convert.ToDouble(girlshst_present1);
                    }
                    else
                    {
                        newprecentvalue = Convert.ToDouble(girlshst_present1);
                    }

                    if (absent.Trim() != "")
                    {
                        absentvalue = Convert.ToDouble(absent) + Convert.ToDouble(girlshst_absent1);
                    }
                    else
                    {
                        absentvalue = Convert.ToDouble(girlshst_absent1);
                    }
                }

                Girls_hostel_present.Remove(Convert.ToString(year_value));
                //Boys_hostel_present.Add(Convert.ToString(year), girlshst_present1);
                Girls_hostel_present.Add(Convert.ToString(year_value), Convert.ToString(newprecentvalue) + '-' + Convert.ToString(girlshst_absent1));

            }
        }




        if (!Boys_hostel_present.Contains(Convert.ToString(year_value)))
        {
            Boys_hostel_present.Add(Convert.ToString(year_value), Convert.ToString(boyshst_present1) + '-' + Convert.ToString(boyshst_absent1));
        }
        else
        {
            string values2 = Convert.ToString(Boys_hostel_present[Convert.ToString(year_value)]);
            double boysnewprecentvalue = 0;
            double boysabsentvalue = 0;
            if (values2.Trim() != "")
            {
                string[] splitnew = values2.Split('-');
                if (splitnew.Length > 0)
                {
                    string boyspresentnew = Convert.ToString(splitnew[0]);
                    string boysabsent = Convert.ToString(splitnew[1]);
                    if (boyspresentnew.Trim() != "")
                    {
                        boysnewprecentvalue = Convert.ToDouble(boyspresentnew) + Convert.ToDouble(boyshst_present1);
                    }
                    else
                    {
                        boysnewprecentvalue = Convert.ToDouble(boyshst_present1);
                    }

                    if (boysabsent.Trim() != "")
                    {
                        boysabsentvalue = Convert.ToDouble(boysabsent) + Convert.ToDouble(boyshst_absent1);
                    }
                    else
                    {
                        boysabsentvalue = Convert.ToDouble(boyshst_absent1);
                    }
                }

                Boys_hostel_present.Remove(Convert.ToString(year_value));

                Boys_hostel_present.Add(Convert.ToString(year_value), Convert.ToString(boysnewprecentvalue) + '-' + Convert.ToString(boyshst_absent1));


            }
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
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                for (int j = 3; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }

            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    GridView HeaderGrid = (GridView)sender;
            //    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            //    TableCell HeaderCell = new TableCell();
            //    HeaderCell.Text = "";
            //    HeaderCell.ColumnSpan = 4;


            //    TableCell HeaderCell1 = new TableCell();
            //    HeaderCell1.Text = "Total";
            //    HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            //    HeaderCell1.ColumnSpan = 2;
            //    TableCell HeaderCell2 = new TableCell();
            //    HeaderCell2.Text = "";
            //    HeaderCell2.ColumnSpan = 2;
            //    TableCell HeaderCell3 = new TableCell();
            //    HeaderCell3.Text = "Days Scholar";
            //    HeaderCell3.HorizontalAlign = HorizontalAlign.Center;
            //    HeaderCell3.ColumnSpan = 2;
            //    TableCell HeaderCell4 = new TableCell();
            //    HeaderCell4.Text = "Hostler";
            //    HeaderCell4.HorizontalAlign = HorizontalAlign.Center;
            //    HeaderCell4.ColumnSpan = 2;


            //    HeaderGridRow.Cells.Add(HeaderCell);
            //    HeaderGridRow.Cells.Add(HeaderCell1);
            //    HeaderGridRow.Cells.Add(HeaderCell2);
            //    HeaderGridRow.Cells.Add(HeaderCell3);
            //    HeaderGridRow.Cells.Add(HeaderCell4);
            //    Showgrid.Controls[0].Controls.AddAt(0, HeaderGridRow);

            //}

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
        spReportName.InnerHtml = "Consolidated Cumulative Attendance Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }

}





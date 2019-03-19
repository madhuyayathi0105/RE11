//==========MANIPRABHA A.
//===============================no Special Hour
using System;//=====================================modified on 11/1/12,24/1/12, 7/2/12, 8/2/12,13/2/12, 29/2/12(border width,XL)
//=========================21/3/12(change function into a simple way), 2/4/12(halfday holiday), 25/4/12(tot_sem value get)
//=======================10/5/12(complete print master setting),13/6/12(iso code,try-catch,p_m_s_n)
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;

using System.Reflection;
public partial class ksrattndreport : System.Web.UI.Page
{



   



    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    SqlCommand cmd2;
    SqlCommand cmd1;
    SqlCommand cmd_holi = new SqlCommand();

    Hashtable hat = new Hashtable();
    Hashtable hat_dept = new Hashtable();
    Hashtable hat_roman = new Hashtable();
    Hashtable hat_tot = new Hashtable();
    Hashtable hat_abs = new Hashtable();
    Hashtable hat_pres = new Hashtable();
    Hashtable hat_ind_stud_list = new Hashtable();
    Hashtable hat_dept_wh = new Hashtable();
    Hashtable hat_dept_head = new Hashtable();
    DAccess2 dacces2 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds_calcflag = new DataSet();
    DataSet ds_count = new DataSet();
    DataSet ds_degree = new DataSet();
    DataSet ds_stud = new DataSet();
    DataSet ds_holi = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds_attndmaster = new DataSet();


    Boolean holiday_flag = false;
    int mng_holi = 0, evng_holi = 0;
    DataSet ds_holiday = new DataSet();
    int stud_s_no = 0;
    Boolean dept_flag = true;
    string db_date = "";
    Boolean sflag = false;
    string dept_name = "";
    int x_row_cnt = 0;
    string dept_code = "";
    double per_tage_date = 0;
    int tot_sem = 0;
    double strength_end = 0, present_end = 0, absent_end = 0;
    TimeSpan ts = new TimeSpan();
    string dum_tage_date, dum_tage_hrs;
    double per_con_hrs, per_tage_hrs;
    string tot_days_conducted = "";
    Boolean set_sem_flag = false;
    int deg_count = 0;
    string diff_date = "";
    string date = "";
    string tempvalue = "";
    int ObtValue = 0;
    int per_abshrs = 0;
    int njhr = 0;
    int per_perhrs = 0;
    int tot_per_hrs = 0;
    int per_ondu = 0;
    int tot_ondu = 0;
    int per_leave = 0;
    int per_hhday = 0;
    int unmark = 0;
    int minpresI = 0;
    double Present = 0;
    double Leave = 0;
    double leave_point = 0;
    double leave_pointer = 0;
    double Absent = 0;
    double absent_point = 0;
    double absent_pointer = 0;
    double per_holidate = 0;
    double dum_unmark = 0;
    double Onduty = 0;
    double njdate = 0;
    double workingdays = 0;
    int check = 0;
    int per_tot_ondu = 0;
    double per_njdate = 0;
    int per_per_hrs = 0;
    double pre_present_date = 0;
    double per_absent_date = 0;
    double pre_ondu_date = 0;
    double pre_leave_date = 0;
    double per_workingdays = 0;
    double per_dum_unmark = 0;


    //added by rajasekar 11/09/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    int rowcount1 = 0;

    //==================================//


    //-----------------------------------13/6/12 PRABHA
    string isonumber = "";
    string new_header_string_index = "";// = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
    //--------------------------------------

    int i = 0;
    int stud_count = 0;
    int demfcal, demtcal;
    string frdate = "";
    int cal_from_date = 0;
    string todate = "";
    int cal_to_date = 0;
    DateTime per_from_date = new DateTime();
    DateTime per_to_date = new DateTime();
    DateTime dumm_from_date = new DateTime();
    int count_master = 0;
    int count = 0;
    int value_count = 0;
    int npresent = 0;
    int nabsent = 0;
    int row_value = 0;
    int col_value = 1;
    int count_val = 0;
    int tot_hrs = 0;
    int month_year = 0;
    int first_half_hr = 0;
    int sec_half_hr = 0;
    int Atday = 0;
    int rowhead = 1;
    int minpresII = 0;

    double tot_final_absent = 0;
    double tot_final_present = 0;
    double tot_final_strength = 0;
    double tot_final_ge_strength = 0;
    double tot_final_ge_absent = 0;
    double tot_final_ge_present = 0;
    double tot_present = 0;
    double tot_absent = 0;


    int tot_strength = 0;
    string acronym = "";
    string present_calcflag = "";
    string absent_calcflag = "";
    string tot_absent_student = "";
    string eng_present = "";
    string eng_leav = "";
    string eng_absent = "";
    string eng_sus = "";
    string eng_od = "";
    string eng_proj = "";
    string mng_present_temp_str1 = "", mng_present_temp_str2 = "", mng_present_temp_str3 = "", mng_present_temp_str4 = "", mng_present_temp_string1 = "", mng_present_temp_string2 = "";
    string mng_present = "";
    string mng_leav = "";
    string mng_absent = "";
    string mng_sus = "";
    string mng_od = "";
    string mng_proj = "";
    DateTime start_date = new DateTime();
    Boolean rowheader = false;
    Boolean overflaf = false;
    Boolean finalflag = false;
    Boolean deptflag = false;
    DateTime date_time = new DateTime();
    int count_sno = 0;
    double noofWorkingDays = 0;
    string deg_code = "";
    int curr_sem = 0;
    string roman_val = "";
    string sub_val = "";
    string sem_merge = "";
    int merge2 = 0;
    string merge1 = "";
    int tot_tot = 0;
    int rowhead_val = 1;
    int present_tot = 0;
    int absent_tot = 0;
    string value = "";
    string text_val = "";
    string merge3 = "";
    Boolean flag = false;
    string sub_name = "";
    int sno = 1;
    string xxx = "";
    string yy = "";
    Boolean xflag = false;
    Boolean fflag = false;
    string rollno = "";
    int temp_val = 0;
    string colege_code = "";
    string StDate = "";
    string batch_year_val = "";
    string average_per = "";
    double noofpresent = 0;
    double noofabsent = 0;
    double noofHalfDay = 0;
    double noofWorkingHours = 0;
    double noofpresent_day = 0;
    double percent_day = 0;
    double noofabsent_day = 0;
    double noofHalfDay_day = 0, end_strength = 0, end_pres = 0, end_abs = 0, totabseve = 0, totpreeve = 0;
    double percent_day_avg = 0;
    string strDegree = "";

    //==============0n 25/4/12 PRABHA
    string[] string_session_values = new string[100];
    int temp_count = 0, final_print_col_cnt = 0, split_col_for_footer = 0, col_count = 0, footer_balanc_col = 0, footer_count = 0;
    int col_count_all = 0, span_cnt = 0, child_span_count = 0;
    Boolean check_col_count_flag = false;
    static DataSet dsprint = new DataSet();
    string new_header_string = "", column_field = "", printvar = "";
    string view_footer = "", view_header = "", view_footer_text = "";
    int start_column = 0, end_column = 0;
    string coll_name = "", form_name = "", phoneno = "", faxno = "";
    string footer_text = "", header_alignment = "";
    string degree_deatil = "";
    int new_header_count = 0;
    string[] new_header_string_split;
    string phone = "", fax = "", email_id = "", web_add = "";
    Boolean btnclick_or_print = false;
    int between_visible_col_cnt = 0, between_visible_col_cnt_bal = 0;
    int x = 0;
    int visi_col = 0, visi_col1 = 0;
    string date1 = "", datefrom = "", date2 = "", dateto = "";
    DateTime dt1 = new DateTime();
    DateTime dt2 = new DateTime();
    string email = "";
    string website = "";
    string address1 = "";
    string address2 = "";
    string address3 = "";
    int stud_list_row_val = 0;
    string group_code = "", columnfield = "";
    Double morprenst = 0;
    Double eveprenst = 0;
    Double morabsent = 0;
    Double eveabsent = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        lblerr.Visible = false;
        if (!Page.IsPostBack)
        {
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            datetxt.Attributes.Add("readonly", "readonly");
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            //Button1.Visible = false;
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
                gobtn.Enabled = true;
                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                ddlcollege_SelectedIndexChanged(sender, e);
                datetxt.Enabled = true;
            }
            else
            {
                ddlcollege.Enabled = false;
                gobtn.Enabled = false;
                datetxt.Enabled = false;

            }
            Pageload(sender, e);

        }

    }
    protected void gobtn_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            txtexcelname.Text = "";
            btnclick();

            int temp_col = 0;

            
            if (dtl.Columns.Count > 0 && dtl.Rows.Count > 0)//===========on 9/4/12
            {
                
                Showgrid.Visible = true;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;
                //Button1.Visible = true;
                Panel3.Visible = false;


               

                final_print_col_cnt = 0;
                for (temp_col = 0; temp_col < dtl.Columns.Count; temp_col++)
                {
                    //if (attnd_report.Sheets[0].Columns[temp_col].Visible == true)
                    //{
                    final_print_col_cnt++;
                    //}
                }



                

                //4.college information setting
                //setheader_print();

                //4 end.college information setting


                int dtrowcount =dtl.Rows.Count;

                list_absent_students();//==================================function 25/4/12 PRABHA

                //footer_set();//========footer setting

                //view_header_setting();




                //added by rajasekar 12/09/2018
                Showgrid.DataSource = dtl;
                Showgrid.DataBind();
                Showgrid.HeaderRow.Visible = false;



                int rowspanstart = 0;

                for (int i = 0; i < Showgrid.Rows.Count; i++)
                {
                    int rowspancount = 0;
                    if (i != dtrowcount - 1)
                    {
                        
                        if (rowspanstart == i)
                        {
                            for (int k = rowspanstart + 1; Showgrid.Rows[i].Cells[0].Text == Showgrid.Rows[k].Cells[0].Text; k++)
                            {
                                rowspancount++;
                            }
                            rowspanstart++;
                        }


                        if (rowspancount != 0)
                        {
                            rowspanstart = rowspanstart + rowspancount;
                            Showgrid.Rows[i].Cells[0].RowSpan = rowspancount + 1;
                            for (int a = i; a < rowspanstart - 1; a++)
                                Showgrid.Rows[a + 1].Cells[0].Visible = false;

                            Showgrid.Rows[i].Cells[1].Text = Showgrid.Rows[rowspanstart - 1].Cells[1].Text;
                            Showgrid.Rows[i].Cells[1].RowSpan = rowspancount + 1;
                            for (int a = i; a < rowspanstart - 1; a++)
                                Showgrid.Rows[a + 1].Cells[1].Visible = false;


                            Showgrid.Rows[i].Cells[4].Text = Showgrid.Rows[rowspanstart - 1].Cells[4].Text;
                            Showgrid.Rows[i].Cells[4].RowSpan = rowspancount + 1;
                            for (int a = i; a < rowspanstart - 1; a++)
                                Showgrid.Rows[a + 1].Cells[4].Visible = false;


                            Showgrid.Rows[i].Cells[7].Text = Showgrid.Rows[rowspanstart - 1].Cells[7].Text;
                            Showgrid.Rows[i].Cells[7].RowSpan = rowspancount + 1;
                            for (int a = i; a < rowspanstart - 1; a++)
                                Showgrid.Rows[a + 1].Cells[7].Visible = false;


                            Showgrid.Rows[i].Cells[8].Text = Showgrid.Rows[rowspanstart - 1].Cells[8].Text;
                            Showgrid.Rows[i].Cells[8].RowSpan = rowspancount + 1;
                            for (int a = i; a < rowspanstart - 1; a++)
                                Showgrid.Rows[a + 1].Cells[8].Visible = false;


                            Showgrid.Rows[i].Cells[11].Text = Showgrid.Rows[rowspanstart - 1].Cells[11].Text;
                            Showgrid.Rows[i].Cells[11].RowSpan = rowspancount + 1;
                            for (int a = i; a < rowspanstart - 1; a++)
                                Showgrid.Rows[a + 1].Cells[11].Visible = false;

                            Showgrid.Rows[i].Cells[12].Text = Showgrid.Rows[rowspanstart - 1].Cells[12].Text;
                            Showgrid.Rows[i].Cells[12].RowSpan = rowspancount + 1;
                            for (int a = i; a < rowspanstart - 1; a++)
                                Showgrid.Rows[a + 1].Cells[12].Visible = false;
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
                        }
                        else
                        {

                            if (i < dtrowcount - 1)
                            {
                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                            }
                            else if (i == dtrowcount - 1)
                            {
                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                if (j == 0)
                                {

                                    Showgrid.Rows[i].Cells[j].ColumnSpan = 3;
                                    for (int a = 0; a < 2; a++)
                                        Showgrid.Rows[i].Cells[a + 1].Visible = false;

                                }
                                else if (j == 3 || j == 5 || j == 9)
                                {

                                    Showgrid.Rows[i].Cells[j].ColumnSpan = 2;
                                    for (int a = 0; a < 1; a++)
                                        Showgrid.Rows[i].Cells[j + 1].Visible = false;

                                }

                            }
                            else if (Showgrid.Rows[i].Cells[j].Text == "&nbsp;")
                            {
                                if (j == 2)
                                {
                                    if (Showgrid.Rows[i].Cells[j - 1].Text == Showgrid.Rows[i].Cells[j - 2].Text)
                                    {

                                        Showgrid.Rows[i].Cells[j - 2].HorizontalAlign = HorizontalAlign.Center;
                                        Showgrid.Rows[i].Cells[j - 2].Font.Underline = true;
                                        Showgrid.Rows[i].Cells[j - 2].Font.Bold = true;
                                        Showgrid.Rows[i].Cells[j - 2].ColumnSpan = Showgrid.Rows[i].Cells.Count;

                                        for (int a = 1; a < Showgrid.Rows[i].Cells.Count; a++)
                                            Showgrid.Rows[i].Cells[a].Visible = false;




                                    }
                                }
                                else
                                {
                                    Showgrid.Rows[i].Cells[j - 1].HorizontalAlign = HorizontalAlign.Center;
                                    Showgrid.Rows[i].Cells[j - 1].Font.Bold = true;
                                    Showgrid.Rows[i].Cells[j - 1].ColumnSpan = Showgrid.Rows[i].Cells.Count;
                                    for (int a = 1; a < Showgrid.Rows[i].Cells.Count; a++)
                                        Showgrid.Rows[i].Cells[a].Visible = false;

                                }


                            }
                        }
                    }





                }


                //==========================================//

            }
            else
            {
                
                Showgrid.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                //Button1.Visible = false;
                Panel3.Visible = false;
                errlbl.Visible = true;
            }

        }
        catch
        {
        }
        
    }

    public void btnclick()
    {
        rowhead = 1;
        try
        {
            if (datetxt.Text.Trim() != string.Empty)
            {

                string date1 = "", datefrom = "";
                date1 = datetxt.Text.ToString();
                string[] split1 = date1.Split(new Char[] { '/' });
                datefrom = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                if (dt1 <= System.DateTime.Now)
                {
                    //==check holiday
                    //cmd_holi.CommandText = "select top 1 holiday_desc from holidaystudents where holiday_date='" + dt1 + "'";
                    //cmd_holi.Connection = con;
                    //con.Close();
                    //con.Open();
                    //SqlDataReader dr_holday = cmd_holi.ExecuteReader();
                    //dr_holday.Read();
                    ////===================
                    //if (dr_holday.HasRows == false)
                    {
                        //-----------declaration
                        datelbl.Visible = false;
                        //  pagesetpanel.Visible = true;
                        
                        tot_final_ge_strength = 0;
                        tot_final_ge_present = 0;
                        tot_final_ge_absent = 0;
                        //--------------end dec
                        
                        //attnd_report.Sheets[0].ColumnHeader.RowCount = 7;
                        
                        Panel3.Visible = false;
                        
                        //-------------date split

                        date = datetxt.Text;
                        string[] datesplt = date.Split('/');

                        try
                        {
                            db_date = datesplt[1] + "/" + datesplt[0] + "/" + datesplt[2];
                            date_time = Convert.ToDateTime(db_date.ToString());
                            Atday = int.Parse(datesplt[0].ToString());
                            month_year = int.Parse(datesplt[1]) + (int.Parse(datesplt[2]) * 12);
                        }
                        catch
                        {
                            try
                            {
                                db_date = datesplt[0] + "/" + datesplt[1] + "/" + datesplt[2];
                                Atday = int.Parse(datesplt[1].ToString());
                                date_time = Convert.ToDateTime(db_date.ToString());
                            }
                            catch
                            {

                            }
                        }




                        //=============================0n 9/4/12
                        //hat.Clear();
                        //hat.Add("college_code", Session["InternalCollegeCode"].ToString());
                        //hat.Add("form_name", "overalldailyattndreport.aspx");
                        //dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
                        ////===========================================


                        ////======================0n 11/4/12 PRABHA
                        //if (dsprint.Tables[0].Rows.Count > 0)
                        //{
                        //    isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();

                        //    if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                        //    {
                        //        new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();                             
                        //        attnd_report.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorBottom = Color.White;
                        //        new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                        //        new_header_string_split = new_header_string.Split(',');
                        //        attnd_report.Sheets[0].SheetCorner.RowCount = attnd_report.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
                        //    }
                        //}
                        ////=====================================

                        

                        //----------------end split
                        
                        Showgrid.Visible = true;
                        btnprintmaster.Visible = true;
                        btnPrint.Visible = true;
                        btnxl.Visible = true;
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;

                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        int colu = 0;


                        dtl.Columns.Add("S.No", typeof(string));
                        dtl.Rows[0][colu] = "S.No";
                        colu++;
                        dtl.Columns.Add("Depart Name", typeof(string));
                        dtl.Rows[0][colu] = "Depart Name";
                        colu++;
                        dtl.Columns.Add("Year", typeof(string));
                        dtl.Rows[0][colu] = "Year";
                        colu++;
                        dtl.Columns.Add("Year Wise Total Strength", typeof(string));
                        dtl.Rows[0][colu] = "Year Wise Total Strength";
                        colu++;
                        dtl.Columns.Add("Total Strength", typeof(string));
                        dtl.Rows[0][colu] = "Total Strength";
                        colu++;
                        dtl.Columns.Add("Year Wise No Of Absent Morning", typeof(string));
                        dtl.Rows[0][colu] = "Year Wise No Of Absent Morning";
                        colu++;
                        dtl.Columns.Add("Year Wise No Of Absent Evening", typeof(string));
                        dtl.Rows[0][colu] = "Year Wise No Of Absent Evening";
                        colu++;
                        dtl.Columns.Add("Toatl No Of Absent Morning", typeof(string));
                        dtl.Rows[0][colu] = "Toatl No Of Absent Morning";
                        colu++;
                        dtl.Columns.Add("Toatl No Of Absent Evening", typeof(string));
                        dtl.Rows[0][colu] = "Toatl No Of Absent Evening";
                        colu++;
                        dtl.Columns.Add("Year Wise No Of Present Morning", typeof(string));
                        dtl.Rows[0][colu] = "Year Wise No Of Present Morning";
                        colu++;
                        dtl.Columns.Add("Year Wise No Of Present Evening", typeof(string));
                        dtl.Rows[0][colu] = "Year Wise No Of Present Evening";
                        colu++;
                        dtl.Columns.Add("Total No Of Present Morning", typeof(string));
                        dtl.Rows[0][colu] = "Total No Of Present Morning";
                        colu++;
                        dtl.Columns.Add("Total No Of Present Evening", typeof(string));
                        dtl.Rows[0][colu] = "Total No Of Present Evening";
                        colu++;
                     
                        //=======end col head

                        //---------------get calcflag
                        hat.Clear();
                        hat.Add("colege_code", Session["InternalCollegeCode"].ToString());
                        ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                        count_master = (ds_attndmaster.Tables[0].Rows.Count);
                        if (count_master > 0)
                        {
                            for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                            {
                                if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                                {
                                    if (present_calcflag == "")
                                    {
                                        present_calcflag = ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString();
                                    }
                                    else
                                    {
                                        present_calcflag = present_calcflag + "," + ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString();
                                    }
                                }
                                if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                                {
                                    if (absent_calcflag == "")
                                    {
                                        absent_calcflag = ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString();
                                    }
                                    else
                                    {
                                        absent_calcflag = absent_calcflag + "," + ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString();
                                    }
                                }
                            }


                            if (datesplt.GetUpperBound(0) == 2)//-------date valid
                            {
                                if (Convert.ToInt16(datesplt[0].ToString()) <= 31 && Convert.ToInt16(datesplt[1].ToString()) <= 12 && Convert.ToInt16(datesplt[2].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                                {
                                    //----------------------get info
                                    string strsemval = "select * from seminfo ";
                                    DataSet dssem = d2.select_method_wo_parameter(strsemval, "Text");
                                    DataView dvsem = new DataView();
                                    hat.Clear();
                                    hat.Add("@collegecode", Session["InternalCollegeCode"]);
                                    ds_degree = dacces2.select_method("get_degreeinfo", hat, "sp");
                                    if (ds_degree.Tables[0].Rows.Count > 0)
                                    {
                                        for (deg_count = 0; deg_count < ds_degree.Tables[0].Rows.Count; deg_count++)//-------degree code
                                        {
                                            // hat_dept_head.Add(deg_count, ds_degree.Tables[0].Rows[deg_count]["Dept_name"].ToString());
                                            deptflag = false;
                                            rowheader = false;
                                            count = 0;
                                            tot_final_strength = 0;
                                            tot_final_absent = 0;
                                            tot_final_present = 0;
                                            acronym = ds_degree.Tables[0].Rows[deg_count]["Acronym"].ToString();
                                            deg_code = ds_degree.Tables[0].Rows[deg_count]["Degree_Code"].ToString();
                                            dept_code = ds_degree.Tables[0].Rows[deg_count]["dept_code"].ToString();
                                            dept_name = ds_degree.Tables[0].Rows[deg_count]["dept_name"].ToString();

                                            Double totdeptadsentmor = 0;
                                            Double totdeptadsenteve = 0;
                                            Double totdeptprentmor = 0;
                                            Double totdeptprenteve = 0;
                                            hat.Clear();
                                            hat.Add("degree_code", deg_code);
                                            hat.Add("input_date", date_time);
                                            hat.Add("collegecode", Session["InternalCollegeCode"]);
                                            ds = dacces2.select_method("bind_degree_strength", hat, "sp");
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {

                                                for (count_val = 0; count_val < ds.Tables[0].Rows.Count; count_val++)
                                                {

                                                    holiday_flag = false;
                                                    mng_holi = 1;
                                                    evng_holi = 1;
                                                    if (count_val == ds.Tables[0].Rows.Count - 1)
                                                    {
                                                        tot_sem = int.Parse(ds.Tables[0].Rows[(ds.Tables[0].Rows.Count - 1)]["current_semester"].ToString());
                                                    }
                                                    sflag = true;
                                                    curr_sem = int.Parse(ds.Tables[0].Rows[count_val]["current_semester"].ToString());
                                                    tot_hrs = int.Parse(ds.Tables[0].Rows[count_val]["no_of_hrs_per_day"].ToString());
                                                    first_half_hr = int.Parse(ds.Tables[0].Rows[count_val]["no_of_hrs_I_half_day"].ToString());
                                                    sec_half_hr = int.Parse(ds.Tables[0].Rows[count_val]["no_of_hrs_II_half_day"].ToString());
                                                    start_date = DateTime.Parse(ds.Tables[0].Rows[count_val]["start_date"].ToString());
                                                    minpresI = int.Parse(ds.Tables[0].Rows[count_val]["min_pres_I_half_day"].ToString());
                                                    minpresII = int.Parse(ds.Tables[0].Rows[count_val]["min_pres_II_half_day"].ToString());

                                                    //Added by srinath 1/8/2014 
                                                    dssem.Tables[0].DefaultView.RowFilter = " start_date='" + start_date + "' and degree_code='" + deg_code + "' and semester='" + curr_sem + "'";
                                                    dvsem = dssem.Tables[0].DefaultView;
                                                    string endate = "";
                                                    string startdate = "";
                                                    if (dvsem.Count > 0)
                                                    {//added by annyutha** 04nd sep 14//
                                                        for (int j = 0; j < dvsem.Count; j++)
                                                        {
                                                            //end***//
                                                            startdate = dvsem[j]["start_date"].ToString();
                                                            endate = dvsem[j]["end_date"].ToString();
                                                            DateTime dtstart = Convert.ToDateTime(startdate);
                                                            DateTime dtendate = Convert.ToDateTime(endate);
                                                            if (dtstart <= date_time && dtendate >= date_time)
                                                            {

                                                                //if (curr_sem != 1 && curr_sem != 2)
                                                                //{
                                                                if (rowheader == false)
                                                                {
                                                                    rowheader = true;
                                                                }
                                                                count++;
                                                                if (curr_sem.ToString() != null)
                                                                {
                                                                    if (curr_sem % 2 == 0)
                                                                    {
                                                                        roman_val = sem_roman(curr_sem / 2);
                                                                        GiveCourseName(deg_code, out sub_val, out sub_name);

                                                                    }
                                                                    else
                                                                    {
                                                                        roman_val = sem_roman((curr_sem + 1) / 2);
                                                                        GiveCourseName(deg_code, out sub_val, out sub_name);
                                                                    }
                                                                }


                                                                
                                                                yy = sub_val;
                                                               
                                                                tot_final_strength = tot_final_strength + double.Parse(ds.Tables[0].Rows[count_val]["strength"].ToString());

                                                               
                                                                //added by rajasekar 11/09/2018
                                                                if (rowcount1==1)
                                                                    dtl.Rows.Add(dtrow);

                                                                rowcount1 = 1;
                                                                dtrow = dtl.NewRow();
                                                                dtrow["Depart Name"] = sub_val;//set course  


                                                                dtrow["Year"] = roman_val;//set roman


                                                                dtrow["Year Wise Total Strength"] = ds.Tables[0].Rows[count_val]["strength"].ToString();//set strength

                                                                //===================================//


                                                                con.Close();
                                                                con.Open();
                                                                string holiday_string = "select  holiday_date,halforfull,morning,evening FROM holidayStudents where holiday_date= '" + dt1 + "' and degree_code=" + deg_code + " and semester=" + curr_sem + "";
                                                                SqlDataAdapter da_holiday = new SqlDataAdapter(holiday_string, con);
                                                                da_holiday.Fill(ds_holiday);
                                                                if (ds_holiday.Tables[0].Rows.Count > 0)
                                                                {
                                                                    if (ds_holiday.Tables[0].Rows[0]["halforfull"].ToString() == "False")
                                                                    {
                                                                        holiday_flag = true;
                                                                        mng_holi = 0;
                                                                        evng_holi = 0;
                                                                    }
                                                                    else if (ds_holiday.Tables[0].Rows[0]["halforfull"].ToString() == "True")
                                                                    {
                                                                        holiday_flag = true;
                                                                        if (ds_holiday.Tables[0].Rows[0]["morning"].ToString() == "True")
                                                                        {
                                                                            mng_holi = 0;
                                                                            evng_holi = 1;
                                                                        }
                                                                        if (ds_holiday.Tables[0].Rows[0]["evening"].ToString() == "True")
                                                                        {
                                                                            mng_holi = 1;
                                                                            evng_holi = 0;
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    holiday_flag = false;

                                                                }

                                                                if ((mng_holi == 1) || (evng_holi == 1))
                                                                {
                                                                    attndvalue();
                                                                }

                                                                //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].Text = tot_absent.ToString();
                                                                //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].HorizontalAlign = HorizontalAlign.Center;
                                                                //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].Text = tot_present.ToString();
                                                                //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].HorizontalAlign = HorizontalAlign.Center;

                                                               

                                                                //added by rajasekar 11/09/2018
                                                                dtrow["Year Wise No Of Absent Morning"] = morabsent.ToString();

                                                                dtrow["Year Wise No Of Absent Evening"] = eveabsent.ToString();

                                                                dtrow["Year Wise No Of Present Morning"] = morprenst.ToString();

                                                                dtrow["Year Wise No Of Present Evening"] = eveprenst.ToString();


                                                                //==================================//

                                                                //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].HorizontalAlign = HorizontalAlign.Center;
                                                                //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].Text = tot_present.ToString();
                                                                //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].HorizontalAlign = HorizontalAlign.Center;

                                                                //tot_final_absent = tot_final_absent + tot_absent;
                                                                //tot_final_present = tot_final_present + tot_present;
                                                                totdeptadsentmor = totdeptadsentmor + morabsent;
                                                                totdeptadsenteve = totdeptadsenteve + eveabsent;
                                                                totdeptprentmor = totdeptprentmor + morprenst;
                                                                totdeptprenteve = totdeptprenteve + eveprenst;
                                                                tot_absent = 0;
                                                                tot_present = 0;
                                                                if (xxx == "")
                                                                {
                                                                    xxx = yy;
                                                                }
                                                                if (xxx != yy)
                                                                {
                                                                    rowhead++;
                                                                    

                                                                    dtrow["S.No"] = rowhead.ToString();

                                                                }
                                                                else
                                                                {

                                                                   

                                                                    dtrow["S.No"] = rowhead.ToString();
                                                                }
                                                                xxx = yy;
                                                            }
                                                            //else
                                                            //{
                                                            //    value_count++;
                                                            //    if (curr_sem.ToString() != null)
                                                            //    {
                                                            //        if (curr_sem % 2 == 0)
                                                            //        {
                                                            //            roman_val = sem_roman(curr_sem / 2);
                                                            //            GiveCourseName(deg_code, out sub_val, out sub_name);

                                                            //        }
                                                            //        else
                                                            //        {
                                                            //            roman_val = sem_roman((curr_sem + 1) / 2);
                                                            //            GiveCourseName(deg_code, out sub_val, out sub_name);
                                                            //        }
                                                            //    }
                                                            //    hat_dept.Add(value_count, sub_val);
                                                            //    hat_roman.Add(value_count, roman_val);
                                                            //    hat_tot.Add(value_count, ds.Tables[0].Rows[count_val]["strength"].ToString());
                                                            //    tot_final_ge_strength = tot_final_ge_strength + double.Parse(ds.Tables[0].Rows[count_val]["strength"].ToString());
                                                            //    attndvalue();

                                                            //    hat_abs.Add(value_count, tot_absent);
                                                            //    hat_pres.Add(value_count, tot_present);

                                                            //    tot_final_ge_absent = tot_final_ge_absent + tot_absent;
                                                            //    tot_final_ge_present = tot_final_ge_present + tot_present;
                                                            //    tot_absent = 0;
                                                            //    tot_present = 0;
                                                            //}
                                                        }
                                                    }
                                                }
                                                //}
                                                if (count > 0)
                                                {
                                                    


                                                    dtrow["S.No"] = rowhead.ToString();


                                                    dtrow["Total Strength"] = tot_final_strength.ToString();


                                                    dtrow["Toatl No Of Absent Morning"] = totdeptadsentmor.ToString();


                                                    dtrow["Toatl No Of Absent Evening"] = totdeptadsenteve.ToString();


                                                    dtrow["Total No Of Present Morning"] = totdeptprentmor.ToString();


                                                    dtrow["Total No Of Present Evening"] = totdeptprenteve.ToString();



                                                    absent_end = absent_end + tot_final_absent;

                                                    totabseve = totabseve + totdeptadsenteve;
                                                    totpreeve = totpreeve + totdeptprenteve;
                                                    end_abs = end_abs + totdeptadsentmor;
                                                    end_pres = end_pres + totdeptprentmor;
                                                    end_strength = end_strength + tot_final_strength;
                                                    //end_abs = end_abs + tot_final_absent;
                                                    //end_pres = end_pres + tot_final_present;

                                                    tot_final_strength = 0;
                                                    tot_final_absent = 0;
                                                    tot_final_present = 0;
                                                }

                                            }
                                        }
                                        if (rowcount1 == 1)
                                            dtl.Rows.Add(dtrow);

                                       
                                        //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].Text = end_abs.ToString();
                                        //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 6].Text = totabseve.ToString();
                                        
                                        //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 9].Text = end_pres.ToString();
                                        //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 10].Text = totpreeve.ToString();
                                       




                                        dtrow = dtl.NewRow();

                                        dtrow["S.No"] = "Total";

                                        dtrow["Year Wise Total Strength"] = end_strength.ToString();


                                        dtrow["Toatl No Of Absent Morning"] = end_abs.ToString();


                                        dtrow["Toatl No Of Absent Evening"] = totabseve.ToString();


                                        dtrow["Total No Of Present Morning"] = end_pres.ToString();


                                        dtrow["Total No Of Present Evening"] = totpreeve.ToString();


                                        dtl.Rows.Add(dtrow);




                                        

                                    }//-----------get total stud

                                    if (hat_dept.Count > 0)
                                    {
                                        rowhead++;
                                        int temp_count = 0;
                                        //rowhead++;
                                        for (temp_count = 1; temp_count <= hat_dept.Count; temp_count++)
                                        {
                                            
                                            dtrow = dtl.NewRow();
                                            //  if (temp_count == 1)
                                            {
                                               



                                                dtrow[1] = "GE";
                                                dtrow[0] = rowhead.ToString();

                                            }
                                            


                                            dtrow[2] = (GetCorrespondingKey(temp_count, hat_dept).ToString());
                                            dtrow[3] = (GetCorrespondingKey(temp_count, hat_tot).ToString());
                                            dtrow[5] = (GetCorrespondingKey(temp_count, hat_abs).ToString());
                                            dtrow[7] = (GetCorrespondingKey(temp_count, hat_pres).ToString());


                                        }

                                        //   attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - (temp_count - 1)), 1, temp_count - 1, 1);
                                       
                                        //    attnd_report.Sheets[0].RowHeaderSpanModel.Add((attnd_report.Sheets[0].RowCount - (temp_count - 1)), 0, temp_count - 1, 1);
                                        //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - (temp_count - 1)), 0].Text = rowhead.ToString();

                                        
                                        //  present_end =present_end+tot_final_ge_present;
                                        end_strength = end_strength + tot_final_ge_strength;
                                        end_abs = end_abs + tot_final_ge_absent;
                                        end_pres = end_pres + tot_final_ge_present;



                                        dtrow[4] = tot_final_ge_strength.ToString();
                                        dtrow[6] = tot_final_ge_absent.ToString();
                                        dtrow[8] = tot_final_ge_present.ToString();
                                        

                                    }

                                }
                                else
                                {
                                    
                                    Showgrid.Visible = false;
                                    btnprintmaster.Visible = false;
                                    btnPrint.Visible = false;
                                    //Button1.Visible = false; 
                                    txtexcelname.Visible = false;
                                    lblrptname.Visible = false;
                                    btnxl.Visible = false;
                                    txtexcelname.Visible = false;
                                    lblrptname.Visible = false;
                                    Panel3.Visible = false;
                                    errlbl.Visible = false;
                                    datelbl.Visible = true;
                                    datelbl.Text = "Enter Valid Date";
                                }
                            }


                            else
                            {
                                Panel3.Visible = false;
                                errlbl.Visible = false;
                                datelbl.Visible = false;
                               
                                Showgrid.Visible = false;
                                btnprintmaster.Visible = false;
                                btnPrint.Visible = false;
                                //Button1.Visible = false;
                                btnxl.Visible = false;
                                txtexcelname.Visible = false;
                                lblrptname.Visible = false;
                                //   pagesetpanel.Visible = false;
                                datelbl.Visible = true;
                                datelbl.Text = "Enter Valid Date";
                            }
                        }
                        else
                        {
                            datelbl.Visible = false;
                            Panel3.Visible = false;
                            datelbl.Visible = false;
                            
                            Showgrid.Visible = false;
                            btnprintmaster.Visible = false;
                            btnPrint.Visible = false;
                            //Button1.Visible = false;
                            btnxl.Visible = false;
                            txtexcelname.Visible = false;
                            lblrptname.Visible = false;
                            errlbl.Visible = true;
                            //   pagesetpanel.Visible = false;
                            errlbl.Text = "Update Attendance Master Setting";
                        }
                    }
                    //else
                    //{

                    //    errlbl.Visible = true;
                    //    errlbl.Text = "Selected Day Is Holiday. Reason-" + dr_holday["holiday_desc"].ToString();

                    //}
                }
                else
                {
                    datelbl.Visible = false;
                    Panel3.Visible = false;
                    datelbl.Visible = false;
                    
                    Showgrid.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    //Button1.Visible = false;
                    btnxl.Visible = false;
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    errlbl.Visible = true;
                    // pagesetpanel.Visible = false;
                    errlbl.Text = "Date Should Be Less Than Todate";
                }
            }
            else
            {
                
                Showgrid.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                //Button1.Visible = false;
                btnxl.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                Panel3.Visible = false;
                errlbl.Visible = false;
                datelbl.Visible = true;
                // pagesetpanel.Visible = false;
                datelbl.Text = "Enter Date";
            }

            //else
            //{
            //    datelbl.Visible = false;
            //    Panel3.Visible = false;
            //    datelbl.Visible = false;
            //    attnd_report.Visible = false;
            //    errlbl.Visible = true;
            //    pagesetpanel.Visible = false;
            //    errlbl.Text = "Date Should Be Greater Than Todate";
            //}

        }
        catch
        {
        }
    }

    public void list_absent_students()
    {
        try
        {
            if (sflag == true)
            {
                tot_sem = Convert.ToInt16(GetFunction("select max(current_semester) from registration").ToString());//--25//4/12 PRABHA
                // attnd_report.Sheets[0].RowCount = attnd_report.Sheets[0].RowCount + 1;

                col_count = dtl.Columns.Count;

                
                if (hat_dept_wh.Count > 0)
                {
                    


                    
                    stud_list_row_val = 0;
                    
                    //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 0].Text = "Absent Students List";
                    //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 0].Font.Size = FontUnit.Larger;
                    //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 0].Font.Bold = true;
                    //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 0].Font.Size = FontUnit.Medium;
                    //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 0].HorizontalAlign = HorizontalAlign.Center;
                    //attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 2), 0, 2, 9);

                    //================================


                    

                    
                    


                   
                    //  attnd_report.Sheets[0].SpanModel.Add(stud_list_row_val, start_column, 2, final_print_col_cnt);
                   


                    
                    dtrow = dtl.NewRow();
                    dtrow["S.No"] = "Absent Students List";
                    dtl.Rows.Add(dtrow);

                    //=======================================
                   
                }
                
                int temp_col = 4;
                


                int month_year = 0;
                string day_hour = "", temp_day = "";
                string[] date_split = datetxt.Text.Split('/');
                month_year = (Convert.ToInt16(date_split[2].ToString()) * 12) + Convert.ToInt16(date_split[1].ToString());

                for (int i = 1; i <= tot_hrs; i++)
                {
                    temp_day = " (d" + date_split[0].ToString() + "d" + i.ToString() + "<>'' or d" + date_split[0].ToString() + "d" + i.ToString() + "<>0 or d" + date_split[0].ToString() + "d" + i.ToString() + "<>null)";
                    if (day_hour == "")
                    {
                        day_hour = "( (d" + date_split[0].ToString() + "d" + i.ToString() + "<>'' or d" + date_split[0].ToString() + "d" + i.ToString() + "<>0 or d" + date_split[0].ToString() + "d" + i.ToString() + "<>null)";
                    }
                    else
                    {
                        day_hour = day_hour + " or" + temp_day;
                    }
                }
                day_hour = day_hour + ")";
                int attnd_mark_count = 0;
                Boolean attnd_mark_count_flag = false;



                for (deg_count = 0; deg_count < ds_degree.Tables[0].Rows.Count; deg_count++)//-------degree code
                {
                    attnd_mark_count = 0;

                    set_sem_flag = false;
                    stud_s_no = 0;
                    if (temp_col == 4)
                    {
                        if (dept_flag == true)
                        {
                            temp_col = 0;
                            
                            dept_flag = false;
                        }
                    }
                    else
                    {
                        if (dept_flag == true)
                        {
                            temp_col = 4;
                            dept_flag = false;
                        }
                    }
                    Boolean check_print_or_nt = false;
                    string dept_acronym = ds_degree.Tables[0].Rows[deg_count]["Acronym"].ToString();
                    string dept_name = ds_degree.Tables[0].Rows[deg_count]["dept_name"].ToString();
                    if (hat_dept_wh.ContainsKey(dept_acronym))
                    {
                        tot_days_conducted = GetCorrespondingKey(dept_acronym, hat_dept_wh).ToString();
                        // if (ds.Tables[0].Rows.Count > 0)
                        {
                            //  tot_sem = int.Parse(ds.Tables[0].Rows[(ds.Tables[0].Rows.Count - 1)]["current_semester"].ToString());

                            for (int sem = 1; sem <= tot_sem; sem++)
                            {
                                //--------roman vel
                                if (sem.ToString() != null)
                                {
                                    if (sem % 2 == 0)
                                    {
                                        roman_val = sem_roman(sem / 2);
                                    }
                                    else
                                    {
                                        roman_val = sem_roman((sem + 1) / 2);
                                    }
                                }

                                //====================

                                attnd_mark_count_flag = false;
                                //-----------------------------check sem attendance
                                con.Close();
                                con.Open();
                                DataSet ds_check_reg = new DataSet();

                                string reg = " Select count(*) from registration where degree_code =" + ds_degree.Tables[0].Rows[deg_count]["degree_code"].ToString() + " and current_semester=" + sem + " and  cc =0 and delflag = 0 and exam_flag <>'debar'";
                                SqlDataAdapter da_check_reg = new SqlDataAdapter(reg, con);
                                da_check_reg.Fill(ds_check_reg);
                                int check_count = 0;
                                check_count = ds_check_reg.Tables[0].Rows.Count;
                                if (check_count > 0)
                                {
                                    attnd_mark_count = Convert.ToInt16(ds_check_reg.Tables[0].Rows[0][0].ToString());
                                    if (attnd_mark_count > 0)
                                    {
                                        // for (int t = cal_from_date; t <= cal_to_date; t++)
                                        {
                                            con.Close();
                                            con.Open();
                                            DataSet ds_check = new DataSet();
                                            string query_check = " Select count(*) as 'Count' from attendance,registration where registration.roll_no=attendance.roll_no  and attendance.month_year =" + month_year + " and degree_code =" + ds_degree.Tables[0].Rows[deg_count]["degree_code"].ToString() + " and current_semester=" + sem + " and " + day_hour + " and  cc =0 and delflag = 0 and exam_flag <>'debar'";
                                            SqlDataAdapter da_check = new SqlDataAdapter(query_check, con);
                                            da_check.Fill(ds_check);
                                            check_count = ds_check.Tables[0].Rows.Count;
                                            if (check_count > 0)
                                            {
                                                attnd_mark_count = Convert.ToInt16(ds_check.Tables[0].Rows[0][0].ToString());
                                                if (attnd_mark_count > 0)
                                                {
                                                    attnd_mark_count_flag = true;
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        attnd_mark_count_flag = true;
                                    }
                                }
                                if (attnd_mark_count_flag == true)
                                //========================================
                                {
                                    foreach (DictionaryEntry parameter2 in hat_ind_stud_list)
                                    {
                                        string acro_roll = (parameter2.Key).ToString();
                                        string stud_name = (parameter2.Value).ToString();
                                        string[] acro_rollno = acro_roll.Split('/');

                                        if (dept_acronym == acro_rollno[0].ToString())
                                        {
                                            if (Convert.ToInt16(acro_rollno[2].ToString()) == sem)
                                            {
                                                set_sem_flag = true;
                                                if (temp_col == 0)
                                                {
                                                    if (dept_flag == false)
                                                    {
                                                        check_print_or_nt = false;
                                                        
                                                        if (final_print_col_cnt == 9)
                                                        {
                                                           
                                                            //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 0].Text = " ";
                                                           


                                                            dtrow = dtl.NewRow();
                                                            dtrow[0] = dept_name + tot_days_conducted;
                                                            dtrow[1] = dept_name + tot_days_conducted;
                                                            dtl.Rows.Add(dtrow);

                                                        }
                                                        else
                                                        {
                                                            


                                                            dtrow = dtl.NewRow();
                                                            dtrow[0] = dept_name + tot_days_conducted;
                                                            dtrow[1] = dept_name + tot_days_conducted;
                                                            dtl.Rows.Add(dtrow);
                                                        }
                                                        dept_flag = true;
                                                    }
                                                   
                                                   
                                                   
                                                }
                                                else
                                                {
                                                    if (dept_flag == false)
                                                    {
                                                        check_print_or_nt = true;
                                                        if (final_print_col_cnt == 9)
                                                        {
                                                            
                                                            dtrow = dtl.NewRow();
                                                            dtrow[0] = dept_name + tot_days_conducted;
                                                            dtrow[1] = dept_name + tot_days_conducted;
                                                            dtl.Rows.Add(dtrow);


                                                        }
                                                        else
                                                        {
                                                            

                                                            dtrow = dtl.NewRow();
                                                            dtrow[0] = dept_name + tot_days_conducted;
                                                            dtrow[1] = dept_name + tot_days_conducted;
                                                            dtl.Rows.Add(dtrow);
                                                        }
                                                        dept_flag = true;
                                                    }
                                                    
                                                   
                                                }


                                                stud_s_no++;
                                                if (final_print_col_cnt == 9)
                                                {
                                                    


                                                    dtrow = dtl.NewRow();
                                                    dtrow[0] = stud_s_no + ". " + stud_name + "(" + roman_val + " Year)";
                                                    
                                                    dtl.Rows.Add(dtrow);
                                                }
                                                else
                                                {
                                                    if (final_print_col_cnt != 9 && check_print_or_nt == true)
                                                    {
                                                        
                                                    }
                                                    


                                                    dtrow = dtl.NewRow();
                                                    dtrow[0] = stud_s_no + ". " + stud_name + "(" + roman_val + " Year)";

                                                    dtl.Rows.Add(dtrow);
                                                }
                                                
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    set_sem_flag = true;
                                    if (temp_col == 0)
                                    {
                                        if (dept_flag == false)
                                        {
                                            check_print_or_nt = false;
                                            
                                            if (final_print_col_cnt == 9)
                                            {
                                                



                                                dtrow = dtl.NewRow();
                                                dtrow[0] = dept_name + tot_days_conducted;
                                                dtrow[1] = dept_name + tot_days_conducted;
                                                dtl.Rows.Add(dtrow);
                                            }
                                            else
                                            {
                                                


                                                dtrow = dtl.NewRow();
                                                dtrow[0] = dept_name + tot_days_conducted;
                                                dtrow[1] = dept_name + tot_days_conducted;
                                                dtl.Rows.Add(dtrow);
                                            }
                                            dept_flag = true;
                                        }

                                        
                                    }
                                    else
                                    {
                                        if (dept_flag == false)
                                        {
                                            check_print_or_nt = true;
                                            if (final_print_col_cnt == 9)
                                            {
                                                


                                                dtrow = dtl.NewRow();
                                                dtrow[0] = dept_name + tot_days_conducted;
                                                dtrow[1] = dept_name + tot_days_conducted;
                                                dtl.Rows.Add(dtrow);
                                            }
                                            else
                                            {
                                                
                                                dtrow = dtl.NewRow();
                                                dtrow[0] = dept_name + tot_days_conducted;
                                                dtrow[1] = dept_name + tot_days_conducted;
                                                dtl.Rows.Add(dtrow);
                                            }
                                            dept_flag = true;
                                        }
                                        
                                       
                                    }


                                    if (final_print_col_cnt == 9)
                                    {
                                        
                                        dtrow = dtl.NewRow();
                                        dtrow[0] = "** Attendance Till Not Be Update for " + "(" + roman_val + " Year)" + sem + " Semester **";
                                   
                                        dtl.Rows.Add(dtrow);

                                    }
                                    else
                                    {
                                        if (final_print_col_cnt != 9 && check_print_or_nt == true)
                                        {
                                            
                                        }
                                        

                                        dtrow = dtl.NewRow();
                                        dtrow[0] = "** Attendance Till Not Be Update for " + "(" + roman_val + " Year)" + sem + " Semester **";

                                        dtl.Rows.Add(dtrow);
                                    }
                                    

                                }
                            }

                            if (set_sem_flag == false)
                            {

                                set_sem_flag = true;
                                if (temp_col == 0)
                                {
                                    if (dept_flag == false)
                                    {
                                        check_print_or_nt = false;
                                        
                                        if (final_print_col_cnt == 9)
                                        {
                                           



                                            dtrow = dtl.NewRow();
                                            dtrow[0] = dept_name + tot_days_conducted;
                                            dtrow[1] = dept_name + tot_days_conducted;
                                            dtl.Rows.Add(dtrow);

                                        }
                                        else
                                        {
                                            


                                            dtrow = dtl.NewRow();
                                            dtrow[0] = dept_name + tot_days_conducted;
                                            dtrow[1] = dept_name + tot_days_conducted;
                                            dtl.Rows.Add(dtrow);
                                        }
                                        dept_flag = true;
                                    }

                                    
                                }
                                else
                                {
                                    if (dept_flag == false)
                                    {
                                        check_print_or_nt = true;
                                        if (final_print_col_cnt == 9)
                                        {
                                            


                                            dtrow = dtl.NewRow();
                                            dtrow[0] = dept_name + tot_days_conducted;
                                            dtrow[1] = dept_name + tot_days_conducted;
                                            dtl.Rows.Add(dtrow);
                                        }
                                        else
                                        {

                                            //    attnd_report.Sheets[0].SpanModel.Add(((attnd_report.Sheets[0].RowCount - tem) - 1), 0, 1, 4);
                                            dtrow = dtl.NewRow();
                                            dtrow[0] = dept_name + tot_days_conducted;
                                            dtrow[1] = dept_name + tot_days_conducted;
                                            dtl.Rows.Add(dtrow);


                                        }
                                        dept_flag = true;
                                    }
                                    
                                }


                                stud_s_no++;
                                if (final_print_col_cnt == 9)
                                {
                                    

                                    dtrow = dtl.NewRow();
                                    dtrow[0] = "**NILL**";
                                    
                                    dtl.Rows.Add(dtrow);
                                }
                                else
                                {

                                    if (final_print_col_cnt != 9 && check_print_or_nt == true)
                                    {
                                        
                                    }
                                   
                                    //attnd_report.Sheets[0].Cells[row_cnt, 4].Border.BorderColorBottom = Color.White;
                                    //attnd_report.Sheets[0].Cells[row_cnt, 4].Border.BorderColorTop = Color.White;
                                    
                                    //attnd_report.Sheets[0].SpanModel.Add((row_cnt), 4, 1, 5);


                                    dtrow = dtl.NewRow();
                                    dtrow[0] = "**NILL**";

                                    dtl.Rows.Add(dtrow);
                                }

                               

                            }///////////////////////////////
                        }

                    }

                }


            }
            if (Convert.ToInt32(dtl.Rows.Count) != 0)
            {
                Panel3.Visible = false;
                Double totalRows = 0;
                totalRows = Convert.ToInt32(dtl.Rows.Count);
                DropDownListpage.Items.Clear();
                if (totalRows >= 10)
                {
                    
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    
                    

                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    
                }
                else
                {
                    
                   
                    
                }
                if (Convert.ToInt32(dtl.Rows.Count) > 10)
                {
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    
                    
                }
                //Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
                Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];

            }
        }
        catch
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
    public void attndvalue()
    {
        try
        {

            eng_present = "";
            eng_absent = "";
            mng_present = "";
            mng_absent = "";
            tot_absent_student = "";

            mng_present_temp_string1 = "";
            mng_present_temp_str2 = "";
            mng_present_temp_str4 = "";
            mng_present_temp_str1 = "";
            mng_present_temp_str3 = "";
            int start = 0, end = 0;

            if (mng_holi == 1)// && evng_holi == 0)//===============mng wotkig, evng holi
            {
                for (int mng_hr = 1; mng_hr <= first_half_hr; mng_hr++)
                {
                    mng_present_temp_str1 = "(d" + Atday + "d" + mng_hr + " in(" + present_calcflag + ") or d" + Atday + "d" + mng_hr + " is null )";
                    mng_present_temp_str3 = " d" + Atday + "d" + mng_hr + " is not null";
                    if (mng_present_temp_str2 == "")
                    {
                        mng_present_temp_str2 = mng_present_temp_str1;
                        mng_present_temp_str4 = mng_present_temp_str3;
                    }
                    else
                    {
                        mng_present_temp_str2 = mng_present_temp_str2 + " and " + mng_present_temp_str1;
                        mng_present_temp_str4 = mng_present_temp_str4 + " or " + mng_present_temp_str3;
                    }
                }

                if (mng_present_temp_str2 != "")
                {
                    mng_present = " ( " + mng_present_temp_str2 + " ) and ( " + mng_present_temp_str4 + " ) ";//==========mng present
                }
                else
                {
                    mng_present = "";
                }

                //if (evng_holi == 0)
                //{
                //    eng_present = mng_present;
                //}
            }
            mng_present_temp_string1 = "";
            mng_present_temp_str2 = "";
            mng_present_temp_str4 = "";
            mng_present_temp_str1 = "";
            mng_present_temp_str3 = "";

            if (evng_holi == 1)// && mng_holi==0)//======================evng workin,mng leav
            {

                for (int mng_hr = first_half_hr + 1; mng_hr <= tot_hrs; mng_hr++)
                {
                    mng_present_temp_str1 = "(d" + Atday + "d" + mng_hr + " in(" + present_calcflag + ") or d" + Atday + "d" + mng_hr + " is null )";
                    mng_present_temp_str3 = " d" + Atday + "d" + mng_hr + " is not null";
                    if (mng_present_temp_str2 == "")
                    {
                        mng_present_temp_str2 = mng_present_temp_str1;
                        mng_present_temp_str4 = mng_present_temp_str3;
                    }
                    else
                    {
                        mng_present_temp_str2 = mng_present_temp_str2 + " and " + mng_present_temp_str1;
                        mng_present_temp_str4 = mng_present_temp_str4 + " or " + mng_present_temp_str3;
                    }
                }
                if (mng_present_temp_str2 != "")
                {
                    eng_present = " ( " + mng_present_temp_str2 + " ) and ( " + mng_present_temp_str4 + " ) ";//=====evng present
                }
                else
                {
                    eng_present = "";
                }
            }
            mng_present_temp_string1 = "";
            mng_present_temp_str2 = "";
            mng_present_temp_str4 = "";
            mng_present_temp_str1 = "";
            mng_present_temp_str3 = "";

            if (mng_holi == 1)//===============mng wotkig
            {
                for (int mng_hr = 1; mng_hr <= first_half_hr; mng_hr++)
                {
                    mng_present_temp_str1 = "  d" + Atday + "d" + mng_hr + " in(" + absent_calcflag + ") ";

                    if (mng_present_temp_str2 == "")
                    {
                        mng_present_temp_str2 = mng_present_temp_str1;

                    }
                    else
                    {
                        mng_present_temp_str2 = mng_present_temp_str2 + " or " + mng_present_temp_str1;

                    }
                }

                if (mng_present_temp_str2 != "")
                {
                    mng_absent = " ( " + mng_present_temp_str2 + " ) ";//======================mng absent
                }
                else
                {
                    mng_absent = "";
                }

                //if (evng_holi == 0)
                //{
                //    eng_absent = mng_absent;
                //}
            }

            mng_present_temp_string1 = "";
            mng_present_temp_str2 = "";
            mng_present_temp_str4 = "";
            mng_present_temp_str1 = "";
            mng_present_temp_str3 = "";

            if (evng_holi == 1)
            {
                for (int mng_hr = first_half_hr + 1; mng_hr <= tot_hrs; mng_hr++)
                {
                    mng_present_temp_str1 = "(d" + Atday + "d" + mng_hr + " in(" + absent_calcflag + "))";
                    if (mng_present_temp_str2 == "")
                    {
                        mng_present_temp_str2 = mng_present_temp_str1;
                    }
                    else
                    {
                        mng_present_temp_str2 = mng_present_temp_str2 + " or " + mng_present_temp_str1;
                    }
                }

                if (mng_present_temp_str2 != "")
                {
                    eng_absent = " ( " + mng_present_temp_str2 + " ) ";//======================evng absent
                }
                else
                {
                    eng_absent = "";
                }
                //if (mng_holi == 0)
                //{
                //    mng_absent = eng_absent;
                //}
            }
            if (mng_absent != "" && eng_absent != "")
            {
                tot_absent_student = " ( " + mng_absent + " or " + eng_absent + " )";//=================tot aabsent
            }
            else if (mng_absent == "" && eng_absent != "")
            {
                tot_absent_student = " ( " + eng_absent + " )";//=================tot aabsent
            }
            else if (mng_absent != "" && eng_absent == "")
            {
                tot_absent_student = " ( " + mng_absent + " )";//=================tot aabsent
            }
            else
            {
                tot_absent_student = "";
            }


            hat.Clear();
            hat.Add("monthyear", month_year);
            hat.Add("curr_sem", curr_sem);
            hat.Add("degree_code", deg_code);
            hat.Add("present_mng", mng_present);
            hat.Add("absent_mng", mng_absent);
            hat.Add("present_evng", eng_present);
            hat.Add("absent_evng", eng_absent);
            //  hat.Add("sections",
            //hat.Add("input_date", (DateTime .Parse(date_time.ToString("yyyy")+"-"+date_time .ToString("MM")+"-"+date_time.ToString("dd"))));
            hat.Add("tot_student", tot_absent_student);
            ds_count = dacces2.select_method("find_attnd_values", hat, "sp");
            morprenst = 0;
            eveprenst = 0;
            morabsent = 0;
            eveabsent = 0;
            morabsent = Convert.ToDouble(ds_count.Tables[2].Rows[0]["Count"].ToString());
            eveabsent = Convert.ToDouble(ds_count.Tables[0].Rows[0]["Count"].ToString());
            morprenst = Convert.ToDouble(ds_count.Tables[3].Rows[0]["Count"].ToString());
            eveprenst = Convert.ToDouble(ds_count.Tables[1].Rows[0]["Count"].ToString());
            if (holiday_flag == false)//=ful working day
            {
                if (eng_present != "" && mng_present != "")
                {
                    if (ds_count.Tables[1].Rows.Count > 0 && ds_count.Tables[3].Rows.Count > 0)
                    {

                        tot_present = double.Parse(((double.Parse(ds_count.Tables[1].Rows[0]["Count"].ToString()) / 2) + (double.Parse(ds_count.Tables[3].Rows[0]["Count"].ToString()) / 2)).ToString());
                    }
                }
                else if (eng_present == "" && mng_present != "")
                {
                    if (ds_count.Tables[3].Rows.Count > 0)
                    {
                        tot_present = ((double.Parse(ds_count.Tables[3].Rows[0]["Count"].ToString())) / 2);
                    }
                }
                else if (eng_present != "" && mng_present == "")
                {
                    if (ds_count.Tables[3].Rows.Count > 0)
                    {
                        morprenst = Convert.ToDouble(ds_count.Tables[1].Rows[0]["Count"].ToString());
                        eveprenst = Convert.ToDouble(ds_count.Tables[3].Rows[0]["Count"].ToString());
                        tot_present = ((double.Parse(ds_count.Tables[1].Rows[0]["Count"].ToString())) / 2);
                    }
                }
                else
                {
                    tot_present = 0;
                }
            }

            else// =======half day holiday
            {
                if (eng_present != "" && mng_present != "")
                {
                    if (ds_count.Tables[1].Rows.Count > 0 && ds_count.Tables[3].Rows.Count > 0)
                    {

                        tot_present = double.Parse(((double.Parse(ds_count.Tables[1].Rows[0]["Count"].ToString()) / 2) + (double.Parse(ds_count.Tables[3].Rows[0]["Count"].ToString()) / 2)).ToString());
                        tot_present += tot_present;
                    }
                }
                else if (eng_present == "" && mng_present != "")
                {
                    if (ds_count.Tables[3].Rows.Count > 0)
                    {
                        tot_present = ((double.Parse(ds_count.Tables[3].Rows[0]["Count"].ToString())) / 2);
                        tot_present += tot_present;
                    }
                }
                else if (eng_present != "" && mng_present == "")
                {
                    if (ds_count.Tables[3].Rows.Count > 0)
                    {
                        tot_present = ((double.Parse(ds_count.Tables[1].Rows[0]["Count"].ToString())) / 2);
                        tot_present += tot_present;
                    }
                }
                else
                {
                    tot_present = 0;
                }

            }


            if (holiday_flag == false)//=ful working day
            {

                if (mng_absent != "" && eng_absent != "")
                {
                    if (ds_count.Tables[2].Rows.Count > 0 && ds_count.Tables[0].Rows.Count > 0)
                    {

                        tot_absent = double.Parse(((double.Parse(ds_count.Tables[2].Rows[0]["Count"].ToString()) / 2) + (double.Parse(ds_count.Tables[0].Rows[0]["Count"].ToString()) / 2)).ToString());
                    }
                }
                else if (mng_absent == "" && eng_absent != "")
                {
                    if (ds_count.Tables[0].Rows.Count > 0)
                    {
                        tot_absent = double.Parse(((double.Parse(ds_count.Tables[0].Rows[0]["Count"].ToString()) / 2)).ToString());
                    }
                }
                else if (mng_absent != "" && eng_absent == "")
                {
                    if (ds_count.Tables[2].Rows.Count > 0)
                    {
                        tot_absent = double.Parse(((double.Parse(ds_count.Tables[2].Rows[0]["Count"].ToString()) / 2)).ToString());
                    }
                }
                else
                {
                    tot_absent = 0;
                }
            }
            else//======half day working day
            {
                if (mng_absent != "" && eng_absent != "")
                {
                    if (ds_count.Tables[2].Rows.Count > 0 && ds_count.Tables[0].Rows.Count > 0)
                    {
                        tot_absent = double.Parse(((double.Parse(ds_count.Tables[2].Rows[0]["Count"].ToString()) / 2) + (double.Parse(ds_count.Tables[0].Rows[0]["Count"].ToString()) / 2)).ToString());
                        tot_absent += tot_absent;
                    }
                }
                else if (mng_absent == "" && eng_absent != "")
                {
                    if (ds_count.Tables[0].Rows.Count > 0)
                    {
                        tot_absent = double.Parse(((double.Parse(ds_count.Tables[0].Rows[0]["Count"].ToString()) / 2)).ToString());
                        tot_absent += tot_absent;
                    }
                }
                else if (mng_absent != "" && eng_absent == "")
                {
                    if (ds_count.Tables[2].Rows.Count > 0)
                    {
                        tot_absent = double.Parse(((double.Parse(ds_count.Tables[2].Rows[0]["Count"].ToString()) / 2)).ToString());
                        tot_absent += tot_absent;
                    }
                }
                else
                {
                    tot_absent = 0;
                }
            }



            if (tot_absent_student != "")
            {
                if (ds_count.Tables[4].Rows.Count > 0)
                {
                    for (stud_count = 0; stud_count < ds_count.Tables[4].Rows.Count; stud_count++)
                    {
                        findvalues();
                        per_con_hrs = ((per_workingdays * tot_hrs) - per_dum_unmark);
                        per_tage_hrs = ((per_per_hrs / per_con_hrs) * 100);

                        if (per_tage_hrs > 100)
                        {
                            per_tage_hrs = 100;
                        }
                        dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));

                        if (dum_tage_date == "NaN")
                        {
                            dum_tage_date = "0";
                        }
                        else if (dum_tage_date == "Infinity")
                        {
                            dum_tage_date = "0";
                        }
                        if (!hat_ind_stud_list.ContainsKey(acronym + "/" + ds_count.Tables[4].Rows[stud_count]["roll_no"].ToString() + "/" + curr_sem))
                        {
                            hat_ind_stud_list.Add(acronym + "/" + ds_count.Tables[4].Rows[stud_count]["roll_no"].ToString() + "/" + curr_sem, ds_count.Tables[4].Rows[stud_count]["roll_no"].ToString() + "-" + ds_count.Tables[4].Rows[stud_count]["stud_name"].ToString() + " (" + dum_tage_date + ")");
                        }

                    }
                }
            }
            if (deptflag == false)
            {
                hat_dept_wh.Add(acronym, "(" + per_workingdays + ")");
                deptflag = true;
            }
        }
        catch
        {
        }
    }
    public void findvalues()
    {
        try
        {
            int mmyycount, moncount = 0, next;
            ////-------------------------------------------

            string dt = start_date.ToString();
            frdate = start_date.ToString("yyy") + "/" + start_date.ToString("MM") + "/" + start_date.ToString("dd");
            demfcal = int.Parse(start_date.ToString("yyy"));
            demfcal = demfcal * 12;
            cal_from_date = demfcal + int.Parse(start_date.ToString("MM"));
            dt = date_time.ToString();
            todate = date_time.ToString("yyy") + "/" + date_time.ToString("MM") + "/" + date_time.ToString("dd");
            demtcal = int.Parse(date_time.ToString("yyy"));
            demtcal = demtcal * 12;
            cal_to_date = demtcal + int.Parse(date_time.ToString("MM"));
            per_from_date = Convert.ToDateTime(frdate);
            per_to_date = Convert.ToDateTime(todate);
            dumm_from_date = per_from_date;


            hat.Clear();
            hat.Add("@degree_code ", deg_code);
            hat.Add("@sem", curr_sem);
            hat.Add("@from_date", per_from_date);
            hat.Add("@to_date", per_to_date);
            hat.Add("@coll_code", Session["InternalCollegeCode"].ToString());


            int iscount = 0;
            holidaycon.Close();
            holidaycon.Open();
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + per_from_date.ToString() + "' and '" + per_to_date.ToString() + "' and degree_code=" + deg_code + " and semester=" + curr_sem + "";
            SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
            SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
            DataSet dsholiday = new DataSet();
            daholiday.Fill(dsholiday);
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            hat.Add("iscount", iscount);

            ds_holi = dacces2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
            ////--------------------------------------------------


            hat.Clear();
            hat.Add("std_rollno", ds_count.Tables[4].Rows[stud_count]["roll_no"].ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds_stud = dacces2.select_method("STUD_ATTENDANCE", hat, "sp");

            mmyycount = ds_stud.Tables[0].Rows.Count;
            moncount = mmyycount - 1;

            if (ds_holi.Tables[0].Rows.Count > 0)
            {
                ts = DateTime.Parse(ds_holi.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                next = 0;
                if (ds_stud.Tables[0].Rows.Count != 0)
                {
                    int rowcount = 0;
                    int ccount;
                    ccount = ds_holi.Tables[1].Rows.Count;
                    ccount = ccount - 1;

                    while (dumm_from_date <= (per_to_date))
                    {
                        for (int i = 1; i <= mmyycount; i++)
                        {
                            if (cal_from_date == int.Parse(ds_stud.Tables[0].Rows[next]["month_year"].ToString()))
                            {
                                if (dumm_from_date != DateTime.Parse(ds_holi.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()))
                                {
                                    ts = DateTime.Parse(ds_holi.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                    diff_date = Convert.ToString(ts.Days);
                                    for (i = 1; i <= first_half_hr; i++)
                                    {
                                        date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                        value = ds_stud.Tables[0].Rows[next][date].ToString();

                                        if (value != null && value != "0" && value != "7" && value != "")
                                        {
                                            if (tempvalue != value)
                                            {
                                                tempvalue = value;
                                                for (int j = 0; j < count_master; j++)
                                                {

                                                    if (ds_attndmaster.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                    {
                                                        ObtValue = int.Parse(ds_attndmaster.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                        j = count_master;
                                                    }
                                                }
                                            }
                                            if (ObtValue == 1)
                                            {
                                                per_abshrs += 1;
                                            }
                                            else if (ObtValue == 2)
                                            {

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

                                    if (per_perhrs >= minpresI)
                                    {
                                        Present += 0.5;
                                    }

                                    else if (per_leave >= minpresI)
                                    {
                                        leave_point += leave_pointer / 2;
                                    }

                                    else if (per_abshrs >= 1)
                                    {
                                        Absent += 0.5;
                                        absent_point += absent_pointer / 2;
                                    }

                                    if (per_ondu >= 1)
                                    {
                                        Onduty += 0.5;
                                    }

                                    if (njhr >= minpresI)
                                    {
                                        njdate += 0.5;

                                    }
                                    if (per_leave >= 1)
                                    {
                                        Leave += 0.5;
                                    }
                                    per_perhrs = 0;
                                    per_ondu = 0;
                                    per_leave = 0;
                                    per_abshrs = 0;
                                    // unmark = 0;
                                    njhr = 0;

                                    int k = i;
                                    for (i = k; i <= tot_hrs; i++)
                                    {
                                        date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                        value = ds_stud.Tables[0].Rows[next][date].ToString();

                                        if (value != null && value != "0" && value != "7" && value != "")
                                        {
                                            if (tempvalue != value)
                                            {
                                                tempvalue = value;
                                                for (int j = 0; j < count_master; j++)
                                                {

                                                    if (ds_attndmaster.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                    {
                                                        ObtValue = int.Parse(ds_attndmaster.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                        j = count_master;
                                                    }
                                                }
                                            }
                                            if (ObtValue == 1)
                                            {
                                                per_abshrs += 1;
                                            }
                                            else if (ObtValue == 2)
                                            {
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
                                    if (per_perhrs >= minpresII)
                                    {
                                        Present += 0.5;
                                    }

                                    else if (per_leave >= minpresII)
                                    {

                                        leave_point += leave_pointer / 2;
                                    }
                                    else if (per_abshrs >= 1)
                                    {
                                        Absent += 0.5;
                                        absent_point += absent_pointer / 2;
                                    }
                                    if (unmark == tot_hrs)
                                    {
                                        per_holidate += 1;
                                        unmark = 0;
                                    }
                                    else
                                    {
                                        dum_unmark += unmark;
                                    }
                                    if (per_ondu >= 1)
                                    {
                                        Onduty += 0.5;
                                    }

                                    if (njhr >= minpresII)
                                    {

                                        njdate += 0.5;
                                    }
                                    if (per_leave >= 1)
                                    {
                                        Leave += 0.5;
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

                                    workingdays += 1;
                                    per_perhrs = 0;

                                }
                                else
                                {
                                    workingdays += 1;
                                    dumm_from_date = dumm_from_date.AddDays(1);
                                    per_holidate += 1;

                                }
                            }
                            else
                            {
                                if (dumm_from_date.Day == 1)
                                {
                                    cal_from_date++;
                                }
                                dumm_from_date = dumm_from_date.AddDays(1);
                                if (moncount > next)
                                {
                                    i--;
                                }
                            }

                        }
                    }
                    int diff_Date = per_from_date.Day - dumm_from_date.Day;
                }

            }
            else
            {
                next = 0;

                if (ds_stud.Tables[0].Rows.Count != 0)
                {
                    int rowcount = 0;
                    int ccount;
                    ccount = ds_holi.Tables[1].Rows.Count;
                    ccount = ccount - 1;
                    //if ( == ds2.Tables [0].Rows [mmyycount].["Month_year"])
                    while (dumm_from_date <= (per_to_date))
                    {
                        for (int i = 1; i <= mmyycount; i++)
                        {
                            if (cal_from_date == int.Parse(ds_stud.Tables[0].Rows[next]["month_year"].ToString()))
                            {

                                for (i = 1; i <= first_half_hr; i++)
                                {
                                    date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                    value = ds_stud.Tables[0].Rows[next][date].ToString();

                                    if (value != null && value != "0" && value != "7" && value != "")
                                    {
                                        if (tempvalue != value)
                                        {
                                            tempvalue = value;
                                            for (int j = 0; j < count_master; j++)
                                            {

                                                if (ds_attndmaster.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                {
                                                    ObtValue = int.Parse(ds_attndmaster.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                    j = count_master;
                                                }
                                            }
                                        }
                                        if (ObtValue == 1)
                                        {
                                            per_abshrs += 1;
                                        }
                                        else if (ObtValue == 2)
                                        {

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

                                if (per_perhrs >= minpresI)
                                {
                                    Present += 0.5;
                                }

                                else if (per_leave >= minpresI)
                                {
                                    leave_point += leave_pointer / 2;
                                }

                                else if (per_abshrs >= 1)
                                {
                                    Absent += 0.5;
                                    absent_point += absent_pointer / 2;
                                }

                                if (per_ondu >= 1)
                                {
                                    Onduty += 0.5;
                                }

                                if (njhr >= minpresI)
                                {
                                    njdate += 0.5;

                                }
                                if (per_leave >= 1)
                                {
                                    Leave += 0.5;
                                }
                                per_perhrs = 0;
                                per_ondu = 0;
                                per_leave = 0;
                                per_abshrs = 0;
                                // unmark = 0;
                                njhr = 0;

                                int k = i;
                                for (i = k; i <= tot_hrs; i++)
                                {
                                    date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                    value = ds_stud.Tables[0].Rows[next][date].ToString();

                                    if (value != null && value != "0" && value != "7" && value != "")
                                    {
                                        if (tempvalue != value)
                                        {
                                            tempvalue = value;
                                            for (int j = 0; j < count_master; j++)
                                            {

                                                if (ds_attndmaster.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                {
                                                    ObtValue = int.Parse(ds_attndmaster.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                    j = count_master;
                                                }
                                            }
                                        }
                                        if (ObtValue == 1)
                                        {
                                            per_abshrs += 1;
                                        }
                                        else if (ObtValue == 2)
                                        {
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
                                if (per_perhrs >= minpresII)
                                {
                                    Present += 0.5;
                                }

                                else if (per_leave >= minpresII)
                                {

                                    leave_point += leave_pointer / 2;
                                }
                                else if (per_abshrs >= 1)
                                {
                                    Absent += 0.5;
                                    absent_point += absent_pointer / 2;
                                }
                                if (unmark == tot_hrs)
                                {
                                    per_holidate += 1;
                                    unmark = 0;
                                }
                                else
                                {
                                    dum_unmark += unmark;
                                }
                                if (per_ondu >= 1)
                                {
                                    Onduty += 0.5;
                                }

                                if (njhr >= minpresII)
                                {

                                    njdate += 0.5;
                                }
                                if (per_leave >= 1)
                                {
                                    Leave += 0.5;
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

                                workingdays += 1;
                                per_perhrs = 0;

                            }
                            else
                            {
                                if (dumm_from_date.Day == 1)
                                {
                                    cal_from_date++;
                                }
                                dumm_from_date = dumm_from_date.AddDays(1);
                                if (moncount > next)
                                {
                                    i--;
                                }
                            }

                        }

                    }

                }
            }

            per_tot_ondu = tot_ondu;
            per_njdate = njdate;
            pre_present_date = Present;
            per_per_hrs = tot_per_hrs;
            per_absent_date = Absent;
            pre_ondu_date = Onduty;
            pre_leave_date = Leave;
            per_workingdays = workingdays - per_holidate - per_njdate;
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
        catch
        {
        }
    }


    //void CalculateTotalPages()
    //{
    //    Double totalRows = 0;
    //    totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
    //    Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
    //    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //    Buttontotal.Visible = true;
    //}
    //protected void pageddltxt_TextChanged(object sender, EventArgs e)
    //{
    //    errmsg.Visible = false;

    //    attnd_report.CurrentPage = 0;
    //    pagesearch_txt.Text = "";
    //    try
    //    {
    //        if (pageddltxt.Text != string.Empty)
    //        {
    //            if (attnd_report.Sheets[0].RowCount >= Convert.ToInt16(pageddltxt.Text.ToString()) && Convert.ToInt16(pageddltxt.Text.ToString()) != 0)
    //            {
    //                attnd_report.Sheets[0].PageSize = Convert.ToInt16(pageddltxt.Text.ToString());
    //                errmsg.Visible = false;
    //                CalculateTotalPages();
    //            }
    //            else
    //            {
    //                errmsg.Visible = true;
    //                errmsg.Text = "Please Enter valid Record count";
    //                pageddltxt.Text = "";
    //            }
    //        }
    //    }
    //    catch
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = "Please Enter valid Record count";
    //        pageddltxt.Text = "";
    //    }
    //}

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        

        pagesearch_txt.Text = "";
        errmsg.Visible = false;
        pagesearch_txt.Text = "";
        pageddltxt.Text = "";
        pageddltxt.Text = "";
        if (DropDownListpage.SelectedItem.ToString() == "Others")
        {

            pageddltxt.Visible = true;
            pageddltxt.Focus();

        }
        else
        {
            pageddltxt.Visible = false;
            
            
        }
    }

    protected void pagesearch_txt_TextChanged(object sender, EventArgs e)
    {

        errmsg.Visible = false;
        if (pagesearch_txt.Text.Trim() != "")
        {
            if (Convert.ToInt64(pagesearch_txt.Text) > Convert.ToInt64(Session["totalPages"]))
            {
                errmsg.Visible = true;
                errmsg.Text = "Exceed The Page Limit";
                
                Showgrid.Visible = true;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;
                //Button1.Visible = true;
                btnxl.Visible = true;
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
                pagesearch_txt.Text = "";
            }
            else if (Convert.ToInt64(pagesearch_txt.Text) == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "Page search should be more than 0";
                
                Showgrid.Visible = true;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;
                //Button1.Visible = true;
                btnxl.Visible = true;
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
                pagesearch_txt.Text = "";
            }

            else
            {
                errmsg.Visible = false;
                
                Showgrid.Visible = true;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;
                //Button1.Visible = true;
                btnxl.Visible = true;
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
            }
        }
    }

    public string sem_roman(int sem)
    {
        string sql = "";
        string sem_roman = "";
        SqlDataReader rsChkSet;
        con1.Close();
        con1.Open();
        sql = "select * from inssettings where college_code=" + Session["InternalCollegeCode"] + " and LinkName ='Semester Display'";
        cmd1 = new SqlCommand(sql, con1);
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
    public void GiveCourseName(string deg_code, out string course_value, out string course_namevalue)
    {
        string course_val = "";
        string course_name = "";
        SqlDataReader RsCName;
        con2.Close();
        con2.Open();
        cmd2 = new SqlCommand("select distinct ltrim(Dept_acronym) as CName,dept_name from Course,Department,Degree where  Degree.Course_Id = Course.Course_Id And Department.Dept_Code = Degree.Dept_Code  and Degree.Degree_Code = " + deg_code + " ", con2);
        RsCName = cmd2.ExecuteReader();
        RsCName.Read();
        if (RsCName.HasRows == true)
        {
            if (RsCName["CName"].ToString() != "")
            {
                course_val = RsCName["CName"].ToString();
                course_name = RsCName["dept_name"].ToString();
            }
        }
        course_value = course_val;
        course_namevalue = course_name;
        // return course_val;
    }




    //protected override void Render(System.Web.UI.HtmlTextWriter writer)
    //{
    //    Control cntPageNextBtn = attnd_report.FindControl("Next");
    //    Control cntPagePreviousBtn = attnd_report.FindControl("Prev");

    //    if ((cntPageNextBtn != null))
    //    {

    //        TableCell tc = (TableCell)cntPageNextBtn.Parent;
    //        TableRow tr = (TableRow)tc.Parent;

    //        tr.Cells.Remove(tc);
    //        tc = (TableCell)cntPagePreviousBtn.Parent;
    //        tr.Cells.Remove(tc);
    //    }

    //    base.Render(writer);
    //}
    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        getcon.Close();
        getcon.Open();
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr, getcon);
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

    protected void datetxt_TextChanged(object sender, EventArgs e)
    {
        errlbl.Visible = false;
        errmsg.Visible = false;
        
        Showgrid.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Button1.Visible = false;
        Panel3.Visible = false;
        btnxl.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        //pagesetpanel.Visible = false;
    }

    //=============Hided by Manikandan 20/05/2013
    //public void setheader_print()
    //{
    //    try
    //    {
    //        // attnd_report.Sheets[0].RemoveSpanCell
    //        //================header
    //        temp_count = 0;


    //        MyImg mi = new MyImg();
    //        mi.ImageUrl = "~/images/10BIT001.jpeg";
    //        mi.ImageUrl = "Handler/Handler2.ashx?";
    //        MyImg mi2 = new MyImg();
    //        mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //        mi2.ImageUrl = "Handler/Handler5.ashx?";

    //        if (final_print_col_cnt == 1)
    //        {
    //            for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    // one_column();
    //                    //more_column();
    //                    break;
    //                }
    //            }

    //        }

    //        else if (final_print_col_cnt == 2)
    //        {
    //            for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 0)
    //                    {
    //                        start_column = col_count;
    //                        //   attnd_report.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 1);
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else
    //                    {
    //                        //  one_column();
    //                        //more_column();
    //                        for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount - 1; row_cnt++)
    //                        {
    //                            attnd_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        }
    //                    }
    //                    temp_count++;
    //                    if (temp_count == 2)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        else if (final_print_col_cnt == 3)
    //        {
    //            for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 0)
    //                    {
    //                        start_column = col_count;
    //                        //   attnd_report.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 1);
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else if (temp_count == 1)
    //                    {
    //                        // one_column();
    //                        //more_column();
    //                        for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount - 1; row_cnt++)
    //                        {
    //                            attnd_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                            attnd_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                        }
    //                    }
    //                    else if (temp_count == 2)
    //                    {
    //                        //--------------------ISO CODE 13/6/12 PRABAH
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Text = "ISO CODE:" + isonumber;
    //                        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, (attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[1, col_count].CellType = mi2;
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorRight = Color.Black;
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.Black;
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //                    }
    //                    temp_count++;
    //                    if (temp_count == 3)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }

    //        }
    //        else//-----------column count more than 3
    //        {
    //            for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 0)
    //                    {
    //                        start_column = col_count;
    //                        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 1);
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                        // attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //                    }

    //                    end_column = col_count;

    //                    temp_count++;
    //                    if (final_print_col_cnt == temp_count)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //            //--------------ISO 13/6/12 PRABHA
    //            if (isonumber != string.Empty)
    //            {
    //                attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Text = "ISO CODE:";// +isonumber;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Text = isonumber;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].HorizontalAlign = HorizontalAlign.Left;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //                attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(1, end_column, (attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                attnd_report.Sheets[0].ColumnHeader.Cells[1, end_column].CellType = mi2;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorRight = Color.Black;
    //                attnd_report.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorTop = Color.White;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorRight = Color.Black;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorBottom = Color.White;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[1, end_column - 1].Border.BorderColorTop = Color.White;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorRight = Color.White;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //            }
    //            else
    //            {
    //                attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 1);
    //                attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //                attnd_report.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorRight = Color.Black;
    //            }
    //            //attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 1);
    //            //attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //            //attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //            //----------------------------------  

    //            temp_count = 0;
    //            for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 1)
    //                    {
    //                        //more_column();
    //                        for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount - 1; row_cnt++)
    //                        {
    //                            attnd_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                            attnd_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                        }
    //                    }
    //                    temp_count++;
    //                }
    //            }
    //        }
    //        //=========================

    //    }
    //    catch
    //    {
    //    }

    //}
    ////===========================================================================

    //public void footer_set()
    //{
    //    try
    //    {
    //        //2.Footer setting

    //        if (dsprint.Tables[0].Rows.Count > 0)
    //        {
    //            if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
    //            {
    //                attnd_report.Sheets[0].Rows[(attnd_report.Sheets[0].RowCount - 1)].Border.BorderColorBottom = Color.Black;
    //                footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
    //                attnd_report.Sheets[0].RowCount = attnd_report.Sheets[0].RowCount + 3;

    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 3), start_column].ColumnSpan = attnd_report.Sheets[0].ColumnCount - start_column;
    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), start_column].ColumnSpan = attnd_report.Sheets[0].ColumnCount - start_column;

    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 3), start_column].Border.BorderColorBottom = Color.White;
    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), start_column].Border.BorderColorTop = Color.White;
    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), start_column].Border.BorderColorBottom = Color.White;
    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), start_column].Border.BorderColorTop = Color.White;


    //                footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //                string[] footer_text_split = footer_text.Split(',');
    //                footer_text = "";




    //                if (final_print_col_cnt < footer_count)
    //                {
    //                    for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
    //                    {
    //                        if (footer_text == "")
    //                        {
    //                            footer_text = footer_text_split[concod_footer].ToString();
    //                        }
    //                        else
    //                        {
    //                            footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
    //                        }
    //                    }

    //                    for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //                        {
    //                            attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                            break;
    //                        }
    //                    }

    //                }

    //                else if (final_print_col_cnt == footer_count)
    //                {
    //                    temp_count = 0;
    //                    for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //                        {
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                            temp_count++;
    //                            if (temp_count == footer_count)
    //                            {
    //                                break;
    //                            }
    //                        }
    //                    }

    //                }

    //                else
    //                {
    //                    temp_count = 0;
    //                    split_col_for_footer = final_print_col_cnt / footer_count;
    //                    footer_balanc_col = final_print_col_cnt % footer_count;

    //                    for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //                        {
    //                            if (temp_count == 0)
    //                            {
    //                                attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                            }
    //                            else
    //                            {

    //                                attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

    //                            }
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                            if (col_count - 1 > 0)
    //                            {
    //                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                            }

    //                            if (col_count == 0)
    //                            {
    //                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.Black;
    //                            }
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                            if (col_count + 1 < attnd_report.Sheets[0].ColumnCount)
    //                            {
    //                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
    //                            }

    //                            if ((split_col_for_footer + footer_balanc_col) == final_print_col_cnt)
    //                            {
    //                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.Black;
    //                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.Black;
    //                                // attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), col_count, 1, col_count - start_column);
    //                            }

    //                            temp_count++;
    //                            if (temp_count == 0)
    //                            {
    //                                col_count = col_count + split_col_for_footer + footer_balanc_col;
    //                            }
    //                            else
    //                            {
    //                                col_count = col_count + split_col_for_footer;
    //                            }



    //                            if (temp_count == footer_count)
    //                            {
    //                                break;
    //                            }
    //                        }
    //                    }
    //                }



    //            }
    //        }

    //        //2 end.Footer setting
    //    }
    //    catch
    //    {
    //    }
    //}
    //public void more_column()
    //{

    //    try
    //    {
    //        header_text();


    //        ////================================
    //        //if (hat_dept_wh.Count > 0)
    //        //{


    //        //    attnd_report.Sheets[0].Cells[stud_list_row_val, col_count].Text = "Absent Students List";
    //        //    attnd_report.Sheets[0].Cells[stud_list_row_val, col_count].Font.Size = FontUnit.Larger;
    //        //    attnd_report.Sheets[0].Cells[stud_list_row_val, col_count].Font.Bold = true;
    //        //    attnd_report.Sheets[0].Cells[stud_list_row_val, col_count].Font.Size = FontUnit.Medium;
    //        //    attnd_report.Sheets[0].Cells[stud_list_row_val, col_count].HorizontalAlign = HorizontalAlign.Center;
    //        //    attnd_report.Sheets[0].SpanModel.Add(stud_list_row_val, col_count, 2, end_column - col_count);

    //        //}
    //        ////=======================================

    //        if (final_print_col_cnt > 3)
    //        {
    //            if (isonumber != string.Empty)
    //            {
    //                attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count - 1));
    //            }
    //            else
    //            {
    //                attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count));
    //            }
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count));
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count));
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count));
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count));
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count));
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count));
    //        }
    //        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //        //  attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, final_print_col_cnt - 2);

    //        attnd_report.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;

    //        attnd_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;

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

    //        attnd_report.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;

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

    //        attnd_report.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;

    //        if (form_name != "" && form_name != null)
    //        {
    //            attnd_report.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------";
    //        }

    //        attnd_report.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;

    //        string dt = DateTime.Today.ToShortDateString();
    //        string[] dsplit = dt.Split(new Char[] { '/' });
    //        attnd_report.Sheets[0].ColumnHeader.Cells[6, col_count].Text = "Attendance Date: " + datetxt.Text + " Date On: " + dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();

    //        attnd_report.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;



    //        int temp_count_temp = 0;
    //        if (dsprint.Tables[0].Rows.Count > 0)
    //        {
    //            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //            {

    //                string[] new_header_string_index_split = new_header_string_index.Split(',');

    //                new_header_string_split = (dsprint.Tables[0].Rows[0]["new_header_name"].ToString()).Split(',');
    //                attnd_report.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;
    //                for (int row_head_count = 7; row_head_count < (7 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //                {
    //                    attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Text = new_header_string_split[temp_count_temp].ToString();
    //                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, col_count, 1, (end_column - col_count));
    //                    attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorTop = Color.White;
    //                    if (row_head_count != (7 + new_header_string_split.GetUpperBound(0)))
    //                    {
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorBottom = Color.White;
    //                    }

    //                    if (temp_count_temp <= new_header_string_index_split.GetUpperBound(0))
    //                    {
    //                        header_alignment = new_header_string_index_split[temp_count_temp].ToString();
    //                        if (header_alignment != string.Empty)
    //                        {
    //                            if (header_alignment == "2")
    //                            {
    //                                attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Center;
    //                            }
    //                            else if (header_alignment == "1")
    //                            {
    //                                attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Left;
    //                            }
    //                            else
    //                            {
    //                                attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Right;
    //                            }
    //                        }
    //                    }

    //                    temp_count_temp++;
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    public void header_text()
    {

        Boolean check_print_row = false;

        SqlDataReader dr_collinfo;
        con.Close();
        con.Open();
        cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header from print_master_setting  where form_name='overalldailyattndreport.aspx'", con);
        dr_collinfo = cmd.ExecuteReader();
        while (dr_collinfo.Read())
        {
            if (dr_collinfo.HasRows == true)
            {
                check_print_row = true;
                coll_name = dr_collinfo["collname"].ToString();
                address1 = dr_collinfo["address1"].ToString();
                address2 = dr_collinfo["address2"].ToString();
                address3 = dr_collinfo["address3"].ToString();
                phoneno = dr_collinfo["phoneno"].ToString();
                faxno = dr_collinfo["faxno"].ToString();
                email = dr_collinfo["email"].ToString();
                website = dr_collinfo["website"].ToString();
                form_name = dr_collinfo["form_name"].ToString();
                degree_deatil = dr_collinfo["degree_deatil"].ToString();
                header_alignment = dr_collinfo["header_alignment"].ToString();
                view_header = dr_collinfo["view_header"].ToString();
            }

        }
        if (check_print_row == false)
        {

            con.Close();
            con.Open();
            cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["InternalCollegeCode"] + "", con);
            dr_collinfo = cmd.ExecuteReader();
            while (dr_collinfo.Read())
            {
                if (dr_collinfo.HasRows == true)
                {

                    string sec_val = "";



                    check_print_row = true;
                    coll_name = dr_collinfo["collname"].ToString();
                    address1 = dr_collinfo["address1"].ToString();
                    address2 = dr_collinfo["address2"].ToString();
                    address3 = dr_collinfo["address3"].ToString();
                    phoneno = dr_collinfo["phoneno"].ToString();
                    faxno = dr_collinfo["faxno"].ToString();
                    email = dr_collinfo["email"].ToString();
                    website = dr_collinfo["website"].ToString();
                    form_name = "Overall Daily Attendance Report";

                    // header_alignment = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                    // view_header = dr_collinfo["view_header"].ToString();
                }

            }
        }
    }


    //protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    //{
    //    attnd_report.Sheets[0].Rows[0].Visible = true;
    //    attnd_report.Sheets[0].Rows[1].Visible = true;
    //    attnd_report.Sheets[0].Rows[2].Visible = true;
    //    attnd_report.Sheets[0].Rows[3].Visible = true;
    //    attnd_report.Sheets[0].Rows[4].Visible = true;
    //    attnd_report.Sheets[0].Rows[5].Visible = true;
    //    attnd_report.Sheets[0].Rows[6].Visible = true;
    //    attnd_report.Sheets[0].Rows[7].Visible = true;

    //    int i = 0;
    //    ddlpage.Items.Clear();
    //    int totrowcount = attnd_report.Sheets[0].RowCount;
    //    int pages = totrowcount / 25;
    //    int intialrow = 1;
    //    int remainrows = totrowcount % 25;
    //    if (attnd_report.Sheets[0].RowCount > 0)
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
    //    if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //    {
    //        for (i = 0; i < attnd_report.Sheets[0].RowCount; i++)
    //        {
    //            attnd_report.Sheets[0].Rows[i].Visible = true;
    //        }
    //        Double totalRows = 0;
    //        totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
    //        Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
    //        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //        DropDownListpage.Items.Clear();
    //        if (totalRows >= 10)
    //        {
    //            attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //            {
    //                DropDownListpage.Items.Add((k + 10).ToString());
    //            }
    //            DropDownListpage.Items.Add("Others");
    //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            attnd_report.Height = 335;

    //        }
    //        else if (totalRows == 0)
    //        {
    //            DropDownListpage.Items.Add("0");
    //            attnd_report.Height = 100;
    //        }
    //        else
    //        {
    //            attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());
    //            attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //        }
    //        if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
    //        {
    //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //            //   attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
                
    //        }


    //        Panel3.Visible = false;


    //    }
    //    else
    //    {

    //        errlbl.Visible = false;
    //        Panel3.Visible = false;
    //    }
    //}
    //protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    //{
    //    int i = 0;
    //    errlbl.Visible = false;
    //    ddlpage.Items.Clear();
    //    int totrowcount = attnd_report.Sheets[0].RowCount;
    //    int pages = totrowcount / 25;
    //    int intialrow = 1;
    //    int remainrows = totrowcount % 25;
    //    if (attnd_report.Sheets[0].RowCount > 0)
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
    //    if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //    {
    //        for (i = 0; i < attnd_report.Sheets[0].RowCount; i++)
    //        {
    //            attnd_report.Sheets[0].Rows[i].Visible = true;
    //        }
    //        Double totalRows = 0;
    //        totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
    //        Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
    //        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //        DropDownListpage.Items.Clear();
    //        if (totalRows >= 10)
    //        {
    //            attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //            {
    //                DropDownListpage.Items.Add((k + 10).ToString());
    //            }
    //            DropDownListpage.Items.Add("Others");
    //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            attnd_report.Height = 335;

    //        }
    //        else if (totalRows == 0)
    //        {
    //            DropDownListpage.Items.Add("0");
    //            attnd_report.Height = 100;
    //        }
    //        else
    //        {
    //            attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());
    //            attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //        }
    //        if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
    //        {
    //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //            //  attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
                
    //        }
    //        Panel3.Visible = false;
    //    }
    //    else
    //    {
    //        Panel3.Visible = false;
    //    }
    //}
    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {

                d2.printexcelreportgrid(Showgrid, reportname);
                lblerr.Visible = false;
            }
            else
            {
                lblerr.Text = "Please Enter Your Report Name";
                lblerr.Visible = true;
            }
        }
        catch (Exception ex)
        {
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
        //        print = "Overall Daily Attendance Report" + i;
        //        //attnd_report.SaveExcel(appPath + "/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
        //        //Aruna on 26feb2013============================
        //        string szPath = appPath + "/Report/";
        //        string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

        //        attnd_report.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
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
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string subcolumntext = "";
            Boolean child_flag = false;

            Session["page_redirect_value"] = datetxt.Text + "," + ddlcollege.SelectedIndex.ToString();

            gobtn_Click(sender, e);

            // if (tofromlbl.Visible == false)
            {
                lblpages.Visible = true;
                ddlpage.Visible = true;
                string clmnheadrname = "";
                //int total_clmn_count = attnd_report.Sheets[0].ColumnCount;


                //for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
                //{
                //    if (attnd_report.Sheets[0].Columns[srtcnt].Visible == true)
                //    {
                //        if (attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text != "")
                //        {
                //            subcolumntext = "";
                //            if (clmnheadrname == "")
                //            {
                //                clmnheadrname = attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
                //            }
                //            else
                //            {
                //                if (child_flag == false)
                //                {
                //                    clmnheadrname = clmnheadrname + "," + attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
                //                }
                //                else
                //                {
                //                    clmnheadrname = clmnheadrname + "$)," + attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
                //                }

                //            }
                //            child_flag = false;
                //        }
                //    }
                //}
                Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "overalldailyattndreport.aspx" + ":" + "Overall Daily Attendance Report");
            }
        }
        catch
        {
        }
    }

    //public void view_header_setting()
    //{
    //    try
    //    {
    //        if (dsprint.Tables[0].Rows.Count > 0)
    //        {

    //            ddlpage.Visible = true;
    //            lblpages.Visible = true;

    //            view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
    //            view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
    //            view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //            if (view_header == "0" || view_header == "1")
    //            {
    //                errmsg.Visible = false;

    //                for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
    //                {
    //                    attnd_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
    //                }

    //                int i = 0;
    //                ddlpage.Items.Clear();
    //                int totrowcount = attnd_report.Sheets[0].RowCount;
    //                int pages = totrowcount / 25;
    //                int intialrow = 1;
    //                int remainrows = totrowcount % 25;
    //                if (attnd_report.Sheets[0].RowCount > 0)
    //                {
    //                    int i5 = 0;
    //                    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    //                    for (i = 1; i <= pages; i++)
    //                    {
    //                        i5 = i;

    //                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //                        intialrow = intialrow + 25;
    //                    }
    //                    if (remainrows > 0)
    //                    {
    //                        i = i5 + 1;
    //                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //                    }
    //                }
    //                if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //                {
    //                    for (i = 0; i < attnd_report.Sheets[0].RowCount; i++)
    //                    {
    //                        attnd_report.Sheets[0].Rows[i].Visible = true;
    //                    }
    //                    Double totalRows = 0;
    //                    totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
    //                    Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
    //                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //                    DropDownListpage.Items.Clear();
    //                    if (totalRows >= 10)
    //                    {
    //                        attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //                        {
    //                            DropDownListpage.Items.Add((k + 10).ToString());
    //                        }
    //                        DropDownListpage.Items.Add("Others");
    //                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //                        attnd_report.Height = 335;

    //                    }
    //                    else if (totalRows == 0)
    //                    {
    //                        DropDownListpage.Items.Add("0");
    //                        attnd_report.Height = 100;
    //                    }
    //                    else
    //                    {
    //                        attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //                        DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());
    //                        attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //                    }
    //                    if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
    //                    {
    //                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //                        attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //                        attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
                           
    //                    }


    //                    Panel3.Visible = false;


    //                }
    //                else
    //                {
    //                    errmsg.Visible = false;
    //                    Panel3.Visible = false;
    //                }
    //            }
    //            else if (view_header == "2")
    //            {

    //                for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
    //                {
    //                    attnd_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
    //                }

    //                errmsg.Visible = false;
    //                int i = 0;
    //                ddlpage.Items.Clear();
    //                int totrowcount = attnd_report.Sheets[0].RowCount;
    //                int pages = totrowcount / 25;
    //                int intialrow = 1;
    //                int remainrows = totrowcount % 25;
    //                if (attnd_report.Sheets[0].RowCount > 0)
    //                {
    //                    int i5 = 0;
    //                    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    //                    for (i = 1; i <= pages; i++)
    //                    {
    //                        i5 = i;

    //                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //                        intialrow = intialrow + 25;
    //                    }
    //                    if (remainrows > 0)
    //                    {
    //                        i = i5 + 1;
    //                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //                    }
    //                }
    //                if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //                {
    //                    for (i = 0; i < attnd_report.Sheets[0].RowCount; i++)
    //                    {
    //                        attnd_report.Sheets[0].Rows[i].Visible = true;
    //                    }
    //                    Double totalRows = 0;
    //                    totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
    //                    Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
    //                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //                    DropDownListpage.Items.Clear();
    //                    if (totalRows >= 10)
    //                    {
    //                        attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //                        {
    //                            DropDownListpage.Items.Add((k + 10).ToString());
    //                        }
    //                        DropDownListpage.Items.Add("Others");
    //                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //                        attnd_report.Height = 335;

    //                    }
    //                    else if (totalRows == 0)
    //                    {
    //                        DropDownListpage.Items.Add("0");
    //                        attnd_report.Height = 100;
    //                    }
    //                    else
    //                    {
    //                        attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //                        DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());
    //                        attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //                    }
    //                    if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
    //                    {
    //                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //                        attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //                        //  attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
                            
    //                    }
    //                    Panel3.Visible = false;
    //                }
    //                else
    //                {
    //                    Panel3.Visible = false;
    //                }
    //            }
    //            else
    //            {

    //            }
    //            lblpages.Visible = true;
    //            ddlpage.Visible = true;
    //        }
    //        else
    //        {
    //            lblpages.Visible = false;
    //            ddlpage.Visible = false;
    //        }
    //    }
    //    catch
    //    {
    //    }


    //}

    //public void print_btngo()
    //{
    //    try
    //    {
    //        final_print_col_cnt = 0;
    //        errlbl.Visible = false;
    //        check_col_count_flag = false;

    //        attnd_report.Sheets[0].SheetCorner.RowCount = 0;
    //        attnd_report.Sheets[0].ColumnCount = 0;
    //        attnd_report.Sheets[0].RowCount = 0;
    //        attnd_report.Sheets[0].SheetCorner.RowCount = 8;
    //        attnd_report.Sheets[0].ColumnCount = 5;


    //        hat.Clear();
    //        hat.Add("college_code", Session["InternalCollegeCode"].ToString());
    //        hat.Add("form_name", "overalldailyattndreport.aspx");
    //        dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
    //        if (dsprint.Tables[0].Rows.Count > 0)
    //        {
    //            lblpages.Visible = true;
    //            ddlpage.Visible = true;

    //            btnclick();

    //            new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
    //            isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();

    //            //1.set visible columns
    //            column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
    //            if (column_field != "" && column_field != null)
    //            {
    //                //  check_col_count_flag = true;

    //                for (col_count_all = 0; col_count_all < attnd_report.Sheets[0].ColumnCount; col_count_all++)
    //                {
    //                    attnd_report.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column                                
    //                }


    //                printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
    //                string[] split_printvar = printvar.Split(',');
    //                for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
    //                {
    //                    span_cnt = 0;
    //                    string[] split_star = split_printvar[splval].Split('*');

    //                    {
    //                        for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //                        {
    //                            if (attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), col_count].Text == split_printvar[splval])
    //                            {
    //                                if (col_count == 1 || col_count == 3 || col_count == 5 || col_count == 7)
    //                                {
    //                                    attnd_report.Sheets[0].Columns[col_count + 1].Visible = true;
    //                                    final_print_col_cnt++;
    //                                }
    //                                attnd_report.Sheets[0].Columns[col_count].Visible = true;

    //                                final_print_col_cnt++;
    //                                break;
    //                            }
    //                        }
    //                    }
    //                }
    //                //1 end.set visible columns
    //            }
    //            else
    //            {
    //                attnd_report.Visible = false;
    //                Showgrid.Visible = false;
    //                btnprintmaster.Visible = false;
    //                //Button1.Visible = false;
    //                Panel3.Visible = false;
    //                lblpages.Visible = false;
    //                ddlpage.Visible = false;
    //                errlbl.Visible = true;
    //                errlbl.Text = "Select Atleast One Column Field From The Treeview";
    //            }
    //        }
    //        // attnd_report.Width = final_print_col_cnt * 100;
    //    }
    //    catch
    //    {
    //    }
    //}

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

        Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
        Pageload(sender, e);
    }
    public void Pageload(object sender, EventArgs e)
    {
        ddlpage.Visible = false;
        lblpages.Visible = false;

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
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        
        Showgrid.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Button1.Visible = false;
        btnxl.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        Panel3.Visible = false;
        errlbl.Visible = false;
        //   pagesetpanel.Visible = false;
        pageddltxt.Visible = false;
        datelbl.Visible = false;
        

        string dt = DateTime.Today.ToShortDateString();
        string[] dsplit = dt.Split(new Char[] { '/' });
        datetxt.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
        Session["curr_year"] = dsplit[2].ToString();

        //-------------------spread design
        
        //attnd_report.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
        //attnd_report.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        //attnd_report.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
        


       
       
        //--------------------------end design


        //if (Session["prntvissble"].ToString() == "true")
        //{
        //    btnPrint.Visible = true;
        //}
        //else
        //{
        //    btnPrint.Visible = false;
        //}
        Session["QueryString"] = "";
        if (Request.QueryString["val"] != null)
        {
            try//-----------------------13/6/12 PRABHA
            {

                Session["QueryString"] = Convert.ToString(Request.QueryString["val"]);
                string_session_values = Request.QueryString["val"].Split(',');
                datetxt.Text = string_session_values[0].ToString();
                ddlcollege.SelectedIndex = Convert.ToInt16(string_session_values[1].ToString());
                Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();

               

                if (final_print_col_cnt > 0)
                {
                    // setheader_print();

                    list_absent_students();

                   

                }


            }
            catch
            {
            }

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        //Session["column_header_row_count"] = attnd_report.Sheets[0].ColumnHeader.RowCount;
        string degreedetails = string.Empty;

        degreedetails = "Overall Daily Attendance Percentage @ Attendance Date: " + datetxt.Text.ToString();
        string pagename = "overalldailyattndreport.aspx";

        //Printcontrol.loadspreaddetails(attnd_report, pagename, degreedetails);
        Printcontrol.Visible = true;
        //attnd_report.Sheets[0].PageSize = attnd_report.Sheets[0].RowCount;

        string ss = null;
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        
        Printcontrol.Visible = true;
    }

    public void btnPrint11()
    {
        DAccess2 d2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = d2.select_method_wo_parameter(colQ, "Text");
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
        spReportName.InnerHtml = "Overall Daily Attendance Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }


    public override void VerifyRenderingInServerForm(Control control)
    { }
}
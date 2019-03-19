using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;


public partial class MasterTimeTable : System.Web.UI.Page
{
    static Boolean forschoolsetting = false;
    Hashtable hat = new Hashtable();

    DataSet ds_load = new DataSet();
    DAccess2 daccess = new DAccess2();
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();


    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }

        collegecode = Session["collegecode"].ToString();
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        // Session["Semester"] =Convert.ToString(ddlSem.SelectedValue);
        if (!IsPostBack)
        {
            //'--------------------------------------
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            Fptimetable.Visible = false;
            Fpstaff.Visible = false;
            MyStyle.Font.Bold = true;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fptimetable.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            Fpstaff.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnprintmaster.Visible = false;
            btnxl.Visible = false;

            Lblreport.Visible = false;
            txtexcl.Visible = false;
            btnprnt.Visible = false;
            btnxcl.Visible = false;

            lblexer.Visible = false;


            bindbatch();
            binddegree();
            bindbranch();
            colg();
            // bindsem();
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            // Added By Sridharan 12 Mar 2015
            //{
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = daccess.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                if (schoolvalue.Trim() == "0")
                {
                    forschoolsetting = true;
                    Lblclg.Text = "School";
                    lblBatch.Text = "Year";
                    lblDegree.Text = "School Type";
                    lblBranch.Text = "Standard";
                    //lblsem.Text = "Term";
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
        lblexer.Visible = false;
    }

    public void bindbatch()
    {
        ddlBatch.Items.Clear();
        ds_load = daccess.select_method_wo_parameter("bind_batch", "sp");
        int count = ds_load.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlBatch.DataSource = ds_load;
            ddlBatch.DataTextField = "batch_year";
            ddlBatch.DataValueField = "batch_year";
            ddlBatch.DataBind();
        }
        int count1 = ds_load.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
            ddlBatch.SelectedValue = max_bat.ToString();
        }
        ddlBatch.Items.Insert(0, "ALL");
    }
    public void bindbranch()
    {
        ddlBranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddlDegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);

        ds_load = daccess.select_method("bind_branch", hat, "sp");
        int count2 = ds_load.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlBranch.DataSource = ds_load;
            ddlBranch.DataTextField = "dept_name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }
    }
    public void binddegree()
    {
        ddlDegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load = daccess.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddlDegree.DataSource = ds_load;
            ddlDegree.DataTextField = "course_name";
            ddlDegree.DataValueField = "course_id";
            ddlDegree.DataBind();
        }
    }

    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = ddlDegree.SelectedValue.ToString();
            ddlBranch.Items.Clear();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds_load.Dispose();
            ds_load.Reset();
            ds_load = daccess.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds_load.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataSource = ds_load;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
        }
        catch (Exception)
        {
            // errmsg.Text = ex.ToString();
        }
    }
    public void colg()
    {

        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        string clgname = "select college_code,collname from collinfo ";
        if (clgname != "")
        {
            ds_load = daccess.select_method(clgname, hat, "Text");
            ddlclg.DataSource = ds_load;
            ddlclg.DataTextField = "collname";
            ddlclg.DataValueField = "college_code";
            ddlclg.DataBind();
        }

    }
    /* protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
     {
         ddlBranch.Items.Clear();

         string course_id = ddlDegree.SelectedValue.ToString();
         BindBranch(singleuser, group_user, course_id, collegecode, usercode);
     }*/
    protected void btnGo_Click(object sender, EventArgs e)
    {
        loadfunction();

    }

    protected void loadfunction()
    {
        try
        {
            Hashtable hatHr = new Hashtable();
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnprintmaster.Visible = true;
            btnxl.Visible = true;

            btnxcl.Visible = true;
            Lblreport.Visible = true;
            txtexcl.Visible = true;
            btnprnt.Visible = true;

            Fptimetable.Sheets[0].RowCount = 0;
            lblerr.Visible = false;
            lblexer.Visible = false;
            int noofhours = 0;
            int noofdays = 0;
            int dayorder = 0;
            bool isvisible = false;
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string[] Daymon = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            strbatchyear = ddlBatch.Text.ToString();
            strbranch = ddlBranch.SelectedValue.ToString();
            string strpriodquery = string.Empty;
            string cursems=string.Empty;
            if (ddlBatch.SelectedItem.Text.ToLower().Trim() != "all") //added by mullai
            {
                string cursems1 = daccess.GetFunction("select distinct Current_Semester from Registration  where  degree_code='" + ddlBranch.SelectedItem.Value.ToString() + "' and Batch_Year ='" + ddlBatch.SelectedItem.Text.ToString() + "' and CC=0 and DelFlag=0 and Exam_Flag<>'debar'");
                cursems = " and semester='" + cursems1 + "'";
            }


            strpriodquery = "Select No_of_hrs_per_day,nodays from PeriodAttndSchedule where degree_code = " + ddlBranch.SelectedValue.ToString() + " " + cursems + " order by No_of_hrs_per_day desc";
            ds_load = daccess.select_method(strpriodquery, hat, "Text");
            if (ds_load.Tables[0].Rows.Count > 0)
            {

                noofhours = Convert.ToInt32(ds_load.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                noofdays = Convert.ToInt32(ds_load.Tables[0].Rows[0]["nodays"]);
                // noofsem = Convert.ToInt32(ds_load.Tables[0].Rows[0]["semester"]);

                Session["totalhrs"] = Convert.ToString(noofhours);
                Session["totnoofdays"] = Convert.ToString(noofdays);
                // Session["semorder"] = Convert.ToString(noofsem);
            }
            // Fptimetable.Sheets[0].RowCount = noofdays;


            if (noofhours > 0)
            {

                Fptimetable.Visible = true;
                Fptimetable.RowHeader.Visible = false;
                Fptimetable.Sheets[0].ColumnHeader.RowCount = 2;
                Fptimetable.Sheets[0].ColumnCount = 3;
                Fptimetable.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S No";
                Fptimetable.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Day/Timings";
                Fptimetable.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Semester";
                if (forschoolsetting == true)
                {
                    Fptimetable.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Term";
                }
                Fptimetable.Sheets[0].ColumnHeader.Rows[0].Font.Name = "Book Antiqua";
                Fptimetable.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
                Fptimetable.Sheets[0].ColumnHeader.Rows[1].Font.Name = "Book Antiqua";
                Fptimetable.Sheets[0].ColumnHeader.Rows[1].Font.Size = FontUnit.Medium;
                //Fptimetable.Sheets[0].Columns[0].Width = 100;
                Fptimetable.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                Fptimetable.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                Fptimetable.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                Fptimetable.Sheets[0].Columns[0].Font.Bold = true;

                for (int gh = 0; gh < Fptimetable.Sheets[0].ColumnCount; gh++)
                {
                    Fptimetable.Sheets[0].Columns[gh].Font.Name = "Book Antiqua";
                    Fptimetable.Sheets[0].Columns[gh].Font.Size = FontUnit.Medium;

                }
                //    Fptimetable.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                // Fptimetable.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                // Fptimetable.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                // Fptimetable.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
                // Fptimetable.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                // Fptimetable.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
                // Fptimetable.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                // Fptimetable.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                // Fptimetable.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                // Fptimetable.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
                // Fptimetable.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
                // Fptimetable.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
                // Fptimetable.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
                // Fptimetable.Sheets[0].Columns[6].Font.Size = FontUnit.Medium;
                // Fptimetable.Sheets[0].Columns[7].Font.Name = "Book Antiqua";
                // Fptimetable.Sheets[0].Columns[7].Font.Size = FontUnit.Medium;
                // Fptimetable.Sheets[0].Columns[8].Font.Name = "Book Antiqua";
                // Fptimetable.Sheets[0].Columns[8].Font.Size = FontUnit.Medium;
                // Fptimetable.Sheets[0].Columns[9].Font.Name = "Book Antiqua";
                // Fptimetable.Sheets[0].Columns[9].Font.Size = FontUnit.Medium;
                // Fptimetable.Sheets[0].Columns[10].Font.Name = "Book Antiqua";
                // Fptimetable.Sheets[0].Columns[10].Font.Size = FontUnit.Medium;
                //// Fptimetable.Sheets[0].Columns[11].Font.Name = "Book Antiqua";
                //// Fptimetable.Sheets[0].Columns[11].Font.Size = FontUnit.Medium;

                Fptimetable.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                Fptimetable.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                Fptimetable.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
                Fptimetable.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                Fptimetable.Sheets[0].Columns[1].Font.Bold = true;
                Fptimetable.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                Fptimetable.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                Fptimetable.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                //Fptimetable.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 9);
                Fptimetable.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Large;
                Fptimetable.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fptimetable.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fptimetable.Columns[0].Width = 40;


                Fpstaff.Visible = true;
                Fpstaff.Width = 800;
                Fpstaff.RowHeader.Visible = false;
                Fpstaff.Sheets[0].ColumnHeader.RowCount = 1;
                Fpstaff.Sheets[0].ColumnCount = 6;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S No";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "SUBJECT CODE";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "SUBJECT NAME";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total Hours";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "STAFF";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 5].Text = "STAFF DEPARTMENT";
                Fpstaff.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                Fpstaff.Sheets[0].Columns[0].Width = 30;
                Fpstaff.Sheets[0].Columns[1].Width = 100;
                Fpstaff.Sheets[0].Columns[2].Width = 350;
                Fpstaff.Sheets[0].Columns[3].Width = 30;
                Fpstaff.Sheets[0].Columns[4].Width = 200;
                Fpstaff.Sheets[0].Columns[5].Width = 200;
                Fpstaff.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                Fpstaff.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
                Fpstaff.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
                Fpstaff.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                Fpstaff.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;


                Fpstaff.Sheets[0].RowCount = 0;




                Fptimetable.Sheets[0].RowCount = 0;
                // if (ddlBatch.SelectedValue.ToString()!= "ALL")
                // {
                // Fptimetable.Sheets[0].ColumnCount = noofhours + 4;
                string sqlsmm = "";
                if (ddlBatch.SelectedItem.Text != "ALL")
                {
                    sqlsmm = " select distinct batch_year,Current_Semester from Registration  where  degree_code=" + ddlBranch.SelectedValue.ToString() + " and Batch_Year =" + ddlBatch.SelectedItem.Text + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar'"; // added by jairam 18-11-2014
                }
                else
                {
                    sqlsmm = " select distinct batch_year,Current_Semester from Registration  where  degree_code=" + ddlBranch.SelectedValue.ToString() + "  and CC=0 and DelFlag=0 and Exam_Flag<>'debar'";
                }
                DataSet dlsql = daccess.select_method(sqlsmm, hat, "Text");
                if (dlsql.Tables[0].Rows.Count > 0)
                {
                    string semes = dlsql.Tables[0].Rows[0]["Current_Semester"].ToString();
                    string sqlbrk = "select no_of_hrs_I_half_day from PeriodAttndSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + semes + "";
                    DataSet dsbrk = daccess.select_method(sqlbrk, hat, "Text");
                    if (dsbrk.Tables[0].Rows.Count > 0)
                    {
                        string brk = dsbrk.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                        int columncount = 0;

                        for (int i = 1; i <= Convert.ToInt32(brk); i++)
                        {
                            Fptimetable.Sheets[0].ColumnCount++;
                            Fptimetable.Sheets[0].ColumnHeader.Cells[0, i + 2].Text = "Period " + i + "";   //added by Mullai
                            string belltime = "";
                            string sttimequery = "";
                            if (ddlBatch.SelectedItem.Text != "ALL")
                            {
                                sttimequery = "Select start_time,end_time from BellSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and period1='" + i + "' and batch_year ='" + ddlBatch.SelectedItem.Text + "' and semester ='" + semes + "'";  // added by jairam 18-11-2014
                            }
                            else
                            {
                                sttimequery = "Select start_time,end_time from BellSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and period1='" + i + "' and semester ='" + semes + "'";  // added by jairam 18-11-2014
                            }
                            ds_load = daccess.select_method(sttimequery, hat, "Text");
                            if (ds_load.Tables[0].Rows.Count > 0)
                            {
                                if (ds_load.Tables[0].Rows[0]["start_time"].ToString() != "" && ds_load.Tables[0].Rows[0]["start_time"].ToString() != null && ds_load.Tables[0].Rows[0]["end_time"].ToString() != "" && ds_load.Tables[0].Rows[0]["end_time"].ToString() != null)
                                {
                                    string[] splitstarttime = ds_load.Tables[0].Rows[0]["start_time"].ToString().Split(' ');
                                    string[] splitendtime = ds_load.Tables[0].Rows[0]["end_time"].ToString().Split(' ');
                                    belltime = splitstarttime[1].ToString() + ' ' + splitstarttime[2].ToString() + ' ' + " To" + ' ' + splitendtime[1].ToString() + ' ' + splitendtime[2].ToString();

                                }

                                Fptimetable.Sheets[0].ColumnHeader.Cells[0, Fptimetable.Sheets[0].ColumnCount - 1].Text = i.ToString();
                                Fptimetable.Sheets[0].ColumnHeader.Cells[1, Fptimetable.Sheets[0].ColumnCount - 1].Text = belltime;

                            }



                        }
                       Fptimetable.Sheets[0].ColumnCount++;
                        Fptimetable.Sheets[0].ColumnHeaderSpanModel.Add(0, Fptimetable.Sheets[0].ColumnCount - 1, 2, 1);

                        // Fptimetable.Sheets[0].ColumnHeader.Cells[0, columncount + 2].Text = "";
                        //Fptimetable.Sheets[0].ColumnHeader.Cells[1, columncount + 2].Text = "Break";


                        for (int k = Convert.ToInt32(brk) + 1; k <= noofhours; k++)
                        {
                            Fptimetable.Sheets[0].ColumnCount++;
                            Fptimetable.Sheets[0].ColumnHeader.Cells[0, k+3].Text = "Period " + k + "";  //added by Mullai
                            string belltime = "";
                            string sttimequery = "";
                            if (ddlBatch.SelectedItem.Text != "ALL")
                            {
                                sttimequery = "Select distinct start_time,end_time from BellSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and period1='" + k + "' and batch_year ='" + ddlBatch.SelectedItem.Text + "' and semester='"+semes+"'"; // added by jairam 18-11-2014
                            }
                            else
                            {
                                sttimequery = "Select distinct start_time,end_time from BellSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and period1='" + k + "'  and semester  in (select distinct Current_Semester from Registration  where  degree_code='" + ddlBranch.SelectedValue.ToString() + "'  and CC=0 and DelFlag=0 and Exam_Flag<>'debar')"; // added by jairam 18-11-2014
                            }
                            ds_load = daccess.select_method(sttimequery, hat, "Text");
                            if (ds_load.Tables[0].Rows.Count > 0)
                            {
                                if (ds_load.Tables[0].Rows[0]["start_time"].ToString() != "" && ds_load.Tables[0].Rows[0]["start_time"].ToString() != null && ds_load.Tables[0].Rows[0]["end_time"].ToString() != "" && ds_load.Tables[0].Rows[0]["end_time"].ToString() != null)
                                {
                                    string[] splitstarttime = ds_load.Tables[0].Rows[0]["start_time"].ToString().Split(' ');
                                    string[] splitendtime = ds_load.Tables[0].Rows[0]["end_time"].ToString().Split(' ');
                                    belltime = splitstarttime[1].ToString() + ' ' + splitstarttime[2].ToString() + ' ' + " To " + ' ' + splitendtime[1].ToString() + ' ' + splitendtime[2].ToString();

                                }
                                Fptimetable.Sheets[0].ColumnHeader.Cells[0, Fptimetable.Sheets[0].ColumnCount - 1].Text = k.ToString();
                                Fptimetable.Sheets[0].ColumnHeader.Cells[1, Fptimetable.Sheets[0].ColumnCount - 1].Text = belltime;
                            }


                        }
                    }
                    else
                    {
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnprintmaster.Visible = false;
                        btnxl.Visible = false;

                        Fpstaff.Visible = false;
                        Fptimetable.Visible = false;
                        lblerr.Text = "No Records Found";
                        lblerr.Visible = true;

                        btnxcl.Visible = false;
                        Lblreport.Visible = false;
                        txtexcl.Visible = false;
                        btnprnt.Visible = false;
                        return;

                    }




                }
            }
            else
            {
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;

                Fpstaff.Visible = false;
                Fptimetable.Visible = false;
                lblerr.Text = "No Records Found";
                lblerr.Visible = true;

                btnxcl.Visible = false;
                Lblreport.Visible = false;
                txtexcl.Visible = false;
                btnprnt.Visible = false;
                return;
            }

            int r = 0;
            int delflag = 0; // Sangeetha On 29 Aug 2014
            for (int day = 0; day < noofdays; day++)
            {

                r++;
                string dayofweek = Days[day];
                string dayofweek1 = Daymon[day];
                string dayvalue = "";
                for (int i = 1; i <= noofhours; i++)
                {

                    if (dayvalue == "")
                    {
                        dayvalue = dayofweek + i;
                    }
                    else
                    {
                        dayvalue = dayvalue + ',' + dayofweek + i;
                    }
                }
                
                string batch = "";
                if (ddlBatch.SelectedValue.ToString() != "ALL")
                {
                    batch = "and batch_year=" + ddlBatch.SelectedValue.ToString();

                }
                else
                {
                    batch = "";
                }

                string semsql = "select distinct batch_year,Current_Semester,Sections from Registration  where  degree_code=" + ddlBranch.SelectedValue.ToString() + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar'" + batch + " order by batch_year desc";
                DataSet dssemester = daccess.select_method(semsql, hat, "Text");

                if (dssemester.Tables[0].Rows.Count > 0)
                {


                    for (int k = 0; k <= dssemester.Tables[0].Rows.Count - 1; k++)
                    {
                        string sec = "";
                        string semester = dssemester.Tables[0].Rows[k]["Current_Semester"].ToString();
                        string batchyear = dssemester.Tables[0].Rows[k]["batch_year"].ToString();
                        string strsection = dssemester.Tables[0].Rows[k]["Sections"].ToString();
                        if (strsection.Trim() == "" || strsection == null || strsection.Trim() == "-1")
                        {
                            sec = " ";
                        }
                        else
                        {
                            sec = " and sections='" + dssemester.Tables[0].Rows[k]["Sections"].ToString() + "'";
                        }
                        string schedule = "Select  top 1" + dayvalue + " from semester_schedule where degree_code=" + ddlBranch.SelectedValue.ToString() + "and batch_year=" + batchyear + " " + sec + " and semester='" + semester + "' order by FromDate Desc";
                        ds_load = daccess.select_method(schedule, hat, "Text");

                        
                        if (ds_load.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < ds_load.Tables[0].Rows.Count; i++)
                            {

                                Fptimetable.Sheets[0].RowCount++;


                                // Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].Text = srno+1.ToString();
                                string value = dayofweek;

                                for (int j = 1; j < ds_load.Tables[0].Columns.Count + 1; j++)
                                {

                                    string dsvalue = value + j;
                                    string classhour = ds_load.Tables[0].Rows[i]["" + dsvalue + ""].ToString();
                                    if (classhour.Trim() != "" && classhour.Trim() != "0" && classhour != null)
                                    {
                                        string[] spiltmulpl = classhour.Split(';');
                                        string setclasshour = "";
                                        for (int mul = 0; mul <= spiltmulpl.GetUpperBound(0); mul++)
                                        {

                                            string[] spiltclasshour = spiltmulpl[mul].Split('-');

                                           

                                            
                                            string sqlbrk = "select no_of_hrs_I_half_day from PeriodAttndSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + semester + "";
                                            DataSet dsbrk = daccess.select_method(sqlbrk, hat, "Text");

                                            string brk = dsbrk.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();


                                            if (j <= Convert.ToInt32(brk))
                                            {
                                                if (setclasshour == "")
                                                {
                                                    if (j == Convert.ToInt32(brk))
                                                    {
                                                        Fptimetable.Sheets[0].ColumnHeader.Cells[0, j + 3].Text = "Lunch Break";
                                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 3].Text = "Break";
                                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 3].HorizontalAlign = HorizontalAlign.Center;
                                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 3].VerticalAlign = VerticalAlign.Middle;
                                                        Fptimetable.Sheets[0].SetColumnMerge(j + 3, FarPoint.Web.Spread.Model.MergePolicy.Always);

                                                    }

                                                    try
                                                    {
                                                        delflag = 1;
                                                        setclasshour = daccess.GetFunction("select acronym from subject where subject_no='" + spiltclasshour[0].ToString() + "'");
                                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 2].Text = setclasshour;
                                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 2].Text = setclasshour;
                                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 2].HorizontalAlign = HorizontalAlign.Center;
                                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 2].VerticalAlign = VerticalAlign.Middle;
                                                    }
                                                    catch (Exception)
                                                    {
                                                    }
                                                }
                                                else
                                                {
                                                    delflag = 1;
                                                    setclasshour = setclasshour + "/" + daccess.GetFunction("select acronym from subject where subject_no='" + spiltclasshour[0].ToString() + "'");
                                                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 2].Text = setclasshour;
                                                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 2].Text = setclasshour;
                                                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 2].HorizontalAlign = HorizontalAlign.Center;
                                                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 2].VerticalAlign = VerticalAlign.Middle;
                                                }

                                            }


                                            else
                                            {
                                                if (setclasshour == "")

                                                    try
                                                    {
                                                        delflag = 1;
                                                        setclasshour = daccess.GetFunction("select acronym from subject where subject_no='" + spiltclasshour[0].ToString() + "'");
                                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 3].Text = setclasshour;

                                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 3].HorizontalAlign = HorizontalAlign.Center;
                                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 3].VerticalAlign = VerticalAlign.Middle;
                                                    }
                                                    catch (Exception)
                                                    {
                                                    }



                                                else
                                                {
                                                    delflag = 1;
                                                    setclasshour = setclasshour + "/" + daccess.GetFunction("select acronym from subject where subject_no='" + spiltclasshour[0].ToString() + "'");
                                                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 3].Text = setclasshour;

                                                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 3].HorizontalAlign = HorizontalAlign.Center;
                                                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, j + 3].VerticalAlign = VerticalAlign.Middle;
                                                }
                                            }
                                            string sqlorder = "select schOrder from PeriodAttndSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + semester + "";
                                            DataSet dsorder = daccess.select_method(sqlorder, hat, "Text");
                                            dayorder = Convert.ToInt32(dsorder.Tables[0].Rows[0]["schOrder"]);


                                            if (dayorder == 1)
                                            {
                                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Text = dayofweek1;
                                            }
                                            else
                                            {
                                                int date = day + 1;
                                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Text = "Day " + date;
                                            }

                                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].Text = getroman(semester) + "/" + strsection.ToString();
                                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                            Fptimetable.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                            Fptimetable.Sheets[0].SetRowMerge(Fptimetable.Sheets[0].RowCount - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].Text = r.ToString();
                                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                            for (int cc = 0; cc < spiltclasshour.Length; cc++)
                                            {
                                                string va = Convert.ToString(spiltclasshour[cc]);
                                                if (!hatHr.ContainsKey(spiltclasshour[0].ToString() + "-" +va+"-"+ strsection))
                                                {
                                                    hatHr.Add(spiltclasshour[0].ToString() + "-" +va+"-"+ strsection, 1);
                                                }
                                                else
                                                {
                                                    int mark = Convert.ToInt32(hatHr[spiltclasshour[0].ToString() + "-" +va+"-"+ strsection]);
                                                    mark = mark + 1;
                                                    hatHr[spiltclasshour[0].ToString() + "-" + va + "-" + strsection] = mark;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }

                        if (hatHr.Count > 0)
                        {

                        }

                        if (day == noofdays - 1)
                        {
                            if (ds_load.Tables[0].Rows.Count > 0)
                            {
                                Fpstaff.Sheets[0].RowCount++;

                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue;

                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = "Batch : " + batchyear + " " + '-' + " Branch : " + ddlBranch.SelectedItem.ToString() + " - Sem : " + getroman(semester) + " " + '-' + " Section " + '-' + " " + strsection + " ";
                                if (forschoolsetting == true)
                                {
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = "Year : " + batchyear + " " + '-' + " Standard : " + ddlBranch.SelectedItem.ToString() + " - Term : " + getroman(semester) + " " + '-' + " Section " + '-' + " " + strsection + " ";
                                }
                                Fpstaff.Sheets[0].SpanModel.Add(Fpstaff.Sheets[0].RowCount - 1, 0, 1, 6);
                            }

                            string staff = "select distinct noofhrsperweek,s.acronym,s.subject_no,subject_code,subject_name,sm.staff_name,sm.staff_code,sam.dept_name from subject s,syllabus_master sy,staffmaster sm,staff_selector ss ,staff_appl_master sam where s.syll_code=sy.syll_code and sy.batch_year= " + batchyear + " and sy.degree_code=" + ddlBranch.SelectedValue.ToString() + " and sy.semester=" + semester + " " + sec + "  and sm.staff_code=ss.staff_code and ss.subject_no=s.subject_no and sam.appl_no=sm.appl_no order by s.subject_no";   //modified by Mullai
                            DataSet desub = daccess.select_method(staff, hat, "Text");


                            if (desub.Tables[0].Rows.Count > 0)
                            {
                                int srno = 0;
                                string temp = "";
                               // string temp1 = "";
                                for (int s = 0; s < desub.Tables[0].Rows.Count; s++)
                                {
                                    Fpstaff.Sheets[0].RowCount++;
                                    string SubjectNo = desub.Tables[0].Rows[s]["subject_no"].ToString();//staff_code
                                    string staffC = desub.Tables[0].Rows[s]["staff_code"].ToString();
                                    string hours = desub.Tables[0].Rows[s]["noofhrsperweek"].ToString();
                                    string Subcode = desub.Tables[0].Rows[s]["subject_code"].ToString();
                                    string subname = desub.Tables[0].Rows[s]["subject_name"].ToString();
                                    string staffname = desub.Tables[0].Rows[s]["staff_name"].ToString();
                                    string acronym = desub.Tables[0].Rows[s]["acronym"].ToString();
                                    string deptname = desub.Tables[0].Rows[s]["dept_name"].ToString();
                                    if (temp != Subcode) //modified by Mullai
                                    {
                                        srno++;
                                        temp = Subcode;
                                    }

                                    int totHr = Convert.ToInt32(hatHr[SubjectNo + "-" +staffC+"-"+ strsection]);

                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = Subcode.ToString();
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;


                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = subname + "(" + acronym + ")";
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                    //Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = hours.ToString();//command by Rajkumar on 11-10-2018
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = totHr.ToString();
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = staffname.ToString();
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].Text = deptname.ToString(); //added by Mullai
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                    Fpstaff.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    Fpstaff.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    Fpstaff.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    Fpstaff.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    Fpstaff.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Restricted);//added by rajasekar 26/10/2018
                                    isvisible = true;
                                  
                                }
                            }

                        }
                    }


                }
                else
                {
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;

                    Fpstaff.Visible = false;
                    Fptimetable.Visible = false;
                    lblerr.Text = "No Records Found";
                    lblerr.Visible = true;

                    btnxcl.Visible = false;
                    Lblreport.Visible = false;
                    txtexcl.Visible = false;
                    btnprnt.Visible = false;
                }

                if (delflag == 0)
                {
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;

                    Fpstaff.Visible = false;
                    Fptimetable.Visible = false;

                    lblerr.Text = "No Records Found";
                    lblerr.Visible = true;

                    btnxcl.Visible = false;
                    Lblreport.Visible = false;
                    txtexcl.Visible = false;
                    btnprnt.Visible = false;

                }
                
            }

            if (isvisible)
            {
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnprintmaster.Visible = true;
                btnxl.Visible = true;

                Fpstaff.Visible = true;
                Fptimetable.Visible = true;

                //lblerr.Text = "No Records Found";
                lblerr.Visible = false;

                btnxcl.Visible = true;
                Lblreport.Visible = true;
                txtexcl.Visible = true;
                btnprnt.Visible = true;
            }

        }

        catch (Exception ex)
        {
            lblerr.Text = ex.ToString();
            lblerr.Visible = true;

        }


    }

    public string getroman(string n)
    {
        string roman = "";
        switch (n)
        {
            case "1":
                roman = "I";
                break;
            case "2":
                roman = "II";
                break;
            case "3":
                roman = "III";
                break;
            case "4":
                roman = "IV";
                break;
            case "5":
                roman = "V";
                break;
            case "6":
                roman = "VI";
                break;
            case "7":
                roman = "VII";
                break;
            case "8":
                roman = "VIII";
                break;
        }
        return roman;

    }


    protected void btnxcl_click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcl.Text;

            if (reportname.ToString().Trim() != "")
            {
                daccess.printexcelreport(Fptimetable, reportname);
            }
            else
            {
                lblexer.Text = "Please Enter Your Report Name";
                lblexer.Visible = true;
            }



        }
        catch (Exception ex)
        {
            lblexer.Text = ex.ToString();
        }

    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {

            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                daccess.printexcelreport(Fpstaff, reportname);
            }
            else
            {
                lblexer.Text = "Please Enter Your Report Name";
                lblexer.Visible = true;
            }


        }
        catch (Exception ex)
        {
            lblexer.Text = ex.ToString();
        }


    }
    protected void btnprnt_Click(object sender, EventArgs e)
    {
        string degreedetails = "" + ddlBatch.SelectedValue.ToString() + '-' + ddlDegree.SelectedItem.ToString() + '-' + ddlBranch.SelectedItem.ToString();
        string pagename = "MasterTimeTable.aspx";
        Printcontrol.loadspreaddetails(Fptimetable, pagename, degreedetails);
        Printcontrol.Visible = true;

    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "" + ddlBatch.SelectedValue.ToString() + '-' + ddlDegree.SelectedItem.ToString() + '-' + ddlBranch.SelectedItem.ToString();
        string pagename = "MasterTimeTable.aspx";
        Printcontrol.loadspreaddetails(Fpstaff, pagename, degreedetails);
        Printcontrol.Visible = true;

    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBranch.Items.Clear();

        string course_id = ddlDegree.SelectedValue.ToString();
        BindBranch(singleuser, group_user, course_id, collegecode, usercode);

    }
}




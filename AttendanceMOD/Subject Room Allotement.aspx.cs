using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using Gios.Pdf;
using InsproDataAccess;

public partial class AttendanceMOD_SubjectRoomAllotement : System.Web.UI.Page
{
    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    SqlConnection con = new SqlConnection(Convert.ToString(ConfigurationManager.AppSettings["con"]));
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    Boolean cellfalsg = false;
    Boolean celldetails = false;
    string tablevalue = string.Empty;
    Boolean allowcom = false;
    Boolean allowmuliallot = false;
    DataSet srids = new DataSet();
    DAccess2 srida = new DAccess2();
    Hashtable allotrow = new Hashtable();
    Hashtable hatHr = new Hashtable();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    DAccess2 dacess = new DAccess2();
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    static int noofdays = 0;
    Hashtable has = new Hashtable();
    static int inofhours = 0;
    static int rowindexs = 0;
    static string dropvalue = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            //errmsg.Visible = false;
            if (!IsPostBack)
            {




                Bindcolg();

                BindBatch();
                BindDegree();
                if (ddldegree.Items.Count > 0)
                {
                    ddldegree.Enabled = true;
                    ddlbranch.Enabled = true;
                    ddlsec.Enabled = true;
                    ddlsem.Enabled = true;
                    btngo.Enabled = true;

                    BindBranch();
                    BindSem();
                    BindSectionDetail(strbatch, strbranch);

                }
                else
                {
                    ddldegree.Enabled = false;
                    ddlbranch.Enabled = false;
                    ddlsec.Enabled = false;
                    ddlsem.Enabled = false;
                    btngo.Enabled = false;

                }



                string grouporusercode = string.Empty;
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }



            }





        }
        catch (Exception ex)
        {

        }
    }
    public void BindBatch()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void Bindcolg()
    {
        //try
        //{
        //    string colg = "select collname,college_code from collinfo";
        //    ds.Dispose();
        //    ds.Reset();
        //    DAccess2 d2 = new DAccess2();
        //    ds = d2.select_method_wo_parameter(colg, "Text");
        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        ddlcolg.DataSource = ds;
        //        ddlcolg.DataTextField = "collname";
        //        ddlcolg.DataValueField = "college_code";
        //        ddlcolg.DataBind();
        //        ddlcolg.SelectedIndex = ddlbatch.Items.Count - 1;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    errmsg.Text = ex.ToString();
        //}
        string group_code = Session["group_code"].ToString();
        string columnfield = string.Empty;
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
        ddlcolg.Items.Clear();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlcolg.Enabled = true;
            ddlcolg.DataSource = ds;
            ddlcolg.DataTextField = "collname";
            ddlcolg.DataValueField = "college_code";
            ddlcolg.DataBind();
        }
    }

    public void BindDegree()
    {
        try
        {
            ddldegree.Items.Clear();
            collegecode = ddlcolg.SelectedValue.ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void BindBranch()
    {
        try
        {
            course_id = ddldegree.SelectedValue.ToString();
            collegecode = ddlcolg.SelectedValue.ToString();
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void BindSem()
    {
        try
        {
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            collegecode = ddlcolg.SelectedValue.ToString();
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
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
        }
        catch (Exception ex)
        {

        }
    }

    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {

            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            ddlsec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSectionDetail(strbatch, strbranch);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                //  ddlsec.Items.Insert(0, "All");
                if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
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
        catch (Exception ex)
        {

        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddldegree.Items.Count > 0)
        {

            BindBranch();
            BindSem();
            BindSectionDetail(strbatch, strbranch);

        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {

        BindBranch();
        BindSem();
        BindSectionDetail(strbatch, strbranch);

    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {

        BindSem();
        BindSectionDetail(strbatch, strbranch);


    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {

        BindSectionDetail(strbatch, strbranch);

    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void collook_load(object sender, EventArgs e)
    {
        try
        {
            BindBatch();
            BindDegree();
            if (ddldegree.Items.Count > 0)
            {
                ddldegree.Enabled = true;
                ddlbranch.Enabled = true;
                ddlsec.Enabled = true;
                ddlsem.Enabled = true;
                btngo.Enabled = true;

                BindBranch();
                BindSem();
                BindSectionDetail(strbatch, strbranch);


            }
            else
            {
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
                ddlsec.Enabled = false;
                ddlsem.Enabled = false;
                btngo.Enabled = false;


            }
        }
        catch
        {
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
      
        loadschedule();
        roomallotment();
        
    }
    public void loadschedule()
    {
        try
        {
            string strsec = string.Empty;
            int intNHrs = 0;
            int SchOrder = 0;
            int nodays = 0;
            string srt_day = string.Empty;
            int order = 0;
            int insert_val = 0;
            string sunjno_staffno = string.Empty;
            int subj_no = 0;
            string acronym_val = string.Empty;
            int day_list = 0;
            string day_order = string.Empty;
            int ind_subj = 0;
            string sunjno_staffno_s = string.Empty;
            string acro = string.Empty;
            string acronym = string.Empty;
            string alt_sched = string.Empty;
            string shed_list = string.Empty;
            int spreadDet_ac = 0;
            string todate = string.Empty;

            DataTable dtTTDisp = new DataTable();
            dtTTDisp.Columns.Add("DateDisp");
            dtTTDisp.Columns.Add("DateVal");
            dtTTDisp.Columns.Add("dayacram");
            dtTTDisp.Columns.Add("P1Val");
            dtTTDisp.Columns.Add("PVal1");
            dtTTDisp.Columns.Add("s1Val");
            dtTTDisp.Columns.Add("sVal1");
            dtTTDisp.Columns.Add("P2Val");
            dtTTDisp.Columns.Add("PVal2");
            dtTTDisp.Columns.Add("s2Val");
            dtTTDisp.Columns.Add("sVal2");
            dtTTDisp.Columns.Add("P3Val");
            dtTTDisp.Columns.Add("PVal3");
            dtTTDisp.Columns.Add("s3Val");
            dtTTDisp.Columns.Add("sVal3");
            dtTTDisp.Columns.Add("P4Val");
            dtTTDisp.Columns.Add("PVal4");
            dtTTDisp.Columns.Add("s4Val");
            dtTTDisp.Columns.Add("sVal4");
            dtTTDisp.Columns.Add("P5Val");
            dtTTDisp.Columns.Add("PVal5");
            dtTTDisp.Columns.Add("s5Val");
            dtTTDisp.Columns.Add("sVal5");
            dtTTDisp.Columns.Add("P6Val");
            dtTTDisp.Columns.Add("PVal6");
            dtTTDisp.Columns.Add("s6Val");
            dtTTDisp.Columns.Add("sVal6");
            dtTTDisp.Columns.Add("P7Val");
            dtTTDisp.Columns.Add("PVal7");
            dtTTDisp.Columns.Add("s7Val");
            dtTTDisp.Columns.Add("sVal7");
            dtTTDisp.Columns.Add("P8Val");
            dtTTDisp.Columns.Add("PVal8");
            dtTTDisp.Columns.Add("s8Val");
            dtTTDisp.Columns.Add("sVal8");
            dtTTDisp.Columns.Add("P9Val");
            dtTTDisp.Columns.Add("PVal9");
            dtTTDisp.Columns.Add("s9Val");
            dtTTDisp.Columns.Add("sVal9");
            dtTTDisp.Columns.Add("P10Val");
            dtTTDisp.Columns.Add("PVal10");
            dtTTDisp.Columns.Add("s10Val");
            dtTTDisp.Columns.Add("sVal10");
            DataRow drNew = null;
            //-------------date
            //string date1;
            //string selectedDate;
            //date1 = txtDate.Text.ToString();
            //string[] split = date1.Split(new Char[] { '/' });
            //selectedDate = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            string[] DaysAcronym = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string[] DaysName = new string[7] { "Monday", "Tuesday", "wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            //DateTime dtSelectedDate = Convert.ToDateTime(selectedDate.ToString());

            string semStartdate = string.Empty;
            //-------------start date

            string qry = "select start_date from seminfo where degree_code=" + Convert.ToString(ddlbranch.SelectedValue) + " and semester=" + Convert.ToString(ddlsem.SelectedValue) + " and batch_year=" + Convert.ToString(ddlbatch.SelectedValue) + " ";

            DataSet qryDataSet = dacess.select_method_wo_parameter(qry, "Text");

            if (qryDataSet.Tables.Count > 0 && qryDataSet.Tables[0].Rows.Count > 0)
            {
                semStartdate = Convert.ToString(qryDataSet.Tables[0].Rows[0]["start_date"]);
            }
            //-------section
            if (Convert.ToString(ddlsec.SelectedValue) == " ")
            {
                strsec = string.Empty;
            }
            else
            {
                if (Convert.ToString(ddlsec.SelectedValue) == "-1")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + Convert.ToString(ddlsec.SelectedValue) + "'";
                }
            }


            string periodDetailsQry = "Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + Convert.ToString(ddlbranch.SelectedValue) + " and semester = " + Convert.ToString(ddlsem.SelectedValue) + "";
            DataSet periodDetailsDataSet = dacess.select_method_wo_parameter(periodDetailsQry, "Text");
            if (periodDetailsDataSet.Tables.Count > 0 && periodDetailsDataSet.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(periodDetailsDataSet.Tables[0].Rows[0]["No_of_hrs_per_day"]) != "")
                {
                    intNHrs = Convert.ToInt32(periodDetailsDataSet.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                    inofhours = intNHrs;
                    SchOrder = Convert.ToInt32(periodDetailsDataSet.Tables[0].Rows[0]["schorder"]);
                    nodays = Convert.ToInt32(periodDetailsDataSet.Tables[0].Rows[0]["nodays"]);
                    noofdays = nodays;

                }
            }
            //------------------------dayorder

            string[] daylist = { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };

            string semScheduleQry = "select top 1 * from semester_schedule where batch_year=" + Convert.ToString(ddlbatch.SelectedValue) + " and degree_code = " + Convert.ToString(ddlbranch.SelectedValue) + " and semester = " + Convert.ToString(ddlsem.SelectedValue) + "  " + strsec + " order by fromdate desc";//and FromDate<= ' " + Convert.ToString(selectedDate) + " '
            DataSet semScheduleDataSet = dacess.select_method_wo_parameter(semScheduleQry, "Text");
            int row = 0;
            int fnrow = 0;
            if (semScheduleDataSet.Tables.Count > 0 && semScheduleDataSet.Tables[0].Rows.Count > 0)
            {

                for (day_list = 0; day_list < nodays; day_list++)
                {
                    int count = 0;
                    row = dtTTDisp.Rows.Count;
                    fnrow = row;
                    for (insert_val = 1; insert_val <= intNHrs; insert_val++)
                    {
                        count++;
                        row = fnrow;

                        string staffname = string.Empty;

                        acro = string.Empty;
                        shed_list = string.Empty;
                        day_order = daylist[day_list] + insert_val.ToString();
                        string dayName = DaysName[day_list];
                        string dayAcronym = DaysAcronym[day_list];
                        sunjno_staffno = Convert.ToString(semScheduleDataSet.Tables[0].Rows[0][day_order]);
                        string[] many_subjs = sunjno_staffno.Split(new Char[] { ';' });
                        for (int ind_staffs = 0; ind_staffs < many_subjs.Length; ind_staffs++)
                        {

                            string[] subjno_staffno_splts = many_subjs[ind_staffs].Split(new Char[] { '-' });
                            for (int ind_subjs = 1; ind_subjs < subjno_staffno_splts.Count() - 1; ind_subjs++)
                            {



                                sunjno_staffno = Convert.ToString(semScheduleDataSet.Tables[0].Rows[0][day_order]);


                                string[] many_subj = sunjno_staffno.Split(new Char[] { ';' });

                                if (many_subjs.GetUpperBound(0) >= 0)
                                {
                                    sunjno_staffno_s = many_subjs[ind_staffs];
                                    if (sunjno_staffno_s.Trim() != "")
                                    {

                                        string[] subjno_staffno_splt = sunjno_staffno_s.Split(new Char[] { '-' });
                                        subj_no = Convert.ToInt32(subjno_staffno_splt[0].ToString());

                                        cona.Close();
                                        cona.Open();

                                        acronym_val = "select(isnull(acronym,subject_code)+'-'+ subject_name ) as acronym,(isnull(acronym,subject_code)+'-'+ acronym ) as subacronym  from subject where subject_no=" + subj_no.ToString() + " ";
                                        string staff_name = d2.GetFunction("select staff_name from staffmaster where staff_code= '" + subjno_staffno_splts[ind_subjs] + "'");

                                        SqlCommand ac_cmd = new SqlCommand(acronym_val, cona);
                                        SqlDataReader ac_dr;
                                        ac_dr = ac_cmd.ExecuteReader();
                                        ac_dr.Read();
                                        if (ac_dr.HasRows == true)
                                        {
                                            if (subacr.Checked==true)
                                                acronym = ac_dr["subacronym"].ToString();
                                            else
                                            acronym = ac_dr["acronym"].ToString();
                                            staffname = staff_name;
                                            acro = acronym;
                                            shed_list = subj_no + "-" + subjno_staffno_splts[ind_subjs];


                                        }
                                    }
                                }
                                string lbl1 = "P" + insert_val + "Val";
                                string lbl2 = "PVal" + insert_val;
                                string lbl3 = "s" + insert_val + "Val";
                                row++;
                                if (count == 1)
                                {
                                    drNew = dtTTDisp.NewRow();
                                    dtTTDisp.Rows.Add(drNew);

                                }
                                else if (dtTTDisp.Rows.Count <= row)
                                {
                                    drNew = dtTTDisp.NewRow();
                                    dtTTDisp.Rows.Add(drNew);
                                }
                                if (SchOrder == 1)
                                {
                                    drNew["DateDisp"] = dayName;
                                    drNew["DateVal"] = dayAcronym;
                                }
                                else
                                {
                                    int dayNo = day_list + 1;
                                    drNew["DateDisp"] = "Day " + dayNo;
                                    drNew["DateVal"] = dayNo;
                                    drNew["dayacram"] = dayAcronym;
                                }
                                
                                if (dtTTDisp.Rows.Count <= ind_subjs - 1)
                                {
                                    drNew = dtTTDisp.NewRow();
                                    dtTTDisp.Rows.Add(drNew);

                                }

                                dtTTDisp.Rows[row - 1][lbl1] = acro;
                                dtTTDisp.Rows[row - 1][lbl2] = shed_list;
                                dtTTDisp.Rows[row - 1][lbl3] = staffname;


                            }
                        }
                    }
                }
                if (dtTTDisp.Rows.Count > 0)
                {
                    GridView2.DataSource = dtTTDisp;
                    GridView2.DataBind();
                    GridView2.Visible = true;
                    btnSave.Visible = true;

                    if (intNHrs > 0)
                    {
                        for (int i = 1; i <= intNHrs * 3; i++)
                        {
                            GridView2.Columns[i].Visible = true;
                        }
                    }
                }
                for (int rows = GridView2.Rows.Count - 1; rows > 0; rows--)
                {
                    GridViewRow roww = GridView2.Rows[rows];
                    GridViewRow previousRow = GridView2.Rows[rows - 1];

                    Label dayy = (Label)roww.FindControl("lblDateDisp");
                    string day = dayy.Text;
                    Label predayy = (Label)previousRow.FindControl("lblDateDisp");
                    string preday = predayy.Text;

                    for (int cell = 0; cell < 1; cell++)
                    {
                        //Label stafnme = (Label)roww.FindControl("lnkPeriod_" + (cell + 1) + "");
                        //string staffname = stafnme.Text;
                        //Label prestanme = (Label)previousRow.FindControl("lnkPeriod_" + (cell + 1) + "");
                        //string prestaffname = prestanme.Text;
                        if (GridView2.Columns[cell].ToString().Trim() == "Day")// || GridView2.Columns[cell].ToString().Trim() == "Staff Name")
                        {

                            if (day == preday)
                            {
                                if (previousRow.Cells[cell].RowSpan == 0)
                                {
                                    if (roww.Cells[cell].RowSpan == 0)
                                    {
                                        previousRow.Cells[cell].RowSpan += 2;
                                    }
                                    else
                                    {
                                        previousRow.Cells[cell].RowSpan = roww.Cells[cell].RowSpan + 1;
                                    }
                                    roww.Cells[cell].Visible = false;
                                }
                                //if (staffname == "" || prestaffname == "")
                                //{
                                //    if (previousRow.Cells[cell].RowSpan == 0)
                                //    {
                                //        if (roww.Cells[cell].RowSpan == 0)
                                //        {
                                //            previousRow.Cells[cell].RowSpan += 2;
                                //        }
                                //        else
                                //        {
                                //            previousRow.Cells[cell].RowSpan = roww.Cells[cell].RowSpan + 1;
                                //        }
                                //        roww.Cells[cell].Visible = false;
                                //    }
                                //}
                            }
                        }
                    }
                }


                for (int rows = GridView2.Rows.Count - 1; rows > 0; rows--)
                {
                    GridViewRow roww = GridView2.Rows[rows];
                    GridViewRow previousRow = GridView2.Rows[rows - 1];
                    int cellval = 0;


                    for (int cell = 0; cell < (inofhours * 3) + 1; cell++)
                    {

                        if (GridView2.Columns[cell].ToString().Trim() == "Subject Name")
                        {
                            cellval++;
                            Label stafnme = (Label)roww.FindControl("lnkPeriod_" + (cellval) + "");
                            string staffname = stafnme.Text;
                            Label prestanme = (Label)previousRow.FindControl("lnkPeriod_" + (cellval) + "");
                            string prestaffname = prestanme.Text;

                            if (staffname == "")//|| prestaffname == ""
                            {
                                if (previousRow.Cells[cell].RowSpan == 0)
                                {
                                    if (roww.Cells[cell].RowSpan == 0)
                                    {
                                        previousRow.Cells[cell].RowSpan += 2;
                                        previousRow.Cells[cell + 2].RowSpan += 2;
                                        previousRow.Cells[cell + 1].RowSpan += 2;

                                    }
                                    else
                                    {
                                        previousRow.Cells[cell].RowSpan = roww.Cells[cell].RowSpan + 1;
                                        previousRow.Cells[cell + 2].RowSpan = roww.Cells[cell + 2].RowSpan + 1;
                                        previousRow.Cells[cell + 1].RowSpan = roww.Cells[cell + 1].RowSpan + 1;
                                    }
                                    roww.Cells[cell].Visible = false;
                                    roww.Cells[cell + 2].Visible = false;
                                    roww.Cells[cell + 1].Visible = false;

                                }
                            }

                        }
                    }
                }


                for (int rows = GridView2.Rows.Count - 1; rows > 0; rows--)
                {
                    GridViewRow roww = GridView2.Rows[rows];
                    GridViewRow previousRow = GridView2.Rows[rows - 1];
                    int cellval = 0;


                    for (int cell = 0; cell < (inofhours * 3) + 1; cell++)
                    {

                        if (GridView2.Columns[cell].ToString().Trim() == "Subject Name")
                        {
                            cellval++;
                            Label stafnme = (Label)roww.FindControl("lnkPeriod_" + (cellval) + "");
                            string staffname = stafnme.Text;
                            Label prestanme = (Label)previousRow.FindControl("lnkPeriod_" + (cellval) + "");
                            string prestaffname = prestanme.Text;

                            if (staffname == prestaffname)
                            {
                                if (previousRow.Cells[cell].RowSpan == 0)
                                {
                                    if (roww.Cells[cell].RowSpan == 0)
                                    {
                                        previousRow.Cells[cell].RowSpan += 2;


                                    }
                                    else
                                    {
                                        previousRow.Cells[cell].RowSpan = roww.Cells[cell].RowSpan + 1;

                                    }
                                    roww.Cells[cell].Visible = false;


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
    public void roomallotment()
    {
        try
        {

            GridView2.Visible = true;
            DataTable dtroom = new DataTable();
            dtroom.Columns.Add("Room_name");
            dtroom.Columns.Add("Roompk");
         //   string roomname = " select Room_name,roompk from Room_Detail  where Building_Name in( select Building_Name from Building_Master where code not in (select HostelBuildingFK from HM_HostelMaster ))";
            string roomname = " select Room_name,roompk from Room_Detail where college_code='"+Convert.ToString(ddlcolg.SelectedValue)+"'";
            DataSet dsroom = d2.select_method_wo_parameter(roomname, "text");

            if (dsroom.Tables.Count > 0 && dsroom.Tables[0].Rows.Count > 0)
            {
                for (int s = 0; s < dsroom.Tables[0].Rows.Count; s++)
                {
                    DataRow drroom = dtroom.NewRow();
                    drroom["Room_name"] = dsroom.Tables[0].Rows[s]["Room_name"];
                    drroom["roompk"] = dsroom.Tables[0].Rows[s]["roompk"];
                    dtroom.Rows.Add(drroom);
                }

            }

            string selquer = "select * from Semester_Schedule_room where degree_code='" + ddlbranch.SelectedValue + "' and  batch_year='" + ddlbatch.SelectedValue + "' and semester='" + ddlsem.SelectedValue + "' and Sections='" + ddlsec.SelectedValue + "'";
            DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
            int count = 0;
            Hashtable htable = new Hashtable();
            htable.Add(1, 1);
            for (int rowI = 0; rowI < GridView2.Rows.Count; rowI++)
            {
                count++;
                for (int colI = 1; colI <= 8; colI++)
                {
                    // (gridSelTT.Rows[rowI].FindControl("ddlr" + colI) as DropDownList).Items.Clear();
                    if (dtroom.Rows.Count > 0)
                    {
                       

                          

                            (GridView2.Rows[rowI].FindControl("ddlr" + colI) as DropDownList).DataSource = dsroom;
                            (GridView2.Rows[rowI].FindControl("ddlr" + colI) as DropDownList).DataTextField = "room_name";// Convert.ToString(dsroom.Tables[0].Rows[ro]["room_name"]);
                            (GridView2.Rows[rowI].FindControl("ddlr" + colI) as DropDownList).DataValueField = "roompk";// Convert.ToString(dsroom.Tables[0].Rows[ro]["roompk"]);
                            (GridView2.Rows[rowI].FindControl("ddlr" + colI) as DropDownList).DataBind();
                            (GridView2.Rows[rowI].FindControl("ddlr" + colI) as DropDownList).Items.Insert(0, "");
                            Label dayy = (Label)GridView2.Rows[rowI].FindControl("dayacr");
                            string day = dayy.Text;
                        string days = string.Empty;
                        if (rowI != 0)
                        {
                            Label dayys = (Label)GridView2.Rows[rowI - 1].FindControl("dayacr");
                            days = dayys.Text;
                        }
                        if (day != days)
                            count = 1;
                      

                            if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
                            {

                                string roomfil = Convert.ToString(ScheduleDataSet.Tables[0].Rows[0][day + colI]);
                                string[] spl = roomfil.Split(';');
                                if (roomfil!="")
                                {
                                if (spl.Length >= 1)
                                {

                                    for (int cn = 0; cn < spl.Length; cn++)
                                        {
                                            if (count - 1 < spl.Length)
                                            {
                                                string[] splroom = spl[count - 1].Split('-');


                                                if (splroom.Length == 3)
                                                {
                                                    //if (htable.Contains(count))
                                                    //{
                                                    (GridView2.Rows[rowI].FindControl("ddlr" + colI) as DropDownList).SelectedIndex = (GridView2.Rows[rowI].FindControl("ddlr" + colI) as DropDownList).Items.IndexOf((GridView2.Rows[rowI].FindControl("ddlr" + colI) as DropDownList).Items.FindByValue(splroom[2]));
                                                }
                                            }
                                                //}
                                            
                                        }
                                    
                                        
                                }
                            }
                            }
                    }
                }
                if (count == 3)
                {
                    count = 0;
                }
            }
        }
        catch
        {
        }
    }
    //protected void gridview2_DataBound(object sender, GridViewRowEventArgs e)
    //{

    //    //if (e.Row.RowType == DataControlRowType.Header)
    //    //{

    //    //    GridView HeaderGrid = (GridView)sender;
    //    //    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    //    //    TableCell headerCell = new TableCell();


    //    //    Table table = (Table)GridView2.Controls[0];
    //    //    TableRow headerRow = table.Rows[0];
    //    //    TableRow headerRow = table.Rows[0];
    //    //    TableCell headerCell = headerRow.Cells[0];
    //    //    int numberOfHeaderCellsToMove = headerRow.Cells.Count - 1;

    //    //    for (int i = 0; i < 1; i++)
    //    //    {
    //    //        headerCell = headerRow.Cells[0];
    //    //        headerRow.Cells.RemoveAt(0);
    //    //        HeaderGridRow.Cells.Add(headerCell);
    //    //        headerCell.RowSpan = 2;
    //    //        TableRow headerrow1 = headerRow.Cells[0];
    //    //    }
    //    //    GridView2.Controls[0].Controls.AddAt(0, HeaderGridRow);
    //    //    GridView Header = (GridView)sender;

    //    //    headerCell = headerRow.Cells[numberOfHeaderCellsToMove];
    //    //    HeaderGridRow.Cells.Add(headerCell);
    //    //    headerCell.RowSpan = 2;
    //    //    gview.Controls[0].Controls.AddAt(0, HeaderGridRow);


    //    //    TableHeaderCell HeaderCell = new TableHeaderCell();
    //    //    HeaderCell.Text = "";
    //    //    HeaderCell.ColumnSpan = tempt;
    //    //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //    //    HeaderGridRow.Cells.Add(HeaderCell);
    //    //    gview.Controls[0].Controls.AddAt(0, HeaderGridRow);
    //    //    for (int no = 1; no <= inofhours; no++)
    //    //    {
    //    //        HeaderCell = new TableHeaderCell();
    //    //        HeaderCell.Text = "Period " + no + "";
    //    //        HeaderCell.ColumnSpan = 2;
    //    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //    //        HeaderGridRow.Cells.Add(HeaderCell);
    //    //        GridView2.Controls[0].Controls.AddAt(0, HeaderGridRow);
    //    //    }





    //    //    int x = 0;
    //    //    DataTable ss = new DataTable();
    //    //    numberOfHeaderCellsToMove = headerRow.Cells.Count - 1;

    //    //    gview.Controls[0].Controls.AddAt(0, HeaderGridRow);
    //    //    headerCell = new TableCell();
    //    //    numberOfHeaderCellsToMove = headerRow.Cells.Count - 1;
    //    //    for (int i = numberOfHeaderCellsToMove; i <= numberOfHeaderCellsToMove; i++)
    //    //    {
    //    //        headerCell = headerRow.Cells[numberOfHeaderCellsToMove];
    //    //        headerCell.RowSpan = 2;
    //    //        headerCell.HorizontalAlign = HorizontalAlign.Center;
    //    //        HeaderGridRow.Cells.Add(headerCell);
    //    //    }
    //    //    HeaderCell.Text = "";

    //    //    GridView2.Controls[0].Controls.AddAt(0, HeaderGridRow);

    //    //    GridView2.Controls[0].Controls.AddAt(0, HeaderGridRow);


    //    //}



    //    //if (e.Row.RowType == DataControlRowType.Header)
    //    //{
    //    //    //For first column set to 200 px
    //    //    TableCell cell = new TableCell();
    //    //    cell = e.Row.Cells[5];
    //    //    //Dim cell As TableCell = e.Row.Cells(0)
    //    //    cell.Width = new Unit("200px");
    //    //}

    //}


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string dayvalue = string.Empty;
            string[] roomsplit;


            string dayroom = string.Empty;
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string[] Daymon = new string[7] { "Monday", "Tuesday", "wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            //for (int day = 0; day < noofdays; day++)
            //{
            //    string dayofweek = Days[day];
            //    string dayofweek1 = Daymon[day];
            //    int daysetweek = day + 2;

            //    if (day == noofdays)
            //    {
            //        daysetweek = 1;
            //    }
            //    for (int i = 1; i <= inofhours; i++)
            //    {
            //        if (dayvalue == "")
            //        {
            //            dayvalue = dayofweek + i;
            //        }
            //        else
            //        {
            //            dayvalue = dayvalue + ',' + dayofweek + i;
            //        }
            //    }
            //}

            string stffroom = string.Empty;
            string dayname = string.Empty;
            string staffname = string.Empty;
            string daynames = string.Empty;
            string roompk = string.Empty;
            if (GridView2.Rows.Count > 0)
            {
                for (int a = 0; a < GridView2.Rows.Count; a++)
                {
                    GridViewRow roww = GridView2.Rows[a];
                    int cellvals = 0;
                    int cellroom = 0;
                    for (int col = 1; col < (inofhours * 3) + 1; col++)
                    {
                        if (GridView2.Columns[col].ToString().Trim() == "Subject Name")
                        {
                            cellvals++;
                            Label stafnme = (Label)roww.FindControl("lblPeriod_" + (cellvals) + "");
                            staffname = stafnme.Text;
                            Label day = (Label)roww.FindControl("lblDateDisp");
                            dayname = day.Text;
                            Label dayval = (Label)roww.FindControl("lblDayVal");
                            daynames = dayval.Text;

                            //Label stafnmes = (Label)roww.FindControl("lnkPeriod_" + (cellvals) + "");
                            //string staffnames = stafnmes.Text;
                            //    roomsplit = staffname.Split('-');
                            //    if (roomsplit.Length == 2)
                            //    {
                            //        if (dayroom == "")
                            //            dayroom = roomsplit[0] + '-' + roomsplit[1];
                            //        else
                            //            dayroom = stffroom + ';' + roomsplit[0] + '-' + roomsplit[1];

                            //    }

                        }

                        else if (GridView2.Columns[col].ToString().Trim() == "Room")
                        {
                            cellroom++;



                            DropDownList room = (DropDownList)roww.FindControl("ddlr" + cellroom);
                            
                                roompk = room.SelectedItem.Value;

                                //if (stffroom == "")
                                //    stffroom = dayroom + '-' + roompk;
                                //else
                                //    stffroom = dayroom + '-' + roompk;


                                if (!has.ContainsKey(dayname + "-" + "Period" + cellvals))
                                {
                                    has.Add(dayname + "-" + "Period" + cellvals, staffname + "-" + roompk);
                                }
                                else
                                {
                                    string val = has[dayname + "-" + "Period" + cellvals].ToString();
                                    if (staffname != "")
                                    {
                                        has.Remove(dayname + "-" + "Period" + cellvals);
                                        has.Add(dayname + "-" + "Period" + cellvals, val + ";" + staffname + "-" + roompk);
                                    }

                                }
                            
                        }

                    }
                }
            }
            string hasvalue = string.Empty;
            string dayst = string.Empty;
            string perio = string.Empty;


      

            for (int hor = 0; hor < noofdays; hor++)
            {
                string dayofweek = Days[hor];
                string dayofweek1 = Daymon[hor];
                int daysetweek = hor + 2;

                if (hor == noofdays)
                {
                    daysetweek = 1;
                }
                for (int per = 1; per <= inofhours; per++)
                {
                   
                    dayst = Convert.ToString(hor+1);
                    perio = "Day " + dayst + "-" + "Period" + per;
                    if (has.ContainsKey(perio))
                    {
                        if (dayvalue == "")
                        {
                            dayvalue = dayofweek + per;
                        }
                        else
                        {
                            dayvalue = dayvalue + ',' + dayofweek + per;
                        }
                    }
                    if (hasvalue == "")
                    {
                        if (has.ContainsKey(perio))
                        {
                            hasvalue = "" + has[perio].ToString() + "";
                        }
                       
                    }
                    else
                    {
                        if (has.ContainsKey(perio))
                        {
                            hasvalue = hasvalue + "'" + "," + "'" + has[perio].ToString() + "";
                        }
                    }
                }
            }
            string selquer = "select * from Semester_Schedule_room where degree_code='" + ddlbranch.SelectedValue + "' and  batch_year='" + ddlbatch.SelectedValue + "' and semester='" + ddlsem.SelectedValue + "' and Sections='" + ddlsec.SelectedValue + "'";
            DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
            if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
            {
                string delet = "delete from Semester_Schedule_room where degree_code='" + ddlbranch.SelectedValue + "' and  batch_year='" + ddlbatch.SelectedValue + "' and semester='" + ddlsem.SelectedValue + "' and Sections='" + ddlsec.SelectedValue + "'";
                int del = d2.update_method_wo_parameter(delet, "Text");
            }
            string roominsert = "insert into Semester_Schedule_room (" + dayvalue + ",degree_code,semester,batch_year,Sections) values ('" + hasvalue + "','" + ddlbranch.SelectedValue + "','" + ddlsem.SelectedValue + "', '" + ddlbatch.SelectedValue + "','" + ddlsec.SelectedValue + "')";

            int ins = d2.update_method_wo_parameter(roominsert, "Text");
            if(ins==1)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
        }
        catch
        {

        }
    }

    protected void ddlr1_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        int getval = 0;
        int rowIndex = rowindexs;
        DropDownList grids = (DropDownList)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
        rowindexs = rowIndx;
     //   Label dayy = (GridView2.SelectedRow.FindControl("lblDayVal") as Label);
           // Rows[rowIndex].FindControl("lblDayVal");
        Label dayy = (Label)GridView2.Rows[rowIndx].FindControl("lblDayVal");
        string day = dayy.Text;
        int.TryParse(day, out getval);
        GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        DropDownList duty = (DropDownList)gvr.FindControl("ddlr1");
        dropvalue = "ddlr1";
        string drop = duty.SelectedItem.Value;
        string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        string dayval = Days[getval-1];
        string selquer = "select "+dayval+"1 from Semester_Schedule_room";
        DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
        if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
        {
            for(int a=0;a<ScheduleDataSet.Tables[0].Rows.Count;a++)
            {
                string getroom = Convert.ToString(ScheduleDataSet.Tables[0].Rows[a][dayval + "1"]);
                string[] spl = getroom.Split(';');
                if (spl.Length >= 1)
                {
                    for (int cn = 0; cn < spl.Length; cn++)
                    {
                        string[] splroom = spl[cn].Split('-');
                        if (splroom.Length == 3)
                        {
                            if (drop == splroom[2])
                            {
                                imgdiv2.Visible = true;
                                lbl_alerterror.Text = "Room Already Selected,Do You Want to Change Room";
                                //((DropDownList)GridView2.FindControl(dropvalue) as DropDownList).SelectedIndex = ((DropDownList)GridView2.FindControl(dropvalue) as DropDownList).Items.IndexOf(((DropDownList)GridView2.FindControl(dropvalue) as DropDownList).Items.FindByText(""));
                               // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Room Already Selected')", true );
                                return;
                            }
                        }
                    }
                }

            }
        }
       
    }
    protected void ddlr2_SelectedIndexChanged(object sender, EventArgs e)
    {

        int getval = 0;
        int rowIndex = rowindexs;
        DropDownList grids = (DropDownList)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
        rowindexs = rowIndx;
        Label dayy = (Label)GridView2.Rows[rowIndx].FindControl("lblDayVal");
        string day = dayy.Text;
        int.TryParse(day, out getval);
        GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        DropDownList duty = (DropDownList)gvr.FindControl("ddlr2");
        dropvalue = "ddlr2";
        string drop = duty.SelectedItem.Value;
        string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        string dayval = Days[getval - 1];
        string selquer = "select " + dayval + "2 from Semester_Schedule_room";
        DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
        if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
        {
            for (int a = 0; a < ScheduleDataSet.Tables[0].Rows.Count; a++)
            {
                string getroom = Convert.ToString(ScheduleDataSet.Tables[0].Rows[a][dayval + "2"]);
                string[] spl = getroom.Split(';');
                if (spl.Length >= 1)
                {
                    for (int cn = 0; cn < spl.Length; cn++)
                    {
                        string[] splroom = spl[cn].Split('-');
                        if (splroom.Length == 3)
                        {
                            if (drop == splroom[2])
                            {
                                imgdiv2.Visible = true;
                                lbl_alerterror.Text = "Room Already Selected,Do You Want to Change Room";
                                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Room Already Selected')", true );
                                return;
                            }
                        }
                    }
                }

            }
        }
       
    }
    protected void ddlr3_SelectedIndexChanged(object sender, EventArgs e)
    {
        int getval = 0;
        int rowIndex = rowindexs;
        DropDownList grids = (DropDownList)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
        rowindexs = rowIndx;
        Label dayy = (Label)GridView2.Rows[rowIndx].FindControl("lblDayVal");
        
        string day = dayy.Text;
        int.TryParse(day, out getval);
        GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        DropDownList duty = (DropDownList)gvr.FindControl("ddlr3");
        dropvalue = "ddlr3";
        string drop = duty.SelectedItem.Value;
        string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        string dayval = Days[getval - 1];
        string selquer = "select " + dayval + "3 from Semester_Schedule_room";
        DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
        if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
        {
            for (int a = 0; a < ScheduleDataSet.Tables[0].Rows.Count; a++)
            {
                string getroom = Convert.ToString(ScheduleDataSet.Tables[0].Rows[a][dayval + "3"]);
                string[] spl = getroom.Split(';');
                if (spl.Length >= 1)
                {
                    for (int cn = 0; cn < spl.Length; cn++)
                    {
                        string[] splroom = spl[cn].Split('-');
                        if (splroom.Length == 3)
                        {
                            if (drop == splroom[2])
                            {
                                imgdiv2.Visible = true;
                                lbl_alerterror.Text = "Room Already Selected,Do You Want to Change Room";
                                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Room Already Selected')", true );
                                return;
                            }
                        }
                    }
                }

            }
        }
       
    }
    protected void ddlr4_SelectedIndexChanged(object sender, EventArgs e)
    {
        int getval = 0;
        int rowIndex = rowindexs;
        DropDownList grids = (DropDownList)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
        rowindexs = rowIndx;
        Label dayy = (Label)GridView2.Rows[rowIndx].FindControl("lblDayVal");
        string day = dayy.Text;
        int.TryParse(day, out getval);
        GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        DropDownList duty = (DropDownList)gvr.FindControl("ddlr4");
        dropvalue = "ddlr4";
        string drop = duty.SelectedItem.Value;
        string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        string dayval = Days[getval - 1];
        string selquer = "select " + dayval + "4 from Semester_Schedule_room";
        DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
        if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
        {
            for (int a = 0; a < ScheduleDataSet.Tables[0].Rows.Count; a++)
            {
                string getroom = Convert.ToString(ScheduleDataSet.Tables[0].Rows[a][dayval + "4"]);
                string[] spl = getroom.Split(';');
                if (spl.Length >= 1)
                {
                    for (int cn = 0; cn < spl.Length; cn++)
                    {
                        string[] splroom = spl[cn].Split('-');
                        if (splroom.Length == 3)
                        {
                            if (drop == splroom[2])
                            {
                                imgdiv2.Visible = true;
                                lbl_alerterror.Text = "Room Already Selected,Do You Want to Change Room";
                                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Room Already Selected')", true );
                                return;
                            }
                        }
                    }
                }

            }
        }
       
    }
    protected void ddlr5_SelectedIndexChanged(object sender, EventArgs e)
    {
        int getval = 0;
        int rowIndex = rowindexs;
        DropDownList grids = (DropDownList)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
        rowindexs = rowIndx;
        Label dayy = (Label)GridView2.Rows[rowIndx].FindControl("lblDayVal");
        string day = dayy.Text;
        int.TryParse(day, out getval);
        GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        DropDownList duty = (DropDownList)gvr.FindControl("ddlr5");
        dropvalue = "ddlr5";
        string drop = duty.SelectedItem.Value;
        string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        string dayval = Days[getval - 1];
        string selquer = "select " + dayval + "5 from Semester_Schedule_room";
        DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
        if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
        {
            for (int a = 0; a < ScheduleDataSet.Tables[0].Rows.Count; a++)
            {
                string getroom = Convert.ToString(ScheduleDataSet.Tables[0].Rows[a][dayval + "5"]);
                string[] spl = getroom.Split(';');
                if (spl.Length >= 1)
                {
                    for (int cn = 0; cn < spl.Length; cn++)
                    {
                        string[] splroom = spl[cn].Split('-');
                        if (splroom.Length == 3)
                        {
                            if (drop == splroom[2])
                            {
                                imgdiv2.Visible = true;
                                lbl_alerterror.Text = "Room Already Selected,Do You Want to Change Room";
                                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Room Already Selected')", true );
                                return;
                            }
                        }
                    }
                }

            }
        }
       
    }
    protected void ddlr6_SelectedIndexChanged(object sender, EventArgs e)
    {
        int getval = 0;
        int rowIndex = rowindexs;
        DropDownList grids = (DropDownList)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
        rowindexs = rowIndx;
        Label dayy = (Label)GridView2.Rows[rowIndx].FindControl("lblDayVal");
        string day = dayy.Text;
        int.TryParse(day, out getval);
        GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        DropDownList duty = (DropDownList)gvr.FindControl("ddlr6");
        dropvalue = "ddlr6";
        string drop = duty.SelectedItem.Value;
        string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        string dayval = Days[getval - 1];
        string selquer = "select " + dayval + "6 from Semester_Schedule_room";
        DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
        if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
        {
            for (int a = 0; a < ScheduleDataSet.Tables[0].Rows.Count; a++)
            {
                string getroom = Convert.ToString(ScheduleDataSet.Tables[0].Rows[a][dayval + "6"]);
                string[] spl = getroom.Split(';');
                if (spl.Length >= 1)
                {
                    for (int cn = 0; cn < spl.Length; cn++)
                    {
                        string[] splroom = spl[cn].Split('-');
                        if (splroom.Length == 3)
                        {
                            if (drop == splroom[2])
                            {
                                imgdiv2.Visible = true;
                                lbl_alerterror.Text = "Room Already Selected,Do You Want to Change Room";
                                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Room Already Selected')", true );
                                return;
                            }
                        }
                    }
                }

            }
        }
       
    }
    protected void ddlr7_SelectedIndexChanged(object sender, EventArgs e)
    {
        int getval = 0;
        int rowIndex = rowindexs;
        DropDownList grids = (DropDownList)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
        rowindexs = rowIndx;
        Label dayy = (Label)GridView2.Rows[rowIndx].FindControl("lblDayVal");
        string day = dayy.Text;
        int.TryParse(day, out getval);
        GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        DropDownList duty = (DropDownList)gvr.FindControl("ddlr7");
        dropvalue = "ddlr7";
        string drop = duty.SelectedItem.Value;
        string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        string dayval = Days[getval - 1];
        string selquer = "select " + dayval + "7 from Semester_Schedule_room";
        DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
        if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
        {
            for (int a = 0; a < ScheduleDataSet.Tables[0].Rows.Count; a++)
            {
                string getroom = Convert.ToString(ScheduleDataSet.Tables[0].Rows[a][dayval + "7"]);
                string[] spl = getroom.Split(';');
                if (spl.Length >= 1)
                {
                    for (int cn = 0; cn < spl.Length; cn++)
                    {
                        string[] splroom = spl[cn].Split('-');
                        if (splroom.Length == 3)
                        {
                            if (drop == splroom[2])
                            {
                                imgdiv2.Visible = true;
                                lbl_alerterror.Text = "Room Already Selected,Do You Want to Change Room";
                                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Room Already Selected')", true );
                                return;
                            }
                        }
                    }
                }

            }
        }
       
    }
    protected void ddlr8_SelectedIndexChanged(object sender, EventArgs e)
    {
      
        int getval = 0;
        int rowIndex = rowindexs;
        DropDownList grids = (DropDownList)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
        rowindexs = rowIndx;
        Label dayy = (Label)GridView2.Rows[rowIndx].FindControl("lblDayVal");
        string day = dayy.Text;
        int.TryParse(day, out getval);
        GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        DropDownList duty = (DropDownList)gvr.FindControl("ddlr8");
        dropvalue = "ddlr8";
        string drop = duty.SelectedItem.Value;
        string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        string dayval = Days[getval - 1];
        string selquer = "select " + dayval + "8 from Semester_Schedule_room";
        DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
        if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
        {
            for (int a = 0; a < ScheduleDataSet.Tables[0].Rows.Count; a++)
            {
                string getroom = Convert.ToString(ScheduleDataSet.Tables[0].Rows[a][dayval + "8"]);
                string[] spl = getroom.Split(';');
                if (spl.Length >= 1)
                {
                    for (int cn = 0; cn < spl.Length; cn++)
                    {
                        string[] splroom = spl[cn].Split('-');
                        if (splroom.Length == 3)
                        {
                            if (drop == splroom[2])
                            {
                                imgdiv2.Visible = true;
                                lbl_alerterror.Text = "Room Already Selected,Do You Want to Change Room";
                                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Room Already Selected')", true );
                                return;
                            }
                        }
                    }
                }

            }
        }
       
    }
    protected void ddlr9_SelectedIndexChanged(object sender, EventArgs e)
    {
        int getval = 0;
        int rowIndex = rowindexs;
        DropDownList grids = (DropDownList)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
        rowindexs = rowIndx;
        Label dayy = (Label)GridView2.Rows[rowIndx].FindControl("lblDayVal");
        string day = dayy.Text;
        int.TryParse(day, out getval);
        GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        DropDownList duty = (DropDownList)gvr.FindControl("ddlr9");
        dropvalue = "ddlr9";
        string drop = duty.SelectedItem.Value;
        string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        string dayval = Days[getval - 1];
        string selquer = "select " + dayval + "9 from Semester_Schedule_room";
        DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
        if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
        {
            for (int a = 0; a < ScheduleDataSet.Tables[0].Rows.Count; a++)
            {
                string getroom = Convert.ToString(ScheduleDataSet.Tables[0].Rows[a][dayval + "9"]);
                string[] spl = getroom.Split(';');
                if (spl.Length >= 1)
                {
                    for (int cn = 0; cn < spl.Length; cn++)
                    {
                        string[] splroom = spl[cn].Split('-');
                        if (splroom.Length == 3)
                        {
                            if (drop == splroom[2])
                            {
                                imgdiv2.Visible = true;
                                lbl_alerterror.Text = "Room Already Selected,Do You Want to Change Room";
                                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Room Already Selected')", true );
                                return;
                            }
                        }
                    }
                }

            }
        }
       
    }
    protected void ddlr10_SelectedIndexChanged(object sender, EventArgs e)
    {
        int getval = 0;
        int rowIndex = rowindexs;
        DropDownList grids = (DropDownList)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
        rowindexs = rowIndx;
        Label dayy = (Label)GridView2.Rows[rowIndx].FindControl("lblDayVal");
        string day = dayy.Text;
        int.TryParse(day, out getval);
        GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        DropDownList duty = (DropDownList)gvr.FindControl("ddlr10");
        dropvalue = "ddlr10";
        string drop = duty.SelectedItem.Value;
        string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        string dayval = Days[getval - 1];
        string selquer = "select " + dayval + "10 from Semester_Schedule_room";
        DataSet ScheduleDataSet = dacess.select_method_wo_parameter(selquer, "Text");
        if (ScheduleDataSet.Tables.Count > 0 && ScheduleDataSet.Tables[0].Rows.Count > 0)
        {
            for (int a = 0; a < ScheduleDataSet.Tables[0].Rows.Count; a++)
            {
                string getroom = Convert.ToString(ScheduleDataSet.Tables[0].Rows[a][dayval + "10"]);
                string[] spl = getroom.Split(';');
                if (spl.Length >= 1)
                {
                    for (int cn = 0; cn < spl.Length; cn++)
                    {
                        string[] splroom = spl[cn].Split('-');
                        if (splroom.Length == 3)
                        {
                            if (drop == splroom[2])
                            {
                                imgdiv2.Visible = true;
                                lbl_alerterror.Text = "Room Already Selected,Do You Want to Change Room";
                                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Room Already Selected')", true );
                                return;
                            }
                        }
                    }
                }

            }
        }
       
    }





    protected void gridview2_OnRowCreated(object sender, GridViewRowEventArgs e)
    {



        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                //TableCell cell = e.Row.Cells[i];
                //cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                //cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                //cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                //   , SelectedGridCellIndex.ClientID, i
                //   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
                 e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView2, "Select$" + e.Row.RowIndex);
     
            }
        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        rowindexs = rowIndex;
        int colIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

        //Label dayy = (Label)GridView2.Rows[0].FindControl("lblDateDisp");
        //string day = dayy.Text;
        //GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        //DropDownList duty = (DropDownList)gvr.FindControl("ddlr1");
        //string drop = duty.SelectedItem.Value;

    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void Btncancle_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        //GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
        //DropDownList duty = (DropDownList)gvr.FindControl("ddlr4");
        //ddlmess.SelectedIndex = ddlmess.Items.IndexOf(ddlmess.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["Messcode"])));
       // DropDownList ddl= ((DropDownList)GridView2.FindControl("ddlr1") as DropDownList).SelectedIndex;// = ((DropDownList)GridView2.FindControl("ddlr1") as DropDownList).Items.IndexOf(((DropDownList)GridView2.FindControl("ddlr1") as DropDownList).Items.FindByText("101"));

        (GridView2.Rows[rowindexs].FindControl(dropvalue) as DropDownList).SelectedIndex = (GridView2.Rows[rowindexs].FindControl(dropvalue) as DropDownList).Items.IndexOf((GridView2.Rows[rowindexs].FindControl(dropvalue) as DropDownList).Items.FindByText(""));

      
    }
}
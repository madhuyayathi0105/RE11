using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.IO;
using AjaxControlToolkit;
using System.Web.Services;
using System.Data.SqlClient;
using Gios.Pdf;
using System.Web.UI.DataVisualization;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.DataVisualization.Charting.ChartTypes;

public partial class DepartmentWiseAttendanceReport : System.Web.UI.Page
{
    #region Variable Declaration

    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds1 = new DataSet();
    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds_sphr = new DataSet();
    DataSet dsabper = new DataSet();
    DataSet ds_attnd_pts = new DataSet();
    DataSet dsholiday = new DataSet();
    Dictionary<string, int> dicstupresentdate = new Dictionary<string, int>();
    Dictionary<string, int> dicstuabsentdate = new Dictionary<string, int>();

    
    static ArrayList ht_sphr = new ArrayList();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    Hashtable hatleavecode = new Hashtable();

    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    DateTime dumm_from_date;
    DateTime Admission_date;
    TimeSpan ts;

    string dd = "";
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string diff_date;
    string value, date;
    string tempvalue = "-1";
    string frdate, todate;
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string monthcal;
    ArrayList total_sp_hr_present = new ArrayList();
    ArrayList total_sp_hr_absent = new ArrayList();
    bool pub_splhr_flag = false;


    int mmyycount;
    int moncount;
    int unmark;
    int count = 0;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int next = 0;
    int minpresII = 0;
    int rows_count;
    int ObtValue = -1;
    int cal_from_date, cal_from_date_tmp;
    int cal_to_date, cal_to_date_tmp;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int demfcal, demtcal;

    double dif_date = 0;
    double dif_date1 = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;

    //added by rajasekar 18/09/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    System.Text.StringBuilder present = new System.Text.StringBuilder();
    System.Text.StringBuilder absent = new System.Text.StringBuilder();
    //=================================//

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        lbl_err.Visible = false;
        if (!IsPostBack)
        {
            setLabelText();
            rbdepartment.Text = Ibldegree.Text + " Wise";
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            clear();
            bindbatch();
            binddegree();
            bindbranch();
            BindSectionDetailmult();
            bindperiod();
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            rbdepartment.Checked = true;
            //for (int c = 0; c < chklscolumn.Items.Count; c++)
            //{
            //    chklscolumn.Items[c].Selected = true;
            //}
        }
        if (Iblbranch.Text.Trim().ToLower() == "standard")
        {
            HeaderSapn.InnerHtml = "" + Iblbranch.Text + " & Period Wise Attendance Report";
        }
        else
        {
            HeaderSapn.InnerHtml = "Department & Period Wise Attendance Report";
        }

    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        //lbl.Add(lbl_clgT);
        lbl.Add(Ibldegree);
        lbl.Add(Iblbranch);
        //lbl.Add(lbl_semT);
        //fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        //fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    public void bindbatch()
    {
        try
        {
            Chklst_batch.Items.Clear();
            Chk_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Chklst_batch.DataSource = ds;
                Chklst_batch.DataTextField = "batch_year";
                Chklst_batch.DataValueField = "batch_year";
                Chklst_batch.DataBind();
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = true;
                    count++;
                }
                if (count > 0)
                {
                    txt_batch.Text = "Batch(" + (Chklst_batch.Items.Count) + ")";
                    if (Chklst_batch.Items.Count == count)
                    {
                        Chk_batch.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void binddegree()
    {
        try
        {
            Chklst_degree.Items.Clear();
            txt_degree.Text = "---Select---";
            chk_degree.Checked = false;
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = Session["collegecode"].ToString();
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
            ds = da.select_method("bind_degree", has, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Chklst_degree.DataSource = ds;
                Chklst_degree.DataTextField = "course_name";
                Chklst_degree.DataValueField = "course_id";
                Chklst_degree.DataBind();

                for (int h = 0; h < Chklst_degree.Items.Count; h++)
                {
                    Chklst_degree.Items[h].Selected = true;
                }
                txt_degree.Text = Ibldegree.Text + "(" + Chklst_degree.Items.Count + ")";
                chk_degree.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void bindbranch()
    {
        try
        {
            string degreecode = "";
            txt_branch.Text = "---Select---";
            chk_branch.Checked = false;
            chklst_branch.Items.Clear();
            for (int h = 0; h < Chklst_degree.Items.Count; h++)
            {
                if (Chklst_degree.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = Chklst_degree.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + Chklst_degree.Items[h].Value;
                    }
                }
            }
            if (degreecode.Trim() != "")
            {
                ds.Clear();
                ds = da.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), degreecode, collegecode = Session["collegecode"].ToString(), Session["usercode"].ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_branch.DataSource = ds;
                    chklst_branch.DataTextField = "dept_name";
                    chklst_branch.DataValueField = "degree_code";
                    chklst_branch.DataBind();
                    for (int h = 0; h < chklst_branch.Items.Count; h++)
                    {
                        chklst_branch.Items[h].Selected = true;
                    }
                    txt_branch.Text = Iblbranch.Text + "(" + (chklst_branch.Items.Count) + ")";
                    chk_branch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void BindSectionDetailmult()
    {
        try
        {
            string strbatch = "", strbranch = "";
            int takecount = 0;
            chklstsection.Items.Clear();
            txtsection.Text = "---Select---";
            txtsection.Enabled = false;
            ds.Dispose();
            ds.Reset();
            txtsection.Text = "---Select---";
            for (int i = 0; i < Chklst_batch.Items.Count; i++)
            {
                if (Chklst_batch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + Chklst_batch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + Chklst_batch.Items[i].Value.ToString() + "'";
                    }
                }
            }

            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklst_branch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklst_branch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            int sectioncount = 0;
            if (strbranch.Trim() != "" && strbatch.Trim() != "")
            {
                
                string strsection = "select distinct sections from registration where batch_year in(" + strbatch + ") and degree_code in(" + strbranch + ") and sections<>'-1' and sections<>' ' and delflag=0  and exam_flag <> 'DEBAR' order by sections";
                ds = da.select_method_wo_parameter(strsection, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    takecount = ds.Tables[0].Rows.Count;
                    chklstsection.DataSource = ds;
                    chklstsection.DataTextField = "sections";
                    chklstsection.DataBind();
                    chklstsection.Items.Insert(takecount, "Empty");

                    if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                    {
                    }
                    else
                    {
                        txtsection.Enabled = true;
                        for (int i = 0; i < chklstsection.Items.Count; i++)
                        {
                            chksection.Checked = true;
                            chklstsection.Items[i].Selected = true;
                            sectioncount += 1;
                        }
                        if (sectioncount > 0)
                        {
                            if (chklstsection.Items.Count == sectioncount)
                            {
                                txtsection.Text = "Sec(" + (chklstsection.Items.Count) + ")";
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Text = ex.ToString();
            lbl_err.Visible = true;
        }
    }

    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string fdtae = txtfromdate.Text.ToString();
            string[] spf = fdtae.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdtae = txttodate.Text.ToString();
            string[] spt = tdtae.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            string currdate = DateTime.Now.ToString("MM/dd/yyyy");
            DateTime dtnow = Convert.ToDateTime(currdate);

            if (dtf > dtnow)
            {
                txtfromdate.Text = dtnow.ToString("dd/MM/yyyy");
                txttodate.Text = dtnow.ToString("dd/MM/yyyy");
                lbl_err.Visible = true;
                lbl_err.Text = "From Date Must Be Lesser Than Or Equal To Current Date";
                return;
            }
            if (dtt > dtnow)
            {
                txtfromdate.Text = dtnow.ToString("dd/MM/yyyy");
                txttodate.Text = dtnow.ToString("dd/MM/yyyy");
                lbl_err.Visible = true;
                lbl_err.Text = "From Date Must Be Lesser Than Or Equal To Current Date";
                return;
            }


            if (dtt < dtf)
            {
                txtfromdate.Text = dtt.ToString("dd/MM/yyyy");
                lbl_err.Visible = true;
                lbl_err.Text = "From Date Must Be Lesser Than To Date";
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string fdtae = txtfromdate.Text.ToString();
            string[] spf = fdtae.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdtae = txttodate.Text.ToString();
            string[] spt = tdtae.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            string currdate = DateTime.Now.ToString("MM/dd/yyyy");
            DateTime dtnow = Convert.ToDateTime(currdate);

            if (dtf > dtnow)
            {
                txtfromdate.Text = dtnow.ToString("dd/MM/yyyy");
                txttodate.Text = dtnow.ToString("dd/MM/yyyy");
                lbl_err.Visible = true;
                lbl_err.Text = "From Date Must Be Lesser Than Or Equal To Current Date";
                return;
            }
            if (dtt > dtnow)
            {
                txtfromdate.Text = dtnow.ToString("dd/MM/yyyy");
                txttodate.Text = dtnow.ToString("dd/MM/yyyy");
                lbl_err.Visible = true;
                lbl_err.Text = "From Date Must Be Lesser Than Or Equal To Current Date";
                return;
            }
            if (dtt < dtf)
            {
                txtfromdate.Text = dtt.ToString("dd/MM/yyyy");
                lbl_err.Visible = true;
                lbl_err.Text = "From Date Must Be Lesser Than To Date";
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void cheklist_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txt_degree.Text = "--Select--";
            chk_degree.Checked = false;
            count = 0;
            for (int i = 0; i < Chklst_degree.Items.Count; i++)
            {
                if (Chklst_degree.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_degree.Text = "Degree(" + count.ToString() + ")";
                if (count == Chklst_degree.Items.Count)
                {
                    chk_degree.Checked = true;
                }
            }
            bindbranch();
            BindSectionDetailmult();
            bindperiod();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void chk_branchchanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chk_branch.Checked == true)
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (chklst_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            BindSectionDetailmult();
            bindperiod();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void Chlk_batchchanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (Chk_batch.Checked == true)
            {
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (Chklst_batch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
            }

            binddegree();
            bindbranch();
            BindSectionDetailmult();
            bindperiod();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void Chlk_batchselected(object sender, EventArgs e)
    {
        try
        {
            clear();
            txt_batch.Text = "--Select--";
            count = 0;
            Chk_batch.Checked = false;
            for (int i = 0; i < Chklst_batch.Items.Count; i++)
            {
                if (Chklst_batch.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }

            if (count > 0)
            {
                txt_batch.Text = "Batch(" + count.ToString() + ")";
                if (count == Chklst_batch.Items.Count)
                {
                    Chk_batch.Checked = true;
                }
            }
            binddegree();
            bindbranch();
            BindSectionDetailmult();
            bindperiod();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }


    protected void chklst_branchselected(object sender, EventArgs e)
    {
        try
        {
            clear();
            count = 0;
            chk_branch.Checked = false;
            txt_branch.Text = "--Select--";
            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_branch.Text = "Branch(" + count.ToString() + ")";
                if (count == chklst_branch.Items.Count)
                {
                    chk_branch.Checked = true;
                }
            }
            BindSectionDetailmult();
            bindperiod();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void chksection_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chksection.Checked == true)
            {
                for (int i = 0; i < chklstsection.Items.Count; i++)
                {
                    chklstsection.Items[i].Selected = true;
                    txtsection.Text = "Sec(" + (chklstsection.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstsection.Items.Count; i++)
                {
                    chklstsection.Items[i].Selected = false;
                    txtsection.Text = "---Select---";
                }
            }
            bindperiod();
            clear();
        }
        catch (Exception ex)
        {
            lbl_err.Text = ex.ToString();
            lbl_err.Visible = true;
        }
    }

    protected void chklstsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chksection.Checked = false;
            int sectioncount = 0;
            for (int i = 0; i < chklstsection.Items.Count; i++)
            {
                if (chklstsection.Items[i].Selected == true)
                {
                    sectioncount = sectioncount + 1;
                }
            }
            if (sectioncount > 0)
            {
                txtsection.Text = "Sec(" + sectioncount.ToString() + ")";
                if (chklstsection.Items.Count == sectioncount)
                {
                    chksection.Checked = true;
                }
            }
            bindperiod();
            clear();
        }
        catch (Exception ex)
        {
            lbl_err.Text = ex.ToString();
            lbl_err.Visible = true;
        }
    }

    protected void chklsperiod_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtperiod.Text = "---Select---";
            chkperiod.Checked = false;
            clear();
            count = 0;
            for (int i = 0; i < chklsperiod.Items.Count; i++)
            {
                if (chklsperiod.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                txtperiod.Text = "Period (" + count + ")";
                if (count == chklsperiod.Items.Count)
                {
                    chkperiod.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void chkperiod_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkperiod.Checked == true)
            {
                for (int i = 0; i < chklsperiod.Items.Count; i++)
                {
                    chklsperiod.Items[i].Selected = true;
                }
                txtperiod.Text = "Period (" + (chklsperiod.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsperiod.Items.Count; i++)
                {
                    chklsperiod.Items[i].Selected = false;
                }
                txtperiod.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void checkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chk_degree.Checked == true)
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    Chklst_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (Chklst_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    Chklst_degree.Items[i].Selected = false;
                }
                txt_degree.Text = "--Select--";
            }
            bindbranch();
            bindperiod();
            BindSectionDetailmult();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void clear()
    {
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        txtexcelname.Text = "";
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        btnPrint.Visible = false;
        Showgrid.Visible = false;
        lbl_err.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void chklscolumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    public void bindperiod()
    {
        txtperiod.Text = "---Select---";
        chklsperiod.Items.Clear();
        chkperiod.Checked = false;

        string testbatchyear = "";
        for (int j = 0; j < Chklst_batch.Items.Count; j++)
        {
            if (Chklst_batch.Items[j].Selected == true)
            {
                if (testbatchyear == "")
                {
                    testbatchyear = "'" + Chklst_batch.Items[j].Value.ToString() + "'";
                }
                else
                {
                    testbatchyear = testbatchyear + ",'" + Chklst_batch.Items[j].Value.ToString() + "'";
                }
            }
        }

        string testbranch = "";
        for (int j = 0; j < chklst_branch.Items.Count; j++)
        {
            if (chklst_branch.Items[j].Selected == true)
            {
                if (testbranch == "")
                {
                    testbranch = "'" + chklst_branch.Items[j].Value.ToString() + "'";
                }
                else
                {
                    testbranch = testbranch + ",'" + chklst_branch.Items[j].Value.ToString() + "'";
                }
            }
        }
        if (testbatchyear.Trim() != "" && testbranch.Trim() != "")
        {
            string strgetmaxhours = da.GetFunction("select max(No_of_hrs_per_day) from PeriodAttndSchedule p,Registration r where p.degree_code=r.degree_code and p.semester=r.Current_Semester and r.Batch_Year in(" + testbatchyear + ") and r.degree_code in (" + testbranch + ")");
            int nohrs = 0;
            if (strgetmaxhours.Trim() != "")
            {
                nohrs = Convert.ToInt32(strgetmaxhours);
            }

            for (int hl = 1; hl <= nohrs; hl++)
            {
                chklsperiod.Items.Add(hl.ToString());
            }

            for (int hl = 0; hl < chklsperiod.Items.Count; hl++)
            {
                chklsperiod.Items[hl].Selected = true;
            }
            txtperiod.Text = "Period (" + chklsperiod.Items.Count + ")";
            chkperiod.Checked = true;

        }
    }

    protected void rbreport_CheckedChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {

                da.printexcelreportgrid(Showgrid, reportname);
                lbl_err.Visible = false;
            }
            else
            {
                lbl_err.Text = "Please Enter Your Report Name";
                lbl_err.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string degreedetails = "DAY-WISE STUDENT'S ATTENDANCE@Date : " + txtfromdate.Text + " to " + txttodate.Text + "";
        if (rbdepartment.Checked == true)
        {
            if (Iblbranch.Text.Trim().ToLower() == "standard")
            {
                degreedetails = "STANDARD-WISE STUDENT'S ATTENDANCE@Date : " + txtfromdate.Text + " to " + txttodate.Text + "";
            }
            else
            {
                degreedetails = "DEPARTMENT-WISE STUDENT'S ATTENDANCE@Date : " + txtfromdate.Text + " to " + txttodate.Text + "";
            }
        }
        string pagename = "DepartmentWiseAttendanceReport.aspx";
        //Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        string ss = null;
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            clear();
            Boolean hrflag = false;
            string fdtae = txtfromdate.Text.ToString();
            string[] spf = fdtae.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdtae = txttodate.Text.ToString();
            string[] spt = tdtae.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            if (dtt < dtf)
            {
                lbl_err.Visible = true;
                lbl_err.Text = "From Date Must Be Lesser Than To Date";
                return;
            }

            string getholidayquery = "select * from holidayStudents where holiday_date between '" + dtf.ToString("MM/dd/yyyy") + "' and '" + dtt.ToString("MM/dd/yyyy") + "'";

            string testbatchyear = "";
            for (int j = 0; j < Chklst_batch.Items.Count; j++)
            {
                if (Chklst_batch.Items[j].Selected == true)
                {
                    if (testbatchyear == "")
                    {
                        testbatchyear = "'" + Chklst_batch.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        testbatchyear = testbatchyear + ",'" + Chklst_batch.Items[j].Value.ToString() + "'";
                    }
                }
            }
            if (testbatchyear.Trim() == "")
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Batch And Then Proceed";
                return;
            }

            string testbranch = "";
            for (int j = 0; j < chklst_branch.Items.Count; j++)
            {
                if (chklst_branch.Items[j].Selected == true)
                {
                    if (testbranch == "")
                    {
                        testbranch = "'" + chklst_branch.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        testbranch = testbranch + ",'" + chklst_branch.Items[j].Value.ToString() + "'";
                    }
                }
            }
            if (testbranch.Trim() == "")
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Degree and Branch And Then Proceed";
                return;
            }

            Boolean isnullval = false;
            string secvalmu = "";
            string secvalmu2 = "";
            for (int c = 0; c < chklstsection.Items.Count; c++)
            {
                if (chklstsection.Items[c].Selected == true)
                {
                    if (chklstsection.Items[c].Text.ToString() == "Empty")
                    {
                        isnullval = true;
                        if (secvalmu == "")
                        {
                            secvalmu = "''";
                        }
                        else
                        {
                            secvalmu = secvalmu + ",''";
                        }
                    }
                    else
                    {
                        if (secvalmu == "")
                        {
                            secvalmu = "'" + chklstsection.Items[c].Text.ToString() + "'";
                        }
                        else
                        {
                            secvalmu = secvalmu + ",'" + chklstsection.Items[c].Text.ToString() + "'";
                        }
                    }
                }
            }
            if (secvalmu.Trim() != "")
            {
                string secc = secvalmu;
                secvalmu2 = " and sections in(" + secvalmu + ")";
                secvalmu = " and r.sections in(" + secvalmu + ")";

                

                if (isnullval)
                {

                    secvalmu2 = " and  (isnull(Sections,'')) in(" + secc + ")";
                    secvalmu = " and  (isnull(r.Sections,'')) in(" + secc + ")";
                 

                }

            }

            string strgetmaxhours = da.GetFunction("select max(No_of_hrs_per_day) from PeriodAttndSchedule p,Registration r where p.degree_code=r.degree_code and p.semester=r.Current_Semester and r.Batch_Year in(" + testbatchyear + ") and r.degree_code in (" + testbranch + ")");
            int nohrs = 0;
            if (strgetmaxhours.Trim() != "")
            {
                nohrs = Convert.ToInt32(strgetmaxhours);
                ViewState["temp_table"] = Convert.ToInt32(strgetmaxhours); 
            }
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Student Count", typeof(string));
            dt2.Columns.Add("Period", typeof(double));
            Dictionary<int, string> dictotperiod = new Dictionary<int, string>();




            string strquery = "select count(r.roll_no) as stucount,r.Batch_Year,r.degree_code,c.Course_Name,de.Dept_Name,r.Current_Semester,ltrim(rtrim(isnull(r.Sections,''))) as  Sections  from Registration r,Degree d,Department de,Course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and r.cc=0 and r.delflag=0 and r.exam_flag <> 'DEBAR' and r.Batch_Year in(" + testbatchyear + ") and r.degree_code in (" + testbranch + ") " + secvalmu + " group by r.Batch_Year,r.degree_code,c.Course_Name,de.Dept_Name,r.Current_Semester,ltrim(rtrim(isnull(r.Sections,''))) order by r.degree_code,r.Batch_Year desc,r.Current_Semester,ltrim(rtrim(isnull(r.Sections,''))) ";
            DataSet dsstu = da.select_method_wo_parameter(strquery, "text");
            if (dsstu.Tables[0].Rows.Count > 0)
            {
                
                Showgrid.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnmasterprint.Visible = true;
                btnPrint.Visible = true;

                int colcount = 1;


                //added by rajasekar 18/09/2018

                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                int colu = 0;
                string temp = " ";

                dtl.Columns.Add("Date", typeof(string));

                dtl.Rows[0][colu] = "Date";
                colu++;

                if (rbdate.Checked == true)
                {
                    

                    colcount++;
                    dtl.Columns.Add(Ibldegree.Text + " Details", typeof(string));
                    dtl.Rows[0][colu] = Ibldegree.Text + " Details";
                    colu++;

                    colcount++;
                    dtl.Columns.Add("Strength", typeof(string));
                    dtl.Rows[0][colu] = "Strength";
                    colu++;
                }

                            //=====================================11/6/12 PRABHA
                //Added by srinath 21/8/2013s

                //rajasekar
                string grouporusercode00 = "";
                string qry = "";
                if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode00 = " group_code=" + Session["group_code"].ToString().Trim().Split(';')[0] + "";
                }
                else
                {
                    grouporusercode00 = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                bool splhr_flag = false;
                //con.Close();
                //cmd.CommandText = "select rights from  special_hr_rights where " + grouporusercode + "";
                //cmd.Connection = con;
                //con.Open();
                //SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
                DataSet dsRights = new DataSet();
                if (!string.IsNullOrEmpty(grouporusercode00))
                {
                    qry = "select rights from  special_hr_rights where " + grouporusercode00 + "";
                    dsRights = da.select_method_wo_parameter(qry, "text");
                }
                //SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
                if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0)
                {
                    //while (dr_rights_spl_hr.Read())
                    foreach (DataRow dr_rights_spl_hr in dsRights.Tables[0].Rows)
                    {
                        string spl_hr_rights = string.Empty;
                        Hashtable od_has = new Hashtable();
                        spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                        if (spl_hr_rights.Trim().ToLower() == "true" || spl_hr_rights.Trim().ToLower() == "1")
                        {
                            splhr_flag = true;
                            pub_splhr_flag = true;
                           //getspecial_hr();
                        }
                    }
                }


                

                for (int h = 1; h <= nohrs; h++)
                {

              
                    temp = temp + " ";
                    colcount = colcount + 2;
                    dtl.Columns.Add(" ", typeof(string));
                    dtl.Columns.Add("  ", typeof(string));
                    
                    dtl.Rows[0][dtl.Columns.Count-2] = "Period " + h + "";
                   
                    

                    if (ddlattendance.SelectedItem.ToString() == "Absent")
                    {

                       
                        dtl.Rows[0][dtl.Columns.Count - 1] = "Period " + h + "";
                    }
                    else
                    {
                       
                    }
                    

                    
                    dtl.Columns[dtl.Columns.Count - 2].ColumnName = "P" + temp;
                    dtl.Columns[dtl.Columns.Count - 1].ColumnName = "A" + temp;

                    dtl.Rows[1][dtl.Columns.Count - 2] = "P";
                    dtl.Rows[1][dtl.Columns.Count - 1] = "A";

                    dictotperiod.Add(colcount - 1, "0");
                    dictotperiod.Add(colcount - 2, "0");

                   

                    

                    if (chklsperiod.Items[h - 1].Selected == true)
                    {
                    
                        
                        if (ddlattendance.SelectedItem.ToString() == "Present")
                        {
                            

                            dtl.Columns.RemoveAt(dtl.Columns.Count - 1);
                           

                        }
                        else if (ddlattendance.SelectedItem.ToString() == "Absent")
                        {
                            
                            dtl.Columns.RemoveAt(dtl.Columns.Count - 2);
                            
                        }
                        
                        hrflag = true;
                    }
                    else
                    {
                       

                        dtl.Columns.RemoveAt(dtl.Columns.Count - 1);

                        dtl.Columns.RemoveAt(dtl.Columns.Count - 1);
                    }
                }

                if (splhr_flag == true)
                {
                    
                    string[] fromdatespit = txtfromdate.Text.Split('/');
                    string[] todatespit = txttodate.Text.Split('/');
                    DateTime spfromdate = Convert.ToDateTime(fromdatespit[1] + '/' + fromdatespit[0] + '/' + fromdatespit[2]);
                    DateTime sptodate = Convert.ToDateTime(todatespit[1] + '/' + todatespit[0] + '/' + todatespit[2]);
                    ht_sphr.Clear();
                    string hrdetno = string.Empty;

                    string getsphr = "select distinct  date,hrdet_no,sd.start_time,sd.end_time from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code in (" + testbranch + ") and Batch_Year in(" + testbatchyear + ")" + " " + secvalmu2 + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "'";
                    ds_sphr = d2.select_method(getsphr, hat, "Text");
                    if (ds_sphr.Tables[0].Rows.Count > 0)
                    {
                        int hr = 0;
                        for (int sphr = 0; sphr < ds_sphr.Tables[0].Rows.Count; sphr++)
                        {

                            string strtime = Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["start_time"]);
                            string endtime = Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["end_time"]);
                            if (ht_sphr.Contains(strtime + "-" + endtime))
                            {
                                
                            }
                            else
                            {
                                ht_sphr.Add(Convert.ToString(strtime + "-" + endtime));
                                hr++;
                                temp = temp + " ";
                                colcount = colcount + 2;
                                dtl.Columns.Add(" ", typeof(string));
                                dtl.Columns.Add("  ", typeof(string));

                                dtl.Rows[0][dtl.Columns.Count - 2] = " SH " + hr + "";



                                if (ddlattendance.SelectedItem.ToString() == "Absent")
                                {


                                    dtl.Rows[0][dtl.Columns.Count - 1] = "SH " + hr + "";
                                }
                                else
                                {

                                }



                                dtl.Columns[dtl.Columns.Count - 2].ColumnName = "P" + temp;
                                dtl.Columns[dtl.Columns.Count - 1].ColumnName = "A" + temp;

                                dtl.Rows[1][dtl.Columns.Count - 2] = "P";
                                dtl.Rows[1][dtl.Columns.Count - 1] = "A";


                                if (ddlattendance.SelectedItem.ToString() == "Present")
                                {


                                    dtl.Columns.RemoveAt(dtl.Columns.Count - 1);


                                }
                                else if (ddlattendance.SelectedItem.ToString() == "Absent")
                                {

                                    dtl.Columns.RemoveAt(dtl.Columns.Count - 2);

                                }
                                

                            }
                        }
                    }
                }
                //===================================




                if (hrflag == false)
                {
                    clear();
                    lbl_err.Text = "Plase Select The Period And Then Proceed";
                    lbl_err.Visible = true;
                    return;
                }

                hrflag = false;
                colcount++;
                

                dtl.Columns.Add("Remarks", typeof(string));
                dtl.Rows[0][dtl.Columns.Count - 1] = "Remarks";

                
    

                frdate = Convert.ToString(txtfromdate.Text);
                todate = Convert.ToString(txttodate.Text);
                string strholidayflag = "select  * FROM holidayStudents where holiday_date between '" + dtf.ToString() + "' and '" + dtt.ToString() + "' and halforfull=0 and degree_code in (" + testbranch + ")";
                dsholiday = da.select_method_wo_parameter(strholidayflag, "text");
                int srno = 0;
                int coln=0;
                if (rbdepartment.Checked == true)
                {

                    for (int i = 0; i < dsstu.Tables[0].Rows.Count; i++)
                    {
                        srno++;
                        
                        string degreecode = dsstu.Tables[0].Rows[i]["degree_code"].ToString();
                        string batchyear = dsstu.Tables[0].Rows[i]["Batch_Year"].ToString();
                        string section = dsstu.Tables[0].Rows[i]["Sections"].ToString();
                        string semesterv = dsstu.Tables[0].Rows[i]["Current_Semester"].ToString();
                        string degreedetails = batchyear + " - " + dsstu.Tables[0].Rows[i]["Course_Name"].ToString() + " - " + dsstu.Tables[0].Rows[i]["Dept_Name"].ToString() + " - " + semesterv;
                        string secval = "";
                        if (section.Trim() != "" && section.Trim() != "-1")
                        {
                            degreedetails = degreedetails + " - " + section;
                            secval = " and r.Sections='" + section + "'";
                        }
                        degreedetails = degreedetails + " ( " + dsstu.Tables[0].Rows[i]["stucount"].ToString() + " )";
                        

                        dtrow = dtl.NewRow();
                        coln=0;
                        dtrow[coln] = degreedetails;
                        coln++;
                        dtl.Rows.Add(dtrow);

                        dicstupresentdate.Clear();
                        dicstuabsentdate.Clear();

         

                        string query = "select distinct r.roll_no as 'ROLL_NO',r.batch_year,r.Reg_No as 'REG_NO',r.Roll_Admit as 'ADMIT_NO',r.stud_type as 'Student_Type', len(roll_no ),r.Current_semester,r.degree_code, convert(varchar(15),r.adm_date,103) as adm_date from registration r,department de,Degree d where d.Degree_Code=r.degree_code and d.Dept_Code=de.Dept_Code and r.cc=0   and r.exam_flag <> 'DEBAR' and r.delflag=0 and  r.batch_year= " + batchyear + " and r.degree_code= " + degreecode + " " + secval + " order by ROLL_NO";
                        ds4 = da.select_method(query, hat, "Text");
                        if (ds4.Tables[0].Rows.Count > 0)
                        {
                            for (rows_count = 0; ds4.Tables[0].Rows.Count > rows_count; rows_count++)
                            {
                                string barnchcode = ds4.Tables[0].Rows[rows_count]["degree_code"].ToString();
                                string semester = ds4.Tables[0].Rows[rows_count]["Current_semester"].ToString();
                                string year = ds4.Tables[0].Rows[rows_count]["Batch_year"].ToString();

                                if (rows_count == 0)
                                {

                                    hat.Clear();
                                    hat.Add("degree_code", barnchcode.ToString());
                                    hat.Add("sem_ester", int.Parse(semester.ToString()));
                                    ds = da.select_method("period_attnd_schedule", hat, "sp");
                                    if (ds.Tables[0].Rows.Count != 0)
                                    {
                                        count = 15;
                                        NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                                        fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                        anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                        minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                        minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                    }
                                    hat.Clear();
                                    hat.Add("colege_code", Session["collegecode"].ToString());
                                    ds1 = da.select_method("ATT_MASTER_SETTING", hat, "sp");
                                    count = ds1.Tables[0].Rows.Count;
                                    hatleavecode.Clear();
                                    for (int atp = 0; atp < ds1.Tables[0].Rows.Count; atp++)
                                    {
                                        string strleab = ds1.Tables[0].Rows[atp]["leavecode"].ToString();
                                        string calfla = ds1.Tables[0].Rows[atp]["calcflag"].ToString();
                                        hatleavecode.Add(strleab, calfla);
                                    }

                                    frdate = Convert.ToString(txtfromdate.Text);
                                    todate = Convert.ToString(txttodate.Text);
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

                                }
                                persentmonthcal();
                            }
                        }

                        for (DateTime dt = dtf; dt <= dtt; dt = dt.AddDays(1))
                        {
                            if (dt.ToString("ddd").Trim().ToLower() != "sun")
                            {
                                dsholiday.Tables[0].DefaultView.RowFilter = "degree_code='" + degreecode + "' and semester='" + semesterv + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "'";
                                DataView dvholiday = dsholiday.Tables[0].DefaultView;
                               
                                if (dvholiday.Count == 0)
                                {
                                    

                                    dtrow = dtl.NewRow();
                                    coln = 0;
                                    dtrow[coln] = dt.ToString("dd/MM/yyyy");
                                    coln++;
                                    

                                    int col = 0;
                                    for (int h = 1; h <= nohrs; h++)
                                    {
                                        col = h * 2;
                                        string datehour = dt.ToString("MM/dd/yyyy") + "@" + h;
                                        string noofst = "0";
                                        if (dicstupresentdate.ContainsKey(datehour))
                                        {
                                            noofst = dicstupresentdate[datehour].ToString();
                                        }
                                        string noofabsent = "0";
                                        if (dicstuabsentdate.ContainsKey(datehour))
                                        {
                                            noofabsent = dicstuabsentdate[datehour].ToString();
                                        }

                                        

                                        

                                        if (chklsperiod.Items[h - 1].Selected == true)
                                        {
                                            if (ddlattendance.SelectedItem.ToString() == "Present")
                                            {
                                                dtrow[coln] = noofst;
                                                coln++;
                                            }
                                            else if (ddlattendance.SelectedItem.ToString() == "Absent")
                                            {
                                                dtrow[coln] = noofabsent;
                                                coln++;
                                            }
                                            else
                                            {

                                                dtrow[coln] = noofst;
                                                coln++;
                                                dtrow[coln] = noofabsent;
                                                coln++;

                                            }
                                        }
                                        
                                        if (noofst == "0")
                                        {
                                           
                                        }
                                        else
                                        {
                                            hrflag = true;
                                        }
                                        if (noofabsent == "0")
                                        {
                                            
                                        }
                                        else
                                        {
                                            hrflag = true;
                                        }
                                        int getval = Convert.ToInt32(dictotperiod[col - 1]) + Convert.ToInt32(noofst);
                                        dictotperiod[col - 1] = getval.ToString();

                                        getval = Convert.ToInt32(dictotperiod[col]) + Convert.ToInt32(noofabsent);
                                        dictotperiod[col] = getval.ToString();
                                    }





                                    if (splhr_flag == true)
                                    {

                                        
                                        if (ds_sphr.Tables[0].Rows.Count > 0)
                                        {
                                            DataSet shpresent = new DataSet();
                                            DataSet shabsent = new DataSet();

                                            for (int aa = 0; aa < ht_sphr.Count; aa++)
                                            {
                                               string[] sph_strtime_endtime = ht_sphr[aa].ToString().Split('-');
                                               string sph_strtime=sph_strtime_endtime[0].ToString();
                                               string sph_endtime=sph_strtime_endtime[1].ToString();




                                               string presentofsh = d2.GetFunctionv("select COUNT(sa.attendance) as present from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code=" + degreecode + " and batch_year=" + batchyear + "  and semester='" + semesterv + "' and sections='" + section + "' and date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt.ToString("MM/dd/yyyy") + "' and sa.hrdet_no=sd.hrdet_no and sa.attendance=1  and start_time='" + sph_strtime + "' and end_time='" + sph_endtime + "'");



                                               string absentofsh = d2.GetFunctionv("select COUNT(sa.attendance) as absent from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code=" + degreecode + " and batch_year=" + batchyear + "  and semester='" + semesterv + "' and sections='" + section + "' and date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt.ToString("MM/dd/yyyy") + "' and sa.hrdet_no=sd.hrdet_no and sa.attendance=2 and start_time='" + sph_strtime + "' and end_time='" + sph_endtime + "'");






                                               if (presentofsh == "0" && absentofsh=="0")
                                               {
                                                   if (ddlattendance.SelectedItem.ToString() == "Present")
                                                   {
                                                       dtrow[coln] = "-";
                                                       coln++;
                                                   }
                                                   else if (ddlattendance.SelectedItem.ToString() == "Absent")
                                                   {
                                                       dtrow[coln] = "-";
                                                       coln++;
                                                   }
                                                   else
                                                   {

                                                       dtrow[coln] = "-";
                                                       coln++;
                                                       dtrow[coln] = "-";
                                                       coln++;

                                                   }

                                               }
                                               else
                                               {
                                                   if (ddlattendance.SelectedItem.ToString() == "Present")
                                                   {
                                                       dtrow[coln] = presentofsh;
                                                       coln++;
                                                       
                                                   }
                                                   else if (ddlattendance.SelectedItem.ToString() == "Absent")
                                                   {
                                                       dtrow[coln] = absentofsh;
                                                       coln++;
                                                       

                                                   }
                                                   else
                                                   {

                                                       dtrow[coln] = presentofsh;
                                                       coln++;
                                                       dtrow[coln] = absentofsh;
                                                       coln++;
                                                       
                                                       

                                                   }


                                               }



                                            }
                                            
                                        }
                                    }


                                    dtl.Rows.Add(dtrow);
                                }
                            }

                           
                        }
                    }

                }
                else
                {
                    for (DateTime dt = dtf; dt <= dtt; dt = dt.AddDays(1))
                    {
                        if (dt.ToString("ddd").Trim().ToLower() != "sun")
                        {
                            for (int i = 0; i < dsstu.Tables[0].Rows.Count; i++)
                            {

                                string degreecode = dsstu.Tables[0].Rows[i]["degree_code"].ToString();
                                string batchyear = dsstu.Tables[0].Rows[i]["Batch_Year"].ToString();
                                string section = dsstu.Tables[0].Rows[i]["Sections"].ToString();
                                string semesterv = dsstu.Tables[0].Rows[i]["Current_Semester"].ToString();
                                string degreedetails = batchyear + " - " + dsstu.Tables[0].Rows[i]["Course_Name"].ToString() + " - " + dsstu.Tables[0].Rows[i]["Dept_Name"].ToString() + " - " + semesterv;
                                string secval = "";
                                if (section.Trim() != "" && section.Trim() != "-1")
                                {
                                    degreedetails = degreedetails + " - " + section;
                                    secval = " and r.Sections='" + section + "'";
                                }

                                dicstupresentdate.Clear();
                                dicstuabsentdate.Clear();

                 

                                string query = "select distinct r.roll_no as 'ROLL_NO',r.batch_year,r.Reg_No as 'REG_NO',r.Roll_Admit as 'ADMIT_NO',r.stud_type as 'Student_Type', len(roll_no ),r.Current_semester,r.degree_code, convert(varchar(15),r.adm_date,103) as adm_date from registration r,department de,Degree d where d.Degree_Code=r.degree_code and d.Dept_Code=de.Dept_Code and r.cc=0 and r.exam_flag <> 'DEBAR' and r.delflag=0 and  r.batch_year= " + batchyear + " and r.degree_code= " + degreecode + " " + secval + " order by ROLL_NO";
                                ds4 = da.select_method(query, hat, "Text");
                                if (ds4.Tables[0].Rows.Count > 0)
                                {
                                    for (rows_count = 0; ds4.Tables[0].Rows.Count > rows_count; rows_count++)
                                    {
                                        string barnchcode = ds4.Tables[0].Rows[rows_count]["degree_code"].ToString();
                                        string semester = ds4.Tables[0].Rows[rows_count]["Current_semester"].ToString();
                                        string year = ds4.Tables[0].Rows[rows_count]["Batch_year"].ToString();

                                        if (rows_count == 0)
                                        {

                                            hat.Clear();
                                            hat.Add("degree_code", barnchcode.ToString());
                                            hat.Add("sem_ester", int.Parse(semester.ToString()));
                                            ds = da.select_method("period_attnd_schedule", hat, "sp");
                                            if (ds.Tables[0].Rows.Count != 0)
                                            {
                                                count = 15;
                                                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                                                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                                anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                            }
                                            hat.Clear();
                                            hat.Add("colege_code", Session["collegecode"].ToString());
                                            ds1 = da.select_method("ATT_MASTER_SETTING", hat, "sp");
                                            count = ds1.Tables[0].Rows.Count;
                                            hatleavecode.Clear();
                                            for (int atp = 0; atp < ds1.Tables[0].Rows.Count; atp++)
                                            {
                                                string strleab = ds1.Tables[0].Rows[atp]["leavecode"].ToString();
                                                string calfla = ds1.Tables[0].Rows[atp]["calcflag"].ToString();
                                                hatleavecode.Add(strleab, calfla);
                                            }

                                            frdate = Convert.ToString(dt.ToString("dd/MM/yyyy"));
                                            todate = Convert.ToString(dt.ToString("dd/MM/yyyy"));
                                            string dt1 = frdate;
                                            string[] dsplit = dt1.Split(new Char[] { '/' });
                                            frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                                            demfcal = int.Parse(dsplit[2].ToString());
                                            demfcal = demfcal * 12;
                                            cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                                            cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());

                                            monthcal = cal_from_date.ToString();
                                            dt1 = todate;
                                            dsplit = dt1.Split(new Char[] { '/' });
                                            todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                                            demtcal = int.Parse(dsplit[2].ToString());
                                            demtcal = demtcal * 12;
                                            cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                                            cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());

                                            per_from_gendate = Convert.ToDateTime(frdate);
                                            per_to_gendate = Convert.ToDateTime(todate);

                                        }
                                        persentmonthcal();
                                    }
                                }

                                if (dt.ToString("ddd").Trim().ToLower() != "sun")
                                {
                                    dsholiday.Tables[0].DefaultView.RowFilter = "degree_code='" + degreecode + "' and semester='" + semesterv + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "'";
                                    DataView dvholiday = dsholiday.Tables[0].DefaultView;
                                    if (dvholiday.Count == 0)
                                    {
                                        srno++;
                                        


                                        dtrow = dtl.NewRow();
                                        coln = 0;
                                        dtrow[coln] = dt.ToString("dd/MM/yyyy");
                                        coln++;
                                        dtrow[coln] = degreedetails;
                                        coln++;
                                        dtrow[coln] = dsstu.Tables[0].Rows[i]["stucount"].ToString();
                                        coln++;

                                        int col = 0;
                                        for (int h = 1; h <= nohrs; h++)
                                        {
                                            col = h * 2;
                                            col = col + 2;
                                            string datehour = dt.ToString("MM/dd/yyyy") + "@" + h;
                                            string noofst = "0";
                                            if (dicstupresentdate.ContainsKey(datehour))
                                            {
                                                noofst = dicstupresentdate[datehour].ToString();
                                            }


                                           

                                            string noofabsent = "0";
                                            if (dicstuabsentdate.ContainsKey(datehour))
                                            {
                                                noofabsent = dicstuabsentdate[datehour].ToString();
                                            }
                                            
                                            if (chklsperiod.Items[h - 1].Selected == true)
                                            {
                                                if (ddlattendance.SelectedItem.ToString() == "Present")
                                                {
                                                    dtrow[coln] = noofst;
                                                    coln++;
                                                }
                                                else if (ddlattendance.SelectedItem.ToString() == "Absent")
                                                {
                                                    dtrow[coln] = noofabsent;
                                                    coln++;
                                                }
                                                else
                                                {

                                                    dtrow[coln] = noofst;
                                                    coln++;
                                                    dtrow[coln] = noofabsent;
                                                    coln++;

                                                }
                                            }
                                            if (noofst == "0")
                                            {
                                                
                                            }
                                            else
                                            {
                                                hrflag = true;
                                            }

                                            if (noofabsent == "0")
                                            {
                                                
                                            }
                                            else
                                            {
                                                hrflag = true;
                                            }

                                            int getval = Convert.ToInt32(dictotperiod[col - 1]) + Convert.ToInt32(noofst);
                                            dictotperiod[col - 1] = getval.ToString();

                                            getval = Convert.ToInt32(dictotperiod[col]) + Convert.ToInt32(noofabsent);
                                            dictotperiod[col] = getval.ToString();
                                        }

                                        if (splhr_flag == true)
                                        {


                                            if (ds_sphr.Tables[0].Rows.Count > 0)
                                            {
                                                DataSet shpresent = new DataSet();
                                                DataSet shabsent = new DataSet();
                                                for (int aa = 0; aa < ht_sphr.Count; aa++)
                                                {
                                                    string[] sph_strtime_endtime = ht_sphr[aa].ToString().Split('-');
                                                    string sph_strtime = sph_strtime_endtime[0].ToString();
                                                    string sph_endtime = sph_strtime_endtime[1].ToString();




                                                    string presentofsh = d2.GetFunctionv("select COUNT(sa.attendance) as present from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code=" + degreecode + " and batch_year=" + batchyear + "  and semester='" + semesterv + "' and sections='" + section + "' and date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt.ToString("MM/dd/yyyy") + "' and sa.hrdet_no=sd.hrdet_no and sa.attendance=1  and start_time='" + sph_strtime + "' and end_time='" + sph_endtime + "'");



                                                    string absentofsh = d2.GetFunctionv("select COUNT(sa.attendance) as absent from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code=" + degreecode + " and batch_year=" + batchyear + "  and semester='" + semesterv + "' and sections='" + section + "' and date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt.ToString("MM/dd/yyyy") + "' and sa.hrdet_no=sd.hrdet_no and sa.attendance=2 and start_time='" + sph_strtime + "' and end_time='" + sph_endtime + "'");






                                                    if (presentofsh == "0" && absentofsh == "0")
                                                    {
                                                        if (ddlattendance.SelectedItem.ToString() == "Present")
                                                        {
                                                            dtrow[coln] = "-";
                                                            coln++;
                                                        }
                                                        else if (ddlattendance.SelectedItem.ToString() == "Absent")
                                                        {
                                                            dtrow[coln] = "-";
                                                            coln++;
                                                        }
                                                        else
                                                        {

                                                            dtrow[coln] = "-";
                                                            coln++;
                                                            dtrow[coln] = "-";
                                                            coln++;

                                                        }

                                                    }
                                                    else
                                                    {
                                                        if (ddlattendance.SelectedItem.ToString() == "Present")
                                                        {
                                                            dtrow[coln] = presentofsh;
                                                            coln++;
                                                        }
                                                        else if (ddlattendance.SelectedItem.ToString() == "Absent")
                                                        {
                                                            dtrow[coln] = absentofsh;
                                                            coln++;
                                                        }
                                                        else
                                                        {

                                                            dtrow[coln] = presentofsh;
                                                            coln++;
                                                            dtrow[coln] = absentofsh;
                                                            coln++;

                                                        }


                                                    }



                                                }

                                            }
                                        }

                                        dtl.Rows.Add(dtrow);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (hrflag == false)
            {
                clear();
                lbl_err.Visible = true;
                lbl_err.Text = "No Records Found";
            }
            else
            {
               
                int c = 0;
                
                c = 0;
                


                dtrow = dtl.NewRow();
                int coln = 0;
                dtrow[coln] = "Total";
                coln++;
                if (rbdate.Checked == true)
                    coln += 2;
                if (rbdepartment.Checked == false)
                {
                    
                    c = 2;
                }
                int colv = 0;
                
                for (int h = 1; h <= nohrs; h++)
                {
                    if (chklsperiod.Items[h - 1].Selected == true)
                    {
                        colv = h * 2;
                        int per = colv - 1;
                        if (rbdepartment.Checked == false)
                        {
                            c = 0;
                            colv = colv + 2;
                            per = colv - 1;
                        }
                        if (ddlattendance.SelectedItem.ToString() == "Present")
                        {
                            DataRow dr2 = dt2.NewRow();
                            dr2[0] = h.ToString();
                            dr2[1] = dictotperiod[per].ToString();
                            dt2.Rows.Add(dr2);
                        }
                        else if (ddlattendance.SelectedItem.ToString() == "Absent")
                        {
                            DataRow dr2 = dt2.NewRow();
                            dr2[0] = h.ToString();
                            dr2[1] = dictotperiod[colv].ToString();
                            dt2.Rows.Add(dr2);
                        }
                        else
                        {
                            if (rbdepartment.Checked == false)
                            {
                                DataRow dr1 = dt2.NewRow();
                                dr1[0] = h.ToString() + " P";
                                dr1[1] = dictotperiod[colv].ToString();
                                dt2.Rows.Add(dr1);
                                DataRow dr2 = dt2.NewRow();
                                dr2[0] = h.ToString() + " A";
                                dr2[1] = dictotperiod[per].ToString();
                                dt2.Rows.Add(dr2);
                            }
                            if (rbdepartment.Checked == true)
                            {
                                DataRow dr1 = dt2.NewRow();
                                dr1[0] = h.ToString() + " P";
                                dr1[1] = dictotperiod[per].ToString();
                                dt2.Rows.Add(dr1);
                                DataRow dr2 = dt2.NewRow();
                                dr2[0] = h.ToString() + " A";
                                dr2[1] = dictotperiod[colv].ToString();
                                dt2.Rows.Add(dr2);
                            }
                        }

                        

                        if (chklsperiod.Items[h - 1].Selected == true)
                        {
                            if (ddlattendance.SelectedItem.ToString() == "Present")
                            {
                                dtrow[coln] = dictotperiod[per].ToString();
                                coln++;
                            }
                            else if (ddlattendance.SelectedItem.ToString() == "Absent")
                            {
                                dtrow[coln] = dictotperiod[colv].ToString();
                                coln++;
                            }
                            else
                            {
                                dtrow[coln] = dictotperiod[per].ToString();
                                coln++;
                                dtrow[coln] = dictotperiod[colv].ToString();
                                coln++;
                                

                            }
                        }
                        
                    }
                }

                if (pub_splhr_flag == true)
                {

                          if (ds_sphr.Tables[0].Rows.Count > 0)
                    {
                        DataSet shpresent = new DataSet();
                        DataSet shabsent = new DataSet();
                        for (int aa = 0; aa < ht_sphr.Count; aa++)
                        {
                            string[] sph_strtime_endtime = ht_sphr[aa].ToString().Split('-');
                            string sph_strtime = sph_strtime_endtime[0].ToString();
                            string sph_endtime = sph_strtime_endtime[1].ToString();

                            string[] fromdatespit = txtfromdate.Text.Split('/');
                            string[] todatespit = txttodate.Text.Split('/');
                            DateTime spfromdate = Convert.ToDateTime(fromdatespit[1] + '/' + fromdatespit[0] + '/' + fromdatespit[2]);
                            DateTime sptodate = Convert.ToDateTime(todatespit[1] + '/' + todatespit[0] + '/' + todatespit[2]);


                            string presentofsh = d2.GetFunctionv("select COUNT(sa.attendance) as present from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code in (" + testbranch + ")  and batch_year in(" + testbatchyear + ")  " + secvalmu2 + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "' and sa.hrdet_no=sd.hrdet_no and sa.attendance=1  and start_time='" + sph_strtime + "' and end_time='" + sph_endtime + "'");



                            string absentofsh = d2.GetFunctionv("select COUNT(sa.attendance) as absent from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code in (" + testbranch + ") and batch_year in(" + testbatchyear + ")  " + secvalmu2 + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "' and sa.hrdet_no=sd.hrdet_no and sa.attendance=2 and start_time='" + sph_strtime + "' and end_time='" + sph_endtime + "'");






                            if (presentofsh == "0" && absentofsh == "0")
                            {
                                if (ddlattendance.SelectedItem.ToString() == "Present")
                                {
                                    dtrow[coln] = "-";
                                    coln++;
                                }
                                else if (ddlattendance.SelectedItem.ToString() == "Absent")
                                {
                                    dtrow[coln] = "-";
                                    coln++;
                                }
                                else
                                {

                                    dtrow[coln] = "-";
                                    coln++;
                                    dtrow[coln] = "-";
                                    coln++;

                                }

                            }
                            else
                            {
                                if (ddlattendance.SelectedItem.ToString() == "Present")
                                {
                                    dtrow[coln] = presentofsh;
                                    coln++;
                                }
                                else if (ddlattendance.SelectedItem.ToString() == "Absent")
                                {
                                    dtrow[coln] = absentofsh;
                                    coln++;
                                }
                                else
                                {

                                    dtrow[coln] = presentofsh;
                                    coln++;
                                    dtrow[coln] = absentofsh;
                                    coln++;

                                }


                            }



                        }

                    }

            }

        

                
                if (dtl.Rows.Count > 0)
                {

                    dtl.Rows.Add(dtrow);
                    Showgrid.DataSource = dtl;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;
                    Showgrid.HeaderRow.Visible = false;
                    int ccc = 0;
                    int rowspanstart = 0;

                    int tempt = Convert.ToInt32(ViewState["temp_table"]);
                    int firstcol = 1;
                    if (rbdate.Checked == true)
                        firstcol = 3;

                    int spancol = 0;
                    if (ddlattendance.Text == "All")
                        spancol = 2;
                    else
                        spancol = 1;

                    ccc = firstcol;

                    for (int i = 0; i < Showgrid.Rows.Count; i++)
                    {
                        
                        int rowspancount = 0;
                        if (i != Showgrid.Rows.Count - 1)
                        {
                            if (rbdate.Checked == true)
                            {
                                Showgrid.Rows[i].Cells[1].Width = 200;
                            }
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
                            }
                        }
                        for (int j = 0; j < Showgrid.HeaderRow.Cells.Count; j++)
                        {
                            if (i == 0 || i == 1)
                            {
                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                Showgrid.Rows[i].Cells[j].Font.Bold = true;

                                if (i == 0)
                                {
                                    if (j < firstcol || j == Showgrid.HeaderRow.Cells.Count - 1)
                                    {
                                        Showgrid.Rows[i].Cells[j].RowSpan = 2;
                                        for (int a = i; a < 1; a++)
                                            Showgrid.Rows[a + 1].Cells[j].Visible = false;
                                    }
                                    else if (ccc == j)
                                    {
                                        Showgrid.Rows[i].Cells[j].ColumnSpan = spancol;
                                        for (int a = j + 1; a < j + spancol; a++)
                                            Showgrid.Rows[i].Cells[a].Visible = false;

                                        ccc += spancol;
                                    } 

                                }
                            }
                            else
                            {
                                if (Showgrid.HeaderRow.Cells[j].Text == "Date" || Showgrid.Rows[i].Cells[j].Text == "&nbsp;")
                                {
                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;
                                    if (Showgrid.Rows.Count - 1 == i)
                                    {
                                        Showgrid.Rows[i].Cells[j].BackColor = Color.AliceBlue;
                                        Showgrid.Rows[i].Cells[j].BackColor = Color.LightGreen;
                                        Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                        if (rbdate.Checked == true && j == 1)
                                        {
                                            Showgrid.Rows[i].Cells[j - 1].ColumnSpan = 3;
                                            for (int a = 1; a < 3; a++)
                                                Showgrid.Rows[i].Cells[a].Visible = false;
                                        }

                                    }
                                    else if (Showgrid.Rows[i].Cells[j].Text == "&nbsp;" && j == 1)
                                    {

                                        Showgrid.Rows[i].Cells[j - 1].HorizontalAlign = HorizontalAlign.Center;
                                        Showgrid.Rows[i].Cells[j - 1].BackColor = Color.LightGray;
                                        Showgrid.Rows[i].Cells[j - 1].ColumnSpan = Showgrid.Rows[i].Cells.Count;
                                        for (int a = 1; a < Showgrid.Rows[i].Cells.Count; a++)
                                            Showgrid.Rows[i].Cells[a].Visible = false;


                                    }
                                }

                                else
                                {
                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;


                                    if (Showgrid.Rows.Count - 1 == i)
                                    {


                                        Showgrid.Rows[i].Cells[j].BackColor = Color.AliceBlue;
                                        Showgrid.Rows[i].Cells[j].BackColor = Color.LightGreen;
                                        Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                    }
                                    else if (Showgrid.Rows[i].Cells[j].Text == "0")
                                    {
                                        if (Showgrid.HeaderRow.Cells[j].Text.Trim() == "P")
                                        {
                                            Showgrid.Rows[i].Cells[j].ForeColor = Color.Red;
                                            Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                        }
                                        else if (Showgrid.HeaderRow.Cells[j].Text.Trim() == "A")
                                        {
                                            Showgrid.Rows[i].Cells[j].ForeColor = Color.Green;
                                            Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }



                attedancechart.DataSource = dt2;
                attedancechart.DataBind();
                attedancechart.Visible = true;
                attedancechart.Enabled = false;
                attedancechart.ChartAreas[0].AxisX.RoundAxisValues();
                attedancechart.ChartAreas[0].AxisX.Minimum = 0;
                attedancechart.ChartAreas[0].AxisX.Interval = 1;
                attedancechart.Series["Series1"].IsValueShownAsLabel = true;
                attedancechart.Series[0].ChartType = SeriesChartType.Column;
                attedancechart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                attedancechart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                attedancechart.ChartAreas[0].AxisX.Title = "Period";
                attedancechart.ChartAreas[0].AxisY.Title = "Student Count";
                attedancechart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                attedancechart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                attedancechart.Series["Series1"].XValueMember = "Student Count";
                attedancechart.Series["Series1"].YValueMembers = "Period";
                attedancechart.ChartAreas[0].AxisX.TitleForeColor = System.Drawing.Color.Black;
                attedancechart.ChartAreas[0].AxisY.TitleForeColor = System.Drawing.Color.Black;
                attedancechart.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.Black;
                attedancechart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 8f);
                attedancechart.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.Black;
            }
            
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void persentmonthcal()
    {
        Boolean isadm = false;
        try
        {
            cal_from_date = cal_from_date_tmp;
            cal_to_date = cal_to_date_tmp;
            per_from_date = per_from_gendate;
            per_to_date = per_to_gendate;

            dumm_from_date = per_from_date;

            string admdate = ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
            Admission_date = Convert.ToDateTime(admdate);

            dd = ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString();
            hat.Clear();
            hat.Add("std_rollno", ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = da.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            if (rows_count == 0)
            {
                string branch_code = ds4.Tables[0].Rows[rows_count]["Degree_Code"].ToString();
                string semester = ds4.Tables[0].Rows[rows_count]["current_semester"].ToString();
                hat.Clear();
                hat.Add("degree_code", int.Parse(branch_code));
                hat.Add("sem", int.Parse(semester));
                hat.Add("from_date", frdate.ToString());
                hat.Add("to_date", todate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));

                int iscount = 0;

                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + branch_code + " and semester=" + semester + "";
                DataSet dsholiday = new DataSet();
                dsholiday = da.select_method(sqlstr_holiday, hat, "Text");
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);

                ds3 = da.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

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
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }
                    }
                }

                if (ds3.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (holiday_table21.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
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
                            holiday_table31.Add(((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()), k);
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
            }

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
                                    value_holi_status = holiday_table11[dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString()].ToString();
                                    split_holiday_status = value_holi_status.Split('*');

                                    if (split_holiday_status[0].ToString() == "3")
                                    {
                                        split_holiday_status_1 = "1";
                                        split_holiday_status_2 = "1";
                                    }
                                    else if (split_holiday_status[0].ToString() == "1")
                                    {
                                        if (split_holiday_status[1].ToString() == "1")
                                        {
                                            split_holiday_status_1 = "0";
                                            split_holiday_status_2 = "1";
                                        }

                                        if (split_holiday_status[2].ToString() == "1")
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
                                            value = ds2.Tables[0].Rows[next][date].ToString();

                                            if (hatleavecode.Contains(value))
                                            {
                                                ObtValue = Convert.ToInt32(hatleavecode[value].ToString());
                                                if (ObtValue == 1)
                                                {
                                                    if (dicstuabsentdate.ContainsKey(dumm_from_date.ToString("MM/dd/yyyy") + "@" + i))
                                                    {
                                                        int noofabse = dicstuabsentdate[dumm_from_date.ToString("MM/dd/yyyy") + "@" + i] + 1;
                                                        dicstuabsentdate[dumm_from_date.ToString("MM/dd/yyyy") + "@" + i] = noofabse;
                                                    }
                                                    else
                                                    {
                                                        dicstuabsentdate.Add(dumm_from_date.ToString("MM/dd/yyyy") + "@" + i, 1);
                                                    }
                                                }
                                                else if (ObtValue == 0)
                                                {

                                                    if (dicstupresentdate.ContainsKey(dumm_from_date.ToString("MM/dd/yyyy") + "@" + i))
                                                    {
                                                        int noper = dicstupresentdate[dumm_from_date.ToString("MM/dd/yyyy") + "@" + i] + 1;
                                                        dicstupresentdate[dumm_from_date.ToString("MM/dd/yyyy") + "@" + i] = noper;
                                                    }
                                                    else
                                                    {
                                                        dicstupresentdate.Add(dumm_from_date.ToString("MM/dd/yyyy") + "@" + i, 1);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    int k = fnhrs + 1;

                                    if (split_holiday_status_2 == "1")
                                    {
                                        for (i = k; i <= NoHrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            value = ds2.Tables[0].Rows[next][date].ToString();
                                            if (hatleavecode.Contains(value))
                                            {
                                                ObtValue = Convert.ToInt32(hatleavecode[value].ToString());
                                                if (ObtValue == 1)
                                                {
                                                    if (dicstuabsentdate.ContainsKey(dumm_from_date.ToString("MM/dd/yyyy") + "@" + i))
                                                    {
                                                        int noofabse = dicstuabsentdate[dumm_from_date.ToString("MM/dd/yyyy") + "@" + i] + 1;
                                                        dicstuabsentdate[dumm_from_date.ToString("MM/dd/yyyy") + "@" + i] = noofabse;
                                                    }
                                                    else
                                                    {
                                                        dicstuabsentdate.Add(dumm_from_date.ToString("MM/dd/yyyy") + "@" + i, 1);
                                                    }
                                                }
                                                else if (ObtValue == 0)
                                                {

                                                    if (dicstupresentdate.ContainsKey(dumm_from_date.ToString("MM/dd/yyyy") + "@" + i))
                                                    {
                                                        int noper = dicstupresentdate[dumm_from_date.ToString("MM/dd/yyyy") + "@" + i] + 1;
                                                        dicstupresentdate[dumm_from_date.ToString("MM/dd/yyyy") + "@" + i] = noper;
                                                    }
                                                    else
                                                    {
                                                        dicstupresentdate.Add(dumm_from_date.ToString("MM/dd/yyyy") + "@" + i, 1);
                                                    }
                                                }
                                            }
                                        }

                                    }
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
                            else
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
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
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
        spReportName.InnerHtml = "Department & Period Wise Attendance Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }


    public override void VerifyRenderingInServerForm(Control control)
    { }
}
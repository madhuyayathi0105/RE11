using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Text;


public partial class StudentWeeklyAttendance : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    int count = 0;
    int attcount = 0;

    string absentcolumn = "";
    int headspan = 0;
    DAccess2 d2 = new DAccess2();
    DataSet ds1 = new DataSet();
    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds_sphr = new DataSet();
    DataSet dsabper = new DataSet();

    Hashtable hatdate = new Hashtable();
    Hashtable hatabsent = new Hashtable();
    Hashtable hattotal = new Hashtable();
    Hashtable hat = new Hashtable();
    static Hashtable ht_sphr = new Hashtable();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();

    Hashtable hatabpet = new Hashtable();
    Hashtable hatabnotpet = new Hashtable();
    Hashtable hatattenper = new Hashtable();
    Double absentper;
    Double absentnotper;
    int hear = 0;
    int hear5 = 0;

    int mmyycount;
    string dd = "";
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0;
    int notconsider_value = 0;
    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;
    int unmark;

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
    int next = 0;
    int minpresII = 0;
    string value, date;
    int rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    int cal_from_date, cal_from_date_tmp;
    int cal_to_date, cal_to_date_tmp;
    double per_perhrs, per_abshrs;
    double per_ondu, per_leave, per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0; double workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_holidate;
    int dum_unmark;
    int tot_per_hrs;
    double njhr, njdate;
    double tot_ondu, tot_ml;
    DataSet ds_attnd_pts = new DataSet();
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    int demfcal, demtcal;
    string monthcal;
    DataTable data = new DataTable();
    DataRow drow;
    Dictionary<int, string> dichear = new Dictionary<int, string>();
    ArrayList arrColHdrNames1 = new ArrayList();
    ArrayList arrColHdrNames2 = new ArrayList();
    ArrayList arrColHdrNames3 = new ArrayList();


    #region "Load Details"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lblnorec.Visible = false;
        errmsg.Visible = false;
        if (!IsPostBack)
        {
            txtfrom.Attributes.Add("readonly", "readonly");
            txtto.Attributes.Add("readonly", "readonly");

            Showgrid.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblexcelname.Visible = false;
            lblnorec.Visible = false;
            errmsg.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            txtexcelname.Visible = false;
            lblexcelname.Visible = false;
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (txtdegree.Enabled == true)
            {
                txtdegree.Enabled = true;
                txtbranch.Enabled = true;
                btngo.Enabled = true;
                txtfrom.Enabled = true;
                txtto.Enabled = true;
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSectransport(strbatch, strbranch);
                BindSectionDetail(strbatch, strbranch);

                txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                txtdegree.Enabled = false;
                txtbranch.Enabled = false;
                btngo.Enabled = false;
                txtfrom.Enabled = false;
                txtto.Enabled = false;
            }
        }

    }
    //  Batch load-------

    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds2;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                chklsbatch.SelectedIndex = chklsbatch.Items.Count - 1;
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklsbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                    }

                }
            }


        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    // Degree load function
    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                chklstdegree.Items[0].Selected = true;
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
                txtdegree.Enabled = true;
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    // Branch load function-------

    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstbranch.Items.Count == count)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = "Please Select the Degree";
        }

    }

    // section laod function

    public void BindSectransport(string strbatch, string strbranch)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                }
            }

            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            chklssec.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklssec.DataSource = ds2;
                chklssec.DataTextField = "sections";
                chklssec.DataBind();
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    chklssec.Enabled = false;
                }
                else
                {
                    chklssec.Enabled = true;
                    chklssec.SelectedIndex = chklssec.Items.Count - 2;
                    chklssec.Items[0].Selected = true;
                    for (int i = 0; i < chklssec.Items.Count; i++)
                    {
                        chklssec.Items[i].Selected = true;
                        if (chklssec.Items[i].Selected == true)
                        {
                            count += 1;
                        }
                        if (chklssec.Items.Count == count)
                        {
                            chksec.Checked = true;
                        }
                    }
                }
            }
            else
            {
                chklssec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = " Please Select the Branch";
        }
    }

    // check box load function

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstbranch.Items.Count == count)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
            BindSectionDetail(strbatch, strbranch);
        }

        catch (Exception ex)
        {
            errmsg.Text = "Please Select the Degree";
        }
    }

    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }

            chklssec.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklssec.DataSource = ds2;
                chklssec.DataTextField = "sections";
                chklssec.DataBind();
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    chklssec.Enabled = false;
                }
                else
                {
                    txtsec.Enabled = true;
                    chklssec.Enabled = true;
                    chklssec.SelectedIndex = chklssec.Items.Count - 2;
                    chklssec.Items[0].Selected = true;
                    for (int i = 0; i < chklssec.Items.Count; i++)
                    {
                        chklssec.Items[i].Selected = true;
                        if (chklssec.Items[i].Selected == true)
                        {
                            count += 1;
                        }
                        if (chklssec.Items.Count == count)
                        {
                            chksec.Checked = true;
                        }
                    }
                }
                chklssec.Items.Insert(0, "Empty Section");
            }
            else
            {
                chklssec.Enabled = false;
                txtsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = " Please Select the Branch";
        }

    }

    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                    txtbatch.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    // bind batch check box load function

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklsbatch.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklsbatch.Items[i].Value;
                    }
                }
            }

            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                    txtdegree.Text = "---Select---";
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklstdegree.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklstdegree.Items[i].Value;
                    }
                }
            }
            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }

            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                    txtbranch.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklstbranch.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklstbranch.Items[i].Value;
                    }
                }
            }

            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }

            BindSectionDetail(strbatch, strbranch);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void chksec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chksec.Checked == true)
            {
                for (int i = 0; i < chklssec.Items.Count; i++)
                {
                    chklssec.Items[i].Selected = true;
                    txtsec.Text = "Section(" + (chklssec.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklssec.Items.Count; i++)
                {
                    chklssec.Items[i].Selected = false;
                    txtsec.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void chklstsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            for (int i = 0; i < chklssec.Items.Count; i++)
            {
                if (chklssec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtsec.Text = "Section(" + commcount.ToString() + ")";

                }
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    #endregion

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            txtexcelname.Text = "";
            string[] splitfromdate = txtfrom.Text.Split(new Char[] { '/' });
            string[] splittodate = txtto.Text.Split(new char[] { '/' });
            string chechfromdate = splitfromdate[1] + '/' + splitfromdate[0] + '/' + splitfromdate[2];
            string checktoodate = splittodate[1] + '/' + splittodate[0] + '/' + splittodate[2];
            DateTime confromdate = Convert.ToDateTime(chechfromdate);
            DateTime contodate = Convert.ToDateTime(checktoodate);
            if (confromdate > contodate)
            {
                Showgrid.Visible = false;
                btnxl.Visible = false;
                txtexcelname.Visible = false;
                lblexcelname.Visible = false;
                errmsg.Text = "Please Enter To Date Grater Than From Date";
                errmsg.Visible = true;
            }
            else
            {
                string sqlbatch = "";
                string sqlbatchquery = "";
                string sqlbranch = "";
                string sqlbranchquery = "";
                string sqlsec = "";
                string sqlsecquery = "";

                if (txtbatch.Text != "--Select--")
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < chklsbatch.Items.Count; itemcount++)
                    {
                        if (chklsbatch.Items[itemcount].Selected == true)
                        {
                            if (sqlbatch == "")
                                sqlbatch = "'" + chklsbatch.Items[itemcount].Value.ToString() + "'";
                            else
                                sqlbatch = sqlbatch + "," + "'" + chklsbatch.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (sqlbatch != "")
                    {
                        sqlbatch = " in(" + sqlbatch + ")";
                        sqlbatchquery = " and r.batch_year  " + sqlbatch + "";
                    }
                    else
                    {
                        sqlbatchquery = " ";
                    }
                }

                if (txtbranch.Text != "---Select---")
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < chklstbranch.Items.Count; itemcount++)
                    {
                        if (chklstbranch.Items[itemcount].Selected == true)
                        {
                            if (sqlbranch == "")
                                sqlbranch = "'" + chklstbranch.Items[itemcount].Value.ToString() + "'";
                            else
                                sqlbranch = sqlbranch + "," + "'" + chklstbranch.Items[itemcount].Value.ToString() + "'";
                        }
                    }

                    if (sqlbranch != "")
                    {
                        sqlbranch = " in(" + sqlbranch + ")";
                        sqlbranchquery = " and r.degree_code  " + sqlbranch + "";
                    }
                    else
                    {
                        sqlbranchquery = " ";
                    }
                }
                Boolean emval = false;
                if (txtsec.Text != "---Select---")
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < chklssec.Items.Count; itemcount++)
                    {
                        if (chklssec.Items[itemcount].Selected == true)
                        {
                            if (chklssec.Items[itemcount].Text.ToString() == "Empty Section")
                            {
                                if (sqlsec == "")
                                    sqlsec = "'','-1'";
                                else
                                    sqlsec = sqlsec + "," + "'','-1'";
                                emval = true;
                            }
                            else
                            {
                                if (sqlsec == "")
                                    sqlsec = "'" + chklssec.Items[itemcount].Value.ToString() + "'";
                                else
                                    sqlsec = sqlsec + "," + "'" + chklssec.Items[itemcount].Value.ToString() + "'";
                            }
                        }
                    }
                    if (sqlsec != "")
                    {
                        if (emval == false)
                        {
                            sqlsecquery = " and r.sections in (" + sqlsec + ")";
                        }
                        else
                        {
                            sqlsecquery = " and ( r.sections in (" + sqlsec + ") or r.sections is null)";
                        }
                    }
                    else
                    {
                        sqlsecquery = " ";
                    }
                }


                loadheader();

                string testyearofstudy = "";
                int sno = 0;
                string sqlquery = "select count(r.roll_no)as strength,(c.course_name+'-'+ dp.dept_acronym) as dept,r.current_semester,r.batch_year,r.degree_code,r.sections from registration r,degree de,course c,department dp where r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  " + sqlbatchquery + " " + sqlbranchquery + " " + sqlsecquery + " group by r.degree_code,r.batch_year,course_name,dept_acronym,current_semester,r.sections";
                DataSet dsselect = new DataSet();
                dsselect = d2.select_method(sqlquery, hat, "Text");
                if (dsselect.Tables[0].Rows.Count > 0)
                {
                    lblnorec.Visible = false;
                    btnxl.Visible = true;
                    txtexcelname.Visible = true;
                    lblexcelname.Visible = true;
                    btnprintmaster.Visible = true;
                    btnPrint.Visible = true;
                    for (int row_count = 0; dsselect.Tables[0].Rows.Count > row_count; row_count++)
                    {
                        string batchyear = dsselect.Tables[0].Rows[row_count]["current_semester"].ToString();

                        if (batchyear == "1" || batchyear == "2")
                        {
                            batchyear = "I";
                        }
                        if (batchyear == "3" || batchyear == "4")
                        {
                            batchyear = "II";
                        }
                        if (batchyear == "5" || batchyear == "6")
                        {
                            batchyear = "III";
                        }
                        if (batchyear == "7" || batchyear == "8")
                        {
                            batchyear = "IV";
                        }
                        if (batchyear == "9" || batchyear == "10")
                        {
                            batchyear = "V";
                        }
                        string section = dsselect.Tables[0].Rows[row_count]["sections"].ToString();
                        if (section != "")
                        {
                            batchyear = batchyear + '-' + section;
                        }

                        string yearofstudy = dsselect.Tables[0].Rows[row_count]["dept"].ToString();

                        if (testyearofstudy != yearofstudy)
                        {
                            sno++;
                        }


                        drow = data.NewRow();
                        drow["S.No"] = sno.ToString();
                        drow["Course"] = dsselect.Tables[0].Rows[row_count]["dept"].ToString();
                        drow["Year Of Study"] = batchyear;

                        string strenth = dsselect.Tables[0].Rows[row_count]["strength"].ToString();
                        drow["Student Strength"] = strenth;
                        data.Rows.Add(drow);


                        testyearofstudy = dsselect.Tables[0].Rows[row_count]["dept"].ToString();
                        string yearvalue = dsselect.Tables[0].Rows[row_count]["Batch_year"].ToString();
                        string branchcode = dsselect.Tables[0].Rows[row_count]["degree_code"].ToString();
                        string sections = dsselect.Tables[0].Rows[row_count]["sections"].ToString();
                        if (sections != "" && sections != null)
                        {
                            sections = "and r.sections='" + sections + "'";
                        }
                        string query = "select distinct r.roll_no as 'ROLL_NO',r.batch_year,r.Reg_No as 'REG_NO',r.Roll_Admit as 'ADMIT_NO',r.stud_type as 'Student_Type', len(roll_no ),r.Current_semester,r.degree_code, convert(varchar(15),r.adm_date,103) as adm_date from registration r,department de,Degree d where d.Degree_Code=r.degree_code and d.Dept_Code=de.Dept_Code and r.cc=0 and r.exam_flag<>'debar' and r.delflag=0 and  r.batch_year= " + yearvalue + " and r.degree_code= " + branchcode + " " + sections + "  order by  len(r.roll_no )";
                        ds4 = d2.select_method(query, hat, "Text");
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
                                    ds = d2.select_method("period_attnd_schedule", hat, "sp");
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
                                    ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                                    count = ds1.Tables[0].Rows.Count;

                                    frdate = Convert.ToString(txtfrom.Text);
                                    todate = Convert.ToString(txtto.Text);
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

                                    ht_sphr.Clear();
                                    string hrdetno = "";
                                    string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + barnchcode.ToString() + " and batch_year=" + year.ToString() + " and semester=" + semester.ToString() + " and date between '" + per_from_gendate.ToString() + "' and '" + per_to_gendate.ToString() + "'";
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
                                }

                                persentmonthcal();
                            }
                        }

                        //Total Strenth
                        Double totalstrenthdept = Convert.ToDouble(strenth);
                        int totlstrent = Convert.ToInt32(strenth);
                        if (!hattotal.Contains(Convert.ToString(3)))
                        {
                            hattotal.Add(Convert.ToString(3), totlstrent);
                        }
                        else
                        {
                            int total = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(3), hattotal));
                            totlstrent = totlstrent + total;
                            hattotal[(Convert.ToString(3))] = totlstrent;
                        }

                        //Present Count Bind

                        for (hear = 4; hear < headspan + 4; hear++)
                        {
                            string valuereturn = "-";
                            string headerdate1 = Convert.ToString(data.Columns[hear].ColumnName);
                            if (hatdate.Contains(Convert.ToString(headerdate1)))
                            {
                                valuereturn = Convert.ToString(GetCorrespondingKey(Convert.ToString(headerdate1), hatdate));
                            }
                            data.Rows[data.Rows.Count - 1][hear] = valuereturn;


                            //Attendance % in Current Row
                            if (valuereturn == "-")
                            {
                                valuereturn = "0";
                            }
                            Double totalpre = Convert.ToDouble(valuereturn);
                            Double attenperce = 0;

                            attenperce = totalpre / totalstrenthdept * 100;
                            attenperce = Math.Round(attenperce, 2);

                            if (!hatattenper.Contains(Convert.ToString(headerdate1)))
                            {
                                hatattenper.Add(Convert.ToString(headerdate1), attenperce);
                            }

                            //Total Count in Present
                            if (!hattotal.Contains(Convert.ToString(hear)))
                            {
                                hattotal.Add(Convert.ToString(hear), totalpre);
                            }
                            else
                            {
                                Double total = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(hear), hattotal));
                                totalpre = totalpre + total;
                                hattotal[(Convert.ToString(hear))] = totalpre;
                            }

                        }
                        hatdate.Clear();

                        //Absent Count Bind
                        int hear2 = hear + hear - 4;
                        for (int hear1 = hear; hear1 < hear2; hear1++)
                        {
                            string valuereturn = "-";

                            string headerdate1 = Convert.ToString(data.Columns[hear1].ColumnName);
                            if (hatabsent.Contains(Convert.ToString(headerdate1)))
                            {
                                valuereturn = Convert.ToString(GetCorrespondingKey(headerdate1, hatabsent));
                            }
                            if (valuereturn == "-")
                            {
                                valuereturn = "0";
                            }
                            data.Rows[data.Rows.Count - 1][hear1] = valuereturn;


                            //Total Count in Absent
                            if (!hattotal.Contains(Convert.ToString(hear1)))
                            {

                                Double totalpre = Convert.ToDouble(valuereturn);
                                hattotal.Add(Convert.ToString(hear1), totalpre);
                            }
                            else
                            {
                                Double totalpre = Convert.ToDouble(valuereturn);
                                Double total = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(hear1), hattotal));
                                totalpre = totalpre + total;
                                hattotal[(Convert.ToString(hear1))] = totalpre;
                            }
                        }
                        hatabsent.Clear();

                        //With Permission Count Bind
                        int hear3 = hear2 + hear - 4;
                        for (int hear1 = hear2; hear1 < hear3; hear1++)
                        {
                            string valuereturn = "-";
                            string headerdate1 = Convert.ToString(data.Columns[hear1].ColumnName);
                            if (hatabpet.Contains(Convert.ToString(headerdate1)))
                            {
                                valuereturn = Convert.ToString(GetCorrespondingKey(headerdate1, hatabpet));
                            }
                            data.Rows[data.Rows.Count - 1][hear1] = valuereturn;
                            //Total Count in With Permission
                            if (valuereturn == "-")
                            {
                                valuereturn = "0";
                            }
                            if (!hattotal.Contains(Convert.ToString(hear1)))
                            {

                                Double totalpre = Convert.ToDouble(valuereturn);
                                hattotal.Add(Convert.ToString(hear1), valuereturn);
                            }
                            else
                            {
                                Double totalpre = Convert.ToDouble(valuereturn);
                                Double total = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(hear1), hattotal));
                                totalpre = totalpre + total;
                                hattotal[(Convert.ToString(hear1))] = totalpre;
                            }

                        }
                        hatabpet.Clear();

                        //With out Permission Count Bind
                        int hear4 = hear3 + hear - 4;
                        for (int hear1 = hear3; hear1 < hear4; hear1++)
                        {
                            string valuereturn = "-";
                            string headerdate1 = Convert.ToString(data.Columns[hear1].ColumnName);
                            if (hatabnotpet.Contains(Convert.ToString(headerdate1)))
                            {
                                valuereturn = Convert.ToString(GetCorrespondingKey(headerdate1, hatabnotpet));
                            }
                            if (valuereturn == "-")
                            {
                                valuereturn = "0";
                            }
                            data.Rows[data.Rows.Count - 1][hear1] = valuereturn;

                            //Total Count in With Out Permission
                            if (!hattotal.Contains(Convert.ToString(hear1)))
                            {

                                Double totalpre = Convert.ToDouble(valuereturn);
                                hattotal.Add(Convert.ToString(hear1), totalpre);
                            }
                            else
                            {
                                Double total = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(hear1), hattotal));
                                total = Convert.ToDouble(valuereturn) + total;
                                hattotal[(Convert.ToString(hear1))] = total;
                            }
                        }
                        hatabnotpet.Clear();

                        //Attendance % Count Bind
                        hear5 = hear4 + hear - 4;
                        for (int hear1 = hear4; hear1 < hear5; hear1++)
                        {
                            string valuereturn = "-";

                            string headerdate1 = Convert.ToString(data.Columns[hear1].ColumnName);
                            if (hatattenper.Contains(Convert.ToString(headerdate1)))
                            {
                                valuereturn = Convert.ToString(GetCorrespondingKey(headerdate1, hatattenper));
                            }
                            data.Rows[data.Rows.Count - 1][hear1] = valuereturn;
                        }
                        hatattenper.Clear();
                    }

                    drow = data.NewRow();
                    drow["S.No"] = "Total";

                    data.Rows.Add(drow);


                    //Over All Total Bind
                    double totalstude = 0;
                    double totalattendstude = 0;
                    double totalpercenta = 0;
                    int abspercount = 2;
                    for (int l = 3; l < hear5 - hear + 4; l++)
                    {
                        String totalstrenth = Convert.ToString(GetCorrespondingKey(Convert.ToString(l), hattotal));
                        abspercount = abspercount + 1;
                        if (l == 3)
                        {
                            totalstude = Convert.ToDouble(totalstrenth);
                        }
                        if (l != 3)
                        {
                            if (l <= hear - 1)
                            {
                                //Over all Attendance Percentage Bind
                                totalattendstude = Convert.ToDouble(totalstrenth);
                                totalpercenta = totalattendstude / totalstude * 100;
                                totalpercenta = Math.Round(totalpercenta, 2);
                                int abspercentcolume = hear5 - hear + abspercount;
                                data.Rows[data.Rows.Count - 1][abspercentcolume] = totalpercenta.ToString();

                            }
                        }
                        data.Rows[data.Rows.Count - 1][l] = totalstrenth;

                    }
                    if (data.Columns.Count > 0 && data.Rows.Count > 2)
                    {
                        Showgrid.DataSource = data;
                        Showgrid.DataBind();
                        Showgrid.Visible = true;

                        int rowcnt = Showgrid.Rows.Count - 3;
                        //Rowspan
                        for (int t = Showgrid.Rows.Count - 1; t > 0; t--)
                        {
                            GridViewRow row = Showgrid.Rows[t];
                            GridViewRow previousRow = Showgrid.Rows[t - 1];
                            for (int i = 0; i < 2; i++)
                            {
                                if (row.Cells[i].Text == previousRow.Cells[i].Text)
                                {
                                    if (previousRow.Cells[i].RowSpan == 0)
                                    {
                                        if (row.Cells[i].RowSpan == 0)
                                        {
                                            previousRow.Cells[i].RowSpan += 2;
                                        }
                                        else
                                        {
                                            previousRow.Cells[i].RowSpan = row.Cells[i].RowSpan + 1;
                                        }
                                        row.Cells[i].Visible = false;
                                    }
                                }
                            }

                        }
                        for (int rowIndex = Showgrid.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
                        {
                            GridViewRow row = Showgrid.Rows[rowIndex];
                            GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                            Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            Showgrid.Rows[rowIndex].Font.Bold = true;
                            Showgrid.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;

                            for (int i = 0; i < row.Cells.Count; i++)
                            {
                                if (row.Cells[i].Text == previousRow.Cells[i].Text)
                                {

                                    row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                           previousRow.Cells[i].RowSpan + 1;
                                    previousRow.Cells[i].Visible = false;
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

                        Showgrid.Rows[data.Rows.Count - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        Showgrid.Rows[data.Rows.Count - 1].Cells[0].ColumnSpan = 3;
                        Showgrid.Rows[data.Rows.Count - 1].Cells[0].Font.Bold = true;
                        for (int a = 1; a < 3; a++)
                            Showgrid.Rows[data.Rows.Count - 1].Cells[a].Visible = false;
                        for (int a = 3; a < data.Columns.Count - 1; a++)
                            Showgrid.Rows[data.Rows.Count - 1].Cells[a].Font.Bold = true;

                    }


                }
                else
                {
                    Showgrid.Visible = false;
                    btnxl.Visible = false;
                    txtexcelname.Visible = false;
                    lblexcelname.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Records Found";
                }


            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }



    public void loadheader()
    {
        try
        {
            int percnt = 0;
            int abscnt = 0;
            int withpercnt = 0;
            int withoutpercnt = 0;
            int perabscnt = 0;
            string headercont = "";

            System.Text.StringBuilder dayordate = new System.Text.StringBuilder();

            arrColHdrNames1.Add("S.No");
            arrColHdrNames2.Add("S.No");
            arrColHdrNames3.Add("S.No");
            arrColHdrNames1.Add("Course");
            arrColHdrNames2.Add("Course");
            arrColHdrNames3.Add("Course");
            arrColHdrNames1.Add("Year Of Study");
            arrColHdrNames2.Add("Year Of Study");
            arrColHdrNames3.Add("Year Of Study");
            arrColHdrNames1.Add("Student Strength");
            arrColHdrNames2.Add("Student Strength");
            arrColHdrNames3.Add("Student Strength");

            data.Columns.Add("S.No", typeof(string));
            data.Columns.Add("Course", typeof(string));
            data.Columns.Add("Year Of Study", typeof(string));
            data.Columns.Add("Student Strength", typeof(string));
            string[] splitfrom = txtfrom.Text.Split(new Char[] { '/' });
            string heddate = "" + splitfrom[1] + "/" + splitfrom[0] + "/" + splitfrom[2];

            string[] splitto = txtto.Text.Split(new Char[] { '/' });
            string headertodate = splitto[1] + "/" + splitto[0] + "/" + splitto[2];

            DateTime headtodate = Convert.ToDateTime(headertodate);
            DateTime headerdate = Convert.ToDateTime(heddate);
            headtodate = headtodate.AddDays(1);

            string datefun = "";
            // Present Header

            for (DateTime headerdate1 = headerdate; headerdate1 < headtodate; headerdate1.AddDays(1))
            {

                string hfilldate = Convert.ToString(headerdate1.DayOfWeek);
                if (hfilldate != "Sunday")
                {
                    if (hfilldate == "Monday")
                    {
                        hfilldate = "Mon";
                    }
                    if (hfilldate == "Tuesday")
                    {
                        hfilldate = "Tue";
                    }
                    if (hfilldate == "Wednesday")
                    {
                        hfilldate = "Wed";
                    }
                    if (hfilldate == "Thursday")
                    {
                        hfilldate = "Thu";
                    }
                    if (hfilldate == "Friday")
                    {
                        hfilldate = "Fri";
                    }
                    if (hfilldate == "Saturday")
                    {
                        hfilldate = "Sat";
                    }
                    if (chkdate.Checked == true)
                    {
                        datefun = Convert.ToString(headerdate1);
                        string[] datefun1 = datefun.Split(new Char[] { ' ' });
                        string spilktdate = datefun1[0].ToString();

                        string[] orderdate = spilktdate.Split(new Char[] { '/' });
                        datefun = orderdate[1] + "/" + orderdate[0] + "/" + orderdate[2];
                        hfilldate = hfilldate + "-" + datefun;
                    }
                    headspan++;
                    dayordate = new System.Text.StringBuilder(hfilldate);

                    AddTableColumn(data, dayordate);
                    percnt++;

                    arrColHdrNames1.Add("No Of Presentees");
                    arrColHdrNames2.Add("No Of Presentees");
                    arrColHdrNames3.Add(hfilldate);

                }
                headerdate1 = headerdate1.AddDays(1);
            }
            headercont = "No Of Presentees " + "-" + percnt.ToString();
            dichear.Add(1, headercont);

            // Absent Header



            for (DateTime headerdate1 = headerdate; headerdate1 < headtodate; headerdate1.AddDays(1))
            {

                string hfilldate = Convert.ToString(headerdate1.DayOfWeek);
                if (hfilldate != "Sunday")
                {
                    if (hfilldate == "Monday")
                    {
                        hfilldate = "Mon";
                    }
                    if (hfilldate == "Tuesday")
                    {
                        hfilldate = "Tue";
                    }
                    if (hfilldate == "Wednesday")
                    {
                        hfilldate = "Wed";
                    }
                    if (hfilldate == "Thursday")
                    {
                        hfilldate = "Thu";
                    }
                    if (hfilldate == "Friday")
                    {
                        hfilldate = "Fri";
                    }
                    if (hfilldate == "Saturday")
                    {
                        hfilldate = "Sat";
                    }
                    if (chkdate.Checked == true)
                    {
                        datefun = Convert.ToString(headerdate1);
                        string[] datefun1 = datefun.Split(new Char[] { ' ' });
                        string spilktdate = datefun1[0].ToString();

                        string[] orderdate = spilktdate.Split(new Char[] { '/' });
                        datefun = orderdate[1] + "/" + orderdate[0] + "/" + orderdate[2];
                        hfilldate = hfilldate + "-" + datefun;
                    }
                    dayordate = new System.Text.StringBuilder(hfilldate);

                    AddTableColumn(data, dayordate);
                    abscnt++;
                    arrColHdrNames1.Add("No Of Absentees");
                    arrColHdrNames2.Add("No Of Absentees");
                    arrColHdrNames3.Add(hfilldate);
                }
                headerdate1 = headerdate1.AddDays(1);
            }
            headercont = "No Of Absentees " + "-" + abscnt.ToString();
            dichear.Add(2, headercont);





            //With Permission Header


            for (DateTime headerdate1 = headerdate; headerdate1 < headtodate; headerdate1.AddDays(1))
            {

                string hfilldate = Convert.ToString(headerdate1.DayOfWeek);
                if (hfilldate != "Sunday")
                {
                    if (hfilldate == "Monday")
                    {
                        hfilldate = "Mon";
                    }
                    if (hfilldate == "Tuesday")
                    {
                        hfilldate = "Tue";
                    }
                    if (hfilldate == "Wednesday")
                    {
                        hfilldate = "Wed";
                    }
                    if (hfilldate == "Thursday")
                    {
                        hfilldate = "Thu";
                    }
                    if (hfilldate == "Friday")
                    {
                        hfilldate = "Fri";
                    }
                    if (hfilldate == "Saturday")
                    {
                        hfilldate = "Sat";
                    }
                    if (chkdate.Checked == true)
                    {
                        datefun = Convert.ToString(headerdate1);
                        string[] datefun1 = datefun.Split(new Char[] { ' ' });
                        string spilktdate = datefun1[0].ToString();

                        string[] orderdate = spilktdate.Split(new Char[] { '/' });
                        datefun = orderdate[1] + "/" + orderdate[0] + "/" + orderdate[2];
                        hfilldate = hfilldate + "-" + datefun;
                    }
                    dayordate = new System.Text.StringBuilder(hfilldate);

                    AddTableColumn(data, dayordate);

                    withpercnt++;

                    arrColHdrNames1.Add("Absentees");
                    arrColHdrNames2.Add("With Permission");
                    arrColHdrNames3.Add(hfilldate);
                }
                headerdate1 = headerdate1.AddDays(1);
            }
            headercont = "With Permission " + "-" + withpercnt.ToString();
            dichear.Add(3, headercont);

            //with Out Permission Header


            for (DateTime headerdate1 = headerdate; headerdate1 < headtodate; headerdate1.AddDays(1))
            {
                string hfilldate = Convert.ToString(headerdate1.DayOfWeek);
                if (hfilldate != "Sunday")
                {
                    if (hfilldate == "Monday")
                    {
                        hfilldate = "Mon";
                    }
                    if (hfilldate == "Tuesday")
                    {
                        hfilldate = "Tue";
                    }
                    if (hfilldate == "Wednesday")
                    {
                        hfilldate = "Wed";
                    }
                    if (hfilldate == "Thursday")
                    {
                        hfilldate = "Thu";
                    }
                    if (hfilldate == "Friday")
                    {
                        hfilldate = "Fri";
                    }
                    if (hfilldate == "Saturday")
                    {
                        hfilldate = "Sat";
                    }
                    if (chkdate.Checked == true)
                    {
                        datefun = Convert.ToString(headerdate1);
                        string[] datefun1 = datefun.Split(new Char[] { ' ' });
                        string spilktdate = datefun1[0].ToString();

                        string[] orderdate = spilktdate.Split(new Char[] { '/' });
                        datefun = orderdate[1] + "/" + orderdate[0] + "/" + orderdate[2];
                        hfilldate = hfilldate + "-" + datefun;
                    }
                    dayordate = new System.Text.StringBuilder(hfilldate);

                    AddTableColumn(data, dayordate);
                    withoutpercnt++;
                    arrColHdrNames1.Add("Absentees");
                    arrColHdrNames2.Add("With Out Permission");
                    arrColHdrNames3.Add(hfilldate);
                }
                headerdate1 = headerdate1.AddDays(1);
            }
            headercont = "With Out Permission " + "-" + withoutpercnt.ToString();
            dichear.Add(4, headercont);



            // % of Attendance



            for (DateTime headerdate1 = headerdate; headerdate1 < headtodate; headerdate1.AddDays(1))
            {
                string hfilldate = Convert.ToString(headerdate1.DayOfWeek);
                if (hfilldate != "Sunday")
                {
                    if (hfilldate == "Monday")
                    {
                        hfilldate = "Mon";
                    }
                    if (hfilldate == "Tuesday")
                    {
                        hfilldate = "Tue";
                    }
                    if (hfilldate == "Wednesday")
                    {
                        hfilldate = "Wed";
                    }
                    if (hfilldate == "Thursday")
                    {
                        hfilldate = "Thu";
                    }
                    if (hfilldate == "Friday")
                    {
                        hfilldate = "Fri";
                    }
                    if (hfilldate == "Saturday")
                    {
                        hfilldate = "Sat";
                    }
                    if (chkdate.Checked == true)
                    {
                        datefun = Convert.ToString(headerdate1);
                        string[] datefun1 = datefun.Split(new Char[] { ' ' });
                        string spilktdate = datefun1[0].ToString();

                        string[] orderdate = spilktdate.Split(new Char[] { '/' });
                        datefun = orderdate[1] + "/" + orderdate[0] + "/" + orderdate[2];
                        hfilldate = hfilldate + "-" + datefun;
                    }
                    dayordate = new System.Text.StringBuilder(hfilldate);

                    AddTableColumn(data, dayordate);
                    perabscnt++;
                    arrColHdrNames1.Add("% of Attendance");
                    arrColHdrNames2.Add("% of Attendance");
                    arrColHdrNames3.Add(hfilldate);
                }
                headerdate1 = headerdate1.AddDays(1);
            }
            headercont = "% of Attendance " + "-" + perabscnt.ToString();
            dichear.Add(5, headercont);


            DataRow drHdr1 = data.NewRow();
            DataRow drHdr2 = data.NewRow();
            DataRow drHdr3 = data.NewRow();

            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames1[grCol];
                drHdr2[grCol] = arrColHdrNames2[grCol];
                drHdr3[grCol] = arrColHdrNames3[grCol];

            }

            data.Rows.Add(drHdr1);
            data.Rows.Add(drHdr2);
            data.Rows.Add(drHdr3);
        }
        catch
        {


        }

    }

    public void persentmonthcal()
    {
        Boolean isadm = false;
        try
        {
            int my_un_mark = 0;
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;

            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;

            notconsider_value = 0;

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
            ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
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


                //------------------------------------------------------------------
                int iscount = 0;

                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + branch_code + " and semester=" + semester + "";
                DataSet dsholiday = new DataSet();
                dsholiday = d2.select_method(sqlstr_holiday, hat, "Text");
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
                        holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        // holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
                    }
                }

                if (ds3.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
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
                        holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                    }
                }

                if (ds3.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
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
                                                    if (absentcolumn == "")
                                                    {
                                                        absentcolumn = date;
                                                    }
                                                    else
                                                    {
                                                        absentcolumn = absentcolumn + ',' + date;
                                                    }
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
                                    temp_unmark = 0;
                                    njhr = 0;

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
                                                    if (absentcolumn == "")
                                                    {
                                                        absentcolumn = date;
                                                    }
                                                    else
                                                    {
                                                        absentcolumn = absentcolumn + ',' + date;
                                                    }
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
                                    unmark = 0;
                                    njhr = 0;

                                    //Check Permissioin Count
                                    if (absentcolumn != "")
                                    {
                                        string[] absentcolumnspilt = absentcolumn.Split(',');

                                        string absentcolumn1 = absentcolumnspilt[0];
                                        string absentperquery = "select " + absentcolumn1 + " from attendance_withreason where roll_no='" + dd + "' and month_year between " + cal_from_date + " and " + cal_to_date + " order by month_year";
                                        dsabper.Dispose();
                                        dsabper.Reset();
                                        string permission = "";
                                        permission = d2.GetFunctionv(absentperquery);
                                        if (permission == "")
                                        {
                                            absentnotper = Absent;
                                        }
                                        else
                                        {
                                            absentper = Absent;
                                        }
                                    }
                                    absentcolumn = "";

                                    //With Permissioin Count
                                    if (!hatabpet.Contains(Convert.ToString(dumm_from_date)))
                                    {
                                        hatabpet.Add(Convert.ToString(dumm_from_date), absentper);
                                    }
                                    else
                                    {
                                        Double Absentstudent = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(dumm_from_date), hatabpet));
                                        absentper = absentper + Absentstudent;
                                        hatabpet[Convert.ToString(dumm_from_date)] = absentper;
                                    }

                                    //With out Permissioin Count
                                    if (!hatabnotpet.Contains(Convert.ToString(dumm_from_date)))
                                    {
                                        hatabnotpet.Add(Convert.ToString(dumm_from_date), absentnotper);
                                    }
                                    else
                                    {
                                        Double Absentstudent = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(dumm_from_date), hatabnotpet));
                                        absentnotper = absentnotper + Absentstudent;
                                        hatabnotpet[Convert.ToString(dumm_from_date)] = absentnotper;
                                    }
                                    //Present Count
                                    if (!hatdate.Contains(Convert.ToString(dumm_from_date)))
                                    {
                                        hatdate.Add(Convert.ToString(dumm_from_date), Present);
                                    }
                                    else
                                    {
                                        Double presentstudent = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(dumm_from_date), hatdate));
                                        Present = Present + presentstudent;
                                        hatdate[Convert.ToString(dumm_from_date)] = Present;
                                    }

                                    //Absent Count
                                    if (!hatabsent.Contains(Convert.ToString(dumm_from_date)))
                                    {
                                        hatabsent.Add(Convert.ToString(dumm_from_date), Absent);
                                    }
                                    else
                                    {
                                        Double Absentstudent = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(dumm_from_date), hatabsent));
                                        Absent = Absent + Absentstudent;
                                        hatabsent[Convert.ToString(dumm_from_date)] = Absent;
                                    }



                                    Present = 0;
                                    absentnotper = 0;
                                    absentper = 0;
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
            lblnorec.Text = ex.ToString();
        }
    }

    private static void AddTableColumn(DataTable resultsTable, StringBuilder ColumnName)
    {
        try
        {
            DataColumn tableCol = new DataColumn(ColumnName.ToString());
            resultsTable.Columns.Add(tableCol);
        }
        catch (System.Data.DuplicateNameException)
        {
            ColumnName.Append(" ");
            AddTableColumn(resultsTable, ColumnName);
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
                for (int j = 2; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
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
    private object GetCorrespondingKey(string p, string valuereturn)
    {
        throw new NotImplementedException();
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {

        try
        {
            errmsg.Visible = true;
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(Showgrid, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
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
        //        print = "Student Weekly Attendance Report" + i;
        //        //FpStudentAttendance.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
        //        //Aruna on 26feb2013============================
        //        string szPath = appPath + "/Report/";
        //        string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

        //        FpStudentAttendance.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
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
        // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    { }

    protected void txtfrom_TextChanged(object sender, EventArgs e)
    {
        string[] splitfrom = txtfrom.Text.Split(new Char[] { '/' });
        string[] splitto = txtto.Text.Split(new char[] { '/' });
        string fdate = splitfrom[1] + '/' + splitfrom[0] + '/' + splitfrom[2];
        string tdate = splitto[1] + '/' + splitto[0] + '/' + splitto[2];
        DateTime fromdate = Convert.ToDateTime(fdate);
        DateTime todate = Convert.ToDateTime(tdate);
        if (fromdate > todate)
        {
            Showgrid.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblexcelname.Visible = false;
            errmsg.Text = "Please Enter To Date Grater Than From Date";
            errmsg.Visible = true;
        }
        else
        {
            errmsg.Visible = false;
        }
    }
    protected void txtto_TextChanged(object sender, EventArgs e)
    {
        string[] splitfrom = txtfrom.Text.Split(new Char[] { '/' });
        string[] splitto = txtto.Text.Split(new char[] { '/' });
        string fdate = splitfrom[1] + '/' + splitfrom[0] + '/' + splitfrom[2];
        string tdate = splitto[1] + '/' + splitto[0] + '/' + splitto[2];
        DateTime fromdate = Convert.ToDateTime(fdate);
        DateTime todate = Convert.ToDateTime(tdate);
        if (fromdate > todate)
        {
            Showgrid.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblexcelname.Visible = false;
            errmsg.Text = "Please Enter To Date Grater Than From Date";
            errmsg.Visible = true;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
        }
        else
        {
            errmsg.Visible = false;
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string ss = null;
        Session["column_header_row_count"] = 3;
        string degreedetails = "Date : " + txtfrom.Text.ToString() + " - " + txtto.Text.ToString();
        degreedetails = "Student Weekly Report@" + date;
        string pagename = "StudentWeeklyAttendance.aspx";
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;
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
        spReportName.InnerHtml = "Student Weekly Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
}
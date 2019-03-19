using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Net.Mail;
using System.Net;

public partial class Day_Wise_Absentees_sm : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds1 = new DataSet();

    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable hatsetrights = new Hashtable();

    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    DateTime dumm_from_date;
    DateTime Admission_date;

    TimeSpan ts;
    Boolean deptflag = false;

    string batch = "";
    string degree = "";
    string sem = "";
    string sections = "";
    string frdate, todate;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string dd = "";
    string diff_date;
    string value, date;
    string tempvalue = "-1";
    string usercode = "", collegecode = "", singleuser = "", group_user = "";
    string value_holi_status = "";
    string split_holiday_status_1 = "", split_holiday_status_2 = "";

    string[] split_holiday_status = new string[1000];

    double dif_date = 0;
    double dif_date1 = 0;
    double per_perhrs, per_abshrs, per_leavehrs;
    double per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_holidate;
    double njhr, njdate, per_njdate;
    double per_per_hrs;
    Double leavfinaeamount = 0;

    Double minpresday = 0, nohrsprsentperday = 0, noofdaypresen = 0;
    Double moringabsentfine = 0, eveingabsentfine = 0, studentabsentfine = 0;


    int mmyycount = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0;
    int notconsider_value = 0;
    int moncount;
    int unmark;
    int NoHrs = 0;
    int fnhrs = 0;
    int minpresI = 0;
    int count;
    int next = 0;
    int minpresII = 0;
    int rows_count;
    int ObtValue = -1;
    int cal_from_date, cal_from_date_tmp;
    int cal_to_date, cal_to_date_tmp;
    int per_dum_unmark, dum_unmark;
    int tot_per_hrs;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode = Session["collegecode"].ToString();
        lbl_err.Visible = false;
        if (!IsPostBack)
        {
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            clear();
            bindbatch();
            binddegree();
            bindbranch();
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim();
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim();
            }

            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["attdaywisecla"] = "0";
            Session["Fineleaveamount"] = "0";
            string daywisecal = da.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
            if (daywisecal.Trim() == "1")
            {
                Session["attdaywisecla"] = "1";
            }
            daywisecal = da.GetFunction("select value from master_settings where settings='Fine Amount Not For Leave'");
            if (daywisecal.Trim() == "1")
            {
                Session["Fineleaveamount"] = "1";
            }
            rb1.Checked = true;
            txtfromrange.Enabled = true;
            TextBox1.Enabled = false;
            TextBox2.Enabled = false;
            string Master = "select * from Master_Settings where " + grouporusercode;
            DataSet ds = da.select_method_wo_parameter(Master, "Text");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Rollflag"] = "1";
                }
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Regflag"] = "1";
                }
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Studflag"] = "1";
                }
            }

            chklscolumn.Items[1].Selected = true;
            chklscolumn.Items[2].Selected = true;
            chklscolumn.Items[3].Selected = true;
            chklscolumn.Items[4].Selected = true;
            chklscolumn.Items[9].Selected = true;
            chklscolumn.Items[13].Selected = true;
            chklscolumn.Items[14].Selected = true;
        }

    }

    public void bindbatch()
    {
        try
        {


            Chklst_batch.Items.Clear();
            Chk_batch.Checked = false;
            txt_batch.Text = "---Select---";
            // ds = da.select_method_wo_parameter("bind_batch", "sp");

            string Master1 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string collegecode = Session["collegecode"].ToString();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' order by batch_year desc";
            ds = da.select_method_wo_parameter(strbinddegree, "Text");

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
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = da.select_method("bind_degree", hat, "sp");
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
                txt_degree.Text = "Degree" + "(" + Chklst_degree.Items.Count + ")";
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
                    txt_branch.Text = "Branch(" + (chklst_branch.Items.Count) + ")";
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
        FpSpread1.Visible = false;
        lbl_err.Visible = false;
        Printcontrol.Visible = false;
        lblmsg.Visible = false;
        txtmsg.Visible = false;
        btnmsg.Visible = false;
        txtmsg.Text = "";
    }

    protected void txtfromrange_TextChanged(object sender, EventArgs e)
    {
        try
        {

            clear();
            if (txtfromrange.Text.ToString().Trim() != "")
            {
                //int frange = Convert.ToInt32(txtfromrange.Text.ToString());
                //if (frange > 100)
                //{
                //    txtfromrange.Text = "";
                //    lbl_err.Visible = true;
                //    lbl_err.Text = "Please Enter Lesser than equal to 100";

                //}
                //if (txttorange.Text.ToString().Trim() != "")
                //{
                //    int trange = Convert.ToInt32(txttorange.Text.ToString());

                //    if (frange > trange)
                //    {
                //        txtfromrange.Text = "";
                //        lbl_err.Visible = true;
                //        lbl_err.Text = "Please Enter From Lesser than or equal to To";
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void txttorange_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            //if (txttorange.Text.ToString().Trim() != "")
            //{
            //    int trange = Convert.ToInt32(txttorange.Text.ToString());
            //    if (trange > 100)
            //    {

            //        txttorange.Text = "";
            //        lbl_err.Visible = true;
            //        lbl_err.Text = "Please Enter Lesser than equal to 100";
            //    }
            //    if (txtfromrange.Text.ToString().Trim() != "")
            //    {
            //        int frange = Convert.ToInt32(txtfromrange.Text.ToString());
            //        if (frange > trange)
            //        {
            //            txttorange.Text = "";
            //            lbl_err.Visible = true;
            //            lbl_err.Text = "Please Enter From Lesser than or equal to To";
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {

            string spread = "";
            Control control = null;
            string ctrlname = Page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                string[] spiltspreadname = ctrlname.Split('$');
                if (spiltspreadname.GetUpperBound(0) > 1)
                {
                    string getrowxol = spiltspreadname[3].ToString().Trim();
                    string[] spr = getrowxol.Split(',');
                    if (spr.GetUpperBound(0) == 1)
                    {
                        int arow = Convert.ToInt32(spr[0]);
                        int acol = Convert.ToInt32(spr[1]);
                        if (arow == 0 && acol > 4)
                        {
                            string setval = e.EditValues[acol].ToString();
                            int setvalcel = 0;
                            if (setval.Trim().ToLower() == "true" || setval.Trim() == "1")
                            {
                                setvalcel = 1;
                            }
                            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
                            {
                                FpSpread1.Sheets[0].Cells[r, acol].Value = setvalcel;
                            }
                        }
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
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string fineamountmag = "";
            clear();
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


            string strbatchsectionrights = "";
            if (Session["Single_User"].ToString() == "True")
            {
                strbatchsectionrights = "and user_id='" + Session["UserCode"].ToString() + "'";
            }
            else
            {
                string groupcode = Session["group_code"].ToString();
                string[] from_split = groupcode.Split(';');
                if (from_split[0].ToString() != "")
                {
                    strbatchsectionrights = "and user_id='" + from_split[0].ToString() + "'";
                }
            }

            hatsetrights.Clear();
            string strbatchsectionsrights = "select sections,batch_year from tbl_attendance_rights where Batch_year in(" + testbatchyear + ") " + strbatchsectionrights + "";
            DataSet dssections = da.select_method_wo_parameter(strbatchsectionsrights, "Text");
            if (dssections.Tables[0].Rows.Count > 0)
            {
                for (int se = 0; se < dssections.Tables[0].Rows.Count; se++)
                {
                    string strval = dssections.Tables[0].Rows[se]["sections"].ToString();
                    string bathrights = dssections.Tables[0].Rows[se]["batch_year"].ToString();
                    string[] spsec = strval.Split(',');
                    for (int sp = 0; sp <= spsec.GetUpperBound(0); sp++)
                    {
                        string valu = spsec[sp].ToString().Trim();
                        if (!hatsetrights.Contains(bathrights + ',' + valu))
                        {
                            hatsetrights.Add(bathrights + ',' + valu, bathrights + ',' + valu);
                        }
                    }
                }
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Update The Batch Year and Sections Rights";
                return;
            }
            Double absentdaysall = 0;
            string absentdaysfrom = txtfromrange.Text.ToString();
            if (absentdaysfrom.ToString() != "")
            {
                absentdaysall = Convert.ToDouble(absentdaysfrom);
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


            string strorder = ",r.Roll_No";
            string serialno = da.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = ",r.serialno";
            }
            else
            {
                string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = ",r.Roll_No";
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
                    strorder = ",r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = ",r.Roll_No,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = ",r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = ",r.Roll_No,r.Stud_Name";
                }
            }
            Boolean rowflag = false;

            string strquery = "select r.Batch_Year,c.Course_Name,de.dept_acronym,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No,r.Roll_No,r.Stud_Name,r.Stud_Type,r.serialno,r.Adm_Date,a.StuPer_Id,a.Student_Mobile,a.parentF_Mobile,a.parentM_Mobile from Registration r,Degree d,Department de,Course c,applyn a where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + testbatchyear + ") and r.degree_code in (" + testbranch + ")  order by r.Batch_Year desc,c.Course_Name,de.Dept_name,r.Sections" + strorder;
            ds4 = da.select_method_wo_parameter(strquery, "text");
            if (ds4.Tables[0].Rows.Count > 0)
            {

                string morfi = da.GetFunction("select value from Master_Settings where settings='I Half Absent Fine'");
                if (morfi.Trim() != "" && morfi.Trim() != "0")
                {
                    moringabsentfine = Convert.ToDouble(morfi);
                }
                else
                {
                    fineamountmag = "Morning Absent Fine Amount Not Set";
                }

                morfi = da.GetFunction("select value from Master_Settings where settings='II Half Absent Fine'");
                if (morfi.Trim() != "" && morfi.Trim() != "0")
                {
                    eveingabsentfine = Convert.ToDouble(morfi);
                }
                else
                {
                    if (fineamountmag.Trim() == "")
                    {
                        fineamountmag = "Evening Absent Fine Amount Not Set";
                    }
                    else
                    {
                        fineamountmag = "Day Wise Absent Fine Amount Not Set";
                    }
                }

                FpSpread1.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnmasterprint.Visible = true;
                lblmsg.Visible = true;
                txtmsg.Visible = true;
                btnmsg.Visible = true;

                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;

                FpSpread1.Sheets[0].ColumnCount = 63;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.NO";
                FpSpread1.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[0].Width = 50;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 21].Text = "S.NO";
                FpSpread1.Sheets[0].Columns[21].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[21].Width = 50;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 42].Text = "S.NO";
                FpSpread1.Sheets[0].Columns[42].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[42].Width = 50;

                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(21, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(42, FarPoint.Web.Spread.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree Details";
                FpSpread1.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[1].Width = 350;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 22].Text = "Degree Details";
                FpSpread1.Sheets[0].Columns[22].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[22].Width = 350;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 43].Text = "Degree Details";
                FpSpread1.Sheets[0].Columns[43].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[43].Width = 350;

                if (chklscolumn.Items[0].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[1].Visible = true;
                    FpSpread1.Sheets[0].Columns[22].Visible = true;
                    FpSpread1.Sheets[0].Columns[43].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[1].Visible = false;
                    FpSpread1.Sheets[0].Columns[22].Visible = false;
                    FpSpread1.Sheets[0].Columns[43].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                FpSpread1.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[2].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 23].Text = "Roll No";
                FpSpread1.Sheets[0].Columns[23].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[23].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 44].Text = "Roll No";
                FpSpread1.Sheets[0].Columns[44].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[44].Width = 100;



                if (Session["Rollflag"].ToString() == "1")
                {
                    FpSpread1.Sheets[0].Columns[2].Visible = true;
                    FpSpread1.Sheets[0].Columns[23].Visible = true;
                    FpSpread1.Sheets[0].Columns[44].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[2].Visible = false;
                    FpSpread1.Sheets[0].Columns[23].Visible = false;
                    FpSpread1.Sheets[0].Columns[44].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                FpSpread1.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[3].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 24].Text = "Reg No";
                FpSpread1.Sheets[0].Columns[24].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[24].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 45].Text = "Reg No";
                FpSpread1.Sheets[0].Columns[45].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[45].Width = 100;


                if (Session["Regflag"].ToString() == "1")
                {
                    FpSpread1.Sheets[0].Columns[3].Visible = true;
                    FpSpread1.Sheets[0].Columns[24].Visible = true;
                    FpSpread1.Sheets[0].Columns[45].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[3].Visible = false;
                    FpSpread1.Sheets[0].Columns[24].Visible = false;
                    FpSpread1.Sheets[0].Columns[45].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                FpSpread1.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[4].Width = 150;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 25].Text = "Student Name";
                FpSpread1.Sheets[0].Columns[25].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[25].Width = 150;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 46].Text = "Student Name";
                FpSpread1.Sheets[0].Columns[46].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[46].Width = 150;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Type";
                FpSpread1.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[5].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 26].Text = "Student Type";
                FpSpread1.Sheets[0].Columns[26].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[26].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 47].Text = "Student Type";
                FpSpread1.Sheets[0].Columns[47].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[47].Width = 100;

                if (Session["Studflag"].ToString() == "1")
                {
                    FpSpread1.Sheets[0].Columns[5].Visible = true;
                    FpSpread1.Sheets[0].Columns[26].Visible = true;
                    FpSpread1.Sheets[0].Columns[47].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[5].Visible = false;
                    FpSpread1.Sheets[0].Columns[26].Visible = false;
                    FpSpread1.Sheets[0].Columns[47].Visible = false;
                }


                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Conducted Days";
                FpSpread1.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[6].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 27].Text = "Conducted Days";
                FpSpread1.Sheets[0].Columns[27].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[27].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 48].Text = "Conducted Days";
                FpSpread1.Sheets[0].Columns[48].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[48].Width = 80;

                if (chklscolumn.Items[1].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[6].Visible = true;
                    FpSpread1.Sheets[0].Columns[27].Visible = true;
                    FpSpread1.Sheets[0].Columns[48].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[6].Visible = false;
                    FpSpread1.Sheets[0].Columns[27].Visible = false;
                    FpSpread1.Sheets[0].Columns[48].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Present Days";
                FpSpread1.Sheets[0].Columns[7].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[7].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 28].Text = "Present Days";
                FpSpread1.Sheets[0].Columns[28].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[28].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 49].Text = "Present Days";
                FpSpread1.Sheets[0].Columns[49].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[49].Width = 80;

                if (chklscolumn.Items[2].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[7].Visible = true;
                    FpSpread1.Sheets[0].Columns[28].Visible = true;
                    FpSpread1.Sheets[0].Columns[49].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[7].Visible = false;
                    FpSpread1.Sheets[0].Columns[28].Visible = false;
                    FpSpread1.Sheets[0].Columns[49].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Absent Days";
                FpSpread1.Sheets[0].Columns[8].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[8].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 29].Text = "Absent Days";
                FpSpread1.Sheets[0].Columns[29].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[29].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 50].Text = "Absent Days";
                FpSpread1.Sheets[0].Columns[50].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[50].Width = 80;

                if (chklscolumn.Items[3].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[8].Visible = true;
                    FpSpread1.Sheets[0].Columns[29].Visible = true;
                    FpSpread1.Sheets[0].Columns[50].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[8].Visible = false;
                    FpSpread1.Sheets[0].Columns[29].Visible = false;
                    FpSpread1.Sheets[0].Columns[50].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Attendance Precentage";
                FpSpread1.Sheets[0].Columns[9].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[9].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 30].Text = "Attendance Precentage";
                FpSpread1.Sheets[0].Columns[30].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[30].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 51].Text = "Attendance Precentage";
                FpSpread1.Sheets[0].Columns[51].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[51].Width = 80;

                if (chklscolumn.Items[4].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[9].Visible = true;
                    FpSpread1.Sheets[0].Columns[30].Visible = true;
                    FpSpread1.Sheets[0].Columns[51].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[9].Visible = false;
                    FpSpread1.Sheets[0].Columns[30].Visible = false;
                    FpSpread1.Sheets[0].Columns[51].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Conducted Periods";
                FpSpread1.Sheets[0].Columns[10].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[10].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 31].Text = "Conducted Periods";
                FpSpread1.Sheets[0].Columns[31].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[31].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 52].Text = "Conducted Periods";
                FpSpread1.Sheets[0].Columns[52].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[52].Width = 80;

                if (chklscolumn.Items[5].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[10].Visible = true;
                    FpSpread1.Sheets[0].Columns[31].Visible = true;
                    FpSpread1.Sheets[0].Columns[52].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[10].Visible = false;
                    FpSpread1.Sheets[0].Columns[31].Visible = false;
                    FpSpread1.Sheets[0].Columns[52].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Attend Periods";
                FpSpread1.Sheets[0].Columns[11].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[11].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 32].Text = "Attend Periods";
                FpSpread1.Sheets[0].Columns[32].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[32].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 53].Text = "Attend Periods";
                FpSpread1.Sheets[0].Columns[53].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[53].Width = 80;

                if (chklscolumn.Items[6].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[11].Visible = true;
                    FpSpread1.Sheets[0].Columns[32].Visible = true;
                    FpSpread1.Sheets[0].Columns[53].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[11].Visible = false;
                    FpSpread1.Sheets[0].Columns[32].Visible = false;
                    FpSpread1.Sheets[0].Columns[53].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Absent Periods";
                FpSpread1.Sheets[0].Columns[12].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[12].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 33].Text = "Absent Periods";
                FpSpread1.Sheets[0].Columns[33].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[33].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 54].Text = "Absent Periods";
                FpSpread1.Sheets[0].Columns[54].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[54].Width = 80;
                if (chklscolumn.Items[7].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[12].Visible = true;
                    FpSpread1.Sheets[0].Columns[33].Visible = true;
                    FpSpread1.Sheets[0].Columns[54].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[12].Visible = false;
                    FpSpread1.Sheets[0].Columns[33].Visible = false;
                    FpSpread1.Sheets[0].Columns[54].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Attendance Precentage";
                FpSpread1.Sheets[0].Columns[13].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[13].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 34].Text = "Attendance Precentage";
                FpSpread1.Sheets[0].Columns[34].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[34].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 55].Text = "Attendance Precentage";
                FpSpread1.Sheets[0].Columns[55].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[55].Width = 80;
                if (chklscolumn.Items[8].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[13].Visible = true;
                    FpSpread1.Sheets[0].Columns[34].Visible = true;
                    FpSpread1.Sheets[0].Columns[55].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[13].Visible = false;
                    FpSpread1.Sheets[0].Columns[34].Visible = false;
                    FpSpread1.Sheets[0].Columns[55].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Student Mobile";
                FpSpread1.Sheets[0].Columns[14].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[14].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 35].Text = "Student Mobile";
                FpSpread1.Sheets[0].Columns[35].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[35].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 56].Text = "Student Mobile";
                FpSpread1.Sheets[0].Columns[56].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[56].Width = 100;
                if (chklscolumn.Items[9].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[14].Visible = true;
                    FpSpread1.Sheets[0].Columns[35].Visible = true;
                    FpSpread1.Sheets[0].Columns[56].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[14].Visible = false;
                    FpSpread1.Sheets[0].Columns[35].Visible = false;
                    FpSpread1.Sheets[0].Columns[56].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Student Email";
                FpSpread1.Sheets[0].Columns[15].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[15].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 36].Text = "Student Email";
                FpSpread1.Sheets[0].Columns[36].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[36].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 57].Text = "Student Email";
                FpSpread1.Sheets[0].Columns[57].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[57].Width = 100;
                if (chklscolumn.Items[10].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[15].Visible = true;
                    FpSpread1.Sheets[0].Columns[36].Visible = true;
                    FpSpread1.Sheets[0].Columns[57].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[15].Visible = false;
                    FpSpread1.Sheets[0].Columns[36].Visible = false;
                    FpSpread1.Sheets[0].Columns[57].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 16].Text = "Father Mobile";
                FpSpread1.Sheets[0].Columns[16].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[16].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 37].Text = "Father Mobile";
                FpSpread1.Sheets[0].Columns[37].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[37].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 58].Text = "Father Mobile";
                FpSpread1.Sheets[0].Columns[58].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[58].Width = 100;
                if (chklscolumn.Items[11].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[16].Visible = true;
                    FpSpread1.Sheets[0].Columns[37].Visible = true;
                    FpSpread1.Sheets[0].Columns[58].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[16].Visible = false;
                    FpSpread1.Sheets[0].Columns[37].Visible = false;
                    FpSpread1.Sheets[0].Columns[58].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Mother Mobile";
                FpSpread1.Sheets[0].Columns[17].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[17].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 38].Text = "Mother Mobile";
                FpSpread1.Sheets[0].Columns[38].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[38].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 59].Text = "Mother Mobile";
                FpSpread1.Sheets[0].Columns[59].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[59].Width = 100;
                if (chklscolumn.Items[12].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[17].Visible = true;
                    FpSpread1.Sheets[0].Columns[38].Visible = true;
                    FpSpread1.Sheets[0].Columns[59].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[17].Visible = false;
                    FpSpread1.Sheets[0].Columns[38].Visible = false;
                    FpSpread1.Sheets[0].Columns[59].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 18].Text = "Fine Amount";
                FpSpread1.Sheets[0].Columns[18].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[18].Width = 30;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 39].Text = "Fine Amount";
                FpSpread1.Sheets[0].Columns[39].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[39].Width = 30;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 60].Text = "Fine Amount";
                FpSpread1.Sheets[0].Columns[60].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[60].Width = 30;
                if (chklscolumn.Items[13].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[18].Visible = true;
                    FpSpread1.Sheets[0].Columns[39].Visible = true;
                    FpSpread1.Sheets[0].Columns[60].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[18].Visible = false;
                    FpSpread1.Sheets[0].Columns[39].Visible = false;
                    FpSpread1.Sheets[0].Columns[60].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 19].Text = "Select";
                FpSpread1.Sheets[0].Columns[19].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[19].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 40].Text = "Select";
                FpSpread1.Sheets[0].Columns[40].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[40].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 61].Text = "Select";
                FpSpread1.Sheets[0].Columns[61].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[61].Width = 80;
                if (chklscolumn.Items[14].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[19].Visible = true;
                    FpSpread1.Sheets[0].Columns[40].Visible = true;
                    FpSpread1.Sheets[0].Columns[61].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[19].Visible = false;
                    FpSpread1.Sheets[0].Columns[40].Visible = false;
                    FpSpread1.Sheets[0].Columns[61].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 20].Text = "Remark";
                FpSpread1.Sheets[0].Columns[20].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[20].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 41].Text = "Remark";
                FpSpread1.Sheets[0].Columns[41].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[41].Width = 80;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 20].Text = "Remark";
                FpSpread1.Sheets[0].Columns[62].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[62].Width = 80;

                if (chklscolumn.Items[15].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[20].Visible = true;
                    FpSpread1.Sheets[0].Columns[41].Visible = true;
                    FpSpread1.Sheets[0].Columns[62].Visible = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[20].Visible = false;
                    FpSpread1.Sheets[0].Columns[41].Visible = false;
                    FpSpread1.Sheets[0].Columns[62].Visible = false;
                }

                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = System.Drawing.Color.Black;
                style2.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

                FpSpread1.Sheets[0].SheetName = " ";
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Visible = true;

                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 19);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 19].CellType = chkall;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 19].HorizontalAlign = HorizontalAlign.Center;

                hat.Clear();
                hat.Add("colege_code", Session["collegecode"].ToString());
                ds1 = da.select_method("ATT_MASTER_SETTING", hat, "sp");
                count = ds1.Tables[0].Rows.Count;
                string tempdegree = "";
                int sno = 0;
                int newrow = 0;
                int columnval = 0;
                string tempdegreeddetails = "";
                bool isval = false;
                for (rows_count = 0; rows_count < ds4.Tables[0].Rows.Count; rows_count++)
                {
                    studentabsentfine = 0;
                    batch = ds4.Tables[0].Rows[rows_count]["Batch_Year"].ToString();
                    degree = ds4.Tables[0].Rows[rows_count]["degree_code"].ToString();
                    sem = ds4.Tables[0].Rows[rows_count]["Current_Semester"].ToString();
                    sections = ds4.Tables[0].Rows[rows_count]["sections"].ToString();
                    isval = false;
                    if (hatsetrights.Contains(batch + ',' + sections.Trim()))
                    {
                        string course = ds4.Tables[0].Rows[rows_count]["Course_Name"].ToString();
                        string department = ds4.Tables[0].Rows[rows_count]["dept_acronym"].ToString();
                        string roll = ds4.Tables[0].Rows[rows_count]["roll_no"].ToString();
                        string reg = ds4.Tables[0].Rows[rows_count]["reg_no"].ToString();
                        string name = ds4.Tables[0].Rows[rows_count]["stud_name"].ToString();
                        string studtype = ds4.Tables[0].Rows[rows_count]["Stud_Type"].ToString();
                        string semail = ds4.Tables[0].Rows[rows_count]["StuPer_Id"].ToString();
                        string smobile = ds4.Tables[0].Rows[rows_count]["Student_Mobile"].ToString();
                        string fmobile = ds4.Tables[0].Rows[rows_count]["parentF_Mobile"].ToString();
                        string mmobile = ds4.Tables[0].Rows[rows_count]["parentM_Mobile"].ToString();

                        string degreedetails = batch + " -" + course + " -" + department + " -" + sem;
                        if (sections.Trim() != "")
                        {
                            degreedetails = degreedetails + " - " + sections;
                        }
                        if (tempdegree != batch + "-" + degree + "-" + sem)
                        {
                            tempdegree = batch + "-" + degree + "-" + sem;
                            deptflag = false;

                            hat.Clear();
                            hat.Add("degree_code", degree);
                            hat.Add("sem_ester", int.Parse(sem.ToString()));
                            ds = da.select_method("period_attnd_schedule", hat, "sp");
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                minpresday = int.Parse(ds.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
                            }
                        }


                        frdate = txtfromdate.Text;
                        todate = txttodate.Text;
                        string dt = frdate;
                        string[] dsplit = dt.Split(new Char[] { '/' });
                        frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                        int demfcal = int.Parse(dsplit[2].ToString());
                        demfcal = demfcal * 12;
                        cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                        cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());

                        string monthcal = cal_from_date.ToString();
                        dt = todate;
                        dsplit = dt.Split(new Char[] { '/' });
                        todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                        int demtcal = int.Parse(dsplit[2].ToString());
                        demtcal = demtcal * 12;
                        cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                        cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());

                        per_from_gendate = Convert.ToDateTime(frdate);
                        per_to_gendate = Convert.ToDateTime(todate);

                        per_abshrs_spl = 0;
                        tot_per_hrs_spl = 0;
                        tot_ondu_spl = 0;
                        tot_ml_spl = 0;
                        tot_conduct_hr_spl = 0;
                        per_workingdays1 = 0;
                        leavfinaeamount = 0;
                        persentmonthcal();


                        Double absenthours = per_workingdays1 - per_per_hrs;
                        double fper = 0;
                        double tper = 0;

                        double.TryParse(TextBox1.Text,out fper);
                        double.TryParse(TextBox2.Text, out tper);

                        string dum_tage_date, dum_tage_hrs;
                        double per_tage_date = ((pre_present_date / per_workingdays) * 100);
                        if (per_tage_date > 100)
                        {
                            per_tage_date = 100;
                        }

                        double per_tage_hrs = (((per_per_hrs) / (per_workingdays1)) * 100);

                        if (per_tage_hrs > 100)
                        {
                            per_tage_hrs = 100;
                        }

                        dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
                        dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));

                        per_tage_hrs = Math.Round(per_tage_hrs, 2);
                        dum_tage_hrs = per_tage_hrs.ToString();

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

                        if (rb2.Checked)//Ra
                        {
                            if (per_tage_date >= fper && per_tage_date <= tper)
                            {
                                isval = true;
                            }
                        }
                        else
                        {
                            if (absentdaysall <= per_absent_date)
                            {
                                isval = true;
                            }
                        }

                        if(isval)
                        //if (absentdaysall <= per_absent_date)
                        {
                            rowflag = true;
                            sno++;
                            newrow++;
                            if (tempdegreeddetails != degreedetails)
                            {
                                newrow = 4;
                                tempdegreeddetails = degreedetails;
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = degreedetails;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.XXLarge;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.LightGreen;
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 63);
                            }

                            //if(true)
                            if ((newrow % 4) == 0)
                            {
                                newrow = 1;
                                FpSpread1.Sheets[0].RowCount++;

                                if ((FpSpread1.Sheets[0].RowCount % 2) == 1)
                                {
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightGray;
                                }
                            }
                            columnval = newrow * 21 - 21;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0 + columnval].Text = sno.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0 + columnval].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0 + columnval].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1 + columnval].Text = degreedetails.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1 + columnval].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1 + columnval].Tag = batch;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2 + columnval].Text = roll.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2 + columnval].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2 + columnval].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2 + columnval].Tag = course;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3 + columnval].Text = reg.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3 + columnval].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3 + columnval].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3 + columnval].Tag = department;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4 + columnval].Text = name.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4 + columnval].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4 + columnval].Tag = sem;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5 + columnval].Text = studtype.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5 + columnval].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5 + columnval].Tag = sections;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6 + columnval].Text = per_workingdays.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6 + columnval].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6 + columnval].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7 + columnval].Text = pre_present_date.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7 + columnval].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7 + columnval].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8 + columnval].Text = per_absent_date.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8 + columnval].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8 + columnval].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9 + columnval].Text = dum_tage_date.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9 + columnval].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9 + columnval].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10 + columnval].Text = per_workingdays1.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10 + columnval].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10 + columnval].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11 + columnval].Text = per_per_hrs.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11 + columnval].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11 + columnval].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12 + columnval].Text = absenthours.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12 + columnval].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12 + columnval].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 13 + columnval].Text = dum_tage_hrs.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 13 + columnval].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9 + columnval].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14 + columnval].Text = smobile.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14 + columnval].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14 + columnval].CellType = txt;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 15 + columnval].Text = semail.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 15 + columnval].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 15 + columnval].CellType = txt;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 16 + columnval].Text = fmobile.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 16 + columnval].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 16 + columnval].CellType = txt;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 17 + columnval].Text = mmobile.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 17 + columnval].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 17 + columnval].CellType = txt;

                            if (Session["Fineleaveamount"].ToString() == "1")
                            {
                                if (leavfinaeamount <= studentabsentfine)
                                {
                                    studentabsentfine = studentabsentfine - leavfinaeamount;
                                }
                            }

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 18 + columnval].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 18 + columnval].Text = studentabsentfine.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 18 + columnval].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 18 + columnval].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 19 + columnval].CellType = chk;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 19 + columnval].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
            if (rowflag == false)
            {
                clear();
                lbl_err.Visible = true;
                lbl_err.Text = "No Records Found";
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = fineamountmag;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
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
        per_abshrs_spl = 0;
        tot_per_hrs_spl = 0;
        tot_conduct_hr_spl = 0;
        tot_ondu_spl = 0;
        tot_ml_spl = 0;
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
        Admission_date = Convert.ToDateTime(admdate);

        dd = ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString();
        hat.Clear();
        hat.Add("std_rollno", ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString());
        hat.Add("from_month", cal_from_date);
        hat.Add("to_month", cal_to_date);
        ds2 = da.select_method("STUD_ATTENDANCE", hat, "sp");
        mmyycount = ds2.Tables[0].Rows.Count;
        moncount = mmyycount - 1;
        if (deptflag == false)
        {
            deptflag = true;
            hat.Clear();
            hat.Add("degree_code", int.Parse(degree));
            hat.Add("sem", int.Parse(sem));
            hat.Add("from_date", frdate.ToString());
            hat.Add("to_date", todate.ToString());
            hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));
            int iscount = 0;
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + degree + " and semester=" + sem;
            DataSet dsholiday = da.select_method_wo_parameter(sqlstr_holiday, "Text");
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            hat.Add("iscount", iscount);
            ds3 = da.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

            DataSet dsondutyva = new DataSet();
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
                    if (!holiday_table11.Contains((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
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
                    if (holiday_table1.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))   //added by Mullai
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
                    if (!holiday_table31.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                    {
                        holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
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
                    if (holiday_table3.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
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
                nohrsprsentperday = 0;
                noofdaypresen = 0;
                isadm = false;
                if (dumm_from_date >= Admission_date)
                {
                    isadm = true;
                    int temp_unmark = 0;

                    for (int i = 1; i <= mmyycount; i++)
                    {
                        ds2.Tables[0].DefaultView.RowFilter = "month_year='" + cal_from_date + "' and roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "'";
                        DataView dvattvalue = ds2.Tables[0].DefaultView;
                        if (dvattvalue.Count > 0)
                        {
                            if (cal_from_date == int.Parse(dvattvalue[0]["month_year"].ToString()))
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
                                    per_leavehrs = 0;
                                    if (split_holiday_status_1 == "1")
                                    {
                                        for (i = 1; i <= fnhrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            value = dvattvalue[0][date].ToString();
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

                                                if (value == "10")
                                                {
                                                    per_leavehrs++;
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
                                                my_un_mark++;
                                            }

                                        }
                                        nohrsprsentperday = per_perhrs + njhr;
                                        if (per_perhrs + njhr >= minpresI)
                                        {
                                            Present += 0.5;
                                            noofdaypresen = 0.5;
                                        }
                                        else if (per_abshrs >= 1)
                                        {
                                            Absent += 0.5;
                                            absent_point += absent_pointer / 2;
                                            studentabsentfine = studentabsentfine + moringabsentfine;
                                            if (per_leavehrs > 0)
                                            {
                                                Leave += 0.5;
                                                leavfinaeamount = leavfinaeamount + moringabsentfine;
                                            }
                                        }
                                        if (njhr >= minpresI)
                                        {
                                            njdate += 0.5;
                                            njdate_mng += 1;
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
                                    per_abshrs = 0;
                                    temp_unmark = 0;
                                    per_leavehrs = 0;
                                    njhr = 0;
                                    int k = fnhrs + 1;
                                    if (split_holiday_status_2 == "1")
                                    {
                                        for (i = k; i <= NoHrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            value = dvattvalue[0][date].ToString();
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
                                                if (value == "10")
                                                {
                                                    per_leavehrs++;
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
                                                my_un_mark++;
                                            }
                                        }
                                        nohrsprsentperday = nohrsprsentperday + per_perhrs + njhr;
                                        if (per_perhrs + njhr >= minpresII)
                                        {
                                            Present += 0.5;
                                            noofdaypresen = noofdaypresen + 0.5;
                                        }
                                        else if (per_abshrs >= 1)
                                        {
                                            Absent += 0.5;
                                            absent_point += absent_pointer / 2;
                                            studentabsentfine = studentabsentfine + eveingabsentfine;
                                            if (per_leavehrs > 0)
                                            {
                                                Leave += 0.5;
                                                leavfinaeamount = leavfinaeamount + eveingabsentfine;
                                            }
                                        }
                                        if (njhr >= minpresII)
                                        {
                                            njdate_evng += 1;
                                            njdate += 0.5;
                                        }
                                        if (Session["attdaywisecla"].ToString() == "1")
                                        {
                                            if (nohrsprsentperday < minpresday)
                                            {
                                                Present = Present - noofdaypresen;
                                                Absent = Absent + noofdaypresen;
                                            }
                                        }
                                        nohrsprsentperday = 0;
                                        noofdaypresen = 0;
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
                            i = mmyycount + 1;
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
                nohrsprsentperday = 0;
                noofdaypresen = 0;
            }
            int diff_Date = per_from_date.Day - dumm_from_date.Day;
        }
        per_njdate = njdate;
        pre_present_date = Present - njdate;
        per_per_hrs = tot_per_hrs;
        per_absent_date = Absent;
        pre_ondu_date = Onduty;
        pre_leave_date = Leave;
        per_workingdays = workingdays - per_njdate;
        per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //dum_unmark hided on 08.08.12 // ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));
        per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili
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
    }
    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "DAY-WISE STUDENT'S ATTENDANCE@Date : " + txtfromdate.Text + " to " + txttodate.Text;
            string pagename = "day_Wise_Absentees_sms.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                da.printexcelreport(FpSpread1, reportname);
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
    protected void chklscolumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    protected void btnmsg_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            deptflag = false;

            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                for (int stcol = 1; stcol < 4; stcol++)
                {
                    int colval = stcol * 21 - 21;
                    int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 19 + colval].Value);
                    if (isval == 1)
                    {
                        deptflag = true;
                    }
                }
            }
            if (deptflag == false)
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Student and then Proceed";
                return;
            }

            string strsmstext = txtmsg.Text.ToString();
            if (strsmstext.Trim() != "")
            {

                if (FpSpread1.Sheets[0].Columns[14].Visible == false && FpSpread1.Sheets[0].Columns[15].Visible == false && FpSpread1.Sheets[0].Columns[16].Visible == false && FpSpread1.Sheets[0].Columns[17].Visible == false)
                {
                    lbl_err.Visible = true;
                    lbl_err.Text = "Please Select Mobile No Columns and then proceed";
                    return;
                }

                string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + collegecode + "'";
                ds1 = da.select_method_wo_parameter(strsenderquery, "Text");
                string user_id = "";
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
                }
                string send_mail = "", send_pw = "";
                string strquery = "select massemail,masspwd from collinfo where college_code = " + collegecode.ToString() + " ";
                ds1.Dispose();
                ds1.Reset();
                ds1 = da.select_method(strquery, hat, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    send_mail = Convert.ToString(ds1.Tables[0].Rows[0]["massemail"]);
                    send_pw = Convert.ToString(ds1.Tables[0].Rows[0]["masspwd"]);
                }

                for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
                {
                    for (int stcol = 1; stcol < 4; stcol++)
                    {
                        int colval = stcol * 21 - 21;
                        int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 19 + colval].Value);
                        if (isval == 1 && FpSpread1.Sheets[0].Cells[r, 2 + colval].Text.ToString().Trim() != "")
                        {
                            string roll = FpSpread1.Sheets[0].Cells[r, 2 + colval].Text.ToString();
                            string reg = FpSpread1.Sheets[0].Cells[r, 3 + colval].Text.ToString();
                            string name = FpSpread1.Sheets[0].Cells[r, 4 + colval].Text.ToString();

                            string fdate = txtfromdate.Text.ToString();
                            string tdate = txttodate.Text.ToString();

                            string batch = FpSpread1.Sheets[0].Cells[r, 1 + colval].Tag.ToString();
                            string degree = FpSpread1.Sheets[0].Cells[r, 2 + colval].Tag.ToString();
                            string dept = FpSpread1.Sheets[0].Cells[r, 3 + colval].Tag.ToString();
                            string sem = FpSpread1.Sheets[0].Cells[r, 4 + colval].Tag.ToString();
                            string section = FpSpread1.Sheets[0].Cells[r, 5 + colval].Tag.ToString();

                            string CDAY = FpSpread1.Sheets[0].Cells[r, 6 + colval].Text.ToString();
                            string PDAY = FpSpread1.Sheets[0].Cells[r, 7 + colval].Text.ToString();
                            string ADAY = FpSpread1.Sheets[0].Cells[r, 8 + colval].Text.ToString();
                            string DPER = FpSpread1.Sheets[0].Cells[r, 9 + colval].Text.ToString();
                            string CHOUR = FpSpread1.Sheets[0].Cells[r, 10 + colval].Text.ToString();
                            string PHOUR = FpSpread1.Sheets[0].Cells[r, 11 + colval].Text.ToString();
                            string AHOUR = FpSpread1.Sheets[0].Cells[r, 12 + colval].Text.ToString();
                            string HPER = FpSpread1.Sheets[0].Cells[r, 13 + colval].Text.ToString();
                            string fineamount = FpSpread1.Sheets[0].Cells[r, 18 + colval].Text.ToString();

                            string mobileno = "";

                            if (FpSpread1.Sheets[0].Columns[14 + colval].Visible == true)
                            {
                                mobileno = FpSpread1.Sheets[0].Cells[r, 14 + colval].Text.ToString();
                            }
                            if (FpSpread1.Sheets[0].Columns[16 + colval].Visible == true)
                            {
                                if (mobileno.Trim() != "")
                                {
                                    mobileno = mobileno + "," + FpSpread1.Sheets[0].Cells[r, 16 + colval].Text.ToString();
                                }
                                else
                                {
                                    mobileno = FpSpread1.Sheets[0].Cells[r, 16 + colval].Text.ToString();
                                }
                            }
                            if (FpSpread1.Sheets[0].Columns[17 + colval].Visible == true)
                            {
                                if (mobileno.Trim() != "")
                                {
                                    mobileno = mobileno + "," + FpSpread1.Sheets[0].Cells[r, 17 + colval].Text.ToString();
                                }
                                else
                                {
                                    mobileno = FpSpread1.Sheets[0].Cells[r, 17 + colval].Text.ToString();
                                }
                            }
                            string strbval = strsmstext;
                            strbval = strbval.ToUpper().Replace("$ROLLNO$", roll);
                            strbval = strbval.ToUpper().Replace("$REGNO$", reg);
                            strbval = strbval.ToUpper().Replace("$NAME$", name);
                            strbval = strbval.ToUpper().Replace("$BATCH$", batch);
                            strbval = strbval.ToUpper().Replace("$DEGREE$", degree);
                            strbval = strbval.ToUpper().Replace("$DEPT$", dept);
                            strbval = strbval.ToUpper().Replace("$SEM$", sem);
                            strbval = strbval.ToUpper().Replace("$SEC$", section);
                            strbval = strbval.ToUpper().Replace("$FDATE$", fdate);
                            strbval = strbval.ToUpper().Replace("$TDATE$", tdate);
                            strbval = strbval.ToUpper().Replace("$CDAY$", CDAY);
                            strbval = strbval.ToUpper().Replace("$PDAY$", PDAY);
                            strbval = strbval.ToUpper().Replace("$ADAY$", ADAY);
                            strbval = strbval.ToUpper().Replace("$DAYPER$", DPER);
                            strbval = strbval.ToUpper().Replace("$PHOUR$", PHOUR);
                            strbval = strbval.ToUpper().Replace("$CHOUR$", CHOUR);
                            strbval = strbval.ToUpper().Replace("$AHOUR$", AHOUR);
                            strbval = strbval.ToUpper().Replace("$HOURPER$", HPER);
                            strbval = strbval.ToUpper().Replace("$FINE$", fineamount);
                            if (mobileno.Trim() != "")
                            {
                                int sms = da.send_sms(user_id, collegecode, usercode, mobileno, strbval, "0");
                            }


                            if (FpSpread1.Sheets[0].Columns[15 + colval].Visible == true)
                            {
                                string strstuname = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 15 + colval].Text);
                                if (strstuname != "")
                                {
                                    SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                    MailMessage mailmsg = new MailMessage();
                                    MailAddress mfrom = new MailAddress(send_mail);
                                    mailmsg.From = mfrom;
                                    mailmsg.To.Add(strstuname);
                                    mailmsg.Subject = "Absentees Report";
                                    mailmsg.IsBodyHtml = true;
                                    mailmsg.Body = strstuname;
                                    mailmsg.Body = mailmsg.Body + strbval;
                                    mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                                    Mail.EnableSsl = true;
                                    NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                    Mail.UseDefaultCredentials = false;
                                    Mail.Credentials = credentials;
                                    Mail.Send(mailmsg);
                                }
                            }
                        }
                    }
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Message Sent Successfully')", true);
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Enter The Message and then proceed";
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void rb1_checked(object sender, EventArgs e)
    {
        if (rb1.Checked)
        {
            txtfromrange.Enabled = true;
            TextBox1.Enabled = false;
            TextBox2.Enabled = false;
        }
        else
        {
            txtfromrange.Enabled = false;
            TextBox1.Enabled = true;
            TextBox2.Enabled = true;
        }
    }
    protected void rb2_checked(object sender, EventArgs e)
    {
        if (rb1.Checked)
        {
            txtfromrange.Enabled = true;
            TextBox1.Enabled = false;
            TextBox2.Enabled = false;
        }
        else
        {
            txtfromrange.Enabled = false;
            TextBox1.Enabled = true;
            TextBox2.Enabled = true;
        }
    }
}
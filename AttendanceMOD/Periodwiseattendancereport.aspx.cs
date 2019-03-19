using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.IO;
using FarPoint.Web.Spread;

public partial class Periodwiseattendancereport : System.Web.UI.Page
{
    string grouporusercode = "";
    string collegecode = "", singleuser = "", group_user = "", usercode = "";
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataTable data = new DataTable();

    string includediscon = "";
    string includedebar = "";
    string includedisco = "";
    string includedeba = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        errmsg.Visible = false;

        if (!IsPostBack)
        {
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            string set = "select * from Master_Settings where settings in('Admission No','RollNo','RegisterNo','Student_Type') " + grouporusercode + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(set, "text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int u = 0; u < ds.Tables[0].Rows.Count; u++)
                {
                    if (ds.Tables[0].Rows[u]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[u]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[u]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[u]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[u]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[u]["value"].ToString() == "1")
                    {
                        Session["AdmissionNo"] = "1";
                    }
                    if (ds.Tables[0].Rows[u]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[u]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
            }

            clear(); setLabelText();
            chkalldept.Text = "Include All " + lbldeg.Text;
            txtFromDate.Attributes.Add("readonly", "readonly");
            BindBatch();
            BindDegree();
            BindBranchMultiple();
            BindSectionDetailmult();
            loadhour();
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            for (int c = 0; c < cblsearch.Items.Count; c++)
            {
                cblsearch.Items[c].Selected = true;
            }

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
        lbl.Add(lbldeg);
        lbl.Add(lblbranch);
        //lbl.Add(lbl_semT);
        //fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        //fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    public void clear()
    {
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        btnprint.Visible = false;
        btnPrint1.Visible = false;
        //Printcontrol.Visible = false;
        NEWPrintMater1.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        txtexcelname.Text = "";
    }

    public void BindBatch()
    {
        try
        {
            int countbatch = 0;
            chk_batch.Checked = false;
            txt_batch.Text = "---Select---";
            chklst_batch.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_batch.DataSource = ds;
                chklst_batch.DataTextField = "Batch_year";
                chklst_batch.DataValueField = "Batch_year";
                chklst_batch.DataBind();

                for (int i = 0; i < chklst_batch.Items.Count; i++)
                {
                    chklst_batch.Items[i].Selected = true;
                    countbatch += 1;
                }
                if (countbatch > 0)
                {
                    txt_batch.Text = "Batch(" + (chklst_batch.Items.Count) + ")";
                    if (chklst_batch.Items.Count == countbatch)
                    {
                        chk_batch.Checked = true;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void BindDegree()
    {
        try
        {
            int countdeg = 0;
            chk_degree.Checked = false;
            txt_degree.Text = "---Select---";
            chklst_degree.Items.Clear();
            if (chklst_batch.Items.Count > 0)
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                ds.Dispose();
                ds.Reset();
                ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_degree.DataSource = ds;
                    chklst_degree.DataTextField = "course_name";
                    chklst_degree.DataValueField = "course_id";
                    chklst_degree.DataBind();

                    for (int i = 0; i < chklst_degree.Items.Count; i++)
                    {
                        chklst_degree.Items[i].Selected = true;
                        countdeg += 1;
                    }
                    if (countdeg > 0)
                    {
                        txt_degree.Text = lbldeg.Text + "(" + (chklst_degree.Items.Count) + ")";
                        if (chklst_degree.Items.Count == countdeg)
                        {
                            chk_degree.Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void BindBranchMultiple()
    {
        try
        {
            chk_branch.Checked = false;
            txt_branch.Text = "---Select---";
            chklst_branch.Items.Clear();
            string course_id = "";
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
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            int countbranch = 0;
            ds.Dispose();
            ds.Reset();
            if (course_id.Trim() != "")
            {
                ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_branch.DataSource = ds;
                    chklst_branch.DataTextField = "dept_name";
                    chklst_branch.DataValueField = "degree_code";
                    chklst_branch.DataBind();

                    for (int i = 0; i < chklst_branch.Items.Count; i++)
                    {
                        chklst_branch.Items[i].Selected = true;
                        countbranch += 1;
                    }
                    if (countbranch > 0)
                    {
                        txt_branch.Text = lblbranch.Text + "(" + (chklst_branch.Items.Count) + ")";
                        if (chklst_branch.Items.Count == countbranch)
                        {
                            chk_branch.Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void BindSectionDetailmult()
    {
        try
        {
            includediscon = " and delflag=0";
            includedebar = " and exam_flag <> 'DEBAR'";



            string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount'" + grouporusercode + "");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includediscon = string.Empty;

            }
            getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Debar'" + grouporusercode + "");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includedebar = string.Empty;

            }

            string strbatch = "", strbranch = "";
            int takecount = 0;
            chklstsection.Items.Clear();
            txtsection.Text = "---Select---";
            txtsection.Enabled = false;
            ds.Dispose();
            ds.Reset();
            txtsection.Text = "---Select---";
            for (int i = 0; i < chklst_batch.Items.Count; i++)
            {
                if (chklst_batch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklst_batch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklst_batch.Items[i].Value.ToString() + "'";
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
                string strsection = "select distinct sections from registration where batch_year in(" + strbatch + ") and degree_code in(" + strbranch + ") and sections<>'-1' and sections<>' ' " + includediscon + includedebar + " order by sections";
                ds = d2.select_method_wo_parameter(strsection, "text");
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
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void loadhour()
    {
        try
        {


            includedisco = " and r.delflag=0";
            includedeba = " and r.exam_flag <> 'DEBAR'";



            string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount'" + grouporusercode + "");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includedisco = "";
            }
            getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Debar'" + grouporusercode + "");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {

                includedeba = string.Empty;
            }


            ddlhour.Items.Clear();
            string strbranch = "";
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

            if (strbranch != "")
            {
                string noofperiods = d2.GetFunction("select max(p.No_of_hrs_per_day) as periods from Registration r,PeriodAttndSchedule p where p.degree_code=r.degree_code and p.semester=r.Current_Semester and r.cc=0 " + includedisco + includedeba + " and p.degree_code in(" + strbranch + ") ");
                if (noofperiods.Trim() != "" && noofperiods.Trim() != "0" && noofperiods != null)
                {
                    int order = 0;
                    int periods = Convert.ToInt32(noofperiods);
                    for (int p = 0; p < periods; p++)
                    {
                        order = p + 1;
                        ddlhour.Items.Insert(p, Convert.ToString(order));
                    }
                }
                else
                {
                    errmsg.Text = "Please Update Attendance Perameters";
                    errmsg.Visible = true;
                }
            }
        }

        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chk_batch_ChekedChanged(object sender, EventArgs e)
    {
        try
        {
            int chkbatchcount = 0;
            if (chk_batch.Checked == true)
            {
                chkbatchcount++;
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

            BindDegree();
            BindBranchMultiple();
            BindSectionDetailmult();
            loadhour();
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklst_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;
            chk_batch.Checked = false;
            txt_batch.Text = "---Select---";

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
            BindDegree();
            BindBranchMultiple();
            BindSectionDetailmult();
            loadhour();
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chk_degree_ChekedChanged(object sender, EventArgs e)
    {
        try
        {
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
            BindBranchMultiple();
            BindSectionDetailmult();
            loadhour();
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklst_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
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
            BindBranchMultiple();
            BindSectionDetailmult();
            loadhour();
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chk_branch_ChekedChanged(object sender, EventArgs e)
    {
        try
        {
            string strbranch = "";
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
                }//End
            }
            else
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = false;
                }
            }

            //Bind Hours
            ddlhour.Items.Clear();
            if (txt_branch.Text != "---Select---")
            {
                if (strbranch != "")
                {
                    string noofperiods = "select max(p.No_of_hrs_per_day) as periods from Registration r,PeriodAttndSchedule p where p.degree_code=r.degree_code and p.semester=r.Current_Semester and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and p.degree_code in(" + strbranch + ") ";
                    DataSet dsnoofperiods = d2.select_method_wo_parameter(noofperiods, "text");
                    if (dsnoofperiods.Tables[0].Rows.Count > 0)
                    {
                        int order = 0;
                        int periods = Convert.ToInt32(dsnoofperiods.Tables[0].Rows[0]["periods"].ToString());
                        for (int p = 0; p < periods; p++)
                        {
                            order = p + 1;
                            ddlhour.Items.Insert(p, Convert.ToString(order));
                        }
                    }
                }
            }
            BindSectionDetailmult();
            loadhour();
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklst_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
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
            BindSectionDetailmult();
            loadhour();
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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
            loadhour();
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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
            loadhour();
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void ddlhour_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string[] fda = txtFromDate.Text.ToString().Split('/');
            DateTime dtf = Convert.ToDateTime(fda[1] + '/' + fda[0] + '/' + fda[2]);
            string strf = DateTime.Now.ToString("MM/d/yyyy");
            if (dtf > Convert.ToDateTime(strf))
            {
                txtFromDate.Text = DateTime.Now.ToString("d/MM/yyyy");
                errmsg.Text = "From Date Must Be Lesser Than Or Equal To Current Date";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void cblsearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void Cbcolumn_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        if (Cbcolumn.Checked == true)
        {
            for (int c = 0; c < cblsearch.Items.Count; c++)
            {
                cblsearch.Items[c].Selected = true;
            }
        }
        else
        {
            for (int c = 0; c < cblsearch.Items.Count; c++)
            {
                cblsearch.Items[c].Selected = false;
            }
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
             if (!chkabsent.Checked ||  chkabsent.Checked)
            
            {
                btnPrint11();
                showad.Visible = false;
                includediscon = " and delflag=0";
                includedebar = " and exam_flag <> 'DEBAR'";
                includedisco = " and r.delflag=0";
                includedeba = " and r.exam_flag <> 'DEBAR'";

                gview.Visible = false;

                string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount'" + grouporusercode + "");
                if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                {
                    includediscon = string.Empty;
                    includedisco = "";
                }
                getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Debar'" + grouporusercode + "");
                if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                {
                    includedebar = string.Empty;
                    includedeba = string.Empty;
                }

                ArrayList arrColHdrNames1 = new ArrayList();

                DataRow drow;
                clear();
                Boolean setflag = false;
                for (int c = 0; c < cblsearch.Items.Count; c++)
                {
                    if (cblsearch.Items[c].Selected == true)
                    {
                        setflag = true;
                        // FpSpread1.Sheets[0].Columns[c].Visible = true;
                    }
                    else
                    {
                        //  FpSpread1.Sheets[0].Columns[c].Visible = false;
                    }
                }
                if (setflag == false)
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Columns and Then Proceed";
                    return;
                }

                string batchyear = "";
                for (int b = 0; b < chklst_batch.Items.Count; b++)
                {
                    if (chklst_batch.Items[b].Selected == true)
                    {
                        if (batchyear == "")
                        {
                            batchyear = "'" + chklst_batch.Items[b].Text + "'";
                        }
                        else
                        {
                            batchyear = batchyear + ",'" + chklst_batch.Items[b].Text + "'";
                        }
                    }
                }
                if (batchyear.Trim() != "")
                {
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Batch and Then Proceed";
                    return;
                }

                string strsecquury = "";
                string strsec = "";
                for (int itemcount = 0; itemcount < chklstsection.Items.Count; itemcount++)
                {
                    if (chklstsection.Items[itemcount].Selected == true)
                    {
                        if (chklstsection.Items[itemcount].Text.ToString() == "Empty")
                        {
                            if (strsecquury == "")
                                strsecquury = "''";
                            else
                                strsecquury = strsecquury + ",''";
                        }
                        else
                        {
                            if (strsecquury == "")
                                strsecquury = "'" + chklstsection.Items[itemcount].Value.ToString() + "'";
                            else
                                strsecquury = strsecquury + ",'" + chklstsection.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                }
                if (strsecquury.Trim() != "")
                {
                    strsec = strsecquury;
                    strsecquury = " and isnull(r.sections,'') in(" + strsecquury + ")";  //modified by Mullai
                }


                string strdegree = "";
                for (int b = 0; b < chklst_branch.Items.Count; b++)
                {
                    if (chklst_branch.Items[b].Selected == true)
                    {
                        if (strdegree == "")
                        {
                            strdegree = "'" + chklst_branch.Items[b].Value + "'";
                        }
                        else
                        {
                            strdegree = strdegree + ",'" + chklst_branch.Items[b].Value + "'";
                        }
                    }
                }//strdegree,batchyear,strsec
                if (strdegree.Trim() != "")
                {
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Degree and Branch and Then Proceed";
                    return;
                }

                if (ddlhour.Items.Count == 0)
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Period and Then Proceed";
                    return;
                }

                string fdate = txtFromDate.Text.ToString();
                string[] spd = fdate.Split('/');
                DateTime dt = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
                int monthyear = Convert.ToInt32((Convert.ToInt32(spd[2]) * 12) + Convert.ToInt32(spd[1]));
                string dayperiod = "d" + dt.Day + "d" + ddlhour.SelectedItem.ToString();
                string presentcode = "";
                string absentcode = "";
                Hashtable hat = new Hashtable();
                hat.Add("colege_code", Session["collegecode"].ToString());
                DataSet dsattval = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                if (dsattval.Tables[0].Rows.Count > 0)
                {
                    for (int a = 0; a < dsattval.Tables[0].Rows.Count; a++)
                    {
                        string attcode = dsattval.Tables[0].Rows[a]["leavecode"].ToString();
                        string attval = dsattval.Tables[0].Rows[a]["calcflag"].ToString();
                        if (attval == "0")
                        {
                            if (presentcode == "")
                            {
                                presentcode = attcode;
                            }
                            else
                            {
                                presentcode = presentcode + ',' + attcode;
                            }
                        }
                        else if (attval == "1")
                        {
                            if (absentcode == "")
                            {
                                absentcode = attcode;
                            }
                            else
                            {
                                absentcode = absentcode + ',' + attcode;
                            }
                        }
                    }
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Update Attendance Master Settings";
                    return;
                }

                int totactstr = 0;
                int totstre = 0;
                int totpresent = 0;
                int totabsent = 0;



                bool isListall = false;
                ArrayList arr_Batch_Degree_Sec = new ArrayList();
                string qrydegdet = "select Edu_Level,c.Course_Name,dt.Dept_Name,dg.Degree_Code,Duration,NoofSections from Degree dg,Department dt,Course c where c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code";
                DataSet dsDegdet = d2.select_method_wo_parameter(qrydegdet, "Text");

                string absentquery = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,r.Roll_Admit,case when a.sex=0 then 'Male' else 'Female' end as sex,r.Batch_Year,r.degree_code,r.Current_Semester,isnull(r.Sections,'') as Sections ,a.parent_name,a.parentF_Mobile,c.Course_Name,de.Dept_Name from Registration r,applyn a,attendance at,Degree d,Course c,Department de where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.App_No=a.app_no and r.Roll_No=at.roll_no and r.Batch_Year in(" + batchyear + ") and r.degree_code in(" + strdegree + ") " + strsecquury + " and at.month_year='" + monthyear + "' and at." + dayperiod + " in(" + absentcode + ") and r.cc=0 " + includedisco + includedeba + " order by r.degree_code,r.Batch_Year desc,r.Current_Semester,r.Sections,r.Roll_No";
                DataSet dsattabsent = d2.select_method_wo_parameter(absentquery, "text");

                string prenstcount = "select Count(r.Roll_No) as prenstcount,r.Batch_Year,r.degree_code,r.Current_Semester,isnull(r.Sections,'') as Sections  from Registration r,attendance at where r.Roll_No=at.roll_no and r.Batch_Year in(" + batchyear + ") and r.degree_code in(" + strdegree + ")  " + strsecquury + " and r.cc=0 " + includedisco + includedeba + " and at.month_year='" + monthyear + "' and at." + dayperiod + " in(" + presentcode + ") group by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections";
                DataSet dspresnt = d2.select_method_wo_parameter(prenstcount, "text");

                string strgetcount = "select count(r.roll_no) as strenth,r.Batch_Year,r.degree_code,r.Current_Semester,isnull(r.Sections,'') as Sections  from Registration r where  r.Batch_Year in(" + batchyear + ") and r.degree_code in(" + strdegree + ") " + strsecquury + " and r.cc=0 " + includedisco + includedeba + " group by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections";
                DataSet dsstrength = d2.select_method_wo_parameter(strgetcount, "text");
                if (dsattabsent.Tables[0].Rows.Count > 0)
                {

                    data.Columns.Add("S.No", typeof(string));
                    data.Columns.Add("Degree Details", typeof(string));
                    data.Columns.Add("Admission No", typeof(string));
                    data.Columns.Add("Roll No", typeof(string));
                    data.Columns.Add("Register No", typeof(string));
                    data.Columns.Add("Student Name", typeof(string));
                    data.Columns.Add("Student Type", typeof(string));
                    data.Columns.Add("Gender", typeof(string));
                    data.Columns.Add("Father Name", typeof(string));
                    data.Columns.Add("Mobile Number", typeof(string));
                    data.Columns.Add("Date", typeof(string));
                    data.Columns.Add("Actual Class Strength", typeof(string));
                    data.Columns.Add("Total Strength", typeof(string));
                    data.Columns.Add("No of Student Present", typeof(string));
                    data.Columns.Add("No of Student Absent", typeof(string));
                    data.Columns.Add("Remarks", typeof(string));



                    arrColHdrNames1.Add("S.No");
                    arrColHdrNames1.Add("Degree Details");
                    arrColHdrNames1.Add("Admission No");
                    arrColHdrNames1.Add("Roll No");
                    arrColHdrNames1.Add("Register No");
                    arrColHdrNames1.Add("Student Name");
                    arrColHdrNames1.Add("Student Type");
                    arrColHdrNames1.Add("Gender");
                    arrColHdrNames1.Add("Father Name");
                    arrColHdrNames1.Add("Mobile Number");
                    arrColHdrNames1.Add("Date");
                    arrColHdrNames1.Add("Actual Class Strength");
                    arrColHdrNames1.Add("Total Strength");
                    arrColHdrNames1.Add("No of Student Present");
                    arrColHdrNames1.Add("No of Student Absent");
                    arrColHdrNames1.Add("Remarks");

                    DataRow drHdr1 = data.NewRow();
                    for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                        drHdr1[grCol] = arrColHdrNames1[grCol];
                    data.Rows.Add(drHdr1);

                    btnprint.Visible = true;
                    btnPrint1.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnxl.Visible = true;

                    string tempdegree = "";
                    int sno = 0;
                    int rowcount = 0;
                    for (int i = 0; i < dsattabsent.Tables[0].Rows.Count; i++)
                    {
                        string rollno = dsattabsent.Tables[0].Rows[i]["Roll_No"].ToString();
                        string regno = dsattabsent.Tables[0].Rows[i]["Reg_No"].ToString();
                        string studname = dsattabsent.Tables[0].Rows[i]["Stud_Name"].ToString();
                        string studtype = dsattabsent.Tables[0].Rows[i]["Stud_Type"].ToString();
                        string sex = dsattabsent.Tables[0].Rows[i]["sex"].ToString();
                        string admissionno = dsattabsent.Tables[0].Rows[i]["Roll_Admit"].ToString();
                        string batch = dsattabsent.Tables[0].Rows[i]["Batch_Year"].ToString();
                        string degerecode = dsattabsent.Tables[0].Rows[i]["degree_code"].ToString();
                        string degree = dsattabsent.Tables[0].Rows[i]["Course_Name"].ToString();
                        string branch = dsattabsent.Tables[0].Rows[i]["Dept_Name"].ToString();
                        string semesetr = dsattabsent.Tables[0].Rows[i]["Current_Semester"].ToString();
                        string section = dsattabsent.Tables[0].Rows[i]["Sections"].ToString();
                        string fathername = dsattabsent.Tables[0].Rows[i]["parent_name"].ToString();
                        string fathermobile = dsattabsent.Tables[0].Rows[i]["parentF_Mobile"].ToString();


                        string degreedetails = batch + " - " + degree + " - " + branch + " - " + semesetr;
                        string arr_bat_deg = batch + " - " + degerecode + " - " + semesetr;
                        if (section.Trim() != "" && section.Trim() != "-1" && section.Trim() != null)
                        {
                            degreedetails = degreedetails + " - " + section;
                            arr_bat_deg += " - " + section;
                        }
                        if (!arr_Batch_Degree_Sec.Contains(arr_bat_deg))
                        {
                            arr_Batch_Degree_Sec.Add(arr_bat_deg);
                        }

                        if (chkconso.Checked == true)
                        {
                            if (tempdegree != degreedetails)
                            {
                                tempdegree = degreedetails;
                                sno++;
                                drow = data.NewRow();
                                drow["S.No"] = sno.ToString();
                                drow["Degree Details"] = degreedetails.ToString();
                                drow["Admission No"] = admissionno.ToString();
                                drow["Roll No"] = rollno.ToString();
                                drow["Register No"] = regno.ToString();
                                drow["Student Name"] = studname.ToString();
                                drow["Student Type"] = studtype.ToString();
                                drow["Gender"] = sex.ToString();
                                drow["Father Name"] = fathername.ToString();
                                drow["Mobile Number"] = fathermobile.ToString();
                                drow["Date"] = txtFromDate.Text.ToString();

                                string pdegree = dsattabsent.Tables[0].Rows[i]["degree_code"].ToString();
                                string pbatch = dsattabsent.Tables[0].Rows[i]["Batch_Year"].ToString();
                                string psem = dsattabsent.Tables[0].Rows[i]["Current_Semester"].ToString();

                                string getsectio = dsattabsent.Tables[0].Rows[i]["Sections"].ToString();
                                if (getsectio.Trim() != "" && getsectio.Trim() != "-1" && getsectio.Trim() != null)
                                {
                                    getsectio = " and sections='" + getsectio + "'";
                                }

                                string presentcount = "0";

                                dspresnt.Tables[0].DefaultView.RowFilter = "Batch_year='" + pbatch + "' and degree_code='" + pdegree + "' and Current_Semester='" + psem + "' " + getsectio + "";
                                DataView dvpres = dspresnt.Tables[0].DefaultView;
                                if (dvpres.Count > 0)
                                {
                                    int cnt = 0;
                                    for (int t = 0; t < dvpres.Count; t++)
                                    {
                                        cnt = cnt + Convert.ToInt32(dvpres[t]["prenstcount"]);

                                    }
                                    presentcount = Convert.ToString(cnt);
                                }

                                string strenth = "0";
                                dsstrength.Tables[0].DefaultView.RowFilter = "Batch_year='" + pbatch + "' and degree_code='" + pdegree + "' and Current_Semester='" + psem + "' " + getsectio + "";
                                DataView dvstreth = dsstrength.Tables[0].DefaultView;
                                if (dvstreth.Count > 0)
                                {
                                    int cnt = 0;
                                    for (int t = 0; t < dvstreth.Count; t++)
                                    {
                                        cnt = cnt + Convert.ToInt32(dvstreth[t]["strenth"]);

                                    }
                                    strenth = cnt.ToString();
                                }

                                dsattabsent.Tables[0].DefaultView.RowFilter = "Batch_year='" + pbatch + "' and degree_code='" + pdegree + "' and Current_Semester='" + psem + "' " + getsectio + "";
                                DataView dvabse = dsattabsent.Tables[0].DefaultView;


                                rowcount = dvabse.Count;


                                int totalatt = Convert.ToInt32(presentcount) + rowcount;

                                totactstr = totactstr + Convert.ToInt32(strenth);
                                totstre = totstre + Convert.ToInt32(totalatt);
                                totpresent = totpresent + Convert.ToInt32(presentcount);
                                totabsent = totabsent + rowcount;

                                drow["Actual Class Strength"] = strenth.ToString();
                                drow["Total Strength"] = totalatt.ToString();
                                drow["No of Student Present"] = presentcount.ToString();
                                drow["No of Student Absent"] = rowcount.ToString();
                                drow["Remarks"] = "";
                                data.Rows.Add(drow);

                            }
                            else
                            {
                                string d = data.Rows[data.Rows.Count - 1][5].ToString();
                                data.Rows[data.Rows.Count - 1][2] = data.Rows[data.Rows.Count - 1][2] + ", " + admissionno.ToString();
                                data.Rows[data.Rows.Count - 1][3] = data.Rows[data.Rows.Count - 1][3] + ", " + rollno.ToString();
                                data.Rows[data.Rows.Count - 1][4] = data.Rows[data.Rows.Count - 1][4] + ", " + regno.ToString();
                                data.Rows[data.Rows.Count - 1][5] = data.Rows[data.Rows.Count - 1][5] + ", " + studname.ToString();
                                data.Rows[data.Rows.Count - 1][6] = data.Rows[data.Rows.Count - 1][6] + ", " + studtype.ToString();
                                data.Rows[data.Rows.Count - 1][7] = data.Rows[data.Rows.Count - 1][7] + ", " + sex.ToString();

                                data.Rows[data.Rows.Count - 1][8] = data.Rows[data.Rows.Count - 1][8] + ", " + fathername.ToString();
                                data.Rows[data.Rows.Count - 1][9] = data.Rows[data.Rows.Count - 1][9] + ", " + fathermobile.ToString();

                            }
                        }
                        else
                        {
                            if (tempdegree != degreedetails)
                            {
                                if (tempdegree != "")
                                {
                                    string pdegree = dsattabsent.Tables[0].Rows[i - 1]["degree_code"].ToString();
                                    string pbatch = dsattabsent.Tables[0].Rows[i - 1]["Batch_Year"].ToString();
                                    string psem = dsattabsent.Tables[0].Rows[i - 1]["Current_Semester"].ToString();

                                    string getsectio = dsattabsent.Tables[0].Rows[i - 1]["Sections"].ToString();
                                    if (getsectio.Trim() != "" && getsectio.Trim() != "-1" && getsectio.Trim() != null)
                                    {
                                        getsectio = " and sections='" + getsectio + "'";
                                    }

                                    string presentcount = "0";
                                    dspresnt.Tables[0].DefaultView.RowFilter = "Batch_year='" + pbatch + "' and degree_code='" + pdegree + "' and Current_Semester='" + psem + "' " + getsectio + "";
                                    DataView dvpres = dspresnt.Tables[0].DefaultView;
                                    if (dvpres.Count > 0)
                                    {
                                        int cnt = 0;
                                        for (int t = 0; t < dvpres.Count; t++)
                                        {
                                            cnt = cnt + Convert.ToInt32(dvpres[t]["prenstcount"]);

                                        }
                                        presentcount = cnt.ToString();
                                    }

                                    string strenth = "0";
                                    dsstrength.Tables[0].DefaultView.RowFilter = "Batch_year='" + pbatch + "' and degree_code='" + pdegree + "' and Current_Semester='" + psem + "' " + getsectio + "";
                                    DataView dvstreth = dsstrength.Tables[0].DefaultView;
                                    if (dvstreth.Count > 0)
                                    {
                                        int cnt = 0;
                                        for (int t = 0; t < dvstreth.Count; t++)
                                        {
                                            cnt = cnt + Convert.ToInt32(dvstreth[t]["strenth"]);

                                        }
                                        strenth = cnt.ToString();

                                    }

                                    int totalatt = Convert.ToInt32(presentcount) + rowcount;

                                    totactstr = totactstr + Convert.ToInt32(strenth);
                                    totstre = totstre + Convert.ToInt32(totalatt);
                                    totpresent = totpresent + Convert.ToInt32(presentcount);
                                    totabsent = totabsent + rowcount;

                                    for (int setrow = 0; setrow < rowcount; setrow++)
                                    {
                                        data.Rows[data.Rows.Count - (rowcount - setrow)][12] = totalatt.ToString();
                                        data.Rows[data.Rows.Count - (rowcount - setrow)][14] = rowcount.ToString();
                                        data.Rows[data.Rows.Count - (rowcount - setrow)][11] = strenth.ToString();
                                        data.Rows[data.Rows.Count - (rowcount - setrow)][13] = presentcount.ToString();

                                    }

                                    rowcount = 0;
                                }
                                tempdegree = degreedetails;
                            }
                            rowcount++;

                            sno++;

                            drow = data.NewRow();
                            drow["S.No"] = sno.ToString();
                            drow["Degree Details"] = degreedetails.ToString();
                            drow["Admission No"] = admissionno.ToString();
                            drow["Roll No"] = rollno.ToString();
                            drow["Register No"] = regno.ToString();
                            drow["Student Name"] = studname.ToString();
                            drow["Student Type"] = studtype.ToString();
                            drow["Gender"] = sex.ToString();
                            drow["Father Name"] = fathername.ToString();
                            drow["Mobile Number"] = fathermobile.ToString();
                            drow["Date"] = txtFromDate.Text.ToString();
                            //drow["Actual Class Strength"] = sno.ToString();
                            //drow["Total Strength"] = sno.ToString();
                            //drow["No of Student Present"] = sno.ToString();
                            //drow["No of Student Absent"] = sno.ToString();
                            drow["Remarks"] = "";
                            data.Rows.Add(drow);

                            if (i == dsattabsent.Tables[0].Rows.Count - 1)
                            {
                                string pdegree = dsattabsent.Tables[0].Rows[i]["degree_code"].ToString();
                                string pbatch = dsattabsent.Tables[0].Rows[i]["Batch_Year"].ToString();
                                string psem = dsattabsent.Tables[0].Rows[i]["Current_Semester"].ToString();

                                string getsectio = dsattabsent.Tables[0].Rows[i]["Sections"].ToString();
                                if (getsectio.Trim() != "" && getsectio.Trim() != "-1" && getsectio.Trim() != null)
                                {
                                    getsectio = " and sections='" + getsectio + "'";
                                }

                                string presentcount = "0";
                                dspresnt.Tables[0].DefaultView.RowFilter = "Batch_year='" + pbatch + "' and degree_code='" + pdegree + "' and Current_Semester='" + psem + "' " + getsectio + "";
                                DataView dvpres = dspresnt.Tables[0].DefaultView;
                                if (dvpres.Count > 0)
                                {
                                    int cnt = 0;
                                    for (int t = 0; t < dvpres.Count; t++)
                                    {
                                        cnt = cnt + Convert.ToInt32(dvpres[t]["prenstcount"]);

                                    }
                                    presentcount = cnt.ToString();
                                }

                                string strenth = "0";
                                dsstrength.Tables[0].DefaultView.RowFilter = "Batch_year='" + pbatch + "' and degree_code='" + pdegree + "' and Current_Semester='" + psem + "' " + getsectio + "";
                                DataView dvstreth = dsstrength.Tables[0].DefaultView;
                                if (dvstreth.Count > 0)
                                {
                                    int cnt = 0;
                                    for (int t = 0; t < dvstreth.Count; t++)
                                    {
                                        cnt = cnt + Convert.ToInt32(dvstreth[t]["strenth"]);

                                    }
                                    strenth = cnt.ToString();

                                }

                                int totalatt = Convert.ToInt32(presentcount) + rowcount;
                                totactstr = totactstr + Convert.ToInt32(strenth);
                                totstre = totstre + Convert.ToInt32(totalatt);
                                totpresent = totpresent + Convert.ToInt32(presentcount);
                                totabsent = totabsent + rowcount;

                                for (int setrow = 0; setrow < rowcount; setrow++)
                                {
                                    data.Rows[data.Rows.Count - (rowcount - setrow)][12] = totalatt.ToString();
                                    data.Rows[data.Rows.Count - (rowcount - setrow)][14] = rowcount.ToString();
                                    data.Rows[data.Rows.Count - (rowcount - setrow)][11] = strenth.ToString();
                                    data.Rows[data.Rows.Count - (rowcount - setrow)][13] = presentcount.ToString();


                                }

                            }
                        }
                    }
                    if (chkalldept.Checked == true)
                    {
                        // strdegree,batchyear,strsec
                        isListall = true;
                        string[] selbatch = batchyear.Split(',');
                        string[] seldegree = strdegree.Split(',');
                        string[] selsec = strsec.Split(',');
                        string deg_det = "";
                        string degreedetails = "";
                        if (selbatch.Length > 0)
                        {
                            for (int bt = 0; bt < selbatch.Length; bt++)
                            {
                                string newbatch = selbatch[bt].ToString();
                                deg_det = selbatch[bt].ToString().Replace("'", "");
                                if (seldegree.Length > 0)
                                {
                                    for (int deg = 0; deg < seldegree.Length; deg++)
                                    {
                                        string newdegree = seldegree[deg].ToString();
                                        deg_det += " - " + seldegree[deg].ToString().Replace("'", "");
                                        DataView dvdeg = new DataView();
                                        if (dsDegdet.Tables[0].Rows.Count > 0)
                                        {
                                            if (newdegree != "")
                                            {
                                                dsDegdet.Tables[0].DefaultView.RowFilter = "Degree_Code=" + newdegree;
                                                dvdeg = dsDegdet.Tables[0].DefaultView;
                                            }
                                            string selsem = d2.GetFunctionv("select distinct Current_Semester from Registration where Batch_Year=" + newbatch + " and degree_code=" + newdegree + " and cc=0 " + includediscon + includedebar + "");
                                            DataSet dsSec = new DataSet();
                                            dsSec = d2.select_method_wo_parameter("select distinct Sections from Registration where Batch_Year=" + newbatch + " and degree_code=" + newdegree + " and cc=0 " + includediscon + includedebar + "", "Text");
                                            if (selsem != "" && selsem != null)
                                            {
                                                deg_det += " - " + selsem;
                                            }
                                            if (selsec.Length > 0)
                                            {
                                                for (int se = 0; se < selsec.Length; se++)
                                                {
                                                    rowcount = 0;
                                                    string newsec = selsec[se].ToString();
                                                    deg_det += " - " + selsec[se].ToString().Replace("'", "");
                                                    DataView dvSec = new DataView();
                                                    //if (dsSec.Tables[0].Rows.Count > 0)
                                                    //{
                                                    //    dsSec.Tables[0].DefaultView.RowFilter = "Sections=" + newsec;
                                                    //    dvSec = dsSec.Tables[0].DefaultView;
                                                    //}
                                                    if (newsec != "")
                                                    {
                                                        if (dsSec.Tables[0].Rows.Count > 0)
                                                        {
                                                            dsSec.Tables[0].DefaultView.RowFilter = "Sections=" + newsec;
                                                            dvSec = dsSec.Tables[0].DefaultView;
                                                        }
                                                    }
                                                    if (dvSec.Count > 0)
                                                    {
                                                        newsec = selsec[se].ToString();
                                                    }
                                                    else
                                                    {
                                                        continue;
                                                    }
                                                    //if (dvSec.Count > 0)
                                                    //{
                                                    if (dvdeg.Count > 0)
                                                    {
                                                        string deg_name = Convert.ToString(dvdeg[0]["Course_Name"]);
                                                        string branch_name = Convert.ToString(dvdeg[0]["Dept_Name"]);
                                                        if (newsec.Replace("'", "") != "")
                                                        {
                                                            degreedetails = Convert.ToString(newbatch.Replace("'", "")) + " - " + Convert.ToString(deg_name.Replace("'", "")) + " - " + Convert.ToString(branch_name.Replace("'", "")) + " - " + Convert.ToString(selsem.Replace("'", "")) + " - " + Convert.ToString(newsec.Replace("'", ""));
                                                            deg_det = Convert.ToString(newbatch.Replace("'", "")) + " - " + Convert.ToString(newdegree.Replace("'", "")) + " - " + Convert.ToString(selsem.Replace("'", "")) + " - " + Convert.ToString(newsec.Replace("'", ""));
                                                        }
                                                        else
                                                        {
                                                            degreedetails = Convert.ToString(newbatch.Replace("'", "")) + " - " + Convert.ToString(deg_name.Replace("'", "")) + " - " + Convert.ToString(branch_name.Replace("'", "")) + " - " + Convert.ToString(selsem.Replace("'", ""));
                                                            deg_det = Convert.ToString(newbatch.Replace("'", "")) + " - " + Convert.ToString(newdegree.Replace("'", "")) + " - " + Convert.ToString(selsem.Replace("'", ""));
                                                        }

                                                        //degreedetails = batch + " - " + degree + " - " + branch + " - " + semesetr;
                                                        if (!arr_Batch_Degree_Sec.Contains(deg_det))
                                                        {
                                                            //FpSpread1.Sheets[0].RowCount++;
                                                            if (newsec.Trim() != "" && newsec.Trim() != "-1" && newsec.Trim() != null)
                                                            {
                                                                newsec = " and sections=" + newsec + "";
                                                            }
                                                            //else
                                                            //{

                                                            //}
                                                            string presentcount = "0";
                                                            dspresnt.Tables[0].DefaultView.RowFilter = "Batch_year=" + newbatch + " and degree_code=" + newdegree + " and Current_Semester='" + selsem + "' " + newsec + "";
                                                            DataView dvpres = dspresnt.Tables[0].DefaultView;
                                                            if (dvpres.Count > 0)
                                                            {
                                                                int cnt = 0;
                                                                for (int t = 0; t < dvpres.Count; t++)
                                                                {
                                                                    cnt = cnt + Convert.ToInt32(dvpres[t]["prenstcount"]);

                                                                }
                                                                presentcount = cnt.ToString();
                                                            }

                                                            string strenth = "0";
                                                            dsstrength.Tables[0].DefaultView.RowFilter = "Batch_year=" + newbatch + " and degree_code=" + newdegree + " and Current_Semester='" + selsem + "' " + newsec + "";
                                                            DataView dvstreth = dsstrength.Tables[0].DefaultView;
                                                            if (dvstreth.Count > 0)
                                                            {
                                                                int cnt = 0;
                                                                for (int t = 0; t < dvstreth.Count; t++)
                                                                {
                                                                    cnt = cnt + Convert.ToInt32(dvstreth[t]["strenth"]);

                                                                }
                                                                strenth = cnt.ToString();
                                                            }

                                                            int totalatt = Convert.ToInt32(presentcount) + rowcount;
                                                            totactstr = totactstr + Convert.ToInt32(strenth);
                                                            totstre = totstre + Convert.ToInt32(totalatt);
                                                            totpresent = totpresent + Convert.ToInt32(presentcount);
                                                            totabsent = totabsent + 0;
                                                            sno++;

                                                            drow = data.NewRow();
                                                            drow["S.No"] = sno.ToString();
                                                            drow["Degree Details"] = degreedetails.ToString();
                                                            drow["Admission No"] = "Nil";
                                                            drow["Roll No"] = "Nil";
                                                            drow["Register No"] = "Nil";
                                                            drow["Student Name"] = "Nil";
                                                            drow["Student Type"] = "Nil";
                                                            drow["Gender"] = "Nil";
                                                            drow["Father Name"] = "Nil";
                                                            drow["Mobile Number"] = "Nil";
                                                            drow["Date"] = txtFromDate.Text.ToString();
                                                            drow["Actual Class Strength"] = strenth.ToString();
                                                            drow["Total Strength"] = totalatt.ToString();
                                                            drow["No of Student Present"] = presentcount.ToString();
                                                            drow["No of Student Absent"] = "0";
                                                            drow["Remarks"] = "";
                                                            data.Rows.Add(drow);



                                                            arr_Batch_Degree_Sec.Add(deg_det);
                                                        }
                                                    }
                                                    //}
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    drow = data.NewRow();
                    drow["S.No"] = "Total";
                    drow["Actual Class Strength"] = totactstr.ToString();
                    drow["Total Strength"] = totstre.ToString();
                    drow["No of Student Present"] = totpresent.ToString();
                    drow["No of Student Absent"] = totabsent.ToString();
                    data.Rows.Add(drow);
                }

                if (arr_Batch_Degree_Sec.Count == 0 && chkalldept.Checked == true)
                {
                    if (chkalldept.Checked == true)
                    {
                        data.Columns.Add("S.No", typeof(string));
                        data.Columns.Add("Degree Details", typeof(string));
                        data.Columns.Add("Admission No", typeof(string));
                        data.Columns.Add("Roll No", typeof(string));
                        data.Columns.Add("Register No", typeof(string));
                        data.Columns.Add("Student Name", typeof(string));
                        data.Columns.Add("Student Type", typeof(string));
                        data.Columns.Add("Gender", typeof(string));
                        data.Columns.Add("Father Name", typeof(string));
                        data.Columns.Add("Mobile Number", typeof(string));
                        data.Columns.Add("Date", typeof(string));
                        data.Columns.Add("Actual Class Strength", typeof(string));
                        data.Columns.Add("Total Strength", typeof(string));
                        data.Columns.Add("No of Student Present", typeof(string));
                        data.Columns.Add("No of Student Absent", typeof(string));
                        data.Columns.Add("Remarks", typeof(string));



                        arrColHdrNames1.Add("S.No");
                        arrColHdrNames1.Add("Degree Details");
                        arrColHdrNames1.Add("Admission No");
                        arrColHdrNames1.Add("Roll No");
                        arrColHdrNames1.Add("Register No");
                        arrColHdrNames1.Add("Student Name");
                        arrColHdrNames1.Add("Student Type");
                        arrColHdrNames1.Add("Gender");
                        arrColHdrNames1.Add("Father Name");
                        arrColHdrNames1.Add("Mobile Number");
                        arrColHdrNames1.Add("Date");
                        arrColHdrNames1.Add("Actual Class Strength");
                        arrColHdrNames1.Add("Total Strength");
                        arrColHdrNames1.Add("No of Student Present");
                        arrColHdrNames1.Add("No of Student Absent");
                        arrColHdrNames1.Add("Remarks");

                        DataRow drHdr1 = data.NewRow();
                        for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                            drHdr1[grCol] = arrColHdrNames1[grCol];
                        data.Rows.Add(drHdr1);

                        Showgrid.Visible = true;
                        btnprint.Visible = true;
                        btnPrint1.Visible = true;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnxl.Visible = true;

                        // strdegree,batchyear,strsec
                        isListall = true;
                        string[] selbatch = batchyear.Split(',');
                        string[] seldegree = strdegree.Split(',');
                        string[] selsec = strsec.Split(',');
                        string deg_det = "";
                        string degreedetails = "";
                        int sno = 0;
                        if (selbatch.Length > 0)
                        {
                            for (int bt = 0; bt < selbatch.Length; bt++)
                            {
                                string newbatch = selbatch[bt].ToString();
                                deg_det = selbatch[bt].ToString().Replace("'", "");
                                if (seldegree.Length > 0)
                                {
                                    for (int deg = 0; deg < seldegree.Length; deg++)
                                    {
                                        string newdegree = seldegree[deg].ToString();
                                        deg_det += " - " + seldegree[deg].ToString().Replace("'", "");
                                        DataView dvdeg = new DataView();
                                        if (dsDegdet.Tables[0].Rows.Count > 0)
                                        {
                                            if (newdegree != "")
                                            {
                                                dsDegdet.Tables[0].DefaultView.RowFilter = "Degree_Code=" + newdegree;
                                                dvdeg = dsDegdet.Tables[0].DefaultView;
                                            }
                                            string selsem = d2.GetFunctionv("select distinct Current_Semester from Registration where Batch_Year=" + newbatch + " and degree_code=" + newdegree + " and cc=0 " + includediscon + includedebar + "");
                                            DataSet dsSec = new DataSet();
                                            dsSec = d2.select_method_wo_parameter("select distinct Sections from Registration where Batch_Year=" + newbatch + " and degree_code=" + newdegree + " and cc=0 " + includediscon + includedebar + "", "Text");
                                            if (selsem != "" && selsem != null)
                                            {
                                                deg_det += " - " + selsem;
                                            }
                                            if (selsec.Length > 0)
                                            {
                                                for (int se = 0; se < selsec.Length; se++)
                                                {
                                                    //rowcount = 0;
                                                    string newsec = selsec[se].ToString();
                                                    deg_det += " - " + selsec[se].ToString().Replace("'", "");
                                                    DataView dvSec = new DataView();
                                                    if (newsec != "")
                                                    {
                                                        if (dsSec.Tables[0].Rows.Count > 0)
                                                        {
                                                            dsSec.Tables[0].DefaultView.RowFilter = "Sections=" + newsec;
                                                            dvSec = dsSec.Tables[0].DefaultView;
                                                        }
                                                    }
                                                    if (dvSec.Count > 0)
                                                    {
                                                        newsec = selsec[se].ToString();
                                                    }
                                                    else
                                                    {
                                                        continue;
                                                    }

                                                    //if (dvSec.Count > 0)
                                                    //{
                                                    if (dvdeg.Count > 0)
                                                    {
                                                        string deg_name = Convert.ToString(dvdeg[0]["Course_Name"]);
                                                        string branch_name = Convert.ToString(dvdeg[0]["Dept_Name"]);
                                                        if (newsec.Trim().Replace("'", "") != "")
                                                        {
                                                            degreedetails = Convert.ToString(newbatch.Replace("'", "")) + " - " + Convert.ToString(deg_name.Replace("'", "")) + " - " + Convert.ToString(branch_name.Replace("'", "")) + " - " + Convert.ToString(selsem.Replace("'", "")) + " - " + Convert.ToString(newsec.Replace("'", ""));
                                                            deg_det = Convert.ToString(newbatch.Replace("'", "")) + " - " + Convert.ToString(newdegree.Replace("'", "")) + " - " + Convert.ToString(selsem.Replace("'", "")) + " - " + Convert.ToString(newsec.Replace("'", ""));
                                                        }
                                                        else
                                                        {
                                                            degreedetails = Convert.ToString(newbatch.Replace("'", "")) + " - " + Convert.ToString(deg_name.Replace("'", "")) + " - " + Convert.ToString(branch_name.Replace("'", "")) + " - " + Convert.ToString(selsem.Replace("'", ""));
                                                            deg_det = Convert.ToString(newbatch.Replace("'", "")) + " - " + Convert.ToString(newdegree.Replace("'", "")) + " - " + Convert.ToString(selsem.Replace("'", ""));
                                                        }

                                                        //degreedetails = batch + " - " + degree + " - " + branch + " - " + semesetr;
                                                        if (!arr_Batch_Degree_Sec.Contains(deg_det))
                                                        {

                                                            if (newsec.Trim() != "" && newsec.Trim() != "-1" && newsec.Trim() != null)
                                                            {
                                                                newsec = " and sections=" + newsec + "";
                                                            }
                                                            //else
                                                            //{

                                                            //}
                                                            string presentcount = "0";
                                                            dspresnt.Tables[0].DefaultView.RowFilter = "Batch_year=" + newbatch + " and degree_code=" + newdegree + " and Current_Semester='" + selsem + "' " + newsec + "";
                                                            DataView dvpres = dspresnt.Tables[0].DefaultView;
                                                            if (dvpres.Count > 0)
                                                            {
                                                                int cnt = 0;
                                                                for (int t = 0; t < dvpres.Count; t++)
                                                                {
                                                                    cnt = cnt + Convert.ToInt32(dvpres[t]["prenstcount"]);

                                                                }
                                                                presentcount = cnt.ToString();
                                                            }

                                                            string strenth = "0";
                                                            dsstrength.Tables[0].DefaultView.RowFilter = "Batch_year=" + newbatch + " and degree_code=" + newdegree + " and Current_Semester='" + selsem + "' " + newsec + "";
                                                            DataView dvstreth = dsstrength.Tables[0].DefaultView;
                                                            if (dvstreth.Count > 0)
                                                            {
                                                                int cnt = 0;
                                                                for (int t = 0; t < dvstreth.Count; t++)
                                                                {
                                                                    cnt = cnt + Convert.ToInt32(dvstreth[t]["strenth"]);

                                                                }
                                                                strenth = cnt.ToString();
                                                            }

                                                            int totalatt = Convert.ToInt32(presentcount);
                                                            totactstr = totactstr + Convert.ToInt32(strenth);
                                                            totstre = totstre + Convert.ToInt32(totalatt);
                                                            totpresent = totpresent + Convert.ToInt32(presentcount);
                                                            totabsent = totabsent + 0;
                                                            sno++;
                                                            drow = data.NewRow();
                                                            drow["S.No"] = sno.ToString();
                                                            drow["Degree Details"] = degreedetails.ToString();
                                                            drow["Admission No"] = "Nil";
                                                            drow["Roll No"] = "Nil";
                                                            drow["Register No"] = "Nil";
                                                            drow["Student Name"] = "Nil";
                                                            drow["Student Type"] = "Nil";
                                                            drow["Gender"] = "Nil";
                                                            drow["Father Name"] = "Nil";
                                                            drow["Mobile Number"] = "Nil";
                                                            drow["Date"] = txtFromDate.Text.ToString();



                                                            drow["Actual Class Strength"] = strenth.ToString();
                                                            drow["Total Strength"] = totalatt.ToString();
                                                            drow["No of Student Present"] = presentcount.ToString();
                                                            drow["No of Student Absent"] = "0";
                                                            drow["Remarks"] = "";

                                                            data.Rows.Add(drow);

                                                            arr_Batch_Degree_Sec.Add(deg_det);
                                                        }
                                                    }
                                                    //}
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        drow = data.NewRow();
                        drow["S.No"] = "Total";
                        drow["Actual Class Strength"] = totactstr.ToString();
                        drow["Total Strength"] = totstre.ToString();
                        drow["No of Student Present"] = totpresent.ToString();
                        drow["No of Student Absent"] = totabsent.ToString();
                        data.Rows.Add(drow);

                    }
                }
                else if (arr_Batch_Degree_Sec.Count == 0 && isListall == false && chkalldept.Checked == false)
                {
                    clear();
                    errmsg.Visible = true;
                    errmsg.Text = "No Records Found";
                }

                if (data.Columns.Count > 0 && data.Rows.Count > 0)
                {
                    Showgrid.DataSource = data;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;
                    divMainContents.Visible = true;



                    Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[0].Font.Bold = true;
                    Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;





                    int colcnt = 0;
                    for (int c = 0; c < cblsearch.Items.Count; c++)
                    {
                        if (cblsearch.Items[c].Selected == false)
                        {
                            string colname = cblsearch.Items[c].Text;

                            for (int g = 0; g < data.Columns.Count; g++)
                            {
                                string columname = data.Columns[g].ColumnName;
                                if (colname == columname)
                                {
                                    for (int r = 0; r < data.Rows.Count; r++)
                                        Showgrid.Rows[r].Cells[g].Visible = false;
                                    colcnt++;
                                }

                            }

                        }
                        else
                        {

                            string colname = cblsearch.Items[c].Text;

                            for (int g = 0; g < data.Columns.Count; g++)
                            {
                                string columname = data.Columns[g].ColumnName;
                                if (colname == columname)
                                {

                                    if (colname.ToUpper() == "ROLL NO")
                                    {

                                        if (Convert.ToString(Session["Rollflag"]) == "0" || Convert.ToString(Session["Rollflag"]) == "")
                                        {

                                            for (int r = 0; r < data.Rows.Count; r++)
                                                Showgrid.Rows[r].Cells[g].Visible = false;
                                            colcnt++;

                                        }
                                    }
                                    else if (colname.ToUpper() == "REGISTER NO")
                                    {
                                        if (Convert.ToString(Session["Regflag"]) == "0" || Convert.ToString(Session["Regflag"]) == "")
                                        {

                                            for (int r = 0; r < data.Rows.Count; r++)
                                                Showgrid.Rows[r].Cells[g].Visible = false;
                                            colcnt++;

                                        }
                                    }
                                    else if (colname.ToUpper() == "STUDENT TYPE")
                                    {
                                        if (Convert.ToString(Session["Studflag"]) == "0" || Convert.ToString(Session["Studflag"]) == "")
                                        {

                                            for (int r = 0; r < data.Rows.Count; r++)
                                                Showgrid.Rows[r].Cells[g].Visible = false;
                                            colcnt++;

                                        }
                                    }
                                    else if (colname.ToUpper() == "ADMISSION NO")
                                    {
                                        if (Convert.ToString(Session["AdmissionNo"]) == "0" || Convert.ToString(Session["AdmissionNo"]) == "")
                                        {

                                            for (int r = 0; r < data.Rows.Count; r++)
                                                Showgrid.Rows[r].Cells[g].Visible = false;
                                            colcnt++;

                                        }
                                    }
                                    else
                                    {

                                        for (int r = 0; r < data.Rows.Count; r++)
                                            Showgrid.Rows[r].Cells[g].Visible = true;
                                    }
                                }

                            }
                        }

                    }

                    int d = Convert.ToInt32(data.Columns.Count - 5);


                    //Rowspan
                    for (int rowIndex = Showgrid.Rows.Count - 3; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow row = Showgrid.Rows[rowIndex];
                        GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];

                        if (rowIndex != 0)
                        {
                            for (int i = Showgrid.Rows[rowIndex].Cells.Count - 2; i > 0; i--)
                            {
                                if (Showgrid.HeaderRow.Cells[i].Text == "Degree Details" || Showgrid.HeaderRow.Cells[i].Text == "Date" || Showgrid.HeaderRow.Cells[i].Text == "Actual Class Strength" || Showgrid.HeaderRow.Cells[i].Text == "Total Strength" || Showgrid.HeaderRow.Cells[i].Text == "No of Student Present" || Showgrid.HeaderRow.Cells[i].Text == "No of Student Absent" || Showgrid.HeaderRow.Cells[i].Text == "No of Student Remarks")
                                {
                                    if (row.Cells[1].Text == previousRow.Cells[1].Text && row.Cells[i].Text == previousRow.Cells[i].Text)
                                    {
                                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                               previousRow.Cells[i].RowSpan + 1;
                                        previousRow.Cells[i].Visible = false;
                                    }
                                }
                            }
                        }
                    }

                    Showgrid.Rows[data.Rows.Count - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    Showgrid.Rows[data.Rows.Count - 1].Cells[0].ColumnSpan = d - colcnt;
                    for (int a = 1; a < d; a++)
                        Showgrid.Rows[data.Rows.Count - 1].Cells[a].Visible = false;



                    for (int col = 0; col < data.Columns.Count; col++)
                    {
                        if (Showgrid.HeaderRow.Cells[col].Text == "Date" || Showgrid.HeaderRow.Cells[col].Text == "Actual Class Strength" || Showgrid.HeaderRow.Cells[col].Text == "Total Strength" || Showgrid.HeaderRow.Cells[col].Text == "No of Student Present" || Showgrid.HeaderRow.Cells[col].Text == "No of Student Absent" || Showgrid.HeaderRow.Cells[col].Text == "No of Student Remarks")
                        {
                            for (int row = 0; row < data.Rows.Count; row++)
                            {
                                Showgrid.Rows[row].Cells[col].HorizontalAlign = HorizontalAlign.Center;

                            }
                        }
                    }
                }
            }
            
            if (chkabsent.Checked)
            {
                btnPrint12();
             //   Showgrid.Visible = false;
                //Imagefilter.Visible = false;
                //Labelfilter.Visible = false;
                DataTable dtstud = new DataTable();
                Hashtable hstable = new Hashtable();
                DataRow drrow;
                string strsecquury = "";
                string strsec = "";

                string batchyears = "";
                for (int b = 0; b < chklst_batch.Items.Count; b++)
                {
                    if (chklst_batch.Items[b].Selected == true)
                    {
                        if (batchyears == "")
                        {
                            batchyears = "'" + chklst_batch.Items[b].Text + "'";
                        }
                        else
                        {
                            batchyears = batchyears + ",'" + chklst_batch.Items[b].Text + "'";
                        }
                    }
                }
                if (batchyears.Trim() != "")
                {
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Batch and Then Proceed";
                    return;
                }

                string strdegrees = "";
                for (int b = 0; b < chklst_branch.Items.Count; b++)
                {
                    if (chklst_branch.Items[b].Selected == true)
                    {
                        if (strdegrees == "")
                        {
                            strdegrees = "'" + chklst_branch.Items[b].Value + "'";
                        }
                        else
                        {
                            strdegrees = strdegrees + ",'" + chklst_branch.Items[b].Value + "'";
                        }
                    }
                }//strdegree,batchyear,strsec
                if (strdegrees.Trim() != "")
                {
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Degree and Branch and Then Proceed";
                    return;
                }
                string presentcode = "";
                string absentcode = "";
                Hashtable hat = new Hashtable();
                hat.Add("colege_code", Session["collegecode"].ToString());
                DataSet dsattval = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                if (dsattval.Tables[0].Rows.Count > 0)
                {
                    for (int a = 0; a < dsattval.Tables[0].Rows.Count; a++)
                    {
                        string attcode = dsattval.Tables[0].Rows[a]["leavecode"].ToString();
                        string attval = dsattval.Tables[0].Rows[a]["calcflag"].ToString();
                        if (attval == "0")
                        {
                            if (presentcode == "")
                            {
                                presentcode = attcode;
                            }
                            else
                            {
                                presentcode = presentcode + ',' + attcode;
                            }
                        }
                        else if (attval == "1")
                        {
                            if (absentcode == "")
                            {
                                absentcode = attcode;
                            }
                            else
                            {
                                absentcode = absentcode + ',' + attcode;
                            }
                        }
                    }
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Update Attendance Master Settings";
                    return;
                }

               
                string fdate = txtFromDate.Text.ToString();
                string[] spd = fdate.Split('/');
                DateTime dt = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);


                int tomonthyear = Convert.ToInt32((Convert.ToInt32(spd[2]) * 12) + Convert.ToInt32(spd[1]));
                string todayperiod = "d" + dt.Day + "d" + ddlhour.SelectedItem.ToString();
                string sem = d2.GetFunction("select distinct  top 1  Current_Semester from Registration where CC=0 and DelFlag=0  and Exam_Flag<>'debar' and Batch_Year in(" + batchyears + ") order by Current_Semester desc");
                              
                string absentquery = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,r.Roll_Admit,case when a.sex=0 then 'Male' else 'Female' end as sex,r.Batch_Year,r.degree_code,r.Current_Semester,isnull(r.Sections,'') as Sections ,a.parent_name,a.parentF_Mobile,c.Course_Name,de.Dept_Name from Registration r,applyn a,attendance at,Degree d,Course c,Department de where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.App_No=a.app_no and r.Roll_No=at.roll_no and r.Batch_Year in(" + batchyears + ") and r.degree_code in(" + strdegrees + ") " + strsecquury + " and at.month_year='" + tomonthyear + "' and at." + todayperiod + " in(" + absentcode + ") and r.cc=0 " + includedisco + includedeba + " order by r.degree_code,r.Batch_Year desc,r.Current_Semester,r.Sections,r.Roll_No";
                DataSet dsattabsent = d2.select_method_wo_parameter(absentquery, "text");
                string date = d2.GetFunction("Select top 1 (convert(nvarchar(15),start_date,103)) as date from seminfo where  semester=" + sem + "  order by datepart(year,start_date) desc ,datepart(month ,start_date) desc");
                string[] spd1 = date.Split('/');
                DateTime fromdat = Convert.ToDateTime(spd1[1] + '/' + spd1[0] + '/' + spd1[2]);
                for (int itemcount = 0; itemcount < chklstsection.Items.Count; itemcount++)
                {
                    if (chklstsection.Items[itemcount].Selected == true)
                    {
                        if (chklstsection.Items[itemcount].Text.ToString() == "Empty")
                        {
                            if (strsecquury == "")
                                strsecquury = "''";
                            else
                                strsecquury = strsecquury + ",''";
                        }
                        else
                        {
                            if (strsecquury == "")
                                strsecquury = "'" + chklstsection.Items[itemcount].Value.ToString() + "'";
                            else
                                strsecquury = strsecquury + ",'" + chklstsection.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                }
                if (strsecquury.Trim() != "")
                {
                    strsec = strsecquury;
                    strsecquury = " and isnull(r.sections,'') in(" + strsecquury + ")";  //modified by Mullai
                }

                if (ddlhour.Items.Count == 0)
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Period and Then Proceed";
                    return;
                }

               Hashtable    hashheader=new Hashtable();
             int cun=0;
             dtstud.Columns.Add("S.No", typeof(string));
             dtstud.Columns.Add("Roll No");
             dtstud.Columns.Add("Reg No");
             dtstud.Columns.Add("Student Name", typeof(string));
             dtstud.Columns.Add("Department");
                dtstud.Columns.Add("Father Name");
                dtstud.Columns.Add("Mobile Number");
             drrow = dtstud.NewRow();
             drrow["S.No"] = "S.No";
             drrow["Roll No"] = "Roll No";
             drrow["Reg No"] = "Reg No";
             drrow["Student Name"] = "Student Name";
             drrow["Department"] = "Department";
                 drrow["Father Name"] = "Father Name";
                  drrow["Mobile Number"] = "Mobile Number";
             dtstud.Rows.Add(drrow);
                 string batchyear = "";
                int hascun=0;
                Hashtable stunam=new Hashtable();
                 Hashtable stunamcol=new Hashtable();
                if( dsattabsent.Tables.Count > 0 && dsattabsent.Tables[0].Rows.Count > 0)
                {
                 //for (int b = 0; b < chklst_batch.Items.Count; b++)
                 //{
                 //    if (chklst_batch.Items[b].Selected == true)
                 //    {

                 //        batchyear = "'" + chklst_batch.Items[b].Text + "'";


                        

                         //string strdegree = "";
                         //for (int c = 0; c < chklst_branch.Items.Count; c++)
                         //{
                         //    if (chklst_branch.Items[c].Selected == true)
                         //    {

                         //        strdegree = "'" + chklst_branch.Items[c].Value + "'";
                         //        cun++;
                                 //string fdate = txtFromDate.Text.ToString();
                                 //string[] spd = fdate.Split('/');
                                 //DateTime dt = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
                                
                                 //int tomonthyear = Convert.ToInt32((Convert.ToInt32(spd[2]) * 12) + Convert.ToInt32(spd[1]));
                                 //string todayperiod = "d" + dt.Day + "d" + ddlhour.SelectedItem.ToString();
                              
                              
                              
                                 Boolean flag = false;
                               
                                 while (fromdat <= dt.AddDays(-1))
                                 {
                                     int rowcun=0;
                                     Hashtable sturoll = new Hashtable();
                                     string stuname = string.Empty;
                                     string stunames = string.Empty;
                                       
                                     string mon = fromdat.ToString("MM");
                                     string year = fromdat.ToString("yyyy");
                                     string coldate = fromdat.ToString("dd/MM/yyyy");
                                     int monthyear = Convert.ToInt32((Convert.ToInt32(year) * 12) + Convert.ToInt32(mon));
                                     string dayperiod = "d" + fromdat.Day + "d" + ddlhour.SelectedItem.ToString();
                                     string absentquerys = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,r.Roll_Admit,case when a.sex=0 then 'Male' else 'Female' end as sex,r.Batch_Year,r.degree_code,r.Current_Semester,isnull(r.Sections,'') as Sections ,a.parent_name,a.parentF_Mobile,c.Course_Name,de.Dept_Name from Registration r,applyn a,attendance at,Degree d,Course c,Department de where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.App_No=a.app_no and r.Roll_No=at.roll_no and r.Batch_Year in(" + batchyears + ") and r.degree_code in(" + strdegrees + ") " + strsecquury + " and at.month_year='" + monthyear + "' and at." + dayperiod + " in(" + absentcode + ") and r.cc=0 " + includedisco + includedeba + " and r.Roll_No in(select r.Roll_No from Registration r,applyn a,attendance at,Degree d,Course c,Department de where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.App_No=a.app_no and r.Roll_No=at.roll_no and r.Batch_Year in(" + batchyears + ") and r.degree_code in(" + strdegrees + ") " + strsecquury + " and at.month_year='" + tomonthyear + "' and at." + todayperiod + " in(" + absentcode + ") and r.cc=0 " + includedisco + includedeba + " and a.sex='" + ddlgen.SelectedValue + "')  order by r.degree_code,r.Batch_Year desc,r.Current_Semester,r.Sections,r.Roll_No";
                                     DataSet dsattabsents = d2.select_method_wo_parameter(absentquerys, "text");
                                     if (dsattabsents.Tables.Count > 0 && dsattabsents.Tables[0].Rows.Count > 0)
                                     {
                                          
                                            
                                         string absentqus = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,r.Roll_Admit,case when a.sex=0 then 'Male' else 'Female' end as sex,r.Batch_Year,r.degree_code,r.Current_Semester,isnull(r.Sections,'') as Sections ,a.parent_name,a.parentF_Mobile,c.Course_Name,de.Dept_Name from Registration r,applyn a,attendance at,Degree d,Course c,Department de where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.App_No=a.app_no and r.Roll_No=at.roll_no and r.Batch_Year in(" + batchyears + ") and r.degree_code in(" + strdegrees + ") " + strsecquury + " and at.month_year='" + monthyear + "' and at." + dayperiod + " in(" + absentcode + ") and r.cc=0 " + includedisco + includedeba + " and r.Roll_No in(select r.Roll_No from Registration r,applyn a,attendance at,Degree d,Course c,Department de where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.App_No=a.app_no and r.Roll_No=at.roll_no and r.Batch_Year in(" + batchyears + ") and r.degree_code in(" + strdegrees + ") " + strsecquury + " and at.month_year='" + tomonthyear + "' and at." + todayperiod + " in(" + absentcode + ") and r.cc=0 " + includedisco + includedeba + " and a.sex='" + ddlgender.SelectedValue + "')  order by r.degree_code,r.Batch_Year desc,r.Current_Semester,r.Sections,r.Roll_No";
                                         DataSet dsattabsen = d2.select_method_wo_parameter(absentqus, "text");
                                         if (dsattabsen.Tables.Count > 0 && dsattabsen.Tables[0].Rows.Count > 0)
                                         {

                                         rowcun++;
                                                  drrow = dtstud.NewRow();
                                                  dtstud.Rows.Add(drrow);
                                             
                                             for (int i = 0; i < dsattabsents.Tables[0].Rows.Count; i++)
                                             {
                                                
                                               
                                                 
                                                 if (stunames == "")
                                                 {
                                                     cun++;
                                                     stuname = Convert.ToString(dsattabsents.Tables[0].Rows[i]["Reg_No"]) + '-' + Convert.ToString(dsattabsents.Tables[0].Rows[i]["Stud_Name"]);
                                                     stunames = stuname;
                                                     sturoll.Add(Convert.ToString(dsattabsents.Tables[0].Rows[i]["Reg_No"]), Convert.ToString(dsattabsents.Tables[0].Rows[i]["Stud_Name"]));
                                                     rowcun++;

                                                      drrow = dtstud.NewRow();
                                                        drrow["S.No"] = cun;
                                                        drrow["Roll No"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["Roll_No"]);
                                                        drrow["Reg No"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["Reg_No"]);
                                                        drrow["Student Name"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["Stud_Name"]);
                                                        drrow["Department"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["Course_Name"]) + '-' + Convert.ToString(dsattabsents.Tables[0].Rows[i]["Dept_Name"]);
                                                         drrow["Father Name"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["parent_name"]);
                                                         drrow["Mobile Number"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["parentF_Mobile"]);
                                                     dtstud.Rows.Add(drrow);

                                                 }
                                                 else
                                                 {
                                                     if (!sturoll.ContainsKey(Convert.ToString(dsattabsents.Tables[0].Rows[i]["Reg_No"])))
                                                     {
                                                         cun++;
                                                         stuname = Convert.ToString(dsattabsents.Tables[0].Rows[i]["Reg_No"]) + '-' + Convert.ToString(dsattabsents.Tables[0].Rows[i]["Stud_Name"]);
                                                         stunames = stunames + ';' + stuname;
                                                         sturoll.Add(Convert.ToString(dsattabsents.Tables[0].Rows[i]["Reg_No"]), Convert.ToString(dsattabsents.Tables[0].Rows[i]["Stud_Name"]));
                                                           rowcun++;
                                                            drrow = dtstud.NewRow();
                                                            drrow["S.No"] = cun ;
                                                            drrow["Roll No"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["Roll_No"]);
                                                            drrow["Reg No"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["Reg_No"]);
                                                            drrow["Student Name"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["Stud_Name"]);
                                                            drrow["Father Name"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["parent_name"]);
                                                            drrow["Mobile Number"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["parentF_Mobile"]);
                                                            drrow["Department"] = Convert.ToString(dsattabsents.Tables[0].Rows[i]["Course_Name"]) + '-' + Convert.ToString(dsattabsents.Tables[0].Rows[i]["Dept_Name"]);
                                                         dtstud.Rows.Add(drrow);
                                                     }
                                                 }


                                             }
                                             for (int i = 0; i < dsattabsen.Tables[0].Rows.Count; i++)
                                             {
                                                 if (stunames == "")
                                                 {
                                                     if (!sturoll.ContainsKey(Convert.ToString(dsattabsen.Tables[0].Rows[i]["Reg_No"])))
                                                     {
                                                         cun++;
                                                         stuname = Convert.ToString(dsattabsen.Tables[0].Rows[i]["Reg_No"]) + '-' + Convert.ToString(dsattabsen.Tables[0].Rows[i]["Stud_Name"]);
                                                         stunames = stuname;
                                                         sturoll.Add(Convert.ToString(dsattabsen.Tables[0].Rows[i]["Reg_No"]), Convert.ToString(dsattabsen.Tables[0].Rows[i]["Stud_Name"]));
                                                           rowcun++;
                                                            drrow = dtstud.NewRow();
                                                            drrow["S.No"] = cun ;
                                                            drrow["Roll No"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["Roll_No"]);
                                                            drrow["Reg No"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["Reg_No"]);
                                                            drrow["Student Name"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["Stud_Name"]);
                                                          drrow["Father Name"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["parent_name"]);
                                                          drrow["Mobile Number"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["parentF_Mobile"]);
                                                          drrow["Department"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["Course_Name"]) + '-' + Convert.ToString(dsattabsen.Tables[0].Rows[i]["Dept_Name"]);
                                                         dtstud.Rows.Add(drrow);
                                                     }

                                                 }
                                                 else
                                                 {
                                                     if (!sturoll.ContainsKey(Convert.ToString(dsattabsen.Tables[0].Rows[i]["Reg_No"])))
                                                     {
                                                         cun++;
                                                         stuname = Convert.ToString(dsattabsen.Tables[0].Rows[i]["Reg_No"]) + '-' + Convert.ToString(dsattabsen.Tables[0].Rows[i]["Stud_Name"]);
                                                         stunames = stunames + ';' + stuname;
                                                         sturoll.Add(Convert.ToString(dsattabsen.Tables[0].Rows[i]["Reg_No"]), Convert.ToString(dsattabsen.Tables[0].Rows[i]["Stud_Name"]));
                                                           rowcun++;
                                                            drrow = dtstud.NewRow();
                                                            drrow["S.No"] = cun;
                                                            drrow["Roll No"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["Roll_No"]);
                                                            drrow["Reg No"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["Reg_No"]);
                                                            drrow["Student Name"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["Stud_Name"]);
                                                            drrow["Father Name"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["parent_name"]);
                                                            drrow["Mobile Number"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["parentF_Mobile"]);
                                                            drrow["Department"] = Convert.ToString(dsattabsen.Tables[0].Rows[i]["Course_Name"]) + '-' + Convert.ToString(dsattabsen.Tables[0].Rows[i]["Dept_Name"]);
                                                         dtstud.Rows.Add(drrow);
                                                     }
                                                 }


                                             }
                                             string[] spl = stunames.Split(';');
                                             if (spl.Length > 1)
                                             {
                                                 hascun++;

                                                 //  dtstud.Columns.Add(coldate);
                                                 // dtstud.Rows[dtstud.Rows.Count-(rowcun-1)][coldate] = coldate;
                                                 //   drrow[coldate] = stunames;
                                                 //  dict.Add(coldate,stunames);
                                                 if (stunam.ContainsValue(stunames))
                                                 {
                                                     string hsdate = Convert.ToString(hstable[stunames]);
                                                     int hedercun = Convert.ToInt32(hashheader[hsdate]);
                                                     stunamcol[hedercun] = hsdate + ',' + coldate;
                                                     hstable[stunames] = hsdate + ',' + coldate;
                                                     for (int m = dtstud.Rows.Count - rowcun; m < dtstud.Rows.Count; m++)
                                                     {
                                                         DataRow drs = dtstud.Rows[m];
                                                         dtstud.Rows.Remove(drs);
                                                         m--;
                                                         if (m != dtstud.Rows.Count - rowcun)
                                                         cun--;
                                                     }

                                                 }
                                                 else
                                                 {
                                                     if (!hstable.ContainsKey(stunames))
                                                     {
                                                         stunam.Add(coldate, stunames);
                                                         hstable.Add(stunames, coldate);
                                                     }
                                                     hashheader.Add(coldate, dtstud.Rows.Count - (rowcun));
                                                     stunamcol.Add(dtstud.Rows.Count - (rowcun), coldate);
                                                 }

                                             }
                                             else
                                             {
                                                 for (int m = dtstud.Rows.Count - rowcun; m < dtstud.Rows.Count; m++)
                                                 {
                                                     DataRow drs = dtstud.Rows[m];
                                                     dtstud.Rows.Remove(drs);
                                                     m--;
                                                    if(m!= dtstud.Rows.Count - rowcun)
                                                     cun--;
                                                 }
                                             }
                                            
                                         
                                                 //chklst_batch.Items[b].Text + '-' + chklst_branch.Items[c].Text;
                                            // dtstud.Rows[dtstud.Rows.Count-1][coldate] = stunames;
                                             flag = true;
                                         }


                                     }
                                     fromdat = fromdat.AddDays(1);
                                    // dtstud.Rows.Add(drrow);
                                 }
                                 //if (flag==true)
                                 //dtstud.Rows.Add(drrow);
                                 
                             //}
                        // }
                   //  }
                 }
                if (dtstud.Rows.Count > 1)
                {
                    for (int j = 0; j < dtstud.Rows.Count; j++)
                    {
                        string nam = Convert.ToString(dtstud.Rows[j]["Student Name"]);
                        if (nam == "")
                        {
                            string val = Convert.ToString(stunamcol[j]);
                            dtstud.Rows[j]["S.No"] = val;
                        }

                    }
                    gview.DataSource = dtstud;
                    gview.DataBind();
                    gview.Visible = true;
                    //  Div4.Visible = true;
                    btnprint.Visible = true;
                    btnPrint1.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnxl.Visible = true;
                    divMainContents.Visible = true;
                    showad.Visible = true;

                    Button3.Visible = true;

                    for (int g = 0; g < dtstud.Columns.Count; g++)
                    {
                        string columname = dtstud.Columns[g].ColumnName;


                        if (columname.ToUpper() == "ROLL NO")
                            {

                                if (Convert.ToString(Session["Rollflag"]) == "0" || Convert.ToString(Session["Rollflag"]) == "")
                                {

                                    for (int r = 0; r < gview.Rows.Count; r++)
                                        gview.Rows[r].Cells[g].Visible = false;
                                   

                                }
                            }
                        else if (columname.ToUpper() == "REGISTER NO")
                            {
                                if (Convert.ToString(Session["Regflag"]) == "0" || Convert.ToString(Session["Regflag"]) == "")
                                {

                                    for (int r = 0; r < gview.Rows.Count; r++)
                                        gview.Rows[r].Cells[g].Visible = false;
                                   

                                }
                            }
                        else if (columname.ToUpper() == "STUDENT TYPE")
                            {
                                if (Convert.ToString(Session["Studflag"]) == "0" || Convert.ToString(Session["Studflag"]) == "")
                                {

                                    for (int r = 0; r < gview.Rows.Count; r++)
                                        gview.Rows[r].Cells[g].Visible = false;
                                    

                                }
                            }
                        else if (columname.ToUpper() == "ADMISSION NO")
                            {
                                if (Convert.ToString(Session["AdmissionNo"]) == "0" || Convert.ToString(Session["AdmissionNo"]) == "")
                                {

                                    for (int r = 0; r < gview.Rows.Count; r++)
                                        gview.Rows[r].Cells[g].Visible = false;
                                   

                                }
                            }
                        //else
                        //{

                        //    for (int r = 0; r < gview.Rows.Count; r++)
                        //        gview.Rows[r].Cells[g].Visible = true;
                        //}
                        

                    }

                    for (int m = gview.Rows.Count - 1; m >= 1; m--)
                    {
                        GridViewRow rows = gview.Rows[m];
                        GridViewRow previousRows = gview.Rows[m];
                        GridViewRow previousRowss = gview.Rows[m];
                        //gview.Rows[m].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                        //gview.Rows[m].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                        //gview.Rows[m].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                        gview.Rows[m].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        string cellte = gview.Rows[m].Cells[0].Text;
                        if (!Convert.ToString(cellte).All(char.IsNumber))
                        {
                            gview.Rows[m].Cells[0].ColumnSpan = gview.Rows[m].Cells.Count;

                            gview.Rows[m].Cells[0].ColumnSpan = gview.Rows[m].Cells.Count;
                            for (int j = 1; j < gview.Rows[m].Cells.Count; j++)
                            {
                                //gview.Rows[m].Cells[0].Visible = false;
                                gview.Rows[m].Cells[j].Visible = false;
                                gview.Rows[m].Cells[j].BackColor = Color.DarkSeaGreen;
                                gview.Rows[m].Cells[0].BackColor = Color.DarkSeaGreen;

                            }
                        }
                    }
                    RowHead(gview);


                }
                else
                {
                    gview.Visible = false;
                    //  Div4.Visible = true;
                    btnprint.Visible = false;
                    Button3.Visible = true;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    divMainContents.Visible = false;
                    errmsg.Visible = true;
                    showad.Visible = false;
                    errmsg.Text = "No Record";

                }


            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
            Showgrid.Visible = false;
            divMainContents.Visible = false;
        }
    }
     protected void RowHead(GridView gview)
    {
        for (int head = 0; head < 1; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
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



        }
        catch
        {
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            if (Showgrid.Visible == true)
            {
                string reportname = txtexcelname.Text.ToString().Trim();
                if (reportname != "")
                {
                    d2.printexcelreportgrid(Showgrid, reportname);
                }
                else
                {
                    errmsg.Text = "Please Enter Your Report Name";
                    errmsg.Visible = true;
                }
            }
             else if (gview.Visible == true)
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                d2.printexcelreportgrid(gview, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void btnprint1_Click(object sender, EventArgs e)
    {
        string ss = null;
        if (Showgrid.Visible == true)
        {
            NEWPrintMater1.loadspreaddetails(Showgrid, "Periodwiseattendancereport123.aspx", "PERIOD WISE CONSOLIDATED STUDENT'S ATTENDANCE@Date : " + txtFromDate.Text + " @Period: " + ddlhour.SelectedItem.ToString() + "", 0, ss);
            NEWPrintMater1.Visible = true;
        }
       
    }

    protected void btnprint2_Click(object sender, EventArgs e)
    {
        string ss = null;
         if (gview.Visible == true)
        {
            NEWPrintMater1.loadspreaddetails(gview, "Periodwiseattendancereport.aspx", "PERIOD WISE CONSOLIDATED STUDENT'S ATTENDANCE@Date : " + txtFromDate.Text + " @Period: " + ddlhour.SelectedItem.ToString() + "", 0, ss);
            NEWPrintMater1.Visible = true;
        }
    }
    protected void btnxl2_Click(object sender, EventArgs e)
    {
        try
        {
            if (gview.Visible == true)
            {
                string reportname = TextBox1.Text.ToString().Trim();
                if (reportname != "")
                {
                    d2.printexcelreportgrid(gview, reportname);
                }
                else
                {
                    errmsg.Text = "Please Enter Your Report Name";
                    errmsg.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
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
        spReportName.InnerHtml = "Absentees Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }

    public void btnPrint12()
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
        spCollege.InnerHtml = collegeName;
        spAddress.InnerHtml = collegeAdd;
       // spDegreeName.InnerHtml = acr;
        spReport.InnerHtml = "Absentees Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
    //protected void ImageButton3_Click(object sender, EventArgs e)
    //{
    //    Div4.Visible = false;
    //}
    public override void VerifyRenderingInServerForm(Control control)
    { }
    protected void chkabsent_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkabsent.Checked == true)
        {
            lblgen.Visible = true;
            ddlgen.Visible = true;
            ddlgender.Visible = true;
        }
        else
        {
            lblgen.Visible = false;
            ddlgen.Visible = false;
            ddlgender.Visible = false;
        }
    }

}
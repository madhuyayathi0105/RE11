using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
public partial class TimeTableBlackBox : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable has = new Hashtable();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    int count = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        lbl_err.Visible = false;
        if (!IsPostBack)
        {
            clear();
            bindbatch();
            binddegree();
            bindbranch();
            bindsem();
            bindsec();
            //rbtimetable.Checked = true;
            //rbbatch.Checked = true;
            //chkles.Checked = true;
            rbtimetable.Checked = true;
            ddltimetable.Enabled = true;
            ddlbatchallocation.Enabled = false;
            ddllession.Enabled = false;
            for (int i = 0; i < chklscolumn.Items.Count; i++)
            {
                chklscolumn.Items[i].Selected = true;
            }
        }

    }
    public void bindbatch()
    {
        try
        {
            Chklst_batch.Items.Clear();
            Chk_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds = da.BindBatch();
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
    public void bindsem()
    {
        try
        {
            chklssem.Items.Clear();
            txtsem.Text = "---Select---";
            chksem.Checked = false;
            string degreecode = "";
            for (int h = 0; h < chklst_branch.Items.Count; h++)
            {
                if (chklst_branch.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = chklst_branch.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + chklst_branch.Items[h].Value;
                    }
                }
            }
            string strgetfuncuti = da.GetFunction("select max(Duration) from Degree");
            if (degreecode.Trim() != "")
            {
                strgetfuncuti = da.GetFunction("select max(Duration) from Degree where Degree_Code in(" + degreecode + ")");
            }
            if (Convert.ToInt32(strgetfuncuti) > 0)
            {
                for (int loop_val = 1; loop_val <= Convert.ToInt32(strgetfuncuti); loop_val++)
                {
                    chklssem.Items.Add(loop_val.ToString());
                    chklssem.Items[loop_val - 1].Selected = true;
                }
                txtsem.Text = "Sem (" + Convert.ToInt16(strgetfuncuti) + ")";
                chksem.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Text = ex.ToString();
            lbl_err.Visible = true;
        }
    }
    public void bindsec()
    {
        try
        {
            chksec.Checked = false;
            txtsec.Text = "---Select---";
            chklssec.Items.Clear();
            string batchquery = "";
            txtsec.Enabled = false;
            for (int h = 0; h < Chklst_batch.Items.Count; h++)
            {
                if (Chklst_batch.Items[h].Selected == true)
                {
                    if (batchquery == "")
                    {
                        batchquery = Chklst_batch.Items[h].Value;
                    }
                    else
                    {
                        batchquery = batchquery + ',' + Chklst_batch.Items[h].Value;
                    }
                }
            }
            string degreecode = "";
            for (int h = 0; h < chklst_branch.Items.Count; h++)
            {
                if (chklst_branch.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = chklst_branch.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + chklst_branch.Items[h].Value;
                    }
                }
            }
            if (degreecode.Trim() != "" && batchquery.Trim() != "")
            {
                string secquery = "select distinct Sections from Registration where Batch_Year in (" + batchquery + ") and degree_code in(" + degreecode + ") and isnull(Sections,'')<>''  order by Sections"; //and Sections<>'-1' //Modified By Mullai
                ds.Dispose();
                ds.Reset();
                ds = da.select_method_wo_parameter(secquery, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklssec.DataSource = ds;
                    chklssec.DataValueField = "Sections";
                    chklssec.DataTextField = "Sections";
                    chklssec.DataBind();

                    chklssec.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Empty Section", ""));
                    for (int i = 0; i < chklssec.Items.Count; i++)
                    {
                        chklssec.Items[i].Selected = true;
                    }
                    txtsec.Text = "Sec (" + chklssec.Items.Count + ")";
                    chksec.Checked = true;
                    txtsec.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Text = ex.ToString();
            lbl_err.Visible = true;
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
            bindsem();
            bindsec();
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
            bindsem();
            bindsec();
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
            bindsec();
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
            bindsem();
            bindsec();
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
            bindsem();
            bindsec();
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
            bindsem();
            bindsec();
            bindsec();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    protected void chklssem_selected(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtsem.Text = "--Select--";
            count = 0;
            chksem.Checked = false;
            for (int i = 0; i < chklssem.Items.Count; i++)
            {
                if (chklssem.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }

            if (count > 0)
            {
                txtsem.Text = "Sem (" + count.ToString() + ")";
                if (count == chklssem.Items.Count)
                {
                    chksem.Checked = true;
                }
            }
            bindsec();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    protected void chksem_changed(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chksem.Checked == true)
            {
                for (int i = 0; i < chklssem.Items.Count; i++)
                {
                    chklssem.Items[i].Selected = true;
                }
                txtsem.Text = "Sem (" + (chklssem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklssem.Items.Count; i++)
                {
                    chklssem.Items[i].Selected = false;
                }
                txtsem.Text = "--Select--";
            }
            bindsec();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    protected void chksec_changed(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chksec.Checked == true)
            {
                for (int i = 0; i < chklssec.Items.Count; i++)
                {
                    chklssec.Items[i].Selected = true;
                }
                txtsec.Text = "Sec (" + (chklssec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklssec.Items.Count; i++)
                {
                    chklssec.Items[i].Selected = false;
                }
                txtsec.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    protected void chklssec_selected(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtsec.Text = "--Select--";
            count = 0;
            chksec.Checked = false;
            for (int i = 0; i < chklssec.Items.Count; i++)
            {
                if (chklssec.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }

            if (count > 0)
            {
                txtsec.Text = "Sec (" + count.ToString() + ")";
                if (count == chklssec.Items.Count)
                {
                    chksec.Checked = true;
                }
            }
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
    }
    protected void Logout_btn_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    protected void headrechnge(object sender, EventArgs e)
    {
        clear();
        if (chkcurrent.Checked == true)
        {
            txtsem.Enabled = false;
            for (int i = 0; i < chklssem.Items.Count; i++)
            {
                chklssem.Items[i].Selected = true;
            }
        }
        else
        {
            txtsem.Enabled = true;
            chklssem_selected(sender, e);
        }
        ddltimetable.Enabled = false;
        ddlbatchallocation.Enabled = false;
        ddllession.Enabled = false;
        if (rbtimetable.Checked == true)
        {
            ddltimetable.Enabled = true;
        }
        if (rbbatch.Checked == true)
        {
            ddlbatchallocation.Enabled = true;
        }
        if (rblession.Checked == true)
        {
            ddllession.Enabled = true;
        }
    }

    //protected void btngo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Boolean recflag = false;
    //        clear();
    //        string testbatchyear = "";
    //        for (int j = 0; j < Chklst_batch.Items.Count; j++)
    //        {
    //            if (Chklst_batch.Items[j].Selected == true)
    //            {
    //                if (testbatchyear == "")
    //                {
    //                    testbatchyear = "'" + Chklst_batch.Items[j].Value.ToString() + "'";
    //                }
    //                else
    //                {
    //                    testbatchyear = testbatchyear + ",'" + Chklst_batch.Items[j].Value.ToString() + "'";
    //                }
    //            }
    //        }
    //        if (testbatchyear.Trim() == "")
    //        {
    //            lbl_err.Visible = true;
    //            lbl_err.Text = "Please Select The Batch And Then Proceed";
    //            return;
    //        }

    //        string testbranch = "";
    //        for (int j = 0; j < chklst_branch.Items.Count; j++)
    //        {
    //            if (chklst_branch.Items[j].Selected == true)
    //            {
    //                if (testbranch == "")
    //                {
    //                    testbranch = "'" + chklst_branch.Items[j].Value.ToString() + "'";
    //                }
    //                else
    //                {
    //                    testbranch = testbranch + ",'" + chklst_branch.Items[j].Value.ToString() + "'";
    //                }
    //            }
    //        }
    //        if (testbranch.Trim() == "")
    //        {
    //            lbl_err.Visible = true;
    //            lbl_err.Text = "Please Select The Degree and Branch And Then Proceed";
    //            return;
    //        }

    //        string strsem = "";
    //        for (int j = 0; j < chklssem.Items.Count; j++)
    //        {
    //            if (chklssem.Items[j].Selected == true)
    //            {
    //                if (strsem == "")
    //                {
    //                    strsem = "'" + chklssem.Items[j].Value.ToString() + "'";
    //                }
    //                else
    //                {
    //                    strsem = strsem + ",'" + chklssem.Items[j].Value.ToString() + "'";
    //                }
    //            }
    //        }
    //        if (strsem.Trim() == "")
    //        {
    //            lbl_err.Visible = true;
    //            lbl_err.Text = "Please Select The Semester And Then Proceed";
    //            return;
    //        }



    //        string strsec = "";
    //        for (int j = 0; j < chklssec.Items.Count; j++)
    //        {
    //            if (chklssec.Items[j].Selected == true)
    //            {
    //                if (strsec == "")
    //                {
    //                    if (chklssec.Items[j].Text == "Empty Section")
    //                    {
    //                        strsec = "''";
    //                    }
    //                    else
    //                    {
    //                        strsec = "'" + chklssec.Items[j].Value.ToString() + "'";
    //                    }
    //                }
    //                else
    //                {
    //                    if (chklssec.Items[j].Text == "Empty Section")
    //                    {
    //                        strsec = strsec + ",''";
    //                    }
    //                    else
    //                    {
    //                        strsec = strsec + ",'" + chklssec.Items[j].Value.ToString() + "'";
    //                    }
    //                }
    //            }
    //        }
    //        if (strsec.Trim() != "")
    //        {
    //            strsec = " and r.sections in(" + strsec + ")";
    //        }

    //        //if (rbtimetable.Checked == false && rbbatch.Checked == false && chkles.Checked == false)
    //        //{
    //        //    lbl_err.Visible = true;
    //        //    lbl_err.Text = "Please Select Anyone Report to be Display";
    //        //    return;
    //        //}

    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

    //        FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
    //        FpSpread1.Sheets[0].SheetCorner.RowCount = 1;
    //        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
    //        style.Font.Size = 10;
    //        style.Font.Bold = true;
    //        FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //        FpSpread1.Sheets[0].AllowTableCorner = true;
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
    //        FpSpread1.Sheets[0].RowHeader.Visible = false;
    //        FpSpread1.CommandBar.Visible = false;
    //        FpSpread1.Sheets[0].AutoPostBack = true;

    //        FpSpread1.Sheets[0].RowCount = 0;
    //        FpSpread1.Sheets[0].ColumnCount = 0;
    //        FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
    //        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
    //        FpSpread1.Sheets[0].ColumnCount = 16;

    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
    //        FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[0].Width = 30;
    //        if (chklscolumn.Items[0].Selected == true)
    //        {
    //            FpSpread1.Sheets[0].Columns[0].Visible = true;
    //            recflag = true;
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[0].Visible = false;
    //        }

    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
    //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
    //        FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[1].Width = 30;
    //        if (chklscolumn.Items[1].Selected == true)
    //        {
    //            FpSpread1.Sheets[0].Columns[1].Visible = true;
    //            recflag = true;
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[1].Visible = false;
    //        }

    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
    //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
    //        FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[2].Width = 80;
    //        if (chklscolumn.Items[2].Selected == true)
    //        {
    //            FpSpread1.Sheets[0].Columns[2].Visible = true;
    //            recflag = true;
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[2].Visible = false;
    //        }
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Branch";
    //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
    //        FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[3].Width = 100;
    //        if (chklscolumn.Items[3].Selected == true)
    //        {
    //            FpSpread1.Sheets[0].Columns[3].Visible = true;
    //            recflag = true;
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[3].Visible = false;
    //        }

    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sem";
    //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
    //        FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[4].Width = 30;
    //        if (chklscolumn.Items[4].Selected == true)
    //        {
    //            FpSpread1.Sheets[0].Columns[4].Visible = true;
    //            recflag = true;
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[4].Visible = false;
    //        }
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Section";
    //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
    //        FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[5].Width = 30;
    //        if (chklscolumn.Items[5].Selected == true)
    //        {
    //            FpSpread1.Sheets[0].Columns[5].Visible = true;
    //            recflag = true;
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[5].Visible = false;
    //        }

    //        //if (rbtimetable.Checked == true)
    //        if (rbtimetable.Checked == true)
    //        {
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Name";
    //            FpSpread1.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
    //            FpSpread1.Sheets[0].Columns[6].Width = 100;
    //            if (chklscolumn.Items[6].Selected == true)
    //            {
    //                FpSpread1.Sheets[0].Columns[6].Visible = true;
    //                recflag = true;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Time Table";
    //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 2);
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Columns[6].Visible = false;
    //            }

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Start Date";
    //            FpSpread1.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
    //            FpSpread1.Sheets[0].Columns[7].Width = 80;

    //            if (chklscolumn.Items[7].Selected == true)
    //            {
    //                FpSpread1.Sheets[0].Columns[7].Visible = true;
    //                recflag = true;
    //                if (chklscolumn.Items[6].Selected == false)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Time Table";
    //                }
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Columns[7].Visible = false;
    //            }
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[6].Visible = false;
    //            FpSpread1.Sheets[0].Columns[7].Visible = false;
    //        }

    //        //if (rbbatch.Checked == true)
    //        if (rbbatch.Checked == true)
    //        {
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Status";
    //            FpSpread1.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
    //            FpSpread1.Sheets[0].Columns[0].Width = 30;

    //            if (chklscolumn.Items[8].Selected == true)
    //            {
    //                FpSpread1.Sheets[0].Columns[8].Visible = true;
    //                recflag = true;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Batch Allocation";
    //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 3);
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Columns[8].Visible = false;
    //            }

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Student Batch";
    //            FpSpread1.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
    //            FpSpread1.Sheets[0].Columns[9].Width = 30;
    //            if (chklscolumn.Items[9].Selected == true)
    //            {
    //                FpSpread1.Sheets[0].Columns[9].Visible = true;
    //                recflag = true;
    //                if (chklscolumn.Items[8].Selected == false)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Batch Allocation";
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 1, 2);
    //                }
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Columns[9].Visible = false;
    //            }

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Text = "Student Count";
    //            FpSpread1.Sheets[0].Columns[10].VerticalAlign = VerticalAlign.Middle;
    //            FpSpread1.Sheets[0].Columns[10].Width = 30;

    //            if (chklscolumn.Items[10].Selected == true)
    //            {
    //                FpSpread1.Sheets[0].Columns[10].Visible = true;
    //                recflag = true;
    //                if (chklscolumn.Items[8].Selected == false && chklscolumn.Items[9].Selected == false)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Batch Allocation";
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 1, 1);
    //                }
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Columns[10].Visible = false;
    //            }
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[8].Visible = false;
    //            FpSpread1.Sheets[0].Columns[9].Visible = false;
    //            FpSpread1.Sheets[0].Columns[10].Visible = false;
    //        }


    //        //  if (chkles.Checked == true)
    //        if (rblession.Checked == true)
    //        {
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Text = "Status";
    //            FpSpread1.Sheets[0].Columns[11].VerticalAlign = VerticalAlign.Middle;
    //            FpSpread1.Sheets[0].Columns[11].Width = 30;
    //            if (chklscolumn.Items[11].Selected == true)
    //            {
    //                recflag = true;
    //                FpSpread1.Sheets[0].Columns[11].Visible = true;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Lesson Planner";
    //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 1, 5);
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Columns[11].Visible = false;
    //            }

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Text = "Subject Code";
    //            FpSpread1.Sheets[0].Columns[12].VerticalAlign = VerticalAlign.Middle;
    //            FpSpread1.Sheets[0].Columns[12].Width = 80;

    //            if (chklscolumn.Items[12].Selected == true)
    //            {
    //                recflag = true;
    //                FpSpread1.Sheets[0].Columns[12].Visible = true;
    //                if (chklscolumn.Items[11].Selected == false)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Lesson Planner";
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 1, 4);
    //                }
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Columns[12].Visible = false;
    //            }

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].Text = "Subject Name";
    //            FpSpread1.Sheets[0].Columns[13].VerticalAlign = VerticalAlign.Middle;
    //            FpSpread1.Sheets[0].Columns[13].Width = 200;

    //            if (chklscolumn.Items[13].Selected == true)
    //            {
    //                recflag = true;
    //                FpSpread1.Sheets[0].Columns[13].Visible = true;
    //                if (chklscolumn.Items[11].Selected == false && chklscolumn.Items[12].Selected == false)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Lesson Planner";
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 13, 1, 3);
    //                }
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Columns[13].Visible = false;
    //            }

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 14].Text = "Staff Code";
    //            FpSpread1.Sheets[0].Columns[14].VerticalAlign = VerticalAlign.Middle;
    //            FpSpread1.Sheets[0].Columns[14].Width = 80;

    //            if (chklscolumn.Items[14].Selected == true)
    //            {
    //                recflag = true;
    //                FpSpread1.Sheets[0].Columns[13].Visible = true;
    //                if (chklscolumn.Items[11].Selected == false && chklscolumn.Items[12].Selected == false && chklscolumn.Items[13].Selected == false)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Lesson Planner";
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 14, 1, 2);
    //                }
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Columns[14].Visible = false;
    //            }

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 15].Text = "Staff Name";
    //            FpSpread1.Sheets[0].Columns[15].VerticalAlign = VerticalAlign.Middle;
    //            FpSpread1.Sheets[0].Columns[15].Width = 150;

    //            if (chklscolumn.Items[15].Selected == true)
    //            {
    //                recflag = true;
    //                FpSpread1.Sheets[0].Columns[15].Visible = true;
    //                if (chklscolumn.Items[11].Selected == false && chklscolumn.Items[12].Selected == false && chklscolumn.Items[13].Selected == false && chklscolumn.Items[14].Selected == false)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Lesson Planner";
    //                }
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Columns[15].Visible = false;
    //            }
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[11].Visible = false;
    //            FpSpread1.Sheets[0].Columns[12].Visible = false;
    //            FpSpread1.Sheets[0].Columns[13].Visible = false;
    //            FpSpread1.Sheets[0].Columns[14].Visible = false;
    //            FpSpread1.Sheets[0].Columns[15].Visible = false;
    //        }

    //        if (recflag == false)
    //        {
    //            lbl_err.Visible = true;
    //            lbl_err.Text = "Please Select The Column Order And Then Proceed";
    //            return;
    //        }
    //        recflag = false;
    //        FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //        FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //        FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //        FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //        FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //        FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);


    //        FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;

    //        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
    //        style2.Font.Size = 13;
    //        style2.Font.Name = "Book Antiqua";
    //        style2.Font.Bold = true;
    //        style2.HorizontalAlign = HorizontalAlign.Center;
    //        style2.ForeColor = System.Drawing.Color.White;
    //        style2.BackColor = System.Drawing.Color.Teal;
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

    //        string currrstatus = "";
    //        if (chkcurrent.Checked == true)
    //        {
    //            currrstatus = " and r.Current_Semester=s.semester and r.cc=0 and r.exam_flag<>'debar' and r.delflag=0";
    //        }

    //        string strdegreequery = "select distinct r.Batch_Year,c.Course_Name,de.Dept_Name,d.Course_Id,r.degree_code,s.semester,r.Sections from seminfo s,Registration r,Degree d,Course c,Department de where s.batch_year=r.Batch_Year and s.degree_code=r.degree_code and d.Degree_Code=r.degree_code and s.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id " + currrstatus + " " + strsec + " order by d.Course_Id,r.degree_code,r.Batch_Year desc,s.semester,r.Sections";
    //        DataSet dsdegreedetails = da.select_method_wo_parameter(strdegreequery, "Text");

    //        string strsemesterscehduelquery = "select batch_year,degree_code,semester,Sections,TTName,fromdate,convert(nvarchar(15),fromdate,103) as sdate from Semester_Schedule order by fromdate";
    //        DataSet dssemscehdule = da.select_method_wo_parameter(strsemesterscehduelquery, "Text");

    //        string strbatchallocquery = "select count(distinct sc.roll_no) as stucount,l.batch_year,l.degree_code,l.semester,l.Sections,l.Timetablename,l.Stu_Batch,l.fromdate from subjectChooser sc,Registration r,LabAlloc l where l.Subject_No=sc.subject_no and r.Batch_Year=l.Batch_Year and l.Degree_Code=r.degree_code and r.Roll_No=sc.roll_no and l.Sections=r.Sections and l.Subject_No=sc.subject_no and l.Stu_Batch=sc.Batch and l.Semester=sc.semester and sc.Batch=l.Stu_Batch and isnull(sc.Batch,'')<>'' and isnull(l.Stu_Batch,'')<>'' group by l.batch_year,l.degree_code,l.semester,l.Sections,l.Timetablename,l.Stu_Batch,l.fromdate";
    //        DataSet dsbatchalloc = da.select_method_wo_parameter(strbatchallocquery, "Text");

    //        string strsubjectquery = "select sy.Batch_Year,sy.degree_code,sy.semester,s.subject_no,s.subject_name,s.subject_code from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=ss.syll_code and ss.syll_code=s.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no";
    //        DataSet dssubject = da.select_method_wo_parameter(strsubjectquery, "Text");

    //        string strsubuint = "select distinct subject_no from sub_unit_details";
    //        DataSet dssubunit = da.select_method_wo_parameter(strsubuint, "Text");

    //        string strlessionquery = "select distinct subject_no,l.Sections from lesson_plan l,lessonPlanTopics lp where l.LP_code=lp.LP_code";
    //        DataSet dslession = da.select_method_wo_parameter(strlessionquery, "Text");

    //        string strstaffsubject = "select st.staff_code,st.staff_name,ss.subject_no,ss.Sections from staffmaster st,staff_selector ss where st.staff_code=ss.staff_code";
    //        DataSet dssstaff = da.select_method_wo_parameter(strstaffsubject, "Text");

    //        int srno = 0;

    //        for (int b = 0; b < Chklst_batch.Items.Count; b++)
    //        {
    //            if (Chklst_batch.Items[b].Selected == true)
    //            {
    //                string batchyear = Chklst_batch.Items[b].Value.ToString();

    //                for (int br = 0; br < chklst_branch.Items.Count; br++)
    //                {
    //                    if (chklst_branch.Items[br].Selected == true)
    //                    {
    //                        string degreecode = chklst_branch.Items[br].Value.ToString();
    //                        for (int j = 0; j < chklssem.Items.Count; j++)
    //                        {
    //                            if (chklssem.Items[j].Selected == true)
    //                            {
    //                                string sem = chklssem.Items[j].Text.ToString();
    //                                dsdegreedetails.Tables[0].DefaultView.RowFilter = "batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem.ToString() + "'";
    //                                DataView dvdegree = dsdegreedetails.Tables[0].DefaultView;
    //                                for (int i = 0; i < dvdegree.Count; i++)
    //                                {

    //                                    int rowval = -1;
    //                                    string course = dvdegree[i]["Course_Name"].ToString();
    //                                    string department = dvdegree[i]["Dept_Name"].ToString();
    //                                    string section = dvdegree[i]["Sections"].ToString();
    //                                    string sectval = "";

    //                                    if (section.Trim() != "" && section.Trim() != "-1")
    //                                    {
    //                                        sectval = " and sections='" + section + "'";
    //                                    }
    //                                    else
    //                                    {
    //                                        section = " ";
    //                                    }
    //                                    dssubject.Tables[0].DefaultView.RowFilter = "batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem.ToString() + "'";
    //                                    DataView dvsubject = dssubject.Tables[0].DefaultView;
    //                                    if (dvsubject.Count > 0)
    //                                    {
    //                                        dssemscehdule.Tables[0].DefaultView.RowFilter = "batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem.ToString() + "' " + sectval + "";
    //                                        DataView dvsemschedule = dssemscehdule.Tables[0].DefaultView;
    //                                        if(rbtimetable.Checked==true || rbbatch.Checked==true)
    //                                        {
    //                                            if (dvsemschedule.Count > 0)
    //                                            {
    //                                                if (ddltimetable.SelectedValue.ToString() != "2")
    //                                                {
    //                                                    recflag = true;
    //                                                    for (int s = 0; s < dvsemschedule.Count; s++)
    //                                                    {
    //                                                        if ((rbtimetable.Checked == true && ddlbatchallocation.SelectedValue.ToString() == "0"))
    //                                                        {
    //                                                            srno++;
    //                                                            FpSpread1.Sheets[0].RowCount++;
    //                                                            rowval = FpSpread1.Sheets[0].RowCount - 1;
    //                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
    //                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

    //                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear;
    //                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = course;
    //                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = department;
    //                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sem;
    //                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = section;
    //                                                        }
    //                                                        string strsemschedule = dvsemschedule[s]["TTName"].ToString();
    //                                                        string sdate = dvsemschedule[s]["sdate"].ToString();
    //                                                        string getdate = dvsemschedule[s]["fromdate"].ToString();

    //                                                        Boolean batchallocflag = false;
    //                                                        if ((rbtimetable.Checked == true && ddlbatchallocation.SelectedValue.ToString() == "0"))
    //                                                        {
    //                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = strsemschedule;
    //                                                            if (strsemschedule.Trim() == "")
    //                                                            {
    //                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Not Created";
    //                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
    //                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
    //                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Red;
    //                                                            }
    //                                                            else
    //                                                            {
    //                                                                batchallocflag = true;
    //                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = sdate;
    //                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
    //                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Green;
    //                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
    //                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Green;
    //                                                            }
    //                                                        }
    //                                                        batchallocflag = false;
    //                                                        if (rbbatch.Checked == true)
    //                                                        {
    //                                                            dsbatchalloc.Tables[0].DefaultView.RowFilter = "batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem.ToString() + "' and Timetablename='" + strsemschedule.Trim() + "' " + sectval + " and fromdate='" + getdate + "'";
    //                                                            DataView dvbatchalloc = dsbatchalloc.Tables[0].DefaultView;

    //                                                            if (dvbatchalloc.Count > 0)
    //                                                            {
    //                                                                if (ddlbatchallocation.SelectedValue.ToString() != "2")
    //                                                                {
    //                                                                    if (rbtimetable.Checked == false || (rbtimetable.Checked == true && ddlbatchallocation.SelectedValue.ToString() != "0") || (rbtimetable.Checked == true && ddlbatchallocation.SelectedValue.ToString() != "0"))
    //                                                                    {
    //                                                                        srno++;
    //                                                                        FpSpread1.Sheets[0].RowCount++;
    //                                                                        rowval = FpSpread1.Sheets[0].RowCount - 1;
    //                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
    //                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

    //                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear;
    //                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = course;
    //                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = department;
    //                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sem;
    //                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = section;
    //                                                                    }
    //                                                                    recflag = true;
    //                                                                    batchallocflag = true;
    //                                                                    FpSpread1.Sheets[0].Cells[rowval, 8].Text = "Y";
    //                                                                    FpSpread1.Sheets[0].Cells[rowval, 8].HorizontalAlign = HorizontalAlign.Left;
    //                                                                    FpSpread1.Sheets[0].Cells[rowval, 8].ForeColor = Color.Green;
    //                                                                    for (int st = 0; st < dvbatchalloc.Count; st++)
    //                                                                    {
    //                                                                        string stubatch = dvbatchalloc[st]["Stu_Batch"].ToString();
    //                                                                        string stucount = dvbatchalloc[st]["stucount"].ToString();
    //                                                                        if (rowval + st > FpSpread1.Sheets[0].RowCount - 1)
    //                                                                        {
    //                                                                            FpSpread1.Sheets[0].RowCount++;
    //                                                                        }
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 0].Text = srno.ToString();
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 0].HorizontalAlign = HorizontalAlign.Center;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 1].Text = batchyear;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 2].Text = course;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 3].Text = department;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 4].Text = sem;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 5].Text = section;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 6].Text = strsemschedule;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 7].Text = sdate;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 6].HorizontalAlign = HorizontalAlign.Left;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 6].ForeColor = Color.Green;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 7].HorizontalAlign = HorizontalAlign.Left;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 7].ForeColor = Color.Green;

    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 8].Text = "Y";
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 8].HorizontalAlign = HorizontalAlign.Center;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 8].ForeColor = Color.Green;

    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 9].Text = stubatch;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 9].HorizontalAlign = HorizontalAlign.Left;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 9].ForeColor = Color.Green;

    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 10].Text = stucount;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 10].HorizontalAlign = HorizontalAlign.Center;
    //                                                                        FpSpread1.Sheets[0].Cells[rowval + st, 10].ForeColor = Color.Green;

    //                                                                    }
    //                                                                }
    //                                                            }
    //                                                            else
    //                                                            {
    //                                                                if (rbbatch.Checked == true || rbtimetable.Checked == true)
    //                                                                {
    //                                                                    if (ddltimetable.SelectedValue.ToString() != "2" && ddlbatchallocation.SelectedValue.ToString() != "1")
    //                                                                    {
    //                                                                        if (rbtimetable.Checked == false || (rbtimetable.Checked == true && ddlbatchallocation.SelectedValue.ToString() == "2"))
    //                                                                        {
    //                                                                            srno++;
    //                                                                            FpSpread1.Sheets[0].RowCount++;
    //                                                                            rowval = FpSpread1.Sheets[0].RowCount - 1;
    //                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
    //                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

    //                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear;
    //                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = course;
    //                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = department;
    //                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sem;
    //                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = section;
    //                                                                            if (rbtimetable.Checked == true)
    //                                                                            {
    //                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = strsemschedule;
    //                                                                                if (strsemschedule.Trim() == "")
    //                                                                                {
    //                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Not Created";
    //                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
    //                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
    //                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Red;
    //                                                                                }
    //                                                                                else
    //                                                                                {
    //                                                                                    batchallocflag = true;
    //                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = sdate;
    //                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
    //                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Green;
    //                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
    //                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Green;
    //                                                                                }
    //                                                                            }
    //                                                                        }
    //                                                                        //}
    //                                                                        //if (ddlbatchallocation.SelectedValue.ToString() != "1")
    //                                                                        //{
    //                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Not Alloted";
    //                                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 8, 1, 3);
    //                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
    //                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.Red;
    //                                                                    }
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                    }
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                if (rbbatch.Checked == true || rbtimetable.Checked == true)
    //                                                {
    //                                                    if (ddltimetable.SelectedValue.ToString() != "1" && ddlbatchallocation.SelectedValue.ToString() != "1")
    //                                                    {
    //                                                        srno++;
    //                                                        FpSpread1.Sheets[0].RowCount++;
    //                                                        rowval = FpSpread1.Sheets[0].RowCount - 1;
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear;
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = course;
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = department;
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sem;
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = section;

    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Not Generated";
    //                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 6, 1, 2);
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Red;

    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Not Alloted";
    //                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 8, 1, 3);
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
    //                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.Red;
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                        if (chkles.Checked == true)
    //                                        {
    //                                            //Lession Planner
    //                                            int getno = -1;

    //                                            for (int s = 0; s < dvsubject.Count; s++)
    //                                            {
    //                                                recflag = true;
    //                                                Boolean lessionfalag = false;
    //                                                string subjectname = dvsubject[s]["subject_name"].ToString();
    //                                                string subcode = dvsubject[s]["subject_code"].ToString();
    //                                                string subno = dvsubject[s]["subject_no"].ToString();
    //                                                dssubunit.Tables[0].DefaultView.RowFilter = "subject_no='" + subno + "'  ";
    //                                                DataView dvsubunit = dssubunit.Tables[0].DefaultView;

    //                                                dssstaff.Tables[0].DefaultView.RowFilter = "subject_no='" + subno + "' " + sectval + "";
    //                                                DataView dvstaff = dssstaff.Tables[0].DefaultView;
    //                                                string staffname = "";
    //                                                string staffcode = "";
    //                                                for (int sst = 0; sst < dvstaff.Count; sst++)
    //                                                {
    //                                                    if (staffcode.Trim() == "")
    //                                                    {
    //                                                        staffname = dvstaff[sst]["staff_name"].ToString();
    //                                                        staffcode = dvstaff[sst]["staff_code"].ToString();
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        staffname = staffname + ", " + dvstaff[sst]["staff_name"].ToString();
    //                                                        staffcode = staffcode + ", " + dvstaff[sst]["staff_code"].ToString();
    //                                                    }
    //                                                }

    //                                                if (dvsubunit.Count > 0)
    //                                                {
    //                                                    dslession.Tables[0].DefaultView.RowFilter = "subject_no='" + subno + "' " + sectval + "";
    //                                                    DataView dvlession = dslession.Tables[0].DefaultView;
    //                                                    if (dvlession.Count > 0)
    //                                                    {
    //                                                        lessionfalag = true;
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        lessionfalag = false;
    //                                                    }
    //                                                }
    //                                                Boolean lsetflag = false;
    //                                                if (ddllession.SelectedValue.ToString() == "1")
    //                                                {
    //                                                    if (lessionfalag == true)
    //                                                    {
    //                                                        lsetflag = true;
    //                                                    }
    //                                                }
    //                                                else if (ddllession.SelectedValue.ToString() == "2")
    //                                                {
    //                                                    if (lessionfalag == false)
    //                                                    {
    //                                                        lsetflag = true;
    //                                                    }
    //                                                }
    //                                                else
    //                                                {
    //                                                    lsetflag = true;
    //                                                }
    //                                                if (lsetflag == true)
    //                                                {

    //                                                    if (getno == -1)
    //                                                    {
    //                                                        getno = 0;
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        getno++;
    //                                                    }
    //                                                    if ((rowval + getno) > (FpSpread1.Sheets[0].RowCount - 1) || rowval == -1)
    //                                                    {
    //                                                        FpSpread1.Sheets[0].RowCount++;
    //                                                        if (rowval == -1 && getno == 0)
    //                                                        {
    //                                                            srno++;
    //                                                            rowval = FpSpread1.Sheets[0].RowCount - 1;
    //                                                        }
    //                                                    }

    //                                                    FpSpread1.Sheets[0].Cells[rowval + getno, 0].Text = srno.ToString();
    //                                                    FpSpread1.Sheets[0].Cells[rowval + getno, 0].HorizontalAlign = HorizontalAlign.Center;

    //                                                    FpSpread1.Sheets[0].Cells[rowval + getno, 1].Text = batchyear;
    //                                                    FpSpread1.Sheets[0].Cells[rowval + getno, 2].Text = course;
    //                                                    FpSpread1.Sheets[0].Cells[rowval + getno, 3].Text = department;
    //                                                    FpSpread1.Sheets[0].Cells[rowval + getno, 4].Text = sem;
    //                                                    FpSpread1.Sheets[0].Cells[rowval + getno, 5].Text = section;
    //                                                    if (lessionfalag == true)
    //                                                    {
    //                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 11].Text = "Y";
    //                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 11].HorizontalAlign = HorizontalAlign.Center;
    //                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 11].ForeColor = Color.Green;
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 11].Text = "N";
    //                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 11].HorizontalAlign = HorizontalAlign.Center;
    //                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 11].ForeColor = Color.Red;
    //                                                    }
    //                                                    FpSpread1.Sheets[0].Cells[rowval + getno, 12].Text = subcode;
    //                                                    FpSpread1.Sheets[0].Cells[rowval + getno, 13].Text = subjectname;
    //                                                    FpSpread1.Sheets[0].Cells[rowval + getno, 14].Text = staffcode;
    //                                                    FpSpread1.Sheets[0].Cells[rowval + getno, 15].Text = staffname;
    //                                                }
    //                                            }

    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        //if ((rowval > (FpSpread1.Sheets[0].RowCount - 1) || rowval == -1))
    //                                        //{
    //                                        //    FpSpread1.Sheets[0].RowCount++;
    //                                        //    if (rowval == -1)
    //                                        //    {
    //                                        //        rowval = FpSpread1.Sheets[0].RowCount - 1;
    //                                        //    }
    //                                        //}
    //                                        //if (rbtimetable.Checked == true)
    //                                        //{
    //                                        //    FpSpread1.Sheets[0].Cells[rowval, 6].Text = "No Subjects Created";
    //                                        //    FpSpread1.Sheets[0].Cells[rowval, 6].ForeColor = Color.Red;
    //                                        //    FpSpread1.Sheets[0].Cells[rowval, 6].HorizontalAlign = HorizontalAlign.Center;
    //                                        //    FpSpread1.Sheets[0].SpanModel.Add(rowval, 6, 1, 10);
    //                                        //}
    //                                        //if (rbbatch.Checked == true && rbtimetable.Checked == false)
    //                                        //{
    //                                        //    FpSpread1.Sheets[0].Cells[rowval, 8].Text = "No Subjects Created";
    //                                        //    FpSpread1.Sheets[0].Cells[rowval, 8].ForeColor = Color.Red;
    //                                        //    FpSpread1.Sheets[0].Cells[rowval, 8].HorizontalAlign = HorizontalAlign.Center;
    //                                        //    FpSpread1.Sheets[0].SpanModel.Add(rowval, 8, 1, 10);
    //                                        //}
    //                                        //FpSpread1.Sheets[0].Cells[rowval, 0].Text = srno.ToString();
    //                                        //FpSpread1.Sheets[0].Cells[rowval, 0].HorizontalAlign = HorizontalAlign.Center;
    //                                        //FpSpread1.Sheets[0].Cells[rowval, 1].Text = batchyear;
    //                                        //FpSpread1.Sheets[0].Cells[rowval, 2].Text = course;
    //                                        //FpSpread1.Sheets[0].Cells[rowval, 3].Text = department;
    //                                        //FpSpread1.Sheets[0].Cells[rowval, 4].Text = sem;
    //                                        //FpSpread1.Sheets[0].Cells[rowval, 5].Text = section;
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        if (FpSpread1.Sheets[0].RowCount > 0)
    //        {
    //            FpSpread1.Visible = true;
    //            lblrptname.Visible = true;
    //            txtexcelname.Visible = true;
    //            btnxl.Visible = true;
    //            btnmasterprint.Visible = true;
    //        }
    //        else
    //        {
    //            lbl_err.Visible = true;
    //            lbl_err.Text = "No Records Found";
    //        }
    //        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
    //    }
    //    catch (Exception ex)
    //    {
    //        lbl_err.Visible = true;
    //        lbl_err.Text = ex.ToString();
    //        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
    //    }
    //}
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean recflag = false;
            clear();
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

            string strsem = "";
            for (int j = 0; j < chklssem.Items.Count; j++)
            {
                if (chklssem.Items[j].Selected == true)
                {
                    if (strsem == "")
                    {
                        strsem = "'" + chklssem.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        strsem = strsem + ",'" + chklssem.Items[j].Value.ToString() + "'";
                    }
                }
            }
            if (strsem.Trim() == "")
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Semester And Then Proceed";
                return;
            }



            string strsec = "";
            for (int j = 0; j < chklssec.Items.Count; j++)
            {
                if (chklssec.Items[j].Selected == true)
                {
                    if (strsec == "")
                    {
                        if (chklssec.Items[j].Text == "Empty Section")
                        {
                            strsec = "''";
                        }
                        else
                        {
                            strsec = "'" + chklssec.Items[j].Value.ToString() + "'";
                        }
                    }
                    else
                    {
                        if (chklssec.Items[j].Text == "Empty Section")
                        {
                            strsec = strsec + ",''";
                        }
                        else
                        {
                            strsec = strsec + ",'" + chklssec.Items[j].Value.ToString() + "'";
                        }
                    }
                }
            }
            if (strsec.Trim() != "")
            {
                strsec = "  and ISNULL(r.Sections,'') in(" + strsec + ")";  //modified by mullai
            }

            //if (rbtimetable.Checked == false && rbbatch.Checked == false && chkles.Checked == false)
            //{
            //    lbl_err.Visible = true;
            //    lbl_err.Text = "Please Select Anyone Report to be Display";
            //    return;
            //}

            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.Sheets[0].SheetCorner.RowCount = 1;
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpSpread1.Sheets[0].AllowTableCorner = true;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;

            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].ColumnCount = 16;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[0].Width = 30;
            if (chklscolumn.Items[0].Selected == true)
            {
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                recflag = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[0].Visible = false;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].Width = 30;
            if (chklscolumn.Items[1].Selected == true)
            {
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                recflag = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[1].Visible = false;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[2].Width = 80;
            if (chklscolumn.Items[2].Selected == true)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                recflag = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Branch";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].Width = 100;
            if (chklscolumn.Items[3].Selected == true)
            {
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                recflag = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[3].Visible = false;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sem";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[4].Width = 30;
            if (chklscolumn.Items[4].Selected == true)
            {
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                recflag = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[4].Visible = false;
            }
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Section";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[5].Width = 30;
            if (chklscolumn.Items[5].Selected == true)
            {
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                recflag = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[5].Visible = false;
            }

            //if (rbtimetable.Checked == true)
            if (rbtimetable.Checked == true || rbbatch.Checked == true)
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Name";
                FpSpread1.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[6].Width = 100;
                if (chklscolumn.Items[6].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[6].Visible = true;
                    recflag = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Time Table";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 2);
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[6].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Start Date";
                FpSpread1.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[7].Width = 80;

                if (chklscolumn.Items[7].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[7].Visible = true;
                    recflag = true;
                    if (chklscolumn.Items[6].Selected == false)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Time Table";
                    }
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[7].Visible = false;
                }
            }
            else
            {
                FpSpread1.Sheets[0].Columns[6].Visible = false;
                FpSpread1.Sheets[0].Columns[7].Visible = false;
            }

            //if (rbbatch.Checked == true)
            if (rbbatch.Checked == true)
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Status";
                FpSpread1.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[0].Width = 30;

                if (chklscolumn.Items[8].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[8].Visible = true;
                    recflag = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Batch Allocation";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 3);
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[8].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Student Batch";
                FpSpread1.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[9].Width = 30;
                if (chklscolumn.Items[9].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[9].Visible = true;
                    recflag = true;
                    if (chklscolumn.Items[8].Selected == false)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Batch Allocation";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 1, 2);
                    }
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[9].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Text = "Student Count";
                FpSpread1.Sheets[0].Columns[10].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[10].Width = 30;

                if (chklscolumn.Items[10].Selected == true)
                {
                    FpSpread1.Sheets[0].Columns[10].Visible = true;
                    recflag = true;
                    if (chklscolumn.Items[8].Selected == false && chklscolumn.Items[9].Selected == false)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Batch Allocation";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 1, 1);
                    }
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[10].Visible = false;
                }
            }
            else
            {
                FpSpread1.Sheets[0].Columns[8].Visible = false;
                FpSpread1.Sheets[0].Columns[9].Visible = false;
                FpSpread1.Sheets[0].Columns[10].Visible = false;
            }


            //  if (chkles.Checked == true)
            if (rblession.Checked == true)
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Text = "Status";
                FpSpread1.Sheets[0].Columns[11].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[11].Width = 30;
                if (chklscolumn.Items[11].Selected == true)
                {
                    recflag = true;
                    FpSpread1.Sheets[0].Columns[11].Visible = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Lesson Planner";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 1, 5);
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[11].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Text = "Subject Code";
                FpSpread1.Sheets[0].Columns[12].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[12].Width = 80;

                if (chklscolumn.Items[12].Selected == true)
                {
                    recflag = true;
                    FpSpread1.Sheets[0].Columns[12].Visible = true;
                    if (chklscolumn.Items[11].Selected == false)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Lesson Planner";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 1, 4);
                    }
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[12].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].Text = "Subject Name";
                FpSpread1.Sheets[0].Columns[13].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[13].Width = 200;

                if (chklscolumn.Items[13].Selected == true)
                {
                    recflag = true;
                    FpSpread1.Sheets[0].Columns[13].Visible = true;
                    if (chklscolumn.Items[11].Selected == false && chklscolumn.Items[12].Selected == false)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Lesson Planner";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 13, 1, 3);
                    }
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[13].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 14].Text = "Staff Code";
                FpSpread1.Sheets[0].Columns[14].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[14].Width = 80;

                if (chklscolumn.Items[14].Selected == true)
                {
                    recflag = true;
                    FpSpread1.Sheets[0].Columns[13].Visible = true;
                    if (chklscolumn.Items[11].Selected == false && chklscolumn.Items[12].Selected == false && chklscolumn.Items[13].Selected == false)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Lesson Planner";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 14, 1, 2);
                    }
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[14].Visible = false;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 15].Text = "Staff Name";
                FpSpread1.Sheets[0].Columns[15].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[15].Width = 150;

                if (chklscolumn.Items[15].Selected == true)
                {
                    recflag = true;
                    FpSpread1.Sheets[0].Columns[15].Visible = true;
                    if (chklscolumn.Items[11].Selected == false && chklscolumn.Items[12].Selected == false && chklscolumn.Items[13].Selected == false && chklscolumn.Items[14].Selected == false)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Lesson Planner";
                    }
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[15].Visible = false;
                }
            }
            else
            {
                FpSpread1.Sheets[0].Columns[11].Visible = false;
                FpSpread1.Sheets[0].Columns[12].Visible = false;
                FpSpread1.Sheets[0].Columns[13].Visible = false;
                FpSpread1.Sheets[0].Columns[14].Visible = false;
                FpSpread1.Sheets[0].Columns[15].Visible = false;
            }

            if (recflag == false)
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Column Order And Then Proceed";
                return;
            }
            recflag = false;
            FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.White;
            style2.BackColor = System.Drawing.Color.Teal;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            string currrstatus = "";
            if (chkcurrent.Checked == true)
            {
                currrstatus = " and r.Current_Semester=s.semester and r.cc=0 and r.exam_flag<>'debar' and r.delflag=0";
            }

            string strdegreequery = "select distinct r.Batch_Year,c.Course_Name,de.Dept_Name,d.Course_Id,r.degree_code,s.semester,ISNULL(r.Sections,'') as sections  from seminfo s,Registration r,Degree d,Course c,Department de where s.batch_year=r.Batch_Year and s.degree_code=r.degree_code and d.Degree_Code=r.degree_code and s.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id " + currrstatus + " " + strsec + " order by d.Course_Id,r.degree_code,r.Batch_Year desc,s.semester,sections";  //modified by mullai
            DataSet dsdegreedetails = da.select_method_wo_parameter(strdegreequery, "Text");

            string strsemesterscehduelquery = "select batch_year,degree_code,semester,Sections,TTName,fromdate,convert(nvarchar(15),fromdate,103) as sdate from Semester_Schedule order by fromdate";
            DataSet dssemscehdule = da.select_method_wo_parameter(strsemesterscehduelquery, "Text");

            string strbatchallocquery = "select count(distinct sc.roll_no) as stucount,l.batch_year,l.degree_code,l.semester,l.Sections,l.Timetablename,l.Stu_Batch,l.fromdate from subjectChooser sc,Registration r,LabAlloc l where l.Subject_No=sc.subject_no and r.Batch_Year=l.Batch_Year and l.Degree_Code=r.degree_code and r.Roll_No=sc.roll_no and l.Sections=r.Sections and l.Subject_No=sc.subject_no and l.Stu_Batch=sc.Batch and l.Semester=sc.semester and sc.Batch=l.Stu_Batch and isnull(sc.Batch,'')<>'' and isnull(l.Stu_Batch,'')<>'' group by l.batch_year,l.degree_code,l.semester,l.Sections,l.Timetablename,l.Stu_Batch,l.fromdate";
            DataSet dsbatchalloc = da.select_method_wo_parameter(strbatchallocquery, "Text");

            string strsubjectquery = "select sy.Batch_Year,sy.degree_code,sy.semester,s.subject_no,s.subject_name,s.subject_code from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=ss.syll_code and ss.syll_code=s.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no";
            DataSet dssubject = da.select_method_wo_parameter(strsubjectquery, "Text");

            string strsubuint = "select distinct subject_no from sub_unit_details";
            DataSet dssubunit = da.select_method_wo_parameter(strsubuint, "Text");

            string strlessionquery = "select distinct subject_no,l.Sections from lesson_plan l,lessonPlanTopics lp where l.LP_code=lp.LP_code";
            DataSet dslession = da.select_method_wo_parameter(strlessionquery, "Text");

            string strstaffsubject = "select st.staff_code,st.staff_name,ss.subject_no,ss.Sections from staffmaster st,staff_selector ss where st.staff_code=ss.staff_code";
            DataSet dssstaff = da.select_method_wo_parameter(strstaffsubject, "Text");

            int srno = 0;

            for (int b = 0; b < Chklst_batch.Items.Count; b++)
            {
                if (Chklst_batch.Items[b].Selected == true)
                {
                    string batchyear = Chklst_batch.Items[b].Value.ToString();

                    for (int br = 0; br < chklst_branch.Items.Count; br++)
                    {
                        if (chklst_branch.Items[br].Selected == true)
                        {
                            string degreecode = chklst_branch.Items[br].Value.ToString();
                            for (int j = 0; j < chklssem.Items.Count; j++)
                            {
                                if (chklssem.Items[j].Selected == true)
                                {
                                    string sem = chklssem.Items[j].Text.ToString();
                                    dsdegreedetails.Tables[0].DefaultView.RowFilter = "batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem.ToString() + "'";
                                    DataView dvdegree = dsdegreedetails.Tables[0].DefaultView;
                                    for (int i = 0; i < dvdegree.Count; i++)
                                    {

                                        int rowval = -1;
                                        string course = dvdegree[i]["Course_Name"].ToString();
                                        string department = dvdegree[i]["Dept_Name"].ToString();
                                        string section = dvdegree[i]["Sections"].ToString();
                                        string sectval = "";

                                        if (section.Trim() != "" && section.Trim() != "-1")
                                        {
                                            sectval = " and sections='" + section + "'";
                                        }
                                        else
                                        {
                                            section = " ";
                                        }
                                        dssubject.Tables[0].DefaultView.RowFilter = "batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem.ToString() + "'";
                                        DataView dvsubject = dssubject.Tables[0].DefaultView;
                                        if (dvsubject.Count > 0)
                                        {
                                            dssemscehdule.Tables[0].DefaultView.RowFilter = "batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem.ToString() + "' " + sectval + "";
                                            DataView dvsemschedule = dssemscehdule.Tables[0].DefaultView;

                                            if (dvsemschedule.Count > 0)
                                            {
                                                recflag = true;
                                                for (int s = 0; s < dvsemschedule.Count; s++)
                                                {
                                                    string strsemschedule = dvsemschedule[s]["TTName"].ToString();
                                                    string sdate = dvsemschedule[s]["sdate"].ToString();
                                                    string getdate = dvsemschedule[s]["fromdate"].ToString();
                                                    if (rbtimetable.Checked == true)
                                                    {
                                                        if (ddltimetable.SelectedValue.ToString() != "2")
                                                        {
                                                            srno++;
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            rowval = FpSpread1.Sheets[0].RowCount - 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = course;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = department;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sem;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = section;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = strsemschedule;
                                                            if (strsemschedule.Trim() == "")
                                                            {
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Not Created";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Red;
                                                            }
                                                            else
                                                            {
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = sdate;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Green;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Green;
                                                            }
                                                        }
                                                    }

                                                    if (rbbatch.Checked == true)
                                                    {
                                                        dsbatchalloc.Tables[0].DefaultView.RowFilter = "batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem.ToString() + "' and Timetablename='" + strsemschedule.Trim() + "' " + sectval + " and fromdate='" + getdate + "'";
                                                        DataView dvbatchalloc = dsbatchalloc.Tables[0].DefaultView;

                                                        if (dvbatchalloc.Count > 0)
                                                        {
                                                            if (ddlbatchallocation.SelectedValue.ToString() != "2")
                                                            {
                                                                srno++;
                                                                FpSpread1.Sheets[0].RowCount++;
                                                                rowval = FpSpread1.Sheets[0].RowCount - 1;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = course;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = department;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sem;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = section;

                                                                recflag = true;
                                                                FpSpread1.Sheets[0].Cells[rowval, 8].Text = "Y";
                                                                FpSpread1.Sheets[0].Cells[rowval, 8].HorizontalAlign = HorizontalAlign.Left;
                                                                FpSpread1.Sheets[0].Cells[rowval, 8].ForeColor = Color.Green;
                                                                for (int st = 0; st < dvbatchalloc.Count; st++)
                                                                {
                                                                    string stubatch = dvbatchalloc[st]["Stu_Batch"].ToString();
                                                                    string stucount = dvbatchalloc[st]["stucount"].ToString();
                                                                    if (rowval + st > FpSpread1.Sheets[0].RowCount - 1)
                                                                    {
                                                                        FpSpread1.Sheets[0].RowCount++;
                                                                    }
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 0].Text = srno.ToString();
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 1].Text = batchyear;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 2].Text = course;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 3].Text = department;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 4].Text = sem;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 5].Text = section;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 6].Text = strsemschedule;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 7].Text = sdate;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 6].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 6].ForeColor = Color.Green;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 7].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 7].ForeColor = Color.Green;

                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 8].Text = "Y";
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 8].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 8].ForeColor = Color.Green;

                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 9].Text = stubatch;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 9].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 9].ForeColor = Color.Green;

                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 10].Text = stucount;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 10].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[rowval + st, 10].ForeColor = Color.Green;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (ddlbatchallocation.SelectedValue.ToString() != "1")
                                                            {
                                                                srno++;
                                                                FpSpread1.Sheets[0].RowCount++;
                                                                rowval = FpSpread1.Sheets[0].RowCount - 1;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = course;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = department;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sem;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = section;

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = strsemschedule;
                                                                if (strsemschedule.Trim() == "")
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Not Created";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Red;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = sdate;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Green;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Green;
                                                                }
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Not Alloted";
                                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 8, 1, 3);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.Red;
                                                            }
                                                        }
                                                    }
                                                    //else
                                                    //{
                                                    //    if ((rbtimetable.Checked == true && ddltimetable.SelectedValue.ToString() != "1") || (rbbatch.Checked == true && ddlbatchallocation.SelectedValue.ToString() != "1"))
                                                    //    {
                                                    //        srno++;
                                                    //        FpSpread1.Sheets[0].RowCount++;
                                                    //        rowval = FpSpread1.Sheets[0].RowCount - 1;
                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear;
                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = course;
                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = department;
                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sem;
                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = section;

                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = strsemschedule;
                                                    //        if (strsemschedule.Trim() == "")
                                                    //        {
                                                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Not Created";
                                                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Red;
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = sdate;
                                                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Green;
                                                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Green;
                                                    //        }

                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Not Alloted";
                                                    //        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 8, 1, 3);
                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.Red;
                                                    //    }
                                                    //}
                                                }
                                            }
                                            else
                                            {
                                                if ((rbtimetable.Checked == true && ddltimetable.SelectedValue.ToString() != "1") || (rbbatch.Checked == true && ddlbatchallocation.SelectedValue.ToString() != "1"))
                                                {
                                                    srno++;
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    rowval = FpSpread1.Sheets[0].RowCount - 1;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = course;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = department;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sem;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = section;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Not Generated";
                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 6, 1, 2);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Red;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Not Alloted";
                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 8, 1, 3);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.Red;
                                                }
                                            }
                                            if (rblession.Checked == true)
                                            {
                                                int getno = -1;

                                                for (int s = 0; s < dvsubject.Count; s++)
                                                {
                                                    recflag = true;
                                                    Boolean lessionfalag = false;
                                                    string subjectname = dvsubject[s]["subject_name"].ToString();
                                                    string subcode = dvsubject[s]["subject_code"].ToString();
                                                    string subno = dvsubject[s]["subject_no"].ToString();
                                                    dssubunit.Tables[0].DefaultView.RowFilter = "subject_no='" + subno + "'  ";
                                                    DataView dvsubunit = dssubunit.Tables[0].DefaultView;

                                                    dssstaff.Tables[0].DefaultView.RowFilter = "subject_no='" + subno + "' " + sectval + "";
                                                    DataView dvstaff = dssstaff.Tables[0].DefaultView;
                                                    string staffname = "";
                                                    string staffcode = "";
                                                    for (int sst = 0; sst < dvstaff.Count; sst++)
                                                    {
                                                        if (staffcode.Trim() == "")
                                                        {
                                                            staffname = dvstaff[sst]["staff_name"].ToString();
                                                            staffcode = dvstaff[sst]["staff_code"].ToString();
                                                        }
                                                        else
                                                        {
                                                            staffname = staffname + ", " + dvstaff[sst]["staff_name"].ToString();
                                                            staffcode = staffcode + ", " + dvstaff[sst]["staff_code"].ToString();
                                                        }
                                                    }

                                                    if (dvsubunit.Count > 0)
                                                    {
                                                        dslession.Tables[0].DefaultView.RowFilter = "subject_no='" + subno + "' " + sectval + "";
                                                        DataView dvlession = dslession.Tables[0].DefaultView;
                                                        if (dvlession.Count > 0)
                                                        {
                                                            lessionfalag = true;
                                                        }
                                                        else
                                                        {
                                                            lessionfalag = false;
                                                        }
                                                    }
                                                    Boolean lsetflag = false;
                                                    if (ddllession.SelectedValue.ToString() == "1")
                                                    {
                                                        if (lessionfalag == true)
                                                        {
                                                            lsetflag = true;
                                                        }
                                                    }
                                                    else if (ddllession.SelectedValue.ToString() == "2")
                                                    {
                                                        if (lessionfalag == false)
                                                        {
                                                            lsetflag = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lsetflag = true;
                                                    }
                                                    if (lsetflag == true)
                                                    {

                                                        if (getno == -1)
                                                        {
                                                            getno = 0;
                                                        }
                                                        else
                                                        {
                                                            getno++;
                                                        }
                                                        if ((rowval + getno) > (FpSpread1.Sheets[0].RowCount - 1) || rowval == -1)
                                                        {
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            if (rowval == -1 && getno == 0)
                                                            {
                                                                srno++;
                                                                rowval = FpSpread1.Sheets[0].RowCount - 1;
                                                            }
                                                        }

                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 0].Text = srno.ToString();
                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 0].HorizontalAlign = HorizontalAlign.Center;

                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 1].Text = batchyear;
                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 2].Text = course;
                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 3].Text = department;
                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 4].Text = sem;
                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 5].Text = section;
                                                        if (lessionfalag == true)
                                                        {
                                                            FpSpread1.Sheets[0].Cells[rowval + getno, 11].Text = "Planned";
                                                            FpSpread1.Sheets[0].Cells[rowval + getno, 11].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[rowval + getno, 11].ForeColor = Color.Green;
                                                        }
                                                        else
                                                        {
                                                            FpSpread1.Sheets[0].Cells[rowval + getno, 11].Text = "Not Planned";
                                                            FpSpread1.Sheets[0].Cells[rowval + getno, 11].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[rowval + getno, 11].ForeColor = Color.Red;
                                                        }

                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 12].Text = subcode;
                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 13].Text = subjectname;
                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 14].Text = staffcode;
                                                        FpSpread1.Sheets[0].Cells[rowval + getno, 15].Text = staffname;
                                                        if (staffcode.Trim() == "")
                                                        {
                                                            FpSpread1.Sheets[0].Cells[rowval + getno, 14].Text = "No Staff Alloted";
                                                            FpSpread1.Sheets[0].Cells[rowval + getno, 14].ForeColor = Color.Red;
                                                            FpSpread1.Sheets[0].SpanModel.Add(rowval + getno, 14, 1, 2);
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            if ((rbtimetable.Checked == true && ddltimetable.SelectedValue.ToString() != "1") || (rbbatch.Checked == true && ddlbatchallocation.SelectedValue.ToString() != "1") || (rblession.Checked == true && ddllession.SelectedValue.ToString() != "1"))
                                            {
                                                srno++;
                                                FpSpread1.Sheets[0].RowCount++;
                                                rowval = FpSpread1.Sheets[0].RowCount - 1;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = course;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = department;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sem;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = section;

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Not Generated";
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 6, 1, 2);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Red;

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Not Alloted";
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 8, 1, 3);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.Red;

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = "Not Planned";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].ForeColor = Color.Red;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 11, 1, 5);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                FpSpread1.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnmasterprint.Visible = true;
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "No Records Found";
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            int rows = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Height = (rows + 20) * 100;
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                lbl_err.Text = "Please Enter Your Report Name";
                lbl_err.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Text = ex.ToString();
            lbl_err.Visible = true;
        }
    }
    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.loadspreaddetails(FpSpread1, "TimeTableBlackBox.aspx", "Black Box 2");
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            lbl_err.Text = ex.ToString();
            lbl_err.Visible = true;
        }
    }
}
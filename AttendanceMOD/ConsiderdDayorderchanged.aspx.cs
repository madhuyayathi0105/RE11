using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using System.Collections.Generic;
using System.Text;
using InsproDataAccess;





public partial class ConsiderdDayorderchanged : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    DataTable dtCommon = new DataTable();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    ReuasableMethods rs = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Boolean Cellclick = false;
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    protected void Page_Load(object sender, EventArgs e)
    {
        // lblnorec.Visible = false;
        lblerrmsg.Visible = false;
        lblreasonerr.Visible = false;
        if (!IsPostBack)
        {
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            txtfromdateadd.Attributes.Add("readonly", "readonly");
            txttodateadd.Attributes.Add("readonly", "readonly");

            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            FpSpread1.Visible = false;
            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            chkConsiderDayOrder.Checked = false;
            divAlternateDayOrder.Visible = false;
            txtfromdate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtfromdateadd.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txttodateadd.Text = DateTime.Today.ToString("dd/MM/yyyy");

            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.CommandBar.Visible = true;
            FpSpread1.Sheets[0].SheetName = " ";
            FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = Color.Black;
            style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].AllowTableCorner = true;
            FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            FpSpread1.Pager.Align = HorizontalAlign.Right;
            FpSpread1.Pager.Font.Bold = true;
            FpSpread1.Pager.Font.Name = "Book Antiqua";
            FpSpread1.Pager.ForeColor = Color.DarkGreen;
            FpSpread1.Pager.BackColor = Color.Beige;
            FpSpread1.Pager.BackColor = Color.AliceBlue;
            FpSpread1.Pager.PageCount = 5;
            FpSpread1.CommandBar.Visible = false;
            BindBatch();
            BindDegree();
            bindbranch();
            ddlreason.Attributes.Add("onfocus", "frelig()");
            bindspread();
            Bindcollege();
            loadreason();
            panelreason.Visible = false;
        }
    }

    public void BindBatch()
    {
        try
        {
            chklsbatch.Items.Clear();
            chklsbatchadd.Items.Clear();
            string batch = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc='0' and DelFlag='0' and Exam_Flag<>'Debar' order by batch_year";
            ds = d2.select_method_wo_parameter(batch, "Text");
           // ds = ClsAttendanceAccess.GetBatchDetail();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();

                chklsbatchadd.DataSource = ds;
                chklsbatchadd.DataTextField = "Batch_year";
                chklsbatchadd.DataValueField = "Batch_year";
                chklsbatchadd.DataBind();
            }
        }
        catch
        {
        }
    }
    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;
                ddlcoll.DataSource = dtCommon;
                ddlcoll.DataTextField = "collname";
                ddlcoll.DataValueField = "college_code";
                ddlcoll.DataBind();
                ddlcoll.SelectedIndex = 0;
                ddlcoll.Enabled = true;
            }
        }
        catch (Exception ex)
        {

        }
    }
    //public void Bindcollege()
    //{
    //    try
    //    {
           
    //        DataSet dsprint = new DataSet();
    //        dsprint.Clear();
    //        string qryUserCodeOrGroupCode = string.Empty;
    //        string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
    //        if (group_user.Contains(';'))
    //        {
    //            string[] group_semi = group_user.Split(';');
    //            group_user = Convert.ToString(group_semi[0]);
    //        }
    //        if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
    //        {
    //            qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
    //        }
    //        else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
    //        {
    //            qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
    //        }
    //        if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
    //        {
    //            dicQueryParameter.Clear();
    //            dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
    //            dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
    //        }
    //        if (dtCommon.Rows.Count > 0)
    //        {
    //            Cblcollege.DataSource = dtCommon;
    //            Cblcollege.DataTextField = "collname";
    //            Cblcollege.DataValueField = "college_code";
    //            Cblcollege.DataBind();
    //           // Cblcollege.SelectedIndex = 0;
    //            Cblcollege.Enabled = true;
    //            Cblcoll.DataSource = dtCommon;
    //            Cblcoll.DataTextField = "collname";
    //            Cblcoll.DataValueField = "college_code";
    //            Cblcoll.DataBind();
    //            // Cblcollege.SelectedIndex = 0;
    //            Cblcoll.Enabled = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    //public void BindDegree()
    //{
    //    try
    //    {
    //        txtdegree.Text = "---Select---";
    //        txtdegreeadd.Text = "---Select---";
    //        chklsdegree.Items.Clear();
    //        chklsdegreeadd.Items.Clear();
    //        chkdegree.Checked = false;
    //        chkdegreeadd.Checked = false;
    //        usercode = Session["usercode"].ToString();
    //        //collegecode = Session["collegecode"].ToString();
    //        collegecode=
    //        singleuser = Session["single_user"].ToString();
    //        group_user = Session["group_code"].ToString();
    //        if (group_user.Contains(';'))
    //        {
    //            string[] group_semi = group_user.Split(';');
    //            group_user = group_semi[0].ToString();
    //        }
    //        hat.Clear();
    //        hat.Add("single_user", singleuser);
    //        hat.Add("group_code", group_user);
    //        hat.Add("college_code", collegecode);
    //        hat.Add("user_code", usercode);
    //        ds = d2.select_method("bind_degree", hat, "sp");
    //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            chklsdegree.DataSource = ds;
    //            chklsdegree.DataTextField = "course_name";
    //            chklsdegree.DataValueField = "course_id";
    //            chklsdegree.DataBind();

    //            chklsdegreeadd.DataSource = ds;
    //            chklsdegreeadd.DataTextField = "course_name";
    //            chklsdegreeadd.DataValueField = "course_id";
    //            chklsdegreeadd.DataBind();
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    //public void bindbranchadd()
    //{
    //    try
    //    {
    //        txtbranchadd.Text = "---Select---";
    //        chkbranchadd.Checked = false;
    //        chklsbranchadd.Items.Clear();
    //        hat.Clear();
    //        usercode = Session["usercode"].ToString();
    //        collegecode = Session["collegecode"].ToString();
    //        singleuser = Session["single_user"].ToString();
    //        group_user = Session["group_code"].ToString();
    //        if (group_user.Contains(';'))
    //        {
    //            string[] group_semi = group_user.Split(';');
    //            group_user = group_semi[0].ToString();
    //        }
    //        string courseid = string.Empty;
    //        for (int i = 0; i < chklsdegreeadd.Items.Count; i++)
    //        {
    //            if (chklsdegreeadd.Items[i].Selected == true)
    //            {
    //                if (courseid == "")
    //                {
    //                    courseid = chklsdegreeadd.Items[i].Value;
    //                }
    //                else
    //                {
    //                    courseid = courseid + "," + chklsdegree.Items[i].Value;
    //                }
    //            }
    //        }
    //        ds = d2.BindBranchMultiple(singleuser, group_user, courseid, collegecode, usercode);
    //        int count2 = ds.Tables[0].Rows.Count;
    //        if (count2 > 0)
    //        {
    //            chklsbranchadd.DataSource = ds;
    //            chklsbranchadd.DataTextField = "dept_name";
    //            chklsbranchadd.DataValueField = "degree_code";
    //            chklsbranchadd.DataBind();
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    //public void bindbranch()
    //{
    //    try
    //    {
    //        txtbranch.Text = "---Select---";
    //        chkbranch.Checked = false;
    //        chklsbranch.Items.Clear();
    //        hat.Clear();
    //        usercode = Session["usercode"].ToString();
    //        collegecode = Session["collegecode"].ToString();
    //        singleuser = Session["single_user"].ToString();
    //        group_user = Session["group_code"].ToString();
    //        if (group_user.Contains(';'))
    //        {
    //            string[] group_semi = group_user.Split(';');
    //            group_user = group_semi[0].ToString();
    //        }
    //        string courseid = string.Empty;
    //        for (int i = 0; i < chklsdegree.Items.Count; i++)
    //        {
    //            if (chklsdegree.Items[i].Selected == true)
    //            {
    //                if (courseid == "")
    //                {
    //                    courseid = chklsdegree.Items[i].Value;
    //                }
    //                else
    //                {
    //                    courseid = courseid + "," + chklsdegree.Items[i].Value;
    //                }
    //            }
    //        }
    //        ds = d2.BindBranchMultiple(singleuser, group_user, courseid, collegecode, usercode);
    //        int count2 = ds.Tables[0].Rows.Count;
    //        if (count2 > 0)
    //        {
    //            chklsbranch.DataSource = ds;
    //            chklsbranch.DataTextField = "dept_name";
    //            chklsbranch.DataValueField = "degree_code";
    //            chklsbranch.DataBind();
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    public void loadreason()
    {
        try
        {
            chklsreason.Items.Clear();
            ddlreason.Items.Clear();
            string strquery = "select * from textvaltable where TextCriteria='dayor' and college_code='"+ddlCollege.SelectedValue+"'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsreason.DataSource = ds;
                chklsreason.DataTextField = "TextVal";
                chklsreason.DataValueField = "Textcode";
                chklsreason.DataBind();

                ddlreason.DataSource = ds;
                ddlreason.DataTextField = "TextVal";
                ddlreason.DataValueField = "Textcode";
                ddlreason.DataBind();
            }
        }
        catch { }
    }

    public void clear()
    {
        try
        {
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            Printcontrol.Visible = false;
            FpSpread1.Visible = false;
            txtexcelname.Text = string.Empty;
            btnxl.Visible = false;
        }
        catch
        {
        }
    }

    public void reportdate()
    {
        try
        {
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            Printcontrol.Visible = false;
            FpSpread1.Visible = false;
            txtexcelname.Text = string.Empty;
            btnxl.Visible = false;

            string fdate = txtfromdate.Text.ToString();
            string[] fd = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(fd[1] + '/' + fd[0] + '/' + fd[2]);

            string tdate = txttodate.Text.ToString();
            string[] td = tdate.Split('/');
            DateTime dtt = Convert.ToDateTime(td[1] + '/' + td[0] + '/' + td[2]);
            if (dtt < dtf)
            {
                lblnorec.Visible = true;
                lblnorec.Text = "To Date Must Be Greater Than From Date";
                txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                return;
            }

        }
        catch
        {
            lblnorec.Visible = true;
            lblnorec.Text = "To Date Must Be Greater Than From Date";
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    public void entrydate()
    {
        try
        {
            string fdate = txtfromdateadd.Text.ToString();
            string[] fd = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(fd[1] + '/' + fd[0] + '/' + fd[2]);

            string tdate = txttodateadd.Text.ToString();
            string[] td = tdate.Split('/');
            DateTime dtt = Convert.ToDateTime(td[1] + '/' + td[0] + '/' + td[2]);
            if (dtt < dtf)
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "To Date Must Be Greater Than From Date";
                txtfromdateadd.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodateadd.Text = DateTime.Now.ToString("dd/MM/yyyy");
                return;
            }

        }
        catch
        {
            lblerrmsg.Visible = true;
            lblerrmsg.Text = "To Date Must Be Greater Than From Date";
            txtfromdateadd.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodateadd.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            Boolean val = false;
            txtbatch.Text = "---Select---";
            if (chkbatch.Checked == true)
            {
                val = true;
                txtbatch.Text = "Batch (" + chklsbatch.Items.Count + ")";
            }
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                chklsbatch.Items[i].Selected = val;
            }

            BindDegree1();
            bindbranch1();
            loadreason();
        }
        catch
        {
        }
    }

    protected void chklsbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txtbatch.Text = "---Select---";
            chkbatch.Checked = false;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == chklsbatch.Items.Count)
            {
                txtbatch.Text = "Batch (" + count + ")";
                chkbatch.Checked = true;
            }
            else if (count > 0)
            {
                txtbatch.Text = "Batch (" + count + ")";
                chkbatch.Checked = false;
            }
            BindDegree1();
            bindbranch1();
            loadreason();
        }
        catch
        {
        }
    }

    protected void chkdegree_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            Boolean val = false;
            txtdegree.Text = "---Select---";
            if (chkdegree.Checked == true)
            {
                val = true;
                txtdegree.Text = "Degree (" + chklsdegree.Items.Count + ")";
            }
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                chklsdegree.Items[i].Selected = val;
            }

            bindbranch1();
          
        }
        catch
        {
        }
    }

    protected void chklsdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txtdegree.Text = "---Select---";
            chkdegree.Checked = false;
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                if (chklsdegree.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == chklsdegree.Items.Count)
            {
                txtdegree.Text = "Degree (" + count + ")";
                chkdegree.Checked = true;
            }
            else if (count > 0)
            {
                txtdegree.Text = "Degree (" + count + ")";
                chkdegree.Checked = false;
            }
            bindbranch1();
        }
        catch
        {
        }
    }

    protected void chkbranch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            Boolean val = false;
            txtbranch.Text = "---Select---";
            if (chkbranch.Checked == true)
            {
                val = true;
                txtbranch.Text = "Branch (" + chklsbranch.Items.Count + ")";
            }
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                chklsbranch.Items[i].Selected = val;
            }

        }
        catch
        {
        }
    }

    protected void chklsbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txtbranch.Text = "---Select---";
            chkbranch.Checked = false;
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                if (chklsbranch.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == chklsbranch.Items.Count)
            {
                txtbranch.Text = "Branch (" + count + ")";
                chkbranch.Checked = true;
            }
            else if (count > 0)
            {
                txtbranch.Text = "Branch (" + count + ")";
                chkbranch.Checked = false;
            }
        }
        catch
        {
        }
    }

    protected void chkbatchadd_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            Boolean val = false;
            txtbatchadd.Text = "---Select---";
            if (chkbatchadd.Checked == true)
            {
                val = true;
                txtbatchadd.Text = "Batch (" + chklsbatchadd.Items.Count + ")";
            }
            for (int i = 0; i < chklsbatchadd.Items.Count; i++)
            {
                chklsbatchadd.Items[i].Selected = val;
            }

            BindDegree();
            bindbranchadd();
        }
        catch
        {
        }
    }

    protected void chklsbatchadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txtbatchadd.Text = "---Select---";
            chkbatchadd.Checked = false;
            for (int i = 0; i < chklsbatchadd.Items.Count; i++)
            {
                if (chklsbatchadd.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == chklsbatchadd.Items.Count)
            {
                txtbatchadd.Text = "Batch (" + count + ")";
                chkbatchadd.Checked = true;
            }
            else if (count > 0)
            {
                txtbatchadd.Text = "Batch (" + count + ")";
                chkbatchadd.Checked = false;
            }
            BindDegree();
            bindbranchadd();
        }
        catch
        {
        }
    }

    protected void chkdegreeadd_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            Boolean val = false;
            txtdegreeadd.Text = "---Select---";
            if (chkdegreeadd.Checked == true)
            {
                val = true;
            }
            for (int i = 0; i < chklsdegreeadd.Items.Count; i++)
            {
                chklsdegreeadd.Items[i].Selected = val;
            }
            txtdegreeadd.Text = "Degree (" + chklsdegreeadd.Items.Count + ")";
            bindbranchadd();
        }
        catch
        {
        }
    }

    protected void chklsdegreeadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txtdegreeadd.Text = "---Select---";
            chkdegreeadd.Checked = false;
            for (int i = 0; i < chklsdegreeadd.Items.Count; i++)
            {
                if (chklsdegreeadd.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == chklsdegreeadd.Items.Count)
            {
                txtdegreeadd.Text = "Degree (" + count + ")";
                chkdegreeadd.Checked = true;
            }
            else if (count > 0)
            {
                txtdegreeadd.Text = "Degree (" + count + ")";
                chkdegreeadd.Checked = false;
            }
            bindbranchadd();
        }
        catch
        {
        }
    }

    protected void chkbranchadd_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            Boolean val = false;
            txtbranchadd.Text = "---Select---";
            if (chkbranchadd.Checked == true)
            {
                val = true;
                txtbranchadd.Text = "Branch (" + chklsbranchadd.Items.Count + ")";
            }
            for (int i = 0; i < chklsbranchadd.Items.Count; i++)
            {
                chklsbranchadd.Items[i].Selected = val;
            }

        }
        catch
        {
        }
    }

    protected void chklsbranchadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txtbranchadd.Text = "---Select---";
            chkbranchadd.Checked = false;
            for (int i = 0; i < chklsbranchadd.Items.Count; i++)
            {
                if (chklsbranchadd.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == chklsbranchadd.Items.Count)
            {
                txtbranchadd.Text = "Branch (" + count + ")";
                chkbranchadd.Checked = true;
            }
            else if (count > 0)
            {
                txtbranchadd.Text = "Branch (" + count + ")";
                chkbranchadd.Checked = false;
            }
        }
        catch
        {
        }
    }

    protected void chkreason_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            Boolean val = false;
            txtreason.Text = "---Select---";
            if (chkreason.Checked == true)
            {
                val = true;
                txtreason.Text = "Reason (" + chklsreason.Items.Count + ")";
            }
            for (int i = 0; i < chklsreason.Items.Count; i++)
            {
                chklsreason.Items[i].Selected = val;
            }

        }
        catch
        {
        }
    }

    protected void chklsreason_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txtreason.Text = "---Select---";
            chkreason.Checked = false;
            for (int i = 0; i < chklsreason.Items.Count; i++)
            {
                if (chklsreason.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == chklsreason.Items.Count)
            {
                txtreason.Text = "Reason (" + count + ")";
                chkreason.Checked = true;
            }
            else if (count > 0)
            {
                txtreason.Text = "Reason (" + count + ")";
                chkreason.Checked = false;
            }
        }
        catch
        {
        }
    }

    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        reportdate();
    }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        reportdate();
    }

    protected void txtfromdateadd_TextChanged(object sender, EventArgs e)
    {
        entrydate();
    }

    protected void txttodateadd_TextChanged(object sender, EventArgs e)
    {
        entrydate();
    }

    protected void chkdate_CheckedChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        Printcontrol.loadspreaddetails(FpSpread1, "ConsiderDayOrder.aspx", "Consider Day Order Report");
        Printcontrol.Visible = true;
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
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        bindspread();
    }

    public void bindspread()
    {
        try
        {
            string batch = string.Empty;
            string Degree = string.Empty;
            string reasontext = string.Empty;
            txtexcelname.Text = string.Empty;
            Printcontrol.Visible = false;
            for (int ba = 0; ba < chklsbatch.Items.Count; ba++)
            {
                if (chklsbatch.Items[ba].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = "'" + chklsbatch.Items[ba].Value.ToString() + "'";
                    }
                    else
                    {
                        batch = batch + ",'" + chklsbatch.Items[ba].Value.ToString() + "'";
                    }
                }
            }
            for (int ba = 0; ba < chklsbranch.Items.Count; ba++)
            {
                if (chklsbranch.Items[ba].Selected == true)
                {
                    if (Degree == "")
                    {
                        Degree = "'" + chklsbranch.Items[ba].Value.ToString() + "'";
                    }
                    else
                    {
                        Degree = Degree + ",'" + chklsbranch.Items[ba].Value.ToString() + "'";
                    }
                }
            }

            for (int ba = 0; ba < chklsreason.Items.Count; ba++)
            {
                if (chklsreason.Items[ba].Selected == true)
                {
                    if (reasontext == "")
                    {
                        reasontext = "'" + chklsreason.Items[ba].Text.ToString() + "'";
                    }
                    else
                    {
                        reasontext = reasontext + ",'" + chklsreason.Items[ba].Text.ToString() + "'";
                    }
                }
            }

            string batchvalues = string.Empty;
            string degreevalues = string.Empty;
            string reasonvalues = string.Empty;
            if (batch.Trim() != "")
            {
                batchvalues = " and t.batch_year in(" + batch + ")";
            }
            if (Degree.Trim() != "")
            {
                degreevalues = " and t.degree_code in(" + Degree + ")";
            }
            if (reasontext.Trim() != "")
            {
                reasonvalues = " and t.reason in(" + reasontext + ")";
            }
            string datevalue = string.Empty;
            if (chkdate.Checked == true)
            {
                string fdate = txtfromdate.Text.ToString();
                string[] fd = fdate.Split('/');
                DateTime dtf = Convert.ToDateTime(fd[1] + '/' + fd[0] + '/' + fd[2]);

                string tdate = txttodate.Text.ToString();
                string[] td = tdate.Split('/');
                DateTime dtt = Convert.ToDateTime(td[1] + '/' + td[0] + '/' + td[2]);
                if (dtt < dtf)
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "To Date Must Be Greater Than From Date";
                    clear();
                    return;
                }
                else
                {
                    datevalue = " and ((t.from_date between '" + dtf.ToString("MM/dd/yyyy") + "' and '" + dtt.ToString("MM/dd/yyyy") + "') or  (t.to_date between '" + dtf.ToString("MM/dd/yyyy") + "' and '" + dtt.ToString("MM/dd/yyyy") + "'))";
                }
            }

            string strquery = "select convert(nvarchar(15),t.from_date,103) as fdate,convert(nvarchar(15),t.to_date,103) as tdate,de.Dept_Name,c.Course_Name,c.course_id,de.dept_code,t.semester,t.reason,t.batch_year,t.degree_code,case when isnull(DayOrder,'0')=0 then convert(varchar(100),'') else convert(varchar(100),'Day '+convert(varchar(100),DayOrder)) end as [Alternate Day Order] from tbl_consider_day_order t,Degree d,Department de,course c where t.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id " + batchvalues + " " + degreevalues + " " + reasonvalues + " " + datevalue + " order by fdate,tdate,t.batch_year,t.degree_code,t.semester";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                btnprintmaster.Visible = true;
                btnxl.Visible = true;
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
                FpSpread1.Visible = true;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 9;
                FpSpread1.Width = 900;
                FpSpread1.SheetCorner.ColumnCount = 0;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Reason";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "From Date";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "To Date";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Alternate Day Order";

                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Locked = true;

                FpSpread1.Sheets[0].Columns[0].Width = 30;
                FpSpread1.Sheets[0].Columns[1].Width = 80;
                FpSpread1.Sheets[0].Columns[2].Width = 150;
                FpSpread1.Sheets[0].Columns[3].Width = 150;
                FpSpread1.Sheets[0].Columns[4].Width = 50;
                FpSpread1.Sheets[0].Columns[5].Width = 200;
                FpSpread1.Sheets[0].Columns[6].Width = 80;
                FpSpread1.Sheets[0].Columns[7].Width = 80;
                FpSpread1.Sheets[0].Columns[8].Width = 120;

                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;

                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                FpSpread1.Sheets[0].Columns[0].CellType = txt;
                FpSpread1.Sheets[0].Columns[1].CellType = txt;
                FpSpread1.Sheets[0].Columns[2].CellType = txt;
                FpSpread1.Sheets[0].Columns[3].CellType = txt;
                FpSpread1.Sheets[0].Columns[4].CellType = txt;
                FpSpread1.Sheets[0].Columns[5].CellType = txt;
                FpSpread1.Sheets[0].Columns[6].CellType = txt;
                FpSpread1.Sheets[0].Columns[7].CellType = txt;
                FpSpread1.Sheets[0].Columns[8].CellType = txt;

                int srno = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdate = ds.Tables[0].Rows[i]["fdate"].ToString();
                    string edate = ds.Tables[0].Rows[i]["tdate"].ToString();
                    string batchyear = ds.Tables[0].Rows[i]["batch_year"].ToString();
                    string degree = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                    string dept = ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                    string sem = ds.Tables[0].Rows[i]["semester"].ToString();
                    string reason = ds.Tables[0].Rows[i]["reason"].ToString();
                    string alternateDayOrder = Convert.ToString(ds.Tables[0].Rows[i]["Alternate Day Order"]).Trim();
                    srno++;
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = degree.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = ds.Tables[0].Rows[i]["Course_id"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dept.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[i]["Degree_code"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sem.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = reason.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = fdate.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = edate.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = alternateDayOrder.ToString();
                }
            }
            else
            {
                btnprintmaster.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                Printcontrol.Visible = false;
                FpSpread1.Visible = false;
                txtexcelname.Text = string.Empty;
                btnxl.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "No Records Found";

            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch
        {
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnsave.Text == "Update")
            {
              
                Boolean batchset = false;
                Boolean deptset = false;
                string existdetails = string.Empty;
                lblPopErr.Text = string.Empty;
                divPopErr.Visible = false;
                if (ddlreason.Items.Count == 0)
                {
                    lblerrmsg.Text = "Please Enter Reason";
                    lblerrmsg.Visible = true;
                    return;
                }
                string reason = ddlreason.SelectedItem.ToString();
                string strsem = "Select Distinct Current_Semester,Degree_code,Batch_Year from registration where cc=0 and delflag=0 and exam_flag<>'debar' ;";
                strsem = strsem + " Select * from Seminfo";
                DataSet dssem = d2.select_method_wo_parameter(strsem, "Text");

                string fdate = txtfromdateadd.Text.ToString();
                string[] fd = fdate.Split('/');
                string tdate = txttodateadd.Text.ToString();
                string[] ttda = tdate.Split('/');
                DateTime dtfrom = Convert.ToDateTime(fd[1] + '/' + fd[0] + '/' + fd[2]);
                DateTime dtto = Convert.ToDateTime(ttda[1] + '/' + ttda[0] + '/' + ttda[2]);
                if (dtfrom > dtto)
                {
                    lblerrmsg.Text = "Please Enter To Date Must Be Greater Than From Date";
                    lblerrmsg.Visible = true;
                    return;
                }

                string getaldegree = "select de.Dept_Name,c.Course_Name,d.Degree_Code from Degree d,Department de,course c where d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id ";
                DataSet dsdegree = d2.select_method_wo_parameter(getaldegree, "Text");

                Boolean saveflag = false;
                for (int ba = 0; ba < chklsbatchadd.Items.Count; ba++)
                {
                    if (chklsbatchadd.Items[ba].Selected == true)
                    {
                        string batchyear = chklsbatchadd.Items[ba].Value.ToString();
                        batchset = true;
                        for (int br = 0; br < chklsbranchadd.Items.Count; br++)
                        {
                            if (chklsbranchadd.Items[br].Selected == true)
                            {
                                deptset = true;
                                string degree = chklsbranchadd.Items[br].Value.ToString();
                                dssem.Tables[0].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "'";
                                DataView dvsem = dssem.Tables[0].DefaultView;
                                for (int se = 0; se < dvsem.Count; se++)
                                {
                                    string sem = dvsem[se]["Current_Semester"].ToString();
                                    dssem.Tables[1].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "' and semester='" + sem + "'";
                                    DataView dvseminfo = dssem.Tables[1].DefaultView;
                                    for (int si = 0; si < dvseminfo.Count; si++)
                                    {
                                        string sdate = dvseminfo[si]["start_date"].ToString();
                                        string edate = dvseminfo[si]["end_date"].ToString();
                                        DateTime dtstart = Convert.ToDateTime(sdate);
                                        DateTime dtend = Convert.ToDateTime(edate);
                                        if (dtfrom >= dtstart && dtfrom <= dtend && dtto >= dtstart && dtto <= dtend)
                                        {
                                            int asperday = 0;
                                            int includeattn = 0;
                                            int skipday = 0;
                                            int nextday = 0;
                                            string alternateDayOrder = "0";
                                            if (chkConsiderDayOrder.Checked)
                                            {
                                                if (ddlAlternateDayOrder.Items.Count > 0)
                                                {
                                                    alternateDayOrder = Convert.ToString(ddlAlternateDayOrder.SelectedValue).Trim();
                                                }
                                            }
                                           
                                         
                                          
                                               // if (Chkasperday.Checked == true)
                                            if (rdbasperday.Checked == true)
                                                asperday = 1;

                                            else if (rdbskipday.Checked == true)
                                                asperday = 2;

                                            else if (rdbnextorder.Checked == true)
                                                asperday = 3;
                                            
                                                if (Chkincludeattendance.Checked == true)
                                                    includeattn = 1;
                                                else
                                                    includeattn = 0;
                                                string insertvalue = "if exists (select * from tbl_consider_day_order where Degree_code='" + degree + "' and Batch_year='" + batchyear + "' and Semester='" + sem + "' and ((from_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "') or  (to_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "')) )update  tbl_consider_day_order set DayOrder='" + alternateDayOrder + "',asperday='" + asperday + "',include_attendance='" + includeattn + "' where Degree_code='" + degree + "' and Batch_year='" + batchyear + "' and Semester='" + sem + "' and ((from_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "') or  (to_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "')) else insert into tbl_consider_day_order (From_Date,To_Date,Reason,Batch_year,Degree_code,Semester,DayOrder,asperday,include_attendance)";

                                                insertvalue = insertvalue + " Values('" + dtfrom + "','" + dtto + "','" + reason + "','" + batchyear + "','" + degree + "','" + sem + "','" + alternateDayOrder + "','" + asperday + "','" + includeattn + "')";
                                                int insert = d2.update_method_wo_parameter(insertvalue, "Text");
                                                saveflag = true;
                                           
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                if (batchset == false)
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Please Select the Batch and Proceed";
                    return;
                }
                if (deptset == false)
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Please Select the Degree,Branch and Proceed";
                    return;
                }
                if (saveflag == true)
                {
                    bindspread();
                    //lblPopErr.Text = "Successfully Saved";
                    //divPopErr.Visible = true;
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Update Successfully";
                }
                else
                {
                    if (existdetails == "")
                    {
                        lblerrmsg.Visible = true;
                        lblerrmsg.Text = "Please Update Semeter Information";
                    }
                }
                if (existdetails.Trim() != "")
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = existdetails + " Already Exists The Given Date";
                }
            }
            else
            {
                Boolean batchset = false;
                Boolean deptset = false;
                string existdetails = string.Empty;
                lblPopErr.Text = string.Empty;
                divPopErr.Visible = false;
                if (ddlreason.Items.Count == 0)
                {
                    lblerrmsg.Text = "Please Enter Reason";
                    lblerrmsg.Visible = true;
                    return;
                }
                string reason = ddlreason.SelectedItem.ToString();
                string strsem = "Select Distinct Current_Semester,Degree_code,Batch_Year from registration where cc=0 and delflag=0 and exam_flag<>'debar' ;";
                strsem = strsem + " Select * from Seminfo";
                DataSet dssem = d2.select_method_wo_parameter(strsem, "Text");

                string fdate = txtfromdateadd.Text.ToString();
                string[] fd = fdate.Split('/');
                string tdate = txttodateadd.Text.ToString();
                string[] ttda = tdate.Split('/');
                DateTime dtfrom = Convert.ToDateTime(fd[1] + '/' + fd[0] + '/' + fd[2]);
                DateTime dtto = Convert.ToDateTime(ttda[1] + '/' + ttda[0] + '/' + ttda[2]);
                if (dtfrom > dtto)
                {
                    lblerrmsg.Text = "Please Enter To Date Must Be Greater Than From Date";
                    lblerrmsg.Visible = true;
                    return;
                }

                string getaldegree = "select de.Dept_Name,c.Course_Name,d.Degree_Code from Degree d,Department de,course c where d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id ";
                DataSet dsdegree = d2.select_method_wo_parameter(getaldegree, "Text");

                Boolean saveflag = false;
                for (int ba = 0; ba < chklsbatchadd.Items.Count; ba++)
                {
                    if (chklsbatchadd.Items[ba].Selected == true)
                    {
                        string batchyear = chklsbatchadd.Items[ba].Value.ToString();
                        batchset = true;
                        for (int br = 0; br < chklsbranchadd.Items.Count; br++)
                        {
                            if (chklsbranchadd.Items[br].Selected == true)
                            {
                                deptset = true;
                                string degree = chklsbranchadd.Items[br].Value.ToString();
                                dssem.Tables[0].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "'";
                                DataView dvsem = dssem.Tables[0].DefaultView;
                                for (int se = 0; se < dvsem.Count; se++)
                                {
                                    string sem = dvsem[se]["Current_Semester"].ToString();
                                    dssem.Tables[1].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "' and semester='" + sem + "'";
                                    DataView dvseminfo = dssem.Tables[1].DefaultView;
                                    for (int si = 0; si < dvseminfo.Count; si++)
                                    {
                                        string sdate = dvseminfo[si]["start_date"].ToString();
                                        string edate = dvseminfo[si]["end_date"].ToString();
                                        DateTime dtstart = Convert.ToDateTime(sdate);
                                        DateTime dtend = Convert.ToDateTime(edate);
                                        if (dtfrom >= dtstart && dtfrom <= dtend && dtto >= dtstart && dtto <= dtend)
                                        {
                                            string strexistrecdord = "select * from tbl_consider_day_order where Degree_code='" + degree + "' and Batch_year='" + batchyear + "' and Semester='" + sem + "' and ((from_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "') or  (to_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'))";
                                            DataSet dsex = d2.select_method_wo_parameter(strexistrecdord, "Text");
                                            string alternateDayOrder = "0";
                                            if (chkConsiderDayOrder.Checked)
                                            {
                                                if (ddlAlternateDayOrder.Items.Count > 0)
                                                {
                                                    alternateDayOrder = Convert.ToString(ddlAlternateDayOrder.SelectedValue).Trim();
                                                }
                                            }
                                            int asperday = 0;
                                            int skipday = 0;
                                            int nextday = 0;
                                            int includeattn = 0;
                                            if (dsex.Tables[0].Rows.Count == 0)
                                            {
                                               // if (Chkasperday.Checked == true)
                                                if (rdbasperday.Checked == true)
                                                    asperday = 1;
                                                
                                                else if (rdbskipday.Checked == true)
                                                    asperday = 2;

                                                else if (rdbnextorder.Checked == true)
                                                    asperday = 3;
                                               
                                                if (Chkincludeattendance.Checked == true)
                                                    includeattn = 1;
                                                else
                                                    includeattn = 0;
                                                string insertvalue = "insert into tbl_consider_day_order (From_Date,To_Date,Reason,Batch_year,Degree_code,Semester,DayOrder,asperday,include_attendance)";
                                                insertvalue = insertvalue + " Values('" + dtfrom + "','" + dtto + "','" + reason + "','" + batchyear + "','" + degree + "','" + sem + "','" + alternateDayOrder + "','" + asperday + "','" + includeattn + "')";
                                                int insert = d2.update_method_wo_parameter(insertvalue, "Text");
                                                saveflag = true;
                                            }
                                            else
                                            {
                                                dsdegree.Tables[0].DefaultView.RowFilter = " Degree_code='" + degree + "'";
                                                DataView dvdegree = dsdegree.Tables[0].DefaultView;
                                                if (existdetails == "")
                                                {
                                                    existdetails = batchyear + " - " + dvdegree[0]["Course_Name"].ToString() + " - " + dvdegree[0]["Dept_Name"].ToString() + " - " + sem + " Sem ";
                                                }
                                                else
                                                {
                                                    existdetails = existdetails + " , " + batchyear + " - " + dvdegree[0]["Course_Name"].ToString() + " - " + dvdegree[0]["Dept_Name"].ToString() + " - " + sem + " Sem ";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                if (batchset == false)
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Please Select the Batch and Proceed";
                    return;
                }
                if (deptset == false)
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Please Select the Degree,Branch and Proceed";
                    return;
                }
                if (saveflag == true)
                {
                    bindspread();
                    //lblPopErr.Text = "Successfully Saved";
                    //divPopErr.Visible = true;
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Successfully Saved";
                }
                else
                {
                    if (existdetails == "")
                    {
                        lblerrmsg.Visible = true;
                        lblerrmsg.Text = "Please Update Semeter Information";
                    }
                }
                if (existdetails.Trim() != "")
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = existdetails + " Already Exists The Given Date";
                }
            }
        }
        catch
        {
        }
    }

    protected void btnreasonadd_Click(object sender, EventArgs e)
    {
        textreason.Text = string.Empty;
        panelreason.Visible = true;
        

    }

    protected void btnreasondelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopErr.Text = string.Empty;
            divPopErr.Visible = false;
            if (ddlreason.Items.Count > 0)
            {
                string reason = ddlreason.SelectedItem.ToString();
                string strquery = "select * from tbl_consider_day_order where Reason='" + reason + "'";
                DataSet dsexres = d2.select_method_wo_parameter(strquery, "Text");
                if (dsexres.Tables[0].Rows.Count == 0)
                {
                    string insertvalue = "Delete from textvaltable where TextVal='" + reason + "' and TextCriteria='dayor'";
                    int inserty = d2.update_method_wo_parameter(insertvalue, "Text");
                    loadreason();
                    lblPopErr.Text = "Reason Deleted Successfully";
                    divPopErr.Visible = true;
                }
                else
                {
                    lblerrmsg.Text = "You Can't Delete This Because  Already Exists The Day Order Change This Reason";
                    lblerrmsg.Visible = true;
                    return;
                }
            }
            else
            {
                lblerrmsg.Text = "No Reason For Delete";
                lblerrmsg.Visible = true;
                return;
            }
        }
        catch
        {
        }
    }

    protected void btnreasonsave_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopErr.Text = string.Empty;
            divPopErr.Visible = false;
            collegecode = Session["collegecode"].ToString();
            string reason = textreason.Text.ToString();
            if (reason.Trim() != "" && reason != null)
            {
                string insvalues = "select * from textvaltable where TextCriteria='dayor' and TextVal='" + reason + "'";
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(insvalues, "Text");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    string insertvalue = "insert into textvaltable(TextVal,TextCriteria,college_code) values('" + reason + "','dayor','" + collegecode + "')";
                    int inserty = d2.update_method_wo_parameter(insertvalue, "Text");
                    textreason.Text = string.Empty;
                    loadreason();
                    lblPopErr.Text = "Reason Successfully Saved";
                    divPopErr.Visible = true;
                }
                else
                {
                    lblreasonerr.Visible = true;
                    lblreasonerr.Text = "Already Exists Reason";
                    return;
                }
            }
            else
            {
                lblreasonerr.Visible = true;
                lblreasonerr.Text = "Please Enter Reason";
                return;
            }
        }
        catch
        {
        }
    }

    protected void btnreasonexit_Click(object sender, EventArgs e)
    {
        panelreason.Visible = false;
        txtreason.Text = string.Empty;
    }

    protected void chkConsiderDayOrder_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divAlternateDayOrder.Visible = false;
            ddlAlternateDayOrder.SelectedIndex = 0;
            if (chkConsiderDayOrder.Checked)
            {
                divAlternateDayOrder.Visible = true;
                Chkincludeattendance.Visible = true;
                alterdayy.ColSpan = 6;
            }
            else
                alterdayy.ColSpan =0;
        }
        catch
        {
        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnPopErrClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopErr.Text = string.Empty;
            divPopErr.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }


    protected void btnHelp_Click(object sender, EventArgs e)
    {
        if (lblnote.Visible == true)
        {
            lblnote.Visible = false;
            btnHelp.Text = "Note";
        }
        else
        {
            lblnote.Visible = true;
            btnHelp.Text = "Hide";
        }
        //lblnote.Text="1.As Perday Schedule: if you want to change the particular dayorder check the (Consider Alternate Day Order)  checkbox you can change it, with this if you have checked the (As Perday Schedule)checkbox the day order will remain same for next day."
        //lblnote.Text = " if you want to change the particular dayorder you have checked (Consider Alternate Day Order)  checkbox  when you can change it, with this if you have checked the (As Perday Schedule)checkbox the day order will remain same for next day. when you have check the (Include Period in Attendance Report)  this dayorder show in  attendance report.";

        lblnote.Text = " if you want to change the particular dayorder you have checked (Consider Alternate Day Order)  checkbox  when you can change it, with this if you have checked the (As Perday Schedule)RadioButton the day order will remain same for next day (or)  you have checked the (Skip Dayorder Change)RadioButton the day order will Skip for Pervious day (or)  you have checked the (Next Dayorder)RadioButton the day order will Continuous  for next day. when you have check the (Include Period in Attendance Report)  this dayorder show in  attendance report.Example: 10.5.18 - 5th day order you have change 3rd Dayorder(Consider Alternate Day Order),check (As Perday Schedule) - 11.5.18 1st dayorder (or) check (Skip Dayorder Change) - 11.5.18 5th dayorder (or) you have checked the (Next Dayorder)- 11.5.18 4th day order.";

      
        
    }

    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        Cellclick = true;
    }
    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            int col = 0;
            int.TryParse(activecol, out col);
            if (activerow.Trim() != "" && activecol.Trim() != "")
            {

                string batch = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                string degree = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                string branch = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                if (chklsbatchadd.Items.Count > 0)
                {
                    // BindBatch();
                    int cun = 0;
                    for (int i = 0; i < chklsbatchadd.Items.Count; i++)
                    {

                        string val = chklsbatchadd.Items[i].Text;
                        if (chklsbatchadd.Items[i].Text == batch)
                        {
                            cun++;

                            chklsbatchadd.Items[i].Selected = true;
                            chklsbatchadd.Items[i].Enabled = true;
                        }
                        else
                        {
                            chklsbatchadd.Items[i].Selected = false;
                            chklsbatchadd.Items[i].Enabled = false;
                            chkbatchadd.Enabled = false;
                        }

                    }
                    txtbatch.Text = "Batch (" + cun + ")";
                }
                BindDegree();
                if (chklsdegreeadd.Items.Count > 0)
                {
                    int cun = 0;
                    for (int i = 0; i < chklsdegreeadd.Items.Count; i++)
                    {
                       
                        string val = chklsdegreeadd.Items[i].Value;
                        if (chklsdegreeadd.Items[i].Value == degree)
                        {
                            cun++;
                            chklsdegreeadd.Items[i].Selected = true;
                            chklsdegreeadd.Items[i].Enabled = true;
                        }
                        else
                        {
                            chklsdegreeadd.Items[i].Enabled = false;
                            chklsdegreeadd.Items[i].Selected = false;
                            chkdegreeadd.Enabled = false;
                        }
                    }
                    txtdegree.Text = "Degree (" + cun + ")";
                
              //  if (chklsbranchadd.Items.Count > 0)
               // {
                    int cuns = 0;
                    bindbranchadd();
                    for (int i = 0; i < chklsbranchadd.Items.Count; i++)
                    {
                       
                        string branchs = chklsbranchadd.Items[i].Value;
                        if (chklsbranchadd.Items[i].Value == branch)
                        {
                            cun++;
                            chklsbranchadd.Items[i].Selected = true;
                            chklsbranchadd.Items[i].Enabled = true;
                        }
                        else
                        {
                            chklsbranchadd.Items[i].Selected = false;
                            chklsbranchadd.Items[i].Enabled = false;
                            chkbranchadd.Enabled = false;
                        }
                    }
                    txtbranch.Text = "Branch (" + cuns + ")";
                }
                loadreason();
                    btnsave.Text = "Update";
                    txtfromdateadd.Text = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text;
                    txttodateadd.Text = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text;
                    Btndelete.Visible = true;
                    btnclear.Visible = true;
                   // string strquery = d2.GetFunction("select * from textvaltable where TextCriteria='dayor'");
                
               // BindBatch1(batch);
            }
        }

    }

    public void BindBatch1( string batchs)
    {
        try
        {
            chklsbatch.Items.Clear();
            chklsbatchadd.Items.Clear();
            string batch = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc='0' and DelFlag='0' and Exam_Flag<>'Debar' order by batch_year";
            ds = d2.select_method_wo_parameter(batch, "Text");
            // ds = ClsAttendanceAccess.GetBatchDetail();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();

                chklsbatchadd.DataSource = ds;
                chklsbatchadd.DataTextField = "Batch_year";
                chklsbatchadd.DataValueField = "Batch_year";
                chklsbatchadd.DataBind();

               
            }
        }
        catch
        {
        }
    }
    //protected void btnsave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (btnsave.Text == "Update")
    //        {

    //            Boolean batchset = false;
    //            Boolean deptset = false;
    //            string existdetails = string.Empty;
    //            lblPopErr.Text = string.Empty;
    //            divPopErr.Visible = false;
    //            if (ddlreason.Items.Count == 0)
    //            {
    //                lblerrmsg.Text = "Please Enter Reason";
    //                lblerrmsg.Visible = true;
    //                return;
    //            }
    //            string reason = ddlreason.SelectedItem.ToString();
    //            string strsem = "Select Distinct Current_Semester,Degree_code,Batch_Year from registration where cc=0 and delflag=0 and exam_flag<>'debar' ;";
    //            strsem = strsem + " Select * from Seminfo";
    //            DataSet dssem = d2.select_method_wo_parameter(strsem, "Text");

    //            string fdate = txtfromdateadd.Text.ToString();
    //            string[] fd = fdate.Split('/');
    //            string tdate = txttodateadd.Text.ToString();
    //            string[] ttda = tdate.Split('/');
    //            DateTime dtfrom = Convert.ToDateTime(fd[1] + '/' + fd[0] + '/' + fd[2]);
    //            DateTime dtto = Convert.ToDateTime(ttda[1] + '/' + ttda[0] + '/' + ttda[2]);
    //            if (dtfrom > dtto)
    //            {
    //                lblerrmsg.Text = "Please Enter To Date Must Be Greater Than From Date";
    //                lblerrmsg.Visible = true;
    //                return;
    //            }

    //            string getaldegree = "select de.Dept_Name,c.Course_Name,d.Degree_Code from Degree d,Department de,course c where d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id ";
    //            DataSet dsdegree = d2.select_method_wo_parameter(getaldegree, "Text");

    //            Boolean saveflag = false;
    //            for (int ba = 0; ba < chklsbatchadd.Items.Count; ba++)
    //            {
    //                if (chklsbatchadd.Items[ba].Selected == true)
    //                {
    //                    string batchyear = chklsbatchadd.Items[ba].Value.ToString();
    //                    batchset = true;
    //                    for (int br = 0; br < chklsbranchadd.Items.Count; br++)
    //                    {
    //                        if (chklsbranchadd.Items[br].Selected == true)
    //                        {
    //                            deptset = true;
    //                            string degree = chklsbranchadd.Items[br].Value.ToString();
    //                            dssem.Tables[0].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "'";
    //                            DataView dvsem = dssem.Tables[0].DefaultView;
    //                            for (int se = 0; se < dvsem.Count; se++)
    //                            {
    //                                string sem = dvsem[se]["Current_Semester"].ToString();
    //                                dssem.Tables[1].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "' and semester='" + sem + "'";
    //                                DataView dvseminfo = dssem.Tables[1].DefaultView;
    //                                for (int si = 0; si < dvseminfo.Count; si++)
    //                                {
    //                                    string sdate = dvseminfo[si]["start_date"].ToString();
    //                                    string edate = dvseminfo[si]["end_date"].ToString();
    //                                    DateTime dtstart = Convert.ToDateTime(sdate);
    //                                    DateTime dtend = Convert.ToDateTime(edate);
    //                                    if (dtfrom >= dtstart && dtfrom <= dtend && dtto >= dtstart && dtto <= dtend)
    //                                    {

    //                                        string alternateDayOrder = "0";
    //                                        if (chkConsiderDayOrder.Checked)
    //                                        {
    //                                            if (ddlAlternateDayOrder.Items.Count > 0)
    //                                            {
    //                                                alternateDayOrder = Convert.ToString(ddlAlternateDayOrder.SelectedValue).Trim();
    //                                            }
    //                                        }
    //                                        int asperday = 0;
    //                                        int includeattn = 0;

    //                                        if (Chkasperday.Checked == true)
    //                                            asperday = 1;
    //                                        else
    //                                            asperday = 0;
    //                                        if (Chkincludeattendance.Checked == true)
    //                                            includeattn = 1;
    //                                        else
    //                                            includeattn = 0;
    //                                        string insertvalue = "if exists (select * from tbl_consider_day_order where Degree_code='" + degree + "' and Batch_year='" + batchyear + "' and Semester='" + sem + "' and ((from_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "') or  (to_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "')) update  tbl_consider_day_order set DayOrder='" + alternateDayOrder + "',asperday='" + asperday + "',include_attendance='" + includeattn + "' else insert into tbl_consider_day_order (From_Date,To_Date,Reason,Batch_year,Degree_code,Semester,DayOrder,asperday,include_attendance)";

    //                                        insertvalue = insertvalue + " Values('" + dtfrom + "','" + dtto + "','" + reason + "','" + batchyear + "','" + degree + "','" + sem + "','" + alternateDayOrder + "','" + asperday + "','" + includeattn + "')";
    //                                        int insert = d2.update_method_wo_parameter(insertvalue, "Text");
    //                                        saveflag = true;

    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }


    //            if (batchset == false)
    //            {
    //                lblerrmsg.Visible = true;
    //                lblerrmsg.Text = "Please Select the Batch and Proceed";
    //                return;
    //            }
    //            if (deptset == false)
    //            {
    //                lblerrmsg.Visible = true;
    //                lblerrmsg.Text = "Please Select the Degree,Branch and Proceed";
    //                return;
    //            }
    //            if (saveflag == true)
    //            {
    //                bindspread();
    //                //lblPopErr.Text = "Successfully Saved";
    //                //divPopErr.Visible = true;
    //                lblerrmsg.Visible = true;
    //                lblerrmsg.Text = "Update Successfully";
    //            }
    //            else
    //            {
    //                if (existdetails == "")
    //                {
    //                    lblerrmsg.Visible = true;
    //                    lblerrmsg.Text = "Please Update Semeter Information";
    //                }
    //            }
    //            if (existdetails.Trim() != "")
    //            {
    //                lblerrmsg.Visible = true;
    //                lblerrmsg.Text = existdetails + " Already Exists The Given Date";
    //            }
    //        }
    //        else
    //        {
    //            Boolean batchset = false;
    //            Boolean deptset = false;
    //            string existdetails = string.Empty;
    //            lblPopErr.Text = string.Empty;
    //            divPopErr.Visible = false;
    //            if (ddlreason.Items.Count == 0)
    //            {
    //                lblerrmsg.Text = "Please Enter Reason";
    //                lblerrmsg.Visible = true;
    //                return;
    //            }
    //            string reason = ddlreason.SelectedItem.ToString();
    //            string strsem = "Select Distinct Current_Semester,Degree_code,Batch_Year from registration where cc=0 and delflag=0 and exam_flag<>'debar' ;";
    //            strsem = strsem + " Select * from Seminfo";
    //            DataSet dssem = d2.select_method_wo_parameter(strsem, "Text");

    //            string fdate = txtfromdateadd.Text.ToString();
    //            string[] fd = fdate.Split('/');
    //            string tdate = txttodateadd.Text.ToString();
    //            string[] ttda = tdate.Split('/');
    //            DateTime dtfrom = Convert.ToDateTime(fd[1] + '/' + fd[0] + '/' + fd[2]);
    //            DateTime dtto = Convert.ToDateTime(ttda[1] + '/' + ttda[0] + '/' + ttda[2]);
    //            if (dtfrom > dtto)
    //            {
    //                lblerrmsg.Text = "Please Enter To Date Must Be Greater Than From Date";
    //                lblerrmsg.Visible = true;
    //                return;
    //            }

    //            string getaldegree = "select de.Dept_Name,c.Course_Name,d.Degree_Code from Degree d,Department de,course c where d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id ";
    //            DataSet dsdegree = d2.select_method_wo_parameter(getaldegree, "Text");

    //            Boolean saveflag = false;
    //            for (int ba = 0; ba < chklsbatchadd.Items.Count; ba++)
    //            {
    //                if (chklsbatchadd.Items[ba].Selected == true)
    //                {
    //                    string batchyear = chklsbatchadd.Items[ba].Value.ToString();
    //                    batchset = true;
    //                    for (int br = 0; br < chklsbranchadd.Items.Count; br++)
    //                    {
    //                        if (chklsbranchadd.Items[br].Selected == true)
    //                        {
    //                            deptset = true;
    //                            string degree = chklsbranchadd.Items[br].Value.ToString();
    //                            dssem.Tables[0].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "'";
    //                            DataView dvsem = dssem.Tables[0].DefaultView;
    //                            for (int se = 0; se < dvsem.Count; se++)
    //                            {
    //                                string sem = dvsem[se]["Current_Semester"].ToString();
    //                                dssem.Tables[1].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "' and semester='" + sem + "'";
    //                                DataView dvseminfo = dssem.Tables[1].DefaultView;
    //                                for (int si = 0; si < dvseminfo.Count; si++)
    //                                {
    //                                    string sdate = dvseminfo[si]["start_date"].ToString();
    //                                    string edate = dvseminfo[si]["end_date"].ToString();
    //                                    DateTime dtstart = Convert.ToDateTime(sdate);
    //                                    DateTime dtend = Convert.ToDateTime(edate);
    //                                    if (dtfrom >= dtstart && dtfrom <= dtend && dtto >= dtstart && dtto <= dtend)
    //                                    {
    //                                        string strexistrecdord = "select * from tbl_consider_day_order where Degree_code='" + degree + "' and Batch_year='" + batchyear + "' and Semester='" + sem + "' and ((from_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "') or  (to_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'))";
    //                                        DataSet dsex = d2.select_method_wo_parameter(strexistrecdord, "Text");
    //                                        string alternateDayOrder = "0";
    //                                        if (chkConsiderDayOrder.Checked)
    //                                        {
    //                                            if (ddlAlternateDayOrder.Items.Count > 0)
    //                                            {
    //                                                alternateDayOrder = Convert.ToString(ddlAlternateDayOrder.SelectedValue).Trim();
    //                                            }
    //                                        }
    //                                        int asperday = 0;
    //                                        int includeattn = 0;
    //                                        if (dsex.Tables[0].Rows.Count == 0)
    //                                        {
    //                                            if (Chkasperday.Checked == true)
    //                                                asperday = 1;
    //                                            else
    //                                                asperday = 0;
    //                                            if (Chkincludeattendance.Checked == true)
    //                                                includeattn = 1;
    //                                            else
    //                                                includeattn = 0;
    //                                            string insertvalue = "insert into tbl_consider_day_order (From_Date,To_Date,Reason,Batch_year,Degree_code,Semester,DayOrder,asperday,include_attendance)";
    //                                            insertvalue = insertvalue + " Values('" + dtfrom + "','" + dtto + "','" + reason + "','" + batchyear + "','" + degree + "','" + sem + "','" + alternateDayOrder + "','" + asperday + "','" + includeattn + "')";
    //                                            int insert = d2.update_method_wo_parameter(insertvalue, "Text");
    //                                            saveflag = true;
    //                                        }
    //                                        else
    //                                        {
    //                                            dsdegree.Tables[0].DefaultView.RowFilter = " Degree_code='" + degree + "'";
    //                                            DataView dvdegree = dsdegree.Tables[0].DefaultView;
    //                                            if (existdetails == "")
    //                                            {
    //                                                existdetails = batchyear + " - " + dvdegree[0]["Course_Name"].ToString() + " - " + dvdegree[0]["Dept_Name"].ToString() + " - " + sem + " Sem ";
    //                                            }
    //                                            else
    //                                            {
    //                                                existdetails = existdetails + " , " + batchyear + " - " + dvdegree[0]["Course_Name"].ToString() + " - " + dvdegree[0]["Dept_Name"].ToString() + " - " + sem + " Sem ";
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }


    //            if (batchset == false)
    //            {
    //                lblerrmsg.Visible = true;
    //                lblerrmsg.Text = "Please Select the Batch and Proceed";
    //                return;
    //            }
    //            if (deptset == false)
    //            {
    //                lblerrmsg.Visible = true;
    //                lblerrmsg.Text = "Please Select the Degree,Branch and Proceed";
    //                return;
    //            }
    //            if (saveflag == true)
    //            {
    //                bindspread();
    //                //lblPopErr.Text = "Successfully Saved";
    //                //divPopErr.Visible = true;
    //                lblerrmsg.Visible = true;
    //                lblerrmsg.Text = "Successfully Saved";
    //            }
    //            else
    //            {
    //                if (existdetails == "")
    //                {
    //                    lblerrmsg.Visible = true;
    //                    lblerrmsg.Text = "Please Update Semeter Information";
    //                }
    //            }
    //            if (existdetails.Trim() != "")
    //            {
    //                lblerrmsg.Visible = true;
    //                lblerrmsg.Text = existdetails + " Already Exists The Given Date";
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    protected void Btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            
                Boolean batchset = false;
                Boolean deptset = false;
                string existdetails = string.Empty;
                lblPopErr.Text = string.Empty;
                divPopErr.Visible = false;
                if (ddlreason.Items.Count == 0)
                {
                    lblerrmsg.Text = "Please Enter Reason";
                    lblerrmsg.Visible = true;
                    return;
                }
                string reason = ddlreason.SelectedItem.ToString();
                string strsem = "Select Distinct Current_Semester,Degree_code,Batch_Year from registration where cc=0 and delflag=0 and exam_flag<>'debar' ;";
                strsem = strsem + " Select * from Seminfo";
                DataSet dssem = d2.select_method_wo_parameter(strsem, "Text");

                string fdate = txtfromdateadd.Text.ToString();
                string[] fd = fdate.Split('/');
                string tdate = txttodateadd.Text.ToString();
                string[] ttda = tdate.Split('/');
                DateTime dtfrom = Convert.ToDateTime(fd[1] + '/' + fd[0] + '/' + fd[2]);
                DateTime dtto = Convert.ToDateTime(ttda[1] + '/' + ttda[0] + '/' + ttda[2]);
                if (dtfrom > dtto)
                {
                    lblerrmsg.Text = "Please Enter To Date Must Be Greater Than From Date";
                    lblerrmsg.Visible = true;
                    return;
                }

                string getaldegree = "select de.Dept_Name,c.Course_Name,d.Degree_Code from Degree d,Department de,course c where d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id ";
                DataSet dsdegree = d2.select_method_wo_parameter(getaldegree, "Text");

                Boolean saveflag = false;
                for (int ba = 0; ba < chklsbatchadd.Items.Count; ba++)
                {
                    if (chklsbatchadd.Items[ba].Selected == true)
                    {
                        string batchyear = chklsbatchadd.Items[ba].Value.ToString();
                        batchset = true;
                        for (int br = 0; br < chklsbranchadd.Items.Count; br++)
                        {
                            if (chklsbranchadd.Items[br].Selected == true)
                            {
                                deptset = true;
                                string degree = chklsbranchadd.Items[br].Value.ToString();
                                dssem.Tables[0].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "'";
                                DataView dvsem = dssem.Tables[0].DefaultView;
                                for (int se = 0; se < dvsem.Count; se++)
                                {
                                    string sem = dvsem[se]["Current_Semester"].ToString();
                                    dssem.Tables[1].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "' and semester='" + sem + "'";
                                    DataView dvseminfo = dssem.Tables[1].DefaultView;
                                    for (int si = 0; si < dvseminfo.Count; si++)
                                    {
                                        string sdate = dvseminfo[si]["start_date"].ToString();
                                        string edate = dvseminfo[si]["end_date"].ToString();
                                        DateTime dtstart = Convert.ToDateTime(sdate);
                                        DateTime dtend = Convert.ToDateTime(edate);
                                        if (dtfrom >= dtstart && dtfrom <= dtend && dtto >= dtstart && dtto <= dtend)
                                        {
                                            string strexistrecdord = "select * from tbl_consider_day_order where Degree_code='" + degree + "' and Batch_year='" + batchyear + "' and Semester='" + sem + "' and ((from_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "') or  (to_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'))";
                                            DataSet dsex = d2.select_method_wo_parameter(strexistrecdord, "Text");
                                            string alternateDayOrder = "0";
                                            if (chkConsiderDayOrder.Checked)
                                            {
                                                if (ddlAlternateDayOrder.Items.Count > 0)
                                                {
                                                    alternateDayOrder = Convert.ToString(ddlAlternateDayOrder.SelectedValue).Trim();
                                                }
                                            }
                                            int asperday = 0;
                                            int includeattn = 0;
                                            if (dsex.Tables[0].Rows.Count > 0)
                                            {
                                                //if (Chkasperday.Checked == true)
                                               if (rdbasperday.Checked == true) 
                                                asperday = 1;
                                                else
                                                    asperday = 0;
                                                if (Chkincludeattendance.Checked == true)
                                                    includeattn = 1;
                                                else
                                                    includeattn = 0;
                                                 string insertvalue ="delete from tbl_consider_day_order where Degree_code='" + degree + "' and Batch_year='" + batchyear + "' and Semester='" + sem + "' and ((from_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "') or  (to_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'))" ;
                                              //  string insertvalue = "insert into tbl_consider_day_order (From_Date,To_Date,Reason,Batch_year,Degree_code,Semester,DayOrder,asperday,include_attendance)";
                                               // insertvalue = insertvalue + " Values('" + dtfrom + "','" + dtto + "','" + reason + "','" + batchyear + "','" + degree + "','" + sem + "','" + alternateDayOrder + "','" + asperday + "','" + includeattn + "')";
                                                int insert = d2.update_method_wo_parameter(insertvalue, "Text");
                                                saveflag = true;
                                            }
                                            //else
                                            //{
                                            //    dsdegree.Tables[0].DefaultView.RowFilter = " Degree_code='" + degree + "'";
                                            //    DataView dvdegree = dsdegree.Tables[0].DefaultView;
                                            //    if (existdetails == "")
                                            //    {
                                            //        existdetails = batchyear + " - " + dvdegree[0]["Course_Name"].ToString() + " - " + dvdegree[0]["Dept_Name"].ToString() + " - " + sem + " Sem ";
                                            //    }
                                            //    else
                                            //    {
                                            //        existdetails = existdetails + " , " + batchyear + " - " + dvdegree[0]["Course_Name"].ToString() + " - " + dvdegree[0]["Dept_Name"].ToString() + " - " + sem + " Sem ";
                                            //    }
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                if (batchset == false)
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Please Select the Batch and Proceed";
                    return;
                }
                if (deptset == false)
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Please Select the Degree,Branch and Proceed";
                    return;
                }
                if (saveflag == true)
                {
                    bindspread();
                    //lblPopErr.Text = "Successfully Saved";
                    //divPopErr.Visible = true;
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Successfully Delete";
                }
                else
                {
                    if (existdetails == "")
                    {
                        lblerrmsg.Visible = true;
                        lblerrmsg.Text = "Please Update Semeter Information";
                    }
                }
                if (existdetails.Trim() != "")
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = existdetails + " Already Exists The Given Date";
                }
            
        }
        catch
        {
        }
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        Btndelete.Visible = false;
        btnsave.Text = "Save";
        btnclear.Visible = false;
        Cellclick = false;
     //   chklsbranchadd.Enabled = true;
      //  chklsbranchadd.Enabled = true;
        chkbranchadd.Enabled = true;
       // chklsdegreeadd.Enabled = true;
        chkdegreeadd.Enabled = true;
        chkbatchadd.Enabled = true;
        if (chklsbatchadd.Items.Count > 0)
        {
            // BindBatch();
            int cun = 0;
            for (int i = 0; i < chklsbatchadd.Items.Count; i++)
            {

                string val = chklsbatchadd.Items[i].Text;
               
                    cun++;

                    
                    chklsbatchadd.Items[i].Enabled = true;
                }
                
            
            txtbatch.Text = "Batch (" + cun + ")";
        }
        if (chklsdegreeadd.Items.Count > 0)
        {
            int cun = 0;
            for (int i = 0; i < chklsdegreeadd.Items.Count; i++)
            {
                cun++;

                    chklsdegreeadd.Items[i].Enabled = true;
                }
               
            
            txtdegree.Text = "Degree (" + cun + ")";

            //  if (chklsbranchadd.Items.Count > 0)
            // {
            int cuns = 0;
            //bindbranchadd();
            for (int i = 0; i < chklsbranchadd.Items.Count; i++)
            {

              
                    cuns++;
                    
                    chklsbranchadd.Items[i].Enabled = true;
             
              
            }
            txtbranch.Text = "Branch (" + cuns + ")";
        }
    }




    public void BindDegree()
    {
        try
        {
            ds.Clear();
            txtdegreeadd.Text = "---Select---";
            string batchCode = string.Empty;
            chkdegreeadd.Checked = false;
            chklsdegreeadd.Items.Clear();
            //userCode = Session["usercode"].ToString();
            //singleUser = Session["single_user"].ToString();
            //groupUserCode = Session["group_code"].ToString();
            //collegeCode = string.Empty;
            string collegeCode = string.Empty;
            if (ddlcoll.Items.Count > 0)
                collegeCode = ddlcoll.SelectedValue.ToString().Trim();
           
          

            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string valBatch = string.Empty;
            if (chklsbatchadd.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(chklsbatchadd);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = d2.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsdegreeadd.DataSource = ds;
                chklsdegreeadd.DataTextField = "course_name";
                chklsdegreeadd.DataValueField = "course_id";
                chklsdegreeadd.DataBind();
               // checkBoxListselectOrDeselect(chklsdegreeadd, true);
               // CallCheckboxListChange(chkdegreeadd, chklsdegreeadd, txtdegreeadd, Label2.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindbranch()
    {
        try
        {
            string degreecode = string.Empty;
            //collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            txtbranchadd.Text = "---Select---";
            chkbranchadd.Checked = false;
            chklsbranchadd.Items.Clear();
            ds.Clear();
           string collegeCode = string.Empty;
           
           if (ddlcoll.Items.Count > 0)
               collegeCode = ddlcoll.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            //string valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            //string valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
            if (chklsbatchadd.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(chklsbatchadd);
            if (chklsdegreeadd.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(chklsdegreeadd);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = d2.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsbranchadd.DataSource = ds;
                chklsbranchadd.DataTextField = "dept_name";
                chklsbranchadd.DataValueField = "degree_code";
                chklsbranchadd.DataBind();
               // checkBoxListselectOrDeselect(cblBranch, true);
                //CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void bindbranchadd()
    {
        try
        {
            string degreecode = string.Empty;
            //collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            txtbranchadd.Text = "---Select---";
            chkbranchadd.Checked = false;
            chklsbranchadd.Items.Clear();
            ds.Clear();
            string collegeCode = string.Empty;
           
            if (ddlcoll.Items.Count > 0)
                collegeCode = ddlcoll.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            //string valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            //string valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
            if (chklsbatchadd.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(chklsbatchadd);
            if (chklsdegreeadd.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(chklsdegreeadd);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = d2.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsbranchadd.DataSource = ds;
                chklsbranchadd.DataTextField = "dept_name";
                chklsbranchadd.DataValueField = "degree_code";
                chklsbranchadd.DataBind();
                // checkBoxListselectOrDeselect(cblBranch, true);
                //CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void BindDegree1()
    {
        try
        {
            ds.Clear();
            txtdegreeadd.Text = "---Select---";
            string batchCode = string.Empty;
            chkdegreeadd.Checked = false;
            chklsdegreeadd.Items.Clear();
            //userCode = Session["usercode"].ToString();
            //singleUser = Session["single_user"].ToString();
            //groupUserCode = Session["group_code"].ToString();
            //collegeCode = string.Empty;
            string collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
           
        

            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string valBatch = string.Empty;
            if (chklsbatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(chklsbatch);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = d2.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsdegree.DataSource = ds;
                chklsdegree.DataTextField = "course_name";
                chklsdegree.DataValueField = "course_id";
                chklsdegree.DataBind();
                // checkBoxListselectOrDeselect(chklsdegreeadd, true);
                // CallCheckboxListChange(chkdegreeadd, chklsdegreeadd, txtdegreeadd, Label2.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindbranch1()
    {
        try
        {
            string degreecode = string.Empty;
            //collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            txtbranchadd.Text = "---Select---";
            chkbranchadd.Checked = false;
            chklsbranchadd.Items.Clear();
            ds.Clear();
            string collegeCode = string.Empty;

            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            //string valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            //string valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
            if (chklsbatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(chklsbatch);
            if (chklsdegree.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(chklsdegree);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = d2.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsbranch.DataSource = ds;
                chklsbranch.DataTextField = "dept_name";
                chklsbranch.DataValueField = "degree_code";
                chklsbranch.DataBind();
                // checkBoxListselectOrDeselect(cblBranch, true);
                //CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void bindbranchadd1()
    {
        try
        {
            string degreecode = string.Empty;
            //collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            txtbranchadd.Text = "---Select---";
            chkbranchadd.Checked = false;
            chklsbranchadd.Items.Clear();
            ds.Clear();
            string collegeCode = string.Empty;

            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            //string valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            //string valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
            if (chklsbatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(chklsbatch);
            if (chklsdegree.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(chklsdegree);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = d2.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsbranch.DataSource = ds;
                chklsbranch.DataTextField = "dept_name";
                chklsbranch.DataValueField = "degree_code";
                chklsbranch.DataBind();
                // checkBoxListselectOrDeselect(cblBranch, true);
                //CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
           
            BindDegree1();
            bindbranchadd1();
            loadreason();
        }
        catch
        {
        }
    }



    protected void ddlcoll_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
          
            BindDegree();
            bindbranchadd();
            
        }
        catch
        {
        }
    }

   
}
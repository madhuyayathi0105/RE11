using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StudentConduct : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string examcodeval = string.Empty;
    string strgrade = string.Empty;
    string strsec = string.Empty;
    string curr_date = string.Empty;
    string sturollno = string.Empty;
    string strsubcrd = string.Empty;
    string graders = string.Empty;
    string sqlstr = string.Empty;
    string sturoll = string.Empty;

    Boolean cellroll = false;
    Boolean Cellclick = false;
    Boolean flag_true = false;

    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string sql = string.Empty;
    string grouporusercode = string.Empty;

    int dis = 0;
    int fin = 0;
    int sus = 0;
    int war = 0;
    int feeofroll = 0;
    int remarkval = 0;

    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet1();
    DataSet ds2 = new DataSet1();
    DataSet dsbind = new DataSet();
    DataTable dt = new DataTable();
    DAccess2 dset = new DAccess2();
    byte schoolOrCollege = 0;
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Convert.ToString(Session["usercode"]).Trim();
        collegecode = Convert.ToString(Session["collegecode"]).Trim();
        singleuser = Convert.ToString(Session["single_user"]).Trim();
        group_user = Convert.ToString(Session["group_code"]).Trim();

        errmsg.Visible = false;
        lblerrstaffcode.Visible = false;
        norecordlbl.Visible = false;

        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        Institution ins = new Institution(grouporusercode);
        setLabelText();
        schoolOrCollege = ins.TypeInstitute;

        if (!Page.IsPostBack)
        {
            txtfromdate.Visible = false;
            txttodate.Visible = false;
            lbltodat.Visible = false;
            lbldate.Visible = false;
            chkdate.Checked = false;
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            txtdate1.Attributes.Add("readonly", "readonly");
            txtstartdate.Attributes.Add("readonly", "readonly");
            txtFeeOnRollDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
            divFeeOnRollDate.Visible = false;
            lblErrMsg.Text = string.Empty;
            lblErrMsg.Visible = false;
            txtEndDate.Visible = false;
            lblEndDate.Visible = false;
            //end***//
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["AdmissionNo"] = "0";

            if (schoolOrCollege == 0)
            {
                divLeftAdmit.Visible = false;
                divRightAdmit.Visible = false;
                divLeftRoll.Visible = true;
                divRightRoll.Visible = true;
            }
            else
            {
                divLeftAdmit.Visible = true;
                divRightAdmit.Visible = true;
                divLeftRoll.Visible = false;
                divRightRoll.Visible = false;
            }
            //if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            //{
            //    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            //}
            //else
            //{
            //    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            //}
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            string Master1 = "select * from Master_Settings where " + grouporusercode + "";

            ds = dset.select_method(Master1, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "roll no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "register no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "admission no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                    {
                        Session["AdmissionNo"] = "1";
                    }
                }
            }

            //--------Spread Design Format-----------
            txtsturollno.Enabled = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;

            fsstaff.Sheets[0].AutoPostBack = true;

            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.CommandBar.Visible = true;
            FpSpread1.Sheets[0].SheetName = " ";
            FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
            FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

            //FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            //style1.Font.Size = 12;
            //style1.Font.Bold = false;
            //style1.HorizontalAlign = HorizontalAlign.Center;
            //style1.ForeColor = System.Drawing.Color.Black;
            //FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            //FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            //FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].AllowTableCorner = true;

            MyStyle.Font.Bold = true;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(MyStyle);
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(MyStyle);
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].AllowTableCorner = true;

            //---------------page number

            FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            FpSpread1.Pager.Align = HorizontalAlign.Right;
            FpSpread1.Pager.Font.Bold = false;
            FpSpread1.Pager.Font.Name = "Book Antiqua";
            FpSpread1.Pager.ForeColor = System.Drawing.Color.DarkGreen;
            FpSpread1.Pager.BackColor = System.Drawing.Color.Beige;
            FpSpread1.Pager.BackColor = System.Drawing.Color.AliceBlue;
            FpSpread1.Pager.PageCount = 5;
            FpSpread1.Visible = false;
            btnprint.Visible = false;
            panelrollnopop.Visible = false;
            norecordlbl.Visible = false;
            btnexcel.Visible = false;


            txtfromdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtFeeOnRollDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtEndDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            string startdaterload = DateTime.Now.ToString("dd-MM-yyyy");
            txtstartdate.Text = startdaterload;
            lblstartdate.Visible = false;
            lblfine.Visible = false;
            lblstartdate.Visible = false;
            lbldays.Visible = false;
            txtstartdate.Visible = false;
            txtdays.Visible = false;
            txtfine.Visible = false;

            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddldegree.Items.Count > 0)
            {
                ddldegree.Enabled = true;
                ddlbranch.Enabled = true;
                ddlsemester.Enabled = true;
                ddlsection.Enabled = true;
                txtfromdate.Enabled = true;
                txttodate.Enabled = true;
                btnadd.Enabled = true;
                btngo.Enabled = true;
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSectionDetail(strbatch, strbranch);
                BindSem(strbranch, strbatchyear, collegecode);
                // spreedbind();
                txtstdrollno.Enabled = true;
                txtsturollno.Enabled = true;
                loadinfarction();
            }
            else
            {
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
                ddlsemester.Enabled = false;
                ddlsection.Enabled = false;
                txtfromdate.Enabled = false;
                txttodate.Enabled = false;
                btnadd.Enabled = false;
                btngo.Enabled = false;
                txtsturollno.Enabled = false;
                txtstdrollno.Enabled = false;
            }
            fsstaff.Sheets[0].RowCount = 0;
            fsstaff.Sheets[0].AutoPostBack = false;
            fsstaff.Sheets[0].SheetCorner.RowCount = 1;
            fsstaff.Sheets[0].RowHeader.Visible = false;
            fsstaff.CommandBar.Visible = false;
            fsstaff.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fsstaff.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fsstaff.Sheets[0].DefaultStyle.Font.Bold = false;
            fsstaff.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;

            sprdselectrollno.Sheets[0].RowCount = 0;
            sprdselectrollno.Sheets[0].AutoPostBack = false;
            sprdselectrollno.Sheets[0].RowHeader.Visible = false;
            sprdselectrollno.CommandBar.Visible = false;
            sprdselectrollno.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            sprdselectrollno.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            sprdselectrollno.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            sprdselectrollno.Sheets[0].DefaultStyle.Font.Bold = false;
            ddlfraction.Attributes.Add("onfocus", "frelig()");
        }

    }

    //Load Batch Details...,
    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds2;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    // Load Degree Details...
    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            ddldegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds2;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();

            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    // Load Branch Details...
    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = ddldegree.SelectedValue.ToString();
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds2;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Semester Details-----
    public void BindSem(string strbranch, string strbatchyear, string collegecode)
    {

        try
        {
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsemester.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds2.Tables[0].Rows[0][1]).Trim());
                duration = Convert.ToInt32(Convert.ToString(ds2.Tables[0].Rows[0][0]).Trim());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Section Details-----
    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsection.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlsection.DataSource = ds2;
                ddlsection.DataTextField = "sections";
                ddlsection.DataBind();
                ddlsection.Items.Insert(0, "All");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]).Trim() == string.Empty)
                {
                    ddlsection.Enabled = false;
                }
                else
                {
                    ddlsection.Enabled = true;
                }
            }
            else
            {
                ddlsection.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    // Bind Bath pop-------
    public void BindBatchadd()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlbatchadd.DataSource = ds2;
                ddlbatchadd.DataTextField = "Batch_year";
                ddlbatchadd.DataValueField = "Batch_year";
                ddlbatchadd.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //-------load popup Degree---
    public void BindDegreepop(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            ddldegreeadd.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddldegreeadd.DataSource = ds2;
                ddldegreeadd.DataTextField = "course_name";
                ddldegreeadd.DataValueField = "course_id";
                ddldegreeadd.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    // -----Load Batch Pop--------
    public void BindBranchpop(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = ddldegreeadd.SelectedValue.ToString();
            ddlbrachadd.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlbrachadd.DataSource = ds2;
                ddlbrachadd.DataTextField = "dept_name";
                ddlbrachadd.DataValueField = "degree_code";
                ddlbrachadd.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //  -----load sem pop-
    public void BindSectionDetailpop(string strbatch, string strbranch)
    {
        try
        {
            strbatch = ddlbatchadd.SelectedValue.ToString();
            strbranch = ddlbrachadd.SelectedValue.ToString();

            ddlsecadd.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlsecadd.DataSource = ds2;
                ddlsecadd.DataTextField = "sections";
                ddlsecadd.DataBind();
                ddlsecadd.Items.Insert(0, "All");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsecadd.Enabled = false;
                    ddlsecadd.Items.Add("All");
                }
                else
                {
                    ddlsecadd.Enabled = true;

                }
            }
            else
            {
                ddlsecadd.Enabled = false;
                ddlsecadd.Items.Add("All");
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    // ----load sem pop---
    public void BindSempop(string strbranch, string strbatchyear, string collegecode)
    {
        try
        {
            strbatchyear = ddlbatchadd.Text.ToString();
            strbranch = ddlbrachadd.SelectedValue.ToString();

            ddlsemadd.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds2.Tables[0].Rows[0][1]).Trim());
                duration = Convert.ToInt32(Convert.ToString(ds2.Tables[0].Rows[0][0]).Trim());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsemadd.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemadd.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            norecordlbl.Visible = false;
            btnprint.Visible = false;
            BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            BindSem(strbranch, strbatchyear, collegecode);
            BindSectionDetail(strbatch, strbranch);
            FpSpread1.Visible = false;
            btnexcel.Visible = false;// added by srinath
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            norecordlbl.Visible = false;
            btnprint.Visible = false;
            BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            BindSem(strbranch, strbatchyear, collegecode);
            BindSectionDetail(strbatch, strbranch);
            FpSpread1.Visible = false;// Added By srinath 3/1/2013
            btnexcel.Visible = false;
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;// added by srinath
        norecordlbl.Visible = false;
        btnprint.Visible = false;
        btnexcel.Visible = false;

        if (!Page.IsPostBack == false)
        {
            //ddlsemester.Items.Clear();
        }
        try
        {
            if ((ddlbranch.SelectedIndex != 0) && (ddlbranch.SelectedIndex > 0))
            {
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            norecordlbl.Visible = false;
            FpSpread1.Visible = false;
            btnprint.Visible = false;
            btnexcel.Visible = false;//added by srinath
            if (!Page.IsPostBack == false)
            {
                ddlsection.Items.Clear();
            }
            BindSectionDetail(strbatch, strbranch);

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnprint.Visible = false;
        norecordlbl.Visible = false;
        FpSpread1.Visible = false;//add by srinath 3/1/13
        btnexcel.Visible = false;
    }

    protected void txtdate_TextChanged(object sender, EventArgs e)
    {
        btnprint.Visible = false;
        norecordlbl.Visible = false;
        FpSpread1.Visible = false;
        btnexcel.Visible = false;
        spreedbind();
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        if (chkdate.Checked == true)
        {
            if (txtfromdate.Text != "" && txttodate.Text != "")
            {
                spreedbind();
            }
            else
            {
                norecordlbl.Text = "Please Enter From date and To Date";
                norecordlbl.Visible = true;
            }
        }
        else
        {
            spreedbind();
        }
    }

    protected void txtsturollno_TextChanged(object sender, EventArgs e)
    {
        DataSet dsSettingsNew = new DataSet();
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " and  group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        if (!string.IsNullOrEmpty(grouporusercode))
        {
            string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
            dsSettingsNew = dset.select_method(Master1, hat, "Text");
        }
        if (txtsturollno.Text == "")
        {

        }
        else
        {
            string strstuqurey = "select stud_name,Roll_No from registration where roll_no='" + txtsturollno.Text + "'";
            DataSet dsroll = d2.select_method_wo_parameter(strstuqurey, "Text");
            if (dsroll.Tables.Count > 0 && dsroll.Tables[0].Rows.Count > 0)
            {
                if (chkdate.Checked == true)
                {
                    DateTime dtFromDate = new DateTime();
                    bool isFrom = DateTime.TryParseExact(txtfromdate.Text.Trim(), "dd-MM-yyyy", null, DateTimeStyles.None, out dtFromDate);
                    DateTime dtToDate = new DateTime();
                    bool isTo = DateTime.TryParseExact(txttodate.Text.Trim(), "dd-MM-yyyy", null, DateTimeStyles.None, out dtToDate);
                    if (txtfromdate.Text == "" || txttodate.Text == "")
                    {
                        curr_date = string.Empty;
                    }
                    else if (isFrom && isTo)
                    {
                        curr_date = " and curr_date Between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "'";
                    }
                }
                //r.app_no,r.Reg_no,r.college_code,r.roll_no,r.stud_name,r.current_semester,convert(varchar(15),curr_date,103) as curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,ack_remarks,s.Remark
                FpSpread1.Sheets[0].SheetName = " ";
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 8;
                string bindspread = "select r.app_no,r.Reg_no,r.Roll_Admit,r.college_code,r.roll_no,r.stud_name,r.current_semester,convert(varchar(15),curr_date,103) as curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,ack_remarks,s.Remark,convert(varchar(50),feeOnRollDate,105) feeOnRollDate,convert(varchar(50),suspendFromDate,105) as suspendFromDate,convert(varchar(50),suspendToDate,105) as suspendToDate from stucon s,Registration r where  r.Roll_No=s.roll_no and cc=0 and delflag=0 and exam_flag!='debar' and  s.roll_no in(select distinct roll_no from registration where roll_no='" + txtsturollno.Text + "') " + curr_date + "  order by ack_date asc ";
                DataSet dsbindspread = d2.select_method_wo_parameter(bindspread, "Text");
                if (dsbindspread.Tables.Count > 0 && dsbindspread.Tables[0].Rows.Count > 0)
                {
                    string action = string.Empty;
                    int sno = 0;
                    for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
                    {
                        sno++;
                        action = string.Empty;
                        string days = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["tot_days"]).Trim();
                        string appNo = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["app_no"]).Trim();
                        string collegeCode = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["college_code"]).Trim();
                        string rollno = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["roll_no"]).Trim();
                        string dismis = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_diss"]).Trim();
                        string susp = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_susp"]).Trim();
                        string fin = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_fine"]).Trim();
                        string warn = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_warn"]).Trim();
                        string staffcode = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["prof_code"]).Trim();
                        string feeofroll = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_fee_of_roll"]).Trim();
                        string remarks = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["Remark"]).Trim();
                        string remaaction = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_remarks"]).Trim();
                        string currentSemester = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["current_semester"]).Trim();
                        string feeOnRollDate = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["feeOnRollDate"]).Trim();
                        string SuspendStartDate = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["suspendFromDate"]).Trim();
                        string SuspendEndDate = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["suspendToDate"]).Trim();

                        DateTime dtFeeOnRollDate = new DateTime();
                        DateTime dtSuspendStartDate = new DateTime();
                        DateTime dtSuspendEndDate = new DateTime();
                        bool isFeeOnRoll = DateTime.TryParseExact(feeOnRollDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFeeOnRollDate);
                        bool isSuspendStart = DateTime.TryParseExact(SuspendStartDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendStartDate);
                        bool isSuspendEnd = DateTime.TryParseExact(SuspendEndDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendEndDate);

                        if (dismis != "0")
                        {
                            if (action != "")
                            {
                                action = action + " / " + "Dismissal";
                            }
                            else
                            {
                                action = "Dismissal";
                            }
                        }
                        if (susp != "0")
                        {
                            if (action != "")
                            {
                                action = action + " / " + "Suspension";
                            }
                            else
                            {
                                action = "Suspension";
                            }
                        }
                        if (fin != "0")
                        {
                            if (action != "")
                            {
                                action = action + " /  " + "Fine";
                            }
                            else
                            {
                                action = "Fine";
                            }
                        }
                        if (warn != "0")
                        {
                            if (action != "")
                            {
                                action = action + " / " + "Warning";
                            }
                            else
                            {
                                action = "Warning";
                            }
                        }
                        if (feeofroll != "0" && feeofroll != "")
                        {
                            if (action != "")
                            {
                                action = action + " / " + "Fee Off The Roll";
                            }
                            else
                            {
                                action = "Fee Off The Roll";
                            }
                        }
                        else
                        {
                            if (isFeeOnRoll)
                            {
                                if (action != "")
                                {
                                    action = action + " / " + "Fee On The Roll";
                                }
                                else
                                {
                                    action = "Fee On The Roll";
                                }
                            }
                        }
                        if (remaaction != "0" && remaaction != "")
                        {
                            //if (action != "")
                            //{
                            //    action = action + " / " + " Remarks";
                            //}
                            //else
                            //{
                            //    action = "Remarks";
                            //}
                        }

                        FpSpread1.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Columns[1].Label = "Student Name";
                        FpSpread1.Sheets[0].ColumnHeader.Columns[2].Label = "Date";
                        FpSpread1.Sheets[0].ColumnHeader.Columns[3].Label = "Infraction";
                        FpSpread1.Sheets[0].ColumnHeader.Columns[4].Label = "Action Taken";
                        FpSpread1.Sheets[0].ColumnHeader.Columns[5].Label = "Days";
                        FpSpread1.Sheets[0].ColumnHeader.Columns[6].Label = "Staff Name";
                        FpSpread1.Sheets[0].ColumnHeader.Columns[7].Label = "Remark";

                        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

                        FpSpread1.Sheets[0].Columns[0].Width = 50;
                        FpSpread1.Sheets[0].Columns[1].Width = 150;
                        FpSpread1.Sheets[0].Columns[2].Width = 80;
                        FpSpread1.Sheets[0].Columns[3].Width = 150;
                        FpSpread1.Sheets[0].Columns[4].Width = 150;
                        FpSpread1.Sheets[0].Columns[5].Width = 50;
                        FpSpread1.Sheets[0].Columns[6].Width = 180;
                        FpSpread1.Sheets[0].Columns[7].Width = 250;

                        for (int i = 0; i < 7; i++)
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
                        }
                        string name = Convert.ToString(dsroll.Tables[0].Rows[0]["stud_name"]).Trim();
                        string Roll_No = Convert.ToString(dsroll.Tables[0].Rows[0]["Roll_No"]).Trim();
                        string regNo = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["Reg_no"]).Trim();
                        string admitNo = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["Roll_Admit"]).Trim();

                        string stfname = d2.GetFunction("select staff_name as staff_name from staffmaster where staff_code='" + staffcode + "' ");
                        string staff = string.Empty;
                        if (stfname.Trim() != "" && stfname.Trim() != "0")
                        {
                            staff = stfname;
                        }

                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Bold = false;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(currentSemester);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = collegeCode;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["curr_date"]).Trim();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["infr_type"]).Trim();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        if (action == "")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "-";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = action;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        }

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = days;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        string studentName = string.Empty;
                        if (ColumnHeaderVisiblity(0, dsSettingsNew))
                        {
                            if (string.IsNullOrEmpty(studentName))
                            {
                                studentName = Roll_No;
                            }
                            else
                            {
                                studentName += " - " + Roll_No;
                            }
                        }
                        if (ColumnHeaderVisiblity(1, dsSettingsNew))
                        {
                            if (string.IsNullOrEmpty(studentName))
                            {
                                studentName = regNo;
                            }
                            else
                            {
                                studentName += " - " + regNo;
                            }
                        }
                        if (ColumnHeaderVisiblity(2, dsSettingsNew))
                        {
                            if (string.IsNullOrEmpty(studentName))
                            {
                                studentName = admitNo;
                            }
                            else
                            {
                                studentName += " - " + admitNo;
                            }
                        }
                        studentName += "-" + name;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = studentName;// Roll_No + "-" + name;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Roll_No.Trim();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = appNo.Trim();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        if (staff == "")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "-";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = stfname;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        }

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = remarks;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    FpSpread1.Visible = true;
                    btnprint.Visible = true;
                    btnexcel.Visible = true;
                    norecordlbl.Text = string.Empty;
                }
                else
                {
                    FpSpread1.Visible = false;
                    btnprint.Visible = false;
                    norecordlbl.Visible = true;
                    norecordlbl.Text = "No Records Found";
                    btnexcel.Visible = false;
                }
                int rowcount = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Height = 300;
                FpSpread1.Sheets[0].PageSize = 25 + (rowcount * 20);
                FpSpread1.SaveChanges();
            }
            else
            {
                FpSpread1.Visible = false;
                btnprint.Visible = false;
                btnexcel.Visible = false;
                norecordlbl.Visible = true;
                norecordlbl.Text = "Please Enter Valid Roll No";
            }
        }
    }

    public void spreedbind()
    {
        DataSet dsSettingsNew = new DataSet();
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " and  group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        if (!string.IsNullOrEmpty(grouporusercode))
        {
            string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
            dsSettingsNew = dset.select_method(Master1, hat, "Text");
        }

        if (ddlsection.Items.Count > 0)
        {
            if (Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == string.Empty || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "-1")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + Convert.ToString(ddlsection.SelectedItem.Text).Trim() + "'";
            }
        }
        else
        {
            strsec = string.Empty;
        }
        if (chkdate.Checked == true)
        {
            string fromdate = string.Empty;
            fromdate = txtfromdate.Text.ToString();
            string[] splitfrom = fromdate.Split(new Char[] { '-' });

            int splitfromdate = Convert.ToInt32(splitfrom[0]);
            int splitfrommonth = Convert.ToInt32(splitfrom[1]);
            int splitfromyear = Convert.ToInt32(splitfrom[2]);

            string fromdatego = splitfrommonth + "-" + splitfromdate + "-" + splitfromyear;

            string todate = string.Empty;
            todate = txttodate.Text.ToString();
            string[] splitto = todate.Split(new Char[] { '-' });

            int splittodate = Convert.ToInt32(splitto[0]);
            int splittomonth = Convert.ToInt32(splitto[1]);
            int splittoyear = Convert.ToInt32(splitto[2]);
            string todatego = splittomonth + "-" + splittodate + "-" + splittoyear;

            if (txtfromdate.Text == "" || txttodate.Text == "")
            {
                curr_date = string.Empty;
            }
            else
            {
                curr_date = " and curr_date Between '" + fromdatego + "' and '" + todatego + "'";
            }
        }

        //added By Srinath 15/8/2013
        string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
        string strorder = ",len(r.roll_no)";
        if (orderby_Setting == "0")
        {
            strorder = ",len(r.roll_no)";
        }
        else if (orderby_Setting == "1")
        {
            strorder = ",len(r.Reg_No)";
        }
        else if (orderby_Setting == "2")
        {
            strorder = ",r.Stud_Name";
        }
        else if (orderby_Setting == "0,1,2")
        {
            strorder = ",len(r.roll_no),len(r.Reg_No),r.stud_name";
        }
        else if (orderby_Setting == "0,1")
        {
            strorder = ",len(r.roll_no),len(registration.Reg_No)";
        }
        else if (orderby_Setting == "1,2")
        {
            strorder = ",len(r.Reg_No),r.Stud_Name";
        }
        else if (orderby_Setting == "0,2")
        {
            strorder = ",len(r.roll_no),r.Stud_Name";
        }
        FpSpread1.Sheets[0].SheetName = " ";
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 8;
        string bindspread = "select r.app_no,r.Reg_no,r.Roll_Admit,r.college_code,r.roll_no,r.stud_name,r.current_semester,convert(varchar(15),curr_date,103) as curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,convert(varchar(10),convert(datetime, ack_date),105) as ack_date,tot_days,fine_amo,ack_fee_of_roll,ack_remarks,s.Remark,convert(varchar(50),feeOnRollDate,105) feeOnRollDate,convert(varchar(50),suspendFromDate,105) as suspendFromDate,convert(varchar(50),suspendToDate,105) as suspendToDate,StuConID from stucon s,Registration r where  r.Roll_No=s.roll_no and cc=0 and delflag=0 and exam_flag!='debar' and batch_year='" + ddlbatch.SelectedValue + "'  and degree_code='" + ddlbranch.SelectedValue + "' and current_semester='" + ddlsemester.SelectedValue + "' " + strsec + " " + curr_date + " order by curr_date asc " + strorder + "";
        DataSet dsbindspread = d2.select_method_wo_parameter(bindspread, "Text");
        if (dsbindspread.Tables.Count > 0 && dsbindspread.Tables[0].Rows.Count > 0)
        {
            string action = string.Empty;
            int sno = 0;
            FpSpread1.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Columns[1].Label = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Columns[2].Label = "Date";
            FpSpread1.Sheets[0].ColumnHeader.Columns[3].Label = "Infraction";
            FpSpread1.Sheets[0].ColumnHeader.Columns[4].Label = "Action Taken";
            FpSpread1.Sheets[0].ColumnHeader.Columns[5].Label = "Days";
            FpSpread1.Sheets[0].ColumnHeader.Columns[6].Label = "Staff Name";
            FpSpread1.Sheets[0].ColumnHeader.Columns[7].Label = "Remark";

            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

            FpSpread1.Sheets[0].Columns[0].Width = 50;
            FpSpread1.Sheets[0].Columns[1].Width = 150;
            FpSpread1.Sheets[0].Columns[2].Width = 80;
            FpSpread1.Sheets[0].Columns[3].Width = 150;
            FpSpread1.Sheets[0].Columns[4].Width = 150;
            FpSpread1.Sheets[0].Columns[5].Width = 50;
            FpSpread1.Sheets[0].Columns[6].Width = 180;
            FpSpread1.Sheets[0].Columns[7].Width = 250;
            //**added by annyutha 2nd sep 2014*//
            for (int i = 0; i < 7; i++)
            {
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
            }
            //***** end*****//

            for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
            {
                action = string.Empty;
                sno++;
                string days = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["tot_days"]).Trim();
                string appNo = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["app_no"]).Trim();

                string Identity = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["StuConID"]).Trim();

                string collegeCode = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["college_code"]).Trim();
                string rollno = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["roll_no"]).Trim();
                string dismis = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_diss"]).Trim();
                string susp = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_susp"]).Trim();
                string fin = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_fine"]).Trim();
                string warn = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_warn"]).Trim();
                string staffcode = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["prof_code"]).Trim();
                string feeofroll = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_fee_of_roll"]).Trim();
                string remarks = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["Remark"]).Trim();
                string remaaction = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_remarks"]).Trim();
                string currentSemester = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["current_semester"]).Trim();
                string feeOnRollDate = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["feeOnRollDate"]).Trim();
                string SuspendStartDate = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["suspendFromDate"]).Trim();
                string SuspendEndDate = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["suspendToDate"]).Trim();
                string ActualDate = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["ack_date"]).Trim();

                DateTime dtFeeOnRollDate = new DateTime();
                DateTime dtSuspendStartDate = new DateTime();
                DateTime dtSuspendEndDate = new DateTime();
                DateTime dtActualDate = new DateTime();
                bool isFeeOnRoll = DateTime.TryParseExact(feeOnRollDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFeeOnRollDate);
                bool isSuspendStart = DateTime.TryParseExact(SuspendStartDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendStartDate);
                bool isSuspendEnd = DateTime.TryParseExact(SuspendEndDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendEndDate);
                bool isactual = DateTime.TryParseExact(ActualDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtActualDate);
                TimeSpan t = new TimeSpan();
                int day = 0;
                if (dismis != "0")
                {
                    if (action != "")
                    {
                        action = action + " / " + "Dismissal";
                    }
                    else
                    {
                        action = "Dismissal";
                    }
                }
                if (susp != "0")
                {
                    if (action != "")
                    {
                        action = action + " / " + "Suspension";
                    }
                    else
                    {
                        action = "Suspension";
                    }
                }
                if (fin != "0")
                {
                    if (action != "")
                    {
                        action = action + " /  " + "Fine";
                    }
                    else
                    {
                        action = "Fine";
                    }
                }
                if (warn != "0")
                {
                    if (action != "")
                    {
                        action = action + " / " + "Warning";
                    }
                    else
                    {
                        action = "Warning";
                    }
                }
                if (feeofroll != "0" && feeofroll != "")
                {
                    if (action != "")
                    {
                        action = action + " / " + "Fee Off The Roll";
                    }
                    else
                    {
                        action = "Fee Off The Roll";
                    }
                }
                else
                {
                    if (isFeeOnRoll)
                    {
                        if (action != "")
                        {
                            action = action + " / " + "Fee On The Roll";
                        }
                        else
                        {
                            action = "Fee On The Roll";
                        }
                        t = dtFeeOnRollDate - dtActualDate;
                        day = t.Days;
                        days = Convert.ToString(day + 1);
                    }
                }
                if (remaaction != "0" && remaaction != "")
                {

                }

                string name = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["stud_name"]).Trim();
                string Roll_No = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["Roll_No"]).Trim();
                string regNo = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["Reg_no"]).Trim();
                string admitNo = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["Roll_Admit"]).Trim();
                //r.Reg_no,r.Roll_Admit
                string stfname = d2.GetFunction("select staff_name as staff_name from staffmaster where staff_code='" + staffcode + "'");
                string staff = string.Empty;
                if (stfname.Trim() != "" && stfname.Trim() != "0")
                {
                    staff = Convert.ToString(stfname);
                }

                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Bold = false;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(currentSemester);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = collegeCode;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                //string currdateformete = dsbindspread.Tables[0].Rows[rolcount]["convert(varchar(15),curr_date,103) as curr_date"].ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["curr_date"]).Trim();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(Identity);

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsbindspread.Tables[0].Rows[rolcount]["infr_type"]).Trim();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                if (action == "")
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "-";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                }
                else
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = action;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                }

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = days;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                string studentName = string.Empty;
                if (ColumnHeaderVisiblity(0, dsSettingsNew))
                {
                    if (string.IsNullOrEmpty(studentName))
                    {
                        studentName = Roll_No;
                    }
                    else
                    {
                        studentName += " - " + Roll_No;
                    }
                }
                if (ColumnHeaderVisiblity(1, dsSettingsNew))
                {
                    if (string.IsNullOrEmpty(studentName))
                    {
                        studentName = regNo;
                    }
                    else
                    {
                        studentName += " - " + regNo;
                    }
                }
                if (ColumnHeaderVisiblity(2, dsSettingsNew))
                {
                    if (string.IsNullOrEmpty(studentName))
                    {
                        studentName = admitNo;
                    }
                    else
                    {
                        studentName += " - " + admitNo;
                    }
                }
                studentName += "-" + name;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = studentName;// Roll_No + "-" + name;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Roll_No.Trim();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = appNo.Trim();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                if (staff == "")
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "-";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                }
                else
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = stfname;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                }

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = remarks;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                txtsturollno.Text = string.Empty;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            FpSpread1.Visible = true;
            btnprint.Visible = true;
            btnexcel.Visible = true;
            norecordlbl.Text = string.Empty;
        }
        else
        {
            FpSpread1.SaveChanges();
            FpSpread1.Visible = false;
            btnprint.Visible = false;
            norecordlbl.Visible = true;
            norecordlbl.Text = "No Records Found";
            btnexcel.Visible = false;
        }
        int rowcount = FpSpread1.Sheets[0].RowCount;
        FpSpread1.Height = 300;
        FpSpread1.Width = 950;
        FpSpread1.Sheets[0].PageSize = 25 + (rowcount * 20);
        FpSpread1.SaveChanges();
    }

    protected void ddlbatch_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        sprdselectrollno.Sheets[0].PageSize = sprdselectrollno.Sheets[0].RowCount;
        sprdselectrollno.SaveChanges();
        BindDegreepop(singleuser, group_user, collegecode, usercode);
        BindBranchpop(singleuser, group_user, course_id, collegecode, usercode);
        BindSempop(strbranch, strbatchyear, collegecode);
        BindSectionDetailpop(strbatch, strbranch);
        sprdrollbind();
    }

    protected void ddldegree_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        sprdselectrollno.Sheets[0].PageSize = sprdselectrollno.Sheets[0].RowCount;
        sprdselectrollno.SaveChanges();
        BindBranchpop(singleuser, group_user, course_id, collegecode, usercode);
        BindSectionDetailpop(strbatch, strbranch);
        BindSempop(strbranch, strbatchyear, collegecode);
        sprdrollbind();
    }

    protected void txtdate1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string currentdate = Convert.ToString(DateTime.Now);
            DateTime current = Convert.ToDateTime(currentdate);
            int curday = current.Day;
            int curmonth = current.Month;
            int curyear = current.Year;

            string date1 = string.Empty;
            date1 = txtdate1.Text.ToString();
            string[] split = date1.Split(new Char[] { '-' });
            int splitdate = Convert.ToInt32(split[0]);
            int splitmonth = Convert.ToInt32(split[1]);
            int splityear = Convert.ToInt32(split[2]);

            //if (curyear > splityear)
            //{
            //    errmsg.Visible = false;
            //}
            //else if (curyear == splityear)
            //{
            //    if (curmonth > splitmonth)
            //    {
            //        errmsg.Visible = false;
            //    }
            //    else if (curmonth == splitmonth)
            //    {
            //        if (curday >= splitdate)
            //        {
            //            errmsg.Visible = false;
            //        }
            //        else
            //        {
            //            errmsg.Visible = true;
            //            errmsg.Text = "Please Enter Correct Date";
            //            txtdate1.Text = string.Empty;
            //        }
            //    }
            //    else
            //    {
            //        errmsg.Visible = true;
            //        errmsg.Text = "Please Enter Correct Date";
            //        txtdate1.Text = string.Empty;
            //    }
            //}
            //else
            //{
            //    errmsg.Visible = true;
            //    errmsg.Text = "Please Enter Correct Date";
            //    txtdate1.Text = string.Empty;
            //}
        }
        catch
        {
        }
    }

    protected void ddlbranch_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        sprdselectrollno.Sheets[0].PageSize = sprdselectrollno.Sheets[0].RowCount;
        sprdselectrollno.SaveChanges();
        BindSectionDetailpop(strbatch, strbranch);
        BindSempop(strbranch, strbatchyear, collegecode);
        BindSectionDetailpop(strbatch, strbranch);
        sprdrollbind();
    }

    protected void ddldegreeadd_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        sprdselectrollno.Sheets[0].PageSize = sprdselectrollno.Sheets[0].RowCount;
        sprdselectrollno.SaveChanges();
        BindBranchpop(singleuser, group_user, course_id, collegecode, usercode);
        BindSectionDetailpop(strbatch, strbranch);
        BindSempop(strbranch, strbatchyear, collegecode);
        sprdrollbind();
    }

    protected void ddlbrachadd_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        sprdselectrollno.Sheets[0].PageSize = sprdselectrollno.Sheets[0].RowCount;
        sprdselectrollno.SaveChanges();
        BindSectionDetailpop(strbatch, strbranch);
        BindSempop(strbranch, strbatchyear, collegecode);
        sprdrollbind();
    }

    protected void ddlsem_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        sprdselectrollno.Sheets[0].PageSize = sprdselectrollno.Sheets[0].RowCount;
        sprdselectrollno.SaveChanges();
        BindSectionDetailpop(strbatch, strbranch);
        sprdrollbind();
    }

    protected void ddlsec_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        sprdselectrollno.Sheets[0].PageSize = sprdselectrollno.Sheets[0].RowCount;
        sprdselectrollno.SaveChanges();
        sprdrollbind();
    }

    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        btnprint.Visible = false;
        sprdselectrollno.Visible = true;
        chkfeeonroll.Visible = false;
        chkfeeonroll.Checked = false;
        //****added by annyutha**2nd sep 2014**//
        btnexcel.Visible = false;
        //*end****//
        ddlbatchadd.Enabled = true;
        ddldegreeadd.Enabled = true;
        ddlbrachadd.Enabled = true;
        ddlsecadd.Enabled = true;
        ddlsemadd.Enabled = true;

        panelrollnopop.Visible = true;
        errmsg.Visible = false;
        errmsg.Text = string.Empty;
        norecordlbl.Visible = false;
        sprdselectrollno.Sheets[0].RowCount = 0;
        sprdselectrollno.Sheets[0].PageSize = sprdselectrollno.Sheets[0].RowCount;
        sprdselectrollno.SaveChanges();
        BindBatchadd();
        BindDegreepop(singleuser, group_user, collegecode, usercode);
        BindBranchpop(singleuser, group_user, course_id, collegecode, usercode);
        BindSectionDetailpop(strbatch, strbranch);
        BindSempop(strbranch, strbatchyear, collegecode);
        loadinfarction();
        sprdrollbind();
        txtdate1.Text = DateTime.Now.ToString("dd-MM-yyyy");
        txtremarks.Text = string.Empty;
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
        clear();
        // txtstartdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        //if (ddlfraction.SelectedIndex == -1 || ddlfraction.SelectedIndex == 0)
        //{
        //    btnfractiobremove.Visible = true;
        //    btnaddfraction.Visible = true;
        //}
        //else
        //{
        //    btnfractiobremove.Visible = false;
        //    btnaddfraction.Visible = false;
        //}

        //sankar modify-------------May'27

        //lblstartdate.Visible = false;
        //lblfine.Visible = false;
        //lblstartdate.Visible = false;
        //lbldays.Visible = false;
        //txtstartdate.Visible = false;
        //txtdays.Visible = false;
        //txtfine.Visible = false;
        //txtdays.Text =string.Empty;
        //txtfine.Text =string.Empty;
        //txtstaff.Text =string.Empty;
        //chkdismissal.Checked = false;
        //chkfine.Checked = false;
        //chkwarning.Checked = false;
        //chksuspension.Checked = false;
        //btnexcel.Visible = false;
        ddlfraction.Attributes.Add("onfocus", "frelig()");
    }

    public void sprdrollbind()
    {
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
        DataSet dsSettingsNew = new DataSet();
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " and  group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        if (!string.IsNullOrEmpty(grouporusercode))
        {
            string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
            dsSettingsNew = dset.select_method(Master1, hat, "Text");
        }

        sprdselectrollno.Sheets[0].RowCount = 0;
        sprdselectrollno.Sheets[0].ColumnCount = 7;
        sprdselectrollno.SaveChanges();
        FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
        sprdselectrollno.Sheets[0].Columns[5].CellType = chkcell;

        sprdselectrollno.Sheets[0].RowCount = sprdselectrollno.Sheets[0].RowCount + 1;
        sprdselectrollno.Sheets[0].SpanModel.Add(sprdselectrollno.Sheets[0].RowCount - 1, 0, 1, 5);

        sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 5].CellType = chkcell;
        sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
        sprdselectrollno.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
        sprdselectrollno.Sheets[0].AutoPostBack = true;

        string strsec = string.Empty;
        if (ddlsecadd.Items.Count > 0)
        {
            if (Convert.ToString(ddlsecadd.SelectedValue).ToLower().Trim() != "all" && Convert.ToString(ddlsecadd.SelectedValue).ToLower().Trim() != "" && Convert.ToString(ddlsecadd.SelectedValue).ToLower().Trim() != "-1")
            {
                strsec = "and sections='" + Convert.ToString(ddlsecadd.SelectedValue).Trim() + "'";

            }
            else
            {
                strsec = string.Empty;
            }
        }
        else
        {
            strsec = string.Empty;
        }
        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();

        sprdselectrollno.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].Label = "Roll No";
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].Label = "Reg No";
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[3].Label = "Admission No";
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[4].Label = "Name";
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[5].Label = "Select";
       // sprdselectrollno.Sheets[0].ColumnHeader.Columns[5].CellType = textcel_type;
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[4].CellType = textcel_type;
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[3].CellType = textcel_type;
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].CellType = textcel_type;
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].CellType = textcel_type;
        sprdselectrollno.Sheets[0].Columns[0].Width = 60;
        sprdselectrollno.Sheets[0].Columns[1].Width = 140;
        sprdselectrollno.Sheets[0].Columns[2].Width = 140;
        sprdselectrollno.Sheets[0].Columns[3].Width = 140;
        sprdselectrollno.Sheets[0].Columns[4].Width = 260;
        sprdselectrollno.Sheets[0].Columns[5].Width = 60;
        sprdselectrollno.Sheets[0].Columns[6].Width = 1;
        sprdselectrollno.Sheets[0].Columns[0].Font.Bold = false;

        //if (Session["Rollflag"].ToString() == "0")
        //{
        //    sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].Visible = false;
        //}
        //else
        //{
        //    sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].Visible = true;
        //}
        //if (Session["Regflag"].ToString() == "0")
        //{
        //    sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].Visible = false;
        //}
        //else
        //{
        //    sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].Visible = true;
        //}
        //if (Session["AdmissionNo"].ToString() == "0")
        //{
        //    sprdselectrollno.Sheets[0].ColumnHeader.Columns[3].Visible = false;
        //}
        //else
        //{
        //    sprdselectrollno.Sheets[0].ColumnHeader.Columns[3].Visible = true;
        //}

        //if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "1")
        //{
        //    sprdselectrollno.Sheets[0].Columns[2].Width = 220;
        //    sprdselectrollno.Sheets[0].Columns[4].Width = 320;
        //}
        //else if (Session["Rollflag"].ToString() == "1" && Session["Regflag"].ToString() == "0")
        //{
        //    sprdselectrollno.Sheets[0].Columns[1].Width = 220;
        //    sprdselectrollno.Sheets[0].Columns[4].Width = 320;
        //}
        //else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "0")
        //{
        //    sprdselectrollno.Sheets[0].Columns[4].Width = 540;
        //}

        sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].Visible = ColumnHeaderVisiblity(0, dsSettingsNew);
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].Visible = ColumnHeaderVisiblity(1, dsSettingsNew);
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[3].Visible = ColumnHeaderVisiblity(2, dsSettingsNew);

        if (!sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].Visible && !sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].Visible && sprdselectrollno.Sheets[0].ColumnHeader.Columns[3].Visible)
        {
            ;
            sprdselectrollno.Sheets[0].Columns[3].Width = 220;
            sprdselectrollno.Sheets[0].Columns[4].Width = 320;
        }
        if (!sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].Visible && sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].Visible && !sprdselectrollno.Sheets[0].ColumnHeader.Columns[3].Visible)
        {
            sprdselectrollno.Sheets[0].Columns[2].Width = 220;
            sprdselectrollno.Sheets[0].Columns[4].Width = 320;
        }
        else if (sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].Visible && !sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].Visible && !sprdselectrollno.Sheets[0].ColumnHeader.Columns[3].Visible)
        {
            sprdselectrollno.Sheets[0].Columns[1].Width = 220;
            sprdselectrollno.Sheets[0].Columns[4].Width = 320;
        }
        else if (!sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].Visible && !sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].Visible && !sprdselectrollno.Sheets[0].ColumnHeader.Columns[3].Visible)
        {
            sprdselectrollno.Sheets[0].Columns[4].Width = 540;
        }

        Boolean serialflag = false;
        string strorder = "ORDER BY len(registration.roll_no),registration.roll_no";
        string strserial = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
        if (strserial != "" && strserial != "0" && strserial != null)
        {
            serialflag = true;
            strorder = "ORDER BY registration.serialno";
        }
        else
        {
            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");

            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY len(registration.roll_no),registration.roll_no";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Reg_No,registration.stud_name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Stud_Name";
            }
        }

        //string getrollnoquery = "select roll_no,reg_no as reg_no,stud_name,current_semester from registration where batch_year=" + ddlbatchadd.SelectedValue.ToString() + " and current_semester=" + ddlsemadd.SelectedValue.ToString() + " " + strsec + " and   degree_code='" + ddlbrachadd.SelectedValue.ToString() + "' order by roll_no";
        string getrollnoquery = "select roll_no,reg_no as reg_no,stud_name,current_semester,Roll_Admit,college_code,app_no from registration where cc=0 and delflag=0 and exam_flag<>'Debar' and batch_year=" + ddlbatchadd.SelectedValue.ToString() + " and current_semester=" + ddlsemadd.SelectedValue.ToString() + " " + strsec + " and   degree_code='" + ddlbrachadd.SelectedValue.ToString() + "' " + strorder + "";
        DataSet dsgetrollnoquery = d2.select_method_wo_parameter(getrollnoquery, "Text");
        // sprdselectrollno.Sheets[0].RowCount = 0;
        if (dsgetrollnoquery.Tables.Count > 0 && dsgetrollnoquery.Tables[0].Rows.Count > 0)
        {
            lblnorec.Visible = false;
            int sno = 0;
            for (int rollnocount = 0; rollnocount < dsgetrollnoquery.Tables[0].Rows.Count; rollnocount++)
            {
                sno++;
                string rollno = Convert.ToString(dsgetrollnoquery.Tables[0].Rows[rollnocount]["roll_no"]).Trim();
                string regno = Convert.ToString(dsgetrollnoquery.Tables[0].Rows[rollnocount]["reg_no"]).Trim();
                string currentSemester = Convert.ToString(dsgetrollnoquery.Tables[0].Rows[rollnocount]["current_semester"]).Trim();
                string appNo = Convert.ToString(dsgetrollnoquery.Tables[0].Rows[rollnocount]["app_no"]).Trim();
                string collegeCode = Convert.ToString(dsgetrollnoquery.Tables[0].Rows[rollnocount]["college_code"]).Trim();
                sprdselectrollno.Sheets[0].RowCount++;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 0].Tag = currentSemester;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 0].Font.Bold = false;

                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsgetrollnoquery.Tables[0].Rows[rollnocount]["roll_no"]).Trim();
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 1].Tag = collegeCode;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 1].Note = appNo;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                sprdselectrollno.Sheets[0].Rows[(sprdselectrollno.Sheets[0].RowCount - 1)].CellType = textcel_type;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 1].CellType = textcel_type;
                if (regno == "")
                {
                    sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 2].Text = "-";
                    sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 2].Text = regno;
                    sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                }
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 2].CellType = textcel_type;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 2].Font.Bold = false;


                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsgetrollnoquery.Tables[0].Rows[rollnocount]["Roll_Admit"]).Trim();
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].Font.Bold = false;

                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].CellType = textcel_type;

                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsgetrollnoquery.Tables[0].Rows[rollnocount]["stud_name"]).Trim();
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 4].Font.Bold = false;

                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 4].CellType = textcel_type;

                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 5].CellType = chkcell1;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 5].Font.Bold = false;

                sprdselectrollno.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dsgetrollnoquery.Tables[0].Rows[rollnocount]["current_semester"]).Trim();
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                chkcell.AutoPostBack = true;
                chkcell1.AutoPostBack = true;
            }
        }
        else
        {
            //lblnorec.Visible = true;
            //lblnorec.Text = "No Records Found";
        }

        sprdselectrollno.Sheets[0].Columns[5].CellType = chkcell;
        int rowcount = sprdselectrollno.Sheets[0].RowCount;
        sprdselectrollno.Height = 300;
        sprdselectrollno.Sheets[0].PageSize = 25 + (rowcount * 20);
        sprdselectrollno.SaveChanges();
        //loadinfarction();
        sprdselectrollno.Sheets[0].Columns[0].Font.Bold = false;
        sprdselectrollno.SaveChanges();
        sprdselectrollno.Sheets[0].AutoPostBack = false;
    }

    protected void sprdselectrollno_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellroll = true;
    }

    protected void sprdselectrollno_SelectedIndexChanged(Object sender, EventArgs e)
    {
        //lblErrMsg.Visible = false;
        //lblErrMsg.Text = string.Empty;
        if (cellroll == true)
        {
            lblErrMsg.Visible = false;
            lblErrMsg.Text = string.Empty;
            cellroll = false;
            int flag = 0;
            string rollNo = string.Empty;
            string rollAdmit = string.Empty;
            int count = 0;
            for (int res = 1; res <= Convert.ToInt32(sprdselectrollno.Sheets[0].RowCount) - 1; res++)
            {
                int isval = 0;
                string s = sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 5].Text;
                //sankar added may'20...............................
                string roll_no = sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 1].Text;
                string staff_name = sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 4].Text;
                isval = Convert.ToInt32(sprdselectrollno.Sheets[0].Cells[res, 5].Value);
                if (isval == 0 || s == "")
                {

                }
                else
                {
                    rollNo = Convert.ToString(sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 1].Text).Trim();
                    rollAdmit = Convert.ToString(sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 3].Text).Trim();
                    count++;
                    //txtstdrollno.Enabled = false;
                    //txtstdrollno.Text =string.Empty;
                    //if (divLeftRoll.Visible == true && divRightRoll.Visible == true)
                    if (schoolOrCollege == 0)
                    {
                        txtstdrollno.Enabled = false;
                        txtstdrollno.Text = string.Empty;
                    }
                    else if (schoolOrCollege == 1)
                    {
                        txtAdmissionNo.Text = string.Empty;
                        txtAdmissionNo.Enabled = false;
                    }
                    flag = 1;
                }
            }

            if (flag == 1)
            {
                //if (divLeftRoll.Visible == true && divRightRoll.Visible == true)
                if (schoolOrCollege == 0)
                {
                    txtstdrollno.Enabled = true;
                    txtstdrollno.Text = string.Empty;
                }
                else if (schoolOrCollege == 1)
                {
                    txtAdmissionNo.Text = string.Empty;
                    txtAdmissionNo.Enabled = true;
                }
                if (count == 1)
                {
                    txtstdrollno.Text = rollNo;
                    txtAdmissionNo.Text = rollAdmit;
                    txtstdrollno.Enabled = false;
                    txtAdmissionNo.Enabled = false;
                }
            }
            else
            {
                //if (divLeftRoll.Visible == true && divRightRoll.Visible == true)
                if (schoolOrCollege == 0)
                {
                    txtstdrollno.Enabled = false;
                    txtstdrollno.Text = string.Empty;
                }
                else if (schoolOrCollege == 1)
                {
                    txtAdmissionNo.Text = string.Empty;
                    txtAdmissionNo.Enabled = false;
                }
            }
        }
    }

    protected void sprdselectrollno_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            lblErrMsg.Visible = false;
            lblErrMsg.Text = string.Empty;
            sprdselectrollno.SaveChanges();

            string actrow = e.SheetView.ActiveRow.ToString();
            if (flag_true == false && actrow == "0")
            {
                for (int j = 1; j < Convert.ToInt16(sprdselectrollno.Sheets[0].RowCount); j++)
                {
                    string actcol = e.SheetView.ActiveColumn.ToString();
                    //string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                    //if (seltext != "System.Object")
                    int val = Convert.ToInt32(sprdselectrollno.Sheets[0].Cells[e.SheetView.ActiveRow, e.SheetView.ActiveColumn].Value);
                    sprdselectrollno.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Value = val.ToString();
                }
                flag_true = true;
            }
            //string actrow = e.SheetView.ActiveRow.ToString();

            int flag = 0;
            string rollNo = string.Empty;
            string rollAdmit = string.Empty;
            int count = 0;

            if (actrow == "0")
            {

            }

            for (int res = 1; res <= Convert.ToInt32(sprdselectrollno.Sheets[0].RowCount) - 1; res++)
            {
                int isval = 0;
                string s = Convert.ToString(sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(actrow), 5].Value).Trim();
                //sankar added may'20...............................
                string roll_no = sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 1].Text;
                string staff_name = sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 4].Text;
                isval = Convert.ToInt32(sprdselectrollno.Sheets[0].Cells[res, 5].Value);
                if (isval == 0 || s == "")
                {

                }
                else
                {
                    rollNo = Convert.ToString(sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 1].Text).Trim();
                    rollAdmit = Convert.ToString(sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 3].Text).Trim();
                    count++;
                    //if (divLeftRoll.Visible == true && divRightRoll.Visible == true)
                    if (schoolOrCollege == 0)
                    {
                        txtstdrollno.Enabled = false;
                        txtstdrollno.Text = string.Empty;
                    }
                    else if (schoolOrCollege == 1)
                    {
                        txtAdmissionNo.Text = string.Empty;
                        txtAdmissionNo.Enabled = false;
                    }
                    flag = 1;
                }
            }

            if (flag == 1)
            {
                txtAdmissionNo.Text = string.Empty;
                txtstdrollno.Text = string.Empty;
                //if (divLeftRoll.Visible == true && divRightRoll.Visible == true)
                if (schoolOrCollege == 0)
                {
                    txtstdrollno.Enabled = true;
                    txtstdrollno.Text = string.Empty;
                    if (count == 1)
                    {
                        divLeftRoll.Visible = true;
                        divRightRoll.Visible = true;

                        divLeftAdmit.Visible = false;
                        divRightAdmit.Visible = false;
                    }
                    else
                    {
                        divLeftRoll.Visible = false;
                        divRightRoll.Visible = false;
                        divLeftAdmit.Visible = false;
                        divRightAdmit.Visible = false;
                    }
                }
                else if (schoolOrCollege == 1)
                {
                    txtAdmissionNo.Text = string.Empty;
                    txtAdmissionNo.Enabled = true;

                    if (count == 1)
                    {
                        divLeftRoll.Visible = false;
                        divRightRoll.Visible = false;
                        divLeftAdmit.Visible = true;
                        divRightAdmit.Visible = true;
                    }
                    else
                    {
                        divLeftRoll.Visible = false;
                        divRightRoll.Visible = false;
                        divLeftAdmit.Visible = false;
                        divRightAdmit.Visible = false;
                    }
                }
                if (count == 1)
                {
                    txtstdrollno.Text = rollNo;
                    txtAdmissionNo.Text = rollAdmit;
                    txtstdrollno.Enabled = false;
                    txtAdmissionNo.Enabled = false;
                }
            }
            else
            {
                //if (divLeftRoll.Visible == true && divRightRoll.Visible == true)
                if (schoolOrCollege == 0)
                {
                    txtstdrollno.Enabled = false;
                    txtstdrollno.Text = string.Empty;
                }
                else if (schoolOrCollege == 1)
                {
                    txtAdmissionNo.Text = string.Empty;
                    txtAdmissionNo.Enabled = false;
                }
                divLeftRoll.Visible = false;
                divRightRoll.Visible = false;
                divLeftAdmit.Visible = false;
                divRightAdmit.Visible = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void chkdismissal_CheckedChanged(object sender, EventArgs e)
    {
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
    }

    protected void txtstdrollno_TextChanged(object sender, EventArgs e)
    {
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
        string getroll = d2.GetFunction("select roll_no from registration where roll_no='" + txtstdrollno.Text + "' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
        if (getroll.Trim() != "" && getroll.Trim() != "0")
        {
            errmsg.Visible = false;
        }
        else
        {
            txtstdrollno.Text = string.Empty;
            errmsg.Text = "Please Enter Valid Roll No";
            errmsg.Visible = true;
        }
    }

    protected void txtAdmissionNo_TextChanged(object sender, EventArgs e)
    {
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
        string getroll = d2.GetFunction("select Roll_Admit from registration where Roll_Admit='" + txtAdmissionNo.Text.Trim() + "'  and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
        if (getroll.Trim() != "" && getroll.Trim() != "0")
        {
            errmsg.Visible = false;
        }
        else
        {
            txtAdmissionNo.Text = string.Empty;
            errmsg.Text = "Please Enter Valid Admission No";
            errmsg.Visible = true;
        }
    }

    protected void chksuspension_CheckedChanged(object sender, EventArgs e)
    {
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
        if (chksuspension.Checked == true)
        {
            txtstartdate.Visible = true;
            lblstartdate.Visible = true;
            txtEndDate.Visible = true;
            lblEndDate.Visible = true;
            lbldays.Visible = true;
            txtdays.Visible = true;
        }
        else
        {
            txtstartdate.Visible = false;
            lblstartdate.Visible = false;
            txtEndDate.Visible = false;
            lblEndDate.Visible = false;
            lbldays.Visible = false;
            txtdays.Visible = false;
        }

    }

    protected void chkfine_CheckedChanged(object sender, EventArgs e)
    {
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
        if (chkfine.Checked == true)
        {
            lblfine.Visible = true;
            txtfine.Visible = true;
        }
        else
        {
            lblfine.Visible = false;
            txtfine.Visible = false;
        }
    }

    protected void chkwarning_CheckedChanged(object sender, EventArgs e)
    {
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
    }

    protected void savedetails()
    {
        try
        {
            lblErrMsg.Visible = false;
            lblErrMsg.Text = string.Empty;

            string feeOnRollDate = txtFeeOnRollDate.Text.Trim();
            DateTime dtFeeOnRollDate = new DateTime();
            bool isSuccOnroll = DateTime.TryParseExact(feeOnRollDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFeeOnRollDate);
            string qryFeeOnRollInsert = string.Empty;
            string qryFeeOnRollUpdate = string.Empty;
            string qryFeeOnRollValue = string.Empty;

            string suspendedFromDate = txtstartdate.Text.Trim();
            string suspendedToDate = txtEndDate.Text.Trim();
            DateTime dtSuspendFromDate = new DateTime();
            DateTime dtSuspendToDate = new DateTime();

            bool isFromSuccess = DateTime.TryParseExact(suspendedFromDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendFromDate);
            bool isToSuccess = DateTime.TryParseExact(suspendedToDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendToDate);

            string qrySuspendInsert = string.Empty;
            string qrySuspendUpdate = string.Empty;
            string qrySuspendValue = string.Empty;

            sprdselectrollno.SaveChanges();
            if (chkdismissal.Checked == true)
            {
                dis = 1;
            }
            if (chkfine.Checked == true)
            {
                fin = 1;
            }
            if (chksuspension.Checked == true)
            {
                sus = 1;
                if (!ValidateSuspended())
                    return;
                else
                {
                    qrySuspendInsert = " ,suspendFromDate,suspendToDate ";
                    qrySuspendUpdate = " ,suspendFromDate='" + dtSuspendFromDate.ToString("MM/dd/yyyy") + "',suspendToDate='" + dtSuspendToDate.ToString("MM/dd/yyyy") + "' ";
                    qrySuspendValue = " ,'" + dtSuspendFromDate.ToString("MM/dd/yyyy") + "','" + dtSuspendToDate.ToString("MM/dd/yyyy") + "' ";
                    txtdate1.Text = dtSuspendFromDate.ToString("dd-MM-yyyy");
                }
            }
            else
            {

            }
            if (chkwarning.Checked == true)
            {
                war = 1;
            }

            if (chkfeeofroll.Checked == true)
            {
                feeofroll = 1;
            }
            else if (chkfeeonroll.Checked)
            {
                feeofroll = 0;
                if (!ValidateFeeOnRoll())
                    return;
                else if (btnsave.Text == "Update")
                {
                    qryFeeOnRollInsert = " ,feeOnRollDate ";
                    qryFeeOnRollUpdate = " ,feeOnRollDate='" + dtFeeOnRollDate.ToString("MM/dd/yyyy") + "' ";
                    qryFeeOnRollValue = " ,'" + dtFeeOnRollDate.ToString("MM/dd/yyyy") + "' ";
                }
            }

            if (chkremark.Checked == true)
            {
                remarkval = 1;
            }
            string ackdate = string.Empty;
            ackdate = txtdate1.Text.ToString();

            string[] splitack = ackdate.Split(new Char[] { '-' });

            int splitackdate = Convert.ToInt32(splitack[0]);
            int splitackmonth = Convert.ToInt32(splitack[1]);
            int splitackyear = Convert.ToInt32(splitack[2]);

            string ack_date = splitackmonth + "/" + splitackdate + "/" + splitackyear;
            string curdate = string.Empty;
            curdate = txtdate1.Text.ToString();

            string[] split = curdate.Split(new Char[] { '-' });

            int splitdate = Convert.ToInt32(split[0]);
            int splitmonth = Convert.ToInt32(split[1]);
            int splityear = Convert.ToInt32(split[2]);

            string curr_date = splitmonth + "-" + splitdate + "-" + splityear;

            string strinsupdaequery = string.Empty;
            int insupdatequery = 0;

            int flag = 0;
            errmsg.Visible = false;
            lblnorec.Visible = false;
            if (txtstaff.Text != "")
            {
                if (chkdismissal.Checked == true || chkfine.Checked == true || chksuspension.Checked == true || chkwarning.Checked == true || chkfeeofroll.Checked == true || chkfeeonroll.Checked == true || chkremark.Checked == true)
                {
                    if (txtstdrollno.Text != "")
                    {
                        flag = 1;
                        //sankar modify may'27
                        if (btnsave.Text == "Save")
                        {
                            //Added by gowtham  and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'
                            string strquery = "select roll_no,convert(varchar(100),curr_date,105) as curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,convert(varchar(100),cast(ack_date as DateTime),105) as ack_date,tot_days,fine_amo,serial_no,semester,ack_fee_of_roll,Remark,ack_remarks,convert(varchar(100),cast(ack_date as DateTime),105) as feeOffRollDate,convert(varchar(100),feeOnRollDate,105) as feeOnRollDate,convert(varchar(50),suspendFromDate,105) as suspendFromDate,convert(varchar(50),suspendToDate,105) as suspendToDate from stucon where roll_no='" + txtstdrollno.Text + "' order by convert(varchar(100),cast(ack_date as DateTime),105) desc";
                            DataSet alreadystuddetail = d2.select_method_wo_parameter(strquery, "Text");
                            if (alreadystuddetail.Tables.Count > 0 && alreadystuddetail.Tables[0].Rows.Count > 0)
                            {
                                string dismissal = string.Empty;
                                dismissal = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_diss"]).Trim();
                                string suspension = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_susp"]).Trim();

                                string AckFine = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_fine"]).Trim();
                                string AckWarn = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_warn"]).Trim();
                                string ack_fee_of_roll = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_fee_of_roll"]).Trim();
                                string ack_remarks = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_remarks"]).Trim();

                                if (dismissal == "1")
                                {
                                    errmsg.Visible = true;
                                    errmsg.Text = "Student Already Dismissed";
                                }
                                else if (suspension == "1")
                                {

                                    int numofdays = 0;// Convert.ToInt16(alreadystuddetail.Tables[0].Rows[0]["tot_days"].ToString());
                                    int.TryParse(Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["tot_days"]).Trim(), out numofdays);
                                    DateTime startdate = new DateTime();// Convert.ToDateTime(alreadystuddetail.Tables[0].Rows[0]["ack_date"].ToString());
                                    DateTime.TryParseExact(Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_date"]).Trim(), "dd-MM-yyyy", null, DateTimeStyles.None, out startdate);
                                    startdate = startdate.AddDays(numofdays);
                                    string stdate = startdate.ToString("MM/dd/yyyy");
                                    string[] splitdates = stdate.Split(new Char[] { ' ' });
                                    string spdate = splitdates[0].ToString();
                                    string[] splitd = spdate.Split(new Char[] { '/' });
                                    int splitdatess = Convert.ToInt32(splitd[1]);
                                    int splitmonths = Convert.ToInt32(splitd[0]);
                                    int splityears = Convert.ToInt32(splitd[2]);
                                    string punishdate = splitmonths + "-" + splitdatess + "-" + splityears;
                                    if (Convert.ToDateTime(curr_date) < Convert.ToDateTime(punishdate))
                                    {
                                        errmsg.Visible = true;
                                        errmsg.Text = "Student Already in Suspension";
                                        return;
                                    }
                                    else
                                    {
                                        strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,semester,Remark,ack_remarks " + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + txtstdrollno.Text + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + feeofroll + "','" + Convert.ToString(ddlsemadd.SelectedValue).Trim() + "'," + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                        insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                        lblnorec.Visible = false;
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                                    }
                                    if (sus == 1)
                                    {
                                        int monthyear = splityear * 12 + splitmonth;
                                        string noofhrs = d2.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + "");
                                        if (noofhrs.Trim() != "" && noofhrs != "0" && noofhrs != null)
                                        {
                                            DateTime dtDummyStart = new DateTime();
                                            DateTime dtDummyEnd = new DateTime();
                                            int totalDaySuspended = 0;
                                            int.TryParse(txtdays.Text.Trim(), out totalDaySuspended);

                                            ArrayList arrMonthYear = new ArrayList();
                                            Dictionary<long, string> dicQInsertColumn = new Dictionary<long, string>();
                                            Dictionary<long, string> dicQInsertValues = new Dictionary<long, string>();
                                            Dictionary<long, string> dicQUpdate = new Dictionary<long, string>();

                                            string insertColumn = string.Empty;
                                            string insertValue = string.Empty;
                                            string updateValue = string.Empty;

                                            for (DateTime dtTemp = dtSuspendFromDate; dtTemp <= dtSuspendToDate; dtTemp = dtTemp.AddDays(1))
                                            {
                                                string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + dtTemp.ToString() + "'";
                                                DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                                long tempMonthYear = dtTemp.Year * 12 + dtTemp.Month;
                                                insertColumn = string.Empty;
                                                insertValue = string.Empty;
                                                updateValue = string.Empty;
                                                string day = dtTemp.Day.ToString();
                                                if (!arrMonthYear.Contains(tempMonthYear))
                                                {
                                                    arrMonthYear.Add(tempMonthYear);
                                                }
                                                if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                {
                                                    dicQInsertColumn.Add(tempMonthYear, string.Empty);
                                                }
                                                if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                {
                                                    dicQInsertValues.Add(tempMonthYear, string.Empty);
                                                }
                                                if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                {
                                                    dicQUpdate.Add(tempMonthYear, string.Empty);
                                                }
                                                if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count == 0)
                                                {
                                                    for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                                    {
                                                        if (insertColumn == "")
                                                        {
                                                            insertColumn = "d" + day + "d" + i + "";
                                                            insertValue = "9";
                                                            updateValue = "d" + day + "d" + i + "=9";
                                                        }
                                                        else
                                                        {
                                                            insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                            insertValue = insertValue + ',' + "9";
                                                            updateValue = updateValue + ',' + "d" + day + "d" + i + "=9";
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(insertColumn.Trim()))
                                                    {
                                                        if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQInsertColumn.Add(tempMonthYear, insertColumn);
                                                        }
                                                        else
                                                        {
                                                            string value = dicQInsertColumn[tempMonthYear];
                                                            if (!string.IsNullOrEmpty(value))
                                                            {
                                                                dicQInsertColumn[tempMonthYear] = value + "," + insertColumn;
                                                            }
                                                            else
                                                            {
                                                                dicQInsertColumn[tempMonthYear] = insertColumn;
                                                            }
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(insertValue.Trim()))
                                                    {
                                                        if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQInsertValues.Add(tempMonthYear, insertValue);
                                                        }
                                                        else
                                                        {
                                                            string value = dicQInsertValues[tempMonthYear];
                                                            if (!string.IsNullOrEmpty(value))
                                                            {
                                                                dicQInsertValues[tempMonthYear] = value + "," + insertValue;
                                                            }
                                                            else
                                                            {
                                                                dicQInsertValues[tempMonthYear] = insertValue;
                                                            }
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                    {
                                                        if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQUpdate.Add(tempMonthYear, updateValue);
                                                        }
                                                        else
                                                        {
                                                            string value = dicQUpdate[tempMonthYear];
                                                            dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                            if (!string.IsNullOrEmpty(value))
                                                            {
                                                                dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                            }
                                                            else
                                                            {
                                                                dicQUpdate[tempMonthYear] = updateValue;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            foreach (long dicEntry in arrMonthYear)
                                            {
                                                string monthYear = Convert.ToString(dicEntry).Trim();
                                                long longMonthYear = 0;
                                                long.TryParse(monthYear.Trim(), out longMonthYear);
                                                insertColumn = string.Empty;
                                                insertValue = string.Empty;
                                                updateValue = string.Empty;
                                                string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + txtstdrollno.Text + "' and month_year=" + monthYear.Trim() + "");
                                                if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                                {
                                                    if (dicQUpdate.ContainsKey(longMonthYear))
                                                    {
                                                        updateValue = dicQUpdate[longMonthYear];
                                                    }
                                                    if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                    {
                                                        string insquery = "update attendance set " + updateValue + " where roll_no='" + txtstdrollno.Text + "' and month_year=" + monthYear + "";
                                                        int a = d2.update_method_wo_parameter(insquery, "Text");
                                                    }
                                                }
                                                else
                                                {
                                                    if (dicQInsertColumn.ContainsKey(longMonthYear))
                                                    {
                                                        insertColumn = dicQInsertColumn[longMonthYear];
                                                    }
                                                    if (dicQInsertValues.ContainsKey(longMonthYear))
                                                    {
                                                        insertValue = dicQInsertValues[longMonthYear];
                                                    }
                                                    if (!string.IsNullOrEmpty(insertColumn.Trim()) && !string.IsNullOrEmpty(insertValue.Trim()))
                                                    {
                                                        string insquery = "insert into attendance(roll_no,month_year," + insertColumn + ") values('" + txtstdrollno.Text + "'," + monthYear + "," + insertValue + ")";
                                                        int a = d2.update_method_wo_parameter(insquery, "Text");
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                else if (AckFine.Trim() == "0" && AckWarn.Trim() == "0" && ack_fee_of_roll.Trim() == "0" && ack_remarks.Trim() == "0" && suspension.Trim() == "0" && dismissal.Trim() == "0")
                                {
                                    if (chkfeeofroll.Checked == true)
                                    {
                                        strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,semester,Remark,ack_remarks " + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + txtstdrollno.Text + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + feeofroll + "','" + Convert.ToString(ddlsemadd.SelectedValue).Trim() + "','" + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                        insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                        lblnorec.Visible = false;
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                                    }
                                }
                                #region Added by Idhris for New Remarks -- 03-10-2016
                                try
                                {
                                    if (chkremark.Checked || AckFine.Trim() != "0" || AckWarn.Trim() != "0" || ack_fee_of_roll.Trim() != "0" || ack_remarks.Trim() != "0" || suspension.Trim() != "0" || dismissal.Trim() != "0")//Rajkumar 061/2018
                                    {
                                        strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,semester,Remark,ack_remarks" + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + txtstdrollno.Text + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + feeofroll + "','" + Convert.ToString(ddlsemadd.SelectedValue).Trim() + "','" + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                        insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                                    }
                                }
                                catch { ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Not Saved')", true); }
                                #endregion
                            }
                            else
                            {
                                strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,semester,Remark,ack_remarks" + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + txtstdrollno.Text + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + feeofroll + "','" + Convert.ToString(ddlsemadd.SelectedValue) + "','" + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                lblnorec.Visible = false;
                                if (sus == 1)
                                {
                                    int monthyear = splityear * 12 + splitmonth;
                                    string noofhrs = d2.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + "");
                                    if (noofhrs.Trim() != "" && noofhrs != "0" && noofhrs != null)
                                    {
                                        DateTime dtDummyStart = new DateTime();
                                        DateTime dtDummyEnd = new DateTime();
                                        int totalDaySuspended = 0;
                                        int.TryParse(txtdays.Text.Trim(), out totalDaySuspended);

                                        ArrayList arrMonthYear = new ArrayList();
                                        Dictionary<long, string> dicQInsertColumn = new Dictionary<long, string>();
                                        Dictionary<long, string> dicQInsertValues = new Dictionary<long, string>();
                                        Dictionary<long, string> dicQUpdate = new Dictionary<long, string>();

                                        string insertColumn = string.Empty;
                                        string insertValue = string.Empty;
                                        string updateValue = string.Empty;

                                        for (DateTime dtTemp = dtSuspendFromDate; dtTemp <= dtSuspendToDate; dtTemp = dtTemp.AddDays(1))
                                        {
                                            string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + dtTemp.ToString() + "'";
                                            DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                            long tempMonthYear = dtTemp.Year * 12 + dtTemp.Month;
                                            insertColumn = string.Empty;
                                            insertValue = string.Empty;
                                            updateValue = string.Empty;
                                            string day = dtTemp.Day.ToString();
                                            if (!arrMonthYear.Contains(tempMonthYear))
                                            {
                                                arrMonthYear.Add(tempMonthYear);
                                            }
                                            if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                            {
                                                dicQInsertColumn.Add(tempMonthYear, string.Empty);
                                            }
                                            if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                            {
                                                dicQInsertValues.Add(tempMonthYear, string.Empty);
                                            }
                                            if (!dicQUpdate.ContainsKey(tempMonthYear))
                                            {
                                                dicQUpdate.Add(tempMonthYear, string.Empty);
                                            }
                                            if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count == 0)
                                            {
                                                for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                                {
                                                    if (insertColumn == "")
                                                    {
                                                        insertColumn = "d" + day + "d" + i + "";
                                                        insertValue = "9";
                                                        updateValue = "d" + day + "d" + i + "=9";
                                                    }
                                                    else
                                                    {
                                                        insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                        insertValue = insertValue + ',' + "9";
                                                        updateValue = updateValue + ',' + "d" + day + "d" + i + "=9";
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(insertColumn.Trim()))
                                                {
                                                    if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQInsertColumn.Add(tempMonthYear, insertColumn);
                                                    }
                                                    else
                                                    {
                                                        string value = dicQInsertColumn[tempMonthYear];
                                                        if (!string.IsNullOrEmpty(value))
                                                        {
                                                            dicQInsertColumn[tempMonthYear] = value + "," + insertColumn;
                                                        }
                                                        else
                                                        {
                                                            dicQInsertColumn[tempMonthYear] = insertColumn;
                                                        }
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(insertValue.Trim()))
                                                {
                                                    if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQInsertValues.Add(tempMonthYear, insertValue);
                                                    }
                                                    else
                                                    {
                                                        string value = dicQInsertValues[tempMonthYear];
                                                        if (!string.IsNullOrEmpty(value))
                                                        {
                                                            dicQInsertValues[tempMonthYear] = value + "," + insertValue;
                                                        }
                                                        else
                                                        {
                                                            dicQInsertValues[tempMonthYear] = insertValue;
                                                        }
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                {
                                                    if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQUpdate.Add(tempMonthYear, updateValue);
                                                    }
                                                    else
                                                    {
                                                        string value = dicQUpdate[tempMonthYear];
                                                        dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                        if (!string.IsNullOrEmpty(value))
                                                        {
                                                            dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                        }
                                                        else
                                                        {
                                                            dicQUpdate[tempMonthYear] = updateValue;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        foreach (long dicEntry in arrMonthYear)
                                        {
                                            string monthYear = Convert.ToString(dicEntry).Trim();
                                            long longMonthYear = 0;
                                            long.TryParse(monthYear.Trim(), out longMonthYear);
                                            insertColumn = string.Empty;
                                            insertValue = string.Empty;
                                            updateValue = string.Empty;
                                            string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + txtstdrollno.Text + "' and month_year=" + monthYear.Trim() + "");
                                            if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                            {
                                                if (dicQUpdate.ContainsKey(longMonthYear))
                                                {
                                                    updateValue = dicQUpdate[longMonthYear];
                                                }
                                                if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                {
                                                    string insquery = "update attendance set " + updateValue + " where roll_no='" + txtstdrollno.Text + "' and month_year=" + monthYear + "";
                                                    int a = d2.update_method_wo_parameter(insquery, "Text");
                                                }
                                            }
                                            else
                                            {
                                                if (dicQInsertColumn.ContainsKey(longMonthYear))
                                                {
                                                    insertColumn = dicQInsertColumn[longMonthYear];
                                                }
                                                if (dicQInsertValues.ContainsKey(longMonthYear))
                                                {
                                                    insertValue = dicQInsertValues[longMonthYear];
                                                }
                                                if (!string.IsNullOrEmpty(insertColumn.Trim()) && !string.IsNullOrEmpty(insertValue.Trim()))
                                                {
                                                    string insquery = "insert into attendance(roll_no,month_year," + insertColumn + ") values('" + txtstdrollno.Text + "'," + monthYear + "," + insertValue + ")";
                                                    int a = d2.update_method_wo_parameter(insquery, "Text");
                                                }
                                            }
                                        }
                                    }
                                }
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                            }
                        }
                        else if (btnsave.Text == "Update")
                        {
                            //magesh 12.10.18
                          string  strinsselectquery = "select convert(nvarchar(15),suspendFromDate,103) suspendFromDate,convert(nvarchar(15),suspendToDate,103) suspendToDate from stucon where roll_no = '" + txtstdrollno.Text + "' and StuConID='" + lblindex.Text.Trim() + "'";
                          DataSet selectqueryy = d2.select_method_wo_parameter(strinsselectquery, "text");

                            strinsupdaequery = "update stucon set curr_date = '" + curr_date + "',infr_type = '" + ddlfraction.SelectedItem + "',ack_diss = '" + dis + "',ack_susp = '" + sus + "',ack_fine = '" + fin + "',ack_warn = '" + war + "',prof_code = '" + lblerrstaffcode.Text + "',ack_date = '" + ack_date + "',tot_days = '" + txtdays.Text + "',fine_amo = '" + txtfine.Text + "',ack_fee_of_roll='" + feeofroll + "',Remark='" + txtremarks.Text.ToString() + "',ack_remarks='" + remarkval.ToString() + "'" + qryFeeOnRollUpdate + qrySuspendUpdate + " where roll_no = '" + txtstdrollno.Text + "' and StuConID='" + lblindex.Text.Trim() + "'";
                            insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");

                          
                            lblnorec.Visible = false;
                            if (sus == 1)
                            {
                                int monthyear = splityear * 12 + splitmonth;
                                string fromdate = Convert.ToString(selectqueryy.Tables[0].Rows[0]["suspendFromDate"]).Trim();
                                string todayte = Convert.ToString(selectqueryy.Tables[0].Rows[0]["suspendToDate"]).Trim();  //magesh 12.10.18

                                DateTime upSuspendFromDate = new DateTime();
                                DateTime upSuspendToDate = new DateTime();
                                string[] spl = fromdate.Split('/');
                                string upSuspendFromDates = spl[0] + '-' + spl[1] + '-' + spl[2];
                                string[] spl2 = todayte.Split('/');
                                string upSuspendtoDates = spl2[0] + '-' + spl2[1] + '-' + spl2[2];
                                bool isFromSucces = DateTime.TryParseExact(upSuspendFromDates, "dd-MM-yyyy", null, DateTimeStyles.None, out upSuspendFromDate);
                                bool isToSucces = DateTime.TryParseExact(upSuspendtoDates, "dd-MM-yyyy", null, DateTimeStyles.None, out upSuspendToDate);
                                string noofhrs = d2.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + "");
                                if (noofhrs.Trim() != "" && noofhrs != "0" && noofhrs != null)
                                {
                                    //DateTime datesus = Convert.ToDateTime(curr_date);
                                    //int day = Convert.ToInt32(txtdays.Text);
                                    //string datecolumn = string.Empty;

                                    //datesus= datesus.AddDays(10);
                                    //string attvalue = string.Empty;
                                    //string dateattvalue = string.Empty;
                                    //
                                    //for (int date = 0; date < day; date++)
                                    //{
                                    //    datesus = datesus.AddDays(date);
                                    //    string dateva = datesus.Day.ToString();
                                    //    string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + datesus.ToString() + "'";
                                    //    DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                    //    if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count == 0)
                                    //    {
                                    //        for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                    //        {
                                    //            if (datecolumn == "")
                                    //            {
                                    //                datecolumn = "d" + dateva + "d" + i + "";
                                    //                attvalue = "9";
                                    //                dateattvalue = "d" + dateva + "d" + i + "=9";
                                    //            }
                                    //            else
                                    //            {
                                    //                datecolumn = "" + datecolumn + "," + "d" + dateva + "d" + i + "";
                                    //                attvalue = attvalue + ',' + "9";
                                    //                dateattvalue = dateattvalue + ',' + "d" + dateva + "d" + i + "=9";
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + txtstdrollno.Text + "' and month_year=" + monthyear + "");
                                    //if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                    //{
                                    //    string insquery = "update attendance set " + dateattvalue + " where roll_no='" + txtstdrollno.Text + "' and month_year=" + monthyear + "";
                                    //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                    //}
                                    //else
                                    //{
                                    //    string insquery = "insert into attendance(roll_no,month_year," + datecolumn + ") values('" + txtstdrollno.Text + "'," + monthyear + "," + attvalue + ")";
                                    //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                    //}
                                    DateTime dtDummyStart = new DateTime();
                                    DateTime dtDummyEnd = new DateTime();
                                    DateTime dtDummyStartd = new DateTime();
                                    DateTime dtDummyStartto = new DateTime();
                                    int totalDaySuspended = 0;
                                    int.TryParse(txtdays.Text.Trim(), out totalDaySuspended);

                                    ArrayList arrMonthYear = new ArrayList();
                                    Dictionary<long, string> dicQInsertColumn = new Dictionary<long, string>();
                                    Dictionary<long, string> dicQInsertValues = new Dictionary<long, string>();
                                    Dictionary<long, string> dicQUpdate = new Dictionary<long, string>();

                                    string insertColumn = string.Empty;
                                    string insertValue = string.Empty;
                                    string updateValue = string.Empty;
                                   
                                   
                                      //  for (DateTime dtTemp = dtSuspendFromDate; dtTemp <= dtSuspendToDate; dtTemp = dtTemp.AddDays(1))
                                    //magesh 12.10.18
                                    if (upSuspendFromDate <= dtSuspendFromDate )
                                    {
                                        dtDummyStartd = upSuspendFromDate;
                                    }
                                    else
                                    {
                                        dtDummyStartd = dtSuspendFromDate;
                                    }
                                    if (upSuspendToDate >= dtSuspendToDate)
                                    {
                                        dtDummyStartto = upSuspendToDate;
                                    }
                                    else
                                    {
                                        dtDummyStartto = dtSuspendToDate;
                                    }  //magesh 12.10.18
                                    for (DateTime dtTemp = dtDummyStartd; dtTemp <= dtDummyStartto; dtTemp = dtTemp.AddDays(1))
                                        {
                                            string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + dtTemp.ToString() + "'";
                                            DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                            long tempMonthYear = dtTemp.Year * 12 + dtTemp.Month;
                                            insertColumn = string.Empty;
                                            insertValue = string.Empty;
                                            updateValue = string.Empty;
                                            string day = dtTemp.Day.ToString();
                                            if (!arrMonthYear.Contains(tempMonthYear))
                                            {
                                                arrMonthYear.Add(tempMonthYear);
                                            }
                                            if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                            {
                                                dicQInsertColumn.Add(tempMonthYear, string.Empty);
                                            }
                                            if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                            {
                                                dicQInsertValues.Add(tempMonthYear, string.Empty);
                                            }
                                            if (!dicQUpdate.ContainsKey(tempMonthYear))
                                            {
                                                dicQUpdate.Add(tempMonthYear, string.Empty);
                                            }
                                            if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count == 0)
                                            {
                                                for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                                {
                                                   // if (dtSuspendFromDate <= dtSuspendToDate)
                                                    //magesh 12.10.18
                                                    if (dtTemp >= dtSuspendFromDate && dtTemp <= dtSuspendToDate)
                                    {
                                                   // if (upSuspendFromDate >= dtDummyStartd && upSuspendToDate >= dtSuspendToDate)
                                                   // {
                                                        if (insertColumn == "")
                                                        {
                                                            insertColumn = "d" + day + "d" + i + "";
                                                            insertValue = "9";
                                                            updateValue = "d" + day + "d" + i + "=9";
                                                        }
                                                        else
                                                        {
                                                            insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                            insertValue = insertValue + ',' + "9";
                                                            updateValue = updateValue + ',' + "d" + day + "d" + i + "=9";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (insertColumn == "")
                                                        {
                                                            insertColumn = "d" + day + "d" + i + "";
                                                            insertValue = "";
                                                            updateValue = "d" + day + "d" + i + "=''";
                                                        }
                                                        else
                                                        {
                                                            insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                            insertValue = insertValue + ',' + "";
                                                            updateValue = updateValue + ',' + "d" + day + "d" + i + "=''";
                                                        }
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(insertColumn.Trim()))
                                                {
                                                    if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQInsertColumn.Add(tempMonthYear, insertColumn);
                                                    }
                                                    else
                                                    {
                                                        string value = dicQInsertColumn[tempMonthYear];
                                                        if (!string.IsNullOrEmpty(value))
                                                        {
                                                            dicQInsertColumn[tempMonthYear] = value + "," + insertColumn;
                                                        }
                                                        else
                                                        {
                                                            dicQInsertColumn[tempMonthYear] = insertColumn;
                                                        }
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(insertValue.Trim()))
                                                {
                                                    if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQInsertValues.Add(tempMonthYear, insertValue);
                                                    }
                                                    else
                                                    {
                                                        string value = dicQInsertValues[tempMonthYear];
                                                        if (!string.IsNullOrEmpty(value))
                                                        {
                                                            dicQInsertValues[tempMonthYear] = value + "," + insertValue;
                                                        }
                                                        else
                                                        {
                                                            dicQInsertValues[tempMonthYear] = insertValue;
                                                        }
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                {
                                                    if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQUpdate.Add(tempMonthYear, updateValue);
                                                    }
                                                    else
                                                    {
                                                        string value = dicQUpdate[tempMonthYear];
                                                        dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                        if (!string.IsNullOrEmpty(value))
                                                        {
                                                            dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                        }
                                                        else
                                                        {
                                                            dicQUpdate[tempMonthYear] = updateValue;
                                                        }
                                                    }
                                                }
                                            }
                                        
                                    }
                                    foreach (long dicEntry in arrMonthYear)
                                    {
                                        string monthYear = Convert.ToString(dicEntry).Trim();
                                        long longMonthYear = 0;
                                        long.TryParse(monthYear.Trim(), out longMonthYear);
                                        insertColumn = string.Empty;
                                        insertValue = string.Empty;
                                        updateValue = string.Empty;
                                        string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + txtstdrollno.Text + "' and month_year=" + monthYear.Trim() + "");
                                        if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                        {
                                            if (dicQUpdate.ContainsKey(longMonthYear))
                                            {
                                                updateValue = dicQUpdate[longMonthYear];
                                            }
                                            if (!string.IsNullOrEmpty(updateValue.Trim()))
                                            {
                                                string insquery = "update attendance set " + updateValue + " where roll_no='" + txtstdrollno.Text + "' and month_year=" + monthYear + "";
                                                int a = d2.update_method_wo_parameter(insquery, "Text");
                                            }
                                        }
                                        else
                                        {
                                            if (dicQInsertColumn.ContainsKey(longMonthYear))
                                            {
                                                insertColumn = dicQInsertColumn[longMonthYear];
                                            }
                                            if (dicQInsertValues.ContainsKey(longMonthYear))
                                            {
                                                insertValue = dicQInsertValues[longMonthYear];
                                            }
                                            if (!string.IsNullOrEmpty(insertColumn.Trim()) && !string.IsNullOrEmpty(insertValue.Trim()))
                                            {
                                                string insquery = "insert into attendance(roll_no,month_year," + insertColumn + ") values('" + txtstdrollno.Text + "'," + monthYear + "," + insertValue + ")";
                                                int a = d2.update_method_wo_parameter(insquery, "Text");
                                            }
                                        }
                                    }
                                }
                            }
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Updated Successfully')", true);
                        }
                    }
                    else if (txtAdmissionNo.Text != "")
                    {
                        flag = 1;
                        string rollNo = d2.GetFunctionv("select Roll_No from Registration where Roll_Admit='" + Convert.ToString(txtAdmissionNo.Text).Trim() + "' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
                        //sankar modify may'27
                        if (btnsave.Text == "Save")
                        {
                            //Added by gowtham
                            string strquery = "select roll_no,convert(varchar(100),curr_date,105) as curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,convert(varchar(100),cast(ack_date as DateTime),105) as ack_date,tot_days,fine_amo,serial_no,semester,ack_fee_of_roll,Remark,ack_remarks,convert(varchar(100),cast(ack_date as DateTime),105) as feeOffRollDate,convert(varchar(100),feeOnRollDate,105) as feeOnRollDate,convert(varchar(50),suspendFromDate,105) as suspendFromDate,convert(varchar(50),suspendToDate,105) as suspendToDate from stucon where roll_no='" + rollNo + "'  order by convert(varchar(100),cast(ack_date as DateTime),105) desc";
                            DataSet alreadystuddetail = d2.select_method_wo_parameter(strquery, "Text");
                            if (alreadystuddetail.Tables.Count > 0 && alreadystuddetail.Tables[0].Rows.Count > 0)
                            {
                                string dismissal = string.Empty;
                                dismissal = alreadystuddetail.Tables[0].Rows[0]["ack_diss"].ToString();
                                string suspension = alreadystuddetail.Tables[0].Rows[0]["ack_susp"].ToString();
                                string AckFine = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_fine"]).Trim();
                                string AckWarn = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_warn"]).Trim();
                                string ack_fee_of_roll = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_fee_of_roll"]).Trim();
                                string ack_remarks = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_remarks"]).Trim();
                                if (dismissal == "1")
                                {
                                    errmsg.Visible = true;
                                    errmsg.Text = "Student Already Dismissed";
                                    return;
                                }
                                else if (suspension == "1")
                                {

                                    int numofdays = Convert.ToInt16(alreadystuddetail.Tables[0].Rows[0]["tot_days"].ToString());
                                    //DateTime startdate = Convert.ToDateTime(alreadystuddetail.Tables[0].Rows[0]["ack_date"].ToString());
                                    DateTime startdate = new DateTime();// Convert.ToDateTime(alreadystuddetail.Tables[0].Rows[0]["ack_date"].ToString());
                                    DateTime.TryParseExact(Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_date"]).Trim(), "dd-MM-yyyy", null, DateTimeStyles.None, out startdate);
                                    startdate = startdate.AddDays(numofdays);

                                    string stdate = startdate.ToString("MM/dd/yyyy");
                                    string[] splitdates = stdate.Split(new Char[] { ' ' });
                                    string spdate = splitdates[0].ToString();
                                    string[] splitd = spdate.Split(new Char[] { '/' });
                                    int splitdatess = Convert.ToInt32(splitd[1]);
                                    int splitmonths = Convert.ToInt32(splitd[0]);
                                    int splityears = Convert.ToInt32(splitd[2]);
                                    string punishdate = splitmonths + "-" + splitdatess + "-" + splityears;

                                    if (Convert.ToDateTime(curr_date) < Convert.ToDateTime(punishdate))
                                    {
                                        errmsg.Visible = true;
                                        errmsg.Text = "Student Already in Suspension";
                                        return;
                                    }
                                    else
                                    {
                                        strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,semester,Remark,ack_remarks" + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + rollNo + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + feeofroll + "','" + Convert.ToString(ddlsemadd.SelectedValue).Trim() + "'," + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                        insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                        lblnorec.Visible = false;
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                                    }
                                    if (sus == 1)
                                    {
                                        int monthyear = splityear * 12 + splitmonth;
                                        string noofhrs = d2.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + "");
                                        if (noofhrs.Trim() != "" && noofhrs != "0" && noofhrs != null)
                                        {
                                            //DateTime datesus = Convert.ToDateTime(curr_date);
                                            //int day = Convert.ToInt32(txtdays.Text);
                                            //string datecolumn = string.Empty;
                                            //string attvalue = string.Empty;
                                            //string dateattvalue = string.Empty;
                                            //for (int date = 0; date < day; date++)
                                            //{
                                            //    datesus = datesus.AddDays(date);
                                            //    string dateva = datesus.Day.ToString();
                                            //    string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + datesus.ToString() + "'";
                                            //    DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                            //    if (dsholiday.Tables[0].Rows.Count == 0)
                                            //    {
                                            //        for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                            //        {
                                            //            if (datecolumn == "")
                                            //            {
                                            //                datecolumn = "d" + dateva + "d" + i + "";
                                            //                attvalue = "9";
                                            //                dateattvalue = "d" + dateva + "d" + i + "=9";
                                            //            }
                                            //            else
                                            //            {
                                            //                datecolumn = "" + datecolumn + "," + "d" + dateva + "d" + i + "";
                                            //                attvalue = attvalue + ',' + "9";
                                            //                dateattvalue = dateattvalue + ',' + "d" + dateva + "d" + i + "=9";
                                            //            }
                                            //        }
                                            //    }
                                            //}

                                            //string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + rollNo + "' and month_year=" + monthyear + "");
                                            //if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                            //{
                                            //    string insquery = "update attendance set " + dateattvalue + " where roll_no='" + rollNo + "' and month_year=" + monthyear + "";
                                            //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                            //}
                                            //else
                                            //{
                                            //    string insquery = "insert into attendance(roll_no,month_year," + datecolumn + ") values('" + rollNo + "'," + monthyear + "," + attvalue + ")";
                                            //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                            //}
                                            DateTime dtDummyStart = new DateTime();
                                            DateTime dtDummyEnd = new DateTime();
                                            int totalDaySuspended = 0;
                                            int.TryParse(txtdays.Text.Trim(), out totalDaySuspended);

                                            ArrayList arrMonthYear = new ArrayList();
                                            Dictionary<long, string> dicQInsertColumn = new Dictionary<long, string>();
                                            Dictionary<long, string> dicQInsertValues = new Dictionary<long, string>();
                                            Dictionary<long, string> dicQUpdate = new Dictionary<long, string>();

                                            string insertColumn = string.Empty;
                                            string insertValue = string.Empty;
                                            string updateValue = string.Empty;

                                            for (DateTime dtTemp = dtSuspendFromDate; dtTemp <= dtSuspendToDate; dtTemp = dtTemp.AddDays(1))
                                            {
                                                string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + dtTemp.ToString() + "'";
                                                DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                                long tempMonthYear = dtTemp.Year * 12 + dtTemp.Month;
                                                insertColumn = string.Empty;
                                                insertValue = string.Empty;
                                                updateValue = string.Empty;
                                                string day = dtTemp.Day.ToString();
                                                if (!arrMonthYear.Contains(tempMonthYear))
                                                {
                                                    arrMonthYear.Add(tempMonthYear);
                                                }
                                                if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                {
                                                    dicQInsertColumn.Add(tempMonthYear, string.Empty);
                                                }
                                                if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                {
                                                    dicQInsertValues.Add(tempMonthYear, string.Empty);
                                                }
                                                if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                {
                                                    dicQUpdate.Add(tempMonthYear, string.Empty);
                                                }
                                                if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count == 0)
                                                {
                                                    for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                                    {
                                                        if (insertColumn == "")
                                                        {
                                                            insertColumn = "d" + day + "d" + i + "";
                                                            insertValue = "9";
                                                            updateValue = "d" + day + "d" + i + "=9";
                                                        }
                                                        else
                                                        {
                                                            insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                            insertValue = insertValue + ',' + "9";
                                                            updateValue = updateValue + ',' + "d" + day + "d" + i + "=9";
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(insertColumn.Trim()))
                                                    {
                                                        if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQInsertColumn.Add(tempMonthYear, insertColumn);
                                                        }
                                                        else
                                                        {
                                                            string value = dicQInsertColumn[tempMonthYear];
                                                            if (!string.IsNullOrEmpty(value))
                                                            {
                                                                dicQInsertColumn[tempMonthYear] = value + "," + insertColumn;
                                                            }
                                                            else
                                                            {
                                                                dicQInsertColumn[tempMonthYear] = insertColumn;
                                                            }
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(insertValue.Trim()))
                                                    {
                                                        if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQInsertValues.Add(tempMonthYear, insertValue);
                                                        }
                                                        else
                                                        {
                                                            string value = dicQInsertValues[tempMonthYear];
                                                            if (!string.IsNullOrEmpty(value))
                                                            {
                                                                dicQInsertValues[tempMonthYear] = value + "," + insertValue;
                                                            }
                                                            else
                                                            {
                                                                dicQInsertValues[tempMonthYear] = insertValue;
                                                            }
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                    {
                                                        if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQUpdate.Add(tempMonthYear, updateValue);
                                                        }
                                                        else
                                                        {
                                                            string value = dicQUpdate[tempMonthYear];
                                                            dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                            if (!string.IsNullOrEmpty(value))
                                                            {
                                                                dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                            }
                                                            else
                                                            {
                                                                dicQUpdate[tempMonthYear] = updateValue;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            foreach (long dicEntry in arrMonthYear)
                                            {
                                                string monthYear = Convert.ToString(dicEntry).Trim();
                                                long longMonthYear = 0;
                                                long.TryParse(monthYear.Trim(), out longMonthYear);
                                                insertColumn = string.Empty;
                                                insertValue = string.Empty;
                                                updateValue = string.Empty;
                                                string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + rollNo + "' and month_year=" + monthYear.Trim() + "");
                                                if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                                {
                                                    if (dicQUpdate.ContainsKey(longMonthYear))
                                                    {
                                                        updateValue = dicQUpdate[longMonthYear];
                                                    }
                                                    if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                    {
                                                        string insquery = "update attendance set " + updateValue + " where roll_no='" + rollNo + "' and month_year=" + monthYear + "";
                                                        int a = d2.update_method_wo_parameter(insquery, "Text");
                                                    }
                                                }
                                                else
                                                {
                                                    if (dicQInsertColumn.ContainsKey(longMonthYear))
                                                    {
                                                        insertColumn = dicQInsertColumn[longMonthYear];
                                                    }
                                                    if (dicQInsertValues.ContainsKey(longMonthYear))
                                                    {
                                                        insertValue = dicQInsertValues[longMonthYear];
                                                    }
                                                    if (!string.IsNullOrEmpty(insertColumn.Trim()) && !string.IsNullOrEmpty(insertValue.Trim()))
                                                    {
                                                        string insquery = "insert into attendance(roll_no,month_year," + insertColumn + ") values('" + rollNo + "'," + monthYear + "," + insertValue + ")";
                                                        int a = d2.update_method_wo_parameter(insquery, "Text");
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                                else if (AckFine.Trim() == "0" && AckWarn.Trim() == "0" && ack_fee_of_roll.Trim() == "0" && ack_remarks.Trim() == "0" && suspension.Trim() == "0" && dismissal.Trim() == "0")
                                {
                                    if (chkfeeofroll.Checked == true)
                                    {
                                        strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,semester,Remark,ack_remarks " + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + rollNo + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + feeofroll + "','" + Convert.ToString(ddlsemadd.SelectedValue).Trim() + "','" + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                        insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                        lblnorec.Visible = false;
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                                    }
                                }
                                #region Added by Idhris for New Remarks -- 03-10-2016
                                try
                                {
                                    if (chkremark.Checked)
                                    {
                                        strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,semester,Remark,ack_remarks" + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + rollNo + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + feeofroll + "','" + Convert.ToString(ddlsemadd.SelectedValue).Trim() + "','" + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                        insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                                    }
                                }
                                catch { ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Not Saved')", true); }
                                #endregion
                            }
                            else
                            {
                                strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,semester,Remark,ack_remarks" + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + rollNo + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + feeofroll + "','" + Convert.ToString(ddlsemadd.SelectedValue) + "','" + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                lblnorec.Visible = false;
                                if (sus == 1)
                                {
                                    int monthyear = splityear * 12 + splitmonth;
                                    string noofhrs = d2.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + "");
                                    if (noofhrs != null && noofhrs.Trim() != "" && noofhrs != "0")
                                    {
                                        //DateTime datesus = Convert.ToDateTime(curr_date);
                                        //int day = Convert.ToInt32(txtdays.Text);
                                        //string datecolumn = string.Empty;
                                        //string attvalue = string.Empty;
                                        //string dateattvalue = string.Empty;
                                        //for (int date = 0; date < day; date++)
                                        //{
                                        //    datesus = datesus.AddDays(date);
                                        //    string dateva = datesus.Day.ToString();
                                        //    string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + datesus.ToString() + "'";
                                        //    DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                        //    if (dsholiday.Tables[0].Rows.Count == 0)
                                        //    {
                                        //        for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                        //        {
                                        //            if (datecolumn == "")
                                        //            {
                                        //                datecolumn = "d" + dateva + "d" + i + "";
                                        //                attvalue = "9";
                                        //                dateattvalue = "d" + dateva + "d" + i + "=9";
                                        //            }
                                        //            else
                                        //            {
                                        //                datecolumn = "" + datecolumn + "," + "d" + dateva + "d" + i + "";
                                        //                attvalue = attvalue + ',' + "9";
                                        //                dateattvalue = dateattvalue + ',' + "d" + dateva + "d" + i + "=9";
                                        //            }
                                        //        }
                                        //    }
                                        //}

                                        //string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + rollNo + "' and month_year=" + monthyear + "");
                                        //if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                        //{
                                        //    string insquery = "update attendance set " + dateattvalue + " where roll_no='" + rollNo + "' and month_year=" + monthyear + "";
                                        //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                        //}
                                        //else
                                        //{
                                        //    string insquery = "insert into attendance(roll_no,month_year," + datecolumn + ") values('" + rollNo + "'," + monthyear + "," + attvalue + ")";
                                        //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                        //}
                                        DateTime dtDummyStart = new DateTime();
                                        DateTime dtDummyEnd = new DateTime();
                                        int totalDaySuspended = 0;
                                        int.TryParse(txtdays.Text.Trim(), out totalDaySuspended);

                                        ArrayList arrMonthYear = new ArrayList();
                                        Dictionary<long, string> dicQInsertColumn = new Dictionary<long, string>();
                                        Dictionary<long, string> dicQInsertValues = new Dictionary<long, string>();
                                        Dictionary<long, string> dicQUpdate = new Dictionary<long, string>();

                                        string insertColumn = string.Empty;
                                        string insertValue = string.Empty;
                                        string updateValue = string.Empty;

                                        for (DateTime dtTemp = dtSuspendFromDate; dtTemp <= dtSuspendToDate; dtTemp = dtTemp.AddDays(1))
                                        {
                                            string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + dtTemp.ToString() + "'";
                                            DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                            long tempMonthYear = dtTemp.Year * 12 + dtTemp.Month;
                                            insertColumn = string.Empty;
                                            insertValue = string.Empty;
                                            updateValue = string.Empty;
                                            string day = dtTemp.Day.ToString();
                                            if (!arrMonthYear.Contains(tempMonthYear))
                                            {
                                                arrMonthYear.Add(tempMonthYear);
                                            }
                                            if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                            {
                                                dicQInsertColumn.Add(tempMonthYear, string.Empty);
                                            }
                                            if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                            {
                                                dicQInsertValues.Add(tempMonthYear, string.Empty);
                                            }
                                            if (!dicQUpdate.ContainsKey(tempMonthYear))
                                            {
                                                dicQUpdate.Add(tempMonthYear, string.Empty);
                                            }
                                            if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count == 0)
                                            {
                                                for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                                {
                                                    if (insertColumn == "")
                                                    {
                                                        insertColumn = "d" + day + "d" + i + "";
                                                        insertValue = "9";
                                                        updateValue = "d" + day + "d" + i + "=9";
                                                    }
                                                    else
                                                    {
                                                        insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                        insertValue = insertValue + ',' + "9";
                                                        updateValue = updateValue + ',' + "d" + day + "d" + i + "=9";
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(insertColumn.Trim()))
                                                {
                                                    if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQInsertColumn.Add(tempMonthYear, insertColumn);
                                                    }
                                                    else
                                                    {
                                                        string value = dicQInsertColumn[tempMonthYear];
                                                        if (!string.IsNullOrEmpty(value))
                                                        {
                                                            dicQInsertColumn[tempMonthYear] = value + "," + insertColumn;
                                                        }
                                                        else
                                                        {
                                                            dicQInsertColumn[tempMonthYear] = insertColumn;
                                                        }
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(insertValue.Trim()))
                                                {
                                                    if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQInsertValues.Add(tempMonthYear, insertValue);
                                                    }
                                                    else
                                                    {
                                                        string value = dicQInsertValues[tempMonthYear];
                                                        if (!string.IsNullOrEmpty(value))
                                                        {
                                                            dicQInsertValues[tempMonthYear] = value + "," + insertValue;
                                                        }
                                                        else
                                                        {
                                                            dicQInsertValues[tempMonthYear] = insertValue;
                                                        }
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                {
                                                    if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQUpdate.Add(tempMonthYear, updateValue);
                                                    }
                                                    else
                                                    {
                                                        string value = dicQUpdate[tempMonthYear];
                                                        dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                        if (!string.IsNullOrEmpty(value))
                                                        {
                                                            dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                        }
                                                        else
                                                        {
                                                            dicQUpdate[tempMonthYear] = updateValue;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        foreach (long dicEntry in arrMonthYear)
                                        {
                                            string monthYear = Convert.ToString(dicEntry).Trim();
                                            long longMonthYear = 0;
                                            long.TryParse(monthYear.Trim(), out longMonthYear);
                                            insertColumn = string.Empty;
                                            insertValue = string.Empty;
                                            updateValue = string.Empty;
                                            string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + rollNo + "' and month_year=" + monthYear.Trim() + "");
                                            if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                            {
                                                if (dicQUpdate.ContainsKey(longMonthYear))
                                                {
                                                    updateValue = dicQUpdate[longMonthYear];
                                                }
                                                if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                {
                                                    string insquery = "update attendance set " + updateValue + " where roll_no='" + rollNo + "' and month_year=" + monthYear + "";
                                                    int a = d2.update_method_wo_parameter(insquery, "Text");
                                                }
                                            }
                                            else
                                            {
                                                if (dicQInsertColumn.ContainsKey(longMonthYear))
                                                {
                                                    insertColumn = dicQInsertColumn[longMonthYear];
                                                }
                                                if (dicQInsertValues.ContainsKey(longMonthYear))
                                                {
                                                    insertValue = dicQInsertValues[longMonthYear];
                                                }
                                                if (!string.IsNullOrEmpty(insertColumn.Trim()) && !string.IsNullOrEmpty(insertValue.Trim()))
                                                {
                                                    string insquery = "insert into attendance(roll_no,month_year," + insertColumn + ") values('" + rollNo + "'," + monthYear + "," + insertValue + ")";
                                                    int a = d2.update_method_wo_parameter(insquery, "Text");
                                                }
                                            }
                                        }
                                    }
                                }
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                            }
                        }
                        else if (btnsave.Text == "Update")
                        {
                            string strinsselectquery = "select convert(nvarchar(15),suspendFromDate,103) suspendFromDate,convert(nvarchar(15),suspendToDate,103) suspendToDate from stucon where roll_no = '" + rollNo + "' and StuConID='" + lblindex.Text.Trim() + "'";
                            DataSet selectqueryy = d2.select_method_wo_parameter(strinsselectquery, "text");

                            strinsupdaequery = "update stucon set curr_date = '" + curr_date + "',infr_type = '" + ddlfraction.SelectedItem + "',ack_diss = '" + dis + "',ack_susp = '" + sus + "',ack_fine = '" + fin + "',ack_warn = '" + war + "',prof_code = '" + lblerrstaffcode.Text + "',ack_date = '" + ack_date + "',tot_days = '" + txtdays.Text + "',fine_amo = '" + txtfine.Text + "',ack_fee_of_roll='" + feeofroll + "',Remark='" + txtremarks.Text.ToString() + "',ack_remarks='" + remarkval.ToString() + "' " + qryFeeOnRollUpdate + qrySuspendUpdate + " where roll_no = '" + rollNo + "' and StuConID='" + lblindex.Text.Trim() + "'";
                            insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                            lblnorec.Visible = false;
                            if (sus == 1)
                            {
                                int monthyear = splityear * 12 + splitmonth;
                                string fromdate = Convert.ToString(selectqueryy.Tables[0].Rows[0]["suspendFromDate"]).Trim();
                                string todayte = Convert.ToString(selectqueryy.Tables[0].Rows[0]["suspendToDate"]).Trim();
                                string noofhrs = d2.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + "");

                                DateTime upSuspendFromDate = new DateTime();
                                DateTime upSuspendToDate = new DateTime();
                                string[] spl = fromdate.Split('/');
                                string upSuspendFromDates = spl[0] + '-' + spl[1] + '-' + spl[2];
                                string[] spl2 = todayte.Split('/');
                                string upSuspendtoDates = spl2[0] + '-' + spl2[1] + '-' + spl2[2];
                                bool isFromSucces = DateTime.TryParseExact(upSuspendFromDates, "dd-MM-yyyy", null, DateTimeStyles.None, out upSuspendFromDate);
                                bool isToSucces = DateTime.TryParseExact(upSuspendtoDates, "dd-MM-yyyy", null, DateTimeStyles.None, out upSuspendToDate);

                                if (noofhrs.Trim() != "" && noofhrs != "0" && noofhrs != null)
                                {
                                    //DateTime datesus = Convert.ToDateTime(curr_date);
                                    //int day = Convert.ToInt32(txtdays.Text);
                                    //string datecolumn = string.Empty;
                                    //string attvalue = string.Empty;
                                    //string dateattvalue = string.Empty;
                                    //for (int date = 0; date < day; date++)
                                    //{
                                    //    datesus = datesus.AddDays(date);
                                    //    string dateva = datesus.Day.ToString();
                                    //    string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + datesus.ToString() + "'";
                                    //    DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                    //    if (dsholiday.Tables[0].Rows.Count == 0)
                                    //    {
                                    //        for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                    //        {
                                    //            if (datecolumn == "")
                                    //            {
                                    //                datecolumn = "d" + dateva + "d" + i + "";
                                    //                attvalue = "9";
                                    //                dateattvalue = "d" + dateva + "d" + i + "=9";
                                    //            }
                                    //            else
                                    //            {
                                    //                datecolumn = "" + datecolumn + "," + "d" + dateva + "d" + i + "";
                                    //                attvalue = attvalue + ',' + "9";
                                    //                dateattvalue = dateattvalue + ',' + "d" + dateva + "d" + i + "=9";
                                    //            }
                                    //        }
                                    //    }
                                    //}

                                    //string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + rollNo + "' and month_year=" + monthyear + "");
                                    //if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                    //{
                                    //    string insquery = "update attendance set " + dateattvalue + " where roll_no='" + rollNo + "' and month_year=" + monthyear + "";
                                    //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                    //}
                                    //else
                                    //{
                                    //    string insquery = "insert into attendance(roll_no,month_year," + datecolumn + ") values('" + rollNo + "'," + monthyear + "," + attvalue + ")";
                                    //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                    //}

                                    DateTime dtDummyStart = new DateTime();
                                    DateTime dtDummyEnd = new DateTime();
                                    int totalDaySuspended = 0;
                                    int.TryParse(txtdays.Text.Trim(), out totalDaySuspended);
                                    DateTime dtDummyStartd = new DateTime();
                                    DateTime dtDummyStartto = new DateTime();
                                    ArrayList arrMonthYear = new ArrayList();
                                    Dictionary<long, string> dicQInsertColumn = new Dictionary<long, string>();
                                    Dictionary<long, string> dicQInsertValues = new Dictionary<long, string>();
                                    Dictionary<long, string> dicQUpdate = new Dictionary<long, string>();

                                    string insertColumn = string.Empty;
                                    string insertValue = string.Empty;
                                    string updateValue = string.Empty;
                                    if (upSuspendFromDate <= dtSuspendFromDate)
                                    {
                                        dtDummyStartd = upSuspendFromDate;
                                    }
                                    else
                                    {
                                        dtDummyStartd = dtSuspendFromDate;
                                    }
                                    if (upSuspendToDate >= dtSuspendToDate)
                                    {
                                        dtDummyStartto = upSuspendToDate;
                                    }
                                    else
                                    {
                                        dtDummyStartto = dtSuspendToDate;
                                    }
                                   // for (DateTime dtTemp = dtSuspendFromDate; dtTemp <= dtSuspendToDate; dtTemp = dtTemp.AddDays(1))
                                    for (DateTime dtTemp = dtDummyStartd; dtTemp <= dtDummyStartto; dtTemp = dtTemp.AddDays(1))
                                    {
                                        string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + dtTemp.ToString() + "'";
                                        DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                        long tempMonthYear = dtTemp.Year * 12 + dtTemp.Month;
                                        insertColumn = string.Empty;
                                        insertValue = string.Empty;
                                        updateValue = string.Empty;
                                        string day = dtTemp.Day.ToString();
                                        if (!arrMonthYear.Contains(tempMonthYear))
                                        {
                                            arrMonthYear.Add(tempMonthYear);
                                        }
                                        if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                        {
                                            dicQInsertColumn.Add(tempMonthYear, string.Empty);
                                        }
                                        if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                        {
                                            dicQInsertValues.Add(tempMonthYear, string.Empty);
                                        }
                                        if (!dicQUpdate.ContainsKey(tempMonthYear))
                                        {
                                            dicQUpdate.Add(tempMonthYear, string.Empty);
                                        }
                                        if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count == 0)
                                        {
                                            for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                            {
                                                if (dtTemp >= dtSuspendFromDate && dtTemp <= dtSuspendToDate)
                                                {

                                                    if (insertColumn == "")
                                                    {
                                                        insertColumn = "d" + day + "d" + i + "";
                                                        insertValue = "9";
                                                        updateValue = "d" + day + "d" + i + "=9";
                                                    }
                                                    else
                                                    {
                                                        insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                        insertValue = insertValue + ',' + "9";
                                                        updateValue = updateValue + ',' + "d" + day + "d" + i + "=9";
                                                    }
                                                }
                                                else
                                                {
                                                    if (insertColumn == "")
                                                    {
                                                        insertColumn = "d" + day + "d" + i + "";
                                                        insertValue = "";
                                                        updateValue = "d" + day + "d" + i + "=''";
                                                    }
                                                    else
                                                    {
                                                        insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                        insertValue = insertValue + ',' + "";
                                                        updateValue = updateValue + ',' + "d" + day + "d" + i + "=''";
                                                    }
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(insertColumn.Trim()))
                                            {
                                                if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                {
                                                    dicQInsertColumn.Add(tempMonthYear, insertColumn);
                                                }
                                                else
                                                {
                                                    string value = dicQInsertColumn[tempMonthYear];
                                                    if (!string.IsNullOrEmpty(value))
                                                    {
                                                        dicQInsertColumn[tempMonthYear] = value + "," + insertColumn;
                                                    }
                                                    else
                                                    {
                                                        dicQInsertColumn[tempMonthYear] = insertColumn;
                                                    }
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(insertValue.Trim()))
                                            {
                                                if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                {
                                                    dicQInsertValues.Add(tempMonthYear, insertValue);
                                                }
                                                else
                                                {
                                                    string value = dicQInsertValues[tempMonthYear];
                                                    if (!string.IsNullOrEmpty(value))
                                                    {
                                                        dicQInsertValues[tempMonthYear] = value + "," + insertValue;
                                                    }
                                                    else
                                                    {
                                                        dicQInsertValues[tempMonthYear] = insertValue;
                                                    }
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(updateValue.Trim()))
                                            {
                                                if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                {
                                                    dicQUpdate.Add(tempMonthYear, updateValue);
                                                }
                                                else
                                                {
                                                    string value = dicQUpdate[tempMonthYear];
                                                    dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                    if (!string.IsNullOrEmpty(value))
                                                    {
                                                        dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                    }
                                                    else
                                                    {
                                                        dicQUpdate[tempMonthYear] = updateValue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    foreach (long dicEntry in arrMonthYear)
                                    {
                                        string monthYear = Convert.ToString(dicEntry).Trim();
                                        long longMonthYear = 0;
                                        long.TryParse(monthYear.Trim(), out longMonthYear);
                                        insertColumn = string.Empty;
                                        insertValue = string.Empty;
                                        updateValue = string.Empty;
                                        string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + rollNo + "' and month_year=" + monthYear.Trim() + "");
                                        if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                        {
                                            if (dicQUpdate.ContainsKey(longMonthYear))
                                            {
                                                updateValue = dicQUpdate[longMonthYear];
                                            }
                                            if (!string.IsNullOrEmpty(updateValue.Trim()))
                                            {
                                                string insquery = "update attendance set " + updateValue + " where roll_no='" + rollNo + "' and month_year=" + monthYear + "";
                                                int a = d2.update_method_wo_parameter(insquery, "Text");
                                            }
                                        }
                                        else
                                        {
                                            if (dicQInsertColumn.ContainsKey(longMonthYear))
                                            {
                                                insertColumn = dicQInsertColumn[longMonthYear];
                                            }
                                            if (dicQInsertValues.ContainsKey(longMonthYear))
                                            {
                                                insertValue = dicQInsertValues[longMonthYear];
                                            }
                                            if (!string.IsNullOrEmpty(insertColumn.Trim()) && !string.IsNullOrEmpty(insertValue.Trim()))
                                            {
                                                string insquery = "insert into attendance(roll_no,month_year," + insertColumn + ") values('" + rollNo + "'," + monthYear + "," + insertValue + ")";
                                                int a = d2.update_method_wo_parameter(insquery, "Text");
                                            }
                                        }
                                    }
                                }
                            }
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Updated Successfully')", true);
                        }
                    }
                    else
                    {
                        for (int res = 1; res <= Convert.ToInt32(sprdselectrollno.Sheets[0].RowCount) - 1; res++)
                        {
                            int isval = 0;
                            string s = sprdselectrollno.Sheets[0].Cells[res, 5].Text;
                            isval = Convert.ToInt32(sprdselectrollno.Sheets[0].Cells[res, 5].Value);
                            if (isval == 1)
                            {
                                flag = 1;
                                string stdRollno = sprdselectrollno.Sheets[0].Cells[res, 1].Value.ToString();
                                string smester = sprdselectrollno.Sheets[0].Cells[res, 6].Value.ToString();
                                string Identity = Convert.ToString(sprdselectrollno.Sheets[0].Cells[res, 3].Tag);
                                if (btnsave.Text == "Save")
                                {
                                    string strquer = "select roll_no,convert(varchar(100),curr_date,105) as curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,convert(varchar(100),cast(ack_date as DateTime),105) as ack_date,tot_days,fine_amo,serial_no,semester,ack_fee_of_roll,Remark,ack_remarks,convert(varchar(100),cast(ack_date as DateTime),105) as feeOffRollDate,convert(varchar(100),feeOnRollDate,105) as feeOnRollDate,convert(varchar(50),suspendFromDate,105) as suspendFromDate,convert(varchar(50),suspendToDate,105) as suspendToDate from stucon where roll_no='" + stdRollno + "' order by convert(varchar(100),cast(ack_date as DateTime),105) desc";
                                    DataSet alreadystuddetail = d2.select_method_wo_parameter(strquer, "Text");
                                    if (alreadystuddetail.Tables.Count > 0 && alreadystuddetail.Tables[0].Rows.Count > 0)
                                    {
                                        string dismissal = string.Empty;
                                        dismissal = alreadystuddetail.Tables[0].Rows[0]["ack_diss"].ToString();
                                        string suspension = alreadystuddetail.Tables[0].Rows[0]["ack_susp"].ToString();

                                        string AckFine = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_fine"]).Trim();
                                        string AckWarn = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_warn"]).Trim();
                                        string ack_fee_of_roll = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_fee_of_roll"]).Trim();
                                        string ack_remarks = Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_remarks"]).Trim();
                                        if (dismissal == "1")
                                        {
                                            errmsg.Visible = true;
                                            errmsg.Text = "Student Already Dismissed";
                                            return;
                                        }
                                        else if (suspension == "1")
                                        {

                                            int numofdays = Convert.ToInt16(alreadystuddetail.Tables[0].Rows[0]["tot_days"].ToString());
                                            //DateTime startdate = Convert.ToDateTime(alreadystuddetail.Tables[0].Rows[0]["ack_date"].ToString());
                                            DateTime startdate = new DateTime();// Convert.ToDateTime(alreadystuddetail.Tables[0].Rows[0]["ack_date"].ToString());
                                            DateTime.TryParseExact(Convert.ToString(alreadystuddetail.Tables[0].Rows[0]["ack_date"]).Trim(), "dd-MM-yyyy", null, DateTimeStyles.None, out startdate);
                                            startdate = startdate.AddDays(numofdays);
                                            string stdate = startdate.ToString("MM/dd/yyyy");

                                            string[] splitdates = stdate.Split(new Char[] { ' ' });
                                            string spdate = splitdates[0].ToString();
                                            string[] splitd = spdate.Split(new Char[] { '/' });
                                            int splitdatess = Convert.ToInt32(splitd[1]);
                                            int splitmonths = Convert.ToInt32(splitd[0]);
                                            int splityears = Convert.ToInt32(splitd[2]);
                                            string punishdate = splitmonths + "-" + splitdatess + "-" + splityears;

                                            if (Convert.ToDateTime(curr_date) < Convert.ToDateTime(punishdate))
                                            {
                                                errmsg.Visible = true;
                                                errmsg.Text = "Student Already in Suspension";
                                                return;
                                            }
                                            else
                                            {
                                                strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,semester,Remark,ack_remarks" + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + stdRollno + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + feeofroll + "','" + Convert.ToString(ddlsemadd.SelectedValue) + "','" + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                                insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                                lblnorec.Visible = false;
                                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                                            }
                                            if (sus == 1)
                                            {
                                                int monthyear = splityear * 12 + splitmonth;
                                                string noofhrs = d2.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + "");
                                                if (noofhrs.Trim() != "" && noofhrs != "0" && noofhrs != null)
                                                {
                                                    //DateTime datesus = Convert.ToDateTime(curr_date);
                                                    //int day = Convert.ToInt32(txtdays.Text);
                                                    //string datecolumn = string.Empty;
                                                    //string attvalue = string.Empty;
                                                    //string dateattvalue = string.Empty;
                                                    //for (int date = 0; date < day; date++)
                                                    //{
                                                    //    datesus = datesus.AddDays(date);
                                                    //    string dateva = datesus.Day.ToString();
                                                    //    string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + datesus.ToString() + "'";
                                                    //    DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                                    //    if (dsholiday.Tables[0].Rows.Count == 0)
                                                    //    {
                                                    //        for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                                    //        {
                                                    //            if (datecolumn == "")
                                                    //            {
                                                    //                datecolumn = "d" + dateva + "d" + i + "";
                                                    //                attvalue = "9";
                                                    //                dateattvalue = "d" + dateva + "d" + i + "=9";
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                datecolumn = "" + datecolumn + "," + "d" + dateva + "d" + i + "";
                                                    //                attvalue = attvalue + ',' + "9";
                                                    //                dateattvalue = dateattvalue + ',' + "d" + dateva + "d" + i + "=9";
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}

                                                    //string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + stdRollno + "' and month_year=" + monthyear + "");
                                                    //if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                                    //{
                                                    //    string insquery = "update attendance set " + dateattvalue + " where roll_no='" + stdRollno + "' and month_year=" + monthyear + "";
                                                    //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                                    //}
                                                    //else
                                                    //{
                                                    //    string insquery = "insert into attendance(roll_no,month_year," + datecolumn + ") values('" + stdRollno + "'," + monthyear + "," + attvalue + ")";
                                                    //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                                    //}
                                                    DateTime dtDummyStart = new DateTime();
                                                    DateTime dtDummyEnd = new DateTime();
                                                    int totalDaySuspended = 0;
                                                    int.TryParse(txtdays.Text.Trim(), out totalDaySuspended);

                                                    ArrayList arrMonthYear = new ArrayList();
                                                    Dictionary<long, string> dicQInsertColumn = new Dictionary<long, string>();
                                                    Dictionary<long, string> dicQInsertValues = new Dictionary<long, string>();
                                                    Dictionary<long, string> dicQUpdate = new Dictionary<long, string>();

                                                    string insertColumn = string.Empty;
                                                    string insertValue = string.Empty;
                                                    string updateValue = string.Empty;

                                                    for (DateTime dtTemp = dtSuspendFromDate; dtTemp <= dtSuspendToDate; dtTemp = dtTemp.AddDays(1))
                                                    {
                                                        string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + dtTemp.ToString() + "'";
                                                        DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                                        long tempMonthYear = dtTemp.Year * 12 + dtTemp.Month;
                                                        insertColumn = string.Empty;
                                                        insertValue = string.Empty;
                                                        updateValue = string.Empty;
                                                        string day = dtTemp.Day.ToString();
                                                        if (!arrMonthYear.Contains(tempMonthYear))
                                                        {
                                                            arrMonthYear.Add(tempMonthYear);
                                                        }
                                                        if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQInsertColumn.Add(tempMonthYear, string.Empty);
                                                        }
                                                        if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQInsertValues.Add(tempMonthYear, string.Empty);
                                                        }
                                                        if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQUpdate.Add(tempMonthYear, string.Empty);
                                                        }
                                                        if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count == 0)
                                                        {
                                                            for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                                            {
                                                                if (insertColumn == "")
                                                                {
                                                                    insertColumn = "d" + day + "d" + i + "";
                                                                    insertValue = "9";
                                                                    updateValue = "d" + day + "d" + i + "=9";
                                                                }
                                                                else
                                                                {
                                                                    insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                                    insertValue = insertValue + ',' + "9";
                                                                    updateValue = updateValue + ',' + "d" + day + "d" + i + "=9";
                                                                }
                                                            }
                                                            if (!string.IsNullOrEmpty(insertColumn.Trim()))
                                                            {
                                                                if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                                {
                                                                    dicQInsertColumn.Add(tempMonthYear, insertColumn);
                                                                }
                                                                else
                                                                {
                                                                    string value = dicQInsertColumn[tempMonthYear];
                                                                    if (!string.IsNullOrEmpty(value))
                                                                    {
                                                                        dicQInsertColumn[tempMonthYear] = value + "," + insertColumn;
                                                                    }
                                                                    else
                                                                    {
                                                                        dicQInsertColumn[tempMonthYear] = insertColumn;
                                                                    }
                                                                }
                                                            }
                                                            if (!string.IsNullOrEmpty(insertValue.Trim()))
                                                            {
                                                                if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                                {
                                                                    dicQInsertValues.Add(tempMonthYear, insertValue);
                                                                }
                                                                else
                                                                {
                                                                    string value = dicQInsertValues[tempMonthYear];
                                                                    if (!string.IsNullOrEmpty(value))
                                                                    {
                                                                        dicQInsertValues[tempMonthYear] = value + "," + insertValue;
                                                                    }
                                                                    else
                                                                    {
                                                                        dicQInsertValues[tempMonthYear] = insertValue;
                                                                    }
                                                                }
                                                            }
                                                            if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                            {
                                                                if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                                {
                                                                    dicQUpdate.Add(tempMonthYear, updateValue);
                                                                }
                                                                else
                                                                {
                                                                    string value = dicQUpdate[tempMonthYear];
                                                                    dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                                    if (!string.IsNullOrEmpty(value))
                                                                    {
                                                                        dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                                    }
                                                                    else
                                                                    {
                                                                        dicQUpdate[tempMonthYear] = updateValue;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    foreach (long dicEntry in arrMonthYear)
                                                    {
                                                        string monthYear = Convert.ToString(dicEntry).Trim();
                                                        long longMonthYear = 0;
                                                        long.TryParse(monthYear.Trim(), out longMonthYear);
                                                        insertColumn = string.Empty;
                                                        insertValue = string.Empty;
                                                        updateValue = string.Empty;
                                                        string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + stdRollno + "' and month_year=" + monthYear.Trim() + "");
                                                        if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                                        {
                                                            if (dicQUpdate.ContainsKey(longMonthYear))
                                                            {
                                                                updateValue = dicQUpdate[longMonthYear];
                                                            }
                                                            if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                            {
                                                                string insquery = "update attendance set " + updateValue + " where roll_no='" + stdRollno + "' and month_year=" + monthYear + "";
                                                                int a = d2.update_method_wo_parameter(insquery, "Text");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (dicQInsertColumn.ContainsKey(longMonthYear))
                                                            {
                                                                insertColumn = dicQInsertColumn[longMonthYear];
                                                            }
                                                            if (dicQInsertValues.ContainsKey(longMonthYear))
                                                            {
                                                                insertValue = dicQInsertValues[longMonthYear];
                                                            }
                                                            if (!string.IsNullOrEmpty(insertColumn.Trim()) && !string.IsNullOrEmpty(insertValue.Trim()))
                                                            {
                                                                string insquery = "insert into attendance(roll_no,month_year," + insertColumn + ") values('" + stdRollno + "'," + monthYear + "," + insertValue + ")";
                                                                int a = d2.update_method_wo_parameter(insquery, "Text");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                string fine = alreadystuddetail.Tables[0].Rows[0]["ack_fine"].ToString();
                                                if (fine == "1")
                                                {
                                                    //  int numofdays = Convert.ToInt16(alreadystuddetail.Tables[0].Rows[0]["tot_days"].ToString());
                                                    string datef = d2.GetFunction("select ack_date from stucon where ack_fine=1 and curr_date='" + curr_date + "'");
                                                    if (datef.Trim() != null && datef.Trim() != "" && datef.Trim() != "0")
                                                    {
                                                        startdate = Convert.ToDateTime(datef);
                                                    }
                                                    else
                                                    {
                                                        startdate = Convert.ToDateTime("01/01/1995");
                                                    }
                                                    // startdate = Convert.ToDateTime(alreadystuddetail.Tables[0].Rows[0]["ack_date"].ToString());
                                                    //    startdate = startdate.AddDays(numofdays);
                                                    stdate = startdate.ToString();

                                                    splitdates = stdate.Split(new Char[] { ' ' });
                                                    spdate = splitdates[0].ToString();
                                                    splitd = spdate.Split(new Char[] { '/' });
                                                    splitdatess = Convert.ToInt32(splitd[1]);
                                                    splitmonths = Convert.ToInt32(splitd[0]);
                                                    splityears = Convert.ToInt32(splitd[2]);
                                                    punishdate = splitmonths + "-" + splitdatess + "-" + splityears;
                                                    if (Convert.ToDateTime(curr_date) < Convert.ToDateTime(punishdate))
                                                    {
                                                        errmsg.Visible = true;
                                                        errmsg.Text = "Student Already Entered Fine";
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,semester,Remark,ack_remarks" + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + stdRollno + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + feeofroll + "','" + Convert.ToString(ddlsemadd.SelectedValue) + "','" + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                                        insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                                        lblnorec.Visible = false;
                                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                                                    }
                                                }
                                                else
                                                {

                                                }
                                            }

                                        }
                                        else if (AckFine.Trim() == "0" && AckWarn.Trim() == "0" && ack_fee_of_roll.Trim() == "0" && ack_remarks.Trim() == "0" && suspension.Trim() == "0" && dismissal.Trim() == "0")
                                        {
                                            if (chkfeeofroll.Checked == true)
                                            {
                                                strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,ack_fee_of_roll,semester,Remark,ack_remarks " + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + stdRollno + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + feeofroll + "','" + Convert.ToString(ddlsemadd.SelectedValue).Trim() + "','" + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                                insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                                lblnorec.Visible = false;
                                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                                            }
                                        }
                                        #region Added by Idhris for New Remarks -- 03-10-2016
                                        try
                                        {
                                            if (chkremark.Checked)
                                            {
                                                strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,semester,ack_fee_of_roll,Remark,ack_remarks" + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + stdRollno + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + smester + "','" + feeofroll + "','" + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "'" + qryFeeOnRollValue + qrySuspendValue + ")";
                                                insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                                            }
                                        }
                                        catch { ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Not Saved')", true); }
                                        #endregion
                                    }
                                    else
                                    {
                                        strinsupdaequery = "insert into stucon(roll_no,curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,ack_date,tot_days,fine_amo,semester,ack_fee_of_roll,Remark,ack_remarks" + qryFeeOnRollInsert + qrySuspendInsert + ")values('" + stdRollno + "','" + curr_date + "','" + ddlfraction.SelectedItem + "','" + dis + "','" + sus + "','" + fin + "','" + war + "','" + lblerrstaffcode.Text + "','" + ack_date + "','" + txtdays.Text + "','" + txtfine.Text + "','" + smester + "','" + feeofroll + "','" + txtremarks.Text.ToString() + "','" + remarkval.ToString() + "' " + qryFeeOnRollValue + qrySuspendValue + ")";
                                        insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                        lblnorec.Visible = false;
                                        errmsg.Visible = false;
                                        if (sus == 1)
                                        {
                                            int monthyear = splityear * 12 + splitmonth;
                                            string noofhrs = d2.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + "");
                                            if (noofhrs.Trim() != "" && noofhrs != "0" && noofhrs != null)
                                            {
                                                //DateTime datesus = Convert.ToDateTime(curr_date);
                                                //int day = Convert.ToInt32(txtdays.Text);
                                                //string datecolumn = string.Empty;
                                                //string attvalue = string.Empty;
                                                //string dateattvalue = string.Empty;
                                                //for (int date = 0; date < day; date++)
                                                //{
                                                //    datesus = datesus.AddDays(date);
                                                //    string dateva = datesus.Day.ToString();
                                                //    string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + datesus.ToString() + "'";
                                                //    DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                                //    if (dsholiday.Tables[0].Rows.Count == 0)
                                                //    {
                                                //        for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                                //        {
                                                //            if (datecolumn == "")
                                                //            {
                                                //                datecolumn = "d" + dateva + "d" + i + "";
                                                //                attvalue = "9";
                                                //                dateattvalue = "d" + dateva + "d" + i + "=9";
                                                //            }
                                                //            else
                                                //            {
                                                //                datecolumn = "" + datecolumn + "," + "d" + dateva + "d" + i + "";
                                                //                attvalue = attvalue + ',' + "9";
                                                //                dateattvalue = dateattvalue + ',' + "d" + dateva + "d" + i + "=9";
                                                //            }
                                                //        }
                                                //    }
                                                //}

                                                //string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + stdRollno + "' and month_year=" + monthyear + "");
                                                //if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                                //{
                                                //    string insquery = "update attendance set " + dateattvalue + " where roll_no='" + stdRollno + "' and month_year=" + monthyear + "";
                                                //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                                //}
                                                //else
                                                //{
                                                //    string insquery = "insert into attendance(roll_no,month_year," + datecolumn + ") values('" + stdRollno + "'," + monthyear + "," + attvalue + ")";
                                                //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                                //}

                                                DateTime dtDummyStart = new DateTime();
                                                DateTime dtDummyEnd = new DateTime();
                                                int totalDaySuspended = 0;
                                                int.TryParse(txtdays.Text.Trim(), out totalDaySuspended);

                                                ArrayList arrMonthYear = new ArrayList();
                                                Dictionary<long, string> dicQInsertColumn = new Dictionary<long, string>();
                                                Dictionary<long, string> dicQInsertValues = new Dictionary<long, string>();
                                                Dictionary<long, string> dicQUpdate = new Dictionary<long, string>();

                                                string insertColumn = string.Empty;
                                                string insertValue = string.Empty;
                                                string updateValue = string.Empty;

                                                for (DateTime dtTemp = dtSuspendFromDate; dtTemp <= dtSuspendToDate; dtTemp = dtTemp.AddDays(1))
                                                {
                                                    string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + dtTemp.ToString() + "'";
                                                    DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                                    long tempMonthYear = dtTemp.Year * 12 + dtTemp.Month;
                                                    insertColumn = string.Empty;
                                                    insertValue = string.Empty;
                                                    updateValue = string.Empty;
                                                    string day = dtTemp.Day.ToString();
                                                    if (!arrMonthYear.Contains(tempMonthYear))
                                                    {
                                                        arrMonthYear.Add(tempMonthYear);
                                                    }
                                                    if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQInsertColumn.Add(tempMonthYear, string.Empty);
                                                    }
                                                    if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQInsertValues.Add(tempMonthYear, string.Empty);
                                                    }
                                                    if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                    {
                                                        dicQUpdate.Add(tempMonthYear, string.Empty);
                                                    }
                                                    if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count == 0)
                                                    {
                                                        for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                                        {
                                                            if (insertColumn == "")
                                                            {
                                                                insertColumn = "d" + day + "d" + i + "";
                                                                insertValue = "9";
                                                                updateValue = "d" + day + "d" + i + "=9";
                                                            }
                                                            else
                                                            {
                                                                insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                                insertValue = insertValue + ',' + "9";
                                                                updateValue = updateValue + ',' + "d" + day + "d" + i + "=9";
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(insertColumn.Trim()))
                                                        {
                                                            if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                            {
                                                                dicQInsertColumn.Add(tempMonthYear, insertColumn);
                                                            }
                                                            else
                                                            {
                                                                string value = dicQInsertColumn[tempMonthYear];
                                                                if (!string.IsNullOrEmpty(value))
                                                                {
                                                                    dicQInsertColumn[tempMonthYear] = value + "," + insertColumn;
                                                                }
                                                                else
                                                                {
                                                                    dicQInsertColumn[tempMonthYear] = insertColumn;
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(insertValue.Trim()))
                                                        {
                                                            if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                            {
                                                                dicQInsertValues.Add(tempMonthYear, insertValue);
                                                            }
                                                            else
                                                            {
                                                                string value = dicQInsertValues[tempMonthYear];
                                                                if (!string.IsNullOrEmpty(value))
                                                                {
                                                                    dicQInsertValues[tempMonthYear] = value + "," + insertValue;
                                                                }
                                                                else
                                                                {
                                                                    dicQInsertValues[tempMonthYear] = insertValue;
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                        {
                                                            if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                            {
                                                                dicQUpdate.Add(tempMonthYear, updateValue);
                                                            }
                                                            else
                                                            {
                                                                string value = dicQUpdate[tempMonthYear];
                                                                dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                                if (!string.IsNullOrEmpty(value))
                                                                {
                                                                    dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                                }
                                                                else
                                                                {
                                                                    dicQUpdate[tempMonthYear] = updateValue;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                foreach (long dicEntry in arrMonthYear)
                                                {
                                                    string monthYear = Convert.ToString(dicEntry).Trim();
                                                    long longMonthYear = 0;
                                                    long.TryParse(monthYear.Trim(), out longMonthYear);
                                                    insertColumn = string.Empty;
                                                    insertValue = string.Empty;
                                                    updateValue = string.Empty;
                                                    string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + stdRollno + "' and month_year=" + monthYear.Trim() + "");
                                                    if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                                    {
                                                        if (dicQUpdate.ContainsKey(longMonthYear))
                                                        {
                                                            updateValue = dicQUpdate[longMonthYear];
                                                        }
                                                        if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                        {
                                                            string insquery = "update attendance set " + updateValue + " where roll_no='" + stdRollno + "' and month_year=" + monthYear + "";
                                                            int a = d2.update_method_wo_parameter(insquery, "Text");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dicQInsertColumn.ContainsKey(longMonthYear))
                                                        {
                                                            insertColumn = dicQInsertColumn[longMonthYear];
                                                        }
                                                        if (dicQInsertValues.ContainsKey(longMonthYear))
                                                        {
                                                            insertValue = dicQInsertValues[longMonthYear];
                                                        }
                                                        if (!string.IsNullOrEmpty(insertColumn.Trim()) && !string.IsNullOrEmpty(insertValue.Trim()))
                                                        {
                                                            string insquery = "insert into attendance(roll_no,month_year," + insertColumn + ") values('" + stdRollno + "'," + monthYear + "," + insertValue + ")";
                                                            int a = d2.update_method_wo_parameter(insquery, "Text");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Saved Successfully')", true);
                                    }
                                }
                                else if (btnsave.Text == "Update")
                                {
                                    string strinsselectquery = "";
                                    if (Identity.Trim() != "")
                                    {
                                         strinsselectquery = "select convert(nvarchar(15),suspendFromDate,103) suspendFromDate,convert(nvarchar(15),suspendToDate,103) suspendToDate from stucon where roll_no = '" + txtstdrollno.Text + "' and StuConID='" + Identity + "'";
                                       

                                        strinsupdaequery = "update stucon set curr_date = '" + curr_date + "',infr_type = '" + ddlfraction.SelectedItem + "',ack_diss = '" + dis + "',ack_susp = '" + sus + "',ack_fine = '" + fin + "',ack_warn = '" + war + "',prof_code = '" + lblerrstaffcode.Text + "',ack_date = '" + ack_date + "',tot_days = '" + txtdays.Text + "',fine_amo = '" + txtfine.Text + "',ack_fee_of_roll='" + feeofroll + "',Remark='" + txtremarks.Text.ToString() + "',ack_remarks='" + remarkval.ToString() + "' " + qryFeeOnRollUpdate + qrySuspendUpdate + " where roll_no = '" + txtstdrollno.Text + "' and StuConID='" + Identity + "'";
                                    }
                                    else
                                    {
                                         strinsselectquery = "select convert(nvarchar(15),suspendFromDate,103) suspendFromDate,convert(nvarchar(15),suspendToDate,103) suspendToDate from stucon where roll_no = '" + txtstdrollno.Text + "'";
                                        strinsupdaequery = "update stucon set curr_date = '" + curr_date + "',infr_type = '" + ddlfraction.SelectedItem + "',ack_diss = '" + dis + "',ack_susp = '" + sus + "',ack_fine = '" + fin + "',ack_warn = '" + war + "',prof_code = '" + lblerrstaffcode.Text + "',ack_date = '" + ack_date + "',tot_days = '" + txtdays.Text + "',fine_amo = '" + txtfine.Text + "',ack_fee_of_roll='" + feeofroll + "',Remark='" + txtremarks.Text.ToString() + "',ack_remarks='" + remarkval.ToString() + "' " + qryFeeOnRollUpdate + qrySuspendUpdate + " where roll_no = '" + txtstdrollno.Text + "'";
                                    }
                                    DataSet selectqueryy = d2.select_method_wo_parameter(strinsselectquery, "text");
                                    insupdatequery = d2.update_method_wo_parameter(strinsupdaequery, "Text");
                                    lblnorec.Visible = false;
                                    errmsg.Visible = false;
                                    if (sus == 1)
                                    {
                                        int monthyear = splityear * 12 + splitmonth;
                                        string fromdate = Convert.ToString(selectqueryy.Tables[0].Rows[0]["suspendFromDate"]).Trim();
                                        string todayte = Convert.ToString(selectqueryy.Tables[0].Rows[0]["suspendToDate"]).Trim();
                                        string noofhrs = d2.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + "");
                                        DateTime upSuspendFromDate = new DateTime();
                                        DateTime upSuspendToDate = new DateTime();
                                        string[] spl = fromdate.Split('/');
                                        string upSuspendFromDates = spl[0] + '-' + spl[1] + '-' + spl[2];
                                        string[] spl2 = todayte.Split('/');
                                        string upSuspendtoDates = spl2[0] + '-' + spl2[1] + '-' + spl2[2];
                                        bool isFromSucces = DateTime.TryParseExact(upSuspendFromDates, "dd-MM-yyyy", null, DateTimeStyles.None, out upSuspendFromDate);
                                        bool isToSucces = DateTime.TryParseExact(upSuspendtoDates, "dd-MM-yyyy", null, DateTimeStyles.None, out upSuspendToDate);


                                        if (noofhrs.Trim() != "" && noofhrs != "0" && noofhrs != null)
                                        {
                                            //DateTime datesus = Convert.ToDateTime(curr_date);
                                            //int day = Convert.ToInt32(txtdays.Text);
                                            //string datecolumn = string.Empty;
                                            //string attvalue = string.Empty;
                                            //string dateattvalue = string.Empty;
                                            //for (int date = 0; date < day; date++)
                                            //{
                                            //    datesus = datesus.AddDays(date);
                                            //    string dateva = datesus.Day.ToString();
                                            //    string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + datesus.ToString() + "'";
                                            //    DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                            //    if (dsholiday.Tables[0].Rows.Count == 0)
                                            //    {
                                            //        for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                            //        {
                                            //            if (datecolumn == "")
                                            //            {
                                            //                datecolumn = "d" + dateva + "d" + i + "";
                                            //                attvalue = "9";
                                            //                dateattvalue = "d" + dateva + "d" + i + "=9";
                                            //            }
                                            //            else
                                            //            {
                                            //                datecolumn = "" + datecolumn + "," + "d" + dateva + "d" + i + "";
                                            //                attvalue = attvalue + ',' + "9";
                                            //                dateattvalue = dateattvalue + ',' + "d" + dateva + "d" + i + "=9";
                                            //            }
                                            //        }
                                            //    }
                                            //}
                                            //string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + stdRollno + "' and month_year=" + monthyear + "");
                                            //if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                            //{
                                            //    string insquery = "update attendance set " + dateattvalue + " where roll_no='" + stdRollno + "' and month_year=" + monthyear + "";
                                            //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                            //}
                                            //else
                                            //{
                                            //    string insquery = "insert into attendance(roll_no,month_year," + datecolumn + ") values('" + stdRollno + "'," + monthyear + "," + attvalue + ")";
                                            //    int a = d2.update_method_wo_parameter(insquery, "Text");
                                            //}
                                            DateTime dtDummyStart = new DateTime();
                                            DateTime dtDummyEnd = new DateTime();
                                            int totalDaySuspended = 0;
                                            int.TryParse(txtdays.Text.Trim(), out totalDaySuspended);

                                            DateTime dtDummyStartd = new DateTime();
                                            DateTime dtDummyStartto = new DateTime();
                                            ArrayList arrMonthYear = new ArrayList();
                                            Dictionary<long, string> dicQInsertColumn = new Dictionary<long, string>();
                                            Dictionary<long, string> dicQInsertValues = new Dictionary<long, string>();
                                            Dictionary<long, string> dicQUpdate = new Dictionary<long, string>();

                                            string insertColumn = string.Empty;
                                            string insertValue = string.Empty;
                                            string updateValue = string.Empty;
                                            if (upSuspendFromDate <= dtSuspendFromDate)
                                            {
                                                dtDummyStartd = upSuspendFromDate;
                                            }
                                            else
                                            {
                                                dtDummyStartd = dtSuspendFromDate;
                                            }
                                            if (upSuspendToDate >= dtSuspendToDate)
                                            {
                                                dtDummyStartto = upSuspendToDate;
                                            }
                                            else
                                            {
                                                dtDummyStartto = dtSuspendToDate;
                                            }
                                            //for (DateTime dtTemp = dtSuspendFromDate; dtTemp <= dtSuspendToDate; dtTemp = dtTemp.AddDays(1))
                                            for (DateTime dtTemp = dtDummyStartd; dtTemp <= dtDummyStartto; dtTemp = dtTemp.AddDays(1))
                                            {
                                                string queryholi = "select * from holidayStudents where degree_code=" + ddlbrachadd.SelectedValue.ToString() + " and semester=" + ddlsemadd.SelectedValue.ToString() + " and holiday_date='" + dtTemp.ToString() + "'";
                                                DataSet dsholiday = d2.select_method(queryholi, hat, "Text");
                                                long tempMonthYear = dtTemp.Year * 12 + dtTemp.Month;
                                                insertColumn = string.Empty;
                                                insertValue = string.Empty;
                                                updateValue = string.Empty;
                                                string day = dtTemp.Day.ToString();
                                                if (!arrMonthYear.Contains(tempMonthYear))
                                                {
                                                    arrMonthYear.Add(tempMonthYear);
                                                }
                                                if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                {
                                                    dicQInsertColumn.Add(tempMonthYear, string.Empty);
                                                }
                                                if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                {
                                                    dicQInsertValues.Add(tempMonthYear, string.Empty);
                                                }
                                                if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                {
                                                    dicQUpdate.Add(tempMonthYear, string.Empty);
                                                }
                                                if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count == 0)
                                                {
                                                    for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                                    {

                                                        if (dtTemp >= dtSuspendFromDate && dtTemp <= dtSuspendToDate)
                                                        {

                                                            if (insertColumn == "")
                                                            {
                                                                insertColumn = "d" + day + "d" + i + "";
                                                                insertValue = "9";
                                                                updateValue = "d" + day + "d" + i + "=9";
                                                            }
                                                            else
                                                            {
                                                                insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                                insertValue = insertValue + ',' + "9";
                                                                updateValue = updateValue + ',' + "d" + day + "d" + i + "=9";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (insertColumn == "")
                                                            {
                                                                insertColumn = "d" + day + "d" + i + "";
                                                                insertValue = "";
                                                                updateValue = "d" + day + "d" + i + "=''";
                                                            }
                                                            else
                                                            {
                                                                insertColumn = "" + insertColumn + "," + "d" + day + "d" + i + "";
                                                                insertValue = insertValue + ',' + "";
                                                                updateValue = updateValue + ',' + "d" + day + "d" + i + "=''";
                                                            }
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(insertColumn.Trim()))
                                                    {
                                                        if (!dicQInsertColumn.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQInsertColumn.Add(tempMonthYear, insertColumn);
                                                        }
                                                        else
                                                        {
                                                            string value = dicQInsertColumn[tempMonthYear];
                                                            if (!string.IsNullOrEmpty(value))
                                                            {
                                                                dicQInsertColumn[tempMonthYear] = value + "," + insertColumn;
                                                            }
                                                            else
                                                            {
                                                                dicQInsertColumn[tempMonthYear] = insertColumn;
                                                            }
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(insertValue.Trim()))
                                                    {
                                                        if (!dicQInsertValues.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQInsertValues.Add(tempMonthYear, insertValue);
                                                        }
                                                        else
                                                        {
                                                            string value = dicQInsertValues[tempMonthYear];
                                                            if (!string.IsNullOrEmpty(value))
                                                            {
                                                                dicQInsertValues[tempMonthYear] = value + "," + insertValue;
                                                            }
                                                            else
                                                            {
                                                                dicQInsertValues[tempMonthYear] = insertValue;
                                                            }
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                    {
                                                        if (!dicQUpdate.ContainsKey(tempMonthYear))
                                                        {
                                                            dicQUpdate.Add(tempMonthYear, updateValue);
                                                        }
                                                        else
                                                        {
                                                            string value = dicQUpdate[tempMonthYear];
                                                            dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                            if (!string.IsNullOrEmpty(value))
                                                            {
                                                                dicQUpdate[tempMonthYear] = value + "," + updateValue;
                                                            }
                                                            else
                                                            {
                                                                dicQUpdate[tempMonthYear] = updateValue;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            foreach (long dicEntry in arrMonthYear)
                                            {
                                                string monthYear = Convert.ToString(dicEntry).Trim();
                                                long longMonthYear = 0;
                                                long.TryParse(monthYear.Trim(), out longMonthYear);
                                                insertColumn = string.Empty;
                                                insertValue = string.Empty;
                                                updateValue = string.Empty;
                                                string strmonthyear = d2.GetFunction("Select month_year from attendance where roll_no='" + stdRollno + "' and month_year=" + monthYear.Trim() + "");
                                                if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                                                {
                                                    if (dicQUpdate.ContainsKey(longMonthYear))
                                                    {
                                                        updateValue = dicQUpdate[longMonthYear];
                                                    }
                                                    if (!string.IsNullOrEmpty(updateValue.Trim()))
                                                    {
                                                        string insquery = "update attendance set " + updateValue + " where roll_no='" + stdRollno + "' and month_year=" + monthYear + "";
                                                        int a = d2.update_method_wo_parameter(insquery, "Text");
                                                    }
                                                }
                                                else
                                                {
                                                    if (dicQInsertColumn.ContainsKey(longMonthYear))
                                                    {
                                                        insertColumn = dicQInsertColumn[longMonthYear];
                                                    }
                                                    if (dicQInsertValues.ContainsKey(longMonthYear))
                                                    {
                                                        insertValue = dicQInsertValues[longMonthYear];
                                                    }
                                                    if (!string.IsNullOrEmpty(insertColumn.Trim()) && !string.IsNullOrEmpty(insertValue.Trim()))
                                                    {
                                                        string insquery = "insert into attendance(roll_no,month_year," + insertColumn + ") values('" + stdRollno + "'," + monthYear + "," + insertValue + ")";
                                                        int a = d2.update_method_wo_parameter(insquery, "Text");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Updated Successfully')", true);
                                }
                                sprdselectrollno.Sheets[0].Cells[res, 5].Value = 0;
                            }
                        }
                    }
                    if (flag == 0)
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Students and Proceed";
                        return;
                    }
                }
                //added by annyutha*****3rd sep 2014***//
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Check Any One Action";
                    return;
                }
                //*end*****/
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select Staff";
                return;
            }
            // spreedbind();
            //sprdrollbind();
            txtdays.Text = string.Empty;
            txtfine.Text = string.Empty;
            txtstaff.Text = string.Empty;
            chkfeeofroll.Checked = false;
            chkdismissal.Checked = false;
            chkfine.Checked = false;
            chkwarning.Checked = false;
            chksuspension.Checked = false;
            lblstartdate.Visible = false;
            lblfine.Visible = false;
            lblstartdate.Visible = false;
            lbldays.Visible = false;
            txtstartdate.Visible = false;
            txtdays.Visible = false;
            txtfine.Visible = false;
            if (divLeftRoll.Visible && divRightRoll.Visible)
            {
                txtstdrollno.Text = string.Empty;
                txtstdrollno.Enabled = true;
            }
            else
            {
                txtAdmissionNo.Text = string.Empty;
                txtAdmissionNo.Enabled = true;
            }
            btnsave.Text = "Save";
            string stratdatereload = DateTime.Now.ToString("dd-MM-yyyy");
            txtstartdate.Text = stratdatereload;
            txtdate1.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtremarks.Text = string.Empty;
            clear();
            chkremark.Checked = false;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Visible = false;
            errmsg.Visible = false;
            remarkval = 0;

            if (txtAdmissionNo.Enabled == true && schoolOrCollege == 1 && divRightAdmit.Visible == true && divLeftAdmit.Visible == true)
            {
                if (string.IsNullOrEmpty(txtAdmissionNo.Text.Trim()))
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter Admission No";
                    return;
                }
            }
            else if (txtstdrollno.Enabled == true && schoolOrCollege == 0 && divRightRoll.Visible == true && divLeftRoll.Visible == true)
            {
                if (string.IsNullOrEmpty(txtstdrollno.Text.Trim()))
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter Roll No";
                    return;
                }
            }
            if (txtdate1.Text != "")
            {
                if (ddlfraction.Text != "")
                {
                    if (txtstaff.Text != "")
                    {
                        if (chkdismissal.Checked == true || chkfine.Checked == true || chksuspension.Checked == true || chkwarning.Checked == true || chkfeeofroll.Checked == true || chkfeeonroll.Checked == true || chkremark.Checked == true)
                        {
                            if (chkdismissal.Checked == true)
                            {
                                dis = 1;
                            }
                            if (chkfine.Checked == true)
                            {
                                fin = 1;
                                if (txtfine.Text != "")
                                {

                                }
                                else
                                {
                                    errmsg.Visible = true;
                                    errmsg.Text = "Please Enter fine amount";
                                    return;
                                }
                            }
                            if (chksuspension.Checked == true)
                            {
                                sus = 1;
                                if (txtstartdate.Text.ToString().Trim() == "" && txtdays.Text.ToString().Trim() == "")
                                {
                                    errmsg.Visible = true;
                                    errmsg.Text = "Please Enter The Start Date and Days";
                                    return;
                                }
                                if (!ValidateSuspended())
                                    return;
                            }
                            if (chkwarning.Checked == true)
                            {
                                war = 1;
                            }
                            if (chkremark.Checked == true)
                            {
                                remarkval = 1;
                                if (txtremarks.Text.ToString().Trim() == "")
                                {
                                    errmsg.Visible = true;
                                    errmsg.Text = "Please Enter The Remarks";
                                    return;
                                }
                            }
                        }
                        else
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "Please Select Any One Action And Then Proceed";
                            return;
                        }
                    }
                    else
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Staff";
                        return;
                    }
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please select Infraction";
                    return;
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter Date";
                return;
            }
            savedetails();
            btngo_Click(sender, e);
        }
        catch (Exception ex) {
            string collegeCode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegeCode1, "ChallanReceipt"); 
        }
    }

    protected void exitpop_Click(object sender, EventArgs e)
    {
        panel3.Visible = false;
        panelrollnopop.Visible = true;
    }

    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = string.Empty;
            string activecol = string.Empty;
            activerow = fsstaff.ActiveSheetView.ActiveRow.ToString();
            activecol = fsstaff.ActiveSheetView.ActiveColumn.ToString();
            string purpose = fsstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            string retroll = fsstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            //Sankar Modify May'27.......................
            txtstaff.Text = retroll;
            lblerrstaffcode.Text = purpose;
            panel3.Visible = false;
            // panelrollnopop.Visible = true;
        }
        catch (Exception ex)
        {
            string collegeCode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegeCode1, "ChallanReceipt"); 
        }
    }

    // staff popup
    protected void btnstaff_Click(object sender, EventArgs e)
    {
        //sankar add may'20
        panel3.Visible = true;
        // panelrollnopop.Visible = false;
        fsstaff.Visible = true;
        fsstaff.Sheets[0].RowCount = 0;
        BindCollege();
        loadstaffdep();
        loadinfarction();
        loadfsstaff();
        loadallstaff();
    }

    // ----Load Staff pop College---
    public void BindCollege()
    {
        string strcollquery = "select collname,college_code from collinfo";
        DataSet ds = d2.select_method_wo_parameter(strcollquery, "Text");
        ddlcollege.DataSource = ds;
        ddlcollege.DataTextField = "collname";
        ddlcollege.DataValueField = "college_code";
        ddlcollege.DataBind();
        //ddlcollege.SelectedIndex = ddlcollege.Items.Count - 1;
    }

    // ----Load staff department----
    public void loadstaffdep()
    {
        try
        {
            string strquery = "select distinct dept_name,dept_code from hrdept_master where college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
            ddldepratstaff.DataSource = ds;
            ddldepratstaff.DataTextField = "dept_name";
            ddldepratstaff.DataValueField = "dept_code";
            ddldepratstaff.DataBind();
            ddldepratstaff.Items.Insert(0, "All");
        }
        catch
        {

        }
    }

    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        loadfsstaff();
        // loadstaffdep();
    }

    //sankar add may'20
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadstaffdep();
        loadallstaff();
        //loadstaffdep();
    }

    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        fsstaff.Sheets[0].PageSize = fsstaff.Sheets[0].RowCount;
        fsstaff.SaveChanges();
        loadfsstaff();
    }

    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        fsstaff.Sheets[0].PageSize = fsstaff.Sheets[0].RowCount;
        fsstaff.SaveChanges();
        loadfsstaff();
    }

    protected void loadinfarction()
    {
        string strstaff = "select name,coll_code from infraction ";
        DataSet ds = d2.select_method_wo_parameter(strstaff, "Text");
        //ddlfraction.DataSource = ds;
        //ddlfraction.DataTextField = "name";
        //ddlfraction.DataValueField = "name";
        //ddlfraction.DataBind();
        //ddlfraction.SelectedIndex = ddlfraction.Items.Count - 1;
        //con1.Close();
        ddlfraction.Items.Clear();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlfraction.DataSource = ds.Tables[0];
            ddlfraction.DataTextField = "name";
            ddlfraction.DataValueField = "name";
            ddlfraction.DataBind();
        }
        //ddlfraction.Items.Insert(0,"");
    }

    protected void loadallstaff()
    {

        sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and college_code='" + ddlcollege.SelectedValue.ToString() + "'";
        fsstaff.Sheets[0].RowCount = 0;
        //fsstaff.Sheets[0].PageSize = fsstaff.Sheets[0].RowCount;
        fsstaff.SaveChanges();

        FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
        sprdselectrollno.Sheets[0].Columns[4].CellType = chkcell;

        //sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].CellType = chkcell1;
        //sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
        //sprdselectrollno.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

        fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
        fsstaff.Sheets[0].SpanModel.Add(fsstaff.Sheets[0].RowCount - 1, 0, 1, 3);
        fsstaff.Sheets[0].AutoPostBack = false;
        string bindspread = sql;
        DataSet dsbindspread = d2.select_method_wo_parameter(bindspread, "Text");
        fsstaff.Sheets[0].RowCount = 0;
        if (dsbindspread.Tables.Count > 0 && dsbindspread.Tables[0].Rows.Count > 0)
        {
            int sno = 0;
            fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
            fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Name";
            fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Code";
            fsstaff.Sheets[0].ColumnHeader.Columns[3].Label = "Select";

            fsstaff.Sheets[0].Columns[0].Width = 50;
            fsstaff.Sheets[0].Columns[1].Width = 210;
            fsstaff.Sheets[0].Columns[2].Width = 350;
            fsstaff.Sheets[0].Columns[3].Width = 64;

            fsstaff.Sheets[0].ColumnCount = 4;
            fsstaff.Width = 690;
            for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
            {
                sno++;
                string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();

                fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
                fsstaff.Sheets[0].Rows[fsstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Text = name;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Text = code;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].CellType = chkcell1;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                fsstaff.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                chkcell1.AutoPostBack = true;

            }
            int rowcount = fsstaff.Sheets[0].RowCount;
            fsstaff.Height = 370;
            fsstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
            fsstaff.SaveChanges();
        }
    }

    protected void loadfsstaff()
    {
        if (ddldepratstaff.SelectedIndex != 0)
        {
            if (txt_search.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0)and  (staffmaster.settled = 0)  and (staff_name like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                }
                else if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                }
            }
            else
            {
                //sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_name = '" + ddldepratstaff.Text + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "' and (staffmaster.college_code =hrdept_master.college_code)";
                sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";

            }
        }
        else if (txt_search.Text != "")
        {
            if (ddlstaff.SelectedIndex == 0)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code)";
            }
            else if (ddlstaff.SelectedIndex == 1)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code)";
            }
            else if (ddlcollege.SelectedIndex != -1)
            {
                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            }

            else
            {
                sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster,hrdept_master.dept_name where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0";

            }
        }
        else
            if (ddldepratstaff.SelectedValue.ToString().Trim().ToLower() == "all")
            {
                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";

            }
        fsstaff.Sheets[0].RowCount = 0;
        fsstaff.SaveChanges();

        FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
        sprdselectrollno.Sheets[0].Columns[4].CellType = chkcell;

        //sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].CellType = chkcell1;
        //sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
        //sprdselectrollno.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

        fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
        fsstaff.Sheets[0].SpanModel.Add(fsstaff.Sheets[0].RowCount - 1, 0, 1, 3);
        fsstaff.Sheets[0].AutoPostBack = false;
        string bindspread = sql;
        DataSet dsbindspread = d2.select_method_wo_parameter(bindspread, "Text");
        fsstaff.Sheets[0].RowCount = 0;
        if (dsbindspread.Tables.Count > 0 && dsbindspread.Tables[0].Rows.Count > 0)
        {
            int sno = 0;
            fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
            fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Name";
            fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Code";
            fsstaff.Sheets[0].ColumnHeader.Columns[3].Label = "Select";

            fsstaff.Sheets[0].Columns[0].Width = 50;
            fsstaff.Sheets[0].Columns[1].Width = 210;
            fsstaff.Sheets[0].Columns[2].Width = 350;
            fsstaff.Sheets[0].Columns[3].Width = 64;

            fsstaff.Sheets[0].ColumnCount = 4;
            fsstaff.Width = 690;
            for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
            {
                sno++;
                string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();

                fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
                fsstaff.Sheets[0].Rows[fsstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Text = name;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Text = code;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].CellType = chkcell1;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                fsstaff.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                chkcell1.AutoPostBack = true;

            }
            int rowcount = fsstaff.Sheets[0].RowCount;
            fsstaff.Height = 370;
            fsstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
            fsstaff.SaveChanges();

        }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        panelrollnopop.Visible = false;
        //FpSpread1.Visible = true;
        //btnexcel.Visible = true;
        //btnprint.Visible =true;
        btnsave.Text = "Save";
    }

    protected void addnew_Click(object sender, EventArgs e)
    {

    }

    protected void exitnew_Click(object sender, EventArgs e)
    {

    }

    protected void btnaddfraction_Click(object sender, EventArgs e)
    {
        panel4.Visible = true;
    }

    protected void btnfractiobremove_Click(object sender, EventArgs e)
    {
        int del = d2.update_method_wo_parameter("delete from infraction where name='" + ddlfraction.Text + "'", "Text");
        loadinfarction();
    }

    protected void ddlfraction_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            clear();
            chkfeeonroll.Visible = false;
            chkfeeonroll.Checked = false;
            string activerow = string.Empty;
            string activecol = string.Empty;
            btnsave.Text = "Update";
            activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            FpSpread1.SaveChanges();
            string Date = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            string Remark = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
            string Stud_Name = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            string Staff_name = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text;
            string rollNo = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag).Trim();
            string appNo = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note).Trim();
            string collegeCodeNew = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag).Trim();
            string currentSemester = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Note).Trim();
            string Identity = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag).Trim();
            //string fraction = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            string[] stud_roll = Stud_Name.Split(new char[] { '-' });
            //string roll_no_Stud = d2.GetFunction("select distinct roll_no from registration where cc=0 and delflag=0 and exam_flag!='debar' and batch_year='" + ddlbatch.SelectedValue + "' and degree_code='" + ddlbranch.SelectedValue + "'and current_semester='" + ddlsemester.SelectedValue + "' and Stud_Name = '" + stud_roll[1] + "'");


            //string batch = "", dept = "", course = "", sems = "", sec = "", regno =string.Empty;
            //string strquery = "select reg_no,Batch_Year,de.Dept_Name,c.Course_Name,Current_Semester,Sections from Registration r,Degree d,Department de,course c where roll_no='" + stud_roll[0].ToString() + "' and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and r.college_code=d.college_code";

            string batch = string.Empty, dept = string.Empty, course = string.Empty, sems = string.Empty, sec = string.Empty, regno = string.Empty;
            string collegeCode = string.Empty;
            string strquery = "select reg_no,Batch_Year,r.Roll_Admit,de.Dept_Name,r.degree_code,c.Course_Name,c.Course_Id,Current_Semester,Sections,r.college_code from Registration r,Degree d,Department de,course c where roll_no='" + rollNo + "' and r.app_no='" + appNo + "' and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and r.college_code=d.college_code";
            DataSet dsdetal = d2.select_method_wo_parameter(strquery, "Text");
            if (dsdetal.Tables.Count > 0 && dsdetal.Tables[0].Rows.Count > 0)
            {
                if (sprdselectrollno.Sheets[0].ColumnCount < 5)
                {
                    btnadd_Click(sender, e);
                }

                batch = Convert.ToString(dsdetal.Tables[0].Rows[0]["Batch_Year"]).Trim();
                course = Convert.ToString(dsdetal.Tables[0].Rows[0]["Course_Name"]).Trim();
                string courseID = Convert.ToString(dsdetal.Tables[0].Rows[0]["Course_Id"]).Trim();
                dept = Convert.ToString(dsdetal.Tables[0].Rows[0]["Dept_Name"]).Trim();
                string degreeCode = Convert.ToString(dsdetal.Tables[0].Rows[0]["degree_code"]).Trim();
                sems = Convert.ToString(dsdetal.Tables[0].Rows[0]["Current_Semester"]).Trim();
                sec = Convert.ToString(dsdetal.Tables[0].Rows[0]["Sections"]).Trim();
                regno = Convert.ToString(dsdetal.Tables[0].Rows[0]["reg_no"]).Trim();
                collegeCode = Convert.ToString(dsdetal.Tables[0].Rows[0]["college_code"]).Trim();
                string rollAdmit = Convert.ToString(dsdetal.Tables[0].Rows[0]["Roll_Admit"]).Trim();
                if (batch != null && batch.Trim() != "")
                {
                    ddlbatchadd.Items.Clear();
                    ddlbatchadd.Items.Insert(0, batch);
                    ddlbatchadd.Enabled = false;
                }
                if (course != null && courseID != null && course.Trim() != "" && courseID.Trim() != "")
                {
                    ddldegreeadd.Items.Clear();
                    ddldegreeadd.Items.Insert(0, new ListItem(course.Trim(), courseID));
                    ddldegreeadd.Enabled = false;
                }
                if (dept != null && degreeCode != null && degreeCode.Trim() != "" && dept != "")
                {
                    ddlbrachadd.Items.Clear();
                    ddlbrachadd.Items.Insert(0, new ListItem(dept, degreeCode));
                    ddlbrachadd.Enabled = false;
                }
                if (sems != null && sems.Trim() != "")
                {
                    ddlsemadd.Items.Clear();
                    ddlsemadd.Items.Insert(0, new ListItem(sems, sems));
                    ddlsemadd.Enabled = false;
                }
                if (sec != null && sec.Trim() != "" && sec.Trim() != "-1")
                {
                    ddlsecadd.Items.Clear();
                    ddlsecadd.Items.Insert(0, new ListItem(sec.Trim(), sec.Trim()));
                    ddlsecadd.Enabled = false;
                }
            }
            string selectquery = "select roll_no,convert(varchar(100),curr_date,105) as curr_date,infr_type,ack_diss,ack_susp,ack_fine,ack_warn,prof_code,convert(varchar(100),cast(ack_date as DateTime),105) as ack_date,tot_days,fine_amo,serial_no,semester,ack_fee_of_roll,Remark,ack_remarks,convert(varchar(100),cast(ack_date as DateTime),105) as feeOffRollDate,convert(varchar(100),feeOnRollDate,105) as feeOnRollDate,convert(varchar(50),suspendFromDate,105) as suspendFromDate,convert(varchar(50),suspendToDate,105) as suspendToDate,StuConID from stucon where roll_no='" + rollNo + "' and StuConID='" + Identity + "'";
            if (selectquery != "")
            {
                DataSet dsselectquery = d2.select_method_wo_parameter(selectquery, "Text");
                if (dsselectquery.Tables.Count > 0 && dsselectquery.Tables[0].Rows.Count > 0)
                {
                    btndelete.Enabled = true;
                    string rollAdmit = string.Empty;
                    for (int i1 = 0; i1 < dsselectquery.Tables[0].Rows.Count; i1++)
                    {
                        string ack_diss = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["ack_diss"]).Trim();
                        string ack_susp = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["ack_susp"]).Trim();
                        string ack_fine = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["ack_fine"]).Trim();
                        string ack_warn = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["ack_warn"]).Trim();
                        string prof_code = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["prof_code"]).Trim();
                        string ack_date = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["ack_date"]).Trim();
                        string curr_date = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["curr_date"]).Trim();
                        string roll_no = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["roll_no"]).Trim();
                        string infr_type = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["infr_type"]).Trim();
                        string tot_days = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["tot_days"]).Trim();
                        string fine_amount = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["fine_amo"]).Trim();
                        string semester = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["semester"]).Trim();
                        string feeoftheroll = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["ack_fee_of_roll"]).Trim();
                        string remarks = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["Remark"]).Trim();
                        string remaaction = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["ack_remarks"]).Trim();
                        string feeOnRollDate = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["feeOnRollDate"]).Trim();
                        string suspendStartDate = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["suspendFromDate"]).Trim();
                        string suspendEndDate = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["suspendToDate"]).Trim();
                        //string Iden = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["StuConID"]).Trim();
                        //chkdismissal.Checked = true;
                        DateTime dtAckDate = new DateTime();
                        if (!DateTime.TryParseExact(ack_date, "dd-MM-yyyy", null, DateTimeStyles.None, out dtAckDate))
                        {
                            dtAckDate = DateTime.Now;
                        }
                        txtdate1.Text = dtAckDate.ToString("dd-MM-yyyy");

                        DateTime dtFeeOnRollDate = new DateTime();
                        if (!DateTime.TryParseExact(feeOnRollDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFeeOnRollDate))
                        {
                            dtFeeOnRollDate = DateTime.Now;
                        }
                        txtFeeOnRollDate.Text = dtFeeOnRollDate.ToString("dd-MM-yyyy");

                        DateTime dtSuspendStartDate = new DateTime();
                        if (!DateTime.TryParseExact(suspendStartDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendStartDate))
                        {
                            dtSuspendStartDate = DateTime.Now;
                        }
                        txtstartdate.Text = dtSuspendStartDate.ToString("dd-MM-yyyy");

                        DateTime dtSuspendEndDate = new DateTime();
                        if (!DateTime.TryParseExact(suspendEndDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendEndDate))
                        {
                            dtSuspendEndDate = DateTime.Now;
                        }
                        txtEndDate.Text = dtSuspendEndDate.ToString("dd-MM-yyyy");
                        txtremarks.Text = remarks;
                        if (roll_no.Trim() != "")
                        {
                            string[] roll = Stud_Name.Split(new char[] { '-' });
                            rollAdmit = d2.GetFunctionv("select Roll_Admit from Registration where Roll_No='" + Convert.ToString(rollNo).Trim() + "' and app_no='" + appNo + "' and college_code='" + Convert.ToString(collegeCode).Trim() + "'");
                            if (schoolOrCollege == 0)
                            {
                                txtstdrollno.Text = rollNo;
                                txtstdrollno.Enabled = false;
                            }
                            else if (schoolOrCollege == 1)
                            {
                                txtAdmissionNo.Text = rollAdmit;
                                txtAdmissionNo.Enabled = false;
                            }
                            lblindex.Text = Identity;
                        }

                        if (infr_type.Trim() != "")
                        {
                            ddlfraction.SelectedItem.Text = Convert.ToString(infr_type).Trim();
                        }
                        if (Staff_name.Trim() != "")
                        {
                            txtstaff.Text = Staff_name;
                        }
                        if (prof_code.Trim() != "")
                        {
                            lblerrstaffcode.Text = prof_code;
                        }
                        if (ack_fine.Trim() == "1")
                        {
                            lblfine.Visible = true;
                            txtfine.Visible = true;
                            txtfine.Text = fine_amount;
                            chkfine.Checked = true;
                        }
                        else
                        {
                            lblfine.Visible = false;
                            txtfine.Visible = false;
                        }
                        if (ack_diss.Trim() == "1")
                        {
                            chkdismissal.Checked = true;
                        }
                        else
                        {
                            chkdismissal.Checked = false;
                        }
                        if (ack_susp.Trim() == "1")
                        {
                            lblstartdate.Visible = true;
                            lblEndDate.Visible = true;
                            txtEndDate.Visible = true;
                            txtstartdate.Visible = true;
                            int totalDays = 0;
                            int.TryParse(tot_days, out totalDays);
                            txtdays.Visible = true;
                            lbldays.Visible = true;
                            txtdays.Text = tot_days;
                            if (!DateTime.TryParseExact(suspendStartDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendStartDate))
                            {
                                txtstartdate.Text = dtAckDate.ToString("dd-MM-yyyy");
                            }
                            else
                            {
                                txtstartdate.Text = dtSuspendStartDate.ToString("dd-MM-yyyy");
                            }
                            if (!DateTime.TryParseExact(suspendEndDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendEndDate))
                            {
                                txtEndDate.Text = dtAckDate.AddDays(totalDays).ToString("dd-MM-yyyy");
                            }
                            else
                            {
                                txtEndDate.Text = dtSuspendEndDate.ToString("dd-MM-yyyy");
                            }
                            chksuspension.Checked = true;
                        }
                        else
                        {
                            txtstartdate.Visible = false;
                            txtdays.Visible = false;
                        }
                        if (ack_warn == "1")
                        {
                            chkwarning.Checked = true;
                        }
                        else
                        {
                            chkwarning.Checked = false;
                        }
                        if (feeoftheroll == "1")
                        {
                            chkfeeofroll.Checked = true;
                            chkfeeonroll.Visible = true;
                            chkfeeonroll.Checked = false;
                            divFeeOnRollDate.Visible = false;
                        }
                        else
                        {
                            divFeeOnRollDate.Visible = false;
                            chkfeeofroll.Checked = false;
                            chkfeeonroll.Visible = false;
                            if (DateTime.TryParseExact(feeOnRollDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFeeOnRollDate))
                            {
                                chkfeeonroll.Visible = true;
                                chkfeeonroll.Checked = true;
                                txtFeeOnRollDate.Text = dtFeeOnRollDate.ToString("dd-MM-yyyy");
                                txtFeeOnRollDate.Visible = true;
                                divFeeOnRollDate.Visible = true;
                            }
                        }

                        if (remaaction.Trim() == "1")
                        {
                            chkremark.Checked = true;
                        }
                    }
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();//rajasekar

                    sprdselectrollno.Sheets[0].RowCount = 2;
                    sprdselectrollno.Sheets[0].Cells[1, 0].Text = "1";//collegeCode sems
                    sprdselectrollno.Sheets[0].Cells[1, 0].Tag = sems;
                    sprdselectrollno.Sheets[0].Cells[1, 1].Text = rollNo;
                    sprdselectrollno.Sheets[0].Cells[1, 1].Note = appNo;
                    sprdselectrollno.Sheets[0].Cells[1, 1].Tag = collegeCode;
                    sprdselectrollno.Sheets[0].Cells[1, 1].CellType = txt;
                    sprdselectrollno.Sheets[0].Cells[1, 2].Text = regno.ToString();
                    sprdselectrollno.Sheets[0].Cells[1, 2].CellType = txt;
                    sprdselectrollno.Sheets[0].Cells[1, 3].Text = rollAdmit;
                    sprdselectrollno.Sheets[0].Cells[1, 3].Tag = Convert.ToString(Identity);
                    sprdselectrollno.Sheets[0].Cells[1, 3].CellType = txt;
                    sprdselectrollno.Sheets[0].Cells[1, 4].Text = stud_roll[2].ToString();
                    sprdselectrollno.Sheets[0].Cells[1, 4].CellType = txt;
                    sprdselectrollno.Sheets[0].Cells[1, 5].Value = 1;
                    sprdselectrollno.Sheets[0].Cells[1, 5].Locked = true;
                    panelrollnopop.Visible = true;
                    sprdselectrollno.Visible = true;
                    errmsg.Visible = false;
                    errmsg.Text = string.Empty;
                    norecordlbl.Visible = false;
                }
                else
                {
                    panelrollnopop.Visible = false;
                }
                //sprdselectrollno.Sheets[0].RowCount = 0;
                //BindBatchadd();
                //BindDegreepop(singleuser, group_user, collegecode, usercode);
                //BindBranchpop(singleuser, group_user, course_id, collegecode, usercode);
                //BindSectionDetailpop(strbatch, strbranch);
                //BindSempop(strbranch, strbatchyear, collegecode);
                //loadinfarction();
                //sprdrollbind();
                //txtdate1.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
        }
    }

    //public string GetFunction(string Att_strqueryst)
    //{

    //    string sqlstr;
    //    sqlstr = Att_strqueryst;
    //    con2.Close();
    //    con2.Open();
    //    SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con2);
    //    SqlDataReader drnew;
    //    SqlCommand cmd = new SqlCommand(sqlstr);
    //    cmd.Connection = con2;
    //    drnew = cmd.ExecuteReader();
    //    drnew.Read();

    //    if (drnew.HasRows == true)
    //    {
    //        return drnew[0].ToString();
    //    }
    //    else
    //    {
    //        return "";
    //    }
    //}

    protected void FpSpread1_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        FpSpread1.Sheets[0].AutoPostBack = true;
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        Cellclick = true;
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        mpemsgboxdelete.Show();
    }

    protected void btnfractionnew_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtfractionnew.Text != null)
            {
                if (txtfractionnew.Text.Length <= 500)
                {
                    {
                        int insinfr = d2.update_method_wo_parameter("Insert into infraction (name) values('" + txtfractionnew.Text + "')", "Text");
                        loadinfarction();
                        panel4.Visible = false;
                        txtfractionnew.Text = string.Empty;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter 500 Character only')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Infraction ')", true);
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void btnfractiondelete_Click(object sender, EventArgs e)
    {
        int insinf = d2.update_method_wo_parameter("delete from infraction where name='" + ddlfraction.Text + "'", "Text");
        loadinfarction();
        panel4.Visible = false;
    }

    protected void FpSpread1_UpdateCommand1(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        string appPath = HttpContext.Current.Server.MapPath("~");
        string print = string.Empty;
        if (appPath != "")
        {
            int i = 1;
            appPath = appPath.Replace("\\", "/");
        e:
            try
            {
                print = "Student Conduct" + i;
                // FpSpread1.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                //Aruna on 26feb2013============================
                string szPath = appPath + "/Report/";
                string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                FpSpread1.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Flush();
                Response.WriteFile(szPath + szFile);
                //=============================================

            }
            catch
            {
                i++;
                goto e;

            }
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
        }
    }

    protected void btnfractionExit_Click(object sender, EventArgs e)
    {
        panel4.Visible = false;
    }

    protected void btninfractionexit_Click(object sender, EventArgs e)
    {
        panel4.Visible = false;
    }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        if (txtfromdate.Text != "" && txttodate.Text != "")
        {
            string fromdate = string.Empty;
            fromdate = txtfromdate.Text.ToString();
            string[] splitfrom = fromdate.Split(new Char[] { '-' });
            int splitdatefrom = Convert.ToInt32(splitfrom[1]);
            int splitmonthfrom = Convert.ToInt32(splitfrom[0]);
            int splityearfrom = Convert.ToInt32(splitfrom[2]);

            string todate = string.Empty;
            todate = txttodate.Text.ToString();
            string[] splitto = todate.Split(new Char[] { '-' });

            int splitdate = Convert.ToInt32(splitto[1]);
            int splitmonth = Convert.ToInt32(splitto[0]);
            int splityear = Convert.ToInt32(splitto[2]);

            if (splityear > splityearfrom)
            {
                errmsg.Visible = false;
            }
            else if (splityear == splityearfrom)
            {
                if (splitmonth > splitmonthfrom)
                {
                    errmsg.Visible = false;
                }
                else if (splitmonth == splitmonthfrom)
                {
                    if (splitdate >= splitdatefrom)
                    {
                        errmsg.Visible = false;
                    }
                    else
                    {

                        //txttodate.Text =string.Empty;
                    }
                }
                else
                {

                    //txttodate.Text =string.Empty;
                }
            }
            else
            {

                //txttodate.Text =string.Empty;
            }
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        mpemsgboxdelete.Hide();
        try
        {
            if (divLeftRoll.Visible && divRightRoll.Visible && txtstdrollno.Text.Trim() != "" && schoolOrCollege == 0)
            {
                lblerrstaffcode.Visible = false;
                string sqlcmd = string.Empty;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                sqlcmd = "delete from stucon where roll_no ='" + txtstdrollno.Text.Trim() + "'";
                int n = dset.update_method_wo_parameter(sqlcmd, "text");
                //LoadMainEnquiry_date();
                spreedbind();
                btndelete.Enabled = false;
                clear();
            }
            else if (divLeftAdmit.Visible && divRightAdmit.Visible && txtAdmissionNo.Text.Trim() != "" && schoolOrCollege == 1)
            {
                lblerrstaffcode.Visible = false;
                string sqlcmd = string.Empty;
                string rollNo = d2.GetFunctionv("select Roll_No from Registration where Roll_Admit='" + Convert.ToString(txtAdmissionNo.Text).Trim() + "'");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                sqlcmd = "delete from stucon where roll_no ='" + rollNo.Trim() + "'";
                int n = dset.update_method_wo_parameter(sqlcmd, "text");
                //LoadMainEnquiry_date();
                spreedbind();
                btndelete.Enabled = false;
                clear();
            }
            else
            {
                lblerrstaffcode.Text = "Select the Student Details";
                lblerrstaffcode.Visible = true;
            }
        }
        catch
        {

        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mpemsgboxdelete.Hide();
    }

    public void clear()
    {
        lblnorec.Text = string.Empty;
        lblnorec.Visible = false;

        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;

        lblerrstaffcode.Text = string.Empty;
        lblerrstaffcode.Visible = false;

        txtremarks.Text = string.Empty;

        errmsg.Text = string.Empty;
        errmsg.Visible = false;

        txtdays.Text = string.Empty;
        txtfine.Text = string.Empty;
        txtstaff.Text = string.Empty;
        chkfeeonroll.Checked = false;
        divFeeOnRollDate.Visible = false;
        chkfeeonroll.Visible = false;
        chkdismissal.Checked = false;
        chkfeeofroll.Checked = false;
        chkfine.Checked = false;
        chkwarning.Checked = false;
        chksuspension.Checked = false;
        lblstartdate.Visible = false;
        lblfine.Visible = false;
        lblstartdate.Visible = false;
        lbldays.Visible = false;
        txtstartdate.Visible = false;

        txtEndDate.Visible = false;
        lblEndDate.Visible = false;

        txtdays.Visible = false;
        txtfine.Visible = false;
        if (divLeftRoll.Visible && divRightRoll.Visible)
        {
            txtstdrollno.Text = string.Empty;
            txtstdrollno.Enabled = true;
        }
        else
        {
            txtAdmissionNo.Text = string.Empty;
            txtAdmissionNo.Enabled = true;
        }
        string stratdatereload = DateTime.Now.ToString("dd-MM-yyyy");
        txtstartdate.Text = stratdatereload;
        txtdate1.Text = DateTime.Now.ToString("dd-MM-yyyy");
        txtFeeOnRollDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        txtEndDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
    }

    protected void txtdays_Changed(object sender, EventArgs e)// added by gowtham july'22
    {
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
        try
        {
            //string sday = txtdays.Text.ToString();
            //if (sday.Trim() != "" && sday != null)
            //{
            //    string enddate = d2.GetFunction("select end_date from seminfo where degree_code='" + ddlbrachadd.SelectedValue.ToString() + "' and batch_year='" + ddlbatchadd.SelectedValue.ToString() + "' and semester='" + ddlsemadd.SelectedItem.ToString() + "'");
            //    if (enddate.Trim() != "" && enddate.Trim() != "0" && enddate != null)
            //    {
            //        DateTime dtend = Convert.ToDateTime(enddate);
            //        string[] spcur = txtdate1.Text.ToString().Split('-');
            //        DateTime dt = Convert.ToDateTime(spcur[1] + '/' + spcur[0] + '/' + spcur[2]);
            //        int da = Convert.ToInt32(sday);
            //        if (da > 0)
            //        {
            //            da = da - 1;
            //        }
            //        dt = dt.AddDays(da);
            //        if (dtend < dt)
            //        {
            //            lblnorec.Visible = true;
            //            lblnorec.Text = "Please Enter Days Less Than Semseter End Date";
            //            txtdays.Text = string.Empty;
            //        }
            //        else
            //        {
            //            lblnorec.Visible = false;
            //        }
            //    }
            //}
            lblnorec.Text = string.Empty;
            lblnorec.Visible = false;
            lblErrMsg.Text = string.Empty;
            lblErrMsg.Visible = false;
            txtdays.Text = "0";
            bool isValidAll = false;
            string suspendedFromDate = txtstartdate.Text.Trim();
            string suspendedToDate = txtEndDate.Text.Trim();
            DateTime dtSuspendFromDate = new DateTime();
            DateTime dtSuspendToDate = new DateTime();

            DateTime dtSemStart = new DateTime();
            DateTime dtSemEnd = new DateTime();

            bool isFromSuccess = DateTime.TryParseExact(suspendedFromDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendFromDate);
            bool isToSuccess = DateTime.TryParseExact(suspendedToDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendToDate);


            string semesterEndDate = d2.GetFunction("select convert(varchar(50),end_date,105) end_date from seminfo where degree_code='" + Convert.ToString(ddlbrachadd.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlbatchadd.SelectedValue).Trim() + "' and semester='" + Convert.ToString(ddlsemadd.SelectedItem.Text).Trim() + "'");
            string semesterStartDate = d2.GetFunction("select convert(varchar(50),start_date,105) start_date from seminfo where degree_code='" + Convert.ToString(ddlbrachadd.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlbatchadd.SelectedValue).Trim() + "' and semester='" + Convert.ToString(ddlsemadd.SelectedItem.Text).Trim() + "'");

            bool isSemStart = DateTime.TryParseExact(semesterStartDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSemStart);
            bool isSemEnd = DateTime.TryParseExact(semesterEndDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSemEnd);

            if (string.IsNullOrEmpty(semesterStartDate.Trim()) || string.IsNullOrEmpty(semesterEndDate.Trim()))
            {
                lblErrMsg.Text = "Please Set " + ((string.IsNullOrEmpty(semesterStartDate.Trim()) && string.IsNullOrEmpty(semesterEndDate.Trim())) ? "Semester Start Date And Semester End Date " : ((string.IsNullOrEmpty(semesterEndDate.Trim())) ? "Semester End Date " : ((string.IsNullOrEmpty(semesterStartDate.Trim())) ? "Semester Start Date " : "")));
                lblErrMsg.Visible = true;
                return;
            }
            else if (semesterStartDate.Trim() == "0" || semesterEndDate.Trim() == "0")
            {
                lblErrMsg.Text = "Please Set " + ((semesterStartDate.Trim() == "0" && semesterEndDate.Trim() == "0") ? "Semester Start Date and Semester End Date " : ((semesterEndDate.Trim() == "0") ? "Semester End Date " : ((semesterStartDate.Trim() == "0") ? "Semester Start Date " : "")));
                lblErrMsg.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(suspendedFromDate.Trim()) || string.IsNullOrEmpty(suspendedToDate.Trim()))
            {
                lblErrMsg.Text = "Please Select " + ((string.IsNullOrEmpty(suspendedFromDate.Trim()) && string.IsNullOrEmpty(suspendedToDate.Trim())) ? "Suspend Start Date And Suspend End Date " : ((string.IsNullOrEmpty(suspendedToDate.Trim())) ? "Suspend End Date " : ((string.IsNullOrEmpty(suspendedFromDate.Trim())) ? "Suspend Start Date " : "")));
                lblErrMsg.Visible = true;
                txtstartdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtEndDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
            else
            {
                if (isFromSuccess && isToSuccess)
                {
                    if (dtSuspendFromDate < dtSemStart)
                    {
                        lblErrMsg.Text = "Please Select Suspend Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Semester Start Date " + dtSemStart.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendFromDate > dtSemEnd)
                    {
                        lblErrMsg.Text = "Please Select Suspend Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + " Must Be lesser Than or Equal To Semester End Date " + dtSemEnd.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendToDate < dtSemStart)
                    {
                        lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Semester Start Date " + dtSemStart.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendToDate > dtSemEnd)
                    {
                        lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be lesser Than or Equal To Semester End Date " + dtSemEnd.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendFromDate > dtSuspendToDate)
                    {
                        lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else
                    {
                        txtdays.Text = Convert.ToString(dtSuspendToDate.Subtract(dtSuspendFromDate).Days + 1).Trim();
                        isValidAll = true;
                    }
                }
                else
                {
                    lblErrMsg.Text = "Please Select Valid " + ((!isFromSuccess && !isToSuccess) ? " Suspend Start Date and Suspend End Date" : ((!isFromSuccess) ? " Suspend Start Date" : ((!isToSuccess) ? "Suspend End Date" : "")));
                    lblErrMsg.Visible = true;
                }
            }
            //if (!isValidAll)
            //{
            //    txtstartdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            //    txtEndDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            //}
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)// added by gowtham july'22
    {
        Session["column_header_row_count"] = 1;
        string deg_details = string.Empty;
        string degree_pdf = string.Empty;
        string header = string.Empty;
        deg_details = "Student Conduct Details";
        string sections = string.Empty;
        if (ddlsection.Text.ToString().Trim().ToLower() != "all" && ddlsection.Text.ToString().Trim().ToLower() != string.Empty && ddlsection.Text.ToString().Trim().ToLower() != "-1")
        {
            sections = "-Sec-" + ddlsection.Text.ToString() + "";
        }
        degree_pdf = "" + ddlbatch.SelectedItem.Text + " -" + ddldegree.SelectedItem.Text + " - " + ddlbranch.SelectedItem.Text + "- sem-" + ddlsemester.SelectedItem.Text + "" + sections + "";

        string degreedetails = string.Empty;
        degreedetails = deg_details + "@ Degree :" + degree_pdf;
        string pagename = "StudentConduct.aspx";

        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void chkfeeonroll_CheckedChanged(object sender, EventArgs e)
    {
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
        if (chkfeeonroll.Checked)
        {
            chkfeeofroll.Checked = false;
            divFeeOnRollDate.Visible = true;
            txtFeeOnRollDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
        else
        {
            chkfeeonroll.Checked = false;
            chkfeeofroll.Checked = true;
            divFeeOnRollDate.Visible = false;
        }
    }

    protected void chkfeeofroll_CheckedChanged(object sender, EventArgs e)
    {
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
        if (chkfeeofroll.Checked)
        {
            chkfeeonroll.Checked = true;
            divFeeOnRollDate.Visible = true;
            txtFeeOnRollDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
        else
        {
            chkfeeonroll.Checked = false;
            divFeeOnRollDate.Visible = false;
        }
    }

    protected void chkdatevisible(object sender, EventArgs e)
    {
        if (chkdate.Checked == true)
        {
            txtfromdate.Visible = true;
            txttodate.Visible = true;
            lbldate.Visible = true;
            lbltodat.Visible = true;
        }
        else
        {
            txtfromdate.Visible = false;
            txttodate.Visible = false;
            lbldate.Visible = false;
            lbltodat.Visible = false;
        }
    }

    #region Added By Malang Raja

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
        lbl.Add(lblcollege);
        lbl.Add(lblDegree1);
        lbl.Add(lbldegree);
        lbl.Add(lblBranch1);
        lbl.Add(lblbranchadd);
        lbl.Add(lblSem1);
        lbl.Add(lblSem);
        fields.Add(0);
        fields.Add(2);
        fields.Add(2);
        fields.Add(3);
        fields.Add(3);
        fields.Add(4);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type</param>
    /// <param name="dsSettingsOptional">it is Optional Parameter</param>
    /// <returns>true or false</returns>
    private bool ColumnHeaderVisiblity(int type, DataSet dsSettingsOptional = null)
    {
        bool hasValues = false;
        try
        {
            DataSet dsSettings = new DataSet();
            if (dsSettingsOptional == null)
            {
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " and  group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                }
                else if (Session["usercode"] != null)
                {
                    grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
                    dsSettings = dset.select_method(Master1, hat, "Text");
                }
            }
            else
            {
                dsSettings = dsSettingsOptional;
            }
            if (dsSettings.Tables.Count > 0 && dsSettings.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSettings in dsSettings.Tables[0].Rows)
                {
                    switch (type)
                    {
                        case 0:
                            if (Convert.ToString(drSettings["settings"]).Trim() == "Roll No")
                            {
                                hasValues = true;
                            }
                            break;
                        case 1:
                            if (Convert.ToString(drSettings["settings"]).Trim() == "Register No")
                            {
                                hasValues = true;
                            }
                            break;
                        case 2:
                            if (Convert.ToString(drSettings["settings"]).Trim() == "Admission No")
                            {
                                hasValues = true;
                            }
                            break;
                        case 3:
                            if (Convert.ToString(drSettings["settings"]).Trim() == "Student_Type")
                            {
                                hasValues = true;
                            }
                            break;
                    }
                    if (hasValues)
                        break;
                }
            }
            return hasValues;
        }
        catch
        {
            return false;
        }
    }

    protected void txtFeeOnRollDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrMsg.Text = string.Empty;
            lblErrMsg.Visible = false;
            string feeOffRollDate = txtdate1.Text.Trim();
            string feeOnRollDate = txtFeeOnRollDate.Text.Trim();
            DateTime dtFeeOffRollDate = new DateTime();
            DateTime dtFeeOnRollDate = new DateTime();
            lblErrMsg.Visible = false;
            lblErrMsg.Text = string.Empty;
            bool isSuccOffroll = DateTime.TryParseExact(feeOffRollDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFeeOffRollDate);
            bool isSuccOnroll = DateTime.TryParseExact(feeOnRollDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFeeOnRollDate);
            if (!string.IsNullOrEmpty(feeOffRollDate.Trim()))
            {
                if (isSuccOffroll)
                {
                    if (!string.IsNullOrEmpty(feeOnRollDate.Trim()))
                    {
                        if (isSuccOnroll)
                        {
                            if (dtFeeOffRollDate > dtFeeOnRollDate)
                            {
                                lblErrMsg.Text = "Please Select Fee On Roll Date " + dtFeeOnRollDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Fee off Roll Date " + dtFeeOffRollDate.ToString("dd-MM-yyyy") + " ";
                                lblErrMsg.Visible = true;
                            }
                        }
                        else
                        {
                            lblErrMsg.Text = "Please Select Valid Fee on Roll Date " + dtFeeOnRollDate.ToString("dd-MM-yyyy") + "";
                            lblErrMsg.Visible = true;
                        }
                    }
                    else
                    {
                        lblErrMsg.Text = "Please Select Fee on Roll Date " + dtFeeOnRollDate.ToString("dd-MM-yyyy") + "";
                        lblErrMsg.Visible = true;
                    }
                }
                else
                {
                    lblErrMsg.Text = "Please Select Valid Fee off Roll Date " + dtFeeOffRollDate.ToString("dd-MM-yyyy") + "";
                    lblErrMsg.Visible = true;
                }
            }
            else
            {
                lblErrMsg.Text = "Please Select Fee off Roll Date " + dtFeeOffRollDate.ToString("dd-MM-yyyy") + "";
                lblErrMsg.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void txtstartdate_TextChanged(object sender, EventArgs e)
    {
        //lblErrMsg.Visible = false;
        //lblErrMsg.Text = string.Empty;
        //string cureentdate = string.Empty;
        //cureentdate = txtdate1.Text.ToString();

        //string[] splitcur = cureentdate.Split(new Char[] { '-' });

        //int curmonth = Convert.ToInt32(splitcur[1]);
        //int curdate = Convert.ToInt32(splitcur[0]);
        //int curyear = Convert.ToInt32(splitcur[2]);

        //string dateack = string.Empty;
        //dateack = txtstartdate.Text.ToString();

        //string[] splitack = dateack.Split(new Char[] { '-' });

        //int splitackdate = Convert.ToInt32(splitack[0]);
        //int splitackmonth = Convert.ToInt32(splitack[1]);
        //int splitackyear = Convert.ToInt32(splitack[2]);

        //if (splitackyear > curyear)
        //{
        //    errmsg.Visible = false;
        //}
        //else if (splitackyear == curyear)
        //{
        //    if (splitackmonth > curmonth)
        //    {
        //        errmsg.Visible = false;
        //    }
        //    else if (splitackmonth == curmonth)
        //    {
        //        if (splitackdate >= curdate)
        //        {
        //            errmsg.Visible = false;
        //        }
        //        else
        //        {
        //            errmsg.Visible = true;
        //            errmsg.Text = "Please Enter Correct Date";
        //            txtstartdate.Text = string.Empty;
        //        }
        //    }
        //    else
        //    {
        //        errmsg.Visible = true;
        //        errmsg.Text = "Please Enter Correct Date";
        //        txtstartdate.Text = string.Empty;
        //    }
        //}
        //else
        //{
        //    errmsg.Visible = true;
        //    errmsg.Text = "Please Enter Correct Date";
        //    txtstartdate.Text = string.Empty;
        //}
        lblErrMsg.Text = string.Empty;
        lblErrMsg.Visible = false;
        txtdays.Text = "0";
        bool isValidAll = false;
        string suspendedFromDate = txtstartdate.Text.Trim();
        string suspendedToDate = txtEndDate.Text.Trim();
        DateTime dtSuspendFromDate = new DateTime();
        DateTime dtSuspendToDate = new DateTime();

        DateTime dtSemStart = new DateTime();
        DateTime dtSemEnd = new DateTime();

        bool isFromSuccess = DateTime.TryParseExact(suspendedFromDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendFromDate);
        bool isToSuccess = DateTime.TryParseExact(suspendedToDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendToDate);

        string semesterEndDate = d2.GetFunction("select convert(varchar(50),end_date,105) end_date from seminfo where degree_code='" + Convert.ToString(ddlbrachadd.SelectedItem.Value).Trim() + "' and batch_year='" + Convert.ToString(ddlbatchadd.SelectedValue).Trim() + "' and semester='" + Convert.ToString(ddlsemadd.SelectedItem.Text).Trim() + "'");
        string semesterStartDate = d2.GetFunction("select convert(varchar(50),start_date,105) start_date from seminfo where degree_code='" + Convert.ToString(ddlbrachadd.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlbatchadd.SelectedValue).Trim() + "' and semester='" + Convert.ToString(ddlsemadd.SelectedItem.Text).Trim() + "'");

        bool isSemStart = DateTime.TryParseExact(semesterStartDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSemStart);
        bool isSemEnd = DateTime.TryParseExact(semesterEndDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSemEnd);

        if (string.IsNullOrEmpty(semesterStartDate.Trim()) || string.IsNullOrEmpty(semesterEndDate.Trim()))
        {
            lblErrMsg.Text = "Please Set " + ((string.IsNullOrEmpty(semesterStartDate.Trim()) && string.IsNullOrEmpty(semesterEndDate.Trim())) ? "Semester Start Date And Semester End Date " : ((string.IsNullOrEmpty(semesterEndDate.Trim())) ? "Semester End Date " : ((string.IsNullOrEmpty(semesterStartDate.Trim())) ? "Semester Start Date " : "")));
            lblErrMsg.Visible = true;
            return;
        }
        else if (semesterStartDate.Trim() == "0" || semesterEndDate.Trim() == "0")
        {
            lblErrMsg.Text = "Please Set " + ((semesterStartDate.Trim() == "0" && semesterEndDate.Trim() == "0") ? "Semester Start Date and Semester End Date " : ((semesterEndDate.Trim() == "0") ? "Semester End Date " : ((semesterStartDate.Trim() == "0") ? "Semester Start Date " : "")));
            lblErrMsg.Visible = true;
            return;
        }

        if (string.IsNullOrEmpty(suspendedFromDate.Trim()) || string.IsNullOrEmpty(suspendedToDate.Trim()))
        {
            lblErrMsg.Text = "Please Select " + ((string.IsNullOrEmpty(suspendedFromDate.Trim()) && string.IsNullOrEmpty(suspendedToDate.Trim())) ? "Suspend Start Date And Suspend End Date " : ((string.IsNullOrEmpty(suspendedToDate.Trim())) ? "Suspend End Date " : ((string.IsNullOrEmpty(suspendedFromDate.Trim())) ? "Suspend Start Date " : "")));
            lblErrMsg.Visible = true;
        }
        else
        {
            if (isFromSuccess && isToSuccess)
            {
                if (dtSuspendFromDate < dtSemStart)
                {
                    lblErrMsg.Text = "Please Select Suspend Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Semester Start Date " + dtSemStart.ToString("dd-MM-yyyy") + ".";
                    lblErrMsg.Visible = true;
                }
                else if (dtSuspendFromDate > dtSemEnd)
                {
                    lblErrMsg.Text = "Please Select Suspend Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + " Must Be lesser Than or Equal To Semester End Date " + dtSemEnd.ToString("dd-MM-yyyy") + ".";
                    lblErrMsg.Visible = true;
                }
                else if (dtSuspendToDate < dtSemStart)
                {
                    lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Semester Start Date " + dtSemStart.ToString("dd-MM-yyyy") + ".";
                    lblErrMsg.Visible = true;
                }
                else if (dtSuspendToDate > dtSemEnd)
                {
                    lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be lesser Than or Equal To Semester End Date " + dtSemEnd.ToString("dd-MM-yyyy") + ".";
                    lblErrMsg.Visible = true;
                }
                else if (dtSuspendFromDate > dtSuspendToDate)
                {
                    lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + ".";
                    lblErrMsg.Visible = true;
                }
                else
                {
                    txtdays.Text = Convert.ToString(dtSuspendToDate.Subtract(dtSuspendFromDate).Days + 1).Trim();
                    isValidAll = true;
                }
            }
            else
            {
                lblErrMsg.Text = "Please Select Valid " + ((!isFromSuccess && !isToSuccess) ? " Suspend Start Date and Suspend End Date" : ((!isFromSuccess) ? " Suspend Start Date" : ((!isToSuccess) ? "Suspend End Date" : "")));
                lblErrMsg.Visible = true;
            }
        }
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrMsg.Text = string.Empty;
            lblErrMsg.Visible = false;
            txtdays.Text = "0";
            bool isValidAll = false;
            string suspendedFromDate = txtstartdate.Text.Trim();
            string suspendedToDate = txtEndDate.Text.Trim();
            DateTime dtSuspendFromDate = new DateTime();
            DateTime dtSuspendToDate = new DateTime();

            DateTime dtSemStart = new DateTime();
            DateTime dtSemEnd = new DateTime();

            bool isFromSuccess = DateTime.TryParseExact(suspendedFromDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendFromDate);
            bool isToSuccess = DateTime.TryParseExact(suspendedToDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendToDate);

            string semesterEndDate = d2.GetFunction("select convert(varchar(50),end_date,105) end_date from seminfo where degree_code='" + Convert.ToString(ddlbrachadd.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlbatchadd.SelectedValue).Trim() + "' and semester='" + Convert.ToString(ddlsemadd.SelectedItem.Text).Trim() + "'");
            string semesterStartDate = d2.GetFunction("select convert(varchar(50),start_date,105) start_date from seminfo where degree_code='" + Convert.ToString(ddlbrachadd.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlbatchadd.SelectedValue).Trim() + "' and semester='" + Convert.ToString(ddlsemadd.SelectedItem.Text).Trim() + "'");

            bool isSemStart = DateTime.TryParseExact(semesterStartDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSemStart);
            bool isSemEnd = DateTime.TryParseExact(semesterEndDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSemEnd);

            if (string.IsNullOrEmpty(semesterStartDate.Trim()) || string.IsNullOrEmpty(semesterEndDate.Trim()))
            {
                lblErrMsg.Text = "Please Set " + ((string.IsNullOrEmpty(semesterStartDate.Trim()) && string.IsNullOrEmpty(semesterEndDate.Trim())) ? "Semester Start Date And Semester End Date " : ((string.IsNullOrEmpty(semesterEndDate.Trim())) ? "Semester End Date " : ((string.IsNullOrEmpty(semesterStartDate.Trim())) ? "Semester Start Date " : "")));
                lblErrMsg.Visible = true;
                return;
            }
            else if (semesterStartDate.Trim() == "0" || semesterEndDate.Trim() == "0")
            {
                lblErrMsg.Text = "Please Set " + ((semesterStartDate.Trim() == "0" && semesterEndDate.Trim() == "0") ? "Semester Start Date and Semester End Date " : ((semesterEndDate.Trim() == "0") ? "Semester End Date " : ((semesterStartDate.Trim() == "0") ? "Semester Start Date " : "")));
                lblErrMsg.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(suspendedFromDate.Trim()) || string.IsNullOrEmpty(suspendedToDate.Trim()))
            {
                lblErrMsg.Text = "Please Select " + ((string.IsNullOrEmpty(suspendedFromDate.Trim()) && string.IsNullOrEmpty(suspendedToDate.Trim())) ? "Suspend Start Date And Suspend End Date " : ((string.IsNullOrEmpty(suspendedToDate.Trim())) ? "Suspend End Date " : ((string.IsNullOrEmpty(suspendedFromDate.Trim())) ? "Suspend Start Date " : "")));
                lblErrMsg.Visible = true;
            }
            else
            {
                if (isFromSuccess && isToSuccess)
                {
                    if (dtSuspendFromDate < dtSemStart)
                    {
                        lblErrMsg.Text = "Please Select Suspend Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Semester Start Date " + dtSemStart.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendFromDate > dtSemEnd)
                    {
                        lblErrMsg.Text = "Please Select Suspend Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + " Must Be lesser Than or Equal To Semester End Date " + dtSemEnd.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendToDate < dtSemStart)
                    {
                        lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Semester Start Date " + dtSemStart.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendToDate > dtSemEnd)
                    {
                        lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be lesser Than or Equal To Semester End Date " + dtSemEnd.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendFromDate > dtSuspendToDate)
                    {
                        lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else
                    {
                        txtdays.Text = Convert.ToString(dtSuspendToDate.Subtract(dtSuspendFromDate).Days + 1).Trim();
                        isValidAll = true;
                    }
                }
                else
                {
                    lblErrMsg.Text = "Please Select Valid " + ((!isFromSuccess && !isToSuccess) ? " Suspend Start Date and Suspend End Date" : ((!isFromSuccess) ? " Suspend Start Date" : ((!isToSuccess) ? "Suspend End Date" : "")));
                    lblErrMsg.Visible = true;
                }
            }
        }
        catch
        {
        }
    }

    public bool ValidateFeeOnRoll()
    {
        bool isValid = false;
        lblErrMsg.Text = string.Empty;
        lblErrMsg.Visible = false;
        string feeOffRollDate = txtdate1.Text.Trim();
        string feeOnRollDate = txtFeeOnRollDate.Text.Trim();
        DateTime dtFeeOffRollDate = new DateTime();
        DateTime dtFeeOnRollDate = new DateTime();
        lblErrMsg.Visible = false;
        lblErrMsg.Text = string.Empty;
        bool isSuccOffroll = DateTime.TryParseExact(feeOffRollDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFeeOffRollDate);
        bool isSuccOnroll = DateTime.TryParseExact(feeOnRollDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFeeOnRollDate);
        if (!string.IsNullOrEmpty(feeOffRollDate.Trim()))
        {
            if (isSuccOffroll)
            {
                if (!string.IsNullOrEmpty(feeOnRollDate.Trim()))
                {
                    if (isSuccOnroll)
                    {
                        if (dtFeeOffRollDate > dtFeeOnRollDate)
                        {
                            lblErrMsg.Text = "Please Select Fee On Roll Date " + dtFeeOnRollDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Fee off Roll Date " + dtFeeOffRollDate.ToString("dd-MM-yyyy") + " ";
                            lblErrMsg.Visible = true;
                            isValid = false;
                        }
                        else
                        {
                            isValid = true;
                        }
                    }
                    else
                    {
                        lblErrMsg.Text = "Please Select Valid Fee on Roll Date " + dtFeeOnRollDate.ToString("dd-MM-yyyy") + "";
                        lblErrMsg.Visible = true;
                        isValid = false;
                    }
                }
                else
                {
                    lblErrMsg.Text = "Please Select Fee on Roll Date " + dtFeeOnRollDate.ToString("dd-MM-yyyy") + "";
                    lblErrMsg.Visible = true;
                    isValid = false;
                }
            }
            else
            {
                lblErrMsg.Text = "Please Select Valid Fee off Roll Date " + dtFeeOffRollDate.ToString("dd-MM-yyyy") + "";
                lblErrMsg.Visible = true;
                isValid = false;
            }
        }
        else
        {
            lblErrMsg.Text = "Please Select Fee off Roll Date " + dtFeeOffRollDate.ToString("dd-MM-yyyy") + "";
            lblErrMsg.Visible = true;
            isValid = false;
        }
        return isValid;
    }

    public bool ValidateSuspended()
    {
        try
        {
            lblErrMsg.Text = string.Empty;
            lblErrMsg.Visible = false;
            txtdays.Text = "0";
            bool isValidAll = false;

            string suspendedFromDate = txtstartdate.Text.Trim();
            string suspendedToDate = txtEndDate.Text.Trim();

            DateTime dtSuspendFromDate = new DateTime();
            DateTime dtSuspendToDate = new DateTime();

            DateTime dtSemStart = new DateTime();
            DateTime dtSemEnd = new DateTime();

            bool isFromSuccess = DateTime.TryParseExact(suspendedFromDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendFromDate);
            bool isToSuccess = DateTime.TryParseExact(suspendedToDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSuspendToDate);

            string semesterEndDate = d2.GetFunction("select convert(varchar(50),end_date,105) end_date from seminfo where degree_code='" + Convert.ToString(ddlbrachadd.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlbatchadd.SelectedValue).Trim() + "' and semester='" + Convert.ToString(ddlsemadd.SelectedItem.Text).Trim() + "'");
            string semesterStartDate = d2.GetFunction("select convert(varchar(50),start_date,105) start_date from seminfo where degree_code='" + Convert.ToString(ddlbrachadd.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlbatchadd.SelectedValue).Trim() + "' and semester='" + Convert.ToString(ddlsemadd.SelectedItem.Text).Trim() + "'");

            bool isSemStart = DateTime.TryParseExact(semesterStartDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSemStart);
            bool isSemEnd = DateTime.TryParseExact(semesterEndDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtSemEnd);

            if (string.IsNullOrEmpty(semesterStartDate.Trim()) || string.IsNullOrEmpty(semesterEndDate.Trim()))
            {
                lblErrMsg.Text = "Please Set " + ((string.IsNullOrEmpty(semesterStartDate.Trim()) && string.IsNullOrEmpty(semesterEndDate.Trim())) ? "Semester Start Date And Semester End Date " : ((string.IsNullOrEmpty(semesterEndDate.Trim())) ? "Semester End Date " : ((string.IsNullOrEmpty(semesterStartDate.Trim())) ? "Semester Start Date " : "")));
                lblErrMsg.Visible = true;
            }
            else if (semesterStartDate.Trim() == "0" || semesterEndDate.Trim() == "0")
            {
                lblErrMsg.Text = "Please Set " + ((semesterStartDate.Trim() == "0" && semesterEndDate.Trim() == "0") ? "Semester Start Date and Semester End Date " : ((semesterEndDate.Trim() == "0") ? "Semester End Date " : ((semesterStartDate.Trim() == "0") ? "Semester Start Date " : "")));
                lblErrMsg.Visible = true;
            }

            if (string.IsNullOrEmpty(suspendedFromDate.Trim()) || string.IsNullOrEmpty(suspendedToDate.Trim()))
            {
                lblErrMsg.Text = "Please Select " + ((string.IsNullOrEmpty(suspendedFromDate.Trim()) && string.IsNullOrEmpty(suspendedToDate.Trim())) ? "Suspend Start Date And Suspend End Date " : ((string.IsNullOrEmpty(suspendedToDate.Trim())) ? "Suspend End Date " : ((string.IsNullOrEmpty(suspendedFromDate.Trim())) ? "Suspend Start Date " : "")));
                lblErrMsg.Visible = true;
            }
            else
            {
                if (isFromSuccess && isToSuccess)
                {
                    if (dtSuspendFromDate < dtSemStart)
                    {
                        lblErrMsg.Text = "Please Select Suspend Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Semester Start Date " + dtSemStart.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendFromDate > dtSemEnd)
                    {
                        lblErrMsg.Text = "Please Select Suspend Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + " Must Be lesser Than or Equal To Semester End Date " + dtSemEnd.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendToDate < dtSemStart)
                    {
                        lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Semester Start Date " + dtSemStart.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendToDate > dtSemEnd)
                    {
                        lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be lesser Than or Equal To Semester End Date " + dtSemEnd.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else if (dtSuspendFromDate > dtSuspendToDate)
                    {
                        lblErrMsg.Text = "Please Select Suspend End Date " + dtSuspendToDate.ToString("dd-MM-yyyy") + " Must Be Greater Than or Equal To Start Date " + dtSuspendFromDate.ToString("dd-MM-yyyy") + ".";
                        lblErrMsg.Visible = true;
                    }
                    else
                    {
                        txtdays.Text = Convert.ToString(dtSuspendToDate.Subtract(dtSuspendFromDate).Days + 1).Trim();
                        isValidAll = true;
                    }
                }
                else
                {
                    lblErrMsg.Text = "Please Select Valid " + ((!isFromSuccess && !isToSuccess) ? " Suspend Start Date and Suspend End Date" : ((!isFromSuccess) ? " Suspend Start Date" : ((!isToSuccess) ? "Suspend End Date" : "")));
                    lblErrMsg.Visible = true;
                }
            }
            return isValidAll;
        }
        catch
        {
            return false;
        }
    }

    #endregion Added By Malang Raja

}
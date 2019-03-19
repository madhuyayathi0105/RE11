#region Namespace Declaration

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion Namespace Declaration

public partial class PostedHourReport : System.Web.UI.Page
{
    #region Variable Declaration

    Hashtable hat = new Hashtable();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string batch_year = string.Empty;
    string degree_code = string.Empty;

    string qry = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    Boolean b_school = false;
    bool check = false;

    #endregion Variable Declaration

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Header.DataBind();
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(Session["collegecode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            string grouporusercode = string.Empty;
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables.Count > 0 && schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]);
                if (schoolvalue.Trim() == "0")
                {
                    b_school = true;
                }
            }
            if (!IsPostBack)
            {
                setLabelText();
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Admissionno"] = "0";
                Session["Studflag"] = "0";
                string Master1 = "select * from Master_Settings where settings in('Admission No','Roll No','Register No','Student_Type') and" + grouporusercode + "";
                ds2.Clear();
                ds2 = d2.select_method_wo_parameter(Master1, "Text");
                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {
                        if (ds2.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds2.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds2.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds2.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds2.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds2.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (ds2.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds2.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Admissionno"] = "1";
                        }
                    }
                }
                cb_notposted.Checked = true;
                chkuser.Checked = true;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                Bindcollege();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
                Bindhour();
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Init_Spread();
                rptprint1.Visible = false;
                popupdiv.Visible = false;
                divPostHrSpread.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Page Load

    #region Logout

    protected void lbtnLogout_Click(object sender, EventArgs e)
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Logout

    #region Bind Header

    public void Bindcollege()
    {
        try
        {
            string columnfield = string.Empty;
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]) + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = dsprint;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
            else
            {
                lblErrSearch.Text = "Set College Rights to the Staff or User";
                lblErrSearch.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindBatch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            txtBatch.Text = "---Select---";
            cbBatch.Checked = false;
            cblBatch.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            int count = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblBatch.DataSource = ds.Tables[0];
                cblBatch.DataTextField = "Batch_year";
                cblBatch.DataValueField = "Batch_year";
                cblBatch.DataBind();
                cblBatch.SelectedIndex = cblBatch.Items.Count - 1;
                for (int i = 0; i < cblBatch.Items.Count; i++)
                {
                    cblBatch.Items[i].Selected = true;
                    if (cblBatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (cblBatch.Items.Count == count)
                    {
                        cbBatch.Checked = true;
                    }
                }
                if (cbBatch.Checked == true)
                {
                    for (int i = 0; i < cblBatch.Items.Count; i++)
                    {
                        cblBatch.Items[i].Selected = true;
                        txtBatch.Text = "Batch(" + (cblBatch.Items.Count) + ")";
                        if (b_school == true)
                        {
                            txtBatch.Text = "Year(" + (cblBatch.Items.Count) + ")";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cblBatch.Items.Count; i++)
                    {
                        cblBatch.Items[i].Selected = false;
                        txtBatch.Text = "---Select---";
                    }
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            int count = 0;
            txtDegree.Text = "---Select---";
            cblDegree.Items.Clear();
            cbDegree.Checked = false;
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblDegree.DataSource = ds;
                cblDegree.DataTextField = "course_name";
                cblDegree.DataValueField = "course_id";
                cblDegree.DataBind();
                cblDegree.Items[0].Selected = true;
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = true;
                    if (cblDegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (cblDegree.Items.Count == count)
                    {
                        cbDegree.Checked = true;
                    }
                }
                if (cbDegree.Checked == true)
                {
                    for (int i = 0; i < cblDegree.Items.Count; i++)
                    {
                        cblDegree.Items[i].Selected = true;
                        txtDegree.Text = "Degree(" + (cblDegree.Items.Count) + ")";
                        if (b_school == true)
                        {
                            txtDegree.Text = "School Type(" + (cblDegree.Items.Count) + ")";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cblDegree.Items.Count; i++)
                    {
                        cblDegree.Items[i].Selected = false;
                        txtDegree.Text = "---Select---";
                    }
                }
                txtDegree.Enabled = true;
            }
            else
            {
                txtDegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            int count = 0;
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                if (cblDegree.Items[i].Selected == true)
                {
                    if (string.IsNullOrEmpty(course_id))
                    {
                        course_id = "" + Convert.ToString(cblDegree.Items[i].Value) + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + Convert.ToString(cblDegree.Items[i].Value) + "";
                    }
                }
            }
            txtBranch.Text = "---Select---";
            cbBranch.Checked = false;
            cblBranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblBranch.DataSource = ds;
                cblBranch.DataTextField = "dept_name";
                cblBranch.DataValueField = "degree_code";
                cblBranch.DataBind();
                cblBranch.Items[0].Selected = true;
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    cblBranch.Items[i].Selected = true;
                    if (cblBranch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (cblBranch.Items.Count == count)
                    {
                        cbBranch.Checked = true;
                    }
                }
                if (cbBranch.Checked == true)
                {
                    for (int i = 0; i < cblBranch.Items.Count; i++)
                    {
                        cblBranch.Items[i].Selected = true;
                        txtBranch.Text = "Branch(" + (cblBranch.Items.Count) + ")";
                        if (b_school == true)
                        {
                            txtBranch.Text = "Standard(" + (cblBranch.Items.Count) + ")";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cblBranch.Items.Count; i++)
                    {
                        cblBranch.Items[i].Selected = false;
                        txtBranch.Text = "---Select---";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void Bindhour()
    {
        try
        {
            string sqlbatch = string.Empty;
            string sqlbranch = string.Empty;
            string sqlbatchquery = string.Empty;
            string sqlbranchquery = string.Empty;
            txtPeriod.Text = "---Select---";
            cbPeriod.Checked = false;
            sqlbatch = string.Empty;
            //if (sqlbatch != "")
            //{
            //    sqlbatch = " in(" + sqlbatch + ")";
            //    sqlbatchquery = " and si.batch_year  " + sqlbatch + "";
            //}
            //else
            //{
            //    sqlbatchquery = " ";
            //}
            if (txtBatch.Text != "---Select---")
            {
                int itemcount = 0;
                for (itemcount = 0; itemcount < cblBatch.Items.Count; itemcount++)
                {
                    if (cblBatch.Items[itemcount].Selected == true)
                    {
                        if (string.IsNullOrEmpty(sqlbatch))
                            sqlbatch = "'" + Convert.ToString(cblBatch.Items[itemcount].Value) + "'";
                        else
                            sqlbatch = sqlbatch + "," + "'" + Convert.ToString(cblBatch.Items[itemcount].Value) + "'";
                    }
                }
                if (!string.IsNullOrEmpty(sqlbatch))
                {
                    sqlbatch = " in (" + sqlbatch + ")";
                    sqlbatchquery = " and si.batch_year  " + sqlbatch + "";
                }
                else
                {
                    sqlbatchquery = " ";
                }
            }
            if (txtBranch.Text != "---Select---")
            {
                int itemcount = 0;
                for (itemcount = 0; itemcount < cblBranch.Items.Count; itemcount++)
                {
                    if (cblBranch.Items[itemcount].Selected == true)
                    {
                        if (string.IsNullOrEmpty(sqlbranch))
                            sqlbranch = "'" + Convert.ToString(cblBranch.Items[itemcount].Value) + "'";
                        else
                            sqlbranch = sqlbranch + "," + "'" + Convert.ToString(cblBranch.Items[itemcount].Value) + "'";
                    }
                }
                if (!string.IsNullOrEmpty(sqlbranch))
                {
                    sqlbranch = " in(" + sqlbranch + ")";
                    sqlbranchquery = " and pa.degree_code  " + sqlbranch + "";
                }
                else
                {
                    sqlbranchquery = " ";
                }
            }
            cblPeriod.Items.Clear();
            ds.Dispose();
            ds.Reset();
            string qeryss = "select max(pa.No_of_hrs_per_day) as No_of_hrs_per_day from PeriodAttndSchedule pa,seminfo si,Registration r where pa.degree_code=si.degree_code and r.degree_code=pa.degree_code and r.degree_code=si.degree_code and r.Batch_Year=si.batch_year and pa.semester=si.semester " + sqlbatchquery + " " + sqlbranchquery + " and  college_code='" + Convert.ToString(ddlCollege.SelectedValue) + "'";
            qeryss = "select max(pa.No_of_hrs_per_day) as No_of_hrs_per_day from PeriodAttndSchedule pa,seminfo si where pa.degree_code=si.degree_code and pa.semester=si.semester " + sqlbatchquery + " " + sqlbranchquery + "";
            ds = d2.select_method(qeryss, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int noofhour = 0;
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["No_of_hrs_per_day"]), out noofhour);
                txtPeriod.Enabled = true;
                cblPeriod.Enabled = true;
                cbPeriod.Checked = false;
                for (int i = 1; i <= noofhour; i++)
                {
                    cblPeriod.Items.Add(Convert.ToString(i));
                }
                if (cbPeriod.Checked == true)
                {
                    for (int i = 0; i < cblPeriod.Items.Count; i++)
                    {
                        cblPeriod.Items[i].Selected = true;
                        txtPeriod.Text = "Periods(" + (cblPeriod.Items.Count) + ")";
                        if (b_school == true)
                        {
                            txtPeriod.Text = "Periods(" + (cblPeriod.Items.Count) + ")";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cblPeriod.Items.Count; i++)
                    {
                        cblPeriod.Items[i].Selected = false;
                        txtPeriod.Text = "---Select---";
                    }
                }
            }
            else
            {
                cblPeriod.Enabled = false;
                txtPeriod.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void DateValidation()
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            string FromDate = string.Empty;
            string toDate = string.Empty;
            DateTime dtsemstart = new DateTime();
            DateTime dtsemend = new DateTime();
            DateTime dtFromDate = new DateTime();
            DateTime dttoDate = new DateTime();
            DateTime dtToday = DateTime.Now;
            bool isValidFrom = false, isValidTo = false;
            if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
            {
                FromDate = Convert.ToString(txtFromDate.Text);
                isValidFrom = DateTime.TryParseExact(FromDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
            }
            else
            {
                lblErrSearch.Text = "Please Choose From Date";
                lblErrSearch.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
            {
                toDate = Convert.ToString(txtToDate.Text);
                isValidTo = DateTime.TryParseExact(toDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dttoDate);
            }
            else
            {
                lblErrSearch.Text = "Please Choose To Date";
                lblErrSearch.Visible = true;
                return;
            }
            if (isValidFrom && isValidTo)
            {
                if (dtFromDate > dtToday)
                {
                    lblErrSearch.Text = "From Date Must Be Lesser Than or Equal to Today Date";
                    lblErrSearch.Visible = true;
                    return;
                }
                if (dttoDate > dtToday)
                {
                    lblErrSearch.Text = "To Date Must Be Lesser Than or Equal to Today Date";
                    lblErrSearch.Visible = true;
                    return;
                }
                if (dtFromDate > dttoDate)
                {
                    lblErrSearch.Text = "From Date Must Be Lesser Than or Equal to To Date";
                    lblErrSearch.Visible = true;
                    return;
                }
                if (dttoDate < dtFromDate)
                {
                    lblErrSearch.Text = "To Date Must Be Greater Than or Equal to From Date";
                    lblErrSearch.Visible = true;
                    return;
                }
            }
            else
            {
                lblErrSearch.Text = "From Date and To Date Must Be in the Format dd/MM/yyyy";
                lblErrSearch.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            return;
        }
    }

    public void Init_Spread()
    {
        #region FpSpread Style

        FpPostedHr.Visible = false;
        FpPostedHr.Sheets[0].ColumnCount = 0;
        FpPostedHr.Sheets[0].RowCount = 0;
        FpPostedHr.Sheets[0].SheetCorner.ColumnCount = 0;
        FpPostedHr.CommandBar.Visible = false;

        #endregion FpSpread Style

        FpPostedHr.Visible = false;
        FpPostedHr.CommandBar.Visible = false;
        FpPostedHr.RowHeader.Visible = false;
        FpPostedHr.Sheets[0].AutoPostBack = true;
        FpPostedHr.Sheets[0].RowCount = 0;
        FpPostedHr.Sheets[0].ColumnCount = 4;
        FpPostedHr.Sheets[0].FrozenColumnCount = 4;

        #region SpreadStyles

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
        //darkstyle.ForeColor = System.Drawing.Color.Black;
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Font.Bold = true;
        darkstyle.HorizontalAlign = HorizontalAlign.Center;
        darkstyle.VerticalAlign = VerticalAlign.Middle;
        darkstyle.ForeColor = System.Drawing.Color.White;
        darkstyle.Border.BorderSize = 0;
        darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
        FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
        //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
        //darkstyle.ForeColor = System.Drawing.Color.Black;
        sheetstyle.Font.Name = "Book Antiqua";
        sheetstyle.Font.Size = FontUnit.Medium;
        sheetstyle.Font.Bold = true;
        sheetstyle.HorizontalAlign = HorizontalAlign.Left;
        sheetstyle.VerticalAlign = VerticalAlign.Middle;
        sheetstyle.ForeColor = System.Drawing.Color.Black;
        sheetstyle.Border.BorderSize = 1;
        sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

        #endregion SpreadStyles

        FpPostedHr.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        FpPostedHr.Sheets[0].DefaultStyle = sheetstyle;
        FpPostedHr.Sheets[0].ColumnHeader.RowCount = 2;
        FpPostedHr.Sheets[0].Columns[0].Width = 40;
        FpPostedHr.Sheets[0].Columns[1].Width = 250;
        FpPostedHr.Sheets[0].Columns[2].Width = 100;
        FpPostedHr.Sheets[0].Columns[3].Width = 250;

        FpPostedHr.Sheets[0].Columns[0].Locked = true;
        FpPostedHr.Sheets[0].Columns[1].Locked = true;
        FpPostedHr.Sheets[0].Columns[2].Locked = true;
        FpPostedHr.Sheets[0].Columns[3].Locked = true;

        FpPostedHr.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpPostedHr.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblBranch.Text;
        FpPostedHr.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Sections";
        FpPostedHr.Sheets[0].ColumnHeader.Cells[0, 3].Text = "User";

        FpPostedHr.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        FpPostedHr.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        FpPostedHr.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        FpPostedHr.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

        FpPostedHr.Sheets[0].SelectionBackColor = Color.Transparent;
        if (chkuser.Checked == true)
        {
            FpPostedHr.Sheets[0].Columns[3].Visible = true;
        }
        else
        {
            FpPostedHr.Sheets[0].Columns[3].Visible = false;
        }
    }

    #endregion Bind Header

    #region DropDownList Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(Session["collegecode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            cblBatch.Items.Clear();
            cblDegree.Items.Clear();
            cblBranch.Items.Clear();
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
            Bindhour();
            //semstartend();
            FpSpread1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion DropDownList Events

    #region CheckBox Events

    protected void cbBatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            if (cbBatch.Checked == true)
            {
                for (int i = 0; i < cblBatch.Items.Count; i++)
                {
                    cblBatch.Items[i].Selected = true;
                }
                txtBatch.Text = "Batch(" + (cblBatch.Items.Count) + ")";
                if (b_school == true)
                {
                    txtBatch.Text = "Year(" + (cblBatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cblBatch.Items.Count; i++)
                {
                    cblBatch.Items[i].Selected = false;
                }
                txtBatch.Text = "---Select---";
            }
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
            Bindhour();
            FpSpread1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cbDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbDegree.Checked == true)
            {
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = true;
                }
                txtDegree.Text = "Degree(" + (cblDegree.Items.Count) + ")";
                if (b_school == true)
                {
                    txtDegree.Text = "School Type(" + (cblDegree.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = false;
                }
                txtDegree.Text = "---Select---";
                txtBranch.Text = "---Select---";
            }
            BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
            Bindhour();
            //semstartend();
            FpSpread1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cbBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            if (cbBranch.Checked == true)
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    cblBranch.Items[i].Selected = true;
                }
                txtBranch.Text = "Branch(" + (cblBranch.Items.Count) + ")";
                if (b_school == true)
                {
                    txtBranch.Text = "Standard(" + (cblBranch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    cblBranch.Items[i].Selected = false;
                }
                txtBranch.Text = "---Select---";
            }
            Bindhour();
            FpSpread1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cbPeriod_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            if (cbPeriod.Checked == true)
            {
                for (int i = 0; i < cblPeriod.Items.Count; i++)
                {
                    cblPeriod.Items[i].Selected = true;
                }
                txtPeriod.Text = "Periods(" + (cblPeriod.Items.Count) + ")";
                if (b_school == true)
                {
                    txtPeriod.Text = "Periods(" + (cblPeriod.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cblPeriod.Items.Count; i++)
                {
                    cblPeriod.Items[i].Selected = false;
                }
                txtPeriod.Text = "---Select---";
            }
            FpSpread1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion CheckBox Events

    #region CheckBoxList Events

    protected void cblBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            int commcount = 0;
            txtBatch.Text = "--Select--";
            cbBatch.Checked = false;
            for (int i = 0; i < cblBatch.Items.Count; i++)
            {
                if (cblBatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtBatch.Text = "Batch(" + Convert.ToString(commcount) + ")";
                if (b_school == true)
                {
                    txtBatch.Text = "Year(" + Convert.ToString(commcount) + ")";
                }
                if (commcount == cblBatch.Items.Count)
                {
                    cbBatch.Checked = true;
                }
            }
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
            Bindhour();
            FpSpread1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            int commcount = 0;
            cbDegree.Checked = false;
            txtDegree.Text = "---Select---";
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                if (cblDegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtDegree.Text = "Degree(" + Convert.ToString(commcount) + ")";
                if (b_school == true)
                {
                    txtDegree.Text = "School Type(" + Convert.ToString(commcount) + ")";
                }
                if (commcount == cblDegree.Items.Count)
                {
                    cbDegree.Checked = true;
                }
            }
            BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
            Bindhour();
            //semstartend();
            FpSpread1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            string clg = string.Empty;
            int commcount = 0;
            txtBranch.Text = "--Select--";
            cbBranch.Checked = false;
            for (int i = 0; i < cblBranch.Items.Count; i++)
            {
                if (cblBranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtBranch.Text = "Branch(" + Convert.ToString(commcount) + ")";
                if (b_school == true)
                {
                    txtBranch.Text = "Standard(" + Convert.ToString(commcount) + ")";
                }
                if (commcount == cblBranch.Items.Count)
                {
                    cbBranch.Checked = true;
                }
            }
            Bindhour();
            FpSpread1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            int commcount = 0;
            cbPeriod.Checked = false;
            txtPeriod.Text = "---Select---";
            for (int i = 0; i < cblPeriod.Items.Count; i++)
            {
                if (cblPeriod.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtPeriod.Text = "Periods(" + Convert.ToString(commcount) + ")";
                if (b_school == true)
                {
                    txtPeriod.Text = "Periods(" + Convert.ToString(commcount) + ")";
                }
                if (commcount == cblPeriod.Items.Count)
                {
                    cbPeriod.Checked = true;
                }
            }
            FpSpread1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion CheckBoxList Events

    #region TextBox Changed Events

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            FpSpread1.Visible = false;
            string FromDate = string.Empty;
            string toDate = string.Empty;
            DateTime dtsemstart = new DateTime();
            DateTime dtsemend = new DateTime();
            DateTime dtFromDate = new DateTime();
            DateTime dttoDate = new DateTime();
            DateTime dtToday = DateTime.Now;
            bool isValidFrom = false, isValidTo = false;
            if (txtFromDate.Text.Trim() != "")
            {
                FromDate = Convert.ToString(txtFromDate.Text);
                isValidFrom = DateTime.TryParseExact(FromDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
            }
            else
            {
                lblErrSearch.Text = "Please Choose From Date";
                lblErrSearch.Visible = true;
                return;
            }
            if (txtToDate.Text.Trim() != "")
            {
                toDate = Convert.ToString(txtToDate.Text);
                isValidTo = DateTime.TryParseExact(toDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dttoDate);
            }
            else
            {
                lblErrSearch.Text = "Please Choose To Date";
                lblErrSearch.Visible = true;
                return;
            }
            if (isValidFrom && isValidTo)
            {
                if (dtFromDate > dtToday)
                {
                    lblErrSearch.Text = "From Date Must Be Lesser Than or Equal to Today Date";
                    lblErrSearch.Visible = true;
                    //txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    return;
                }
                if (dttoDate > dtToday)
                {
                    lblErrSearch.Text = "To Date Must Be Lesser Than or Equal to Today Date";
                    lblErrSearch.Visible = true;
                    //txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    return;
                }
                if (dtFromDate > dttoDate)
                {
                    lblErrSearch.Text = "From Date Must Be Lesser Than or Equal to To Date";
                    lblErrSearch.Visible = true;
                    //txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    return;
                }
                if (dttoDate < dtFromDate)
                {
                    lblErrSearch.Text = "To Date Must Be Greater Than or Equal to From Date";
                    lblErrSearch.Visible = true;
                    //txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    return;
                }
            }
            else
            {
                lblErrSearch.Text = "From Date and To Date Must Be in the Format dd/MM/yyyy";
                lblErrSearch.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            FpSpread1.Visible = false;
            string FromDate = string.Empty;
            string toDate = string.Empty;
            DateTime dtsemstart = new DateTime();
            DateTime dtsemend = new DateTime();
            DateTime dtFromDate = new DateTime();
            DateTime dttoDate = new DateTime();
            DateTime dtToday = DateTime.Now;
            bool isValidFrom = false, isValidTo = false;
            if (txtFromDate.Text.Trim() != "")
            {
                FromDate = Convert.ToString(txtFromDate.Text);
                isValidFrom = DateTime.TryParseExact(FromDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
            }
            else
            {
                lblErrSearch.Text = "Please Choose From Date";
                lblErrSearch.Visible = true;
                return;
            }
            if (txtToDate.Text.Trim() != "")
            {
                toDate = Convert.ToString(txtToDate.Text);
                isValidTo = DateTime.TryParseExact(toDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dttoDate);
            }
            else
            {
                lblErrSearch.Text = "Please Choose To Date";
                lblErrSearch.Visible = true;
                return;
            }
            if (isValidFrom && isValidTo)
            {
                if (dtFromDate > dtToday)
                {
                    lblErrSearch.Text = "From Date Must Be Lesser Than or Equal to Today Date";
                    lblErrSearch.Visible = true;
                    //txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    return;
                }
                if (dttoDate > dtToday)
                {
                    lblErrSearch.Text = "To Date Must Be Lesser Than or Equal to Today Date";
                    lblErrSearch.Visible = true;
                    //txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    return;
                }
                if (dtFromDate > dttoDate)
                {
                    lblErrSearch.Text = "From Date Must Be Lesser Than or Equal to To Date";
                    lblErrSearch.Visible = true;
                    //txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    return;
                }
                if (dttoDate < dtFromDate)
                {
                    lblErrSearch.Text = "To Date Must Be Greater Than or Equal to From Date";
                    lblErrSearch.Visible = true;
                    //txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    return;
                }
            }
            else
            {
                lblErrSearch.Text = "From Date and To Date Must Be in the Format dd/MM/yyyy";
                lblErrSearch.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion TextBox Changed Events

    #region Button Click

    #region GO Button

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            divPostHrSpread.Visible = false;
            string LeaveCodes = string.Empty;
            DataSet dsLeavecode = new DataSet();
            DataView dvlc = new DataView();
            DataSet dsAtt = new DataSet();
            DataView dvAtt = new DataView();
            int degselcount = 0;
            int selbatchcount = 0;
            int selperiod = 0;
            bool isfromDate = false, isToDate = false;
            DateTime dtFrom = new DateTime();
            DateTime dtTo = new DateTime();
            DateTime dtDummyFrom = new DateTime();
            string fromdate = string.Empty;
            string todate = string.Empty;
            string period = string.Empty;
            if (ddlCollege.Items.Count == 0)
            {
                lblpoperr.Text = ((b_school) ? "School" : "College") + " is not Found.Please Give Rights To User";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            }
            if (cblBatch.Items.Count == 0)
            {
                lblpoperr.Text = ((b_school) ? "" : "Batch ") + "Year is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                batch_year = string.Empty;
                foreach (ListItem li in cblBatch.Items)
                {
                    if (li.Selected)
                    {
                        selbatchcount++;
                        if (batch_year == "")
                        {
                            batch_year = li.Value;
                        }
                        else
                        {
                            batch_year += "," + li.Value;
                        }
                    }
                }
            }
            if (selbatchcount == 0)
            {
                lblpoperr.Text = "Please Select Any One " + ((b_school) ? "" : "Batch ") + "Year";
                popupdiv.Visible = true;
                return;
            }
            if (cblDegree.Items.Count == 0)
            {
                lblpoperr.Text = ((b_school) ? "School Type" : "Degree") + " is not found";
                popupdiv.Visible = true;
                return;
            }
            if (cblBranch.Items.Count == 0)
            {
                lblpoperr.Text = ((b_school) ? "Standard" : "Branch") + " is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                degree_code = string.Empty;
                foreach (ListItem li in cblBranch.Items)
                {
                    if (li.Selected)
                    {
                        degselcount++;
                        if (degree_code == "")
                        {
                            degree_code = li.Value;
                        }
                        else
                        {
                            degree_code += "," + li.Value;
                        }
                    }
                }
            }
            if (degselcount == 0)
            {
                lblpoperr.Text = "Please Select Any One " + ((b_school) ? "Standard" : "Branch");
                popupdiv.Visible = true;
                return;
            }
            if (cblPeriod.Items.Count == 0)
            {
                lblpoperr.Text = "Period is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                period = string.Empty;
                foreach (ListItem li in cblPeriod.Items)
                {
                    if (li.Selected)
                    {
                        selperiod++;
                        if (period == "")
                        {
                            period = li.Value;
                        }
                        else
                        {
                            period += "," + li.Value;
                        }
                    }
                }
            }
            if (selperiod == 0)
            {
                lblpoperr.Text = "Please Select Any One Period";
                popupdiv.Visible = true;
                return;
            }
            //DateValidation();
            if (txtFromDate.Text.Trim() != "")
            {
                isfromDate = DateTime.TryParseExact(txtFromDate.Text, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFrom);
                dtDummyFrom = dtFrom;
            }
            else
            {
                lblpoperr.Text = "Please Select From Date";
                popupdiv.Visible = true;
                return;
            }
            if (txtToDate.Text.Trim() != "")
            {
                isToDate = DateTime.TryParseExact(txtToDate.Text, "dd/MM/yyyy", null, DateTimeStyles.None, out dtTo);
            }
            else
            {
                lblpoperr.Text = "Please Select To Date";
                popupdiv.Visible = true;
                return;
            }
            if (!isfromDate)
            {
                lblpoperr.Text = "From Date Must Be In The Format dd/MM/yyyy Only!";
                popupdiv.Visible = true;
                return;
            }
            if (!isToDate)
            {
                lblpoperr.Text = "To Date Must Be In The Format dd/MM/yyyy Only!";
                popupdiv.Visible = true;
                return;
            }
            if (selbatchcount != 0 && degselcount != 0 && selperiod != 0 && collegecode != "" && collegecode != null && batch_year != "" && degree_code != "" && period != "" && isfromDate && isToDate)
            {
                hat.Clear();
                hat.Add("colege_code", collegecode);
                dsLeavecode = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                qry = "select degree_code,No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,semester from PeriodAttndSchedule where degree_code in(" + degree_code + ")";
                dsAtt = d2.select_method_wo_parameter(qry, "Text");
                if (dsLeavecode.Tables.Count > 0 && dsLeavecode.Tables[0].Rows.Count > 0)
                {
                    dsLeavecode.Tables[0].DefaultView.RowFilter = "CalcFlag=1";
                    dvlc = dsLeavecode.Tables[0].DefaultView;
                    if (dvlc.Count > 0)
                    {
                        for (int lc = 0; lc < dvlc.Count; lc++)
                        {
                            if (LeaveCodes == "")
                            {
                                LeaveCodes = Convert.ToString(dvlc[lc]["LeaveCode"]);
                            }
                            else
                            {
                                LeaveCodes += "," + Convert.ToString(dvlc[lc]["LeaveCode"]);
                            }
                        }
                    }
                    else
                    {
                        LeaveCodes = "2";
                    }
                }
                else
                {
                    LeaveCodes = "2";
                }
                string strdeptrightsquery = "select u.user_code,u.User_id username,u.Full_Name,(select staff_name from staffmaster sm where sm.staff_code=u.staff_code) staff_name,ar.batch_year,dp.degree_code,ar.sections from UserMaster u,DeptPrivilages dp,tbl_attendance_rights ar where u.User_code=dp.user_code and ar.user_id=u.User_code";
                strdeptrightsquery = strdeptrightsquery + " union ";
                strdeptrightsquery = strdeptrightsquery + " select u.user_code,u.User_id username,u.Full_Name,(select staff_name from staffmaster sm where sm.staff_code=u.staff_code) staff_name,ar.batch_year,dp.degree_code,ar.sections from UserMaster u,DeptPrivilages dp,tbl_attendance_rights ar where Convert(nvarchar(15),replace(u.group_code,';',''))=Convert(nvarchar(15),dp.group_code) and Convert(nvarchar(15),ar.user_id)=Convert(nvarchar(15),replace(u.group_code,';',''))  order by username";
                DataSet dsval = d2.select_method_wo_parameter(strdeptrightsquery, "Text");
                qry = "select distinct r.Batch_Year,c.Course_Id,dg.Degree_Code,r.Current_Semester,c.Course_Name,dt.Dept_Name,isnull(r.Sections,'') as sections,c.Priority,r.college_code from Registration r,Course c,Degree dg,Department dt where c.college_code=r.college_code and r.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=c.college_code and r.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and r.Batch_Year in(" + batch_year + ") and r.degree_code in(" + degree_code + ") and r.college_code='" + collegecode + "' and CC=0 and DelFlag=0 and Exam_Flag<>'debar' order by  r.college_code,r.Batch_Year desc,c.Priority,dg.Degree_Code,r.Current_Semester,sections";
                ds.Clear();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Init_Spread();
                    while (dtDummyFrom <= dtTo)
                    {
                        if (dtDummyFrom.DayOfWeek != DayOfWeek.Sunday)
                        {
                            int totperiod = 0;
                            int startcol = FpPostedHr.Sheets[0].ColumnCount++;
                            foreach (ListItem li in cblPeriod.Items)
                            {
                                if (li.Selected)
                                {
                                    if (totperiod == 0)
                                    {
                                        FpPostedHr.Sheets[0].ColumnHeader.Cells[0, FpPostedHr.Sheets[0].ColumnCount - 1].Text = dtDummyFrom.ToString("dd/MM/yyyy");
                                        //FpPostedHr.Sheets[0].ColumnHeader.Cells[0, FpPostedHr.Sheets[0].ColumnCount - 1].Tag = li.Value;
                                        FpPostedHr.Sheets[0].ColumnHeader.Cells[0, FpPostedHr.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpPostedHr.Sheets[0].ColumnHeader.Cells[0, FpPostedHr.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpPostedHr.Sheets[0].ColumnHeader.Cells[0, FpPostedHr.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                        FpPostedHr.Sheets[0].Columns[FpPostedHr.Sheets[0].ColumnCount - 1].Width = Convert.ToString(dtDummyFrom.ToString("dd/MM/yyyy")).Length * 10;
                                        FpPostedHr.Sheets[0].Columns[FpPostedHr.Sheets[0].ColumnCount - 1].Locked = true;
                                    }
                                    else
                                    {
                                        FpPostedHr.Sheets[0].ColumnCount++;
                                    }
                                    totperiod++;
                                    //FpPostedHr.Sheets[0].ColumnHeader.Cells[1, FpPostedHr.Sheets[0].ColumnCount - 1].Note = li.Value;
                                    FpPostedHr.Sheets[0].ColumnHeader.Cells[1, FpPostedHr.Sheets[0].ColumnCount - 1].Text = ToOrdinal(li.Value) + " Hour";
                                    FpPostedHr.Sheets[0].ColumnHeader.Cells[1, FpPostedHr.Sheets[0].ColumnCount - 1].Tag = dtDummyFrom.ToString("dd/MM/yyyy");
                                    int month = dtDummyFrom.Month;
                                    int year = dtDummyFrom.Year;
                                    int monthyear = (year * 12) + month;
                                    FpPostedHr.Sheets[0].ColumnHeader.Cells[1, FpPostedHr.Sheets[0].ColumnCount - 1].Note = Convert.ToString(monthyear);
                                    //year = dtDummyFrom.Year;
                                    //month = dtDummyFrom.Month;
                                    FpPostedHr.Sheets[0].ColumnHeader.Cells[1, FpPostedHr.Sheets[0].ColumnCount - 1].Locked = true;
                                    FpPostedHr.Sheets[0].ColumnHeader.Cells[1, FpPostedHr.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpPostedHr.Sheets[0].ColumnHeader.Cells[1, FpPostedHr.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                    FpPostedHr.Sheets[0].Columns[FpPostedHr.Sheets[0].ColumnCount - 1].Width = Convert.ToString(ToOrdinal(li.Value) + " Hour").Length * 10;
                                    FpPostedHr.Sheets[0].Columns[FpPostedHr.Sheets[0].ColumnCount - 1].Locked = true;
                                }
                            }
                            FpPostedHr.Sheets[0].ColumnHeaderSpanModel.Add(0, startcol, 1, totperiod);
                        }
                        dtDummyFrom = dtDummyFrom.AddDays(1);
                    }
                    int co = 0;
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            co++;
                            bool notpostedchk = false;

                            #region All posted

                            FpPostedHr.Sheets[0].RowCount++;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 0].Locked = true;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                            //Batch_Year  Degree_Code  Sections   dtDummyFrom.ToString("dd/MM/yyyy");
                            string Sem = Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]);
                            string deg_code = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                            string batch = Convert.ToString(ds.Tables[0].Rows[row]["Batch_Year"]);
                            int yearval = (Convert.ToInt32(Sem) + 1) / 2;
                            string degCourse = ToRoman(Convert.ToString(yearval)) + " - " + Convert.ToString(ds.Tables[0].Rows[row]["Course_Name"]) + " - " + Convert.ToString(ds.Tables[0].Rows[row]["Dept_Name"]);
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(degCourse);
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(batch);
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(deg_code);
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 1].Locked = true;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            string sec = Convert.ToString(ds.Tables[0].Rows[row]["Sections"]);
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(sec);
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 2].Locked = true;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            string username = string.Empty;
                            dsval.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batch + "' and Degree_Code='" + deg_code + "' ";
                            DataView dvrighst = dsval.Tables[0].DefaultView;
                            for (int st = 0; st < dvrighst.Count; st++)
                            {
                                string bname = dvrighst[st]["username"].ToString();
                                string secrights = dvrighst[st]["sections"].ToString();
                                Boolean valfle = true;
                                if (sec.Trim() != "")
                                {
                                    valfle = false;
                                    if (secrights.Trim().ToLower().Contains(sec.Trim().ToLower()))
                                    {
                                        valfle = true;
                                    }
                                }
                                if (valfle == true)
                                {
                                    if (username.Trim() == "")
                                    {
                                        username = bname;
                                    }
                                    else
                                    {
                                        username = username + ", " + bname;
                                    }
                                }
                            }
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(username);
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 3].Locked = true;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            string noofhr = string.Empty;
                            string Isthalf = string.Empty;
                            string IIndhalf = string.Empty;
                            int no_of_hr = 0;
                            int frsthalf = 0;
                            int sndhalf = 0;
                            if (dsAtt.Tables.Count > 0 && dsAtt.Tables[0].Rows.Count > 0)
                            {
                                dsAtt.Tables[0].DefaultView.RowFilter = "degree_code='" + deg_code + "' and semester='" + Sem + "'";
                                dvAtt = dsAtt.Tables[0].DefaultView;
                                if (dvAtt.Count > 0)
                                {
                                    noofhr = Convert.ToString(dvAtt[0]["No_of_hrs_per_day"]);
                                    Isthalf = Convert.ToString(dvAtt[0]["no_of_hrs_I_half_day"]);
                                    IIndhalf = Convert.ToString(dvAtt[0]["no_of_hrs_II_half_day"]);
                                    int.TryParse(noofhr, out no_of_hr);
                                    int.TryParse(Isthalf, out frsthalf);
                                    int.TryParse(IIndhalf, out sndhalf);
                                }
                            }
                            int count = 0;
                            for (int col = 4; col < FpPostedHr.Sheets[0].ColumnCount; col++)
                            {
                                count++;
                                string date = Convert.ToString(FpPostedHr.Sheets[0].ColumnHeader.Cells[1, col].Tag).Trim();
                                // string combo = Convert.ToString(count) + "-" + date;
                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Tag = Convert.ToString(count);
                                //string day = Convert.ToString(FpPostedHr.Sheets[0].ColumnHeader.Cells[1, col].Value).Trim();
                                // FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Note = dtDummyFrom.ToString("dd/MM/yyyy");
                                string month_year = Convert.ToString(FpPostedHr.Sheets[0].ColumnHeader.Cells[1, col].Note).Trim();
                                string peri = (Convert.ToString(FpPostedHr.Sheets[0].ColumnHeader.Cells[1, col].Text)).Substring(0, 1).Trim();
                                DataSet dsAttType = new DataSet();
                                DateTime dtentry = new DateTime();
                                bool isvalid = DateTime.TryParseExact(date, "dd/MM/yyyy", null, DateTimeStyles.None, out dtentry);
                                int day = dtentry.Day;
                                string daycol = "d" + day + "d" + peri;
                                bool isFullholiday = false;
                                bool isHoliMorn = false;
                                bool isHoliEven = false;
                                int fsthalf = 0;
                                isholidayCheck(collegecode, deg_code, Sem, dtentry.ToString("dd/MM/yyyy"), out isFullholiday, out isHoliMorn, out isHoliEven, out fsthalf);
                                string setHoliday = string.Empty;
                                bool isworkingday = false;
                                bool isholiday = false;
                                int hrs = 0;
                                int.TryParse(peri, out hrs);
                                if (isFullholiday && isHoliMorn && isHoliEven)
                                {
                                    if (hrs <= no_of_hr)
                                        isholiday = true;
                                    else
                                    {
                                        isholiday = false;
                                        isworkingday = false;
                                    }
                                }
                                else if (!isFullholiday && !isHoliMorn && isHoliEven)
                                {
                                    if (hrs <= frsthalf)
                                    {
                                        isworkingday = true;
                                    }
                                    else if (hrs <= no_of_hr)
                                    {
                                        isholiday = true;
                                    }
                                    else
                                    {
                                        isworkingday = false;
                                        isholiday = false;
                                    }
                                }
                                else if (!isFullholiday && isHoliMorn && !isHoliEven)
                                {
                                    if (hrs > frsthalf && hrs <= no_of_hr)
                                    {
                                        isworkingday = true;
                                    }
                                    else if (hrs <= no_of_hr)
                                    {
                                        isholiday = true;
                                    }
                                    else
                                    {
                                        isworkingday = false;
                                        isholiday = false;
                                    }
                                }
                                else if (!isFullholiday && !isHoliMorn && !isHoliEven)
                                {
                                    if (hrs <= no_of_hr)
                                        isworkingday = true;
                                    else
                                    {
                                        isholiday = false;
                                        isworkingday = false;
                                    }
                                }
                                string newqry = "select * from tbl_spl_attendace where college_code='" + collegecode + "' and attype=0 and batch_year='" + Convert.ToString(batch) + "' and degree_code='" + Convert.ToString(deg_code) + "' and semester='" + Sem + "' and section='" + sec + "' and entry_date='" + dtentry + "' and period='" + peri + "' ; select * from tbl_spl_attendace where college_code='" + collegecode + "' and attype=1 and batch_year='" + Convert.ToString(batch) + "' and degree_code='" + Convert.ToString(deg_code) + "' and semester='" + Sem + "' and section='" + sec + "' and entry_date='" + dtentry + "' and period='" + peri + "'";
                                dsAttType = d2.select_method_wo_parameter(newqry, "Text");
                                if (dsAttType.Tables.Count > 0)
                                {
                                    if (isholiday)
                                    {
                                        FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Holiday");
                                        FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Note = "H";
                                        FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].BackColor = Color.Gray;
                                        FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].ForeColor = Color.White;
                                        notpostedchk = true;
                                    }
                                    else if (isworkingday)
                                    {
                                        if (dsAttType.Tables.Count > 0 && dsAttType.Tables[0].Rows.Count > 0)
                                        {
                                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Free Hour");
                                            int width = Convert.ToString("Free Hour").Length * 10;
                                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].BackColor = Color.Blue;
                                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].ForeColor = Color.White;
                                            //FpPostedHr.Sheets[0].LockBackColor = Color.Blue;
                                            //FpPostedHr.Sheets[0].LockForeColor = Color.White;
                                            FpPostedHr.Sheets[0].SelectionForeColor = Color.Black;
                                            notpostedchk = true;
                                        }
                                        else if (dsAttType.Tables.Count > 1 && dsAttType.Tables[1].Rows.Count > 0)
                                        {
                                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Special Day");
                                            int width = Convert.ToString("Special Day").Length * 10;
                                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].BackColor = Color.Maroon;
                                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].ForeColor = Color.White;
                                            //FpPostedHr.Sheets[0].LockBackColor = Color.Maroon;
                                            //FpPostedHr.Sheets[0].LockForeColor = Color.White;
                                            notpostedchk = true;
                                        }
                                        else
                                        {
                                            int tot_stud = 0;
                                            int att_marked = 0;
                                            int stud_absend = 0;
                                            string totstud = d2.GetFunctionv("select count(r.roll_no) as Total_Students from Registration r where r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Adm_Date<='" + dtentry + "' and r.college_code='" + collegecode + "' and r.current_semester='" + Sem + "' and r.Batch_Year='" + Convert.ToString(batch) + "' and r.degree_code='" + Convert.ToString(deg_code) + "' and isnull(r.Sections,'')='" + sec + "' ;");
                                            int.TryParse(totstud, out tot_stud);
                                            string marked = d2.GetFunctionv("select Count(a.roll_no) as Marked from attendance a,Registration r where a.roll_no=r.roll_no and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and isnull(a." + daycol + ",'')<>'' and a." + daycol + "<>'0' and a.month_year='" + month_year + "' and r.Adm_Date<='" + dtentry + "' and r.college_code='" + collegecode + "' and r.Batch_Year='" + Convert.ToString(batch) + "' and r.current_semester='" + Sem + "' and r.degree_code='" + Convert.ToString(deg_code) + "' and isnull(r.Sections,'')='" + sec + "';");
                                            int.TryParse(marked, out att_marked);
                                            string absentstud = d2.GetFunctionv("select Count(a.roll_no) as Absent_Marked from attendance a,Registration r where a.roll_no=r.roll_no and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and a.month_year='" + month_year + "' and a." + daycol + " in(" + LeaveCodes + ")  and r.Adm_Date<='" + dtentry + "' and r.college_code='" + collegecode + "' and r.Batch_Year='" + Convert.ToString(batch) + "' and r.current_semester='" + Sem + "' and r.degree_code='" + Convert.ToString(deg_code) + "' and isnull(r.Sections,'')='" + sec + "'");
                                            int.TryParse(absentstud, out stud_absend);
                                            if (att_marked == tot_stud)
                                            {
                                                if (stud_absend == 0)
                                                {
                                                    FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Nil Absent");
                                                    FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].BackColor = Color.DeepPink;
                                                    FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].ForeColor = Color.White;
                                                    notpostedchk = true;
                                                }
                                                else
                                                {
                                                    FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Posted");
                                                    FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].BackColor = Color.Green;
                                                    FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].ForeColor = Color.White;
                                                    notpostedchk = true;
                                                }
                                            }
                                            else if (att_marked != tot_stud)
                                            {
                                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Not Posted");
                                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].BackColor = Color.Red;
                                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].ForeColor = Color.White;
                                            }
                                            else
                                            {
                                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Posted");
                                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].BackColor = Color.Green;
                                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].ForeColor = Color.White;
                                                notpostedchk = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Not Applicable Hour");
                                        FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Note = "N";
                                        FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].BackColor = Color.MediumPurple;
                                        FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].ForeColor = Color.White;
                                        notpostedchk = true;
                                    }
                                }
                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Locked = true;
                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, col].VerticalAlign = VerticalAlign.Middle;
                                Session["count"] = count;
                            }
                            if (cb_notposted.Checked == true)
                            {
                                if (notpostedchk == true)
                                {
                                    FpPostedHr.Sheets[0].Rows[FpPostedHr.Sheets[0].RowCount - 1].Visible = false;
                                    co--;
                                }
                                else
                                {
                                    //FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row - 1);
                                }
                            }
                            FpPostedHr.Sheets[0].Cells[FpPostedHr.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(co);

                            #endregion
                        }
                    }
                    //int visibleCount = 0;
                    //for (int row = 0; row < FpPostedHr.Sheets[0].RowCount; row++)
                    //{
                    //    if (FpPostedHr.Sheets[0].Rows[row].Visible)
                    //    {
                    //        visibleCount++;
                    //    }
                    //}
                    //if (visibleCount > 0 && FpPostedHr.Sheets[0].RowCount > 0)
                    //{
                    FpPostedHr.Sheets[0].PageSize = FpPostedHr.Sheets[0].RowCount;
                    FpPostedHr.Height = (FpPostedHr.Sheets[0].RowCount * 25) + 50;
                    if ((FpPostedHr.Sheets[0].RowCount * 25) + 50 < 200)
                        FpPostedHr.Height = 450;
                    FpPostedHr.SaveChanges();
                    FpPostedHr.Visible = true;
                    divPostHrSpread.Visible = true;
                    rptprint1.Visible = true;
                    lblpoperr.Text = string.Empty;
                    popupdiv.Visible = false;
                    //}
                    //else
                    //{
                    //    lblErrSearch.Text =string.Empty;
                    //    lblErrSearch.Visible = false;
                    //    rptprint1.Visible = false;
                    //    divPostHrSpread.Visible = false;
                    //    lblpoperr.Text = "No Record(s) Found!";
                    //    popupdiv.Visible = true;
                    //    return;
                    //}
                }
                else
                {
                    lblErrSearch.Text = string.Empty;
                    lblErrSearch.Visible = false;
                    rptprint1.Visible = false;
                    divPostHrSpread.Visible = false;
                    lblpoperr.Text = "No Record(s) Found!";
                    popupdiv.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion GO Button

    #region Popup Close

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblpoperr.Text = string.Empty;
            popupdiv.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Popup Close

    #region Generate Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text;
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpPostedHr.Visible == true)
                {
                    d2.printexcelreport(FpPostedHr, reportname);
                }
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Generate Excel

    #region Print PDF

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string rptheadname = "Posted Hour Report";
            string pagename = "PostedHourReport.aspx";
            //dptname = dptname + "@ " + "Exam Year : " + Convert.ToString(ddlExamyr.SelectedItem) + "@Exam Month : " + Convert.ToString(ddlExamMonth.SelectedItem);
            if (FpPostedHr.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpPostedHr, pagename, rptheadname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Print PDF

    #endregion Button Click

    #region Reused Methods

    public static string ToOrdinal(string num)
    {
        int number = 0;
        bool valid = int.TryParse(num, out number);
        if (valid)
        {
            if (number < 0) return Convert.ToString(number);
            long rem = number % 100;
            if (rem >= 11 && rem <= 13) return number + "th";
            switch (number % 10)
            {
                case 1:
                    return number + "st";
                case 2:
                    return number + "nd";
                case 3:
                    return number + "rd";
                default:
                    return number + "th";
            }
        }
        else
        {
            return num;
        }
    }

    public string ToRoman(string part)
    {
        string roman = string.Empty;
        try
        {
            switch (part)
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
                case "9":
                    roman = "IX";
                    break;
                case "10":
                    roman = "X";
                    break;
                case "11":
                    roman = "XI";
                    break;
                case "12":
                    roman = "XII";
                    break;
            }
        }
        catch (Exception ex)
        {

        }
        return roman;
    }

    public string Toint(string smno)
    {
        string no = string.Empty;
        smno = smno.Trim();
        try
        {
            switch (smno)
            {
                case "I":
                    no = "1";
                    break;
                case "II":
                    no = "2";
                    break;
                case "III":
                    no = "3";
                    break;
                case "IV":
                    no = "4";
                    break;
                case "V":
                    no = "5";
                    break;
                case "VI":
                    no = "6";
                    break;
                case "VII":
                    no = "7";
                    break;
                case "VIII":
                    no = "8";
                    break;
                case "IX":
                    no = "9";
                    break;
                case "X":
                    no = "10";
                    break;
                case "XI":
                    no = "11";
                    break;
                case "XII":
                    no = "12";
                    break;
            }
        }
        catch (Exception ex)
        {

        }
        return no;
    }

    public int getyear(string sem)
    {
        try
        {
            int newsem = 0;
            bool valid = int.TryParse(sem, out newsem);
            int year = 0;
            if (valid)
            {
                if (newsem % 2 == 0)
                {
                    year = newsem / 2;
                }
                else
                {
                    year = (newsem + 1) / 2;
                }
            }
            return year;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            return -1;
        }
    }

    public string get_period(string perd)
    {
        string p = string.Empty;
        try
        {
            switch (perd)
            {
                case "1st":
                    p = "1";
                    break;
                case "2nd":
                    p = "2";
                    break;
                case "3rd":
                    p = "3";
                    break;
                case "4th":
                    p = "4";
                    break;
                case "5th":
                    p = "5";
                    break;
                case "6th":
                    p = "6";
                    break;
                case "7th":
                    p = "7";
                    break;
                case "8th":
                    p = "8";
                    break;
                case "9th":
                    p = "9";
                    break;
            }
        }
        catch
        {

        }
        return p;
    }

    public void isholidayCheck(string college_code, string degree_code, string semester, string frdate, out bool ishoilday, out bool isholimorn, out bool isholieven, out int fhrs)
    {
        Hashtable holiday_table = new Hashtable();
        DataSet ds2 = new DataSet();
        DataSet ds_holi = new DataSet();
        DateTime dumm_from_date = new DateTime();
        string[] dsplit = frdate.Split(new Char[] { '/' });
        frdate = Convert.ToString(dsplit[2]) + "/" + Convert.ToString(dsplit[1]) + "/" + Convert.ToString(dsplit[0]);
        dumm_from_date = Convert.ToDateTime(frdate);
        ishoilday = false;
        isholimorn = false;
        isholieven = false;
        fhrs = 0;
        try
        {
            hat.Clear();
            hat.Add("degree_code", degree_code);
            hat.Add("sem", semester);
            hat.Add("from_date", frdate);
            hat.Add("to_date", frdate);
            hat.Add("coll_code", college_code);
            int iscount = 0;
            string strquery = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate + "' and '" + frdate + "' and degree_code=" + degree_code + " and semester=" + semester + "";
            ds2.Reset();
            ds2.Dispose();
            ds2 = d2.select_method(strquery, hat, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                iscount = 0;
                int.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["cnt"]), out iscount);
            }
            hat.Add("iscount", iscount);
            ds_holi = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
            holiday_table.Clear();
            if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count != 0)
            {
                for (int k = 0; k < ds_holi.Tables[0].Rows.Count; k++)
                {
                    if (!holiday_table.ContainsKey(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[0].Rows[k]["HOLI_DATE"]))))
                    {
                        holiday_table.Add(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[0].Rows[k]["HOLI_DATE"])), k);
                    }
                }
            }
            if (ds_holi.Tables.Count > 1 && ds_holi.Tables[1].Rows.Count != 0)
            {
                for (int k = 0; k < ds_holi.Tables[1].Rows.Count; k++)
                {
                    if (!holiday_table.ContainsKey(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[1].Rows[k]["HOLI_DATE"]))))
                    {
                        holiday_table.Add(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[1].Rows[k]["HOLI_DATE"])), k);
                    }
                }
            }
            if (ds_holi.Tables.Count > 2 && ds_holi.Tables[2].Rows.Count != 0)
            {
                for (int k = 0; k < ds_holi.Tables[2].Rows.Count; k++)
                {
                    if (!holiday_table.ContainsKey(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[2].Rows[k]["HOLI_DATE"]))))
                    {
                        holiday_table.Add(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[2].Rows[k]["HOLI_DATE"])), k);
                    }
                }
            }
            fhrs = 0;
            string hrs = d2.GetFunction("select no_of_hrs_I_half_day from periodattndschedule where degree_code=" + degree_code + " and semester='" + semester + "'");
            if (hrs.Trim() != "" && hrs != null && hrs.Trim() != "0")
            {
                int.TryParse(hrs, out fhrs);
            }
            if (!holiday_table.ContainsKey(dumm_from_date))
            {
                ishoilday = false;
                isholimorn = false;
                isholieven = false;
            }
            else
            {
                ishoilday = true;
                isholimorn = false;
                isholieven = false;
                int starthout = 0;
                string strholyquery = "select halforfull,morning,evening from holidaystudents where halforfull=1 and holiday_date='" + dumm_from_date.ToString("MM/dd/yyyy") + "'";
                DataSet dsholidayval = d2.select_method_wo_parameter(strholyquery, "Text");
                if (dsholidayval.Tables.Count > 0 && dsholidayval.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(dsholidayval.Tables[0].Rows[0]["morning"]) == "1" || Convert.ToString(dsholidayval.Tables[0].Rows[0]["morning"]).Trim().ToLower() == "true")
                    {
                        ishoilday = false;
                        isholimorn = true;
                        isholieven = false;
                    }
                    if (Convert.ToString(dsholidayval.Tables[0].Rows[0]["evening"]) == "1" || Convert.ToString(dsholidayval.Tables[0].Rows[0]["evening"]).Trim().ToLower() == "true")
                    {
                        isholimorn = false;
                        ishoilday = false;
                        isholieven = true;
                    }
                }
                else
                {
                    ishoilday = true;
                    isholimorn = true;
                    isholieven = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    #endregion Reused Methods

    #region Cell Click

    protected void Cell1_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {
        }
    }

    #endregion

    #region Spread Render

    protected void FpPostedHr_OnPreRender(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                string selectedtext = string.Empty;
                string det = string.Empty;
                string batch = string.Empty;
                string degreecode = string.Empty;
                string sem = string.Empty;
                string sec = string.Empty;
                string d1 = string.Empty;
                string cond = string.Empty;
                string d = string.Empty;
                string activerow = string.Empty;
                string activecol = string.Empty;
                string date = string.Empty;
                string calcflag = string.Empty;
                string attcode = string.Empty;
                string attendance = string.Empty;
                string present = string.Empty;
                int fd1 = 0, fmm = 0, fyy = 0, fmonthyear = 0, sno1 = 0, sno = 0;
                bool test = false, checkempty = false;
                DateTime dt = new DateTime();
                activerow = FpPostedHr.ActiveSheetView.ActiveRow.ToString();
                activecol = FpPostedHr.ActiveSheetView.ActiveColumn.ToString();
                //FpPostedHr.Sheets[0].a
                if (activerow.Trim() != "")
                {
                    FpPostedHr.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].ForeColor = Color.Black;
                    selectedtext = Convert.ToString(FpPostedHr.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Tag);
                    det = Convert.ToString(FpPostedHr.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    batch = Convert.ToString(FpPostedHr.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    degreecode = Convert.ToString(FpPostedHr.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note);
                    sec = Convert.ToString(FpPostedHr.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    if (activecol.Trim() != "")
                    {
                        d1 = Convert.ToString(FpPostedHr.Sheets[0].ColumnHeader.Cells[1, Convert.ToInt32(activecol)].Value);
                        if (d1.Trim() != "")
                        {
                            string[] psplit = d1.Split(' ');
                            string period = psplit[0];
                            d = get_period(period);
                        }
                        cond = Convert.ToString(FpPostedHr.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Note);
                        date = Convert.ToString(FpPostedHr.Sheets[0].ColumnHeader.Cells[1, Convert.ToInt32(activecol)].Tag);
                    }
                }
                test = DateTime.TryParseExact(date, "dd/MM/yyyy", null, DateTimeStyles.None, out dt);
                if (test == true)
                {
                    fd1 = dt.Day;
                    fmm = dt.Month;
                    fyy = dt.Year;
                    fmonthyear = (fyy * 12) + fmm;
                }
                if (cond.Trim() != "")
                {
                    if (cond == "H")
                    {
                        lblpoperr.Text = "Selected Period Is Holiday !!!";
                        popupdiv.Visible = true;
                        FpSpread1.Visible = false;
                        return;
                    }
                    if (cond == "N")
                    {
                        lblpoperr.Text = "Selected Period Is Not Applicable Period !!!";
                        popupdiv.Visible = true;
                        FpSpread1.Visible = false;
                        return;
                    }
                }
                if (det.Trim() != "")
                {
                    string[] getsem = det.Split('-');
                    string semno = getsem[0];
                    sem = Toint(semno);
                }
                Dictionary<string, string> dicattvalue = new Dictionary<string, string>();
                string getleavecode = "select LeaveCode,CalcFlag  from AttMasterSetting where CollegeCode='" + collegecode + "'";
                ds1 = d2.select_method_wo_parameter(getleavecode, "Text");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int sk = 0; sk < ds1.Tables[0].Rows.Count; sk++)
                    {
                        calcflag = ds1.Tables[0].Rows[sk]["CalcFlag"].ToString();
                        attcode = ds1.Tables[0].Rows[sk]["LeaveCode"].ToString();
                        if (!dicattvalue.ContainsKey(attcode))
                        {
                            dicattvalue.Add(attcode, calcflag);
                        }
                    }
                    FpSpread1.Visible = false;
                    FarPoint.Web.Spread.NamedStyle fontblue = new FarPoint.Web.Spread.NamedStyle("blue");
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                    FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                    FpSpread1.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
                    FpSpread1.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
                    FpSpread1.Sheets[0].SheetCorner.RowCount = 10;
                    FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                    style.Font.Size = 12;
                    style.Font.Bold = true;
                    style.HorizontalAlign = HorizontalAlign.Center;
                    style.ForeColor = Color.Black;
                    FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].AllowTableCorner = true;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0080ff");
                    darkstyle.Font.Name = "Book Antiqua";
                    darkstyle.Font.Size = 11;
                    darkstyle.Font.Bold = true;
                    darkstyle.Border.BorderSize = 1;
                    darkstyle.HorizontalAlign = HorizontalAlign.Center;
                    darkstyle.VerticalAlign = VerticalAlign.Middle;
                    darkstyle.Border.BorderColor = System.Drawing.Color.Black;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FpSpread1.Sheets[0].AutoPostBack = false;
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                    FpSpread1.Sheets[0].ColumnCount = 10;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.CommandBar.Visible = false;

                    FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Presented Students";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Presented Students";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Presented Students";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Presented Students";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 4);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Text = "Student Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Reg No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Admission No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Roll No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Absented Students";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 4);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Student Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Reg No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Admission No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Roll No";
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[0].Width = 50;
                    FpSpread1.Sheets[0].Columns[5].Width = 50;
                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[2].Width = 120;
                    FpSpread1.Sheets[0].Columns[7].Width = 120;
                    FpSpread1.Sheets[0].Columns[1].Width = 180;
                    FpSpread1.Sheets[0].Columns[6].Width = 180;

                    FpSpread1.Sheets[0].Columns[0].CellType = txtCell;
                    FpSpread1.Sheets[0].Columns[1].CellType = txtCell;
                    FpSpread1.Sheets[0].Columns[2].CellType = txtCell;
                    FpSpread1.Sheets[0].Columns[3].CellType = txtCell;
                    FpSpread1.Sheets[0].Columns[4].CellType = txtCell;
                    FpSpread1.Sheets[0].Columns[5].CellType = txtCell;
                    FpSpread1.Sheets[0].Columns[6].CellType = txtCell;
                    FpSpread1.Sheets[0].Columns[7].CellType = txtCell;
                    FpSpread1.Sheets[0].Columns[8].CellType = txtCell;
                    FpSpread1.Sheets[0].Columns[9].CellType = txtCell;

                    FpSpread1.Sheets[0].Columns[0].Locked = true;
                    FpSpread1.Sheets[0].Columns[1].Locked = true;
                    FpSpread1.Sheets[0].Columns[2].Locked = true;
                    FpSpread1.Sheets[0].Columns[3].Locked = true;
                    FpSpread1.Sheets[0].Columns[4].Locked = true;
                    FpSpread1.Sheets[0].Columns[5].Locked = true;
                    FpSpread1.Sheets[0].Columns[6].Locked = true;
                    FpSpread1.Sheets[0].Columns[7].Locked = true;
                    FpSpread1.Sheets[0].Columns[8].Locked = true;
                    FpSpread1.Sheets[0].Columns[9].Locked = true;

                    int spancount = 0;
                    if (Session["Rollflag"].ToString() == "1")
                    {
                        FpSpread1.Sheets[0].Columns[4].Visible = true;
                        FpSpread1.Sheets[0].Columns[9].Visible = true;
                        spancount++;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Columns[4].Visible = false;
                        FpSpread1.Sheets[0].Columns[9].Visible = false;
                    }
                    if (Session["Regflag"].ToString() == "1")
                    {
                        FpSpread1.Sheets[0].Columns[2].Visible = true;
                        FpSpread1.Sheets[0].Columns[7].Visible = true;
                        spancount++;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Columns[2].Visible = false;
                        FpSpread1.Sheets[0].Columns[7].Visible = false;
                    }
                    if (Session["Admissionno"].ToString() == "1")
                    {
                        FpSpread1.Sheets[0].Columns[3].Visible = true;
                        FpSpread1.Sheets[0].Columns[8].Visible = true;
                        spancount++;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Columns[3].Visible = false;
                        FpSpread1.Sheets[0].Columns[8].Visible = false;
                    }
                    if (Session["Rollflag"].ToString() == "1" && Session["Regflag"].ToString() == "1" && Session["Admissionno"].ToString() == "1")
                    {
                        FpSpread1.Width = 950;
                    }
                    else if ((Session["Rollflag"].ToString() == "1" && Session["Regflag"].ToString() == "1") || (Session["Admissionno"].ToString() == "1" && Session["Rollflag"].ToString() == "1") || (Session["Regflag"].ToString() == "1" && Session["Admissionno"].ToString() == "1"))
                    {
                        FpSpread1.Width = 900;
                    }
                    else if (Session["Rollflag"].ToString() == "1" || Session["Regflag"].ToString() == "1" || Session["Admissionno"].ToString() == "1")
                    {
                        FpSpread1.Width = 650;
                    }
                    string getquery = "select r.Roll_Admit, r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno,(a.d" + fd1 + "d" + d + ") as attn from Registration r,attendance a where r.Roll_No=a.roll_no and a.month_year='" + fmonthyear + "' and r.Batch_Year='" + batch + "' and r.degree_code='" + degreecode + "'   and ltrim(rtrim(isnull(r.sections,'')))='" + sec + "' and cc=0 and exam_flag <> 'debar' and delflag=0";
                  
                    //and r.Current_Semester='" + sem + "'
                    ds = d2.select_method_wo_parameter(getquery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int presentrow = 0;
                        int absentrow = 0;
                        for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                        {
                            string rollno = "", regno = "", sname = "", roll_no = "", reg_no = "", s_name;
                            string admissionno = string.Empty;
                            attendance = ds.Tables[0].Rows[sk]["attn"].ToString();
                            if (dicattvalue.ContainsKey(attendance))
                            {
                                present = dicattvalue[attendance].ToString();
                                if (present == "0")
                                {
                                    if (FpSpread1.Sheets[0].RowCount == presentrow)
                                        FpSpread1.Sheets[0].RowCount++;
                                    sno++;
                                    rollno = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                    regno = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                    sname = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                    admissionno = ds.Tables[0].Rows[sk]["Roll_Admit"].ToString();
                                    checkempty = true;
                                    FpSpread1.Sheets[0].Cells[presentrow, 0].Text = Convert.ToString(sno);
                                    FpSpread1.Sheets[0].Cells[presentrow, 1].Text = sname;
                                    FpSpread1.Sheets[0].Cells[presentrow, 2].Text = regno;
                                    FpSpread1.Sheets[0].Cells[presentrow, 3].Text = admissionno;
                                    FpSpread1.Sheets[0].Cells[presentrow, 4].Text = rollno;
                                    presentrow++;
                                }
                                else if (present == "1")
                                {
                                    if (FpSpread1.Sheets[0].RowCount == absentrow)
                                        FpSpread1.Sheets[0].RowCount++;
                                    sno1++;
                                    roll_no = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                    reg_no = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                    s_name = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                    admissionno = ds.Tables[0].Rows[sk]["Roll_Admit"].ToString();
                                    checkempty = true;
                                    FpSpread1.Sheets[0].Cells[absentrow, 5].Text = Convert.ToString(sno1);
                                    FpSpread1.Sheets[0].Cells[absentrow, 6].Text = s_name;
                                    FpSpread1.Sheets[0].Cells[absentrow, 6].CellType = txtCell;
                                    FpSpread1.Sheets[0].Cells[absentrow, 6].Locked = true;
                                    FpSpread1.Sheets[0].Cells[absentrow, 7].Text = reg_no;
                                    FpSpread1.Sheets[0].Cells[absentrow, 7].CellType = txtCell;
                                    FpSpread1.Sheets[0].Cells[absentrow, 6].Locked = true;
                                    FpSpread1.Sheets[0].Cells[absentrow, 8].Text = admissionno;
                                    FpSpread1.Sheets[0].Cells[absentrow, 7].CellType = txtCell;
                                    FpSpread1.Sheets[0].Cells[absentrow, 6].Locked = true;
                                    FpSpread1.Sheets[0].Cells[absentrow, 9].Text = roll_no;
                                    absentrow++;
                                }
                            }
                        }
                        if (checkempty == false)
                        {
                            lblpoperr.Text = "No Records Found !!!";
                            popupdiv.Visible = true;
                            FpSpread1.Visible = false;
                            return;
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Height = (FpSpread1.Sheets[0].RowCount * 25) + 60;
                        if ((FpSpread1.Sheets[0].RowCount * 25) + 50 < 200)
                            FpSpread1.Height = 460;
                        FpSpread1.SaveChanges();
                        FpSpread1.Visible = true;
                    }
                    else
                    {
                        lblpoperr.Text = "No Records Found !!!";
                        popupdiv.Visible = true;
                        FpSpread1.Visible = false;
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

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
        lbl.Add(lblCollege);
        lbl.Add(lblDegree);
        lbl.Add(lblBranch);
        //lbl.Add(Label2);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        //fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    protected void cb_notposted_CheckedChanged(object sender, EventArgs e)
    {
        btnGo_Click(sender, e);
    }

}
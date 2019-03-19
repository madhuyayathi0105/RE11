#region Namespace Declaration

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using BalAccess;
using DalConnection;
using Farpnt = FarPoint.Web.Spread;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Text;

#endregion Namespace Declaration

public partial class AttendanceSplDayFreeHrEntry : System.Web.UI.Page
{

    #region Variable Declaration

    Hashtable hat = new Hashtable();
    string usercode = "", collegecode = "", singleuser = "", group_user = string.Empty;
    string batch_year = "", degree_code = "", semester = "", section = string.Empty;

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();

    string qry = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    Boolean b_school = false;

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
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]);
                if (schoolvalue.Trim() == "0")
                {
                    b_school = true;
                }
            }
            if (!IsPostBack)
            {
                cbViewOrNot.Checked = false;
                cbreason.Checked = false;
                cbreason.Visible = false;
                cbPeriods.Visible = false;
                cbPeriods.Checked = false;
                lblErrSearch.Visible = false;
                btnView.Visible = false;
                Bindcollege();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
                bindsemester();
                BindSectionDetailmult();
                Bindhour();
                loadreason();
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                if (rblSplFree.SelectedValue == "0")
                {
                    lblFromDate.Text = "Date";
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    lblFromDate.Visible = true;
                    txtFromDate.Visible = true;
                    lblPeriod.Visible = true;
                    upnlPeriod.Visible = true;
                    txtPeriod.Visible = true;
                    lblToDate.Visible = false;
                    txtToDate.Visible = false;
                    lblAttBasedon.Visible = true;
                    ddlAttBaseon.Visible = true;
                }
                else
                {
                    lblFromDate.Text = "From Date";
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    lblFromDate.Visible = true;
                    txtFromDate.Visible = true;
                    lblPeriod.Visible = false;
                    upnlPeriod.Visible = false;
                    txtPeriod.Visible = false;
                    lblToDate.Visible = true;
                    txtToDate.Visible = true;
                    lblAttBasedon.Visible = false;
                    ddlAttBaseon.Visible = false;
                }
                semstartend();

                ItemList.Clear();
                Itemindex.Clear();
                ddlpurpose.Attributes.Add("onfocus", "frelig()");
                divViewSpread.Visible = false;
                popupdiv.Visible = false;
                rptprint1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Page Load

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
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {

                ddlCollege.DataSource = dsprint;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
            else
            {
                lblErrSearch.Text = "Set college rights to the staff";
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
            lblErrSearch.Visible = false;
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
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
            lblErrSearch.Visible = false;
            int count = 0;
            cblDegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
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
            lblErrSearch.Visible = false;
            int count = 0;
            collegecode = Convert.ToString(ddlCollege.SelectedValue); ;
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                if (cblDegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + Convert.ToString(cblDegree.Items[i].Value) + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + Convert.ToString(cblDegree.Items[i].Value) + "";
                    }
                }
            }
            cblBranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
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

    public void bindsemester()
    {
        try
        {
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string strquery = string.Empty;
            string strbranch = string.Empty;
            for (int b = 0; b < cblBranch.Items.Count; b++)
            {
                if (cblBranch.Items[b].Selected == true)
                {
                    if (strbranch.Trim() == "")
                    {
                        strbranch = "'" + cblBranch.Items[b].Value + "'";
                    }
                    else
                    {
                        strbranch += ",'" + cblBranch.Items[b].Value + "'";
                    }
                }
            }
            if (strbranch.Trim() != "")
            {
                strbranch = " and degree_code in(" + strbranch + ")";
            }

            strquery = "select distinct Max(ndurations) ndurations,first_year_nonsemester from ndegree where college_code=" + Convert.ToString(ddlCollege.SelectedValue) + " and batch_year='" + Convert.ToString(ddlbatch.Text) + "' " + strbranch + " group by first_year_nonsemester order by ndurations desc";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]));
                duration = Convert.ToInt16(Convert.ToString(ds.Tables[0].Rows[0][0]));
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                }
            }
            else
            {
                strquery = "select distinct max(duration) duration,first_year_nonsemester  from degree where college_code='" + Convert.ToString(ddlCollege.SelectedValue) + "' " + strbranch + "  group by first_year_nonsemester order by duration desc";
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]));
                    duration = Convert.ToInt16(Convert.ToString(ds.Tables[0].Rows[0][0]));
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(Convert.ToString(i));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsem.Items.Add(Convert.ToString(i));
                        }
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

    public void BindSectionDetailmult()
    {
        try
        {
            int takecount = 0;
            chklstsection.Items.Clear();
            ds.Dispose();
            ds.Reset();
            txtsection.Text = "---Select---";
            string strdegree = "", strbranch = string.Empty;
            string strbatch = Convert.ToString(ddlbatch.SelectedValue);
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                if (cblDegree.Items[i].Selected == true)
                {
                    if (strdegree == "")
                    {
                        strdegree = "'" + Convert.ToString(cblDegree.Items[i].Value) + "'";
                    }
                    else
                    {
                        strdegree = strdegree + "," + "'" + Convert.ToString(cblDegree.Items[i].Value) + "'";
                    }
                }
            }
            for (int i = 0; i < cblBranch.Items.Count; i++)
            {
                if (cblBranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + Convert.ToString(cblBranch.Items[i].Value) + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + Convert.ToString(cblBranch.Items[i].Value) + "'";
                    }

                }
            }
            int sectioncount = 0;
            if (txtBranch.Text != "---Select---" && strbranch != "" && strbatch != "")
            {
                string strsection = "select distinct sections from registration where batch_year in(" + strbatch + ") and degree_code in(" + strbranch + ") and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
                ds = d2.select_method_wo_parameter(strsection, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    takecount = ds.Tables[0].Rows.Count;
                    chklstsection.DataSource = ds;
                    chklstsection.DataTextField = "sections";
                    chklstsection.DataValueField = "sections";
                    chklstsection.DataBind();
                    chklstsection.Items.Insert(takecount, new ListItem("Empty", "0"));

                    if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                    {
                        txtsection.Text = "---Select---";
                        txtsection.Enabled = false;
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
                else
                {
                    txtsection.Text = "---Select---";
                    chklstsection.Items.Insert(takecount, new ListItem("Empty", "0"));
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
            sqlbatch = Convert.ToString(ddlbatch.SelectedItem);
            if (sqlbatch != "")
            {
                sqlbatch = " in(" + sqlbatch + ")";
                sqlbatchquery = " and si.batch_year  " + sqlbatch + "";
            }
            else
            {
                sqlbatchquery = " ";
            }
            if (txtBranch.Text != "---Select---")
            {
                int itemcount = 0;
                for (itemcount = 0; itemcount < cblBranch.Items.Count; itemcount++)
                {
                    if (cblBranch.Items[itemcount].Selected == true)
                    {
                        if (sqlbranch == "")
                            sqlbranch = "'" + Convert.ToString(cblBranch.Items[itemcount].Value) + "'";
                        else
                            sqlbranch = sqlbranch + "," + "'" + Convert.ToString(cblBranch.Items[itemcount].Value) + "'";
                    }
                }

                if (sqlbranch != "")
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
            ddlAttBaseon.Items.Clear();
            ds.Dispose();
            ds.Reset();
            string qeryss = "select max(pa.No_of_hrs_per_day) as No_of_hrs_per_day from PeriodAttndSchedule pa,seminfo si where pa.degree_code=si.degree_code and pa.semester=si.semester " + sqlbatchquery + " " + sqlbranchquery + " and pa.semester='" + Convert.ToString(ddlsem.SelectedValue) + "'";// and  college_code='" + Convert.ToString(ddlCollege.SelectedValue) + "' 
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
                    ddlAttBaseon.Items.Insert(i - 1, new ListItem(Convert.ToString(i), Convert.ToString(i)));
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
                if (ddlAttBaseon.Items.Count > 0)
                    ddlAttBaseon.SelectedIndex = 0;
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
            string degree_code1 = string.Empty;
            string batch_year1 = string.Empty;
            string semester1 = string.Empty;
            string semstart = string.Empty;
            string semend = string.Empty;
            string val = string.Empty;
            DateTime dtsemstart = new DateTime();
            DateTime dtsemend = new DateTime();
            DateTime dttoDate = new DateTime();
            if (rblSplFree.SelectedValue == "0")
            {
                val = "Date";
            }
            else
            {
                val = "From Date";
            }

            if (ddlbatch.Items.Count > 0)
            {
                batch_year1 = Convert.ToString(ddlbatch.SelectedValue);
            }
            if (ddlsem.Items.Count > 0)
            {
                semester1 = Convert.ToString(ddlsem.SelectedValue);
            }
            if (cblBranch.Items.Count > 0)
            {
                foreach (ListItem li in cblBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (degree_code1 == "")
                        {
                            degree_code1 = li.Value;
                        }
                        else
                        {
                            degree_code1 += "," + li.Value;
                        }
                    }
                }
            }
            if (batch_year1 != "" && degree_code1 != "" && semester1 != "")
            {
                qry = "select distinct Convert(Varchar,start_date,103) as Start_Date from seminfo where batch_year='" + batch_year1 + "' and semester='" + semester1 + "' and degree_code in(" + degree_code1 + ")";
                semstart = d2.GetFunctionv(qry);

                qry = "select distinct Convert(Varchar,end_date,103) as End_Date from seminfo where batch_year='" + batch_year1 + "' and semester='" + semester1 + "' and degree_code in(" + degree_code1 + ")";
                semend = d2.GetFunctionv(qry);

                if (semstart != "" && semend != "")
                {
                    DateTime.TryParseExact(semstart, "dd/MM/yyyy", null, DateTimeStyles.None, out dtsemstart);
                    DateTime.TryParseExact(semend, "dd/MM/yyyy", null, DateTimeStyles.None, out dtsemend);

                    DateTime dtnow = DateTime.Now;
                    string datefad, dtfromad;
                    string datefromad;
                    string yr4, m4, d4;
                    datefad = Convert.ToString(txtFromDate.Text);
                    string[] split4 = datefad.Split(new Char[] { '/' });
                    string toDate = Convert.ToString(txtToDate.Text);
                    string[] splitTo = toDate.Split(new Char[] { '/' });


                    if (split4.Length == 3)
                    {
                        datefromad = Convert.ToString(split4[0]) + "/" + Convert.ToString(split4[1]) + "/" + Convert.ToString(split4[2]);
                        yr4 = Convert.ToString(split4[2]);
                        m4 = Convert.ToString(split4[1]);
                        d4 = Convert.ToString(split4[0]);
                        dtfromad = m4 + "/" + d4 + "/" + yr4;
                        DateTime dt1 = Convert.ToDateTime(dtfromad);
                        if (dt1 > dtnow)
                        {
                            lblErrSearch.Text = val + " Can't Be Greater Than Today Date";
                            lblErrSearch.Visible = true;
                            txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");
                            txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                            return;
                        }
                        if (dt1 < dtsemstart)
                        {
                            lblErrSearch.Text = val + " Can't Be Lesser Than Semester Start Date " + dtsemstart.ToString("dd/MM/yyyy");
                            lblErrSearch.Visible = true;
                            txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                            txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                            return;
                        }

                        if (dt1 > dtsemend)
                        {
                            lblErrSearch.Text = val + " Can't Be Greater Than Semester End Date " + dtsemend.ToString("dd/MM/yyyy");
                            lblErrSearch.Visible = true;
                            txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                            txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                            return;
                        }
                        else
                        {
                            lblErrSearch.Visible = false;
                        }
                        if (rblSplFree.SelectedValue != "0")
                        {
                            if (splitTo.Length == 3)
                            {
                                toDate = Convert.ToString(splitTo[0]) + "/" + Convert.ToString(splitTo[1]) + "/" + Convert.ToString(splitTo[2]);
                                yr4 = Convert.ToString(splitTo[2]);
                                m4 = Convert.ToString(splitTo[1]);
                                d4 = Convert.ToString(splitTo[0]);
                                dtfromad = m4 + "/" + d4 + "/" + yr4;
                                dttoDate = Convert.ToDateTime(dtfromad);
                            }
                            if (dttoDate > dtnow)
                            {
                                lblErrSearch.Text = "To Date Can't Be Greater Than Today Date ";
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy"); ;
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                            if (dt1 > dttoDate)
                            {
                                lblErrSearch.Text = val + " Can't Be Greater Than To Date ";
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy"); ;
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                            if (dttoDate < dt1)
                            {
                                lblErrSearch.Text = "To Date Can't Be Lesser Than From Date ";// + dtsemstart.ToString("dd/MM/yyyy")
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                            if (dttoDate < dtsemstart)
                            {
                                lblErrSearch.Text = "To Date Can't Be Lesser Than Semester Start Date " + dtsemstart.ToString("dd/MM/yyyy");
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }

                            if (dttoDate > dtsemend)
                            {
                                lblErrSearch.Text = "To Date Can't Be Greater Than Semester End Date " + dtsemend.ToString("dd/MM/yyyy");
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    lblErrSearch.Text = "Semester Start Date And End Date Are Not Found";
                    lblErrSearch.Visible = true;
                    return;
                }
            }
            else
            {
                lblErrSearch.Text = "First Select Batch,Degree,Department and Semester And Then Proceed";
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

    public void loadreason()
    {
        try
        {
            ddlpurpose.Items.Clear();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
            string query = "select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code=" + collegecode + "";
            DataSet ds = new DataSet();
            ds.Dispose(); ds.Reset();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlpurpose.DataSource = ds;
                ddlpurpose.DataTextField = "Textval";
                ddlpurpose.DataValueField = "TextCode";
                ddlpurpose.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void semstartend()
    {
        try
        {
            if (ddlbatch.Items.Count > 0)
            {
                batch_year = Convert.ToString(ddlbatch.SelectedValue);
            }
            if (ddlsem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlsem.SelectedValue);
            }
            if (cblBranch.Items.Count > 0)
            {
                foreach (ListItem li in cblBranch.Items)
                {
                    if (li.Selected)
                    {
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
            if (batch_year != "" && degree_code != "" && semester != "")
            {
                qry = "select distinct Convert(Varchar,start_date,103) as Start_Date from seminfo where batch_year='" + batch_year + "' and semester='" + semester + "' and degree_code in(" + degree_code + ") order by Start_Date desc";
                string semstart = d2.GetFunctionv(qry);

                qry = "select distinct Convert(Varchar,end_date,103) as End_Date from seminfo where batch_year='" + batch_year + "' and semester='" + semester + "' and degree_code in(" + degree_code + ") order by End_Date";
                string semend = d2.GetFunctionv(qry);
                DateTime dtsemstart = new DateTime();
                DateTime dtsemend = new DateTime();
                if (semstart != "" && semend != "")
                {

                    //DateTime.TryParseExact(semstart, "dd/MM/yyyy", null, DateTimeStyles.None, out dtsemstart);
                    //DateTime.TryParseExact(semend, "dd/MM/yyyy", null, DateTimeStyles.None, out dtsemend);
                    //txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");
                    //txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy"); // Modify by jairam 03-03-2017 for Jamal college 
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
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
            divViewSpread.Visible = false;
            popupdiv.Visible = false;
            rptprint1.Visible = false;

            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(Session["collegecode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            ddlbatch.Items.Clear();
            cblDegree.Items.Clear();
            cblBranch.Items.Clear();
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
            bindsemester();
            BindSectionDetailmult();
            semstartend();
            Bindhour();
            ViewOrSave();

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divViewSpread.Visible = false;
            popupdiv.Visible = false;
            rptprint1.Visible = false;
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
            bindsemester();
            BindSectionDetailmult();
            semstartend();
            Bindhour();
            ViewOrSave();


        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            divViewSpread.Visible = false;
            popupdiv.Visible = false;
            BindSectionDetailmult();
            semstartend();
            Bindhour();
            ViewOrSave();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlpurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divViewSpread.Visible = false;
            popupdiv.Visible = false;
            rptprint1.Visible = false;
            //ViewOrSave();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    protected void ddlAttBaseon_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion DropDownList Events

    #region CheckBox Events

    protected void cbDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            divViewSpread.Visible = false;
            popupdiv.Visible = false;
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
            bindsemester();
            BindSectionDetailmult();
            Bindhour();
            semstartend();
            ViewOrSave();
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
            divViewSpread.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
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
            bindsemester();
            BindSectionDetailmult();
            Bindhour();
            semstartend();
            ViewOrSave();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void chksection_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            divViewSpread.Visible = false;
            popupdiv.Visible = false;

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
            Bindhour();
            semstartend();
            ViewOrSave();
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
            divViewSpread.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            string hourval = string.Empty;
            if (cbPeriod.Checked == true)
            {
                ItemList.Clear();
                Itemindex.Clear();
                txtPeriod.Text = string.Empty;
                for (int i = 0; i < cblPeriod.Items.Count; i++)
                {
                    cblPeriod.Items[i].Selected = true;
                    ItemList.Add(Convert.ToString(cblPeriod.Items[i].Text));
                    Itemindex.Add(i);
                    if (txtPeriod.Text != "")
                    {
                        txtPeriod.Text = txtPeriod.Text + "," + Convert.ToString(ItemList[i]);
                    }
                    else
                    {
                        txtPeriod.Text = Convert.ToString(ItemList[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < cblPeriod.Items.Count; i++)
                {
                    cblPeriod.Items[i].Selected = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                }
                txtPeriod.Text = "---Select---";
            }
            //ViewOrSave();

            //if (cbPeriod.Checked == true)
            //{
            //    for (int i = 0; i < cblPeriod.Items.Count; i++)
            //    {
            //        cblPeriod.Items[i].Selected = true;
            //        if (hourval.Trim() == "")
            //        {
            //            hourval = Convert.ToString(cblPeriod.Items[i].Text);
            //        }
            //        else
            //        {
            //            hourval = hourval + ", " + Convert.ToString(cblPeriod.Items[i].Text);
            //        }
            //    }
            //    //txtPeriod.Text = "Periods(" + (cblPeriod.Items.Count) + ")";
            //    //if (b_school == true)
            //    //{
            //    //    txtPeriod.Text = "Periods(" + (cblPeriod.Items.Count) + ")";
            //    //}
            //    txtPeriod.Text = hourval;
            //}
            //else
            //{
            //    for (int i = 0; i < cblPeriod.Items.Count; i++)
            //    {
            //        cblPeriod.Items[i].Selected = false;
            //    }
            //    txtPeriod.Text = "---Select---";
            //}


        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cbViewOrNot_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divViewSpread.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            btnView.Visible = false;
            btnSave.Visible = false;
            if (cbViewOrNot.Checked)
            {
                cbPeriods.Checked = false;
                cbPeriods.Visible = true;

                cbreason.Checked = false;
                cbreason.Visible = true;

                btnView.Visible = true;
                btnSave.Visible = false;

                ddlpurpose.Enabled = false;

                if (rblSplFree.SelectedValue == "0")
                {
                    txtPeriod.Enabled = false;
                    txtToDate.Visible = false;
                    txtPeriod.Visible = true;
                }
                else
                {
                    cbPeriods.Visible = false;
                    txtPeriod.Visible = false;
                    txtToDate.Visible = true;
                }


            }
            else
            {
                cbreason.Checked = false;
                cbreason.Visible = false;

                cbPeriods.Visible = false;
                cbPeriods.Checked = false;

                btnView.Visible = false;
                btnSave.Visible = true;

                ddlpurpose.Enabled = true;

                if (rblSplFree.SelectedValue == "0")
                {
                    txtPeriod.Enabled = true;
                    txtToDate.Visible = false;
                    txtPeriod.Visible = true;
                }
                else
                {
                    cbPeriods.Visible = false;
                    txtPeriod.Visible = false;
                    txtToDate.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cbPeriods_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divViewSpread.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            if (rblSplFree.SelectedValue == "0")
            {
                txtToDate.Visible = false;
                txtPeriod.Visible = true;
                if (cbPeriods.Checked)
                {
                    txtPeriod.Enabled = true;
                }
                else
                {
                    txtPeriod.Enabled = false;
                }
                cblPeriod.ClearSelection();
                cbPeriod.Checked = false;
                txtPeriod.Text = "---Select---";
                Itemindex.Clear();
                ItemList.Clear();
            }
            else
            {
                cblPeriod.ClearSelection();
                cbPeriod.Checked = false;
                txtPeriod.Text = "---Select---";
                Itemindex.Clear();
                ItemList.Clear();
                txtToDate.Visible = true;
                txtPeriod.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cbreason_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divViewSpread.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            //ddlpurpose.Enabled = true;
            if (cbreason.Checked)
            {
                ddlpurpose.Enabled = true;
            }
            else
            {
                ddlpurpose.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion CheckBox Events

    #region CheckBoxList Events

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divViewSpread.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;

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
            bindsemester();
            BindSectionDetailmult();
            Bindhour();
            semstartend();
            ViewOrSave();
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
            divViewSpread.Visible = false;
            popupdiv.Visible = false;
            rptprint1.Visible = false;

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
            bindsemester();
            BindSectionDetailmult();
            Bindhour();
            semstartend();
            ViewOrSave();
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
            divViewSpread.Visible = false;
            popupdiv.Visible = false;
            rptprint1.Visible = false;

            int commcount = 0;
            cbPeriod.Checked = false;
            txtPeriod.Text = string.Empty;
            string hourval = string.Empty;

            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            int index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblPeriod.Items[index].Selected == true)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(Convert.ToString(cblPeriod.Items[index].Text));
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(Convert.ToString(cblPeriod.Items[index].Text));
                Itemindex.Remove(sindex);
                cbPeriod.Checked = false;
            }
            for (int i = 0; i < cblPeriod.Items.Count; i++)
            {
                if (cblPeriod.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(Convert.ToString(cblPeriod.Items[i].Text));
                    Itemindex.Remove(sindex);
                }
            }
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (txtPeriod.Text == "")
                {
                    txtPeriod.Text = Convert.ToString(ItemList[i]);
                }
                else
                {
                    txtPeriod.Text = txtPeriod.Text + "," + Convert.ToString(ItemList[i]);
                }
            }
            if (ItemList.Count == cblPeriod.Items.Count)
            {
                cbPeriod.Checked = true;
            }

            //for (int i = 0; i < cblPeriod.Items.Count; i++)
            //{
            //    if (cblPeriod.Items[i].Selected == true)
            //    {
            //        commcount = commcount + 1;
            //        if (hourval.Trim() == "")
            //        {
            //            hourval =  Convert.ToString(cblPeriod.Items[i].Text);
            //        }
            //        else
            //        {
            //            hourval = hourval + ", " +  Convert.ToString(cblPeriod.Items[i].Text);
            //        }
            //    }
            //}
            //if (commcount > 0)
            //{
            //    //txtPeriod.Text = "Periods(" +  Convert.ToString(commcount) + ")";
            //    txtPeriod.Text = hourval;
            //    if (b_school == true)
            //    {
            //        txtPeriod.Text = hourval;
            //        //txtPeriod.Text = "Periods(" +  Convert.ToString(commcount) + ")";
            //    }
            //    if (commcount == cblPeriod.Items.Count)
            //    {
            //        cbPeriod.Checked = true;
            //    }
            //}
            //ViewOrSave();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void chklstsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divViewSpread.Visible = false;
            popupdiv.Visible = false;
            rptprint1.Visible = false;
            psection.Focus();
            int sectioncount = 0;
            string value = string.Empty;
            string code = string.Empty;
            for (int i = 0; i < chklstsection.Items.Count; i++)
            {
                if (chklstsection.Items[i].Selected == true)
                {
                    value = chklstsection.Items[i].Text;
                    code = Convert.ToString(chklstsection.Items[i].Value);
                    sectioncount = sectioncount + 1;
                    txtsection.Text = "Sec(" + Convert.ToString(sectioncount) + ")";
                }
            }
            if (chklstsection.Items.Count == sectioncount)
            {
                chksection.Checked = true;
            }
            else
            {
                chksection.Checked = false;
            }
            if (sectioncount == 0)
            {
                txtsection.Text = "---Select---";
            }
            semstartend();
            Bindhour();
            ViewOrSave();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion CheckBoxList Events

    #region RadioButtonList Events

    protected void rblSplFree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divViewSpread.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            if (rblSplFree.SelectedValue == "0")
            {
                lblFromDate.Text = "Date";
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblFromDate.Visible = true;
                txtFromDate.Visible = true;
                lblPeriod.Visible = true;
                upnlPeriod.Visible = true;
                txtPeriod.Visible = true;
                lblToDate.Visible = false;
                txtToDate.Visible = false;
                lblAttBasedon.Visible = true;
                ddlAttBaseon.Visible = true;
            }
            else
            {
                cbPeriods.Visible = false;
                lblFromDate.Text = "From Date";
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblFromDate.Visible = true;
                txtFromDate.Visible = true;
                lblPeriod.Visible = false;
                upnlPeriod.Visible = false;
                txtPeriod.Visible = false;
                lblToDate.Visible = true;
                txtToDate.Visible = true;
                lblAttBasedon.Visible = false;
                ddlAttBaseon.Visible = false;

            }
            semstartend();
            ViewOrSave();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion RadioButtonList Events

    #region TextBox Changed Events

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divViewSpread.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;

            string degree_code1 = string.Empty;
            string batch_year1 = string.Empty;
            string semester1 = string.Empty;
            string semstart = string.Empty;
            string semend = string.Empty;
            string val = string.Empty;
            DateTime dtsemstart = new DateTime();
            DateTime dtsemend = new DateTime();
            DateTime dttoDate = new DateTime();
            if (rblSplFree.SelectedValue == "0")
            {
                val = "Date";
            }
            else
            {
                val = "From Date";
            }

            if (ddlbatch.Items.Count > 0)
            {
                batch_year1 = Convert.ToString(ddlbatch.SelectedValue);
            }
            if (ddlsem.Items.Count > 0)
            {
                semester1 = Convert.ToString(ddlsem.SelectedValue);
            }
            if (cblBranch.Items.Count > 0)
            {
                foreach (ListItem li in cblBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (degree_code1 == "")
                        {
                            degree_code1 = li.Value;
                        }
                        else
                        {
                            degree_code1 += "," + li.Value;
                        }
                    }
                }
            }
            if (batch_year1 != "" && degree_code1 != "" && semester1 != "")
            {
                qry = "select distinct Convert(Varchar,start_date,103) as Start_Date from seminfo where batch_year='" + batch_year1 + "' and semester='" + semester1 + "' and degree_code in(" + degree_code1 + ") order by Start_Date desc";
                semstart = d2.GetFunctionv(qry);

                qry = "select distinct Convert(Varchar,end_date,103) as End_Date from seminfo where batch_year='" + batch_year1 + "' and semester='" + semester1 + "' and degree_code in(" + degree_code1 + ") order by End_Date";
                semend = d2.GetFunctionv(qry);

                if (semstart != "" && semend != "")
                {
                    DateTime.TryParseExact(semstart, "dd/MM/yyyy", null, DateTimeStyles.None, out dtsemstart);
                    DateTime.TryParseExact(semend, "dd/MM/yyyy", null, DateTimeStyles.None, out dtsemend);

                    DateTime dtnow = DateTime.Now;
                    string datefad, dtfromad;
                    string datefromad;
                    string yr4, m4, d4;
                    datefad = Convert.ToString(txtFromDate.Text);
                    string[] split4 = datefad.Split(new Char[] { '/' });
                    string toDate = Convert.ToString(txtToDate.Text);
                    string[] splitTo = toDate.Split(new Char[] { '/' });


                    if (split4.Length == 3)
                    {
                        datefromad = Convert.ToString(split4[0]) + "/" + Convert.ToString(split4[1]) + "/" + Convert.ToString(split4[2]);
                        yr4 = Convert.ToString(split4[2]);
                        m4 = Convert.ToString(split4[1]);
                        d4 = Convert.ToString(split4[0]);
                        dtfromad = m4 + "/" + d4 + "/" + yr4;
                        DateTime dt1 = Convert.ToDateTime(dtfromad);
                        if (dt1 > dtnow)
                        {
                            lblErrSearch.Text = val + " Can't Be Greater Than Today Date";
                            lblErrSearch.Visible = true;
                            txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");
                            txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                            return;
                        }
                        if (dt1 < dtsemstart)
                        {
                            lblErrSearch.Text = val + " Can't Be Lesser Than Semester Start Date " + dtsemstart.ToString("dd/MM/yyyy");
                            lblErrSearch.Visible = true;
                            txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                            txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                            return;
                        }

                        if (dt1 > dtsemend)
                        {
                            lblErrSearch.Text = val + " Can't Be Greater Than Semester End Date " + dtsemend.ToString("dd/MM/yyyy");
                            lblErrSearch.Visible = true;
                            txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                            txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                            return;
                        }
                        else
                        {
                            lblErrSearch.Visible = false;
                        }
                        if (rblSplFree.SelectedValue != "0")
                        {
                            if (splitTo.Length == 3)
                            {
                                toDate = Convert.ToString(splitTo[0]) + "/" + Convert.ToString(splitTo[1]) + "/" + Convert.ToString(splitTo[2]);
                                yr4 = Convert.ToString(splitTo[2]);
                                m4 = Convert.ToString(splitTo[1]);
                                d4 = Convert.ToString(splitTo[0]);
                                dtfromad = m4 + "/" + d4 + "/" + yr4;
                                dttoDate = Convert.ToDateTime(dtfromad);
                            }
                            if (dttoDate > dtnow)
                            {
                                lblErrSearch.Text = "To Date Can't Be Greater Than Today Date ";
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy"); ;
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                            if (dt1 > dttoDate)
                            {
                                lblErrSearch.Text = val + " Can't Be Greater Than To Date ";
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy"); ;
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                            if (dttoDate < dt1)
                            {
                                lblErrSearch.Text = "To Date Can't Be Lesser Than From Date ";// + dtsemstart.ToString("dd/MM/yyyy")
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                            if (dttoDate < dtsemstart)
                            {
                                lblErrSearch.Text = "To Date Can't Be Lesser Than Semester Start Date " + dtsemstart.ToString("dd/MM/yyyy");
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }

                            if (dttoDate > dtsemend)
                            {
                                lblErrSearch.Text = "To Date Can't Be Greater Than Semester End Date " + dtsemend.ToString("dd/MM/yyyy");
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    lblErrSearch.Text = "Please Check The Semester Start Date And End Date.";
                    lblErrSearch.Visible = true;
                    return;
                }
            }
            else
            {
                lblErrSearch.Text = "Please Select The Batch,Degree,Department and Semester And Then Proceed.";
                lblErrSearch.Visible = true;
                return;
            }
            ViewOrSave();
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
            divViewSpread.Visible = false;
            popupdiv.Visible = false;

            degree_code = string.Empty;
            batch_year = string.Empty;
            semester = string.Empty;
            string semstart = string.Empty;
            string semend = string.Empty;
            string val = string.Empty;
            DateTime dtsemstart = new DateTime();
            DateTime dtsemend = new DateTime();
            DateTime dttoDate = new DateTime();
            if (rblSplFree.SelectedValue == "0")
            {
                val = "Date";
            }
            else
            {
                val = "From Date";
            }

            if (ddlbatch.Items.Count > 0)
            {
                batch_year = Convert.ToString(ddlbatch.SelectedValue);
            }
            if (ddlsem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlsem.SelectedValue);
            }
            if (cblBranch.Items.Count > 0)
            {
                foreach (ListItem li in cblBranch.Items)
                {
                    if (li.Selected)
                    {
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
            if (batch_year != "" && degree_code != "" && semester != "")
            {
                qry = "select distinct Convert(Varchar,start_date,103) as Start_Date from seminfo where batch_year='" + batch_year + "' and semester='" + semester + "' and degree_code in(" + degree_code + ") order by Start_Date desc";
                semstart = d2.GetFunctionv(qry);

                qry = "select distinct Convert(Varchar,end_date,103) as End_Date from seminfo where batch_year='" + batch_year + "' and semester='" + semester + "' and degree_code in(" + degree_code + ") order by End_Date";
                semend = d2.GetFunctionv(qry);

                if (semstart != "" && semend != "")
                {
                    DateTime.TryParseExact(semstart, "dd/MM/yyyy", null, DateTimeStyles.None, out dtsemstart);
                    DateTime.TryParseExact(semend, "dd/MM/yyyy", null, DateTimeStyles.None, out dtsemend);

                    DateTime dtnow = DateTime.Now;
                    string datefad, dtfromad;
                    string datefromad;
                    string yr4, m4, d4;
                    datefad = Convert.ToString(txtFromDate.Text);
                    string[] split4 = datefad.Split(new Char[] { '/' });
                    string toDate = Convert.ToString(txtToDate.Text);
                    string[] splitTo = toDate.Split(new Char[] { '/' });


                    if (split4.Length == 3)
                    {
                        datefromad = Convert.ToString(split4[0]) + "/" + Convert.ToString(split4[1]) + "/" + Convert.ToString(split4[2]);
                        yr4 = Convert.ToString(split4[2]);
                        m4 = Convert.ToString(split4[1]);
                        d4 = Convert.ToString(split4[0]);
                        dtfromad = m4 + "/" + d4 + "/" + yr4;
                        DateTime dt1 = Convert.ToDateTime(dtfromad);
                        if (dt1 > dtnow)
                        {
                            lblErrSearch.Text = val + " Can't Be Greater Than Today Date";
                            lblErrSearch.Visible = true;
                            txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");
                            txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                            return;
                        }
                        if (dt1 < dtsemstart)
                        {
                            lblErrSearch.Text = val + " Can't Be Lesser Than Semester Start Date " + dtsemstart.ToString("dd/MM/yyyy");
                            lblErrSearch.Visible = true;
                            txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                            txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                            return;
                        }

                        if (dt1 > dtsemend)
                        {
                            lblErrSearch.Text = val + " Can't Be Greater Than Semester End Date " + dtsemend.ToString("dd/MM/yyyy");
                            lblErrSearch.Visible = true;
                            txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                            txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                            return;
                        }
                        else
                        {
                            lblErrSearch.Visible = false;
                        }
                        if (rblSplFree.SelectedValue != "0")
                        {
                            if (splitTo.Length == 3)
                            {
                                toDate = Convert.ToString(splitTo[0]) + "/" + Convert.ToString(splitTo[1]) + "/" + Convert.ToString(splitTo[2]);
                                yr4 = Convert.ToString(splitTo[2]);
                                m4 = Convert.ToString(splitTo[1]);
                                d4 = Convert.ToString(splitTo[0]);
                                dtfromad = m4 + "/" + d4 + "/" + yr4;
                                dttoDate = Convert.ToDateTime(dtfromad);
                            }
                            if (dttoDate > dtnow)
                            {
                                lblErrSearch.Text = "To Date Can't Be Greater Than Today Date ";
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy"); ;
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                            if (dttoDate < dtsemstart)
                            {
                                lblErrSearch.Text = "To Date Can't Be Lesser Than Semester Start Date " + dtsemstart.ToString("dd/MM/yyyy");
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                            if (dttoDate < dt1)
                            {
                                lblErrSearch.Text = "To Date Can't Be Lesser Than From Date ";// + dtsemstart.ToString("dd/MM/yyyy")
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                            if (dttoDate > dtsemend)
                            {
                                lblErrSearch.Text = "To Date Can't Be Greater Than Semester End Date " + dtsemend.ToString("dd/MM/yyyy");
                                lblErrSearch.Visible = true;
                                txtFromDate.Text = dtsemstart.ToString("dd/MM/yyyy");// DateTime.Now.ToString("dd/MM/yyyy");
                                txtToDate.Text = dtsemend.ToString("dd/MM/yyyy");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    lblErrSearch.Text = "Please Check The Semester Start Date And End Date.";
                    lblErrSearch.Visible = true;
                    return;
                }
            }
            else
            {
                lblErrSearch.Text = "Please Select The Batch,Degree,Department and Semester And Then Proceed.";
                lblErrSearch.Visible = true;
                return;
            }
            ViewOrSave();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion TextBox Changed Events

    #region Initialize Spread

    public void Init_Spread()
    {
        try
        {
            bool isSpl = false;
            bool isfree = false;
            int reasonrow = 0;

            #region FpSpread Style

            FpViewSpread.Visible = false;
            FpViewSpread.Sheets[0].ColumnCount = 0;
            FpViewSpread.Sheets[0].RowCount = 0;
            FpViewSpread.Sheets[0].SheetCorner.ColumnCount = 0;
            FpViewSpread.CommandBar.Visible = false;

            #endregion FpSpread Style

            FpViewSpread.Visible = false;
            FpViewSpread.CommandBar.Visible = false;
            FpViewSpread.RowHeader.Visible = false;
            FpViewSpread.Sheets[0].AutoPostBack = false;
            FpViewSpread.Sheets[0].RowCount = 0;
            if (rblSplFree.SelectedValue == "0")
            {
                isfree = true;
                //reasonrow = 0;
                FpViewSpread.Sheets[0].ColumnCount = 10;
            }
            else if (rblSplFree.SelectedValue == "1")
            {
                isSpl = true;
                FpViewSpread.Sheets[0].ColumnCount = 9;
            }
            //FpViewSpread.Sheets[0].FrozenColumnCount = 8;

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
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;

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

            FpViewSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpViewSpread.Sheets[0].DefaultStyle = sheetstyle;
            FpViewSpread.Sheets[0].ColumnHeader.RowCount = 2;
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Section";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Reason";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Date";
            if (isfree)
            {
                FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Period";
                FpViewSpread.Sheets[0].Columns[8].Width = 60;
                FpViewSpread.Sheets[0].Columns[8].Locked = true;
                FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
            }

            FpViewSpread.Sheets[0].Columns[0].Width = 40;
            FpViewSpread.Sheets[0].Columns[1].Width = 100;
            FpViewSpread.Sheets[0].Columns[2].Width = 100;
            FpViewSpread.Sheets[0].Columns[3].Width = 200;
            FpViewSpread.Sheets[0].Columns[4].Width = 80;
            FpViewSpread.Sheets[0].Columns[5].Width = 75;
            FpViewSpread.Sheets[0].Columns[6].Width = 170;
            FpViewSpread.Sheets[0].Columns[7].Width = 100;

            FpViewSpread.Sheets[0].Columns[0].Locked = true;
            FpViewSpread.Sheets[0].Columns[1].Locked = true;
            FpViewSpread.Sheets[0].Columns[2].Locked = true;
            FpViewSpread.Sheets[0].Columns[3].Locked = true;
            FpViewSpread.Sheets[0].Columns[4].Locked = true;
            FpViewSpread.Sheets[0].Columns[5].Locked = true;
            FpViewSpread.Sheets[0].Columns[6].Locked = true;
            FpViewSpread.Sheets[0].Columns[7].Locked = true;

            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);

            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, FpViewSpread.Sheets[0].ColumnCount - 1].Text = "Select";
            FpViewSpread.Sheets[0].Columns[8].Width = 80;
            FpViewSpread.Sheets[0].Columns[8].Locked = false;
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpViewSpread.Sheets[0].ColumnCount - 1, 2, 1);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    #endregion Initialize Spread

    #region Farpoint Spread Events

    protected void FpViewSpread_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpViewSpread.SaveChanges();
            int row = FpViewSpread.ActiveSheetView.ActiveRow;
            int col = FpViewSpread.ActiveSheetView.ActiveColumn;
            if (col == FpViewSpread.Sheets[0].ColumnCount - 1)
            {
                if (Convert.ToInt32(FpViewSpread.Sheets[0].Cells[row, col].Value) == 0)
                {
                    FpViewSpread.Sheets[0].Cells[row, col].Value = 0;
                }
                else
                {
                    FpViewSpread.Sheets[0].Cells[row, col].Value = 1;
                }
            }

            if (Convert.ToInt32(FpViewSpread.Sheets[0].Cells[0, FpViewSpread.Sheets[0].ColumnCount - 1].Value) == 1)
            {
                for (int i = 0; i < FpViewSpread.Sheets[0].RowCount; i++)
                {
                    FpViewSpread.Sheets[0].Cells[i, FpViewSpread.Sheets[0].ColumnCount - 1].Value = 1;
                }
            }
            else if (Convert.ToInt32(FpViewSpread.Sheets[0].Cells[0, FpViewSpread.Sheets[0].ColumnCount - 1].Value) == 0)
            {
                for (int i = 0; i < FpViewSpread.Sheets[0].RowCount; i++)
                {
                    FpViewSpread.Sheets[0].Cells[i, FpViewSpread.Sheets[0].ColumnCount - 1].Value = 0;
                }

            }
            FpViewSpread.Visible = true;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Farpoint Spread Events

    #region Button Click

    #region Save

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            divViewSpread.Visible = false;
            popupdiv.Visible = false;
            rptprint1.Visible = false;
            string fromdate = string.Empty;
            string todate = string.Empty;
            string period = string.Empty;
            bool isFreeHour = false;
            bool isSplDay = false;
            DateTime dtFrom = new DateTime();
            DateTime dtTo = new DateTime();

            DataSet dsStud = new DataSet();
            DataView dv = new DataView();
            DataTable dtStud = new DataTable();
            DataSet dsDeg = new DataSet();

            DataSet dsAtt = new DataSet();
            DataView dvAtt = new DataView();
            DataSet dsattbased = new DataSet();
            DataTable dtAttBased = new DataTable();

            bool isfromDate = false, isToDate = false;
            int degselcount = 0;
            int secselcount = 0;
            int selhourcount = 0;
            bool success = false;
            string error = string.Empty;

            string attendbasedon = string.Empty;
            if (ddlCollege.Items.Count == 0)
            {
                lblpoperr.Text = "College is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (ddlbatch.Items.Count == 0)
            {
                lblpoperr.Text = "Batch Year is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                batch_year = Convert.ToString(ddlbatch.SelectedValue).Trim();
            }
            if (cblDegree.Items.Count == 0)
            {
                lblpoperr.Text = "Degree is not found";
                popupdiv.Visible = true;
                return;
            }
            if (cblBranch.Items.Count == 0)
            {
                lblpoperr.Text = "Branch is not found";
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
                        if (string.IsNullOrEmpty(degree_code.Trim()))
                        {
                            degree_code = "'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                        else
                        {
                            degree_code += ",'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                    }
                }
            }
            if (degselcount == 0)
            {
                lblpoperr.Text = "Please Select Any One Branch";
                popupdiv.Visible = true;
                return;
            }
            if (ddlsem.Items.Count == 0)
            {
                lblpoperr.Text = "Semester is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                semester = Convert.ToString(ddlsem.SelectedValue);
            }
            if (txtsection.Text != "---Select---")
            {
                section = string.Empty;
                bool hasEmpty = false;
                foreach (ListItem li in chklstsection.Items)
                {
                    string selValue = string.Empty;
                    string selText = string.Empty;
                    selValue = Convert.ToString(li.Value).Trim().ToLower();
                    selText = Convert.ToString(li.Text).Trim();
                    if (li.Selected)
                    {
                        secselcount++;
                        if (string.IsNullOrEmpty(section))
                        {
                            if (!string.IsNullOrEmpty(selValue.ToLower().Trim()) && selValue.ToLower().Trim() != "all" && selValue.ToLower().Trim() != "empty" && selValue.ToLower().Trim() != "-1" && selValue.ToLower().Trim() != "0")
                                section = "'" + selValue.Trim() + "'";
                            else if (!hasEmpty)
                            {
                                section = "''";
                                hasEmpty = true;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(selValue.ToLower().Trim()) && selValue.ToLower().Trim() != "all" && selValue.ToLower().Trim() != "empty" && selValue.ToLower().Trim() != "-1" && selValue.ToLower().Trim() != "0")
                                section += ",'" + selValue.Trim() + "'";
                            else if (!hasEmpty)
                            {
                                section += ",''";
                                hasEmpty = true;
                            }
                        }
                    }
                }
            }
            if (secselcount == 0)
            {
                lblpoperr.Text = "Please Select Any One Section";
                popupdiv.Visible = true;
                return;
            }
            if (Convert.ToString(rblSplFree.SelectedValue).Trim() == "0")
            {
                if (cblPeriod.Items.Count == 0)
                {
                    lblpoperr.Text = "Periods are not found";
                    popupdiv.Visible = true;
                    return;
                }
                else
                {
                    foreach (ListItem li in cblPeriod.Items)
                    {
                        if (li.Selected)
                        {
                            selhourcount++;
                        }
                    }
                }
                if (selhourcount == 0)
                {
                    lblpoperr.Text = "Please Select Any One Period And Then Proceed";
                    popupdiv.Visible = true;
                    return;
                }
                if (ddlAttBaseon.Items.Count == 0)
                {
                    lblpoperr.Text = "Attendance Based on Hour is not found";
                    popupdiv.Visible = true;
                    return;
                }
                else
                {
                    attendbasedon = Convert.ToString(ddlAttBaseon.SelectedItem).Trim();
                }
            }
            else
            {
                attendbasedon = string.Empty;
            }
            if (ddlpurpose.Items.Count == 0)
            {
                lblpoperr.Text = "Please Add Reason And Then Proceed";
                popupdiv.Visible = true;
                return;
            }
            else
            {

            }

            //DateValidation();

            if (!string.IsNullOrEmpty(batch_year) && !string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(degree_code) && degselcount != 0)
            {
                string newqry = "select  r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,r.degree_code,r.Batch_Year,r.Current_Semester,r.college_code,r.Sections,c.Course_Name,dt.Dept_Name  from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=r.college_code and r.college_code=dt.college_code and dt.college_code=c.college_code and dg.Degree_Code=r.degree_code and dg.Dept_Code=dt.Dept_Code and c.Course_Id=dg.Course_Id and r.college_code='" + collegecode + "' and r.Batch_Year='" + batch_year + "' and r.degree_code in(" + degree_code + ") and r.Current_Semester in (" + semester + ")  and r.Sections in(" + section + ") and r.CC='0' and r.DelFlag='0' and Exam_Flag<>'debar'";
                qry = "select distinct r.degree_code,r.Batch_Year,r.Current_Semester,r.college_code,r.Sections,c.Course_Name,dt.Dept_Name  from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=r.college_code and r.college_code=dt.college_code and dt.college_code=c.college_code and dg.Degree_Code=r.degree_code and dg.Dept_Code=dt.Dept_Code and c.Course_Id=dg.Course_Id  and r.college_code='" + collegecode + "' and r.Batch_Year='" + batch_year + "' and r.degree_code in(" + degree_code + ") and r.Current_Semester in (" + semester + ")  and r.Sections in(" + section + ") and r.CC='0' and r.DelFlag='0' and Exam_Flag<>'debar'";
                if (section == "")
                {
                    newqry = "select r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,r.degree_code,r.Batch_Year,r.Current_Semester,r.college_code,r.Sections,c.Course_Name,dt.Dept_Name  from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=r.college_code and r.college_code=dt.college_code and dt.college_code=c.college_code and dg.Degree_Code=r.degree_code and dg.Dept_Code=dt.Dept_Code and c.Course_Id=dg.Course_Id and r.college_code='" + collegecode + "' and r.Batch_Year='" + batch_year + "' and r.degree_code in(" + degree_code + ") and r.Current_Semester in (" + semester + ")  and r.CC='0' and r.DelFlag='0' and Exam_Flag<>'debar'";
                    qry = "select distinct r.degree_code,r.Batch_Year,r.Current_Semester,r.college_code,r.Sections,c.Course_Name,dt.Dept_Name from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=r.college_code and r.college_code=dt.college_code and dt.college_code=c.college_code and dg.Degree_Code=r.degree_code and dg.Dept_Code=dt.Dept_Code and c.Course_Id=dg.Course_Id  and r.college_code='" + collegecode + "' and r.Batch_Year='" + batch_year + "' and r.degree_code in(" + degree_code + ") and r.Current_Semester in (" + semester + ")  and r.CC='0' and r.DelFlag='0' and Exam_Flag<>'debar'";
                }

                dsDeg.Clear();
                dsDeg.Reset();
                dsDeg.Dispose();
                dsDeg = d2.select_method_wo_parameter(qry, "Text");

                dsStud.Clear();
                dsStud.Reset();
                dsStud.Dispose();
                dsStud = d2.select_method_wo_parameter(newqry, "Text");

                qry = "select degree_code,No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,semester from PeriodAttndSchedule where degree_code in(" + degree_code + ")";
                dsAtt = d2.select_method_wo_parameter(qry, "Text");

            }

            int mont_year = 0;
            int newdate = 0;
            string dcol = string.Empty;
            if (txtFromDate.Text != "")
            {
                isfromDate = DateTime.TryParseExact(txtFromDate.Text, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFrom);

            }
            if (txtToDate.Text != "")
            {
                isToDate = DateTime.TryParseExact(txtToDate.Text, "dd/MM/yyyy", null, DateTimeStyles.None, out dtTo);
            }
            if (dsDeg.Tables.Count > 0 && dsDeg.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(rblSplFree.SelectedValue).Trim() == "0")
                {
                    if (attendbasedon.Trim() == "")
                    {
                        lblpoperr.Text = "Attendance Based on Hour is not found";
                        popupdiv.Visible = true;
                        return;
                    }
                    if (isfromDate && attendbasedon.Trim() != "")
                    {
                        int yr = dtFrom.Year;
                        mont_year = (yr * 12) + dtFrom.Month;
                        newdate = dtFrom.Day;
                        dcol = "d" + newdate + "d" + attendbasedon;
                        qry = "select r.Batch_Year,r.degree_code,r.Roll_No,a." + dcol + " as Attend_based from Registration r,attendance A WHERE R.Roll_No=A.roll_no and a.month_year='" + mont_year + "' and Batch_Year in(" + batch_year + ") and degree_code in(" + degree_code + ") ";
                        dsattbased.Clear();
                        dsattbased.Reset();
                        dsattbased = d2.select_method_wo_parameter(qry, "text");
                        if (dtFrom.DayOfWeek == DayOfWeek.Sunday)
                        {
                            lblErrSearch.Text = "" + dtFrom.ToString("dd/MM/yyyy") + " is Sunday.";
                            lblErrSearch.Visible = true;
                            return;
                        }
                        else
                        {
                            foreach (ListItem li in cblBranch.Items)
                            {
                                if (li.Selected)
                                {
                                    string dept_name = li.Text;
                                    string course_name = d2.GetFunctionv("select Course_Name from Course c,Department dt,Degree dg where c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and c.college_code=dt.college_code and dt.college_code= dg.college_code and dg.college_code=c.college_code and dg.Degree_Code='" + li.Value + "'");
                                    degree_code = li.Value;
                                    dsDeg.Tables[0].DefaultView.RowFilter = "degree_code='" + degree_code + "'";
                                    DataView dvdeg = dsDeg.Tables[0].DefaultView;
                                    if (dvdeg.Count > 0)
                                    {
                                        string noofhr = string.Empty;
                                        string Isthalf = string.Empty;
                                        string IIndhalf = string.Empty;
                                        int no_of_hr = 0;
                                        int frsthalf = 0;
                                        int sndhalf = 0;
                                        if (dsAtt.Tables.Count > 0 && dsAtt.Tables[0].Rows.Count > 0)
                                        {
                                            dsAtt.Tables[0].DefaultView.RowFilter = "degree_code='" + degree_code + "' and semester='" + semester + "'";
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

                                        for (int i = 0; i < dvdeg.Count; i++)
                                        {
                                            bool isFullholiday = false;
                                            bool isHoliMorn = false;
                                            bool isHoliEven = false;
                                            int fsthalf = 0;

                                            int res = 0;
                                            bool issucc = false;
                                            string newhour = string.Empty;
                                            degree_code = Convert.ToString(dvdeg[i]["degree_code"]).Trim();
                                            string rsection = Convert.ToString(dvdeg[i]["Sections"]).Trim();
                                            string batch = Convert.ToString(dvdeg[i]["Batch_Year"]).Trim();
                                            course_name = Convert.ToString(dvdeg[i]["Course_Name"]).Trim();
                                            //string sem=
                                            isholidayCheck(collegecode, degree_code, semester, dtFrom.ToString("dd/MM/yyyy"), out isFullholiday, out isHoliMorn, out isHoliEven, out fsthalf);
                                            if (isFullholiday && isHoliMorn && isHoliEven)
                                            {
                                                if (error == "")
                                                {
                                                    error = "The Date " + dtFrom.ToString("dd/MM/yyyy") + " is Holiday";
                                                }
                                                else
                                                {
                                                    error += "\nThe Date " + dtFrom.ToString("dd/MM/yyyy") + " is Holiday";
                                                }
                                                continue;
                                            }
                                            if (isApplicableForFreeSpecial(collegecode, batch_year, degree_code, semester, rsection, dtFrom.ToString("MM/dd/yyyy"), "1"))
                                            {
                                                if (dsStud.Tables.Count > 0 && dsStud.Tables[0].Rows.Count > 0)
                                                {
                                                    dsStud.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "' and Sections='" + rsection + "'";
                                                    dv = dsStud.Tables[0].DefaultView;
                                                    dtStud = dv.ToTable();
                                                }
                                                if (dsattbased.Tables.Count > 0 && dsattbased.Tables[0].Rows.Count > 0)
                                                {
                                                    dtAttBased = new DataTable();
                                                    dsattbased.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "'";
                                                    dv = dsattbased.Tables[0].DefaultView;
                                                    dtAttBased = dv.ToTable();
                                                }
                                                foreach (ListItem peri in cblPeriod.Items)
                                                {
                                                    if (peri.Selected)
                                                    {
                                                        bool isValid = false;
                                                        int perHr = 0;
                                                        int.TryParse(peri.Text.Trim(), out perHr);
                                                        qry = "if not exists (select * from tbl_spl_attendace where college_code='" + collegecode + "' and batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and semester='" + semester + "' and section='" + rsection + "' and entry_date='" + dtFrom.ToString("MM/dd/yyyy") + "' and period='" + peri.Text.Trim() + "' and  attype='0') insert into tbl_spl_attendace (attype,college_code,batch_year,degree_code,semester,section,entry_date,period,reason) values ('0','" + collegecode + "','" + batch_year + "','" + degree_code + "','" + semester + "','" + rsection + "','" + dtFrom.ToString("MM/dd/yyyy") + "','" + peri.Text.Trim() + "','" + Convert.ToString(ddlpurpose.SelectedItem) + "') else update  tbl_spl_attendace set  attype='0' where college_code='" + collegecode + "' and batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and semester='" + semester + "' and section='" + rsection + "' and entry_date='" + dtFrom.ToString("MM/dd/yyyy") + "' and period='" + peri.Text.Trim() + "' and  attype='0'";
                                                        if (!isFullholiday && !isHoliMorn && isHoliEven)
                                                        {
                                                            if (perHr <= frsthalf)
                                                            {
                                                                isValid = true;
                                                                res = d2.update_method_wo_parameter(qry, "Text");
                                                            }
                                                            else
                                                            {
                                                                if (error == "")
                                                                {
                                                                    error = "The Date " + dtFrom.ToString("dd/MM/yyyy") + " Evening(IInd Half) is Holiday.So,The Period " + perHr + " Can't Set as Free Hour For Department : " + course_name + " - " + dept_name + " - Semester : " + semester + ((rsection != "") ? " - Section : " + rsection + " " : "");
                                                                }
                                                                else
                                                                {
                                                                    error += "\nThe Date " + dtFrom.ToString("dd/MM/yyyy") + " Evening(IInd Half) is Holiday.So,The Period " + perHr + " Can't Set as Free Hour For Department : " + course_name + " - " + dept_name + " - Semester : " + semester + ((rsection != "") ? " - Section : " + rsection + " " : "");
                                                                }
                                                            }
                                                        }
                                                        else if (!isFullholiday && isHoliMorn && !isHoliEven)
                                                        {
                                                            if (perHr > frsthalf && perHr <= no_of_hr)
                                                            {
                                                                isValid = true;
                                                                res = d2.update_method_wo_parameter(qry, "Text");
                                                            }
                                                            else
                                                            {
                                                                if (error == "")
                                                                {
                                                                    error = "The Date " + dtFrom.ToString("dd/MM/yyyy") + " Morning(Ist Half) is Holiday.So,The Period " + perHr + " Can't Set as Free Hour For Department : " + course_name + " - " + dept_name + " - Semester : " + semester + ((rsection != "") ? " - Section : " + rsection + " " : "");
                                                                }
                                                                else
                                                                {
                                                                    error += "\nThe Date " + dtFrom.ToString("dd/MM/yyyy") + " Morning(Ist Half) is Holiday.So,The Period " + perHr + " Can't Set as Free Hour For Department : " + course_name + " - " + dept_name + " - Semester : " + semester + ((rsection != "") ? " - Section : " + rsection + " " : "");
                                                                }
                                                            }
                                                        }
                                                        else if (!isFullholiday && !isHoliMorn && !isHoliEven)
                                                        {
                                                            if (perHr <= no_of_hr)
                                                            {
                                                                isValid = true;
                                                                res = d2.update_method_wo_parameter(qry, "Text");
                                                            }
                                                            else
                                                            {
                                                                if (error == "")
                                                                {
                                                                    error = "The Period " + perHr + " Is Invalid For Department : " + course_name + " - " + dept_name + " - Semester : " + semester + ((rsection != "") ? " - Section : " + rsection + " " : "");
                                                                }
                                                                else
                                                                {
                                                                    error += "\nThe Period " + perHr + " Is Invalid For Department : " + course_name + " - " + dept_name + " - Semester : " + semester + ((rsection != "") ? " - Section : " + rsection + " " : "");
                                                                }
                                                            }
                                                        }
                                                        if (res > 0 && isValid)
                                                        {
                                                            issucc = true;
                                                            success = true;
                                                            //if (newhour == "")
                                                            //{
                                                            //    newhour = peri.Text.Trim();
                                                            //}
                                                            //else
                                                            //{
                                                            //    newhour += "," + peri.Text.Trim();
                                                            //}
                                                        }
                                                        if (dtStud.Rows.Count > 0 && issucc)
                                                            save(dtStud, collegecode, batch_year, degree_code, semester, rsection, dtFrom.ToString("dd/MM/yyyy"), dtFrom.ToString("dd/MM/yyyy"), Convert.ToString(perHr), Convert.ToString(ddlpurpose.SelectedItem), ref error, ref success, dtAttBased: dtAttBased);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (error == "")
                                                {
                                                    error = "The Date " + dtFrom.ToString("dd/MM/yyyy") + " is Already Set To Special Day For Department : " + course_name + " - " + dept_name + " - Semester : " + semester + ((rsection != "") ? " - Section : " + rsection + " " : "");

                                                }
                                                else
                                                {
                                                    error += "\nThe Date " + dtFrom.ToString("dd/MM/yyyy") + " is Already Set To Special Day For Department : " + course_name + " - " + dept_name + " - Semester : " + semester + ((rsection != "") ? " - Section : " + rsection + " " : "");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (error == "")
                                        {
                                            error = "The Department  " + course_name + " - " + dept_name + " is Not Found Any Students.";
                                        }
                                        else
                                        {
                                            error += "\nThe Department  " + course_name + " - " + dept_name + " is Not Found Any Students.";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lblpoperr.Text = "Date Must Be In The Format dd/MM/yyyy!!";
                        popupdiv.Visible = true;
                        return;
                    }
                }
                else if (Convert.ToString(rblSplFree.SelectedValue).Trim() == "1")
                {
                    DateTime dtDummyFrom = new DateTime();
                    isSplDay = true;
                    qry = string.Empty;
                    dtDummyFrom = dtFrom;
                    int maxnoofhr = 0;
                    
                    if (isfromDate && isToDate)
                    {
                        degree_code = string.Empty;
                        foreach (ListItem li in cblBranch.Items)
                        {
                            maxnoofhr = 0;
                            /*
                             * byte - 0 Contains Sunday
                             * byte - 1 Contains Holidays
                             * byte - 2 Contains Morning Holiday
                             * byte - 3 Contains Evening Holiday
                             * byte - 4 Contains Arlready Set Free Hour
                             * */
                            Dictionary<byte, DateTime[]> dicErrMessage = new Dictionary<byte, DateTime[]>();
                            DateTime[] dtErrDateList = new DateTime[0];
                            //dicErrMessage.Add(0, dtErrDateList);
                            //dicErrMessage.Add(1, dtErrDateList);
                            //dicErrMessage.Add(2, dtErrDateList);
                            //dicErrMessage.Add(3, dtErrDateList);
                            //dicErrMessage.Add(4, dtErrDateList);
                            if (li.Selected)
                            {
                                string dept_name = li.Text;
                                degree_code = li.Value;
                                string course_name = string.Empty;
                                //string course_name = d2.GetFunctionv("select Course_Name from Course c,Department dt,Degree dg where c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and c.college_code=dt.college_code and dt.college_code= dg.college_code and dg.college_code=c.college_code and dg.Degree_Code='" + li.Value + "'");
                                dsDeg.Tables[0].DefaultView.RowFilter = "degree_code='" + degree_code + "'";
                                DataView dvdeg = dsDeg.Tables[0].DefaultView;
                                string maxhr = d2.GetFunctionv("select max(pa.No_of_hrs_per_day) as No_of_hrs_per_day from PeriodAttndSchedule pa,seminfo si,Registration r where pa.degree_code=si.degree_code and r.degree_code=pa.degree_code and r.degree_code=si.degree_code and r.Batch_Year=si.batch_year and pa.semester=si.semester and r.batch_year='" + batch_year + "' and r.degree_code='" + degree_code + "'  and  college_code='" + collegecode + "' and pa.semester='" + semester + "'");
                                int.TryParse(maxhr, out maxnoofhr);
                                if (dvdeg.Count > 0)
                                {
                                    string noofhr = string.Empty;
                                    string Isthalf = string.Empty;
                                    string IIndhalf = string.Empty;
                                    int no_of_hr = 0;
                                    int frsthalf = 0;
                                    int sndhalf = 0;
                                    dtDummyFrom = dtFrom;
                                    if (dsAtt.Tables.Count > 0 && dsAtt.Tables[0].Rows.Count > 0)
                                    {
                                        dsAtt.Tables[0].DefaultView.RowFilter = "degree_code='" + degree_code + "' and semester='" + semester + "'";
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
                                    for (int i = 0; i < dvdeg.Count; i++)
                                    {
                                        bool isFullholiday = false;
                                        bool isHoliMorn = false;
                                        bool isHoliEven = false;
                                        int fsthalf = 0;

                                        int res = 0;
                                        bool issucc = false;
                                        string newhour = string.Empty;
                                        degree_code = Convert.ToString(dvdeg[i]["degree_code"]).Trim();
                                        string rsection = Convert.ToString(dvdeg[i]["Sections"]).Trim();
                                        string batch = Convert.ToString(dvdeg[i]["Batch_Year"]).Trim();
                                        course_name = Convert.ToString(dvdeg[i]["Course_Name"]).Trim();
                                        dtDummyFrom = dtFrom;
                                        if (dsStud.Tables.Count > 0 && dsStud.Tables[0].Rows.Count > 0)
                                        {
                                            dsStud.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "' and Sections='" + rsection + "'";
                                            dv = dsStud.Tables[0].DefaultView;
                                            dtStud = dv.ToTable();
                                        }
                                        int totdays = 0;
                                        while (dtDummyFrom <= dtTo && dtStud.Rows.Count > 0)
                                        {
                                            bool issuc = false;
                                            newhour = string.Empty;
                                            if (dtDummyFrom.DayOfWeek != DayOfWeek.Sunday)
                                            {
                                                isFullholiday = false;
                                                isHoliMorn = false;
                                                isHoliEven = false;
                                                fsthalf = 0;
                                                isholidayCheck(collegecode, degree_code, semester, dtDummyFrom.ToString("dd/MM/yyyy"), out isFullholiday, out isHoliMorn, out isHoliEven, out fsthalf);
                                                if (isFullholiday && isHoliMorn && isHoliEven)
                                                {
                                                    if (dicErrMessage.ContainsKey(1))
                                                    {
                                                        List<DateTime> lst = new List<DateTime>();

                                                        dtErrDateList = new DateTime[0];
                                                        dtErrDateList = dicErrMessage[1];
                                                        Array.Resize(ref dtErrDateList, dtErrDateList.Length);

                                                        dtErrDateList[dtErrDateList.Length - 1] = dtDummyFrom;

                                                        lst = dtErrDateList.ToList<DateTime>();
                                                        dtErrDateList = new DateTime[0];
                                                        dtErrDateList = lst.ToArray<DateTime>();

                                                        dicErrMessage[1] = dtErrDateList;
                                                    }
                                                    else
                                                    {
                                                        dtErrDateList = new DateTime[1];
                                                        dtErrDateList[0] = dtDummyFrom;
                                                        dicErrMessage.Add(1, dtErrDateList);
                                                    }
                                                    //if (error == "")
                                                    //{
                                                    //    error = "The Date " + dtDummyFrom.ToString("dd/MM/yyyy") + " is Holiday";
                                                    //}
                                                    //else
                                                    //{
                                                    //    error += "\nThe Date " + dtDummyFrom.ToString("dd/MM/yyyy") + " is Holiday";
                                                    //}
                                                    dtDummyFrom = dtDummyFrom.AddDays(1);
                                                    totdays++;
                                                    continue;
                                                }
                                                else
                                                {
                                                    if (isApplicableForFreeSpecial(collegecode, batch_year, degree_code, semester, rsection, dtDummyFrom.ToString("MM/dd/yyyy"), "0"))
                                                    {
                                                        int starthr = 1;
                                                        int endhr = no_of_hr;
                                                        if (!isFullholiday && !isHoliMorn && isHoliEven)
                                                        {
                                                            starthr = 1;
                                                            endhr = frsthalf;
                                                            if (dicErrMessage.ContainsKey(3))
                                                            {
                                                                List<DateTime> lst = new List<DateTime>();

                                                                dtErrDateList = new DateTime[0];
                                                                dtErrDateList = dicErrMessage[3];
                                                                Array.Resize(ref dtErrDateList, dtErrDateList.Length);

                                                                dtErrDateList[dtErrDateList.Length - 1] = dtDummyFrom;

                                                                lst = dtErrDateList.ToList<DateTime>();
                                                                dtErrDateList = new DateTime[0];
                                                                dtErrDateList = lst.ToArray<DateTime>();

                                                                dicErrMessage[3] = dtErrDateList;
                                                            }
                                                            else
                                                            {
                                                                dtErrDateList = new DateTime[1];
                                                                dtErrDateList[0] = dtDummyFrom;
                                                                dicErrMessage.Add(3, dtErrDateList);
                                                            }
                                                        }
                                                        else if (!isFullholiday && isHoliMorn && !isHoliEven)
                                                        {
                                                            starthr = frsthalf + 1;
                                                            endhr = no_of_hr;
                                                            if (dicErrMessage.ContainsKey(2))
                                                            {
                                                                List<DateTime> lst = new List<DateTime>();

                                                                dtErrDateList = new DateTime[0];
                                                                dtErrDateList = dicErrMessage[2];
                                                                Array.Resize(ref dtErrDateList, dtErrDateList.Length);

                                                                dtErrDateList[dtErrDateList.Length - 1] = dtDummyFrom;

                                                                lst = dtErrDateList.ToList<DateTime>();
                                                                dtErrDateList = new DateTime[0];
                                                                dtErrDateList = lst.ToArray<DateTime>();

                                                                dicErrMessage[2] = dtErrDateList;
                                                            }
                                                            else
                                                            {
                                                                dtErrDateList = new DateTime[1];
                                                                dtErrDateList[0] = dtDummyFrom;
                                                                dicErrMessage.Add(2, dtErrDateList);
                                                            }
                                                        }
                                                        else if (!isFullholiday && !isHoliMorn && !isHoliEven)
                                                        {
                                                            starthr = 1;
                                                            endhr = no_of_hr;
                                                        }
                                                        //else
                                                        //{
                                                        //    starthr = 0;
                                                        //    endhr = 0;
                                                        //}
                                                        for (int peri = starthr; peri <= endhr; peri++)
                                                        {
                                                            bool isValid = false;
                                                            qry = "if not exists (select * from tbl_spl_attendace where college_code='' and batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and semester='" + semester + "' and section='" + rsection + "' and entry_date='" + dtDummyFrom.ToString("MM/dd/yyyy") + "' and period='" + peri + "' and attype='1') insert into tbl_spl_attendace (attype,college_code,batch_year,degree_code,semester,section,entry_date,period,reason) values ('1','" + collegecode + "','" + batch_year + "','" + degree_code + "','" + semester + "','" + rsection + "','" + dtDummyFrom.ToString("MM/dd/yyyy") + "','" + peri + "','" + Convert.ToString(ddlpurpose.SelectedItem) + "') else update tbl_spl_attendace set attype='1' where college_code='' and batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and semester='" + semester + "' and section='" + rsection + "' and entry_date='" + dtDummyFrom.ToString("MM/dd/yyyy") + "' and period='" + peri + "' and attype='1'";
                                                            //if (!isFullholiday && !isHoliMorn && isHoliEven)
                                                            //{
                                                            //    if (peri <= fsthalf)
                                                            //    {
                                                            //        isValid = true;
                                                            //        res = d2.update_method_wo_parameter(qry, "Text");
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        if (error == "")
                                                            //        {
                                                            //            error = "The Date " + dtFrom.ToString("dd/MM/yyyy") + " Evening(IInd Half) is Holiday";
                                                            //        }
                                                            //        else
                                                            //        {
                                                            //            error += "\nThe Date " + dtFrom.ToString("dd/MM/yyyy") + " Evening(IInd Half) is Holiday";
                                                            //        }
                                                            //    }

                                                            //}
                                                            //else if (!isFullholiday && isHoliMorn && !isHoliEven)
                                                            //{
                                                            //    if (peri > fsthalf)
                                                            //    {
                                                            //        isValid = true;
                                                            //        res = d2.update_method_wo_parameter(qry, "Text");
                                                            //    }
                                                            //    if (error == "")
                                                            //    {
                                                            //        error = "The Date " + dtFrom.ToString("dd/MM/yyyy") + " Morning(Ist Half) is Holiday";
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        error += "\nThe Date " + dtFrom.ToString("dd/MM/yyyy") + " Morning(Ist Half) is Holiday";
                                                            //    }
                                                            //}
                                                            //else if (!isFullholiday && !isHoliMorn && !isHoliEven)
                                                            //{
                                                            isValid = true;
                                                            res = d2.update_method_wo_parameter(qry, "Text");
                                                            //}
                                                            if (res > 0 && isValid)
                                                            {
                                                                issuc = true;
                                                                success = true;
                                                            }

                                                            if (newhour == "")
                                                            {
                                                                newhour = Convert.ToString(peri);
                                                            }
                                                            else
                                                            {
                                                                newhour += "," + Convert.ToString(peri);
                                                            }
                                                        }
                                                        if (dtStud.Rows.Count > 0 && issuc)
                                                            save(dtStud, collegecode, batch_year, degree_code, semester, rsection, dtDummyFrom.ToString("dd/MM/yyyy"), dtDummyFrom.ToString("dd/MM/yyyy"), newhour, Convert.ToString(ddlpurpose.SelectedItem), ref error, ref success);
                                                    }
                                                    else
                                                    {
                                                        if (dicErrMessage.ContainsKey(4))
                                                        {
                                                            List<DateTime> lst = new List<DateTime>();

                                                            dtErrDateList = new DateTime[0];
                                                            dtErrDateList = dicErrMessage[4];
                                                            Array.Resize(ref dtErrDateList, dtErrDateList.Length);

                                                            dtErrDateList[dtErrDateList.Length - 1] = dtDummyFrom;

                                                            lst = dtErrDateList.ToList<DateTime>();
                                                            dtErrDateList = new DateTime[0];
                                                            dtErrDateList = lst.ToArray<DateTime>();

                                                            dicErrMessage[4] = dtErrDateList;
                                                        }
                                                        else
                                                        {
                                                            dtErrDateList = new DateTime[1];
                                                            dtErrDateList[0] = dtDummyFrom;
                                                            dicErrMessage.Add(4, dtErrDateList);
                                                        }
                                                        if (error == "")
                                                        {
                                                            error = "The Date " + dtDummyFrom.ToString("dd/MM/yyyy") + " is Already Set To Free Hour For Department : " + course_name + " - " + dept_name + " - Semester : " + semester + ((rsection != "") ? "- Section : " + rsection + " " : "");
                                                        }
                                                        else
                                                        {
                                                            error += "\nThe Date " + dtDummyFrom.ToString("dd/MM/yyyy") + " is Already Set To Free Hour For Department : " + course_name + " - " + dept_name + " - Semester : " + semester + ((rsection != "") ? "- Section : " + rsection + " " : "");
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //if (error == "")
                                                //{
                                                //    error = "The Date " + dtDummyFrom.ToString("dd/MM/yyyy") + " is Sunday";
                                                //}
                                                //else
                                                //{
                                                //    error += "\nThe Date " + dtDummyFrom.ToString("dd/MM/yyyy") + " is Sunday";
                                                //}
                                                if (dicErrMessage.ContainsKey(0))
                                                {
                                                    List<DateTime> lst = new List<DateTime>();

                                                    dtErrDateList = new DateTime[0];
                                                    dtErrDateList = dicErrMessage[0];
                                                    Array.Resize(ref dtErrDateList, dtErrDateList.Length);
                                                   
                                                    dtErrDateList[dtErrDateList.Length - 1] = dtDummyFrom;

                                                    lst = dtErrDateList.ToList<DateTime>();
                                                    dtErrDateList = new DateTime[0];
                                                    dtErrDateList = lst.ToArray<DateTime>();

                                                    dicErrMessage[0] = dtErrDateList;
                                                }
                                                else
                                                {
                                                    dtErrDateList = new DateTime[1];
                                                    dtErrDateList[0]=dtDummyFrom;
                                                    dicErrMessage.Add(0, dtErrDateList);
                                                }
                                            }
                                            dtDummyFrom = dtDummyFrom.AddDays(1);
                                            totdays++;
                                        }
                                    }
                                }
                                else
                                {
                                    if (error == "")
                                    {
                                        error = "The Department " + course_name + " - " + dept_name + " is Not Found Any Students.";
                                    }
                                    else
                                    {
                                        error += "\nThe Department " + course_name + " - " + dept_name + " is Not Found Any Students.";
                                    }
                                }
                                string departErr = string.Empty;
                                if(dicErrMessage.Count>0)
                                {
                                    departErr = "The Department " + course_name + " - " + dept_name;
                                    foreach (KeyValuePair<byte,DateTime[]> dicKeyValuePair in dicErrMessage)
                                    {
                                        byte key = dicKeyValuePair.Key;
                                        DateTime[] dtValue = dicKeyValuePair.Value;
                                        string errMsg=string.Empty;
                                        List<DateTime> ldtDate = new List<DateTime>();
                                        List<string> lst = new List<string>();
                                        string dateList = string.Empty;
                                        switch (key)
                                        {
                                            case 0:
                                                //DateTime[] strDateList = dtValue.ToList<DateTime>().ToArray<DateTime>();
                                                //ldtDate.Select(lst => ldtDate.ToString("dd/MM/yyyy")).ToList();
                                                //dateList = dtValue.ToList<DateTime>().ToString("dd/MM/yyyy");
                                                ldtDate = dtValue.ToList<DateTime>();
                                                lst = ldtDate.Select(date => date.ToString("dd/MM/yyyy")).ToList();
                                                errMsg = " The Date " + string.Join(",", lst.ToArray()) + ((lst.ToArray().Length == 1) ? " is " : " are ") + " Sunday";
                                                break;
                                            case 1:
                                                ldtDate = dtValue.ToList<DateTime>();
                                                lst = ldtDate.Select(date => date.ToString("dd/MM/yyyy")).ToList();
                                                errMsg = "The Date " + string.Join(",", lst.ToArray()) + ((lst.ToArray().Length == 1) ? " is " : " are ") + " Holiday";
                                                break;
                                            case 2:
                                                ldtDate = dtValue.ToList<DateTime>();
                                                lst = ldtDate.Select(date => date.ToString("dd/MM/yyyy")).ToList();
                                                errMsg = "The Date " + string.Join(",", lst.ToArray()) + ((lst.ToArray().Length == 1) ? " is " : " are ") + " Morning Holiday";
                                                break;
                                            case 3:
                                                ldtDate = dtValue.ToList<DateTime>();
                                                lst = ldtDate.Select(date => date.ToString("dd/MM/yyyy")).ToList();
                                                errMsg = "The Date " + string.Join(",", lst.ToArray()) + ((lst.ToArray().Length == 1) ? " is " : " are ") + "  Evening Holiday";
                                                break;
                                            case 4:
                                                ldtDate = dtValue.ToList<DateTime>();
                                                lst = ldtDate.Select(date => date.ToString("dd/MM/yyyy")).ToList();
                                                //errMsg = " The Date " + string.Join(",", lst.ToArray()) + ((lst.ToArray().Length == 1) ? " is " : " are ") + " Sunday";
                                                break;
                                        }
                                        if (error == "")
                                        {
                                            error = departErr + "\t\t:\t\t" + errMsg;
                                        }
                                        else
                                        {
                                            error += "\n\n" + departErr + "\t\t:\t\t" + errMsg;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lblpoperr.Text = "From Date and To Date Must Be In The Format dd/MM/yyyy!!!";
                        popupdiv.Visible = true;
                        return;
                    }

                }
            }
            else
            {
                lblpoperr.Text = "No Record(s) Were Found";
                popupdiv.Visible = true;
                return;
            }

            if (success)
            {
                lblpoperr.Text = "Saved Successfully";
                popupdiv.Visible = true;
                if (error != "")
                {
                    lblErrSearch.Text = error;
                    lblErrSearch.Visible = true;
                }
                return;
            }
            else
            {
                if (error != "")
                {
                    lblErrSearch.Text = error;
                    lblErrSearch.Visible = true;
                }
                lblpoperr.Text = "Not Saved";
                popupdiv.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Save

    #region View

    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divViewSpread.Visible = false;
            rptprint1.Visible = false;
            bool isfromDate = false, isToDate = false;
            int degselcount = 0;
            int secselcount = 0;
            int selhourcount = 0;
            bool success = false;

            string selqueries = string.Empty;
            string periods = string.Empty;

            DateTime dtFrom = new DateTime();
            DateTime dtTo = new DateTime();
            DateTime dtDummyFrom = new DateTime();


            string val = "Date";
            if (ddlCollege.Items.Count == 0)
            {
                lblpoperr.Text = "College is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            }
            if (ddlbatch.Items.Count == 0)
            {
                lblpoperr.Text = "Batch Year is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                batch_year = Convert.ToString(ddlbatch.SelectedValue);
            }
            if (cblDegree.Items.Count == 0)
            {
                lblpoperr.Text = "Degree is not found";
                popupdiv.Visible = true;
                return;
            }
            if (cblBranch.Items.Count == 0)
            {
                lblpoperr.Text = "Branch is not found";
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
                lblpoperr.Text = "Please Select Any One Branch";
                popupdiv.Visible = true;
                return;
            }
            if (ddlsem.Items.Count == 0)
            {
                lblpoperr.Text = "Semester is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                semester = Convert.ToString(ddlsem.SelectedValue);
            }

            if (txtsection.Text != "---Select---")
            {
                section = string.Empty;
                foreach (ListItem li in chklstsection.Items)
                {
                    if (li.Selected)
                    {
                        secselcount++;
                        if (section == "")
                        {
                            if (li.Value != "0")
                                section = "'" + li.Value + "'";
                            else
                                section = "''";
                        }
                        else
                        {
                            if (li.Value != "0")
                                section += ",'" + li.Value + "'";
                            else
                                section += ",''";
                        }
                    }
                }
            }
            if (secselcount == 0)
            {
                lblpoperr.Text = "Please Select Any One Section";
                popupdiv.Visible = true;
                return;
            }
            if (rblSplFree.SelectedValue == "0")
            {
                val = "Date";
                if (cbPeriods.Checked)
                {
                    if (cblPeriod.Items.Count == 0)
                    {
                        lblpoperr.Text = "Periods are not found";
                        popupdiv.Visible = true;
                        return;
                    }
                    else
                    {
                        foreach (ListItem li in cblPeriod.Items)
                        {
                            if (li.Selected)
                            {
                                selhourcount++;
                                if (periods == "")
                                {
                                    periods = li.Value;
                                }
                                else
                                {
                                    periods += "," + li.Value;
                                }
                            }
                        }
                        selqueries = " and period in (" + periods + ")";
                    }
                    if (selhourcount == 0)
                    {
                        lblpoperr.Text = "Please Select Any One Period And Then Proceed";
                        popupdiv.Visible = true;
                        return;
                    }
                }
                else
                {
                    if (selqueries == "")
                    {
                        selqueries = string.Empty;
                    }
                    else
                    {
                        selqueries = selqueries;
                    }
                }
            }
            else
            {
                val = "From Date";
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
                if (!isToDate)
                {
                    lblpoperr.Text = "To Date Must Be In The Format dd/MM/yyyy Only!";
                    popupdiv.Visible = true;
                    return;
                }
            }


            if (txtFromDate.Text.Trim() != "")
            {
                isfromDate = DateTime.TryParseExact(txtFromDate.Text, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFrom);
                dtDummyFrom = dtFrom;
            }
            else
            {
                lblpoperr.Text = "Please Select " + val;
                popupdiv.Visible = true;
                return;
            }
            if (!isfromDate)
            {
                lblpoperr.Text = val + " Must Be In The Format dd/MM/yyyy Only!";
                popupdiv.Visible = true;
                return;
            }
            if (cbreason.Checked)
            {
                if (ddlpurpose.Items.Count == 0)
                {
                    lblpoperr.Text = "Please Add Reason And Then Proceed";
                    popupdiv.Visible = true;
                    return;
                }
                else
                {
                    if (selqueries == "")
                    {
                        selqueries = " and reason='" + Convert.ToString(ddlpurpose.SelectedItem) + "'";
                    }
                    else
                    {
                        selqueries += " and reason='" + Convert.ToString(ddlpurpose.SelectedItem) + "' ";
                    }
                }
            }

            if (collegecode != "" && batch_year != "" && degree_code != "" && semester != "" && degselcount != 0 && secselcount != 0)
            {
                Init_Spread();
                if (rblSplFree.SelectedValue == "0")
                {
                    if (isfromDate)
                    {
                        qry = "select batch_year,d.degree_code,c.Course_Name,de.Dept_Name,semester,section,Convert(nvarchar(15),entry_date,103) entry_date,period,reason,c.Priority,c.college_code from tbl_spl_attendace a,Degree d,course c,Department de where a.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and de.Dept_Code=d.Dept_Code and batch_year in(" + batch_year + ") and  d.degree_code in (" + degree_code + ") and semester in (" + semester + ") and section in(" + section + ")  and entry_date='" + dtFrom.ToString("MM/dd/yyyy") + "' " + selqueries + "  and attype='0'  order by c.college_code,batch_year desc,c.Priority,d.Degree_Code,period,entry_date desc";
                        //and period in (" + periods + ") and reason='" + Convert.ToString(ddlpurpose.SelectedItem) + "'
                        ds.Reset();
                        ds.Dispose();
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            FpViewSpread.Sheets[0].RowCount++;
                            FarPoint.Web.Spread.CheckBoxCellType chktypeall = new FarPoint.Web.Spread.CheckBoxCellType();
                            chktypeall.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType chktype = new FarPoint.Web.Spread.CheckBoxCellType();
                            FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].CellType = chktypeall;
                            FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            FpViewSpread.SaveChanges();
                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                string coll_code = Convert.ToString(ds.Tables[0].Rows[row]["college_code"]);
                                string batch = Convert.ToString(ds.Tables[0].Rows[row]["batch_year"]);
                                string deg_code = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                                string coursename = Convert.ToString(ds.Tables[0].Rows[row]["Course_Name"]);
                                string dept_name = Convert.ToString(ds.Tables[0].Rows[row]["Dept_Name"]);
                                string sem = Convert.ToString(ds.Tables[0].Rows[row]["semester"]);
                                string sec = Convert.ToString(ds.Tables[0].Rows[row]["section"]);
                                string date = Convert.ToString(ds.Tables[0].Rows[row]["entry_date"]);
                                string reason = Convert.ToString(ds.Tables[0].Rows[row]["reason"]);
                                string period = Convert.ToString(ds.Tables[0].Rows[row]["period"]);
                                FpViewSpread.Sheets[0].RowCount++;


                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].Tag = coll_code;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].Text = batch;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].Text = coursename;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].Tag = deg_code;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].Text = dept_name;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].Text = sem;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].Text = sec;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].Text = reason.Trim();
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].Text = date;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 8].Text = period;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 8].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].CellType = chktype;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

                            }
                            FpViewSpread.Sheets[0].PageSize = FpViewSpread.Sheets[0].RowCount;
                            FpViewSpread.Height = (FpViewSpread.Sheets[0].RowCount * 25) + 50;
                            if ((FpViewSpread.Sheets[0].RowCount * 25) + 50 < 500)
                                FpViewSpread.Height = 500;
                            FpViewSpread.SaveChanges();
                            FpViewSpread.Visible = true;
                            divViewSpread.Visible = true;
                            rptprint1.Visible = true;
                            lblpoperr.Text = string.Empty;
                            popupdiv.Visible = false;
                        }
                        else
                        {
                            lblpoperr.Text = "No Record(s) Found";
                            popupdiv.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblpoperr.Text = "Please Check Date";
                        popupdiv.Visible = true;
                        return;
                    }
                }
                else if (rblSplFree.SelectedValue == "1")
                {
                    if (isfromDate && isToDate)
                    {
                        qry = "select distinct batch_year,d.degree_code,c.Course_Name,de.Dept_Name,semester,section,Convert(nvarchar(15),entry_date,103) entry_date,reason,c.Priority,c.college_code from tbl_spl_attendace a,Degree d,course c,Department de where a.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and de.Dept_Code=d.Dept_Code and batch_year in(" + batch_year + ") and  d.degree_code in (" + degree_code + ") and semester in (" + semester + ") and section in(" + section + ") and entry_date>='" + dtFrom.ToString("MM/dd/yyyy") + "' and entry_date<='" + dtTo.ToString("MM/dd/yyyy") + "' and attype='1' " + selqueries + " order by c.college_code,batch_year desc,c.Priority,d.Degree_Code,entry_date desc";

                        //and reason='" + Convert.ToString(ddlpurpose.SelectedItem) + "' 
                        ds.Reset();
                        ds.Dispose();
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            FpViewSpread.Sheets[0].RowCount++;
                            FarPoint.Web.Spread.CheckBoxCellType chktypeall = new FarPoint.Web.Spread.CheckBoxCellType();
                            chktypeall.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType chktype = new FarPoint.Web.Spread.CheckBoxCellType();
                            FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].CellType = chktypeall;
                            FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            FpViewSpread.SaveChanges();
                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                string coll_code = Convert.ToString(ds.Tables[0].Rows[row]["college_code"]);
                                string batch = Convert.ToString(ds.Tables[0].Rows[row]["batch_year"]);
                                string deg_code = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                                string coursename = Convert.ToString(ds.Tables[0].Rows[row]["Course_Name"]);
                                string dept_name = Convert.ToString(ds.Tables[0].Rows[row]["Dept_Name"]);
                                string sem = Convert.ToString(ds.Tables[0].Rows[row]["semester"]);
                                string sec = Convert.ToString(ds.Tables[0].Rows[row]["section"]);
                                string date = Convert.ToString(ds.Tables[0].Rows[row]["entry_date"]);
                                string reason = Convert.ToString(ds.Tables[0].Rows[row]["reason"]);

                                FpViewSpread.Sheets[0].RowCount++;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].Tag = coll_code;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].Text = batch;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].Text = coursename;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].Tag = deg_code;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].Text = dept_name;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].Text = sem;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].Text = sec;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].Text = reason.Trim();
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].Text = date;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].Locked = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].CellType = chktype;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpViewSpread.Sheets[0].Cells[FpViewSpread.Sheets[0].RowCount - 1, FpViewSpread.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

                            }

                            FpViewSpread.Sheets[0].PageSize = FpViewSpread.Sheets[0].RowCount;
                            FpViewSpread.Height = (FpViewSpread.Sheets[0].RowCount * 25) + 50;
                            if ((FpViewSpread.Sheets[0].RowCount * 25) + 50 < 500)
                                FpViewSpread.Height = 500;
                            FpViewSpread.SaveChanges();
                            FpViewSpread.Visible = true;
                            divViewSpread.Visible = true;
                            rptprint1.Visible = true;
                            lblpoperr.Text = string.Empty;
                            popupdiv.Visible = false;

                        }
                        else
                        {
                            lblpoperr.Text = "No Record(s) Found";
                            popupdiv.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblpoperr.Text = "Please Check Date And Periods";
                        popupdiv.Visible = true;
                        return;
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

    #endregion View

    #region Delete

    protected void btnDeleteFreeSpl_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            popupdiv.Visible = false;
            string attendtype = string.Empty;
            bool isdelsuc = false;

            if (rblSplFree.SelectedValue == "0")
            {
                attendtype = "0";
            }
            else if (rblSplFree.SelectedValue == "1")
            {
                attendtype = "1";
            }
            int count = 0;
            FpViewSpread.SaveChanges();
            if (FpViewSpread.Sheets[0].RowCount > 0)
            {
                for (int row = 1; row < FpViewSpread.Sheets[0].RowCount; row++)
                {
                    int val = 0;
                    int.TryParse(Convert.ToString(FpViewSpread.Sheets[0].Cells[row, FpViewSpread.Sheets[0].ColumnCount - 1].Value), out val);
                    if (val == 1)
                    {
                        count++;
                        int del = 0;
                        string coll_code = Convert.ToString(FpViewSpread.Sheets[0].Cells[row, 0].Tag);
                        string batch = Convert.ToString(FpViewSpread.Sheets[0].Cells[row, 1].Text);
                        string deg_code = Convert.ToString(FpViewSpread.Sheets[0].Cells[row, 2].Tag);
                        string sem = Convert.ToString(FpViewSpread.Sheets[0].Cells[row, 4].Text);
                        string sec = Convert.ToString(FpViewSpread.Sheets[0].Cells[row, 5].Text);
                        string reason = Convert.ToString(FpViewSpread.Sheets[0].Cells[row, 6].Text);
                        string date = Convert.ToString(FpViewSpread.Sheets[0].Cells[row, 7].Text);
                        DateTime dtDate = new DateTime();
                        bool isdate = DateTime.TryParseExact(date, "dd/MM/yyyy", null, DateTimeStyles.None, out dtDate);
                        if (rblSplFree.SelectedValue == "0")
                        {
                            string period = Convert.ToString(FpViewSpread.Sheets[0].Cells[row, 8].Text);
                            qry = "delete from tbl_spl_attendace where college_code='" + coll_code + "' and attype='" + attendtype + "' and batch_year='" + batch + "' and degree_code='" + deg_code + "' and semester='" + sem + "' and section='" + sec + "' and entry_date='" + dtDate + "' and period='" + period + "' and reason='" + reason + "'";
                            del = d2.update_method_wo_parameter(qry, "text");
                            if (del > 0)
                            {
                                isdelsuc = true;
                            }
                        }
                        else if (rblSplFree.SelectedValue == "1")
                        {
                            qry = "delete from tbl_spl_attendace where college_code='" + coll_code + "' and attype='" + attendtype + "' and batch_year='" + batch + "' and degree_code='" + deg_code + "' and semester='" + sem + "' and section='" + sec + "' and entry_date='" + dtDate + "' and reason='" + reason + "'";
                            del = d2.update_method_wo_parameter(qry, "text");
                            if (del > 0)
                            {
                                isdelsuc = true;
                            }
                        }
                    }
                }
            }
            else
            {
                lblpoperr.Text = "No Record(s) Found";
                popupdiv.Visible = true;
                return;
            }
            if (count == 0)
            {
                lblpoperr.Text = "Please Select Any One Record And Then Proceed Delete";
                popupdiv.Visible = true;
                return;
            }
            if (isdelsuc)
            {
                btnView_Click(sender, e);
                lblpoperr.Text = "Deleted Succesfully!!!";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                btnView_Click(sender, e);
                lblpoperr.Text = "Not Deleted!!!";
                popupdiv.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Delete

    #region Popup Error

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            popupdiv.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Popup Error

    #region Save Reasons

    protected void btnAddReason_Click(object sender, EventArgs e)
    {
        try
        {
            bool issuc = false;
            divAddReason.Visible = true;
            CultureInfo culinfo = Thread.CurrentThread.CurrentCulture;
            TextInfo txtinfo = culinfo.TextInfo;
            //lblResult.Text = txtinfo.ToTitleCase(txtDetails.Text);
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
            string reason = Convert.ToString(txtinfo.ToTitleCase(txtAddReason.Text)).Trim();
            if (reason.Trim() != "")
            {
                qry = "if not exists(select * from textvaltable where college_code='" + collegecode + "' and TextVal='" + reason.Trim() + "' and TextCriteria='Attrs') insert into textvaltable (TextVal,TextCriteria,college_code) values('" + reason.Trim() + "','Attrs','" + collegecode + "') else update textvaltable set TextVal='" + reason.Trim() + "' where college_code='" + collegecode + "' and TextVal='" + reason.Trim() + "' and TextCriteria='Attrs'";
                //string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + reason.Trim() + "','Attrs','" + collegecode + "')";
                int a = d2.update_method_wo_parameter(qry, "Text");
                if (a > 0)
                {
                    issuc = true;
                }
                txtAddReason.Text = string.Empty;
                loadreason();
            }
            else
            {
                lblpoperr.Text = "Please Type The Reason And Then Proceed Add";
                popupdiv.Visible = true;
                return;
            }
            if (issuc)
            {
                divAddReason.Visible = false;
                lblpoperr.Text = "Reason Added Successfully!";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                divAddReason.Visible = false;
                lblpoperr.Text = "Reason Not Added!";
                popupdiv.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Save Reasons

    #region Close Reason Popup

    protected void btnReasonExit_Click(object sender, EventArgs e)
    {
        try
        {
            divAddReason.Visible = false;
            txtAddReason.Text = string.Empty;
        }
        catch (Exception ex)
        {

        }
    }

    #endregion Close Reason Popup

    #region Delete Reason

    protected void btnreasonre_Click(object sender, EventArgs e)
    {
        try
        {
            bool isdelsuc = false;
            divAddReason.Visible = false;
            popupdiv.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            string reason = string.Empty;
            if (ddlpurpose.Items.Count > 0)
            {
                string collegecode = Convert.ToString(ddlCollege.SelectedValue);
                reason = Convert.ToString(ddlpurpose.SelectedItem).Trim();
                //qry = "select * from Onduty_Stud where  Purpose='"+reason.Trim()+"' and college_code='"+collegecode+"'";
                //select * from tbl_spl_attendace where college_code='"+collegecode+"' and reason='"+reason.Trim()+"'
                string newqry = string.Empty;

                if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                {
                    newqry = "select * from tbl_spl_attendace where college_code='" + collegecode + "' and reason='" + reason.Trim() + "'";
                    qry = "select * from Onduty_Stud where  Purpose='" + reason.Trim() + "' and college_code='" + collegecode + "'";
                    if (!hasRecords(qry))
                    {
                        if (!hasRecords(newqry))
                        {

                            string strquery = "delete textvaltable where TextVal='" + reason.Trim() + "' and TextCriteria='Attrs' and college_code='" + collegecode + "'";
                            int a = d2.update_method_wo_parameter(strquery, "Text");
                            if (a > 0)
                            {
                                isdelsuc = true;
                            }
                            loadreason();
                        }
                        else
                        {
                            lblpoperr.Text = "The reason " + reason.Trim() + " is used in the free hour or special day  details.So,can't be deleted!!!";
                            popupdiv.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblpoperr.Text = "The reason " + reason.Trim() + " is used in the OD details.So,can't be deleted!!!";
                        popupdiv.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                lblpoperr.Text = "The Reason Is Not Found!";
                popupdiv.Visible = true;
                return;
            }
            if (isdelsuc)
            {
                lblpoperr.Text = "The Reason " + ((reason.Trim() != "") ? reason.Trim() : "") + " Is Deleted Successfully!";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                lblpoperr.Text = "The Reason " + ((reason.Trim() != "") ? reason.Trim() : "") + " Is Not Deleted!";
                popupdiv.Visible = true;
                return;
            }
            divAddReason.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Delete Reason

    #region Show Reason Add Popup

    protected void btnreasonset_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            popupdiv.Visible = false;
            divAddReason.Visible = true;
            txtAddReason.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Show Reason Add Popup

    #region Generate Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divAddReason.Visible = false;
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text.Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpViewSpread.Visible == true)
                {
                    d2.printexcelreport(FpViewSpread, reportname);
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
            popupdiv.Visible = false;
            divAddReason.Visible = false;
            string rptheadname = string.Empty;
            if (rblSplFree.SelectedValue == "0")
                rptheadname = "Free Hour Report";
            else if (rblSplFree.SelectedValue == "1")
                rptheadname = "Special Day Report";
            string pagename = "AttendanceSplDayFreeHrEntry.aspx";

            if (FpViewSpread.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpViewSpread, pagename, rptheadname);
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

    #endregion

    #region Reused Methods

    private void save(DataTable dsstud, string college_code, string batch_year, string degree_code, string semester, string section, string dtFrom, string dtTo, string hour, string reasonning, ref string error, ref bool result, DataTable dtAttBased = null)
    {
        try
        {
            Hashtable holiday_table = new Hashtable();
            DataSet ds2 = new DataSet();
            DataSet ds_holi = new DataSet();

            string frdate = Convert.ToString(dtFrom);
            string todate = Convert.ToString(dtTo);
            bool save_flag = false;
            string dt = frdate;
            string strholiday = string.Empty;
            string reason = reasonning.Trim();

            //string attendancebased = null;
            bool isbasedonAttendHour = false;
            if (rblSplFree.SelectedValue == "0" && dtAttBased != null)
            {
                isbasedonAttendHour = true;
            }
            else
            {
                isbasedonAttendHour = false;
            }

            if (reason == "" || reason == null)
            {
                reason = string.Empty;

            }
            else
            {
                reason = reasonning;
            }

            string[] dsplit = dt.Split(new Char[] { '/' });
            frdate = Convert.ToString(dsplit[2]) + "/" + Convert.ToString(dsplit[1]) + "/" + Convert.ToString(dsplit[0]);
            int demfcal = 0;
            int.TryParse(Convert.ToString(dsplit[2]), out demfcal);
            demfcal = demfcal * 12;
            int mon = 0;
            int.TryParse(Convert.ToString(dsplit[1]), out mon);
            int cal_from_date = demfcal + mon;
            string monthcal = Convert.ToString(cal_from_date);
            dt = todate;
            dsplit = dt.Split(new Char[] { '/' });
            todate = Convert.ToString(dsplit[2]) + "/" + Convert.ToString(dsplit[1]) + "/" + Convert.ToString(dsplit[0]);
            int demtcal = 0;
            int.TryParse(Convert.ToString(dsplit[2]), out demtcal);
            demtcal = demtcal * 12;
            int mon1 = 0;
            int.TryParse(Convert.ToString(dsplit[1]), out mon1);
            int cal_to_date = demfcal + mon1;
            DateTime per_from_date = Convert.ToDateTime(frdate);
            DateTime per_to_date = Convert.ToDateTime(todate);
            DateTime dumm_from_date = per_from_date;

            hat.Clear();
            hat.Add("degree_code", degree_code);
            hat.Add("sem", semester);
            hat.Add("from_date", frdate);
            hat.Add("to_date", todate);
            hat.Add("coll_code", college_code);
            int iscount = 0;

            string strquery = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate + "' and '" + todate + "' and degree_code=" + degree_code + " and semester=" + semester + "";
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
            int fhrs = 0;
            string hrs = d2.GetFunction("select no_of_hrs_I_half_day from periodattndschedule where degree_code='" + degree_code + "' and semester='" + semester + "'");
            if (hrs.Trim() != "" && hrs != null && hrs.Trim() != "0")
            {
                int.TryParse(hrs, out fhrs);
            }
            string leavhlf = string.Empty;

            int flag = 0;
            int reval = 0;
            bool leaveflag = false;

            if (dsstud.Rows.Count > 0)
            {
                for (int res = 0; res < dsstud.Rows.Count; res++)
                {

                    int isval = 0;
                    string s = string.Empty;
                    isval = 1;
                    if (isval == 1)
                    {
                        flag = 1;
                        string AppNo = Convert.ToString(dsstud.Rows[res]["app_no"]).Trim();
                        string stdRollno = Convert.ToString(dsstud.Rows[res]["Roll_no"]).Trim();
                        string stdregno = Convert.ToString(dsstud.Rows[res]["Reg_No"]).Trim();
                        string stdname = Convert.ToString(dsstud.Rows[res]["Stud_Name"]).Trim();
                        string stdsem = Convert.ToString(dsstud.Rows[res]["Current_Semester"]).Trim();
                        string collegeCodeNew = Convert.ToString(dsstud.Rows[res]["college_code"]).Trim();
                        string degreecode = degree_code;
                        string degree = "New Degree";
                        string branch = "Department";
                        string branchdeg = degree + "-" + branch;
                        int taken_hourse = 0;
                        string Attvalue = "1";
                        if (isbasedonAttendHour)
                        {
                            if (dtAttBased.Rows.Count > 0)
                            {
                                dtAttBased.DefaultView.RowFilter = "Roll_no='" + stdRollno + "'";
                                DataView dvatt = dtAttBased.DefaultView;
                                if (dvatt.Count > 0)
                                {
                                    string attval = Convert.ToString(dvatt[0]["Attend_based"]).Trim();
                                    if (attval.Trim() != "" && attval.Trim() != "0" && attval.Trim() != "-1")
                                    {
                                        Attvalue = attval;
                                    }
                                }
                            }
                        }

                        if (hour != "")
                        {
                            if (hour != "")
                            {
                                frdate = Convert.ToString(dtFrom);
                                todate = Convert.ToString(dtTo);
                                dt = frdate;
                                dsplit = dt.Split(new Char[] { '/' });

                                frdate = Convert.ToString(dsplit[2]) + "/" + Convert.ToString(dsplit[1]) + "/" + Convert.ToString(dsplit[0]);
                                demfcal = 0;
                                int.TryParse(Convert.ToString(dsplit[2]), out demfcal);
                                demfcal = demfcal * 12;
                                mon = 0;
                                int.TryParse(Convert.ToString(dsplit[1]), out mon);
                                cal_from_date = demfcal + mon;
                                monthcal = Convert.ToString(cal_from_date);
                                dt = todate;
                                dsplit = dt.Split(new Char[] { '/' });
                                todate = Convert.ToString(dsplit[2]) + "/" + Convert.ToString(dsplit[1]) + "/" + Convert.ToString(dsplit[0]);
                                int mon3 = 0;
                                demtcal = 0;
                                int.TryParse(Convert.ToString(dsplit[2]), out demtcal);
                                demtcal = demfcal * 12;
                                int.TryParse(Convert.ToString(dsplit[1]), out mon3);
                                cal_to_date = demfcal + mon3;
                                per_from_date = Convert.ToDateTime(frdate);
                                per_to_date = Convert.ToDateTime(todate);
                                dumm_from_date = per_from_date;
                                Dictionary<string, StringBuilder[]> dicQueryValue = new Dictionary<string, StringBuilder[]>();
                                if (dumm_from_date <= per_to_date)
                                {
                                    while (dumm_from_date <= per_to_date)
                                    {
                                        StringBuilder sbQueryUpdate = new StringBuilder();
                                        StringBuilder sbQUeryInsertValue = new StringBuilder();
                                        StringBuilder sbQueryColumnName = new StringBuilder();
                                        int monthYearValue = 0;
                                        if (dumm_from_date.DayOfWeek == DayOfWeek.Sunday)
                                        {

                                        }
                                        if (!holiday_table.ContainsKey(dumm_from_date))
                                        {
                                            string dummfromdate = Convert.ToString(dumm_from_date);
                                            string[] fromdate1 = dummfromdate.Split(new char[] { ' ' });
                                            string fromdate2 = Convert.ToString(fromdate1[0]);
                                            string[] fromdate = fromdate2.Split(new char[] { '/' });
                                            string fromdatedate = Convert.ToString(fromdate[1]);
                                            string fromdatemonth = Convert.ToString(fromdate[0]);
                                            string fromdateyear = Convert.ToString(fromdate[2]);
                                            int monthyear = Convert.ToInt32(fromdatemonth) + Convert.ToInt32(fromdateyear) * 12;
                                            monthYearValue = monthyear;
                                            string valueupdate = string.Empty;
                                            string insertvalue = string.Empty;
                                            string odvalue = string.Empty;

                                            int totnoofhours = 0;

                                            string[] hourslimit = hour.Split(new char[] { ',' });
                                            totnoofhours = Convert.ToInt32(Convert.ToString(hourslimit.GetUpperBound(0))) + 1;
                                            taken_hourse = taken_hourse + totnoofhours;
                                            for (int i = 0; i < Convert.ToInt32(totnoofhours); i++)
                                            {
                                                string particularhrs = Convert.ToString(hourslimit[i]);
                                                string value = ("d" + fromdatedate + "d" + particularhrs);
                                                string selectddl_value = string.Empty;
                                                selectddl_value = string.Empty;
                                                //Attvalue = "1";
                                                if (valueupdate == "")
                                                {
                                                    valueupdate = value + "=" + Attvalue;
                                                }
                                                else
                                                {
                                                    valueupdate = valueupdate + "," + value + "=" + Attvalue;
                                                }

                                                if (insertvalue == "")
                                                {
                                                    insertvalue = value;
                                                }
                                                else
                                                {
                                                    insertvalue = insertvalue + "," + value;
                                                }

                                                if (odvalue == "")
                                                {
                                                    odvalue = Attvalue;
                                                }
                                                else
                                                {
                                                    odvalue = odvalue + "," + Attvalue;
                                                }
                                                //if (Attvalue.Trim() == "3")
                                                //{
                                                //    hat.Clear();
                                                //    hat.Add("AtWr_App_no", AppNo);
                                                //    hat.Add("AttWr_CollegeCode", ddlCollege.SelectedItem.Value);
                                                //    hat.Add("columnname", value);
                                                //    hat.Add("roll_no", stdRollno);
                                                //    hat.Add("month_year", monthyear);
                                                //    hat.Add("values", reason);
                                                //    strquery = "sp_ins_upd_student_attendance_reason";
                                                //    int insert = d2.insert_method(strquery, hat, "sp");
                                                //}
                                                //else
                                                //{
                                                //    hat.Clear();
                                                //    hat.Add("AtWr_App_no", AppNo);
                                                //    hat.Add("AttWr_CollegeCode", ddlCollege.SelectedItem.Value);
                                                //    hat.Add("columnname", value);
                                                //    hat.Add("roll_no", stdRollno);
                                                //    hat.Add("month_year", monthyear);
                                                //    hat.Add("values", reason);
                                                //    strquery = "sp_ins_upd_student_attendance_reason";
                                                //    int insert = d2.insert_method(strquery, hat, "sp");
                                                //}
                                            }
                                            if (!string.IsNullOrEmpty(insertvalue))
                                            {
                                                sbQueryColumnName.Append(insertvalue + ",");
                                            }
                                            if (!string.IsNullOrEmpty(odvalue))
                                            {
                                                sbQUeryInsertValue.Append(odvalue + ",");
                                            }
                                            if (!string.IsNullOrEmpty(valueupdate))
                                            {
                                                sbQueryUpdate.Append(valueupdate + ",");
                                            }
                                            //hat.Clear();
                                            //hat.Add("Att_App_no", AppNo);
                                            //hat.Add("Att_CollegeCode", ddlCollege.SelectedItem.Value);
                                            //hat.Add("rollno", stdRollno);
                                            //hat.Add("monthyear", monthyear);
                                            //hat.Add("columnname", insertvalue);
                                            //hat.Add("colvalues", odvalue);
                                            //hat.Add("coulmnvalue", valueupdate);
                                            //int savevalue = d2.insert_method("sp_ins_upd_student_attendance_Dead", hat, "sp");
                                            //if (savevalue > 0)
                                            //{
                                            //    save_flag = true;
                                            //}
                                        }
                                        else
                                        {
                                            int starthout = 0;
                                            taken_hourse = 0;
                                            string strholyquery = "select halforfull,morning,evening from holidaystudents where halforfull=1 and holiday_date='" + dumm_from_date.ToString("MM/dd/yyyy") + "'";
                                            DataSet dsholidayval = d2.select_method_wo_parameter(strholyquery, "Text");
                                            if (dsholidayval.Tables.Count > 0 && dsholidayval.Tables[0].Rows.Count > 0)
                                            {
                                                string sethours = string.Empty;
                                                string[] sphrsp = hour.Split(',');
                                                for (int sph = 0; sph <= sphrsp.GetUpperBound(0); sph++)
                                                {
                                                    int sehrou = Convert.ToInt32(sphrsp[sph]);
                                                    if (sehrou <= fhrs)
                                                    {
                                                        if (Convert.ToString(dsholidayval.Tables[0].Rows[0]["morning"]) == "1" || Convert.ToString(dsholidayval.Tables[0].Rows[0]["morning"]).Trim().ToLower() == "true")
                                                        {

                                                        }
                                                        else
                                                        {
                                                            taken_hourse = taken_hourse + 1;
                                                            if (sethours == "")
                                                            {
                                                                sethours = Convert.ToString(sehrou);
                                                            }
                                                            else
                                                            {
                                                                sethours = sethours + ',' + Convert.ToString(sehrou);
                                                            }
                                                        }

                                                    }
                                                    if (sehrou > fhrs)
                                                    {
                                                        if (Convert.ToString(dsholidayval.Tables[0].Rows[0]["evening"]) == "1" || Convert.ToString(dsholidayval.Tables[0].Rows[0]["evening"]).Trim().ToLower() == "true")
                                                        {

                                                        }
                                                        else
                                                        {
                                                            taken_hourse = taken_hourse + 1;
                                                            if (sethours == "")
                                                            {
                                                                sethours = Convert.ToString(sehrou);
                                                            }
                                                            else
                                                            {
                                                                sethours = sethours + ',' + Convert.ToString(sehrou);
                                                            }
                                                        }
                                                    }
                                                }
                                                if (sethours != "")
                                                {
                                                    int totnoofhours = 0;
                                                    string[] hourslimit = sethours.Split(new char[] { ',' });
                                                    totnoofhours = Convert.ToInt32(Convert.ToString(hourslimit.GetUpperBound(0))) + 1;

                                                    string dummfromdate = Convert.ToString(dumm_from_date);
                                                    string[] fromdate1 = dummfromdate.Split(new char[] { ' ' });
                                                    string fromdate2 = Convert.ToString(fromdate1[0]);
                                                    string[] fromdate = fromdate2.Split(new char[] { '/' });
                                                    string fromdatedate = Convert.ToString(fromdate[1]);
                                                    string fromdatemonth = Convert.ToString(fromdate[0]);
                                                    string fromdateyear = Convert.ToString(fromdate[2]);
                                                    int monthyear = Convert.ToInt32(fromdatemonth) + Convert.ToInt32(fromdateyear) * 12;
                                                    monthYearValue = monthyear;
                                                    string valueupdate = string.Empty;
                                                    string insertvalue = string.Empty;
                                                    string odvalue = string.Empty;
                                                    for (int i = starthout; i < Convert.ToInt32(totnoofhours); i++)
                                                    {
                                                        string particularhrs = Convert.ToString(hourslimit[i]);
                                                        string value = ("d" + fromdatedate + "d" + particularhrs);
                                                        string selectddl_value = string.Empty;
                                                        // Attvalue = "1";
                                                        if (valueupdate == "")
                                                        {
                                                            valueupdate = value + "=" + Attvalue;
                                                        }
                                                        else
                                                        {
                                                            valueupdate = valueupdate + "," + value + "=" + Attvalue;
                                                        }

                                                        if (insertvalue == "")
                                                        {
                                                            insertvalue = value;
                                                        }
                                                        else
                                                        {
                                                            insertvalue = insertvalue + "," + value;
                                                        }

                                                        if (odvalue == "")
                                                        {
                                                            odvalue = Attvalue;
                                                        }
                                                        else
                                                        {
                                                            odvalue = odvalue + "," + Attvalue;
                                                        }
                                                        //if (Attvalue.Trim() == "3")
                                                        //{
                                                        //    hat.Clear();
                                                        //    hat.Add("AtWr_App_no", AppNo);
                                                        //    hat.Add("AttWr_CollegeCode", ddlCollege.SelectedItem.Value);
                                                        //    hat.Add("columnname", value);
                                                        //    hat.Add("roll_no", stdRollno);
                                                        //    hat.Add("month_year", monthyear);
                                                        //    hat.Add("values", reason);
                                                        //    strquery = "sp_ins_upd_student_attendance_reason";
                                                        //    int insert = d2.insert_method(strquery, hat, "sp");
                                                        //}
                                                    }
                                                    if (!string.IsNullOrEmpty(insertvalue))
                                                    {
                                                        sbQueryColumnName.Append(insertvalue + ",");
                                                    }
                                                    if (!string.IsNullOrEmpty(odvalue))
                                                    {
                                                        sbQUeryInsertValue.Append(odvalue + ",");
                                                    }
                                                    if (!string.IsNullOrEmpty(valueupdate))
                                                    {
                                                        sbQueryUpdate.Append(valueupdate + ",");
                                                    }
                                                    //hat.Clear();
                                                    //hat.Add("Att_App_no", AppNo);
                                                    //hat.Add("Att_CollegeCode", ddlCollege.SelectedItem.Value);
                                                    //hat.Add("rollno", stdRollno);
                                                    //hat.Add("monthyear", monthyear);
                                                    //hat.Add("columnname", insertvalue);
                                                    //hat.Add("colvalues", odvalue);
                                                    //hat.Add("coulmnvalue", valueupdate);
                                                    //int savevalue = d2.insert_method("sp_ins_upd_student_attendance_Dead", hat, "sp");
                                                    //if (savevalue > 0)
                                                    //{
                                                    //    save_flag = true;
                                                    //}
                                                }
                                                if (leaveflag == false && sethours == "")
                                                {
                                                    if (strholiday == "")
                                                    {
                                                        strholiday = "Holiday(s) are : " + dumm_from_date.ToString("dd/MM/yyyy") + "(Half day Holiday)";
                                                    }
                                                    else
                                                    {
                                                        strholiday = strholiday + "," + dumm_from_date.ToString("dd/MM/yyyy") + "(Half day Holiday)";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (leaveflag == false)
                                                {
                                                    if (strholiday == "")
                                                    {
                                                        strholiday = "Holiday(s) are : " + dumm_from_date.ToString("dd/MM/yyyy");
                                                    }
                                                    else
                                                    {
                                                        strholiday = strholiday + "," + dumm_from_date.ToString("dd/MM/yyyy");
                                                    }
                                                }
                                            }
                                        }
                                        StringBuilder[] sbAll = new StringBuilder[3];
                                        if (!string.IsNullOrEmpty(sbQueryColumnName.ToString().Trim()) && !string.IsNullOrEmpty(sbQUeryInsertValue.ToString().Trim()) && !string.IsNullOrEmpty(sbQueryUpdate.ToString().Trim()))
                                        {
                                            if (dicQueryValue.ContainsKey(monthYearValue.ToString().Trim()))
                                            {
                                                sbAll = dicQueryValue[monthYearValue.ToString().Trim()];
                                                sbAll[0].Append(sbQueryColumnName);
                                                sbAll[1].Append(sbQUeryInsertValue);
                                                sbAll[2].Append(sbQueryUpdate);
                                                dicQueryValue[monthYearValue.ToString().Trim()] = sbAll;
                                            }
                                            else if (monthYearValue != 0)
                                            {
                                                sbAll[0] = new StringBuilder();
                                                sbAll[1] = new StringBuilder();
                                                sbAll[2] = new StringBuilder();
                                                sbAll[0].Append(Convert.ToString(sbQueryColumnName));
                                                sbAll[1].Append(Convert.ToString(sbQUeryInsertValue));
                                                sbAll[2].Append(Convert.ToString(sbQueryUpdate));
                                                dicQueryValue.Add(monthYearValue.ToString().Trim(), sbAll);
                                            }
                                        }
                                        dumm_from_date = dumm_from_date.AddDays(1);
                                    }
                                    if (dicQueryValue.Count > 0)
                                    {
                                        StringBuilder[] spAll = new StringBuilder[3];
                                        foreach (KeyValuePair<string, StringBuilder[]> dicQueery in dicQueryValue)
                                        {
                                            spAll = new StringBuilder[3];
                                            string monthValue = dicQueery.Key;
                                            spAll = dicQueery.Value;
                                            string insertColumnName = spAll[0].ToString().Trim(',');
                                            string insertColumnValue = spAll[1].ToString().Trim(',');
                                            string updateColumnNameValue = spAll[2].ToString().Trim(',');
                                            if (Attvalue.Trim() == "3")
                                            {
                                                string[] splitColumn = insertColumnName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (string sp in splitColumn)
                                                {
                                                    //ht.Clear();
                                                    //ht.Add("AtWr_App_no", appNo);
                                                    //ht.Add("AttWr_CollegeCode", collegeCode);
                                                    //ht.Add("columnname", sp);
                                                    //ht.Add("roll_no", stdRollno);
                                                    //ht.Add("month_year", monthValue);
                                                    //ht.Add("values", reason);
                                                    //string strquery = "sp_ins_upd_student_attendance_reason";
                                                    //int insert = da.insert_method(strquery, ht, "sp");
                                                    //if (insert != 0)
                                                    //{
                                                    //    isSaveAttendance = true;
                                                    //}
                                                    hat.Clear();
                                                    hat.Add("AtWr_App_no", AppNo);
                                                    hat.Add("AttWr_CollegeCode", collegeCodeNew);
                                                    hat.Add("columnname", sp);
                                                    hat.Add("roll_no", stdRollno);
                                                    hat.Add("month_year", monthValue);
                                                    hat.Add("values", reason);
                                                    strquery = "sp_ins_upd_student_attendance_reason";
                                                    int insert = d2.insert_method(strquery, hat, "sp");
                                                    if (insert > 0)
                                                    {
                                                        save_flag = true;
                                                    }
                                                }
                                            }
                                            //ht.Clear();
                                            //ht.Add("Att_App_no", appNo);
                                            //ht.Add("Att_CollegeCode", collegeCode);
                                            //ht.Add("rollno", stdRollno);
                                            //ht.Add("monthyear", monthValue);
                                            //ht.Add("columnname", insertColumnName);
                                            //ht.Add("colvalues", insertColumnValue);
                                            //ht.Add("coulmnvalue", updateColumnNameValue);
                                            //savevalue = da.insert_method("sp_ins_upd_student_attendance_Dead", ht, "sp");
                                            //if (savevalue != 0)
                                            //{
                                            //    isSaveAttendance = true;
                                            //}
                                            hat.Clear();
                                            hat.Add("Att_App_no", AppNo);
                                            hat.Add("Att_CollegeCode", collegeCodeNew);
                                            hat.Add("rollno", stdRollno);
                                            hat.Add("monthyear", monthValue);
                                            hat.Add("columnname", insertColumnName);
                                            hat.Add("colvalues", insertColumnValue);
                                            hat.Add("coulmnvalue", updateColumnNameValue);
                                            int savevalue = d2.insert_method("sp_ins_upd_student_attendance_Dead", hat, "sp");
                                            if (savevalue > 0)
                                            {
                                                save_flag = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //lblnorec.Visible = true;
                                    //lblnorec.Text = "From date should be less than Todate";
                                }
                            }
                            else
                            {
                                //lblnorec.Visible = true;
                                //lblnorec.Text = "Select Hours";
                            }
                        }
                        leaveflag = true;
                    }
                }
                //if (error == "")
                //    error = strholiday;
                //else
                //{
                //    error += "\n" + strholiday;
                //}
            }
            if (flag == 0)
            {
                //lblnorec.Visible = true;
                //lblnorec.Text = "Select Students and Proceed";
            }
            if (save_flag)//save_flag == 
            {
                result = save_flag = true;
                //if (strholiday != "")
                //{
                //    lblnorec.Visible = true;
                //    lblnorec.Text = strholiday + " " + leavhlf;
                //}
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Successfully Updated')", true);
                //clear();
            }
            else
            {
                if (strholiday != "")
                {
                    //lblnorec.Visible = true;
                    //lblnorec.Text = strholiday;
                }
            }
        }
        catch (Exception ex)
        {

            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public bool isApplicableForFreeSpecial(string coll_code, string batch, string degree_code, string sem, string sec, string entrydate, string attendtype)
    {
        try
        {
            bool isApplicableForFreeSpecial = false;
            if (coll_code != "" && batch != "" && degree_code != "" && sem != "" && entrydate != "" && attendtype != "")
            {
                int tot;
                string appqry = "select Count(*) from tbl_spl_attendace where college_code='" + coll_code + "' and batch_year='" + batch + "' and degree_code='" + degree_code + "' and semester='" + sem + "' and section='" + sec + "' and entry_date='" + entrydate + "' and attype='" + attendtype + "'";
                string totcount = d2.GetFunctionv(appqry);
                bool isvalid = int.TryParse(totcount, out tot);
                if (isvalid)
                {
                    if (tot == 0)
                        isApplicableForFreeSpecial = true;
                }
                else
                {
                    isApplicableForFreeSpecial = false;
                }
            }
            return isApplicableForFreeSpecial;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            return false;
        }
    }

    public void isholidayCheck(string college_code, string degree_code, string semester, string frdate, out bool ishoilday, out bool isholimorn, out bool isholieven, out int fhrs)
    {
        ishoilday = false;
        isholimorn = false;
        isholieven = false;
        fhrs = 0;

        Hashtable holiday_table = new Hashtable();
        DataSet ds2 = new DataSet();
        DataSet ds_holi = new DataSet();
        DateTime dumm_from_date = new DateTime();

        try
        {
            string[] dsplit = frdate.Split(new Char[] { '/' });
            frdate = Convert.ToString(dsplit[2]) + "/" + Convert.ToString(dsplit[1]) + "/" + Convert.ToString(dsplit[0]);
            dumm_from_date = Convert.ToDateTime(frdate);

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

            if (ds2.Tables[0].Rows.Count > 0)
            {
                iscount = 0;
                int.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["cnt"]), out iscount);
            }

            hat.Add("iscount", iscount);
            ds_holi = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

            holiday_table.Clear();
            if (ds_holi.Tables[0].Rows.Count != 0)
            {
                for (int k = 0; k < ds_holi.Tables[0].Rows.Count; k++)
                {
                    if (!holiday_table.ContainsKey(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[0].Rows[k]["HOLI_DATE"]))))
                    {
                        holiday_table.Add(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[0].Rows[k]["HOLI_DATE"])), k);
                    }
                }
            }
            if (ds_holi.Tables[1].Rows.Count != 0)
            {
                for (int k = 0; k < ds_holi.Tables[1].Rows.Count; k++)
                {
                    if (!holiday_table.ContainsKey(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[1].Rows[k]["HOLI_DATE"]))))
                    {
                        holiday_table.Add(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[1].Rows[k]["HOLI_DATE"])), k);
                    }
                }
            }
            if (ds_holi.Tables[2].Rows.Count != 0)
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
                if (dsholidayval.Tables[0].Rows.Count > 0)
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public bool hasRecords(string q)
    {
        bool isHasRec = false;
        try
        {
            q = q.Trim();
            DataSet dsRec = new DataSet();
            if (q != "")
            {
                dsRec = d2.select_method_wo_parameter(q, "text");
                if (dsRec.Tables.Count > 0 && dsRec.Tables[0].Rows.Count > 0)
                {
                    isHasRec = true;
                }
                else
                {
                    isHasRec = false;
                }
            }
            return isHasRec;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            return isHasRec;
        }
    }

    public void ViewOrSave()
    {
        try
        {
            if (cbViewOrNot.Checked)
            {
                cbPeriods.Checked = false;
                cbPeriods.Visible = true;

                cbreason.Checked = false;
                cbreason.Visible = true;

                btnView.Visible = true;
                btnSave.Visible = false;

                ddlpurpose.Enabled = false;

                if (rblSplFree.SelectedValue == "0")
                {
                    txtPeriod.Enabled = false;
                    txtToDate.Visible = false;
                    txtPeriod.Visible = true;
                }
                else
                {
                    cbPeriods.Visible = false;
                    txtPeriod.Visible = false;
                    txtToDate.Visible = true;
                }


            }
            else
            {
                cbreason.Checked = false;
                cbreason.Visible = false;

                cbPeriods.Visible = false;
                cbPeriods.Checked = false;

                btnView.Visible = false;
                btnSave.Visible = true;

                ddlpurpose.Enabled = true;

                if (rblSplFree.SelectedValue == "0")
                {
                    txtPeriod.Enabled = true;
                    txtToDate.Visible = false;
                    txtPeriod.Visible = true;
                }
                else
                {
                    cbPeriods.Visible = false;
                    txtPeriod.Visible = false;
                    txtToDate.Visible = true;
                }
            }


            //if (cbViewOrNot.Checked)
            //{
            //    btnSave.Visible = false;
            //    btnView.Visible = true;
            //    if (cbreason.Checked)
            //    {
            //        ddlpurpose.Enabled = true;
            //    }
            //    else
            //    {
            //        ddlpurpose.Enabled = false;
            //    }
            //    if (rblSplFree.SelectedValue == "0")
            //    {
            //        cbPeriods.Visible = true;
            //        if (cbPeriods.Checked)
            //        {
            //            txtPeriod.Enabled = true;
            //        }
            //        else
            //        {
            //            txtPeriod.Enabled = false;
            //        }
            //    }
            //    else
            //    {
            //        cbPeriod.Enabled = false;
            //        cbPeriods.Visible = false;
            //    }
            //}
            //else
            //{
            //    btnSave.Visible = true;
            //    btnView.Visible = false;
            //    ddlpurpose.Enabled = true;
            //    cbPeriods.Enabled = false;
            //    cbreason.Enabled = false;
            //    cbreason.Visible = false;
            //    cbPeriods.Visible = false;
            //}
        }
        catch (Exception ex)
        {
        }
    }

    #endregion Reused Methods

}
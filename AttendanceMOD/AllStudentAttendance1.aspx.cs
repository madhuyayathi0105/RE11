using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using InsproDataAccess;

public partial class AllStudentAttendance1 : System.Web.UI.Page
{
    bool b_school = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    static string collegename = string.Empty;
    static string collacronym = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;
    int count = 0;
    DateTime confromdate1;
    bool frsthalf;
    bool sendhalf;
    bool hourcheck = false;
    bool halforfull;
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet data1 = new DataSet();
    DataSet noofhours = new DataSet();
    DataSet noofhours11 = new DataSet();
    DataSet attnmas = new DataSet();
    Hashtable hat = new Hashtable();
    Hashtable loadhour = new Hashtable();
    Hashtable loadhour12 = new Hashtable();
    Hashtable loadhour123 = new Hashtable();
    Hashtable loadhour1 = new Hashtable();
    Hashtable students = new Hashtable();
    Hashtable studentsabbsents = new Hashtable();
    DataView dv1 = new DataView();
    DataView dv2 = new DataView();
    DAccess2 da = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    static ArrayList roll_array = new ArrayList();
    static bool forschoolsetting = false;
    private WebProxy objProxy1 = null;
    ReuasableMethods rs = new ReuasableMethods();
    //String[] abbsentroll;

    string no_of_hrs = string.Empty;
    string sch_order = string.Empty;
    string srt_day = string.Empty;
    string startdate = string.Empty;
    string no_days = string.Empty;
    string starting_dayorder = string.Empty;
    string frst_half_day = string.Empty;
    string secd_half_day = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            errmsg.Visible = false;

            string grouporusercode = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = da.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                if (schoolvalue.Trim() == "0")
                {
                    b_school = true;
                }
            }

            //if (chkselectall.Checked == true)
            //{
            //    if (gvuserodrlist.Rows.Count > 0)
            //    {
            //        gvuserodrlist.Columns[6].Visible = false;
            //        gvuserodrlist.Columns[7].Visible = false;
            //    }
            //}
            //else
            //{
            //    if (gvuserodrlist.Rows.Count > 0)
            //    {
            //        gvuserodrlist.Columns[6].Visible = true;
            //        gvuserodrlist.Columns[7].Visible = true;
            //    }
            //}

            if (!IsPostBack)
            {
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["AdmissionNo"] = "0";
                sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = da.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                    if (schoolvalue.Trim() == "0")
                    {
                        // chkselectall.Attributes.Add("style", "    font-family: Book Antiqua;   font-size: medium;    font-weight: bold; margin-left:-19px; position:relative;");
                        lblsec.Text = "Period";
                        lbldatch.Text = "Year :";
                        lbldegh.Text = "School Type :";
                        bldept.Text = "Standard :";
                        lblseee.Text = "Term & Sec :";
                        b_school = true;
                        forschoolsetting = false;
                        lblbach.Text = "Year";
                        Label1.Text = "School";
                        lblbranch.Text = "Standard";
                        lbldegree.Text = "School Type";
                        //lblStripHead.Text = "Test Mark Entry";
                        //txtsec.Attributes.Add("style", " font-family: 'Book Antiqua';    font-size: medium;    font-weight: bold;    height: 20px;    left: 832px;    position: absolute;    top: 180px;    width: 120px; ");
                        //ddlcollege.Attributes.Add("style", "  font-family: 'Book Antiqua';   font-size: medium;    font-weight: bold;    left: 82px;    position: absolute;    top: 180px;    width: 94px;");
                        //lblbach.Attributes.Add("style", "  color: Black;    font-family: 'Book Antiqua';    font-size: medium;    font-weight: bold;    left: 185px;    position: absolute;    top: 180px;");

                        //txtbatch.Attributes.Add("style", "     font-family: 'Book Antiqua';    font-size: medium;    font-weight: bold;    height: 20px;    left: 224px;    position: absolute;    top: 180px;    width: 120px;");
                        //lbldegree.Attributes.Add("style", "  color: Black;    font-family: 'Book Antiqua';    font-size: medium;    font-weight: bold;    left: 355px;    position: absolute;    top: 180px;");

                        //lbldegree.Attributes.Add("style", "  color: Black;    font-family: 'Book Antiqua';    font-size: medium;    font-weight: bold;    left: 355px;    position: absolute;    top: 180px;");

                        // margin-left: -6px; margin-top: -20px; position: absolute;width: 100px; height: 21px;
                        //ddlDegree.Attributes.Add("style", "margin-left: -36px;margin-top: 29px; position: absolute;");
                        //lblBranch.Attributes.Add("style", "margin-left: -27px; margin-top: 21px; position: absolute; height: 21px; width: 52px;");
                        //txtbranch.Attributes.Add("style", "margin-left: 30px;margin-top: -18px; position: absolute; height: 21px;");
                        //txtbranch.Attributes.Add("style", "font-family: 'Book Antiqua';    font-size: medium;    font-weight: bold;    height: 20px;    left: 654px;    position: absolute;    top: 180px;    width: 120px;");
                        //lblSemYr.Attributes.Add("style", "margin-left: 34px; margin-top: 37px; position: absolute; height: 20px; width: 33px;");
                        //ddlSemYr.Attributes.Add("style", " margin-left: -8px; margin-top: 38px; position: absolute; height: 21px; width: 44px; ");
                        //lblSec.Attributes.Add("style", "margin-left: -41px;margin-top: 37px; position: absolute; height: 21px; width: 30px;");
                        //ddlSec.Attributes.Add("style", " margin-left: -72px; margin-top: 36px; position: absolute; height: 21px; width: 47px;");
                        //btnGo.Attributes.Add("style", " margin-left: -70px; margin-top: 17px;position: absolute; height: 25px; width: 40px");
                        if (Label1.Text.Trim().ToUpper() == "SCHOOL")
                        {
                            ddlentry.SelectedIndex = ddlentry.Items.IndexOf(ddlentry.Items.FindByText("Admission No"));
                            //ddlentry.Enabled = false;
                        }
                        else
                        {
                            //ddlentry.Enabled = true;
                        }
                    }
                    else
                    {
                        forschoolsetting = false;
                    }
                }
                //} Sridharan
                chkselectall.Checked = false;
                chksms.Checked = false;
                chkvoice.Checked = false;
                txtfrom.Attributes.Add("readonly", "readonly");
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                chkselectall.Visible = false;
                chksms.Visible = false;
                chkvoice.Visible = false;
                Bindcollege();
                collegecode = ddlcollege.SelectedValue.ToString();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                if (txtdegree.Enabled == true)
                {
                    txtdegree.Enabled = true;
                    txtbranch.Enabled = true;
                    btngo.Enabled = true;
                    txtfrom.Enabled = true;
                    collegecode = ddlcollege.SelectedValue.ToString();
                    BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                    txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    txtdegree.Enabled = false;
                    txtbranch.Enabled = false;
                    btngo.Enabled = false;
                    txtfrom.Enabled = false;
                }
                Bindhour();
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void Bindcollege()
    {
        try
        {
            string columnfield = string.Empty;
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
            else
            {
                errmsg.Text = "Set college rights to the staff";
                errmsg.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void BindBatchOld()
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
                if (chkbatch.Checked == true)
                {
                    for (int i = 0; i < chklsbatch.Items.Count; i++)
                    {
                        chklsbatch.Items[i].Selected = true;
                        txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                        if (b_school == true)
                        {
                            txtbatch.Text = "Year(" + (chklsbatch.Items.Count) + ")";
                        }
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
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void BindBatch()
    {
        try
        {
            chklsbatch.Items.Clear();
            chkbatch.Checked = false;
            txtbatch.Text = "---Select---";

            string Master1 = string.Empty;
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = Convert.ToString(group_semi[0]);
                }
            }
            else
            {
                Master1 = Convert.ToString(Session["usercode"]).Trim();
            }
            string collegecode = Convert.ToString(ddlcollege.SelectedValue).Trim();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "'";

            DataSet ds = d2.select_method_wo_parameter(strbinddegree, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds;
                chklsbatch.DataTextField = "batch_year";
                chklsbatch.DataValueField = "batch_year";
                chklsbatch.DataBind();
                chklsbatch.SelectedIndex = chklsbatch.Items.Count - 1;
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                }
                if (chklsbatch.Items.Count == count)
                {
                    chkbatch.Checked = true;
                    txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                }
                if (chkbatch.Checked == true)
                {
                    for (int i = 0; i < chklsbatch.Items.Count; i++)
                    {
                        chklsbatch.Items[i].Selected = true;
                        txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                        if (b_school == true)
                        {
                            txtbatch.Text = "Year(" + (chklsbatch.Items.Count) + ")";
                        }
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
        }
        catch
        {
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            gvuserodrlist.Visible = false;
            divscrll1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            chkselectall.Visible = false;
            chksms.Visible = false;
            chkvoice.Visible = false;
            errmsg.Visible = false;
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
                if (chkdegree.Checked == true)
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = true;
                        txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                        if (b_school == true)
                        {
                            txtdegree.Text = "School Type(" + (chklstdegree.Items.Count) + ")";
                        }
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
            errmsg.Visible = true;
        }

    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;
            collegecode = ddlcollege.SelectedValue.ToString();
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
                if (chkbranch.Checked == true)
                {
                    for (int i = 0; i < chklstbranch.Items.Count; i++)
                    {
                        chklstbranch.Items[i].Selected = true;
                        txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                        if (b_school == true)
                        {
                            txtbranch.Text = "Standard(" + (chklstbranch.Items.Count) + ")";
                        }
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
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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
            txtsec.Text = "---Select---";
            chksec.Checked = false;
            string selecteedHour = GetAdminAttendanceHour();
            string[] arrAdminHourRights = selecteedHour.Split(',');
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
                    sqlbatchquery = " and si.batch_year  " + sqlbatch + "";
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
                    sqlbranchquery = " and pa.degree_code  " + sqlbranch + "";
                }
                else
                {
                    sqlbranchquery = " ";
                }
            }

            chklssec.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            string qeryss = "select max(pa.No_of_hrs_per_day) as No_of_hrs_per_day from PeriodAttndSchedule pa,seminfo si,Registration r where pa.degree_code=si.degree_code and r.degree_code=pa.degree_code and r.degree_code=si.degree_code and r.Batch_Year=si.batch_year and pa.semester=si.semester " + sqlbatchquery + " " + sqlbranchquery + " and  college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            ds2 = d2.select_method(qeryss, hat, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                int noofhour = 0;// Convert.ToInt16(ds2.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString());

                int.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["No_of_hrs_per_day"]), out noofhour);
                txtsec.Enabled = true;
                chklssec.Enabled = true;
                chksec.Checked = false;
                for (int i = 1; i <= noofhour; i++)
                {
                    chklssec.Items.Add(i.ToString());
                }
                if (chksec.Checked == true)
                {
                    for (int i = 0; i < chklssec.Items.Count; i++)
                    {
                        chklssec.Items[i].Selected = true;
                        txtsec.Text = "Hours(" + (chklssec.Items.Count) + ")";
                        if (b_school == true)
                        {
                            txtsec.Text = "Periods(" + (chklssec.Items.Count) + ")";
                        }
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
                foreach (ListItem li in chklssec.Items)
                {
                    li.Enabled = true;
                    if (arrAdminHourRights.Contains(li.Text))
                    {
                        li.Selected = true;
                    }
                    else
                    {
                        li.Enabled = false;
                    }
                }
            }
            else
            {
                chklssec.Enabled = false;
                txtsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            gvuserodrlist.Visible = false;
            divscrll1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            chkselectall.Visible = false;
            chksms.Visible = false;
            chkvoice.Visible = false;
            errmsg.Visible = false;
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                }
                txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                if (b_school == true)
                {
                    txtbatch.Text = "Year(" + (chklsbatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                }
                txtbatch.Text = "---Select---";
            }
            Bindhour();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gvuserodrlist.Visible = false;
            divscrll1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            chkselectall.Visible = false;
            chksms.Visible = false;
            chkvoice.Visible = false;
            errmsg.Visible = false;
            int commcount = 0;
            txtbatch.Text = "--Select--";
            chkbatch.Checked = false;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                if (b_school == true)
                {
                    txtbatch.Text = "Year(" + commcount.ToString() + ")";
                }
                if (commcount == chklsbatch.Items.Count)
                {
                    chkbatch.Checked = true;
                }
            }
            Bindhour();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            gvuserodrlist.Visible = false;
            divscrll1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            chkselectall.Visible = false;
            chksms.Visible = false;
            chkvoice.Visible = false;
            errmsg.Visible = false;
            collegecode = ddlcollege.SelectedValue.ToString();
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                if (b_school == true)
                {
                    txtdegree.Text = "School Type(" + (chklstdegree.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "---Select---";
                txtbranch.Text = "---Select---";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            Bindhour();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gvuserodrlist.Visible = false;
            divscrll1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            chkselectall.Visible = false;
            chksms.Visible = false;
            chkvoice.Visible = false;
            errmsg.Visible = false;
            collegecode = ddlcollege.SelectedValue.ToString();
            int commcount = 0;
            chkdegree.Checked = false;
            txtdegree.Text = "---Select---";
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                if (b_school == true)
                {
                    txtdegree.Text = "School Type(" + commcount.ToString() + ")";
                }
                if (commcount == chklstdegree.Items.Count)
                {
                    chkdegree.Checked = true;
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            Bindhour();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            gvuserodrlist.Visible = false;
            divscrll1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            chkselectall.Visible = false;
            chksms.Visible = false;
            chkvoice.Visible = false;
            errmsg.Visible = false;
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                }
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";

                if (b_school == true)
                {
                    txtbranch.Text = "Standard(" + (chklstbranch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                }
                txtbranch.Text = "---Select---";
            }
            Bindhour();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gvuserodrlist.Visible = false;
            divscrll1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            chkselectall.Visible = false;
            chksms.Visible = false;
            chkvoice.Visible = false;
            errmsg.Visible = false;
            string clg = string.Empty;
            int commcount = 0;
            txtbranch.Text = "--Select--";
            chkbranch.Checked = false;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                if (b_school == true)
                {
                    txtbranch.Text = "Standard(" + commcount.ToString() + ")";
                }
                if (commcount == chklstbranch.Items.Count)
                {
                    chkbranch.Checked = true;
                }
            }

            Bindhour();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void ddlcollege_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gvuserodrlist.Visible = false;
            divscrll1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            chkselectall.Visible = false;
            chksms.Visible = false;
            chkvoice.Visible = false;
            errmsg.Visible = false;
            collegecode = ddlcollege.SelectedValue.ToString();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            Bindhour();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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
                }
                txtsec.Text = "Hours(" + (chklssec.Items.Count) + ")";
                if (b_school == true)
                {
                    txtsec.Text = "Periods(" + (chklssec.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklssec.Items.Count; i++)
                {
                    chklssec.Items[i].Selected = false;
                }
                txtsec.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklstsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            chksec.Checked = false;
            txtsec.Text = "---Select---";
            for (int i = 0; i < chklssec.Items.Count; i++)
            {
                if (chklssec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtsec.Text = "Hours(" + commcount.ToString() + ")";
                if (b_school == true)
                {
                    txtsec.Text = "Periods(" + commcount.ToString() + ")";
                }
                if (commcount == chklssec.Items.Count)
                {
                    chksec.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            if (b_school == true)
            {
                this.gvuserodrlist.Columns[1].HeaderText = "Year";
                this.gvuserodrlist.Columns[2].HeaderText = "School Type";
                this.gvuserodrlist.Columns[3].HeaderText = "Standard";
                this.gvuserodrlist.Columns[4].HeaderText = "Term";
            }

            chkselectall.Checked = false;
            chksms.Checked = false;
            chkvoice.Checked = false;
            string sqlbatch = string.Empty;
            string sqlbatchquery = string.Empty;
            string sqlbatchquery1 = string.Empty;
            string sqlbranch = string.Empty;
            string sqlbranchquery = string.Empty;
            string sqlbranchquery1 = string.Empty;
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
            if (sqlbatch.Trim() != "")
            {
                sqlbatch = " in(" + sqlbatch + ")";
                sqlbatchquery = " and r.batch_year  " + sqlbatch + "";
                sqlbatchquery1 = " batch_year  " + sqlbatch + "";
            }
            else
            {
                sqlbatchquery = " ";

                gvuserodrlist.Visible = false;
                divscrll1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                chkselectall.Visible = false;
                chksms.Visible = false;
                chkvoice.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = " Please Select The Batch And Then Proceed";
                if (b_school == true)
                {
                    errmsg.Text = " Please Select The Year And Then Proceed";
                }
                return;
            }

            if (txtdegree.Text == "---Select---")
            {
                gvuserodrlist.Visible = false;
                divscrll1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                chkselectall.Visible = false;
                chksms.Visible = false;
                chkvoice.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Degree  And Then Proceed";
                if (b_school == true)
                {
                    errmsg.Text = "Please Select The School Type  And Then Proceed";
                }
                return;
            }
            itemcount = 0;
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

            if (sqlbranch.Trim() != "")
            {
                sqlbranch = " in(" + sqlbranch + ")";
                sqlbranchquery = " and r.degree_code  " + sqlbranch + "";
                sqlbranchquery1 = " and degree_code  " + sqlbranch + "";
            }
            else
            {
                sqlbranchquery = " ";
                gvuserodrlist.Visible = false;
                divscrll1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                chkselectall.Visible = false;
                chksms.Visible = false;
                chkvoice.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Branch  And Then Proceed";
                if (b_school == true)
                {
                    errmsg.Text = "Please Select The Standard  And Then Proceed";
                }
                return;
            }

            Boolean hrflag = false;
            for (int hrv = 0; hrv < chklssec.Items.Count; hrv++)
            {
                if (chklssec.Items[hrv].Selected == true)
                {
                    hrflag = true;
                }
            }
            if (hrflag == false)
            {
                gvuserodrlist.Visible = false;
                divscrll1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                chkselectall.Visible = false;
                chksms.Visible = false;
                chkvoice.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Hour And Then Proceed";
                if (b_school == true)
                {
                    errmsg.Text = "Please Select The Period And Then Proceed";
                }
                return;
            }

            //string[] spitdate = txtfrom.Text.Split('/');
            //Boolean daychek = daycheck(Convert.ToDateTime(spitdate[1] + '/' + spitdate[0] + '/' + spitdate[2]));
            ////if (Session["UserName"].ToString().Trim() == "admin")
            ////{
            ////    daychek = true;
            ////}
            //if (daychek == true)
            //{
            //    //lblMessage.Text = "Do you want mark attendance from " + fromdate + " to " + todate;
            //    //mpemsgboxsave.Show();
            //}
            //else
            //{
            //    errmsg.Visible = true;
            //    errmsg.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
            //    return;
            //}


            string secrights = string.Empty;
            Boolean secrightsflag = false;
            string collegecode = ddlcollege.SelectedValue.ToString();
            string ucode = string.Empty;
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                ucode = group_code;
            }
            else
            {
                ucode = Session["usercode"].ToString();
            }

            string strgetsec = d2.GetFunction("select sections from tbl_attendance_rights where " + sqlbatchquery1 + " and user_id='" + ucode + "' and college_code='" + collegecode + "'");
            if (strgetsec.Trim() != null && strgetsec.Trim() != "0")
            {
                string[] spsec = strgetsec.Split(',');
                for (int sp = 0; sp <= spsec.GetUpperBound(0); sp++)
                {
                    string valu = spsec[sp].ToString();
                    if (secrights.Trim().ToLower() == valu.Trim().ToLower())
                    {
                        secrightsflag = true;
                    }
                }
            }
            if (secrightsflag == false)
            {
                gvuserodrlist.Visible = false;
                divscrll1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                chkselectall.Visible = false;
                chksms.Visible = false;
                chkvoice.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "Please Set The Batch Year and Sections Rights For The User";
                if (b_school == true)
                {
                    errmsg.Text = "Please Set The Year and Sections Rights For The User";
                }
                return;
            }
            string[] splitfromdate = txtfrom.Text.Split(new Char[] { '/' });
            string chechfromdate = splitfromdate[1] + '/' + splitfromdate[0] + '/' + splitfromdate[2];
            DateTime confromdate = Convert.ToDateTime(chechfromdate);

            if (confromdate.ToString("dddd") == "Sunday")
            {
                gvuserodrlist.Visible = false;
                divscrll1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                chkselectall.Visible = false;
                chksms.Visible = false;
                chkvoice.Visible = false;
                errmsg.Text = "Selected Day is Sunday";
                errmsg.Visible = true;
                return;
            }
            if (confromdate > DateTime.Today)
            {
                gvuserodrlist.Visible = false;
                divscrll1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                chkselectall.Visible = false;
                chksms.Visible = false;
                chkvoice.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "You Can Not Mark Attendance For The Date Greater Than Today";
                errmsg.Visible = true;
                return;
            }
            string sqlquery = "select distinct count(distinct r.roll_no)as strength,(c.Course_Name+'-'+ dp.dept_acronym) as dept,c.Course_Name,dp.dept_acronym,r.current_semester,r.batch_year,r.degree_code,ltrim(rtrim(isnull(r.Sections,''))) as sections,dp.Dept_Name AS Dept_Name   from registration r,degree de,course c,department dp where r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code  " + sqlbatchquery + " " + sqlbranchquery + " and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' group by r.degree_code,r.batch_year,course_name,dept_acronym,current_semester,sections,dp.Dept_Name order by  r.batch_year desc,current_semester asc, r.degree_code,Sections asc";
            DataSet dsselect = new DataSet();
            dsselect = d2.select_method(sqlquery, hat, "Text");
            if (dsselect.Tables[0].Rows.Count > 0)
            {
                gvuserodrlist.DataSource = dsselect.Tables[0];
                gvuserodrlist.DataBind();
                gvuserodrlist.Visible = true;
                divscrll1.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = false;
                chkselectall.Visible = true;
                chksms.Visible = true;
                chkvoice.Visible = true;
            }
            else
            {
                gvuserodrlist.Visible = false;
                divscrll1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                chkselectall.Visible = false;
                chksms.Visible = false;
                chkvoice.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
                errmsg.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            gvuserodrlist.Visible = false;
            divscrll1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            chkselectall.Visible = false;
            chksms.Visible = false;
            chkvoice.Visible = false;
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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

    protected void txtfrom_TextChanged(object sender, EventArgs e)
    {
        gvuserodrlist.Visible = false;
        divscrll1.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        chkselectall.Visible = false;
        chksms.Visible = false;
        chkvoice.Visible = false;
        errmsg.Visible = false;
    }

    protected void grid_view_employee_RowCommad(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string sqlsec = string.Empty;
            string sqlsecquery = string.Empty;
            mdl_full_employee_details.Hide();
            pnl_employee_details.Visible = false;
            string chechfromdate1 = string.Empty;
            if (e.CommandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                Session["activevlues"] = index.ToString();

                #region Added By Malang Raja On Nov 03 2016

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["AdmissionNo"] = "0";

                #endregion

                Label batch_Year1 = (Label)gvuserodrlist.Rows[index].FindControl("lblbatch_Year");
                Label Course_Name1 = (Label)gvuserodrlist.Rows[index].FindControl("lblCourse_Name");
                Label Dept_Name1 = (Label)gvuserodrlist.Rows[index].FindControl("lblDept_Name");
                Label current_semester1 = (Label)gvuserodrlist.Rows[index].FindControl("lblcurrent_semester");
                Label sections1 = (Label)gvuserodrlist.Rows[index].FindControl("lblsections");
                Label degree_code1 = (Label)gvuserodrlist.Rows[index].FindControl("lblCourse_id");

                string batch_Year = batch_Year1.Text;
                string Course_Name = Course_Name1.Text;
                string Dept_Name = Dept_Name1.Text;
                string current_semester = current_semester1.Text;
                string sections = sections1.Text;
                string degree_code = degree_code1.Text;
                string sec = string.Empty;
                string sec1 = string.Empty;
                if (sections == "")
                {
                    // sec =string.Empty;
                    sec = "and ltrim(rtrim(isnull(Sections,'')))='" + sections + "'";
                    sec1 = string.Empty;
                }
                else
                {
                    sec1 = "-" + sections.ToString();
                    sec = "and ltrim(rtrim(isnull(Sections,'')))='" + sections + "'";
                }

                string[] splitfromdate = txtfrom.Text.Split(new Char[] { '/' });
                string chechfromdate = splitfromdate[1] + '/' + splitfromdate[0] + '/' + splitfromdate[2];
                DateTime confromdate = Convert.ToDateTime(chechfromdate);
                lblbaatch.Text = batch_Year;
                lbldegh1.Text = Course_Name;
                bldept1.Text = Dept_Name;
                lblseeemsec.Text = current_semester + sec1;

                string sqlquery1 = "select * from holidayStudents where holiday_date='" + chechfromdate + "' and degree_code='" + degree_code + "' and semester='" + current_semester + "'";
                DataSet dsselect11 = new DataSet();
                dsselect11 = d2.select_method(sqlquery1, hat, "Text");
                string strquery = "select CONVERT(varchar(50), start_date,105) as start_date,isnull(starting_dayorder,1) as starting_dayorder,schorder,nodays,No_of_hrs_per_day,min_hrs_per_day,p.no_of_hrs_I_half_day,p.no_of_hrs_II_half_day from seminfo s,periodattndschedule p where s.degree_code=p.degree_code and s.semester=p.semester and batch_year=" + batch_Year + " and s.degree_code=" + degree_code + " and s.semester=" + current_semester + "";
                noofhours = d2.select_method(strquery, hat, "Text");
                if (noofhours.Tables[0].Rows.Count > 0)
                {
                    sch_order = noofhours.Tables[0].Rows[0]["schorder"].ToString();
                    no_days = noofhours.Tables[0].Rows[0]["nodays"].ToString();
                    startdate = noofhours.Tables[0].Rows[0]["start_date"].ToString();
                    starting_dayorder = noofhours.Tables[0].Rows[0]["starting_dayorder"].ToString();
                    no_of_hrs = noofhours.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                    frst_half_day = noofhours.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                    secd_half_day = noofhours.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();

                    if (startdate == "")
                    {
                        gvdatass.Visible = false;
                        lblerrros.Visible = true;
                        lblerrros.Text = "Please Update Semester Information";
                        divscrll.Visible = false;
                        pnl_employee_details.Visible = true;
                        mdl_full_employee_details.Show();
                        btnok.Visible = false;
                        btnexits.Visible = true;
                        return;
                    }
                    else
                    {
                        string[] splitfromdate1 = startdate.Split(new Char[] { '-' });
                        chechfromdate1 = splitfromdate1[1] + '/' + splitfromdate1[0] + '/' + splitfromdate1[2];
                        confromdate1 = Convert.ToDateTime(chechfromdate1);
                    }
                }

                if (confromdate1 > confromdate)
                {
                    gvdatass.Visible = false;
                    lblerrros.Visible = true;
                    string ssdd = confromdate1.ToString("dd/MM/yyyy");
                    lblerrros.Text = "Select Date Greater Than Semester Date (" + ssdd + ")";
                    divscrll.Visible = false;
                    pnl_employee_details.Visible = true;
                    mdl_full_employee_details.Show();
                    btnok.Visible = false;
                    btnexits.Visible = true;
                    return;
                }
                if (dsselect11.Tables.Count > 0 && dsselect11.Tables[0].Rows.Count > 0)
                {
                    halforfull = Convert.ToBoolean(dsselect11.Tables[0].Rows[0]["halforfull"].ToString());
                    if (halforfull == true)
                    {
                        frsthalf = Convert.ToBoolean(dsselect11.Tables[0].Rows[0]["morning"].ToString());
                        sendhalf = Convert.ToBoolean(dsselect11.Tables[0].Rows[0]["evening"].ToString());

                        string fhours = string.Empty;
                        string shours = string.Empty;

                        if (txtsec.Text != "---Select---")
                        {
                            int itemcount = 0;
                            for (itemcount = 0; itemcount < chklssec.Items.Count; itemcount++)
                            {
                                if (chklssec.Items[itemcount].Selected == true)
                                {
                                    sqlsec = chklssec.Items[itemcount].Value.ToString();
                                    if (!loadhour.Contains(sqlsec))
                                    {
                                        loadhour.Add(sqlsec, sqlsec);
                                    }
                                }
                            }
                            if (frsthalf == true)
                            {
                                for (int i = 1; i <= Convert.ToInt16(frst_half_day); i++)
                                {
                                    string ckkng = i.ToString();
                                    if (loadhour.Contains(ckkng))
                                    {
                                        hourcheck = true;
                                        if (fhours == "")
                                        {
                                            fhours = ckkng.ToString();
                                        }
                                        else
                                        {
                                            fhours = fhours + "," + ckkng.ToString();
                                        }
                                    }
                                }
                            }
                            if (sendhalf == true)
                            {
                                for (int i = Convert.ToInt16(frst_half_day) + 1; i <= Convert.ToInt16(no_of_hrs); i++)
                                {
                                    string ckkngk = i.ToString();
                                    if (loadhour.Contains(ckkngk))
                                    {
                                        hourcheck = true;
                                        if (shours == "")
                                        {
                                            shours = ckkngk.ToString();
                                        }
                                        else
                                        {
                                            shours = shours + "," + ckkngk.ToString();
                                        }
                                    }
                                }
                            }
                            if (hourcheck == true)
                            {
                                string dds = string.Empty;
                                gvdatass.Visible = false;
                                lblerrros.Visible = true;
                                if (fhours != "")
                                {
                                    if (shours != "")
                                    {
                                        dds = fhours + "," + shours;
                                    }
                                    else
                                    {
                                        dds = fhours;
                                    }
                                }
                                else if (shours != "")
                                {
                                    dds = shours;
                                }
                                lblerrros.Text = "You can not mark attendance for " + dds + " Hours [Holiday]";
                                divscrll.Visible = false;
                                pnl_employee_details.Visible = true;
                                mdl_full_employee_details.Show();
                                btnok.Visible = false;
                                btnexits.Visible = true;
                                return;
                            }
                            else
                            {
                                string grouporusercode = string.Empty;
                                DataTable dtgd = new DataTable();
                                dtgd.Columns.Add("SSno");
                                dtgd.Columns.Add("Roll_No");
                                dtgd.Columns.Add("Stud_Name");
                                dtgd.Columns.Add("Roll_Noonly");
                                dtgd.Columns.Add("Reg_Noonly");
                                dtgd.Columns.Add("Roll_Admit");

                                string strorderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
                                if (strorderby == "")
                                {
                                    strorderby = string.Empty;
                                }
                                else
                                {
                                    if (strorderby == "0")
                                    {
                                        strorderby = "ORDER BY registration.Roll_No";
                                    }
                                    else if (strorderby == "1")
                                    {
                                        strorderby = "ORDER BY registration.Reg_No";
                                    }
                                    else if (strorderby == "2")
                                    {
                                        strorderby = "ORDER BY Stud_Name";
                                    }
                                    else if (strorderby == "0,1,2")
                                    {
                                        strorderby = "ORDER BY registration.Roll_No,registration.Reg_No,Stud_Name";
                                    }
                                    else if (strorderby == "0,1")
                                    {
                                        strorderby = "ORDER BY registration.Roll_No,registration.Reg_No";
                                    }
                                    else if (strorderby == "1,2")
                                    {
                                        strorderby = "ORDER BY registration.Reg_No,Stud_Name";
                                    }
                                    else if (strorderby == "0,2")
                                    {
                                        strorderby = "ORDER BY registration.Roll_No,Stud_Name";
                                    }
                                }
                                string serialorderby = d2.GetFunction("select LinkValue from InsSettings where LinkName like 'Student Attendance' and college_code = '" + Session["collegecode"] + "'");
                                string sqlquery = string.Empty;
                                if (serialorderby.Trim() == "1")
                                {
                                    //sqlquery = d2.GetFunction("select  top 1 Convert(nvarchar(50),ISNULL(CONVERT(varchar, serialno),'')) as SSno  from Registration where RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and Batch_Year='" + batch_Year + "' and  degree_code='" + degree_code + "' and Current_Semester='" + current_semester + "' " + sec + " order by SSno ");
                                    //if (sqlquery.Trim() != "")
                                    //{
                                    //sqlquery = "select distinct serialno as SSno,Convert(nvarchar(50),ISNULL(CONVERT(varchar, serialno),'')) as SSno + ' -', Roll_No,Reg_No,dbo.ProperCase(Stud_Name) as Stud_Name,len(serialno) from Registration where RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and Batch_Year='" + batch_Year + "' and  degree_code='" + degree_code + "' and Current_Semester='" + current_semester + "' " + sec + " order by len(serialno),serialno   ";

                                    sqlquery = "select distinct serialno as SSno,Convert(nvarchar(50),ISNULL(CONVERT(varchar, serialno),''))+ ' -' as SSno, Roll_No,Roll_No as Roll_Noonly,Reg_No, dbo.ProperCase(Stud_Name) as Stud_Name,Roll_Admit,len(serialno) from Registration where RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and Batch_Year='" + batch_Year + "' and  degree_code='" + degree_code + "' and Current_Semester='" + current_semester + "' " + sec + "  order by len(serialno),serialno ";
                                    //}
                                    //else
                                    //{
                                    //    sqlquery = "select distinct '' as SSno,Roll_No,Reg_No,dbo.ProperCase(Stud_Name) as Stud_Name from Registration where RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and Batch_Year='" + batch_Year + "' and  degree_code='" + degree_code + "' and Current_Semester='" + current_semester + "' " + sec + " order by Roll_No ";
                                    //}
                                }
                                else
                                {
                                    //sqlquery = "select distinct '' as SSno,Roll_No,Reg_No,dbo.ProperCase(Stud_Name) as Stud_Name from Registration where RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and Batch_Year='" + batch_Year + "' and  degree_code='" + degree_code + "' and Current_Semester='" + current_semester + "' " + sec + " " + strorderby + " ";

                                    sqlquery = "select distinct '' as SSno,Roll_No,Roll_No as Roll_Noonly,Reg_No, dbo.ProperCase(Stud_Name) as Stud_Name,Roll_Admit from Registration where RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and Batch_Year='" + batch_Year + "' and  degree_code='" + degree_code + "' and Current_Semester='" + current_semester + "' " + sec + "  " + strorderby + "";
                                }
                                DataSet dsselect1 = new DataSet();
                                dsselect1 = d2.select_method(sqlquery, hat, "Text");
                                if (dsselect1.Tables.Count > 0 && dsselect1.Tables[0].Rows.Count > 0)
                                {
                                    if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                                    {
                                        grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                                    }
                                    else
                                    {
                                        grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                                    }
                                    string Master1 = "select * from Master_Settings where " + grouporusercode + "";

                                    ds2 = d2.select_method(Master1, hat, "Text");
                                    if (ds2.Tables[0].Rows.Count > 0)
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
                                            }//Admission No
                                            #region Added By Malang Raja On Nov 03 2016

                                            if (ds2.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds2.Tables[0].Rows[i]["value"].ToString() == "1")
                                            {
                                                Session["AdmissionNo"] = "1";
                                            }

                                            #endregion
                                        }
                                    }

                                    string rollfg = string.Empty;
                                    for (int i = 0; i < dsselect1.Tables[0].Rows.Count; i++)
                                    {
                                        string regflag = dsselect1.Tables[0].Rows[i]["Reg_No"].ToString();
                                        string admissionno = dsselect1.Tables[0].Rows[i]["Roll_admit"].ToString();
                                        rollfg = string.Empty;
                                        if (Session["Rollflag"] == "1")
                                        {
                                            if (rollfg == "")
                                            {
                                                rollfg = dsselect1.Tables[0].Rows[i]["Roll_No"].ToString();
                                            }
                                        }
                                        if (Session["Regflag"] == "1")
                                        {
                                            if (rollfg == "")
                                            {
                                                rollfg = dsselect1.Tables[0].Rows[i]["Reg_No"].ToString();
                                            }
                                            else
                                            {
                                                rollfg = rollfg + "*" + dsselect1.Tables[0].Rows[i]["Reg_No"].ToString();
                                            }
                                        }

                                        #region Added By Malang Raja On Nov 03 2016

                                        if (Session["AdmissionNo"] == "1")
                                        {
                                            if (rollfg == "")
                                            {
                                                rollfg = dsselect1.Tables[0].Rows[i]["Roll_Admit"].ToString();
                                            }
                                            else
                                            {
                                                rollfg = rollfg + "*" + dsselect1.Tables[0].Rows[i]["Roll_Admit"].ToString();
                                            }
                                        }

                                        #endregion

                                        dtgd.Rows.Add(dsselect1.Tables[0].Rows[i]["SSno"].ToString(), rollfg, dsselect1.Tables[0].Rows[i]["Stud_Name"].ToString(), dsselect1.Tables[0].Rows[i]["Roll_Noonly"].ToString(), regflag);

                                    }
                                    if (dtgd.Rows.Count > 0)
                                    {
                                        gvdatass.DataSource = dtgd;
                                        gvdatass.DataBind();
                                        gvdatass.Visible = true;
                                        divscrll.Visible = true;
                                    }
                                    else
                                    {
                                        gvdatass.Visible = false;
                                    }
                                    //gvdatass.DataSource = dsselect1.Tables[0];
                                    //gvdatass.DataBind();
                                    //gvdatass.Visible = true;
                                    //divscrll.Visible = true;

                                }
                                lblerrros.Visible = false;
                                btnok.Visible = true;
                                pnl_employee_details.Visible = true;
                                mdl_full_employee_details.Show();
                            }
                        }
                        else
                        {
                            gvdatass.Visible = false;
                            lblerrros.Visible = true;
                            lblerrros.Text = "Please Select Hours";
                            divscrll.Visible = false;
                            pnl_employee_details.Visible = true;
                            mdl_full_employee_details.Show();
                            btnok.Visible = false;
                            btnexits.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        gvdatass.Visible = false;
                        lblerrros.Visible = true;
                        lblerrros.Text = "Selected Day is Holiday";
                        divscrll.Visible = false;
                        pnl_employee_details.Visible = true;
                        mdl_full_employee_details.Show();
                        btnok.Visible = false;
                        btnexits.Visible = true;
                        return;
                    }
                }
                else
                {
                    string strorderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
                    if (strorderby == "")
                    {
                        strorderby = string.Empty;
                    }
                    else
                    {
                        if (strorderby == "0")
                        {
                            strorderby = "ORDER BY registration.Roll_No";
                        }
                        else if (strorderby == "1")
                        {
                            strorderby = "ORDER BY registration.Reg_No";
                        }
                        else if (strorderby == "2")
                        {
                            strorderby = "ORDER BY Stud_Name";
                        }
                        else if (strorderby == "0,1,2")
                        {
                            strorderby = "ORDER BY registration.Roll_No,registration.Reg_No,Stud_Name";
                        }
                        else if (strorderby == "0,1")
                        {
                            strorderby = "ORDER BY registration.Roll_No,registration.Reg_No";
                        }
                        else if (strorderby == "1,2")
                        {
                            strorderby = "ORDER BY registration.Reg_No,Stud_Name";
                        }
                        else if (strorderby == "0,2")
                        {
                            strorderby = "ORDER BY registration.Roll_No,Stud_Name";
                        }
                    }
                    string serialorderby = d2.GetFunction("select LinkValue from InsSettings where LinkName like 'Student Attendance' and college_code = '" + Session["collegecode"] + "'");
                    string sqlquery = string.Empty;
                    string grouporusercode = string.Empty;
                    DataSet dsselect1 = new DataSet();
                    // dsselect1 = d2.select_method(sqlquery, hat, "Text");

                    DataTable dtgd = new DataTable();
                    dtgd.Columns.Add("SSno");
                    dtgd.Columns.Add("Roll_No");
                    dtgd.Columns.Add("Stud_Name");
                    dtgd.Columns.Add("Roll_Noonly");
                    dtgd.Columns.Add("Reg_Noonly");
                    dtgd.Columns.Add("Roll_Admit");
                    if (serialorderby.Trim() == "1")
                    {
                        //sqlquery = d2.GetFunction(" select Top 1 Convert(nvarchar(50),ISNULL(CONVERT(varchar, serialno),'')) as SSno from Registration where RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and Batch_Year='" + batch_Year + "' and  degree_code='" + degree_code + "' and Current_Semester='" + current_semester + "' " + sec + "  order by SSno ");
                        //if (sqlquery.Trim() != "")
                        //{
                        sqlquery = "select distinct serialno as SSno,Convert(nvarchar(50),ISNULL(CONVERT(varchar, serialno),''))+ ' -' as SSno, Roll_No,Roll_No as Roll_Noonly,Reg_No, dbo.ProperCase(Stud_Name) as Stud_Name,Roll_Admit,len(serialno) from Registration where RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and Batch_Year='" + batch_Year + "' and  degree_code='" + degree_code + "' and Current_Semester='" + current_semester + "' " + sec + "  order by len(serialno),serialno ";
                        //}
                        //else
                        //{
                        //    sqlquery = "select distinct '' as SSno,Roll_No,Reg_No, dbo.ProperCase(Stud_Name) as Stud_Name from Registration where RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and Batch_Year='" + batch_Year + "' and  degree_code='" + degree_code + "' and Current_Semester='" + current_semester + "' " + sec + "  order by Roll_No";
                        //}


                    }
                    else
                    {
                        sqlquery = "select distinct '' as SSno,Roll_No,Roll_No as Roll_Noonly,Reg_No, dbo.ProperCase(Stud_Name) as Stud_Name,Roll_Admit from Registration where RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and Batch_Year='" + batch_Year + "' and  degree_code='" + degree_code + "' and Current_Semester='" + current_semester + "' " + sec + "  " + strorderby + "";

                    }

                    dsselect1 = d2.select_method(sqlquery, hat, "Text");
                    if (dsselect1.Tables.Count > 0 && dsselect1.Tables[0].Rows.Count > 0)
                    {
                        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                        {
                            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                        }
                        else
                        {
                            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                        }
                        string Master1 = "select * from Master_Settings where " + grouporusercode + "";

                        ds2 = d2.select_method(Master1, hat, "Text");
                        if (ds2.Tables[0].Rows.Count > 0)
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
                                }//Admission No
                                #region Added By Malang Raja On Nov 03 2016

                                if (ds2.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds2.Tables[0].Rows[i]["value"].ToString() == "1")
                                {
                                    Session["AdmissionNo"] = "1";
                                }

                                #endregion
                            }
                        }

                        string rollfg = string.Empty;
                        for (int i = 0; i < dsselect1.Tables[0].Rows.Count; i++)
                        {
                            string regflag = dsselect1.Tables[0].Rows[i]["Reg_No"].ToString();
                            string admissionno = dsselect1.Tables[0].Rows[i]["Roll_admit"].ToString();
                            rollfg = string.Empty;
                            if (Session["Rollflag"] == "1")
                            {
                                if (rollfg == "")
                                {
                                    rollfg = dsselect1.Tables[0].Rows[i]["Roll_No"].ToString();
                                }
                            }
                            if (Session["Regflag"] == "1")
                            {
                                if (rollfg == "")
                                {
                                    rollfg = dsselect1.Tables[0].Rows[i]["Reg_No"].ToString();
                                }
                                else
                                {
                                    rollfg = rollfg + "*" + dsselect1.Tables[0].Rows[i]["Reg_No"].ToString();
                                }
                            }

                            #region Added By Malang Raja On Nov 03 2016

                            if (Session["AdmissionNo"] == "1")
                            {
                                if (rollfg == "")
                                {
                                    rollfg = dsselect1.Tables[0].Rows[i]["Roll_Admit"].ToString();
                                }
                                else
                                {
                                    rollfg = rollfg + "*" + dsselect1.Tables[0].Rows[i]["Roll_Admit"].ToString();
                                }
                            }
                            #endregion

                            dtgd.Rows.Add(dsselect1.Tables[0].Rows[i]["SSno"].ToString(), rollfg, dsselect1.Tables[0].Rows[i]["Stud_Name"].ToString(), dsselect1.Tables[0].Rows[i]["Roll_Noonly"].ToString(), regflag);
                        }
                    }
                    if (dtgd.Rows.Count > 0)
                    {

                        gvdatass.DataSource = dtgd;
                        gvdatass.DataBind();
                        gvdatass.Visible = true;
                        divscrll.Visible = true;
                    }
                    else
                    {
                        gvdatass.Visible = false;
                    }



                    //if (dsselect1.Tables.Count > 0 && dsselect1.Tables[0].Rows.Count > 0)
                    //{

                    //    gvdatass.DataSource = dsselect1.Tables[0];
                    //    gvdatass.DataBind();
                    //    gvdatass.Visible = true;
                    //    divscrll.Visible = true;
                    //}
                    //else
                    //{
                    //    gvdatass.Visible = false;
                    //}

                    lblerrros.Visible = false;
                    btnok.Visible = true;
                    pnl_employee_details.Visible = true;
                    mdl_full_employee_details.Show();
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string sqlsec = string.Empty;
        DataView roll = new DataView();
        DataView reg = new DataView();
        DataView stdtype = new DataView();
        string sqlroll = "Roll No";
        string sqlrgg = "Register No";
        string sqlstdty = "Student_Type";
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        foreach (DataListItem gvrow in gvdatass.Items)
        {
            CheckBox chkSelect = (gvrow.FindControl("chkup3") as CheckBox);
            if (chkSelect.Checked)
            {
                Label lblname1 = (Label)gvrow.FindControl("lblRoll_Noonly");
                Label lblreg1 = (Label)gvrow.FindControl("lblreg_noonly");
                //Label lblrolladmit = (Label)gvrow.FindControl("lblroll_admit");
                Label lblroll = (Label)gvrow.FindControl("lblRoll_No");

                string rollnos = lblroll.Text.ToLower();

                //if (ddlentry.SelectedItem.ToString() == "Reg No")
                //{
                //    rollnos = lblreg1.Text.ToLower();
                //}
                //if (ddlentry.SelectedItem.ToString() == "Roll No")
                //{
                //    rollnos = lblname1.Text.ToLower();
                //}
                //if (ddlentry.SelectedItem.ToString() == "Admission No")
                //{
                //    rollnos = lblRoll_No.Text.ToLower();
                //}

                if (sqlsec == "")
                {
                    sqlsec = "" + rollnos + "";
                }
                else
                {
                    sqlsec = sqlsec + "," + "" + rollnos + "";
                }
            }
        }
        if (sqlsec == "")
        {
            gvdatass.Visible = true;
            lblerrros.Visible = true;
            lblerrros.Text = "Select Atleast One Student";
            divscrll.Visible = true;
            pnl_employee_details.Visible = true;
            mdl_full_employee_details.Show();
            btnok.Visible = true;
            btnexits.Visible = true;
            return;
        }
        else if (sqlsec != "")
        {
            int i = Convert.ToInt16(Session["activevlues"].ToString());
            TextBox TextBox = (TextBox)gvuserodrlist.Rows[i].FindControl("txtabbsent");
            TextBox.Text = sqlsec.ToString();
        }
    }

    protected void Button3_Click(object sender, EventArgs e)
    {

    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable nohrsdetails = new DataTable();
            bool savefalg = false;
            int savevalue = 0;
            nohrsdetails.Columns.Clear();
            nohrsdetails.Columns.Add("Batch");
            nohrsdetails.Columns.Add("Dept");
            nohrsdetails.Columns.Add("Degree");

            nohrsdetails.Columns.Add("Sem");
            nohrsdetails.Columns.Add("sec");

            Boolean hrflag = false;
            for (int itemcount = 0; itemcount < chklssec.Items.Count; itemcount++)
            {
                if (chklssec.Items[itemcount].Selected == true)
                {
                    hrflag = true;
                }
            }
            if (hrflag == false)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Hour And Then Proceed";
                if (b_school == true)
                {
                    errmsg.Text = "Please Select The Period And Then Proceed";
                }
                return;
            }

            string invalidroll = string.Empty;
            string single_hour = string.Empty;
            string LeaveCodea = string.Empty;
            string DispTexta = string.Empty;
            string LeaveCodep = string.Empty;
            string DispTextp = string.Empty;
            string str_day = string.Empty;
            string Atmonth = string.Empty;
            string Atyear = string.Empty;
            string sqlsec = string.Empty;
            int month_year = 0;
            string min_frst_half_day = string.Empty;
            string min_secd_half_day = string.Empty;

            int noMaxHrsDay = 0;
            int noFstHrsDay = 0;
            int noSndHrsDay = 0;
            int noMinFstHrsDay = 0;
            int noMinSndHrsDay = 0;


            string collquery = "Select collname,Coll_acronymn from collinfo where college_code=" + ddlcollege.SelectedValue.ToString() + "";
            DataSet datacol = new DataSet();
            datacol.Clear();
            datacol = d2.select_method_wo_parameter(collquery, "Text");
            if (datacol.Tables[0].Rows.Count > 0)
            {
                collacronym = datacol.Tables[0].Rows[0]["Coll_acronymn"].ToString();
                collegename = datacol.Tables[0].Rows[0]["collname"].ToString();
            }
            string strquery1 = "Select * from AttMasterSetting where DispText='A'  or DispText='P' and CollegeCode='" + ddlcollege.SelectedValue.ToString() + "' order by DispText desc";
            attnmas = d2.select_method(strquery1, hat, "Text");

            if (attnmas.Tables.Count > 0 && attnmas.Tables[0].Rows.Count > 0)
            {
                //LeaveCodep = attnmas.Tables[0].Rows[0]["LeaveCode"].ToString();
                //DispTextp = attnmas.Tables[0].Rows[0]["DispText"].ToString();

                //LeaveCodea = attnmas.Tables[0].Rows[1]["LeaveCode"].ToString();
                //DispTexta = attnmas.Tables[0].Rows[1]["DispText"].ToString();

                LeaveCodep = "1";
                DispTextp = "P";

                LeaveCodea = "2";
                DispTexta = "A";

            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Update Attendance Master Setting";
                return;
            }

            errmsg.Visible = false;
            string[] splitfromdate = txtfrom.Text.Split(new Char[] { '/' });
            string chechfromdate = splitfromdate[1] + '/' + splitfromdate[0] + '/' + splitfromdate[2];
            string chechfromdate1 = splitfromdate[1] + '-' + splitfromdate[0] + '-' + splitfromdate[2];
            month_year = Convert.ToInt16(splitfromdate[2].ToString()) * 12 + Convert.ToInt16(splitfromdate[1].ToString());

            str_day = splitfromdate[0].ToString();
            Atmonth = splitfromdate[1].ToString();
            Atyear = splitfromdate[2].ToString();

            StringBuilder strPerDay = new StringBuilder();
            string strPerDays = string.Empty;
            bool hrcheck = false;
            for (int hrcnt = 1; hrcnt <= chklssec.Items.Count; hrcnt++)
            {
                int cnt = chklssec.Items.Count - 1;
                strPerDay.Append("d" + str_day.ToString().TrimStart('0') + "d" + hrcnt + ",");
                if (chklssec.Items[cnt].Selected == true)
                    hrcheck = true;
            }
            string strCheckedDays = string.Empty;
            if (strPerDay.Length > 0)
            {
                strPerDays = Convert.ToString(strPerDay);
                strCheckedDays = strPerDays.Replace(",", "<>'' and ");
                strCheckedDays += strPerDays.Replace(",", " is not null and ");
                strCheckedDays = strCheckedDays.Remove(strCheckedDays.Length - 4, 4);
                strPerDay.Remove(strPerDay.Length - 1, 1);
                strPerDays = Convert.ToString(strPerDay);


            }

            string strquery = "select CONVERT(varchar(50), start_date,105) as start_date,isnull(starting_dayorder,1) as starting_dayorder,schorder,nodays,No_of_hrs_per_day,min_hrs_per_day,p.no_of_hrs_I_half_day,p.no_of_hrs_II_half_day,s.batch_year,s.degree_code,s.semester,p.min_pres_I_half_day,p.min_pres_II_half_day from seminfo s,periodattndschedule p where s.degree_code=p.degree_code and s.semester=p.semester ";
            noofhours = d2.select_method(strquery, hat, "Text");

            string sqlquery1 = "select distinct Batch_Year,degree_code,Current_Semester,ltrim(rtrim(isnull(Sections,''))) as Sections,Roll_No,Reg_No,dbo.ProperCase(Stud_Name) as Stud_Name,Roll_Admit,Registration.app_no  from Registration  where  RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0;select *  from attendance where month_year='" + month_year + "'";
            DataSet dsselect14 = new DataSet();
            dsselect14 = d2.select_method(sqlquery1, hat, "Text");
            DataTable dtAttendanceDetails = new DataTable();
            if (!string.IsNullOrEmpty(strPerDays))
            {
                string qry = "select " + strPerDays + ",Att_App_no,r.Batch_Year,r.degree_code,r.Current_Semester,ltrim(rtrim(isnull(Sections,''))) as Sections from attendance a, Registration r where  Att_App_no=r.app_No and month_year='" + month_year + "' " + ((!string.IsNullOrEmpty(strCheckedDays.Trim())) ? " and " + strCheckedDays : "");
                dtAttendanceDetails = dirAcc.selectDataTable(qry);
            }
            //sqlquery1 = " select " + attDay + ",A.ROLL_NO,a.app_no from attendance a,registration r where r.roll_no =a.roll_no and r.college_code='" + collegecode + "' AND month_year='" + month_year + "'";

            DataView roll = new DataView();
            DataView reg = new DataView();
            DataView stdtype = new DataView();
            string sqlroll = "Roll No";
            string sqlrgg = "Register No";
            string sqlstdty = "Student_Type";
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            Boolean savsotnot = false;

            //if (chkselectall.Checked == false)
            //{
            foreach (GridViewRow gvrow in gvuserodrlist.Rows)
            {
                TextBox txtabbsentnew = (TextBox)gvrow.FindControl("txtabbsent");
                CheckBox presentall = (CheckBox)gvrow.FindControl("presentall");
                if (presentall.Checked == true)
                {
                    txtabbsentnew.Text = "00";
                }
                string txtabbsentnewA = txtabbsentnew.Text;
                if (txtabbsentnewA != "")
                {
                    if (presentall.Checked == true)
                    {
                        txtabbsentnew.Text = string.Empty;
                    }

                    string Att_dcolumna = string.Empty;
                    string Att_dcolumnp = string.Empty;
                    string Att_dcolumnainsert = string.Empty;
                    string Att_dcolumnpinsert = string.Empty;
                    string Att_dcolumnainsertr = string.Empty;
                    string Att_dcolumnpinsertr = string.Empty;

                    Label batch_Year1 = (Label)gvrow.FindControl("lblbatch_Year");
                    Label Course_Name1 = (Label)gvrow.FindControl("lblCourse_Name");
                    Label Dept_Name1 = (Label)gvrow.FindControl("lblDept_Name");
                    Label current_semester1 = (Label)gvrow.FindControl("lblcurrent_semester");
                    Label sections1 = (Label)gvrow.FindControl("lblsections");
                    Label degree_code1 = (Label)gvrow.FindControl("lblCourse_id");

                    string batch_Year = batch_Year1.Text;
                    string Course_Name = Course_Name1.Text;
                    string Dept_Name = Dept_Name1.Text;
                    string newdeptcource = Course_Name1.Text + "-" + Dept_Name1.Text;
                    string current_semester = current_semester1.Text;
                    string sections = sections1.Text;
                    string degree_code = degree_code1.Text;
                    string sec = string.Empty;
                    if (sections == "")
                    {
                        sec = string.Empty;
                    }
                    else
                    {
                        sec = "and isnull(Sections,'')='" + sections + "'";
                    }
                    noofhours.Tables[0].DefaultView.RowFilter = "batch_year=" + batch_Year + " and  degree_code=" + degree_code + " and semester=" + current_semester + "";
                    dv1 = noofhours.Tables[0].DefaultView;

                    #region saranya

                    string entrycode = Session["Entry_Code"].ToString();
                    string PageName = "Student Attendance Entry";
                    string batchYear = rs.GetSelectedItemsValueAsString(chklsbatch);
                    string degreeCode = rs.GetSelectedItemsValueAsString(chklstbranch);
                    string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");

                    #endregion

                    if (dv1.Count > 0)
                    {
                        sch_order = dv1[0]["schorder"].ToString();
                        no_days = dv1[0]["nodays"].ToString();
                        startdate = dv1[0]["start_date"].ToString();
                        starting_dayorder = dv1[0]["starting_dayorder"].ToString();
                        no_of_hrs = dv1[0]["No_of_hrs_per_day"].ToString();
                        frst_half_day = dv1[0]["no_of_hrs_I_half_day"].ToString();
                        secd_half_day = dv1[0]["no_of_hrs_II_half_day"].ToString();
                        min_frst_half_day = dv1[0]["min_pres_I_half_day"].ToString();
                        min_secd_half_day = dv1[0]["min_pres_II_half_day"].ToString();

                        int.TryParse(Convert.ToString(no_of_hrs), out noMaxHrsDay);
                        int.TryParse(Convert.ToString(frst_half_day), out noFstHrsDay);
                        int.TryParse(Convert.ToString(secd_half_day), out noSndHrsDay);
                        int.TryParse(Convert.ToString(min_frst_half_day), out noMinFstHrsDay);
                        int.TryParse(Convert.ToString(min_secd_half_day), out noMinSndHrsDay);

                    }
                    if (no_of_hrs.Trim() == "")
                    {
                        nohrsdetails.Rows.Add(batch_Year, Course_Name, Dept_Name, current_semester, sections1.Text);
                        goto skip;
                    }
                    for (int g = 1; g <= Convert.ToInt16(no_of_hrs); g++)
                    {
                        sqlsec = g.ToString();
                        if (!loadhour12.Contains(sqlsec))
                        {
                            loadhour12.Add(sqlsec, sqlsec);
                        }
                    }
                    Dictionary<string, string> dicabsen = new Dictionary<string, string>();
                    string hassabbrollno = string.Empty;
                    string hassprerollno = string.Empty;

                    string[] abbsentroll = txtabbsentnewA.Split(',');
                    for (int arr = 0; arr <= abbsentroll.GetUpperBound(0); arr++)
                    {
                        if (!dicabsen.ContainsKey(abbsentroll[arr].ToString().Trim().ToLower()))
                        {
                            dicabsen.Add(abbsentroll[arr].ToString().Trim().ToLower(), abbsentroll[arr].ToString().Trim().ToLower());
                        }
                    }


                    for (int itemcount = 0; itemcount < chklssec.Items.Count; itemcount++)
                    {
                        if (chklssec.Items[itemcount].Selected == true)
                        {
                            sqlsec = chklssec.Items[itemcount].Value.ToString();
                            if (loadhour12.Contains(sqlsec))
                            {
                                if (Att_dcolumna == "" || Att_dcolumna == "" || Att_dcolumnainsertr == "" || Att_dcolumnpinsertr == "" || Att_dcolumnainsert == "" || Att_dcolumnainsert == "")
                                {
                                    Att_dcolumnainsertr = "'" + LeaveCodea + "'";
                                    Att_dcolumnpinsertr = "'" + LeaveCodep + "'";

                                    Att_dcolumnainsert = "[" + "d" + Convert.ToInt16(str_day) + "d" + sqlsec + "] ";
                                    Att_dcolumnpinsert = "[" + "d" + Convert.ToInt16(str_day) + "d" + sqlsec + "] ";

                                    Att_dcolumna = "[" + "d" + Convert.ToInt16(str_day) + "d" + sqlsec + "] ='" + LeaveCodea + "'";
                                    Att_dcolumnp = "[" + "d" + Convert.ToInt16(str_day) + "d" + sqlsec + "] ='" + LeaveCodep + "'";
                                }
                                else
                                {
                                    Att_dcolumnainsertr = Att_dcolumnainsertr + "," + "'" + LeaveCodea + "'";
                                    Att_dcolumnpinsertr = Att_dcolumnpinsertr + "," + "'" + LeaveCodep + "'";

                                    Att_dcolumnainsert = Att_dcolumnainsert + "," + "[" + "d" + Convert.ToInt16(str_day) + "d" + sqlsec + "]";
                                    Att_dcolumnpinsert = Att_dcolumnpinsert + "," + "[" + "d" + Convert.ToInt16(str_day) + "d" + sqlsec + "]";

                                    Att_dcolumna = Att_dcolumna + "," + "[" + "d" + Convert.ToInt16(str_day) + "d" + sqlsec + "] ='" + LeaveCodea + "'";
                                    Att_dcolumnp = Att_dcolumnp + "," + "[" + "d" + Convert.ToInt16(str_day) + "d" + sqlsec + "] ='" + LeaveCodep + "'";
                                }
                            }
                        }
                    }
                    string DateVal = chechfromdate;
                    dsselect14.Tables[0].DefaultView.RowFilter = "batch_year=" + batch_Year + " and  degree_code=" + degree_code + " and Current_Semester=" + current_semester + "" + sec + "";
                    dv2 = dsselect14.Tables[0].DefaultView;

                    if (dv2.Count > 0)
                    {
                        for (int dc1 = 0; dc1 < dv2.Count; dc1++)
                        {
                            string rollno1 = dv2[dc1]["Roll_No"].ToString().ToLower();
                            if (!loadhour123.Contains(rollno1))
                            {
                                loadhour123.Add(rollno1, rollno1);
                            }
                        }
                        for (int dc = 0; dc < dv2.Count; dc++)
                        {
                            string app_No = dv2[dc]["app_no"].ToString().ToLower();
                            string rollno = dv2[dc]["Roll_No"].ToString().ToLower();
                            string regno = dv2[dc]["reg_no"].ToString().ToLower();
                            string admissionno = dv2[dc]["Roll_admit"].ToString().ToLower();
                            string chkroll = rollno;


                            //if (ddlentry.SelectedItem.Text.ToString().ToUpper() == "REG NO")
                            //{
                            //    chkroll = regno;
                            //}
                            //if (ddlentry.SelectedItem.Text.ToString().ToUpper() == "ADMISSION NO")
                            //{
                            //    chkroll = admissionno;
                            //}
                            if (Session["Rollflag"] == "1")
                            {

                            }
                            if (Session["Regflag"] == "1")
                            {
                                chkroll = regno;
                            }
                            if (Session["AdmissionNo"] == "1")
                            {
                                chkroll = admissionno;
                            }
                            string rollfg = string.Empty;
                            if (Session["Rollflag"] == "1")
                            {
                                if (rollfg == "")
                                {
                                    rollfg = rollno;
                                }
                            }
                            if (Session["Regflag"] == "1")
                            {
                                if (rollfg == "")
                                {
                                    rollfg = regno;
                                }
                                else
                                {
                                    rollfg = rollfg + "*" + regno;
                                }
                            }

                            #region Added By Malang Raja On Nov 03 2016

                            if (Session["AdmissionNo"] == "1")
                            {
                                if (rollfg == "")
                                {
                                    rollfg = admissionno;
                                }
                                else
                                {
                                    rollfg = rollfg + "*" + admissionno;
                                }
                            }

                            #endregion

                            //string qry = "select * from attendance where Att_App_no in(" + app_No + ")and month_year ='" + month_year + "'";


                            if (abbsentroll.Contains(rollfg.ToLower()))
                            {
                                if (dicabsen.ContainsKey(rollfg.Trim().ToLower()))
                                {
                                    dicabsen.Remove(rollfg.Trim().ToLower());
                                }
                                hassabbrollno = rollno.ToString();

                                string sqlquery = "if exists(select * from attendance where Att_App_no in('" + app_No + "') and month_year ='" + month_year + "') update attendance set " + Att_dcolumna + " where Att_App_no='" + app_No + "' and month_year ='" + month_year + "' else insert into attendance(Att_App_no,Att_CollegeCode,Roll_No,month_year," + Att_dcolumnainsert + ") values(" + app_No + "," + ddlcollege.SelectedValue + ",'" + hassabbrollno + "','" + month_year + "'," + Att_dcolumnainsertr + ")";
                                int save = d2.insert_method(sqlquery, hat, "Text");
                                if (save > 0)
                                {
                                    //if (hrcheck)
                                    attendanceMark(app_No, month_year, strPerDays, noMaxHrsDay, noFstHrsDay, noSndHrsDay, noMinFstHrsDay, noMinSndHrsDay, DateVal, ddlcollege.SelectedValue.ToString());
                                    //string ctsname = "Save the Student Attendance Inforamtion";
                                    //da.insertUserActionLog(entrycode, batchYear, degreeCode, current_semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname);//saranya(16.11.2017)
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Text = "Saved Successfully!";
                                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                                }
                                if (chksms.Checked == true)
                                {
                                    string perpriod = string.Empty;
                                    string allhour = string.Empty;
                                    string settingquery = string.Empty;
                                    settingquery = "  select * from Attendance_Settings where TextName='Hour' or TextName='Period' and  College_Code ='" + ddlcollege.SelectedValue.ToString() + "'and user_id='" + Session["usercode"].ToString() + "'";
                                    DataSet hoers = d2.select_method_wo_parameter(settingquery, "Text");
                                    if (hoers.Tables[0].Rows.Count > 0)
                                    {
                                        string permsson = string.Empty;
                                        for (int hrs = 0; hrs < hoers.Tables[0].Rows.Count; hrs++)
                                        {
                                            if (hoers.Tables[0].Rows[hrs]["textname"].ToString().Trim().ToLower() == "hour")
                                            {
                                                allhour = hoers.Tables[0].Rows[hrs]["taxtval"].ToString();
                                                permsson = hoers.Tables[0].Rows[hrs]["textname"].ToString();
                                            }
                                            if (hoers.Tables[0].Rows[hrs]["textname"].ToString().Trim().ToLower() == "period")
                                            {
                                                perpriod = hoers.Tables[0].Rows[hrs]["taxtval"].ToString();
                                                permsson = perpriod;
                                            }
                                        }
                                        if (permsson.Trim() != "" || allhour == "1")
                                        {
                                            for (int k = 0; k < chklssec.Items.Count; k++)
                                            {
                                                if (chklssec.Items[k].Selected == true)
                                                {
                                                    sqlsec = chklssec.Items[k].Value.ToString();
                                                    if (!loadhour1.Contains(sqlsec))
                                                    {
                                                        loadhour1.Add(sqlsec, sqlsec);
                                                        if (perpriod.Trim() == "")
                                                        {
                                                            if (permsson != "")
                                                            {
                                                                permsson = permsson + ',' + sqlsec;
                                                            }
                                                            else
                                                            {
                                                                permsson = sqlsec;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            string[] hourrs = permsson.Split(',');
                                            if (loadhour1.Count > 0)
                                            {
                                                for (int l = 0; l <= hourrs.GetUpperBound(0); l++)
                                                {
                                                    string ckcomd = hourrs[l].ToString();
                                                    if (loadhour1.Contains(ckcomd))
                                                    {
                                                        if (single_hour == "")
                                                        {
                                                            single_hour = ckcomd.ToString();
                                                        }
                                                        else
                                                        {
                                                            single_hour = single_hour + "," + ckcomd.ToString();
                                                        }
                                                    }
                                                }
                                            }

                                            if (single_hour != "")
                                            {
                                                SendingSms(hassabbrollno, chechfromdate1, single_hour, collacronym, newdeptcource, sections);
                                            }
                                        }
                                    }
                                }
                                if (chkvoice.Checked == true)
                                {
                                    sendvoicecall(hassabbrollno, chechfromdate1, single_hour, batch_Year, Dept_Name, collegename, newdeptcource, sections);
                                }
                            }
                            else if (loadhour123.Contains(rollno.ToLower()))
                            {
                                hassprerollno = "'" + rollno.ToString() + "'";
                                string sqlquery = "if exists(select * from attendance where Att_App_no in(" + app_No + ")and month_year ='" + month_year + "') update attendance set " + Att_dcolumnp + " where Att_App_no=" + app_No + " and month_year ='" + month_year + "' else insert into attendance(Att_App_no,Att_CollegeCode,Roll_No,month_year," + Att_dcolumnpinsert + ") values(" + app_No + "," + ddlcollege.SelectedValue + "," + hassprerollno + ",'" + month_year + "'," + Att_dcolumnpinsertr + ")";
                                int save = d2.insert_method(sqlquery, hat, "Text");
                                if (save > 0)
                                {
                                    attendanceMark(app_No, month_year, strPerDays, noMaxHrsDay, noFstHrsDay, noSndHrsDay, noMinFstHrsDay, noMinSndHrsDay, DateVal, ddlcollege.SelectedValue.ToString());
                                    savsotnot = true;
                                }

                            }
                        }

                        string batchdegrabsent = string.Empty;

                        foreach (var kvp in dicabsen)
                        {
                            string setval = kvp.Key.ToString();
                            if (batchdegrabsent.Trim() != "")
                            {
                                batchdegrabsent = batchdegrabsent + ',' + setval;
                            }
                            else
                            {
                                if (sections.Trim() != "" && sections != null && sections.Trim() != "-1" && sections.Trim() != "0")
                                {
                                    batchdegrabsent = batch_Year + '-' + newdeptcource + '-' + current_semester + '-' + sections + " : " + setval;
                                }
                                else
                                {
                                    batchdegrabsent = batch_Year + '-' + newdeptcource + '-' + current_semester + " : " + setval;
                                }
                            }

                        }
                        if (invalidroll.Trim() != "")
                        {
                            invalidroll = invalidroll + " / " + batchdegrabsent;
                        }
                        else
                        {
                            invalidroll = batchdegrabsent;
                        }
                    }
                    savsotnot = true;
                }
                no_of_hrs = " ";
            skip: ;
            }

            if (nohrsdetails.Rows.Count > 0)
            {
                string err = string.Empty;
                for (int i = 0; i < nohrsdetails.Rows.Count; i++)
                {
                    if (err == "")
                    {
                        if (b_school == true)
                        {
                            err = nohrsdetails.Rows[i][0] + "-" + nohrsdetails.Rows[i][1] + "-" + nohrsdetails.Rows[i][2] + "-" + nohrsdetails.Rows[i][3] + "  Term";
                        }
                        else
                        {
                            err = nohrsdetails.Rows[i][0] + "-" + nohrsdetails.Rows[i][1] + "-" + nohrsdetails.Rows[i][2] + "-" + nohrsdetails.Rows[i][3] + "  Sem";
                        }

                    }
                    else
                    {

                        if (b_school == true)
                        {
                            err = err + ";" + nohrsdetails.Rows[i][0] + "-" + nohrsdetails.Rows[i][1] + "-" + nohrsdetails.Rows[i][2] + "-" + nohrsdetails.Rows[i][3] + " Term";
                        }
                        else
                        {
                            err = err + ";" + nohrsdetails.Rows[i][0] + "-" + nohrsdetails.Rows[i][1] + "-" + nohrsdetails.Rows[i][2] + "-" + nohrsdetails.Rows[i][3] + " Sem";
                        }

                    }

                }
                errmsg.Text = "Please Update The Conducted Hour Details for " + err + "";
                errmsg.Visible = true;
                // return;
            }
            if (savsotnot == false)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter Atleast One Student Roll Number";
                return;
            }
            else
            {
                foreach (GridViewRow gvrow in gvuserodrlist.Rows)//saranya 16.11.2017
                {
                    Label current_semester1 = (Label)gvrow.FindControl("lblcurrent_semester");
                    Label sections1 = (Label)gvrow.FindControl("lblsections");

                    Label lblBatchYear = (Label)gvrow.FindControl("lblbatch_Year");
                    Label lblDegreeCode = (Label)gvrow.FindControl("lblCourse_id");
                    string current_semester = current_semester1.Text;
                    string sections = sections1.Text;
                    DataView dvStudentAttend = new DataView();
                    if (dtAttendanceDetails.Rows.Count > 0)
                    {
                        dtAttendanceDetails.DefaultView.RowFilter = "Batch_Year='" + lblBatchYear.Text + "' and degree_code='" + lblDegreeCode.Text + "' and Current_Semester='" + current_semester + "' and Sections='" + sections + "'";
                        dvStudentAttend = dtAttendanceDetails.DefaultView;
                    }

                    string ctsname = "Save the Student Attendance Inforamtion";
                    if (dvStudentAttend.Count > 0)
                    {
                        ctsname = "Update the Student Attendance Inforamtion";
                    }

                    #region saranya

                    string entrycode = Session["Entry_Code"].ToString();
                    string PageName = "Student Attendance Entry";
                    //string batchYear = rs.GetSelectedItemsValueAsString(chklsbatch);
                    //string degreeCode = rs.GetSelectedItemsValueAsString(chklstbranch);
                    string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");

                    #endregion

                    da.insertUserActionLog(entrycode, lblBatchYear.Text, lblDegreeCode.Text, current_semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname,1);


                }
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Saved Successfully!";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
            }
            //if (invalidroll.Trim() != "" && invalidroll != null)
            //{
            //    errmsg.Visible = true;
            //    errmsg.Text = "Following Roll No's Are Not Exists : " + invalidroll + "";
            //}
            chkselectall.Checked = false;
            chksms.Checked = false;
            chkvoice.Checked = false;
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }

    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    public void smsreport(string uril, string isstaff, DateTime dt, string phone, string msg)
    {
        try
        {
            string phoneno = phone;
            string message = msg;
            string date = dt.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = string.Empty;
            groupmsgid = strvel;
            int sms = 0;
            string smsreportinsert = string.Empty;
            string[] split_mobileno = phoneno.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                message = message.Replace("'", "''");
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + split_mobileno[icount] + "','" + groupmsgid + "','" + message + "','" + Session["collegecode"].ToString() + "','" + isstaff + "','" + date + "','" + Session["UserCode"].ToString() + "')";
                sms = d2.insert_method(smsreportinsert, hat, "Text");
            }

        }
        catch (Exception ex)
        {
            //errmsg.Text = ex.ToString();
            //errmsg.Visible = true;
        }

    }

    public void SendingSms(string rollno, string date, string hour, string college, string course, string sectionall)
    {
        try
        {
            string Gender = string.Empty;
            string collegename1 = string.Empty;
            string Hour = hour;
            string admno = string.Empty;
            string app_no = string.Empty;
            string regno = string.Empty;

            string MsgText = string.Empty;
            string RecepientNo = string.Empty;
            int check = 0;
            string user_id = string.Empty;

            collegename1 = college;
            string coursename1 = course;
            string section = string.Empty;
            if (sectionall != "")
            {
                section = sectionall;
            }
            string[] split = date.Split(new Char[] { '-' });
            string datefrom = split[0].ToString() + "-" + split[1].ToString() + "-" + split[2].ToString();
            date = datefrom;


            string str1 = string.Empty;
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + group_code + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and group_code='" + group_code + "'and value='1'";
                str1 = str1 + "  select Sections,Roll_Admit,Reg_No,App_No from Registration where Roll_No='" + rollno + "'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and usercode='" + Session["usercode"].ToString() + "'and value='1'";
                str1 = str1 + "  select Sections,Roll_Admit,Reg_No,App_No from Registration where Roll_No='" + rollno + "'";
            }
            Boolean flage = false;
            DataSet ds1;
            ds1 = d2.select_method_wo_parameter(str1, "txt");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Attendance Sms for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }
                if (flage == true)
                {
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if ((ds1.Tables[0].Rows[k]["TextName"].ToString().ToLower() == "period" || ds1.Tables[0].Rows[k]["TextName"].ToString().ToLower() == "hour") && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (loadhour1.Contains(final_Hours))
                                {
                                    check = check + 1;
                                }
                            }
                        }
                    }
                }
            }
            if (ds1.Tables[2].Rows.Count > 0)
            {
                regno = Convert.ToString(ds1.Tables[2].Rows[0]["Reg_No"]);
                admno = Convert.ToString(ds1.Tables[2].Rows[0]["Roll_Admit"]);
                app_no = Convert.ToString(ds1.Tables[2].Rows[0]["App_No"]);
            }
            if (check > 0)
            {
                check = 0;

                //============================Commented By Malang Raja on Nov 03 2016 =========================================

                //string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                //DataSet dstrack;
                //
                //dstrack = d2.select_method_wo_parameter(ssr, "txt");
                ////if (dstrack.Tables[0].Rows.Count > 0)
                ////{
                //    user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);

                //============================Commented By Malang Raja on Nov 03 2016 =========================================

                string degcode = string.Empty;
                string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,r.degree_code,r.roll_admit from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dsMobile;
                dsMobile = d2.select_method_wo_parameter(Phone, "txt");
                if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                {
                    degcode = Convert.ToString(dsMobile.Tables[0].Rows[0]["degree_code"]).Trim();
                    if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                    {
                        Gender = "Your Son ";
                    }
                    else
                    {
                        Gender = "Your Daughter ";

                    }
                    DateTime dt = Convert.ToDateTime(date);
                    if (ds1.Tables[1].Rows.Count > 0)
                    {
                        string templatevlaue = Convert.ToString(ds1.Tables[1].Rows[0]["template"]);
                        if (templatevlaue.Trim() != "")
                        {
                            string[] splittemplate = templatevlaue.Split('$');
                            if (splittemplate.Length > 0)
                            {
                                for (int j = 0; j <= splittemplate.GetUpperBound(0); j++)
                                {
                                    if (splittemplate[j].ToString() != "")
                                    {
                                        if (splittemplate[j].ToString() == "College Name")
                                        {
                                            MsgText = MsgText + " " + collegename1;
                                        }

                                        else if (splittemplate[j].ToString() == "Student Name")
                                        {
                                            MsgText = MsgText + " " + dsMobile.Tables[0].Rows[0]["StudName"].ToString();
                                        }
                                        else if (splittemplate[j].ToString() == "Degree")
                                        {
                                            MsgText = MsgText + " " + coursename1;
                                        }
                                        else if (splittemplate[j].ToString() == "Section")
                                        {
                                            if (section.Trim() != "")
                                            {
                                                MsgText = MsgText + " " + "'" + section + "' Section";
                                            }
                                        }
                                        else if (splittemplate[j].ToString() == "Thank You")
                                        {
                                            MsgText = MsgText + " " + splittemplate[j].ToString();
                                        }
                                        else if (splittemplate[j].ToString() == "Absent")
                                        {
                                            MsgText = MsgText + " " + Hour + " hour Absent";
                                        }
                                        else if (splittemplate[j].ToString() == "Roll No")
                                        {
                                            MsgText = MsgText + " " + rollno;
                                        }
                                        else if (splittemplate[j].ToString() == "Register No")
                                        {
                                            MsgText = MsgText + " " + regno;
                                        }
                                        else if (splittemplate[j].ToString() == "Application No")
                                        {
                                            MsgText = MsgText + " " + app_no;
                                        }
                                        else if (splittemplate[j].ToString() == "Admission No")
                                        {
                                            MsgText = MsgText + " " + admno;
                                        }
                                        else
                                        {
                                            if (MsgText == "")
                                            {
                                                MsgText = splittemplate[j].ToString();
                                            }
                                            else
                                            {
                                                MsgText = MsgText + " " + splittemplate[j].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MsgText = "Dear Parent, Good Morning. This Message from" + " " + collegename1 + ". Your ward " + dsMobile.Tables[0].Rows[0]["StudName"].ToString() + " of " + coursename1 + "-" + section + " is found absent  " + Hour + " hour. Thank you";
                    }

                    for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                    {
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0" && dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString().Trim() != "" && dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != null)
                            {
                                RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                //string getval = d2.GetUserapi(user_id);
                                //string[] spret = getval.Split('-');
                                //if (spret.GetUpperBound(0) == 1)
                                //{
                                //    SenderID = spret[0].ToString();
                                //    Password = spret[0].ToString();
                                //    Session["api"] = user_id;
                                //    Session["senderid"] = SenderID;
                                //}
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                //string isst = "0";
                                //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                //int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, RecepientNo, MsgText,"0");
                                //New SMS Function
                                SMSSettings smsObject = new SMSSettings();
                                smsObject.User_degreecode = Convert.ToInt32(degcode);
                                smsObject.User_collegecode = Convert.ToInt32(ddlcollege.SelectedValue);
                                smsObject.User_usercode = usercode;
                                smsObject.Text_message = MsgText;
                                smsObject.IsStaff = 0;
                                smsObject.MobileNos = RecepientNo;
                                smsObject.AdmissionNos = dsMobile.Tables[0].Rows[0]["roll_admit"].ToString();
                                smsObject.sendTextMessage();
                                //int nofosmssend = d2.sendNewSMS(degcode, ddlcollege.SelectedValue, usercode, RecepientNo, MsgText, "0");

                            }

                        }
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0" && dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString().Trim() != "" && dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != null)
                            {
                                RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                //string getval = d2.GetUserapi(user_id);
                                //string[] spret = getval.Split('-');
                                //if (spret.GetUpperBound(0) == 1)
                                //{
                                //    SenderID = spret[0].ToString();
                                //    Password = spret[0].ToString();
                                //    Session["api"] = user_id;
                                //    Session["senderid"] = SenderID;
                                //}
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                ////  string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                //string isst = "0";
                                //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                //int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, RecepientNo, MsgText, "0");
                                //New SMS Function
                                SMSSettings smsObject = new SMSSettings();
                                smsObject.User_degreecode = Convert.ToInt32(degcode);
                                smsObject.User_collegecode = Convert.ToInt32(ddlcollege.SelectedValue);
                                smsObject.User_usercode = usercode;
                                smsObject.Text_message = MsgText;
                                smsObject.IsStaff = 0;
                                smsObject.MobileNos = RecepientNo;
                                smsObject.AdmissionNos = dsMobile.Tables[0].Rows[0]["roll_admit"].ToString();
                                smsObject.sendTextMessage();
                                //int nofosmssend = d2.sendNewSMS(degcode, ddlcollege.SelectedValue, usercode, RecepientNo, MsgText, "0");
                            }
                        }
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0" && dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString().Trim() != "" && dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != null)
                            {
                                RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                //string getval = d2.GetUserapi(user_id);
                                //string[] spret = getval.Split('-');
                                //if (spret.GetUpperBound(0) == 1)
                                //{
                                //    SenderID = spret[0].ToString();
                                //    Password = spret[0].ToString();
                                //    Session["api"] = user_id;
                                //    Session["senderid"] = SenderID;
                                //}
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                ////string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                //string isst = "0";
                                //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                //int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, RecepientNo, MsgText, "0");
                                //New SMS Function
                                SMSSettings smsObject = new SMSSettings();
                                smsObject.User_degreecode = Convert.ToInt32(degcode);
                                smsObject.User_collegecode = Convert.ToInt32(ddlcollege.SelectedValue);
                                smsObject.User_usercode = usercode;
                                smsObject.Text_message = MsgText;
                                smsObject.IsStaff = 0;
                                smsObject.MobileNos = RecepientNo;
                                smsObject.AdmissionNos = dsMobile.Tables[0].Rows[0]["roll_admit"].ToString();
                                smsObject.sendTextMessage();
                                //int nofosmssend = d2.sendNewSMS(degcode, ddlcollege.SelectedValue, usercode, RecepientNo, MsgText, "0");
                            }

                        }
                    }

                }
                //}
            }
        }
        catch (Exception ex)
        {
            //errmsg.Text = ex.ToString();
            //errmsg.Visible = true;
        }
    }

    public void SendingSmsOld(string rollno, string date, string hour, string college, string course, string sectionall)
    {
        try
        {
            string Gender = string.Empty;
            string collegename1 = string.Empty;
            string Hour = hour;

            string MsgText = string.Empty;
            string RecepientNo = string.Empty;
            int check = 0;
            string user_id = string.Empty;

            collegename1 = college;
            string coursename1 = course;
            string section = string.Empty;
            if (sectionall != "")
            {
                section = sectionall;
            }
            string[] split = date.Split(new Char[] { '-' });
            string datefrom = split[0].ToString() + "-" + split[1].ToString() + "-" + split[2].ToString();
            date = datefrom;


            string str1 = string.Empty;
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + group_code + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and group_code='" + group_code + "'and value='1'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and usercode='" + Session["usercode"].ToString() + "'and value='1'";
            }
            Boolean flage = false;
            DataSet ds1;
            ds1 = d2.select_method_wo_parameter(str1, "txt");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Attendance Sms for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }
                if (flage == true)
                {
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if ((ds1.Tables[0].Rows[k]["TextName"].ToString().ToLower() == "period" || ds1.Tables[0].Rows[k]["TextName"].ToString().ToLower() == "hour") && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (loadhour1.Contains(final_Hours))
                                {
                                    check = check + 1;
                                }
                            }
                        }
                    }
                }
            }
            if (check > 0)
            {
                check = 0;
                string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dstrack;
                dstrack = d2.select_method_wo_parameter(ssr, "txt");
                if (dstrack.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);

                    string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,r.roll_admit,r.degree_code from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                    DataSet dsMobile;
                    dsMobile = d2.select_method_wo_parameter(Phone, "txt");
                    if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                        {
                            Gender = "Your Son ";
                        }
                        else
                        {
                            Gender = "Your Daughter ";

                        }
                        DateTime dt = Convert.ToDateTime(date);
                        if (ds1.Tables[1].Rows.Count > 0)
                        {
                            string templatevlaue = Convert.ToString(ds1.Tables[1].Rows[0]["template"]);
                            if (templatevlaue.Trim() != "")
                            {
                                string[] splittemplate = templatevlaue.Split('$');
                                if (splittemplate.Length > 0)
                                {
                                    for (int j = 0; j <= splittemplate.GetUpperBound(0); j++)
                                    {
                                        if (splittemplate[j].ToString() != "")
                                        {
                                            if (splittemplate[j].ToString() == "College Name")
                                            {
                                                MsgText = MsgText + " " + collegename1;
                                            }

                                            else if (splittemplate[j].ToString() == "Student Name")
                                            {
                                                MsgText = MsgText + " " + dsMobile.Tables[0].Rows[0]["StudName"].ToString();
                                            }
                                            else if (splittemplate[j].ToString() == "Degree")
                                            {
                                                MsgText = MsgText + " " + coursename1;
                                            }
                                            else if (splittemplate[j].ToString() == "Section")
                                            {
                                                if (section.Trim() != "")
                                                {
                                                    MsgText = MsgText + " " + "'" + section + "' Section";
                                                }
                                            }
                                            else if (splittemplate[j].ToString() == "Thank You")
                                            {
                                                MsgText = MsgText + " " + splittemplate[j].ToString();
                                            }
                                            else if (splittemplate[j].ToString() == "Absent")
                                            {
                                                MsgText = MsgText + " " + Hour + " hour Absent";
                                            }
                                            else
                                            {
                                                if (MsgText == "")
                                                {
                                                    MsgText = splittemplate[j].ToString();
                                                }
                                                else
                                                {
                                                    MsgText = MsgText + " " + splittemplate[j].ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MsgText = "Dear Parent, Good Morning. This Message from" + " " + collegename1 + ". Your ward " + dsMobile.Tables[0].Rows[0]["StudName"].ToString() + " of " + coursename1 + "-" + section + " is found absent  " + Hour + " hour. Thank you";
                        }

                        for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                        {
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0" && dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString().Trim() != "" && dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != null)
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                    string getval = d2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[0].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    //New SMS Function
                                    SMSSettings smsObject = new SMSSettings();
                                    smsObject.User_degreecode = Convert.ToInt32(dsMobile.Tables[0].Rows[0]["degree_code"]);
                                    smsObject.User_collegecode = Convert.ToInt32(ddlcollege.SelectedValue);
                                    smsObject.User_usercode = usercode;
                                    smsObject.Text_message = MsgText;
                                    smsObject.IsStaff = 0;
                                    smsObject.MobileNos = RecepientNo;
                                    smsObject.AdmissionNos = dsMobile.Tables[0].Rows[0]["roll_admit"].ToString();
                                    smsObject.sendTextMessage();
                                    // int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, RecepientNo, MsgText, "0");
                                }

                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0" && dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString().Trim() != "" && dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != null)
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                    string getval = d2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[0].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    ////  string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    //New SMS Function
                                    SMSSettings smsObject = new SMSSettings();
                                    smsObject.User_degreecode = Convert.ToInt32(dsMobile.Tables[0].Rows[0]["degree_code"]);
                                    smsObject.User_collegecode = Convert.ToInt32(ddlcollege.SelectedValue);
                                    smsObject.User_usercode = usercode;
                                    smsObject.Text_message = MsgText;
                                    smsObject.IsStaff = 0;
                                    smsObject.MobileNos = RecepientNo;
                                    smsObject.AdmissionNos = dsMobile.Tables[0].Rows[0]["roll_admit"].ToString();
                                    smsObject.sendTextMessage();
                                    //int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, RecepientNo, MsgText, "0");
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0" && dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString().Trim() != "" && dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != null)
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                    string getval = d2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[0].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    ////string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    //New SMS Function
                                    SMSSettings smsObject = new SMSSettings();
                                    smsObject.User_degreecode = Convert.ToInt32(dsMobile.Tables[0].Rows[0]["degree_code"]);
                                    smsObject.User_collegecode = Convert.ToInt32(ddlcollege.SelectedValue);
                                    smsObject.User_usercode = usercode;
                                    smsObject.Text_message = MsgText;
                                    smsObject.IsStaff = 0;
                                    smsObject.MobileNos = RecepientNo;
                                    smsObject.AdmissionNos = dsMobile.Tables[0].Rows[0]["roll_admit"].ToString();
                                    smsObject.sendTextMessage();
                                    //int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, RecepientNo, MsgText, "0");
                                }

                            }
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            //errmsg.Text = ex.ToString();
            //errmsg.Visible = true;
        }
    }

    public void sendvoicecall(string rollno, string date, string hour, string batch, string degree, string college, string course, string setting) //added by jairam 03-01-2015
    {
        try
        {
            string hour_check = string.Empty;
            hour_check = hour;
            string RecepientNo = string.Empty;
            string gender = string.Empty;
            int check = 0;
            string str1 = string.Empty;
            string group_code = Session["group_code"].ToString();
            DateTime dt = Convert.ToDateTime(date);
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + group_code + "'";
                str1 = str1 + "  select sex, r.stud_name  from Registration r, applyn a where r.App_No=a.app_no and Roll_No='" + rollno + "'";

            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
                str1 = str1 + "  select sex, r.stud_name  from Registration r, applyn a where r.App_No=a.app_no and Roll_No='" + rollno + "'";

            }


            Boolean flage = false;
            DataSet ds1;
            ds1 = d2.select_method_wo_parameter(str1, "txt");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Voice Call for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }

                if (flage == true)
                {
                    string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                    DataSet dsMobile;
                    dsMobile = d2.select_method_wo_parameter(Phone, "txt");
                    if (ds1.Tables[1].Rows.Count > 0)
                    {
                        string sex = Convert.ToString(ds1.Tables[1].Rows[0]["sex"]);
                        if (sex == "0")
                        {
                            gender = "MALE";
                        }
                        else if (sex == "1")
                        {
                            gender = "FEMALE";
                        }

                        string isstaff = "0";
                        for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                        {
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {

                                if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                    //string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    string result = voicecall(RecepientNo, college);
                                    voicereport(result, isstaff, dt, RecepientNo);
                                }

                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                {

                                    RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                    //  string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    string result = voicecall(RecepientNo, college);
                                    voicereport(result, isstaff, dt, RecepientNo);
                                }

                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                    //string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    string result = voicecall(RecepientNo, college);
                                    voicereport(result, isstaff, dt, RecepientNo);
                                }

                            }
                        }
                    }

                    //for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    //{
                    //    if (ds1.Tables[0].Rows[k]["TextName"].ToString() == "Period" && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                    //    {
                    //        string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                    //        string[] fin_split = splihours.Split(',');
                    //        int count = fin_split.Length;
                    //        for (int i = 0; i < count; i++)
                    //        {
                    //            string final_Hours = fin_split[i];
                    //            if (hour_check == final_Hours)
                    //            {
                    //                check = check + 1;
                    //            }
                    //        }

                    //    }
                    //}

                }
            }
        }

        catch
        {

        }
    }

    public string voicecall(string mobile, string filename) // added by jairam 18-07-2015
    {
        string api_key = string.Empty;
        string access_key = string.Empty;
        string Mobile_Number = "91" + mobile + "";
        string error = string.Empty;
        string value1 = string.Empty;
        if (filename.ToUpper() == "JAWAHAR MATRICULATION HIGHER SECONDARY SCHOOL")
        {
            value1 = "http://vapi.unicel.in/voiceapi?request=voiceobd&uname=rcavoice&pass=123456&obdid=0&type=S&dest=" + Mobile_Number + "&msgtype=P&msg=ABSENT_VOICE_MATRIC.wav";
        }
        else if (filename.ToUpper() == "JAWAHAR HIGHER SECONDARY SCHOOL CBSE ")
        {
            value1 = "http://vapi.unicel.in/voiceapi?request=voiceobd&uname=rcavoice&pass=123456&obdid=0&type=S&dest=" + Mobile_Number + "&msgtype=P&msg=absent_jahawar_cbse.wav";
        }
        try
        {

            WebRequest webrequ = WebRequest.Create(value1);
            WebResponse response = webrequ.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            error = Convert.ToString(strvel);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        return error;
    }

    public void voicereport(string uril, string isstaff, DateTime dt, string phone) //added by jairam 03-01-2015
    {
        try
        {
            string smsreportinsert = string.Empty;
            string date = dt.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
            string message = "Student Attendance Absent Voice Message";
            string voicecall = "1";
            string voicecall_Status = string.Empty;
            if (uril.Trim().ToUpper() != "ERROR")
            {
                voicecall_Status = "Sent";
            }
            else
            {
                voicecall_Status = "error";
            }
            smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id,voice_call,voice_call_status)values( '" + phone + "','" + uril + "','" + message + "','" + Session["collegecode"].ToString() + "','" + isstaff + "','" + date + "','" + Session["UserCode"].ToString() + "','" + voicecall + "','" + voicecall_Status + "')";
            int sms = d2.insert_method(smsreportinsert, hat, "Text");
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }

    }

    protected void grid_view_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label chkbox = (Label)e.Row.FindControl("lblSRNO");

            Label batch_Year1 = (Label)e.Row.FindControl("lblbatch_Year");
            Label Course_Name1 = (Label)e.Row.FindControl("lblCourse_Name");
            Label Dept_Name1 = (Label)e.Row.FindControl("lblDept_Name");
            Label current_semester1 = (Label)e.Row.FindControl("lblcurrent_semester");
            Label sections1 = (Label)e.Row.FindControl("lblsections");
            Label degree_code1 = (Label)e.Row.FindControl("lblCourse_id");

            int actrows = Convert.ToInt16(chkbox.Text);

            string batch_Year = batch_Year1.Text;
            string Course_Name = Course_Name1.Text;
            string Dept_Name = Dept_Name1.Text;
            string current_semester = current_semester1.Text;
            string sections = sections1.Text;
            string degree_code = degree_code1.Text;
            string sec = string.Empty;
            if (sections.Trim() == "" || sections.Trim() == "-1" || sections.Trim() == "0")
            {
                sec = string.Empty;
            }
            else
            {
                sec = "and ltrim(rtrim(isnull(Sections,'')))='" + sections + "'";
            }

            string[] splitfromdate = txtfrom.Text.Split(new Char[] { '/' });
            string chechfromdate = splitfromdate[1] + '/' + splitfromdate[0] + '/' + splitfromdate[2];
            DateTime confromdate = Convert.ToDateTime(chechfromdate);

            string sqlquery1 = "select * from holidayStudents where holiday_date='" + chechfromdate + "' and degree_code='" + degree_code + "' and semester='" + current_semester + "'";
            DataSet dsselect1 = new DataSet();
            dsselect1 = d2.select_method(sqlquery1, hat, "Text");
            if (dsselect1.Tables.Count > 0 && dsselect1.Tables[0].Rows.Count > 0)
            {
                e.Row.Cells[6].ColumnSpan = 2;
                e.Row.Cells[6].Text = "Selected Day is Holiday";
                e.Row.Cells[6].Width = 450;
                gvuserodrlist.Columns[6].ItemStyle.Width = 450;
                e.Row.Cells.RemoveAt(7);
            }
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        chklsbatch.Items.Clear();
        chklstdegree.Items.Clear();
        chklstbranch.Items.Clear();
        collegecode = ddlcollege.SelectedValue.ToString();
        BindBatch();
        BindDegree(singleuser, group_user, collegecode, usercode);
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }

    [WebMethod]
    public static string CheckUserName(string rollno, string batch, string degree, string sem, string sec, string entryby)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string user_name = rollno;
            string strsec = string.Empty;
            if (sec.Trim() != "" && sec != null && sec != "-1")
            {
                strsec = " and ltrim(rtrim(isnull(Sections,'')))='" + sec + "'";
            }
            if (user_name.Trim() != "" && user_name != null)
            {
                string[] splitusername = user_name.Split(',');
                for (int i = 0; i <= splitusername.GetUpperBound(0); i++)
                {
                    string firstvalue = splitusername[i].ToString();
                    if (firstvalue.Trim() != "")
                    {
                        string query = dd.GetFunction("select roll_no from registration  where roll_no ='" + firstvalue + "' and batch_year='" + batch + "' and degree_code='" + degree + "' and current_semester='" + sem + "' " + strsec + " and cc=0 and delflag=0 and exam_flag<>'debar'");
                        if (entryby == "1")
                        {
                            query = dd.GetFunction("select Reg_no from registration  where Reg_no ='" + firstvalue + "' and batch_year='" + batch + "' and degree_code='" + degree + "' and current_semester='" + sem + "' " + strsec + " and cc=0 and delflag=0 and exam_flag<>'debar'");
                        }
                        if (query.Trim() == "" || query == null || query == "0" || query == "-1")
                        {
                            returnValue = "0";
                        }
                    }
                }
            }
            else
            {
                returnValue = "2";
            }


        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    //  [WebMethod]
    //public static string romverollno(string rollno, string batch, string degree, string sem, string sec)
    //{
    //    string returnValue =string.Empty;
    //    try
    //    {
    //        DAccess2 dd = new DAccess2();
    //        string user_name = rollno;
    //        string strsec =string.Empty;
    //        if (sec.Trim() != "" && sec != null && sec != "-1")
    //        {
    //            strsec = " and sections='" + sec + "'";
    //        }
    //        if (user_name.Trim() != "" && user_name != null)
    //        {
    //            string[] splitusername = user_name.Split(',');
    //            for (int i = 0; i <= splitusername.GetUpperBound(0); i++)
    //            {
    //                string firstvalue = splitusername[i].ToString();
    //                if (firstvalue.Trim() != "")
    //                {
    //                    string query = dd.GetFunction("select roll_no from registration  where roll_no ='" + firstvalue + "' and batch_year='" + batch + "' and degree_code='" + degree + "' and current_semester='" + sem + "' " + strsec + " and cc=0 and delflag=0 and exam_flag<>'debar'");
    //                    if (query.Trim() != "" && query != null && query != "0" && query != "-1")
    //                    {
    //                        if (returnValue.Trim() != "")
    //                        {
    //                            returnValue = returnValue+','+firstvalue;
    //                        }
    //                        else
    //                        {
    //                            returnValue = firstvalue;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (SqlException ex)
    //    {
    //        returnValue = "error" + ex.ToString();
    //    }
    //    return returnValue;
    //}

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[6].Text = "Absentees-" + Convert.ToString(ddlentry.SelectedItem.Text);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txt1 = (TextBox)e.Row.FindControl("txtabbsent");
            Label lbl1batch = (Label)e.Row.FindControl("lblbatch_Year");
            Label lbl1degree = (Label)e.Row.FindControl("lblCourse_id");
            Label lbl1semester = (Label)e.Row.FindControl("lblcurrent_semester");
            Label lbl1section = (Label)e.Row.FindControl("lblsections");
            //CheckBox presentall = (CheckBox)e.Row.FindControl("presentall");


            txt1.Attributes.Add("onkeyup", "javascript:get('" + txt1.ClientID + "','" + lbl1batch.Text + "','" + lbl1degree.Text + "','" + lbl1semester.Text + "','" + lbl1section.Text + "','" + ddlentry.SelectedValue.ToString() + "')");
            //txt1.Attributes.Add("onblur", "javascript:rollexits('" + txt1.ClientID + "','" + lbl1batch.Text + "','" + lbl1degree.Text + "','" + lbl1semester.Text + "','" + lbl1section.Text + "')");
        }

    }

    protected void chkselectall_Change(object sender, EventArgs e)
    {
        try
        {
            //if (chkselectall.Checked == true)
            //{
            //    if (gvuserodrlist.Rows.Count > 0)
            //    {
            //        gvuserodrlist.Columns[6].Visible = false;
            //        gvuserodrlist.Columns[7].Visible = false;
            //    }
            //}
            //else
            //{
            //    if (gvuserodrlist.Rows.Count > 0)
            //    {
            //        gvuserodrlist.Columns[6].Visible = true;
            //        gvuserodrlist.Columns[7].Visible = true;
            //    }
            //}

            if (chkselectall.Checked == true)
            {
                foreach (GridViewRow gvrow in gvuserodrlist.Rows)
                {

                    CheckBox presentall = (CheckBox)gvrow.FindControl("presentall");
                    presentall.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow gvrow in gvuserodrlist.Rows)
                {

                    CheckBox presentall = (CheckBox)gvrow.FindControl("presentall");
                    presentall.Checked = false;
                }
            }
        }
        catch
        {

        }
    }

    //public bool daycheck(DateTime seldate)
    //{
    //    DAccess2 da = new DAccess2();
    //    string collegecode = Session["collegecode"].ToString();
    //    bool daycheck = false;
    //    DateTime curdate;//, prevdate;
    //    long total, k, s;
    //    string[] ddate = new string[500];
    //    //DateTime[] ddate = new DateTime[500];
    //    //curdate == DateTime.Today.ToString() ;
    //    string c_date = DateTime.Today.ToString();
    //    DateTime todate_day = Convert.ToDateTime(DateTime.Today.ToString());
    //    curdate = DateTime.Today;
    //    if (seldate.ToString() == c_date)
    //    {
    //        daycheck = true;
    //        return daycheck;
    //    }
    //    else
    //    {
    //        //Modified by srinath 12/8/2013
    //        string lockdayvalue = "select lockdays,lflag from collinfo where college_code=" + collegecode + "";
    //        DataSet ds = new DataSet();
    //        ds = da.select_method(lockdayvalue, hat, "Text");
    //        // da.Fill(ds);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                //If StrComp(ChkRs(1), "true", vbTextCompare) = 0 Then
    //                if (ds.Tables[0].Rows[i][1].ToString() == "True")
    //                {
    //                    //If IsNull(ChkRs(0)) = False And val(ChkRs(0)) >= 0 Then
    //                    if (ds.Tables[0].Rows[i][0].ToString() != null && int.Parse(ds.Tables[0].Rows[i][0].ToString()) >= 0)
    //                    {
    //                        total = int.Parse(ds.Tables[0].Rows[i][0].ToString());
    //                        total = total + 1;
    //                        //Modified by srinath 12/8/2013
    //                        String strholidasquery = "select holiday_date from holidaystudents where degree_code=" + ddlbranch.SelectedItem.Value.ToString() + "  and semester=" + ddlsem.SelectedItem.Text.ToString() + "";
    //                        DataSet ds1 = new DataSet();
    //                        ds1 = da.select_method(strholidasquery, hat, "Text");
    //                        if (ds1.Tables[0].Rows.Count <= 0)
    //                        {
    //                            for (int i1 = 1; i1 < total; i1++)
    //                            {
    //                                string temp_date = todate_day.AddDays(-i1).ToString();
    //                                string temp2 = todate_day.AddDays(i1).ToString();
    //                                if (temp_date == seldate.ToString())
    //                                {
    //                                    daycheck = true;
    //                                    return daycheck;
    //                                }
    //                                if (temp2 == seldate.ToString())
    //                                {
    //                                    daycheck = true;
    //                                    return daycheck;
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            k = 0;
    //                            for (int i1 = 1; i1 < ds1.Tables[0].Rows.Count; i1++)
    //                            {
    //                                ddate[k] = ds1.Tables[0].Rows[i1][0].ToString();
    //                                k++;
    //                            }

    //                            i = 0;
    //                            while (i <= total - 1)
    //                            {
    //                                string temp_date = curdate.AddDays(-i).ToString();
    //                                for (s = 0; s < k - 1; s++)
    //                                {
    //                                    if (temp_date == ddate[s].ToString())
    //                                    {
    //                                        total = total + 1;
    //                                        goto lab;
    //                                    }

    //                                }
    //                            lab:
    //                                i = i + 1;
    //                                if (temp_date == seldate.ToString())
    //                                {
    //                                    daycheck = true;
    //                                    return daycheck;
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        daycheck = true;
    //                    }
    //                }
    //                else
    //                {
    //                    daycheck = true;
    //                }
    //            }
    //        }
    //    }
    //    return daycheck;
    //}

    #region allstudentattendancereport new table

    protected void attendanceMark(string appNo, int mnthYear, string attDay, int noMaxHrsDay, int noFstHrsDay, int noSndHrsDay, int noMinFstHrsDay, int noMinSndHrsDay, string DateVal, string collegecode)
    {
        try
        {
            DataSet dsload = new DataSet();
            Dictionary<int, int> AttValueMrng = new Dictionary<int, int>();
            Dictionary<int, int> AttvalueEve = new Dictionary<int, int>();
            double attVal = 0;
            int MPCnt = 0;
            int EPCnt = 0;
            int MnullCnt = 0;
            int EnullCnt = 0;
            string SelQ = " select " + attDay + ",A.ROLL_NO,r.app_no from attendance a,registration r where r.roll_no =a.roll_no and r.college_code='" + collegecode + "' AND month_year='" + mnthYear + "' and Att_App_no='" + appNo + "' ";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            {
                for (int sel = 0; sel < noMaxHrsDay; sel++)
                {
                    if (sel < noFstHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                MPCnt++;
                            else
                            {
                                //  MOCnt = attVal;
                                if (!AttValueMrng.ContainsKey(Convert.ToInt32(attVal)))
                                    AttValueMrng.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttValueMrng[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttValueMrng.Remove(Convert.ToInt32(attVal));
                                    AttValueMrng.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }

                        }
                        else
                            MnullCnt++;
                    }
                    else if (sel >= noSndHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                EPCnt++;
                            else
                            {
                                // EOCnt = attVal;
                                if (!AttvalueEve.ContainsKey(Convert.ToInt32(attVal)))
                                    AttvalueEve.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttvalueEve[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttvalueEve.Remove(Convert.ToInt32(attVal));
                                    AttvalueEve.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }

                        }
                        else
                            EnullCnt++;
                    }
                }
                int matt = attendanceSet(MPCnt, MnullCnt, noMinFstHrsDay, AttValueMrng);
                int eatt = attendanceSet(EPCnt, EnullCnt, noMinSndHrsDay, AttvalueEve);
                if (matt != null && eatt != null)
                {
                    string InsQ = " if exists (select * from AllStudentAttendanceReport where dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "')update AllStudentAttendanceReport set mleavecode='" + matt + "',eleavecode='" + eatt + "' where  dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "' else insert into AllStudentAttendanceReport(AppNo, DateofAttendance,MLeaveCode,ELeaveCode) values('" + dsload.Tables[0].Rows[0]["app_no"] + "','" + DateVal + "','" + matt + "','" + eatt + "')";
                    int save = d2.update_method_wo_parameter(InsQ, "Text");
                }
            }
        }
        catch { }
    }

    protected int attendanceSet(int attCnt, int nullCnt, int hrCntCheck, Dictionary<int, int> val)
    {
        int attVal = 0;
        try
        {
            //if (attCnt >= hrCntCheck)
            //    attVal = 1;
            //else if (nullCnt > 0)
            //    attVal = 0;
            //else
            //    attVal = 2;
            if (attCnt >= hrCntCheck)
                attVal = 1;
            else if (nullCnt > 0)
                attVal = 0;
            else
            {
                val = val.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                foreach (KeyValuePair<int, int> txt in val)
                {
                    attVal = Convert.ToInt32(txt.Key);
                    break;
                }
            }
        }
        catch { }
        return attVal;
    }

    #endregion

    public string GetAdminAttendanceHour()
    {
        string selectedHour = string.Empty;
        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }

            string qry = "select value from Master_Settings where settings='Admin Attendane Hour Rights' " + grouporusercode;
            selectedHour = dirAcc.selectScalarString(qry);
        }
        catch
        {
        }
        return selectedHour;
    }

}
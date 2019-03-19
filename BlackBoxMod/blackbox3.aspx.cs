using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using wc = System.Web.UI.WebControls;
using InsproDataAccess;

public partial class blackbox3 : System.Web.UI.Page
{
    bool isavailstaff = false;
    static bool forschoolsetting = false;
    DAccess2 da = new DAccess2();
    DAccess2 obi_access = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    ReuasableMethods rs = new ReuasableMethods();
    string schorder = string.Empty;
    string subcode_tot = string.Empty;
    string staff_code = string.Empty;
    string strday = string.Empty;
    string Att_strqueryst = "0", Att_strqueryst1 = "0";
    string value = string.Empty;
    string valuecam = string.Empty;
    static string vstffname = string.Empty;
    string collegecode = string.Empty;
    string course_id = string.Empty;
    string noofdays = string.Empty;
    string start_datesem = string.Empty;
    string start_dayorder = string.Empty;
    string degree_var = string.Empty;
    string tmp_datevalue = string.Empty;
    string tmp_camprevar = string.Empty;
    string cur_camprevar = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    int count = 0;
    static int branchcnt = 0;
    static int typecnt = 0;
    static int catcnt = 0;
    static int subjectcnt = 0;

    Hashtable hat = new Hashtable();
    Hashtable htsubjcount = new Hashtable();
    Hashtable ht_attncount = new Hashtable();
    Hashtable htsubjagaincount = new Hashtable();
    Hashtable htsolocamcri = new Hashtable();
    Hashtable htsolocamsubj = new Hashtable();
    Hashtable httagv = new Hashtable();
    Hashtable hatvalue = new Hashtable();

    static Hashtable ht_sch = new Hashtable();
    static Hashtable ht_sdate = new Hashtable();

    DataSet dsstaffname = new DataSet();
    DataSet dssqlfinal = new DataSet();
    DataSet dssqlfinal1 = new DataSet();
    DataSet ds = new DataSet();
    DataSet dssqlfinalcam = new DataSet();
    DataSet dsdesi = new DataSet();
    DataSet dsdept = new DataSet();
    DataSet dsstaff = new DataSet();
    DataSet ds_attndmaster = new DataSet();
    DataSet dsstuatt = new DataSet();
    DataTable dtDegSubject = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        DateTime dtcur = DateTime.Now;
        Session["curr_yearv"] = dtcur.Year;
        lblerror.Visible = false;
        if (!IsPostBack)
        {
            tbfdate.Attributes.Add("readonly", "readonly");
            tbtodate.Attributes.Add("readonly", "readonly");
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            tbfdate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
            tbtodate.Text = DateTime.Now.AddDays(0).ToString("dd-MM-yyyy");
            rdiobtndetailornot.SelectedIndex = 0;
            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].SheetName = " ";
            FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
            FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = 12;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(darkstyle);
            FpSpread1.Sheets[0].AllowTableCorner = true;
            string group_code = Session["group_code"].ToString();
            string columnfield = string.Empty;
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = da.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
            collegecode = ddlcollege.SelectedValue.ToString();
            BindDesignation();
            BindDepartment();
            BindStaff();
            BindSubject();
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
            schoolds = obi_access.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables.Count > 0 && schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                if (schoolvalue.Trim() == "0")
                {
                    forschoolsetting = true;
                    Label1.Text = "School";
                    deptlbl.Text = "Year";
                    lblstaff.Text = "Standard";
                    chkdegreewise.Text = "Standard Wise";
                }
                else
                {
                    forschoolsetting = false;
                }
            }
            chklscolumn.Items[0].Selected = true;
            chklscolumn.Items[4].Selected = true;
            chklscolumn.Items[5].Selected = true;
        }
        else
        {
            collegecode = ddlcollege.SelectedValue.ToString();
        }
    }

    public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    {
        int holiday = 0;
        if (no_days == "")
            return "";
        if (sdate != "")
        {
            DateTime dt1 = Convert.ToDateTime(sdate);
            DateTime dt2 = Convert.ToDateTime(curday);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*)as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";
            string holday = da.GetFunctionv(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);
            int dif_days = ts.Days;
            int nodays = Convert.ToInt32(no_days);
            int order = (dif_days - holiday) % nodays;
            order = order + 1;
            if (stastdayorder.ToString().Trim() != "")
            {
                if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
                {
                    order = order + (Convert.ToInt16(stastdayorder) - 1);
                    if (order == (nodays + 1))
                        order = 1;
                    else if (order > nodays)
                        order = order % nodays;
                }
            }
            string findday = string.Empty;
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";
            return findday;
        }
        else
            return "";
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }
        return null;
    }

    protected void tbfdate_TextChanged(object sender, EventArgs e)
    {
        string date1 = string.Empty;
        string datefrom = string.Empty;
        lblerror.Visible = false;
        Labelstaf.Visible = false;
        lbldatediff.Visible = false;
        date1 = tbfdate.Text.ToString();
        string[] split = date1.Split(new Char[] { '-' });
        datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
        if (dt1 > DateTime.Today)
        {
            Labelstaf.Visible = true;
            Labelstaf.Text = "You can not select the date greater than today";
            tbfdate.Text = DateTime.Today.ToString("d-MM-yyyy");
        }
        else
        {
            string dateto = tbtodate.Text;
            string[] spilttodate = dateto.Split('-');
            DateTime dto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
            if (dt1 > dto)
            {
                Labelstaf.Visible = true;
                Labelstaf.Text = "You can not select the From date greater than Todate";
            }
        }
    }

    protected void tbtodate_TextChanged(object sender, EventArgs e)
    {
        string date1 = string.Empty;
        string datefrom = string.Empty;
        lblerror.Visible = false;
        Labelstaf.Visible = false;
        lbldatediff.Visible = false;
        date1 = tbtodate.Text.ToString();
        string[] split = date1.Split(new Char[] { '-' });
        datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
        if (dt1 > DateTime.Today)
        {
            Labelstaf.Visible = true;
            Labelstaf.Text = "You can not select the date greater than today";
            tbtodate.Text = DateTime.Today.ToString("d-MM-yyyy");
        }
        else
        {
            string dateto = tbfdate.Text;
            string[] spilttodate = dateto.Split('-');
            DateTime dfrom = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
            if (dt1 < dfrom)
            {
                Labelstaf.Visible = true;
                Labelstaf.Text = "You can not select the From date greater than Todate";
            }
        }
    }

    public void BindBatch()
    {
        try
        {
            chkbranch.Checked = false;
            txtbranch.Text = "---Select---";
            ds.Dispose();
            ds.Reset();
            string strsql = "select distinct batch_year from registration where batch_year<>'-1' and batch_year<>'' and cc='0' order by batch_year desc";
            DataSet bibatch = da.select_method_wo_parameter(strsql, "text");
            //ds = obi_access.BindBatch();
            if (bibatch.Tables.Count > 0 && bibatch.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = bibatch;
                chklstbranch.DataTextField = "Batch_year";
                chklstbranch.DataValueField = "Batch_year";
                chklstbranch.DataBind();
                // chklstbranch.SelectedIndex = chklstbranch.Items.Count - 1;
                // txtbranch.Text = "Batch (1)";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            cbldesi.Items.Clear();
            chkdesi.Checked = false;
            txtdesi.Text = "---Select---";
            ds.Dispose();
            ds.Reset();
            ds = obi_access.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbldesi.DataSource = ds;
                cbldesi.DataTextField = "course_name";
                cbldesi.DataValueField = "course_id";
                cbldesi.DataBind();
                //cbldesi.Items[0].Selected = true;
                //txtdesi.Text = "Degree (1)";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            chkstaff.Checked = false;
            chklststaff.Items.Clear();
            txtstaff.Text = "---Select---";
            collegecode = ddlcollege.SelectedValue.ToString();
            for (int i = 0; i < cbldesi.Items.Count; i++)
            {
                if (cbldesi.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "'" + cbldesi.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        course_id = course_id + "," + "'" + cbldesi.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = obi_access.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklststaff.DataSource = ds;
                chklststaff.DataTextField = "dept_name";
                chklststaff.DataValueField = "degree_code";
                chklststaff.DataBind();
                //chklststaff.Items[0].Selected = true;
                //txtstaff.Text = "Branch (1)";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void BindDesignation()
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            dsdesi.Dispose();
            dsdesi.Reset();
            txtdesi.Text = "---Select---";
            chkdesi.Checked = false;
            string academiconly = da.GetFunction("select value from Master_Settings where settings='Black Box Academic'");
            if (academiconly.Trim() == "1")
            {
                string getdesign = "select  distinct desig_name,desig_code from desig_master where collegeCode='" + collegecode + "' and staffcategory='Teaching' order by desig_name";
                dsdesi = da.select_method_wo_parameter(getdesign, "text");
            }
            else
            {
                dsdesi = da.binddesi(collegecode);
            }
            if (dsdesi.Tables.Count > 0 && dsdesi.Tables[0].Rows.Count > 0)
            {
                cbldesi.DataSource = dsdesi;
                cbldesi.DataValueField = "desig_code";
                cbldesi.DataTextField = "desig_name";
                cbldesi.DataBind();
                //cbldesi.SelectedIndex = cbldesi.Items.Count - 1;
                //for (int i = 0; i < cbldesi.Items.Count; i++)
                //{
                //    cbldesi.Items[i].Selected = true;
                //    if (cbldesi.Items[i].Selected == true)
                //    {
                //        count += 1;
                //    }
                //}
                //if (count > 0)
                //{
                //    txtdesi.Text = "Designation (" + cbldesi.Items.Count + ")";
                //    if (cbldesi.Items.Count == count)
                //    {
                //        chkdesi.Checked = true;
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void BindDepartment()
    {
        chklstbranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode = ddlcollege.SelectedValue.ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        string academiconly = da.GetFunction("select value from Master_Settings where settings='Black Box Academic'");
        string strdeptquery = string.Empty;
        if (singleuser.ToLower().Trim() == "true")
        {
            if (academiconly.Trim() == "1")
            {
                strdeptquery = "select distinct d.Dept_Code,d.Dept_Name from hrdept_master d,hr_privilege p,stafftrans t where d.dept_code = p.dept_code and d.dept_code = t.dept_code and d.college_code = '" + collegecode + "' and t.stftype like 'Tea%' and p.user_code='" + usercode + "'";
            }
            else
            {
                strdeptquery = "select distinct d.dept_name,d.dept_code from hrdept_master d,hr_privilege h where h.Dept_Code=h.dept_code and h.user_code='" + usercode + "' and d.college_code='" + collegecode + "'";
            }
        }
        else
        {
            if (academiconly.Trim() == "1")
            {
                strdeptquery = "select distinct d.Dept_Code,d.Dept_Name from hrdept_master d,hr_privilege p,stafftrans t where d.dept_code = p.dept_code and d.dept_code = t.dept_code and d.college_code = '" + collegecode + "' and t.stftype like 'Tea%' and p.group_code='" + group_user + "'";
            }
            else
            {
                strdeptquery = "select distinct d.dept_name,d.dept_code from hrdept_master d,hr_privilege h where h.Dept_Code=h.dept_code and h.group_code='" + group_user + "' and d.college_code='" + collegecode + "'";
            }
        }
        ds = da.select_method_wo_parameter(strdeptquery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                chklstbranch.DataSource = ds;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "Dept_Code";
                chklstbranch.DataBind();
            }
        }
    }

    public void BindStaff()
    {
        string course_id = "", dept_id = string.Empty;
        for (int i = 0; i < cbldesi.Items.Count; i++)
        {
            if (cbldesi.Items[i].Selected == true)
            {
                if (course_id == "")
                {
                    course_id = "'" + cbldesi.Items[i].Value.ToString() + "'";
                }
                else
                {
                    course_id = course_id + "," + "'" + cbldesi.Items[i].Value.ToString() + "'";
                }
            }
        }
        if (chkdegreewise.Checked != true)
        {
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (dept_id == "")
                    {
                        dept_id = "" + chklstbranch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        dept_id = dept_id + "," + "" + chklstbranch.Items[i].Value.ToString() + "";
                    }
                }
            }
        }
        if (dept_id == "" || dept_id == null)
        {
            dept_id = "''";
        }
        if (course_id == "" || course_id == null)
        {
            course_id = "''";
        }
        count = 0;
        string nm = string.Empty;
        collegecode = ddlcollege.SelectedValue.ToString();
        if (chkdegreewise.Checked != true)
        {
            nm = "select distinct  sm.staff_name,sm.staff_code,(sm.staff_name+'-'+sm.staff_code) as staffnamecode from stafftrans s,staffmaster sm,staff_selector st where s.staff_code=sm.staff_code and college_code='" + collegecode + "'  and s.dept_code in(" + dept_id + ") and s.desig_code in(" + course_id + ") and resign = 0 and settled = 0 and st.staff_code=sm.staff_code order by sm.staff_name";
            dsstaff = da.select_method_wo_parameter(nm, "text");
            if (dsstaff.Tables.Count > 0 && dsstaff.Tables[0].Rows.Count > 0)
            {
                chklststaff.DataSource = dsstaff;
                chklststaff.DataTextField = "staff_name";
                chklststaff.DataValueField = "staff_code";
                chklststaff.DataBind();
            }
            if (chklststaff.Items.Count > 0)
            {
                txtstaff.Text = "Staff (" + chklststaff.Items.Count + ")";
            }
            else
            {
                txtstaff.Text = "---Select---";
            }
        }
        else
        {
            ds = obi_access.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklststaff.DataSource = ds;
                chklststaff.DataTextField = "dept_name";
                chklststaff.DataValueField = "degree_code";
                chklststaff.DataBind();
            }
        }
        for (int i = 0; i < chklststaff.Items.Count; i++)
        {
            chklststaff.Items[i].Selected = true;
            if (chklststaff.Items[i].Selected == true)
            {
                count += 1;
            }
            if (chklststaff.Items.Count == count)
            {
                chkstaff.Checked = true;
            }
        }
    }

    public void BindSubject()
    {
        try
        {
            count = 0;
            chklstsubject.Items.Clear();
            string strsqlhourv = da.GetFunctionv("select max(No_of_hrs_per_day) as No_of_hrs_per_day from PeriodAttndSchedule");
            int hrcount = Convert.ToInt32(strsqlhourv);
            for (int i = 0; i < hrcount; i++)
            {
                chklstsubject.Items.Add("" + (i + 1).ToString().Trim() + "");
                chklstsubject.Items[i].Selected = true;
                if (chklstsubject.Items[i].Selected == true)
                {
                    count += 1;
                }
                if (chklstsubject.Items.Count == count)
                {
                    chksubject.Checked = true;
                }
            }
            if (chklstsubject.Items.Count > 0)
            {
                txtsubject.Text = "Hour (" + chklstsubject.Items.Count + ")";
            }
            else
            {
                txtsubject.Text = "---Select---";
            }
        }
        catch (Exception ev)
        {
            string strsubj = ev.ToString();
        }
    }

    public void BindDegSubject()
    {
        try
        {
            txtDegSubject.Text = "---Select---";
            dtDegSubject.Clear();
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            cblDegSubject.Items.Clear();
            chkDegSubject.Checked = false;
            if (chklstbranch.Items.Count > 0)
            {
                valBatch = rs.GetSelectedItemsValueAsString(chklstbranch);
            }
            if (chklststaff.Items.Count > 0)
            {
                valDegree = rs.GetSelectedItemsValueAsString(chklststaff);
            }
            if (!string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                //string strDegSubject = "select distinct s.subject_no,s.subject_name,ISNULL(s.subjectpriority,'0') subjectpriority from subject s,sub_sem ss,syllabus_master sm,Registration r,subjectChooser sc where sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and sm.syll_code=s.syll_code and s.subType_no=ss.subType_no and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.Current_Semester=sm.semester and r.Current_Semester=sc.semester and sm.Batch_Year in ('" + valBatch + "') and sm.degree_code in ('" + valDegree + "') and r.cc=0 and delflag=0 and r.exam_flag<>'debar' order by subjectpriority"; // add and r.Current_Semester=sc.semester



               // select distinct s.subject_no,s.subject_name,ISNULL(s.subjectpriority,'0') subjectpriority from subject s,sub_sem ss,syllabus_master sm,Registration r,subjectChooser sc where  sm.syll_code=s.syll_code and s.subType_no=ss.subType_no and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.Current_Semester=sm.semester and r.Current_Semester=sc.semester and r.Batch_Year in ('2017') and r.degree_code in ('65') and r.cc=0 and delflag=0 and r.exam_flag<>'debar' order by subjectpriority

                string strDegSubject = "   select distinct s.subject_no,s.subject_name,ISNULL(s.subjectpriority,'0') subjectpriority from subject s,sub_sem ss,syllabus_master sm,Registration r where  sm.syll_code=s.syll_code and sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and s.subType_no=ss.subType_no and r.Current_Semester=sm.semester  and r.Batch_Year in ('" + valBatch + "') and r.degree_code in ('" + valDegree + "') and r.cc=0 and delflag=0 and r.exam_flag<>'debar' order by subjectpriority";

                dtDegSubject = dirAcc.selectDataTable(strDegSubject);
                if (dtDegSubject.Rows.Count > 0)
                {
                    cblDegSubject.DataSource = dtDegSubject;
                    cblDegSubject.DataTextField = "subject_name";
                    cblDegSubject.DataValueField = "subject_no";
                    cblDegSubject.DataBind();
                    checkBoxListselectOrDeselect(cblDegSubject, true);
                    CallCheckboxListChange(chkDegSubject, cblDegSubject, txtDegSubject, lblDegSubject.Text, "--Select--");
                }
            }
        }
        catch
        {
        }

    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {

        if (chkdegreewise.Checked == false)
        {
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    txtbranch.Text = "Department (" + (chklstbranch.Items.Count) + ")";
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
            BindStaff();
        }
        else
        {
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    txtbranch.Text = "Batch (" + (chklstbranch.Items.Count) + ")";
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
            // BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            BindDegSubject();
        }
        //if (chkbranch.Checked == true)
        //{
        //    for (int i = 0; i < chklstbranch.Items.Count; i++)
        //    {
        //        chklstbranch.Items[i].Selected = true;
        //        if (chkdegreewise.Checked != true)
        //        {
        //            txtbranch.Text = "Department (" + (chklstbranch.Items.Count) + ")";
        //        }
        //        else
        //        {
        //            txtbranch.Text = "Batch (" + (chklstbranch.Items.Count) + ")";
        //            BindDegSubject();
        //        }
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < chklstbranch.Items.Count; i++)
        //    {
        //        chklstbranch.Items[i].Selected = false;
        //        txtbranch.Text = "---Select---";
        //    }
        //}
        //BindStaff();
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        int cou = 0;
        chkbranch.Checked = false;
        if (chkdegreewise.Checked == false)
        {
            pbranch.Focus();
            int branchcount = 0;
            string value = string.Empty;
            string code = string.Empty;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    cou++;
                    value = chklstbranch.Items[i].Text;
                    code = chklstbranch.Items[i].Value.ToString();
                    branchcount = branchcount + 1;
                    txtbranch.Text = "Department (" + branchcount.ToString() + ")";
                }
            }
            if (branchcount == 0)
                txtbranch.Text = "---Select---";
            else
            {
                Label lbl = branchlabel();
                lbl.Text = " " + value + " ";
                lbl.ID = "lbl1-" + code.ToString();
                ImageButton ib = branchimage();
                ib.ID = "imgbut1_" + code.ToString();
                ib.Click += new ImageClickEventHandler(branchimg_Click);
            }
            branchcnt = branchcount;
            BindStaff();
        }
        else
        {
            pbranch.Focus();
            int degreecount = 0;
            string value = string.Empty;
            string code = string.Empty;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    cou++;
                    value = chklstbranch.Items[i].Text;
                    code = chklstbranch.Items[i].Value.ToString();
                    degreecount = degreecount + 1;
                    txtbranch.Text = "Batch (" + degreecount.ToString() + ")";
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            //BindDepartment();
            BindDegSubject();
        }
        if (cou > 0 && cou == chklstbranch.Items.Count)
        {
            chkbranch.Checked = true;
        }
    }

    protected void LinkButtonbranch_Click(object sender, EventArgs e)
    {
        chklstbranch.ClearSelection();
        branchcnt = 0;
        txtbranch.Text = "---Select---";
    }

    public void branchimg_Click(object sender, ImageClickEventArgs e)
    {
        branchcnt = branchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstbranch.Items[r].Selected = false;
        txtbranch.Text = "Department (" + branchcnt.ToString() + ")";
        if (txtbranch.Text == "Department(0)")
        {
            txtbranch.Text = "---Select---";
        }
    }

    public Label branchlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton branchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    protected void LinkButtondesi_Click(object sender, EventArgs e)
    {
        cbldesi.ClearSelection();
        typecnt = 0;
        txtdesi.Text = "---Select---";
    }

    protected void chkdesi_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdegreewise.Checked == false)
        {
            if (chkdesi.Checked == true)
            {
                for (int i = 0; i < cbldesi.Items.Count; i++)
                {
                    cbldesi.Items[i].Selected = true;
                    txtdesi.Text = "Designation (" + (cbldesi.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbldesi.Items.Count; i++)
                {
                    cbldesi.Items[i].Selected = false;
                    txtdesi.Text = "---Select---";
                }
            }
            BindStaff();
        }
        else
        {
            if (chkdesi.Checked == true)
            {
                for (int i = 0; i < cbldesi.Items.Count; i++)
                {
                    cbldesi.Items[i].Selected = true;
                    txtdesi.Text = "Degree (" + (cbldesi.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbldesi.Items.Count; i++)
                {
                    cbldesi.Items[i].Selected = false;
                    txtdesi.Text = "---Select---";
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            BindDegSubject();
        }
    }

    protected void cbldesi_SelectedIndexChanged(object sender, EventArgs e)
    {
        int cou = 0;
        chkdesi.Checked = false;
        if (chkdegreewise.Checked == false)
        {
            int bloodcount = 0;
            string value = string.Empty;
            string code = string.Empty;
            for (int i = 0; i < cbldesi.Items.Count; i++)
            {
                if (cbldesi.Items[i].Selected == true)
                {
                    cou++;
                    value = cbldesi.Items[i].Text;
                    code = cbldesi.Items[i].Value.ToString();
                    bloodcount = bloodcount + 1;
                    txtdesi.Text = "Designation (" + bloodcount.ToString() + ")";
                }
            }
            if (bloodcount == 0)
            {
                txtdesi.Text = "---Select---";
            }
            else
            {
                Label lbl = bloodlabeldesi();
                lbl.Text = " " + value + " ";
                lbl.ID = "lbl2-" + code.ToString();
                ImageButton ib = bloodimagdesi();
                ib.ID = "imgbut2_" + code.ToString();
                ib.Click += new ImageClickEventHandler(bloodimgdesi_Click);
            }
            typecnt = bloodcount;
            BindStaff();
        }
        else
        {
            int batchcount = 0;
            string value = string.Empty;
            string code = string.Empty;
            for (int i = 0; i < cbldesi.Items.Count; i++)
            {
                if (cbldesi.Items[i].Selected == true)
                {
                    cou++;
                    value = cbldesi.Items[i].Text;
                    code = cbldesi.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                    txtdesi.Text = "Degree (" + batchcount.ToString() + ")";
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            BindDegSubject();
        }
        if (cou > 0 && cou == cbldesi.Items.Count)
        {
            chkdesi.Checked = true;
        }
    }

    public Label bloodlabeldesi()
    {
        Label lbc = new Label();
        ViewState["idesignationcontrol"] = true;
        return (lbc);
    }

    public ImageButton bloodimagdesi()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["idesignationcontrol"] = true;
        return (imc);
    }

    public void bloodimgdesi_Click(object sender, ImageClickEventArgs e)
    {
        typecnt = typecnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cbldesi.Items[r].Selected = false;
        txtdesi.Text = "Designation (" + typecnt.ToString() + ")";
        if (txtdesi.Text == "Designation(0)")
        {
            txtdesi.Text = "---Select---";
        }
    }

    protected void LinkButtonstafftype_Click(object sender, EventArgs e)
    {
        chklststaff.ClearSelection();
        catcnt = 0;
        txtstaff.Text = "---Select---";
    }

    protected void chkstaff_CheckedChanged(object sender, EventArgs e)
    {
        string degreecount = string.Empty;
        if (chkdegreewise.Checked == true)
        {
            degreecount = "Branch ";

            if (chkstaff.Checked == true)
            {
                for (int i = 0; i < chklststaff.Items.Count; i++)
                {
                    chklststaff.Items[i].Selected = true;
                    txtstaff.Text = degreecount + "(" + (chklststaff.Items.Count) + ")";

                }
            }
            else
            {
                for (int i = 0; i < chklststaff.Items.Count; i++)
                {
                    chklststaff.Items[i].Selected = false;
                    txtstaff.Text = "---Select---";
                }
            }
            BindDegSubject();
        }
        else
        {
            degreecount = "Staff ";
            if (chkstaff.Checked == true)
            {
                for (int i = 0; i < chklststaff.Items.Count; i++)
                {
                    chklststaff.Items[i].Selected = true;
                    txtstaff.Text = degreecount + "(" + (chklststaff.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklststaff.Items.Count; i++)
                {
                    chklststaff.Items[i].Selected = false;
                    txtstaff.Text = "---Select---";
                }
            }
        }

    }

    protected void chklststaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        int cou = 0;
        chkstaff.Checked = false;
        if (chkdegreewise.Checked == false)
        {
            int bloodcount = 0;
            string value = string.Empty;
            string code = string.Empty;
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                if (chklststaff.Items[i].Selected == true)
                {
                    cou++;
                    value = chklststaff.Items[i].Text;
                    code = chklststaff.Items[i].Value.ToString();
                    bloodcount = bloodcount + 1;
                    txtstaff.Text = "Staff (" + bloodcount.ToString() + ")";
                }
            }
            if (bloodcount == 0)
            {
                txtstaff.Text = "---Select---";
            }
            else
            {
                Label lbl = bloodlabelsf();
                lbl.Text = " " + value + " ";
                lbl.ID = "lbl2-" + code.ToString();
                ImageButton ib = bloodimagesf();
                ib.ID = "imgbut2_" + code.ToString();
                ib.Click += new ImageClickEventHandler(bloodimgsf_Click);
            }
            catcnt = bloodcount;
        }
        else
        {
            int branchcount = 0;
            string value = string.Empty;
            string code = string.Empty;
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                if (chklststaff.Items[i].Selected == true)
                {
                    cou++;
                    value = chklststaff.Items[i].Text;
                    code = chklststaff.Items[i].Value.ToString();
                    branchcount = branchcount + 1;
                    txtstaff.Text = "Branch (" + branchcount.ToString() + ")";
                }
            }
            BindDegSubject();
        }
        if (cou > 0 && cou == chklststaff.Items.Count)
        {
            chkstaff.Checked = true;
        }
    }

    public Label bloodlabelsf()
    {
        Label lbc = new Label();
        ViewState["istafftypecontrol"] = true;
        return (lbc);
    }

    public ImageButton bloodimagesf()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["istafftypecontrol"] = true;
        return (imc);
    }

    public void bloodimgsf_Click(object sender, ImageClickEventArgs e)
    {
        catcnt = catcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklststaff.Items[r].Selected = false;
        txtstaff.Text = "Staff (" + catcnt.ToString() + ")";
        if (txtstaff.Text == "Staff(0)")
        {
            txtstaff.Text = "---Select---";
        }
    }

    protected void chksubject_CheckedChanged(object sender, EventArgs e)
    {
        if (chksubject.Checked == true)
        {
            for (int i = 0; i < chklstsubject.Items.Count; i++)
            {
                chklstsubject.Items[i].Selected = true;
                txtsubject.Text = "Hour (" + (chklstsubject.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstsubject.Items.Count; i++)
            {
                chklstsubject.Items[i].Selected = false;
                txtsubject.Text = "---Select---";
            }
        }
    }

    protected void chklstsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        psubject.Focus();
        int subjectcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < chklstsubject.Items.Count; i++)
        {
            if (chklstsubject.Items[i].Selected == true)
            {
                value = chklstsubject.Items[i].Text;
                code = chklstsubject.Items[i].Value.ToString();
                subjectcount = subjectcount + 1;
                txtsubject.Text = "Hour (" + subjectcount.ToString() + ")";
            }
        }
        if (subjectcount == 0)
            txtsubject.Text = "---Select---";
        else
        {
            Label lbl = subjectlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = subjectimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(subjectimg_Click);
        }
        subjectcnt = subjectcount;
    }

    public void subjectimg_Click(object sender, ImageClickEventArgs e)
    {
        subjectcnt = subjectcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstsubject.Items[r].Selected = false;
        txtsubject.Text = "Hour (" + subjectcnt.ToString() + ")";
        if (txtsubject.Text == "Hour(0)")
        {
            txtsubject.Text = "---Select---";
        }
    }

    public Label subjectlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton subjectimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    protected void Buttongo_Click(object sender, EventArgs e)
    {
        try
        {
            bool notimetable = false;
            if (chkdegreewise.Checked && rbCAM.Checked)
            {
                CAMReport();
            }
            else
            {
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                FpSpread1.Visible = false;
                bool semflag = false;
                lblerror.Visible = false;
                Labelstaf.Visible = false;
                if (chkdegreewise.Checked == false)
                {
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.Sheets[0].RowCount = 0;
                    string date1 = tbfdate.Text.ToString();
                    string[] split = date1.Split(new Char[] { '-' });
                    string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
                    string date2 = tbtodate.Text.ToString();
                    string[] split1 = date2.Split(new Char[] { '-' });
                    string dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
                    DateTime dt1 = Convert.ToDateTime(datefrom);
                    DateTime dt2 = Convert.ToDateTime(dateto);
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                    if (dt1 <= dt2)
                    {
                        bool isChoiceBased = false;
                        TimeSpan t = dt2.Subtract(dt1);
                        int days = t.Days;
                        DataSet dsgetvalue = new DataSet();
                        string sql1 = string.Empty;
                        string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
                        DataSet dsalterperiod = new DataSet();
                        Hashtable hatsublab = new Hashtable();
                        DataSet dsstuatt = new DataSet();
                        Hashtable hatvalue = new Hashtable();
                        int noofhrs = 0;
                        string vari = string.Empty;
                        Hashtable ht_sch = new Hashtable();
                        DataSet ds_attndmaster = new DataSet();
                        string degree_var = string.Empty;
                        hat.Clear();
                        hat.Add("college_code", Session["collegecode"].ToString());
                        string sql_stringvar = "sp_select_details_staff";
                        ds_attndmaster.Dispose();
                        ds_attndmaster.Reset();
                        ds_attndmaster = da.select_method(sql_stringvar, hat, "sp");
                        if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                        {
                            for (int pcont = 0; pcont < ds_attndmaster.Tables[0].Rows.Count; pcont++)
                            {
                                degree_var = Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["semester"]);
                                if (!ht_sch.Contains(Convert.ToString(degree_var)))
                                {
                                    vari = ds_attndmaster.Tables[0].Rows[pcont]["SchOrder"] + "," + ds_attndmaster.Tables[0].Rows[pcont]["nodays"];
                                    ht_sch.Add(degree_var, Convert.ToString(vari));
                                }
                            }
                        }
                        Hashtable ht_sdate = new Hashtable();
                        ht_sdate.Clear();
                        if (ds_attndmaster.Tables.Count > 1 && ds_attndmaster.Tables[1].Rows.Count > 0)
                        {
                            for (int pcont = 0; pcont < ds_attndmaster.Tables[1].Rows.Count; pcont++)
                            {
                                degree_var = Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["semester"]);
                                if (!ht_sdate.Contains(Convert.ToString(degree_var)))
                                {
                                    vari = ds_attndmaster.Tables[1].Rows[pcont]["sdate"] + "," + ds_attndmaster.Tables[1].Rows[pcont]["starting_dayorder"];
                                    ht_sdate.Add(degree_var, Convert.ToString(vari));
                                }
                            }
                        }
                        string degreename = string.Empty;
                        Hashtable hatdegreename = new Hashtable();
                        if (ds_attndmaster.Tables.Count > 5 && ds_attndmaster.Tables[5].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds_attndmaster.Tables[5].Rows.Count; i++)
                            {
                                if (!hatdegreename.Contains(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString()))
                                {
                                    hatdegreename.Add(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString(), ds_attndmaster.Tables[5].Rows[i]["course"].ToString() + '-' + ds_attndmaster.Tables[5].Rows[i]["dept_acronym"].ToString());
                                }
                            }
                        }
                        noofhrs = 0;
                        if (ds_attndmaster.Tables.Count > 6 && ds_attndmaster.Tables[6].Rows.Count > 0)
                        {
                            if (ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != "" && ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != null && ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != "0")
                            {
                                noofhrs = Convert.ToInt32(ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString());
                            }
                        }
                        string stfcode = string.Empty;
                        for (int stf = 0; stf < chklststaff.Items.Count; stf++)
                        {
                            if (chklststaff.Items[stf].Selected == true)
                            {
                                if (stfcode == "")
                                    stfcode = "'" + chklststaff.Items[stf].Value.ToString() + "'";
                                else
                                    stfcode = stfcode + "," + "'" + chklststaff.Items[stf].Value.ToString() + "'";
                            }
                        }
                        if (stfcode != "")
                        {
                            string getalldetails = "select * from Alternate_Schedule where FromDate between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' ; ";
                            getalldetails = getalldetails + "select * from Semester_Schedule order by FromDate desc; ";
                            getalldetails = getalldetails + "Select * from holidaystudents where holiday_date between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' ; ";
                            getalldetails = getalldetails + "select * from dailyentdet de,dailystaffentry ds where de.lp_code=ds.lp_code and ds.sch_date between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "'  ; ";
                            getalldetails = getalldetails + " select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester,ss.Lab from subject s,sub_sem ss,syllabus_master sy,Registration r where s.syll_code=ss.syll_code and s.syll_code=sy.syll_code and s.subType_no=ss.subType_no and ss.syll_code=sy.syll_code and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and r.Current_Semester=sy.semester and cc=0 and r.Exam_Flag<>'debar' and r.DelFlag=0 and r.college_code='13';";
                            getalldetails = getalldetails + " select distinct Current_Semester,Batch_Year,degree_code from Registration where cc=0 and delflag=0 and exam_flag<>'debar'; ";
                            getalldetails = getalldetails + " select no_of_hrs_I_half_day as mor,no_of_hrs_I_half_day as eve,degree_code,semester from periodattndschedule";
                            getalldetails = getalldetails + " select c.criteria,c.Criteria_no,e.exam_code,sy.Batch_Year,sy.degree_code,sy.semester,e.sections,s.subject_no,s.subject_name,c.LastDate,st.staff_code from CriteriaForInternal c,syllabus_master sy,subject s,Exam_type e,staff_selector st where c.syll_code=s.syll_code and c.Criteria_no=e.criteria_no and e.subject_no=s.subject_no and sy.syll_code=s.syll_code and e.subject_no=st.subject_no and s.subject_no=st.subject_no and c.LastDate between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' ";
                            getalldetails = getalldetails + " select count(r.roll_no) as stucount,c.criteria,c.Criteria_no,e.exam_code,sy.Batch_Year,sy.degree_code,sy.semester,e.sections,s.subject_no,s.subject_name,c.LastDate from CriteriaForInternal c,syllabus_master sy,subject s,Exam_type e,Result r where c.syll_code=s.syll_code and c.Criteria_no=e.criteria_no and e.subject_no=s.subject_no  and sy.syll_code=s.syll_code and r.exam_code=e.exam_code and c.LastDate between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' group by c.criteria,c.Criteria_no,e.exam_code,sy.Batch_Year,sy.degree_code,sy.semester,e.sections,s.subject_no,s.subject_name,c.LastDate";
                            DataSet dsall = da.select_method_wo_parameter(getalldetails, "Text");//
                            //isChoiceBasedSystemWithBatch
                            string strsubstucount = " select count(distinct r.Roll_No) as stucount,r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.subject_no,r.adm_date from registration r,subjectchooser s where  r.roll_no=s.roll_no and  r.current_semester=s.semester";
                            strsubstucount = strsubstucount + " and cc=0 and delflag=0 and exam_flag<>'debar' group by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.subject_no,r.adm_date";
                            DataSet dssubstucount = da.select_method_wo_parameter(strsubstucount, "Text");
                            stfcode = "staff_code in( " + stfcode + ")";
                            string sqlcmdstaffname = "select ROW_NUMBER() OVER (ORDER BY  st.staff_code) As SrNo,st.staff_code,staff_name,sm.college_code,h.dept_name,d.desig_name,sc.category_name,st.stftype from staffmaster sm,stafftrans st,hrdept_master h,desig_master d,staffcategorizer sc where sm.staff_code=st.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.category_code=sc.category_code and sm.college_code=h.college_code and sm.college_code=d.collegeCode and sm.college_code=sc.college_code and st.latestrec=1 and st." + stfcode + " order by h.priority,d.priority,sm.join_date,st.staff_code";
                            dsstaffname = da.select_method(sqlcmdstaffname, hat, "Text");
                            if (dsstaffname != null && dsstaffname.Tables[0] != null && dsstaffname.Tables.Count > 0 && dsstaffname.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].ColumnCount = 0;
                                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                                FpSpread1.Sheets[0].ColumnCount = 7;
                                FpSpread1.Width = 600;
                                FpSpread1.Sheets[0].AutoPostBack = false;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                                if (chklscolumn.Items[0].Selected == true)
                                {
                                    FpSpread1.Sheets[0].Columns[1].Visible = true;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Columns[1].Visible = false;
                                }
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                                if (chklscolumn.Items[1].Selected == true)
                                {
                                    FpSpread1.Sheets[0].Columns[2].Visible = true;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Columns[2].Visible = false;
                                }
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Category";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                                if (chklscolumn.Items[2].Selected == true)
                                {
                                    FpSpread1.Sheets[0].Columns[3].Visible = true;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Columns[3].Visible = false;
                                }
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Staff Type";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                                if (chklscolumn.Items[3].Selected == true)
                                {
                                    FpSpread1.Sheets[0].Columns[4].Visible = true;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Columns[4].Visible = false;
                                }
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Staff Code";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                                if (chklscolumn.Items[4].Selected == true)
                                {
                                    FpSpread1.Sheets[0].Columns[5].Visible = true;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Columns[5].Visible = false;
                                }
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Staff Name";
                                if (chklscolumn.Items[5].Selected == true)
                                {
                                    FpSpread1.Sheets[0].Columns[6].Visible = true;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Columns[6].Visible = false;
                                }
                                FpSpread1.Sheets[0].FrozenColumnCount = 7;
                                FpSpread1.Sheets[0].Columns[0].Locked = true;
                                FpSpread1.Sheets[0].Columns[1].Locked = true;
                                FpSpread1.Sheets[0].Columns[2].Locked = true;
                                FpSpread1.Sheets[0].Columns[3].Locked = true;
                                FpSpread1.Sheets[0].Columns[4].Locked = true;
                                FpSpread1.Sheets[0].Columns[5].Locked = true;
                                FpSpread1.Sheets[0].Columns[6].Locked = true;
                                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
                                FpSpread1.Sheets[0].Columns[0].Width = 40;
                                FpSpread1.Sheets[0].Columns[1].Width = 150;
                                FpSpread1.Sheets[0].Columns[2].Width = 150;
                                FpSpread1.Sheets[0].Columns[3].Width = 100;
                                FpSpread1.Sheets[0].Columns[4].Width = 100;
                                FpSpread1.Sheets[0].Columns[5].Width = 100;
                                FpSpread1.Sheets[0].Columns[6].Width = 300;
                                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Hashtable hatdate = new Hashtable();
                                for (int row_inc33 = 0; row_inc33 <= days; row_inc33++)
                                {
                                    DateTime cur_day33 = new DateTime();
                                    cur_day33 = dt1.AddDays(row_inc33);
                                    if (cur_day33.ToString("ddd").Trim().ToLower() != "sun")
                                    {
                                        FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 2;
                                        hatdate.Add(cur_day33.ToString("MM/dd/yyyy"), FpSpread1.Sheets[0].ColumnCount - 2);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "Attn";
                                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;
                                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Cam ";
                                        if (forschoolsetting == true)
                                        {
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Test ";
                                        }
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 1, 2);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Text = cur_day33.ToString("d-MM-yyyy");
                                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                                string check_lab = string.Empty;
                                int sno = 0;
                                if (dsstaffname.Tables.Count > 0 && dsstaffname.Tables[0].Rows.Count > 0)
                                {
                                    for (int rowi = 0; rowi < dsstaffname.Tables[0].Rows.Count; rowi++)
                                    {
                                        bool attendanceentryflag = false;
                                        string sectionvar = string.Empty;
                                        string sectionsvalue = string.Empty;
                                        isavailstaff = false;
                                        htsubjcount.Clear();
                                        ht_attncount.Clear();
                                        staff_code = dsstaffname.Tables[0].Rows[rowi]["staff_code"].ToString();
                                        string SqlFinal = " select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from staff_selector ss,Registration r,";
                                        SqlFinal = SqlFinal + " subject s,sub_sem sm,syllabus_master sy,seminfo si where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
                                        SqlFinal = SqlFinal + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
                                        SqlFinal = SqlFinal + " and s.subject_no=ss.subject_no and r.sections=ss.sections and ss.batch_year=r.Batch_Year";
                                        SqlFinal = SqlFinal + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
                                        SqlFinal = SqlFinal + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
                                        SqlFinal = SqlFinal + " and r.DelFlag=0 and ss.staff_code='" + staff_code + "'";
                                        DataView dvalternaet = new DataView();
                                        DataView dvsemster = new DataView();
                                        DataView dvholiday = new DataView();
                                        DataView dvdaily = new DataView();
                                        DataView dvsubject = new DataView();
                                        DataView dvsublab = new DataView();
                                        Hashtable hatholiday = new Hashtable();
                                        DataSet dsperiod = da.select_method(SqlFinal, hat, "Text");
                                        if (dsperiod.Tables.Count > 0 && dsperiod.Tables[0].Rows.Count > 0)
                                        {
                                            int setcol = 1;
                                            for (int row_inc = 0; row_inc <= days; row_inc++)
                                            {
                                                Hashtable hatgetsubdetails = new Hashtable();
                                                DateTime cur_day = new DateTime();
                                                for (int pre = 0; pre < dsperiod.Tables[0].Rows.Count; pre++)
                                                {
                                                    cur_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);
                                                    string getdate = string.Empty;
                                                    if (Convert.ToString(tmp_camprevar.Trim()) != Convert.ToString(cur_camprevar.Trim()))
                                                    {
                                                        string strsction = string.Empty;
                                                        if ((Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "") && (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1"))
                                                        {
                                                            strsction = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                        }
                                                        DataView dtcurlab = new DataView();
                                                        if (dsall.Tables.Count > 4 && dsall.Tables[4].Rows.Count > 0)
                                                        {
                                                            dsall.Tables[4].DefaultView.RowFilter = " degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"] + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                            dtcurlab = dsall.Tables[4].DefaultView;
                                                        }
                                                        Hashtable hatcurlab = new Hashtable();
                                                        for (int cula = 0; cula < dtcurlab.Count; cula++)
                                                        {
                                                            string lasubno = dtcurlab[cula]["subject_no"].ToString();
                                                            string labhour = dtcurlab[cula]["lab"].ToString();
                                                            if (labhour.Trim() == "1" || labhour.Trim().ToLower() == "true")
                                                            {
                                                                if (!hatcurlab.Contains(lasubno))
                                                                {
                                                                    hatcurlab.Add(lasubno, lasubno);
                                                                }
                                                            }
                                                        }
                                                        DataView dvsubstucount = new DataView();
                                                        hatholiday.Clear();
                                                        DataView duholiday = new DataView();
                                                        if (dsall.Tables.Count > 2 && dsall.Tables[2].Rows.Count > 0)
                                                        {
                                                            dsall.Tables[2].DefaultView.RowFilter = " degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " ";
                                                            duholiday = dsall.Tables[2].DefaultView;
                                                        }
                                                        for (int i = 0; i < duholiday.Count; i++)
                                                        {
                                                            if (!hatholiday.Contains(duholiday[i]["holiday_date"].ToString()))
                                                            {
                                                                hatholiday.Add(duholiday[i]["holiday_date"].ToString(), duholiday[i]["holiday_desc"].ToString());
                                                            }
                                                        }
                                                        int frshlf = 0, schlf = 0;
                                                        DataView dvperiod = new DataView();
                                                        if (dsall.Tables.Count > 6 && dsall.Tables[6].Rows.Count > 0)
                                                        {
                                                            dsall.Tables[6].DefaultView.RowFilter = " degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and  semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                            dvperiod = dsall.Tables[6].DefaultView;
                                                        }
                                                        if (dvperiod.Count > 0)
                                                        {
                                                            string morhr = dvperiod[0]["mor"].ToString();
                                                            string evehr = dvperiod[0]["mor"].ToString();
                                                            if (morhr != null && morhr.Trim() != "")
                                                            {
                                                                frshlf = Convert.ToInt32(morhr);
                                                            }
                                                            if (evehr != null && evehr.Trim() != "")
                                                            {
                                                                schlf = Convert.ToInt32(evehr);
                                                            }
                                                        }
                                                        string getcurrent_sem = string.Empty;
                                                        DataView dvcurrsem = new DataView();
                                                        if (dsall.Tables.Count > 5 && dsall.Tables[5].Rows.Count > 0)
                                                        {
                                                            dsall.Tables[5].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'";
                                                            dvcurrsem = dsall.Tables[5].DefaultView;
                                                        }
                                                        if (dvcurrsem.Count > 0)
                                                        {
                                                            getcurrent_sem = dvcurrsem[0]["current_semester"].ToString();
                                                        }
                                                        if (Convert.ToString(getcurrent_sem) == Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]))
                                                        {
                                                            string semenddate = dsperiod.Tables[0].Rows[pre]["end_date"].ToString();
                                                            string altersetion = string.Empty;
                                                            if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null && dsperiod.Tables[0].Rows[pre]["sections"].ToString().Trim() != "")
                                                            {
                                                                altersetion = "and Sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                                            }
                                                            if (hatdegreename.Contains(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString()))
                                                            {
                                                                degreename = GetCorrespondingKey(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), hatdegreename).ToString();
                                                            }
                                                            cur_day = dt2.AddDays(-row_inc);
                                                            tmp_datevalue = Convert.ToString(cur_day);
                                                            degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                            string SchOrder = string.Empty;
                                                            string day_from = cur_day.ToString("yyyy-MM-dd");
                                                            DateTime schfromdate = cur_day;
                                                            if (dsall.Tables.Count > 1 && dsall.Tables[1].Rows.Count > 0)
                                                            {
                                                                dsall.Tables[1].DefaultView.RowFilter = "batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate<='" + cur_day.ToString() + "'";
                                                                dvsemster = dsall.Tables[1].DefaultView;
                                                            }
                                                            if (dvsemster.Count > 0)
                                                            {
                                                                getdate = dvsemster[0]["FromDate"].ToString();
                                                            }
                                                            else
                                                            {
                                                                getdate = string.Empty;
                                                            }
                                                            if (Convert.ToString(getdate) != "" && Convert.ToString(getdate).Trim() != "0" && Convert.ToString(getdate).Trim() != null)
                                                            {
                                                                DateTime getsche = Convert.ToDateTime(getdate);
                                                                if (Convert.ToDateTime(schfromdate) == Convert.ToDateTime(getsche) || Convert.ToDateTime(schfromdate) != Convert.ToDateTime(getsche))
                                                                {
                                                                    if (ht_sch.Contains(Convert.ToString(degree_var)))
                                                                    {
                                                                        string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sch));
                                                                        string[] sp_rd_semi = contvar.Split(',');
                                                                        if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                                        {
                                                                            SchOrder = sp_rd_semi[0].ToString();
                                                                            noofdays = sp_rd_semi[1].ToString();
                                                                        }
                                                                    }
                                                                    degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                                    if (ht_sdate.Contains(Convert.ToString(degree_var)))
                                                                    {
                                                                        string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sdate));
                                                                        string[] sp_rd_semi = contvar.Split(',');
                                                                        if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                                        {
                                                                            start_datesem = sp_rd_semi[0].ToString();
                                                                            start_dayorder = sp_rd_semi[1].ToString();
                                                                        }
                                                                    }
                                                                    if (noofdays.ToString().Trim() == "")
                                                                    {
                                                                        goto lb1;
                                                                    }
                                                                    string Day_Order = string.Empty;
                                                                    if (SchOrder == "1")
                                                                    {
                                                                        strday = cur_day.ToString("ddd");
                                                                        Day_Order = "0-" + Convert.ToString(strday);
                                                                    }
                                                                    else
                                                                    {
                                                                        string[] sps = dt2.ToString().Split('/');
                                                                        string curdate = sps[0] + '/' + sps[1] + '/' + sps[2];
                                                                        strday = da.findday(cur_day.ToString(), dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), dsperiod.Tables[0].Rows[pre]["semester"].ToString(), dsperiod.Tables[0].Rows[pre]["batch_year"].ToString(), start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                                                                        Day_Order = "0-" + Convert.ToString(strday);
                                                                    }
                                                                    if (strday.ToString().Trim() == "")
                                                                    {
                                                                        goto lb1;
                                                                    }
                                                                    if (!hatholiday.Contains(cur_day.ToString()) || "sun" != strday.ToLower())
                                                                    {
                                                                        string str_day = strday;
                                                                        string Atmonth = cur_day.Month.ToString();
                                                                        string Atyear = cur_day.Year.ToString();
                                                                        long strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                                                        string day_aten = cur_day.Day.ToString();
                                                                        bool check_hour = false;
                                                                        string strsectionvar = string.Empty;
                                                                        string labsection = string.Empty;
                                                                        if (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "" && Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1")
                                                                        {
                                                                            strsectionvar = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                            labsection = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                        }
                                                                        if (dsall.Tables.Count > 0 && dsall.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            dsall.Tables[0].DefaultView.RowFilter = "degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and fromdate='" + day_from + "'";
                                                                            dvalternaet = dsall.Tables[0].DefaultView;
                                                                        }
                                                                        string subjectname = string.Empty;
                                                                        int temp = 0;
                                                                        string getcolumnfield = string.Empty;
                                                                        bool moringleav = false;
                                                                        bool evenleave = false;
                                                                        if (dsall.Tables.Count > 2 && dsall.Tables[2].Rows.Count > 0)
                                                                        {
                                                                            dsall.Tables[2].DefaultView.RowFilter = "holiday_date='" + cur_day.ToString("MM/dd/yyyy") + "' and degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                                            dvholiday = dsall.Tables[2].DefaultView;
                                                                        }
                                                                        if (dvholiday.Count > 0)
                                                                        {
                                                                            if (!hatholiday.Contains(cur_day.ToString()))
                                                                            {
                                                                                hatholiday.Add(cur_day.ToString(), dvholiday[0]["holiday_desc"].ToString());
                                                                            }
                                                                            if (dvholiday[0]["morning"].ToString() == "1" || dvholiday[0]["morning"].ToString().Trim().ToLower() == "true")
                                                                            {
                                                                                moringleav = true;
                                                                            }
                                                                            if (dvholiday[0]["evening"].ToString() == "1" || dvholiday[0]["evening"].ToString().Trim().ToLower() == "true")
                                                                            {
                                                                                evenleave = true;
                                                                            }
                                                                            if (dvholiday[0]["halforfull"].ToString() == "0" || dvholiday[0]["halforfull"].ToString().Trim().ToLower() == "false")
                                                                            {
                                                                                evenleave = true;
                                                                                moringleav = true;
                                                                            }
                                                                        }
                                                                        for (temp = 1; temp <= noofhrs; temp++)
                                                                        {
                                                                            string sp_rd = string.Empty;
                                                                            bool altfalg = false;
                                                                            if (dvalternaet.Count > 0)
                                                                            {
                                                                                sp_rd = dvalternaet[0]["" + strday.Trim() + temp + ""].ToString();
                                                                                if (hatdegreename.Contains(dvalternaet[0]["degree_code"].ToString()))
                                                                                {
                                                                                    degreename = GetCorrespondingKey(dvalternaet[0]["degree_code"].ToString(), hatdegreename).ToString();
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                sp_rd = string.Empty;
                                                                            }
                                                                            if (sp_rd.Trim() != "" && sp_rd.Trim() != "0" && sp_rd != null)
                                                                            {
                                                                                altfalg = true;
                                                                                string[] sp_rd_split = sp_rd.Split(';');
                                                                                for (int index = 0; index <= sp_rd_split.GetUpperBound(0); index++)
                                                                                {
                                                                                    string[] sp2 = sp_rd_split[index].Split(new Char[] { '-' });
                                                                                    if (sp2.GetUpperBound(0) >= 1)
                                                                                    {
                                                                                        int upperbound = sp2.GetUpperBound(0);
                                                                                        for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                                        {
                                                                                            if (sp2[multi_staff] == staff_code)
                                                                                            {
                                                                                                bool checklabhr = false;
                                                                                                for (int sr = 0; sr <= sp_rd_split.GetUpperBound(0); sr++)
                                                                                                {
                                                                                                    string[] getlasub = sp_rd_split[sr].ToString().Split('-');
                                                                                                    if (getlasub.GetUpperBound(0) > 1)
                                                                                                    {
                                                                                                        string srllab = getlasub[0].ToString();
                                                                                                        if (hatcurlab.Contains(srllab))
                                                                                                        {
                                                                                                            checklabhr = true;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                string sect = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                if (sect != "-1" && sect != null && sect.Trim() != "")
                                                                                                {
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    sect = "Sec : " + sect;
                                                                                                }
                                                                                                if (semenddate.Trim() != "" && semenddate.Trim() != null && semenddate.Trim() != "0")
                                                                                                {
                                                                                                    if (cur_day <= (Convert.ToDateTime(semenddate)))
                                                                                                    {
                                                                                                        check_hour = true;
                                                                                                        double Num;
                                                                                                        bool isNum = double.TryParse(sp2[0].ToString(), out Num);
                                                                                                        if (isNum)
                                                                                                        {
                                                                                                            bool allowleave = false;
                                                                                                            if (hatholiday.Contains(cur_day.ToString()))
                                                                                                            {
                                                                                                                if (moringleav == true)
                                                                                                                {
                                                                                                                    if (frshlf >= temp)
                                                                                                                    {
                                                                                                                        allowleave = true;
                                                                                                                    }
                                                                                                                }
                                                                                                                if (evenleave == true)
                                                                                                                {
                                                                                                                    if (temp > frshlf)
                                                                                                                    {
                                                                                                                        allowleave = true;
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                            if (allowleave == false)
                                                                                                            {
                                                                                                                dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                                dvsubject = dsall.Tables[4].DefaultView;
                                                                                                                if (dvsubject.Count > 0)
                                                                                                                {
                                                                                                                    subjectname = dvsubject[0]["subject_name"].ToString();
                                                                                                                }
                                                                                                                string getsubdetails = dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " - " + degreename + " - Sem : " + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + ' ' + sect + " - Subject : " + subjectname;
                                                                                                                string daystring = dt2.AddDays(-row_inc).ToString("dd");
                                                                                                                string daystring1 = dt2.AddDays(-row_inc).ToString("ddd");
                                                                                                                string Att_dcolumn = "d" + Convert.ToInt16(daystring) + "d" + temp;
                                                                                                                check_lab = string.Empty;
                                                                                                                hatvalue.Clear();
                                                                                                                if (checklabhr == false)
                                                                                                                {
                                                                                                                    check_lab = "0";
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    check_lab = "1";
                                                                                                                }
                                                                                                                sectionvar = string.Empty;
                                                                                                                sectionsvalue = string.Empty;
                                                                                                                if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null)
                                                                                                                {
                                                                                                                    sectionvar = " and sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                                                                                                    sectionsvalue = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                                }
                                                                                                                if (check_lab == "1" || check_lab.Trim().ToLower() == "true")
                                                                                                                {
                                                                                                                    hatvalue.Clear();
                                                                                                                    hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                    hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                    hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                    hatvalue.Add("sections", sectionsvalue);
                                                                                                                    hatvalue.Add("month_year", strdate);
                                                                                                                    hatvalue.Add("date", cur_day);
                                                                                                                    hatvalue.Add("subject_no", sp2[0]);
                                                                                                                    hatvalue.Add("day", strday);
                                                                                                                    hatvalue.Add("hour", temp);
                                                                                                                    dsstuatt.Reset();
                                                                                                                    dsstuatt.Dispose();
                                                                                                                    dsstuatt = da.select_method("sp_stu_atten_month_check_lab_alter", hatvalue, "sp");
                                                                                                                    if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                    {
                                                                                                                        Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                                                                                                        if (int.Parse(Att_strqueryst) > 0)
                                                                                                                        {
                                                                                                                            string strgetatt = "select count(r.Roll_No) as stucount from registration r,attendance a,subjectchooser_new s where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and  exam_flag<>'debar' and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";
                                                                                                                            strgetatt = strgetatt + " r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + sectionvar + " and(" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and fromdate='" + cur_day + "' and batch    in(select stu_batch from laballoc_new where subject_no='" + sp2[0].ToString() + "'  and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'  and hour_value='" + temp + "'  and    degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' ";
                                                                                                                            strgetatt = strgetatt + " and day_value='" + strday + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + sectionvar + " and fdate='" + cur_day + "') and adm_date<='" + cur_day + "'";
                                                                                                                            dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                                            if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                            {
                                                                                                                                if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                                {
                                                                                                                                    Att_strqueryst = "0";
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    Att_strqueryst = "1";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                Att_strqueryst = "1";
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            Att_strqueryst = "1";
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        Att_strqueryst = "1";
                                                                                                                    }
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    if (dssubstucount.Tables.Count > 0 && dssubstucount.Tables[0].Rows.Count > 0)
                                                                                                                    {
                                                                                                                        dssubstucount.Tables[0].DefaultView.RowFilter = "batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and  degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and subject_no='" + sp2[0] + "' " + strsction + " and adm_date<='" + cur_day.ToString("MM/dd/yyyy").ToString() + "' ";
                                                                                                                        dvsubstucount = dssubstucount.Tables[0].DefaultView;
                                                                                                                    }
                                                                                                                    if (dvsubstucount.Count > 0)
                                                                                                                    {
                                                                                                                        int stustradm = 0;
                                                                                                                        for (int stuadmcou = 0; stuadmcou < dvsubstucount.Count; stuadmcou++)
                                                                                                                        {
                                                                                                                            stustradm = stustradm + Convert.ToInt32(dvsubstucount[stuadmcou]["stucount"]);
                                                                                                                        }
                                                                                                                        Att_strqueryst = stustradm.ToString();
                                                                                                                        isChoiceBased = isChoiceBasedSystem(Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]));
                                                                                                                        string qryStudStaff = string.Empty;
                                                                                                                        if (isChoiceBased)
                                                                                                                            qryStudStaff = " and s.staffCode like '%" + staff_code + "%'";
                                                                                                                        if (int.Parse(Att_strqueryst) > 0)
                                                                                                                        {
                                                                                                                            string strgetatt = "select count(registration.roll_no) as stucount  from registration,attendance,subjectchooser s where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                                                                                                            strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + sp2[0] + "' " + sectionvar + "";
                                                                                                                            strgetatt = strgetatt + " and (" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and adm_date<='" + cur_day + "' " + qryStudStaff;
                                                                                                                            dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                                            if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                            {
                                                                                                                                if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                                {
                                                                                                                                    Att_strqueryst = "0";
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    Att_strqueryst = "1";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                Att_strqueryst = "1";
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            Att_strqueryst = "1";
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        Att_strqueryst = "1";
                                                                                                                    }
                                                                                                                }
                                                                                                                if (int.Parse(Att_strqueryst) > 0)
                                                                                                                {
                                                                                                                    if (hatgetsubdetails.Contains(getsubdetails))
                                                                                                                    {
                                                                                                                        string gethour = hatgetsubdetails[getsubdetails].ToString();
                                                                                                                        gethour = gethour + ',' + temp;
                                                                                                                        hatgetsubdetails[getsubdetails] = gethour;
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        hatgetsubdetails.Add(getsubdetails, temp);
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            if (altfalg == false)
                                                                            {
                                                                                getcolumnfield = Convert.ToString(strday + temp);
                                                                                if (dvsemster.Count > 0)
                                                                                {
                                                                                    if (dvsemster[0][getcolumnfield].ToString() != "" && dvsemster[0][getcolumnfield].ToString() != null && dvsemster[0][getcolumnfield].ToString() != "\0")
                                                                                    {
                                                                                        string timetable = string.Empty;
                                                                                        string name = dvsemster[0]["ttname"].ToString();
                                                                                        if (name != null && name.Trim() != "")
                                                                                        {
                                                                                            timetable = name;
                                                                                        }
                                                                                        sp_rd = dvsemster[0][getcolumnfield].ToString();
                                                                                        string[] sp_rd_semi = sp_rd.Split(';');
                                                                                        for (int semi = 0; semi <= sp_rd_semi.GetUpperBound(0); semi++)
                                                                                        {
                                                                                            string[] sp2 = sp_rd_semi[semi].Split(new Char[] { '-' });
                                                                                            if (sp2.GetUpperBound(0) >= 1)
                                                                                            {
                                                                                                int upperbound = sp2.GetUpperBound(0);
                                                                                                for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                                                {
                                                                                                    if (sp2[multi_staff] == staff_code)
                                                                                                    {
                                                                                                        bool checklabhr = false;
                                                                                                        for (int sr = 0; sr <= sp_rd_semi.GetUpperBound(0); sr++)
                                                                                                        {
                                                                                                            string[] getlasub = sp_rd_semi[sr].ToString().Split('-');
                                                                                                            if (getlasub.GetUpperBound(0) > 1)
                                                                                                            {
                                                                                                                string srllab = getlasub[0].ToString();
                                                                                                                if (hatcurlab.Contains(srllab))
                                                                                                                {
                                                                                                                    checklabhr = true;
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        string sect = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                        if (sect == "-1" || sect == null || sect.Trim() == "")
                                                                                                        {
                                                                                                            sect = string.Empty;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            sect = "Sec : " + sect;
                                                                                                        }
                                                                                                        if (semenddate.Trim() != "" && semenddate.Trim() != null && semenddate.Trim() != "0")
                                                                                                        {
                                                                                                            if (cur_day <= (Convert.ToDateTime(semenddate)))
                                                                                                            {
                                                                                                                check_hour = true;
                                                                                                                double Num;
                                                                                                                bool isNum = double.TryParse(sp2[0].ToString(), out Num);
                                                                                                                if (isNum)
                                                                                                                {
                                                                                                                    bool allowleave = false;
                                                                                                                    if (hatholiday.Contains(cur_day.ToString()))
                                                                                                                    {
                                                                                                                        if (moringleav == true)
                                                                                                                        {
                                                                                                                            if (frshlf >= temp)
                                                                                                                            {
                                                                                                                                allowleave = true;
                                                                                                                            }
                                                                                                                        }
                                                                                                                        if (evenleave == true)
                                                                                                                        {
                                                                                                                            if (temp > frshlf)
                                                                                                                            {
                                                                                                                                allowleave = true;
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                    if (allowleave == false)
                                                                                                                    {
                                                                                                                        if (dsall.Tables.Count > 4 && dsall.Tables[4].Rows.Count > 0)
                                                                                                                        {
                                                                                                                            dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                                            dvsubject = dsall.Tables[4].DefaultView;
                                                                                                                        }
                                                                                                                        if (dvsubject.Count > 0)
                                                                                                                        {
                                                                                                                            subjectname = dvsubject[0]["subject_name"].ToString();
                                                                                                                        }
                                                                                                                        string getsubdetails = dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " - " + degreename + " - Sem : " + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + ' ' + sect + " - Subject : " + subjectname;
                                                                                                                        string daystring = dt2.AddDays(-row_inc).ToString("dd");
                                                                                                                        string daystring1 = dt2.AddDays(-row_inc).ToString("ddd");
                                                                                                                        string Att_dcolumn = "d" + Convert.ToInt16(daystring) + "d" + temp;
                                                                                                                        check_lab = string.Empty;
                                                                                                                        hatvalue.Clear();
                                                                                                                        if (checklabhr == false)
                                                                                                                        {
                                                                                                                            check_lab = "0";
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            check_lab = "1";
                                                                                                                        }
                                                                                                                        sectionvar = string.Empty;
                                                                                                                        sectionsvalue = string.Empty;
                                                                                                                        if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null)
                                                                                                                        {
                                                                                                                            sectionvar = " and sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                                                                                                            sectionsvalue = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                                        }
                                                                                                                        if (check_lab == "1" || check_lab.Trim().ToLower() == "true")
                                                                                                                        {
                                                                                                                            hatvalue.Clear();
                                                                                                                            string strstt = "select count(r.Roll_No) as stucount from registration r,subjectchooser s where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and ";
                                                                                                                            strstt = strstt + " current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no ";
                                                                                                                            strstt = strstt + " and r.current_semester=s.semester and subject_no='" + sp2[0] + "' " + sectionvar + " and batch in(select stu_batch from ";
                                                                                                                            strstt = strstt + " laballoc where subject_no='" + sp2[0] + "'  and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'  and hour_value='" + temp + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' ";
                                                                                                                            strstt = strstt + " and day_value='" + strday + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + sectionvar + " and Timetablename='" + timetable + "') and adm_date<='" + cur_day + "'"; dsstuatt.Dispose();
                                                                                                                            dsstuatt.Reset();
                                                                                                                            dsstuatt = da.select_method_wo_parameter(strstt, "text");
                                                                                                                            if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                            {
                                                                                                                                Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                Att_strqueryst = "1";
                                                                                                                            }
                                                                                                                            if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                            {
                                                                                                                                Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                                                                                                                if (int.Parse(Att_strqueryst) > 0)
                                                                                                                                {
                                                                                                                                    hatvalue.Clear();
                                                                                                                                    hatvalue.Add("columnname", Att_dcolumn);
                                                                                                                                    hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                                    hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                                    hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                                    hatvalue.Add("sections", sectionsvalue);
                                                                                                                                    hatvalue.Add("month_year", strdate);
                                                                                                                                    hatvalue.Add("date", cur_day);
                                                                                                                                    hatvalue.Add("subject_no", sp2[0]);
                                                                                                                                    hatvalue.Add("day", strday);
                                                                                                                                    hatvalue.Add("hour", temp);
                                                                                                                                    hatvalue.Add("ttmane", timetable);
                                                                                                                                    dsstuatt.Reset();
                                                                                                                                    dsstuatt.Dispose();
                                                                                                                                    string strgetatt = "select count(r.Roll_No) as stucount from registration r,attendance a,subjectchooser s where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year=" + strdate + "";
                                                                                                                                    strgetatt = strgetatt + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + sectionvar + " and(" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and batch in(select stu_batch from laballoc ";
                                                                                                                                    strgetatt = strgetatt + " where subject_no='" + sp2[0].ToString() + "' and Timetablename='" + timetable + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'  and hour_value='" + temp + "'  and    degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and day_value='" + strday + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + sectionvar + ") and adm_date<='" + cur_day + "'";
                                                                                                                                    dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                                                    if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                                    {
                                                                                                                                        if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                                        {
                                                                                                                                            Att_strqueryst = "0";
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            Att_strqueryst = "1";
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        Att_strqueryst = "1";
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    Att_strqueryst = "1";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                Att_strqueryst = "1";
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            hatvalue.Clear();
                                                                                                                            hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                            hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                            hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                            hatvalue.Add("sections", sectionsvalue);
                                                                                                                            hatvalue.Add("month_year", strdate);
                                                                                                                            hatvalue.Add("date", cur_day);
                                                                                                                            hatvalue.Add("subject_no", sp2[0]);
                                                                                                                            dsstuatt.Reset();
                                                                                                                            dsstuatt.Dispose();
                                                                                                                            if (dssubstucount.Tables.Count > 0 && dssubstucount.Tables[0].Rows.Count > 0)
                                                                                                                            {
                                                                                                                                dssubstucount.Tables[0].DefaultView.RowFilter = "batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and  degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and subject_no='" + sp2[0] + "' " + strsction + " and adm_date<='" + cur_day.ToString("MM/dd/yyyy").ToString() + "' ";
                                                                                                                                dvsubstucount = dssubstucount.Tables[0].DefaultView;
                                                                                                                            }
                                                                                                                            if (dvsubstucount.Count > 0)
                                                                                                                            {
                                                                                                                                int stustradm = 0;
                                                                                                                                for (int stuadmcou = 0; stuadmcou < dvsubstucount.Count; stuadmcou++)
                                                                                                                                {
                                                                                                                                    stustradm = stustradm + Convert.ToInt32(dvsubstucount[stuadmcou]["stucount"]);
                                                                                                                                }
                                                                                                                                Att_strqueryst = stustradm.ToString();
                                                                                                                                if (int.Parse(Att_strqueryst) > 0)
                                                                                                                                {
                                                                                                                                    hatvalue.Clear();
                                                                                                                                    hatvalue.Add("columnname ", Att_dcolumn);
                                                                                                                                    hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                                    hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                                    hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                                    hatvalue.Add("sections", sectionsvalue);
                                                                                                                                    hatvalue.Add("month_year", strdate);
                                                                                                                                    hatvalue.Add("date", cur_day);
                                                                                                                                    hatvalue.Add("subject_no", sp2[0]);
                                                                                                                                    dsstuatt.Reset();
                                                                                                                                    dsstuatt.Dispose();
                                                                                                                                    string strgetatt = "select count( registration.roll_no) as stucount  from registration,attendance,subjectchooser s where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                                                                                                                    strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + sp2[0] + "' " + sectionvar + "";
                                                                                                                                    strgetatt = strgetatt + " and (" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and adm_date<='" + cur_day + "'";
                                                                                                                                    dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                                                    if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                                    {
                                                                                                                                        if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                                        {
                                                                                                                                            Att_strqueryst = "0";
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            Att_strqueryst = "1";
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        Att_strqueryst = "1";
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    Att_strqueryst = "1";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                Att_strqueryst = "1";
                                                                                                                            }
                                                                                                                        }
                                                                                                                        if (int.Parse(Att_strqueryst) > 0)
                                                                                                                        {
                                                                                                                            if (hatgetsubdetails.Contains(getsubdetails))
                                                                                                                            {
                                                                                                                                string gethour = hatgetsubdetails[getsubdetails].ToString();
                                                                                                                                gethour = gethour + ',' + temp;
                                                                                                                                hatgetsubdetails[getsubdetails] = gethour;
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                hatgetsubdetails.Add(getsubdetails, temp);
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                lb1: tmp_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);
                                                }
                                                int noofcam = 0;
                                                string camlocalsub = string.Empty;
                                                DataView dvgettest = new DataView();
                                                if (dsall.Tables.Count > 7 && dsall.Tables[7].Rows.Count > 0)
                                                {
                                                    dsall.Tables[7].DefaultView.RowFilter = " staff_code='" + staff_code + "' and LastDate='" + cur_day.ToString() + "'";
                                                    dvgettest = dsall.Tables[7].DefaultView;
                                                }
                                                for (int cl = 0; cl < dvgettest.Count; cl++)
                                                {
                                                    string testname = dvgettest[cl]["criteria"].ToString();
                                                    string subjectname = dvgettest[cl]["subject_name"].ToString();
                                                    string subjectno = dvgettest[cl]["subject_no"].ToString();
                                                    string criteriano = dvgettest[cl]["Criteria_no"].ToString();
                                                    string examcode = dvgettest[cl]["exam_code"].ToString();
                                                    string batch = dvgettest[cl]["Batch_Year"].ToString();
                                                    string degree = dvgettest[cl]["degree_code"].ToString();
                                                    string sem = dvgettest[cl]["semester"].ToString();
                                                    string sectionsc = dvgettest[cl]["sections"].ToString();
                                                    string secval = string.Empty;
                                                    if (sectionsc.Trim() != "" && sectionsc != "-1" && sectionsc != null)
                                                    {
                                                        secval = " and sections='" + sectionsc + "'";
                                                    }
                                                    DataView dvcamstu = new DataView();
                                                    if (dsall.Tables.Count > 8 && dsall.Tables[8].Rows.Count > 0)
                                                    {
                                                        dsall.Tables[8].DefaultView.RowFilter = "Criteria_no ='" + criteriano + "' and exam_code='" + examcode + "' " + secval + " and subject_no='" + subjectno + "'";
                                                        dvcamstu = dsall.Tables[8].DefaultView;
                                                    }
                                                    if (dvcamstu.Count == 0)
                                                    {
                                                        if (rdiobtndetailornot.Text == "Count")
                                                        {
                                                            noofcam++;
                                                            camlocalsub = noofcam.ToString();
                                                        }
                                                        else
                                                        {
                                                            if (dvalternaet.Count > 0)
                                                            {
                                                                if (hatdegreename.Contains(dvalternaet[0]["degree_code"].ToString()))
                                                                {
                                                                    degreename = GetCorrespondingKey(dvalternaet[0]["degree_code"].ToString(), hatdegreename).ToString();
                                                                }
                                                            }
                                                            string strgetcamdetails = batch + " - " + degreename + " - Sem " + sem;
                                                            if (sectionsc.Trim() != "" && sectionsc.Trim() != "-1" && sectionsc != "0")
                                                            {
                                                                strgetcamdetails = strgetcamdetails + " - Sec : " + sectionsc;
                                                            }
                                                            if (dsall.Tables.Count > 4 && dsall.Tables[4].Rows.Count > 0)
                                                            {
                                                                dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + subjectno + "";
                                                                dvsubject = dsall.Tables[4].DefaultView;
                                                            }
                                                            if (dvsubject.Count > 0)
                                                            {
                                                                strgetcamdetails = camlocalsub + " - Test : " + testname + " - Subject :" + dvsubject[0]["subject_name"].ToString();
                                                            }
                                                            if (camlocalsub == "")
                                                            {
                                                                camlocalsub = strgetcamdetails;
                                                            }
                                                            else
                                                            {
                                                                camlocalsub = camlocalsub + " // " + strgetcamdetails;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (hatgetsubdetails.Count > 0 || camlocalsub.Trim() != "")
                                                {
                                                    if (attendanceentryflag == false)
                                                    {
                                                        FpSpread1.Visible = true;
                                                        sno++;
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dsstaffname.Tables[0].Rows[rowi]["dept_name"].ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dsstaffname.Tables[0].Rows[rowi]["desig_name"].ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dsstaffname.Tables[0].Rows[rowi]["category_name"].ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = 12;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dsstaffname.Tables[0].Rows[rowi]["stftype"].ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = 12;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = dsstaffname.Tables[0].Rows[rowi]["staff_code"].ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = 12;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = dsstaffname.Tables[0].Rows[rowi]["staff_name"].ToString();
                                                        attendanceentryflag = true;
                                                    }
                                                    string getval = string.Empty;
                                                    setcol = Convert.ToInt32(hatdate[cur_day.ToString("MM/dd/yyyy")]);
                                                    int noofhatt = 0;
                                                    foreach (DictionaryEntry entry in hatgetsubdetails)
                                                    {
                                                        if (rdiobtndetailornot.Text == "Count")
                                                        {
                                                            string[] spva = entry.Value.ToString().Split(',');
                                                            int setva = Convert.ToInt32(spva.GetUpperBound(0));
                                                            noofhatt = noofhatt + setva + 1;
                                                            getval = noofhatt.ToString();
                                                        }
                                                        else
                                                        {
                                                            if (getval == "")
                                                            {
                                                                getval = entry.Key.ToString() + " - Hr :" + entry.Value.ToString();
                                                            }
                                                            else
                                                            {
                                                                getval = getval + " // " + entry.Key.ToString() + " - Hr :" + entry.Value.ToString();
                                                            }
                                                        }
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setcol].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setcol].Font.Size = 12;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setcol].Text = getval.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setcol + 1].Text = camlocalsub.ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (FpSpread1.Sheets[0].RowCount == 0)
                            {
                                lblrptname.Visible = false;
                                txtexcelname.Visible = false;
                                btnxl.Visible = false;
                                Printcontrol.Visible = false;
                                btnprintmaster.Visible = false;
                                FpSpread1.Visible = false;
                                lblerror.Visible = true;
                                lblerror.Text = "Staff Completed Attendance";
                            }
                        }
                        else
                        {
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            Printcontrol.Visible = false;
                            btnprintmaster.Visible = false;
                            FpSpread1.Visible = false;
                            lblerror.Visible = true;
                            lblerror.Text = "Please Select The Staff and Then Proceed";
                        }
                    }
                    else
                    {
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = false;
                        FpSpread1.Visible = false;
                        lblerror.Visible = true;
                        lblerror.Text = "To Date must be Equal or Greater Than From Date";
                    }
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        int stff = 0;
                        for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            if (FpSpread1.Sheets[0].Rows[i].Visible == true)
                            {
                                stff++;
                            }
                        }
                        if (stff > 0)
                        {
                            lblrptname.Visible = true;
                            txtexcelname.Visible = true;
                            btnxl.Visible = true;
                            Printcontrol.Visible = false;
                            btnprintmaster.Visible = true;
                        }
                        else
                        {
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            Printcontrol.Visible = false;
                            btnprintmaster.Visible = false;
                            FpSpread1.Visible = false;
                            lblerror.Visible = true;
                            lblerror.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "No Records Found";
                    }
                    int rowcount = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Width = 900;
                    FpSpread1.Height = 70 + (rowcount * 22);
                    FpSpread1.Sheets[0].PageSize = 25 + (rowcount * 20);
                    FpSpread1.SaveChanges();
                }
                else
                {
                    DataSet dssec_sem = new DataSet();
                    DataSet dsholyday = new DataSet();
                    DataSet dsnum_of_hrs = new DataSet();
                    DataSet dsbindvalues = new DataSet();
                    DataSet dssub = new DataSet();
                    DataSet ds2 = new DataSet();
                    string bin_semester = string.Empty;
                    string plannersec = string.Empty;
                    string hr = string.Empty;

                    string[] splitfromcheck = tbfdate.Text.Split(new Char[] { '-' });
                    string[] splittocheck = tbtodate.Text.Split(new char[] { '-' });
                    string fdate = splitfromcheck[1] + '-' + splitfromcheck[0] + '-' + splitfromcheck[2];
                    string tdate = splittocheck[1] + '-' + splittocheck[0] + '-' + splittocheck[2];
                    DateTime fromdatechech = Convert.ToDateTime(fdate);
                    DateTime todatecheck = Convert.ToDateTime(tdate);
                    if (fromdatechech > todatecheck)
                    {
                        FpSpread1.Visible = false;
                        lblerror.Text = "Please Enter To Date Grater Than From Date";
                        lblerror.Visible = true;
                    }
                    else
                    {
                        TimeSpan t = todatecheck.Subtract(fromdatechech);
                        Double days = t.Days;
                        FpSpread1.Width = 750;
                        FpSpread1.Visible = true;
                        FpSpread1.Sheets[0].ColumnCount = 5;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].ColumnHeader.Visible = true;
                        FpSpread1.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.AliceBlue;
                        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Height = 50;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hour";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Staff Name";
                        FpSpread1.Sheets[0].Columns[0].Locked = true;
                        FpSpread1.Sheets[0].Columns[1].Locked = true;
                        FpSpread1.Sheets[0].Columns[2].Locked = true;
                        FpSpread1.Sheets[0].Columns[3].Locked = true;
                        FpSpread1.Sheets[0].Columns[4].Locked = true;
                        FpSpread1.Sheets[0].Columns[0].Width = 50;
                        FpSpread1.Sheets[0].Columns[1].Width = 80;
                        FpSpread1.Sheets[0].Columns[2].Width = 50;
                        FpSpread1.Sheets[0].Columns[3].Width = 350;
                        FpSpread1.Sheets[0].Columns[4].Width = 320;

                        FpSpread1.Sheets[0].Columns[0].Visible = true;
                        FpSpread1.Sheets[0].Columns[1].Visible = true;
                        FpSpread1.Sheets[0].Columns[2].Visible = true;
                        FpSpread1.Sheets[0].Columns[3].Visible = true;
                        FpSpread1.Sheets[0].Columns[4].Visible = true;

                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].RowCount = 0;
                        string sqlbatch = string.Empty;
                        string sqlbranch = string.Empty;
                        string sqlbatchquery = string.Empty;
                        string strsection = string.Empty;

                        if (txtdesi.Text != "--Select--" || chklstbranch.Items.Count != null)
                        {
                            int itemcount = 0;
                            for (itemcount = 0; itemcount < chklstbranch.Items.Count; itemcount++)
                            {
                                if (chklstbranch.Items[itemcount].Selected == true)
                                {
                                    if (sqlbatch == "")
                                        sqlbatch = "'" + chklstbranch.Items[itemcount].Value.ToString() + "'";
                                    else
                                        sqlbatch = sqlbatch + "," + "'" + chklstbranch.Items[itemcount].Value.ToString() + "'";
                                }
                            }
                        }
                        if (txtstaff.Text != "---Select---" || chklststaff.Items.Count != null)
                        {
                            int itemcount = 0;
                            for (itemcount = 0; itemcount < chklststaff.Items.Count; itemcount++)
                            {
                                if (chklststaff.Items[itemcount].Selected == true)
                                {
                                    if (sqlbranch == "")
                                        sqlbranch = "'" + chklststaff.Items[itemcount].Value.ToString() + "'";
                                    else
                                        sqlbranch = sqlbranch + "," + "'" + chklststaff.Items[itemcount].Value.ToString() + "'";
                                }
                            }
                        }
                        DataSet dssection = new DataSet();
                        DataView dvsection = new DataView();
                        if (sqlbatch != "" && sqlbranch != "")
                        {
                            string getsection_sem = "select distinct current_semester from registration where batch_year in(" + sqlbatch + ") and degree_code in(" + sqlbranch + ")  and delflag=0 and exam_flag<>'Debar'";
                            dssec_sem = da.select_method(getsection_sem, hat, "Text");
                            if (dssec_sem.Tables.Count > 0 && dssec_sem.Tables[0].Rows.Count > 0)
                            {
                                for (int cnt = 0; cnt < dssec_sem.Tables[0].Rows.Count; cnt++)
                                {
                                    if (bin_semester == "")
                                    {
                                        bin_semester = dssec_sem.Tables[0].Rows[cnt]["current_semester"].ToString();
                                    }
                                    else
                                    {
                                        bin_semester = bin_semester + ',' + dssec_sem.Tables[0].Rows[cnt]["current_semester"].ToString();
                                    }
                                }
                            }
                            string strsql1 = "select distinct sections,Batch_Year ,degree_code,Current_Semester from registration where batch_year in(" + sqlbatch + ") and degree_code in(" + sqlbranch + ") and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar'";
                            dssection.Clear();
                            dssection = da.select_method_wo_parameter(strsql1, "Text");
                        }
                        string[] from = tbfdate.Text.Split(new char[] { '-' });
                        string fromdate = from[1] + '/' + from[0] + '/' + from[2];
                        string[] to = tbtodate.Text.Split(new char[] { '-' });
                        string todate = to[1] + '/' + to[0] + '/' + to[2];
                        DateTime fromday1 = Convert.ToDateTime(fromdate);
                        DateTime today = Convert.ToDateTime(todate);
                        string classhour = string.Empty;
                        int sno = 0;
                        string bindvals21 = string.Empty;
                        DataView dvsubject = new DataView();
                        DataView dvholiday = new DataView();
                        DataView dvtothour = new DataView();
                        DataView dvsemester = new DataView();
                        DataView dvalter = new DataView();
                        DataView dvstaffname = new DataView();
                        bool isChoiceBased = false;
                        string valSubject = string.Empty;
                        if (cblDegSubject.Items.Count > 0)
                        {
                            valSubject = rs.GetSelectedItemsValueAsString(cblDegSubject);
                        }
                        if (bin_semester != "")
                        {
                            bindvals21 = "select distinct s.degree_code,s.semester,s.batch_year ,s.start_date,s.end_date,de.Dept_Name,c.Course_Name from seminfo s,Registration r,Degree d,Department de,course c where  r.Batch_Year=s.batch_year and r.Current_Semester=s.semester and r.degree_code=s.degree_code and d.Degree_Code =s.degree_code and c.course_id=d.course_id and r.degree_code =d.Degree_Code and d.Dept_Code =de.Dept_Code and d.college_code =r.college_code and r.CC=0 and r.DelFlag=0 and s.semester in(" + bin_semester + ") and s.batch_year in (" + sqlbatch + ") and s.degree_code in (" + sqlbranch + ")  and r.college_code =" + ddlcollege.SelectedItem.Value + " and c.college_code=r.college_code order by s.batch_year,s.semester, s.degree_code";

                            bindvals21 = bindvals21 + "  select distinct S.subject_no, subject_code,subject_name,sm.degree_code,sm.semester ,sm.Batch_Year,Sem.Lab,st.staff_code,smt.staff_name from subject as S,syllabus_master  as SM,Sub_sem as Sem,staff_selector st,staffmaster smt where s.syll_code=SM.syll_code and  st.subject_no=s.subject_no and  S.subtype_no = Sem.subtype_no and smt.staff_code =st.staff_code  and  SM.batch_year in (" + sqlbatch + ") and SM.degree_code in(" + sqlbranch + ") and Sm.semester in(" + bin_semester + ") and s.subject_no in('" + valSubject + "')  order by S.subject_no";

                            bindvals21 = bindvals21 + "  select * from holidaystudents where degree_code in(" + sqlbranch + ") and semester in(" + bin_semester + ") and holiday_date between '" + fromday1.ToString("MM/dd/yyyy") + "' and '" + today.ToString("MM/dd/yyyy") + "'";
                            bindvals21 = bindvals21 + "  select convert(nvarchar(15),s.start_date,101) as start_date,s.end_date,s.starting_dayorder,p.nodays,p.schorder,s.semester ,s.degree_code ,s.batch_year,No_of_hrs_per_day  from periodattndschedule p,seminfo s where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code in(" + sqlbranch + ") and batch_year in(" + sqlbatch + ") and s.semester in(" + bin_semester + ")";
                            bindvals21 = bindvals21 + "   select * from semester_schedule where batch_year in(" + sqlbatch + ") and degree_code in(" + sqlbranch + ") and  semester in(" + bin_semester + ")  order by fromdate desc";
                            bindvals21 = bindvals21 + "  select  * from Alternate_schedule where batch_year in(" + sqlbatch + ") and degree_code in(" + sqlbranch + ") and  semester in(" + bin_semester + ") and fromdate between '" + fromday1.ToString("MM/dd/yyyy") + "' and '" + today.ToString("MM/dd/yyyy") + "' ";
                            bindvals21 = bindvals21 + "  select staff_name,staff_code from staffmaster";
                            dsbindvalues = da.select_method(bindvals21, hat, "Text");
                            if (dsbindvalues.Tables.Count > 0 && dsbindvalues.Tables[0].Rows.Count > 0)
                            {
                                lblerror.Text = string.Empty;
                                lblerror.Visible = false;
                                string subjectquery = string.Empty;
                                Hashtable holidayDate = new Hashtable();
                                for (int cnt = 0; cnt < dsbindvalues.Tables[0].Rows.Count; cnt++)
                                {
                                    strsection = string.Empty;
                                    DateTime s_date = Convert.ToDateTime(dsbindvalues.Tables[0].Rows[cnt]["start_date"]);
                                    DateTime e_date = Convert.ToDateTime(dsbindvalues.Tables[0].Rows[cnt]["end_date"]);
                                    string dcode = dsbindvalues.Tables[0].Rows[cnt]["degree_code"].ToString();
                                    string sem = dsbindvalues.Tables[0].Rows[cnt]["semester"].ToString();
                                    string batchyear = dsbindvalues.Tables[0].Rows[cnt]["batch_year"].ToString();
                                    //string valSubject=string.Empty;
                                    string coursename = Convert.ToString(dsbindvalues.Tables[0].Rows[cnt]["Course_Name"]).Trim();
                                    string Dept_name = dsbindvalues.Tables[0].Rows[cnt]["Dept_Name"].ToString();

                                    if (dsbindvalues.Tables.Count > 1 && dsbindvalues.Tables[1].Rows.Count > 0)
                                    {
                                        dsbindvalues.Tables[1].DefaultView.RowFilter = "Batch_Year=" + batchyear.ToString() + " and degree_code=" + dcode.ToString() + " and semester=" + sem.ToString() + " and subject_no in('" + valSubject + "')";
                                        dvsubject = dsbindvalues.Tables[1].DefaultView;
                                    }
                                    //}
                                    if (dvsubject.Count > 0)
                                    {
                                        for (int i = 0; i < dvsubject.Count; i++)
                                        {
                                            if (subcode_tot == "")
                                            {
                                                subcode_tot = dvsubject[i]["subject_code"].ToString();
                                            }
                                            else
                                            {
                                                subcode_tot = subcode_tot + "," + dvsubject[i]["subject_code"].ToString();
                                            }
                                        }
                                    }
                                    if (dssection.Tables.Count > 0 && dssection.Tables[0].Rows.Count > 0)
                                    {
                                        dssection.Tables[0].DefaultView.RowFilter = "Batch_Year=" + batchyear.ToString() + " and degree_code=" + dcode.ToString() + " and Current_Semester=" + sem.ToString() + "";
                                        dvsection = dssection.Tables[0].DefaultView;
                                    }
                                    if (dvsection.Count > 0)
                                    {
                                        for (int sec = 0; sec < dvsection.Count; sec++)
                                        {
                                            if (strsection == "")
                                            {
                                                strsection = dvsection[sec]["sections"].ToString();
                                            }
                                            else
                                            {
                                                strsection = strsection + '\\' + dvsection[sec]["sections"].ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strsection = string.Empty;
                                    }
                                    Hashtable hatlab = new Hashtable();
                                    if (dsbindvalues.Tables.Count > 1 && dsbindvalues.Tables[1].Rows.Count > 0)
                                    {
                                        dsbindvalues.Tables[1].DefaultView.RowFilter = "Batch_Year=" + batchyear.ToString() + " and degree_code=" + dcode.ToString() + " and semester=" + sem.ToString() + " and Lab=1 and subject_no in('" + valSubject + "')";
                                        dvsubject = dsbindvalues.Tables[1].DefaultView;
                                    }
                                    for (int l = 0; l < dvsubject.Count; l++)
                                    {
                                        string strsub = dvsubject[l]["subject_no"].ToString();
                                        if (!hatlab.Contains(strsub))
                                        {
                                            hatlab.Add(strsub, strsub);
                                        }
                                    }
                                    int maxhour = 0;
                                    string sectionvalue = string.Empty;

                                    for (DateTime caldate = fromday1; caldate <= today; caldate = caldate.AddDays(1))
                                    {
                                        notimetable = false; //Aruna 03sep2018
                                        if (caldate >= s_date && caldate <= e_date)
                                        {
                                            string find_sem = dsbindvalues.Tables[0].Rows[cnt]["end_date"].ToString();
                                            string[] caldtesplit = Convert.ToString(caldate).Split(' ');
                                            string[] datesplit = Convert.ToString(caldtesplit[0]).Split('/');
                                            string date = datesplit[1] + '/' + datesplit[0] + '/' + datesplit[2];
                                            //Rajkumar 8/1/2018
                                            string curDate = datesplit[0] + '/' + datesplit[1] + '/' + datesplit[2];
                                            DateTime dt;
                                            DateTime.TryParse(curDate, out dt);
                                            //-----------
                                            string querydate = Convert.ToString(caldtesplit[0]);
                                            if (dsbindvalues.Tables.Count > 2 && dsbindvalues.Tables[2].Rows.Count > 0)
                                            {
                                                dsbindvalues.Tables[2].DefaultView.RowFilter = "degree_code=" + dcode.ToString() + " and semester=" + sem.ToString() + " and holiday_date='" + dt + "'";//date rajkumar dt.ToString("dd/MM/yyyy")
                                                dvholiday = dsbindvalues.Tables[2].DefaultView;
                                            }
                                            bool isMor = false;
                                            bool isEve = false;
                                            string mrnHalf = string.Empty;
                                            string eveHalf = string.Empty;
                                            int mor = 0;
                                            int eve = 0;
                                            bool isfull = false;
                                            if (dvholiday.Count > 0)//Rajkumar for halfday leave Condtions
                                            {
                                                string MRN = Convert.ToString(dvholiday[0]["morning"]);
                                                string EVE = Convert.ToString(dvholiday[0]["evening"]);
                                                if (MRN.ToLower().Trim() == "true" || MRN.Trim() == "1")
                                                    isMor = true;
                                                if (EVE.ToLower().Trim() == "true" || EVE.Trim() == "1")
                                                    isEve = true;

                                                mrnHalf = da.GetFunction("select no_of_hrs_I_half_day from PeriodAttndSchedule where degree_code='" + dcode + "' and semester='" + sem + "'");
                                                eveHalf = da.GetFunction("select no_of_hrs_II_half_day from PeriodAttndSchedule where degree_code='" + dcode + "' and semester='" + sem + "'");
                                                int.TryParse(mrnHalf, out mor);
                                                int.TryParse(eveHalf, out eve);
                                                if (isMor && isEve)
                                                    isfull = true;
                                            }

                                            if (!isfull)
                                            {
                                                if (dvholiday.Count == 0 || isEve || isMor)
                                                {
                                                    string noofdays = string.Empty;
                                                    string start_datesem = string.Empty;
                                                    string start_dayorder = string.Empty;
                                                    string end_datesem = string.Empty;
                                                    if (dsbindvalues.Tables.Count > 3 && dsbindvalues.Tables[3].Rows.Count > 0)
                                                    {
                                                        dsbindvalues.Tables[3].DefaultView.RowFilter = "batch_year=" + batchyear.ToString() + " and degree_code=" + dcode.ToString() + " and semester=" + sem.ToString() + "";
                                                        dvtothour = dsbindvalues.Tables[3].DefaultView;
                                                    }
                                                    if (dvtothour.Count > 0)
                                                    {
                                                        maxhour = Convert.ToInt32(dvtothour[0]["No_of_hrs_per_day"]);
                                                        schorder = dvtothour[0]["SchOrder"].ToString();
                                                        noofdays = dvtothour[0]["nodays"].ToString();
                                                        start_datesem = dvtothour[0]["start_date"].ToString();
                                                        end_datesem = dvtothour[0]["end_date"].ToString();
                                                        start_dayorder = dvtothour[0]["starting_dayorder"].ToString();
                                                    }
                                                    string dayget = string.Empty;
                                                    if (schorder == "1")
                                                    {
                                                        dayget = Convert.ToString(caldate.ToString("ddd"));
                                                    }
                                                    else
                                                    {
                                                        string[] startdatspilt = start_datesem.Split(' ');
                                                        start_datesem = startdatspilt[0].ToString();
                                                        dayget = da.findday(querydate.ToString(), dcode, sem, batchyear, start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                                                    }
                                                    classhour = string.Empty;
                                                    for (int i = 0; i < chklstsubject.Items.Count; i++)
                                                    {

                                                        if (chklstsubject.Items[i].Selected == true)
                                                        {
                                                            if (!isMor && !isEve)
                                                            {
                                                                if (classhour == "")
                                                                {
                                                                    classhour = classhour + dayget + chklstsubject.Items[i].Text;
                                                                }
                                                                else
                                                                {
                                                                    classhour = classhour + ',' + dayget + chklstsubject.Items[i].Text;
                                                                }
                                                            }
                                                            else if (!isMor)
                                                            {
                                                                if (Convert.ToInt16(chklstsubject.Items[i].Text) <= mor)
                                                                {
                                                                    if (classhour == "")
                                                                    {
                                                                        classhour = classhour + dayget + chklstsubject.Items[i].Text;
                                                                    }
                                                                    else
                                                                    {
                                                                        classhour = classhour + ',' + dayget + chklstsubject.Items[i].Text;
                                                                    }
                                                                }
                                                            }
                                                            else if (!isEve)
                                                            {
                                                                if ((Convert.ToInt16(chklstsubject.Items[i].Text) <= maxhour) && (Convert.ToInt16(chklstsubject.Items[i].Text) > mor))
                                                                    if (classhour == "")
                                                                    {
                                                                        classhour = classhour + dayget + chklstsubject.Items[i].Text;
                                                                    }
                                                                    else
                                                                    {
                                                                        classhour = classhour + ',' + dayget + chklstsubject.Items[i].Text;
                                                                    }
                                                            }
                                                        }
                                                    }

                                                    //if (!isMor || !isEve)
                                                    //{
                                                    //    FpSpread1.Sheets[0].RowCount++;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Moring Holiday";
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Red;
                                                    //    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                                                    //}


                                                    if (!isMor || !isEve)
                                                    {

                                                        string[] sectionspilt = strsection.Split('\\');
                                                        for (int scet = 0; scet <= sectionspilt.GetUpperBound(0); scet++)
                                                        {
                                                            string chksectionvalue = sectionspilt[scet].ToString();
                                                            bool head = false;
                                                            if (string.IsNullOrEmpty(chksectionvalue.Trim()) || chksectionvalue.Trim() == "")
                                                            {
                                                                sectionvalue = string.Empty;
                                                            }
                                                            else
                                                            {
                                                                sectionvalue = " and Sections='" + chksectionvalue.ToString() + "'";
                                                            }
                                                            if (dsbindvalues.Tables.Count > 4 && dsbindvalues.Tables[4].Rows.Count > 0)
                                                            {
                                                                dsbindvalues.Tables[4].DefaultView.RowFilter = "batch_year=" + batchyear + " and degree_code=" + dcode + " and semester=" + sem + " " + sectionvalue + " and fromdate <= '" + querydate.ToString() + "'";
                                                                dvsemester = dsbindvalues.Tables[4].DefaultView;
                                                            }
                                                            if (dvsemester.Count > 0)
                                                            {
                                                                semflag = true;
                                                                for (int i = 0; i <= 0; i++) //for (int i = 0; i < dvsemester.Count; i++) Aruna 29/07/2017
                                                                {
                                                                    string[] classhourspilt = classhour.Split(new char[] { ',' });
                                                                    for (int colu = 0; colu <= classhourspilt.GetUpperBound(0); colu++)
                                                                    {
                                                                        string columnvalue = classhourspilt[colu].ToString();
                                                                        string classhour1 = string.Empty;
                                                                        if (dsbindvalues.Tables.Count > 5 && dsbindvalues.Tables[5].Rows.Count > 0)
                                                                        {
                                                                            dsbindvalues.Tables[5].DefaultView.RowFilter = "batch_year=" + batchyear + " and degree_code=" + dcode + " and semester=" + sem + " " + sectionvalue + " and fromdate= '" + querydate.ToString() + "'";
                                                                            dvalter = dsbindvalues.Tables[5].DefaultView;
                                                                        }
                                                                        bool alternatelab = false;
                                                                        if (dvalter.Count > 0)
                                                                        {
                                                                            classhour1 = dvalter[0]["" + columnvalue + ""].ToString();
                                                                        }
                                                                        if (classhour1 == "")
                                                                        {
                                                                            alternatelab = false;
                                                                            classhour1 = dvsemester[i]["" + columnvalue + ""].ToString();
                                                                        }
                                                                        else
                                                                        {
                                                                            alternatelab = true;
                                                                        }
                                                                        if (classhour1.ToString().Trim() != "")
                                                                        {
                                                                            notimetable = true; //Aruna  03sep2018
                                                                            string[] splitcode = classhour1.Split(';');
                                                                            bool batchflag = false;
                                                                            for (int g = 0; g <= splitcode.GetUpperBound(0); g++)
                                                                            {
                                                                                string[] valhr = splitcode[g].ToString().Split('-');
                                                                                if (valhr.GetUpperBound(0) > 1)
                                                                                {
                                                                                    string lsub = valhr[0].ToString();
                                                                                    if (hatlab.Contains(lsub))
                                                                                    {
                                                                                        batchflag = true;
                                                                                    }
                                                                                }
                                                                            }
                                                                            string[] split_sub_code = subcode_tot.Split(',');
                                                                            for (int k = 0; k <= splitcode.GetUpperBound(0); k++)
                                                                            {
                                                                                string staffcodecheck = splitcode[k].ToString();
                                                                                string[] staffsubject = staffcodecheck.Split('-');
                                                                                string tempstaffcode = string.Empty;
                                                                                string tempsubject_no = string.Empty;
                                                                                //Rajkumar 3/1/2018
                                                                                if (staffsubject.Length >= 2)
                                                                                {
                                                                                    tempstaffcode = Convert.ToString(staffsubject[1]);
                                                                                }
                                                                                if (staffsubject.Length > 0)
                                                                                {
                                                                                    tempsubject_no = staffsubject[0].ToString();
                                                                                }

                                                                                //string tempstaffcode = staffsubject[1].ToString();
                                                                                //string tempsubject_no = staffsubject[0].ToString();
                                                                                //==================================
                                                                                string staffquery = string.Empty;
                                                                                string sectionstraff = string.Empty;
                                                                                if (sectionvalue == "")
                                                                                {
                                                                                    sectionstraff = string.Empty;
                                                                                }
                                                                                else
                                                                                {
                                                                                    sectionstraff = "and st.sections='" + chksectionvalue.ToString() + "'";
                                                                                }
                                                                                string subjectcode = string.Empty;
                                                                                Att_strqueryst1 = "0";
                                                                                bool chkattflag = false;
                                                                                for (int staff = 1; staff < staffsubject.GetUpperBound(0); staff++)
                                                                                {
                                                                                    string subject = tempsubject_no;
                                                                                    chkattflag = false;
                                                                                    subjectcode = tempsubject_no.ToString();
                                                                                    string subjstaff = subjectcode;
                                                                                    string tempsubjstaff = tempstaffcode + '/' + tempsubject_no;
                                                                                    tempstaffcode = staffsubject[staff].ToString();
                                                                                    for (int spilt = 3; spilt < columnvalue.Length; spilt++)
                                                                                    {
                                                                                        hr = columnvalue[spilt].ToString();
                                                                                    }
                                                                                    string[] spiltdate = date.Split('/');
                                                                                    long strdate = (Convert.ToInt32(spiltdate[1]) + Convert.ToInt32(spiltdate[2]) * 12);
                                                                                    string Att_dcolumn1 = "d" + spiltdate[0] + "d" + hr;
                                                                                    string[] sp2 = staffcodecheck.Split('-');
                                                                                    string check_lab = string.Empty;
                                                                                    if (batchflag == false)
                                                                                    {
                                                                                        check_lab = "0";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        check_lab = "1";
                                                                                    }
                                                                                    if (chkattflag == false)
                                                                                    {
                                                                                        chkattflag = true;
                                                                                        if (sp2.GetUpperBound(0) > 1)
                                                                                        /* alter nate*/
                                                                                        {
                                                                                            string sectionsvalue = string.Empty;
                                                                                            string labsection = string.Empty;
                                                                                            if (chksectionvalue != "-1" && chksectionvalue != "" && chksectionvalue != null)
                                                                                            {
                                                                                                sectionsvalue = chksectionvalue;
                                                                                                labsection = " and sections='" + chksectionvalue + "'";
                                                                                            }
                                                                                            if (check_lab == "0" || check_lab.Trim().ToLower() == "false")
                                                                                            {
                                                                                                hatvalue.Clear();
                                                                                                hatvalue.Add("batch_year", batchyear);
                                                                                                hatvalue.Add("degree_code", dcode);
                                                                                                hatvalue.Add("sem", sem);
                                                                                                hatvalue.Add("sections", sectionsvalue);
                                                                                                hatvalue.Add("month_year", strdate);
                                                                                                hatvalue.Add("date", caldate.ToString());
                                                                                                hatvalue.Add("subject_no", sp2[0]);
                                                                                                isChoiceBased = isChoiceBasedSystem(batchyear);
                                                                                                if (isChoiceBased)
                                                                                                {
                                                                                                    hatvalue.Add("isCBCS", 1);
                                                                                                    hatvalue.Add("staffCode", tempstaffcode);
                                                                                                }
                                                                                                dsstuatt.Reset();
                                                                                                dsstuatt.Dispose();
                                                                                                dsstuatt = da.select_method("sp_stu_atten_month_check", hatvalue, "sp");
                                                                                                if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                {
                                                                                                    Att_strqueryst1 = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                                                                                    if (int.Parse(Att_strqueryst1) > 0)
                                                                                                    {
                                                                                                        hatvalue.Clear();
                                                                                                        hatvalue.Add("columnname ", Att_dcolumn1);
                                                                                                        hatvalue.Add("batch_year", batchyear);
                                                                                                        hatvalue.Add("degree_code", dcode);
                                                                                                        hatvalue.Add("sem", sem);
                                                                                                        hatvalue.Add("sections", sectionsvalue);
                                                                                                        hatvalue.Add("month_year", strdate);
                                                                                                        hatvalue.Add("date", caldate.ToString());
                                                                                                        hatvalue.Add("subject_no", sp2[0]);
                                                                                                        if (isChoiceBased)
                                                                                                        {
                                                                                                            hatvalue.Add("isCBCS", 1);
                                                                                                            hatvalue.Add("staffCode", tempstaffcode);
                                                                                                        }
                                                                                                        dsstuatt.Reset();
                                                                                                        dsstuatt.Dispose();
                                                                                                        string qryStudStaff = string.Empty;
                                                                                                        if (isChoiceBased)
                                                                                                            qryStudStaff = " and subjectchooser.staffCode like '%" + tempstaffcode + "%'";
                                                                                                        string strgetatt = "select count(registration.roll_no) as stucount  from registration,attendance,subjectchooser where degree_code='" + dcode + "' and current_semester='" + sem + "' and batch_year='" + batchyear + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and  registration.roll_no=subjectchooser.roll_no ";
                                                                                                        strgetatt = strgetatt + " and registration.current_semester=subjectchooser.semester and subject_no='" + sp2[0] + "' " + labsection + "";// subject_no='" + sp2[0] + "'
                                                                                                        strgetatt = strgetatt + " and (" + Att_dcolumn1 + " is not null and " + Att_dcolumn1 + "<>'0' and " + Att_dcolumn1 + "<>'') and adm_date<='" + caldate.ToString("MM/dd/yyyy") + "' " + qryStudStaff;
                                                                                                        dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                        if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                        {
                                                                                                            if (Att_strqueryst1 == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                            {
                                                                                                                Att_strqueryst1 = "0";
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                Att_strqueryst1 = "1";
                                                                                                            }
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            Att_strqueryst1 = "1";
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        Att_strqueryst1 = "0";
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    Att_strqueryst1 = "1";
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (alternatelab == true)
                                                                                                {
                                                                                                    hatvalue.Clear();
                                                                                                    hatvalue.Add("batch_year", batchyear);
                                                                                                    hatvalue.Add("degree_code", dcode);
                                                                                                    hatvalue.Add("sem", sem);
                                                                                                    hatvalue.Add("sections", sectionsvalue);
                                                                                                    hatvalue.Add("month_year", strdate);
                                                                                                    hatvalue.Add("date", caldate.ToString());
                                                                                                    hatvalue.Add("subject_no", sp2[0]);
                                                                                                    hatvalue.Add("day", dayget);
                                                                                                    hatvalue.Add("hour", hr);
                                                                                                    dsstuatt.Reset();
                                                                                                    dsstuatt.Dispose();
                                                                                                    dsstuatt = da.select_method("sp_stu_atten_month_check_lab_alter", hatvalue, "sp");
                                                                                                    if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                    {
                                                                                                        Att_strqueryst1 = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                                                                                        if (int.Parse(Att_strqueryst1) > 0)
                                                                                                        {
                                                                                                            hatvalue.Clear();
                                                                                                            hatvalue.Add("columnname", Att_dcolumn1);
                                                                                                            hatvalue.Add("batch_year", batchyear);
                                                                                                            hatvalue.Add("degree_code", dcode);
                                                                                                            hatvalue.Add("sem", sem);
                                                                                                            hatvalue.Add("sections", sectionsvalue);
                                                                                                            hatvalue.Add("month_year", strdate);
                                                                                                            hatvalue.Add("date", caldate.ToString());
                                                                                                            hatvalue.Add("subject_no", sp2[0]);
                                                                                                            hatvalue.Add("day", dayget);
                                                                                                            hatvalue.Add("hour", hr);
                                                                                                            dsstuatt.Reset();
                                                                                                            dsstuatt.Dispose();
                                                                                                            isChoiceBased = isChoiceBasedSystem(batchyear);
                                                                                                            string qryStudStaff = string.Empty;
                                                                                                            if (isChoiceBased)
                                                                                                                qryStudStaff = " and subjectchooser.staffCode like '%" + tempstaffcode + "%'";
                                                                                                            //prabha 26jan2018 string strgetatt = "select count(r.Roll_No) as stucount from registration r,attendance a,subjectchooser_new s where degree_code='" + dcode + "' and current_semester='" + sem + "' and batch_year='" + batchyear + "' and cc=0 and delflag=0 and  exam_flag<>'debar' and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";
                                                                                                            string strgetatt = "select count(distinct r.Roll_No) as stucount from registration r,attendance a,subjectchooser_new s where degree_code='" + dcode + "' and current_semester='" + sem + "' and batch_year='" + batchyear + "' and cc=0 and delflag=0 and  exam_flag<>'debar' and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";
                                                                                                            strgetatt = strgetatt + " r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + labsection + " and(" + Att_dcolumn1 + " is not null and " + Att_dcolumn1 + "<>'0' and " + Att_dcolumn1 + "<>'') and fromdate='" + caldate + "' and batch    in(select stu_batch from laballoc_new where subject_no='" + sp2[0].ToString() + "'  and batch_year='" + batchyear + "'  and hour_value='" + hr + "'  and    degree_code='" + dcode + "' ";
                                                                                                            strgetatt = strgetatt + " and day_value='" + dayget + "' and semester='" + sem + "' " + labsection + " and fdate='" + caldate + "') and adm_date<='" + caldate + "'";//+ qryStudStaff
                                                                                                            dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                            //distinct in count (r.Roll_no) has been added by prabha 26jan2018

                                                                                                            if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                            {
                                                                                                                if (Att_strqueryst1 == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                {
                                                                                                                    Att_strqueryst1 = "0";
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Att_strqueryst1 = "1";
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                Att_strqueryst1 = "1";
                                                                                                            }
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            Att_strqueryst1 = "0";
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        Att_strqueryst1 = "1";
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    string timetable = da.GetFunction("select top 1 TTName,  FromDate from  Semester_Schedule where degree_code='" + dcode + "' and semester='" + sem + "' and batch_year='" + batchyear + "' " + labsection + " and FromDate<='" + caldate.ToString("MM/dd/yyyy") + "' order by FromDate Desc");
                                                                                                    hatvalue.Clear();
                                                                                                    hatvalue.Add("batch_year", batchyear);
                                                                                                    hatvalue.Add("degree_code", dcode);
                                                                                                    hatvalue.Add("sem", sem);
                                                                                                    hatvalue.Add("sections", sectionsvalue);
                                                                                                    hatvalue.Add("month_year", strdate);
                                                                                                    hatvalue.Add("date", caldate.ToString());
                                                                                                    hatvalue.Add("subject_no", sp2[0]);
                                                                                                    hatvalue.Add("day", dayget);
                                                                                                    hatvalue.Add("hour", hr);
                                                                                                    hatvalue.Add("ttmane", timetable);
                                                                                                    isChoiceBased = isChoiceBasedSystem(batchyear);
                                                                                                    if (isChoiceBased)
                                                                                                    {
                                                                                                        hatvalue.Add("isCBCS", 1);
                                                                                                        hatvalue.Add("staffCode", tempstaffcode);
                                                                                                    }
                                                                                                    dsstuatt.Reset();
                                                                                                    dsstuatt.Dispose();
                                                                                                    dsstuatt = da.select_method("sp_stu_atten_month_check_lab", hatvalue, "sp");
                                                                                                    if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                    {
                                                                                                        Att_strqueryst1 = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                                                                                        if (int.Parse(Att_strqueryst1) > 0)
                                                                                                        {
                                                                                                            hatvalue.Clear();
                                                                                                            hatvalue.Add("columnname", Att_dcolumn1);
                                                                                                            hatvalue.Add("batch_year", batchyear);
                                                                                                            hatvalue.Add("degree_code", dcode);
                                                                                                            hatvalue.Add("sem", sem);
                                                                                                            hatvalue.Add("sections", sectionsvalue);
                                                                                                            hatvalue.Add("month_year", strdate);
                                                                                                            hatvalue.Add("date", caldate.ToString());
                                                                                                            hatvalue.Add("subject_no", sp2[0]);
                                                                                                            hatvalue.Add("day", dayget);
                                                                                                            hatvalue.Add("hour", hr);
                                                                                                            hatvalue.Add("ttmane", timetable);
                                                                                                            isChoiceBased = isChoiceBasedSystem(batchyear);
                                                                                                            if (isChoiceBased)
                                                                                                            {
                                                                                                                hatvalue.Add("isCBCS", 1);
                                                                                                                hatvalue.Add("staffCode", tempstaffcode);
                                                                                                            }
                                                                                                            dsstuatt.Reset();
                                                                                                            dsstuatt.Dispose();

                                                                                                            string qryStudStaff = string.Empty;
                                                                                                            if (isChoiceBased)
                                                                                                                qryStudStaff = " and s.staffCode like '%" + tempstaffcode + "%'";


                                                                                                            string labbatch = "select stu_batch from laballoc where subject_no='" + sp2[0].ToString() + "' and Timetablename='" + timetable + "' and batch_year='" + batchyear + "'  and hour_value='" + hr + "'  and    degree_code='" + dcode + "' and day_value='" + dayget + "' and semester='" + sem + "' " + labsection + "";
                                                                                                            DataSet lab_batch = da.select_method_wo_parameter(labbatch, "text");
                                                                                                            string batch_lab = string.Empty;
                                                                                                            if (lab_batch.Tables.Count > 0 && lab_batch.Tables[0].Rows.Count > 0)
                                                                                                            {
                                                                                                                for (int batc = 0; batc < lab_batch.Tables[0].Rows.Count; batc++)
                                                                                                                {
                                                                                                                    if (batch_lab == "")
                                                                                                                        batch_lab = "" + Convert.ToString(lab_batch.Tables[0].Rows[batc]["stu_batch"]) + "";
                                                                                                                    else
                                                                                                                    {
                                                                                                                        batch_lab = batch_lab + "'" + "," + "'" + Convert.ToString(lab_batch.Tables[0].Rows[batc]["stu_batch"]) + "";
                                                                                                                    }
                                                                                                                }
                                                                                                            }

                                                                                                            string strgetatt = "select count(r.Roll_No) as stucount from registration r,attendance a,subjectchooser s where degree_code='" + dcode + "' and current_semester='" + sem + "' and batch_year='" + batchyear + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year=" + strdate + "";
                                                                                                            strgetatt = strgetatt + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + labsection + " and(" + Att_dcolumn1 + " is not null and " + Att_dcolumn1 + "<>'0' and " + Att_dcolumn1 + "<>'') and batch in('" + batch_lab + "') ";
                                                                                                            strgetatt = strgetatt + "  and adm_date<='" + caldate + "'" + qryStudStaff;
                                                                                                            //string strgetatt = "select count(r.Roll_No) as stucount from registration r,attendance a,subjectchooser s where degree_code='" + dcode + "' and current_semester='" + sem + "' and batch_year='" + batchyear + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year=" + strdate + "";
                                                                                                            //strgetatt = strgetatt + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + labsection + " and(" + Att_dcolumn1 + " is not null and " + Att_dcolumn1 + "<>'0' and " + Att_dcolumn1 + "<>'') and batch in(select stu_batch from laballoc ";
                                                                                                            //strgetatt = strgetatt + " where subject_no='" + sp2[0].ToString() + "' and Timetablename='" + timetable + "' and batch_year='" + batchyear + "'  and hour_value='" + hr + "'  and    degree_code='" + dcode + "' and day_value='" + dayget + "' and semester='" + sem + "' " + labsection + ") and adm_date<='" + caldate + "'" + qryStudStaff;
                                                                                                            dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                            if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                            {
                                                                                                                if (Att_strqueryst1 == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                {
                                                                                                                    Att_strqueryst1 = "0";
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Att_strqueryst1 = "1";
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                Att_strqueryst1 = "1";
                                                                                                            }
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            Att_strqueryst1 = "0";
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        Att_strqueryst1 = "1";
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    if (Convert.ToInt32(Att_strqueryst1) > 0)
                                                                                    {
                                                                                        if (head == false)
                                                                                        {
                                                                                            head = true;
                                                                                            if (sectionspilt.GetUpperBound(0) > 0)
                                                                                            {
                                                                                                FpSpread1.Sheets[0].RowCount++;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = date.ToString();
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Large;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                                string bat = Dept_name.ToString();
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Batch : " + batchyear + " " + '-' + " Branch : " + coursename + "-" + bat + " - Sem : " + sem + " " + '-' + " Section " + '-' + " " + chksectionvalue + " ";
                                                                                                if (forschoolsetting == true)
                                                                                                {
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Year : " + batchyear + " " + '-' + " Standard : " + bat + " - Term : " + sem + " " + '-' + " Section " + '-' + " " + chksectionvalue + " ";
                                                                                                }
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Large;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.Blue;
                                                                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 4);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                FpSpread1.Sheets[0].RowCount++;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = date.ToString();
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Large;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                                string bat = Dept_name.ToString();
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Batch : " + batchyear + " " + '-' + " Branch : " + coursename + "-" + bat + " - Sem : " + sem + "  ";
                                                                                                if (forschoolsetting == true)
                                                                                                {
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Year : " + batchyear + " " + '-' + " Standard : " + bat + " - Term : " + sem + "  ";
                                                                                                }
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Large;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.Blue;
                                                                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 4);
                                                                                            }
                                                                                        }
                                                                                        string staffcode = tempstaffcode;
                                                                                        string staff_name = string.Empty;
                                                                                        string staff_name_New = string.Empty;
                                                                                        string staff_code_new = string.Empty;
                                                                                        string subjectname = string.Empty;
                                                                                        string Sub_code = tempsubject_no;
                                                                                        dsbindvalues.Tables[1].DefaultView.RowFilter = "subject_no='" + tempsubject_no + "'";
                                                                                        dvstaffname = dsbindvalues.Tables[1].DefaultView;
                                                                                        if (dvstaffname.Count > 0)
                                                                                        {
                                                                                            subjectname = dvstaffname[0]["Subject_name"].ToString();
                                                                                            staff_code_new = dvstaffname[0]["staff_code"].ToString();
                                                                                            staff_name_New = dvstaffname[0]["staff_name"].ToString();
                                                                                        }
                                                                                        DataView dvstaff = new DataView();
                                                                                        if (dsbindvalues.Tables.Count > 6 && dsbindvalues.Tables[6].Rows.Count > 0)
                                                                                        {
                                                                                            dsbindvalues.Tables[6].DefaultView.RowFilter = "staff_code='" + tempstaffcode + "'";
                                                                                            dvstaff = dsbindvalues.Tables[6].DefaultView;
                                                                                        }
                                                                                        if (dvstaff.Count > 0)
                                                                                        {
                                                                                            staff_name = dvstaff[0]["staff_name"].ToString();
                                                                                        }


                                                                                        ds2.Reset();
                                                                                        ds2.Dispose();
                                                                                        sno++;
                                                                                        string[] datespilt = Convert.ToString(caldate).Split(' ');
                                                                                        string[] date1 = datespilt[0].Split('/');
                                                                                        string arrangedate = date1[1] + '/' + date1[0] + '/' + date1[2];
                                                                                        FpSpread1.Sheets[0].RowCount++;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                                        string values = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = dcode + "/" + batchyear + "/" + schorder + "/" + sem;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = arrangedate;
                                                                                        string date_1 = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = arrangedate;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Large;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = subjectname;
                                                                                        string col_value = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = Sub_code + "/" + subjectname;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = hr.ToString();
                                                                                        string hour_staff = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = hr + "/" + tempstaffcode;//Rajkumar 23/12/2017
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = staff_name;////Rajkumar 
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        classhour = string.Empty;
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                if (dvholiday.Count > 0)
                                                {

                                                    string holudayreson = dvholiday[0]["holiday_desc"].ToString();
                                                    if (!holidayDate.ContainsKey(caldate))//Rajkumar 11/1/2018
                                                    {
                                                        holidayDate.Add(caldate, holudayreson);

                                                        FpSpread1.Sheets[0].RowCount++;
                                                        string[] datespilt = Convert.ToString(caldate).Split(' ');
                                                        string[] get_split_date = datespilt[0].Split('/');
                                                        string arrangedate = get_split_date[1] + '/' + get_split_date[0] + '/' + get_split_date[2];
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = " " + arrangedate + " is " + holudayreson + "";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Red;
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                                                    }
                                                }
                                            }


                                            //if (isEve && !isfull)
                                            //{
                                            //    FpSpread1.Sheets[0].RowCount++;
                                            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Evening Holiday";
                                            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                                            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Red;
                                            //    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                            //}

                                        }
                                    }
                                }
                            }
                        }
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnxl.Visible = true;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = true;
                        if (sno == 0)
                        {
                            FpSpread1.Visible = false;
                            lblerror.Visible = true;
                            if (semflag == false)
                            {
                                lblerror.Text = "No Records Found";
                            }
                            else
                            {
                                if (notimetable == false) //Aruna 03sep2018
                                {
                                    lblerror.Text = "No Timetable/Alternate Schedule";
                                }
                                else
                                {
                                    lblerror.Text = "Staff Completed the Attendance";
                                }
                            }
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            Printcontrol.Visible = false;
                            btnprintmaster.Visible = false;
                        }
                    }
                }
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                int rowcount1 = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();
                FpSpread1.Width = 1000;
                FpSpread1.Height = rowcount1 * 50;
            }
        }

        catch (Exception evel)
        {
            //lblerror.Visible = true;
            //lblerror.Text = evel.ToString();
            //string collegecode1 = Session["collegecode"].ToString();
            //da.sendErrorMail(evel, collegecode1, "BlockBox1"); 
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.Trim().Replace(" ", "_").Trim();
            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                Labelstaf.Text = "Please Enter Your Report Name";
                Labelstaf.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Labelstaf.Text = ex.ToString();
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void chkdegreewise_CheckedChanged(object sender, EventArgs e)
    {
        collegecode = ddlcollege.SelectedValue.ToString();
        FpSpread1.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        rbCAM.Visible = false;
        rbAtt.Visible = false;
        if (chkdegreewise.Checked == true)
        {
            rbCAM.Visible = true;
            rbAtt.Visible = true;
            rdiobtndetailornot.Visible = false;
            txtbranch.Text = "---Select---";
            txtdesi.Text = "---Select---";
            txtstaff.Text = "---Select---";
            chklstbranch.Items.Clear();
            chklstsubject.Items.Clear();
            chklststaff.Items.Clear();
            deptlbl.Text = "Batch";
            lbldesignation.Text = "Degree";
            lblstaff.Text = "Branch";
            if (forschoolsetting == true)
            {
                deptlbl.Text = "Year";
                lbldesignation.Text = "School Type";
                lblstaff.Text = "Standard";
            }
            BindBatch();

            BindDegree(singleuser, group_user, collegecode, usercode);
            if (cbldesi.Items.Count > 0)
            {
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                for (int checkall = 0; checkall < chklststaff.Items.Count; checkall++)
                {
                    chklststaff.Items[checkall].Selected = true;
                }
                chkstaff.Checked = true;
                if (chklststaff.Items.Count > 0)
                {
                    txtstaff.Text = "Branch (" + chklststaff.Items.Count + ")";
                }
                else
                {
                    txtstaff.Text = "---Select---";
                }
                // txtstaff.Text = "Branch (" + chklststaff.Items.Count + ")";
            }
            else
            {
                chkstaff.Checked = false;
                txtstaff.Text = "---Select---";
            }
            BindDegSubject();
            BindSubject();
        }
        else
        {
            rdiobtndetailornot.Visible = true;
            txtbranch.Text = "---Select---";
            txtdesi.Text = "---Select---";
            txtstaff.Text = "---Select---";
            deptlbl.Text = "Department";
            lbldesignation.Text = "Designation";
            lblstaff.Text = "Staff Name";
            if (forschoolsetting == true)
            {
                deptlbl.Text = "Standard";
            }
            BindDesignation();
            BindDepartment();
            BindStaff();
            BindSubject();
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "Black Box Report" + '@' + " Date: " + tbfdate.Text + " To " + tbtodate.Text;
        Session["column_header_row_count"] = FpSpread1.ColumnHeader.RowCount;
        string pagename = "Blockbox3.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        collegecode = ddlcollege.SelectedValue.ToString();

        chkdegreewise_CheckedChanged(sender, e);

    }

    protected void chklscolumn_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private bool isChoiceBasedSystem(string batchYear)
    {
        bool staffSelector = false;
        try
        {

            string qryStudeStaffSelector = string.Empty;
            string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where settings='Studnet Staff Selector' and college_code='" + ddlcollege.SelectedValue.ToString() + "' ");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(batchYear.ToString()) >= batchyearsetting)
                    {
                        staffSelector = true;
                    }
                }
            }
            else if (splitminimumabsentsms.Length > 0)
            {
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }
            //if (staffSelector)
            //{
            //    qryStudeStaffSelector = " and sc.staffcode like '%" + staffcode + "%'";
            //}
        }
        catch
        {
        }
        return staffSelector;
    }

    private bool isChoiceBasedSystemWithBatch(string batchYear, ref string batchYearNew)
    {
        bool staffSelector = false;
        try
        {

            string qryStudeStaffSelector = string.Empty;
            string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + ddlcollege.SelectedValue.ToString() + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                batchYearNew = splitminimumabsentsms[1];
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(batchYear.ToString()) >= batchyearsetting)
                    {
                        staffSelector = true;
                    }
                }
            }
            else if (splitminimumabsentsms.Length > 0)
            {
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }
            //if (staffSelector)
            //{
            //    qryStudeStaffSelector = " and sc.staffcode like '%" + staffcode + "%'";
            //}
        }
        catch
        {
        }
        return staffSelector;
    }

    protected void chkDegsubject_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chkDegSubject, cblDegSubject, txtDegSubject, lblDegSubject.Text, "--Select--");
    }

    protected void chklDegstsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkDegSubject, cblDegSubject, txtDegSubject, lblDegSubject.Text, "--Select--");
    }


    public void CAMReport()//rajkumar 05/06/2018
    {
        try
        {
            string batchYear = string.Empty;
            string degCode = string.Empty;
            string suCode = string.Empty;
            if (chklstbranch.Items.Count > 0)
                batchYear = rs.getCblSelectedValue(chklstbranch);
            if (cbldesi.Items.Count > 0)
                degCode = rs.getCblSelectedValue(chklststaff);
            if (cblDegSubject.Items.Count > 0)
                suCode = rs.getCblSelectedValue(cblDegSubject);
            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FpSpread1.Sheets[0].FrozenColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Last Date";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "CAM Details";
            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[4].Locked = true;

            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
            FpSpread1.Sheets[0].Columns[0].Width = 40;
            FpSpread1.Sheets[0].Columns[1].Width = 100;
            FpSpread1.Sheets[0].Columns[2].Width = 250;
            FpSpread1.Sheets[0].Columns[3].Width = 300;
            FpSpread1.Sheets[0].Columns[4].Width = 100;

            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;


            FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            string date1 = tbfdate.Text.ToString();
            string[] split = date1.Split(new Char[] { '-' });
            string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            string date2 = tbtodate.Text.ToString();
            string[] split1 = date2.Split(new Char[] { '-' });
            string dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
            DateTime dt1 = Convert.ToDateTime(datefrom);
            DateTime dt2 = Convert.ToDateTime(dateto);
            string getalldetails = string.Empty;
            int sno = 0;
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degCode) && !string.IsNullOrEmpty(suCode))
            {
                getalldetails = getalldetails + " select c.criteria,c.Criteria_no,e.exam_code,sy.Batch_Year,sy.degree_code,sy.semester,e.sections,s.subject_no,s.subject_name,convert(nvarchar(15),c.LastDate,103) as LastDate,st.staff_code,SM.staff_name from CriteriaForInternal c,syllabus_master sy,subject s,Exam_type e,staff_selector st,staffmaster sm where e.sections=st.Sections and st.staff_code=sm.staff_code and c.syll_code=s.syll_code and c.Criteria_no=e.criteria_no and e.subject_no=s.subject_no and sy.syll_code=s.syll_code and e.subject_no=st.subject_no and s.subject_no=st.subject_no and c.LastDate between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' and sy.Batch_Year in('" + batchYear + "') and sy.degree_code in('" + degCode + "') and s.subject_no in('" + suCode + "') order by sy.Batch_Year,sy.degree_code,sy.semester,s.subject_no";


                getalldetails = getalldetails + " select count(r.roll_no) as stucount,c.criteria,c.Criteria_no,e.exam_code,sy.Batch_Year,sy.degree_code,sy.semester,e.sections,s.subject_no,s.subject_name,convert(nvarchar(15),c.LastDate,103) as LastDate from CriteriaForInternal c,syllabus_master sy,subject s,Exam_type e,Result r where c.syll_code=s.syll_code and c.Criteria_no=e.criteria_no and e.subject_no=s.subject_no  and sy.syll_code=s.syll_code and r.exam_code=e.exam_code and c.LastDate between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' and sy.Batch_Year in('" + batchYear + "') and sy.degree_code in('" + degCode + "') and s.subject_no in('" + suCode + "') group by c.criteria,c.Criteria_no,e.exam_code,sy.Batch_Year,sy.degree_code,sy.semester,e.sections,s.subject_no,s.subject_name,c.LastDate";
                DataSet dsall = da.select_method_wo_parameter(getalldetails, "Text");//
                if (dsall.Tables.Count > 0 && dsall.Tables[0].Rows.Count > 0)
                {
                    if (dt1 <= dt2)
                    {
                        TimeSpan t = dt2.Subtract(dt1);
                        int days = t.Days;
                        for (int row_inc = 0; row_inc <= days; row_inc++)
                        {
                            DateTime cur_day = new DateTime();
                            cur_day = dt2.AddDays(-row_inc);
                            dsall.Tables[0].DefaultView.RowFilter = "LastDate='" + cur_day.ToString("dd/MM/yyyy") + "'";
                            DataTable dtdate = dsall.Tables[0].DefaultView.ToTable();
                            if (dtdate.Rows.Count > 0)
                            {
                                DataTable dictable = dsall.Tables[0].DefaultView.ToTable(true, "degree_code", "Batch_Year", "semester", "sections");
                                foreach (DataRow dt in dictable.Rows)
                                {
                                    string batch = Convert.ToString(dt["Batch_Year"]);
                                    string degCode1 = Convert.ToString(dt["degree_code"]);
                                    string sem = Convert.ToString(dt["semester"]);
                                    string Sec = Convert.ToString(dt["sections"]);
                                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                    string dept = batch + "-" + da.GetFunction("select c.Course_Name+'-'+de.Dept_Name from Degree d,course c, Department de where d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and d.Degree_Code='" + degCode1 + "'") + "- " + sem + "- " + Sec;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = dept;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightPink;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                    DataView dvgettest = new DataView();

                                    if (dsall.Tables.Count > 0 && dsall.Tables[0].Rows.Count > 0)
                                    {
                                        dsall.Tables[0].DefaultView.RowFilter = " degree_code='" + degCode1 + "' and Batch_Year='" + batch + "' and semester='" + sem + "' and sections='" + Sec + "'";
                                        dvgettest = dsall.Tables[0].DefaultView;
                                        for (int cl = 0; cl < dvgettest.Count; cl++)
                                        {
                                            string testname = dvgettest[cl]["criteria"].ToString();
                                            string subjectname = dvgettest[cl]["subject_name"].ToString();
                                            string subjectno = dvgettest[cl]["subject_no"].ToString();
                                            string criteriano = dvgettest[cl]["Criteria_no"].ToString();
                                            string examcode = dvgettest[cl]["exam_code"].ToString();
                                            string batch1 = dvgettest[cl]["Batch_Year"].ToString();
                                            string degree = dvgettest[cl]["degree_code"].ToString();
                                            string sem1 = dvgettest[cl]["semester"].ToString();
                                            string sectionsc = dvgettest[cl]["sections"].ToString();
                                            string lastData = Convert.ToString(dvgettest[cl]["LastDate"]);//
                                            string staffCode = Convert.ToString(dvgettest[cl]["staff_code"]);
                                            string staffName = Convert.ToString(dvgettest[cl]["staff_name"]);
                                            string secval = string.Empty;
                                            if (sectionsc.Trim() != "" && sectionsc != "-1" && sectionsc != null)
                                            {
                                                secval = " and sections='" + sectionsc + "'";
                                            }
                                            DataTable dvcamstu = new DataTable();
                                            if (dsall.Tables.Count > 1 && dsall.Tables[1].Rows.Count > 0)
                                            {
                                                dsall.Tables[1].DefaultView.RowFilter = "Criteria_no ='" + criteriano + "' and exam_code='" + examcode + "' " + secval + " and subject_no='" + subjectno + "'";
                                                dvcamstu = dsall.Tables[1].DefaultView.ToTable();
                                                if (dvcamstu.Rows.Count == 0)
                                                {
                                                    FpSpread1.Visible = true;
                                                    sno++;
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = lastData;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = subjectname;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staffCode + "- " + staffName;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = 12;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = testname;
                                                }
                                            }
                                            else
                                            {
                                                FpSpread1.Visible = true;
                                                sno++;
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = lastData;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = subjectname;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staffCode + "- " + staffName;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = 12;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = testname;
                                            }

                                        }
                                    }

                                }
                            }

                        }
                    }
                    int rowcount = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Width = 900;
                    FpSpread1.Height = 70 + (rowcount * 22);
                    FpSpread1.Sheets[0].PageSize = 25 + (rowcount * 20);
                    FpSpread1.SaveChanges();
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "No record Found";
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Select  Degree Details";
            }
        }
        catch
        {

        }
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        StringBuilder selectedvalue = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        StringBuilder selectedText = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedText.ToString();
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    private bool getSelectedCheckBoxListCount(CheckBoxList cbl, out int selectedCount)
    {
        selectedCount = 0;
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                if (li.Selected)
                {
                    selectedCount++;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="c">Only Data Bound Controls eg.DropDownList,RadioButtonList,CheckBoxList </param>
    /// <param name="selectedValue"></param>
    /// <param name="selectedText"></param>
    /// <param name="type">0 - Index; 1 - Text; 2 - Value;</param>
    private void SelectDataBound(Control c, string selectedValue, string selectedText)
    {
        try
        {
            bool isDataBoundControl = false;
            if (c is DataBoundControl)
            {
                if (c is CheckBoxList || c is DropDownList || c is RadioButtonList)
                {
                    isDataBoundControl = true;
                }
                if (isDataBoundControl)
                {
                    ListControl lstControls = (ListControl)c;
                    if (lstControls.Items.Count > 0)
                    {
                        ListItem[] listItem = new ListItem[lstControls.Items.Count];
                        lstControls.Items.CopyTo(listItem, 0);
                        if (listItem.Contains(new ListItem(selectedText, selectedValue)))
                        {
                            lstControls.SelectedValue = selectedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion



}
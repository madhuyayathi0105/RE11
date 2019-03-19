using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;

public partial class dailystudentattndreport : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection dc_con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection dc_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_date = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    InsproDirectAccess dir = new InsproDirectAccess();
    SqlCommand cmd;
    Boolean chk = false;
    Boolean fflag = false;
    Boolean holidayflag = false;
    Boolean update = false;
    static Boolean forschoolsetting = false;
    DAccess2 dacces2 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds_date = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    static DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    int nohrs = 0;
    int fd = 0;
    int fyy = 0;
    int fm = 0;
    int td = 0;
    int tyy = 0;
    int tm = 0;
    int fcal = 0;
    int tcal = 0;
    int totmonth = 0;
    int dat = 0;
    int totpresentday = 0;
    int daycount = 0;
    int balamonday = 0;
    int endk = 0;
    int mm = 0;
    int row_head_cnt = 1;
    int first_half = 0, second_half = 0;
    int split_holiday_status_1 = 0, split_holiday_status_2 = 0, split_holiday_status_3 = 0, split_holiday_status_4 = 0;
    int pct, act, odct, mlct, sodct, nssct, hct, njct, sct, lct, nccct, hsct, ppct, syodct, codct, oodct, lact, nect, raact = 0;
    DateTime dummydate;
    DateTime dt1 = new DateTime();
    DateTime dt2 = new DateTime();
    static DateTime from_date = new DateTime();
    static DateTime to_date = new DateTime();
    FarPoint.Web.Spread.ComboBoxCellType objintcell = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType objcom = new FarPoint.Web.Spread.ComboBoxCellType();
    string stud_name = string.Empty;
    string frdate = string.Empty;
    string todate = string.Empty;
    string ddd = string.Empty;
    string Att_mark;
    string Attvalue = string.Empty;
    string attnd_val = string.Empty;
    string temp_val = string.Empty;
    string leave_code = string.Empty;
    string date1 = string.Empty;
    string datefrom = string.Empty;
    string date2 = string.Empty;
    string dateto = string.Empty;
    string collegeCode = string.Empty;
    string batchYear = string.Empty;
    string degreeCode = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;
    string currentSemester = string.Empty;
    string to_date_sem = string.Empty;
    string isredoold = string.Empty;
    static string from_date_sem = string.Empty;
    static string grouporusercode = string.Empty;
    static string get_roll_no_r = string.Empty;
    string[] strcomo;
    string[] strcomo1;
    string stdate = string.Empty;
    string endate = string.Empty;
    int countold = 0;
    int attndreportheight = 0;
    double totalRows = 0;
    int rowct = 0;
    bool delflagold = false;
    bool delflagnew = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        string val = d2.GetFunction("select value from Master_Settings where settings='Individual Student Attendace Lock' and usercode='" + Session["usercode"].ToString() + "'");
        if (val.Trim() == "0")
        {
            btnsave.Enabled = true;
        }
        else
        {
            btnsave.Enabled = false;
        }
        if (!Page.IsPostBack)
        {
            ddlSem.Items.Clear();
            ddlSem.Enabled = false;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            txtFromDate.Visible = false;
            txtToDate.Visible = false;
            lblFromdate.Visible = false;
            lbltodate.Visible = false;
            btnGo.Visible = false;
            dateerrlbl.Visible = false;
            attnd_report.Visible = false;
            divNote.Visible = false;
            lblname1.Visible = false;
            lblname2.Visible = false;
            errlbl.Visible = false;
            pageset_pnl.Visible = false;
            pageddltxt.Visible = false;
            btnsave.Visible = false;
            attnd_report.Sheets[0].AutoPostBack = false;
            attnd_report.ActiveSheetView.AutoPostBack = false;
            //if (Convert.ToString(Session["value"]) == "1")
            //{
            //    LinkButton3.Visible = false;
            //    LinkButton2.Visible = true;
            //}
            //else
            //{
            //    LinkButton3.Visible = true;
            //    LinkButton2.Visible = false;
            //}
            Session["curr_year"] = DateTime.Today.ToString("yyyy");
            lblrollno.Text = "Enter Student Roll No";
            FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
            MyStyle.Font.Bold = true;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            attnd_report.ActiveSheetView.ColumnHeader.DefaultStyle = MyStyle;
            attnd_report.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
            attnd_report.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            attnd_report.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
            attnd_report.ActiveSheetView.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Sheets[0].ColumnHeader.RowCount = 2;
            attnd_report.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = FontUnit.Medium;
            style.Font.Bold = true;
            attnd_report.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            attnd_report.Sheets[0].AllowTableCorner = true;
            attnd_report.Sheets[0].SheetCorner.Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Sheets[0].SheetCornerSpanModel.Add(0, 0, 2, 1);
            attnd_report.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
            attnd_report.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            attnd_report.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            attnd_report.Sheets[0].DefaultStyle.Font.Bold = false;
            string strdayflag;
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            string Master1 = string.Empty;
            Master1 = "select * from Master_Settings where " + grouporusercode + "";
            //readcon.Close();
            //readcon.Open();
            //SqlDataReader mtrdr;
            //SqlCommand mtcmd = new SqlCommand(Master1, readcon);
            //mtrdr = mtcmd.ExecuteReader();
            DataSet dsMaster = new DataSet();
            dsMaster = d2.select_method_wo_parameter(Master1, "text");
            strdayflag = string.Empty;
            //while (mtrdr.Read())
            if (dsMaster.Tables.Count > 0 && dsMaster.Tables[0].Rows.Count > 0)
            {
                //if (mtrdr.HasRows == true)
                foreach (DataRow mtrdr in dsMaster.Tables[0].Rows)
                {
                    if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (mtrdr["settings"].ToString() == "Student_Type" && mtrdr["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                    if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                    {
                        strdayflag = " and (Stud_Type='Day Scholar'";
                    }
                    if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                    {
                        if (strdayflag != "" && strdayflag != "\0")
                        {
                            strdayflag = strdayflag + " or Stud_Type='Hostler'";
                        }
                        else
                        {
                            strdayflag = " and (Stud_Type='Hostler'";
                        }
                    }
                    if (mtrdr["settings"].ToString() == "Day Wise" && mtrdr["value"].ToString() == "1")
                    {
                        Session["daywise"] = "1";
                    }
                    if (mtrdr["settings"].ToString() == "Hour Wise" && mtrdr["value"].ToString() == "1")
                    {
                        Session["hourwise"] = "1";
                    }
                }
            }
            if (strdayflag != string.Empty)
            {
                strdayflag = strdayflag + ")";
            }
            Session["strvar"] = strdayflag;
            string grouporusercodeschool = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercodeschool = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercodeschool = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercodeschool + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = dacces2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                if (schoolvalue.Trim() == "0")
                {
                    forschoolsetting = true;
                }
                else
                {
                    forschoolsetting = false;
                }
            }
            else
            {
                forschoolsetting = false;
            }

        }
        Session["attdaywisecla"] = "0";
        string daywisecal = d2.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
        if (daywisecal.Trim() == "1")
        {
            Session["attdaywisecla"] = "1";
        }
    }

    public void BindSem(string collegecode, string batchyear, string degreecode, string currentSem)
    {
        try
        {
            ddlSem.Items.Clear();
            ddlSem.Enabled = false;
            string strbatchyear = string.Empty;
            string strbranch = string.Empty;
            ddlSem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            rptprint1.Visible = false;
            btnsave.Visible = false;
            attnd_report.Visible = false;
            divNote.Visible = false;
            std_info.Visible = false;
            pageset_pnl.Visible = false;
            errlbl.Visible = false;
            lblname1.Visible = false;
            lblname2.Visible = false;
            collegeCode = collegecode.Trim();
            batchYear = batchyear.Trim();
            degreeCode = degreecode.Trim();
            semester = currentSem.Trim();
            section = string.Empty;
            currentSemester = currentSem.Trim();
            string qry = string.Empty;
            string rollNo = txtrollno.Text.Trim();
            DataSet dsSemDetails = new DataSet();
            dsSemDetails.Dispose();
            dsSemDetails.Reset();
            ddlSem.Items.Clear();
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(batchYear))
            {
                qry = "select distinct max(ndurations) as ndurations,first_year_nonsemester from ndegree where degree_code in (" + degreeCode + ") and batch_year in (" + batchYear + ") and college_code='" + collegeCode + "' group by first_year_nonsemester order by ndurations desc; select distinct max(duration) duration,first_year_nonsemester from degree where degree_code in (" + degreeCode + ") and college_code='" + collegeCode + "' group by first_year_nonsemester order by duration desc";
                dsSemDetails = d2.select_method_wo_parameter(qry, "Text");
            }
            if (dsSemDetails.Tables.Count > 0)
            {
                if (dsSemDetails.Tables[0].Rows.Count > 0)
                {
                    bool.TryParse(Convert.ToString(dsSemDetails.Tables[0].Rows[0][1]).Trim(), out first_year);
                    int.TryParse(Convert.ToString(dsSemDetails.Tables[0].Rows[0][0]).Trim(), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                    }
                }
                else if (dsSemDetails.Tables.Count > 1 && dsSemDetails.Tables[1].Rows.Count > 0)
                {
                    bool.TryParse(Convert.ToString(dsSemDetails.Tables[1].Rows[0][1]).Trim(), out first_year);
                    int.TryParse(Convert.ToString(dsSemDetails.Tables[1].Rows[0][0]).Trim(), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                    }
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                ddlSem.Enabled = true;
                foreach (System.Web.UI.WebControls.ListItem lisem in ddlSem.Items)
                {
                    if (lisem.Value.Trim() == currentSem.Trim())
                    {
                        ddlSem.SelectedValue = currentSemester;
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = false;
            btnsave.Visible = false;
            attnd_report.Visible = false;
            divNote.Visible = false;
            std_info.Visible = false;
            pageset_pnl.Visible = false;
            errlbl.Visible = false;
            DateTime Admission_date;
            string field_val = string.Empty;
            string singleorgroup = string.Empty;
            string strbatchsectionrights = string.Empty;
            hat.Clear();
            if (optionddl.Items[0].Selected == true)
            {
                field_val = " and r.roll_no='" + txtrollno.Text.Trim().ToString() + "'";
            }
            else if (optionddl.Items[1].Selected == true)
            {
                field_val = " and r.reg_no='" + txtrollno.Text.Trim().ToString() + "'";
            }
            else
            {
                field_val = " and r.roll_admit='" + txtrollno.Text.Trim().ToString() + "'";
            }
            ds.Clear();
            if (Session["Single_User"] != null && (Convert.ToString(Session["Single_User"]).Trim().ToLower() == "true" || Convert.ToString(Session["Single_User"]).Trim().ToLower() == "1"))
            {
                singleorgroup = " and user_code='" + Session["UserCode"] + "'";
                strbatchsectionrights = "and user_id='" + Session["UserCode"].ToString() + "'";
            }
            else if (Session["group_code"] != null)
            {
                string groupcode = Convert.ToString(Session["group_code"]).Trim();
                string[] from_split = groupcode.Split(';');
                if (from_split[0].ToString() != "")
                {
                    singleorgroup = " and group_code='" + Convert.ToString(from_split[0]).Trim() + "'";
                    strbatchsectionrights = "and user_id='" + Convert.ToString(from_split[0]).Trim() + "'";
                }
            }

            string strquery = "select distinct a.batch_year as appbatch,r.app_NO,r.degree_code as regdegree,r.batch_year as regbatch,a.degree_code as appdegree,r.isRedo,r.DelFlag,sections,r.Stud_Name,dg.college_code,r.current_semester as regsemester,a.current_semester as appsemester,roll_admit,roll_no,convert(varchar(15),adm_date,103) as adm_date,course_name,dg.acronym,r.App_No from registration r,course c,degree dg,applyn a where c.course_id=dg.course_id and r.degree_code=dg.degree_code  and exam_flag<>'debar'  " + field_val + " and r.App_No=a.app_no ";  //Modified By Mullai   and cc=0 and delflag=0


            // string strquery = "select distinct batch_year,r.degree_code,sections,stud_name,dg.college_code,current_semester,roll_admit,roll_no,convert(varchar(15),adm_date,103) as adm_date,course_name,dg.acronym from registration r,course c,degree dg where c.course_id=dg.course_id and r.degree_code=dg.degree_code and cc=0 and delflag=0 and exam_flag<>'debar' " + field_val + " ";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string studdegreecode = string.Empty;
                string stubatch = string.Empty;

                currentSemester = ddlSem.SelectedItem.Text;
                string regsem = Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim();
                string stusections = Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim();
                collegeCode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]).Trim();
                string regcursem = Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim();
                string appcurrsem = Convert.ToString(ds.Tables[0].Rows[0]["appsemester"]).Trim();
                string isredo1 = Convert.ToString(ds.Tables[0].Rows[0]["isRedo"]).Trim();
                string delflag = Convert.ToString(ds.Tables[0].Rows[0]["DelFlag"]).Trim();
                string roll = string.Empty;
                if (optionddl.Items[0].Selected == true)
                {
                    roll = "  roll_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
                }
                else if (optionddl.Items[1].Selected == true)
                {
                    roll = " reg_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
                }
                else
                {
                    roll = " roll_admit='" + Convert.ToString(txtrollno.Text).Trim() + "'";
                }
                string delQry = " select * from Readmission  where app_no in(select app_no from registration where " + roll + ")";

                DataTable dtdisC = dir.selectDataTable(delQry);
                if (dtdisC.Rows.Count > 0)
                {
                    delflag = "1";
                    studdegreecode = Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim();
                    stubatch = Convert.ToString(ds.Tables[0].Rows[0]["appbatch"]).Trim();
                    batchYear = Convert.ToString(ds.Tables[0].Rows[0]["appbatch"]).Trim();
                    degreeCode = Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim();
                }
                else
                {
                    delflag = "0";
                }
                if (isredo1.ToLower() == "true" || isredo1 == "1" || delflag == "1" || delflag.ToLower() == "true")
                {
                    studdegreecode = Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim();
                    stubatch = Convert.ToString(ds.Tables[0].Rows[0]["appbatch"]).Trim();
                    batchYear = Convert.ToString(ds.Tables[0].Rows[0]["appbatch"]).Trim();
                    degreeCode = Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim();

                }
                else
                {
                    studdegreecode = Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim();
                    stubatch = Convert.ToString(ds.Tables[0].Rows[0]["regbatch"]).Trim();
                    batchYear = Convert.ToString(ds.Tables[0].Rows[0]["regbatch"]).Trim();
                    degreeCode = Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim();
                }

                if (stusections == null || stusections.Trim() == "-1" || stusections.Trim() == "")
                {
                    stusections = string.Empty;
                }
                BindSem(collegeCode, batchYear, degreeCode, currentSemester);
                string degreerightsuser = d2.GetFunction("select degree_code from DeptPrivilages where degree_code='" + studdegreecode + "' " + singleorgroup + " ");
                if (degreerightsuser.Trim() == "" || degreerightsuser == null || degreerightsuser.Trim() == "0")
                {
                    errlbl.Text = "Please Update the Degree Rights";
                    errlbl.Visible = true;
                    return;
                }
                Hashtable hatsetrights = new Hashtable();
                string strbatchsectionsrights = "select sections from tbl_attendance_rights where batch_year='" + stubatch + "' " + strbatchsectionrights;
                DataSet dssections = d2.select_method_wo_parameter(strbatchsectionsrights, "Text");
                if (dssections.Tables[0].Rows.Count > 0)
                {
                    string strval = Convert.ToString(dssections.Tables[0].Rows[0]["sections"]).Trim();
                    string[] spsec = strval.Split(',');
                    for (int sp = 0; sp <= spsec.GetUpperBound(0); sp++)
                    {
                        string valu = spsec[sp].ToString().Trim();
                        if (!hatsetrights.Contains(valu))
                        {
                            hatsetrights.Add(valu, valu);
                        }
                    }
                }
                if (!hatsetrights.Contains(stusections))
                {
                    errlbl.Text = "Please Update the Batch Year and Sections Rights";
                    errlbl.Visible = true;
                    return;
                }
                lblname1.Visible = true;
                lblname2.Visible = true;
                stud_name = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]).Trim();
                lblname2.Text = stud_name;
                string admdate = Convert.ToString(ds.Tables[0].Rows[0]["adm_date"]).Trim();
                string[] admdatesp = admdate.Split(new Char[] { '/' });
                admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
                Admission_date = Convert.ToDateTime(admdate);
                Session["Admission_date"] = Admission_date;
                if (isredo1.ToLower() == "true" || isredo1 == "1")
                {

                    if (currentSemester == regsem)
                    {
                        divPopupAlert.Visible = true;
                        divAlertContent.Visible = true;
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "Redo Student.Which Record do you want?";
                        return;
                    }
                }
                if (delflag == "1" || delflag.ToLower() == "true")
                {
                    divPopupAlert.Visible = true;
                    divAlertContent.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Discontinued Student.Which Record do you want?";
                    return;
                }
                binddate();
            }
            else
            {
                errlbl.Visible = true;
                lblname1.Visible = false;
                lblname2.Visible = false;
                attnd_report.Visible = false;
                divNote.Visible = false;
                pageset_pnl.Visible = false;
                btnsave.Visible = false;
                lblFromdate.Visible = false;
                txtFromDate.Visible = false;
                lbltodate.Visible = false;
                txtToDate.Visible = false;
                btnGo.Visible = false;
                string strquery1 = "select isnull(count(*),0) as cnt  from registration r where cc=0 and delflag=0 and exam_flag<>'debar' " + field_val + " ";
                int studcnt = 0;
                studcnt = Convert.ToInt16(GetFunction(strquery1));
                if (studcnt >= 1)
                {
                    errlbl.Text = "You can not edit this student attendance due to security reasons.Please Contact InsproPlus Administrator";
                    return;
                }
                if (optionddl.Items.Count > 0)
                {
                    if (optionddl.Items[0].Selected == true)
                    {
                        errlbl.Text = "Invalid Roll Number";
                    }
                    else if (optionddl.Items[1].Selected == true)
                    {
                        errlbl.Text = "Invalid Reg Number";
                    }
                    else
                    {
                        errlbl.Text = "Invalid Admission Number";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public bool daycheck(DateTime seldate)
    {
        string collegecode = Session["collegecode"].ToString();
        bool daycheck = false;
        DateTime curdate, prevdate;
        long total, k, s;
        string[] ddate = new string[500];
        //DateTime[] ddate = new DateTime[500];
        //curdate == DateTime.Today.ToString() ;
        string c_date = DateTime.Today.ToString();
        DateTime todate_day = Convert.ToDateTime(DateTime.Today.ToString());
        curdate = DateTime.Today;
        if (seldate.ToString() == c_date)
        {
            daycheck = true;
            return daycheck;
        }
        else
        {
            DataSet ds_set = new DataSet();
            ds_set = d2.select_method_wo_parameter("select lockdays,lflag from collinfo where college_code='" + collegecode + "'", "Text");
            if (ds_set.Tables.Count > 0 && ds_set.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds_set.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToString(ds_set.Tables[0].Rows[i][1]).Trim().ToLower() == "true")
                    {
                        if (Convert.ToString(ds_set.Tables[0].Rows[i][0]) != null && int.Parse(Convert.ToString(ds_set.Tables[0].Rows[i][0])) >= 0)
                        {
                            total = int.Parse(Convert.ToString(ds_set.Tables[0].Rows[i][0]).Trim());
                            string isredo1 = Convert.ToString(ds.Tables[0].Rows[0]["isRedo"]).Trim();
                            if (isredo1 == "True")
                            {
                                ds1 = d2.select_method_wo_parameter("select holiday_date from holidaystudents where degree_code='" + Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim() + "'  and semester='" + ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim()) + "'", "Text");
                            }
                            else
                            {
                                ds1 = d2.select_method_wo_parameter("select holiday_date from holidaystudents where degree_code='" + Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim() + "'  and semester='" + ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim()) + "'", "Text");
                            }
                            if (ds1.Tables[0].Rows.Count <= 0)
                            {
                                for (int i1 = 1; i1 < total; i1++)
                                {
                                    string temp_date = todate_day.AddDays(-i1).ToString();
                                    string temp2 = todate_day.AddDays(i1).ToString();
                                    if (temp_date == seldate.ToString())
                                    {
                                        daycheck = true;
                                        return daycheck;
                                    }
                                    if (temp2 == seldate.ToString())
                                    {
                                        daycheck = true;
                                        return daycheck;
                                    }
                                }
                            }
                            else
                            {
                                k = 0;
                                for (int i1 = 1; i1 < ds1.Tables[0].Rows.Count; i1++)
                                {
                                    ddate[k] = Convert.ToString(ds1.Tables[0].Rows[i1][0]).Trim();
                                    k++;
                                }
                                i = 0;
                                while (i <= total - 1)
                                {
                                    string temp_date = curdate.AddDays(-i).ToString();
                                    for (s = 0; s < k - 1; s++)
                                    {
                                        if (temp_date == ddate[s].ToString())
                                        {
                                            total = total + 1;
                                            goto lab;
                                        }
                                    }
                                lab:
                                    i = i + 1;
                                    if (temp_date == seldate.ToString())
                                    {
                                        daycheck = true;
                                        return daycheck;
                                    }
                                }
                            }
                        }
                        else
                        {
                            daycheck = true;
                        }
                    }
                    else
                    {
                        daycheck = true;
                    }
                }
            }
        }
        return daycheck;
    }

    public void go()
    {
        if (!string.IsNullOrEmpty(Convert.ToString(txtrollno.Text).Trim()))
        {
            attnd_report.Sheets[0].RowCount = 0;
            attnd_report.CurrentPage = 0;
            loadstudent();
        }
        else
        {
            lblname1.Visible = false;
            lblname2.Visible = false;
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            txtFromDate.Visible = false;
            txtToDate.Visible = false;
            lblFromdate.Visible = false;
            lbltodate.Visible = false;
            btnGo.Visible = false;
            errlbl.Visible = true;
            string searchOption = string.Empty;
            errlbl.Text = "Enter Student " + optionddl.SelectedItem.Text;
        }
    }

    public void loadstudent()
    {
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        txtFromDate.Enabled = false;
        txtToDate.Enabled = false;
        txtFromDate.Visible = false;
        txtToDate.Visible = false;
        lblFromdate.Visible = false;
        lbltodate.Visible = false;
        btnGo.Visible = false;
        lblname1.Visible = false;
        lblname2.Visible = false;
        DateTime Admission_date;
        string field_val = string.Empty;
        string singleorgroup = string.Empty;
        string strbatchsectionrights = string.Empty;
        hat.Clear();
        if (optionddl.Items[0].Selected == true)
        {
            field_val = " and r.roll_no='" + txtrollno.Text.Trim().ToString() + "'";
        }
        else if (optionddl.Items[1].Selected == true)
        {
            field_val = " and r.reg_no='" + txtrollno.Text.Trim().ToString() + "'";
        }
        else
        {
            field_val = " and r.roll_admit='" + txtrollno.Text.Trim().ToString() + "'";
        }
        ds.Clear();
        if (Session["Single_User"].ToString() == "True")
        {
            singleorgroup = " and user_code='" + Session["UserCode"] + "'";
            strbatchsectionrights = "and user_id='" + Session["UserCode"].ToString() + "'";
        }
        else
        {
            string groupcode = Session["group_code"].ToString();
            string[] from_split = groupcode.Split(';');
            if (from_split[0].ToString() != "")
            {
                singleorgroup = " and group_code='" + from_split[0].ToString() + "'";
                strbatchsectionrights = "and user_id='" + from_split[0].ToString() + "'";
            }
        }
        string strquery = "select distinct a.batch_year as appbatch,r.degree_code as regdegree,r.batch_year as regbatch,a.degree_code as appdegree,r.isRedo,sections,r.Stud_Name,dg.college_code,r.current_semester as regsemester,a.current_semester as appsemester,roll_admit,roll_no,convert(varchar(15),adm_date,103) as adm_date,course_name,dg.acronym,DelFlag from registration r,course c,degree dg,applyn a where c.course_id=dg.course_id and r.degree_code=dg.degree_code   and exam_flag<>'debar' " + field_val + " and r.App_No=a.app_no";  //Modified By Mullai  and cc=0 and delflag=0


        // string strquery = "select distinct batch_year,r.degree_code,sections,stud_name,dg.college_code,current_semester,roll_admit,roll_no,convert(varchar(15),adm_date,103) as adm_date,course_name,dg.acronym from registration r,course c,degree dg where c.course_id=dg.course_id and r.degree_code=dg.degree_code and cc=0 and delflag=0 and exam_flag<>'debar' " + field_val + " ";
        ds.Dispose();
        ds.Reset();
        ds = d2.select_method_wo_parameter(strquery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            string studdegreecode = string.Empty;
            string stubatch = string.Empty;
            // string studdegreecode = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]).Trim();
            // string stubatch = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]).Trim();
            string stusections = Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim();
            collegeCode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]).Trim();
            string isredo1 = Convert.ToString(ds.Tables[0].Rows[0]["isRedo"]).Trim();
            if (isredo1 == "true")
            {
                studdegreecode = Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim();
                stubatch = Convert.ToString(ds.Tables[0].Rows[0]["appbatch"]).Trim();
                batchYear = Convert.ToString(ds.Tables[0].Rows[0]["appbatch"]).Trim();
                degreeCode = Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim();
                // currentSemester = Convert.ToString(ds.Tables[0].Rows[0]["current_semester"]).Trim();
            }
            else
            {
                studdegreecode = Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim();
                stubatch = Convert.ToString(ds.Tables[0].Rows[0]["regbatch"]).Trim();
                batchYear = Convert.ToString(ds.Tables[0].Rows[0]["regbatch"]).Trim();
                degreeCode = Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim();
                //currentSemester = Convert.ToString(ds.Tables[0].Rows[0]["current_semester"]).Trim();
            }

            //batchYear = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]).Trim();
            //degreeCode = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]).Trim();
            currentSemester = Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim();
            if (stusections == null || stusections.Trim() == "-1" || stusections.Trim() == "")
            {
                stusections = string.Empty;
            }
            BindSem(collegeCode, batchYear, degreeCode, currentSemester);
            string degreerightsuser = d2.GetFunction("select degree_code from DeptPrivilages where degree_code='" + studdegreecode + "' " + singleorgroup + " ");
            if (degreerightsuser.Trim() == "" || degreerightsuser == null || degreerightsuser.Trim() == "0")
            {
                errlbl.Text = "Please Update the Degree Rights";
                errlbl.Visible = true;
                return;
            }
            Hashtable hatsetrights = new Hashtable();
            string strbatchsectionsrights = "select sections from tbl_attendance_rights where batch_year='" + stubatch + "' " + strbatchsectionrights;
            DataSet dssections = d2.select_method_wo_parameter(strbatchsectionsrights, "Text");
            if (dssections.Tables[0].Rows.Count > 0)
            {
                string strval = Convert.ToString(dssections.Tables[0].Rows[0]["sections"]).Trim();
                string[] spsec = strval.Split(',');
                for (int sp = 0; sp <= spsec.GetUpperBound(0); sp++)
                {
                    string valu = spsec[sp].ToString().Trim();
                    if (!hatsetrights.Contains(valu))
                    {
                        hatsetrights.Add(valu, valu);
                    }
                }
            }
            if (!hatsetrights.Contains(stusections))
            {
                errlbl.Text = "Please Update the Batch Year and Sections Rights";
                errlbl.Visible = true;
                return;
            }
            lblname1.Visible = true;
            lblname2.Visible = true;
            stud_name = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]).Trim();
            lblname2.Text = stud_name;
            string admdate = Convert.ToString(ds.Tables[0].Rows[0]["adm_date"]).Trim();
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
            Admission_date = Convert.ToDateTime(admdate);
            Session["Admission_date"] = Admission_date;
            string regsem = ddlSem.SelectedItem.Text;
            if (isredo1 == "True")
            {

                if (currentSemester == regsem)
                {
                    divPopupAlert.Visible = true;
                    divAlertContent.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Redo Student. Which Record do you want?";
                    return;
                }
            }
            binddate();
        }
        else
        {
            errlbl.Visible = true;
            lblname1.Visible = false;
            lblname2.Visible = false;
            attnd_report.Visible = false;
            divNote.Visible = false;
            pageset_pnl.Visible = false;
            btnsave.Visible = false;
            lblFromdate.Visible = false;
            txtFromDate.Visible = false;
            lbltodate.Visible = false;
            txtToDate.Visible = false;
            btnGo.Visible = false;
            string strquery1 = "select isnull(count(*),0) as cnt  from registration r where cc=0 and delflag=0 and exam_flag<>'debar' " + field_val + " ";
            int studcnt = 0;
            studcnt = Convert.ToInt16(GetFunction(strquery1));
            if (studcnt >= 1)
            {
                errlbl.Text = "You can not edit this student attendance due to security reasons.Please Contact InsproPlus Administrator";
                return;
            }
            if (optionddl.Items.Count > 0)
            {
                if (optionddl.Items[0].Selected == true)
                {
                    errlbl.Text = "Invalid Roll Number";
                }
                else if (optionddl.Items[1].Selected == true)
                {
                    errlbl.Text = "Invalid Reg Number";
                }
                else
                {
                    errlbl.Text = "Invalid Admission Number";
                }
            }
        }
    }

    public void getdate()
    {
        if (optionddl.Items[0].Selected == true)
        {
            get_roll_no_r = "r.roll_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
        }
        else if (optionddl.Items[1].Selected == true)
        {
            get_roll_no_r = "r.reg_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
        }
        else
        {
            get_roll_no_r = "r.roll_admit='" + Convert.ToString(txtrollno.Text).Trim() + "'";//modified by Srinath 6/5/2013
        }
        ds.Dispose();
        ds.Reset();
        string singleorgroup = string.Empty;
        string field_val = string.Empty;
        if (Session["Single_User"] != null && (Convert.ToString(Session["Single_User"]).Trim().ToLower() == "true" || Convert.ToString(Session["Single_User"]).Trim().ToLower() == "1"))
        {
            singleorgroup = " and d.user_code='" + Convert.ToString(Session["UserCode"]).Trim() + "'";
        }
        else if (Session["group_code"] != null)
        {
            string groupcode = Convert.ToString(Session["group_code"]).Trim();
            string[] from_split = groupcode.Split(';');
            if (Convert.ToString(from_split[0]).Trim() != "")
                singleorgroup = " and d.group_code='" + Convert.ToString(from_split[0]).Trim() + "'";
        }
        if (optionddl.Items[0].Selected == true)
        {
            field_val = " and r.roll_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
        }
        else if (optionddl.Items[1].Selected == true)
        {
            field_val = " and r.reg_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
        }
        else
        {
            field_val = " and r.roll_admit='" + Convert.ToString(txtrollno.Text).Trim() + "'";
        }
        string strquery1 = "select isnull(count(*),0) as cnt  from registration r where cc=0 and delflag=0 and exam_flag='debar' " + field_val + " ";
        int studcnt = 0;
        studcnt = Convert.ToInt16(GetFunction(strquery1));
        if (studcnt >= 1)
        {
            errlbl.Text = "You can not edit this student attendance due to security reasons.Please Contact InsproPlus Administrator";
            return;
        }
        string strquery = "select distinct a.batch_year as appbatch,r.app_no,r.degree_code as regdegree,r.batch_year as regbatch,a.degree_code as appdegree,r.isRedo,r.Delflag,sections,r.stud_name,dg.college_code,r.current_semester as regsemester,a.current_semester as appsemester,roll_admit,roll_no,convert(varchar(15),adm_date,103) as adm_date,course_name,dg.acronym,r.college_code from registration r,deptprivilages d ,course c,degree dg,applyn a where c.course_id=dg.course_id and r.degree_code=dg.degree_code  and exam_flag<>'debar' and r.degree_code=d.degree_code   " + singleorgroup + " " + field_val + " and r.App_No=a.app_no";  //Modified By Mullai  and cc=0  and delflag=0

        // string strquery = "select distinct batch_year,r.degree_code,sections,stud_name,dg.college_code,current_semester,roll_admit,roll_no,convert(varchar(15),adm_date,103) as adm_date,course_name,dg.acronym,r.college_code from registration r,deptprivilages d ,course c,degree dg where c.course_id=dg.course_id and r.degree_code=dg.degree_code and cc=0 and delflag=0 and exam_flag<>'debar' and r.degree_code=d.degree_code  " + singleorgroup + " " + field_val + " ";
        string roll = string.Empty;
        if (optionddl.Items[0].Selected == true)
        {
            roll = "  r.roll_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
        }
        else if (optionddl.Items[1].Selected == true)
        {
            roll = "  r.reg_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
        }
        else
        {
            roll = "  r.roll_admit='" + Convert.ToString(txtrollno.Text).Trim() + "'";
        }
        ds.Clear();
        ds = dacces2.select_method(strquery, hat, "Text");
        ds1.Clear();
        hat.Clear();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            string isredo1 = Convert.ToString(ds.Tables[0].Rows[0]["isRedo"]).Trim();
            string delflag = Convert.ToString(ds.Tables[0].Rows[0]["Delflag"]).Trim();
            string delQry = " select * from Readmission  where app_no in(select app_no from registration r where  " + roll + ")";
            DataTable dtdisC = dir.selectDataTable(delQry);
            if (dtdisC.Rows.Count > 0)
            {
                delflag = "1";
            }
            else
            {
                delflag = "0";
            }

            if (isredo1 == "True" || delflag == "1")
            {

                currentSemester = ddlSem.SelectedItem.Text;
                string regsem = Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim();

                if (isredoold == "1")
                {

                    hat.Add("batch", Convert.ToString(ds.Tables[0].Rows[0]["regbatch"]).Trim());
                    hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim());

                    hat.Add("sem", ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim()));
                }
                else
                {
                    hat.Add("batch", Convert.ToString(ds.Tables[0].Rows[0]["appbatch"]).Trim());
                    hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim());

                    hat.Add("sem", ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim()));
                }
            }
            else
            {
                hat.Add("batch", Convert.ToString(ds.Tables[0].Rows[0]["regbatch"]).Trim());
                hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim());
                //hat.Add("sem", ds.Tables[0].Rows[0]["current_semester"].ToString());
                hat.Add("sem", ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim()));
            }
            ds1 = dacces2.select_method("sem_info", hat, "sp");//-----------------------get dept sem date
        }
        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
        {
            loadattendanceNew();//======================================load attendance function
        }


    }

    public void loadattendanceOLD()
    {
        DateTime Admission_date = new DateTime();
        Admission_date = Convert.ToDateTime(Session["Admission_date"]);
        attnd_report.Sheets[0].SheetName = " ";
        attnd_report.Sheets[0].RowCount = 0;
        attnd_report.Sheets[0].ColumnCount = 0;
        attnd_report.Sheets[0].AutoPostBack = false;
        strcomo1 = new string[] { "Select for All ", " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NJ", "S", "NCC", "HS", "PP", "SYOD", "COD", "OOD", "LA", "RAA" };
        strcomo = new string[] { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NJ", "S", "NCC", "HS", "PP", "SYOD", "COD", "OOD", "LA", "RAA" };
        objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
        objintcell.ShowButton = true;
        objintcell.AutoPostBack = true;
        objintcell.UseValue = true;
        objintcell.BackColor = Color.Gold;
        objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);
        // objcom.ShowButton = true; 
        objcom.BackColor = Color.DarkSeaGreen;
        // objcom.AutoPostBack = true ;
        objcom.UseValue = true;
        ds3.Clear();
        hat.Clear();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            string isredo1 = Convert.ToString(ds.Tables[0].Rows[0]["isRedo"]).Trim();
            if (isredo1 == "True")
            {
                hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim());
            }
            else
            {
                hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim());
            }
            hat.Add("sem_val", ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim()));
            string sections = string.Empty;
            string strsec = string.Empty;
            string coursename = Convert.ToString(ds.Tables[0].Rows[0]["course_name"]).Trim();
            string deptname = Convert.ToString(ds.Tables[0].Rows[0]["acronym"]).Trim();
            string batchyear = Convert.ToString(batchYear).Trim();
            if (Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() != "-1")
            {
                sections = Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim();
                strsec = "Sec" + sections;
            }
            string studentname = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]).Trim();
            string sem = ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim());
            //Modified By Srinath 25/4/2013
            // string rollno = ds.Tables[0].Rows[0]["roll_admit"].ToString();
            string rollno = Convert.ToString(ds.Tables[0].Rows[0]["Roll_no"]).Trim();
            DataSet infods = new DataSet();
            string infoquery = " select r.stud_name,c.Course_Name,dpt.dept_acronym,dpt.Dept_Name,r.Sections,r.Current_Semester as year  from Registration r,Degree d,Department dpt,Course c where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dpt.Dept_Code and r.Roll_No='" + rollno + "' order by r.Current_Semester desc ";
            infods = d2.select_method_wo_parameter(infoquery, "Text");
            ds3 = dacces2.select_method("period_attnd_schedule_sp", hat, "sp");
            if (ds.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                nohrs = int.Parse(Convert.ToString(ds3.Tables[0].Rows[0]["no_of_hrs_per_day"]).Trim());
                first_half = int.Parse(Convert.ToString(ds3.Tables[0].Rows[0]["no_of_hrs_I_half_day"]).Trim());
                second_half = int.Parse(Convert.ToString(ds3.Tables[0].Rows[0]["no_of_hrs_II_half_day"]).Trim());
                Session["nohrs"] = nohrs;
                if (nohrs != null)
                {
                    attnd_report.Sheets[0].ColumnCount = nohrs + 1;
                    attnd_report.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
                    attnd_report.Sheets[0].Columns[0].Width = 100;
                    for (int i = 1; i <= (nohrs); i++)
                    {
                        attnd_report.Sheets[0].Columns[i].Width = 35;
                    }
                    attnd_report.Sheets[0].RowCount = 1;
                    attnd_report.Width = (50 * nohrs) + 670;
                    attnd_report.Sheets[0].Columns[0].BackColor = Color.Gray;
                    attnd_report.Sheets[0].RowHeader.Cells[0, 0].Text = " ";
                    attnd_report.Sheets[0].ColumnHeader.RowCount = 2;
                    attnd_report.Sheets[0].ColumnHeader.Columns.Count = 1;
                    attnd_report.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Left;
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, nohrs);
                    attnd_report.Sheets[0].ColumnHeader.Cells[0, 0].Text = "         " + "Degree:" + batchyear + "-" + coursename + "[" + deptname + "]" + "-" + "Sem  " + sem + "    " + strsec + "  RollNo:" + rollno + "  Name :" + studentname;
                    if (forschoolsetting == true)
                    {
                        attnd_report.Sheets[0].ColumnHeader.Cells[0, 0].Text = "         " + "Standard:" + batchyear + "-" + coursename + "[" + deptname + "]" + "-" + "Term  " + sem + "    " + strsec + "  RollNo:" + rollno + "  Name :" + studentname;
                    }
                    attnd_report.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
                    attnd_report.Sheets[0].ColumnHeader.Cells[1, 0].VerticalAlign = VerticalAlign.Middle;
                    attnd_report.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Date";
                    for (int i = 1; i <= nohrs; i++)
                    {
                        attnd_report.Sheets[0].ColumnHeader.Columns.Count++;
                        attnd_report.Sheets[0].ColumnHeader.Cells[1, i].Text = i.ToString();
                        attnd_report.ActiveSheetView.Columns[i].CellType = objcom;
                        attnd_report.Sheets[0].Cells[0, i].CellType = objintcell;
                        // attnd_report.Sheets[0].Cells[0, 0].Locked = true;
                    }
                    attnd_report.Sheets[0].ColumnHeader.Columns.Count++;
                    attnd_report.Sheets[0].ColumnHeader.Cells[1, nohrs + 1].Text = "Attendance Status";
                    attnd_report.Sheets[0].Columns[nohrs + 1].BackColor = Color.Gray;
                    attnd_report.Sheets[0].Columns[nohrs + 1].Width = 170;
                    attnd_report.Sheets[0].Columns[0].Locked = true;
                    attnd_report.Sheets[0].Columns[nohrs + 1].Locked = true;
                    string frdate_datetime = string.Empty;
                    string todate_datetime = string.Empty;
                    string[] from_split2 = (txtFromDate.Text).Split('/');
                    string[] to_split2 = (txtToDate.Text).Split('/');
                    if (from_split2.Length > 0)
                    {
                        fd = Convert.ToInt16(Convert.ToString(from_split2[0]).Trim());
                        fm = Convert.ToInt16(Convert.ToString(from_split2[1]).Trim());
                        fyy = Convert.ToInt16(Convert.ToString(from_split2[2]).Trim());
                        Session["fd"] = fd;
                        Session["fyy"] = fyy;
                    }
                    if (to_split2.Length > 0)
                    {
                        td = Convert.ToInt16(Convert.ToString(to_split2[0]).Trim());
                        tm = Convert.ToInt16(Convert.ToString(to_split2[1]).Trim());
                        tyy = Convert.ToInt16(Convert.ToString(to_split2[2]).Trim());
                        Session["td"] = td;
                    }
                    fcal = ((fyy * 12) + fm);
                    Session["fcal"] = fcal;
                    tcal = ((tyy * 12) + tm);
                    Session["tcal"] = tcal;
                    string get_rollno = GetFunction("select r.roll_no from registration r where " + get_roll_no_r + "");
                    string degcode = d2.GetFunction("select degree_code from registration  where Roll_No='" + get_rollno + "'");
                    string semester = d2.GetFunction("select Current_Semester from registration  where Roll_No ='" + get_rollno + "'");
                    string section = d2.GetFunction("select Sections from registration  where Roll_No ='" + get_rollno + "'");
                    hat.Clear();
                    hat.Add("degree_code", degcode);
                    hat.Add("sem_ester", int.Parse(((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(semester).Trim())));
                    ds5 = d2.select_method("period_attnd_schedule", hat, "sp");
                    ds2.Clear();
                    hat.Clear();
                    hat.Add("f_date", fcal);
                    hat.Add("t_date", tcal);
                    hat.Add("roll_no", Convert.ToString(get_rollno).Trim());
                    ds2 = dacces2.select_method("ATT_REPORTS_DETAILS", hat, "sp");
                    hat.Clear();
                    hat.Add("colege_code", Convert.ToString(Session["collegecode"]).Trim());
                    DataSet dsattva = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                    Dictionary<string, string> dicattval = new Dictionary<string, string>();
                    if (dsattva.Tables.Count > 0 && dsattva.Tables[0].Rows.Count > 0)
                    {
                        for (int at = 0; at < dsattva.Tables[0].Rows.Count; at++)
                        {
                            string leavcode = Convert.ToString(dsattva.Tables[0].Rows[at]["leavecode"]).Trim();
                            string calc = Convert.ToString(dsattva.Tables[0].Rows[at]["calcflag"]).Trim();
                            if (!dicattval.ContainsKey(leavcode.Trim()))
                            {
                                dicattval.Add(leavcode.Trim(), calc);
                            }
                        }
                    }
                    int NoHrs = 0;
                    int fnhrs = 0;
                    int anhrs = 0;
                    int minpresI = 0;
                    int minpresII = 0;
                    int minpresday = 0;
                    double prsentdays = 0;
                    double absentdays = 0;
                    double conducteddays = 0;
                    double oddays = 0;
                    double leavedays = 0;
                    int fuldayabsentcount = 0;
                    int halfdayabsentcount = 0;
                    double nohrsprsentperday = 0;
                    double noofdaypresen = 0;
                    if (ds5.Tables.Count > 0 && ds5.Tables[0].Rows.Count > 0)
                    {
                        NoHrs = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["PER DAY"]).Trim());
                        fnhrs = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["I_HALF_DAY"]).Trim());
                        anhrs = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["II_HALF_DAY"]).Trim());
                        minpresI = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["MIN PREE I DAY"]).Trim());
                        minpresII = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["MIN PREE II DAY"]).Trim());
                        minpresday = int.Parse(ds5.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
                    }
                    {
                        totmonth = fcal;
                        fflag = true;
                        int i = 0;
                        double k = 1;
                        dat = fd;
                        int k_temp = 0;
                        //int fulldayabsent = 0;
                        //int halfdayabsent = 0;
                        //int leaveapplycount = 0;
                        //int leaveappl = 0;
                        //double odappl = 0;
                        //int conducteddays = 0;
                        //int fpresent = 0;
                        //int hpresent = 0;
                        string latd = string.Empty;
                        dummydate = Convert.ToDateTime(Convert.ToString(dt1.ToShortDateString()).Trim());
                        Session["start_date"] = dummydate;
                        int morningConductedHrs = 0;
                        int eveningConductedHrs = 0;
                        int unMarkedAttendance = 0;
                        int notConsidered = 0;
                        double totalPresentHours = 0;
                        double notJoinDays = 0;
                        double totalWorkingDays = 0;
                        for (int cumd = fcal; cumd <= tcal; cumd++)
                        {
                            totpresentday = 0;
                            if (cumd == tcal)
                            {
                                cal_date(cumd);
                                if (fd == td)
                                {
                                    totpresentday += 1;
                                }
                                else if (td == daycount)
                                {
                                    totpresentday += daycount;
                                    balamonday = daycount;
                                }
                                else
                                {
                                    totpresentday += td - (fd - 1);
                                    balamonday = fd - (td);
                                }
                            }
                            if (cumd != tcal)
                            {
                                cal_date(cumd);
                                totpresentday += daycount;
                                balamonday = daycount;
                            }
                            //------------find start date
                            if (cumd == fcal)
                            {
                                k_temp = fd;
                            }
                            else
                            {
                                k_temp = 1;
                            }
                            if (cumd == tcal)
                            {
                                endk = td;
                            }
                            else
                            {
                                endk = totpresentday;
                            }
                            {
                                for (k = k_temp; k <= endk; k++)
                                {
                                    int forcount = 0;
                                    int pct = 0, act = 0, odct = 0, mlct = 0, sodct = 0, nssct = 0, hct = 0, njct = 0, sct = 0, lct = 0, nccct = 0, hsct = 0, ppct = 0, syodct = 0, codct = 0, oodct = 0, lact = 0, nect = 0, raact = 0;
                                    if (dummydate >= Admission_date)
                                    {
                                        double present = 0;
                                        int noofmorpresent = 0;
                                        int noofmorabsent = 0;
                                        int noofmornj = 0;
                                        int noofevepresent = 0;
                                        int noofeveabsent = 0;
                                        int noofevenj = 0;
                                        int noofmorcon = 0;
                                        int noofevecon = 0;
                                        int noofmorod = 0, noofmorleav = 0, noofeveod = 0, noofeveleav = 0;
                                        ddd = dummydate.ToString("ddd");
                                        find_holiday(dummydate);
                                        if (holidayflag == false)
                                        {
                                            //----------------get date for display in spread
                                            string dummy_date = string.Empty;
                                            string date_text = string.Empty;
                                            dummy_date = dummydate.ToString();
                                            string[] dummy_split = dummy_date.Split(' ');
                                            string[] dummy_split2 = dummy_split[0].Split('/');
                                            //----------------------------set date
                                            attnd_report.Sheets[0].RowCount++;
                                            attnd_report.Sheets[0].RowHeader.Cells[attnd_report.Sheets[0].RowCount - 1, 0].Text = (attnd_report.Sheets[0].RowCount - 1).ToString();
                                            //-----------------------lock row for attnd security
                                            chk = daycheck(Convert.ToDateTime(dummydate));
                                            if (chk == false)
                                            {
                                                attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Locked = true;
                                            }
                                            date_text = Convert.ToString(dummy_split2[1]).Trim() + "/" + Convert.ToString(dummy_split2[0]).Trim() + "/" + Convert.ToString(dummy_split2[2]).Trim();
                                            attnd_report.Sheets[0].SetValue((attnd_report.Sheets[0].RowCount - 1), 0, Convert.ToString(date_text).Trim());
                                            attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dummy_split[0]).Trim();
                                            if (ds2.Tables.Count > 0 && i < ds2.Tables[0].Rows.Count)
                                            {
                                                if (cumd == Convert.ToInt16(Convert.ToString(ds2.Tables[0].Rows[i]["month_year"]).Trim()))
                                                {
                                                    int mfc = 0;
                                                    int msc = 0;
                                                    int lfc = 0;
                                                    int lsc = 0;
                                                    int pfc = 0;
                                                    int psc = 0;
                                                    int emptyfc = 0;
                                                    int emptysc = 0;
                                                    int unMark1 = 0;
                                                    int unMark2 = 0;
                                                    if (split_holiday_status_1 == 1 && split_holiday_status_2 == NoHrs)
                                                    {
                                                        morningConductedHrs += 1;
                                                        eveningConductedHrs += 1;
                                                    }
                                                    else if (split_holiday_status_1 == fnhrs + 1 && split_holiday_status_2 == NoHrs)
                                                    {
                                                        eveningConductedHrs += 1;
                                                    }
                                                    else if (split_holiday_status_1 == 1 && split_holiday_status_2 == fnhrs)
                                                    {
                                                        morningConductedHrs += 1;
                                                    }
                                                    for (int temp = split_holiday_status_1; temp <= split_holiday_status_2; temp++)
                                                    {
                                                        forcount++;
                                                        temp_val = "d" + dummy_split2[1] + "d" + temp;
                                                        if (ds2.Tables[0].Rows.Count > 0)
                                                        {
                                                            if (i < ds2.Tables[0].Rows.Count)
                                                            {
                                                                string valueAtt = Convert.ToString(ds2.Tables[0].Rows[i][temp_val]).Trim();
                                                                Att_mark = string.Empty;
                                                                if (!string.IsNullOrEmpty(valueAtt) && valueAtt != "" && valueAtt != "0" && valueAtt != "7")
                                                                {
                                                                    leave_code = (Convert.ToString(ds2.Tables[0].Rows[i][temp_val]).Trim());
                                                                    Attmark(leave_code);
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].Text = Att_mark;
                                                                    string clav = string.Empty;
                                                                    if (dicattval.ContainsKey(leave_code.Trim()))
                                                                    {
                                                                        clav = Convert.ToString(dicattval[leave_code.Trim()]).Trim();
                                                                    }
                                                                    if (temp <= fnhrs)
                                                                    {
                                                                        if (leave_code == "3")
                                                                        {
                                                                            noofmorod++;
                                                                        }
                                                                        if (leave_code == "10")
                                                                        {
                                                                            noofmorleav++;
                                                                        }
                                                                        if (clav == "0")
                                                                        {
                                                                            noofmorpresent++;
                                                                            noofmorcon++;
                                                                            nohrsprsentperday++;
                                                                            totalPresentHours++;
                                                                        }
                                                                        else if (clav == "1")
                                                                        {
                                                                            noofmorabsent++;
                                                                            noofmorcon++;
                                                                        }
                                                                        else if (clav == "2")
                                                                        {
                                                                            noofmornj++;
                                                                            noofmorcon++;
                                                                            notConsidered++;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (clav == "0")
                                                                        {
                                                                            noofevepresent++;
                                                                            noofevecon++;
                                                                            nohrsprsentperday++;
                                                                            totalPresentHours++;
                                                                        }
                                                                        else if (clav == "1")
                                                                        {
                                                                            noofeveabsent++;
                                                                            noofevecon++;
                                                                        }
                                                                        else if (clav == "2")
                                                                        {
                                                                            noofevenj++;
                                                                            noofevecon++;
                                                                            notConsidered++;
                                                                        }
                                                                        if (leave_code == "3")
                                                                        {
                                                                            noofeveod++;
                                                                        }
                                                                        if (leave_code == "10")
                                                                        {
                                                                            noofeveleav++;
                                                                        }
                                                                    }
                                                                    if (temp <= fnhrs)
                                                                    {
                                                                        if (Att_mark == "A")
                                                                        {
                                                                            mfc++;
                                                                        }
                                                                    }
                                                                    else if (temp <= NoHrs)
                                                                    {
                                                                        if (Att_mark == "A")
                                                                        {
                                                                            msc++;
                                                                        }
                                                                    }
                                                                    if (temp <= fnhrs)
                                                                    {
                                                                        if (Att_mark == "L")
                                                                        {
                                                                            lfc++;
                                                                        }
                                                                    }
                                                                    else if (temp <= NoHrs)
                                                                    {
                                                                        if (Att_mark == "L")
                                                                        {
                                                                            lsc++;
                                                                        }
                                                                    }
                                                                    if (temp <= fnhrs)
                                                                    {
                                                                        if (Att_mark == "P")
                                                                        {
                                                                            pfc++;
                                                                        }
                                                                    }
                                                                    else if (temp <= NoHrs)
                                                                    {
                                                                        if (Att_mark == "P")
                                                                        {
                                                                            psc++;
                                                                        }
                                                                    }
                                                                    if (Att_mark == "A")
                                                                    {
                                                                        act++;
                                                                    }
                                                                    else if (Att_mark == "P")
                                                                    {
                                                                        pct++;
                                                                    }
                                                                    else if (Att_mark == "L")
                                                                    {
                                                                        lct++;
                                                                    }
                                                                    else if (Att_mark == "H")
                                                                    {
                                                                        hct++;
                                                                    }
                                                                    else if (Att_mark == "OD")
                                                                    {
                                                                        odct++;
                                                                    }
                                                                    else if (Att_mark == "ML")
                                                                    {
                                                                        mlct++;
                                                                    }
                                                                    else if (Att_mark == "SOD")
                                                                    {
                                                                        sodct++;
                                                                    }
                                                                    else if (Att_mark == "NSS")
                                                                    {
                                                                        nssct++;
                                                                    }
                                                                    else if (Att_mark == "NJ")
                                                                    {
                                                                        njct++;
                                                                    }
                                                                    else if (Att_mark == "S")
                                                                    {
                                                                        sct++;
                                                                    }
                                                                    else if (Att_mark == "NCC")
                                                                    {
                                                                        nccct++;
                                                                    }
                                                                    else if (Att_mark == "HS")
                                                                    {
                                                                        hsct++;
                                                                    }
                                                                    else if (Att_mark == "PP")
                                                                    {
                                                                        ppct++;
                                                                    }
                                                                    else if (Att_mark == "SYOD")
                                                                    {
                                                                        syodct++;
                                                                    }
                                                                    else if (Att_mark == "COD")
                                                                    {
                                                                        codct++;
                                                                    }
                                                                    else if (Att_mark == "OOD")
                                                                    {
                                                                        oodct++;
                                                                    }
                                                                    else if (Att_mark == "LA")
                                                                    {
                                                                        lact++;
                                                                    }
                                                                    else if (Att_mark == "NE")
                                                                    {
                                                                        nect++;
                                                                    }
                                                                    else if (Att_mark == "RAA")
                                                                    {
                                                                        raact++;
                                                                    }
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].Tag = leave_code;
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.DarkSeaGreen;
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.White;
                                                                    btnsave.Visible = true;
                                                                }
                                                                else
                                                                {
                                                                    unMarkedAttendance++;
                                                                    if (temp <= fnhrs)
                                                                    {
                                                                        unMark1++;
                                                                        if (Att_mark == "")
                                                                        {
                                                                            emptyfc++;
                                                                        }
                                                                    }
                                                                    else if (temp <= NoHrs)
                                                                    {
                                                                        unMark2++;
                                                                        if (Att_mark == "")
                                                                        {
                                                                            emptysc++;
                                                                        }
                                                                    }
                                                                }
                                                                if (Att_mark == "P")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.Green;
                                                                    latd = dummydate.ToString();
                                                                }
                                                                else if (Att_mark == "A")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.Red;
                                                                }
                                                                else if (Att_mark == "H")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.Maroon;
                                                                }
                                                                else if (Att_mark == "OD")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.Blue;
                                                                }
                                                                else if (Att_mark == "SOD")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.GreenYellow;
                                                                }
                                                                else if (Att_mark == "ML")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.DarkSalmon;
                                                                }
                                                                else if (Att_mark == "NSS")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.Goldenrod;
                                                                }
                                                                else if (Att_mark == "L")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#0080ff");
                                                                }
                                                                else if (Att_mark == "NCC")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.Violet;
                                                                }
                                                                else if (Att_mark == "HS")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.SlateGray;
                                                                }
                                                                else if (Att_mark == "PP")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.Pink;
                                                                }
                                                                else if (Att_mark == "SYOD")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.LimeGreen;
                                                                }
                                                                else if (Att_mark == "COD")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.Tan;
                                                                }
                                                                else if (Att_mark == "OOD")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.Wheat;
                                                                }
                                                                else if (Att_mark == "NJ")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.SaddleBrown;
                                                                }
                                                                else if (Att_mark == "S")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.Black;
                                                                }
                                                                else if (Att_mark == "RAA")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.Yellow;
                                                                }
                                                                else if (Att_mark == "")
                                                                {
                                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.MediumSlateBlue;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.DarkSeaGreen;
                                                            }
                                                        }
                                                    }
                                                    #region Hide
                                                    //if (ds5.Tables[0].Rows.Count > 0)
                                                    //{
                                                    //    NoHrs = int.Parse(ds5.Tables[0].Rows[0]["PER DAY"].ToString());
                                                    //    fnhrs = int.Parse(ds5.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                                    //    anhrs = int.Parse(ds5.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                                    //    minpresI = int.Parse(ds5.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                                    //    minpresII = int.Parse(ds5.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                                    //}
                                                    //int cond1 = minpresI + minpresII;
                                                    //int test1 = fnhrs - minpresI;
                                                    //int test2 = anhrs - minpresII;
                                                    //string attendance =string.Empty;
                                                    ////bool freehr = isApplicableForFreeSpecial(Session["collegecode"].ToString(), batchyear, degcode, semester, section, dummydate.ToString("MM/dd/yyyy"), "0");
                                                    ////bool specialday = isApplicableForFreeSpecial(Session["collegecode"].ToString(), batchyear, degcode, semester, section, dummydate.ToString("MM/dd/yyyy"), "1");
                                                    //if (cond1 <= act)
                                                    //{
                                                    //    attendance = "FA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //    fulldayabsent++;
                                                    //}
                                                    //else if (pct >= 1 && pct < cond1)
                                                    //{
                                                    //    //if (pfc >= 1 && psc >= 1)
                                                    //    //{
                                                    //    //    attendance = "FP";
                                                    //    //}
                                                    //    //else
                                                    //    //{
                                                    //    //    attendance = "HP";
                                                    //    //}
                                                    //    if (mfc > test1 && msc > test2)
                                                    //    {
                                                    //        attendance = "FA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //        fulldayabsent++;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        if (mfc == 0 && msc == 0 && pfc > test1 && psc > test2)
                                                    //        {
                                                    //            if ((emptyfc > test1 && emptysc > test2))
                                                    //            {
                                                    //                attendance = "FA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //                fulldayabsent++;
                                                    //            }
                                                    //            else if ((emptyfc > test1 || emptysc > test2))
                                                    //            {
                                                    //                attendance = "HA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                halfdayabsent++;
                                                    //            }
                                                    //            else if ((pfc <= fnhrs || psc <= anhrs))
                                                    //            {
                                                    //                attendance = "HA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                halfdayabsent++;
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                attendance = "FP";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //                fpresent++;
                                                    //            }
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            if ((pfc == fnhrs || psc == anhrs) && odct >= 1)
                                                    //            {
                                                    //                attendance = "FP";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //                fpresent++;
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                if ((emptyfc > test1 || emptysc > test2) && (mfc > test1 || msc > test2))
                                                    //                {
                                                    //                    attendance = "FA";
                                                    //                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //                    fulldayabsent++;
                                                    //                }
                                                    //                else if ((emptyfc > test1 && emptysc > test2) && (pfc > test1 || psc > test2))
                                                    //                {
                                                    //                    attendance = "FA";
                                                    //                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //                    fulldayabsent++;
                                                    //                }
                                                    //                else
                                                    //                {
                                                    //                    attendance = "HA";
                                                    //                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                    halfdayabsent++;
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                    //else if (pct > test1 && pct == cond1)
                                                    //{
                                                    //    attendance = "FP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //    fpresent++;
                                                    //}
                                                    //else if (odct >= 1 && odct < cond1)
                                                    //{
                                                    //    if ((mfc > test1 || msc > test2) && odct >= 1)
                                                    //    {
                                                    //        //attendance = "OD";
                                                    //        attendance = "HA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Blue;
                                                    //        odappl = odappl + 0.5;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        //attendance = "OD";
                                                    //        attendance = "FP";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Blue;
                                                    //        odappl++;
                                                    //    }
                                                    //}
                                                    //else if (odct >= 1 && odct == cond1)
                                                    //{
                                                    //    //attendance = "OD";
                                                    //    attendance = "FP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Blue;
                                                    //    odappl++;
                                                    //}
                                                    //else if (mlct >= 1 && mlct < cond1)
                                                    //{
                                                    //    attendance = "HML";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //}
                                                    //else if (mlct >= 1 && mlct == cond1)
                                                    //{
                                                    //    attendance = "FML";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //}
                                                    //else if (sodct >= 1 && sodct < cond1)
                                                    //{
                                                    //    attendance = "HSOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.GreenYellow;
                                                    //}
                                                    //else if (sodct >= 1 && sodct == cond1)
                                                    //{
                                                    //    attendance = "FSOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.GreenYellow;
                                                    //}
                                                    //else if (nssct >= 1 && nssct < cond1)
                                                    //{
                                                    //    attendance = "HNSS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Goldenrod;
                                                    //}
                                                    //else if (nssct >= 1 && nssct == cond1)
                                                    //{
                                                    //    attendance = "FNSS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Goldenrod;
                                                    //}
                                                    //else if (njct >= 1 && njct < cond1)
                                                    //{
                                                    //    attendance = "HNJ";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SaddleBrown;
                                                    //}
                                                    //else if (njct >= 1 && njct == cond1)
                                                    //{
                                                    //    attendance = "FNJ";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SaddleBrown;
                                                    //}
                                                    //else if (sct >= 1 && sct < cond1)
                                                    //{
                                                    //    attendance = "HS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Black;
                                                    //}
                                                    //else if (sct >= 1 && sct == cond1)
                                                    //{
                                                    //    attendance = "FS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Black;
                                                    //}
                                                    //else if (nccct >= 1 && nccct < cond1)
                                                    //{
                                                    //    attendance = "HNCC";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Violet;
                                                    //}
                                                    //else if (nccct >= 1 && nccct == cond1)
                                                    //{
                                                    //    attendance = "FNCC";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Violet;
                                                    //}
                                                    //else if (hsct >= 1 && hsct < cond1)
                                                    //{
                                                    //    attendance = "HHS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SlateGray;
                                                    //}
                                                    //else if (hsct >= 1 && hsct == cond1)
                                                    //{
                                                    //    attendance = "FHS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SlateGray;
                                                    //}
                                                    //else if (ppct >= 1 && ppct < cond1)
                                                    //{
                                                    //    attendance = "HPP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Pink;
                                                    //}
                                                    //else if (ppct >= 1 && ppct == cond1)
                                                    //{
                                                    //    attendance = "FPP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Pink;
                                                    //}
                                                    //else if (syodct >= 1 && syodct < cond1)
                                                    //{
                                                    //    attendance = "HSYOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.LimeGreen;
                                                    //}
                                                    //else if (syodct >= 1 && syodct == cond1)
                                                    //{
                                                    //    attendance = "FSYOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.LimeGreen;
                                                    //}
                                                    //else if (codct >= 1 && codct < cond1)
                                                    //{
                                                    //    attendance = "HCOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Tan;
                                                    //}
                                                    //else if (codct >= 1 && codct == cond1)
                                                    //{
                                                    //    attendance = "FCOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Tan;
                                                    //}
                                                    //else if (oodct >= 1 && oodct < cond1)
                                                    //{
                                                    //    attendance = "HOOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Wheat;
                                                    //}
                                                    //else if (oodct >= 1 && oodct == cond1)
                                                    //{
                                                    //    attendance = "FOOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Wheat;
                                                    //}
                                                    //else if (lact >= 1 && lact < cond1)
                                                    //{
                                                    //    attendance = "HLA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Indigo;
                                                    //}
                                                    //else if (lact >= 1 && lact == cond1)
                                                    //{
                                                    //    attendance = "FLA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Indigo;
                                                    //}
                                                    //else if (nect >= 1 && nect < cond1)
                                                    //{
                                                    //    attendance = "HNE";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Magenta;
                                                    //}
                                                    //else if (nect >= 1 && nect == cond1)
                                                    //{
                                                    //    attendance = "FNE";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Magenta;
                                                    //}
                                                    //else if (raact >= 1 && raact < cond1)
                                                    //{
                                                    //    attendance = "HRAA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Yellow;
                                                    //}
                                                    //else if (raact >= 1 && raact == cond1)
                                                    //{
                                                    //    attendance = "FRAA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Yellow;
                                                    //}
                                                    //else if (act >= 1 && act < cond1)
                                                    //{
                                                    //    if ((fnhrs - minpresI) != 0 && (fnhrs - minpresI) < act && (anhrs - minpresII) != 0 && (anhrs - minpresII) < act)
                                                    //    {
                                                    //        attendance = "FA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //        fulldayabsent++;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        if (mfc > test1 && msc > test2)
                                                    //        {
                                                    //            attendance = "FA";
                                                    //            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //            fulldayabsent++;
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            if ((pfc == fnhrs || psc == anhrs) && odct >= 1)
                                                    //            {
                                                    //                attendance = "FP";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //                fpresent++;
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                attendance = "HA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                halfdayabsent++;
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                    //else if (lct >= 1 && lct == cond1)
                                                    //{
                                                    //    //attendance = "FL";
                                                    //    attendance = "FA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#0080ff");
                                                    //    leaveappl++;
                                                    //}
                                                    //else if (lct >= 1 && lct < cond1)
                                                    //{
                                                    //    if (lfc >= 1 && lsc >= 1)
                                                    //    {
                                                    //        //attendance = "FL";
                                                    //        attendance = "FA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#0080ff");
                                                    //        leaveappl++;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        //attendance = "HL";
                                                    //        attendance = "HA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Chocolate;
                                                    //        //leaveappl = leaveappl + Convert.ToInt32(0.5);
                                                    //        leaveappl++;
                                                    //    }
                                                    //}
                                                    //else if (hct >= 1 && hct < cond1)
                                                    //{
                                                    //    attendance = "HH";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Maroon;
                                                    //}
                                                    //else if (hct >= 1 && hct == cond1)
                                                    //{
                                                    //    attendance = "FH";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Maroon;
                                                    //}
                                                    //if (attendance != "")
                                                    //{
                                                    //    conducteddays++;
                                                    //}
                                                    //}
                                                    #endregion Hide
                                                    double absentday = 0;
                                                    string attendance = string.Empty;
                                                    int njhr = noofmornj + noofevenj;
                                                    nohrsprsentperday = nohrsprsentperday + noofmorpresent + noofmornj + noofevenj;
                                                    if (noofmorpresent + noofmornj >= minpresI)
                                                    {
                                                        present = 0.5;
                                                        prsentdays = prsentdays + 0.5;
                                                        noofdaypresen = 0.5;
                                                    }
                                                    else if (noofmorabsent >= 1)
                                                    {
                                                        absentday = 0.5;
                                                        absentdays = absentdays + 0.5;
                                                    }
                                                    if (noofmornj >= minpresI)
                                                    {
                                                        notJoinDays += 0.5;
                                                    }
                                                    if (noofevepresent + noofevenj >= minpresII)
                                                    {
                                                        present = present + 0.5;
                                                        prsentdays = prsentdays + 0.5;
                                                        noofdaypresen = noofdaypresen + 0.5;
                                                    }
                                                    else if (noofeveabsent >= 1)
                                                    {
                                                        absentday = absentday + 0.5;
                                                        absentdays = absentdays + 0.5;
                                                    }
                                                    if (noofevenj >= minpresII)
                                                    {
                                                        notJoinDays += 0.5;
                                                    }
                                                    if (fnhrs - unMark1 >= minpresI)
                                                    {
                                                        totalWorkingDays += 0.5;
                                                    }
                                                    if ((NoHrs - fnhrs) - unMark2 >= minpresII)
                                                    {
                                                        totalWorkingDays += 0.5;
                                                    }
                                                    if (Session["attdaywisecla"] != null && Session["attdaywisecla"].ToString() == "1")
                                                    {
                                                        if (nohrsprsentperday < minpresday)
                                                        {
                                                            prsentdays = prsentdays - noofdaypresen;
                                                            absentdays = absentdays + noofdaypresen;
                                                        }
                                                    }
                                                    nohrsprsentperday = 0;
                                                    noofdaypresen = 0;
                                                    if (noofmorod >= minpresI)
                                                    {
                                                        oddays = oddays + 0.5;
                                                    }
                                                    if (noofeveod >= minpresII)
                                                    {
                                                        oddays = oddays + 0.5;
                                                    }
                                                    if (noofmorleav >= minpresI)
                                                    {
                                                        leavedays = leavedays + 0.5;
                                                    }
                                                    if (noofeveleav >= minpresII)
                                                    {
                                                        leavedays = leavedays + 0.5;
                                                    }
                                                    if (noofmorcon >= minpresI)
                                                    {
                                                        conducteddays = conducteddays + 0.5;
                                                    }
                                                    if (noofevecon >= minpresII)
                                                    {
                                                        conducteddays = conducteddays + 0.5;
                                                    }
                                                    if (noofmorcon >= minpresI || noofevecon <= minpresII)
                                                    {
                                                        if (present == 1)
                                                        {
                                                            attendance = "FP";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                        }
                                                        else if (absentday == 0.5)
                                                        {
                                                            attendance = "HA";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Chocolate;
                                                            halfdayabsentcount++;
                                                        }
                                                        else if (absentday == 1)
                                                        {
                                                            attendance = "FA";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                            fuldayabsentcount++;
                                                        }
                                                        else if (present == 0.5 && absentday == 0)
                                                        {
                                                            attendance = "HP";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Pink;
                                                        }
                                                    }
                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].Text = attendance;
                                                    if (split_holiday_status_3 != 0 && split_holiday_status_4 != 0)
                                                    {
                                                        for (int holi_day = split_holiday_status_3; holi_day <= split_holiday_status_4; holi_day++)
                                                        {
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), holi_day].Text = "HD";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), holi_day].BackColor = Color.Red;
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), holi_day].Locked = true;
                                                        }
                                                    }
                                                    row_head_cnt++;
                                                }
                                                else
                                                {
                                                    for (int temp = 1; temp <= nohrs; temp++)
                                                    {
                                                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.MediumSlateBlue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int temp = 1; temp <= nohrs; temp++)
                                                {
                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.MediumSlateBlue;
                                                }
                                            }
                                        }
                                    }
                                    dummydate = dummydate.AddDays(1);
                                }
                                if (ds2.Tables.Count > 0 && i < ds2.Tables[0].Rows.Count)
                                {
                                    if (cumd == Convert.ToInt16(Convert.ToString(ds2.Tables[0].Rows[i]["month_year"]).Trim()))
                                    {
                                        i++;
                                    }
                                }
                            }
                        }
                        double totalConductedHours = (((morningConductedHrs * fnhrs) + (eveningConductedHrs * (NoHrs - fnhrs))) - unMarkedAttendance) - notConsidered;
                        double overAllConducetedDays = totalWorkingDays - notJoinDays;
                        double totalPresentDays = prsentdays - notJoinDays;
                        double percent = 0;
                        if (conducteddays != 0)
                        {
                            percent = Convert.ToDouble(prsentdays) / Convert.ToDouble(conducteddays) * 100;
                        }
                        double hourWisePercentage = 0;
                        if (totalConductedHours > 0)
                        {
                            hourWisePercentage = (totalPresentHours / totalConductedHours) * 100;
                            hourWisePercentage = Math.Round(hourWisePercentage, 2, MidpointRounding.AwayFromZero);
                        }
                        double dayWisePercentage = 0;
                        if (overAllConducetedDays > 0)
                        {
                            dayWisePercentage = (totalPresentDays / overAllConducetedDays) * 100;
                            dayWisePercentage = Math.Round(dayWisePercentage, 2, MidpointRounding.AwayFromZero);
                        }
                        name.Text = Convert.ToString(infods.Tables[0].Rows[0]["stud_name"]).Trim();
                        string yr = Convert.ToString(infods.Tables[0].Rows[0]["year"]).Trim();
                        string course = Convert.ToString(infods.Tables[0].Rows[0]["Course_Name"]).Trim();
                        string dept = Convert.ToString(infods.Tables[0].Rows[0]["dept_acronym"]).Trim();
                        string sc = Convert.ToString(infods.Tables[0].Rows[0]["Sections"]).Trim();
                        yr = Year(sem);
                        if (sc != "")
                        {
                            clas.Text = yr + " " + course + " " + dept + " " + sc;
                        }
                        else
                        {
                            clas.Text = yr + " " + course + " " + dept;
                        }
                        fullday.Text = fuldayabsentcount.ToString();
                        halfday.Text = halfdayabsentcount.ToString();
                        totdays.Text = absentdays.ToString();
                        odapplied.Text = oddays.ToString();
                        leaveapplied.Text = leavedays.ToString();
                        lblHrsWisePercentage.Text = string.Format("{0:0.00}", hourWisePercentage);
                        lblDaysWisePercentage.Text = string.Format("{0:0.00}", dayWisePercentage);
                        if (latd.Trim() != "")
                        {
                            string[] attndt = latd.Split(' ');
                            string[] ltd = attndt[0].Split('/');
                            DateTime dltd = Convert.ToDateTime(ltd[0] + "/" + ltd[1] + "/" + ltd[2]);
                            lastattndate.Text = dltd.ToString("dd/MM/yyyy");
                        }
                        std_info.Visible = true;
                    }
                }
            }
        }
    }

    public void loadattendanceNew()
    {
        try
        {
            lblreadmission.Text = "";
            lbdiscontinue.Text = "";
            DateTime Readmin = new DateTime();
            DateTime Discontin = new DateTime();
            DateTime Admission_date = new DateTime();
            Admission_date = Convert.ToDateTime(Session["Admission_date"]);
            string studentcollegeCode = string.Empty;
            attnd_report.Sheets[0].SheetName = " ";
            attnd_report.Sheets[0].RowCount = 0;
            attnd_report.Sheets[0].ColumnCount = 0;
            attnd_report.SaveChanges();
            attnd_report.Sheets[0].AutoPostBack = false;
            strcomo1 = new string[] { "Select for All ", " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NJ", "S", "NCC", "HS", "PP", "SYOD", "COD", "OOD", "LA", "RAA" };
            strcomo = new string[] { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NJ", "S", "NCC", "HS", "PP", "SYOD", "COD", "OOD", "LA", "RAA" };
            objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
            objintcell.ShowButton = true;
            objintcell.AutoPostBack = true;
            objintcell.UseValue = true;
            objintcell.BackColor = Color.Gold;
            objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);
            // objcom.ShowButton = true; 
            objcom.BackColor = Color.DarkSeaGreen;
            // objcom.AutoPostBack = true ;
            objcom.UseValue = true;
            ds3.Clear();
            hat.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string batchyear = string.Empty;
                studentcollegeCode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]).Trim();//college_code
                string isredo1 = Convert.ToString(ds.Tables[0].Rows[0]["isRedo"]).Trim();
                string delflag = Convert.ToString(ds.Tables[0].Rows[0]["Delflag"]).Trim();
                string roll = string.Empty;
                if (optionddl.Items[0].Selected == true)
                {
                    roll = "  r.roll_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
                }
                else if (optionddl.Items[1].Selected == true)
                {
                    roll = "  r.reg_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
                }
                else
                {
                    roll = "  r.roll_admit='" + Convert.ToString(txtrollno.Text).Trim() + "'";
                }
                string delQry = " select * from Readmission  where app_no in(select app_no from registration r where " + roll + ")";
                DataTable dtdisC = dir.selectDataTable(delQry);
                if (dtdisC.Rows.Count > 0)
                {
                    delflag = "1";
                }
                else
                {
                    delflag = "0";
                }
                if (isredo1 == "True" || delflag == "1")
                {
                    currentSemester = ddlSem.SelectedItem.Text;
                    string regsem = Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim();

                    if (isredoold == "1")
                    {
                        hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim());
                        batchyear = Convert.ToString(ds.Tables[0].Rows[0]["regbatch"]).Trim();
                    }
                    else
                    {
                        hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim());
                        batchyear = Convert.ToString(ds.Tables[0].Rows[0]["appbatch"]).Trim();
                    }
                }

                else
                {
                    hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim());
                    batchyear = Convert.ToString(ds.Tables[0].Rows[0]["regbatch"]).Trim();
                }
                hat.Add("sem_val", ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim()));
                string sections = string.Empty;
                string strsec = string.Empty;
                string coursename = Convert.ToString(ds.Tables[0].Rows[0]["course_name"]).Trim();
                string deptname = Convert.ToString(ds.Tables[0].Rows[0]["acronym"]).Trim();



                if (string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim()) && Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() != "0" && Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() != "-1")
                {
                    sections = Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim();
                    strsec = "Sec" + sections;
                }
                string studentname = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]).Trim();
                string sem = ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim());
                //Modified By Srinath 25/4/2013
                // string rollno = ds.Tables[0].Rows[0]["roll_admit"].ToString();
                string rollno = Convert.ToString(ds.Tables[0].Rows[0]["Roll_no"]).Trim();
                DataSet infods = new DataSet();
                string readmis = "select convert(varchar, Readm_date, 103) as Readm_date, convert(varchar, Dis_Date, 103) as Dis_Date from Readmission where app_no in(select App_no from Registration where Roll_No='" + rollno + "') order by Readm_date desc";
                DataSet readm = new DataSet();
                readm.Clear();
                readm = d2.select_method_wo_parameter(readmis, "Text");

                if (readm.Tables.Count > 0 && readm.Tables[0].Rows.Count > 0)
                {
                    lbdiscontinue.Visible = true;
                    lbldiscontinu.Visible = true;
                    lblread.Visible = true;
                    lblreadmission.Visible = true;
                    lblreadmission.Text = Convert.ToString(readm.Tables[0].Rows[0]["Readm_date"]);
                    lbdiscontinue.Text = Convert.ToString(readm.Tables[0].Rows[0]["Dis_Date"]);
                    string Readmins = Convert.ToString(readm.Tables[0].Rows[0]["Readm_date"]);
                    string Discontins = Convert.ToString(readm.Tables[0].Rows[0]["Dis_Date"]);
                    string[] spl = Readmins.Split('/');
                    Readmins = Convert.ToString(Convert.ToString(spl[1]).Trim() + "/" + Convert.ToString(spl[0]).Trim() + "/" + Convert.ToString(spl[2]).Trim());
                    bool isValidDate = DateTime.TryParseExact(Readmins, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None
       , out Readmin);
                    string[] spls = Discontins.Split('/');
                    Discontins = Convert.ToString(Convert.ToString(spls[1]).Trim() + "/" + Convert.ToString(spls[0]).Trim() + "/" + Convert.ToString(spls[2]).Trim());
                    bool isValidDates = DateTime.TryParseExact(Discontins, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None
       , out Discontin);
                }
                else
                {
                    string discon = d2.GetFunction("select convert(varchar, discontinue_date, 103) as discontinue_date  from Discontinue where app_no in(select App_no from Registration where Roll_No='" + rollno + "')  order by discontinue_date desc");
                    if (discon != "0" && discon != "")
                    {
                        lbdiscontinue.Text = discon;
                        string[] spl = discon.Split('/');
                        discon = Convert.ToString(Convert.ToString(spl[1]).Trim() + "/" + Convert.ToString(spl[0]).Trim() + "/" + Convert.ToString(spl[2]).Trim());
                        bool isValidDate = DateTime.TryParseExact(discon, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None
, out Discontin);



                        lbdiscontinue.Visible = true;
                        lbldiscontinu.Visible = true;
                        lblread.Visible = false;
                        lblreadmission.Visible = false;
                    }
                    else
                    {
                        lbdiscontinue.Visible = false;
                        lbldiscontinu.Visible = false;
                        lblread.Visible = false;
                        lblreadmission.Visible = false;
                    }
                }
                string infoquery = " select r.stud_name,c.Course_Name,dpt.dept_acronym,dpt.Dept_Name,r.Sections,r.Current_Semester as year  from Registration r,Degree d,Department dpt,Course c where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dpt.Dept_Code and r.Roll_No='" + rollno + "' order by r.Current_Semester desc ";
                infods = d2.select_method_wo_parameter(infoquery, "Text");
                ds3 = dacces2.select_method("period_attnd_schedule_sp", hat, "sp");
                if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                {
                    nohrs = int.Parse(Convert.ToString(ds3.Tables[0].Rows[0]["no_of_hrs_per_day"]).Trim());
                    first_half = int.Parse(Convert.ToString(ds3.Tables[0].Rows[0]["no_of_hrs_I_half_day"]).Trim());
                    second_half = int.Parse(Convert.ToString(ds3.Tables[0].Rows[0]["no_of_hrs_II_half_day"]).Trim());
                    Session["nohrs"] = nohrs;
                    if (nohrs != null)
                    {
                        attnd_report.Sheets[0].ColumnCount = nohrs + 1;
                        attnd_report.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
                        attnd_report.Sheets[0].Columns[0].Width = 100;
                        for (int i = 1; i <= (nohrs); i++)
                        {
                            attnd_report.Sheets[0].Columns[i].Width = 35;
                        }
                        attnd_report.Sheets[0].RowCount = 1;
                        attnd_report.Width = (50 * nohrs) + 670;
                        attnd_report.Sheets[0].Columns[0].BackColor = Color.Gray;
                        attnd_report.Sheets[0].RowHeader.Cells[0, 0].Text = " ";
                        attnd_report.Sheets[0].ColumnHeader.RowCount = 2;
                        attnd_report.Sheets[0].ColumnHeader.Columns.Count = 1;
                        attnd_report.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Left;
                        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, nohrs);
                        attnd_report.Sheets[0].ColumnHeader.Cells[0, 0].Text = "         " + "Degree:" + batchyear + "-" + coursename + "[" + deptname + "]" + "-" + "Sem  " + sem + "    " + strsec + "  RollNo:" + rollno + "  Name :" + studentname;
                        if (forschoolsetting == true)
                        {
                            attnd_report.Sheets[0].ColumnHeader.Cells[0, 0].Text = "         " + "Standard:" + batchyear + "-" + coursename + "[" + deptname + "]" + "-" + "Term  " + sem + "    " + strsec + "  RollNo:" + rollno + "  Name :" + studentname;
                        }
                        attnd_report.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].ColumnHeader.Cells[1, 0].VerticalAlign = VerticalAlign.Middle;
                        attnd_report.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Date";
                        for (int i = 1; i <= nohrs; i++)
                        {
                            attnd_report.Sheets[0].ColumnHeader.Columns.Count++;
                            attnd_report.Sheets[0].ColumnHeader.Cells[1, i].Text = i.ToString();
                            attnd_report.ActiveSheetView.Columns[i].CellType = objcom;
                            attnd_report.Sheets[0].Cells[0, i].CellType = objintcell;
                            // attnd_report.Sheets[0].Cells[0, 0].Locked = true;
                        }
                        attnd_report.Sheets[0].ColumnHeader.Columns.Count++;
                        attnd_report.Sheets[0].ColumnHeader.Cells[1, nohrs + 1].Text = "Attendance Status";
                        attnd_report.Sheets[0].Columns[nohrs + 1].BackColor = Color.Gray;
                        attnd_report.Sheets[0].Columns[nohrs + 1].Width = 170;
                        attnd_report.Sheets[0].Columns[0].Locked = true;
                        attnd_report.Sheets[0].Columns[nohrs + 1].Locked = true;
                        string frdate_datetime = string.Empty;
                        string todate_datetime = string.Empty;
                        string[] from_split2 = (txtFromDate.Text).Split('/');
                        string[] to_split2 = (txtToDate.Text).Split('/');
                        if (from_split2.Length > 0)
                        {
                            fd = Convert.ToInt16(Convert.ToString(from_split2[0]).Trim());
                            fm = Convert.ToInt16(Convert.ToString(from_split2[1]).Trim());
                            fyy = Convert.ToInt16(Convert.ToString(from_split2[2]).Trim());
                            Session["fd"] = fd;
                            Session["fyy"] = fyy;
                        }
                        if (to_split2.Length > 0)
                        {
                            td = Convert.ToInt16(Convert.ToString(to_split2[0]).Trim());
                            tm = Convert.ToInt16(Convert.ToString(to_split2[1]).Trim());
                            tyy = Convert.ToInt16(Convert.ToString(to_split2[2]).Trim());
                            Session["td"] = td;
                        }
                        string fDate = fm + "/" + fd + "/" + fyy;
                        string tdate = tm + "/" + td + "/" + tyy;
                        fcal = ((fyy * 12) + fm);
                        Session["fcal"] = fcal;
                        tcal = ((tyy * 12) + tm);
                        Session["tcal"] = tcal;
                        string get_rollno = GetFunction("select r.roll_no from registration r where " + get_roll_no_r + "");
                        string degcode = d2.GetFunction("select degree_code from registration  where Roll_No='" + get_rollno + "'");
                        string semester = d2.GetFunction("select Current_Semester from registration  where Roll_No ='" + get_rollno + "'");
                        string section = d2.GetFunction("select Sections from registration  where Roll_No ='" + get_rollno + "'");
                        ds2.Clear();
                        hat.Clear();
                        hat.Add("f_date", fcal);
                        hat.Add("t_date", tcal);
                        hat.Add("roll_no", Convert.ToString(get_rollno).Trim());
                        ds2 = dacces2.select_method("ATT_REPORTS_DETAILS", hat, "sp");
                        hat.Clear();
                        hat.Add("colege_code", Convert.ToString(studentcollegeCode).Trim());
                        DataSet dsattva = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                        Dictionary<string, string> dicattval = new Dictionary<string, string>();
                        if (dsattva.Tables.Count > 0 && dsattva.Tables[0].Rows.Count > 0)
                        {
                            for (int at = 0; at < dsattva.Tables[0].Rows.Count; at++)
                            {
                                string leavcode = Convert.ToString(dsattva.Tables[0].Rows[at]["leavecode"]).Trim();
                                string calc = Convert.ToString(dsattva.Tables[0].Rows[at]["calcflag"]).Trim();
                                if (!dicattval.ContainsKey(leavcode.Trim()))
                                {
                                    dicattval.Add(leavcode.Trim(), calc);
                                }
                            }
                        }
                        //=======================Student OD Lock
                        DataTable dtOnduty = dirAcc.selectDataTable("select * from Onduty_Stud od where roll_no='" + get_rollno + "'  and (convert(datetime,od.fromdate,105) >= '" + fDate + "' or  convert(datetime,od.Todate,105)>='" + tdate + "') and  (convert(datetime,od.fromdate,105) <='" + fDate + "' or convert(datetime,od.Todate,105)<= '" + tdate + "')");

                       // ===================================


                        int NoHrs = 0;
                        int fnhrs = 0;
                        int anhrs = 0;
                        int minpresI = 0;
                        int minpresII = 0;
                        int minpresday = 0;
                        double prsentdays = 0;
                        double absentdays = 0;
                        double conducteddays = 0;
                        double oddays = 0;
                        double leavedays = 0;
                        int fuldayabsentcount = 0;
                        int halfdayabsentcount = 0;
                        double nohrsprsentperday = 0;
                        double noofdaypresen = 0;
                        hat.Clear();
                        hat.Add("degree_code", degcode);
                        hat.Add("sem_ester", int.Parse(((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(semester).Trim())));
                        ds5 = d2.select_method("period_attnd_schedule", hat, "sp");
                        if (ds5.Tables.Count > 0 && ds5.Tables[0].Rows.Count > 0)
                        {
                            NoHrs = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["PER DAY"]).Trim());
                            fnhrs = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["I_HALF_DAY"]).Trim());
                            anhrs = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["II_HALF_DAY"]).Trim());
                            minpresI = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["MIN PREE I DAY"]).Trim());
                            minpresII = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["MIN PREE II DAY"]).Trim());
                            minpresday = int.Parse(ds5.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
                            //minpresday = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["PER DAY"]).Trim());

                        }
                        {
                            totmonth = fcal;
                            fflag = true;
                            int i = 0;
                            double k = 1;
                            dat = fd;
                            int k_temp = 0;
                            //int fulldayabsent = 0;
                            //int halfdayabsent = 0;
                            //int leaveapplycount = 0;
                            //int leaveappl = 0;
                            //double odappl = 0;
                            //int conducteddays = 0;
                            //int fpresent = 0;
                            //int hpresent = 0;
                            string latd = string.Empty;
                            dummydate = Convert.ToDateTime(Convert.ToString(dt1.ToShortDateString()).Trim());
                            Session["start_date"] = dummydate;
                            int morningConductedHrs = 0;
                            int eveningConductedHrs = 0;
                            int unMarkedAttendance = 0;
                            int notConsidered = 0;
                            double totalPresentHours = 0;
                            double notJoinDays = 0;
                            double totalWorkingDays = 0;
                            //for (int cumd = fcal; cumd <= tcal; cumd++)
                            //{
                            //    totpresentday = 0;
                            //    if (cumd == tcal)
                            //    {
                            //        cal_date(cumd);
                            //        if (fd == td)
                            //        {
                            //            totpresentday += 1;
                            //        }
                            //        else if (td == daycount)
                            //        {
                            //            totpresentday += daycount;
                            //            balamonday = daycount;
                            //        }
                            //        else
                            //        {
                            //            totpresentday += td - (fd - 1);
                            //            balamonday = fd - (td);
                            //        }
                            //    }
                            //    if (cumd != tcal)
                            //    {
                            //        cal_date(cumd);
                            //        totpresentday += daycount;
                            //        balamonday = daycount;
                            //    }
                            //    //------------find start date
                            //    if (cumd == fcal)
                            //    {
                            //        k_temp = fd;
                            //    }
                            //    else
                            //    {
                            //        k_temp = 1;
                            //    }
                            //    if (cumd == tcal)
                            //    {
                            //        endk = td;
                            //    }
                            //    else
                            //    {
                            //        endk = totpresentday;
                            //    }
                            {
                                //for (k = k_temp; k <= endk; k++)
                                DateTime dtStartDate = new DateTime();
                                DateTime dtEndDate = new DateTime();
                                while (dummydate <= dt2)
                                {
                                    int forcount = 0;
                                    int pct = 0, act = 0, odct = 0, mlct = 0, sodct = 0, nssct = 0, hct = 0, njct = 0, sct = 0, lct = 0, nccct = 0, hsct = 0, ppct = 0, syodct = 0, codct = 0, oodct = 0, lact = 0, nect = 0, raact = 0;
                                    if (dummydate >= Admission_date)
                                    {
                                        double present = 0;
                                        int noofmorpresent = 0;
                                        int noofmorabsent = 0;
                                        int noofmornj = 0;
                                        int noofevepresent = 0;
                                        int noofeveabsent = 0;
                                        int noofevenj = 0;
                                        int noofmorcon = 0;
                                        int noofevecon = 0;
                                        int noofmorod = 0, noofmorleav = 0, noofeveod = 0, noofeveleav = 0;
                                        ddd = dummydate.ToString("ddd");
                                        string monthYear = Convert.ToString(((dummydate.Year * 12) + dummydate.Month));
                                        find_holiday(dummydate);
                                        DataView dvCount = new DataView();
                                        string odPeriods = string.Empty;
                                        if (holidayflag == false)
                                        {
                                            if (dtOnduty.Rows.Count > 0)
                                            {
                                                dtOnduty.DefaultView.RowFilter = "Roll_no='" + get_rollno + "' and  (Fromdate>='" + dummydate + "' or  Todate>='" + dummydate + "') and (Fromdate<='" + dummydate + "' or  Todate<='" + dummydate + "') ";
                                                dvCount = dtOnduty.DefaultView;
                                                if(dvCount.Count>0)
                                                    odPeriods = Convert.ToString(dvCount[0]["hourse"]);
                                            }
                                            //----------------get date for display in spread
                                            string dummy_date = string.Empty;
                                            string date_text = string.Empty;
                                            dummy_date = dummydate.ToString();
                                            string[] dummy_split = dummy_date.Split(' ');
                                            string[] dummy_split2 = dummy_split[0].Split('/');

                                            //----------------------------set date
                                            attnd_report.Sheets[0].RowCount++;
                                            attnd_report.Sheets[0].RowHeader.Cells[attnd_report.Sheets[0].RowCount - 1, 0].Text = (attnd_report.Sheets[0].RowCount - 1).ToString();
                                            //-----------------------lock row for attnd security
                                            chk = daycheck(Convert.ToDateTime(dummydate));
                                            if (chk == false)
                                            {
                                                attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Locked = true;
                                            }
                                            date_text = Convert.ToString(dummy_split2[1]).Trim() + "/" + Convert.ToString(dummy_split2[0]).Trim() + "/" + Convert.ToString(dummy_split2[2]).Trim();

                                            attnd_report.Sheets[0].SetValue((attnd_report.Sheets[0].RowCount - 1), 0, Convert.ToString(date_text).Trim());
                                            attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dummy_split[0]).Trim();
                                            if (ds2.Tables.Count > 0 && i < ds2.Tables[0].Rows.Count)
                                            {
                                                DataView dvStudentAttendance = new DataView();
                                                ds2.Tables[0].DefaultView.RowFilter = "month_year='" + monthYear + "'";
                                                dvStudentAttendance = ds2.Tables[0].DefaultView;
                                                //if (cumd == Convert.ToInt16(Convert.ToString(ds2.Tables[0].Rows[i]["month_year"]).Trim()))
                                                if (dvStudentAttendance.Count > 0)
                                                {
                                                    int mfc = 0;
                                                    int msc = 0;
                                                    int lfc = 0;
                                                    int lsc = 0;
                                                    int pfc = 0;
                                                    int psc = 0;
                                                    int emptyfc = 0;
                                                    int emptysc = 0;
                                                    int unMark1 = 0;
                                                    int unMark2 = 0;
                                                    if (split_holiday_status_1 == 1 && split_holiday_status_2 == NoHrs)
                                                    {
                                                        morningConductedHrs += 1;
                                                        eveningConductedHrs += 1;
                                                    }
                                                    else if (split_holiday_status_1 == fnhrs + 1 && split_holiday_status_2 == NoHrs)
                                                    {
                                                        eveningConductedHrs += 1;
                                                    }
                                                    else if (split_holiday_status_1 == 1 && split_holiday_status_2 == fnhrs)
                                                    {
                                                        morningConductedHrs += 1;
                                                    }
                                                    for (int temp = split_holiday_status_1; temp <= split_holiday_status_2; temp++)
                                                    {
                                                        bool isOD = false;
                                                        forcount++;
                                                        temp_val = "d" + dummy_split2[1] + "d" + temp;
                                                        if (!string.IsNullOrEmpty(odPeriods))
                                                        {
                                                            if (odPeriods.Contains(temp.ToString()))
                                                                isOD = true;
                                                        }
                                                        if (dvStudentAttendance.Count > 0)
                                                        {
                                                            //if (dvStudentAttendance.Count > 0)
                                                            //{
                                                            bool freehr = isApplicableForFreeSpecial(Convert.ToString(studentcollegeCode).Trim(), batchyear, degcode, semester, section, dummydate.ToString("MM/dd/yyyy"), "0", temp.ToString(), NoHrs.ToString());

                                                            bool specialday = isApplicableForFreeSpecial(Convert.ToString(studentcollegeCode).Trim(), batchyear, degcode, semester, section, dummydate.ToString("MM/dd/yyyy"), "1", temp.ToString(), NoHrs.ToString());

                                                            string valueAtt = Convert.ToString(dvStudentAttendance[0][temp_val]).Trim();
                                                            Att_mark = string.Empty;
                                                            if (!string.IsNullOrEmpty(valueAtt) && valueAtt != "" && valueAtt != "0" && valueAtt != "7")
                                                            {
                                                                leave_code = (Convert.ToString(dvStudentAttendance[0][temp_val]).Trim());
                                                                Attmark(leave_code);
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].Text = Att_mark;
                                                                string clav = string.Empty;
                                                                if (dicattval.ContainsKey(leave_code.Trim()))
                                                                {
                                                                    clav = Convert.ToString(dicattval[leave_code.Trim()]).Trim();
                                                                }
                                                                if (temp <= fnhrs)
                                                                {
                                                                    if (leave_code == "3")
                                                                    {
                                                                        noofmorod++;
                                                                    }
                                                                    if (leave_code == "10")
                                                                    {
                                                                        noofmorleav++;
                                                                    }
                                                                    if (clav == "0")
                                                                    {
                                                                        noofmorpresent++;
                                                                        noofmorcon++;
                                                                        nohrsprsentperday++;
                                                                        totalPresentHours++;
                                                                    }
                                                                    else if (clav == "1")
                                                                    {
                                                                        noofmorabsent++;
                                                                        noofmorcon++;
                                                                    }
                                                                    else if (clav == "2")
                                                                    {
                                                                        noofmornj++;
                                                                        noofmorcon++;
                                                                        notConsidered++;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (clav == "0")
                                                                    {
                                                                        noofevepresent++;
                                                                        noofevecon++;
                                                                        nohrsprsentperday++;
                                                                        totalPresentHours++;
                                                                    }
                                                                    else if (clav == "1")
                                                                    {
                                                                        noofeveabsent++;
                                                                        noofevecon++;
                                                                    }
                                                                    else if (clav == "2")
                                                                    {
                                                                        noofevenj++;
                                                                        noofevecon++;
                                                                        notConsidered++;
                                                                    }
                                                                    if (leave_code == "3")
                                                                    {
                                                                        noofeveod++;
                                                                    }
                                                                    if (leave_code == "10")
                                                                    {
                                                                        noofeveleav++;
                                                                    }
                                                                }
                                                                if (temp <= fnhrs)
                                                                {
                                                                    if (Att_mark == "A")
                                                                    {
                                                                        mfc++;
                                                                    }
                                                                }
                                                                else if (temp <= NoHrs)
                                                                {
                                                                    if (Att_mark == "A")
                                                                    {
                                                                        msc++;
                                                                    }
                                                                }
                                                                if (temp <= fnhrs)
                                                                {
                                                                    if (Att_mark == "L")
                                                                    {
                                                                        lfc++;
                                                                    }
                                                                }
                                                                else if (temp <= NoHrs)
                                                                {
                                                                    if (Att_mark == "L")
                                                                    {
                                                                        lsc++;
                                                                    }
                                                                }
                                                                if (temp <= fnhrs)
                                                                {
                                                                    if (Att_mark == "P")
                                                                    {
                                                                        pfc++;
                                                                    }
                                                                }
                                                                else if (temp <= NoHrs)
                                                                {
                                                                    if (Att_mark == "P")
                                                                    {
                                                                        psc++;
                                                                    }
                                                                }
                                                                if (Att_mark == "A")
                                                                {
                                                                    act++;
                                                                }
                                                                else if (Att_mark == "P")
                                                                {
                                                                    pct++;
                                                                }
                                                                else if (Att_mark == "L")
                                                                {
                                                                    lct++;
                                                                }
                                                                else if (Att_mark == "H")
                                                                {
                                                                    hct++;
                                                                }
                                                                else if (Att_mark == "OD")
                                                                {
                                                                    odct++;
                                                                }
                                                                else if (Att_mark == "ML")
                                                                {
                                                                    mlct++;
                                                                }
                                                                else if (Att_mark == "SOD")
                                                                {
                                                                    sodct++;
                                                                }
                                                                else if (Att_mark == "NSS")
                                                                {
                                                                    nssct++;
                                                                }
                                                                else if (Att_mark == "NJ")
                                                                {
                                                                    njct++;
                                                                }
                                                                else if (Att_mark == "S")
                                                                {
                                                                    sct++;
                                                                }
                                                                else if (Att_mark == "NCC")
                                                                {
                                                                    nccct++;
                                                                }
                                                                else if (Att_mark == "HS")
                                                                {
                                                                    hsct++;
                                                                }
                                                                else if (Att_mark == "PP")
                                                                {
                                                                    ppct++;
                                                                }
                                                                else if (Att_mark == "SYOD")
                                                                {
                                                                    syodct++;
                                                                }
                                                                else if (Att_mark == "COD")
                                                                {
                                                                    codct++;
                                                                }
                                                                else if (Att_mark == "OOD")
                                                                {
                                                                    oodct++;
                                                                }
                                                                else if (Att_mark == "LA")
                                                                {
                                                                    lact++;
                                                                }
                                                                else if (Att_mark == "NE")
                                                                {
                                                                    nect++;
                                                                }
                                                                else if (Att_mark == "RAA")
                                                                {
                                                                    raact++;
                                                                }
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].Tag = leave_code;
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.DarkSeaGreen;
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.White;
                                                                btnsave.Visible = true;
                                                            }
                                                            else
                                                            {
                                                                unMarkedAttendance++;
                                                                if (temp <= fnhrs)
                                                                {
                                                                    unMark1++;
                                                                    if (Att_mark == "")
                                                                    {
                                                                        emptyfc++;
                                                                    }
                                                                }
                                                                else if (temp <= NoHrs)
                                                                {
                                                                    unMark2++;
                                                                    if (Att_mark == "")
                                                                    {
                                                                        emptysc++;
                                                                    }
                                                                }
                                                            }
                                                            if (freehr)
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#FF00FF"); //Color.Red;
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].Text = "FH";
                                                            }
                                                            else if (specialday)
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#432F5C"); //Color.Lavender;
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].Text = "SH";
                                                            }
                                                            else if (Att_mark == "P")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#008000"); //Color.Green;
                                                                latd = dummydate.ToString();
                                                            }
                                                            else if (Att_mark == "A")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#FF0000"); //Color.Red;//#FF0000	
                                                            }
                                                            else if (Att_mark == "H")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#800000"); //Color.Maroon;
                                                            }
                                                            else if (Att_mark == "OD")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#0000FF"); //Color.Blue;
                                                            }
                                                            else if (Att_mark == "SOD")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#adff2f"); //Color.GreenYellow;
                                                            }
                                                            else if (Att_mark == "ML")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#E9967A"); // Color.DarkSalmon;
                                                            }
                                                            else if (Att_mark == "NSS")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#DAA520"); //Color.Goldenrod;
                                                            }
                                                            else if (Att_mark == "L")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#0080ff");
                                                            }
                                                            else if (Att_mark == "NCC")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#EE82EE"); //Color.Violet;
                                                            }
                                                            else if (Att_mark == "HS")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#708090"); //Color.SlateGray;
                                                            }
                                                            else if (Att_mark == "PP")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#FFC0CB"); // Color.Pink;
                                                            }
                                                            else if (Att_mark == "SYOD")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#32cd32"); //Color.LimeGreen;
                                                            }
                                                            else if (Att_mark == "COD")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#D2B48C"); //Color.Tan;
                                                            }
                                                            else if (Att_mark == "OOD")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#f5deb3"); // Color.Wheat;
                                                            }
                                                            else if (Att_mark == "NJ")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#8b4513"); // Color.SaddleBrown;
                                                            }
                                                            else if (Att_mark == "S")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#000000"); // Color.Black;
                                                            }
                                                            else if (Att_mark == "RAA")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#FFFF00"); // Color.Yellow;
                                                            }
                                                            else if (Att_mark == "")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = ColorTranslator.FromHtml("#7B68EE"); //Color.MediumSlateBlue;
                                                            }
                                                            //}
                                                            //else
                                                            //{
                                                            //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.DarkSeaGreen;
                                                            //}
                                                        }
                                                        else
                                                        {
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.DarkSeaGreen;//ColorTranslator.FromHtml("#008000");
                                                        }
                                                        if (isOD)
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].Locked = true;
                                                    }
                                                    #region Hide
                                                    //if (ds5.Tables[0].Rows.Count > 0)
                                                    //{
                                                    //    NoHrs = int.Parse(ds5.Tables[0].Rows[0]["PER DAY"].ToString());
                                                    //    fnhrs = int.Parse(ds5.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                                    //    anhrs = int.Parse(ds5.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                                    //    minpresI = int.Parse(ds5.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                                    //    minpresII = int.Parse(ds5.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                                    //}
                                                    //int cond1 = minpresI + minpresII;
                                                    //int test1 = fnhrs - minpresI;
                                                    //int test2 = anhrs - minpresII;
                                                    //string attendance =string.Empty;
                                                    ////bool freehr = isApplicableForFreeSpecial(Session["collegecode"].ToString(), batchyear, degcode, semester, section, dummydate.ToString("MM/dd/yyyy"), "0");
                                                    ////bool specialday = isApplicableForFreeSpecial(Session["collegecode"].ToString(), batchyear, degcode, semester, section, dummydate.ToString("MM/dd/yyyy"), "1");
                                                    //if (cond1 <= act)
                                                    //{
                                                    //    attendance = "FA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //    fulldayabsent++;
                                                    //}
                                                    //else if (pct >= 1 && pct < cond1)
                                                    //{
                                                    //    //if (pfc >= 1 && psc >= 1)
                                                    //    //{
                                                    //    //    attendance = "FP";
                                                    //    //}
                                                    //    //else
                                                    //    //{
                                                    //    //    attendance = "HP";
                                                    //    //}
                                                    //    if (mfc > test1 && msc > test2)
                                                    //    {
                                                    //        attendance = "FA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //        fulldayabsent++;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        if (mfc == 0 && msc == 0 && pfc > test1 && psc > test2)
                                                    //        {
                                                    //            if ((emptyfc > test1 && emptysc > test2))
                                                    //            {
                                                    //                attendance = "FA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //                fulldayabsent++;
                                                    //            }
                                                    //            else if ((emptyfc > test1 || emptysc > test2))
                                                    //            {
                                                    //                attendance = "HA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                halfdayabsent++;
                                                    //            }
                                                    //            else if ((pfc <= fnhrs || psc <= anhrs))
                                                    //            {
                                                    //                attendance = "HA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                halfdayabsent++;
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                attendance = "FP";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //                fpresent++;
                                                    //            }
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            if ((pfc == fnhrs || psc == anhrs) && odct >= 1)
                                                    //            {
                                                    //                attendance = "FP";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //                fpresent++;
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                if ((emptyfc > test1 || emptysc > test2) && (mfc > test1 || msc > test2))
                                                    //                {
                                                    //                    attendance = "FA";
                                                    //                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //                    fulldayabsent++;
                                                    //                }
                                                    //                else if ((emptyfc > test1 && emptysc > test2) && (pfc > test1 || psc > test2))
                                                    //                {
                                                    //                    attendance = "FA";
                                                    //                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //                    fulldayabsent++;
                                                    //                }
                                                    //                else
                                                    //                {
                                                    //                    attendance = "HA";
                                                    //                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                    halfdayabsent++;
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                    //else if (pct > test1 && pct == cond1)
                                                    //{
                                                    //    attendance = "FP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //    fpresent++;
                                                    //}
                                                    //else if (odct >= 1 && odct < cond1)
                                                    //{
                                                    //    if ((mfc > test1 || msc > test2) && odct >= 1)
                                                    //    {
                                                    //        //attendance = "OD";
                                                    //        attendance = "HA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Blue;
                                                    //        odappl = odappl + 0.5;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        //attendance = "OD";
                                                    //        attendance = "FP";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Blue;
                                                    //        odappl++;
                                                    //    }
                                                    //}
                                                    //else if (odct >= 1 && odct == cond1)
                                                    //{
                                                    //    //attendance = "OD";
                                                    //    attendance = "FP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Blue;
                                                    //    odappl++;
                                                    //}
                                                    //else if (mlct >= 1 && mlct < cond1)
                                                    //{
                                                    //    attendance = "HML";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //}
                                                    //else if (mlct >= 1 && mlct == cond1)
                                                    //{
                                                    //    attendance = "FML";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //}
                                                    //else if (sodct >= 1 && sodct < cond1)
                                                    //{
                                                    //    attendance = "HSOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.GreenYellow;
                                                    //}
                                                    //else if (sodct >= 1 && sodct == cond1)
                                                    //{
                                                    //    attendance = "FSOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.GreenYellow;
                                                    //}
                                                    //else if (nssct >= 1 && nssct < cond1)
                                                    //{
                                                    //    attendance = "HNSS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Goldenrod;
                                                    //}
                                                    //else if (nssct >= 1 && nssct == cond1)
                                                    //{
                                                    //    attendance = "FNSS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Goldenrod;
                                                    //}
                                                    //else if (njct >= 1 && njct < cond1)
                                                    //{
                                                    //    attendance = "HNJ";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SaddleBrown;
                                                    //}
                                                    //else if (njct >= 1 && njct == cond1)
                                                    //{
                                                    //    attendance = "FNJ";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SaddleBrown;
                                                    //}
                                                    //else if (sct >= 1 && sct < cond1)
                                                    //{
                                                    //    attendance = "HS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Black;
                                                    //}
                                                    //else if (sct >= 1 && sct == cond1)
                                                    //{
                                                    //    attendance = "FS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Black;
                                                    //}
                                                    //else if (nccct >= 1 && nccct < cond1)
                                                    //{
                                                    //    attendance = "HNCC";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Violet;
                                                    //}
                                                    //else if (nccct >= 1 && nccct == cond1)
                                                    //{
                                                    //    attendance = "FNCC";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Violet;
                                                    //}
                                                    //else if (hsct >= 1 && hsct < cond1)
                                                    //{
                                                    //    attendance = "HHS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SlateGray;
                                                    //}
                                                    //else if (hsct >= 1 && hsct == cond1)
                                                    //{
                                                    //    attendance = "FHS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SlateGray;
                                                    //}
                                                    //else if (ppct >= 1 && ppct < cond1)
                                                    //{
                                                    //    attendance = "HPP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Pink;
                                                    //}
                                                    //else if (ppct >= 1 && ppct == cond1)
                                                    //{
                                                    //    attendance = "FPP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Pink;
                                                    //}
                                                    //else if (syodct >= 1 && syodct < cond1)
                                                    //{
                                                    //    attendance = "HSYOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.LimeGreen;
                                                    //}
                                                    //else if (syodct >= 1 && syodct == cond1)
                                                    //{
                                                    //    attendance = "FSYOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.LimeGreen;
                                                    //}
                                                    //else if (codct >= 1 && codct < cond1)
                                                    //{
                                                    //    attendance = "HCOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Tan;
                                                    //}
                                                    //else if (codct >= 1 && codct == cond1)
                                                    //{
                                                    //    attendance = "FCOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Tan;
                                                    //}
                                                    //else if (oodct >= 1 && oodct < cond1)
                                                    //{
                                                    //    attendance = "HOOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Wheat;
                                                    //}
                                                    //else if (oodct >= 1 && oodct == cond1)
                                                    //{
                                                    //    attendance = "FOOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Wheat;
                                                    //}
                                                    //else if (lact >= 1 && lact < cond1)
                                                    //{
                                                    //    attendance = "HLA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Indigo;
                                                    //}
                                                    //else if (lact >= 1 && lact == cond1)
                                                    //{
                                                    //    attendance = "FLA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Indigo;
                                                    //}
                                                    //else if (nect >= 1 && nect < cond1)
                                                    //{
                                                    //    attendance = "HNE";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Magenta;
                                                    //}
                                                    //else if (nect >= 1 && nect == cond1)
                                                    //{
                                                    //    attendance = "FNE";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Magenta;
                                                    //}
                                                    //else if (raact >= 1 && raact < cond1)
                                                    //{
                                                    //    attendance = "HRAA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Yellow;
                                                    //}
                                                    //else if (raact >= 1 && raact == cond1)
                                                    //{
                                                    //    attendance = "FRAA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Yellow;
                                                    //}
                                                    //else if (act >= 1 && act < cond1)
                                                    //{
                                                    //    if ((fnhrs - minpresI) != 0 && (fnhrs - minpresI) < act && (anhrs - minpresII) != 0 && (anhrs - minpresII) < act)
                                                    //    {
                                                    //        attendance = "FA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //        fulldayabsent++;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        if (mfc > test1 && msc > test2)
                                                    //        {
                                                    //            attendance = "FA";
                                                    //            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //            fulldayabsent++;
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            if ((pfc == fnhrs || psc == anhrs) && odct >= 1)
                                                    //            {
                                                    //                attendance = "FP";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //                fpresent++;
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                attendance = "HA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                halfdayabsent++;
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                    //else if (lct >= 1 && lct == cond1)
                                                    //{
                                                    //    //attendance = "FL";
                                                    //    attendance = "FA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#0080ff");
                                                    //    leaveappl++;
                                                    //}
                                                    //else if (lct >= 1 && lct < cond1)
                                                    //{
                                                    //    if (lfc >= 1 && lsc >= 1)
                                                    //    {
                                                    //        //attendance = "FL";
                                                    //        attendance = "FA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#0080ff");
                                                    //        leaveappl++;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        //attendance = "HL";
                                                    //        attendance = "HA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Chocolate;
                                                    //        //leaveappl = leaveappl + Convert.ToInt32(0.5);
                                                    //        leaveappl++;
                                                    //    }
                                                    //}
                                                    //else if (hct >= 1 && hct < cond1)
                                                    //{
                                                    //    attendance = "HH";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Maroon;
                                                    //}
                                                    //else if (hct >= 1 && hct == cond1)
                                                    //{
                                                    //    attendance = "FH";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Maroon;
                                                    //}
                                                    //if (attendance != "")
                                                    //{
                                                    //    conducteddays++;
                                                    //}
                                                    //}
                                                    #endregion Hide
                                                    bool isDayWiseCalcAttendance = false;
                                                    if (Session["attdaywisecla"] != null && Session["attdaywisecla"].ToString() == "1")
                                                    {
                                                        isDayWiseCalcAttendance = true;
                                                    }
                                                    double absentday = 0;
                                                    string attendance = string.Empty;
                                                    int njhr = noofmornj + noofevenj;
                                                    double presentDays = 0;
                                                    double absentDays = 0;
                                                    double noOfPresentHourPerDay = noofmorpresent + noofmornj + noofevepresent + noofevenj;
                                                    nohrsprsentperday = nohrsprsentperday + noofmornj + noofevenj;
                                                    if (noofmorpresent + noofmornj >= minpresI)
                                                    {
                                                        present = 0.5;
                                                        prsentdays = prsentdays + 0.5;
                                                        noofdaypresen = 0.5;
                                                    }
                                                    else if (noofmorabsent >= 1)
                                                    {
                                                        absentday = 0.5;
                                                        absentdays = absentdays + 0.5;
                                                        //absentday = 1;
                                                        //absentdays = absentdays + 1;
                                                    }

                                                    if (noofmornj >= minpresI)
                                                    {
                                                        notJoinDays += 0.5;
                                                    }

                                                    if (noofevepresent + noofevenj >= minpresII)
                                                    {
                                                        present = present + 0.5;
                                                        prsentdays = prsentdays + 0.5;
                                                        noofdaypresen = noofdaypresen + 0.5;
                                                    }
                                                    else if (noofeveabsent >= 1)
                                                    {
                                                        absentday = absentday + 0.5;
                                                        absentdays = absentdays + 0.5;
                                                        //absentday = absentday + 1;
                                                        //absentdays = absentdays + 1;
                                                    }

                                                    if (noofevenj >= minpresII)
                                                    {
                                                        notJoinDays += 0.5;
                                                    }
                                                    if (fnhrs - unMark1 >= minpresI)
                                                    {
                                                        totalWorkingDays += 0.5;
                                                    }
                                                    if ((NoHrs - fnhrs) - unMark2 >= minpresII)
                                                    {
                                                        totalWorkingDays += 0.5;
                                                    }
                                                    presentDays = present;
                                                    absentDays = absentday;
                                                    double totalNoOfAbsent = noofmorabsent + noofeveabsent;
                                                    if (isDayWiseCalcAttendance)
                                                    {
                                                        double minimumAbsent = nohrs - minpresday;
                                                        if (totalNoOfAbsent > minimumAbsent)
                                                        {
                                                            presentDays = 0;
                                                            absentDays = 1;
                                                        }
                                                    }
                                                    if (Session["attdaywisecla"] != null && Session["attdaywisecla"].ToString() == "1")
                                                    {
                                                        //if (noOfPresentHourPerDay > minpresday)
                                                        //{
                                                        //    presentDays = 1;
                                                        //    absentDays = 0;
                                                        //}
                                                        //if (noOfPresentHourPerDay < minpresday)
                                                        //{
                                                        //    presentDays -= noofdaypresen;
                                                        //    absentDays += noofdaypresen;
                                                        //}
                                                        if (nohrsprsentperday < minpresday)
                                                        {
                                                            prsentdays = prsentdays - noofdaypresen;
                                                            absentdays = absentdays + noofdaypresen;
                                                            //presentDays -= noofdaypresen;
                                                            //absentDays += noofdaypresen;
                                                        }
                                                    }
                                                    nohrsprsentperday = 0;
                                                    noofdaypresen = 0;
                                                    if (noofmorod >= minpresI)
                                                    {
                                                        oddays = oddays + 0.5;
                                                    }
                                                    if (noofeveod >= minpresII)
                                                    {
                                                        oddays = oddays + 0.5;
                                                    }
                                                    if (noofmorleav >= minpresI)
                                                    {
                                                        leavedays = leavedays + 0.5;
                                                    }

                                                    if (noofeveleav >= minpresII)
                                                    {
                                                        leavedays = leavedays + 0.5;
                                                    }
                                                    if (noofmorcon >= minpresI)
                                                    {
                                                        conducteddays = conducteddays + 0.5;
                                                    }
                                                    if (noofevecon >= minpresII)
                                                    {
                                                        conducteddays = conducteddays + 0.5;
                                                    }

                                                    if (noofmorcon >= minpresI || noofevecon <= minpresII)
                                                    {
                                                        if (presentDays == 1)
                                                        {
                                                            attendance = "FP";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#008000"); // Color.Green;
                                                        }
                                                        else if (absentDays == 0.5)
                                                        {
                                                            attendance = "HA";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#D2691E"); // Color.Chocolate;
                                                            halfdayabsentcount++;
                                                        }
                                                        else if (absentDays >= 1)
                                                        {
                                                            attendance = "FA";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FF0000"); // Color.Red;
                                                            fuldayabsentcount++;
                                                        }
                                                        //else if (absentday == 1)
                                                        //{
                                                        //    attendance = "FA";
                                                        //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FF0000"); // Color.Red;
                                                        //    fuldayabsentcount++;
                                                        //}
                                                        else if (presentDays == 0.5 && absentDays == 0)
                                                        {
                                                            attendance = "HP";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FFC0CB"); // Color.Pink;
                                                        }

                                                        //if (present == 1)
                                                        //{
                                                        //    attendance = "FP";
                                                        //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#008000"); // Color.Green;
                                                        //}
                                                        //else if (absentday == 0.5)
                                                        //{
                                                        //    attendance = "HA";
                                                        //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#D2691E"); // Color.Chocolate;
                                                        //    halfdayabsentcount++;
                                                        //}
                                                        //else if (absentday >= 1)
                                                        //{
                                                        //    attendance = "FA";
                                                        //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FF0000"); // Color.Red;
                                                        //    fuldayabsentcount++;
                                                        //}
                                                        ////else if (absentday == 1)
                                                        ////{
                                                        ////    attendance = "FA";
                                                        ////    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FF0000"); // Color.Red;
                                                        ////    fuldayabsentcount++;
                                                        ////}
                                                        //else if (present == 0.5 && absentday == 0)
                                                        //{
                                                        //    attendance = "HP";
                                                        //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FFC0CB"); // Color.Pink;
                                                        //}
                                                    }
                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].Text = attendance;
                                                    if (split_holiday_status_3 != 0 && split_holiday_status_4 != 0)
                                                    {
                                                        for (int holi_day = split_holiday_status_3; holi_day <= split_holiday_status_4; holi_day++)
                                                        {
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), holi_day].Text = "HD";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), holi_day].BackColor = Color.Red;
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), holi_day].Locked = true;
                                                        }
                                                    }
                                                    row_head_cnt++;
                                                }
                                                else
                                                {
                                                    for (int temp = 1; temp <= nohrs; temp++)
                                                    {
                                                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.MediumSlateBlue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int temp = 1; temp <= nohrs; temp++)
                                                {
                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.MediumSlateBlue;
                                                }
                                            }
                                            string mon = string.Empty;
                                            string dayste = string.Empty;
                                            if (Convert.ToInt64(dummy_split2[1]) < 10)
                                            {
                                                dayste = "0" + dummy_split2[1];
                                            }
                                            else
                                                dayste = dummy_split2[1];
                                            if (Convert.ToInt64(dummy_split2[0]) < 10)
                                            {
                                                mon = "0" + dummy_split2[0];
                                            }
                                            else
                                                mon = dummy_split2[0];
                                            string datestest = Convert.ToString(mon) + "/" + Convert.ToString(dayste) + "/" + Convert.ToString(dummy_split2[2]).Trim();

                                            DateTime dsdate = Convert.ToDateTime(datestest);

                                            if (lbdiscontinue.Text != "")
                                            {
                                                if (lblreadmission.Text != "")
                                                {
                                                    if (Discontin <= dsdate && Readmin > dsdate)
                                                    {
                                                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Locked = true;

                                                    }
                                                }
                                                else if (Discontin <= dsdate)
                                                {
                                                    attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Locked = true;

                                                }
                                            }
                                        }
                                    }
                                    dummydate = dummydate.AddDays(1);
                                }
                                //if (ds2.Tables.Count > 0 && i < ds2.Tables[0].Rows.Count)
                                //{
                                //    if (cumd == Convert.ToInt16(Convert.ToString(ds2.Tables[0].Rows[i]["month_year"]).Trim()))
                                //    {
                                //        i++;
                                //    }
                                //}
                            }

                            //}
                            double totalConductedHours = (((morningConductedHrs * fnhrs) + (eveningConductedHrs * (NoHrs - fnhrs))) - unMarkedAttendance) - notConsidered;
                            double overAllConducetedDays = totalWorkingDays - notJoinDays;
                            double totalPresentDays = prsentdays - notJoinDays;
                            double percent = 0;
                            if (conducteddays != 0)
                            {
                                percent = Convert.ToDouble(prsentdays) / Convert.ToDouble(conducteddays) * 100;
                            }
                            double hourWisePercentage = 0;
                            if (totalConductedHours > 0)
                            {
                                hourWisePercentage = (totalPresentHours / totalConductedHours) * 100;
                                hourWisePercentage = Math.Round(hourWisePercentage, 2, MidpointRounding.AwayFromZero);
                            }
                            double dayWisePercentage = 0;
                            if (overAllConducetedDays > 0)
                            {
                                dayWisePercentage = (totalPresentDays / overAllConducetedDays) * 100;
                                dayWisePercentage = Math.Round(dayWisePercentage, 2, MidpointRounding.AwayFromZero);
                            }
                            name.Text = Convert.ToString(infods.Tables[0].Rows[0]["stud_name"]).Trim();
                            string yr = Convert.ToString(infods.Tables[0].Rows[0]["year"]).Trim();
                            string course = Convert.ToString(infods.Tables[0].Rows[0]["Course_Name"]).Trim();
                            string dept = Convert.ToString(infods.Tables[0].Rows[0]["dept_acronym"]).Trim();
                            string sc = Convert.ToString(infods.Tables[0].Rows[0]["Sections"]).Trim();

                            yr = Year(sem);
                            if (sc != "")
                            {
                                clas.Text = yr + " " + course + " " + dept + " " + sc;
                            }
                            else
                            {
                                clas.Text = yr + " " + course + " " + dept;
                            }
                            fullday.Text = fuldayabsentcount.ToString();
                            halfday.Text = halfdayabsentcount.ToString();
                            totdays.Text = absentdays.ToString();
                            odapplied.Text = oddays.ToString();
                            leaveapplied.Text = leavedays.ToString();
                            lblHrsWisePercentage.Text = string.Format("{0:0.00}", hourWisePercentage);
                            lblDaysWisePercentage.Text = string.Format("{0:0.00}", dayWisePercentage);
                            if (latd.Trim() != "")
                            {
                                string[] attndt = latd.Split(' ');
                                string[] ltd = attndt[0].Split('/');
                                DateTime dltd = Convert.ToDateTime(ltd[0] + "/" + ltd[1] + "/" + ltd[2]);
                                lastattndate.Text = dltd.ToString("dd/MM/yyyy");
                            }
                            std_info.Visible = true;

                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void loadattendanceNew1()
    {
        try
        {
            DateTime Admission_date = new DateTime();
            Admission_date = Convert.ToDateTime(Session["Admission_date"]);
            string studentcollegeCode = string.Empty;
            attnd_report.Sheets[0].SheetName = " ";
            attnd_report.Sheets[0].RowCount = 0;
            attnd_report.Sheets[0].ColumnCount = 0;
            attnd_report.SaveChanges();
            attnd_report.Sheets[0].AutoPostBack = false;
            strcomo1 = new string[] { "Select for All ", " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NJ", "S", "NCC", "HS", "PP", "SYOD", "COD", "OOD", "LA", "RAA" };
            strcomo = new string[] { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NJ", "S", "NCC", "HS", "PP", "SYOD", "COD", "OOD", "LA", "RAA" };
            objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
            objintcell.ShowButton = true;
            objintcell.AutoPostBack = true;
            objintcell.UseValue = true;
            objintcell.BackColor = Color.Gold;
            objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);
            // objcom.ShowButton = true; 
            objcom.BackColor = Color.DarkSeaGreen;
            // objcom.AutoPostBack = true ;
            objcom.UseValue = true;
            ds3.Clear();
            hat.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string batchyear = string.Empty;
                studentcollegeCode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]).Trim();//college_code
                string isredo1 = Convert.ToString(ds.Tables[0].Rows[0]["isRedo"]).Trim();

                if (isredo1 == "True")
                {
                    currentSemester = ddlSem.SelectedItem.Text;
                    string regsem = Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim();
                    //if (!string.IsNullOrEmpty(isredoold))
                    //{
                    //    currentSemester = isredoold;
                    //}
                    if (isredoold == "2")
                    {
                        hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim());
                        batchyear = Convert.ToString(ds.Tables[0].Rows[0]["regbatch"]).Trim();
                    }
                    else
                    {
                        hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim());
                        batchyear = Convert.ToString(ds.Tables[0].Rows[0]["appbatch"]).Trim();
                    }
                }
                else
                {
                    hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim());
                    batchyear = Convert.ToString(ds.Tables[0].Rows[0]["regbatch"]).Trim();
                }
                hat.Add("sem_val", ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim()));
                string sections = string.Empty;
                string strsec = string.Empty;
                string coursename = Convert.ToString(ds.Tables[0].Rows[0]["course_name"]).Trim();
                string deptname = Convert.ToString(ds.Tables[0].Rows[0]["acronym"]).Trim();



                if (string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim()) && Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() != "0" && Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() != "-1")
                {
                    sections = Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim();
                    strsec = "Sec" + sections;
                }
                string studentname = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]).Trim();
                string sem = ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim());
                //Modified By Srinath 25/4/2013
                // string rollno = ds.Tables[0].Rows[0]["roll_admit"].ToString();
                string rollno = Convert.ToString(ds.Tables[0].Rows[0]["Roll_no"]).Trim();
                DataSet infods = new DataSet();
                string infoquery = " select r.stud_name,c.Course_Name,dpt.dept_acronym,dpt.Dept_Name,r.Sections,r.Current_Semester as year  from Registration r,Degree d,Department dpt,Course c where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dpt.Dept_Code and r.Roll_No='" + rollno + "' order by r.Current_Semester desc ";
                infods = d2.select_method_wo_parameter(infoquery, "Text");
                ds3 = dacces2.select_method("period_attnd_schedule_sp", hat, "sp");
                if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                {
                    nohrs = int.Parse(Convert.ToString(ds3.Tables[0].Rows[0]["no_of_hrs_per_day"]).Trim());
                    first_half = int.Parse(Convert.ToString(ds3.Tables[0].Rows[0]["no_of_hrs_I_half_day"]).Trim());
                    second_half = int.Parse(Convert.ToString(ds3.Tables[0].Rows[0]["no_of_hrs_II_half_day"]).Trim());
                    Session["nohrs"] = nohrs;
                    if (nohrs != null)
                    {
                        attnd_report.Sheets[0].ColumnCount = nohrs + 1;
                        attnd_report.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
                        attnd_report.Sheets[0].Columns[0].Width = 100;
                        for (int i = 1; i <= (nohrs); i++)
                        {
                            attnd_report.Sheets[0].Columns[i].Width = 35;
                        }
                        attnd_report.Sheets[0].RowCount = rowct + 1;
                        attnd_report.Width = (50 * nohrs) + 670;
                        attnd_report.Sheets[0].Columns[0].BackColor = Color.Gray;
                        attnd_report.Sheets[0].RowHeader.Cells[0, 0].Text = " ";
                        attnd_report.Sheets[0].ColumnHeader.RowCount = rowct + 2;
                        attnd_report.Sheets[0].ColumnHeader.Columns.Count = 1;
                        attnd_report.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Left;
                        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(rowct, 0, rowct + 1, nohrs);
                        attnd_report.Sheets[0].ColumnHeader.Cells[rowct, 0].Text = "         " + "Degree:" + batchyear + "-" + coursename + "[" + deptname + "]" + "-" + "Sem  " + sem + "    " + strsec + "  RollNo:" + rollno + "  Name :" + studentname;
                        if (forschoolsetting == true)
                        {
                            attnd_report.Sheets[0].ColumnHeader.Cells[rowct, 0].Text = "         " + "Standard:" + batchyear + "-" + coursename + "[" + deptname + "]" + "-" + "Term  " + sem + "    " + strsec + "  RollNo:" + rollno + "  Name :" + studentname;
                        }
                        attnd_report.Sheets[0].ColumnHeader.Cells[rowct + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].ColumnHeader.Cells[rowct + 1, 0].VerticalAlign = VerticalAlign.Middle;
                        attnd_report.Sheets[0].ColumnHeader.Cells[rowct + 1, 0].Text = "Date";
                        for (int i = 1; i <= nohrs; i++)
                        {
                            attnd_report.Sheets[0].ColumnHeader.Columns.Count++;
                            attnd_report.Sheets[0].ColumnHeader.Cells[rowct + 1, i].Text = i.ToString();
                            attnd_report.ActiveSheetView.Columns[i].CellType = objcom;
                            attnd_report.Sheets[0].Cells[rowct - 1, i].CellType = objintcell;
                            // attnd_report.Sheets[0].Cells[0, 0].Locked = true;
                        }
                        attnd_report.Sheets[0].ColumnHeader.Columns.Count++;
                        attnd_report.Sheets[0].ColumnHeader.Cells[rowct + 1, nohrs + 1].Text = "Attendance Status";
                        attnd_report.Sheets[0].Columns[nohrs + 1].BackColor = Color.Gray;
                        attnd_report.Sheets[0].Columns[nohrs + 1].Width = 170;
                        attnd_report.Sheets[0].Columns[0].Locked = true;
                        attnd_report.Sheets[0].Columns[nohrs + 1].Locked = true;
                        string frdate_datetime = string.Empty;
                        string todate_datetime = string.Empty;
                        string[] from_split2 = (txtFromDate.Text).Split('/');
                        string[] to_split2 = (txtToDate.Text).Split('/');
                        if (from_split2.Length > 0)
                        {
                            fd = Convert.ToInt16(Convert.ToString(from_split2[0]).Trim());
                            fm = Convert.ToInt16(Convert.ToString(from_split2[1]).Trim());
                            fyy = Convert.ToInt16(Convert.ToString(from_split2[2]).Trim());
                            Session["fd"] = fd;
                            Session["fyy"] = fyy;
                        }
                        if (to_split2.Length > 0)
                        {
                            td = Convert.ToInt16(Convert.ToString(to_split2[0]).Trim());
                            tm = Convert.ToInt16(Convert.ToString(to_split2[1]).Trim());
                            tyy = Convert.ToInt16(Convert.ToString(to_split2[2]).Trim());
                            Session["td"] = td;
                        }
                        fcal = ((fyy * 12) + fm);
                        Session["fcal"] = fcal;
                        tcal = ((tyy * 12) + tm);
                        Session["tcal"] = tcal;
                        string get_rollno = GetFunction("select r.roll_no from registration r where " + get_roll_no_r + "");
                        string degcode = d2.GetFunction("select degree_code from registration  where Roll_No='" + get_rollno + "'");
                        string semester = d2.GetFunction("select Current_Semester from registration  where Roll_No ='" + get_rollno + "'");
                        string section = d2.GetFunction("select Sections from registration  where Roll_No ='" + get_rollno + "'");
                        ds2.Clear();
                        hat.Clear();
                        hat.Add("f_date", fcal);
                        hat.Add("t_date", tcal);
                        hat.Add("roll_no", Convert.ToString(get_rollno).Trim());
                        ds2 = dacces2.select_method("ATT_REPORTS_DETAILS", hat, "sp");
                        hat.Clear();
                        hat.Add("colege_code", Convert.ToString(studentcollegeCode).Trim());
                        DataSet dsattva = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                        Dictionary<string, string> dicattval = new Dictionary<string, string>();
                        if (dsattva.Tables.Count > 0 && dsattva.Tables[0].Rows.Count > 0)
                        {
                            for (int at = 0; at < dsattva.Tables[0].Rows.Count; at++)
                            {
                                string leavcode = Convert.ToString(dsattva.Tables[0].Rows[at]["leavecode"]).Trim();
                                string calc = Convert.ToString(dsattva.Tables[0].Rows[at]["calcflag"]).Trim();
                                if (!dicattval.ContainsKey(leavcode.Trim()))
                                {
                                    dicattval.Add(leavcode.Trim(), calc);
                                }
                            }
                        }
                        int NoHrs = 0;
                        int fnhrs = 0;
                        int anhrs = 0;
                        int minpresI = 0;
                        int minpresII = 0;
                        int minpresday = 0;
                        double prsentdays = 0;
                        double absentdays = 0;
                        double conducteddays = 0;
                        double oddays = 0;
                        double leavedays = 0;
                        int fuldayabsentcount = 0;
                        int halfdayabsentcount = 0;
                        double nohrsprsentperday = 0;
                        double noofdaypresen = 0;
                        hat.Clear();
                        hat.Add("degree_code", degcode);
                        hat.Add("sem_ester", int.Parse(((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(semester).Trim())));
                        ds5 = d2.select_method("period_attnd_schedule", hat, "sp");
                        if (ds5.Tables.Count > 0 && ds5.Tables[0].Rows.Count > 0)
                        {
                            NoHrs = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["PER DAY"]).Trim());
                            fnhrs = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["I_HALF_DAY"]).Trim());
                            anhrs = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["II_HALF_DAY"]).Trim());
                            minpresI = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["MIN PREE I DAY"]).Trim());
                            minpresII = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["MIN PREE II DAY"]).Trim());
                            minpresday = int.Parse(ds5.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
                            //minpresday = int.Parse(Convert.ToString(ds5.Tables[0].Rows[0]["PER DAY"]).Trim());

                        }
                        {
                            totmonth = fcal;
                            fflag = true;
                            int i = 0;
                            double k = 1;
                            dat = fd;
                            int k_temp = 0;

                            string latd = string.Empty;
                            dummydate = Convert.ToDateTime(Convert.ToString(dt1.ToShortDateString()).Trim());
                            Session["start_date"] = dummydate;
                            int morningConductedHrs = 0;
                            int eveningConductedHrs = 0;
                            int unMarkedAttendance = 0;
                            int notConsidered = 0;
                            double totalPresentHours = 0;
                            double notJoinDays = 0;
                            double totalWorkingDays = 0;

                            {
                                //for (k = k_temp; k <= endk; k++)
                                DateTime dtStartDate = new DateTime();
                                DateTime dtEndDate = new DateTime();
                                while (dummydate <= dt2)
                                {
                                    int forcount = 0;
                                    int pct = 0, act = 0, odct = 0, mlct = 0, sodct = 0, nssct = 0, hct = 0, njct = 0, sct = 0, lct = 0, nccct = 0, hsct = 0, ppct = 0, syodct = 0, codct = 0, oodct = 0, lact = 0, nect = 0, raact = 0;
                                    if (dummydate >= Admission_date)
                                    {
                                        double present = 0;
                                        int noofmorpresent = 0;
                                        int noofmorabsent = 0;
                                        int noofmornj = 0;
                                        int noofevepresent = 0;
                                        int noofeveabsent = 0;
                                        int noofevenj = 0;
                                        int noofmorcon = 0;
                                        int noofevecon = 0;
                                        int noofmorod = 0, noofmorleav = 0, noofeveod = 0, noofeveleav = 0;
                                        ddd = dummydate.ToString("ddd");
                                        string monthYear = Convert.ToString(((dummydate.Year * 12) + dummydate.Month));
                                        find_holiday(dummydate);
                                        if (holidayflag == false)
                                        {

                                            //----------------get date for display in spread
                                            string dummy_date = string.Empty;
                                            string date_text = string.Empty;
                                            dummy_date = dummydate.ToString();
                                            string[] dummy_split = dummy_date.Split(' ');
                                            string[] dummy_split2 = dummy_split[0].Split('/');
                                            //----------------------------set date
                                            attnd_report.Sheets[0].RowCount++;
                                            attnd_report.Sheets[0].RowHeader.Cells[attnd_report.Sheets[0].RowCount - 1, 0].Text = (attnd_report.Sheets[0].RowCount - 1).ToString();
                                            //-----------------------lock row for attnd security
                                            chk = daycheck(Convert.ToDateTime(dummydate));
                                            if (chk == false)
                                            {
                                                attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Locked = true;
                                            }
                                            date_text = Convert.ToString(dummy_split2[1]).Trim() + "/" + Convert.ToString(dummy_split2[0]).Trim() + "/" + Convert.ToString(dummy_split2[2]).Trim();
                                            attnd_report.Sheets[0].SetValue((attnd_report.Sheets[0].RowCount - 1), 0, Convert.ToString(date_text).Trim());
                                            attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dummy_split[0]).Trim();
                                            if (ds2.Tables.Count > 0 && i < ds2.Tables[0].Rows.Count)
                                            {
                                                DataView dvStudentAttendance = new DataView();
                                                ds2.Tables[0].DefaultView.RowFilter = "month_year='" + monthYear + "'";
                                                dvStudentAttendance = ds2.Tables[0].DefaultView;
                                                //if (cumd == Convert.ToInt16(Convert.ToString(ds2.Tables[0].Rows[i]["month_year"]).Trim()))
                                                if (dvStudentAttendance.Count > 0)
                                                {
                                                    int mfc = 0;
                                                    int msc = 0;
                                                    int lfc = 0;
                                                    int lsc = 0;
                                                    int pfc = 0;
                                                    int psc = 0;
                                                    int emptyfc = 0;
                                                    int emptysc = 0;
                                                    int unMark1 = 0;
                                                    int unMark2 = 0;
                                                    if (split_holiday_status_1 == 1 && split_holiday_status_2 == NoHrs)
                                                    {
                                                        morningConductedHrs += 1;
                                                        eveningConductedHrs += 1;
                                                    }
                                                    else if (split_holiday_status_1 == fnhrs + 1 && split_holiday_status_2 == NoHrs)
                                                    {
                                                        eveningConductedHrs += 1;
                                                    }
                                                    else if (split_holiday_status_1 == 1 && split_holiday_status_2 == fnhrs)
                                                    {
                                                        morningConductedHrs += 1;
                                                    }
                                                    for (int temp = split_holiday_status_1; temp <= split_holiday_status_2; temp++)
                                                    {
                                                        forcount++;
                                                        temp_val = "d" + dummy_split2[1] + "d" + temp;
                                                        if (dvStudentAttendance.Count > 0)
                                                        {
                                                            //if (dvStudentAttendance.Count > 0)
                                                            //{
                                                            bool freehr = isApplicableForFreeSpecial(Convert.ToString(studentcollegeCode).Trim(), batchyear, degcode, semester, section, dummydate.ToString("MM/dd/yyyy"), "0", temp.ToString(), NoHrs.ToString());

                                                            bool specialday = isApplicableForFreeSpecial(Convert.ToString(studentcollegeCode).Trim(), batchyear, degcode, semester, section, dummydate.ToString("MM/dd/yyyy"), "1", temp.ToString(), NoHrs.ToString());

                                                            string valueAtt = Convert.ToString(dvStudentAttendance[0][temp_val]).Trim();
                                                            Att_mark = string.Empty;
                                                            if (!string.IsNullOrEmpty(valueAtt) && valueAtt != "" && valueAtt != "0" && valueAtt != "7")
                                                            {
                                                                leave_code = (Convert.ToString(dvStudentAttendance[0][temp_val]).Trim());
                                                                Attmark(leave_code);
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].Text = Att_mark;
                                                                string clav = string.Empty;
                                                                if (dicattval.ContainsKey(leave_code.Trim()))
                                                                {
                                                                    clav = Convert.ToString(dicattval[leave_code.Trim()]).Trim();
                                                                }
                                                                if (temp <= fnhrs)
                                                                {
                                                                    if (leave_code == "3")
                                                                    {
                                                                        noofmorod++;
                                                                    }
                                                                    if (leave_code == "10")
                                                                    {
                                                                        noofmorleav++;
                                                                    }
                                                                    if (clav == "0")
                                                                    {
                                                                        noofmorpresent++;
                                                                        noofmorcon++;
                                                                        nohrsprsentperday++;
                                                                        totalPresentHours++;
                                                                    }
                                                                    else if (clav == "1")
                                                                    {
                                                                        noofmorabsent++;
                                                                        noofmorcon++;
                                                                    }
                                                                    else if (clav == "2")
                                                                    {
                                                                        noofmornj++;
                                                                        noofmorcon++;
                                                                        notConsidered++;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (clav == "0")
                                                                    {
                                                                        noofevepresent++;
                                                                        noofevecon++;
                                                                        nohrsprsentperday++;
                                                                        totalPresentHours++;
                                                                    }
                                                                    else if (clav == "1")
                                                                    {
                                                                        noofeveabsent++;
                                                                        noofevecon++;
                                                                    }
                                                                    else if (clav == "2")
                                                                    {
                                                                        noofevenj++;
                                                                        noofevecon++;
                                                                        notConsidered++;
                                                                    }
                                                                    if (leave_code == "3")
                                                                    {
                                                                        noofeveod++;
                                                                    }
                                                                    if (leave_code == "10")
                                                                    {
                                                                        noofeveleav++;
                                                                    }
                                                                }
                                                                if (temp <= fnhrs)
                                                                {
                                                                    if (Att_mark == "A")
                                                                    {
                                                                        mfc++;
                                                                    }
                                                                }
                                                                else if (temp <= NoHrs)
                                                                {
                                                                    if (Att_mark == "A")
                                                                    {
                                                                        msc++;
                                                                    }
                                                                }
                                                                if (temp <= fnhrs)
                                                                {
                                                                    if (Att_mark == "L")
                                                                    {
                                                                        lfc++;
                                                                    }
                                                                }
                                                                else if (temp <= NoHrs)
                                                                {
                                                                    if (Att_mark == "L")
                                                                    {
                                                                        lsc++;
                                                                    }
                                                                }
                                                                if (temp <= fnhrs)
                                                                {
                                                                    if (Att_mark == "P")
                                                                    {
                                                                        pfc++;
                                                                    }
                                                                }
                                                                else if (temp <= NoHrs)
                                                                {
                                                                    if (Att_mark == "P")
                                                                    {
                                                                        psc++;
                                                                    }
                                                                }
                                                                if (Att_mark == "A")
                                                                {
                                                                    act++;
                                                                }
                                                                else if (Att_mark == "P")
                                                                {
                                                                    pct++;
                                                                }
                                                                else if (Att_mark == "L")
                                                                {
                                                                    lct++;
                                                                }
                                                                else if (Att_mark == "H")
                                                                {
                                                                    hct++;
                                                                }
                                                                else if (Att_mark == "OD")
                                                                {
                                                                    odct++;
                                                                }
                                                                else if (Att_mark == "ML")
                                                                {
                                                                    mlct++;
                                                                }
                                                                else if (Att_mark == "SOD")
                                                                {
                                                                    sodct++;
                                                                }
                                                                else if (Att_mark == "NSS")
                                                                {
                                                                    nssct++;
                                                                }
                                                                else if (Att_mark == "NJ")
                                                                {
                                                                    njct++;
                                                                }
                                                                else if (Att_mark == "S")
                                                                {
                                                                    sct++;
                                                                }
                                                                else if (Att_mark == "NCC")
                                                                {
                                                                    nccct++;
                                                                }
                                                                else if (Att_mark == "HS")
                                                                {
                                                                    hsct++;
                                                                }
                                                                else if (Att_mark == "PP")
                                                                {
                                                                    ppct++;
                                                                }
                                                                else if (Att_mark == "SYOD")
                                                                {
                                                                    syodct++;
                                                                }
                                                                else if (Att_mark == "COD")
                                                                {
                                                                    codct++;
                                                                }
                                                                else if (Att_mark == "OOD")
                                                                {
                                                                    oodct++;
                                                                }
                                                                else if (Att_mark == "LA")
                                                                {
                                                                    lact++;
                                                                }
                                                                else if (Att_mark == "NE")
                                                                {
                                                                    nect++;
                                                                }
                                                                else if (Att_mark == "RAA")
                                                                {
                                                                    raact++;
                                                                }
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].Tag = leave_code;
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.DarkSeaGreen;
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = Color.White;
                                                                btnsave.Visible = true;
                                                            }
                                                            else
                                                            {
                                                                unMarkedAttendance++;
                                                                if (temp <= fnhrs)
                                                                {
                                                                    unMark1++;
                                                                    if (Att_mark == "")
                                                                    {
                                                                        emptyfc++;
                                                                    }
                                                                }
                                                                else if (temp <= NoHrs)
                                                                {
                                                                    unMark2++;
                                                                    if (Att_mark == "")
                                                                    {
                                                                        emptysc++;
                                                                    }
                                                                }
                                                            }
                                                            if (freehr)
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#FF00FF"); //Color.Red;
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].Text = "FH";
                                                            }
                                                            else if (specialday)
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#432F5C"); //Color.Lavender;
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].Text = "SH";
                                                            }
                                                            else if (Att_mark == "P")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#008000"); //Color.Green;
                                                                latd = dummydate.ToString();
                                                            }
                                                            else if (Att_mark == "A")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#FF0000"); //Color.Red;//#FF0000	
                                                            }
                                                            else if (Att_mark == "H")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#800000"); //Color.Maroon;
                                                            }
                                                            else if (Att_mark == "OD")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#0000FF"); //Color.Blue;
                                                            }
                                                            else if (Att_mark == "SOD")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#adff2f"); //Color.GreenYellow;
                                                            }
                                                            else if (Att_mark == "ML")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#E9967A"); // Color.DarkSalmon;
                                                            }
                                                            else if (Att_mark == "NSS")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#DAA520"); //Color.Goldenrod;
                                                            }
                                                            else if (Att_mark == "L")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#0080ff");
                                                            }
                                                            else if (Att_mark == "NCC")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#EE82EE"); //Color.Violet;
                                                            }
                                                            else if (Att_mark == "HS")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#708090"); //Color.SlateGray;
                                                            }
                                                            else if (Att_mark == "PP")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#FFC0CB"); // Color.Pink;
                                                            }
                                                            else if (Att_mark == "SYOD")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#32cd32"); //Color.LimeGreen;
                                                            }
                                                            else if (Att_mark == "COD")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#D2B48C"); //Color.Tan;
                                                            }
                                                            else if (Att_mark == "OOD")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#f5deb3"); // Color.Wheat;
                                                            }
                                                            else if (Att_mark == "NJ")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#8b4513"); // Color.SaddleBrown;
                                                            }
                                                            else if (Att_mark == "S")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#000000"); // Color.Black;
                                                            }
                                                            else if (Att_mark == "RAA")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].ForeColor = ColorTranslator.FromHtml("#FFFF00"); // Color.Yellow;
                                                            }
                                                            else if (Att_mark == "")
                                                            {
                                                                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = ColorTranslator.FromHtml("#7B68EE"); //Color.MediumSlateBlue;
                                                            }
                                                            //}
                                                            //else
                                                            //{
                                                            //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.DarkSeaGreen;
                                                            //}
                                                        }
                                                        else
                                                        {
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.DarkSeaGreen;//ColorTranslator.FromHtml("#008000");
                                                        }
                                                    }
                                                    #region Hide
                                                    //if (ds5.Tables[0].Rows.Count > 0)
                                                    //{
                                                    //    NoHrs = int.Parse(ds5.Tables[0].Rows[0]["PER DAY"].ToString());
                                                    //    fnhrs = int.Parse(ds5.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                                    //    anhrs = int.Parse(ds5.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                                    //    minpresI = int.Parse(ds5.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                                    //    minpresII = int.Parse(ds5.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                                    //}
                                                    //int cond1 = minpresI + minpresII;
                                                    //int test1 = fnhrs - minpresI;
                                                    //int test2 = anhrs - minpresII;
                                                    //string attendance =string.Empty;
                                                    ////bool freehr = isApplicableForFreeSpecial(Session["collegecode"].ToString(), batchyear, degcode, semester, section, dummydate.ToString("MM/dd/yyyy"), "0");
                                                    ////bool specialday = isApplicableForFreeSpecial(Session["collegecode"].ToString(), batchyear, degcode, semester, section, dummydate.ToString("MM/dd/yyyy"), "1");
                                                    //if (cond1 <= act)
                                                    //{
                                                    //    attendance = "FA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //    fulldayabsent++;
                                                    //}
                                                    //else if (pct >= 1 && pct < cond1)
                                                    //{
                                                    //    //if (pfc >= 1 && psc >= 1)
                                                    //    //{
                                                    //    //    attendance = "FP";
                                                    //    //}
                                                    //    //else
                                                    //    //{
                                                    //    //    attendance = "HP";
                                                    //    //}
                                                    //    if (mfc > test1 && msc > test2)
                                                    //    {
                                                    //        attendance = "FA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //        fulldayabsent++;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        if (mfc == 0 && msc == 0 && pfc > test1 && psc > test2)
                                                    //        {
                                                    //            if ((emptyfc > test1 && emptysc > test2))
                                                    //            {
                                                    //                attendance = "FA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //                fulldayabsent++;
                                                    //            }
                                                    //            else if ((emptyfc > test1 || emptysc > test2))
                                                    //            {
                                                    //                attendance = "HA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                halfdayabsent++;
                                                    //            }
                                                    //            else if ((pfc <= fnhrs || psc <= anhrs))
                                                    //            {
                                                    //                attendance = "HA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                halfdayabsent++;
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                attendance = "FP";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //                fpresent++;
                                                    //            }
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            if ((pfc == fnhrs || psc == anhrs) && odct >= 1)
                                                    //            {
                                                    //                attendance = "FP";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //                fpresent++;
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                if ((emptyfc > test1 || emptysc > test2) && (mfc > test1 || msc > test2))
                                                    //                {
                                                    //                    attendance = "FA";
                                                    //                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //                    fulldayabsent++;
                                                    //                }
                                                    //                else if ((emptyfc > test1 && emptysc > test2) && (pfc > test1 || psc > test2))
                                                    //                {
                                                    //                    attendance = "FA";
                                                    //                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //                    fulldayabsent++;
                                                    //                }
                                                    //                else
                                                    //                {
                                                    //                    attendance = "HA";
                                                    //                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                    halfdayabsent++;
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                    //else if (pct > test1 && pct == cond1)
                                                    //{
                                                    //    attendance = "FP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //    fpresent++;
                                                    //}
                                                    //else if (odct >= 1 && odct < cond1)
                                                    //{
                                                    //    if ((mfc > test1 || msc > test2) && odct >= 1)
                                                    //    {
                                                    //        //attendance = "OD";
                                                    //        attendance = "HA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Blue;
                                                    //        odappl = odappl + 0.5;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        //attendance = "OD";
                                                    //        attendance = "FP";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Blue;
                                                    //        odappl++;
                                                    //    }
                                                    //}
                                                    //else if (odct >= 1 && odct == cond1)
                                                    //{
                                                    //    //attendance = "OD";
                                                    //    attendance = "FP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Blue;
                                                    //    odappl++;
                                                    //}
                                                    //else if (mlct >= 1 && mlct < cond1)
                                                    //{
                                                    //    attendance = "HML";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //}
                                                    //else if (mlct >= 1 && mlct == cond1)
                                                    //{
                                                    //    attendance = "FML";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //}
                                                    //else if (sodct >= 1 && sodct < cond1)
                                                    //{
                                                    //    attendance = "HSOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.GreenYellow;
                                                    //}
                                                    //else if (sodct >= 1 && sodct == cond1)
                                                    //{
                                                    //    attendance = "FSOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.GreenYellow;
                                                    //}
                                                    //else if (nssct >= 1 && nssct < cond1)
                                                    //{
                                                    //    attendance = "HNSS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Goldenrod;
                                                    //}
                                                    //else if (nssct >= 1 && nssct == cond1)
                                                    //{
                                                    //    attendance = "FNSS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Goldenrod;
                                                    //}
                                                    //else if (njct >= 1 && njct < cond1)
                                                    //{
                                                    //    attendance = "HNJ";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SaddleBrown;
                                                    //}
                                                    //else if (njct >= 1 && njct == cond1)
                                                    //{
                                                    //    attendance = "FNJ";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SaddleBrown;
                                                    //}
                                                    //else if (sct >= 1 && sct < cond1)
                                                    //{
                                                    //    attendance = "HS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Black;
                                                    //}
                                                    //else if (sct >= 1 && sct == cond1)
                                                    //{
                                                    //    attendance = "FS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Black;
                                                    //}
                                                    //else if (nccct >= 1 && nccct < cond1)
                                                    //{
                                                    //    attendance = "HNCC";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Violet;
                                                    //}
                                                    //else if (nccct >= 1 && nccct == cond1)
                                                    //{
                                                    //    attendance = "FNCC";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Violet;
                                                    //}
                                                    //else if (hsct >= 1 && hsct < cond1)
                                                    //{
                                                    //    attendance = "HHS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SlateGray;
                                                    //}
                                                    //else if (hsct >= 1 && hsct == cond1)
                                                    //{
                                                    //    attendance = "FHS";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.SlateGray;
                                                    //}
                                                    //else if (ppct >= 1 && ppct < cond1)
                                                    //{
                                                    //    attendance = "HPP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Pink;
                                                    //}
                                                    //else if (ppct >= 1 && ppct == cond1)
                                                    //{
                                                    //    attendance = "FPP";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Pink;
                                                    //}
                                                    //else if (syodct >= 1 && syodct < cond1)
                                                    //{
                                                    //    attendance = "HSYOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.LimeGreen;
                                                    //}
                                                    //else if (syodct >= 1 && syodct == cond1)
                                                    //{
                                                    //    attendance = "FSYOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.LimeGreen;
                                                    //}
                                                    //else if (codct >= 1 && codct < cond1)
                                                    //{
                                                    //    attendance = "HCOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Tan;
                                                    //}
                                                    //else if (codct >= 1 && codct == cond1)
                                                    //{
                                                    //    attendance = "FCOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Tan;
                                                    //}
                                                    //else if (oodct >= 1 && oodct < cond1)
                                                    //{
                                                    //    attendance = "HOOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Wheat;
                                                    //}
                                                    //else if (oodct >= 1 && oodct == cond1)
                                                    //{
                                                    //    attendance = "FOOD";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Wheat;
                                                    //}
                                                    //else if (lact >= 1 && lact < cond1)
                                                    //{
                                                    //    attendance = "HLA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Indigo;
                                                    //}
                                                    //else if (lact >= 1 && lact == cond1)
                                                    //{
                                                    //    attendance = "FLA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Indigo;
                                                    //}
                                                    //else if (nect >= 1 && nect < cond1)
                                                    //{
                                                    //    attendance = "HNE";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Magenta;
                                                    //}
                                                    //else if (nect >= 1 && nect == cond1)
                                                    //{
                                                    //    attendance = "FNE";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Magenta;
                                                    //}
                                                    //else if (raact >= 1 && raact < cond1)
                                                    //{
                                                    //    attendance = "HRAA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Yellow;
                                                    //}
                                                    //else if (raact >= 1 && raact == cond1)
                                                    //{
                                                    //    attendance = "FRAA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Yellow;
                                                    //}
                                                    //else if (act >= 1 && act < cond1)
                                                    //{
                                                    //    if ((fnhrs - minpresI) != 0 && (fnhrs - minpresI) < act && (anhrs - minpresII) != 0 && (anhrs - minpresII) < act)
                                                    //    {
                                                    //        attendance = "FA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //        fulldayabsent++;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        if (mfc > test1 && msc > test2)
                                                    //        {
                                                    //            attendance = "FA";
                                                    //            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Red;
                                                    //            fulldayabsent++;
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            if ((pfc == fnhrs || psc == anhrs) && odct >= 1)
                                                    //            {
                                                    //                attendance = "FP";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Green;
                                                    //                fpresent++;
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                attendance = "HA";
                                                    //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.DarkSalmon;
                                                    //                halfdayabsent++;
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                    //else if (lct >= 1 && lct == cond1)
                                                    //{
                                                    //    //attendance = "FL";
                                                    //    attendance = "FA";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#0080ff");
                                                    //    leaveappl++;
                                                    //}
                                                    //else if (lct >= 1 && lct < cond1)
                                                    //{
                                                    //    if (lfc >= 1 && lsc >= 1)
                                                    //    {
                                                    //        //attendance = "FL";
                                                    //        attendance = "FA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#0080ff");
                                                    //        leaveappl++;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        //attendance = "HL";
                                                    //        attendance = "HA";
                                                    //        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Chocolate;
                                                    //        //leaveappl = leaveappl + Convert.ToInt32(0.5);
                                                    //        leaveappl++;
                                                    //    }
                                                    //}
                                                    //else if (hct >= 1 && hct < cond1)
                                                    //{
                                                    //    attendance = "HH";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Maroon;
                                                    //}
                                                    //else if (hct >= 1 && hct == cond1)
                                                    //{
                                                    //    attendance = "FH";
                                                    //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = Color.Maroon;
                                                    //}
                                                    //if (attendance != "")
                                                    //{
                                                    //    conducteddays++;
                                                    //}
                                                    //}
                                                    #endregion Hide
                                                    bool isDayWiseCalcAttendance = false;
                                                    if (Session["attdaywisecla"] != null && Session["attdaywisecla"].ToString() == "1")
                                                    {
                                                        isDayWiseCalcAttendance = true;
                                                    }
                                                    double absentday = 0;
                                                    string attendance = string.Empty;
                                                    int njhr = noofmornj + noofevenj;
                                                    double presentDays = 0;
                                                    double absentDays = 0;
                                                    double noOfPresentHourPerDay = noofmorpresent + noofmornj + noofevepresent + noofevenj;
                                                    nohrsprsentperday = nohrsprsentperday + noofmornj + noofevenj;
                                                    if (noofmorpresent + noofmornj >= minpresI)
                                                    {
                                                        present = 0.5;
                                                        prsentdays = prsentdays + 0.5;
                                                        noofdaypresen = 0.5;
                                                    }
                                                    else if (noofmorabsent >= 1)
                                                    {
                                                        absentday = 0.5;
                                                        absentdays = absentdays + 0.5;
                                                        //absentday = 1;
                                                        //absentdays = absentdays + 1;
                                                    }

                                                    if (noofmornj >= minpresI)
                                                    {
                                                        notJoinDays += 0.5;
                                                    }

                                                    if (noofevepresent + noofevenj >= minpresII)
                                                    {
                                                        present = present + 0.5;
                                                        prsentdays = prsentdays + 0.5;
                                                        noofdaypresen = noofdaypresen + 0.5;
                                                    }
                                                    else if (noofeveabsent >= 1)
                                                    {
                                                        absentday = absentday + 0.5;
                                                        absentdays = absentdays + 0.5;
                                                        //absentday = absentday + 1;
                                                        //absentdays = absentdays + 1;
                                                    }

                                                    if (noofevenj >= minpresII)
                                                    {
                                                        notJoinDays += 0.5;
                                                    }
                                                    if (fnhrs - unMark1 >= minpresI)
                                                    {
                                                        totalWorkingDays += 0.5;
                                                    }
                                                    if ((NoHrs - fnhrs) - unMark2 >= minpresII)
                                                    {
                                                        totalWorkingDays += 0.5;
                                                    }
                                                    presentDays = present;
                                                    absentDays = absentday;
                                                    double totalNoOfAbsent = noofmorabsent + noofeveabsent;
                                                    if (isDayWiseCalcAttendance)
                                                    {
                                                        double minimumAbsent = nohrs - minpresday;
                                                        if (totalNoOfAbsent > minimumAbsent)
                                                        {
                                                            presentDays = 0;
                                                            absentDays = 1;
                                                        }
                                                    }
                                                    if (Session["attdaywisecla"] != null && Session["attdaywisecla"].ToString() == "1")
                                                    {
                                                        //if (noOfPresentHourPerDay > minpresday)
                                                        //{
                                                        //    presentDays = 1;
                                                        //    absentDays = 0;
                                                        //}
                                                        //if (noOfPresentHourPerDay < minpresday)
                                                        //{
                                                        //    presentDays -= noofdaypresen;
                                                        //    absentDays += noofdaypresen;
                                                        //}
                                                        if (nohrsprsentperday < minpresday)
                                                        {
                                                            prsentdays = prsentdays - noofdaypresen;
                                                            absentdays = absentdays + noofdaypresen;
                                                            //presentDays -= noofdaypresen;
                                                            //absentDays += noofdaypresen;
                                                        }
                                                    }
                                                    nohrsprsentperday = 0;
                                                    noofdaypresen = 0;
                                                    if (noofmorod >= minpresI)
                                                    {
                                                        oddays = oddays + 0.5;
                                                    }
                                                    if (noofeveod >= minpresII)
                                                    {
                                                        oddays = oddays + 0.5;
                                                    }
                                                    if (noofmorleav >= minpresI)
                                                    {
                                                        leavedays = leavedays + 0.5;
                                                    }

                                                    if (noofeveleav >= minpresII)
                                                    {
                                                        leavedays = leavedays + 0.5;
                                                    }
                                                    if (noofmorcon >= minpresI)
                                                    {
                                                        conducteddays = conducteddays + 0.5;
                                                    }
                                                    if (noofevecon >= minpresII)
                                                    {
                                                        conducteddays = conducteddays + 0.5;
                                                    }

                                                    if (noofmorcon >= minpresI || noofevecon <= minpresII)
                                                    {
                                                        if (presentDays == 1)
                                                        {
                                                            attendance = "FP";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#008000"); // Color.Green;
                                                        }
                                                        else if (absentDays == 0.5)
                                                        {
                                                            attendance = "HA";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#D2691E"); // Color.Chocolate;
                                                            halfdayabsentcount++;
                                                        }
                                                        else if (absentDays >= 1)
                                                        {
                                                            attendance = "FA";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FF0000"); // Color.Red;
                                                            fuldayabsentcount++;
                                                        }
                                                        //else if (absentday == 1)
                                                        //{
                                                        //    attendance = "FA";
                                                        //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FF0000"); // Color.Red;
                                                        //    fuldayabsentcount++;
                                                        //}
                                                        else if (presentDays == 0.5 && absentDays == 0)
                                                        {
                                                            attendance = "HP";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FFC0CB"); // Color.Pink;
                                                        }

                                                        //if (present == 1)
                                                        //{
                                                        //    attendance = "FP";
                                                        //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#008000"); // Color.Green;
                                                        //}
                                                        //else if (absentday == 0.5)
                                                        //{
                                                        //    attendance = "HA";
                                                        //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#D2691E"); // Color.Chocolate;
                                                        //    halfdayabsentcount++;
                                                        //}
                                                        //else if (absentday >= 1)
                                                        //{
                                                        //    attendance = "FA";
                                                        //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FF0000"); // Color.Red;
                                                        //    fuldayabsentcount++;
                                                        //}
                                                        ////else if (absentday == 1)
                                                        ////{
                                                        ////    attendance = "FA";
                                                        ////    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FF0000"); // Color.Red;
                                                        ////    fuldayabsentcount++;
                                                        ////}
                                                        //else if (present == 0.5 && absentday == 0)
                                                        //{
                                                        //    attendance = "HP";
                                                        //    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].ForeColor = ColorTranslator.FromHtml("#FFC0CB"); // Color.Pink;
                                                        //}
                                                    }
                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), forcount + 1].Text = attendance;
                                                    if (split_holiday_status_3 != 0 && split_holiday_status_4 != 0)
                                                    {
                                                        for (int holi_day = split_holiday_status_3; holi_day <= split_holiday_status_4; holi_day++)
                                                        {
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), holi_day].Text = "HD";
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), holi_day].BackColor = Color.Red;
                                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), holi_day].Locked = true;
                                                        }
                                                    }
                                                    row_head_cnt++;
                                                }
                                                else
                                                {
                                                    for (int temp = 1; temp <= nohrs; temp++)
                                                    {
                                                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.MediumSlateBlue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int temp = 1; temp <= nohrs; temp++)
                                                {
                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), temp].BackColor = Color.MediumSlateBlue;
                                                }
                                            }
                                        }
                                    }
                                    dummydate = dummydate.AddDays(1);
                                }
                                //if (ds2.Tables.Count > 0 && i < ds2.Tables[0].Rows.Count)
                                //{
                                //    if (cumd == Convert.ToInt16(Convert.ToString(ds2.Tables[0].Rows[i]["month_year"]).Trim()))
                                //    {
                                //        i++;
                                //    }
                                //}
                            }
                            rowct = attnd_report.Sheets[0].RowCount;
                            //}
                            double totalConductedHours = (((morningConductedHrs * fnhrs) + (eveningConductedHrs * (NoHrs - fnhrs))) - unMarkedAttendance) - notConsidered;
                            double overAllConducetedDays = totalWorkingDays - notJoinDays;
                            double totalPresentDays = prsentdays - notJoinDays;
                            double percent = 0;
                            if (conducteddays != 0)
                            {
                                percent = Convert.ToDouble(prsentdays) / Convert.ToDouble(conducteddays) * 100;
                            }
                            double hourWisePercentage = 0;
                            if (totalConductedHours > 0)
                            {
                                hourWisePercentage = (totalPresentHours / totalConductedHours) * 100;
                                hourWisePercentage = Math.Round(hourWisePercentage, 2, MidpointRounding.AwayFromZero);
                            }
                            double dayWisePercentage = 0;
                            if (overAllConducetedDays > 0)
                            {
                                dayWisePercentage = (totalPresentDays / overAllConducetedDays) * 100;
                                dayWisePercentage = Math.Round(dayWisePercentage, 2, MidpointRounding.AwayFromZero);
                            }
                            name.Text = Convert.ToString(infods.Tables[0].Rows[0]["stud_name"]).Trim();
                            string yr = Convert.ToString(infods.Tables[0].Rows[0]["year"]).Trim();
                            string course = Convert.ToString(infods.Tables[0].Rows[0]["Course_Name"]).Trim();
                            string dept = Convert.ToString(infods.Tables[0].Rows[0]["dept_acronym"]).Trim();
                            string sc = Convert.ToString(infods.Tables[0].Rows[0]["Sections"]).Trim();
                            yr = Year(sem);
                            if (sc != "")
                            {
                                clas.Text = yr + " " + course + " " + dept + " " + sc;
                            }
                            else
                            {
                                clas.Text = yr + " " + course + " " + dept;
                            }
                            fullday.Text = fuldayabsentcount.ToString();
                            halfday.Text = halfdayabsentcount.ToString();
                            totdays.Text = absentdays.ToString();
                            odapplied.Text = oddays.ToString();
                            leaveapplied.Text = leavedays.ToString();
                            lblHrsWisePercentage.Text = string.Format("{0:0.00}", hourWisePercentage);
                            lblDaysWisePercentage.Text = string.Format("{0:0.00}", dayWisePercentage);
                            if (latd.Trim() != "")
                            {
                                string[] attndt = latd.Split(' ');
                                string[] ltd = attndt[0].Split('/');
                                DateTime dltd = Convert.ToDateTime(ltd[0] + "/" + ltd[1] + "/" + ltd[2]);
                                lastattndate.Text = dltd.ToString("dd/MM/yyyy");
                            }
                            std_info.Visible = true;

                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void attendanceCalculation(string rollNo, DateTime dtFromDate, DateTime dtToDate, string semester = null)
    {
        try
        {
            DataSet dsStudentDetails = new DataSet();
            DataSet dsSemesterInfo = new DataSet();
            string qry = "select clg.collname,r.college_code,c.Course_Id,dt.Dept_Code,r.Batch_Year,r.degree_code,r.Current_Semester,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,isnull(r.serialno,'0') as serialno,r.App_No,r.Roll_Admit,r.Reg_No,r.Roll_No,r.Stud_Name,ltrim(rtrim(isnull(r.Sections,''))) as Sections,r.CC,r.Exam_Flag,r.DelFlag, case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,''))) else '' end else c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,''))) else '' end end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,''))) else '' end else c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,''))) else '' end end end as DegreeDetails,CONVERT(varchar(50), r.Adm_Date,103) as AdmitionDate from Registration r,Course c,Degree dg,Department dt,collinfo clg where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=c.college_code and dt.college_code=clg.college_code and clg.college_code=r.college_code and r.college_code=dg.college_code and r.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.Degree_Code=r.degree_code and r.Roll_No='" + rollNo + "'";
            dsStudentDetails = d2.select_method_wo_parameter(qry, "Text");
            if (dsStudentDetails.Tables.Count > 0 && dsStudentDetails.Tables[0].Rows.Count > 0)
            {
            }
            else
            {
            }
        }
        catch
        {
        }
    }

    protected string Year(string sem)
    {
        string Year = string.Empty;
        try
        {
            if (sem == "1" || sem == "2")
                Year = "I";
            else if (sem == "3" || sem == "4")
                Year = "II";
            else if (sem == "5" || sem == "6")
                Year = "III";
            else if (sem == "7" || sem == "8")
                Year = "IV";
            else if (sem == "9" || sem == "10")
                Year = "V";
        }
        catch { }
        return Year;
    }

    public bool isApplicableForFreeSpecial(string coll_code, string batch, string degree_code, string sem, string sec, string entrydate, string attendtype, string hour = null, string maxHr = null)
    {
        try
        {
            bool isApplicableForFreeSpecial = false;
            string qryPeriod = string.Empty;
            int Period = 0;
            int maxHrs = 0;
            bool isValidHr = hour != null ? int.TryParse(hour, out Period) : false;
            bool isValidMaxHr = maxHr != null ? int.TryParse(maxHr, out maxHrs) : false;
            if (isValidHr && isValidMaxHr && hour != null && maxHr != null && !string.IsNullOrEmpty(hour) && !string.IsNullOrEmpty(maxHr) && hour != "0" && Period > 0 && maxHr != "0" && maxHrs > 0 && Period <= maxHrs)
            {
                qryPeriod = " and period='" + hour + "'";
            }
            if (!string.IsNullOrEmpty(coll_code) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(degree_code) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(entrydate) && !string.IsNullOrEmpty(attendtype) && coll_code != "" && batch != "" && degree_code != "" && sem != "" && entrydate != "" && attendtype != "")
            {
                int tot;
                string qrySection = string.Empty;
                if (!string.IsNullOrEmpty(sec))
                {
                    qrySection = " and section='" + sec + "' ";
                }
                string appqry = "select Count(*) from tbl_spl_attendace where college_code='" + coll_code + "' and batch_year='" + batch + "' and degree_code='" + degree_code + "' and semester='" + sem + "' and entry_date='" + entrydate + "' and attype='" + attendtype + "'" + qrySection + qryPeriod;
                string totcount = d2.GetFunctionv(appqry);
                bool isvalid = int.TryParse(totcount, out tot);
                if (isvalid)
                {
                    if (tot > 0)
                        isApplicableForFreeSpecial = true;
                    else
                        isApplicableForFreeSpecial = false;
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
            errmsg.Visible = true;
            errmsg.Text = ex.Message;
            pageddltxt.Text = string.Empty;
            return false;
        }
    }

    protected string getAppNo(string rollno, ref string appNo, ref byte isStatus)
    {
        appNo = string.Empty;
        isStatus = 0;
        string qry = string.Empty;
        try
        {
            //if (optionddl.SelectedIndex == 0)
            //    appno = dacces2.GetFunction("select app_no from registration where roll_no='" + rollno + "' and college_code='" + Session["collegecode"].ToString() + "'");
            //else if (optionddl.SelectedIndex == 1)
            //    appno = dacces2.GetFunction("select app_no from registration where reg_no='" + rollno + "' and college_code='" + Session["collegecode"].ToString() + "'");
            //else if (optionddl.SelectedIndex == 2)
            //    appno = dacces2.GetFunction("select app_no from registration where roll_admit='" + rollno + "' and college_code='" + Session["collegecode"].ToString() + "'");
            int selectedIndex = optionddl.SelectedIndex;
            bool isAvail = false;
            switch (selectedIndex)
            {
                case 0:
                    appNo = dacces2.GetFunction("select app_no from registration where roll_no='" + rollno + "' and college_code='" + Session["collegecode"].ToString() + "'");
                    break;
                case 1:
                    appNo = dacces2.GetFunction("select app_no from registration where reg_no='" + rollno + "' and college_code='" + Session["collegecode"].ToString() + "'");
                    break;
                case 2:
                    appNo = dacces2.GetFunction("select app_no from registration where roll_admit='" + rollno + "' and college_code='" + Session["collegecode"].ToString() + "'");
                    break;
            }
            if (string.IsNullOrEmpty(appNo) || appNo.Trim() == "0")
            {
                switch (selectedIndex)
                {
                    case 0:
                        appNo = dacces2.GetFunction("select app_no from registration where roll_no='" + rollno + "'");
                        break;
                    case 1:
                        appNo = dacces2.GetFunction("select app_no from registration where reg_no='" + rollno + "'");
                        break;
                    case 2:
                        appNo = dacces2.GetFunction("select app_no from registration where roll_admit='" + rollno + "'");
                        break;
                }
                if (!string.IsNullOrEmpty(appNo) && appNo.Trim() != "0")
                {
                    isStatus = 2;
                }
                else
                {
                    isStatus = 0;
                }
            }
            else
            {
                isStatus = 1;
            }
        }
        catch { }
        return appNo;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string leave_flag = string.Empty;
            //int k = 0;
            //int row = 0;
            attnd_report.SaveChanges();
            //nohrs = int.Parse(Session["nohrs"].ToString());
            //fcal = int.Parse(Session["fcal"].ToString());
            //tcal = int.Parse(Session["tcal"].ToString());
            //fd = int.Parse(Session["fd"].ToString());
            //td = int.Parse(Session["td"].ToString());
            //fyy = int.Parse(Session["fyy"].ToString());
            //dummydate = DateTime.Parse(Session["start_date"].ToString());
            //int xx = 0;
            //xx = attnd_report.Sheets[0].RowCount;
            string appNo = string.Empty;
            string rollno = txtrollno.Text.Trim().ToString();
            string sem = Convert.ToString(ddlSem.SelectedValue);
            if (optionddl.SelectedIndex == 1)//barath 01.02.17
                rollno = dacces2.GetFunction("select roll_no from registration where reg_no='" + rollno + "' and college_code='" + Session["collegecode"].ToString() + "'");
            else if (optionddl.SelectedIndex == 2)
                rollno = dacces2.GetFunction("select roll_no from registration where roll_admit='" + rollno + "' and college_code='" + Session["collegecode"].ToString() + "'");

            Boolean noentryflag = false;
            for (int r = 1; r < attnd_report.Sheets[0].RowCount; r++)
            {
                string date = attnd_report.Sheets[0].Cells[r, 0].Text.ToString();
                string[] spd1 = date.Split('/');
                string curDate = spd1[1] + "/" + spd1[0] + "/" + spd1[2];
                appNo = string.Empty;
                byte isStatus = 0;
                string ODEntry = string.Empty;
                appNo = getAppNo(txtrollno.Text.Trim().ToString(), ref appNo, ref isStatus);
                DataTable dtOnduty = dirAcc.selectDataTable("select * from Onduty_Stud  where roll_no='" + rollno + "' and (Fromdate>='" + curDate + "' or  Todate>='" + curDate + "') and (Fromdate<='" + curDate + "' or  Todate<='" + curDate + "')");
                if (dtOnduty.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtOnduty.Rows)
                    {
                        string hr = Convert.ToString(dr["hourse"]);
                        if (string.IsNullOrEmpty(ODEntry))
                            ODEntry = hr;
                        else
                            ODEntry = ODEntry + "," + hr;
                    }
                }
                string NewOD = string.Empty;
                if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(appNo) && date.Trim() != "" && appNo != "0")
                {
                    string[] spd = date.Split('/');
                    int year = Convert.ToInt32(spd[2].ToString());
                    int month = Convert.ToInt32(spd[1].ToString());
                    int dayv = Convert.ToInt32(spd[0].ToString());
                 
                    int monthyear = (year * 12) + month;
                    string daycolumnva = string.Empty;
                    string daycolumname = string.Empty;
                    string daycolumnupdate = string.Empty;
                    string atttxt = string.Empty;
                    for (int c = 1; c < attnd_report.Sheets[0].ColumnCount - 1; c++)
                    {
                        leave_flag = attnd_report.Sheets[0].Cells[r, c].Text.ToString();
                        atttxt = attnd_report.Sheets[0].Cells[r, c].Text.ToString();
                        if (leave_flag.Trim() != "")
                        {
                            leave_flag = Attvalues(leave_flag);
                        }
                        else
                        {
                            leave_flag = "0";
                        }
                        if (daycolumname.Trim() == "")
                        {
                            daycolumname = "d" + dayv + "d" + c;
                            daycolumnva = leave_flag;
                            daycolumnupdate = "d" + dayv + "d" + c + "=" + leave_flag;
                        }
                        else
                        {
                            daycolumname = daycolumname + "," + "d" + dayv + "d" + c;
                            daycolumnva = daycolumnva + "," + leave_flag;
                            daycolumnupdate = daycolumnupdate + ",d" + dayv + "d" + c + "=" + leave_flag;
                        }
                        if (atttxt.Contains("OD"))
                        {
                            if (!ODEntry.Contains(c.ToString()))
                            {
                                if (string.IsNullOrEmpty(NewOD))
                                    NewOD = c.ToString();
                                else
                                    NewOD =NewOD+","+ c.ToString();
                            }
                        }
                        
                    }
                    hat.Clear();
                    hat.Add("Att_App_no", appNo);
                    hat.Add("Att_CollegeCode", Session["collegecode"].ToString());
                    hat.Add("rollno", rollno);
                    hat.Add("monthyear", monthyear);
                    hat.Add("columnname", daycolumname);
                    hat.Add("colvalues", daycolumnva);
                    hat.Add("coulmnvalue", daycolumnupdate);
                    int insert = dacces2.insert_method("sp_ins_upd_student_attendance_Dead", hat, "sp");
                    noentryflag = true;

                    if (!string.IsNullOrEmpty(NewOD))
                    {
                        string[] nohrs=NewOD.Split(',');
                        string nhr=nohrs.Length.ToString();
                        string insertQ = "insert into Onduty_Stud(Roll_no,Semester,Purpose,Fromdate,Todate,college_code,attnd_type,hourse,no_of_hourse) values('" + rollno + "'," + sem + ",'" + atttxt + "','" + curDate + "','" + curDate + "','" + Session["collegecode"].ToString() + "',3,'" + NewOD + "','" + nhr + "')";
                        int ins = dacces2.update_method_wo_parameter(insertQ,"Text");
                    }
                   
                }
            }

          
            //for (int cumd = fcal; cumd <= tcal; cumd++)
            //{
            //    totpresentday = 0;
            //    if (cumd == tcal)
            //    {
            //        cal_date(cumd);
            //        if (fd == td)
            //        {
            //            totpresentday += 1;
            //        }
            //        else if (td == daycount)
            //        {
            //            totpresentday += daycount;
            //            balamonday = daycount;
            //        }
            //        else
            //        {
            //            totpresentday += td - (fd - 1);
            //            balamonday = fd - (td);
            //        }
            //    }
            //    if (cumd != tcal)
            //    {
            //        cal_date(cumd);
            //        totpresentday += daycount;
            //        balamonday = daycount;
            //    }
            //    //------------find start date
            //    if (cumd == fcal)
            //    {
            //        k = fd;
            //    }
            //    else
            //    {
            //        k = 1;
            //    }
            //    if (cumd == tcal)
            //    {
            //        endk = td;
            //    }
            //    else
            //    {
            //        endk = totpresentday;
            //    }
            //    string temp_leave =string.Empty;
            //    string update_value =string.Empty;
            //    string field_value =string.Empty;
            //    Boolean Yesflag = false;
            //    for (k = k; k <= endk; k++)
            //    {
            //        ds4.Clear();
            //        hat.Clear();
            //        hat.Add("date_val", dummydate);
            //        hat.Add("degree_code", Session["degree_code"].ToString());
            //        hat.Add("sem_val", Session["sem_val"].ToString());
            //        ds4 = dacces2.select_method("holiday_sp", hat, "sp");
            //        Yesflag = false;
            //        if (ds4.Tables[0].Rows.Count > 0 && ds4.Tables[0].Rows[0]["halforfull"].ToString() == "True")
            //        {
            //            row++;
            //            Yesflag = true;
            //        }
            //        else if (ds4.Tables[0].Rows.Count > 0 && ds4.Tables[0].Rows[0]["halforfull"].ToString() == "False")
            //        {
            //            Yesflag = false;
            //        }
            //        else if (ds4.Tables[0].Rows.Count == 0)
            //        {
            //            row++;
            //            Yesflag = true;
            //        }
            //        if (Yesflag == true)
            //        {
            //            for (int temp = 1; temp <= (nohrs); temp++)
            //            {
            //                if (row <= (attnd_report.Sheets[0].RowCount - 1) && temp <= (attnd_report.Sheets[0].ColumnCount - 1))
            //                {
            //                    leave_flag = attnd_report.Sheets[0].Cells[row, temp].Text.ToString();
            //                    if (leave_flag != " ")
            //                    {
            //                        Attvalues(leave_flag);
            //                    }
            //                    else
            //                    {
            //                        Attvalue = "''";
            //                    }
            //                    if (Attvalue == "")
            //                    {
            //                        Attvalue = "''";
            //                    }
            //                    if (temp_leave == "")
            //                    {
            //                        field_value = "d" + k.ToString() + "d" + temp.ToString();
            //                        temp_leave = Attvalue;
            //                        update_value = field_value + "=" + temp_leave;
            //                    }
            //                    else
            //                    {
            //                        field_value = field_value + "," + "d" + k.ToString() + "d" + temp.ToString();
            //                        temp_leave = temp_leave + "," + Attvalue;
            //                        update_value = update_value + "," + "d" + k.ToString() + "d" + temp.ToString() + "=" + Attvalue; ;
            //                    }
            //                }
            //            }
            //        }
            //        dummydate = dummydate.AddDays(1);
            //    }
            //    get_roll_no_r = "r.roll_no='" + txtrollno.Text.Trim().ToString() + "'";
            //    string get_rollno = GetFunction("select r.roll_no from registration r where " + get_roll_no_r + "");
            //if (field_value != "" && temp_leave != "" && update_value != "")
            //{
            //    SqlDataAdapter da1 = new SqlDataAdapter("select * from Attendance where Roll_no='" + get_rollno.ToString() + "' and month_year=" + cumd.ToString() + "", con);
            //    con.Close();
            //    con.Open();
            //    DataSet ds_save = new DataSet();
            //    da1.Fill(ds_save);
            //    {
            //        if (ds_save.Tables[0].Rows.Count == 0)//Save Attendance
            //        {
            //            string Insertquery =string.Empty;
            //            Insertquery = "insert into Attendance ( Roll_no,month_year," + field_value + ") values ('" + get_rollno.ToString() + "'," + cumd.ToString() + "," + temp_leave.ToString() + ")";
            //            SqlCommand cmd = new SqlCommand(Insertquery);
            //            readcon.Close();
            //            readcon.Open();
            //            cmd.Connection = readcon;
            //            cmd.ExecuteNonQuery();
            //            readcon.Close();
            //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
            //        }
            //        else
            //        {
            //            string updatequery =string.Empty;
            //            updatequery = "update Attendance set " + update_value + "  where  Roll_no='" + get_rollno.ToString() + "' and month_year=" + cumd.ToString() + "";
            //            SqlCommand cmd = new SqlCommand(updatequery);
            //            readcon.Close();
            //            readcon.Open();
            //            cmd.Connection = readcon;
            //            cmd.ExecuteNonQuery();
            //            readcon.Close();
            //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
            //        }
            //    }
            //}
            // }
            btnGo_Click(sender, e);
            if (noentryflag == true)
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Attendance Not Updated\");", true);
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Attendance Not Updated')", true);
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.Message;
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }

    public string Attvalues(string Att_str1)
    {
        Att_str1 = Att_str1.Trim();
        Attvalue = string.Empty;
        if (Att_str1 == "P")
        {
            Attvalue = "1";
        }
        else if (Att_str1 == "A")
        {
            Attvalue = "2";
        }
        else if (Att_str1 == "OD")
        {
            Attvalue = "3";
        }
        else if (Att_str1 == "ML")
        {
            Attvalue = "4";
        }
        else if (Att_str1 == "SOD")
        {
            Attvalue = "5";
        }
        else if (Att_str1 == "NSS")
        {
            Attvalue = "6";
        }
        else if (Att_str1 == "H")
        {
            Attvalue = "7";
        }
        if (Att_str1 == "NJ")
        {
            Attvalue = "8";
        }
        else if (Att_str1 == "S")
        {
            Attvalue = "9";
        }
        else if (Att_str1 == "L")
        {
            Attvalue = "10";
        }
        else if (Att_str1 == "NCC")
        {
            Attvalue = "11";
        }
        else if (Att_str1 == "HS")
        {
            Attvalue = "12";
        }
        else if (Att_str1 == "PP")
        {
            Attvalue = "13";
        }
        else if (Att_str1 == "SYOD")
        {
            Attvalue = "14";
        }
        else if (Att_str1 == "COD")
        {
            Attvalue = "15";
        }
        else if (Att_str1 == "OOD")
        {
            Attvalue = "16";
        }
        else if (Att_str1 == "LA")
        {
            Attvalue = "17";
        }
        //*********added by subburaj******//
        else if (Att_str1 == "RAA")
        {
            Attvalue = "18";
            raact++;
        }
        //****End*******//
        else
        {
            // Attvalue =string.Empty;
        }
        return Attvalue;
    }

    public void find_holiday(DateTime tempdate)
    {
        ds4.Clear();
        hat.Clear();
        hat.Add("date_val", tempdate);
        string isredo1 = Convert.ToString(ds.Tables[0].Rows[0]["isRedo"]).Trim();
        string degCode = string.Empty;
        string sem = string.Empty;
        if (isredo1 == "True")
        {
            degCode = Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]);
            sem = ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim());

            hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim());
            hat.Add("sem_val", ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim()));
            Session["degree_code"] = Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim();
            Session["sem_val"] = ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim());
        }
        else
        {
            degCode = Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim();
            sem = (ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim();
            hat.Add("degree_code", Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim());
            hat.Add("sem_val", ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim()));
            Session["degree_code"] = Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim();
            Session["sem_val"] = ((ddlSem.Items.Count > 0) ? Convert.ToString(ddlSem.SelectedItem.Text).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim());
        }

        ds4 = dacces2.select_method("holiday_sp", hat, "sp");
        //ds4 = dacces2.select_method_wo_parameter("select holiday_desc,halforfull,morning,evening from holidaystudents where holiday_date='" + tempdate + "' and degree_code='" + degCode + "' and semester='" + sem + "'", "text");

        if (ds4.Tables[0].Rows.Count > 0)//-------holiday
        {
            if (ds4.Tables[0].Rows[0]["halforfull"].ToString().Trim().ToLower() == "false")//---ful holiday
            {
                holidayflag = true;
            }
            else
            {
                holidayflag = false;
                if (ds4.Tables[0].Rows[0]["morning"].ToString().Trim().ToLower() == "false")
                {
                    split_holiday_status_1 = 1;
                    split_holiday_status_2 = first_half;
                    split_holiday_status_3 = first_half + 1;
                    split_holiday_status_4 = nohrs;
                }
                else
                {
                    split_holiday_status_1 = first_half + 1;
                    split_holiday_status_2 = nohrs;
                    split_holiday_status_3 = 1;
                    split_holiday_status_4 = first_half;
                }
            }
        }
        else// working day
        {
            split_holiday_status_1 = 1;
            split_holiday_status_2 = nohrs;
            split_holiday_status_3 = 0;
            split_holiday_status_4 = 0;
            holidayflag = false;
        }
    }

    public string Attmark(string Attstr_mark)
    {
        Attstr_mark = Attstr_mark.Trim();
        Att_mark = string.Empty;
        if (Attstr_mark == "1")
        {
            Att_mark = "P";
            pct++;
        }
        else if (Attstr_mark == "2")
        {
            Att_mark = "A";
            act++;
        }
        else if (Attstr_mark == "3")
        {
            Att_mark = "OD";
            odct++;
        }
        else if (Attstr_mark == "4")
        {
            Att_mark = "ML";
            mlct++;
        }
        else if (Attstr_mark == "5")
        {
            Att_mark = "SOD";
            sodct++;
        }
        else if (Attstr_mark == "6")
        {
            Att_mark = "NSS";
            nssct++;
        }
        else if (Attstr_mark == "7")
        {
            Att_mark = "H";
            hct++;
        }
        if (Attstr_mark == "8")
        {
            Att_mark = "NJ";
            njct++;
        }
        else if (Attstr_mark == "9")
        {
            Att_mark = "S";
            sct++;
        }
        else if (Attstr_mark == "10")
        {
            Att_mark = "L";
            lct++;
        }
        else if (Attstr_mark == "11")
        {
            Att_mark = "NCC";
            nccct++;
        }
        else if (Attstr_mark == "12")
        {
            Att_mark = "HS";
            hsct++;
        }
        else if (Attstr_mark == "13")
        {
            Att_mark = "PP";
            ppct++;
        }
        else if (Attstr_mark == "14")
        {
            Att_mark = "SYOD";
            syodct++;
        }
        else if (Attstr_mark == "15")
        {
            Att_mark = "COD";
            codct++;
        }
        else if (Attstr_mark == "16")
        {
            Att_mark = "OOD";
            oodct++;
        }
        else if (Attstr_mark == "17")
        {
            Att_mark = "LA";
            lact++;
        }
        //added by subburaj*28/8/2014//
        else if (Attstr_mark == "18")
        {
            Att_mark = "RAA";
        }
        //*******end*******//
        else
        {
            // Att_mark =string.Empty;
        }
        //return Convert.ToInt32(Att_mark);
        return Att_mark;
    }

    public void cal_date(double cumd)
    {
        int calm1 = fyy * 12 + 1;
        int calm2 = fyy * 12 + 2;
        int calm3 = fyy * 12 + 3;
        int calm4 = fyy * 12 + 4;
        int calm5 = fyy * 12 + 5;
        int calm6 = fyy * 12 + 6;
        int calm7 = fyy * 12 + 7;
        int calm8 = fyy * 12 + 8;
        int calm9 = fyy * 12 + 9;
        int calm10 = fyy * 12 + 10;
        int calm11 = fyy * 12 + 11;
        int calm12 = fyy * 12 + 12;
        if (calm1 == cumd || calm3 == cumd || calm5 == cumd || calm7 == cumd || calm8 == cumd || calm10 == cumd || calm12 == cumd)
        {
            daycount = 31;
        }
        if (calm4 == cumd || calm6 == cumd || calm9 == cumd || calm11 == cumd)
        {
            daycount = 30;
        }
        if (mm == 0 || mm == 1)
        //if (mm == 1) srinath
        {
            if (calm2 == cumd)
            {
                int lyear = 2000;
                int ly;
                if (lyear <= fyy)
                {
                    ly = lyear - fyy;
                }
                else
                {
                    ly = fyy - lyear;
                }
                if (ly == 4)
                {
                    daycount = 29;
                }
                else
                {
                    daycount = 28;
                }
            }
        }
    }

    protected void attnd_report_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
    }

    protected void attnd_report_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        btnsave.Visible = true;
        if (update == false)
        {
            int act_row = 0;
            int act_col = 0;
            act_col = int.Parse(attnd_report.ActiveSheetView.ActiveColumn.ToString());
            act_row = int.Parse(attnd_report.ActiveSheetView.ActiveRow.ToString());
            if (act_col > 0)
            {
                //=================================select all
                if (act_row == 0)
                {
                    string attnd_val = string.Empty;
                    attnd_val = e.EditValues[Convert.ToInt16(act_col)].ToString();
                    for (int row_cnt = 1; row_cnt <= (attnd_report.Sheets[0].RowCount - 1); row_cnt++)
                    {
                        //if(attnd_report.Sheets[0].Rows[row_cnt].Locked==false )
                        //{
                        if (attnd_val != "System.Object")
                        {
                            attnd_report.Sheets[0].Cells[row_cnt, Convert.ToInt16(act_col)].Text = attnd_val.ToString();
                        }
                        attnd_report.Sheets[0].SetText(row_cnt, act_col, attnd_val);
                        attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.DarkSeaGreen;
                        //if (attnd_val == "")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.Aqua;
                        //}
                        //else if (attnd_val == "P")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.DarkSeaGreen;
                        //}
                        //else if (attnd_val == "A")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.Red;
                        //}
                        //else if (attnd_val == "OD")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.Green;
                        //}
                        //else if (attnd_val == "SOD")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.GreenYellow;
                        //}
                        //else if (attnd_val == "ML")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.DarkSalmon;
                        //}
                        //else if (attnd_val == "NSS")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.Goldenrod;
                        //}
                        //else if (attnd_val == "L")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.MediumSlateBlue;
                        //}
                        //else if (attnd_val == "NCC")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.Violet;
                        //}
                        //else if (attnd_val == "HS")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.SlateGray;
                        //}
                        //else if (attnd_val == "PP")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.Pink;
                        //}
                        //else if (attnd_val == "SYOD")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.LimeGreen;
                        //}
                        //else if (attnd_val == "COD")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.Tan;
                        //}
                        //else if (attnd_val == "OOD")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.Wheat;
                        //}
                        //else if (attnd_val == "NJ")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.SaddleBrown;
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].ForeColor = Color.White;
                        //}
                        //else if (attnd_val == "S")
                        //{
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].BackColor = Color.Black;
                        //    attnd_report.Sheets[0].Cells[row_cnt, act_col].ForeColor = Color.White;
                        //}
                    }
                }
                //================================end select all
                //==================================set color for each cell
                //if (act_col != null && act_col != -1)
                //{
                //    if (act_row != null && act_row != -1)
                //    {
                //        if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.Aqua;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "P")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.DarkSeaGreen;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "A")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.Red;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "OD")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.Green;
                //            attnd_report.Sheets[0].Cells[act_row, act_col].ForeColor = Color.White;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "SOD")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.GreenYellow;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "ML")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.DarkSalmon;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "NSS")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.Goldenrod;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "L")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.MediumSlateBlue;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "NCC")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.Violet;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "HS")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.SlateGray;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "PP")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.Pink;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "SYOD")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.LimeGreen;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "COD")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.Tan;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "OOD")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.Wheat;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "NJ")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.SaddleBrown;
                //            attnd_report.Sheets[0].Cells[act_row, act_col].ForeColor = Color.White;
                //        }
                //        else if (e.EditValues[Convert.ToInt16(act_col)].ToString() == "S")
                //        {
                //            attnd_report.Sheets[0].Cells[act_row, act_col].BackColor = Color.Black;
                //            attnd_report.Sheets[0].Cells[act_row, act_col].ForeColor = Color.White;
                //        }
                //    }
                //}
            }
        }
        update = true;
    }

    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        getcon.Close();
        getcon.Open();
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr, getcon);
        drnew = cmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        Control cntUpdateBtn = attnd_report.FindControl("Update");
        Control cntCancelBtn = attnd_report.FindControl("Cancel");
        Control cntCopyBtn = attnd_report.FindControl("Copy");
        Control cntCutBtn = attnd_report.FindControl("Clear");
        Control cntPasteBtn = attnd_report.FindControl("Paste");
        Control cntPageNextBtn = attnd_report.FindControl("Next");
        Control cntPagePreviousBtn = attnd_report.FindControl("Prev");
        //Control cntPagePrintBtn = FpSpread1.FindControl("Print");
        if ((cntUpdateBtn != null))
        {
            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntPageNextBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);
        }
        base.Render(writer);
    }

    void CalculateTotalPages()
    {
        double totalRows = 0;
        totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }

    protected void pageddltxt_TextChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        attnd_report.CurrentPage = 0;
        pagesearch_txt.Text = string.Empty;
        try
        {
            if (pageddltxt.Text != string.Empty)
            {
                if (attnd_report.Sheets[0].RowCount >= Convert.ToInt16(pageddltxt.Text.ToString()) && Convert.ToInt16(pageddltxt.Text.ToString()) != 0)
                {
                    attnd_report.Sheets[0].PageSize = Convert.ToInt16(pageddltxt.Text.ToString());
                    attnd_report.Height = 30 + (24 * Convert.ToInt32(pageddltxt.Text.ToString()));
                    CalculateTotalPages();
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Enter valid Record count";
                    pageddltxt.Text = string.Empty;
                }
            }
        }
        catch
        {
            errmsg.Visible = true;
            errmsg.Text = "Enter valid Record count";
            pageddltxt.Text = string.Empty;
        }
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        attnd_report.CurrentPage = 0;
        pagesearch_txt.Text = string.Empty;
        errmsg.Visible = false;
        pagesearch_txt.Text = string.Empty;
        pageddltxt.Text = string.Empty;
        pageddltxt.Text = string.Empty;
        if (DropDownListpage.Text == "Others")
        {
            pageddltxt.Visible = true;
            pageddltxt.Focus();
        }
        else
        {
            pageddltxt.Visible = false;
            attnd_report.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            attnd_report.Height = 30 + (24 * Convert.ToInt32(DropDownListpage.Text.ToString()));
            CalculateTotalPages();
        }
    }

    protected void pagesearch_txt_TextChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        if (pagesearch_txt.Text.Trim() != string.Empty)
        {
            if (Convert.ToInt64(pagesearch_txt.Text) > Convert.ToInt64(Session["totalPages"]))
            {
                errmsg.Visible = true;
                errmsg.Text = "Exceed The Page Limit";
                pagesearch_txt.Text = string.Empty;
                attnd_report.Visible = true;
                divNote.Visible = true;
            }
            else if (Convert.ToInt64(pagesearch_txt.Text) == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = " Search Should Be Greater Than '0'";
                pagesearch_txt.Text = string.Empty;
                attnd_report.Visible = true;
                divNote.Visible = true;
            }
            else
            {
                errmsg.Visible = false;
                attnd_report.CurrentPage = Convert.ToInt16(pagesearch_txt.Text) - 1;
                attnd_report.Visible = true;
                divNote.Visible = true;
            }
        }
    }

    protected void txtrollno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ddlSem.Items.Clear();
            ddlSem.Enabled = false;
            errlbl.Text = string.Empty;
            errlbl.Visible = false;
            rptprint1.Visible = false;
            btnsave.Visible = false;
            attnd_report.Visible = false;
            divNote.Visible = false;
            std_info.Visible = false;
            pageset_pnl.Visible = false;
            errlbl.Visible = false;
            lblname1.Visible = false;
            lblname2.Visible = false;
            go();

            if (ddlSem.Items.Count > 0 && Convert.ToString(ddlSem.SelectedValue) != "-1")
                ddlSem_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.Message;
            pageddltxt.Text = string.Empty;
        }
    }

    protected void optionddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtrollno.Text = " ";
        errlbl.Visible = false;
        lblname1.Visible = false;
        lblname2.Visible = false;
        pageset_pnl.Visible = false;
        attnd_report.Visible = false;
        divNote.Visible = false;
        btnsave.Visible = false;
        txtFromDate.Visible = false;
        txtToDate.Visible = false;
        lblFromdate.Visible = false;
        lbltodate.Visible = false;
        btnGo.Visible = false;
        dateerrlbl.Visible = false;
        if (optionddl.Items[0].Selected == true)
        {
            lblrollno.Text = "Enter Student Roll No";
        }
        else if (optionddl.Items[1].Selected == true)
        {
            lblrollno.Text = "Enter Student Reg No";
        }
        else
        {
            lblrollno.Text = "Enter Student Admission No";
        }
        rptprint1.Visible = false;
        btnsave.Visible = false;
        attnd_report.Visible = false;
        divNote.Visible = false;
        std_info.Visible = false;
        pageset_pnl.Visible = false;
        errlbl.Visible = false;
        lblname1.Visible = false;
        lblname2.Visible = false;
    }

    public void binddate()
    {
        errlbl.Text = string.Empty;
        errlbl.Visible = false;
        string sem = string.Empty;
        if (ddlSem.Items.Count > 0)
        {
            sem = Convert.ToString(ddlSem.SelectedItem.Text).Trim();
        }
        //********Modified By Mullai
        string appNo = string.Empty;
        string rollno = txtrollno.Text.Trim().ToString();
        if (optionddl.SelectedIndex == 0)
        {
            appNo = dacces2.GetFunction("select app_no from registration where Roll_No='" + rollno + "' ");
        }
        else if (optionddl.SelectedIndex == 1)
        {
            appNo = dacces2.GetFunction("select app_no from registration where Reg_No='" + rollno + "' ");
        }
        else
        {
            appNo = dacces2.GetFunction("select app_no from registration where roll_admit='" + rollno + "' ");
        }
        string batch_year = string.Empty;
        string str_query = string.Empty;
        string isredo1 = Convert.ToString(ds.Tables[0].Rows[0]["isRedo"]).Trim();
        string delflag = Convert.ToString(ds.Tables[0].Rows[0]["DelFlag"]).Trim();
        string roll = string.Empty;
        if (optionddl.Items[0].Selected == true)
        {
            roll = "  r.roll_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
        }
        else if (optionddl.Items[1].Selected == true)
        {
            roll = "  r.reg_no='" + Convert.ToString(txtrollno.Text).Trim() + "'";
        }
        else
        {
            roll = "  r.roll_admit='" + Convert.ToString(txtrollno.Text).Trim() + "'";
        }
        string delQry = " select * from Readmission  where app_no in(select app_no from registration r where " + roll + ")";

        DataTable dtdisC = dir.selectDataTable(delQry);
        if (dtdisC.Rows.Count > 0)
        {
            delflag = "1";
        }
        else
        {
            delflag = "0";
        }
        if (isredoold == "1")
        {
            if (isredo1.ToLower() == "true" || isredo1 == "1" || delflag == "1" || delflag.ToLower() == "true")
            {

                if (optionddl.SelectedIndex == 0)
                {
                    batch_year = dacces2.GetFunction("select Batch_Year from registration where Roll_No='" + rollno + "' ");
                }
                else if (optionddl.SelectedIndex == 1)
                {
                    batch_year = dacces2.GetFunction("select Batch_Year from registration where Reg_No='" + rollno + "' ");
                }
                else
                {
                    batch_year = dacces2.GetFunction("select Batch_Year from registration where roll_admit='" + rollno + "' ");
                }


                str_query = "select  s.start_date ,s.end_date from seminfo s,applyn a where s.degree_code='" + Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim() + "' and s.batch_year='" + batch_year + "' and s.degree_code=a.degree_code  and a.app_no='" + appNo + "' and semester='" + sem + "'";
            }
            else
            {
                batch_year = dacces2.GetFunction("select batch_year from registration where app_no='" + appNo + "'");
                str_query = "select  s.start_date ,s.end_date from seminfo s,applyn a where s.degree_code='" + Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim() + "' and s.batch_year='" + batch_year + "' and s.degree_code=a.degree_code  and a.app_no='" + appNo + "' and semester='" + sem + "'";
            }
        }
        else if (isredo1.ToLower() == "true" || isredo1 == "1" || delflag == "1")
        {
            batch_year = dacces2.GetFunction("select batch_year from applyn where app_no='" + appNo + "'");
            str_query = "select  s.start_date ,s.end_date from seminfo s,applyn a where s.degree_code='" + Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim() + "' and s.batch_year='" + batch_year + "' and s.degree_code=a.degree_code  and a.app_no='" + appNo + "' and semester='" + sem + "'";
        }
        else
        {
            batch_year = dacces2.GetFunction("select batch_year from registration where app_no='" + appNo + "'");
            str_query = "select  s.start_date ,s.end_date from seminfo s,applyn a where s.degree_code='" + Convert.ToString(ds.Tables[0].Rows[0]["appdegree"]).Trim() + "' and s.batch_year='" + batch_year + "' and s.degree_code=a.degree_code  and a.app_no='" + appNo + "' and semester='" + sem + "'";
        }

        //*********


        //string str_query = "select start_date ,end_date from seminfo where degree_code='" + Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]).Trim() + "' and batch_year='" + Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]).Trim() + "' and semester='" + sem + "'";

        ds_date = d2.select_method_wo_parameter(str_query, "Text");
        if (ds_date.Tables.Count > 0 && ds_date.Tables[0].Rows.Count > 0)
        {
            from_date = Convert.ToDateTime(Convert.ToString(ds_date.Tables[0].Rows[0][0]).Trim());
            Session["from_date"] = from_date.ToString();
            to_date = Convert.ToDateTime(Convert.ToString(ds_date.Tables[0].Rows[0][1]).Trim());
            Session["to_date"] = to_date.ToShortDateString();
            from_date_sem = from_date.Day + "/" + from_date.Month + "/" + from_date.Year;
            to_date_sem = to_date.Day + "/" + to_date.Month + "/" + to_date.Year;
            txtFromDate.Text = from_date_sem;
            txtToDate.Text = to_date_sem;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Visible = true;
            txtToDate.Visible = true;
            lblFromdate.Visible = true;
            lbltodate.Visible = true;
            btnGo.Visible = true;
        }
        else
        {
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            txtFromDate.Visible = false;
            txtToDate.Visible = false;
            lblFromdate.Visible = false;
            lbltodate.Visible = false;
            btnGo.Visible = false;
            errlbl.Text = "Please Update Semester Start Date And End Date!!!";
            errlbl.Visible = true;
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        dateerrlbl.Visible = false;
        attnd_report.Visible = false;
        divNote.Visible = false;
        btnsave.Visible = false;
        errlbl.Visible = false;
        rptprint1.Visible = false;
        std_info.Visible = false;
        if (txtFromDate.Text != "")
        {
            string fromdate_txt = string.Empty;
            DateTime from_datetime_txt = new DateTime();
            fromdate_txt = txtFromDate.Text;
            string[] freomdate_split = fromdate_txt.Split('/');
            from_datetime_txt = Convert.ToDateTime(freomdate_split[1].ToString() + "/" + freomdate_split[0].ToString() + "/" + freomdate_split[2].ToString());
            TimeSpan td_from = from_datetime_txt.Subtract(Convert.ToDateTime(Session["from_date"].ToString()));
            TimeSpan td_to = from_datetime_txt.Subtract(Convert.ToDateTime(Session["to_date"].ToString()));
            int days_from = td_from.Days;
            int days_to = td_to.Days;
            if (days_from < 0 || days_to > 0)
            {
                txtFromDate.Text = " ";
                txtFromDate.Text = from_date.Day + "/" + from_date.Month + "/" + from_date.Year;
            }
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        dateerrlbl.Visible = false;
        attnd_report.Visible = false;
        divNote.Visible = false;
        btnsave.Visible = false;
        errlbl.Visible = false;
        rptprint1.Visible = false;
        std_info.Visible = false;
        if (txtFromDate.Text != "")
        {
            string todate_txt = string.Empty;
            DateTime to_datetime_txt = new DateTime();
            todate_txt = txtToDate.Text;
            string[] todate_split = todate_txt.Split('/');
            to_datetime_txt = Convert.ToDateTime(todate_split[1].ToString() + "/" + todate_split[0].ToString() + "/" + todate_split[2].ToString());
            TimeSpan td_to = to_datetime_txt.Subtract(Convert.ToDateTime(Session["to_date"].ToString()));
            TimeSpan td_from = to_datetime_txt.Subtract(Convert.ToDateTime(Session["from_date"].ToString()));
            int days_to = td_to.Days;
            int days_from = td_from.Days;
            if (days_to > 0 || days_from < 0)
            {
                //errmsg.Visible = true;
                //errmsg.Text = "To Date Should Be Between Semester From And To Date";
                txtToDate.Text = " ";
                txtToDate.Text = to_date.Day + "/" + to_date.Month + "/" + to_date.Year;
            }
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        lbl_error.Visible = false;
        dateerrlbl.Visible = false;
        attnd_report.Visible = false;
        divNote.Visible = false;
        btnsave.Visible = false;
        errlbl.Visible = false;
        date1 = txtFromDate.Text;

        string appNo = string.Empty;
        byte isStatus = 0;
        errlbl.Visible = false;
        errlbl.Text = string.Empty;
        appNo = getAppNo(txtrollno.Text.Trim().ToString(), ref appNo, ref isStatus);
        std_info.Visible = false;
        dateerrlbl.Text = string.Empty;
        if (isStatus == 0)
        {
            dateerrlbl.Visible = false;
            attnd_report.Visible = false;
            divNote.Visible = false;
            rptprint1.Visible = false;
            btnsave.Visible = false;
            errlbl.Visible = true;
            errlbl.Text = "Student is Not Available";
            return;
        }
        if (isStatus == 2)
        {
            dateerrlbl.Visible = false;
            attnd_report.Visible = false;
            divNote.Visible = false;
            rptprint1.Visible = false;
            btnsave.Visible = false;
            errlbl.Visible = true;
            errlbl.Text = "Student is Not Available in this Institution";
            return;
        }
        if (date1.Trim() != "")
        {
            string[] split = date1.Split(new Char[] { '/' });
            if (split.GetUpperBound(0) == 2)//-------date valid
            {
                if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                {
                    datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                    date2 = txtToDate.Text.ToString();
                    if (date2.Trim() != "")
                    {
                        string[] split1 = date2.Split(new Char[] { '/' });
                        if (split1.GetUpperBound(0) == 2)//--date valid
                        {
                            if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                            {
                                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                dt1 = Convert.ToDateTime(datefrom.ToString());
                                Session["from_date_time"] = dt1;
                                dt2 = Convert.ToDateTime(dateto.ToString());
                                Session["to_date_time"] = dt2;
                                TimeSpan t = dt2.Subtract(dt1);
                                long days = t.Days;
                                Session["days"] = days;
                                if (days >= 0)//-----check date difference
                                {
                                    string field_val = string.Empty;
                                    if (optionddl.Items[0].Selected == true)
                                    {
                                        field_val = " and r.roll_no='" + txtrollno.Text.Trim().ToString() + "'";
                                    }
                                    else if (optionddl.Items[1].Selected == true)
                                    {
                                        field_val = " and r.reg_no='" + txtrollno.Text.Trim().ToString() + "'";
                                    }
                                    else
                                    {
                                        field_val = " and r.roll_admit='" + txtrollno.Text.Trim().ToString() + "'";
                                    }
                                    string strquery = "select distinct a.batch_year as appbatch,r.degree_code as regdegree,r.batch_year as regbatch,a.degree_code as appdegree,r.isRedo,r.Delflag,sections,r.stud_name,dg.college_code,r.current_semester as regsemester,a.current_semester as appsemester,roll_admit,roll_no,convert(varchar(15),adm_date,103) as adm_date,course_name,dg.acronym,r.college_code from registration r,deptprivilages d ,course c,degree dg,applyn a where c.course_id=dg.course_id and r.degree_code=dg.degree_code   and exam_flag<>'debar' and r.degree_code=d.degree_code  " + field_val + " and r.App_No=a.app_no"; //modified by Mullai   //and delflag=0
                                    ds.Clear();
                                    ds = dacces2.select_method(strquery, hat, "Text");

                                    currentSemester = ddlSem.SelectedItem.Text;
                                    string regsem = Convert.ToString(ds.Tables[0].Rows[0]["regsemester"]).Trim();
                                    string isredo1 = Convert.ToString(ds.Tables[0].Rows[0]["isRedo"]).Trim();
                                    string delflag = Convert.ToString(ds.Tables[0].Rows[0]["Delflag"]).Trim();
                                    string delQry = " select * from Readmission  where app_no='" + appNo + "'";
                                    DataTable dtdisC = dir.selectDataTable(delQry);
                                    if (dtdisC.Rows.Count > 0)
                                    {
                                        delflag = "1";
                                    }
                                    else
                                    {
                                        delflag = "0";
                                    }
                                    if (isredo1 == "True" || delflag == "1")
                                    {
                                        if (currentSemester == regsem)
                                        {
                                            batchYear = Convert.ToString(ds.Tables[0].Rows[0]["regbatch"]).Trim();
                                            degreeCode = Convert.ToString(ds.Tables[0].Rows[0]["regdegree"]).Trim();

                                            stdate = txtFromDate.Text.ToString();
                                            string[] split3 = stdate.Split(new Char[] { '/' });
                                            stdate = split3[2].ToString() + "-" + split3[1].ToString() + "-" + split3[0].ToString();
                                            endate = txtToDate.Text.ToString();
                                            string[] split4 = endate.Split(new Char[] { '/' });
                                            endate = split4[2].ToString() + "-" + split4[1].ToString() + "-" + split4[0].ToString();

                                            string btyr = "select batch_year from seminfo where start_date='" + stdate + "' and end_date='" + endate + "' and degree_code='" + degreeCode + "' and semester='" + currentSemester + "'";
                                            DataSet dyr = d2.select_method_wo_parameter(btyr, "text");
                                            if (dyr.Tables[0].Rows.Count > 0)
                                            {
                                                string yr = Convert.ToString(dyr.Tables[0].Rows[0]["batch_year"]).Trim();
                                                if (yr == batchYear)
                                                {
                                                    isredoold = "1";
                                                }
                                            }

                                        }
                                    }
                                    getdate();//==================================================getdate function
                                    if (fflag == true)
                                    {
                                        pageset_pnl.Visible = false;
                                        attnd_report.Visible = true;
                                        std_info.Visible = true;
                                        divNote.Visible = true;
                                        rptprint1.Visible = true;
                                        btnsave.Visible = true;
                                        errlbl.Visible = false;
                                    }
                                    else
                                    {
                                        pageset_pnl.Visible = false;
                                        attnd_report.Visible = false;
                                        divNote.Visible = false;
                                        rptprint1.Visible = false;
                                        errlbl.Visible = true;
                                        btnsave.Visible = false;
                                    }
                                    //if (countold == 0)
                                    //{
                                    //    bindredoolddetails(sender, e);

                                    //}
                                    //-------------------page setting
                                    if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) != 0)
                                    {
                                        pageset_pnl.Visible = false;

                                        totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
                                        DropDownListpage.Items.Clear();
                                        if (totalRows >= 10)
                                        {
                                            attnd_report.Sheets[0].PageSize = 10;
                                            for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                                            {
                                                DropDownListpage.Items.Add((k + 10).ToString());
                                            }
                                            DropDownListpage.Items.Add("Others");


                                            attndreportheight = 10 + (24 * Convert.ToInt32(totalRows));
                                            attnd_report.Height = attndreportheight;
                                            attnd_report.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                            attnd_report.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                        }
                                        else if (totalRows == 0)
                                        {
                                            DropDownListpage.Items.Add("0");
                                            attnd_report.Height = 400;
                                        }
                                        else
                                        {
                                            attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                                            DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());
                                        }
                                        if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
                                        {
                                            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                                            attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                                            CalculateTotalPages();
                                        }
                                        Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
                                        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                                    }
                                }
                                else
                                {
                                    dateerrlbl.Visible = true;
                                    attnd_report.Visible = false;
                                    divNote.Visible = false;
                                    rptprint1.Visible = false;
                                    btnsave.Visible = false;
                                    errlbl.Visible = false;
                                    dateerrlbl.Text = "From Date Should Be Less Than To Date";
                                }
                            }
                            else
                            {
                                dateerrlbl.Visible = true;
                                rptprint1.Visible = false;
                                attnd_report.Visible = false;
                                divNote.Visible = false;
                                btnsave.Visible = false;
                                errlbl.Visible = false;
                                dateerrlbl.Text = "Enter Valid To Date";
                            }
                        }
                        else
                        {
                            dateerrlbl.Visible = true;
                            attnd_report.Visible = false;
                            divNote.Visible = false;
                            btnsave.Visible = false;
                            rptprint1.Visible = false;
                            errlbl.Visible = false;
                            dateerrlbl.Text = "Enter Valid To Date";
                        }
                    }
                    else
                    {
                        dateerrlbl.Visible = true;
                        attnd_report.Visible = false;
                        divNote.Visible = false;
                        rptprint1.Visible = false;
                        btnsave.Visible = false;
                        errlbl.Visible = false;
                        dateerrlbl.Text = "Enter To Date";
                    }
                }
                else
                {
                    dateerrlbl.Visible = true;
                    attnd_report.Visible = false;
                    divNote.Visible = false;
                    rptprint1.Visible = false;
                    btnsave.Visible = false;
                    errlbl.Visible = false;
                    dateerrlbl.Text = "Enter Valid From Date";
                }
            }
            else
            {
                dateerrlbl.Visible = true;
                attnd_report.Visible = false;
                divNote.Visible = false;
                rptprint1.Visible = false;
                btnsave.Visible = false;
                errlbl.Visible = false;
                dateerrlbl.Text = "Enter Valid From Date";
            }
        }
        else
        {
            dateerrlbl.Visible = true;
            attnd_report.Visible = false;
            divNote.Visible = false;
            rptprint1.Visible = false;
            btnsave.Visible = false;
            errlbl.Visible = false;
            dateerrlbl.Text = "Enter From Date";
        }
        attnd_report.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        attnd_report.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

        if (attnd_report.Sheets[0].Rows.Count > 0)
        {
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string selectvalue = dacces2.GetFunction("select value  from Master_Settings where settings ='Individual Student Attendace Lock' and " + grouporusercode + " ");
            if (selectvalue.Trim() == "1")
            {
                for (int col = 0; col < attnd_report.Sheets[0].Columns.Count; col++)
                {
                    attnd_report.Sheets[0].Columns[col].Locked = true;
                    attnd_report.Sheets[0].Columns[0].Locked = true;
                }
            }
            else
            {
                for (int col = 0; col < attnd_report.Sheets[0].Columns.Count; col++)
                {
                    //attnd_report.Sheets[0].Columns[col].Locked = false;
                }
                btnsave.Visible = true;


            }
        }

    }


    protected void viewattendall_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (viewattendall.Checked == true)
            {
                attnd_report.Sheets[0].Rows[0].Visible = true;
            }
            else
            {
                attnd_report.Sheets[0].Rows[0].Visible = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.Message;
            pageddltxt.Text = string.Empty;
        }
    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (attnd_report.Visible == true)
                {
                    dacces2.printexcelreport(attnd_report, reportname);
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
            errmsg.Visible = true;
            errmsg.Text = ex.Message;
            pageddltxt.Text = string.Empty;
        }
    }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string fdt = txtFromDate.Text;
            string tdt = txtToDate.Text;
            string pagename = "dailystudentattndreport.aspx";
            string dptname = "Daily Individual Student Attendance Report";
            dptname = dptname + "@                                                                                   " + "Name : " + Convert.ToString(lblname2.Text) + "                            " + " Class : " + Convert.ToString(clas.Text) + "@                                                                                   " + "FullDay Absent : " + Convert.ToString(fullday.Text) + "                                    " + "HalfDay Absent : " + Convert.ToString(halfday.Text) + "@                                                                                   " + "Total Days Absent : " + Convert.ToString(totdays.Text) + "                             " + "OD Applied : " + Convert.ToString(odapplied.Text) + "@                                                                                   " + "Leave Applied : " + Convert.ToString(leaveapplied.Text) + "                               " + "Last Attended Date : " + Convert.ToString(lastattndate.Text) + "@                                                                                   " + "Percentage : " + Convert.ToString(lblHrsWisePercentage.Text) + "@" + " From Date : " + fdt + "To :" + tdt;
            if (attnd_report.Visible == true)
            {
                Printcontrol1.loadspreaddetails(attnd_report, pagename, dptname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.Message;
            pageddltxt.Text = string.Empty;
        }
    }

    private void semesterToYear(string semester, string degreeCode, string batchYear, string maximumDuration = null)
    {
        try
        {
            int maxDuration = 0;
            int sem = 0;
            int year = 0;
            bool isHas = int.TryParse(semester.Trim(), out sem);
            if (isHas)
            {
                if (maximumDuration == null)
                {
                }
                else
                {
                    if (int.TryParse(maximumDuration.Trim(), out maxDuration))
                    {
                        if (sem <= maxDuration && sem % 2 == 0)
                        {
                            year = (sem / 2);
                        }
                        else if (sem <= maxDuration && sem % 2 != 0)
                        {
                            year = (sem / 2) + 1;
                        }
                        else
                        {
                            year = (sem - 1);
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btnoldrecord_Click(object sender, EventArgs e)
    {
        try
        {
            divPopupAlert.Visible = false;
            divAlertContent.Visible = false;
            lblAlertMsg.Visible = false;
            // delflagold = true;
            binddate();
        }
        catch
        {

        }
    }
    protected void btnnewrecord_Click(object sender, EventArgs e)
    {
        try
        {
            divPopupAlert.Visible = false;
            divAlertContent.Visible = false;
            lblAlertMsg.Visible = false;
            isredoold = "1";
            // delflagnew = true;
            binddate();
        }
        catch
        {
        }
    }
    protected void btn_popclose_Click(object sender, EventArgs e)
    {
        divPopupAlert.Visible = false;
        divAlertContent.Visible = false;
        lblAlertMsg.Visible = false;
    }
}
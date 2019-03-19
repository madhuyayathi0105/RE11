using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Text;
using System.Globalization;

public partial class ScheduleMOD_TT_AlterSchedule : System.Web.UI.Page
{
    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection dar_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con4a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con5a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con6a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con8 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection tempcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    static bool forschoolsetting = false;
    SqlCommand cmd8 = new SqlCommand();
    SqlCommand cmda;
    SqlCommand cmd1a;
    SqlCommand cmd3a;
    SqlCommand cmd4a;
    SqlCommand cmd6a;
    SqlCommand cmd;
    DAccess2 d2 = new DAccess2();
    bool cellclick1 = false;
    bool Cellclickeve = false;
    bool semclick = false;
    int SchOrder = 0, nodays = 0;
    int intNHrs = 0;
    string start_dayorder = string.Empty;
    DAccess2 dacess = new DAccess2();
    Hashtable hat = new Hashtable();
    string SenderID = "", Password = string.Empty;
    string group_user = "", singleuser = "", usercode = "", collegecode = string.Empty;
    string qry = string.Empty;
    InsproDirectAccess dirAcc = new InsproDirectAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        Session["batch_year"] = ddlbatch.SelectedValue.ToString();
        Session["semester"] = ddlduration.SelectedValue.ToString();
        Session["degree_code"] = ddlbranch.SelectedValue.ToString();
        Session["section"] = ddlsec.SelectedValue.ToString();
        norecordlbl.Visible = false;
        Cellclickeve = false;
        lblcellerrmsg.Visible = false;
        if (!Page.IsPostBack)
        {
            string previousvalue = Request.QueryString["name"];
            ViewState["prevalue"] = previousvalue;
            try
            {
                Session["PreviousBatch"] = string.Empty;
                batchbtn.Visible = false;
                txtFromDate.Attributes.Add("ReadOnly", "ReadOnly");
                txtToDate.Attributes.Add("ReadOnly", "ReadOnly");
                bindbatch();
                binddegree();
                if (ddldegree.SelectedValue.ToString() != "" && ddldegree.SelectedValue.ToString() != null)
                {
                    ddlbatch.Enabled = true;
                    ddldegree.Enabled = true;
                    txtFromDate.Enabled = true;
                    txtToDate.Enabled = true;
                    btnGo.Enabled = true;
                    ddlbranch.Enabled = true;
                    ddlsec.Enabled = true;
                    ddlduration.Enabled = true;
                    bindbranch();
                    bindsem();
                    bindsec();
                }
                else
                {
                    ddlbatch.Enabled = false;
                    ddldegree.Enabled = false;
                    txtFromDate.Enabled = false;
                    txtToDate.Enabled = false;
                    btnGo.Enabled = false;
                    ddlbranch.Enabled = false;
                    ddlsec.Enabled = false;
                    ddlduration.Enabled = false;
                    norecordlbl.Visible = true;
                    norecordlbl.ForeColor = Color.Red;
                    norecordlbl.Text = "Update Degree Rights For The User";
                }
                deglbl.Visible = false;
                branlbl.Visible = false;
                seclbl.Visible = false;
                semlbl.Visible = false;
                frmlbl.Visible = false;
                tolbl.Visible = false;
                SpdInfo.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                Panel3.Visible = false;
                btnsave.Visible = false;
                semspread.Visible = false;
                sem_schedule.Enabled = false;
                txtmultisubj.Visible = false;
                pnlmultisubj.Visible = false;
                chk_multisubj.Visible = false;
                subjtree.Visible = false;
                FpSpread1.Visible = false;
                lblmulstaff.Visible = false;
                txtmulstaff.Visible = false;
                pmulstaff.Visible = false;
                btnmulstaff.Visible = false;
                chkappend.Visible = false;
                btnOk.Visible = false;
                tofromlbl.Visible = false;
                norecordlbl.Visible = false;
                treepanel.Visible = false;
                btn_remove.Visible = false;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 200;
                FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 200;
                FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 100;
                FpSpread1.CommandBar.Visible = false;
                FarPoint.Web.Spread.NamedStyle fontblue = new FarPoint.Web.Spread.NamedStyle("blue");
                SpdInfo.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                SpdInfo.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                SpdInfo.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                SpdInfo.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                SpdInfo.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                SpdInfo.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                SpdInfo.CommandBar.Visible = false;
                SpdInfo.Sheets[0].FrozenColumnCount = 4;
                freestaff.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                freestaff.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                freestaff.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                freestaff.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                freestaff.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                freestaff.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                freestaff.CommandBar.Visible = false;
                semspread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                semspread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                semspread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                semspread.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                semspread.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                semspread.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                semspread.CommandBar.Visible = false;
                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 13;
                style.Font.Bold = true;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpSpread1.Sheets[0].AllowTableCorner = true;
                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 13;
                style1.Font.Bold = true;
                SpdInfo.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                SpdInfo.Sheets[0].AllowTableCorner = true;
                SpdInfo.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Bold = true;
                freestaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                freestaff.Sheets[0].AllowTableCorner = true;
                freestaff.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                freestaff.Sheets[0].SheetCorner.Columns[0].Width = 100;
                FarPoint.Web.Spread.StyleInfo style3 = new FarPoint.Web.Spread.StyleInfo();
                style3.Font.Size = 13;
                style3.Font.Bold = true;
                semspread.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style3);
                semspread.Sheets[0].AllowTableCorner = true;
                semspread.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                SpdInfo.Sheets[0].AutoPostBack = true;
                freestaff.Sheets[0].AutoPostBack = false;
                freestaff.Attributes.Add("onmouseup", "__doPostBack('freestaff','CellClick,' + freestaff.ActiveRow + ',' + freestaff.ActiveCol)");
                semspread.Sheets[0].AutoPostBack = true;
                semspread.Attributes.Add("onmouseup", "__doPostBack('semspread','CellClick,' + semspread.ActiveRow + ',' + semspread.ActiveCol)");
                string dt = DateTime.Today.ToShortDateString();
                string[] dsplit = dt.Split(new Char[] { '/' });
                txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                string stDate;
                stDate = DateTime.Today.ToShortDateString();
                Session["curr_year"] = dsplit[2].ToString();
                string[] dsplit_from = stDate.Split(new Char[] { '/' });
                txtFromDate.Text = dsplit_from[1].ToString() + "/" + dsplit_from[0].ToString() + "/" + dsplit_from[2].ToString();
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
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
                schoolds = dacess.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                    if (schoolvalue.Trim() == "0")
                    {
                        forschoolsetting = true;
                        lblbatch.Text = "Year";
                        lbldegree.Text = "School Type";
                        lblbranch.Text = "Standard";
                        lblduration.Text = "Term";
                        batchbtn.Text = "Year Allocation";
                        lbldegree.Attributes.Add("style", "  position: absolute; margin-top: -8px;");
                        lblbranch.Attributes.Add("style", " margin-left: 34px; margin-top: 4px;  position: absolute;");
                        ddlbranch.Attributes.Add("style", "   margin-left: 107px;");
                        ddldegree.Attributes.Add("style", " margin-left: 94px; position: absolute;margin-top: -12px;");
                    }
                    else
                    {
                        forschoolsetting = false;
                    }
                }
            }
            catch
            {
            }
        }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            DataSet ds1 = new DataSet();
            string sqlstring = string.Empty;
            int max_bat = 0;
            //con.Close();
            //con.Open();
            //cmd = new SqlCommand("select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year", con);
            //SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            //da1.Fill(ds1);
            //con.Close();
            qry = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year";
            ds1 = d2.select_method_wo_parameter(qry, "text");
            ddlbatch.DataSource = ds1;
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();
            sqlstring = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
            max_bat = Convert.ToInt32(GetFunction(sqlstring));
            ddlbatch.SelectedValue = max_bat.ToString();

        }
        catch
        {
        }
    }

    public void binddegree()
    {
        try
        {
            hat.Clear();
            ddldegree.Items.Clear();
            ddldegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
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
            DataSet ds = dacess.select_method("bind_degree", hat, "sp");
            ddldegree.DataSource = ds;
            ddldegree.DataValueField = "course_id";
            ddldegree.DataTextField = "course_name";
            ddldegree.DataBind();
        }
        catch
        {
        }
    }

    public void bindsem()
    {
        try
        {
            ddlduration.Items.Clear();
            if (ddldegree.SelectedValue.ToString() != "" && ddldegree.SelectedValue.ToString() != null)
            {
                bool first_year;
                first_year = false;
                int duration = 0;
                int i = 0;
                con.Close();
                con.Open();
                SqlDataReader dr;
                cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
                dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows == true)
                {
                    first_year = Convert.ToBoolean(dr[1].ToString());
                    duration = Convert.ToInt32(dr[0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlduration.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlduration.Items.Add(i.ToString());
                        }
                    }
                }
                else
                {
                    dr.Close();
                    SqlDataReader dr1;
                    cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"] + "", con);
                    ddlduration.Items.Clear();
                    dr1 = cmd.ExecuteReader();
                    dr1.Read();
                    if (dr1.HasRows == true)
                    {
                        first_year = Convert.ToBoolean(dr1[1].ToString());
                        duration = Convert.ToInt32(dr1[0].ToString());
                        for (i = 1; i <= duration; i++)
                        {
                            if (first_year == false)
                            {
                                ddlduration.Items.Add(i.ToString());
                            }
                            else if (first_year == true && i != 2)
                            {
                                ddlduration.Items.Add(i.ToString());
                            }
                        }
                    }
                    dr1.Close();
                }
                con.Close();
            }
            else
            {
                norecordlbl.Visible = true;
                norecordlbl.ForeColor = Color.Red;
                norecordlbl.Text = "Update Degree Rights For The User";
            }
        }
        catch
        {
        }
    }

    public void bindsec()
    {
        try
        {
            ddlsec.Items.Clear();
            if (ddlbranch.SelectedValue.ToString() != "" && ddlbranch.SelectedValue.ToString() != null)
            {
                con.Close();
                con.Open();
                cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                SqlDataReader dr_sec;
                dr_sec = cmd.ExecuteReader();
                dr_sec.Read();
                if (dr_sec.HasRows == true)
                {
                    if (dr_sec["sections"].ToString() == string.Empty)
                    {
                        ddlsec.Enabled = false;
                    }
                    else
                    {
                        ddlsec.Enabled = true;
                    }
                }
                else
                {
                    ddlsec.Enabled = false;
                }
                frmlbl.Visible = false;
                tolbl.Visible = false;
                con.Close();
            }
            else
            {
                norecordlbl.Visible = true;
                norecordlbl.ForeColor = Color.Red;
                norecordlbl.Text = "Update Degree Rights For The User";
            }
        }
        catch
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            ddlbranch.Items.Clear();
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddldegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            DataSet ds = dacess.select_method("bind_branch", hat, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch
        {
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            SpdInfo.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            norecordlbl.Visible = false;
            Button4.Visible = false;
            sem_schedule.Visible = false;
            btnsave.Visible = false;
            btn_remove.Visible = false;
            batchbtn.Visible = false;
            treepanel.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            bindbranch();
            bindsem();
            bindsec();
        }
        catch
        {
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            SpdInfo.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            norecordlbl.Visible = false;
            Button4.Visible = false;
            sem_schedule.Visible = false;
            btnsave.Visible = false;
            btn_remove.Visible = false;
            batchbtn.Visible = false;
            treepanel.Visible = false;
            bindbranch();
            bindsem();
            bindsec();
        }
        catch
        {
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            SpdInfo.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            norecordlbl.Visible = false;
            Button4.Visible = false;
            sem_schedule.Visible = false;
            btnsave.Visible = false;
            btn_remove.Visible = false;
            batchbtn.Visible = false;
            treepanel.Visible = false;
            bindsem();
            bindsec();
        }
        catch
        {
        }
    }

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            SpdInfo.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            norecordlbl.Visible = false;
            Button4.Visible = false;
            sem_schedule.Visible = false;
            btnsave.Visible = false;
            btn_remove.Visible = false;
            batchbtn.Visible = false;
            treepanel.Visible = false;
            bindsec();
        }
        catch
        {
        }
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            SpdInfo.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            norecordlbl.Visible = false;
            Button4.Visible = false;
            sem_schedule.Visible = false;
            btnsave.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            btn_remove.Visible = false;
            batchbtn.Visible = false;
            treepanel.Visible = false;
        }
        catch
        {
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            SpdInfo.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            norecordlbl.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            Button4.Visible = false;
            sem_schedule.Visible = false;
            btnsave.Visible = false;
            btn_remove.Visible = false;
        }
        catch
        {
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            SpdInfo.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            norecordlbl.Visible = false;
            tolbl.Visible = false;
            Button4.Visible = false;
            sem_schedule.Visible = false;
            btnsave.Visible = false;
            btn_remove.Visible = false;
            //string date1="";
            //string date2 =string.Empty;
            //DateTime dtime;
            //DateTime dtime1;
            //date1 = txtFromDate.Text.ToString();
            //date2 = txtToDate.Text.ToString();
            //string[] split = date1.Split(new Char[] { '/' });
            //string[] split1 = date2.Split(new char[] { '/' });
            //string chkdate = split[1] + "/" + split[0] + "/" + split[2];
            //string chkdate2 = split1[1] + "/" + split1[0] + "/" + split1[2];
            //dtime = Convert.ToDateTime(chkdate.ToString());
            //dtime1 = Convert.ToDateTime(chkdate2.ToString());
            //if (dtime1 < dtime)
            //{
            //    norecordlbl.Text = "To Date Should be Greater than or Equal to From Date";
            //    norecordlbl.Visible = true;
            //}
        }
        catch
        {
        }
    }

    //----------------------------GO button
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btn_remove.Visible = false;
            errmsg.Visible = false;
            if (ddldegree.Text == "-1" || ddldegree.Text == "")
            {
                deglbl.Visible = true;
            }
            if (ddlbranch.Text == "-1" || ddlbranch.Text == "")
            {
                branlbl.Visible = true;
            }
            if (ddlduration.Text == "" || ddlduration.Text == "-1")
            {
                semlbl.Visible = true;
            }
            if (ddlsec.Enabled == true && ddlsec.Text == "")
            {
                seclbl.Visible = true;
            }
            if (ddlsec.Enabled == true && ddlsec.Text == "-1")
            {
                seclbl.Visible = true;
            }
            if (txtFromDate.Text == "")
            {
                frmlbl.Visible = true;
            }
            if (txtToDate.Text == "")
            {
                tolbl.Visible = true;
            }
            if (txtnoofalter.Text == "")
            {
                norecordlbl.Visible = true;
                Panel3.Visible = false;
                norecordlbl.Text = "Please Enter No of Alter Value";
                norecordlbl.ForeColor = Color.Red;
                return;
            }
            if (Convert.ToInt32(txtnoofalter.Text.ToString()) == 0)
            {
                norecordlbl.Visible = true;
                Panel3.Visible = false;
                norecordlbl.Text = "Please Enter No of Alter Must Be Greater Than Zero";
                norecordlbl.ForeColor = Color.Red;
                return;
            }
            if (ddlsec.Enabled == true && ddlsec.Text != "-1" && txtFromDate.Text != "" && txtToDate.Text != "" && Convert.ToInt32(txtnoofalter.Text) > 0)
            {
                gobutton();
            }
            if (ddlsec.Enabled == false && txtFromDate.Text != "" && txtToDate.Text != "" && Convert.ToInt32(txtnoofalter.Text) > 0)
            {
                gobutton();
            }
        }
        catch
        {
        }
    }

    public void gobutton()
    {
        try
        {
            string date1 = string.Empty;
            string date2 = string.Empty;
            string datefrom, dateto;
            string todate = string.Empty;
            string sec_txt = string.Empty;
            string classTimeTableId = string.Empty;

            int intNCtr;
            int rowval = 0;
            string srt_day = string.Empty;
            string splvalnew = string.Empty;
            string splval = string.Empty;
            string startdate = string.Empty;
            string setcellnote = string.Empty;
            bool noflag = false;
            bool alterfalg = true;
            norecordlbl.Visible = false;
            int noofalter = Convert.ToInt32(txtnoofalter.Text.ToString());
            int noaltval = Convert.ToInt32(txtnoofalter.Text.ToString());
            sec_txt = ddlsec.Text;
            string set_lock = string.Empty;
            con.Open();
            string grouporusercode = string.Empty;
            DataTable dtSemSchedule = new DataTable();
            DataTable dtSemAlterSchedule = new DataTable();

            if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code='" + Session["group_code"].ToString().Trim() + "'";
            }
            else
            {
                grouporusercode = " and usercode='" + Session["usercode"].ToString().Trim() + "'";
            }
            SqlCommand cmd_schedlock = new SqlCommand("select value from Master_Settings where settings='schedule_lock' " + grouporusercode + "", con);
            string schedval = (string)cmd_schedlock.ExecuteScalar();
            if (Session["UserName"].ToString().Trim() == "admin")
            {
                schedval = null;
            }
            string holidyres = string.Empty;
            bool shedulelock = false;
            if (!string.IsNullOrEmpty(schedval))
            {
                if (schedval == "1")
                {
                    shedulelock = true;
                    string[] convertfromdate = txtFromDate.Text.Split(new char[] { '/' });
                    string[] converttodate = txtToDate.Text.Split(new char[] { '/' });
                    string convertedfdate = convertfromdate[1] + "/" + convertfromdate[0] + "/" + convertfromdate[2];
                    string convertedtdate = converttodate[1] + "/" + converttodate[0] + "/" + converttodate[2];
                    DateTime chkfromdate = Convert.ToDateTime(convertedfdate);
                    DateTime chktodate = Convert.ToDateTime(convertedtdate);
                    if (chkfromdate > chktodate)
                    {
                        frmlbl.Visible = true;
                        frmlbl.Text = "Entar valid from date";
                        batchbtn.Visible = false;
                        return;
                    }
                    else
                    {
                        string scheddate = Convert.ToString(System.DateTime.Now);
                        string[] splitscheddatetime = scheddate.Split(new char[] { ' ' });
                        string splitedscheddate = splitscheddatetime[0].ToString();
                        string[] splitscheddate = splitedscheddate.Split(new char[] { '/' });
                        date1 = splitscheddate[1] + "/" + splitscheddate[0] + "/" + splitscheddate[2];
                        set_lock = "Settings_True";
                    }
                }
                else
                {
                    date1 = txtFromDate.Text.ToString();
                }
            }
            else
            {
                date1 = txtFromDate.Text.ToString();
            }
            con.Close();
            string[] split = date1.Split(new Char[] { '/' });
            string splitenddate = string.Empty;
            if (split.GetUpperBound(0) == 2)
            {
                if (Convert.ToInt32(split[0].ToString()) <= 31 && Convert.ToInt32(split[1].ToString()) <= 12 && Convert.ToInt32(split[0].ToString()) <= Convert.ToInt32(Session["curr_year"]))
                {
                    datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                    date2 = txtToDate.Text.ToString();
                    string[] split1 = date2.Split(new Char[] { '/' });
                    string qrySection = string.Empty;
                    string qrySection1 = string.Empty;
                    string sectionName = string.Empty;
                    if (ddlsec.Items.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(ddlsec.SelectedItem.Text).Trim()) && Convert.ToString(ddlsec.SelectedItem.Text).Trim().Trim().ToLower() != "all" && Convert.ToString(ddlsec.SelectedItem.Text).Trim().Trim().ToLower() != "-1")
                        {
                            sectionName = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                            qrySection = " and ct.TT_sec='" + Convert.ToString(sectionName).Trim() + "'";
                            qrySection1 = " and nct.TT_sec='" + Convert.ToString(sectionName).Trim() + "'";
                        }
                    }

                    string qryTT = "select distinct ct.TT_ClassPK,ct.TT_colCode,ct.TT_batchyear,ct.TT_degCode,ct.TT_sem,ct.TT_sec,ct.TT_lastRec,ct.TT_name,ct.TT_date,ctd.TT_subno,s.subject_code,s.subject_name,ctd.TT_Day,ctd.TT_Hour,SUBSTRING(do.Daydiscription,1,3)+CONVERT(varchar(20),ctd.TT_Hour) as DAY from TT_ClassTimetable ct,TT_ClassTimetableDet ctd,subject s,TT_Day_Dayorder do where ctd.TT_Day=do.TT_Day_DayorderPK and s.subject_no=ctd.TT_subno and ct.TT_ClassPK  =ctd.TT_ClassFk  and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_batchyear='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' and ct.TT_degCode ='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' and ct.TT_lastRec='1' " + qrySection + "  and ct.TT_date=(select MAX(nct.TT_date) from TT_ClassTimetable nct where nct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and nct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and nct.TT_batchyear='" + ddlbatch.SelectedValue + "' and nct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection1 + ") order by ct.TT_colCode,ct.TT_batchyear,ct.TT_degCode,ct.TT_sem,ct.TT_sec,ctd.TT_Day,ctd.TT_Hour";
                    //DataTable dtSchedule = dirAcc.selectDataTable(qryTT);

                    string qryAlterTT = "select ct.TT_ClassPK,ct.TT_colCode,ct.TT_batchyear,ct.TT_degCode,ct.TT_sem,ct.TT_sec,ct.TT_lastRec,ct.TT_name,ct.TT_date,ctd.TT_AlterDetPK,ctd.TT_subno,s.subject_code,s.subject_name,ctd.TT_staffcode,ctd.TT_Room,ctd.TT_Day,ctd.TT_Hour,ctd.TT_AlterDate,SUBSTRING(do.Daydiscription,1,3)+CONVERT(varchar(20),ctd.TT_Hour) as DAY from TT_ClassTimetable ct,TT_AlterTimetableDet ctd ,subject s,TT_Day_Dayorder do where ctd.TT_Day=do.TT_Day_DayorderPK and s.subject_no=ctd.TT_subno and ct.TT_ClassPK  =ctd.TT_CLassFK and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_batchyear='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' and ct.TT_degCode ='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' and TT_lastRec='1' " + qrySection + " order by ct.TT_colCode,ct.TT_batchyear,ct.TT_degCode,ct.TT_sem,ct.TT_sec,ctd.TT_Day,ctd.TT_Hour";
                    //DataTable dtAlterSchedule = dirAcc.selectDataTable(qryAlterTT);

                    if (split1.GetUpperBound(0) == 2)
                    {
                        if (Convert.ToInt32(split1[0].ToString()) <= 31 && Convert.ToInt32(split1[1].ToString()) <= 12 && Convert.ToInt32(split1[0].ToString()) <= Convert.ToInt32(Session["curr_year"]))
                        {
                            dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                            DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                            DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                            TimeSpan t = dt2.Subtract(dt1);
                            long days = t.Days;
                            //get No_of_hrs_per_day,schorder,nodays
                            int frhlfhr = 0;
                            DateTime CtDate;
                            CtDate = DateTime.Now;
                            con.Open();
                            SqlDataReader dr;
                            cmd = new SqlCommand("Select No_of_hrs_per_day,schorder,nodays,no_of_hrs_I_half_day from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + "", con);
                            dr = cmd.ExecuteReader();
                            dr.Read();
                            if (dr.HasRows == true)
                            {
                                if ((dr["No_of_hrs_per_day"].ToString()) != "")
                                {
                                    intNHrs = Convert.ToInt32(dr["No_of_hrs_per_day"]);
                                    SchOrder = Convert.ToInt32(dr["schorder"]);
                                    nodays = Convert.ToInt32(dr["nodays"]);
                                    frhlfhr = Convert.ToInt32(dr["no_of_hrs_I_half_day"]);
                                }
                            }
                            dr.Close();
                            con.Close();
                            //Idhris

                            classTimeTableId = dirAcc.selectScalarString("select top 1 ct.TT_ClassPK from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection + " and ct.TT_date=(select MAX(nct.TT_date) from TT_ClassTimetable nct where nct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and nct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and nct.TT_batchyear='" + ddlbatch.SelectedValue + "' and nct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection1 + " ) order by ct.TT_date desc ");//and TT_date<='" + dateval + "'

                            DataTable dtSelect = dirAcc.selectDataTable("select TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room,ct.TT_ClassPK from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection + " and ct.TT_date<='" + dateto + "' order by ct.TT_date desc");//and TT_date>='" + datefrom + "' and TT_date<='" + dateto + "' (select MAX(nct.TT_date) from TT_ClassTimetable nct where nct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and nct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and nct.TT_batchyear='" + ddlbatch.SelectedValue + "' and nct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection1 + ")
                            DataTable dtSchedule = new DataTable();
                            //dtSchedule.Columns.Add("TT_ClassPK");
                            //dtSchedule.Columns.Add("TT_date");

                            DataTable dtAlterSchedule = new DataTable();

                            DataTable dtMinMaxSchedule = new DataTable();

                            SemesterandAlternateSchedule(Convert.ToString(Session["collegecode"]).Trim(), ddlbatch.SelectedValue, Convert.ToString(ddlbranch.SelectedValue).Trim(), Convert.ToString(ddlduration.SelectedValue).Trim(), sectionName, datefrom, dateto, intNHrs, ref dtSchedule, ref  dtAlterSchedule);

                            #region Commented
                            //dtMinMaxSchedule = dirAcc.selectDataTable("select CONVERT(varchar(20),min(ct.TT_date),103) as FromDate,CONVERT(varchar(20),max(ct.TT_date),103) as ToDate from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_degCode='54' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' and ct.TT_date <='" + dateto + "'");

                            //if (dtMinMaxSchedule.Rows.Count > 0)
                            //{
                            //    DataRow drSchedule;//= dtSchedule.NewRow();
                            //    DateTime dtMinDate = new DateTime();
                            //    DateTime dtMaxDate = new DateTime();
                            //    if (Convert.ToString(dtMinMaxSchedule.Rows[0]["FromDate"]).Trim() != "" && Convert.ToString(dtMinMaxSchedule.Rows[0]["ToDate"]).Trim() != "")
                            //    {
                            //        DateTime.TryParseExact(Convert.ToString(dtMinMaxSchedule.Rows[0]["FromDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMinDate);
                            //        DateTime.TryParseExact(Convert.ToString(dtMinMaxSchedule.Rows[0]["ToDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMaxDate);
                            //        for (DateTime dtTemp = dtMinDate; dtTemp <= dtMaxDate; dtTemp = dtTemp.AddDays(1))
                            //        {
                            //            drSchedule = dtSchedule.NewRow();
                            //            drSchedule["TT_date"] = Convert.ToString(dtTemp.ToString("MM/dd/yyyy"));
                            //            for (int i = 0; i < 6; i++)
                            //            {
                            //                string curday = string.Empty;
                            //                string curdayFull = string.Empty;
                            //                switch (i)
                            //                {
                            //                    case 0:
                            //                        curdayFull = "Monday";
                            //                        curday = "mon";
                            //                        break;
                            //                    case 1:
                            //                        curdayFull = "Tuesday";
                            //                        curday = "tue";
                            //                        break;
                            //                    case 2:
                            //                        curdayFull = "Wednesday";
                            //                        curday = "wed";
                            //                        break;
                            //                    case 3:
                            //                        curdayFull = "Thursday";
                            //                        curday = "thu";
                            //                        break;
                            //                    case 4:
                            //                        curdayFull = "Friday";
                            //                        curday = "fri";
                            //                        break;
                            //                    case 5:
                            //                        curdayFull = "Saturday";
                            //                        curday = "sat";
                            //                        break;
                            //                }
                            //                for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                            //                {
                            //                    if (!dtSchedule.Columns.Contains(curday + hrsI))
                            //                        dtSchedule.Columns.Add(curday + hrsI);

                            //                    dtSelect.DefaultView.RowFilter = "Daydiscription='" + curdayFull + "' and TT_Hour='" + hrsI + "'";
                            //                    DataTable dtFilter = dtSelect.DefaultView.ToTable();
                            //                    if (dtFilter.Rows.Count > 0)
                            //                    {
                            //                        drSchedule["TT_ClassPK"] = Convert.ToString(dtFilter.Rows[0]["TT_ClassPK"]);
                            //                        StringBuilder sbNew = new StringBuilder();
                            //                        for (int dtI = 0; dtI < dtFilter.Rows.Count; dtI++)
                            //                        {
                            //                            string subno = Convert.ToString(dtFilter.Rows[dtI]["TT_subno"]);
                            //                            string stfcode = Convert.ToString(dtFilter.Rows[dtI]["TT_staffcode"]);
                            //                            string elect = Convert.ToString(dtFilter.Rows[dtI]["Elective"]);
                            //                            string Lab = Convert.ToString(dtFilter.Rows[dtI]["Lab"]);
                            //                            string practicalpair = Convert.ToString(dtFilter.Rows[dtI]["practicalpair"]);
                            //                            string differenciator = "S";
                            //                            string room = Convert.ToString(dtFilter.Rows[dtI]["room"]);
                            //                            if (elect.ToLower() == "true")
                            //                            {
                            //                                differenciator = "E";
                            //                            }
                            //                            else if (Lab.ToLower() == "true")
                            //                            {
                            //                                differenciator = "L";
                            //                            }
                            //                            else if (practicalpair != "0")
                            //                            {
                            //                                differenciator = "C";
                            //                            }

                            //                            sbNew.Append(subno + "-" + stfcode + "-" + differenciator + "-" + room + ";");
                            //                        }
                            //                        drSchedule[curday + hrsI] = sbNew.ToString();
                            //                    }
                            //                }
                            //            }
                            //            dtSchedule.Rows.Add(drSchedule);
                            //        }
                            //    }
                            //}

                            //DataTable dtAlterSelect = dirAcc.selectDataTable("select TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,ctd.TT_AlterDate,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room,ctd.TT_AlterDate from TT_ClassTimetable ct, TT_AlterTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_CLassFK and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection + "  --and ctd.TT_AlterDate>='" + datefrom + "' and ctd.TT_AlterDate<='" + dateto + "' ");
                            ////DataTable dtAlterSchedule = new DataTable();
                            //dtAlterSchedule.Columns.Add("TT_AlterDate");
                            //dtAlterSchedule.Columns.Add("TT_ClassFK");
                            //DataTable dtMinMaxDate = dirAcc.selectDataTable("select CONVERT(varchar(20),min(ctd.TT_AlterDate),103) as FromDate,CONVERT(varchar(20),max(ctd.TT_AlterDate),103) as ToDate from TT_ClassTimetable ct, TT_AlterTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_CLassFK and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection + "  --and ctd.TT_AlterDate>='" + datefrom + "' and ctd.TT_AlterDate<='" + dateto + "'");

                            //if (dtMinMaxDate.Rows.Count > 0)
                            //{
                            //    DataRow drAltert;// = dtAlterSchedule.NewRow();
                            //    DateTime dtMinDate = new DateTime();
                            //    DateTime dtMaxDate = new DateTime();
                            //    if (Convert.ToString(dtMinMaxDate.Rows[0]["FromDate"]).Trim() != "" && Convert.ToString(dtMinMaxDate.Rows[0]["ToDate"]).Trim() != "")
                            //    {
                            //        DateTime.TryParseExact(Convert.ToString(dtMinMaxDate.Rows[0]["FromDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMinDate);
                            //        DateTime.TryParseExact(Convert.ToString(dtMinMaxDate.Rows[0]["ToDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMaxDate);
                            //        for (DateTime dtTemp = dtMinDate; dtTemp <= dtMaxDate; dtTemp = dtTemp.AddDays(1))
                            //        {
                            //            drAltert = dtAlterSchedule.NewRow();
                            //            drAltert["TT_AlterDate"] = Convert.ToString(dtTemp.ToString("MM/dd/yyyy"));
                            //            for (int i = 0; i < 6; i++)
                            //            {
                            //                string curday = string.Empty;
                            //                string curdayFull = string.Empty;
                            //                switch (i)
                            //                {
                            //                    case 0:
                            //                        curdayFull = "Monday";
                            //                        curday = "mon";
                            //                        break;
                            //                    case 1:
                            //                        curdayFull = "Tuesday";
                            //                        curday = "tue";
                            //                        break;
                            //                    case 2:
                            //                        curdayFull = "Wednesday";
                            //                        curday = "wed";
                            //                        break;
                            //                    case 3:
                            //                        curdayFull = "Thursday";
                            //                        curday = "thu";
                            //                        break;
                            //                    case 4:
                            //                        curdayFull = "Friday";
                            //                        curday = "fri";
                            //                        break;
                            //                    case 5:
                            //                        curdayFull = "Saturday";
                            //                        curday = "sat";
                            //                        break;
                            //                }
                            //                for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                            //                {
                            //                    if (!dtAlterSchedule.Columns.Contains(curday + hrsI))
                            //                        dtAlterSchedule.Columns.Add(curday + hrsI);

                            //                    dtAlterSelect.DefaultView.RowFilter = "Daydiscription='" + curdayFull + "' and TT_Hour='" + hrsI + "' and TT_AlterDate='" + dtTemp.ToString("MM/dd/yyyy") + "'";
                            //                    DataTable dtFilter = dtAlterSelect.DefaultView.ToTable();
                            //                    if (dtFilter.Rows.Count > 0)
                            //                    {
                            //                        StringBuilder sbNew = new StringBuilder();
                            //                        for (int dtI = 0; dtI < dtFilter.Rows.Count; dtI++)
                            //                        {
                            //                            string subno = Convert.ToString(dtFilter.Rows[dtI]["TT_subno"]);
                            //                            string stfcode = Convert.ToString(dtFilter.Rows[dtI]["TT_staffcode"]);
                            //                            string elect = Convert.ToString(dtFilter.Rows[dtI]["Elective"]);
                            //                            string Lab = Convert.ToString(dtFilter.Rows[dtI]["Lab"]);
                            //                            string practicalpair = Convert.ToString(dtFilter.Rows[dtI]["practicalpair"]);
                            //                            string differenciator = "S";
                            //                            string room = Convert.ToString(dtFilter.Rows[dtI]["room"]);
                            //                            if (elect.ToLower() == "true")
                            //                            {
                            //                                differenciator = "E";
                            //                            }
                            //                            else if (Lab.ToLower() == "true")
                            //                            {
                            //                                differenciator = "L";
                            //                            }
                            //                            else if (practicalpair != "0")
                            //                            {
                            //                                differenciator = "C";
                            //                            }
                            //                            sbNew.Append(subno + "-" + stfcode + "-" + differenciator + "-" + room + ";");
                            //                        }
                            //                        drAltert[curday + hrsI] = sbNew.ToString();
                            //                    }
                            //                }
                            //            }
                            //            dtAlterSchedule.Rows.Add(drAltert);
                            //        }
                            //    }
                            //} 

                            #endregion

                            if (shedulelock == true)
                            {
                                DateTime dtnow = Convert.ToDateTime(dt1.ToString("MM/dd/yyyy"));
                                string[] spf = txtFromDate.Text.ToString().Split('/');
                                DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
                                DateTime dtt = Convert.ToDateTime(split1[1] + '/' + split1[0] + '/' + split1[2]);
                                if (dtnow > dtf || dtnow > dtt)
                                {
                                    days = -2;
                                }
                            }
                            if (days >= 0)//-----check date difference
                            {
                                date1 = txtFromDate.Text.ToString();
                                string[] fdt = date1.Split('/');
                                datefrom = fdt[1].PadLeft(2, '0') + '/' + fdt[0].PadLeft(2, '0') + '/' + fdt[2];
                                dt1 = Convert.ToDateTime(datefrom);
                                t = dt2.Subtract(dt1);
                                days = t.Days;
                                tofromlbl.Visible = false;
                                string strsec;
                                SpdInfo.Sheets[0].ColumnCount = 0;
                                SpdInfo.Sheets[0].RowCount = 0;
                                if (ddlsec.Text.ToString() == "" || ddlsec.Text.ToString() == "-1")
                                {
                                    strsec = string.Empty;
                                }
                                else
                                {
                                    strsec = " and sections='" + ddlsec.Text.ToString() + "'";
                                }

                                con.Open();
                                SqlDataReader dr1;
                                cmd = new SqlCommand("select * from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " ", con);
                                dr1 = cmd.ExecuteReader();
                                dr1.Read();
                                if (dr1.HasRows == true)
                                {
                                    if ((dr1["start_date"].ToString()) != "" && (dr1["start_date"].ToString()) != "\0")
                                    {
                                        string[] tmpdate = dr1["start_date"].ToString().Split(new char[] { ' ' });
                                        string[] enddate = dr1["end_date"].ToString().Split(new char[] { ' ' });
                                        startdate = tmpdate[0].ToString();
                                        splitenddate = enddate[0].ToString();
                                        if (Convert.ToString(dr1["starting_dayorder"]) != "")
                                        {
                                            start_dayorder = dr1["starting_dayorder"].ToString();
                                        }
                                        else
                                        {
                                            start_dayorder = "1";
                                        }
                                    }
                                    else
                                    {
                                        norecordlbl.Visible = true;
                                        Panel3.Visible = false;
                                        norecordlbl.Text = "Update semester Information";
                                        norecordlbl.ForeColor = Color.Red;
                                        return;
                                    }
                                }
                                else
                                {
                                    norecordlbl.Visible = true;
                                    Panel3.Visible = false;
                                    norecordlbl.Text = "Update semester Information";
                                    norecordlbl.ForeColor = Color.Red;
                                    return;
                                }
                                //Added by srinath 6/9/2014 For Day Order Change=======Start====================
                                string strdayoredr = "Select * from tbl_consider_day_order where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + "  and ((From_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "') or (To_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "'))";
                                DataSet dsdayorder = dacess.select_method_wo_parameter(strdayoredr, "Text");
                                Hashtable hatdoc = new Hashtable();
                                for (int doc = 0; doc < dsdayorder.Tables[0].Rows.Count; doc++)
                                {
                                    DateTime dtsdoc = Convert.ToDateTime(dsdayorder.Tables[0].Rows[doc]["from_date"].ToString());
                                    DateTime dtedoc = Convert.ToDateTime(dsdayorder.Tables[0].Rows[doc]["to_date"].ToString());
                                    string reason = dsdayorder.Tables[0].Rows[doc]["Reason"].ToString();
                                    for (DateTime dtc = dtsdoc; dtc <= dtedoc; dtc = dtc.AddDays(1))
                                    {
                                        if (!hatdoc.Contains(dtc))
                                        {
                                            hatdoc.Add(dtc, reason);
                                        }
                                    }
                                }
                                //=================================End======================================
                                SpdInfo.Sheets[0].SheetCorner.RowCount = 2;
                                SpdInfo.Sheets[0].SheetCorner.Cells[0, 0].Text = "Period";
                                SpdInfo.ActiveSheetView.SheetCornerSpanModel.Add(0, 0, 2, 1);
                                SpdInfo.Sheets[0].RowCount = SpdInfo.Sheets[0].RowCount + intNHrs;
                                string[] differdays = new string[days];
                                SpdInfo.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;//Added by Manikandan 15/08/2013
                                //First Date --------------------------------------------------------------------------------------------            
                                //Modified By Srinath 16/10/2013
                                //SpdInfo.Sheets[0].ColumnCount = SpdInfo.Sheets[0].ColumnCount + 2;
                                SpdInfo.Sheets[0].ColumnCount = SpdInfo.Sheets[0].ColumnCount + noofalter + 1;
                                SpdInfo.Sheets[0].Columns[0].Width = 100;
                                SpdInfo.Sheets[0].Columns[0].Locked = true;
                                SpdInfo.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, noofalter + 1);
                                SpdInfo.Sheets[0].ColumnHeader.Cells[0, 0].Text = date1;
                                SpdInfo.Sheets[0].ColumnHeader.Cells[0, 0].Note = date1;
                                SpdInfo.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Schedule List";
                                SpdInfo.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                                SpdInfo.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                                SpdInfo.Sheets[0].Columns[0].ForeColor = Color.Blue;
                                SpdInfo.Sheets[0].Columns[0].Font.Underline = true;
                                for (int i = 0; i < noofalter; i++)
                                {
                                    int col = i + 1;
                                    SpdInfo.Sheets[0].ColumnHeader.Cells[0, col].Note = date1;
                                    SpdInfo.Sheets[0].ColumnHeader.Cells[1, col].Text = "Alternate Schedule " + col + "";
                                    SpdInfo.Sheets[0].ColumnHeader.Cells[1, col].Note = date1;
                                    SpdInfo.Sheets[0].Columns[col].Font.Name = "Book Antiqua";
                                    SpdInfo.Sheets[0].Columns[col].Font.Size = FontUnit.Medium;
                                    SpdInfo.Sheets[0].Columns[col].Width = 100;
                                }
                                //------------find schedule order type
                                if (intNHrs > 0)
                                {
                                    if (SchOrder != 0)
                                    {
                                        srt_day = dt1.ToString("ddd");
                                    }
                                    else
                                    {
                                        todate = SpdInfo.Sheets[0].ColumnHeader.Cells[0, 0].Text;
                                        //Modified By Srinath 5/9/2014
                                        // srt_day = findday(todate.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                                        string[] sps = todate.ToString().Split('/');
                                        string curdate = sps[1] + '/' + sps[0] + '/' + sps[2];
                                        srt_day = dacess.findday(curdate.ToString(), ddlbranch.SelectedValue.ToString(), ddlduration.SelectedValue.ToString(), ddlbatch.Text.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                                    }
                                }
                                //---check day value
                                if ((dt1 >= Convert.ToDateTime(startdate) && dt1 <= Convert.ToDateTime(splitenddate)) && (dt2 >= Convert.ToDateTime(startdate) && dt2 <= Convert.ToDateTime(splitenddate)))//this if condition added by Manikandan 29/08/2013  // change by sridhar 03 sep 2014
                                {
                                    if (srt_day != "Sun")
                                    {
                                        String sqlsrt = "select top 1 ";
                                        string noofaltee = "select no_of_alter,";
                                        for (intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                                        {
                                            sqlsrt = sqlsrt + srt_day + intNCtr.ToString() + ",";
                                            noofaltee = noofaltee + srt_day + intNCtr.ToString() + ",";
                                        }
                                        //-----------set value into spread
                                        //Start=====Added by Manikandan 26/08/2013========
                                        con.Close();
                                        con.Open();
                                        SqlDataReader dr_holday;
                                        string holday = string.Empty;
                                        bool morleave = false;
                                        bool eveleave = false;
                                        int starthour = 1;
                                        SqlCommand cmd_holday = new SqlCommand("select * from holidaystudents  where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and holiday_date ='" + datefrom.ToString() + "'", con);
                                        dr_holday = cmd_holday.ExecuteReader();
                                        dr_holday.Read();
                                        if (dr_holday.HasRows == true)
                                        {
                                            holday = dr_holday["holiday_desc"].ToString();
                                            string halle = dr_holday["halforfull"].ToString();
                                            if (dr_holday["halforfull"].ToString().Trim() == "1" || dr_holday["halforfull"].ToString().Trim().ToLower() == "true")
                                            {
                                                if (dr_holday["morning"].ToString().Trim() == "1" || dr_holday["morning"].ToString().Trim().ToLower() == "true")
                                                {
                                                    morleave = true;
                                                }
                                                if (dr_holday["evening"].ToString().Trim() == "1" || dr_holday["evening"].ToString().Trim().ToLower() == "true")
                                                {
                                                    eveleave = true;
                                                }
                                            }
                                            else
                                            {
                                                morleave = true;
                                                eveleave = true;
                                            }
                                        }
                                        //====================End=========================
                                        //dr1.Close();
                                        //con.Close();
                                        //con.Open();

                                        //SqlDataReader dr3;
                                        //cmd = new SqlCommand(sqlsrt + "degree_code,semester,batch_year from semester_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate<= ' " + datefrom.ToString() + " ' " + strsec + " order by fromdate desc", con);
                                        //cmd = new SqlCommand(qryTT, con);
                                        //dr3 = cmd.ExecuteReader();
                                        //dr3.Read();
                                        tempcon.Close();
                                        tempcon.Open();
                                        string alternatevalue = sqlsrt + " degree_code , semester , batch_year from Alternate_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate= '" + datefrom.ToString() + "' " + strsec + " ";
                                        //DataSet dsalteranet = dacess.select_method(alternatevalue, hat, "Text");
                                        //DataSet dsalteranet = dacess.select_method(qryAlterTT, hat, "Text");
                                        //SqlDataReader dr_sch;
                                        //SqlCommand cmd_sch;
                                        //cmd_sch = new SqlCommand(sqlsrt + " degree_code , semester , batch_year from Alternate_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate= '" + datefrom.ToString() + "' " + strsec + " ", tempcon);
                                        //dr_sch = cmd_sch.ExecuteReader();
                                        //dr_sch.Read();
                                        dtSemSchedule.Clear();
                                        if (dtSchedule.Rows.Count > 0)
                                        {
                                            dtSchedule.DefaultView.RowFilter = "TTDate<='" + dt1 + "'";
                                            dtSchedule.DefaultView.Sort = "TTDate desc";
                                            dtSemSchedule = dtSchedule.DefaultView.ToTable();
                                        }
                                        if (dtSemSchedule.Rows.Count > 0)
                                        {
                                            if (holidyres == "")
                                            {
                                                holidyres = dt1.ToString("dd/MM/yyyy") + " is Holiday- " + holday;
                                            }
                                            else
                                            {
                                                holidyres = holidyres + ',' + dt1.ToString("dd/MM/yyyy") + " is Holiday- " + holday;
                                            }
                                            for (intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                                            {
                                                string dayValue = srt_day.Trim() + intNCtr;
                                                SpdInfo.Sheets[0].Cells[intNCtr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                                SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                                bool leavefa = false;
                                                if (morleave == true)
                                                {
                                                    if (intNCtr < frhlfhr + 1)
                                                    {
                                                        leavefa = true;
                                                    }
                                                }
                                                if (eveleave == true)
                                                {
                                                    if (intNCtr > frhlfhr)
                                                    {
                                                        leavefa = true;
                                                    }
                                                }
                                                if (leavefa == true)
                                                {
                                                    if (holday != "" && holday != null)//this line added by Manikandan 26/08/2013
                                                    {
                                                        if (dtSemSchedule.Rows[0][dayValue].ToString() != "" && dtSemSchedule.Rows[0][dayValue].ToString() != "\0")
                                                        {
                                                            // SpdInfo.Sheets[0].Cells[rowval, 0].Text = Convert.ToString(splvalnew);//--19/7/12 PRABHA
                                                            SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].Text = holday + " Holiday";
                                                            SpdInfo.Sheets[0].Cells[intNCtr - 1, 1].Text = holday + " Holiday";
                                                            SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].Locked = true;
                                                        }
                                                    }
                                                }
                                                else//this line added by Manikandan 26/08/2013
                                                {
                                                    noflag = true;
                                                    if (dtSemSchedule.Rows[0][dayValue].ToString() != "" && dtSemSchedule.Rows[0][dayValue].ToString() != "\0")
                                                    {
                                                        //============Day Order Change Added by Srinath 6/9/2014=================
                                                        if (hatdoc.Contains(dt1))
                                                        {
                                                            splvalnew = hatdoc[dt1].ToString();
                                                            SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].Locked = true;
                                                        }
                                                        else
                                                        {
                                                            string[] subjnew = ((dtSemSchedule.Rows[0][dayValue].ToString())).Split(new Char[] { ';' });
                                                            for (int i = 0; i <= subjnew.GetUpperBound(0); i++)
                                                            {
                                                                if (subjnew.GetUpperBound(0) >= 0)
                                                                {
                                                                    string[] subjstr = subjnew[i].Split(new Char[] { '-' });
                                                                    if (subjstr.GetUpperBound(0) >= 2)
                                                                    {
                                                                        string strsub = GetFunction("select subject_name from subject where subject_no=" + subjstr[0] + " ");
                                                                        getcon.Close();
                                                                        splvalnew = splvalnew + ((strsub.ToString()) + "-" + subjstr[1] + "-" + subjstr[2]) + ";";
                                                                    }
                                                                }
                                                            }
                                                            SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].Locked = false;
                                                        }
                                                        // SpdInfo.Sheets[0].Cells[rowval, 0].Text = Convert.ToString(splvalnew);//--19/7/12 PRABHA
                                                        SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].Text = Convert.ToString(splvalnew);
                                                        SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].Tag = Convert.ToString(dtSemSchedule.Rows[0]["TT_ClassPK"]).Trim();
                                                    }
                                                    else
                                                    {
                                                        SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].Tag = Convert.ToString(dtSemSchedule.Rows[0]["TT_ClassPK"]).Trim();
                                                    }
                                                }
                                                splvalnew = string.Empty;
                                                setcellnote = string.Empty;
                                                if (alterfalg == true)
                                                {
                                                    //Mofified by srinath 20/2/2014
                                                    alterfalg = false;
                                                    string alternatedetailks = alternatevalue;
                                                    if (noaltval > 1)
                                                    {
                                                        alternatedetailks = noofaltee + "degree_code,semester,batch_year from tbl_alter_schedule_Details where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate= ' " + datefrom.ToString() + " ' " + strsec + " order by no_of_alter, fromdate desc";
                                                    }
                                                    DataSet dsalternate = new DataSet();
                                                    DataTable dtAlter = new DataTable();
                                                    if (dtAlterSchedule.Rows.Count > 0)
                                                    {
                                                        dtAlterSchedule.DefaultView.RowFilter = "TT_AlterDate= '" + datefrom.ToString() + "'";
                                                        dtAlter = dtAlterSchedule.DefaultView.ToTable();
                                                    }
                                                    dsalternate.Tables.Add(dtAlter);
                                                    //DataSet dsalternate = dacess.select_method(qryAlterTT, hat, "Text");
                                                    if (dsalternate.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int hour = 1; hour <= intNHrs; hour++)
                                                        {
                                                            string alterDayValue = string.Empty;
                                                            for (int alternatehour = 0; alternatehour < dsalternate.Tables[0].Rows.Count; alternatehour++)
                                                            {
                                                                if (alternatehour + 1 <= noofalter)
                                                                {
                                                                    string column = srt_day + hour;
                                                                    string value = dsalternate.Tables[0].Rows[alternatehour]["" + column + ""].ToString().Trim();
                                                                    splval = string.Empty;
                                                                    leavefa = false;
                                                                    if (morleave == true)
                                                                    {
                                                                        if (hour < frhlfhr + 1)
                                                                        {
                                                                            leavefa = true;
                                                                        }
                                                                    }
                                                                    if (eveleave == true)
                                                                    {
                                                                        if (hour > frhlfhr)
                                                                        {
                                                                            leavefa = true;
                                                                        }
                                                                    }
                                                                    if (leavefa == true)
                                                                    {
                                                                        if (holday != "" && holday != null)//this line added by Manikandan 26/08/2013
                                                                        {
                                                                            if (value != "" && value != "\0")
                                                                            {
                                                                                SpdInfo.Sheets[0].Cells[hour - 1, alternatehour + 1].Text = holday + " Holiday";
                                                                                SpdInfo.Sheets[0].Cells[hour - 1, alternatehour + 1].Note = holday + " Holiday";
                                                                                SpdInfo.Sheets[0].Cells[hour - 1, alternatehour + 1].Locked = true;
                                                                                splval = string.Empty;
                                                                                batchbtn.Visible = true;//Added by Manikandan 24/08/2013
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (value != "" && value != "\0")
                                                                        {
                                                                            if (hatdoc.Contains(dt1))
                                                                            {
                                                                                SpdInfo.Sheets[0].Cells[hour - 1, alternatehour + 1].Locked = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                SpdInfo.Sheets[0].Cells[hour - 1, alternatehour + 1].Locked = false;
                                                                                setcellnote = value;
                                                                                string[] sple = (value).Split(new Char[] { ';' });
                                                                                for (int i = 0; i <= sple.GetUpperBound(0); i++)
                                                                                {
                                                                                    if (sple.GetUpperBound(0) >= 0)
                                                                                    {
                                                                                        string[] sp1 = (sple[i].ToString()).Split(new Char[] { '-' });
                                                                                        if (sp1.GetUpperBound(0) >= 2)
                                                                                        {
                                                                                            splval = splval + (GetFunction("select subject_name from subject where subject_no=" + sp1[0].ToString() + " ") + "-" + sp1[1].ToString() + "-" + sp1[2].ToString()) + ";";
                                                                                            tempcon.Close();
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            SpdInfo.Sheets[0].Cells[hour - 1, alternatehour + 1].Text = Convert.ToString(splval);
                                                                            SpdInfo.Sheets[0].Cells[hour - 1, alternatehour + 1].Note = Convert.ToString(setcellnote);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                rowval = rowval + 1;
                                                //dr_sch.Close();
                                                //}
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                                        {
                                            SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].Text = "Sunday";
                                            SpdInfo.Sheets[0].Cells[intNCtr - 1, 1].Text = "Sunday";
                                            SpdInfo.Sheets[0].Cells[intNCtr - 1, 1].Note = "Holiday";
                                            SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].Note = "Holiday";
                                        }
                                    }
                                }
                                else//this else condition added by Manikandan 29/08/2013
                                {
                                    //  tofromlbl.Text = "The given date Must be between Semester day";
                                    //  tofromlbl.Visible = true;
                                    norecordlbl.Text = "The given date Must be between Semester day";
                                    Button4.Visible = false;
                                    batchbtn.Visible = false;
                                    norecordlbl.Visible = true;
                                    SpdInfo.Visible = false;
                                    btnprintmaster.Visible = false;
                                    lblrptname.Visible = false;
                                    txtexcelname.Visible = false;
                                    btnxl.Visible = false;
                                    Printcontrol.Visible = false;
                                    return;
                                }
                                rowval = 0;
                                splvalnew = string.Empty;
                                splval = string.Empty;
                                //---------------------------------------------------------------------------------------------------------
                                for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next Date
                                {
                                    rowval = 0;
                                    string finalsplit_date;
                                    differdays[date_loop - 1] = dt1.AddDays(1).ToString();
                                    if (dt1 >= Convert.ToDateTime(startdate) && dt1 <= Convert.ToDateTime(splitenddate))//this if condition added by Manikandan 29/08/2013
                                    {
                                        string[] date_split_time = differdays[date_loop - 1].Split(new Char[] { ' ' });
                                        string[] date_split = date_split_time[0].Split(new Char[] { '/' });
                                        finalsplit_date = date_split[1] + "/" + date_split[0] + "/" + date_split[2];
                                        //SpdInfo.Sheets[0].ColumnCount = SpdInfo.Sheets[0].ColumnCount + 2;
                                        // SpdInfo.Sheets[0].ColumnHeaderSpanModel.Add(0, SpdInfo.Sheets[0].ColumnCount - 2, 1, 2);
                                        SpdInfo.Sheets[0].ColumnCount = SpdInfo.Sheets[0].ColumnCount + 1;
                                        // SpdInfo.Sheets[0].ColumnHeaderSpanModel.Add(0, SpdInfo.Sheets[0].ColumnCount - noofalter+1, 1, noofalter + 1);
                                        int headcolumn = SpdInfo.Sheets[0].ColumnCount - 1;
                                        SpdInfo.Sheets[0].ColumnHeader.Cells[0, headcolumn].Text = Convert.ToString(finalsplit_date);
                                        SpdInfo.Sheets[0].ColumnHeader.Cells[0, headcolumn].Note = Convert.ToString(finalsplit_date);
                                        SpdInfo.Sheets[0].ColumnHeader.Cells[1, headcolumn].Text = "Schedule List";
                                        SpdInfo.Sheets[0].Columns[headcolumn].Locked = true;
                                        SpdInfo.Sheets[0].Columns[headcolumn].Font.Name = "Book Antiqua";
                                        SpdInfo.Sheets[0].Columns[headcolumn].ForeColor = Color.Blue;
                                        SpdInfo.Sheets[0].Columns[headcolumn].Font.Underline = true;
                                        SpdInfo.Sheets[0].Columns[headcolumn].Font.Size = FontUnit.Medium;
                                        SpdInfo.Sheets[0].Columns[headcolumn].Width = 100;
                                        //SpdInfo.Sheets[0].ColumnHeader.Cells[1, SpdInfo.Sheets[0].ColumnCount - 1].Text = "Alternate Schedule";
                                        //SpdInfo.Sheets[0].ColumnHeader.Cells[1, SpdInfo.Sheets[0].ColumnCount - 1].Note = Convert.ToString(finalsplit_date);
                                        //SpdInfo.Sheets[0].Columns[SpdInfo.Sheets[0].ColumnCount - 1].Width = 100;
                                        //SpdInfo.Sheets[0].Columns[SpdInfo.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        //SpdInfo.Sheets[0].Columns[SpdInfo.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        //SpdInfo.Sheets[0].Columns[SpdInfo.Sheets[0].ColumnCount - 1].Locked = true;
                                        // SpdInfo.Sheets[0].ColumnCount = SpdInfo.Sheets[0].ColumnCount + noofalter;
                                        for (int i = 0; i < noofalter; i++)
                                        {
                                            SpdInfo.Sheets[0].ColumnCount++;
                                            int col = SpdInfo.Sheets[0].ColumnCount - 1;
                                            int onoftime = i + 1;
                                            SpdInfo.Sheets[0].ColumnHeader.Cells[0, col].Note = finalsplit_date;
                                            SpdInfo.Sheets[0].ColumnHeader.Cells[1, col].Text = "Alternate Schedule " + onoftime + "";
                                            SpdInfo.Sheets[0].ColumnHeader.Cells[1, col].Note = Convert.ToString(finalsplit_date);
                                            SpdInfo.Sheets[0].ColumnHeader.Cells[1, col].Locked = true;
                                            SpdInfo.Sheets[0].Columns[col].Font.Name = "Book Antiqua";
                                            SpdInfo.Sheets[0].Columns[col].Font.Size = FontUnit.Medium;
                                            SpdInfo.Sheets[0].Columns[col].Width = 100;
                                        }
                                        SpdInfo.Sheets[0].ColumnHeaderSpanModel.Add(0, headcolumn, 1, noofalter + 1);
                                        //Start=====Added by Manikandan 26/08/2013========
                                        con.Close();
                                        con.Open();
                                        SqlDataReader dr_holday;
                                        bool morleave = false;
                                        bool eveleave = false;
                                        string holday = string.Empty;
                                        SqlCommand cmd_holday = new SqlCommand("select * from holidaystudents  where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and holiday_date ='" + date_split_time[0].ToString() + "'", con);
                                        dr_holday = cmd_holday.ExecuteReader();
                                        dr_holday.Read();
                                        if (dr_holday.HasRows == true)
                                        {
                                            holday = dr_holday["holiday_desc"].ToString();
                                            if (dr_holday["halforfull"].ToString().Trim() == "1" || dr_holday["halforfull"].ToString().Trim().ToLower() == "true")
                                            {
                                                if (dr_holday["morning"].ToString().Trim() == "1" || dr_holday["morning"].ToString().Trim().ToLower() == "true")
                                                {
                                                    morleave = true;
                                                }
                                                if (dr_holday["evening"].ToString().Trim() == "1" || dr_holday["evening"].ToString().Trim().ToLower() == "true")
                                                {
                                                    eveleave = true;
                                                }
                                            }
                                            else
                                            {
                                                morleave = true;
                                                eveleave = true;
                                            }
                                        }
                                        //====================End=========================
                                        dt1 = Convert.ToDateTime(date_split_time[0]);
                                        if (intNHrs > 0)
                                        {
                                            if (SchOrder != 0)
                                            {
                                                srt_day = dt1.ToString("ddd");
                                            }
                                            else
                                            {
                                                todate = SpdInfo.Sheets[0].ColumnHeader.Cells[0, SpdInfo.Sheets[0].ColumnCount - 2].Text;
                                                //Modifeid by Srinath 5/9/2014
                                                //srt_day = findday(todate.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                                                //string[] sps = todate.ToString().Split('/');
                                                //string curdate = sps[1] + '/' + sps[0] + '/' + sps[2];
                                                srt_day = dacess.findday(dt1.ToString(), ddlbranch.SelectedValue.ToString(), ddlduration.SelectedValue.ToString(), ddlbatch.Text.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                                            }
                                        }
                                        if (srt_day != "Sun")
                                        {
                                            String sqlsrt1 = "select top 1 ";
                                            string noofaltee = "select no_of_alter, ";
                                            for (intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                                            {
                                                sqlsrt1 = sqlsrt1 + srt_day + intNCtr.ToString() + ",";
                                                noofaltee = noofaltee + srt_day + intNCtr.ToString() + ",";
                                                SpdInfo.Sheets[0].Cells[intNCtr - 1, SpdInfo.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                                                SpdInfo.Sheets[0].Cells[intNCtr - 1, SpdInfo.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                            alterfalg = true;
                                            //------set semester schedule
                                            dr1.Close();
                                            con.Close();
                                            con.Open();
                                            //DataRow dr4;
                                            //cmd = new SqlCommand(sqlsrt1 + " degree_code , semester , batch_year from semester_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate <= '" + date_split_time[0].ToString() + "' " + strsec + " order by fromdate desc", con);
                                            //dr4 = cmd.ExecuteReader();
                                            //dr4.Read();
                                            dtSemSchedule.Clear();
                                            if (dtSchedule.Rows.Count > 0)
                                            {
                                                dtSchedule.DefaultView.RowFilter = "TTDate<='" + dt1 + "'";
                                                dtSchedule.DefaultView.Sort = "TTDate desc";
                                                dtSemSchedule = dtSchedule.DefaultView.ToTable();
                                            }
                                            if (dtSemSchedule.Rows.Count > 0)
                                            {
                                                for (intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                                                {
                                                    string dayValue = srt_day.Trim() + intNCtr;
                                                    bool leavefa = false;
                                                    if (morleave == true)
                                                    {
                                                        if (intNCtr < frhlfhr + 1)
                                                        {
                                                            leavefa = true;
                                                        }
                                                    }
                                                    if (eveleave == true)
                                                    {
                                                        if (intNCtr > frhlfhr)
                                                        {
                                                            leavefa = true;
                                                        }
                                                    }
                                                    if (leavefa == true)
                                                    {
                                                        if (holday != "" && holday != null)
                                                        {
                                                            if (dtSemSchedule.Rows[0][dayValue].ToString() != "" && dtSemSchedule.Rows[0][dayValue].ToString() != "\0")
                                                            {
                                                                SpdInfo.Sheets[0].Cells[(intNCtr - 1), headcolumn].Locked = true;
                                                                SpdInfo.Sheets[0].Cells[(intNCtr - 1), headcolumn].Text = holday + " Holiday";
                                                                SpdInfo.Sheets[0].Cells[(intNCtr - 1), headcolumn].Text = holday + " Holiday";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dtSemSchedule.Rows[0][dayValue].ToString() != "" && dtSemSchedule.Rows[0][dayValue].ToString() != "\0")
                                                        {
                                                            noflag = true;
                                                            if (hatdoc.Contains(dt1))//Added by Srinath 6/9/2014
                                                            {
                                                                splvalnew = hatdoc[dt1].ToString();
                                                                SpdInfo.Sheets[0].Cells[(intNCtr - 1), headcolumn].Locked = true;
                                                            }
                                                            else
                                                            {
                                                                SpdInfo.Sheets[0].Cells[(intNCtr - 1), headcolumn].Locked = false;
                                                                string[] subjnew = ((dtSemSchedule.Rows[0][dayValue].ToString())).Split(new Char[] { ';' });
                                                                for (int i = 0; i <= subjnew.GetUpperBound(0); i++)
                                                                {
                                                                    if (subjnew.GetUpperBound(0) >= 0)
                                                                    {
                                                                        string[] subjstr = subjnew[i].Split(new Char[] { '-' });
                                                                        if (subjstr.GetUpperBound(0) >= 2)
                                                                        {
                                                                            string strsub = GetFunction("select subject_name from subject where subject_no=" + subjstr[0] + " ");
                                                                            getcon.Close();
                                                                            splvalnew = splvalnew + ((strsub.ToString()) + "-" + subjstr[1] + "-" + subjstr[2]) + ";";
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            SpdInfo.Sheets[0].Cells[(intNCtr - 1), headcolumn].Text = Convert.ToString(splvalnew);
                                                            SpdInfo.Sheets[0].Cells[intNCtr - 1, headcolumn].Tag = Convert.ToString(dtSemSchedule.Rows[0]["TT_ClassPK"]).Trim();
                                                        }
                                                        else
                                                        {
                                                            SpdInfo.Sheets[0].Cells[intNCtr - 1, headcolumn].Tag = Convert.ToString(dtSemSchedule.Rows[0]["TT_ClassPK"]).Trim();
                                                        }
                                                    }
                                                    //-----------set alternate schedule
                                                    splvalnew = string.Empty;
                                                    //tempcon.Close();
                                                    //tempcon.Open();
                                                    //SqlDataReader dr_sch1;
                                                    //SqlCommand cmd_sch;
                                                    //cmd_sch = new SqlCommand(sqlsrt1 + " degree_code , semester , batch_year from Alternate_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate= '" + date_split_time[0].ToString() + "' " + strsec + " ", tempcon);
                                                    //dr_sch1 = cmd_sch.ExecuteReader();
                                                    //dr_sch1.Read();
                                                    //if (dr_sch1.HasRows == true)
                                                    //{
                                                    //    if (holday != "" && holday != null)//this line added by Manikandan 26/08/2013
                                                    //    {
                                                    //        if (dr_sch1[intNCtr - 1].ToString() != "" && dr_sch1[intNCtr - 1].ToString() != "\0")
                                                    //        {
                                                    //            SpdInfo.Sheets[0].Cells[(intNCtr - 1), SpdInfo.Sheets[0].ColumnCount - 1].Text = holday + " Holiday";
                                                    //            SpdInfo.Sheets[0].Cells[(intNCtr - 1), SpdInfo.Sheets[0].ColumnCount - 1].Note = holday + " Holiday";
                                                    //            splval =string.Empty;
                                                    //        }
                                                    //    }
                                                    //    else//this line added by Manikandan 26/08/2013
                                                    //    {
                                                    //        if (dr_sch1[intNCtr - 1].ToString() != "" && dr_sch1[intNCtr - 1].ToString() != "\0")
                                                    //        {
                                                    //            noflag = true;
                                                    //            setcellnote = dr_sch1[intNCtr - 1].ToString();
                                                    //            string[] sple = ((dr_sch1[intNCtr - 1]).ToString()).Split(new Char[] { ';' });
                                                    //            for (int i = 0; i <= sple.GetUpperBound(0); i++)
                                                    //            {
                                                    //                if (sple.GetUpperBound(0) >= 0)
                                                    //                {
                                                    //                    string[] sp1 = (sple[i].ToString()).Split(new Char[] { '-' });
                                                    //                    if (sp1.GetUpperBound(0) >= 2)
                                                    //                    {
                                                    //                        splval = splval + (GetFunction("select subject_name from subject where subject_no=" + sp1[0].ToString() + " ") + "-" + sp1[1].ToString() + "-" + sp1[2].ToString()) + ";";
                                                    //                        tempcon.Close();
                                                    //                    }
                                                    //                }
                                                    //            }
                                                    //            SpdInfo.Sheets[0].Cells[(intNCtr - 1), SpdInfo.Sheets[0].ColumnCount - 1].Text = Convert.ToString(splval);
                                                    //            SpdInfo.Sheets[0].Cells[(intNCtr - 1), SpdInfo.Sheets[0].ColumnCount - 1].Note = Convert.ToString(setcellnote);
                                                    //            splval =string.Empty;
                                                    //        }
                                                    //    }
                                                    //}
                                                    //=====================Start=======================================
                                                    if (alterfalg == true)
                                                    {
                                                        //Mofified by srinath 20/2/2014
                                                        alterfalg = false;
                                                        string alternatedetailks = noofaltee + "degree_code,semester,batch_year from tbl_alter_schedule_Details where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate= ' " + dt1.ToString() + " ' " + strsec + " order by no_of_alter, fromdate desc";
                                                        alternatedetailks = sqlsrt1 + " degree_code , semester , batch_year from Alternate_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate= '" + date_split_time[0].ToString() + "' " + strsec + " ";
                                                        if (noaltval > 1)
                                                        {
                                                            alternatedetailks = noofaltee + "degree_code,semester,batch_year from tbl_alter_schedule_Details where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate= ' " + dt1.ToString() + " ' " + strsec + " order by no_of_alter, fromdate desc";
                                                        }
                                                        //DataSet dsalternate = dacess.select_method(alternatedetailks, hat, "Text");
                                                        DataSet dsalternate = new DataSet();
                                                        DataTable dtAlter = new DataTable();
                                                        if (dtAlterSchedule.Rows.Count > 0)
                                                        {
                                                            dtAlterSchedule.DefaultView.RowFilter = "TT_AlterDate= '" + dt1.ToString("MM/dd/yyyy") + "'";
                                                            dtAlter = dtAlterSchedule.DefaultView.ToTable();
                                                        }
                                                        dsalternate.Tables.Add(dtAlter);
                                                        //===================Added by Srinath 6/9/2014===================
                                                        if (hatdoc.Contains(dt1))
                                                        {
                                                            for (int hour = 1; hour <= intNHrs; hour++)
                                                            {
                                                                if (dsalternate.Tables.Count > 0 && dsalternate.Tables[0].Rows.Count == 0)
                                                                {
                                                                    for (int aco = 1; aco <= noofalter; aco++)
                                                                    {
                                                                        int altercolumn = SpdInfo.Sheets[0].ColumnCount - aco;
                                                                        SpdInfo.Sheets[0].Cells[hour - 1, altercolumn].Locked = true;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    for (int alternatehour = 0; alternatehour < dsalternate.Tables[0].Rows.Count; alternatehour++)
                                                                    {
                                                                        SpdInfo.Sheets[0].Cells[hour - 1, alternatehour + 1].Locked = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //==============================End==================================
                                                        if (dsalternate.Tables.Count > 0 && dsalternate.Tables[0].Rows.Count > 0)
                                                        {
                                                            for (int hour = 1; hour <= intNHrs; hour++)
                                                            {
                                                                for (int alternatehour = 0; alternatehour < dsalternate.Tables[0].Rows.Count; alternatehour++)
                                                                {
                                                                    if (alternatehour + 1 <= noofalter)
                                                                    {
                                                                        string column = srt_day + hour;
                                                                        string value = dsalternate.Tables[0].Rows[alternatehour]["" + column + ""].ToString().Trim();
                                                                        splval = string.Empty;
                                                                        int altercolumn = SpdInfo.Sheets[0].ColumnCount - (noofalter - alternatehour);
                                                                        leavefa = false;
                                                                        if (morleave == true)
                                                                        {
                                                                            if (hour < frhlfhr + 1)
                                                                            {
                                                                                leavefa = true;
                                                                            }
                                                                        }
                                                                        if (eveleave == true)
                                                                        {
                                                                            if (hour > frhlfhr)
                                                                            {
                                                                                leavefa = true;
                                                                            }
                                                                        }
                                                                        if (leavefa == true)
                                                                        {
                                                                            if (holday != "" && holday != null)//this line added by Manikandan 26/08/2013
                                                                            {
                                                                                if (value != "" && value != "\0")
                                                                                {
                                                                                    SpdInfo.Sheets[0].Cells[hour - 1, altercolumn].Text = holday + " Holiday";
                                                                                    SpdInfo.Sheets[0].Cells[hour - 1, altercolumn].Note = holday + " Holiday";
                                                                                    splval = string.Empty;
                                                                                    batchbtn.Visible = true;//Added by Manikandan 24/08/2013
                                                                                    SpdInfo.Sheets[0].Cells[hour - 1, alternatehour + 1].Locked = true;
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (value != "" && value != "\0")
                                                                            {
                                                                                if (hatdoc.Contains(dt1))//Added by Srinath 6/9/2014
                                                                                {
                                                                                    SpdInfo.Sheets[0].Cells[hour - 1, alternatehour + 1].Locked = true;
                                                                                }
                                                                                else
                                                                                {
                                                                                    SpdInfo.Sheets[0].Cells[hour - 1, alternatehour + 1].Locked = false;
                                                                                    setcellnote = value;
                                                                                    string[] sple = (value).Split(new Char[] { ';' });
                                                                                    for (int i = 0; i <= sple.GetUpperBound(0); i++)
                                                                                    {
                                                                                        if (sple.GetUpperBound(0) >= 0)
                                                                                        {
                                                                                            string[] sp1 = (sple[i].ToString()).Split(new Char[] { '-' });
                                                                                            if (sp1.GetUpperBound(0) >= 2)
                                                                                            {
                                                                                                splval = splval + (GetFunction("select subject_name from subject where subject_no=" + sp1[0].ToString() + " ") + "-" + sp1[1].ToString() + "-" + sp1[2].ToString()) + ";";
                                                                                                // tempcon.Close();
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                SpdInfo.Sheets[0].Cells[hour - 1, altercolumn].Text = Convert.ToString(splval);
                                                                                SpdInfo.Sheets[0].Cells[hour - 1, altercolumn].Note = Convert.ToString(setcellnote);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //=====================End=======================================
                                                    rowval = rowval + 1;
                                                    // dr_sch1.Close();
                                                }
                                                // }
                                            }
                                            //dr4.Close();
                                        }
                                        else
                                        {
                                            for (intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                                            {
                                                SpdInfo.Sheets[0].Cells[intNCtr - 1, SpdInfo.Sheets[0].ColumnCount - 2].Text = "Sunday";
                                                SpdInfo.Sheets[0].Cells[intNCtr - 1, SpdInfo.Sheets[0].ColumnCount - 1].Text = "Sunday";
                                                SpdInfo.Sheets[0].Cells[intNCtr - 1, SpdInfo.Sheets[0].ColumnCount - 1].Note = "Holiday";
                                                SpdInfo.Sheets[0].Cells[intNCtr - 1, SpdInfo.Sheets[0].ColumnCount - 2].Note = "Holiday";
                                            }
                                        }
                                        con.Close();
                                        cmd.Dispose();
                                    }
                                }
                                free_staff();
                                if (noflag == false)
                                {
                                    SpdInfo.Visible = false;
                                    btnprintmaster.Visible = false;
                                    lblrptname.Visible = false;
                                    txtexcelname.Visible = false;
                                    btnxl.Visible = false;
                                    Printcontrol.Visible = false;
                                    Button4.Visible = false;
                                    batchbtn.Visible = false;
                                    string dt = DateTime.Today.ToShortDateString();
                                    string[] dsplit = dt.Split(new Char[] { '/' });
                                    txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                                    string stDate;
                                    stDate = DateTime.Today.ToShortDateString();
                                    //   ddlbatch.Text = dsplit[2].ToString();
                                    //string from_date = stDate.Date.ToShortDateString();
                                    string[] dsplit_from = stDate.Split(new Char[] { '/' });
                                    txtFromDate.Text = dsplit_from[1].ToString() + "/" + dsplit_from[0].ToString() + "/" + dsplit_from[2].ToString();
                                    norecordlbl.Visible = true;
                                    norecordlbl.ForeColor = Color.Red;
                                    if (holidyres != "")
                                    {
                                        norecordlbl.Text = holidyres;
                                    }
                                    else
                                    {
                                        norecordlbl.Text = "No record found on that day";
                                    }
                                    //return;
                                }
                                else
                                {
                                    SpdInfo.Visible = true;
                                    batchbtn.Visible = true;
                                    btnprintmaster.Visible = true;
                                    Printcontrol.Visible = false;
                                    lblrptname.Visible = true;
                                    txtexcelname.Visible = true;
                                    btnxl.Visible = true;
                                    Panel3.Visible = true;
                                    //batchbtn.Visible = true;//Hided by Manikandan 24/08/2013
                                    //Button4.Visible = true;
                                }
                            }
                            else
                            {
                                tofromlbl.Visible = true;
                                batchbtn.Visible = false;
                                //added by Manikandan
                                if (set_lock == "Settings_True")
                                {
                                    norecordlbl.Text = "Schedule settings is locked Please give date greater than or equal to today";
                                    norecordlbl.ForeColor = Color.Red;
                                    norecordlbl.Visible = true;
                                    tofromlbl.Visible = false;
                                    btnprintmaster.Visible = false;
                                    txtexcelname.Visible = false;
                                    lblrptname.Visible = false;
                                    SpdInfo.Visible = false;
                                    Button4.Visible = false;
                                    btnxl.Visible = false;
                                    treepanel.Visible = false;
                                }
                                //End
                            }
                        }
                        else
                        {
                            tolbl.Visible = true;
                            tolbl.Text = "Entar valid to date";
                        }
                    }
                    else
                    {
                        tolbl.Visible = true;
                        tolbl.Text = "Entar valid to date";
                    }
                }
                else
                {
                    frmlbl.Visible = true;
                    frmlbl.Text = "Entar valid from date";
                }
            }
            else
            {
                frmlbl.Visible = true;
                frmlbl.Text = "Entar valid from date";
            }
            //}
            //else
            //{
            //    norecordlbl.Text = "You Cannot Alter TimeTable for Past Day";
            //    norecordlbl.ForeColor = Color.Red;
            //    norecordlbl.Visible = true;
            //    tofromlbl.Visible = false;
            //}
        }
        catch
        {
        }
    }

    public string GetFunction(string Att_strqueryst)
    {
        try
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
        catch
        {
            return "";
        }
    }

    private string GetSyllabusYear(string degree_code, string batch_year, string sem)
    {
        try
        {
            string syl_year = string.Empty;
            con2a.Close();
            con2a.Open();
            SqlCommand cmd2a;
            SqlDataReader get_syl_year;
            cmd2a = new SqlCommand("select syllabus_year from syllabus_master where degree_code=" + Session["degree_code"] + " and semester =" + Session["semester"] + " and batch_year=" + Session["batch_year"] + " ", con2a);
            get_syl_year = cmd2a.ExecuteReader();
            get_syl_year.Read();
            if (get_syl_year.HasRows == true)
            {
                if (get_syl_year[0].ToString() == "\0")
                {
                    syl_year = "-1";
                }
                else
                {
                    syl_year = get_syl_year[0].ToString();
                }
            }
            else
            {
                syl_year = "-1";
            }
            return syl_year;
            con2a.Close();
        }
        catch
        {
            return "";
        }
    }

    //-----------------------------------node selection in treeview
    protected void subjtree_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            lblmulstaff.Visible = false;
            txtmulstaff.Visible = false;
            pmulstaff.Visible = false;
            btnmulstaff.Visible = false;
            chkmullsstaff.Items.Clear();
            txtmulstaff.Text = "---Select---";
            chkmulstaff.Checked = false;
            int staf_cnt = 0;
            string staff_code = "", staff_name_code = string.Empty;
            subjtree.Visible = true;
            FpSpread1.Visible = true;
            chkappend.Visible = true;
            btnOk.Visible = true;
            treepanel.Visible = true;
            FpSpread1.ActiveSheetView.AutoPostBack = false;
            string strsec;
            int rowval = 0;
            if (Session["section"].ToString() != "0" && Session["section"].ToString() != "\0")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + Session["section"].ToString() + "'";
            }
            int parent_count = subjtree.Nodes.Count;//----------count parent node value
            for (int i = 0; i < parent_count; i++)
            {
                for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                {
                    if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                    {
                        string temp_sec = string.Empty;
                        if (Session["section"].ToString() == "")
                        {
                            temp_sec = string.Empty;
                        }
                        else
                        {
                            temp_sec = " and sections='" + Session["section"].ToString() + "'";
                        }
                        if (chkappend.Checked == true)
                        {
                            FpSpread1.Sheets[0].RowCount = Convert.ToInt32(FpSpread1.Sheets[0].RowCount.ToString()) + 1;
                            //-------set selected subject name into the sprad
                            rowval = Convert.ToInt32(FpSpread1.Sheets[0].RowCount.ToString()) - 1;
                            FpSpread1.Sheets[0].Rows[rowval].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Rows[rowval].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].RowHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].RowHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].RowHeader.Cells[0, 0].Font.Bold = true;
                            FpSpread1.Sheets[0].SetText(rowval, 0, subjtree.Nodes[i].ChildNodes[node_count].Text);
                            FpSpread1.Sheets[0].Cells[rowval, 0].Tag = subjtree.Nodes[i].ChildNodes[node_count].Value;
                            string chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;
                            //--------------bind staff name into the spread
                            string strstaffquery = "select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year=" + Session["batch_year"] + "  " + temp_sec + ")";
                            DataSet staf_set = dacess.select_method_wo_parameter(strstaffquery, "Text");

                            //DataSet staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + Session["batch_year"].ToString() + "' " + temp_sec + "  and TT_date>='" + (txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[2]) + "')");

                            //if (true)
                            //{
                            //    staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' " + temp_sec + "  and ct.TT_batchyear='" + Session["batch_year"].ToString() + "' ) ");
                            //}
                            //FarPoint.Web.Spread.ComboBoxCellType staf_combo = new FarPoint.Web.Spread.ComboBoxCellType();
                            //staf_combo.DataSource = staf_set;
                            //staf_combo.DataTextField = "staff_name";
                            //staf_combo.DataValueField = "staff_code";
                            //   staf_combo.Items[staf_set.Tables[0].Rows.Count].Insert(staf_set.Tables[0].Rows.Count, "All");
                            string[] staff_list = new string[staf_set.Tables[0].Rows.Count + 1];
                            for (staf_cnt = 0; staf_cnt < staf_set.Tables[0].Rows.Count; staf_cnt++)
                            {
                                staff_list[staf_cnt] = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                chkmullsstaff.Items.Add(staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString());
                                //    chklistmultisubj.Items.Add(staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString());
                                if (staff_code == "")
                                {
                                    staff_code = staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    staff_name_code = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                }
                                else
                                {
                                    staff_code = staff_code + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    staff_name_code = staff_name_code + ";" + staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                }
                            }
                            if (staff_list.GetUpperBound(0) > 0)
                            {
                                staff_list[staf_cnt] = "All";
                            }
                            if (staf_set.Tables[0].Rows.Count > 1)
                            {
                                lblmulstaff.Visible = true;
                                txtmulstaff.Visible = true;
                                btnmulstaff.Visible = true;
                                pmulstaff.Visible = true;
                            }
                            FarPoint.Web.Spread.ComboBoxCellType staf_combo = new FarPoint.Web.Spread.ComboBoxCellType(staff_list);
                            FpSpread1.Sheets[0].Cells[rowval, 1].CellType = staf_combo;
                            FpSpread1.Sheets[0].Cells[rowval, 1].Tag = staff_code;
                            FpSpread1.Sheets[0].Cells[rowval, 1].Value = staff_name_code;
                            con3a.Close();
                            FpSpread1.SaveChanges();
                            treepanel.Visible = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].RowCount = 0;
                            FpSpread1.Sheets[0].RowCount = 1;
                            rowval = 0;
                            //-------set selected subject name into the sprad
                            FpSpread1.Sheets[0].SetText(rowval, 0, subjtree.Nodes[i].ChildNodes[node_count].Text);
                            FpSpread1.Sheets[0].Cells[rowval, 0].Tag = subjtree.Nodes[i].ChildNodes[node_count].Value;
                            string chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;
                            FpSpread1.Sheets[0].Rows[rowval].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Rows[rowval].Font.Size = FontUnit.Medium;
                            //--------------bind staff name into the spread
                            con4a.Open();
                            cmd4a = new SqlCommand("select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year=" + Session["batch_year"].ToString() + " " + temp_sec + ")", con4a);
                            SqlDataAdapter staf_name = new SqlDataAdapter(cmd4a);
                            DataSet staf_set = new DataSet();
                            staf_name.Fill(staf_set);

                            //DataSet staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + Session["batch_year"].ToString() + "' " + temp_sec + "  and TT_date>='" + (txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[2]) + "')");

                            //if (true)
                            //{
                            //    staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' " + temp_sec + "  and ct.TT_batchyear='" + Session["batch_year"].ToString() + "' ) ");
                            //}

                            //staf_combo.DataSource = staf_set;
                            //staf_combo.DataTextField = "staff_name";
                            //staf_combo.DataValueField = "staff_code";
                            string[] staff_list = new string[staf_set.Tables[0].Rows.Count + 1];
                            for (staf_cnt = 0; staf_cnt < staf_set.Tables[0].Rows.Count; staf_cnt++)
                            {
                                //  chklistmultisubj.Items.Add(staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString());
                                staff_list[staf_cnt] = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                chkmullsstaff.Items.Add(staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString());
                                if (staff_code == "")
                                {
                                    staff_code = staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    staff_name_code = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                }
                                else
                                {
                                    staff_code = staff_code + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    staff_name_code = staff_name_code + ";" + staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                }
                            }
                            if (staff_list.GetUpperBound(0) > 0)
                            {
                                staff_list[staf_cnt] = "All";
                            }
                            if (staf_set.Tables[0].Rows.Count > 1)
                            {
                                lblmulstaff.Visible = true;
                                txtmulstaff.Visible = true;
                                btnmulstaff.Visible = true;
                                pmulstaff.Visible = true;
                            }
                            FarPoint.Web.Spread.ComboBoxCellType staf_combo = new FarPoint.Web.Spread.ComboBoxCellType(staff_list);
                            staf_combo.AutoPostBack = true;
                            FpSpread1.Sheets[0].Cells[rowval, 1].CellType = staf_combo;
                            FpSpread1.Sheets[0].Cells[rowval, 1].Tag = staff_code;
                            FpSpread1.Sheets[0].Cells[rowval, 1].Value = staff_name_code;
                            treepanel.Visible = true;
                        }
                        //if (chk_multisubj.Checked == true && chklistmultisubj.Items.Count > 2)
                        //{
                        //    txtmultisubj.Visible = true;
                        //    pnlmultisubj.Visible = true;
                        //}
                        //else
                        //{
                        //    txtmultisubj.Visible = false;
                        //    pnlmultisubj.Visible = false;
                        //}
                        btnOk.Visible = true;
                        chkappend.Visible = true;
                        FpSpread1.Visible = true;
                        con4a.Close();
                        FpSpread1.SaveChanges();
                    }
                }
            }
        }
        catch
        {
        }
    }

    //-----------------------------------shedule remove in spread2
    protected void FpSpread1_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            subjtree.Visible = true;
            FpSpread1.Visible = true;
            chkappend.Visible = true;
            btnOk.Visible = true;
            treepanel.Visible = true;
            int ar = 0;
            ar = FpSpread1.ActiveSheetView.ActiveRow;
            FpSpread1.Sheets[0].RemoveRows(ar, 1);
        }
        catch
        {
        }
    }

    //----------------------------------shedule checker ok
    public void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            int x = SpdInfo.ActiveSheetView.ActiveRow;
            int y = SpdInfo.ActiveSheetView.ActiveColumn;
            if (y + 1 < SpdInfo.Sheets[0].ColumnCount)
            {
                string subj_number = string.Empty;
                string splval = "", splval_temp = string.Empty;
                string subno_staff = string.Empty;
                string staffname = "", staff_name_code = "", staffcode = string.Empty;
                if (chk_multisubj.Checked == false)
                {
                    for (int rowcnt = 0; rowcnt <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; rowcnt++)
                    {
                        FpSpread1.SaveChanges();
                        staff_name_code = Convert.ToString(FpSpread1.Sheets[0].GetText(rowcnt, 1));
                        string getstaff = Convert.ToString(FpSpread1.Sheets[0].Cells[rowcnt, 1].Tag);
                        if (staff_name_code == "" || staff_name_code == "System.Object")//-----------check wether the staff name selected or not
                        {
                            subjtree.Visible = true;
                            FpSpread1.Visible = true;
                            chkappend.Visible = true;
                            btnOk.Visible = true;
                            errmsg.Visible = true;
                            treepanel.Visible = true;
                            errmsg.ForeColor = Color.Red;
                            errmsg.Text = "Select Staff name";
                            return;
                        }
                        else
                        {
                            btnsave.Enabled = true;
                            subjtree.Visible = false;
                            FpSpread1.Visible = false;
                            chkappend.Visible = false;
                            btnOk.Visible = false;
                            treepanel.Visible = false;
                            btnsave.Visible = true;
                            errmsg.Visible = false;
                            lblmulstaff.Visible = false;
                            txtmulstaff.Visible = false;
                            pmulstaff.Visible = false;
                            btnmulstaff.Visible = false;
                        }
                    }
                    if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) == 0)//------------message for select the subject from the tree
                    {
                        subjtree.Visible = true;
                        FpSpread1.Visible = true;
                        chkappend.Visible = true;
                        btnOk.Visible = true;
                        errmsg.Visible = true;
                        errmsg.Text = "Select Subject name for alternate schedule from tree view";
                        errmsg.ForeColor = Color.Red;
                        return;
                    }
                    //-----------------set the selected subject name and staff name into the spread
                    for (int row_cnt = 0; row_cnt <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; row_cnt++)
                    {
                        staffname = string.Empty;
                        staffcode = string.Empty;
                        staff_name_code = Convert.ToString(FpSpread1.Sheets[0].GetText(row_cnt, 1));
                        string[] staffCodeList = new string[0];
                        if (staff_name_code != "" && staff_name_code != null)
                        {
                            if (staff_name_code != "All")
                            {
                                string[] staff_name_code_spt = staff_name_code.Split('-');
                                for (int st = 0; st <= staff_name_code_spt.GetUpperBound(0); st = st + 2)
                                {
                                    if (staffcode == "")
                                    {
                                        staffname = staff_name_code_spt[st].ToString();
                                        staffcode = staff_name_code_spt[st + 1].ToString();
                                    }
                                    else
                                    {
                                        staffname = staffname + "-" + staff_name_code_spt[st].ToString();
                                        staffcode = staffcode + "-" + staff_name_code_spt[st + 1].ToString();
                                    }
                                    Array.Resize(ref staffCodeList, staffCodeList.Length + 1);
                                    staffCodeList[staffCodeList.Length - 1] = staff_name_code_spt[st + 1].ToString();
                                    //staffname = staff_name_code_spt[0].ToString();
                                    //staffcode = staff_name_code_spt[1].ToString();
                                }
                            }
                            else
                            {
                                staffcode = FpSpread1.Sheets[0].Cells[row_cnt, 1].Tag.ToString();
                                string[] subList = staffcode.Split('-');
                                foreach (string sub in subList)
                                {
                                    Array.Resize(ref staffCodeList, staffCodeList.Length + 1);
                                    staffCodeList[staffCodeList.Length - 1] = sub;
                                }
                            }
                        }
                        subj_number = FpSpread1.Sheets[0].Cells[row_cnt, 0].Tag.ToString();
                        //con6a.Close();
                        //con6a.Open();
                        //SqlDataReader dr5;
                        //cmd6a = new SqlCommand("select distinct staff_code from staff_selector where subject_no=" + subj_number + "", con6a);
                        //dr5 = cmd6a.ExecuteReader();
                        //dr5.Read();
                        //Start=======Added by srinath 28/01/2014=======
                        string parenttext = subjtree.SelectedNode.Parent.Text;
                        string theory_lab = dacess.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subj_number + "'");
                        if (theory_lab.Trim() == "1" || theory_lab.Trim().ToLower() == "true")
                        {
                            theory_lab = "L";
                        }
                        else
                        {
                            theory_lab = "S";
                        }
                        //============================End==================
                        string subjectName = GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ").Trim();
                        if (staff_name_code != "All")
                        {
                            if (splval == "")
                            {
                                //splval = (GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + theory_lab);

                                subno_staff = subj_number + "-" + staffcode + "-" + theory_lab;
                            }
                            else
                            {
                                //splval = splval + ";" + (GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + theory_lab);
                                subno_staff = subno_staff + ";" + subj_number + "-" + staffcode + "-" + theory_lab;
                            }

                        }
                        else
                        {
                            if (splval == "")
                            {
                                //splval = (GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + theory_lab);

                                subno_staff = subj_number + "-" + staffcode + "-" + theory_lab;
                            }
                            else
                            {
                                //splval = splval + ";" + (GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + theory_lab);
                                subno_staff = subno_staff + ";" + subj_number + "-" + staffcode + "-" + theory_lab;
                            }
                        }
                        foreach (string staff in staffCodeList)
                        {
                            if (splval == "")
                            {
                                splval = subjectName + "-" + staff + "-" + theory_lab;
                            }
                            else
                            {
                                splval += ";" + subjectName + "-" + staff + "-" + theory_lab;
                            }
                        }
                    }
                }
                else
                {
                    for (int row_cnt = 0; row_cnt <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; row_cnt++)
                    {
                        string[] staffCodeList = new string[0];
                        for (int chk_cnt = 0; chk_cnt < chklistmultisubj.Items.Count; chk_cnt++)
                        {
                            if (chklistmultisubj.Items[chk_cnt].Selected == true)
                            {
                                staff_name_code = chklistmultisubj.Items[chk_cnt].Text;
                                string[] staff_name_code_spt = staff_name_code.Split('-');
                                staffname = staff_name_code_spt[0].ToString();
                                staffcode = staff_name_code_spt[1].ToString();
                                subj_number = FpSpread1.Sheets[0].Cells[row_cnt, 0].Tag.ToString();
                                if (splval_temp == "")
                                {
                                    //  splval = (GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "S");
                                    splval_temp = staffcode;// subj_number + "-" + staffcode + "-S";
                                }
                                else
                                {
                                    // splval_temp = splval + ";" + (GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "S");
                                    splval_temp = splval_temp + "-" + staffcode;// subj_number + "-" + staffcode + "-S";
                                }
                                Array.Resize(ref staffCodeList, staffCodeList.Length + 1);
                                staffCodeList[staffCodeList.Length - 1] = staffcode.ToString();
                            }
                        }
                        //Start=======Added by Manikandan 27/08/2013=======
                        string parenttext = subjtree.SelectedNode.Parent.Text;
                        //string theory_lab =string.Empty;
                        //if (parenttext == "Theory")
                        //{
                        //    theory_lab = "S";
                        //}
                        //else
                        //{
                        //    theory_lab = "L";
                        //}
                        //========= Modified by srinath 28/01/2014
                        string theory_lab = dacess.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subj_number + "'");
                        if (theory_lab.Trim() == "1" || theory_lab.Trim().ToLower() == "true")
                        {
                            theory_lab = "L";
                        }
                        else
                        {
                            theory_lab = "S";
                        }
                        //============End===================================
                        string subjectName = GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ").Trim();
                        if (splval == "")
                        {
                            //splval = (GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + theory_lab);
                            subno_staff = subj_number + "-" + splval_temp + "-" + theory_lab;
                        }
                        else
                        {
                            //splval = splval + ";" + (GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + theory_lab);
                            subno_staff = subno_staff + ";" + subj_number + "-" + splval_temp + "-" + theory_lab;
                        }
                        foreach (string staff in staffCodeList)
                        {
                            if (splval == "")
                            {
                                splval = subjectName + "-" + staff + "-" + theory_lab;
                            }
                            else
                            {
                                splval += ";" + subjectName + "-" + staff + "-" + theory_lab;
                            }
                        }
                    }
                }
                //-------------set the cell as the active cell
                SpdInfo.Sheets[0].Cells[x, y + 1].Text = splval.ToString();
                SpdInfo.Sheets[0].Cells[x, y + 1].Note = subno_staff.ToString();
                FarPoint.Web.Spread.SheetView sv = SpdInfo.ActiveSheetView;
                sv.ActiveColumn = y + 1;
                sv.ActiveRow = x;
            }
        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
            norecordlbl.ForeColor = Color.Red;
            norecordlbl.Visible = true;
        }
    }

    protected void SpdInfo_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (Cellclickeve == true)
            {
                lblmulstaff.Visible = false;
                txtmulstaff.Visible = false;
                pmulstaff.Visible = false;
                btnmulstaff.Visible = false;
                if (SpdInfo.Sheets[0].Cells[(SpdInfo.Sheets[0].ActiveRow), (SpdInfo.Sheets[0].ActiveColumn)].Text != "")
                {
                    btn_remove.Visible = true;
                }
                else
                {
                    btn_remove.Visible = false;
                }
                string Active_clmn = string.Empty;
                string Active_row = string.Empty;
                Active_row = SpdInfo.ActiveSheetView.ActiveRow.ToString();
                int ar = Convert.ToInt32(Active_row);
                Active_clmn = SpdInfo.ActiveSheetView.ActiveColumn.ToString();
                int ac = Convert.ToInt32(Active_clmn);
                string celltext = string.Empty;
                celltext = SpdInfo.Sheets[0].Cells[ar, ac].Text;
                //===========================srinath 6/9/2014===============================
                bool loc = SpdInfo.Sheets[0].Cells[ar, ac].Locked;
                if (loc == true)
                {
                    sem_schedule.Visible = true;
                    semspread.Visible = true;
                    semmsglbl.Visible = false;
                    subjtree.Visible = false;
                    chkappend.Visible = false;
                    btnOk.Visible = false;
                    btn_remove.Visible = false;
                    treepanel.Visible = false;
                    sem_schedule.Enabled = false;
                    lblcellerrmsg.Visible = true;
                    lblcellerrmsg.Text = "You Can't Edit This Details";
                    return;
                }
                //===========================End===============================
                string ac1;
                ac1 = Convert.ToString(ac);
                if (ar != -1)
                {
                    string actcell = string.Empty;
                    actcell = SpdInfo.Sheets[0].ColumnHeader.Cells[1, ac].Text;
                    //---------------check the column condition for load the tree 
                    if (actcell == "Schedule List" && celltext != "Sunday")
                    {
                        treepanel.Visible = true;
                        subjtree.Visible = true;
                        //chkappend.Visible = true;
                        FpSpread1.Sheets[0].RowCount = 0;
                    }
                    treeload();
                }
                //---------------------------check condition for day schedule change
                //Modified by Srinath 17/10/2013
                string colhead = SpdInfo.Sheets[0].ColumnHeader.Cells[1, ac].Text;
                string[] spilthead = colhead.Split(' ');
                int noofalter = 0;
                if (spilthead.GetUpperBound(0) >= 2)
                {
                    noofalter = Convert.ToInt32(spilthead[2]);
                }
                int totalnoofalter = Convert.ToInt32(txtnoofalter.Text);
                //if (SpdInfo.Sheets[0].ColumnHeader.Cells[1, ac].Text == "Alternate Schedule" && celltext != "Sunday")
                if (spilthead[0].ToString() == "Alternate" && celltext != "Sunday")
                {
                    sem_schedule.Visible = true;
                    semspread.Visible = true;
                    semmsglbl.Visible = false;
                    subjtree.Visible = false;
                    chkappend.Visible = false;
                    btnOk.Visible = false;
                    sem_schedule_Click();
                    sem_schedule.Enabled = true;
                }
                if (noofalter < totalnoofalter && celltext.Trim() != "")
                {
                    treepanel.Visible = true;
                    subjtree.Visible = true;
                    //chkappend.Visible = true;
                    FpSpread1.Sheets[0].RowCount = 0;
                }
            }
            if (semclick == true)
            {
                string ar = string.Empty;
                int act_row = 0;
                string sqlstr = string.Empty;
                int noofhrs = 0;
                int set_val = 0;
                string Active_clmn = string.Empty;
                //------------------set the day schedule alter into the main spread from popup semester schedule spread
                Active_clmn = SpdInfo.ActiveSheetView.ActiveColumn.ToString();
                int ac = Convert.ToInt32(Active_clmn);
                ar = semspread.ActiveSheetView.ActiveRow.ToString();
                act_row = Convert.ToInt32(ar.ToString());
                con.Close();
                con.Open();
                SqlDataReader dr;
                cmd = new SqlCommand("Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + "", con);
                dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows == true)
                {
                    if ((dr["No_of_hrs_per_day"].ToString()) != "")
                    {
                        noofhrs = Convert.ToInt32(dr["No_of_hrs_per_day"]);
                    }
                }
                sqlstr = "select No_of_hrs_per_day from PeriodAttndSchedule where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester=" + ddlduration.SelectedValue.ToString() + " ";
                noofhrs = Convert.ToInt32(GetFunction(sqlstr));
                for (set_val = 0; set_val < noofhrs; set_val++)//----set value
                {
                    string getvalu = semspread.Sheets[0].Cells[act_row, set_val].Note.ToString();
                    string[] spitsub = getvalu.Split(';');
                    for (int i = 0; i <= spitsub.GetUpperBound(0); i++)
                    {
                        string[] spitsublab = spitsub[i].Split('-');
                        if (spitsublab.GetUpperBound(0) >= 0)
                        {
                            string subcode = spitsublab[0].ToString();
                            if (subcode.Trim() != "" && subcode != null)
                            {
                                string chklab = dacess.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subcode.ToString() + "'");
                                if (chklab.Trim() == "1" || chklab.ToLower().Trim() == "true")
                                {
                                    string getday = semspread.Sheets[0].RowHeader.Cells[act_row, 0].Tag.ToString();
                                    string setval = getday + ',' + set_val;
                                    SpdInfo.Sheets[0].Cells[set_val, ac].Tag = setval;
                                }
                            }
                        }
                    }
                    SpdInfo.Sheets[0].Cells[set_val, ac].Text = semspread.Sheets[0].Cells[act_row, set_val].Tag.ToString();
                    SpdInfo.Sheets[0].Cells[set_val, ac].Note = semspread.Sheets[0].Cells[act_row, set_val].Note.ToString();
                    SpdInfo.SaveChanges();
                    btnsave.Visible = true;
                }
            }
        }
        catch
        {
        }
    }

    private void treeload()
    {
        try
        {
            subjtree.Nodes.Clear();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 3;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Subject Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
            FarPoint.Web.Spread.ButtonCellType staf_butt1 = new FarPoint.Web.Spread.ButtonCellType("OneCommand", FarPoint.Web.Spread.ButtonType.PushButton, "Remove");
            FpSpread1.Sheets[0].Columns[2].CellType = staf_butt1;
            staf_butt1.Text = "Remove";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Remove";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            //---------alternate subj shouldnt be same as subject
            int actrow = 0;
            int actcol = 0;
            string subjname_staffcode = string.Empty;
            string subjname = string.Empty;
            actrow = SpdInfo.ActiveSheetView.ActiveRow;
            actcol = SpdInfo.ActiveSheetView.ActiveColumn;
            subjname_staffcode = SpdInfo.Sheets[0].Cells[actrow, actcol].Text;
            string[] splitsubj = subjname_staffcode.Split(new Char[] { '-' });
            subjname = splitsubj[0].ToString();
            //-------------------
            string Syllabus_year = string.Empty;
            Syllabus_year = GetSyllabusYear(Session["degree_code"].ToString(), Session["batch_year"].ToString(), Session["semester"].ToString());
            if (Syllabus_year != "-1")
            {
                //--------------get subject type and subjects
                cona.Close();
                cona.Open();
                SqlDataReader subTypeRs;
                cmda = new SqlCommand("select distinct subject.subtype_no,subject_type from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + Session["degree_code"] + " and semester=" + Session["semester"] + " and syllabus_year = " + Syllabus_year + " and batch_year = " + Session["batch_year"] + ") order by subject.subtype_no", cona);
                subTypeRs = cmda.ExecuteReader();
                TreeNode node;
                int rec_count = 0;
                while (subTypeRs.Read())
                {
                    if ((subTypeRs["subject_type"].ToString()) != "0")
                    {
                        SqlDataReader subTypeRs1;
                        con1a.Close();
                        con1a.Open();
                        cmd1a = new SqlCommand("select subject.subtype_no,subject_type,subject_no,subject_name,subject_code from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + Session["degree_code"] + " and semester=" + Session["semester"] + " and syllabus_year = " + Syllabus_year + " and batch_year = " + Session["batch_year"] + ") and subject.subtype_no=" + subTypeRs["subtype_no"] + " order by subject.subtype_no,subject.subject_no", con1a);
                        subTypeRs1 = cmd1a.ExecuteReader();
                        node = new TreeNode(subTypeRs["subject_type"].ToString(), rec_count.ToString());
                        while (subTypeRs1.Read())//-------------set to tree
                        {
                            //if (subTypeRs1["subject_name"].ToString() != "0" && subTypeRs1["subject_name"].ToString() != subjname)//Hided by Manikandan for load all subject in treeview on 07/08/2013
                            //{
                            node.ChildNodes.Add(new TreeNode(subTypeRs1["subject_name"].ToString(), subTypeRs1["subject_no"].ToString()));
                            rec_count = rec_count + 1;
                            //}
                        }
                        subjtree.Nodes.Add(node);
                    }
                }
                cona.Close();
                con1a.Close();
            }
        }
        catch
        {
        }
    }

    protected void getAlert()
    {
        try
        {
            string Dateval;
            int jj = 0;
            int j = 0;
            string strsec = string.Empty;
            int intNHrs = 0;
            int SchOrder = 0;
            string strDay = string.Empty;
            string Strsql = string.Empty;
            int nodays = 0;
            string strinsert = string.Empty;
            string VarSch = string.Empty;
            string dateval;
            string Strsqlval = string.Empty;
            string startdate = string.Empty;
            string todate = string.Empty;
            string bacth = ddlbatch.SelectedValue.ToString();
            string degree = ddlbranch.SelectedValue.ToString();
            string sem = ddlduration.SelectedItem.ToString();
            DataSet dssetbatch = new DataSet();
            bool aeperflag = false;
            SqlConnection con7 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
            string smssend = dacess.GetFunction("select value from Master_Settings where settings='Alternatesms'");
            if (SpdInfo.Sheets[0].RowCount > 0)
            {
                if (ddlsec.Items.Count == 0)
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                }
                string qrySection = string.Empty;
                string qrySection1 = string.Empty;
                if (ddlsec.Items.Count > 0)
                {
                    string sectionName = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                    if (!string.IsNullOrEmpty(sectionName) && sectionName.Trim().ToLower() != "all" && sectionName.Trim().ToLower() != "-1")
                    {
                        qrySection = " and ct.TT_sec='" + Convert.ToString(sectionName).Trim() + "'";
                        qrySection1 = " and nct.TT_sec='" + Convert.ToString(sectionName).Trim() + "'";
                    }
                }
                int noofalter = Convert.ToInt32(txtnoofalter.Text);
                Hashtable hataltersc = new Hashtable();
                // for (jj = 1; jj <= SpdInfo.Sheets[0].ColumnCount; jj = jj + 2)//----------incement column value
                for (jj = 1; jj <= SpdInfo.Sheets[0].ColumnCount; jj = jj + noofalter + 1)//----------incement column value
                {
                    Dateval = SpdInfo.Sheets[0].ColumnHeader.Cells[0, jj - 1].Note;
                    string[] split = Dateval.Split(new Char[] { '/' });
                    dateval = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                    DateTime head_date = Convert.ToDateTime(dateval.ToString());
                    con8.Open();
                    SqlDataReader dr8;
                    cmd8 = new SqlCommand("Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + "", con8);
                    dr8 = cmd8.ExecuteReader();
                    dr8.Read();
                    if (dr8.HasRows == true)
                    {
                        if ((dr8["No_of_hrs_per_day"].ToString()) != "")
                        {
                            intNHrs = Convert.ToInt32(dr8["No_of_hrs_per_day"]);
                            SchOrder = Convert.ToInt32(dr8["schorder"]);
                            nodays = Convert.ToInt32(dr8["nodays"]);
                        }
                    }
                    con.Close();
                    con.Open();
                    SqlDataReader dr1;
                    cmd = new SqlCommand("select * from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " ", con);
                    dr1 = cmd.ExecuteReader();
                    dr1.Read();
                    if (dr1.HasRows == true)
                    {
                        if ((dr1["start_date"].ToString()) != "" && (dr1["start_date"].ToString()) != "\0")
                        {
                            string[] tmpdate = dr1["start_date"].ToString().Split(new char[] { ' ' });
                            startdate = tmpdate[0].ToString();
                            if (Convert.ToString(dr1["starting_dayorder"]) != "")
                            {
                                start_dayorder = dr1["starting_dayorder"].ToString();
                            }
                            else
                            {
                                start_dayorder = "1";
                            }
                        }
                        else
                        {
                            norecordlbl.Visible = true;
                            norecordlbl.Text = "Update semester Information";
                            norecordlbl.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        norecordlbl.Visible = true;
                        norecordlbl.Text = "Update semester Information";
                        norecordlbl.ForeColor = Color.Red;
                    }
                    if (intNHrs > 0)
                    {
                        if (SchOrder != 0)
                        {
                            strDay = head_date.ToString("ddd");
                        }
                        else
                        {
                            todate = SpdInfo.Sheets[0].ColumnHeader.Cells[1, jj].Note;
                            // strDay = findday(todate.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                            //Modifeid by Srinath 5/9/2014
                            string[] sps = todate.ToString().Split('/');
                            string curdate = sps[1] + '/' + sps[0] + '/' + sps[2];
                            strDay = dacess.findday(curdate.ToString(), ddlbranch.SelectedValue.ToString(), ddlduration.SelectedValue.ToString(), ddlbatch.Text.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                        }
                        string ttname = dacess.GetFunction("select  top 1 ttname from Semester_Schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate <='" + dateval + "'" + strsec + " order by FromDate desc");

                        ttname = dirAcc.selectScalarString("select top 1 ct.TT_name from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection + " and ct.TT_date=(select MAX(nct.TT_date) from TT_ClassTimetable nct where nct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and nct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and nct.TT_batchyear='" + ddlbatch.SelectedValue + "' and nct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection1 + " ) ");//and TT_date<='" + dateval + "'
                        if (ttname.Trim() != "" && ttname != null && ttname.Trim() != "0")
                        {
                            ttname = " and Timetablename='" + ttname + "'";
                        }
                        Strsqlval = string.Empty;
                        Strsqlval = "select top 1 ";
                        string getday = string.Empty;
                        for (int intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                        {
                            Strsqlval = Strsqlval + strDay + intNCtr.ToString() + ",";
                            if (getday == "")
                            {
                                getday = strDay + intNCtr.ToString();
                            }
                            else
                            {
                                getday = getday + "," + strDay + intNCtr.ToString();
                            }
                        }
                        //---------------------check the record in alternate schedule for update
                        con8.Close();
                        con7.Close();
                        con7.Open();
                        SqlDataReader savedr;
                        //Strsql = Strsqlval + " degree_code , semester , batch_year,lastrec,sections,TTName,Fromdate from Alternate_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate ='" + dateval + "'" + strsec + " order by FromDate";
                        string qryAlter = "select TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,ctd.TT_AlterDate,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room,ctd.TT_AlterDate from TT_ClassTimetable ct, TT_AlterTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_CLassFK and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection + "  and ctd.TT_AlterDate ='" + dateval + "' ";
                        SqlCommand cmd7 = new SqlCommand(qryAlter, con7);
                        savedr = cmd7.ExecuteReader();
                        savedr.Read();
                        string code_value = string.Empty;
                        string cellnote = string.Empty;
                        string sectionval = string.Empty;
                        bool isaltflaf = false;
                        if (ddlsec.Text.ToString().Trim() != "" && ddlsec.Text.ToString().ToLower().Trim() != "all")
                        {
                            sectionval = ddlsec.Text.ToString();
                        }
                        for (int alt = 1; alt <= noofalter; alt++)
                        {
                            string selnoofalter = Strsqlval + " degree_code , semester , batch_year,lastrec,sections,TTName,Fromdate,No_of_Alter from tbl_alter_schedule_Details where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate ='" + dateval + "'" + strsec + " and No_of_Alter='" + alt + "' order by FromDate";
                            DataSet dsnoofalter = dacess.select_method(selnoofalter, hat, "Text");
                            if (dsnoofalter.Tables[0].Rows.Count > 0)
                            {
                                string deltequery = "delete from tbl_alter_schedule_Details where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate ='" + dateval + "'" + strsec + " and No_of_Alter='" + alt + "'";
                                int del = dacess.update_method_wo_parameter(deltequery, "Text");
                            }
                            string getStaffCode = string.Empty;
                            int colCnt = 0;
                            string altersubdeta = string.Empty;
                            string stfName = string.Empty;
                            int altercolu = alt + jj - 1;
                            for (j = 0; j < intNHrs; j++)//---------------loop for row value
                            {
                                string substaffname = SpdInfo.Sheets[0].Cells[j, altercolu].Text;
                                string substaffcode = SpdInfo.Sheets[0].Cells[j, altercolu].Note;
                                string dayval = strDay + Convert.ToInt32(j + 1).ToString();
                                string getlabdetails = string.Empty;
                                if (substaffname != "" && substaffcode != "" && substaffname != "Sunday")
                                {
                                    if (SpdInfo.Sheets[0].Cells[j, altercolu].Locked == false)
                                    {
                                        if (!hataltersc.Contains(dayval))
                                        {
                                            if (SpdInfo.Sheets[0].Cells[j, altercolu].Tag != null)
                                            {
                                                getlabdetails = "/" + SpdInfo.Sheets[0].Cells[j, altercolu].Tag.ToString();
                                            }
                                            hataltersc.Add(dayval, substaffcode + getlabdetails);
                                        }
                                        else
                                        {
                                            if (SpdInfo.Sheets[0].Cells[j, altercolu].Tag != null)
                                            {
                                                getlabdetails = "/" + SpdInfo.Sheets[0].Cells[j, altercolu].Tag.ToString();
                                            }
                                            hataltersc[dayval] = substaffcode + getlabdetails;
                                        }
                                        if (altersubdeta == "")
                                        {
                                            altersubdeta = "'" + substaffcode + "'";
                                        }
                                        else
                                        {
                                            altersubdeta = altersubdeta + ",'" + substaffcode + "'";
                                        }
                                        colCnt = j + 1;
                                        getStaffCode = substaffcode;
                                        stfName = substaffname;
                                    }
                                    else
                                    {
                                        if (altersubdeta == "")
                                        {
                                            altersubdeta = "''";
                                        }
                                        else
                                        {
                                            altersubdeta = altersubdeta + ",''";
                                        }
                                    }
                                }
                                else
                                {
                                    if (altersubdeta == "")
                                    {
                                        altersubdeta = "''";
                                    }
                                    else
                                    {
                                        altersubdeta = altersubdeta + ",''";
                                    }
                                }
                            }
                            string alertStr = string.Empty;
                            //added by sudhagar 06.03.2017
                            bool check = getAlternateScheduleCheck(getStaffCode, strDay, colCnt, dateval, ref alertStr, stfName);
                            if (check)
                            {
                                Div3.Visible = true;
                                Label12.Text = alertStr;
                                return;
                            }
                            else
                            {
                                Save();
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        getAlert();
    }

    protected void Save()
    {
        try
        {
            string Dateval;
            int jj = 0;
            int j = 0;
            string strsec = string.Empty;
            int intNHrs = 0;
            int SchOrder = 0;
            string strDay = string.Empty;
            string Strsql = string.Empty;
            int nodays = 0;
            string strinsert = string.Empty;
            string VarSch = string.Empty;
            string dateval;
            string Strsqlval = string.Empty;
            string startdate = string.Empty;
            string todate = string.Empty;
            string bacth = ddlbatch.SelectedValue.ToString();
            string degree = ddlbranch.SelectedValue.ToString();
            string sem = ddlduration.SelectedItem.ToString();
            DataSet dssetbatch = new DataSet();
            bool aeperflag = false;
            string[] Days = new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            SqlConnection con7 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
            string smssend = dacess.GetFunction("select value from Master_Settings where settings='Alternatesms'");
            Dictionary<string, byte> dicDayOrder = getDayOrder();
            if (SpdInfo.Sheets[0].RowCount > 0)
            {
                if (ddlsec.Items.Count == 0)
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                }
                string qrySection = string.Empty;
                string qrySection1 = string.Empty;
                if (ddlsec.Items.Count > 0)
                {
                    string sectionName = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                    if (!string.IsNullOrEmpty(sectionName) && sectionName.Trim().ToLower() != "all" && sectionName.Trim().ToLower() != "-1")
                    {
                        qrySection = " and ct.TT_sec='" + Convert.ToString(sectionName).Trim() + "'";
                        qrySection1 = " and nct.TT_sec='" + Convert.ToString(sectionName).Trim() + "'";
                    }
                }
                int noofalter = Convert.ToInt32(txtnoofalter.Text);
                Hashtable hataltersc = new Hashtable();
                // for (jj = 1; jj <= SpdInfo.Sheets[0].ColumnCount; jj = jj + 2)//----------incement column value
                for (jj = 1; jj <= SpdInfo.Sheets[0].ColumnCount; jj = jj + noofalter + 1)//----------incement column value
                {
                    Dateval = SpdInfo.Sheets[0].ColumnHeader.Cells[0, jj - 1].Note;

                    string[] split = Dateval.Split(new Char[] { '/' });
                    dateval = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                    DateTime head_date = Convert.ToDateTime(dateval.ToString());
                    con8.Open();
                    SqlDataReader dr8;
                    cmd8 = new SqlCommand("Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + "", con8);
                    dr8 = cmd8.ExecuteReader();
                    dr8.Read();
                    if (dr8.HasRows == true)
                    {
                        if ((dr8["No_of_hrs_per_day"].ToString()) != "")
                        {
                            intNHrs = Convert.ToInt32(dr8["No_of_hrs_per_day"]);
                            SchOrder = Convert.ToInt32(dr8["schorder"]);
                            nodays = Convert.ToInt32(dr8["nodays"]);
                        }
                    }
                    con.Close();
                    con.Open();
                    SqlDataReader dr1;
                    cmd = new SqlCommand("select * from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " ", con);
                    dr1 = cmd.ExecuteReader();
                    dr1.Read();
                    if (dr1.HasRows == true)
                    {
                        if ((dr1["start_date"].ToString()) != "" && (dr1["start_date"].ToString()) != "\0")
                        {
                            string[] tmpdate = dr1["start_date"].ToString().Split(new char[] { ' ' });
                            startdate = tmpdate[0].ToString();
                            if (Convert.ToString(dr1["starting_dayorder"]) != "")
                            {
                                start_dayorder = dr1["starting_dayorder"].ToString();
                            }
                            else
                            {
                                start_dayorder = "1";
                            }
                        }
                        else
                        {
                            norecordlbl.Visible = true;
                            norecordlbl.Text = "Update semester Information";
                            norecordlbl.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        norecordlbl.Visible = true;
                        norecordlbl.Text = "Update semester Information";
                        norecordlbl.ForeColor = Color.Red;
                    }
                    if (intNHrs > 0)
                    {
                        if (SchOrder != 0)
                        {
                            strDay = head_date.ToString("ddd");
                        }
                        else
                        {
                            todate = SpdInfo.Sheets[0].ColumnHeader.Cells[1, jj].Note;
                            // strDay = findday(todate.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                            //Modifeid by Srinath 5/9/2014
                            string[] sps = todate.ToString().Split('/');
                            string curdate = sps[1] + '/' + sps[0] + '/' + sps[2];
                            strDay = dacess.findday(curdate.ToString(), ddlbranch.SelectedValue.ToString(), ddlduration.SelectedValue.ToString(), ddlbatch.Text.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                        }


                        string ttname = dacess.GetFunction("select  top 1 ttname from Semester_Schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate <='" + dateval + "'" + strsec + " order by FromDate desc");

                        //ttname = dirAcc.selectScalarString("select top 1 ct.TT_name from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection + " and TT_date<='" + dateval + "' and ct.TT_date=(select MAX(nct.TT_date) from TT_ClassTimetable nct where nct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and nct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and nct.TT_batchyear='" + ddlbatch.SelectedValue + "' and nct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection1 + " and TT_date<='" + dateval + "') ");

                        ttname = dirAcc.selectScalarString("select top 1 ct.TT_name from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection + " and ct.TT_date=(select MAX(nct.TT_date) from TT_ClassTimetable nct where nct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and nct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and nct.TT_batchyear='" + ddlbatch.SelectedValue + "' and nct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection1 + " ) order by ct.TT_date desc");//and TT_date<='" + dateval + "'

                        string classTimeTableId = dirAcc.selectScalarString("select top 1 ct.TT_ClassPK from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection + " and ct.TT_date=(select MAX(nct.TT_date) from TT_ClassTimetable nct where nct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and nct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and nct.TT_batchyear='" + ddlbatch.SelectedValue + "' and nct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection1 + " ) order by ct.TT_date desc");
                        if (ttname.Trim() != "" && ttname != null && ttname.Trim() != "0")
                        {
                            ttname = " and Timetablename='" + ttname + "'";
                        }

                        Strsqlval = string.Empty;
                        Strsqlval = "select top 1 ";
                        string getday = string.Empty;
                        for (int intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                        {
                            Strsqlval = Strsqlval + strDay + intNCtr.ToString() + ",";
                            if (getday == "")
                            {
                                getday = strDay + intNCtr.ToString();
                            }
                            else
                            {
                                getday = getday + "," + strDay + intNCtr.ToString();
                            }
                        }
                        //---------------------check the record in alternate schedule for update
                        con8.Close();
                        con7.Close();
                        con7.Open();
                        SqlDataReader savedr;
                        Strsql = Strsqlval + " degree_code , semester , batch_year,lastrec,sections,TTName,Fromdate from Alternate_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate ='" + dateval + "'" + strsec + " order by FromDate";
                        SqlCommand cmd7 = new SqlCommand(Strsql, con7);
                        savedr = cmd7.ExecuteReader();
                        savedr.Read();
                        string code_value = string.Empty;
                        string cellnote = string.Empty;
                        string sectionval = string.Empty;
                        bool isaltflaf = false;
                        if (ddlsec.Text.ToString() != "" && ddlsec.Text.ToString() != "All")
                        {
                            sectionval = ddlsec.Text.ToString();
                        }

                        for (int alt = 1; alt <= noofalter; alt++)
                        {
                            string selnoofalter = Strsqlval + " degree_code , semester , batch_year,lastrec,sections,TTName,Fromdate,No_of_Alter from tbl_alter_schedule_Details where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate ='" + dateval + "'" + strsec + " and No_of_Alter='" + alt + "' order by FromDate";
                            DataSet dsnoofalter = dacess.select_method(selnoofalter, hat, "Text");
                            if (dsnoofalter.Tables[0].Rows.Count > 0)
                            {
                                string deltequery = "delete from tbl_alter_schedule_Details where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate ='" + dateval + "'" + strsec + " and No_of_Alter='" + alt + "'";
                                int del = dacess.update_method_wo_parameter(deltequery, "Text");
                            }
                            string getStaffCode = string.Empty;
                            int colCnt = 0;
                            string altersubdeta = string.Empty;
                            int altercolu = alt + jj - 1;
                            for (j = 0; j < intNHrs; j++)//---------------loop for row value
                            {
                                string substaffname = SpdInfo.Sheets[0].Cells[j, altercolu].Text;
                                string substaffcode = SpdInfo.Sheets[0].Cells[j, altercolu].Note;
                                string dayval = strDay + Convert.ToInt32(j + 1).ToString();
                                string getlabdetails = string.Empty;
                                if (substaffname != "" && substaffcode != "" && substaffname != "Sunday")
                                {
                                    if (SpdInfo.Sheets[0].Cells[j, altercolu].Locked == false)
                                    {
                                        if (!hataltersc.Contains(dayval))
                                        {
                                            if (!string.IsNullOrEmpty(Convert.ToString(SpdInfo.Sheets[0].Cells[j, altercolu].Tag).Trim()) && SpdInfo.Sheets[0].Cells[j, altercolu].Tag != null)
                                            {
                                                getlabdetails = "/" + SpdInfo.Sheets[0].Cells[j, altercolu].Tag.ToString();
                                            }
                                            hataltersc.Add(dayval, substaffcode + getlabdetails);
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(Convert.ToString(SpdInfo.Sheets[0].Cells[j, altercolu].Tag).Trim()) && SpdInfo.Sheets[0].Cells[j, altercolu].Tag != null)
                                            {
                                                getlabdetails = "/" + SpdInfo.Sheets[0].Cells[j, altercolu].Tag.ToString();
                                            }
                                            hataltersc[dayval] = substaffcode + getlabdetails;
                                        }
                                        if (altersubdeta == "")
                                        {
                                            altersubdeta = "'" + substaffcode + "'";
                                        }
                                        else
                                        {
                                            altersubdeta = altersubdeta + ",'" + substaffcode + "'";
                                        }
                                        colCnt = j + 1;
                                        getStaffCode = substaffcode;
                                    }
                                    else
                                    {
                                        if (altersubdeta == "")
                                        {
                                            altersubdeta = "''";
                                        }
                                        else
                                        {
                                            altersubdeta = altersubdeta + ",''";
                                        }
                                    }
                                }
                                else
                                {
                                    if (altersubdeta == "")
                                    {
                                        altersubdeta = "''";
                                    }
                                    else
                                    {
                                        altersubdeta = altersubdeta + ",''";
                                    }
                                }
                            }
                            //string alertStr = string.Empty;
                            ////added by sudhagar 06.03.2017
                            //bool check = getAlternateScheduleCheck(getStaffCode, strDay, colCnt, dateval, ref alertStr);
                            //if (check)
                            //{
                            //    Div3.Visible = true;
                            //    Label12.Text = alertStr;
                            //    return;
                            //    //  ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "confirm('Unable to locate your search item. Do you want to search the closest match from your item?');", true);
                            //}
                            if (altersubdeta != "" && intNHrs > 0)
                            {
                                string insertnoofalter = "insert into tbl_alter_schedule_Details(degree_code,semester,batch_year,fromdate,lastrec,sections,No_of_Alter," + getday + ") values(" + ddlbranch.SelectedValue.ToString() + "," + ddlduration.SelectedValue.ToString() + "," + ddlbatch.SelectedValue.ToString() + ",'" + dateval + "',0,'" + sectionval + "','" + alt + "'," + altersubdeta + ")";
                                int ins = dacess.update_method_wo_parameter(insertnoofalter, "Text");
                            }
                        }
                        //---------------delete the record from the alternate schedule for insert the updated record
                        if (savedr.HasRows == true)
                        {
                            string delsql = string.Empty;
                            con7.Close();
                            con7.Open();
                            delsql = "delete from Alternate_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate ='" + dateval + "'" + strsec + "";
                            SqlCommand delcmd = new SqlCommand(delsql, con7);
                            SqlDataReader del_dr;
                            del_dr = delcmd.ExecuteReader();
                            del_dr.Read();
                        }
                       
                        string rsec = string.Empty;
                        string getrsec = string.Empty;
                        if (sectionval != "")
                        {
                            rsec = ddlsec.SelectedItem.ToString();
                            getrsec = "and sections='" + rsec + "'";
                        }
                        for (j = 0; j < intNHrs; j++)//---------------loop for row value
                        {
                            VarSch = string.Empty;
                            cellnote = string.Empty;
                            string daygetval = strDay + Convert.ToInt32(j + 1).ToString();
                            if (hataltersc.Contains(daygetval))
                            {
                                VarSch = GetCorrespondingKey(daygetval, hataltersc).ToString();
                                cellnote = GetCorrespondingKey(daygetval, hataltersc).ToString();
                            }
                            if (VarSch != "" && cellnote != "")
                            {
                                string setcode = string.Empty;
                                try
                                {
                                    string[] spitlabhour = cellnote.Split('/');
                                    setcode = spitlabhour[0].ToString();
                                    string[] spitsubject = setcode.Split(';');
                                    for (int subalter = 0; subalter <= spitsubject.GetUpperBound(0); subalter++)
                                    {
                                        string[] splitcode = spitsubject[subalter].Split('-');
                                        string[] splitcodeNew = spitsubject[subalter].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (spitlabhour.GetUpperBound(0) > 0)
                                        {
                                            aeperflag = true;
                                            string getdayhour = spitlabhour[1].ToString();
                                            string[] spitgetdayhour = getdayhour.Split(',');
                                            string strquery = string.Empty;
                                            int insert = 0;
                                            string dayvalue = spitgetdayhour[0].ToString();
                                            int hourvalue = int.Parse(spitgetdayhour[1]) + 1;
                                            if (subalter == 0)
                                            {
                                                strquery = "delete from subjectChooser_New where subject_no='" + splitcode[0].ToString() + "' and semester='" + sem + "' and fromdate='" + dateval + "' and roll_no in( select roll_no from Registration where  batch_year='" + bacth + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and cc=0 and delflag=0 and exam_flag<>'debar' )";
                                                insert = dacess.update_method_wo_parameter(strquery, "Text");
                                                strquery = "delete from laballoc_new where  batch_year='" + bacth + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and day_value='" + strDay + "' and hour_value='" + hourvalue + "' and fdate='" + dateval + "'";
                                                insert = dacess.update_method_wo_parameter(strquery, "Text");
                                            }
                                            strquery = "select distinct s.subtype_no,s.Batch,r.roll_no from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and s.subject_no='" + splitcode[0].ToString() + "' and batch_year='" + bacth + "' and r.degree_code='" + degree + "' and s.semester='" + sem + "' " + getrsec + "";
                                            dssetbatch = dacess.select_method_wo_parameter(strquery, "Text");
                                            if (dssetbatch.Tables[0].Rows.Count > 0)
                                            {
                                                strquery = "insert into subjectChooser_New (semester,roll_no,subject_no,subtype_no,Batch,fromdate,todate) ";
                                                strquery = strquery + "(select s.semester,s.roll_no,s.subject_no,s.subtype_no,s.Batch,'" + dateval + "' as fromdate ,'" + dateval + "' as todate from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and s.subject_no='" + splitcode[0].ToString() + "' and batch_year='" + bacth + "' and r.degree_code='" + degree + "' and s.semester='" + sem + "' " + getrsec + ")";
                                                insert = dacess.update_method_wo_parameter(strquery, "Text");
                                            }
                                            strquery = "select distinct Stu_Batch,Day_Value,Hour_Value from laballoc where batch_year='" + bacth + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and day_value='" + dayvalue + "' and hour_value='" + hourvalue + "' and subject_no='" + splitcode[0].ToString() + "' " + ttname + "";
                                            dssetbatch = dacess.select_method_wo_parameter(strquery, "Text");
                                            for (int b = 0; b < dssetbatch.Tables[0].Rows.Count; b++)
                                            {
                                                string subtype = dssetbatch.Tables[0].Rows[b]["Stu_Batch"].ToString();
                                                string day = dssetbatch.Tables[0].Rows[b]["Day_Value"].ToString();
                                                string hour = dssetbatch.Tables[0].Rows[b]["Day_Value"].ToString();
                                                strquery = "insert into laballoc_new (Batch_Year,Degree_Code,Semester,Sections,Subject_No,Stu_Batch,Day_Value,Hour_Value,fdate,tdate) ";
                                                strquery = strquery + "values('" + bacth + "','" + degree + "','" + sem + "','" + rsec + "','" + splitcode[0].ToString() + "','" + subtype + "','" + strDay + "','" + hourvalue + "','" + dateval + "','" + dateval + "')";
                                                insert = dacess.update_method_wo_parameter(strquery, "Text");
                                            }
                                        }
                                        else
                                        {
                                            int hours = j + 1;
                                            string strquery = "delete from subjectChooser_New where subject_no='" + splitcode[0].ToString() + "' and semester='" + sem + "' and fromdate='" + dateval + "' and roll_no in( select roll_no from Registration where  batch_year='" + bacth + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and cc=0 and delflag=0 and exam_flag<>'debar' )";
                                            int insert = dacess.update_method_wo_parameter(strquery, "Text");
                                            strquery = "delete from laballoc_new where  batch_year='" + bacth + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and day_value='" + strDay + "' and hour_value='" + hours + "' and fdate='" + dateval + "'";
                                            insert = dacess.update_method_wo_parameter(strquery, "Text");
                                            strquery = "select distinct s.subtype_no,s.Batch,r.roll_no from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and s.subject_no='" + splitcode[0].ToString() + "' and batch_year='" + bacth + "' and r.degree_code='" + degree + "' and s.semester='" + sem + "' " + getrsec + "";
                                            dssetbatch = dacess.select_method_wo_parameter(strquery, "Text");
                                            if (dssetbatch.Tables.Count > 0 && dssetbatch.Tables[0].Rows.Count > 0)
                                            {
                                                strquery = "insert into subjectChooser_New (semester,roll_no,subject_no,subtype_no,Batch,fromdate,todate) ";
                                                strquery = strquery + "(select s.semester,s.roll_no,s.subject_no,s.subtype_no,s.Batch,'" + dateval + "' as fromdate ,'" + dateval + "' as todate from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and s.subject_no='" + splitcode[0].ToString() + "' and batch_year='" + bacth + "' and r.degree_code='" + degree + "' and s.semester='" + sem + "' " + getrsec + ")";
                                                insert = dacess.update_method_wo_parameter(strquery, "Text");
                                                aeperflag = true;
                                            }
                                        }
                                        string TTclassPk = Convert.ToString(SpdInfo.Sheets[0].Cells[j, jj - 1].Tag).Trim();                                       
                                        string dayofweek = Days[Convert.ToInt32(GetWeakDay(strDay))];
                                        string daypk = dicDayOrder[dayofweek].ToString();
                                        string delAlterTT = " delete from TT_AlterTimetableDet where TT_Hour ='" + (j + 1) + "' and TT_Day='" + daypk + "' and  TT_AlterDate='" + dateval + "' and TT_ClassFk='" + TTclassPk + "'";
                                        int delTT = dirAcc.insertData(delAlterTT);
                                        if (splitcodeNew.Length > 2)
                                        {
                                            for (int sub = 1; sub < splitcodeNew.Length - 1; sub++)
                                            {
                                                string staffCode = splitcodeNew[sub];
                                                string insUpd = " if exists ( select TT_AlterDetPK from TT_AlterTimetableDet where TT_Hour ='" + (j + 1) + "' and TT_Day='" + daypk + "' and TT_ClassFK='" + TTclassPk + "' and TT_subno='" + splitcodeNew[0] + "' and TT_staffcode='" + staffCode + "' and TT_AlterDate='" + dateval + "') update TT_AlterTimetableDet  set TT_subno='" + splitcodeNew[0] + "',TT_staffcode='" + staffCode + "'  where TT_Hour ='" + (j + 1) + "' and TT_Day='" + daypk + "' and TT_ClassFK='" + TTclassPk + "'  and TT_subno='" + splitcodeNew[0] + "' and TT_staffcode='" + staffCode + "' and TT_AlterDate='" + dateval + "' else insert into TT_AlterTimetableDet (TT_Hour,TT_Day,TT_ClassFK,TT_subno,TT_staffcode,TT_Room,TT_AlterDate) values ('" + (j + 1) + "','" + daypk + "','" + TTclassPk + "','" + splitcodeNew[0] + "','" + staffCode + "','0','" + dateval + "') ";
                                                int a = dirAcc.updateData(insUpd);
                                            }

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                                if (code_value == "")
                                {
                                    code_value = "'" + setcode + "'";
                                    isaltflaf = true;
                                }
                                else
                                {
                                    code_value = code_value + ",'" + setcode + "'";
                                    isaltflaf = true;
                                }
                                if (smssend.Trim() != "" && smssend != null && smssend.Trim() != "0")
                                {
                                    sendsms(daygetval, ddlbatch.SelectedValue.ToString(), ddlbranch.SelectedValue.ToString(), ddlduration.SelectedValue.ToString(), sectionval, dateval, setcode);
                                }
                            }
                            else
                            {
                                if (code_value == "")
                                {
                                    code_value = "''";
                                }
                                else
                                {
                                    code_value = code_value + ",''";
                                }
                            }
                        }
                        if (aeperflag == false)
                        {
                            string strquery = "delete from subjectChooser_New where semester='" + sem + "' and fromdate='" + dateval + "'  and roll_no in(Select roll_no from registration where batch_year='" + bacth + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and cc=0 and delflag=0 and exam_flag<>'debar'  )";
                            int insert = dacess.update_method_wo_parameter(strquery, "Text");
                        }
                        //---------------save the record into altenate schedule
                        if (code_value != "" && isaltflaf == true)
                        {
                            strinsert = "insert into Alternate_schedule(degree_code,semester,batch_year,fromdate,lastrec,sections," + getday + ") values(" + ddlbranch.SelectedValue.ToString() + "," + ddlduration.SelectedValue.ToString() + "," + ddlbatch.SelectedValue.ToString() + ",'" + dateval + "',0,'" + sectionval + "'," + code_value + ")";
                            con1a.Close();
                            con1a.Open();
                            SqlCommand savecmd = new SqlCommand(strinsert, con1a);
                            SqlDataReader save_dr;
                            save_dr = savecmd.ExecuteReader();
                            save_dr.Read();
                            //for (j = 0; j < intNHrs; j++)//---------------loop for row value
                            //{
                            //    VarSch = string.Empty;
                            //    cellnote = string.Empty;
                            //    string daygetval = strDay + Convert.ToInt32(j + 1).ToString();
                            //    if (hataltersc.Contains(daygetval))
                            //    {
                            //        VarSch = GetCorrespondingKey(daygetval, hataltersc).ToString();
                            //        cellnote = GetCorrespondingKey(daygetval, hataltersc).ToString();
                            //    }
                            //    if (VarSch != "" && cellnote != "")
                            //    {
                            //    }
                            //}
                            btnsave.Enabled = false;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                        }
                    }
                }
                batchbtn.Visible = true;
            }
        }
        catch
        {
        }
    }

    public void sendsms(string day, string batch, string degree, string sem, string sec, string date, string alterperiod)
    {
        try
        {
            Dictionary<string, string> dicstaffname = new Dictionary<string, string>();
            string semstaffs = "", semsubject = "", semstaffname = string.Empty;
            string alterstaffs = "", altsubject = "", altstaffname = string.Empty;
            string sect = string.Empty;
            string secsms = string.Empty;
            if (sec.Trim() != "" && sec != null && sec != "-1")
            {
                sect = "and sections='" + sec + "'";
                secsms = " Sec-" + sec + "";
            }
            string user_id = dacess.GetFunction("select SMS_User_ID from Track_Value where college_code = '" + Session["collegecode"].ToString() + "'");
            if (user_id.Trim() != "" && user_id.Trim() != "0" && user_id != null)
            {
                //modified by srinath 1/8/2014
                //GetUserapi(user_id);
                string getval = dacess.GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {
                    SenderID = spret[0].ToString();
                    Password = spret[1].ToString();
                    Session["api"] = user_id;
                    Session["senderid"] = SenderID;
                }
                if (SenderID.Trim() != "" && Password.Trim() != "")
                {
                    if (alterperiod.Trim() != "")
                    {
                        string[] spalsu = alterperiod.Split(';');
                        for (int su = 0; su <= spalsu.GetUpperBound(0); su++)
                        {
                            string[] altsubst = spalsu[su].Split('-');
                            if (altsubst.GetUpperBound(0) > 0)
                            {
                                string altersub = dacess.GetFunction("Select Subject_code from subject where subject_no='" + altsubst[0].ToString() + "'");
                                if (altsubject == "")
                                {
                                    altsubject = altersub;
                                }
                                else
                                {
                                    altsubject = altsubject + ',' + altersub;
                                }
                                for (int altst = 1; altst < altsubst.GetUpperBound(0); altst++)
                                {
                                    string staffcode = altsubst[altst];
                                    string staffname = dacess.GetFunction("Select staff_name from staffmaster where staff_code='" + staffcode + "'");
                                    if (!dicstaffname.ContainsKey(staffcode))
                                    {
                                        dicstaffname.Add(staffcode, staffname);
                                    }
                                    if (alterstaffs == "")
                                    {
                                        alterstaffs = staffcode;
                                        altstaffname = staffname;
                                    }
                                    else
                                    {
                                        alterstaffs = alterstaffs + ',' + staffcode;
                                        altstaffname = altstaffname + ',' + staffname;
                                    }
                                }
                            }
                        }
                    }
                    string getsemperiod = dacess.GetFunction("select top 1 " + day + " from Semester_Schedule where batch_year=" + batch + " and degree_code = " + degree + " and semester = " + sem + " and FromDate<='" + date + "' " + sect + " order by FromDate");
                    if (getsemperiod.Trim() != "" && getsemperiod != "0" && getsemperiod != null)
                    {
                        string[] spsub = getsemperiod.Split(';');
                        for (int su = 0; su <= spsub.GetUpperBound(0); su++)
                        {
                            string[] sptstaff = spsub[su].Split('-');
                            if (sptstaff.GetUpperBound(0) > 0)
                            {
                                string subjectcode = dacess.GetFunction("Select Subject_code from subject where subject_no='" + sptstaff[0].ToString() + "'");
                                if (semsubject == "")
                                {
                                    semsubject = subjectcode;
                                }
                                else
                                {
                                    semsubject = semsubject + ',' + subjectcode;
                                }
                                for (int sust = 1; sust < sptstaff.GetUpperBound(0); sust++)
                                {
                                    string staffcode = sptstaff[sust].ToString();
                                    string staffname = dacess.GetFunction("Select staff_name from staffmaster where staff_code='" + sptstaff[sust].ToString() + "'");
                                    if (!dicstaffname.ContainsKey(staffcode))
                                    {
                                        dicstaffname.Add(staffcode, staffname);
                                    }
                                    if (semstaffs == "")
                                    {
                                        semstaffs = staffcode;
                                        semstaffname = staffname;
                                    }
                                    else
                                    {
                                        semstaffs = semstaffs + ',' + staffcode;
                                        semstaffname = semstaffname + ',' + staffname;
                                    }
                                }
                            }
                        }
                    }
                    char hr = day[3];
                    degree = dacess.GetFunction("select c.Course_Name+'-'+de.dept_acronym as degree from Degree d,course c,Department de where d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and c.college_code=de.college_code and d.Degree_Code=" + degree + "");
                    string[] spdt = date.Split('/');
                    string smsdate = spdt[1] + '/' + spdt[0] + '/' + spdt[2];
                    string[] spsem = semstaffs.Split(',');
                    for (int staff = 0; staff <= spsem.GetUpperBound(0); staff++)
                    {
                        string staffcode = spsem[staff].ToString();
                        string mobileno = GetFunction("select per_mobileno from staff_appl_master a,staffmaster m where m.appl_no = a.appl_no and m.college_code = a.college_code and m.staff_code = '" + staffcode.ToString() + "'");
                        if (mobileno != null && mobileno.Trim() != "0" && mobileno.Trim() != "")
                        {
                            if (dicstaffname.ContainsKey(staffcode))
                            {
                                string staffname = dicstaffname[staffcode];
                                string sex = dacess.GetFunction("select sa.sex from staffmaster sm,staff_appl_master sa where sa.appl_no=sm.appl_no and sm.staff_code='" + staffcode + "'");
                                if (sex.Trim().ToLower() == "male")
                                {
                                    sex = "Mr ";
                                }
                                else if (sex.Trim().ToLower() == "female")
                                {
                                    sex = "Mrs/Ms ";
                                }
                                //Modified By Srinath 8/2/2014
                                string strmsg = sex + staffname + " your schedule on " + smsdate + " period-" + hr + " Batch Year " + batch + "-" + degree + "-Sem " + sem + " " + secsms + " Subject-" + semsubject + " has been altered to staff name-" + altstaffname + " subject code-" + altsubject + "";
                                //  string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mobileno + "&message=" + strmsg + "&sender=" + SenderID;
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobileno + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                                //smsreport(strpath, mobileno, strmsg);
                                //int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, RecepientNo, MsgText, "0");
                                int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), usercode, mobileno, strmsg, "1");
                            }
                        }
                    }
                    string[] altsem = alterstaffs.Split(',');
                    for (int staff = 0; staff <= altsem.GetUpperBound(0); staff++)
                    {
                        string staffcode = altsem[staff].ToString();
                        string mobileno = GetFunction("select per_mobileno from staff_appl_master a,staffmaster m where m.appl_no = a.appl_no and m.college_code = a.college_code and m.staff_code = '" + staffcode + "'");
                        if (mobileno != null && mobileno.Trim() != "0" && mobileno.Trim() != "")
                        {
                            if (dicstaffname.ContainsKey(staffcode))
                            {
                                string staffname = dicstaffname[staffcode];
                                string sex = dacess.GetFunction("select sa.sex from staffmaster sm,staff_appl_master sa where sa.appl_no=sm.appl_no and sm.staff_code='" + staffcode + "'");
                                if (sex.Trim().ToLower() == "male")
                                {
                                    sex = "Mr ";
                                }
                                else if (sex.Trim().ToLower() == "female")
                                {
                                    sex = "Mrs/Ms ";
                                }
                                //Modified By Srinath 8/2/2014
                                string strmsg = sex + staffname + " your schedule Alterde on " + smsdate + " period-" + hr + " Batch Year " + batch + "-" + degree + "-Sem " + sem + " " + secsms + " Subject-" + altsubject + " altered from staff name-" + semstaffname + "";
                                //string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mobileno + "&message=" + strmsg + "&sender=" + SenderID;
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobileno + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                                //smsreport(strpath, mobileno, strmsg);
                                int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), usercode, mobileno, strmsg, "1");
                            }
                        }
                    }
                }
            }
            try
            {
                string strquery = "select massemail,masspwd from collinfo where college_code = " + Session["collegecode"].ToString() + " ";
                string send_mail = "", send_pw = "", to_mail = "", strstuname = "", strmsg = string.Empty;
                DataSet ds1 = dacess.select_method(strquery, hat, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    send_mail = Convert.ToString(ds1.Tables[0].Rows[0]["massemail"]);
                    send_pw = Convert.ToString(ds1.Tables[0].Rows[0]["masspwd"]);
                }
                char hr = day[3];
                string[] spdt = date.Split('/');
                string smsdate = spdt[1] + '/' + spdt[0] + '/' + spdt[2];
                string[] spsem = semstaffs.Split(',');
                for (int staff = 0; staff <= spsem.GetUpperBound(0); staff++)
                {
                    string staffcode = spsem[staff].ToString();
                    to_mail = dacess.GetFunction("select email from staff_appl_master sa,staffmaster sm where sm.staff_code='" + staffcode + "' and sm.appl_no=sa.appl_no");
                    if (to_mail != null && to_mail.Trim() != "0" && to_mail.Trim() != "")
                    {
                        if (dicstaffname.ContainsKey(staffcode))
                        {
                            string staffname = dicstaffname[staffcode];
                            string sex = dacess.GetFunction("select sa.sex from staffmaster sm,staff_appl_master sa where sa.appl_no=sm.appl_no and sm.staff_code='" + staffcode + "'");
                            if (sex.Trim().ToLower() == "male")
                            {
                                sex = "Mr ";
                            }
                            else if (sex.Trim().ToLower() == "female")
                            {
                                sex = "Mrs/Ms ";
                            }
                            strmsg = " your schedule on " + smsdate + " period-" + hr + " Batch Year " + batch + "-" + degree + "-Sem " + sem + " " + secsms + " Subject-" + semsubject + " has been altered to staff name-" + altstaffname + " subject code-" + altsubject + "";
                            SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                            MailMessage mailmsg = new MailMessage();
                            MailAddress mfrom = new MailAddress(send_mail);
                            mailmsg.From = mfrom;
                            mailmsg.To.Add(to_mail);
                            mailmsg.Subject = "Report";
                            mailmsg.IsBodyHtml = true;
                            mailmsg.Body = "" + sex + " " + staffname + "";
                            mailmsg.Body = mailmsg.Body + strstuname;
                            mailmsg.Body = mailmsg.Body + strmsg;
                            mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                            Mail.EnableSsl = true;
                            NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                            Mail.UseDefaultCredentials = false;
                            Mail.Credentials = credentials;
                            Mail.Send(mailmsg);
                        }
                    }
                }
                string[] altsem = alterstaffs.Split(',');
                for (int staff = 0; staff <= altsem.GetUpperBound(0); staff++)
                {
                    string staffcode = altsem[staff].ToString();
                    to_mail = dacess.GetFunction("select email from staff_appl_master sa,staffmaster sm where sm.staff_code='" + staffcode + "' and sm.appl_no=sa.appl_no");
                    if (to_mail != null && to_mail.Trim() != "0" && to_mail.Trim() != "")
                    {
                        if (dicstaffname.ContainsKey(staffcode))
                        {
                            string staffname = dicstaffname[staffcode];
                            string sex = dacess.GetFunction("select sa.sex from staffmaster sm,staff_appl_master sa where sa.appl_no=sm.appl_no and sm.staff_code='" + staffcode + "'");
                            if (sex.Trim().ToLower() == "male")
                            {
                                sex = "Mr ";
                            }
                            else if (sex.Trim().ToLower() == "female")
                            {
                                sex = "Mrs/Ms ";
                            }
                            strmsg = " your schedule Alterde on " + smsdate + " period-" + hr + " Batch Year " + batch + "-" + degree + "-Sem " + sem + " " + secsms + " Subject-" + altsubject + " altered from staff name-" + semstaffname + "";
                            SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                            MailMessage mailmsg = new MailMessage();
                            MailAddress mfrom = new MailAddress(send_mail);
                            mailmsg.From = mfrom;
                            mailmsg.To.Add(to_mail);
                            mailmsg.Subject = "Report";
                            mailmsg.IsBodyHtml = true;
                            mailmsg.Body = "" + sex + " " + staffname + "";
                            mailmsg.Body = mailmsg.Body + strstuname;
                            mailmsg.Body = mailmsg.Body + strmsg;
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
            catch
            {
            }
        }
        catch
        {
        }
    }

    public void smsreport(string uril, string mobilenos, string strmsg)
    {
        try
        {
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = string.Empty;
            groupmsgid = strvel.Trim().ToString(); //aruna 02oct2013 strvel;       
            int sms = 0;
            string smsreportinsert = string.Empty;
            string[] split_mobileno = mobilenos.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + split_mobileno[icount] + "','" + groupmsgid + "','" + strmsg + "','" + Session["collegecode"].ToString() + "','1','" + date + "','" + Session["UserCode"].ToString() + "')"; // Added by jairam 21-11-2014
                sms = dacess.insert_method(smsreportinsert, hat, "Text");
            }
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Message has been Sended Successfully')", true);
        }
        catch
        {
        }
    }

    //---------------------------------------------------------avaialble staff
    protected void free_staff()
    {
        try
        {
            freestaff.Sheets[0].ColumnCount = 0;
            freestaff.Sheets[0].RowCount = 0;
            string date1;
            string date2;
            string datefrom;
            string dateto;
            string strDay = string.Empty;
            string detail_no = string.Empty;
            string staff_code = string.Empty;
            string subj_no = string.Empty;
            string sub_staff = string.Empty;
            string asql = string.Empty;
            string Staff_Code = string.Empty;
            string sqlstr;
            int noofhrs;
            string date_change;
            bool isstafffree = false;
            //---------------------------------------------               
            freestaff.Sheets[0].SheetCorner.Cells[0, 0].Text = "Date";
            if (txtFromDate.Text.ToString() != "" && txtFromDate.Text.ToString() != "\0")
            {
                if (txtToDate.Text.ToString() != "" && txtToDate.Text.ToString() != "\0")
                {
                    date1 = txtFromDate.Text.ToString();
                    string[] split = date1.Split(new Char[] { '/' });
                    datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                    date2 = txtToDate.Text.ToString();
                    string[] split1 = date2.Split(new Char[] { '/' });
                    dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                    DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                    DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                    TimeSpan t = dt2.Subtract(dt1);
                    long days = t.Days;
                    string[] differ_days = new string[days];
                    sqlstr = "select No_of_hrs_per_day from PeriodAttndSchedule where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester=" + ddlduration.SelectedValue.ToString() + " ";
                    noofhrs = Convert.ToInt32(GetFunction(sqlstr));
                    if (days >= 0)
                    {
                        if (noofhrs != 0)
                        {
                            for (int i = 1; i <= noofhrs; i++)
                            {
                                freestaff.Sheets[0].ColumnCount = freestaff.Sheets[0].ColumnCount + 1;
                                freestaff.Sheets[0].ColumnHeader.Cells[0, freestaff.Sheets[0].ColumnCount - 1].Text = "Period " + Convert.ToString(i);
                                freestaff.Sheets[0].Columns[freestaff.Sheets[0].ColumnCount - 1].Width = 100;
                                freestaff.Sheets[0].Columns[freestaff.Sheets[0].ColumnCount - 1].Locked = true;//Added by Manikandan 21/08/2013
                                freestaff.Sheets[0].Columns[i - 1].Font.Name = "Book Antiqua";
                                freestaff.Sheets[0].Columns[i - 1].Font.Size = FontUnit.Medium;
                            }
                            string[] split_1 = date1.Split(new Char[] { '/' });
                            for (int k = 0; k <= days; k++)
                            {
                                DateTime split_plus = dt1.AddDays(k);
                                string split_str = string.Empty;
                                split_str = split_plus.ToString();
                                string[] split_1_str = split_str.Split(' ');
                                string[] split_dt = split_1_str[0].Split('/');
                                //date_change = split_dt[0].ToString() + "/" + split_dt[1].ToString() + "/" + split_dt[2].ToString();
                                date_change = split_dt[1].ToString() + "/" + split_dt[0].ToString() + "/" + split_dt[2].ToString();//Modified by Manikandan 15/08/2013 from above Line
                                freestaff.Sheets[0].RowCount = freestaff.Sheets[0].RowCount + 1;
                                freestaff.Sheets[0].RowHeader.Cells[freestaff.Sheets[0].RowCount - 1, 0].Text = date_change;
                                con1a.Close();
                                con1a.Open();
                                SqlCommand cmd_holi = new SqlCommand("select holiday_desc from holidaystudents where holiday_date='" + split_1_str[0].ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + "", con1a);
                                string str_holiday = (string)cmd_holi.ExecuteScalar();
                                con1a.Close();
                                con1a.Open();
                                SqlCommand cmd1a;
                                SqlDataReader staff_list;
                                cmd1a = new SqlCommand("select distinct st.subject_no,s.subject_name,st.staff_code from subject s,staff_selector st,syllabus_master sy where s.syll_code= sy.syll_code  and sy.degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and sy.batch_year=" + ddlbatch.SelectedValue.ToString() + " and st.batch_year=" + ddlbatch.SelectedValue.ToString() + " and st.sections='" + ddlsec.SelectedValue.ToString() + "' and s.subject_no=st.subject_no order by st.subject_no,s.subject_name,st.staff_code ", con1a);
                                staff_list = cmd1a.ExecuteReader();
                                while (staff_list.Read())
                                {
                                    isstafffree = false;
                                    if (staff_list.HasRows == true)
                                    {
                                        Staff_Code = staff_list[2].ToString();
                                        Session["Staff_Code_Temp"] = Staff_Code.ToString();
                                        if (noofhrs > 0)
                                        {
                                            string sql_s = string.Empty;
                                            string sql1 = string.Empty;
                                            string day_change;
                                            string SqlBatchYear = string.Empty;
                                            string SqlPrefinal1 = string.Empty;
                                            string SqlPrefinal2 = string.Empty;
                                            string SqlPrefinal3 = string.Empty;
                                            string SqlPrefinal4 = string.Empty;
                                            string SqlFinal = string.Empty;
                                            string SqlBatchYear1 = string.Empty;
                                            string SqlPrefinal11 = string.Empty;
                                            string SqlPrefinal22 = string.Empty;
                                            string SqlPrefinal33 = string.Empty;
                                            string SqlPrefinal44 = string.Empty;
                                            string SqlFinal1 = string.Empty;
                                            string Schedule_string = string.Empty;
                                            string staff_name = string.Empty;
                                            string[] split_1a = date1.Split(new Char[] { '/' });
                                            //for (int m = 0; m <= days; m++)
                                            //{
                                            string Strsql = string.Empty;
                                            //split_plus = Convert.ToInt32(split_1a[0]) + k;
                                            //date_change = split_plus.ToString() + "/" + split_1a[1].ToString() + "/" + split_1a[2].ToString();
                                            //string[] split_sub = date_change.Split(new Char[] { '/' });
                                            //day_change = split_sub[1].ToString() + "/" + split_sub[0].ToString() + "/" + split_sub[2].ToString();
                                            //DateTime day_name = Convert.ToDateTime(day_change.ToString());
                                            //DateTime split_plus_1 = dt1.AddDays(m);
                                            //string split_str_1 =string.Empty;
                                            //split_str_1 = split_plus_1.ToString();
                                            //string[] split_1_str_1 = split_str_1.Split(' ');
                                            //string[] splitdate = split_1_str_1[0].Split(new char[] { '/' });
                                            //string finddaydate = splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2];
                                            strDay = split_plus.ToString("ddd");
                                            //=========Added by Manikandan 26/08/2013=========
                                            DateTime startdate = Convert.ToDateTime(GetFunction("select start_date from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " "));
                                            if (startdate.ToString() != "" && startdate.ToString() != null)
                                            {
                                                strDay = startdate.ToString("ddd");
                                            }
                                            con.Close();
                                            con.Open();
                                            SqlDataReader dr;
                                            cmd = new SqlCommand("Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + "", con);
                                            dr = cmd.ExecuteReader();
                                            dr.Read();
                                            if (dr.HasRows == true)
                                            {
                                                if ((dr["No_of_hrs_per_day"].ToString()) != "")
                                                {
                                                    intNHrs = Convert.ToInt32(dr["No_of_hrs_per_day"]);
                                                    SchOrder = Convert.ToInt32(dr["schorder"]);
                                                    nodays = Convert.ToInt32(dr["nodays"]);
                                                }
                                            }
                                            if (intNHrs > 0)
                                            {
                                                if (SchOrder != 0)
                                                {
                                                    strDay = (Convert.ToDateTime(split_1_str[0].ToString())).ToString("ddd");
                                                }
                                                else
                                                {
                                                    //todate = SpdInfo.Sheets[0].ColumnHeader.Cells[0, 0].Text;
                                                    //strDay = findday(date_change, startdate.ToString(), nodays.ToString(), start_dayorder.ToString());  //findday(nodays, startdate.ToString(), dt2) + (ar + 1);
                                                    //Modifeid by Srinath 5/9/2014
                                                    string[] sps = date_change.ToString().Split('/');
                                                    string curdate = sps[1] + '/' + sps[0] + '/' + sps[2];
                                                    strDay = dacess.findday(curdate.ToString(), ddlbranch.SelectedValue.ToString(), ddlduration.SelectedValue.ToString(), ddlbatch.Text.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                                                }
                                            }
                                            //=======================================
                                            con2a.Close();
                                            con2a.Open();
                                            sql_s = "select semester_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=semester_schedule.degree_code and semester=semester_schedule.semester), ";
                                            asql = "select Alternate_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=Alternate_schedule.degree_code and semester=Alternate_schedule.semester), ";
                                            SqlCommand cmdasql = new SqlCommand(asql, con4a);
                                            for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                            {
                                                Strsql = Strsql + strDay + Convert.ToString(i_loop) + ",";
                                                if (sql1 == "")
                                                {
                                                    sql1 = sql1 + strDay + Convert.ToString(i_loop) + " like '%" + (string)Session["Staff_Code_Temp"] + "%'";
                                                }
                                                else
                                                {
                                                    sql1 = sql1 + " or " + strDay + Convert.ToString(i_loop) + " like '%" + (string)Session["Staff_Code_Temp"] + "%'";
                                                }
                                            }
                                            sql1 = "(" + sql1 + ")";
                                            sql_s = sql_s + Strsql + "";
                                            asql = asql + Strsql + "";
                                            string day_from;
                                            date1 = txtFromDate.Text.ToString();
                                            string[] split_su = date1.Split(new Char[] { '/' });
                                            day_from = split_su[1].ToString() + "/" + split_su[0].ToString() + "/" + split_su[2].ToString();
                                            DateTime date_from = Convert.ToDateTime(day_from.ToString());
                                            SqlBatchYear = "(select distinct(registration.batch_year) from registration,semester_schedule where registration.degree_code=semester_schedule.degree_code and registration.cc=0 and delflag=0 and registration.exam_flag<>'DEBAR' AND registration.current_Semester = semester_schedule.semester)";
                                            SqlPrefinal1 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and semester=1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                            SqlPrefinal2 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Semester' and First_Year_Nonsemester=0)";
                                            SqlPrefinal3 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Non Semester') ";
                                            SqlPrefinal4 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and semester<>1 and semester<>-1  and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                            SqlFinal = "(" + SqlPrefinal1 + ") union all (" + SqlPrefinal4 + ") union all (" + SqlPrefinal2 + ") union all (" + SqlPrefinal3 + ")";
                                            //SqlBatchYear = "(select distinct(registration.batch_year) from registration,semester_schedule where registration.degree_code=semester_schedule.degree_code and registration.cc=0 and delflag=0 and registration.exam_flag<>'DEBAR' AND registration.current_Semester = semester_schedule.semester)";
                                            //SqlPrefinal1 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and semester=1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                            //SqlPrefinal2 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Semester' and First_Year_Nonsemester=0)";
                                            //SqlPrefinal3 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and  batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Non Semester') ";
                                            //SqlPrefinal4 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and  batch_year in " + SqlBatchYear + " and " + sql1 + " and semester<>1 and semester<>-1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                            //SqlFinal = "(" + SqlPrefinal1 + ") union all (" + SqlPrefinal4 + ") union all (" + SqlPrefinal2 + ") union all (" + SqlPrefinal3 + ")";
                                            SqlBatchYear1 = "(select distinct(registration.batch_year) from registration,Alternate_schedule where registration.degree_code=Alternate_schedule.degree_code and registration.cc=0 and delflag=0 and registration.exam_flag<>'DEBAR' AND registration.current_Semester = Alternate_schedule.semester)";
                                            SqlPrefinal11 = asql + " semester,sections from Alternate_schedule where batch_year in " + SqlBatchYear1 + " and " + sql1 + " and semester=1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                            SqlPrefinal22 = asql + " semester,sections from Alternate_schedule where  FromDate ='" + split_1_str[0] + "'  and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Semester' and First_Year_Nonsemester=0)";
                                            SqlPrefinal33 = asql + " semester,sections from Alternate_schedule where  FromDate ='" + split_1_str[0] + "' and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Non Semester') ";
                                            SqlPrefinal44 = asql + " semester,sections from Alternate_schedule where  FromDate ='" + split_1_str[0] + "'  and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and semester<>1  and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                            SqlFinal1 = "(" + SqlPrefinal11 + ") union all (" + SqlPrefinal44 + ") union all (" + SqlPrefinal22 + ") union all (" + SqlPrefinal33 + ")";
                                            con4a.Close();
                                            con4a.Open();
                                            SqlDataAdapter da_alternate = new SqlDataAdapter("select degree_code,semester," + Strsql + " sections from Alternate_schedule where fromdate='" + split_1_str[0] + "'", con4a);
                                            DataTable dt_alternate = new DataTable();
                                            da_alternate.Fill(dt_alternate);
                                            //Semester Schedule
                                            con4a.Close();
                                            con4a.Open();
                                            SqlCommand cmd_1 = new SqlCommand(SqlFinal, con4a);
                                            SqlDataAdapter da_1 = new SqlDataAdapter(cmd_1);
                                            DataTable dt_1 = new DataTable();
                                            da_1.Fill(dt_1);
                                            //Alternate Schedule
                                            SqlCommand cmd_2 = new SqlCommand(SqlFinal1, con4a);
                                            SqlDataAdapter da_2 = new SqlDataAdapter(cmd_2);
                                            DataTable dt_2 = new DataTable();
                                            da_2.Fill(dt_2);
                                            string staffavail = string.Empty;
                                            int rowcount = 0;
                                            string freestaffname = string.Empty;
                                            DateTime? curfromsem = null;
                                            DateTime? curtosem = null;
                                            for (int col_cnt = 1; col_cnt <= noofhrs; col_cnt++)
                                            {
                                                int a = 0;
                                                int b = 0;
                                                if (!string.IsNullOrEmpty(str_holiday))
                                                {
                                                    freestaff.Sheets[0].Cells[k, col_cnt - 1].Text = str_holiday + " Holiday";
                                                }
                                                else
                                                {
                                                    for (int row_cnt = 0; row_cnt < dt_1.Rows.Count; row_cnt++)
                                                    {
                                                        con4a.Close();
                                                        con4a.Open();
                                                        string cmd_semdate = "select start_date,end_date from seminfo where degree_code=" + dt_1.Rows[row_cnt]["degree_code"].ToString() + " and semester=" + dt_1.Rows[row_cnt]["semester"].ToString() + " and batch_year=" + dt_1.Rows[row_cnt]["batch_year"].ToString() + "";
                                                        SqlDataAdapter da_semdate = new SqlDataAdapter(cmd_semdate, con4a);
                                                        DataTable dt_semdate = new DataTable();
                                                        da_semdate.Fill(dt_semdate);
                                                        if (dt_semdate.Rows.Count > 0)
                                                        {
                                                            curfromsem = Convert.ToDateTime(dt_semdate.Rows[0]["Start_date"].ToString());
                                                            curtosem = Convert.ToDateTime(dt_semdate.Rows[0]["end_date"].ToString());
                                                        }
                                                        if (Convert.ToDateTime(day_from) >= curfromsem && Convert.ToDateTime(day_from) <= curtosem)
                                                        {
                                                            string staffcode = dt_1.Rows[row_cnt][col_cnt + 1].ToString();
                                                            if (staffcode.Contains(Staff_Code) == true)
                                                            {
                                                                a++;
                                                            }
                                                            //Check alternate
                                                            for (int row_cnt_1 = 0; row_cnt_1 < dt_alternate.Rows.Count; row_cnt_1++)
                                                            {
                                                                staffcode = dt_alternate.Rows[row_cnt_1][col_cnt + 1].ToString();
                                                                if (!string.IsNullOrEmpty(staffcode))
                                                                {
                                                                    b = 1;
                                                                    a = 0;
                                                                }
                                                            }
                                                            //alternate end
                                                        }
                                                    }
                                                    //if (a != 100)
                                                    //{
                                                    for (int row_cnt_1 = 0; row_cnt_1 < dt_2.Rows.Count; row_cnt_1++)
                                                    {
                                                        string staffcode = dt_2.Rows[row_cnt_1][col_cnt + 1].ToString();
                                                        if (staffcode.Contains(Staff_Code) == true)
                                                        {
                                                            a++;
                                                            b = 1;
                                                        }
                                                    }
                                                    //}
                                                    //free staff name
                                                    if (dt_2.Rows.Count == 0)
                                                    {
                                                        b = 1;
                                                    }
                                                    if (a == 0 && b == 0)
                                                    {
                                                        b = 1;
                                                    }
                                                    if (a == 0 && b == 1)
                                                    {
                                                        freestaffname = freestaff.Sheets[0].Cells[rowcount, col_cnt - 1].Text;
                                                        rowcount = Convert.ToInt32(freestaff.Sheets[0].RowCount) - 1;
                                                        staff_name = GetFunction("select staff_name from staffmaster where staff_code='" + Staff_Code + "'");
                                                        if (freestaffname.Contains(staff_name) == false)
                                                        {
                                                            freestaff.Sheets[0].Cells[k, col_cnt - 1].Text += staff_name + ";";
                                                        }
                                                    }//free staff name end
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
            //freestaff.Sheets[0].PageSize = freestaff.Sheets[0].RowCount;//Added by Manikandan 15/08/2013
        }
        catch
        {
        }
    }

    protected void SpdInfo_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Cellclickeve = true;
            subjtree.Visible = true;
            //btnOk.Visible = true;
            //chkappend.Visible = true;
        }
        catch
        {
        }
    }

    protected void sem_schedule_Click()
    {
        try
        {
            string Syllabus_year = string.Empty;
            Syllabus_year = GetSyllabusYear("+ddlbranch.SelectedValue.ToString()+", "+ddlbatch.SelectedValue.ToString()+", "+ ddlduration.SelectedValue.ToString()+");
            if ((Syllabus_year).ToString() != "0")
            {
                loadschedule();
            }
        }
        catch
        {
        }
    }

    public void loadschedule()
    {
        try
        {
            string strsec = string.Empty;
            int intNHrs = 0;
            int SchOrder = 0;
            int nodays = 0;
            string srt_day = string.Empty;
            int order = 0;
            int insert_val = 0;
            string sunjno_staffno = string.Empty;
            int subj_no = 0;
            string acronym_val = string.Empty;
            int day_list = 0;
            string day_order = string.Empty;
            int ind_subj = 0;
            string sunjno_staffno_s = string.Empty;
            string acro = string.Empty;
            string acronym = string.Empty;
            string alt_sched = string.Empty;
            string shed_list = string.Empty;
            int spdinfo_ac = 0;
            string todate = string.Empty;
            semspread.Sheets[0].RowCount = 0;
            semspread.Sheets[0].ColumnCount = 0;
            spdinfo_ac = SpdInfo.ActiveSheetView.ActiveColumn;
            semspread.Sheets[0].SheetCorner.Cells[0, 0].Text = "Day/Week Order";
            //-------------date
            string date1, date2;
            string datefrom, dateto;
            date1 = txtFromDate.Text.ToString();
            string[] split = date1.Split(new Char[] { '/' });
            datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            date2 = txtToDate.Text.ToString();
            string[] split1 = date2.Split(new Char[] { '/' });
            dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
            DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
            DateTime dt2 = Convert.ToDateTime(dateto.ToString());
            TimeSpan t = dt2.Subtract(dt1);
            long days = t.Days;
            string dept_fmdate = string.Empty;
            //-------------start date
            SqlDataReader sem_dr;
            SqlCommand cmd_dr;
            con.Close();
            con.Open();
            cmd_dr = new SqlCommand("select start_date from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + "", con);
            sem_dr = cmd_dr.ExecuteReader();
            sem_dr.Read();
            if (sem_dr.HasRows == true)
            {
                dept_fmdate = sem_dr["start_date"].ToString();
            }
            //-------section
            if (ddlsec.Text == "")
            {
                strsec = string.Empty;
            }
            else
            {
                if (ddlsec.SelectedValue.ToString() == "-1")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                }
            }
            semspread.Sheets[0].ColumnCount = 0;
            semspread.Sheets[0].RowCount = 0;
            con.Close();
            con.Open();
            SqlDataReader dr;
            cmd = new SqlCommand("Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + "", con);
            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows == true)
            {
                if ((dr["No_of_hrs_per_day"].ToString()) != "")
                {
                    intNHrs = Convert.ToInt32(dr["No_of_hrs_per_day"]);
                    SchOrder = Convert.ToInt32(dr["schorder"]);
                    nodays = Convert.ToInt32(dr["nodays"]);
                }
            }
            //------------------------dayorder
            if (intNHrs > 0)
            {
                if (SchOrder != 0)
                {
                    srt_day = dt1.ToString("ddd");
                    semspread.Sheets[0].RowCount = nodays;
                    if (nodays >= 1)
                    {
                        semspread.Sheets[0].RowHeader.Cells[0, 0].Text = "Monday";
                        semspread.Sheets[0].RowHeader.Cells[0, 0].Tag = "mon";
                        semspread.Sheets[0].RowHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (nodays >= 2)
                    {
                        semspread.Sheets[0].RowHeader.Cells[1, 0].Text = "Tueday";
                        semspread.Sheets[0].RowHeader.Cells[1, 0].Tag = "tue";
                        semspread.Sheets[0].RowHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (nodays >= 3)
                    {
                        semspread.Sheets[0].RowHeader.Cells[2, 0].Text = "Wednesday";
                        semspread.Sheets[0].RowHeader.Cells[2, 0].Tag = "wed";
                        semspread.Sheets[0].RowHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (nodays >= 4)
                    {
                        semspread.Sheets[0].RowHeader.Cells[3, 0].Text = "Thursday";
                        semspread.Sheets[0].RowHeader.Cells[3, 0].Tag = "thu";
                        semspread.Sheets[0].RowHeader.Cells[3, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (nodays >= 5)
                    {
                        semspread.Sheets[0].RowHeader.Cells[4, 0].Text = "Friday";
                        semspread.Sheets[0].RowHeader.Cells[4, 0].Tag = "fri";
                        semspread.Sheets[0].RowHeader.Cells[4, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (nodays >= 6)
                    {
                        semspread.Sheets[0].RowHeader.Cells[5, 0].Text = "Saturday";
                        semspread.Sheets[0].RowHeader.Cells[5, 0].Text = "sat";
                        semspread.Sheets[0].RowHeader.Cells[5, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                }
                else
                {
                    todate = SpdInfo.Sheets[0].ColumnHeader.Cells[1, spdinfo_ac].Note;
                    //srt_day = findday(todate.ToString(), dept_fmdate.ToString(), nodays.ToString(), start_dayorder.ToString());  //findday(nodays, dept_fmdate.ToString(), todate.ToString());
                    //Modifeid by Srinath 5/9/2014
                    string[] sps = todate.ToString().Split('/');
                    string curdate = sps[1] + '/' + sps[0] + '/' + sps[2];
                    srt_day = dacess.findday(curdate.ToString(), ddlbranch.SelectedValue.ToString(), ddlduration.SelectedValue.ToString(), ddlbatch.Text.ToString(), dept_fmdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                    for (order = 1; order <= nodays; order++)
                    {
                        semspread.Sheets[0].RowCount = semspread.Sheets[0].RowCount + 1;
                        semspread.Sheets[0].RowHeader.Cells[order - 1, 0].Text = "Dayorder" + order;
                        semspread.Sheets[0].RowHeader.Cells[order - 1, 0].Tag = srt_day;
                    }
                }
            }
            string[] daylist = { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            //dar_con.Close();
            //dar_con.Open();
            //SqlDataReader day_dr;
            //cmd = new SqlCommand("select top 1 * from semester_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate<= ' " + datefrom.ToString() + " ' " + strsec + " order by fromdate desc", dar_con);
            //day_dr = cmd.ExecuteReader();

            string qrySection = string.Empty;
            string qrySection1 = string.Empty;
            if (ddlsec.Items.Count > 0)
            {
                string sectionName = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(sectionName) && sectionName.Trim().ToLower() != "all" && sectionName.Trim().ToLower() != "-1")
                {
                    qrySection = " and ct.TT_sec='" + Convert.ToString(sectionName).Trim() + "'";
                    qrySection1 = " and nct.TT_sec='" + Convert.ToString(sectionName).Trim() + "'";
                }
            }

            DataTable dtSelect = dirAcc.selectDataTable("select TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room,ct.TT_ClassPK from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection + " and ct.TT_date=(select MAX(nct.TT_date) from TT_ClassTimetable nct where nct.TT_colCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and nct.TT_degCode='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and nct.TT_batchyear='" + ddlbatch.SelectedValue + "' and nct.TT_sem='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' " + qrySection1 + ") ");//and TT_date>='" + datefrom + "' and TT_date<='" + dateto + "'
            DataTable dtSchedule = new DataTable();
            dtSchedule.Columns.Add("TT_ClassPK");
            DataRow drSchedule = dtSchedule.NewRow();
            for (int i = 0; i < 6; i++)
            {
                string curday = string.Empty;
                string curdayFull = string.Empty;
                switch (i)
                {
                    case 0:
                        curdayFull = "Monday";
                        curday = "mon";
                        break;
                    case 1:
                        curdayFull = "Tuesday";
                        curday = "tue";
                        break;
                    case 2:
                        curdayFull = "Wednesday";
                        curday = "wed";
                        break;
                    case 3:
                        curdayFull = "Thursday";
                        curday = "thu";
                        break;
                    case 4:
                        curdayFull = "Friday";
                        curday = "fri";
                        break;
                    case 5:
                        curdayFull = "Saturday";
                        curday = "sat";
                        break;
                }
                for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                {
                    if (!dtSchedule.Columns.Contains(curday + hrsI))
                        dtSchedule.Columns.Add(curday + hrsI);

                    dtSelect.DefaultView.RowFilter = "Daydiscription='" + curdayFull + "' and TT_Hour='" + hrsI + "'";
                    DataTable dtFilter = dtSelect.DefaultView.ToTable();
                    if (dtFilter.Rows.Count > 0)
                    {
                        drSchedule["TT_ClassPK"] = Convert.ToString(dtFilter.Rows[0]["TT_ClassPK"]);
                        StringBuilder sbNew = new StringBuilder();
                        for (int dtI = 0; dtI < dtFilter.Rows.Count; dtI++)
                        {
                            string subno = Convert.ToString(dtFilter.Rows[dtI]["TT_subno"]);
                            string stfcode = Convert.ToString(dtFilter.Rows[dtI]["TT_staffcode"]);
                            string elect = Convert.ToString(dtFilter.Rows[dtI]["Elective"]);
                            string Lab = Convert.ToString(dtFilter.Rows[dtI]["Lab"]);
                            string practicalpair = Convert.ToString(dtFilter.Rows[dtI]["practicalpair"]);
                            string differenciator = "S";
                            string room = Convert.ToString(dtFilter.Rows[dtI]["room"]);
                            if (elect.ToLower() == "true")
                            {
                                differenciator = "E";
                            }
                            else if (Lab.ToLower() == "true")
                            {
                                differenciator = "L";
                            }
                            else if (practicalpair != "0")
                            {
                                differenciator = "C";
                            }

                            sbNew.Append(subno + "-" + stfcode + "-" + differenciator + ";");
                        }
                        drSchedule[curday + hrsI] = sbNew.ToString().Trim(';');
                    }
                }
            }
            dtSchedule.Rows.Add(drSchedule);

            //while (day_dr.Read())
            if (dtSchedule.Rows.Count > 0)
            {
                //if (day_dr.HasRows == true)
                foreach (DataRow day_dr in dtSchedule.Rows)
                {
                    semspread.Sheets[0].ColumnCount = intNHrs;
                    for (day_list = 0; day_list < nodays; day_list++)
                    {
                        for (insert_val = 1; insert_val <= intNHrs; insert_val++)
                        {
                            semspread.Sheets[0].ColumnHeader.Cells[0, insert_val - 1].Text = "Period" + insert_val.ToString();
                            acro = string.Empty;
                            shed_list = string.Empty;
                            day_order = daylist[day_list] + insert_val.ToString();
                            sunjno_staffno = day_dr[day_order].ToString();
                            //---------------getupper bound for many subject
                            string[] many_subj = sunjno_staffno.Split(new Char[] { ';' });
                            for (ind_subj = 0; ind_subj <= many_subj.GetUpperBound(0); ind_subj++)
                            {
                                if (many_subj.GetUpperBound(0) >= 0)
                                {
                                    sunjno_staffno_s = many_subj[ind_subj];
                                    if (sunjno_staffno_s.Trim() != "")
                                    {
                                        //---------------------------
                                        string[] subjno_staffno_splt = sunjno_staffno_s.Split(new Char[] { '-' });
                                        subj_no = Convert.ToInt32(subjno_staffno_splt[0].ToString());
                                        //---------tag
                                        SqlDataReader sub_dr;
                                        SqlCommand sub_cmd;
                                        con2a.Close();
                                        con2a.Open();
                                        sub_cmd = new SqlCommand("select subject_name from subject where subject_no=" + subj_no.ToString() + "", con2a);
                                        sub_dr = sub_cmd.ExecuteReader();
                                        sub_dr.Read();
                                        if (sub_dr.HasRows == true)//This line added by Manikandan
                                        {
                                            alt_sched = sub_dr[0].ToString() + "-" + subjno_staffno_splt[1].ToString() + "-" + subjno_staffno_splt[2].ToString();
                                        }
                                        //------------------
                                        cona.Close();
                                        cona.Open();
                                        acronym_val = "select isnull(acronym,subject_code) acronym from subject where subject_no=" + subj_no.ToString() + " ";
                                        SqlCommand ac_cmd = new SqlCommand(acronym_val, cona);
                                        SqlDataReader ac_dr;
                                        ac_dr = ac_cmd.ExecuteReader();
                                        ac_dr.Read();
                                        if (ac_dr.HasRows == true)
                                        {
                                            acronym = ac_dr["acronym"].ToString();
                                            if (acro == "")
                                            {
                                                acro = acro + acronym;
                                            }
                                            else
                                            {
                                                acro = acro + "," + acronym;
                                            }
                                            if (shed_list == "")
                                            {
                                                shed_list = shed_list + alt_sched;
                                            }
                                            else
                                            {
                                                shed_list = shed_list + ";" + alt_sched;
                                            }
                                        }
                                    }
                                }
                            }
                            semspread.Sheets[0].Cells[day_list, insert_val - 1].Text = acro.ToString();
                            semspread.Sheets[0].Cells[day_list, insert_val - 1].Font.Name = "Book Antiqua";
                            semspread.Sheets[0].Cells[day_list, insert_val - 1].Font.Size = FontUnit.Medium;
                            semspread.Sheets[0].Cells[day_list, insert_val - 1].Tag = shed_list;
                            semspread.Sheets[0].Cells[day_list, insert_val - 1].Note = sunjno_staffno;
                        }
                    }
                    semspread.SaveChanges();
                }
            }
        }
        catch (Exception ex)
        {
            lblnoofalter.Visible = true;
            lblnoofalter.Text = ex.ToString();
        }
    }

    protected void semspread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            SpdInfo.Visible = true;
            batchbtn.Visible = true;
            btnprintmaster.Visible = true;
            Printcontrol.Visible = false;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            semclick = true;
            // Panel3.Visible = false;
            batchbtn.Visible = true; ;
            treepanel.Visible = false;
            btnsave.Enabled = true;
            btnsave.Visible = true;
        }
        catch
        {
        }
    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        Response.Redirect("~/ScheduleMOD/NewAlterBatchallocation.aspx");
    }

    protected void btn_remove_Click(object sender, EventArgs e)
    {
        try
        {
            int ar = 0, ac = 0;
            ar = SpdInfo.Sheets[0].ActiveRow;
            ac = SpdInfo.Sheets[0].ActiveColumn;
            int actCell = 0;
            string getheaderval = SpdInfo.Sheets[0].ColumnHeader.Cells[1, ac].Text.Trim();
            if (getheaderval == "Schedule List")
            {
                SpdInfo.Sheets[0].Cells[ar, ac + 1].Text = string.Empty;
                SpdInfo.Sheets[0].Cells[ar, ac + 1].Note = string.Empty;
                SpdInfo.Sheets[0].Cells[ar, ac + 1].Tag = string.Empty;
                actCell = ac + 1;
            }
            else
            {
                SpdInfo.Sheets[0].Cells[ar, ac].Text = string.Empty;
                SpdInfo.Sheets[0].Cells[ar, ac].Note = string.Empty;
                SpdInfo.Sheets[0].Cells[ar, ac].Tag = string.Empty;
                actCell = ac;
            }
            string day_value = "", srt_day = "", dt1 = "", dt2 = "", strsec = string.Empty;
            if (ddlsec.Text.ToString() == "" || ddlsec.Text.ToString() == "-1")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + ddlsec.Text.ToString() + "'";
            }
            //dt2 = SpdInfo.Sheets[0].ColumnHeader.Cells[0, ac-1].Text;
            dt2 = SpdInfo.Sheets[0].ColumnHeader.Cells[0, ac].Text;//this line modified by Manikandan from above line
            if (dt2 == "")
            {
                dt2 = SpdInfo.Sheets[0].ColumnHeader.Cells[0, ac - 1].Note;
            }
            string[] dt1_split = dt2.Split('/');
            dt1 = dt1_split[1] + "-" + dt1_split[0] + "-" + dt1_split[2];
            DateTime startdate = Convert.ToDateTime(GetFunction("select start_date from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " "));
            //===========Added by Manikandan 23/09/2013=============
            start_dayorder = GetFunction("select starting_dayorder from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " ");
            //========================End===========================
            if (startdate.ToString() != "" && startdate.ToString() != null)
            {
                day_value = startdate.ToString("ddd");
            }
            con.Close();
            con.Open();
            SqlDataReader dr;
            cmd = new SqlCommand("Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + "", con);
            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows == true)
            {
                if ((dr["No_of_hrs_per_day"].ToString()) != "")
                {
                    intNHrs = Convert.ToInt32(dr["No_of_hrs_per_day"]);
                    SchOrder = Convert.ToInt32(dr["schorder"]);
                    nodays = Convert.ToInt32(dr["nodays"]);
                }
            }
            if (intNHrs > 0)
            {
                if (SchOrder != 0)
                {
                    srt_day = (Convert.ToDateTime(dt1)).ToString("ddd");
                }
                else
                {
                    //todate = SpdInfo.Sheets[0].ColumnHeader.Cells[0, 0].Text;
                    //srt_day = findday(dt2.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());  //findday(nodays, startdate.ToString(), dt2) + (ar + 1);
                    //Modifeid by Srinath 5/9/2014
                    string[] sps = dt2.ToString().Split('/');
                    string curdate = sps[1] + '/' + sps[0] + '/' + sps[2];
                    srt_day = dacess.findday(curdate.ToString(), ddlbranch.SelectedValue.ToString(), ddlduration.SelectedValue.ToString(), ddlbatch.Text.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                }
            }
            btn_remove.Visible = false;
            string[] Days = new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            Dictionary<string, byte> dicDayOrder = getDayOrder();
            if (srt_day != "")
            {
                string altersech = string.Empty;
                string nohr = SpdInfo.Sheets[0].ColumnHeader.Cells[1, actCell].Text;

                string[] spilthr = nohr.Split(' ');
                int nooftime = Convert.ToInt32(spilthr[2].ToString());
                string daycolumn = srt_day + ac;
                int startcol = ac - nooftime + 1;
                int endcol = startcol + Convert.ToInt32(nooftime);
                string dayclu = srt_day;
                int hr = 0;
                int hrvalue = ar + 1;
                for (int i = startcol; i <= endcol; i++)
                {
                    hr++;
                    if (i < SpdInfo.Sheets[0].ColumnCount)
                    {
                        string value = SpdInfo.Sheets[0].Cells[ar, i].Text;
                        string note = SpdInfo.Sheets[0].Cells[ar, i].Note;
                        if (value.Trim() != "" && note.Trim() != "")
                        {
                            altersech = note;
                            dayclu = "" + srt_day + hrvalue + "='" + note + "'";
                        }
                        else
                        {
                            dayclu = "" + srt_day + hrvalue + "=''";
                        }
                        string set = dacess.GetFunction("select COUNT(*) from tbl_alter_schedule_Details where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedItem.ToString() + " " + strsec + " and semester=" + ddlduration.SelectedItem.ToString() + " and fromdate='" + dt1 + "' and No_of_Alter=" + hr + " ");
                        if (set.Trim() != "" && set.Trim() != null && set.Trim() != "0")
                        {
                            string alterquery = "update tbl_alter_schedule_Details set " + dayclu + " where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedItem.ToString() + " " + strsec + " and semester=" + ddlduration.SelectedItem.ToString() + " and fromdate='" + dt1 + "' and No_of_Alter=" + hr + "";
                            int a = dacess.update_method_wo_parameter(alterquery, "Text");
                        }
                        else
                        {
                            string secvalue = string.Empty;
                            if (ddlsec.Text.ToString() == "" || ddlsec.Text.ToString() == "-1")
                            {
                                secvalue = string.Empty;
                            }
                            else
                            {
                                secvalue = "'" + ddlsec.Text.ToString() + "'";
                            }
                            string alterquery = "insert into  tbl_alter_schedule_Details(batch_year,degree_code,semester,sections,fromdate,No_of_Alter," + srt_day + hrvalue + ") values(" + ddlbatch.SelectedValue.ToString() + "," + ddlbranch.SelectedValue.ToString() + " ," + ddlduration.SelectedItem.ToString() + " ," + secvalue + ",'" + dt1 + "' ," + hr + ",'" + note + "')";
                            int a = dacess.update_method_wo_parameter(alterquery, "Text");
                        }
                    }
                    SqlDataReader dr_update;
                    con.Close();
                    con.Open();
                    cmd = new SqlCommand("update alternate_schedule set " + srt_day + (ar + 1) + "='" + altersech + "' where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedItem.ToString() + " " + strsec + " and semester=" + ddlduration.SelectedItem.ToString() + " and fromdate='" + dt1 + "' ", con);
                    dr_update = cmd.ExecuteReader();
                    string TTclassPk = Convert.ToString(SpdInfo.Sheets[0].Cells[ar, actCell - 1].Tag).Trim();
                    string dayofweek = Days[Convert.ToInt32(GetWeakDay(srt_day))];
                    string daypk = dicDayOrder[dayofweek].ToString();
                    string delAlterTT = " delete from TT_AlterTimetableDet where  TT_AlterDate='" + dt1 + "' and TT_ClassFk='" + TTclassPk + "' and TT_Hour ='" + (ar + 1) + "' and TT_Day='" + daypk + "' and TT_ClassFK='" + TTclassPk + "'";
                    int delTT = dirAcc.insertData(delAlterTT);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Removed successfully')", true);
                }
            }
        }
        catch
        {
        }
    }

    protected void chk_multisubj_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_multisubj.Checked == true)// && chklistmultisubj .Items.Count>0)
            {
                //txtmultisubj.Visible = true;
                //pnlmultisubj.Visible = true;
                string staff_name_code = string.Empty;
                staff_name_code = FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].ActiveRow), 1].Value.ToString();
                string[] staff_name_code_spt = staff_name_code.Split(';');
                for (int many_staff = 0; many_staff <= staff_name_code_spt.GetUpperBound(0); many_staff++)
                {
                    chklistmultisubj.Items.Add(staff_name_code_spt[many_staff]);
                }
            }
            else
            {
                txtmultisubj.Visible = false;
                pnlmultisubj.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void chklistmultisubj_selectedindetxchange(object sender, EventArgs e)
    {
        try
        {
            int cnt = 0;
            for (int chk_cnt = 0; chk_cnt < chklistmultisubj.Items.Count; chk_cnt++)
            {
                if (chklistmultisubj.Items[chk_cnt].Selected == true)
                {
                    cnt++;
                }
            }
            txtmultisubj.Text = cnt + " Staff(s)";
        }
        catch
        {
        }
    }

    protected void FpSpread1_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellclick1 = true;
    }

    protected void FpSpread1_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (cellclick1 == true)
            {
                chklistmultisubj.Items.Clear();
                string staff_name_code = string.Empty;
                if (FpSpread1.Sheets[0].ActiveColumn == 0)
                {
                    if (chk_multisubj.Checked == true)
                    {
                        //txtmultisubj.Visible = true;
                        //pnlmultisubj.Visible = true;
                    }
                    else
                    {
                        txtmultisubj.Visible = false;
                        pnlmultisubj.Visible = false;
                    }
                    staff_name_code = FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].ActiveRow), 1].Value.ToString();
                    string[] staff_name_code_spt = staff_name_code.Split(';');
                    for (int many_staff = 0; many_staff <= staff_name_code_spt.GetUpperBound(0); many_staff++)
                    {
                        chklistmultisubj.Items.Add(staff_name_code_spt[many_staff]);
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {
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

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            //Modified by Srinath 27/2/2013
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                dacess.printexcelreport(SpdInfo, reportname);
            }
            else
            {
                norecordlbl.Text = "Please Enter Your Report Name";
                norecordlbl.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            norecordlbl.Visible = false;
            if (SpdInfo.Visible == true)
            {
                Session["column_header_row_count"] = 2;
                string sections = string.Empty;
                if (ddlsec.Text.ToString() == "" || ddlsec.Text.ToString() == "-1")
                {
                    sections = string.Empty;
                }
                else
                {
                    sections = " @ sections=" + ddlsec.Text.ToString() + "";
                }
                string degreedetails = "Alternate Schedule Change @ Batch : " + ddlbatch.SelectedValue.ToString() + " @ Degree : " + ddldegree.SelectedItem.ToString() + " @ Branch : " + ddlbranch.SelectedItem.ToString() + " @ Sem : " + ddlduration.SelectedItem.ToString() + " " + sections + " @ Date : " + txtFromDate.Text + " to " + txtToDate.Text + "";
                string pagename = "Alternatesched.aspx";
                Printcontrol.loadspreaddetails(SpdInfo, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            else
            {
                norecordlbl.Visible = true;
                norecordlbl.Text = "Please Click Go Button Before Print";
            }
        }
        catch
        {
        }
    }

    protected void chkmulstaff_ChekedChange(object sender, EventArgs e)
    {
        txtmulstaff.Text = "---Select---";
        if (chkmulstaff.Checked == true)
        {
            if (chkmullsstaff.Items.Count > 0)
            {
                for (int i = 0; i < chkmullsstaff.Items.Count; i++)
                {
                    chkmullsstaff.Items[i].Selected = true;
                }
                txtmulstaff.Text = "Staff (" + chkmullsstaff.Items.Count + ")";
            }
        }
        else
        {
            for (int i = 0; i < chkmullsstaff.Items.Count; i++)
            {
                chkmullsstaff.Items[i].Selected = false;
            }
        }
    }

    protected void chkmullsstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtmulstaff.Text = "---Select---";
        chkmulstaff.Checked = false;
        int cou = 0;
        for (int i = 0; i < chkmullsstaff.Items.Count; i++)
        {
            if (chkmullsstaff.Items[i].Selected == true)
            {
                cou++;
            }
        }
        if (cou > 0)
        {
            txtmulstaff.Text = "Staff (" + cou + ")";
            if (chkmullsstaff.Items.Count == cou)
            {
                chkmulstaff.Checked = true;
            }
        }
    }

    protected void btnmulstaff_Click(object sender, EventArgs e)
    {
        try
        {
            string strsec = string.Empty;
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.ToString() != "" && ddlsec.SelectedItem.ToString() != "-1" && ddlsec.SelectedItem.ToString() != null)
                {
                    strsec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                }
            }
            string strbatchyear = ddlbatch.Text.ToString();
            string strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlduration.SelectedValue.ToString();
            int activerow = FpSpread1.Sheets[0].RowCount - 1;
            if (activerow != -1)
            {
                int rowval = Convert.ToInt32(activerow);
                if (chkmullsstaff.Items.Count > 0)
                {
                    string stafftext = string.Empty;
                    string stafftag = string.Empty;
                    for (int i = 0; i < chkmullsstaff.Items.Count; i++)
                    {
                        if (chkmullsstaff.Items[i].Selected == true)
                        {
                            string stte = chkmullsstaff.Items[i].Text.ToString();
                            string[] stcode = stte.Split('-');
                            if (stafftext == "")
                            {
                                stafftext = chkmullsstaff.Items[i].Text.ToString();
                                stafftag = stcode[stcode.GetUpperBound(0)].ToString();
                            }
                            else
                            {
                                stafftext = stafftext + "-" + chkmullsstaff.Items[i].Text.ToString();
                                stafftag = stafftag + '-' + stcode[stcode.GetUpperBound(0)].ToString();
                            }
                        }
                    }
                    int staf_cnt = 0;
                    string staff_code = string.Empty;
                    string staff_name_code = string.Empty;
                    int parent_count = subjtree.Nodes.Count;//----------count parent node value
                    for (int i = 0; i < parent_count; i++)
                    {
                        for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                        {
                            if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                            {
                                FpSpread1.Visible = true;
                                subjtree.Visible = true;
                                chkappend.Visible = true;
                                btnOk.Visible = true;
                                treepanel.Visible = true;
                                FpSpread1.Sheets[0].SetText(rowval, 0, subjtree.Nodes[i].ChildNodes[node_count].Text);
                                FpSpread1.Sheets[0].Cells[rowval, 0].Tag = subjtree.Nodes[i].ChildNodes[node_count].Value;
                                string chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;
                                FpSpread1.Sheets[0].Rows[rowval].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Rows[rowval].Font.Size = FontUnit.Medium;
                                DataSet staf_set = dacess.select_method("select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year=" + strbatchyear.ToString() + " " + strsec + ")", hat, "Text");

                                //DataSet staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "' and ct.TT_sec='" + ddlsec.SelectedItem.ToString() + "'  and TT_date>='" + (txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[2]) + "')");
                                //if (true)
                                //{
                                //    staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "' ) ");
                                //}
                                if (staf_set.Tables.Count > 0 && staf_set.Tables[0].Rows.Count > 1)
                                {
                                    txtmulstaff.Visible = true;
                                    lblmulstaff.Visible = true;
                                    string[] staff_list = new string[staf_set.Tables[0].Rows.Count + 2];
                                    for (staf_cnt = 0; staf_cnt < staf_set.Tables[0].Rows.Count; staf_cnt++)
                                    {
                                        staff_list[staf_cnt] = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        if (staff_code == "")
                                        {
                                            staff_code = staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                            staff_name_code = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        }
                                        else
                                        {
                                            staff_code = staff_code + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                            staff_name_code = staff_name_code + ";" + staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        }
                                    }
                                    if (staff_list.GetUpperBound(0) > 0)
                                    {
                                        staff_list[staf_cnt] = stafftext;
                                        staff_list[staf_cnt + 1] = "All";
                                    }
                                    FarPoint.Web.Spread.ComboBoxCellType staf_combo = new FarPoint.Web.Spread.ComboBoxCellType(staff_list);
                                    staf_combo.AutoPostBack = true;
                                    FpSpread1.Sheets[0].Cells[rowval, 1].CellType = staf_combo;
                                    FpSpread1.Sheets[0].Cells[rowval, 1].Locked = false;
                                }
                                FpSpread1.Sheets[0].Cells[rowval, 1].Text = stafftext;
                                FpSpread1.Sheets[0].Cells[rowval, 1].Tag = stafftag;
                                treepanel.Visible = true;
                            }
                            FpSpread1.SaveChanges();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void batchbtn_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Batch_ReDir"] = "FromAlternateSched";
            Response.Redirect("~/ScheduleMOD/NewAlterBatchallocation.aspx");
        }
        catch { }
    }

    protected bool getAlternateScheduleCheck(string staffcode, string day, int col, string fromdate, ref string alertStr, string stfName)
    {
        bool checkbool = false;
        try
        {
            string[] staffcode_check = staffcode.Split('-');
            string staffname = d2.GetFunction("select s.staff_name from staffmaster s,staff_appl_master sm where s.appl_no=sm.appl_no and s.staff_code='" + staffcode_check[1] + "'");
            string tablevalue = string.Empty;
            if (day == "Mon")
                tablevalue = "mon" + col + "";
            else if (day == "Tue")
                tablevalue = "tue" + col + "";
            else if (day == "Wed")
                tablevalue = "wed" + col + "";
            else if (day == "Thu")
                tablevalue = "thu" + col + "";
            else if (day == "Fri")
                tablevalue = "fri" + col + "";
            else if (day == "Sat")
                tablevalue = "sat" + col + "";
            string SqlFinal = string.Empty;
            string history_data = string.Empty;
            for (int i = 0; i <= staffcode_check.Length - 1; i++)
            {
                string staff_code = staffcode_check[i].ToString();
                Hashtable hatdegree = new Hashtable();
                SqlFinal = " select cc.Course_Name, de.Acronym, r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from staff_selector ss,Registration r,";
                SqlFinal = SqlFinal + " subject s,sub_sem sm,syllabus_master sy,seminfo si,Degree de,COURSE cc where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
                SqlFinal = SqlFinal + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
                SqlFinal = SqlFinal + " and s.subject_no=ss.subject_no and r.sections=ss.sections and ss.batch_year=r.Batch_Year";
                SqlFinal = SqlFinal + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
                SqlFinal = SqlFinal + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
                SqlFinal = SqlFinal + " and r.DelFlag=0 and ss.staff_code='" + staff_code + "' and de.Degree_Code=si.degree_code and de.Course_Id=cc.Course_Id and '" + fromdate + "' between si.start_date and si.end_date";
                // srids.Clear();
                DataSet srids = d2.select_method_wo_parameter(SqlFinal, "Text");
                for (int j = 0; j < srids.Tables[0].Rows.Count; j++)
                {
                    string btch = srids.Tables[0].Rows[j]["batch_year"].ToString();
                    string dgre = srids.Tables[0].Rows[j]["degree_code"].ToString();
                    string ster = srids.Tables[0].Rows[j]["semester"].ToString();
                    string sctn = srids.Tables[0].Rows[j]["Sections"].ToString();
                    string acrnym = srids.Tables[0].Rows[j]["Acronym"].ToString();
                    string coursename = srids.Tables[0].Rows[j]["Course_Name"].ToString();
                    if (!hatdegree.ContainsKey(btch + '-' + dgre + '-' + ster + '-' + sctn))
                    {
                        hatdegree.Add(btch + '-' + dgre + '-' + ster + '-' + sctn, btch + '-' + dgre + '-' + ster + '-' + sctn);
                        string slq = "select top 1 * from Semester_Schedule where batch_year='" + btch + "' and semester ='" + ster + "' and degree_code='" + dgre + "' and Sections='" + sctn + "' and FromDate <= '" + fromdate + "' ORDER BY FromDate desc";
                        string rept = string.Empty;
                        // ds.Clear();
                        DataSet ds = d2.select_method_wo_parameter(slq, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            string strsetval = "" + tablevalue + " like '%" + staff_code + "%'";
                            ds.Tables[0].DefaultView.RowFilter = strsetval;
                            DataView dvfils = ds.Tables[0].DefaultView;
                            if (dvfils.Count > 0)
                            {
                                if (history_data == "")
                                {
                                    if (ster == "1")
                                    {
                                        history_data = btch + "-" + coursename + "-" + acrnym + "-" + ster + "st Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                    if (ster == "2")
                                    {
                                        history_data = btch + "-" + coursename + "-" + acrnym + "-" + ster + "nd Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                    if (ster == "3")
                                    {
                                        history_data = btch + "-" + coursename + "-" + acrnym + "-" + ster + "rd Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                    else
                                    {
                                        history_data = btch + "-" + coursename + "-" + acrnym + "-" + ster + "th Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                }
                                else
                                {
                                    if (ster == "1")
                                    {
                                        history_data = history_data + " ; " + btch + "-" + coursename + "-" + acrnym + "-" + ster + "st Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                    else if (ster == "2")
                                    {
                                        history_data = history_data + " ; " + btch + "-" + coursename + "-" + acrnym + "-" + ster + "nd Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "-Sec";
                                        }
                                    }
                                    else if (ster == "3")
                                    {
                                        history_data = history_data + " ; " + btch + "-" + coursename + "-" + acrnym + "-" + ster + "rd Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                    else
                                    {
                                        history_data = history_data + " ; " + btch + "-" + coursename + "-" + acrnym + "-" + ster + "th Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = "-" + history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (history_data != "")
            {
                string getRights = d2.GetFunction("select value from Master_Settings where  settings='Time Table Alert Rights'");
                if (getRights.Trim() == "0" || String.IsNullOrEmpty(getRights))
                {
                    checkbool = true;
                    alertStr = "The Staff " + staffname + " is BUSY in " + history_data + " - Do you want to Schedule the Class Anyway?";
                }
            }
        }
        catch { }
        return checkbool;
    }

    protected void bt_closedalter_Clik(object sender, EventArgs e)
    {
        btnGo_Click(sender, e);
        Div3.Visible = false;
    }

    protected void btnOKsave_Clik(object sender, EventArgs e)
    {
        Div3.Visible = false;
        Save();
    }

    private string GetWeakDay(string weakDayName)
    {
        string weakDayValue = string.Empty;
        weakDayName = weakDayName.Trim().ToLower();
        try
        {
            switch (weakDayName)
            {
                case "sun":
                    weakDayValue = "0";
                    break;
                case "mon":
                    weakDayValue = "1";
                    break;
                case "tue":
                    weakDayValue = "2";
                    break;
                case "wed":
                    weakDayValue = "3";
                    break;
                case "thu":
                    weakDayValue = "4";
                    break;
                case "fri":
                    weakDayValue = "5";
                    break;
                case "sat":
                    weakDayValue = "6";
                    break;
            }
        }
        catch
        {
        }
        return weakDayValue;
    }

    //Get day values for access from time table Added by Idhris
    private Dictionary<string, byte> getDayOrder()
    {
        Dictionary<string, byte> dicDayOrder = new Dictionary<string, byte>();
        try
        {
            DataTable dtDayOrder = dirAcc.selectDataTable("select TT_Day_DayorderPK,Daydiscription from TT_Day_Dayorder");
            if (dtDayOrder.Rows.Count > 0)
            {
                foreach (DataRow drDayOrder in dtDayOrder.Rows)
                {
                    dicDayOrder.Add(Convert.ToString(drDayOrder["Daydiscription"]), Convert.ToByte(drDayOrder["TT_Day_DayorderPK"]));
                }
            }
        }
        catch { dicDayOrder.Clear(); }
        return dicDayOrder;
    }

    ///// <summary>
    ///// To Get Semester Schedule and Aleternate Schedule From New To Old Table Structue
    ///// Developed By Malang Raja T
    ///// </summary> 
    ///// <param name="Collegecode">CollegeCode</param>
    ///// <param name="Batchyear">BatchYear</param>
    ///// <param name="Degreecode">DegreeCode</param>
    ///// <param name="Semester">Semester</param>
    ///// <param name="Section">Section Name</param>
    ///// <param name="SemStartdate">From Date in the Format MM/dd/yyyy</param>
    ///// <param name="SemEnddate">To Date in the Format MM/dd/yyyy</param>
    ///// <param name="intNHrs">Total No Of Hours for this Class</param>
    ///// <param name="dtSchedule">Semester Schedule </param>
    ///// <param name="dtAlterSchedule">Aleternate Schedule</param>
    //protected void SemesterandAlternateSchedule(string Collegecode, string Batchyear, string Degreecode, string Semester, string Section, string SemStartdate, string SemEnddate, int intNHrs, ref DataTable dtSchedule, ref DataTable dtAlterSchedule)
    //{
    //    try
    //    {
    //        string qrySection = string.Empty;
    //        if (Section.Trim() != "")
    //            qrySection = " and isnull(ct.TT_sec,'')='" + Section + "'";
    //        DataTable dtSelect = dirAcc.selectDataTable("select CONVERT(varchar(20),ct.TT_date,103) as TT_date,TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room,ct.TT_ClassPK from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Collegecode.Trim() + "' and ct.TT_degCode='" + Convert.ToString(Degreecode).Trim() + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Convert.ToString(Semester).Trim() + "' " + qrySection + " and ct.TT_date<='" + SemEnddate + "' order by  ct.TT_date desc");
    //        dtSchedule = new DataTable();
    //        dtSchedule.Columns.Add("TT_ClassPK");
    //        dtSchedule.Columns.Add("FromDate");
    //        dtSchedule.Columns.Add("TTDate");
    //        DataTable dtMinMaxSchedule = new DataTable();
    //        dtMinMaxSchedule = dirAcc.selectDataTable("select distinct CONVERT(varchar(20),ct.TT_date,103) as TT_date,ct.TT_date TTDate from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Collegecode + "' and ct.TT_degCode='" + Degreecode + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Semester + "'  " + qrySection + " and ct.TT_date<='" + SemEnddate + "' order by  TTDate desc");//CONVERT(varchar(20),min(ct.TT_date),103) as FromDate,CONVERT(varchar(20),max(ct.TT_date),103) as ToDate
    //        if (dtMinMaxSchedule.Rows.Count > 0)
    //        {
    //            DataRow drSchedule;//= dtSchedule.NewRow();
    //            //DateTime dtMinDate = new DateTime();
    //            //DateTime dtMaxDate = new DateTime();
    //            DateTime dtTTDate = new DateTime();
    //            for (int ttRow = 0; ttRow < dtMinMaxSchedule.Rows.Count; ttRow++)
    //            {
    //                if (Convert.ToString(dtMinMaxSchedule.Rows[ttRow]["TT_date"]).Trim() != "" && Convert.ToString(dtMinMaxSchedule.Rows[ttRow]["TTDate"]).Trim() != "")
    //                {
    //                    DateTime.TryParseExact(Convert.ToString(dtMinMaxSchedule.Rows[ttRow]["TT_date"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtTTDate);
    //                    drSchedule = dtSchedule.NewRow();
    //                    drSchedule["FromDate"] = Convert.ToString(dtTTDate.ToString("MM/dd/yyyy"));
    //                    drSchedule["TTDate"] = dtTTDate;
    //                    for (int i = 0; i < 7; i++)
    //                    {
    //                        string curday = string.Empty;
    //                        string curdayFull = string.Empty;
    //                        switch (i)
    //                        {
    //                            case 0:
    //                                curdayFull = "Monday";
    //                                curday = "mon";
    //                                break;
    //                            case 1:
    //                                curdayFull = "Tuesday";
    //                                curday = "tue";
    //                                break;
    //                            case 2:
    //                                curdayFull = "Wednesday";
    //                                curday = "wed";
    //                                break;
    //                            case 3:
    //                                curdayFull = "Thursday";
    //                                curday = "thu";
    //                                break;
    //                            case 4:
    //                                curdayFull = "Friday";
    //                                curday = "fri";
    //                                break;
    //                            case 5:
    //                                curdayFull = "Saturday";
    //                                curday = "sat";
    //                                break;
    //                            case 6:
    //                                curdayFull = "Sunday";
    //                                curday = "sun";
    //                                break;
    //                        }
    //                        for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
    //                        {
    //                            if (!dtSchedule.Columns.Contains(curday + hrsI))
    //                                dtSchedule.Columns.Add(curday + hrsI);
    //                            dtSelect.DefaultView.RowFilter = "Daydiscription='" + curdayFull + "' and TT_Hour='" + hrsI + "' and TT_date='" + Convert.ToString(dtTTDate.ToString("dd/MM/yyyy")) + "'";
    //                            DataTable dtFilter = dtSelect.DefaultView.ToTable();
    //                            if (dtFilter.Rows.Count > 0)
    //                            {
    //                                drSchedule["TT_ClassPK"] = Convert.ToString(dtFilter.Rows[0]["TT_ClassPK"]);
    //                                StringBuilder sbNew = new StringBuilder();
    //                                for (int dtI = 0; dtI < dtFilter.Rows.Count; dtI++)
    //                                {
    //                                    string subno = Convert.ToString(dtFilter.Rows[dtI]["TT_subno"]);
    //                                    string stfcode = Convert.ToString(dtFilter.Rows[dtI]["TT_staffcode"]);
    //                                    string elect = Convert.ToString(dtFilter.Rows[dtI]["Elective"]);
    //                                    string Lab = Convert.ToString(dtFilter.Rows[dtI]["Lab"]);
    //                                    string practicalpair = Convert.ToString(dtFilter.Rows[dtI]["practicalpair"]);
    //                                    string differenciator = "S";
    //                                    string room = Convert.ToString(dtFilter.Rows[dtI]["room"]);
    //                                    if (elect.ToLower() == "true")
    //                                    {
    //                                        differenciator = "E";
    //                                    }
    //                                    else if (Lab.ToLower() == "true")
    //                                    {
    //                                        differenciator = "L";
    //                                    }
    //                                    else if (practicalpair != "0")
    //                                    {
    //                                        differenciator = "C";
    //                                    }

    //                                    sbNew.Append(subno + "-" + stfcode + "-" + differenciator + "-" + room + ";");
    //                                }
    //                                drSchedule[curday + hrsI] = sbNew.ToString();
    //                            }
    //                        }
    //                    }
    //                    dtSchedule.Rows.Add(drSchedule);
    //                }
    //            }
    //        }
    //        DataTable dtAlterSelect = dirAcc.selectDataTable("select TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,ctd.TT_AlterDate,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room,ctd.TT_AlterDate from TT_ClassTimetable ct, TT_AlterTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_CLassFK and ct.TT_colCode='" + Convert.ToString(Collegecode).Trim() + "' and ct.TT_degCode='" + Convert.ToString(Degreecode).Trim() + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Convert.ToString(Semester).Trim() + "' " + qrySection + "  and ctd.TT_AlterDate>='" + SemStartdate + "' and ctd.TT_AlterDate<='" + SemEnddate + "' order by TT_AlterDate");
    //        dtAlterSchedule = new DataTable();
    //        dtAlterSchedule.Columns.Add("TT_AlterDate");
    //        dtAlterSchedule.Columns.Add("TTAlterDate");
    //        dtAlterSchedule.Columns.Add("TT_ClassFK");
    //        DataTable dtMinMaxDate = dirAcc.selectDataTable("select distinct CONVERT(varchar(20),ctd.TT_AlterDate,103) as TT_date,ctd.TT_AlterDate TTDate from TT_ClassTimetable ct, TT_AlterTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_CLassFK and ct.TT_colCode='" + Convert.ToString(Collegecode).Trim() + "' and ct.TT_degCode='" + Convert.ToString(Degreecode).Trim() + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Convert.ToString(Semester).Trim() + "' " + qrySection + "  and ctd.TT_AlterDate>='" + SemStartdate + "' and ctd.TT_AlterDate<='" + SemEnddate + "' order by TTDate");//CONVERT(varchar(20),min(ctd.TT_AlterDate),103) as FromDate,CONVERT(varchar(20),max(ctd.TT_AlterDate),103) as ToDate,
    //        if (dtMinMaxDate.Rows.Count > 0)
    //        {
    //            DataRow drAltert;// = dtAlterSchedule.NewRow();
    //            DateTime dtMinDate = new DateTime();
    //            DateTime dtMaxDate = new DateTime();
    //            DateTime dtTTDate = new DateTime();

    //            //DateTime.TryParseExact(Convert.ToString(dtMinMaxDate.Rows[0]["FromDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMinDate);
    //            //DateTime.TryParseExact(Convert.ToString(dtMinMaxDate.Rows[0]["ToDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMaxDate);
    //            for (int ttRow = 0; ttRow < dtMinMaxSchedule.Rows.Count; ttRow++)
    //            {
    //                if (Convert.ToString(dtMinMaxDate.Rows[0]["TT_date"]).Trim() != "" && Convert.ToString(dtMinMaxDate.Rows[0]["TTDate"]).Trim() != "")
    //                {
    //                    DateTime.TryParseExact(Convert.ToString(dtMinMaxSchedule.Rows[ttRow]["TT_date"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtTTDate);
    //                    drAltert = dtAlterSchedule.NewRow();
    //                    drAltert["TT_AlterDate"] = Convert.ToString(dtTTDate.ToString("MM/dd/yyyy"));
    //                    drAltert["TTAlterDate"] = dtTTDate;
    //                    for (int i = 0; i < 7; i++)
    //                    {
    //                        string curday = string.Empty;
    //                        string curdayFull = string.Empty;
    //                        switch (i)
    //                        {
    //                            case 0:
    //                                curdayFull = "Monday";
    //                                curday = "mon";
    //                                break;
    //                            case 1:
    //                                curdayFull = "Tuesday";
    //                                curday = "tue";
    //                                break;
    //                            case 2:
    //                                curdayFull = "Wednesday";
    //                                curday = "wed";
    //                                break;
    //                            case 3:
    //                                curdayFull = "Thursday";
    //                                curday = "thu";
    //                                break;
    //                            case 4:
    //                                curdayFull = "Friday";
    //                                curday = "fri";
    //                                break;
    //                            case 5:
    //                                curdayFull = "Saturday";
    //                                curday = "sat";
    //                                break;
    //                            case 6:
    //                                curdayFull = "Sunday";
    //                                curday = "sun";
    //                                break;
    //                        }
    //                        for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
    //                        {
    //                            if (!dtAlterSchedule.Columns.Contains(curday + hrsI))
    //                                dtAlterSchedule.Columns.Add(curday + hrsI);

    //                            dtAlterSelect.DefaultView.RowFilter = "Daydiscription='" + curdayFull + "' and TT_Hour='" + hrsI + "' and TT_AlterDate='" + dtTTDate.ToString("MM/dd/yyyy") + "'";
    //                            DataTable dtFilter = dtAlterSelect.DefaultView.ToTable();
    //                            if (dtFilter.Rows.Count > 0)
    //                            {
    //                                StringBuilder sbNew = new StringBuilder();
    //                                for (int dtI = 0; dtI < dtFilter.Rows.Count; dtI++)
    //                                {
    //                                    string subno = Convert.ToString(dtFilter.Rows[dtI]["TT_subno"]);
    //                                    string stfcode = Convert.ToString(dtFilter.Rows[dtI]["TT_staffcode"]);
    //                                    string elect = Convert.ToString(dtFilter.Rows[dtI]["Elective"]);
    //                                    string Lab = Convert.ToString(dtFilter.Rows[dtI]["Lab"]);
    //                                    string practicalpair = Convert.ToString(dtFilter.Rows[dtI]["practicalpair"]);
    //                                    string differenciator = "S";
    //                                    string room = Convert.ToString(dtFilter.Rows[dtI]["room"]);
    //                                    if (elect.ToLower() == "true")
    //                                    {
    //                                        differenciator = "E";
    //                                    }
    //                                    else if (Lab.ToLower() == "true")
    //                                    {
    //                                        differenciator = "L";
    //                                    }
    //                                    else if (practicalpair != "0")
    //                                    {
    //                                        differenciator = "C";
    //                                    }
    //                                    sbNew.Append(subno + "-" + stfcode + "-" + differenciator + "-" + room + ";");
    //                                }
    //                                drAltert[curday + hrsI] = sbNew.ToString();
    //                            }
    //                        }
    //                    }
    //                    dtAlterSchedule.Rows.Add(drAltert);
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    /// <summary>
    /// To Get Semester Schedule and Aleternate Schedule From New To Old Table Structue
    /// Developed By Malang Raja T
    /// </summary> 
    /// <param name="Collegecode">CollegeCode</param>
    /// <param name="Batchyear">BatchYear</param>
    /// <param name="Degreecode">DegreeCode</param>
    /// <param name="Semester">Semester</param>
    /// <param name="Section">Section Name</param>
    /// <param name="SemStartdate">From Date in the Format MM/dd/yyyy</param>
    /// <param name="SemEnddate">To Date in the Format MM/dd/yyyy</param>
    /// <param name="intNHrs">Total No Of Hours for this Class</param>
    /// <param name="dtSchedule">Semester Schedule </param>
    /// <param name="dtAlterSchedule">Aleternate Schedule</param>
    protected void SemesterandAlternateSchedule(string Collegecode, string Batchyear, string Degreecode, string Semester, string Section, string SemStartdate, string SemEnddate, int intNHrs, ref DataTable dtSchedule, ref DataTable dtAlterSchedule)
    {
        try
        {
            string qrySection = string.Empty;
            if (Section.Trim() != "")
                qrySection = " and ltrim(rtrim(isnull(ct.TT_sec,'')))='" + Section + "'";
            DataTable dtSelect = dirAcc.selectDataTable("select CONVERT(varchar(20),ct.TT_date,103) as TT_date,TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room,ct.TT_ClassPK from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Collegecode.Trim() + "' and ct.TT_degCode='" + Convert.ToString(Degreecode).Trim() + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Convert.ToString(Semester).Trim() + "' " + qrySection + " and ct.TT_date<='" + SemEnddate + "' order by  ct.TT_date desc");
            dtSchedule = new DataTable();
            dtSchedule.Columns.Add("TT_ClassPK");
            dtSchedule.Columns.Add("FromDate");
            dtSchedule.Columns.Add("TTDate");
            DataTable dtMinMaxSchedule = new DataTable();
            dtMinMaxSchedule = dirAcc.selectDataTable("select distinct CONVERT(varchar(20),ct.TT_date,103) as TT_date,ct.TT_date TTDate from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Collegecode + "' and ct.TT_degCode='" + Degreecode + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Semester + "'  " + qrySection + " and ct.TT_date<='" + SemEnddate + "' order by  TTDate desc");//CONVERT(varchar(20),min(ct.TT_date),103) as FromDate,CONVERT(varchar(20),max(ct.TT_date),103) as ToDate
            if (dtMinMaxSchedule.Rows.Count > 0)
            {
                DataRow drSchedule;//= dtSchedule.NewRow();
                //DateTime dtMinDate = new DateTime();
                //DateTime dtMaxDate = new DateTime();
                DateTime dtTTDate = new DateTime();
                for (int ttRow = 0; ttRow < dtMinMaxSchedule.Rows.Count; ttRow++)
                {
                    if (Convert.ToString(dtMinMaxSchedule.Rows[ttRow]["TT_date"]).Trim() != "" && Convert.ToString(dtMinMaxSchedule.Rows[ttRow]["TTDate"]).Trim() != "")
                    {
                        DateTime.TryParseExact(Convert.ToString(dtMinMaxSchedule.Rows[ttRow]["TT_date"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtTTDate);
                        drSchedule = dtSchedule.NewRow();
                        drSchedule["FromDate"] = Convert.ToString(dtTTDate.ToString("MM/dd/yyyy"));
                        drSchedule["TTDate"] = dtTTDate;
                        for (int i = 0; i < 7; i++)
                        {
                            string curday = string.Empty;
                            string curdayFull = string.Empty;
                            switch (i)
                            {
                                case 0:
                                    curdayFull = "Monday";
                                    curday = "mon";
                                    break;
                                case 1:
                                    curdayFull = "Tuesday";
                                    curday = "tue";
                                    break;
                                case 2:
                                    curdayFull = "Wednesday";
                                    curday = "wed";
                                    break;
                                case 3:
                                    curdayFull = "Thursday";
                                    curday = "thu";
                                    break;
                                case 4:
                                    curdayFull = "Friday";
                                    curday = "fri";
                                    break;
                                case 5:
                                    curdayFull = "Saturday";
                                    curday = "sat";
                                    break;
                                case 6:
                                    curdayFull = "Sunday";
                                    curday = "sun";
                                    break;
                            }
                            for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                            {
                                if (!dtSchedule.Columns.Contains(curday + hrsI))
                                    dtSchedule.Columns.Add(curday + hrsI);
                                dtSelect.DefaultView.RowFilter = "Daydiscription='" + curdayFull + "' and TT_Hour='" + hrsI + "' and TT_date='" + Convert.ToString(dtTTDate.ToString("dd/MM/yyyy")) + "'";
                                DataTable dtFilter = dtSelect.DefaultView.ToTable();
                                if (dtFilter.Rows.Count > 0)
                                {
                                    drSchedule["TT_ClassPK"] = Convert.ToString(dtFilter.Rows[0]["TT_ClassPK"]);
                                    StringBuilder sbNew = new StringBuilder();
                                    for (int dtI = 0; dtI < dtFilter.Rows.Count; dtI++)
                                    {
                                        string subno = Convert.ToString(dtFilter.Rows[dtI]["TT_subno"]);
                                        string stfcode = Convert.ToString(dtFilter.Rows[dtI]["TT_staffcode"]);
                                        string elect = Convert.ToString(dtFilter.Rows[dtI]["Elective"]);
                                        string Lab = Convert.ToString(dtFilter.Rows[dtI]["Lab"]);
                                        string practicalpair = Convert.ToString(dtFilter.Rows[dtI]["practicalpair"]);
                                        string differenciator = "S";
                                        string room = Convert.ToString(dtFilter.Rows[dtI]["room"]);
                                        if (elect.ToLower() == "true")
                                        {
                                            differenciator = "E";
                                        }
                                        else if (Lab.ToLower() == "true")
                                        {
                                            differenciator = "L";
                                        }
                                        else if (practicalpair != "0")
                                        {
                                            differenciator = "C";
                                        }

                                        sbNew.Append(subno + "-" + stfcode + "-" + differenciator + "-" + room + ";");
                                    }
                                    drSchedule[curday + hrsI] = sbNew.ToString();
                                }
                            }
                        }
                        dtSchedule.Rows.Add(drSchedule);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    string curday = string.Empty;
                    string curdayFull = string.Empty;
                    switch (i)
                    {
                        case 0:
                            curdayFull = "Monday";
                            curday = "mon";
                            break;
                        case 1:
                            curdayFull = "Tuesday";
                            curday = "tue";
                            break;
                        case 2:
                            curdayFull = "Wednesday";
                            curday = "wed";
                            break;
                        case 3:
                            curdayFull = "Thursday";
                            curday = "thu";
                            break;
                        case 4:
                            curdayFull = "Friday";
                            curday = "fri";
                            break;
                        case 5:
                            curdayFull = "Saturday";
                            curday = "sat";
                            break;
                        case 6:
                            curdayFull = "Sunday";
                            curday = "sun";
                            break;
                    }
                    for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                    {
                        if (!dtSchedule.Columns.Contains(curday + hrsI))
                            dtSchedule.Columns.Add(curday + hrsI);
                    }
                }
            }
            DataTable dtAlterSelect = dirAcc.selectDataTable("select TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,ctd.TT_AlterDate,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room,ctd.TT_AlterDate from TT_ClassTimetable ct, TT_AlterTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_CLassFK and ct.TT_colCode='" + Convert.ToString(Collegecode).Trim() + "' and ct.TT_degCode='" + Convert.ToString(Degreecode).Trim() + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Convert.ToString(Semester).Trim() + "' " + qrySection + "  and ctd.TT_AlterDate>='" + SemStartdate + "' and ctd.TT_AlterDate<='" + SemEnddate + "' order by TT_AlterDate");
            dtAlterSchedule = new DataTable();
            dtAlterSchedule.Columns.Add("TT_AlterDate");
            dtAlterSchedule.Columns.Add("TTAlterDate");
            dtAlterSchedule.Columns.Add("TT_ClassFK");
            DataTable dtMinMaxDate = dirAcc.selectDataTable("select distinct CONVERT(varchar(20),ctd.TT_AlterDate,103) as TT_date,ctd.TT_AlterDate TTDate from TT_ClassTimetable ct, TT_AlterTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_CLassFK and ct.TT_colCode='" + Convert.ToString(Collegecode).Trim() + "' and ct.TT_degCode='" + Convert.ToString(Degreecode).Trim() + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Convert.ToString(Semester).Trim() + "' " + qrySection + "  and ctd.TT_AlterDate>='" + SemStartdate + "' and ctd.TT_AlterDate<='" + SemEnddate + "' order by TTDate");//CONVERT(varchar(20),min(ctd.TT_AlterDate),103) as FromDate,CONVERT(varchar(20),max(ctd.TT_AlterDate),103) as ToDate,
            if (dtMinMaxDate.Rows.Count > 0)
            {
                DataRow drAltert;// = dtAlterSchedule.NewRow();
                DateTime dtMinDate = new DateTime();
                DateTime dtMaxDate = new DateTime();
                DateTime dtTTDate = new DateTime();
                //DateTime.TryParseExact(Convert.ToString(dtMinMaxDate.Rows[0]["FromDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMinDate);
                //DateTime.TryParseExact(Convert.ToString(dtMinMaxDate.Rows[0]["ToDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMaxDate);
                for (int ttRow = 0; ttRow < dtMinMaxDate.Rows.Count; ttRow++)
                {
                    if (Convert.ToString(dtMinMaxDate.Rows[0]["TT_date"]).Trim() != "" && Convert.ToString(dtMinMaxDate.Rows[0]["TTDate"]).Trim() != "")
                    {
                        DateTime.TryParseExact(Convert.ToString(dtMinMaxDate.Rows[ttRow]["TT_date"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtTTDate);
                        drAltert = dtAlterSchedule.NewRow();
                        drAltert["TT_AlterDate"] = Convert.ToString(dtTTDate.ToString("MM/dd/yyyy"));
                        drAltert["TTAlterDate"] = dtTTDate;
                        for (int i = 0; i < 7; i++)
                        {
                            string curday = string.Empty;
                            string curdayFull = string.Empty;
                            switch (i)
                            {
                                case 0:
                                    curdayFull = "Monday";
                                    curday = "mon";
                                    break;
                                case 1:
                                    curdayFull = "Tuesday";
                                    curday = "tue";
                                    break;
                                case 2:
                                    curdayFull = "Wednesday";
                                    curday = "wed";
                                    break;
                                case 3:
                                    curdayFull = "Thursday";
                                    curday = "thu";
                                    break;
                                case 4:
                                    curdayFull = "Friday";
                                    curday = "fri";
                                    break;
                                case 5:
                                    curdayFull = "Saturday";
                                    curday = "sat";
                                    break;
                                case 6:
                                    curdayFull = "Sunday";
                                    curday = "sun";
                                    break;
                            }
                            for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                            {
                                if (!dtAlterSchedule.Columns.Contains(curday + hrsI))
                                    dtAlterSchedule.Columns.Add(curday + hrsI);

                                dtAlterSelect.DefaultView.RowFilter = "Daydiscription='" + curdayFull + "' and TT_Hour='" + hrsI + "' and TT_AlterDate='" + dtTTDate.ToString("MM/dd/yyyy") + "'";
                                DataTable dtFilter = dtAlterSelect.DefaultView.ToTable();
                                if (dtFilter.Rows.Count > 0)
                                {
                                    StringBuilder sbNew = new StringBuilder();
                                    for (int dtI = 0; dtI < dtFilter.Rows.Count; dtI++)
                                    {
                                        string subno = Convert.ToString(dtFilter.Rows[dtI]["TT_subno"]);
                                        string stfcode = Convert.ToString(dtFilter.Rows[dtI]["TT_staffcode"]);
                                        string elect = Convert.ToString(dtFilter.Rows[dtI]["Elective"]);
                                        string Lab = Convert.ToString(dtFilter.Rows[dtI]["Lab"]);
                                        string practicalpair = Convert.ToString(dtFilter.Rows[dtI]["practicalpair"]);
                                        string differenciator = "S";
                                        string room = Convert.ToString(dtFilter.Rows[dtI]["room"]);
                                        if (elect.ToLower() == "true")
                                        {
                                            differenciator = "E";
                                        }
                                        else if (Lab.ToLower() == "true")
                                        {
                                            differenciator = "L";
                                        }
                                        else if (practicalpair != "0")
                                        {
                                            differenciator = "C";
                                        }
                                        sbNew.Append(subno + "-" + stfcode + "-" + differenciator + "-" + room + ";");
                                    }
                                    drAltert[curday + hrsI] = sbNew.ToString();
                                }
                            }
                        }
                        dtAlterSchedule.Rows.Add(drAltert);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    string curday = string.Empty;
                    string curdayFull = string.Empty;
                    switch (i)
                    {
                        case 0:
                            curdayFull = "Monday";
                            curday = "mon";
                            break;
                        case 1:
                            curdayFull = "Tuesday";
                            curday = "tue";
                            break;
                        case 2:
                            curdayFull = "Wednesday";
                            curday = "wed";
                            break;
                        case 3:
                            curdayFull = "Thursday";
                            curday = "thu";
                            break;
                        case 4:
                            curdayFull = "Friday";
                            curday = "fri";
                            break;
                        case 5:
                            curdayFull = "Saturday";
                            curday = "sat";
                            break;
                        case 6:
                            curdayFull = "Sunday";
                            curday = "sun";
                            break;
                    }
                    for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                    {
                        if (!dtAlterSchedule.Columns.Contains(curday + hrsI))
                            dtAlterSchedule.Columns.Add(curday + hrsI);
                    }
                }
            }
        }
        catch
        {
        }
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using Gios.Pdf;
using InsproDataAccess;

public class MyImg : ImageCellType
{
    //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
    public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
    {
        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
        img.Width = Unit.Percentage(80);
        // img.Height = Unit.Percentage(40);
        return img;
    }
}

public class MyImg1 : ImageCellType
{
    //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
    public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
    {
        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
        img.Width = Unit.Percentage(90);
        img.Height = Unit.Percentage(90);
        return img;
    }
}

public partial class NewStudentTimeTable : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    Boolean cellfalsg = false;
    Boolean celldetails = false;
    string tablevalue = string.Empty;
    Boolean allowcom = false;
    DataSet srids = new DataSet();
    DAccess2 srida = new DAccess2();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();

    #region Load Details

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            //errmsg.Visible = false;
            if (!IsPostBack)
            {
                lblErrMsg.Text = string.Empty;
                chkSectionWise.Checked = false;
                chkSectionWise.Visible = false;
                btnCopySubject.Visible = false;
                Fptimetable.Visible = false;
                Fptimetable.Sheets[0].SheetCorner.Columns[0].Visible = false;
                Fptimetable.Sheets[0].SheetCorner.RowCount = 0;
                Fptimetable.ActiveSheetView.AutoPostBack = true;
                MyStyle.Font.Size = 10;
                MyStyle.Font.Bold = true;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.HorizontalAlign = HorizontalAlign.Center;
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Fptimetable.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                Fptimetable.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(MyStyle);
                Fptimetable.Sheets[0].AllowTableCorner = true;
                Fpclassadvisor.Visible = false;
                Fpclassadvisor.Sheets[0].SheetCorner.Columns[0].Visible = false;
                Fpclassadvisor.Sheets[0].SheetCorner.RowCount = 0;
                Fpclassadvisor.ActiveSheetView.AutoPostBack = false;
                MyStyle.Font.Size = 10;
                MyStyle.Font.Bold = true;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Fpclassadvisor.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                Fpclassadvisor.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(MyStyle);
                Fpclassadvisor.Sheets[0].AllowTableCorner = true;
                fpdetails.Visible = false;
                fpdetails.Sheets[0].SheetCorner.Columns[0].Visible = false;
                fpdetails.Sheets[0].SheetCorner.RowCount = 0;
                fpdetails.ActiveSheetView.AutoPostBack = true;
                MyStyle.Font.Size = 10;
                MyStyle.Font.Bold = true;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpdetails.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                fpdetails.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(MyStyle);
                fpdetails.Sheets[0].AllowTableCorner = true;
                FpSpread1.Visible = false;
                gridSelTT.Visible = false;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
                FpSpread1.Sheets[0].SheetCorner.RowCount = 0;
                FpSpread1.ActiveSheetView.AutoPostBack = true;
                MyStyle.Font.Size = 10;
                MyStyle.Font.Bold = true;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(MyStyle);
                FpSpread1.Sheets[0].AllowTableCorner = true;
                txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                errmsg.Visible = false;
                txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Bindcolg();
                ddlcollege.SelectedIndex = 0;
                BindBatch();
                BindDegree();
                if (ddldegree.Items.Count > 0)
                {
                    ddldegree.Enabled = true;
                    ddlbranch.Enabled = true;
                    ddlsec.Enabled = true;
                    ddlsem.Enabled = true;
                    btngo.Enabled = true;
                    ddltimetable.Enabled = true;
                    BindBranch();
                    BindSem();
                    BindSectionDetail(strbatch, strbranch);
                    BindTimetable();
                    txtdate.Enabled = true;
                }
                else
                {
                    ddldegree.Enabled = false;
                    ddlbranch.Enabled = false;
                    ddlsec.Enabled = false;
                    ddlsem.Enabled = false;
                    btngo.Enabled = false;
                    ddltimetable.Enabled = false;
                    txtdate.Enabled = false;
                }
                lblday.Visible = false;
                lblday1.Visible = false;
                lbltimings.Visible = false;
                lblfromtime.Visible = false;
                lbltotime.Visible = false;
                treepanel.Visible = false;
                btnsave.Enabled = false;
                btnsave.Visible = false;
                txtmultisubj.Enabled = false;
                txttimetable.Visible = false;
                panelstaff.Visible = true;
                fsstaff.Sheets[0].RowCount = 0;
                fsstaff.Sheets[0].AutoPostBack = false;
                fsstaff.Sheets[0].SheetCorner.RowCount = 1;
                fsstaff.Sheets[0].RowHeader.Visible = false;
                fsstaff.CommandBar.Visible = false;
                fsstaff.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                MyStyle.Font.Size = 10;
                MyStyle.Font.Bold = true;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fsstaff.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                fsstaff.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                fsstaff.Sheets[0].DefaultStyle.Font.Bold = false;
                panelstaff.Visible = false;
                btnclassadvisor.Visible = false;
                btndelete.Visible = false;
                btnexcel.Visible = false;
                //btnprint.Visible = false;
                btnPDF.Visible = false;//Added by Manikandan on 10/12/2013
                Printcontrol.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                string grouporusercode = string.Empty;
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                // Added By Sridharan 12 Mar 2015
                //{
                DataSet schoolds = new DataSet();
                string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                    if (schoolvalue.Trim() == "0")
                    {
                        //forschoolsetting = true;
                        lblcolg.Text = "School";
                        lblbatch.Text = "Year";
                        lbldegree.Text = "School Type";
                        lblbranch.Text = "Standard";
                        lblsem.Text = "Term";
                        //lblDegree.Attributes.Add("style", " width: 95px;");
                        //lblBranch.Attributes.Add("style", " width: 67px;");
                        //ddlBranch.Attributes.Add("style", " width: 241px;");
                    }
                    else
                    {
                        // forschoolsetting = false;
                    }
                }
                //} Sridharan
            }
            errmsg.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
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

    //========Sangeetha On 29 Aug2014==========
    public void Bindcolg()
    {
        //try
        //{
        //    string colg = "select collname,college_code from collinfo";
        //    ds.Dispose();
        //    ds.Reset();
        //    DAccess2 d2 = new DAccess2();
        //    ds = d2.select_method_wo_parameter(colg, "Text");
        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        ddlcolg.DataSource = ds;
        //        ddlcolg.DataTextField = "collname";
        //        ddlcolg.DataValueField = "college_code";
        //        ddlcolg.DataBind();
        //        ddlcolg.SelectedIndex = ddlbatch.Items.Count - 1;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    errmsg.Text = ex.ToString();
        //}
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
        ds = d2.select_method("bind_college", hat, "sp");
        ddlcolg.Items.Clear();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlcolg.Enabled = true;
            ddlcolg.DataSource = ds;
            ddlcolg.DataTextField = "collname";
            ddlcolg.DataValueField = "college_code";
            ddlcolg.DataBind();
        }
    }

    public void BindDegree()
    {
        try
        {
            ddldegree.Items.Clear();
            collegecode = ddlcolg.SelectedValue.ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
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

    public void BindBranch()
    {
        try
        {
            course_id = ddldegree.SelectedValue.ToString();
            collegecode = ddlcolg.SelectedValue.ToString();
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
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

    public void BindSem()
    {
        try
        {
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            collegecode = ddlcolg.SelectedValue.ToString();
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            errmsg.Visible = false;
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            ddlsec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSectionDetail(strbatch, strbranch);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                //  ddlsec.Items.Insert(0, "All");
                if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
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
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void collook_load(object sender, EventArgs e)
    {
        try
        {
            BindBatch();
            BindDegree();
            if (ddldegree.Items.Count > 0)
            {
                ddldegree.Enabled = true;
                ddlbranch.Enabled = true;
                ddlsec.Enabled = true;
                ddlsem.Enabled = true;
                btngo.Enabled = true;
                ddltimetable.Enabled = true;
                BindBranch();
                BindSem();
                BindSectionDetail(strbatch, strbranch);
                BindTimetable();
                txtdate.Enabled = true;
            }
            else
            {
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
                ddlsec.Enabled = false;
                ddlsem.Enabled = false;
                btngo.Enabled = false;
                ddltimetable.Enabled = false;
                txtdate.Enabled = false;
            }
        }
        catch
        {
        }
    }

    public void BindTimetable()
    {
        try
        {
            lblday.Visible = false;
            lblday1.Visible = false;
            lbltimings.Visible = false;
            lblfromtime.Visible = false;
            lbltotime.Visible = false;
            treepanel.Visible = false;
            btnsave.Enabled = false;
            btnsave.Visible = false;
            txtmultisubj.Enabled = false;
            txttimetable.Visible = false;
            errmsg.Visible = false;
            btnexcel.Visible = false;
            //btnprint.Visible = false;
            btnPDF.Visible = false;//Added by Manikandan on 10/12/2013
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            ddltimetable.Items.Clear();
            btndelete.Visible = false;
            Fpclassadvisor.Visible = false;
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            string section = string.Empty;
            string newSections = string.Empty;
            if (strsec != "" && strsec != "-1" && strsec.Trim().ToLower() != "all")
            {
                section = "and sections='" + strsec + "'";
                newSections = " and ct.TT_sec='" + strsec + "' ";
            }
            ds.Dispose();
            ds.Reset();
            //string strtimetable = "Select DISTINCT TTname from semester_schedule where batch_year=" + strbatchyear + " and degree_code=" + strbranch + " and semester=" + strsem + " " + section + "";
            //ds = d2.select_method(strtimetable, hat, "Text");
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    ddltimetable.DataSource = ds;
            //    ddltimetable.DataTextField = "TTname";
            //    ddltimetable.DataBind();
            //    binddate();
            //}
            //Idhris
            string strtimetable = "select distinct TT_name,TT_date from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Convert.ToString(ddlcolg.SelectedValue) + "' and ct.TT_degCode='" + strbranch + "' and ct.TT_batchyear='" + strbatchyear + "' and ct.TT_sem='" + strsem + "' " + newSections + "order by TT_date asc -- and TT_date>='" + (txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) + "'  ";
            ds = d2.select_method(strtimetable, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddltimetable.DataSource = ds;
                ddltimetable.DataTextField = "TT_name";
                ddltimetable.DataBind();
                binddate();
            }
            ddltimetable.Items.Insert(0, "");
            ddltimetable.Items.Insert(1, "New");
            if (ddltimetable.Items.Count >= 3)
            {
                ddltimetable.SelectedIndex = ddltimetable.Items.Count - 1;
                loaddetail();
            }
            else
            {
                ddltimetable.SelectedIndex = 0;
            }
            txttimetable.Visible = false;
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void binddate()
    {
        try
        {
            btnexcel.Visible = false;
            //btnprint.Visible = false;
            btnPDF.Visible = false;//Added by Manikandan on 10/12/2013
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            errmsg.Visible = false;
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            string section = string.Empty;
            if (strsec != "" && strsec != "-1" && strsec != "All")
            {
                section = "and TT_sec='" + strsec + "'";
            }
            //string date = d2.GetFunction("Select convert(nvarchar(15),Fromdate,103) as date from semester_schedule where batch_year=" + strbatchyear + " and degree_code=" + strbranch + " and semester=" + strsem + " " + section + " and ttname='" + ddltimetable.SelectedItem.ToString() + "'");
            //if (date != "" && date != null && date != "0" && ddltimetable.Enabled == true)
            //{
            //    txtdate.Text = date;
            //}
            string date = dirAcc.selectScalarString("select distinct convert(varchar(10),TT_date,103),TT_date  from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Convert.ToString(ddlcolg.SelectedValue) + "' and ct.TT_degCode='" + strbranch + "' and ct.TT_batchyear='" + strbatchyear + "' and ct.TT_sem='" + strsem + "' " + section + " and TT_name='" + ddltimetable.SelectedItem.ToString() + "' order by TT_date desc");
            if (date != "" && date != null && date != "0" && ddltimetable.Enabled == true)
            {
                txtdate.Text = date;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddldegree.Items.Count > 0)
        {
            chkSectionWise.Checked = false;
            chkSectionWise.Visible = false;
            errmsg.Visible = false;
            BindBranch();
            BindSem();
            BindSectionDetail(strbatch, strbranch);
            Fptimetable.Visible = false;
            fpdetails.Visible = false;
            BindTimetable();
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkSectionWise.Checked = false;
        chkSectionWise.Visible = false;
        errmsg.Visible = false;
        BindBranch();
        BindSem();
        BindSectionDetail(strbatch, strbranch);
        Fptimetable.Visible = false;
        fpdetails.Visible = false;
        BindTimetable();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkSectionWise.Checked = false;
        chkSectionWise.Visible = false;
        errmsg.Visible = false;
        BindSem();
        BindSectionDetail(strbatch, strbranch);
        Fptimetable.Visible = false;
        fpdetails.Visible = false;
        BindTimetable();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkSectionWise.Checked = false;
        chkSectionWise.Visible = false;
        errmsg.Visible = false;
        BindSectionDetail(strbatch, strbranch);
        Fptimetable.Visible = false;
        fpdetails.Visible = false;
        BindTimetable();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkSectionWise.Checked = false;
        chkSectionWise.Visible = false;
        errmsg.Visible = false;
        Fptimetable.Visible = false;
        fpdetails.Visible = false;
        BindTimetable();
    }

    protected void ddltimetable_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkSectionWise.Checked = false;
        chkSectionWise.Visible = false;
        loaddetail();
    }

    protected void loaddetail()
    {
        try
        {
            if (ddltimetable.SelectedItem.ToString() == "New")
            {
                txttimetable.Visible = true;
                txttimetable.Text = string.Empty;
                tdTime.Attributes.Add("style", "display:block;");
                tdbranch.Attributes.Add("colspan", "3");
            }
            else
            {
                tdbranch.Attributes.Add("colspan", "2");
                tdTime.Attributes.Add("style", "display:none;");
                txttimetable.Visible = false;
                btnclassadvisor.Visible = false;
                if (ddltimetable.SelectedItem.ToString() != "")
                {
                    fpdetails.Visible = false;
                    binddate();
                    //loadfunction();//Hidden By Srinath 16/8/2013
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    protected void btngo_Click(object sender, EventArgs e)
    {
        loadfunction();
    }

    protected void loadfunction()
    {
        try
        {
            lblErrMsg.Text = string.Empty;
            btndelete.Visible = true;
            lblday.Visible = false;
            lblday1.Visible = false;
            lbltimings.Visible = false;
            lblfromtime.Visible = false;
            lbltotime.Visible = false;
            btnsave.Enabled = false;
            btnsave.Visible = true;
            fpdetails.Visible = false;
            treepanel.Visible = false;
            btnexcel.Visible = false;
            chkSectionWise.Checked = false;
            chkSectionWise.Visible = false;
            //btnprint.Visible = false;
            btnPDF.Visible = false;//Added by Manikandan on 10/12/2013
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Fpclassadvisor.Visible = false;
            Fpclassadvisor.Sheets[0].RowCount = 0;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            string semester = string.Empty;
            string section = string.Empty;
            if (ddlbatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            }
            if (ddlbranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlbranch.SelectedItem.Value).Trim();
            }
            if (ddlsem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            }
            if (ddlsec.Enabled == true && ddlsec.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(ddlsec.SelectedItem.Text).Trim()) && Convert.ToString(ddlsec.SelectedItem.Text).Trim() != "-1" && Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower().Trim() != "all")
                {
                    section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                }
            }
            if (Convert.ToString(ddltimetable.SelectedItem.Text).Trim().ToLower() == "new")
            {
                if (!string.IsNullOrEmpty(txttimetable.Text.Trim()) && txttimetable.Text.Trim().ToLower() != "new")
                {
                    bool isAvailabeName = false;
                    isAvailabeName = IsTimeTableExistsOrNot(txttimetable.Text.Trim(), Convert.ToString(batchYear).Trim(), Convert.ToString(degreeCode).Trim(), Convert.ToString(semester).Trim(), Convert.ToString(section).Trim(), txtdate.Text.Trim());
                    if (isAvailabeName)
                    {
                        btnsave.Visible = false;
                        Fpclassadvisor.Visible = false;
                        btndelete.Visible = false;
                        errmsg.Visible = true;
                        Fptimetable.Visible = false;
                        errmsg.Visible = true;
                        errmsg.Text = "Time Table Name is Already Exists!!!";
                        return;
                    }
                }
                else
                {
                    btnsave.Visible = false;
                    Fpclassadvisor.Visible = false;
                    btndelete.Visible = false;
                    errmsg.Visible = true;
                    Fptimetable.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter Time Table Name";
                    return;
                }
            }
            else
            {
            }
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            int noofhours = 0;
            int dayorder = 0;
            int noofdays = 0;
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string[] Daymon = new string[7] { "Monday", "Tuesday", "wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            errmsg.Visible = false;
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            if (strsec != "" && strsec != "-1" && strsec != "All")
            {
                strsec = "and sections='" + strsec + "'";
            }
            else
            {
                strsec = string.Empty;
            }
            string[] datespilt = txtdate.Text.Split('/');
            string temp_date = datespilt[1] + '/' + datespilt[0] + '/' + datespilt[2];
            ds.Dispose();
            ds.Reset();
            string holiday = string.Empty;
            string strpriodquery = "Select No_of_hrs_per_day,schorder,nodays,holiday from PeriodAttndSchedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + strsem + "";
            ds = d2.select_method(strpriodquery, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dayorder = Convert.ToInt32(ds.Tables[0].Rows[0]["schorder"]);
                noofhours = Convert.ToInt32(ds.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                noofdays = Convert.ToInt32(ds.Tables[0].Rows[0]["nodays"]);
                holiday = Convert.ToString(ds.Tables[0].Rows[0]["holiday"]);
                Session["totalhrs"] = Convert.ToString(noofhours);
                Session["totnoofdays"] = Convert.ToString(noofdays);
                Session["dayorder"] = Convert.ToString(dayorder);
            }
            Fptimetable.Sheets[0].RowCount = noofdays;
            if (noofhours > 0)
            {
                Fptimetable.Visible = true;
                btnexcel.Visible = true;
                //btnprint.Visible = true;
                btnPDF.Visible = true;//Added by Manikandan on 10/12/2013
                Printcontrol.Visible = false;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                Fptimetable.Sheets[0].ColumnHeader.RowCount = 2;
                Fptimetable.Sheets[0].ColumnCount = noofhours + 1;
                Fptimetable.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Day";
                Fptimetable.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Timings";
                Fptimetable.Sheets[0].Columns[0].Width = 100;
                Fptimetable.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                Fptimetable.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                Fptimetable.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                Fptimetable.Sheets[0].Columns[0].Font.Bold = true;
                Fptimetable.Sheets[0].ColumnHeader.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                Fptimetable.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
                Fptimetable.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                Fptimetable.Sheets[0].Columns[1].Font.Bold = true;
                Fptimetable.Sheets[0].RowCount = 0;
                for (int i = 1; i <= noofhours; i++)
                {
                    string belltime = string.Empty;
                    //Modified by srinath 10/6/2014
                    // string strtimequery = "Select start_time,end_time from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + i + "'";
                    string strtimequery = "Select RIGHT(CONVERT(VARCHAR, start_time, 100),7) as sTime,RIGHT(CONVERT(VARCHAR, end_time, 100),7) as endtime from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + i + "'";
                    ds = d2.select_method(strtimequery, hat, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string starttime = ds.Tables[0].Rows[0]["sTime"].ToString();
                        string endtime = ds.Tables[0].Rows[0]["endtime"].ToString();
                        if (starttime != null && starttime.Trim() != "" && endtime != null && endtime.Trim() != "")
                        {
                            belltime = starttime + " To " + endtime;
                        }
                        //if (ds.Tables[0].Rows[0]["sTime"].ToString() != "" && ds.Tables[0].Rows[0]["endtime"].ToString() != null && ds.Tables[0].Rows[0]["end_time"].ToString() != "" && ds.Tables[0].Rows[0]["end_time"].ToString() != null)
                        //{
                        //string[] spiltstarttime = ds.Tables[0].Rows[0]["start_time"].ToString().Split(' ');
                        //string[] spiltendtime = ds.Tables[0].Rows[0]["end_time"].ToString().Split(' ');
                        //belltime=spiltstarttime[1].ToString() + ' ' + spiltstarttime[2].ToString() + ' ' + " To "+spiltendtime[1].ToString() + ' ' + spiltendtime[2].ToString();
                        // }
                    }
                    string settext = i + " " + belltime;
                    Fptimetable.Sheets[0].ColumnHeader.Cells[0, i].Text = i.ToString();
                    Fptimetable.Sheets[0].ColumnHeader.Cells[1, i].Text = belltime;
                }
                string dayvalue = string.Empty;
                for (int day = 0; day < 6; day++)
                {
                    string dayofweek = Days[day];
                    string dayofweek1 = Daymon[day];
                    int daysetweek = day + 2;
                    if (day == 6)
                    {
                        daysetweek = 1;
                    }
                    if (!holiday.Contains(daysetweek.ToString()))
                    {
                        Fptimetable.Sheets[0].RowCount++;
                        if (dayorder == 1)
                        {
                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].Text = dayofweek1;
                        }
                        else
                        {
                            int date = day + 1;
                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].Text = "Day " + date;
                        }
                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].Note = dayofweek;
                        for (int i = 1; i <= noofhours; i++)
                        {
                            if (dayvalue == "")
                            {
                                dayvalue = dayofweek + i;
                            }
                            else
                            {
                                dayvalue = dayvalue + ',' + dayofweek + i;
                            }
                        }
                    }
                }
                if (ddltimetable.SelectedItem.ToString() != "New")
                {
                    //Idhris
                    DataTable dtSelect = dirAcc.selectDataTable("select TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + ddlcolg.SelectedValue + "' and ct.TT_degCode='" + ddlbranch.SelectedValue + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + ddlsem.SelectedValue + "' and ct.TT_sec='" + ddlsec.SelectedValue + "' and ct.TT_name='" + ddltimetable.SelectedValue + "'  and TT_date>='" + (txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) + "'");
                    DataTable dtNewContruct = new DataTable();
                    DataRow drNewConstruct = dtNewContruct.NewRow();
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
                        for (int hrsI = 1; hrsI <= noofhours; hrsI++)
                        {
                            dtNewContruct.Columns.Add(curday + hrsI);
                            dtSelect.DefaultView.RowFilter = "Daydiscription='" + curdayFull + "' and TT_Hour='" + hrsI + "'";
                            DataTable dtFilter = dtSelect.DefaultView.ToTable();
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
                                drNewConstruct[curday + hrsI] = sbNew.ToString();
                            }
                        }
                    }
                    dtNewContruct.Rows.Add(drNewConstruct);
                    ds.Clear();
                    //string schedukle = "select  top 1 " + dayvalue + " from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + strsem + " and batch_year = " + strbatchyear + " and ttname='" + ddltimetable.SelectedItem.ToString() + "' and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc";
                    //ds = d2.select_method(schedukle, hat, "Text");
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        ds.Tables.RemoveAt(0);
                    }
                    ds.Tables.Add(dtNewContruct);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < Fptimetable.Sheets[0].RowCount; i++)
                        {
                            string value = Fptimetable.Sheets[0].Cells[i, 0].Note.ToString();
                            for (int j = 1; j < Fptimetable.Sheets[0].ColumnCount; j++)
                            {
                                string dsvalue = value + j;
                                string classhour = ds.Tables[0].Rows[0]["" + dsvalue + ""].ToString();
                                string setclasshour = string.Empty;
                                if (classhour.Trim() != "" && classhour.Trim() != "0" && classhour != null)
                                {
                                    string[] spiltmulpl = classhour.Split(';');
                                    for (int mul = 0; mul <= spiltmulpl.GetUpperBound(0); mul++)
                                    {
                                        string[] spiltclasshour = spiltmulpl[mul].Split('-');
                                        if (spiltclasshour.Length > 1)
                                        {
                                            for (int sp = 0; sp <= spiltclasshour.GetUpperBound(0); sp++)
                                            {
                                                if (sp == 0)
                                                {
                                                    if (setclasshour == "")
                                                    {
                                                        try
                                                        {
                                                            setclasshour = d2.GetFunction("select case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no='" + spiltclasshour[sp].ToString() + "'");
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                        }
                                                    }
                                                    else
                                                    {
                                                        setclasshour = setclasshour + ';' + d2.GetFunction("select case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no='" + spiltclasshour[sp].ToString() + "'"); ;
                                                    }
                                                }
                                                else
                                                {
                                                    setclasshour = setclasshour + '-' + spiltclasshour[sp].ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                                Fptimetable.Sheets[0].Cells[i, j].Text = setclasshour;
                                Fptimetable.Sheets[0].Cells[i, j].Note = classhour;
                            }
                        }
                        btnsave.Visible = true;
                        btndelete.Visible = false;
                        Fpclassadvisor.Visible = false;
                        //string schedukle = d2.GetFunction("select  top 1 class_advisor from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + strsem + " and batch_year = " + strbatchyear + " and ttname='" + ddltimetable.SelectedItem.ToString() + "' and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc");
                        //Idhris
                        string schedukle = dirAcc.selectScalarString("select TT_classAdvisor from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + ddlcolg.SelectedValue + "' and ct.TT_degCode='" + ddlbranch.SelectedValue + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + ddlsem.SelectedValue + "' and ct.TT_sec='" + ddlsec.SelectedValue + "' and ct.TT_name='" + ddltimetable.SelectedValue + "' and TT_date>='" + (txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) + "'");
                        if (schedukle != null && schedukle.Trim() != "" && schedukle.Trim() != "0")
                        {
                            Fpclassadvisor.Visible = true;
                            Fpclassadvisor.Sheets[0].ColumnCount = 4;
                            Fpclassadvisor.Sheets[0].ColumnHeader.RowCount = 1;
                            Fpclassadvisor.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                            Fpclassadvisor.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpclassadvisor.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
                            Fpclassadvisor.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
                            Fpclassadvisor.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Remove";
                            Fpclassadvisor.Sheets[0].Columns[0].Width = 50;
                            Fpclassadvisor.Sheets[0].Columns[1].Width = 220;
                            Fpclassadvisor.Sheets[0].Columns[2].Width = 200;
                            Fpclassadvisor.Sheets[0].Columns[3].Width = 100;
                            FarPoint.Web.Spread.ButtonCellType staf_butt1 = new FarPoint.Web.Spread.ButtonCellType("OneCommand", FarPoint.Web.Spread.ButtonType.PushButton, "Remove");
                            Fpclassadvisor.Sheets[0].Columns[3].CellType = staf_butt1;
                            staf_butt1.Text = "Remove";
                            Fpclassadvisor.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                            Fpclassadvisor.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                            Fpclassadvisor.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                            string[] spiltadvisor = schedukle.Split(',');
                            for (int i = 0; i <= spiltadvisor.GetUpperBound(0); i++)
                            {
                                Fpclassadvisor.Sheets[0].RowCount++;
                                string staffname = d2.GetFunction("select staff_name from staffmaster where staff_code='" + spiltadvisor[i].ToString() + "'");
                                Fpclassadvisor.Sheets[0].Cells[Fpclassadvisor.Sheets[0].RowCount - 1, 0].Text = Fpclassadvisor.Sheets[0].RowCount.ToString();
                                Fpclassadvisor.Sheets[0].Cells[Fpclassadvisor.Sheets[0].RowCount - 1, 1].CellType = txt;
                                Fpclassadvisor.Sheets[0].Cells[Fpclassadvisor.Sheets[0].RowCount - 1, 1].Text = staffname;
                                Fpclassadvisor.Sheets[0].Cells[Fpclassadvisor.Sheets[0].RowCount - 1, 2].CellType = txt;
                                Fpclassadvisor.Sheets[0].Cells[Fpclassadvisor.Sheets[0].RowCount - 1, 2].Text = spiltadvisor[i].ToString();
                            }
                        }
                        Fpclassadvisor.Sheets[0].PageSize = Fpclassadvisor.Sheets[0].RowCount;
                        Fpclassadvisor.Height = 20 + (Fpclassadvisor.Sheets[0].RowCount * 25);
                        Fpclassadvisor.Width = 565;
                    }
                    else
                    {
                        btndelete.Visible = false;
                        errmsg.Visible = true;
                        Fptimetable.Visible = false;
                        errmsg.Text = "No Records Found";
                        btnsave.Visible = false;
                    }
                    btnclassadvisor.Visible = true;
                }
                else
                {
                    if (txttimetable.Text == "" || txttimetable.Text == null || txttimetable.Text.ToLower() == "new")
                    {
                        btndelete.Visible = false;
                        errmsg.Visible = true;
                        Fptimetable.Visible = false;
                        errmsg.Text = "Please Enter Time Table Name";
                        btnsave.Visible = false;
                        Fpclassadvisor.Visible = false;
                    }
                    else
                    {
                        if (Convert.ToString(ddltimetable.SelectedItem.Text).Trim().ToLower() == "new")
                        {
                            bool isAvailabeName = false;
                            isAvailabeName = IsTimeTableExistsOrNot(txttimetable.Text.Trim(), Convert.ToString(ddlbatch.SelectedItem.Text).Trim(), Convert.ToString(ddlbatch.SelectedItem.Text).Trim(), Convert.ToString(ddlbatch.SelectedItem.Text).Trim(), Convert.ToString(ddlbatch.SelectedItem.Text).Trim(), txtdate.Text.Trim());
                            if (isAvailabeName)
                            {
                                btnsave.Visible = false;
                                Fpclassadvisor.Visible = false;
                                btndelete.Visible = false;
                                errmsg.Visible = true;
                                Fptimetable.Visible = false;
                                errmsg.Visible = true;
                                errmsg.Text = "Time Table Name is Already Exists!!!";
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                btndelete.Visible = false;
                btnsave.Visible = false;
                errmsg.Visible = true;
                Fptimetable.Visible = false;
                errmsg.Text = "Please Update Semester Information";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void Fptimetable_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellfalsg = true;
    }

    protected void Fptimetable_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (cellfalsg == true)
            {
                txtmulstaff.Visible = false;
                lblmulstaff.Visible = false;
                pmulstaff.Visible = false;
                btnCopySubject.Visible = false;
                btnmulstaff.Visible = false;
                btndelete.Visible = false;
                btndelete.Enabled = false;
                lbltimings.Visible = false;
                lblfromtime.Visible = false;
                lbltotime.Visible = false;
                fpdetails.Visible = false;
                btnOk.Visible = false;
                txtmultisubj.Visible = false;
                chkappend.Checked = false;
                chkappend.Visible = false;
                //chkSectionWise.Checked = false;
                chkSectionWise.Visible = false;
                cellfalsg = false;
                string activerow = Fptimetable.ActiveSheetView.ActiveRow.ToString();
                string activecol = Fptimetable.ActiveSheetView.ActiveColumn.ToString();
                if (activecol != "0" && activerow != "-1")
                {
                    int row = Convert.ToInt32(activerow);
                    int col = Convert.ToInt32(activecol);
                    if (ddltimetable.SelectedItem.Text.Trim() == "New")
                    {
                        chkSectionWise.Visible = true;
                    }
                    else
                    {
                        chkSectionWise.Visible = true;
                    }
                    lblday.Visible = true;
                    lblday1.Visible = true;
                    lblday1.Text = Fptimetable.Sheets[0].Cells[row, 0].Text;
                    ds.Dispose();
                    ds.Reset();
                    string strtimequery = "Select RIGHT(CONVERT(VARCHAR, start_time, 100),7) as start_time,RIGHT(CONVERT(VARCHAR, end_time, 100),7) as end_time from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + col + "'";
                    ds = d2.select_method(strtimequery, hat, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["start_time"].ToString() != "" && ds.Tables[0].Rows[0]["start_time"].ToString() != null && ds.Tables[0].Rows[0]["end_time"].ToString() != "" && ds.Tables[0].Rows[0]["end_time"].ToString() != null)
                        {
                            //string[] spiltstarttime = ds.Tables[0].Rows[0]["start_time"].ToString().Split(' ');
                            //string[] spiltendtime = ds.Tables[0].Rows[0]["end_time"].ToString().Split(' ');
                            //lblfromtime.Text = spiltstarttime[1].ToString() + ' ' + spiltstarttime[2].ToString() + ' ' + "To";
                            //lbltotime.Text = spiltendtime[1].ToString() + ' ' + spiltendtime[2].ToString();
                            lblfromtime.Text = ds.Tables[0].Rows[0]["start_time"].ToString();
                            lbltotime.Text = ds.Tables[0].Rows[0]["end_time"].ToString();
                            lbltimings.Visible = true;
                            lblfromtime.Visible = true;
                            lbltotime.Visible = true;
                        }
                    }
                    if (Fptimetable.Sheets[0].Cells[row, col].Note != null && Fptimetable.Sheets[0].Cells[row, col].Note != "")
                    {
                        btndelete.Visible = true;
                        btndelete.Enabled = true;
                        string columnvalue = Fptimetable.Sheets[0].Cells[row, col].Note.ToString();
                        fpdetails.Visible = true;
                        fpdetails.Sheets[0].ColumnHeader.RowCount = 1;
                        fpdetails.Sheets[0].ColumnCount = 3;
                        fpdetails.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Subject";
                        fpdetails.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
                        fpdetails.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Class Type";
                        fpdetails.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                        fpdetails.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                        fpdetails.Sheets[0].RowCount = 0;
                        string[] spitcolumnvalue = columnvalue.Split(';');
                        ArrayList arrStaffSubjects = new ArrayList();
                        for (int i = 0; i <= spitcolumnvalue.GetUpperBound(0); i++)
                        {
                            string[] spitsubject = spitcolumnvalue[i].Split('-');
                            int t = spitsubject.GetUpperBound(0);
                            for (int j = 1; j < t; j++)
                            {
                                for (int next = j; next <= j; next++)
                                {
                                    string subjectno = d2.GetFunction("Select subject_name from subject where subject_no='" + spitsubject[0] + "'");
                                    string type = spitsubject[spitsubject.GetUpperBound(0)].Trim();
                                    if (type == "S")
                                    {
                                        type = "Single";
                                    }
                                    else
                                    {
                                        type = "Lab";
                                    }
                                    string staff = d2.GetFunction("Select Staff_name from staffmaster where staff_code='" + spitsubject[next].ToString() + "'");
                                    if (staff.Trim() == "" || staff.Trim() == "0")
                                    {
                                        staff = "-";
                                    }
                                    string keyList = spitsubject[0].Trim() + "@" + Convert.ToString(spitsubject[next]).Trim();
                                    if (!arrStaffSubjects.Contains(keyList.Trim().ToLower()))
                                    {
                                        arrStaffSubjects.Add(keyList.Trim().ToLower());
                                        fpdetails.Sheets[0].RowCount++;
                                        fpdetails.Sheets[0].Cells[fpdetails.Sheets[0].RowCount - 1, 0].Text = subjectno;
                                        fpdetails.Sheets[0].Cells[fpdetails.Sheets[0].RowCount - 1, 1].Text = staff;
                                        fpdetails.Sheets[0].Cells[fpdetails.Sheets[0].RowCount - 1, 2].Text = type;
                                    }
                                }
                            }
                        }
                    }
                    treeload();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void treeload()
    {
        try
        {
            chkappend.Checked = false;
            txtmultisubj.Visible = false;
            pnlmultisubj.Visible = false;
            chk_multisubj.Visible = false;
            FpSpread1.Visible = false;
            gridSelTT.Visible = false;
            subjtree.Visible = true;
            treepanel.Visible = true;
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            subjtree.Nodes.Clear();
            //sankar added july'01
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Subject Name";
            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[0].Width = 200;
            FpSpread1.Sheets[0].Columns[1].Width = 200;
            FpSpread1.Sheets[0].Columns[2].Width = 100;
            FpSpread1.Sheets[0].Columns[3].Width = 80;
            FpSpread1.Sheets[0].Columns[4].Width = 80;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
            FarPoint.Web.Spread.ButtonCellType staf_butt1 = new FarPoint.Web.Spread.ButtonCellType("OneCommand", FarPoint.Web.Spread.ButtonType.PushButton, "Remove");
            FpSpread1.Sheets[0].Columns[4].CellType = staf_butt1;
            staf_butt1.Text = "Remove";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Room";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Credit";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Remove";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            //---------alternate subj shouldnt be same as subject
            int actrow = 0;
            int actcol = 0;
            string subjname_staffcode = string.Empty;
            string subjname = string.Empty;
            actrow = Fptimetable.ActiveSheetView.ActiveRow;
            actcol = Fptimetable.ActiveSheetView.ActiveColumn;
            if (actrow != -1)
            {
                // subjname_staffcode = fpdetails.Sheets[0].Cells[actrow, actcol].Text;
                subjname_staffcode = Fptimetable.Sheets[0].Cells[actrow, actcol].Text;
                string[] splitsubj = subjname_staffcode.Split(new Char[] { '-' });
                subjname = splitsubj[0].ToString();
                //-------------------
                string Syllabus_year = string.Empty;
                Syllabus_year = GetSyllabusYear(strbranch, strbatchyear, strsem);
                if (Syllabus_year != "-1")
                {
                    //--------------get subject type and subjects
                    string query = "select distinct subject.subtype_no,subject_type from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + strbranch + " and semester=" + strsem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + strbatchyear + ") order by subject.subtype_no";
                    DataSet dssubTypeRs = d2.select_method(query, hat, "Text");
                    TreeNode node;
                    int rec_count = 0;
                    if (dssubTypeRs.Tables.Count > 0 && dssubTypeRs.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dssubTypeRs.Tables[0].Rows.Count; i++)
                        {
                            if ((dssubTypeRs.Tables[0].Rows[i]["subject_type"].ToString()) != "0")
                            {
                                query = "select subject.subtype_no,subject_type,subject_no,subject_name,subject_code from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + strbranch + " and semester=" + strsem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + strbatchyear + ") and subject.subtype_no=" + dssubTypeRs.Tables[0].Rows[i]["subtype_no"].ToString() + " order by subject.subtype_no,subject.subject_no";
                                DataSet subTypeRs1 = d2.select_method(query, hat, "Text");
                                node = new TreeNode(dssubTypeRs.Tables[0].Rows[i]["subject_type"].ToString(), rec_count.ToString());
                                for (int j = 0; j < subTypeRs1.Tables[0].Rows.Count; j++)
                                {
                                    if (subTypeRs1.Tables[0].Rows[j]["subject_name"].ToString() != "0" && subTypeRs1.Tables[0].Rows[j]["subject_name"] != subjname)
                                    {
                                        node.ChildNodes.Add(new TreeNode(subTypeRs1.Tables[0].Rows[j]["subject_name"].ToString(), subTypeRs1.Tables[0].Rows[j]["subject_no"].ToString()));
                                        rec_count = rec_count + 1;
                                    }
                                }
                                subjtree.Nodes.Add(node);
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    private string GetSyllabusYear(string degree_code, string batch_year, string sem)
    {
        string syl_year = string.Empty;
        try
        {
            DataSet dssyl_year = d2.select_method("select syllabus_year from syllabus_master where degree_code=" + degree_code + " and semester =" + sem + " and batch_year=" + batch_year + " ", hat, "Text");
            if (dssyl_year.Tables[0].Rows.Count > 0)
            {
                if (dssyl_year.Tables[0].Rows[0]["syllabus_year"].ToString() == "\0")
                {
                    syl_year = "-1";
                }
                else
                {
                    syl_year = dssyl_year.Tables[0].Rows[0]["syllabus_year"].ToString();
                }
            }
            else
            {
                syl_year = "-1";
            }
        }
        catch (Exception ex)
        {
        }
        return syl_year;
    }

    protected void Fpclassadvisor_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        int ar = 0;
        ar = Fpclassadvisor.ActiveSheetView.ActiveRow;
        Fpclassadvisor.Sheets[0].RemoveRows(ar, 1);
        classadvisorsave();
    }

    public void classadvisorsave()
    {
        try
        {
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            if (strsec != "" && strsec != "-1" && strsec != "All")
            {
                strsec = "and sections='" + strsec + "'";
            }
            else
            {
                strsec = string.Empty;
            }
            string[] datespilt = txtdate.Text.Split('/');
            string temp_date = datespilt[1] + '/' + datespilt[0] + '/' + datespilt[2];
            string classadvisor = string.Empty;
            for (int i = 0; i < Fpclassadvisor.Sheets[0].RowCount; i++)
            {
                string[] spiltcheck = classadvisor.Split(',');
                Boolean chevalflag = false;
                for (int ch = 0; ch <= spiltcheck.GetUpperBound(0); ch++)
                {
                    if (Fpclassadvisor.Sheets[0].Cells[i, 2].Text.ToString() == spiltcheck[ch].ToString())
                    {
                        chevalflag = true;
                    }
                }
                if (chevalflag == false)
                {
                    if (classadvisor == "")
                    {
                        classadvisor = Fpclassadvisor.Sheets[0].Cells[i, 2].Text.ToString();
                    }
                    else
                    {
                        classadvisor = classadvisor + ',' + Fpclassadvisor.Sheets[0].Cells[i, 2].Text.ToString();
                    }
                }
            }
            if (Fpclassadvisor.Sheets[0].RowCount > 0)
            {
                Fpclassadvisor.Sheets[0].ColumnCount = 4;
                Fpclassadvisor.Sheets[0].ColumnHeader.RowCount = 1;
                Fpclassadvisor.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                Fpclassadvisor.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpclassadvisor.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
                Fpclassadvisor.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
                Fpclassadvisor.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Remove";
                Fpclassadvisor.Sheets[0].Columns[0].Width = 50;
                Fpclassadvisor.Sheets[0].Columns[1].Width = 220;
                Fpclassadvisor.Sheets[0].Columns[2].Width = 200;
                Fpclassadvisor.Sheets[0].Columns[3].Width = 100;
                FarPoint.Web.Spread.ButtonCellType staf_butt1 = new FarPoint.Web.Spread.ButtonCellType("OneCommand", FarPoint.Web.Spread.ButtonType.PushButton, "Remove");
                Fpclassadvisor.Sheets[0].Columns[3].CellType = staf_butt1;
                staf_butt1.Text = "Remove";
                Fpclassadvisor.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                Fpclassadvisor.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                Fpclassadvisor.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                Fpclassadvisor.Visible = true;
                Fpclassadvisor.Sheets[0].PageSize = Fpclassadvisor.Sheets[0].RowCount;
                Fpclassadvisor.Height = 20 + (Fpclassadvisor.Sheets[0].RowCount * 25);
                Fpclassadvisor.Width = 565;
                Fpclassadvisor.SaveChanges();
            }
            else
            {
                Fpclassadvisor.Visible = false;
            }
            //string schedukle = "update semester_schedule set class_advisor='" + classadvisor + "' where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedItem.ToString() + " and batch_year = " + strbatchyear + " and ttname='" + ddltimetable.SelectedItem.ToString() + "' and FromDate <='" + temp_date + "' " + strsec + "";
            //int insert = d2.update_method_wo_parameter(schedukle, "Text");
            //string[] cladv = classadvisor.Split(',');
            //classadvisor = cladv[cladv.Length - 1];
            string clupdate = "Update TT_ClassTimetable set TT_classAdvisor ='" + classadvisor + "' where TT_colCode='" + ddlcolg.SelectedValue + "' and TT_degCode='" + ddlbranch.SelectedValue + "' and TT_batchyear='" + ddlbatch.SelectedValue + "' and TT_sem='" + ddlsem.SelectedValue + "' and TT_sec='" + ddlsec.SelectedValue + "' and TT_name='" + ddltimetable.SelectedValue + "' and TT_date>='" + (txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) + "'";
            int ins = dirAcc.insertData(clupdate);
            if (ins > 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void FpSpread1_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        subjtree.Visible = true;
        FpSpread1.Visible = true;
        if (ddltimetable.SelectedItem.Text.Trim() == "New")
        {
            gridSelTT.Visible = true;
            bindGridTTSelect();
        }
        chkappend.Visible = true;
        btnOk.Visible = true;
        treepanel.Visible = true;
        int ar = 0;
        ar = FpSpread1.ActiveSheetView.ActiveRow;
        FpSpread1.Sheets[0].RemoveRows(ar, 1);
        txtmulstaff.Visible = false;
        lblmulstaff.Visible = false;
        pmulstaff.Visible = false;
        btnmulstaff.Visible = false;
    }

    protected void subjtree_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            txtmulstaff.Visible = false;
            lblmulstaff.Visible = false;
            btnCopySubject.Visible = false;
            pmulstaff.Visible = false;
            btnmulstaff.Visible = false;
            chkmullsstaff.Items.Clear();
            chkmulstaff.Checked = false;
            txtmulstaff.Text = "---Select---";
            chk_multisubj.Visible = false;
            FpSpread1.Visible = false;
            gridSelTT.Visible = false;
            subjtree.Visible = true;
            chkappend.Visible = false;
            btnOk.Visible = false;
            treepanel.Visible = true;
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec1 = string.Empty;
            int staf_cnt = 0;
            string staff_code = "", staff_name_code = string.Empty;
            FpSpread1.ActiveSheetView.AutoPostBack = false;
            int rowval = 0;
            string strsec = string.Empty;
            if (!chkSectionWise.Checked)
            {
                if (ddlsec.Enabled == true && ddlsec.Items.Count > 0)
                {
                    if (ddlsec.SelectedItem.ToString().Trim().ToLower() != "all" && ddlsec.SelectedItem.ToString().Trim().ToLower() != "" && ddlsec.SelectedItem.ToString().Trim().ToLower() != "-1" && ddlsec.SelectedItem.ToString().Trim().ToLower() != null)
                    {
                        strsec = " and sections='" + ddlsec.SelectedItem.ToString().Trim() + "'";
                        strsec1 = ddlsec.SelectedItem.ToString();
                    }
                }
            }
            int parent_count = subjtree.Nodes.Count;//----------count parent node value
            for (int i = 0; i < parent_count; i++)
            {
                for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                {
                    if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                    {
                        if (ddltimetable.SelectedItem.Text.Trim() == "New")
                        {
                            gridSelTT.Visible = true;
                            bindGridTTSelect();
                        }
                        else
                        {
                            //gridSelTT.Visible = false;
                            gridSelTT.Visible = true;
                            bindGridTTSelect();
                        }
                        FpSpread1.Visible = true;
                        subjtree.Visible = true;
                        chkappend.Visible = true;
                        btnOk.Visible = true;
                        treepanel.Visible = true;
                        string temp_sec = string.Empty;
                        if (strsec == "")
                        {
                            temp_sec = string.Empty;
                        }
                        else
                        {
                            temp_sec = " and Sections='" + strsec1 + "'";
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
                            //Added by Idhris
                            string nohrofsub = dirAcc.selectScalarString(" select noofhrsperweek from subject where subject_no='" + Convert.ToInt32(chile_index) + "'");
                            FpSpread1.Sheets[0].Cells[rowval, 3].Text = nohrofsub;
                            FpSpread1.Sheets[0].Cells[rowval, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[rowval, 3].Locked = true;
                            string isLab = dirAcc.selectScalarString("select isnull(ss.Lab,'0') from subject s,sub_sem ss where s.syll_code=ss.syll_code and s.subType_no=ss.subType_no and s.subject_no='" + Convert.ToInt32(chile_index) + "'");
                            DataTable roomDet = getRoomDetails(chile_index, isLab);
                            if (roomDet.Rows.Count > 0)
                            {
                                string[] strRoomList = roomDet.AsEnumerable().Select(r => r.Field<string>("Room_Name")).ToArray<string>();
                                FarPoint.Web.Spread.ComboBoxCellType roomCombo = new FarPoint.Web.Spread.ComboBoxCellType(strRoomList);
                                roomCombo.AutoPostBack = true;
                                FpSpread1.Sheets[0].Cells[rowval, 2].CellType = roomCombo;
                                FpSpread1.Sheets[0].Cells[rowval, 2].Locked = false;
                            }
                            //DataSet staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "' and ct.TT_sec='" + ddlsec.SelectedItem.ToString() + "'  and TT_date>='" + (txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) + "')");
                            //if (true)
                            //{
                            //    staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "' ) ");
                            //}
                            //if (ddltimetable.SelectedItem.Text.Trim() == "New")
                            //{
                            //    //staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "' and ct.TT_sec='" + ddlsec.SelectedItem.ToString() + "') ");
                            //    staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "')  ");
                            //}
                            //Ended by Idhris
                            //--------------bind staff name into the spread
                            DataSet staf_set = d2.select_method("select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year=" + strbatchyear + "  " + temp_sec + ")", hat, "Text");
                            if (staf_set.Tables[0].Rows.Count > 1)
                            {
                                txtmulstaff.Visible = true;
                                lblmulstaff.Visible = true;
                                pmulstaff.Visible = true;
                                btnmulstaff.Visible = true;
                                btnCopySubject.Visible = true;
                                string[] staff_list = new string[staf_set.Tables[0].Rows.Count + 1];
                                for (staf_cnt = 0; staf_cnt < staf_set.Tables[0].Rows.Count; staf_cnt++)
                                {
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
                                FarPoint.Web.Spread.ComboBoxCellType staf_combo = new FarPoint.Web.Spread.ComboBoxCellType(staff_list);
                                FpSpread1.Sheets[0].Cells[rowval, 1].CellType = staf_combo;
                            }
                            else
                            {
                                btnCopySubject.Visible = true;
                                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                                FpSpread1.Sheets[0].Cells[rowval, 1].CellType = txt;
                                staff_code = staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                staff_name_code = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                FpSpread1.Sheets[0].Cells[rowval, 1].Text = staff_name_code;
                                FpSpread1.Sheets[0].Cells[rowval, 1].Locked = true;
                            }
                            FpSpread1.Sheets[0].Cells[rowval, 1].Tag = staff_code;
                            FpSpread1.Sheets[0].Cells[rowval, 1].Value = staff_name_code;
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
                            DataSet staf_set = d2.select_method("select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year=" + strbatchyear.ToString() + " " + temp_sec + ")", hat, "Text");
                            //Idhris
                            string nohrofsub = dirAcc.selectScalarString(" select noofhrsperweek from subject where subject_no='" + Convert.ToInt32(chile_index) + "'");
                            FpSpread1.Sheets[0].Cells[rowval, 3].Text = nohrofsub;
                            FpSpread1.Sheets[0].Cells[rowval, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[rowval, 3].Locked = true;
                            string isLab = dirAcc.selectScalarString("select isnull(ss.Lab,'0') from subject s,sub_sem ss where s.syll_code=ss.syll_code and s.subType_no=ss.subType_no and s.subject_no='" + Convert.ToInt32(chile_index) + "'");
                            DataTable roomDet = getRoomDetails(chile_index, isLab);
                            if (roomDet.Rows.Count > 0)
                            {
                                string[] strRoomList = roomDet.AsEnumerable().Select(r => r.Field<string>("Room_Name")).ToArray<string>();
                                FarPoint.Web.Spread.ComboBoxCellType roomCombo = new FarPoint.Web.Spread.ComboBoxCellType(strRoomList);
                                roomCombo.AutoPostBack = true;
                                FpSpread1.Sheets[0].Cells[rowval, 2].CellType = roomCombo;
                                FpSpread1.Sheets[0].Cells[rowval, 2].Locked = false;
                            }
                            //DataSet staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "' and ct.TT_sec='" + ddlsec.SelectedItem.ToString() + "'  and TT_date>='" + (txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) + "')");
                            //if (true)
                            //{
                            //    staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "' ) ");
                            //}
                            //if (ddltimetable.SelectedItem.Text.Trim() == "New")
                            //{
                            //    //staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "' and ct.TT_sec='" + ddlsec.SelectedItem.ToString() + "') ");
                            //    staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "')  ");
                            //}
                            if (staf_set.Tables[0].Rows.Count > 1)
                            {
                                txtmulstaff.Visible = true;
                                lblmulstaff.Visible = true;
                                pmulstaff.Visible = true;
                                btnmulstaff.Visible = true;
                                btnCopySubject.Visible = true;
                                string[] staff_list = new string[staf_set.Tables[0].Rows.Count + 1];
                                for (staf_cnt = 0; staf_cnt < staf_set.Tables[0].Rows.Count; staf_cnt++)
                                {
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
                                FarPoint.Web.Spread.ComboBoxCellType staf_combo = new FarPoint.Web.Spread.ComboBoxCellType(staff_list);
                                staf_combo.AutoPostBack = true;
                                FpSpread1.Sheets[0].Cells[rowval, 1].CellType = staf_combo;
                                FpSpread1.Sheets[0].Cells[rowval, 1].Locked = false;
                            }
                            else
                            {
                                btnCopySubject.Visible = true;
                                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                                FpSpread1.Sheets[0].Cells[rowval, 1].CellType = txt;
                                staff_code = staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                staff_name_code = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                FpSpread1.Sheets[0].Cells[rowval, 1].Text = staff_name_code;
                                FpSpread1.Sheets[0].Cells[rowval, 1].Locked = true;
                            }
                            FpSpread1.Sheets[0].Cells[rowval, 1].Tag = staff_code;
                            FpSpread1.Sheets[0].Cells[rowval, 1].Value = staff_name_code;
                            treepanel.Visible = true;
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Visible = true;
                        if (ddltimetable.SelectedItem.Text.Trim() == "New")
                        {
                            if (ddltimetable.SelectedItem.Text.Trim() == "New")
                            {
                                gridSelTT.Visible = true;
                                bindGridTTSelect();
                            }
                            else
                            {
                                //gridSelTT.Visible = false;
                                gridSelTT.Visible = true;
                                bindGridTTSelect();
                            }
                        }
                        FpSpread1.SaveChanges();
                    }
                }
            }
        }
        catch
        {
        }
    }

    private DataTable getRoomDetails(string subNo, string isLab)
    {
        DataTable dtRoomDet = new DataTable();
        try
        {
            string degCode = ddlbranch.SelectedValue.ToString();
            string seleQ = string.Empty;
            dtRoomDet = dirAcc.selectDataTable(seleQ);
            string deptCode = string.Empty;
            if (!string.IsNullOrEmpty(degCode))
            {
                DataTable dtDept = dirAcc.selectDataTable("select distinct dt.Dept_Code from Degree dg,Department dt,Course c where c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and dg.Degree_Code='" + degCode + "'");
                List<decimal> list = dtDept.AsEnumerable().Select(r => r.Field<decimal>("Dept_Code"))
                                                       .ToList();
                deptCode = string.Join("%", list.ToArray());
            }
            if (isLab.ToLower().Trim() == "true")
            {
                seleQ = " select  cast(r.RoomPK as varchar) as RoomPK,Dept_Code,Room_type,Room_Name from room_detail r where  r.RoomPk in ( select s.roompk from subject s,sub_sem ss where s.syll_code=ss.syll_code and s.subType_no=ss.subType_no and ss.Lab='1' and s.subject_no='" + subNo + "') order by r.RoomPK asc";
                dtRoomDet = dirAcc.selectDataTable(seleQ);
                if (dtRoomDet.Rows.Count == 0)
                {
                    seleQ = " select  cast(RoomPK as varchar) as RoomPK,Dept_Code,Room_type,Room_Name from room_detail where Room_Type='Lab' and (Dept_Code like '%" + deptCode + "' or Dept_Code like '" + deptCode + "%' or Dept_Code like '%" + deptCode + "%' or Dept_Code = '" + deptCode + "')  order by RoomPK asc";
                    dtRoomDet = dirAcc.selectDataTable(seleQ);
                }
            }
            else
            {
                seleQ = " select  cast(RoomPK as varchar) as RoomPK,Dept_Code,Room_type,Room_Name from room_detail where Room_Type='Hall' and (Dept_Code like '%" + deptCode + "' or Dept_Code like '" + deptCode + "%' or Dept_Code like '%" + deptCode + "%' or Dept_Code = '" + deptCode + "')  order by RoomPK asc";
                dtRoomDet = dirAcc.selectDataTable(seleQ);
            }
        }
        catch { dtRoomDet.Clear(); }
        return dtRoomDet;
    }

    private string getRoomPk(string roomName)
    {
        string roomVal = "0";
        string degCode = ddlbranch.SelectedValue;
        try
        {
            string seleQ = " select  cast(RoomPK as varchar) as RoomPK,Dept_Code,Room_type,Room_Name from room_detail where  Room_Name='" + roomName + "'";
            //(Dept_Code like '%" + degCode + "' or Dept_Code like '" + degCode + "%' or Dept_Code like '%" + degCode + "%' or Dept_Code = '" + degCode + "') and
            roomVal = dirAcc.selectScalarInt(seleQ).ToString();
        }
        catch { roomVal = "0"; }
        return roomVal;
    }

    private void bindGridTTSelect()
    {
        DataTable dtTTSel = new DataTable();
        dtTTSel.Columns.Add("Day");
        dtTTSel.Columns.Add("DayVal");
        dtTTSel.Columns.Add("H1");
        dtTTSel.Columns.Add("H2");
        dtTTSel.Columns.Add("H3");
        dtTTSel.Columns.Add("H4");
        dtTTSel.Columns.Add("H5");
        dtTTSel.Columns.Add("H6");
        dtTTSel.Columns.Add("H7");
        dtTTSel.Columns.Add("H8");
        dtTTSel.Columns.Add("H9");
        dtTTSel.Columns.Add("H10");
        for (int i = 1; i < 7; i++)
        {
            DataRow dr = dtTTSel.NewRow();
            string day = string.Empty;
            switch (i)
            {
                case 1:
                    day = "Monday";
                    break;
                case 2:
                    day = "Tuesday";
                    break;
                case 3:
                    day = "Wednesday";
                    break;
                case 4:
                    day = "Thursday";
                    break;
                case 5:
                    day = "Friday";
                    break;
                case 6:
                    day = "Saturday";
                    break;
                case 7:
                    day = "Sunday";
                    break;
            }
            dr["Day"] = day;
            dr["DayVal"] = i;
            dtTTSel.Rows.Add(dr);
        }
        gridSelTT.DataSource = dtTTSel;
        gridSelTT.DataBind();
    }

    protected void chk_multisubj_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txtmultisubj.Visible = true;
            pnlmultisubj.Visible = true;
            if (chk_multisubj.Checked == true)// && chklistmultisubj .Items.Count>0)
            {
                txtmultisubj.Enabled = true;
                if (FpSpread1.Sheets[0].ActiveRow >= 0)
                {
                    string staff_name_code = string.Empty;
                    staff_name_code = FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].ActiveRow), 1].Value.ToString();
                    string[] staff_name_code_spt = staff_name_code.Split(';');
                    for (int many_staff = 0; many_staff <= staff_name_code_spt.GetUpperBound(0); many_staff++)
                    {
                        chklistmultisubj.Items.Add(staff_name_code_spt[many_staff]);
                    }
                }
            }
            else
            {
                txtmultisubj.Visible = false;
                pnlmultisubj.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklistmultisubj_selectedindetxchange(object sender, EventArgs e)
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

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrMsg.Text = string.Empty;
            string subj_number = string.Empty;
            string splval = string.Empty; string splval_temp = string.Empty;
            string subno_staff = string.Empty; string subno_staffnote = string.Empty;
            string staffname = string.Empty; string staff_name_code = string.Empty; string staffcode = string.Empty;
            btnsave.Visible = true;
            btnsave.Enabled = true;
            string SqlFinal = string.Empty;
            string[] date = txtdate.Text.Split('/');
            string fromdate = date[1] + '/' + date[0] + '/' + date[2];
            string activerow = Fptimetable.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fptimetable.ActiveSheetView.ActiveColumn.ToString();
            string coursename = string.Empty;
            string acrnym = string.Empty;
            string ster = string.Empty;
            string btch = string.Empty;
            string sctn = string.Empty;
            string history_data = string.Empty;
            string strsec = string.Empty;
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.Items.Count > 0)
                {
                    if (ddlsec.SelectedItem.ToString().Trim() != "" && ddlsec.SelectedItem.ToString().Trim() != "-1" && ddlsec.SelectedItem.ToString() != null && ddlsec.SelectedItem.ToString().Trim().ToLower() != "all")
                    {
                        strsec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                    }
                }
            }
            //string fp_staffcode =string.Empty;
            //fp_staffcode = Fptimetable.Sheets[0].GetText(Convert.ToInt32(activerow), Convert.ToInt32(activecol));
            //string[] fp_staff_code = fp_staffcode.Split('-');
            bool invisi = false;
            if (chk_multisubj.Checked == false)
            {
                for (int rowcnt = 0; rowcnt <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; rowcnt++)
                {
                    FpSpread1.SaveChanges();
                    staff_name_code = Convert.ToString(FpSpread1.Sheets[0].GetText(rowcnt, 1));
                    if (staff_name_code == "" || staff_name_code == "System.Object")//-----------check wether the staff name selected or not
                    {
                        subjtree.Visible = true;
                        FpSpread1.Visible = true;
                        if (ddltimetable.SelectedItem.Text.Trim() == "New")
                        {
                            gridSelTT.Visible = true;
                            bindGridTTSelect();
                        }
                        chkappend.Visible = true;
                        btnOk.Visible = true;
                        treepanel.Visible = true;
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "Select Staff name";
                        return;
                    }
                    else
                    {
                        invisi = true;
                    }
                    //else
                    //{
                    //    btnsave.Enabled = true;
                    //    subjtree.Visible = false;
                    //    FpSpread1.Visible = false;
                    //    chkappend.Visible = false;
                    //    btnOk.Visible = false;
                    //    treepanel.Visible = false;
                    //    lblErrMsg.Text = string.Empty;
                    //    lblErrMsg.Visible = false; ;
                    //}
                }
                if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) == 0)//------------message for select the subject from the tree
                {
                    subjtree.Visible = true;
                    FpSpread1.Visible = true;
                    if (ddltimetable.SelectedItem.Text.Trim() == "New")
                    {
                        gridSelTT.Visible = true;
                        bindGridTTSelect();
                    }
                    chkappend.Visible = true;
                    btnOk.Visible = true;
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Select Subject name for alternate schedule from tree view";
                    return;
                }
                for (int row_cnt = 0; row_cnt <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; row_cnt++)
                {
                    staff_name_code = Convert.ToString(FpSpread1.Sheets[0].GetText(row_cnt, 1));
                    if (staff_name_code == null)
                    {
                        staff_name_code = string.Empty;
                    }
                    if (staff_name_code != "All")
                    {
                        string[] staff_name_code_spt = staff_name_code.Split('-');
                        if (staff_name_code_spt.GetUpperBound(0) > 0)
                        {
                            staffname = staff_name_code_spt[0].ToString();
                            staffcode = staff_name_code_spt[1].ToString();
                            if (staff_name_code_spt.GetUpperBound(0) > 1)
                            {
                                staffcode = FpSpread1.Sheets[0].Cells[row_cnt, 1].Tag.ToString();
                            }
                        }
                    }
                    else
                    {
                        staffcode = string.Empty;
                        string strquery = "select staff_code from staff_selector where subject_no='" + FpSpread1.Sheets[0].Cells[row_cnt, 0].Tag + "' " + strsec + "";
                        DataSet dssubstaff = d2.select_method_wo_parameter(strquery, "text");
                        if (dssubstaff.Tables[0].Rows.Count > 0)
                        {
                            for (int st = 0; st < dssubstaff.Tables[0].Rows.Count; st++)
                            {
                                if (staffcode == "")
                                {
                                    staffcode = dssubstaff.Tables[0].Rows[st]["staff_code"].ToString();
                                }
                                else
                                {
                                    staffcode = staffcode + '-' + dssubstaff.Tables[0].Rows[st]["staff_code"].ToString();
                                }
                            }
                        }
                        // staffcode = FpSpread1.Sheets[0].Cells[row_cnt, 1].Tag.ToString();
                    }
                    // Added by sridhar 11.07.2014
                    if (allowcom == false)
                    {
                        string[] staffcode_check = staffcode.Split('-');
                        if (activecol != "0" && activerow != "-1")
                        {
                            int row = Convert.ToInt32(activerow);
                            int col = Convert.ToInt32(activecol);
                            if (row == 0)
                            {
                                tablevalue = "mon" + col + "";
                            }
                            else if (row == 1)
                            {
                                tablevalue = "tue" + col + "";
                            }
                            else if (row == 2)
                            {
                                tablevalue = "wed" + col + "";
                            }
                            else if (row == 3)
                            {
                                tablevalue = "thu" + col + "";
                            }
                            else if (row == 4)
                            {
                                tablevalue = "fri" + col + "";
                            }
                            else if (row == 5)
                            {
                                tablevalue = "sat" + col + "";
                            }
                            //string getRights = d2.GetFunction("select value from Master_Settings where  settings='Time Table Alert Rights'");
                            //if (getRights.Trim() == "0" || String.IsNullOrEmpty(getRights))
                            //{
                            for (int i = 0; i <= staffcode_check.GetUpperBound(0); i++)
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
                                srids.Clear();
                                srids = srida.select_method_wo_parameter(SqlFinal, "Text");
                                for (int j = 0; j < srids.Tables[0].Rows.Count; j++)
                                {
                                    btch = srids.Tables[0].Rows[j]["batch_year"].ToString();
                                    string dgre = srids.Tables[0].Rows[j]["degree_code"].ToString();
                                    ster = srids.Tables[0].Rows[j]["semester"].ToString();
                                    sctn = srids.Tables[0].Rows[j]["Sections"].ToString();
                                    acrnym = srids.Tables[0].Rows[j]["Acronym"].ToString();
                                    coursename = srids.Tables[0].Rows[j]["Course_Name"].ToString();
                                    if (!hatdegree.ContainsKey(btch + '-' + dgre + '-' + ster + '-' + sctn))
                                    {
                                        hatdegree.Add(btch + '-' + dgre + '-' + ster + '-' + sctn, btch + '-' + dgre + '-' + ster + '-' + sctn);
                                        // loadstaffinfo();
                                        // string slq = "select * from Semester_Schedule where batch_year='" + btch + "' and semester ='" + ster + "' and degree_code='" + dgre + "' and Sections='" + sctn + "' and " + tablevalue + " like '%" + staff_code + "%' and FromDate = '" + fromdate + "' ";
                                        string slq = "select top 1 * from Semester_Schedule where batch_year='" + btch + "' and semester ='" + ster + "' and degree_code='" + dgre + "' and Sections='" + sctn + "' and FromDate <= '" + fromdate + "' ORDER BY FromDate desc";
                                        string rept = string.Empty;
                                        ds.Clear();
                                        //ds = srida.select_method_wo_parameter(slq, "Text");
                                        for (int tblI = 0; tblI < ds.Tables.Count; tblI++)
                                        {
                                            ds.Tables.RemoveAt(0);
                                        }
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
                                                    //history_data = history_data + ";" + btch + "-" + coursename + "-" + acrnym + "-" + ster + "th Sem -" + sctn + "Sec";
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
                            mpemsgboxupdate.Show();
                            Label1.Text = "The Staff " + staffname + " is BUSY in " + history_data + " - Do you want to Schedule the Class Anyway?";
                        }
                        else
                        {
                            mpemsgboxupdate.Hide();
                            btnupOk_Click(sender, e);
                            //allowcom = true;
                            //btnOk_Click(sender, e);
                        }
                        goto golabel;
                    }
                    //............................................................................................................................
                    subj_number = FpSpread1.Sheets[0].Cells[row_cnt, 0].Tag.ToString();
                    string roomName = FpSpread1.Sheets[0].Cells[row_cnt, 2].Text.ToString();
                    if (splval == "")
                    {
                        string val = "S";
                        string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subj_number.ToString() + "'");
                        if (subj_type == "1" || subj_type.ToLower().Trim() == "true")
                        {
                            val = "L";
                        }
                        if (allowcom == true)
                        {
                            val = "C";
                        }
                        splval = (d2.GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                        string[] staffs = staffcode.Split('-');
                        foreach (string stf in staffs)
                        {
                            subno_staff += subj_number + "-" + stf + "-" + val + "-" + roomName + ";";
                            subno_staffnote += (d2.GetFunction("select  case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no=" + subj_number.ToString() + " ") + "-" + stf + "-" + "" + val + "-" + roomName + ";");
                        }
                    }
                    else
                    {
                        string val = "S";
                        string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subj_number.ToString() + "'");
                        if (subj_type == "1" || subj_type.Trim().ToLower() == "true")
                        {
                            val = "L";
                        }
                        if (allowcom == true)
                        {
                            val = "C";
                        }
                        splval = splval + ";" + (d2.GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                        //subno_staff = subno_staff + ";" + subj_number + "-" + staffcode + "-" + val + "";
                        //subno_staffnote = subno_staffnote + ";" + (d2.GetFunction("select  case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                        string[] staffs = staffcode.Split('-');
                        foreach (string stf in staffs)
                        {
                            subno_staff += subj_number + "-" + stf + "-" + val + "-" + roomName + ";";
                            subno_staffnote += (d2.GetFunction("select case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no=" + subj_number.ToString() + " ") + "-" + stf + "-" + "" + val + "-" + roomName + ";");
                        }
                    }
                }
            }
            else
            {
                string roomName = string.Empty;
                for (int row_cnt = 0; row_cnt <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; row_cnt++)
                {
                    for (int chk_cnt = 0; chk_cnt < chklistmultisubj.Items.Count; chk_cnt++)
                    {
                        if (chklistmultisubj.Items[chk_cnt].Selected == true)
                        {
                            staff_name_code = chklistmultisubj.Items[chk_cnt].Text;
                            string[] staff_name_code_spt = staff_name_code.Split('-');
                            staffname = staff_name_code_spt[0].ToString();
                            staffcode = staff_name_code_spt[1].ToString();
                            subj_number = FpSpread1.Sheets[0].Cells[row_cnt, 0].Tag.ToString();
                            roomName = FpSpread1.Sheets[0].Cells[row_cnt, 2].Text.ToString();
                            if (splval_temp == "")
                            {
                                splval_temp = staffcode;// subj_number + "-" + staffcode + "-S";
                            }
                            else
                            {
                                splval_temp = splval_temp + "-" + staffcode;// subj_number + "-" + staffcode + "-S";
                            }
                        }
                    }
                    if (splval == "")
                    {
                        string val = "S";
                        string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subj_number.ToString() + "'");
                        if (subj_type == "1" || subj_type.Trim().ToLower() == "true")
                        {
                            val = "L";
                        }
                        if (allowcom == true)
                        {
                            val = "C";
                        }
                        splval = (d2.GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                        //subno_staff = subj_number + "-" + splval_temp + "-" + val + "";
                        //subno_staffnote = d2.GetFunction("select  case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "";
                        string[] staffs = staffcode.Split('-');
                        foreach (string stf in staffs)
                        {
                            subno_staff += subj_number + "-" + stf + "-" + val + "-" + roomName + ";";
                            subno_staffnote += (d2.GetFunction("select  case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no=" + subj_number.ToString() + " ") + "-" + stf + "-" + "" + val + "-" + roomName + ";");
                        }
                    }
                    else
                    {
                        string val = "S";
                        string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subj_number.ToString() + "'");
                        if (subj_type == "1" || subj_type.Trim().ToLower() == "true")
                        {
                            val = "L";
                        }
                        if (allowcom == true)
                        {
                            val = "C";
                        }
                        splval = splval + ";" + (d2.GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "-" + roomName + "");
                        //subno_staff = subno_staff + ";" + subj_number + "-" + splval_temp + "-" + val + "";
                        //subno_staffnote = subno_staffnote + ";" + (d2.GetFunction("select  case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                        string[] staffs = staffcode.Split('-');
                        foreach (string stf in staffs)
                        {
                            subno_staff += subj_number + "-" + stf + "-" + val + "-" + roomName + ";";
                            subno_staffnote += (d2.GetFunction("select  case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no=" + subj_number.ToString() + " ") + "-" + stf + "-" + "" + val + "-" + roomName + ";");
                        }
                    }
                }
            }
            int x = Fptimetable.ActiveSheetView.ActiveRow;
            int y = Fptimetable.ActiveSheetView.ActiveColumn;
            // Fptimetable.Sheets[0].Cells[x, y + 1].Text = splval.ToString();
            if (y > 0)
            {
                int noofHrs = dirAcc.selectScalarInt("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code='" + ddlbranch.SelectedValue + "' and semester='" + ddlsem.SelectedValue + "'");
                if (gridSelTT.Visible)
                {
                    for (int rowI = 0; rowI < 6; rowI++)
                    {
                        for (int colI = 1; colI <= noofHrs; colI++)
                        {
                            DropDownList ddlVal = (DropDownList)gridSelTT.Rows[rowI].FindControl("ddlH" + colI);
                            if (ddlVal.SelectedIndex == 1)
                            {
                                if (Fptimetable.Sheets[0].RowCount > rowI)
                                {
                                    Fptimetable.Sheets[0].Cells[rowI, colI].Text = subno_staffnote.ToString();
                                    Fptimetable.Sheets[0].Cells[rowI, colI].Note = subno_staff.ToString();
                                    FarPoint.Web.Spread.SheetView sv = Fptimetable.ActiveSheetView;
                                    sv.ActiveColumn = y;
                                    sv.ActiveRow = x;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Fptimetable.Sheets[0].Cells[x, y].Text = subno_staffnote.ToString();
                    Fptimetable.Sheets[0].Cells[x, y].Note = subno_staff.ToString();
                    FarPoint.Web.Spread.SheetView sv = Fptimetable.ActiveSheetView;
                    sv.ActiveColumn = y;
                    sv.ActiveRow = x;
                }
            }
            if (invisi)
            {
                //else
                //    {
                btnsave.Enabled = true;
                subjtree.Visible = false;
                FpSpread1.Visible = false;
                gridSelTT.Visible = false;
                chkappend.Visible = false;
                btnOk.Visible = false;
                treepanel.Visible = false;
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Visible = false; ;
                // }
            }
        golabel: ;
            //Modified By srinath 25/8/2015 For KCG======
            if (ddltimetable.SelectedItem.ToString().Trim() != "New")
            {
                //New time table save Idhris
                bool saved = false;
                string hour = activecol;
                string daypk = (Convert.ToInt32(activerow) + 1).ToString();
                string ttpk = dirAcc.selectScalarString("select TT_ClassPK from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + ddlcolg.SelectedValue + "' and ct.TT_degCode='" + ddlbranch.SelectedValue + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + ddlsem.SelectedValue + "' and ct.TT_sec='" + ddlsec.SelectedValue + "' and ct.TT_name='" + ddltimetable.SelectedItem.Text + "'  and TT_date>='" + (txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) + "'");
                string delQ = "if exists ( select TT_ClassDetPK from TT_ClassTimetableDet where TT_Hour ='" + hour + "' and TT_Day='" + daypk + "' and TT_ClassFK='" + ttpk + "' ) delete from TT_ClassTimetableDet  where TT_Hour ='" + hour + "' and TT_Day='" + daypk + "' and TT_ClassFK='" + ttpk + "'";
                string[] substaffrooms = subno_staff.Split(';');
                foreach (string substaffroom in substaffrooms)
                {
                    string[] res = substaffroom.Split('-');
                    if (res.Length != 4)
                        continue;
                    string roomFk = getRoomPk(res[3]);
                    if (!string.IsNullOrEmpty(ttpk))
                    {
                        //string insUpd = " if exists ( select TT_ClassDetPK from TT_ClassTimetableDet where TT_Hour ='" + hour + "' and TT_Day='" + daypk + "' and TT_ClassFK='" + ttpk + "' ) update TT_ClassTimetableDet  set TT_subno='" + res[0] + "',TT_staffcode='" + res[1] + "'  where TT_Hour ='" + hour + "' and TT_Day='" + daypk + "' and TT_ClassFK='" + ttpk + "' else insert into TT_ClassTimetableDet (TT_Hour,TT_Day,TT_ClassFK,TT_subno,TT_staffcode,TT_Room) values ('" + hour + "','" + daypk + "','" + ttpk + "','" + res[0] + "','" + res[1] + "','" + roomFk + "') ";
                        string insUpd = " insert into TT_ClassTimetableDet (TT_Hour,TT_Day,TT_ClassFK,TT_subno,TT_staffcode,TT_Room) values ('" + hour + "','" + daypk + "','" + ttpk + "','" + res[0] + "','" + res[1] + "','" + roomFk + "') ";
                        int a = dirAcc.updateData(insUpd);
                        if (a > 0)
                        {
                            saved = true;
                        }
                    }
                }
                if (saved)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    //protected void btnuupdetails_Click(object sender, EventArgs e)
    //{
    //    fp1.Visible = true;
    //    pnlsp1.Visible = true;
    //   // loadstaffinfo();
    //}

    protected void btnupOk_Click(object sender, EventArgs e)
    {
        mpemsgboxupdate.Hide();
        try
        {
            allowcom = true;
            btnOk_Click(sender, e);
        }
        catch
        {
        }
    }

    //protected void exitpop_Click1(object sender, EventArgs e)
    //{
    //    pnlsp1.Visible = false;
    //}

    protected void btnupCancel_Click(object sender, EventArgs e)
    {
        try
        {
            mpemsgboxupdate.Hide();
            return;
        }
        catch
        {
        }
    }

    //public void loadstaffinfo()
    //{
    //    try
    //    {
    //        fp1.Sheets[0].RowCount = 0;
    //        fp1.Sheets[0].RowHeader.Visible = false;
    //        fp1.CommandBar.Visible = false;
    //        fp1.Sheets[0].ColumnCount = 6;
    //        if (srids.Tables[0].Rows.Count > 0)
    //        {
    //            fp1.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
    //            fp1.Sheets[0].ColumnHeader.Columns[0].Font.Size = FontUnit.Medium;
    //            fp1.Sheets[0].ColumnHeader.Columns[0].Font.Name = "Book Antiqua";
    //            fp1.ActiveSheetView.ColumnHeader.Columns[0].Font.Bold = true;
    //            fp1.Sheets[0].ColumnHeader.Columns[1].Label = "Batch_Year";
    //            fp1.Sheets[0].ColumnHeader.Columns[1].Font.Size = FontUnit.Medium;
    //            fp1.Sheets[0].ColumnHeader.Columns[1].Font.Name = "Book Antiqua";
    //            fp1.ActiveSheetView.ColumnHeader.Columns[1].Font.Bold = true;
    //            fp1.Sheets[0].ColumnHeader.Columns[2].Label = "Degree";
    //            fp1.Sheets[0].ColumnHeader.Columns[2].Font.Size = FontUnit.Medium;
    //            fp1.Sheets[0].ColumnHeader.Columns[2].Font.Name = "Book Antiqua";
    //            fp1.ActiveSheetView.ColumnHeader.Columns[2].Font.Bold = true;
    //            fp1.Sheets[0].ColumnHeader.Columns[3].Label = "Branch";
    //            fp1.Sheets[0].ColumnHeader.Columns[3].Font.Size = FontUnit.Medium;
    //            fp1.Sheets[0].ColumnHeader.Columns[3].Font.Name = "Book Antiqua";
    //            fp1.ActiveSheetView.ColumnHeader.Columns[3].Font.Bold = true;
    //            fp1.Sheets[0].ColumnHeader.Columns[4].Label = "Sem";
    //            fp1.Sheets[0].ColumnHeader.Columns[4].Font.Size = FontUnit.Medium;
    //            fp1.Sheets[0].ColumnHeader.Columns[4].Font.Name = "Book Antiqua";
    //            fp1.ActiveSheetView.ColumnHeader.Columns[4].Font.Bold = true;
    //            fp1.Sheets[0].ColumnHeader.Columns[5].Label = "Section";
    //            fp1.Sheets[0].ColumnHeader.Columns[5].Font.Size = FontUnit.Medium;
    //            fp1.Sheets[0].ColumnHeader.Columns[5].Font.Name = "Book Antiqua";
    //            fp1.ActiveSheetView.ColumnHeader.Columns[5].Font.Bold = true;
    //            fp1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
    //            fp1.Sheets[0].DefaultStyle.Font.Bold = false;
    //            fp1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
    //            fp1.Sheets[0].DefaultStyle.Border.BorderSizeBottom = 0;
    //            fp1.Sheets[0].DefaultStyle.Border.BorderSizeRight = 0;
    //            fp1.Sheets[0].Columns[0].Width = 60;
    //            fp1.Sheets[0].Columns[1].Width = 100;
    //            fp1.Sheets[0].Columns[2].Width = 100;
    //            fp1.Sheets[0].Columns[3].Width = 120;
    //            fp1.Sheets[0].Columns[4].Width = 60;
    //            fp1.Sheets[0].Columns[5].Width = 100;
    //            for (int k = 0; k < 6; k++)
    //            {
    //                fp1.Sheets[0].Columns[k].Locked = true;
    //            }
    //            fp1.Width = 410;
    //            fp1.Height = 240;
    //            int sno = 0;
    //            fp1.Sheets[0].RowCount = srids.Tables[0].Rows.Count;
    //            fp1.Sheets[0].ColumnCount = 6;
    //            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
    //            for (int i = 0; i < srids.Tables[0].Rows.Count; i++)
    //            {
    //                sno++;
    //                fp1.Sheets[0].Cells[i, 0].Text = Convert.ToString(sno);
    //                fp1.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
    //                fp1.Sheets[0].Cells[i, 0].VerticalAlign = VerticalAlign.Middle;
    //                fp1.Sheets[0].Cells[i, 1].Text = srids.Tables[0].Rows[i]["Batch_Year"].ToString();
    //                fp1.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Center;
    //                fp1.Sheets[0].Cells[i, 1].VerticalAlign = VerticalAlign.Middle;
    //                fp1.Sheets[0].Cells[i, 2].Text = srids.Tables[0].Rows[i]["Course_Name"].ToString();
    //                fp1.Sheets[0].Cells[i, 2].HorizontalAlign = HorizontalAlign.Center;
    //                fp1.Sheets[0].Cells[i, 2].VerticalAlign = VerticalAlign.Middle;
    //                fp1.Sheets[0].Cells[i, 3].Text = srids.Tables[0].Rows[i]["Acronym"].ToString();
    //                fp1.Sheets[0].Cells[i, 3].HorizontalAlign = HorizontalAlign.Center;
    //                fp1.Sheets[0].Cells[i, 3].VerticalAlign = VerticalAlign.Middle;
    //                fp1.Sheets[0].Cells[i, 3].CellType = txt;
    //                fp1.Sheets[0].Cells[i, 4].Text = srids.Tables[0].Rows[i]["semester"].ToString();
    //                fp1.Sheets[0].Cells[i, 4].HorizontalAlign = HorizontalAlign.Center;
    //                fp1.Sheets[0].Cells[i, 4].VerticalAlign = VerticalAlign.Middle;
    //                fp1.Sheets[0].Cells[i, 5].Text = srids.Tables[0].Rows[i]["Sections"].ToString();
    //                fp1.Sheets[0].Cells[i, 5].HorizontalAlign = HorizontalAlign.Center;
    //                fp1.Sheets[0].Cells[i, 5].VerticalAlign = VerticalAlign.Middle;
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    protected void btnsave_Click(object sender, EventArgs e)
    {
        //Idhris
        try
        {
            string[] date = txtdate.Text.Split('/');
            string fromdate = date[1] + '/' + date[0] + '/' + date[2];
            errmsg.Visible = false;
            string[] Days = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            string strsec = string.Empty;
            string section = string.Empty;
            string ttname = string.Empty;
            btndelete.Enabled = false;
            string qryStaffSection = string.Empty;
            string staffSectionName = string.Empty;
            if (ddlsec.Enabled == true && ddlsec.Items.Count > 0)
            {
                if (ddlsec.SelectedItem.ToString() != "" && ddlsec.SelectedItem.ToString() != "-1" && ddlsec.SelectedItem.ToString().ToLower().Trim() != "all" && ddlsec.SelectedItem.ToString() != null)
                {
                    section = ddlsec.SelectedItem.ToString();
                    strsec = " and TT_Sec='" + ddlsec.SelectedItem.ToString() + "'";
                    qryStaffSection = " and LTRIM(RTRIM(ISNULL(Sections,'')))='" + ddlsec.SelectedItem.ToString() + "'";
                    staffSectionName = Convert.ToString(ddlsec.SelectedItem).Trim();
                }
            }
            string batch = ddlbatch.SelectedValue.ToString();
            string degree = ddlbranch.SelectedValue.ToString();
            string sem = ddlsem.SelectedValue.ToString();
            if (ddltimetable.SelectedItem.ToString() == "New")
            {
                ttname = txttimetable.Text.Trim();
            }
            else
            {
                ttname = Convert.ToString(ddltimetable.SelectedItem.Text).Trim();
            }
            string Classadvisor = string.Empty;
            if (ttname == "" || ttname.ToLower() == "new")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter Time Table Name";
                return;
            }
            else
            {
                if (Convert.ToString(ddltimetable.SelectedItem.Text).Trim().ToLower() == "new")
                {
                    bool isAvailabeName = false;
                    isAvailabeName = IsTimeTableExistsOrNot(ttname, batch, degree, sem, section, txtdate.Text.Trim());
                    if (isAvailabeName)
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Time Table Name is Already Exists!!!";
                        return;
                    }
                }
            }
            //string strtimetable = "Select DISTINCT TTname from semester_schedule where batch_year=" + ddlbatch.Text.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " " + strsec + " and ttname='" + ttname + "'";
            string strtimetable = "select TT_classpk from TT_ClassTimetable ct where ct.TT_colCode='" + Convert.ToString(ddlcolg.SelectedValue) + "' and ct.TT_degCode='" + degree + "' and ct.TT_batchyear='" + batch + "' and ct.TT_sem='" + sem + "' " + strsec + " and TT_date>='" + (txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) + "' and TT_name='" + ttname + "'";
            string ttpk = dirAcc.selectScalarString(strtimetable);
            if (string.IsNullOrEmpty(ttpk))
            {
                string insQ = " insert into TT_ClassTimetable (TT_colCode,TT_batchyear,TT_degCode,TT_sem,TT_sec,TT_lastRec,TT_name,TT_date) values  ('" + Convert.ToString(ddlcolg.SelectedValue) + "','" + batch + "','" + degree + "','" + sem + "','" + section + "','1','" + ttname + "','" + (txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) + "')";
                dirAcc.insertData(insQ);
                strtimetable = "select TT_classpk from TT_ClassTimetable ct where ct.TT_colCode='" + Convert.ToString(ddlcolg.SelectedValue) + "' and ct.TT_degCode='" + degree + "' and ct.TT_batchyear='" + batch + "' and ct.TT_sem='" + sem + "' " + strsec + " and TT_date>='" + (txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) + "' and TT_name='" + ttname + "'";
                ttpk = dirAcc.selectScalarString(strtimetable);
            }
            if (!string.IsNullOrEmpty(ttpk))
            {
                string delQ = "delete From TT_ClassTimetableDet where TT_ClassFk='" + ttpk + "'";
                dirAcc.deleteData(delQ);
                Dictionary<string, byte> dicDayOrder = getDayOrder();
                string holiday = d2.GetFunction("Select holiday from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedValue.ToString() + "");
                string Daycoulmn = string.Empty;
                string Daycoulmnvalue = string.Empty;
                int noofdays = Convert.ToInt32(d2.GetFunction("Select nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedValue.ToString() + ""));
                int day = 0;
                for (int day1 = 0; day1 < 6; day1++)
                {
                    string dayofweek = Days[day1];
                    int daysetweek = day1 + 2;
                    if (day1 == 6)
                    {
                        daysetweek = 1;
                    }
                    if (!holiday.Contains(daysetweek.ToString()))
                    {
                        for (int j = 1; j < Fptimetable.Sheets[0].ColumnCount; j++)
                        {
                            string[] mainres = Fptimetable.Sheets[0].Cells[day, j].Note.Split(';');
                            foreach (string value in mainres)
                            {
                                string[] res = value.Split('-');
                                string hour = j.ToString();
                                string daypk = dicDayOrder[dayofweek].ToString();
                                if (res.Length > 3)
                                {
                                    string roomFk = getRoomPk(res[3]);
                                    string insUpd = " if exists ( select TT_ClassDetPK from TT_ClassTimetableDet where TT_Hour ='" + hour + "' and TT_Day='" + daypk + "' and TT_ClassFK='" + ttpk + "' and TT_subno='" + res[0] + "' and TT_staffcode='" + res[1] + "' ) update TT_ClassTimetableDet  set TT_subno='" + res[0] + "',TT_staffcode='" + res[1] + "'  where TT_Hour ='" + hour + "' and TT_Day='" + daypk + "' and TT_ClassFK='" + ttpk + "'  and TT_subno='" + res[0] + "' and TT_staffcode='" + res[1] + "'  else insert into TT_ClassTimetableDet (TT_Hour,TT_Day,TT_ClassFK,TT_subno,TT_staffcode,TT_Room) values ('" + hour + "','" + daypk + "','" + ttpk + "','" + res[0] + "','" + res[1] + "','" + roomFk + "') ";
                                    int a = dirAcc.updateData(insUpd);
                                    string qryStaff = "select staffPriority,facultyChoice from  staff_selector where batch_year='" + batch + "' and staff_code='" + res[1] + "' and subject_no='" + res[0] + "' and LTRIM(RTRIM(ISNULL(Sections,'')))=''";
                                    DataTable dtStaffPriFC = new DataTable();
                                    dtStaffPriFC = dirAcc.selectDataTable(qryStaff);
                                    string facultyChoice = string.Empty;
                                    string staffPriority = string.Empty;
                                    if (dtStaffPriFC.Rows.Count > 0)
                                    {
                                        facultyChoice = Convert.ToString(dtStaffPriFC.Rows[0]["facultyChoice"]).Trim();
                                        staffPriority = Convert.ToString(dtStaffPriFC.Rows[0]["staffPriority"]).Trim();
                                    }
                                    facultyChoice = ((string.IsNullOrEmpty(facultyChoice)) ? "0" : facultyChoice);
                                    staffPriority = ((string.IsNullOrEmpty(staffPriority)) ? "0" : staffPriority);
                                    qryStaff = "if exists (select * from  staff_selector where batch_year='" + batch + "' and staff_code='" + res[1] + "' and subject_no='" + res[0] + "' " + qryStaffSection + ") update staff_selector set Sections='" + staffSectionName + "' where batch_year='" + batch + "' and staff_code='" + res[1] + "' and subject_no='" + res[0] + "' " + qryStaffSection + " else insert into staff_selector (subject_no,staff_code,batch_year,dailyflag,facultyChoice,Sections) values ('" + res[0] + "','" + res[1] + "','" + batch + "','0','" + facultyChoice + "','" + staffSectionName + "')";
                                    int st = dirAcc.updateData(qryStaff);
                                }
                            }
                        }
                        day++;
                    }
                }
                txttimetable.Visible = false;
                btngo_Click(sender, e);
                if (Fpclassadvisor.Sheets[0].RowCount > 0)
                {
                    Fpclassadvisor.Visible = true;
                }
                btnclassadvisor.Visible = true;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                btnsave.Enabled = false;
                btnsave.Visible = true;
                btndelete.Enabled = false;
                if (ddltimetable.SelectedItem.ToString() == "New")
                {
                    ddlsem_SelectedIndexChanged(sender, e);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
            }
        }
        catch { }
        //try
        //{
        //    string[] date = txtdate.Text.Split('/');
        //    string fromdate = date[1] + '/' + date[0] + '/' + date[2];
        //    errmsg.Visible = false;
        //    string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        //    string strsec = string.Empty;
        //    string section = string.Empty;
        //    string ttname = string.Empty;
        //    btndelete.Enabled = false;
        //    if (ddlsec.Enabled == true)
        //    {
        //        if (ddlsec.SelectedItem.ToString() != "" && ddlsec.SelectedItem.ToString() != "-1" && ddlsec.SelectedItem.ToString().ToLower().Trim() != "all" && ddlsec.SelectedItem.ToString() != null)
        //        {
        //            section = ddlsec.SelectedItem.ToString();
        //            strsec = "and sections='" + ddlsec.SelectedItem.ToString() + "'";
        //        }
        //    }
        //    string batch = ddlbatch.SelectedValue.ToString();
        //    string degree = ddlbranch.SelectedValue.ToString();
        //    string sem = ddlsem.SelectedValue.ToString();
        //    if (ddltimetable.SelectedItem.ToString() == "New")
        //    {
        //        ttname = txttimetable.Text;
        //    }
        //    else
        //    {
        //        ttname = ddltimetable.SelectedItem.ToString();
        //    }
        //    string Classadvisor = string.Empty;
        //    if (ttname == "" || ttname.ToLower() == "new")
        //    {
        //        errmsg.Visible = true;
        //        errmsg.Text = "Please Enter Time Table Name";
        //        return;
        //    }
        //    string strtimetable = "Select DISTINCT TTname from semester_schedule where batch_year=" + ddlbatch.Text.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " " + strsec + " and ttname='" + ttname + "'";
        //    ds = d2.select_method(strtimetable, hat, "Text");
        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        DataSet dstrigger = new DataSet();
        //        string trigger = "select * from sysobjects where name='TR_Semester_Schudule' and Type='TR'";
        //        dstrigger = d2.select_method(trigger, hat, "Text");
        //        if (dstrigger.Tables[0].Rows.Count > 0)
        //        {
        //            con.Close();
        //            con.Open();
        //            SqlCommand sqlcmd = new SqlCommand("drop trigger TR_Semester_Schudule", con);
        //            SqlDataReader dr = sqlcmd.ExecuteReader();
        //        }
        //        con.Close();
        //        con.Open();
        //        SqlCommand sqlcmd1 = new SqlCommand("Delete from semester_schedule where batch_year=" + ddlbatch.Text.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " " + strsec + " and ttname='" + ttname + "'", con);
        //        SqlDataReader dr1 = sqlcmd1.ExecuteReader();
        //        trigger = "select * from sysobjects where name='TR_Semester_Schudule' and Type='TR'";
        //        dstrigger = d2.select_method(trigger, hat, "Text");
        //        if (dstrigger.Tables[0].Rows.Count == 0)
        //        {
        //            con.Close();
        //            con.Open();
        //            SqlCommand sqlcmd = new SqlCommand("Create TRIGGER TR_Semester_Schudule On Semester_Schedule For Delete AS BEGIN Insert Into Semester_Schedule select * FROM deleted ins End", con);
        //            SqlDataReader dr = sqlcmd.ExecuteReader();
        //        }
        //    }
        //    string holiday = d2.GetFunction("Select holiday from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedValue.ToString() + "");
        //    string Daycoulmn = string.Empty;
        //    string Daycoulmnvalue = string.Empty;
        //    int noofdays = Convert.ToInt32(d2.GetFunction("Select nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedValue.ToString() + ""));
        //    int day = 0;
        //    for (int day1 = 0; day1 < 6; day1++)
        //    {
        //        string dayofweek = Days[day1];
        //        int daysetweek = day1 + 2;
        //        if (day1 == 6)
        //        {
        //            daysetweek = 1;
        //        }
        //        if (!holiday.Contains(daysetweek.ToString()))
        //        {
        //            for (int j = 1; j < Fptimetable.Sheets[0].ColumnCount; j++)
        //            {
        //                if (Daycoulmn == "")
        //                {
        //                    Daycoulmn = dayofweek + j;
        //                    string value = Fptimetable.Sheets[0].Cells[day, j].Note;
        //                    Daycoulmnvalue = "'" + value + "'";
        //                }
        //                else
        //                {
        //                    Daycoulmn = Daycoulmn + ',' + dayofweek + j;
        //                    string value = Fptimetable.Sheets[0].Cells[day, j].Note;
        //                    Daycoulmnvalue = Daycoulmnvalue + ',' + "'" + value + "'";
        //                }
        //            }
        //            Classadvisor = string.Empty;
        //            for (int i = 0; i < Fpclassadvisor.Sheets[0].RowCount; i++)
        //            {
        //                string[] spiltcheck = Classadvisor.Split(',');
        //                Boolean chevalflag = false;
        //                for (int ch = 0; ch <= spiltcheck.GetUpperBound(0); ch++)
        //                {
        //                    if (Fpclassadvisor.Sheets[0].Cells[i, 2].Text.ToString() == spiltcheck[ch].ToString())
        //                    {
        //                        chevalflag = true;
        //                    }
        //                }
        //                if (chevalflag == false)
        //                {
        //                    if (Classadvisor == "")
        //                    {
        //                        Classadvisor = Fpclassadvisor.Sheets[0].Cells[i, 2].Text.ToString();
        //                    }
        //                    else
        //                    {
        //                        Classadvisor = Classadvisor + ',' + Fpclassadvisor.Sheets[0].Cells[i, 2].Text.ToString();
        //                    }
        //                }
        //            }
        //            day++;
        //        }
        //    }
        //    string inserquery = "insert into Semester_Schedule (class_advisor,degree_code,batch_year,semester,sections,TTName,FromDate," + Daycoulmn + ",lastrec) values('" + Classadvisor + "'," + degree + "," + batch + "," + sem + ",'" + section + "','" + ttname + "','" + fromdate + "'," + Daycoulmnvalue + ",1)";
        //    int a = d2.insert_method(inserquery, hat, "Text");
        //    txttimetable.Visible = false;
        //    btngo_Click(sender, e);
        //    if (Fpclassadvisor.Sheets[0].RowCount > 0)
        //    {
        //        Fpclassadvisor.Visible = true;
        //    }
        //    btnclassadvisor.Visible = true;
        //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
        //    btnsave.Enabled = false;
        //    btnsave.Visible = true;
        //    btndelete.Enabled = false;
        //    if (ddltimetable.SelectedItem.ToString() == "New")
        //    {
        //        ddlsem_SelectedIndexChanged(sender, e);
        //    }
        //}
        //catch
        //{
        //}
    }

    public string findday(string curday, string sdate, string no_days, string stastdayorder)
    {
        int holiday = 0;
        if (no_days == "")
            return "";
        if (sdate != "")
        {
            string[] sp_date = curday.Split(new Char[] { '/' });
            string cur_date = sp_date[1].ToString() + "-" + sp_date[0].ToString() + "-" + sp_date[2].ToString();
            DateTime dt1 = Convert.ToDateTime(sdate);
            DateTime dt2 = Convert.ToDateTime(cur_date);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*)as count from holidaystudents  where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";
            string holday = d2.GetFunction(query1);
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
                    order = order + (Convert.ToInt32(stastdayorder) - 1);
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

    public void BindCollege()
    {
        string collquery = "select collname,college_code from collinfo";
        ds = d2.select_method(collquery, hat, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
    }

    public void loadstaffdep()
    {
        try
        {
            string staffquery = "select distinct dept_name,dept_code from hrdept_master where college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            ds = d2.select_method(staffquery, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldepratstaff.DataSource = ds;
                ddldepratstaff.DataTextField = "dept_name";
                ddldepratstaff.DataValueField = "dept_code";
                ddldepratstaff.DataBind();
                ddldepratstaff.Items.Insert(0, "All");
            }
        }
        catch
        {
        }
    }

    protected void btnclassadvisor_Click(object sender, EventArgs e)
    {
        panelstaff.Visible = true;
        fsstaff.Visible = true;
        BindCollege();
        loadstaffdep();
        loadfsstaff();
    }

    protected void loadfsstaff()
    {
        try
        {
            string sql = string.Empty;
            if (ddldepratstaff.SelectedItem.ToString() == "All")
            {
                if (txt_search.Text != "")
                {
                    if (ddlstaff.SelectedIndex == 0)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '" + txt_search.Text + "%') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                    }
                    else if (ddlstaff.SelectedIndex == 1)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                    }
                }
                else
                {
                    sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "')";
                }
            }
            else
            {
                if (txt_search.Text != "")
                {
                    if (ddlstaff.SelectedIndex == 0)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '" + txt_search.Text + "%') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                    }
                    else if (ddlstaff.SelectedIndex == 1)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                    }
                }
                else
                {
                    sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                }
            }
            fsstaff.Sheets[0].RowCount = 0;
            fsstaff.SaveChanges();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            fsstaff.Sheets[0].RowCount = 0;
            //fsstaff.Sheets[0].SpanModel.Add(fsstaff.Sheets[0].RowCount - 1, 0, 1, 3);
            fsstaff.Sheets[0].AutoPostBack = false;
            fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
            fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Name";
            fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Code";
            fsstaff.Sheets[0].ColumnHeader.Columns[3].Label = "Select";
            fsstaff.Sheets[0].Columns[0].Width = 50;
            fsstaff.Sheets[0].Columns[1].Width = 320;
            fsstaff.Sheets[0].Columns[2].Width = 200;
            fsstaff.Sheets[0].Columns[3].Width = 62;
            fsstaff.Sheets[0].ColumnCount = 4;
            fsstaff.Width = 650;
            ds = d2.select_method(sql, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    string name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    string code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();
                    fsstaff.Sheets[0].RowCount++;
                    fsstaff.Sheets[0].Rows[fsstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].CellType = txt;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].CellType = txt;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Text = name;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].CellType = txt;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Text = code;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].CellType = chkcell;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    fsstaff.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                }
                int rowcount = fsstaff.Sheets[0].RowCount;
                fsstaff.Height = 300;
                fsstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                fsstaff.SaveChanges();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        try
        {
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            string classadvisor = string.Empty;
            for (int i = 0; i < Fpclassadvisor.Sheets[0].RowCount; i++)
            {
                string[] spiltcheck = classadvisor.Split(',');
                Boolean chevalflag = false;
                for (int ch = 0; ch <= spiltcheck.GetUpperBound(0); ch++)
                {
                    if (Fpclassadvisor.Sheets[0].Cells[i, 2].Text.ToString() == spiltcheck[ch].ToString())
                    {
                        chevalflag = true;
                    }
                }
                if (chevalflag == false)
                {
                    if (classadvisor == "")
                    {
                        classadvisor = Fpclassadvisor.Sheets[0].Cells[i, 2].Text.ToString();
                    }
                    else
                    {
                        classadvisor = classadvisor + ',' + Fpclassadvisor.Sheets[0].Cells[i, 2].Text.ToString();
                    }
                }
            }
            string Staffcode = string.Empty;
            fsstaff.SaveChanges();
            for (int rolcount = 0; rolcount < fsstaff.Sheets[0].RowCount; rolcount++)
            {
                string[] spiltcheck = classadvisor.Split(',');
                Boolean chevalflag = false;
                for (int ch = 0; ch <= spiltcheck.GetUpperBound(0); ch++)
                {
                    if (fsstaff.Sheets[0].Cells[rolcount, 2].Text.ToString() == spiltcheck[ch].ToString())
                    {
                        chevalflag = true;
                    }
                }
                if (chevalflag == false)
                {
                    int isval = Convert.ToInt32(fsstaff.Sheets[0].Cells[rolcount, 3].Value);
                    if (isval == 1)
                    {
                        Fpclassadvisor.Sheets[0].RowCount++;
                        Fpclassadvisor.Sheets[0].Cells[Fpclassadvisor.Sheets[0].RowCount - 1, 0].CellType = txt;
                        Fpclassadvisor.Sheets[0].Cells[Fpclassadvisor.Sheets[0].RowCount - 1, 1].CellType = txt;
                        Fpclassadvisor.Sheets[0].Cells[Fpclassadvisor.Sheets[0].RowCount - 1, 2].CellType = txt;
                        Fpclassadvisor.Sheets[0].Cells[Fpclassadvisor.Sheets[0].RowCount - 1, 0].Text = Fpclassadvisor.Sheets[0].RowCount.ToString();
                        Fpclassadvisor.Sheets[0].Cells[Fpclassadvisor.Sheets[0].RowCount - 1, 1].Text = fsstaff.Sheets[0].Cells[rolcount, 1].Text.ToString();
                        Fpclassadvisor.Sheets[0].Cells[Fpclassadvisor.Sheets[0].RowCount - 1, 2].Text = fsstaff.Sheets[0].Cells[rolcount, 2].Text.ToString();
                    }
                }
            }
            classadvisorsave();
            Session["ClassAdvisor"] = Staffcode;
            panelstaff.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void exitpop_Click(object sender, EventArgs e)
    {
        panelstaff.Visible = false;
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        chkSectionWise.Checked = false;
        chkSectionWise.Visible = false;
        loadstaffdep();
    }

    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }

    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }

    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        //string Batch = ddlbatch.SelectedItem.ToString();
        //string Degree = ddlbranch.SelectedValue.ToString();
        //string Sem = ddlsem.SelectedItem.ToString();
        //string ttname = ddltimetable.SelectedItem.ToString();
        //string sec =string.Empty;
        //if (ddlsec.Enabled == true )
        //{
        //    if (ddlsec.SelectedItem.ToString() != "All")
        //    {
        //        sec = " and Sections='" + ddlsec.SelectedItem.ToString() + "'";
        //    }
        //}
        //if (ttname.ToLower() != "new" && Fptimetable.Visible == true)
        //{
        //    string deletquery = "Delete from Semester_Schedule where batch_year=" + Batch + " and degree_code=" + Degree + " and semester=" + Sem + " and ttname='" + ttname + "' " + sec + "";
        //    int a = d2.insert_method(deletquery, hat, "Text");
        //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Deleted successfully')", true);
        //}
        string activerow = Fptimetable.ActiveSheetView.ActiveRow.ToString();
        string activecol = Fptimetable.ActiveSheetView.ActiveColumn.ToString();
        if (activecol != "0" && activerow != "-1")
        {
            Fptimetable.Sheets[0].Cells[int.Parse(activerow), int.Parse(activecol)].Text = string.Empty;
            Fptimetable.Sheets[0].Cells[int.Parse(activerow), int.Parse(activecol)].Tag = string.Empty;
            Fptimetable.Sheets[0].Cells[int.Parse(activerow), int.Parse(activecol)].Note = string.Empty;
            Fptimetable.SaveChanges();
            btnsave.Enabled = true;
            errmsg.Visible = false;
            lblday.Visible = false;
            lblday1.Visible = false;
            lbltimings.Visible = false;
            lblfromtime.Visible = false;
            lbltotime.Visible = false;
            treepanel.Visible = false;
            btnsave.Visible = true;
            btnsave.Enabled = true;
            txtmultisubj.Enabled = false;
            treepanel.Visible = false;
            txttimetable.Visible = false;
            panelstaff.Visible = false;
            fpdetails.Visible = false;
            btndelete.Enabled = false;
        }
    }

    protected void txtdate_TextChanged(object sender, EventArgs e)
    {
        btnsave.Enabled = true;
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fptimetable, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    //protected void btnprintmaster_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Session["column_header_row_count"] = 1;
    //        string deg_details = string.Empty;
    //        string degree_pdf = string.Empty;
    //        string header = string.Empty;
    //        deg_details = "Semester Time Table";
    //        string strsec = ddlsec.SelectedValue.ToString();
    //        if (strsec != "" && strsec != "-1" && strsec != "All")
    //        {
    //            degree_pdf = "Degree : " + ddlbatch.SelectedItem.Text + " - " + ddldegree.SelectedItem.Text + " -" + ddlbranch.SelectedItem.Text + "- sem- " + ddlsem.SelectedItem.Text + "- sec-" + strsec + " @ Time Table Name : " + ddltimetable.Text + " @ Date  : " + txtdate.Text + "";
    //        }
    //        else
    //        {
    //            degree_pdf = "Degree : " + ddlbatch.SelectedItem.Text + " - " + ddldegree.SelectedItem.Text + " -" + ddlbranch.SelectedItem.Text + "- sem- " + ddlsem.SelectedItem.Text + " @ Time Table Name : " + ddltimetable.Text + " @ Date  : " + txtdate.Text + "";
    //        }
    //        string degreedetails = string.Empty;
    //        degreedetails = deg_details + "@" + degree_pdf;
    //        string pagename = "StudentTimeTable.aspx";
    //        Printcontrol.loadspreaddetails(Fptimetable, pagename, degreedetails);
    //        Printcontrol.Visible = true;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    protected void btnPDF_Click(object sender, EventArgs e)
    {
        GeneratePDF();
    }

    List<string> str_arr = new List<string>();
    Font table_cell = new Font("Book Antiqua", 11, FontStyle.Regular);
    Font table_col = new Font("Book Antiqua", 12, FontStyle.Bold);
    Font FontboldNew = new Font("Book Antiqua", 15, FontStyle.Bold);

    public void GeneratePDF()
    {
        try
        {
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            string[] datespilt = txtdate.Text.Split('/');
            string temp_date = datespilt[1] + '/' + datespilt[0] + '/' + datespilt[2];
            if (strsec != "" && strsec != "-1" && strsec != "All")
            {
                strsec = "and sections='" + strsec + "'";
            }
            else
            {
                strsec = string.Empty;
            }
            Font Fontbold = new Font("Book Antiqua", 14, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 12, FontStyle.Regular);
            Font Fontbold1 = new Font("Book Antiqua", 12, FontStyle.Bold);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            string[] shortday = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string[] days = new string[7] { "Monday", "Tuesday", "wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            int noofhours = Convert.ToInt32(Session["totalhrs"]);
            int noofdays = Convert.ToInt32(Session["totnoofdays"]);
            int dayorder = Convert.ToInt32(Session["dayorder"]);
            DataTable dt = new DataTable();
            DataColumn dc;
            DataRow dr;
            dc = new DataColumn("Day");
            dt.Columns.Add(dc);
            for (int i = 1; i <= noofhours; i++)
            {
                dc = new DataColumn(i.ToString());
                dt.Columns.Add(dc);
            }
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dr["Day"] = "Timings";
            string dayvalue = string.Empty;
            int day = 0;
            string holiday = d2.GetFunction("Select holiday from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedValue.ToString() + "");
            for (int day1 = 0; day1 < 6; day1++)
            {
                string dayofweek = shortday[day1];
                string dayofweek1 = days[day1];
                int daysetweek = day1 + 2;
                if (day1 == 6)
                {
                    daysetweek = 1;
                }
                if (!holiday.Contains(daysetweek.ToString()))
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    if (dayorder == 1)
                    {
                        dr["Day"] = dayofweek1;
                    }
                    else
                    {
                        int date = day + 1;
                        dr["Day"] = "Day " + date;
                    }
                    for (int i = 1; i <= noofhours; i++)
                    {
                        if (dayvalue == "")
                        {
                            dayvalue = dayofweek + i;
                        }
                        else
                        {
                            dayvalue = dayvalue + ',' + dayofweek + i;
                        }
                    }
                }
            }
            //added by srinath 8/1/2014
            //added by srinath 10/6/2014
            for (int i = 1; i <= noofhours; i++)
            {
                string belltime = string.Empty;
                // string strtimequery = "Select start_time,end_time from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + i + "'";
                //ds = d2.select_method(strtimequery, hat, "Text");
                //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    if (ds.Tables[0].Rows[0]["start_time"].ToString() != "" && ds.Tables[0].Rows[0]["start_time"].ToString() != null && ds.Tables[0].Rows[0]["end_time"].ToString() != "" && ds.Tables[0].Rows[0]["end_time"].ToString() != null)
                //    {
                //        string[] spiltstarttime = ds.Tables[0].Rows[0]["start_time"].ToString().Split(' ');
                //        string[] spiltendtime = ds.Tables[0].Rows[0]["end_time"].ToString().Split(' ');
                //        belltime = spiltstarttime[1].ToString() + ' ' + spiltstarttime[2].ToString() + ' ' + " To " + spiltendtime[1].ToString() + ' ' + spiltendtime[2].ToString();
                //    }
                string strtimequery = "Select RIGHT(CONVERT(VARCHAR, start_time, 100),7) as sTime,RIGHT(CONVERT(VARCHAR, end_time, 100),7) as endtime from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + i + "'";
                ds = d2.select_method(strtimequery, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string starttime = ds.Tables[0].Rows[0]["sTime"].ToString();
                    string endtime = ds.Tables[0].Rows[0]["endtime"].ToString();
                    if (starttime != null && starttime.Trim() != "" && endtime != null && endtime.Trim() != "")
                    {
                        belltime = starttime + " To " + endtime;
                    }
                }
                dt.Rows[0][i] = belltime;
            }
            string schedukle = "select  top 1 " + dayvalue + " from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + strsem + " and batch_year = " + ddlbatch.SelectedItem.ToString() + " and ttname='" + ddltimetable.SelectedItem.ToString() + "' and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc";
            ds = d2.select_method(schedukle, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    string value = Fptimetable.Sheets[0].Cells[i, 0].Note.ToString();
                    for (int j = 1; j < dt.Columns.Count; j++)
                    {
                        string dsvalue = value + j;
                        string classhour = ds.Tables[0].Rows[0]["" + dsvalue + ""].ToString();
                        string setclasshour = string.Empty;
                        if (classhour.Trim() != "" && classhour.Trim() != "0" && classhour != null)
                        {
                            string[] spiltmulpl = classhour.Split(';');
                            for (int mul = 0; mul <= spiltmulpl.GetUpperBound(0); mul++)
                            {
                                string[] spiltclasshour = spiltmulpl[mul].Split('-');
                                for (int sp = 0; sp <= spiltclasshour.GetUpperBound(0); sp++)
                                {
                                    if (sp == 0)
                                    {
                                        if (setclasshour == "")
                                        {
                                            try
                                            {
                                                setclasshour = d2.GetFunction("select case when isnull(acronym,'')='' then subject_code end from subject where subject_no='" + spiltclasshour[sp].ToString() + "'");
                                                if (!str_arr.Contains(spiltclasshour[sp].ToString()))
                                                {
                                                    str_arr.Add(spiltclasshour[sp].ToString());
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        else
                                        {
                                            setclasshour = setclasshour + "; " + d2.GetFunction("select case when isnull(acronym,'')='' then subject_code end from subject where subject_no='" + spiltclasshour[sp].ToString() + "'"); ;
                                            if (!str_arr.Contains(spiltclasshour[sp].ToString()))
                                            {
                                                str_arr.Add(spiltclasshour[sp].ToString());
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        dt.Rows[i + 1][j] = setclasshour;
                    }
                }
                btnsave.Visible = true;
                btndelete.Visible = false;
                Fpclassadvisor.Visible = false;
            }
            bindall(mydoc, Fontsmall, Fontbold, Fontbold1, dt, str_arr);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void bindall(Gios.Pdf.PdfDocument mydoc, Font Fontsmall, Font Fontbold, Font Fontbold1, DataTable dt_dummyno, List<string> str_arr)
    {
        try
        {
            Hashtable hat_select_method = new Hashtable();
            MyImg mi = new MyImg();
            mi.ImageUrl = "~/images/10BIT001.jpeg";
            mi.ImageUrl = "Handler/Handler2.ashx?";
            MyImg1 mi2 = new MyImg1();
            mi2.ImageUrl = "~/images/10BIT001.jpeg";
            mi2.ImageUrl = "Handler/Handler5.ashx?";
            string str = "select isnull(collname, ' ') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(district, ' ') as district,isnull(pincode,' ') as pincode,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            con.Close();
            con.Open();
            SqlCommand comm = new SqlCommand(str, con);
            SqlDataReader drr = comm.ExecuteReader();
            drr.Read();
            string coll_name = Convert.ToString(drr["collname"]);
            string coll_address1 = Convert.ToString(drr["address1"]);
            string coll_address2 = Convert.ToString(drr["address2"]);
            string coll_address3 = Convert.ToString(drr["address3"]);
            string district = Convert.ToString(drr["district"]);
            string pin_code = Convert.ToString(drr["pincode"]);
            string catgory = drr["category"].ToString();
            catgory = "(An " + catgory + " Institution" + " " + "-" + "";
            string affliatedby = drr["affliated"].ToString();
            string affliatedbynew = Regex.Replace(affliatedby, ",", " ");
            string affiliated = catgory + " " + "Affiliated to" + " " + affliatedbynew + ")";
            string address = coll_address1 + "," + " " + coll_address2 + "," + " " + district + "-" + " " + pin_code + ".";
            string phoneno = drr["phoneno"].ToString();
            string faxno = drr["faxno"].ToString();
            string email = drr["email"].ToString();
            string website = drr["website"].ToString();
            con.Close();
            con.Open();
            SqlCommand cmd_timetabledate = new SqlCommand("select convert(varchar(10),fromdate,104) from semester_schedule where batch_year=" + ddlbatch.SelectedItem.ToString() + " and semester=" + ddlsem.SelectedItem.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString(), con);
            string date_affect = Convert.ToString(cmd_timetabledate.ExecuteScalar());
            int total_row_count = dt_dummyno.Rows.Count - 1;
            double pdfpage = total_row_count / 25;
            int ppcount = (int)Math.Round(pdfpage + 1);
            int recordcount = 0;
            //if (ppcount != 0)
            //{
            double recordcount_1 = 25;
            //}
            int prowcount = (int)Math.Round(recordcount_1);
            int recordcount_2 = 0;
            int rowincree = 0;
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            string[] datespilt = txtdate.Text.Split('/');
            string temp_date = datespilt[1] + '/' + datespilt[0] + '/' + datespilt[2];
            if (strsec != "" && strsec != "-1" && strsec != "All")
            {
                strsec = "and sections='" + strsec + "'";
            }
            else
            {
                strsec = string.Empty;
            }
            string getyear = string.Empty;
            if (ddlsem.SelectedItem.ToString() == "1" || ddlsem.SelectedItem.ToString() == "2")
            {
                getyear = "I";
            }
            else if (ddlsem.SelectedItem.ToString() == "3" || ddlsem.SelectedItem.ToString() == "4")
            {
                getyear = "II";
            }
            else if (ddlsem.SelectedItem.ToString() == "5" || ddlsem.SelectedItem.ToString() == "6")
            {
                getyear = "III";
            }
            else if (ddlsem.SelectedItem.ToString() == "7" || ddlsem.SelectedItem.ToString() == "8")
            {
                getyear = "IV";
            }
            else if (ddlsem.SelectedItem.ToString() == "9" || ddlsem.SelectedItem.ToString() == "10")
            {
                getyear = "V";
            }
            //for (int pagecount = 0; pagecount < ppcount; pagecount++)
            for (int pagecount = 0; pagecount < 1; pagecount++)
            {
                Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    PdfImage Left_Logo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(Left_Logo, 25, 20, 500);
                }
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))//Aruna
                {
                    PdfImage Right_Logo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                    mypdfpage.Add(Right_Logo, 700, 20, 500);
                }
                PdfTextArea ptc9 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 160, 20, 480, 30), System.Drawing.ContentAlignment.MiddleCenter, coll_name);
                PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 180, 40, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, address);
                PdfTextArea ptc1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydoc, 200, 60, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, affiliated);
                PdfTextArea ptc2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, 150, 70, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "Phone: " + phoneno + ", Fax: " + faxno);
                PdfTextArea ptc3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, 150, 90, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "E-Mail: " + email + ",Website: " + website);
                PdfTextArea ptc4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, 150, 120, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "Department of " + ddlbranch.SelectedItem.ToString());
                PdfTextArea ptc5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, 150, 140, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "Time Table with effect from " + date_affect);
                PdfTextArea ptc6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, -20, 160, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "Year of Study : " + getyear);
                PdfTextArea ptc7 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 60, 160, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "Sem : " + ddlsem.SelectedItem.ToString());
                strsec = ddlsec.SelectedValue.ToString();
                if (strsec != "" && strsec != "-1" && strsec != "All")
                {
                    strsec = "and sections='" + strsec + "'";
                    PdfTextArea ptcsec = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 355, 160, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Sec :" + ddlsec.SelectedValue.ToString() + "");
                    mypdfpage.Add(ptcsec);
                    PdfTextArea ptc8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 410, 160, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class : " + ddldegree.SelectedItem.ToString() + "[" + ddlbranch.SelectedItem.ToString() + "]");
                    mypdfpage.Add(ptc8);
                }
                else
                {
                    PdfTextArea ptc8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 355, 160, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class : " + ddldegree.SelectedItem.ToString() + "[" + ddlbranch.SelectedItem.ToString() + "]");
                    mypdfpage.Add(ptc8);
                }
                PdfTextArea ptc10 = new PdfTextArea(FontboldNew, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 25, 190, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Schedule:");
                //PdfTextArea ptc11 = new PdfTextArea(FontboldNew, System.Drawing.Color.Black,
                //                                   new PdfArea(mydoc, 25, 940, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Class Advisor(s):");
                mypdfpage.Add(ptc9);
                mypdfpage.Add(ptc);
                //mypdfpage.Add(ptc1);
                mypdfpage.Add(ptc2);
                mypdfpage.Add(ptc3);
                mypdfpage.Add(ptc4);
                mypdfpage.Add(ptc5);
                mypdfpage.Add(ptc6);
                mypdfpage.Add(ptc7);
                //  mypdfpage.Add(ptc8);
                mypdfpage.Add(ptc10);
                //mypdfpage.Add(ptc11);            
                if (pagecount == 0)
                {
                    recordcount_2 = 0;
                    recordcount = recordcount + prowcount;
                }
                else
                {
                    recordcount = recordcount + prowcount;
                    recordcount_2 = recordcount_2 + prowcount;
                }
                int bindrow_2 = 0;
                if (pagecount == 0)
                {
                    rowincree = recordcount / 2;
                    bindrow_2 = rowincree;
                }
                else
                {
                    bindrow_2 = (recordcount - 25 + rowincree);
                }
                DataTable dt_temp_1 = new DataTable();
                DataRow dr_temp_1;
                DataColumn dc_temp_1;
                dc_temp_1 = new DataColumn();
                dc_temp_1.ColumnName = "S.No";
                dt_temp_1.Columns.Add(dc_temp_1);
                dc_temp_1 = new DataColumn();
                dc_temp_1.ColumnName = "Subject Code";
                dt_temp_1.Columns.Add(dc_temp_1);
                dc_temp_1 = new DataColumn();
                dc_temp_1.ColumnName = "Subject";
                dt_temp_1.Columns.Add(dc_temp_1);
                dc_temp_1 = new DataColumn();
                dc_temp_1.ColumnName = "Staff";
                dt_temp_1.Columns.Add(dc_temp_1);
                int sno = 0;
                for (int i = 0; i < str_arr.Count; i++)
                {
                    string str_staffdetails = "select subject_code,subject_name from subject where subject_no='" + str_arr[i].ToString() + "';select staff_name from staffmaster where staff_code in(select staff_code from staff_selector where subject_no='" + str_arr[i].ToString() + "' and batch_year=" + ddlbatch.SelectedItem.ToString() + " " + strsec + ")";
                    DataSet ds_staffdetails = new DataSet();
                    ds_staffdetails = d2.select_method(str_staffdetails, hat_select_method, "Text");
                    dr_temp_1 = dt_temp_1.NewRow();
                    sno++;
                    if (ds_staffdetails.Tables[0].Rows.Count > 0)
                    {
                        dr_temp_1["S.No"] = sno.ToString();
                        dr_temp_1["Subject Code"] = ds_staffdetails.Tables[0].Rows[0]["subject_code"].ToString();
                        dr_temp_1["Subject"] = ds_staffdetails.Tables[0].Rows[0]["subject_name"].ToString();
                    }
                    if (ds_staffdetails.Tables[1].Rows.Count > 0)
                    {
                        //Modified by srinath 19/12/2013
                        string staffname = string.Empty;
                        for (int st = 0; st < ds_staffdetails.Tables[1].Rows.Count; st++)
                        {
                            if (staffname == "")
                            {
                                staffname = ds_staffdetails.Tables[1].Rows[st]["staff_name"].ToString();
                            }
                            else
                            {
                                staffname = staffname + ", " + ds_staffdetails.Tables[1].Rows[st]["staff_name"].ToString();
                            }
                        }
                        //  dr_temp_1["Staff"] = ds_staffdetails.Tables[1].Rows[0]["staff_name"].ToString();
                        dr_temp_1["Staff"] = staffname;
                    }
                    dt_temp_1.Rows.Add(dr_temp_1);
                }
                //***************added by srinath**********************
                if (str_arr.Count == 1)
                {
                    dr_temp_1 = dt_temp_1.NewRow();
                    sno++;
                    dr_temp_1["S.No"] = sno.ToString();
                    dr_temp_1["Subject Code"] = " ";
                    dr_temp_1["Subject"] = " ";
                    dr_temp_1["Staff"] = " ";
                    dt_temp_1.Rows.Add(dr_temp_1);
                }
                //***********************************************************
                DataTable dt_temp_4 = new DataTable();
                DataColumn dc_temp_4;
                DataRow dr_temp_4;
                string schedukle = d2.GetFunction("select  top 1 class_advisor from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + strsem + " and batch_year = " + ddlbatch.SelectedItem.ToString() + " and ttname='" + ddltimetable.SelectedItem.ToString() + "' and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc");
                if (schedukle != null && schedukle.Trim() != "" && schedukle.Trim() != "0")
                {
                    dc_temp_4 = new DataColumn("S.No");
                    dt_temp_4.Columns.Add(dc_temp_4);
                    dc_temp_4 = new DataColumn("Staff code");
                    dt_temp_4.Columns.Add(dc_temp_4);
                    dc_temp_4 = new DataColumn("Staff Name");
                    dt_temp_4.Columns.Add(dc_temp_4);
                    string[] spiltadvisor = schedukle.Split(',');
                    int serial = 0;
                    for (int i = 0; i <= spiltadvisor.GetUpperBound(0); i++)
                    {
                        serial++;
                        string staffname = d2.GetFunction("select staff_name from staffmaster where staff_code='" + spiltadvisor[i].ToString() + "'");
                        dr_temp_4 = dt_temp_4.NewRow();
                        dr_temp_4["S.No"] = serial.ToString();
                        dr_temp_4["Staff Code"] = spiltadvisor[i].ToString();
                        dr_temp_4["Staff Name"] = staffname;
                        dt_temp_4.Rows.Add(dr_temp_4);
                    }
                }
                DataTable dt_staffdetails = new DataTable();
                dt_staffdetails.Columns.Add("Stafflabel", typeof(String));
                dt_staffdetails.Rows.Add("Staff Details:");
                DataTable dt_classadvisor = new DataTable();
                dt_classadvisor.Columns.Add("classadvisor", typeof(String));
                dt_classadvisor.Rows.Add("Class Advisor:");
                PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                PdfTable myPdfTable = myPdfDocument.NewTable(new Font("Verdana", 13), dt_dummyno.Rows.Count, dt_dummyno.Columns.Count, 8);
                myPdfTable.ImportDataTable(dt_dummyno);
                //myPdfTable.SetColumnsWidth(new int[] { 100 });
                //myPdfTable.VisibleHeaders = false;
                myPdfTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                PdfDocument mypdfstafflabel = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                PdfTable myPdfTablestafflabel = mypdfstafflabel.NewTable(new Font("Book Antiqua", 15, FontStyle.Bold), dt_staffdetails.Rows.Count, dt_staffdetails.Columns.Count, 5);
                myPdfTablestafflabel.ImportDataTable(dt_staffdetails);
                //myPdfTable.SetColumnsWidth(new int[] { 100 });
                myPdfTablestafflabel.VisibleHeaders = false;
                myPdfTablestafflabel.SetBorders(Color.White, 1, BorderType.CompleteGrid);
                myPdfTablestafflabel.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                int norowva = dt_temp_1.Rows.Count;
                PdfDocument myPdfDocument_3 = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                PdfTable myPdfTable_3 = myPdfDocument_3.NewTable(new Font("Verdana", 13), norowva, 4, 8);
                myPdfTable_3.ImportDataTable(dt_temp_1);
                myPdfTable_3.SetColumnsWidth(new int[] { 30, 100, 150, 150 });
                //myPdfTable_3.VisibleHeaders = false;
                myPdfTable_3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                PdfDocument mypdfclassadvisor = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                PdfTable myPdfTableclassadvisor = mypdfclassadvisor.NewTable(new Font("Book Antiqua", 15, FontStyle.Bold), dt_classadvisor.Rows.Count, dt_classadvisor.Columns.Count, 6);
                myPdfTableclassadvisor.ImportDataTable(dt_classadvisor);
                //myPdfTable.SetColumnsWidth(new int[] { 100 });
                myPdfTableclassadvisor.VisibleHeaders = false;
                myPdfTableclassadvisor.SetBorders(Color.White, 1, BorderType.CompleteGrid);
                myPdfTableclassadvisor.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                PdfDocument myPdfDocument_1 = null;
                PdfTable myPdfTable_1 = null;
                int findrow = 0;
                if (dt_temp_4.Rows.Count > 0)
                {
                    findrow = 1;
                    myPdfDocument_1 = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                    myPdfTable_1 = myPdfDocument_1.NewTable(new Font("Verdana", 13), dt_temp_4.Rows.Count, 3, 8);
                    myPdfTable_1.ImportDataTable(dt_temp_4);
                    myPdfTable_1.SetColumnsWidth(new int[] { 30, 150, 200 });
                    myPdfTable_1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                }
                //myPdfTable.HeadersRow.SetColors(Color.Black, Color.LightGray);
                //myPdfTable_3.HeadersRow.SetColors(Color.Black, Color.LightGray);
                //myPdfTable_1.HeadersRow.SetColors(Color.Black, Color.LightGray);
                myPdfTable.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                myPdfTable_3.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                myPdfTable_3.Columns[3].SetContentAlignment(ContentAlignment.MiddleLeft);
                if (findrow == 1)
                {
                    myPdfTable_1.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                    myPdfTable_1.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                }
                Gios.Pdf.PdfTable table = mydoc.NewTable(table_cell, dt_dummyno.Rows.Count + 1, dt_dummyno.Columns.Count, 1);
                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                Gios.Pdf.PdfTable table_1 = mydoc.NewTable(table_cell, dt_temp_1.Rows.Count + 1, dt_temp_1.Columns.Count, 1);
                table_1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                if (findrow == 1)
                {
                    Gios.Pdf.PdfTable table_4 = mydoc.NewTable(table_cell, dt_temp_4.Rows.Count + 1, dt_temp_4.Columns.Count, 1);
                    table_4.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                }
                PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, 20, -75, 800, 500));
                mypdfpage.Add(newPdfTablePage);
                double lastrow = newPdfTablePage.Area.BottomRightCornerY;
                PdfTablePage newPdfTablePagestafflabel = myPdfTablestafflabel.CreateTablePage(new PdfArea(mypdfclassadvisor, 20, lastrow + 20, 800, 500));
                //   mypdfpage.Add(newPdfTablePagestafflabel);
                PdfTextArea ptcf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydoc, 0, 1075, 250, 30), System.Drawing.ContentAlignment.MiddleCenter, "Class Advisor");
                PdfTextArea ptcf2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, 80, 1075, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "HOD");
                PdfTextArea ptcf3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, 360, 1075, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "AEC Coordinator ");
                PdfTextArea ptcf4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, 550, 1075, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "Principal");
                mypdfpage.Add(ptcf1);
                mypdfpage.Add(ptcf2);
                mypdfpage.Add(ptcf3);
                mypdfpage.Add(ptcf4);
                double lastrowofstafflabel = newPdfTablePagestafflabel.Area.BottomRightCornerY;
                PdfTablePage newPdfTablePage_3 = myPdfTable_3.CreateTablePage(new PdfArea(myPdfDocument_3, 20, lastrowofstafflabel + 10, 800, (dt_temp_1.Rows.Count + 1) * 34));
                double totalh = newPdfTablePage_3.Area.Height;
                totalh = totalh + lastrowofstafflabel + 10;
                if (totalh < 830)
                {
                    mypdfpage.Add(newPdfTablePagestafflabel);
                    mypdfpage.Add(newPdfTablePage_3);
                    mypdfpage.SaveToDocument();
                    double lastrowofstaffdetails = newPdfTablePage_3.Area.BottomRightCornerY;
                    if (lastrowofstaffdetails <= 620)
                    {
                        if (findrow == 1)
                        {
                            PdfTablePage newPdfTablePageclassadvisor = myPdfTableclassadvisor.CreateTablePage(new PdfArea(mypdfclassadvisor, 20, lastrowofstaffdetails + 20, 200, 100));
                            mypdfpage.Add(newPdfTablePageclassadvisor);
                            double lastrowofclassadvisor = newPdfTablePageclassadvisor.Area.BottomRightCornerY;
                            if (schedukle != null && schedukle.Trim() != "" && schedukle.Trim() != "0")
                            {
                                PdfTablePage newPdfTablePage_1 = myPdfTable_1.CreateTablePage(new PdfArea(myPdfDocument_1, 20, lastrowofclassadvisor + 10, 800, (dt_temp_4.Rows.Count + 1) * 34));
                                mypdfpage.Add(newPdfTablePage_1);
                            }
                        }
                    }
                    else
                    {
                        if (findrow == 1)
                        {
                            mypdfpage = mydoc.NewPage();
                            PdfTablePage newPdfTablePageclassadvisor = myPdfTableclassadvisor.CreateTablePage(new PdfArea(mypdfclassadvisor, 20, -150, 200, 100));
                            mypdfpage.Add(newPdfTablePageclassadvisor);
                            if (schedukle != null && schedukle.Trim() != "" && schedukle.Trim() != "0")
                            {
                                PdfTablePage newPdfTablePage_1 = myPdfTable_1.CreateTablePage(new PdfArea(myPdfDocument_1, 20, -120, 800, (dt_temp_4.Rows.Count + 1) * 34));
                                mypdfpage.Add(newPdfTablePage_1);
                            }
                            ptcf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 0, 1075, 250, 30), System.Drawing.ContentAlignment.MiddleCenter, "Class Advisor");
                            ptcf2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 80, 1075, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "HOD");
                            ptcf3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 360, 1075, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "AEC Coordinator ");
                            ptcf4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 550, 1075, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "Principal");
                            mypdfpage.Add(ptcf1);
                            mypdfpage.Add(ptcf2);
                            mypdfpage.Add(ptcf3);
                            mypdfpage.Add(ptcf4);
                        }
                        //mypdfpage.SaveToDocument();
                    }
                }
                else
                {
                    mypdfpage.SaveToDocument();
                    mypdfpage = mydoc.NewPage();
                    PdfTextArea ptcfst = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                              new PdfArea(mydoc, 20, 50, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, "Staff Details:");
                    mypdfpage.Add(ptcfst);
                    PdfDocument myPdfDocument_5 = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                    PdfTable myPdfTable_5 = myPdfDocument_5.NewTable(new Font("Verdana", 13), dt_temp_1.Rows.Count, 4, 8);
                    myPdfTable_5.ImportDataTable(dt_temp_1);
                    myPdfTable_5.SetColumnsWidth(new int[] { 30, 100, 150, 150 });
                    myPdfTable_5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    //PdfTablePage newPdfTablePagestafflabe2 = myPdfTablestafflabel.CreateTablePage(new PdfArea(mypdfstafflabel, 20, 10, 800, 500));
                    //mypdfpage.Add(newPdfTablePagestafflabe2);
                    PdfTablePage newPdfTablePage4 = myPdfTable_5.CreateTablePage(new PdfArea(myPdfDocument_5, 20, -200, 800, (dt_temp_1.Rows.Count + 1) * 34));
                    mypdfpage.Add(newPdfTablePage4);
                    double lastrowofstaffdetails = newPdfTablePage4.Area.BottomRightCornerY;
                    if (lastrowofstaffdetails <= 620)
                    {
                        PdfTablePage newPdfTablePageclassadvisor = myPdfTableclassadvisor.CreateTablePage(new PdfArea(mypdfclassadvisor, 20, lastrowofstaffdetails + 50, 200, 100));
                        mypdfpage.Add(newPdfTablePageclassadvisor);
                        if (schedukle != null && schedukle.Trim() != "" && schedukle.Trim() != "0")
                        {
                            PdfTablePage newPdfTablePage_1 = myPdfTable_1.CreateTablePage(new PdfArea(myPdfDocument_1, 20, lastrowofstaffdetails + 80, 800, (dt_temp_4.Rows.Count + 1) * 34));
                            mypdfpage.Add(newPdfTablePage_1);
                        }
                        ptcf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydoc, 0, 1075, 250, 30), System.Drawing.ContentAlignment.MiddleCenter, "Class Advisor");
                        ptcf2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 80, 1075, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "HOD");
                        ptcf3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 360, 1075, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "AEC Coordinator ");
                        ptcf4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 550, 1075, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "Principal");
                        mypdfpage.Add(ptcf1);
                        mypdfpage.Add(ptcf2);
                        mypdfpage.Add(ptcf3);
                        mypdfpage.Add(ptcf4);
                    }
                    else
                    {
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydoc.NewPage();
                        PdfTablePage newPdfTablePageclassadvisor = myPdfTableclassadvisor.CreateTablePage(new PdfArea(mypdfclassadvisor, 20, -150, 200, 100));
                        mypdfpage.Add(newPdfTablePageclassadvisor);
                        PdfTablePage newPdfTablePage_1 = myPdfTable_1.CreateTablePage(new PdfArea(myPdfDocument_1, 20, -120, 800, (dt_temp_4.Rows.Count + 1) * 34));
                        mypdfpage.Add(newPdfTablePage_1);
                        ptcf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydoc, 0, 1075, 250, 30), System.Drawing.ContentAlignment.MiddleCenter, "Class Advisor");
                        ptcf2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 80, 1075, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "HOD");
                        ptcf3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 360, 1075, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "AEC Coordinator ");
                        ptcf4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 550, 1075, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "Principal");
                        mypdfpage.Add(ptcf1);
                        mypdfpage.Add(ptcf2);
                        mypdfpage.Add(ptcf3);
                        mypdfpage.Add(ptcf4);
                    }
                }
                mypdfpage.SaveToDocument();
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "TimeTable.pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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
            if (!chkSectionWise.Checked)
            {
                if (ddlsec.Enabled == true)
                {
                    if (ddlsec.SelectedItem.ToString() != "" && ddlsec.SelectedItem.ToString() != "-1" && ddlsec.SelectedItem.ToString() != null)
                    {
                        strsec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                    }
                }
            }
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
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
                                if (ddltimetable.SelectedItem.Text.Trim() == "New")
                                {
                                    gridSelTT.Visible = true;
                                    bindGridTTSelect();
                                }
                                subjtree.Visible = true;
                                chkappend.Visible = true;
                                btnOk.Visible = true;
                                treepanel.Visible = true;
                                FpSpread1.Sheets[0].SetText(rowval, 0, subjtree.Nodes[i].ChildNodes[node_count].Text);
                                FpSpread1.Sheets[0].Cells[rowval, 0].Tag = subjtree.Nodes[i].ChildNodes[node_count].Value;
                                string chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;
                                FpSpread1.Sheets[0].Rows[rowval].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Rows[rowval].Font.Size = FontUnit.Medium;
                                DataSet staf_set = d2.select_method("select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year=" + strbatchyear.ToString() + " " + strsec + ")", hat, "Text");
                                //DataSet staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "' and ct.TT_sec='" + ddlsec.SelectedItem.ToString() + "'  and TT_date>='" + (txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) + "')");
                                //if (true)
                                //{
                                //    staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "' ) ");
                                //}
                                //if (ddltimetable.SelectedItem.Text.Trim() == "New")
                                //{
                                //    //staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "' and ct.TT_sec='" + ddlsec.SelectedItem.ToString() + "') ");
                                //    staf_set = dirAcc.selectDataSet("select staff_code,staff_name from staffmaster where staff_code in (select TT_staffcode from TT_ClassTimetable ct, TT_ClassTimetableDet ctd where ct.TT_ClassPK = ctd.TT_ClassFk and ctd.TT_subno='" + Convert.ToInt32(chile_index) + "' and ct.TT_batchyear='" + strbatchyear.ToString() + "')  ");
                                //}
                                if (staf_set.Tables[0].Rows.Count > 1)
                                {
                                    txtmulstaff.Visible = true;
                                    lblmulstaff.Visible = true;
                                    btnCopySubject.Visible = true;
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

    protected void btnCopySubject_Click(object sender, EventArgs e)
    {
        try
        {
            subjtree_SelectedNodeChanged(sender, e);
        }
        catch { }
    }

    private bool IsTimeTableExistsOrNot(string timeTableName, string batchYear, string degreeCode, string semester, string section, string timeTableDate)
    {
        bool isExists = false;
        try
        {
            DateTime dtTTDate = new DateTime();
            //(txtdate.Text.Split('/')[1] + "/" + txtdate.Text.Split('/')[0] + "/" + txtdate.Text.Split('/')[2]) 
            DateTime.TryParseExact(timeTableDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtTTDate);
            string qrySection = string.Empty;
            if (!string.IsNullOrEmpty(section))
            {
                qrySection = " and LTRIM(RTRIM(ISNULL(TT_Sec,''))) ='" + section + "'";
            }
            string strtimetable = "select TT_classpk from TT_ClassTimetable ct where ct.TT_colCode='" + Convert.ToString(ddlcolg.SelectedValue) + "' and ct.TT_degCode='" + degreeCode + "' and ct.TT_batchyear='" + batchYear + "' and ct.TT_sem='" + semester + "' " + qrySection + " and TT_date>='" + dtTTDate.ToString("MM/dd/yyyy") + "' and TT_name='" + timeTableName + "'";
            string ttpk = dirAcc.selectScalarString(strtimetable);
            if (!string.IsNullOrEmpty(ttpk))
            {
                isExists = true;
            }
        }
        catch { }
        return isExists;
    }

}
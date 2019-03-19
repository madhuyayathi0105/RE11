using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;

public partial class Class_Time_Table : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string strstaffcode = "";
    Hashtable hat = new Hashtable();
    Dictionary<string, string> dicDbCol = new Dictionary<string, string>();
    Dictionary<string, string> dicDays = new Dictionary<string, string>();
    Dictionary<string, string> class_tt_dic = new Dictionary<string, string>();
    Dictionary<string, string> class_tt_det_dic = new Dictionary<string, string>();
    Dictionary<string, string> multiple_dic = new Dictionary<string, string>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
            Response.Redirect("~/Default.aspx");
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        strstaffcode = Session["Staff_Code"].ToString();

        if (!IsPostBack)
        {
            string DegCode = "0";
            string Batch = "0";
            string Branch = "0";
            string Sem = "0";
            bindcollege();
            collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
            setLabelText();
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddlDegree.Items.Count > 0)
                DegCode = Convert.ToString(ddlDegree.SelectedItem.Value);
            BindBranch(singleuser, group_user, DegCode, collegecode, usercode);
            if (ddlBatch.Items.Count > 0)
                Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
            if (ddlBranch.Items.Count > 0)
                Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
            BindSem(Branch, Batch, collegecode);
            if (ddlSem.Items.Count > 0)
                Sem = Convert.ToString(ddlSem.SelectedItem.Value);
            BindSectionDetail(Batch, Branch, Sem, collegecode);
            txtFrmDt.Attributes.Add("readonly", "readonly");
            txtToDt.Attributes.Add("readonly", "readonly");
            loadcolumns(sender, e);
        }
        collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
    }

    private void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                columnfield = " and group_code='" + group_code + "'";
            else
                columnfield = " and user_code='" + Session["usercode"] + "'";

            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception e) { }
    }

    private void BindBatch()
    {
        try
        {
            string DegCode = "0";
            string Batch = "0";
            string Branch = "0";
            string Sem = "0";
            ds.Dispose();
            ds.Reset();
            string Master1 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string collegecode = Session["collegecode"].ToString();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' order by batch_year desc";
            ds = d2.select_method_wo_parameter(strbinddegree, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
            }
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddlDegree.Items.Count > 0)
                DegCode = Convert.ToString(ddlDegree.SelectedItem.Value);
            if (ddlBatch.Items.Count > 0)
                Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
            if (ddlBranch.Items.Count > 0)
                Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
            BindBranch(singleuser, group_user, DegCode, collegecode, usercode);
            BindSem(Branch, Batch, collegecode);
            if (ddlSem.Items.Count > 0)
                Sem = Convert.ToString(ddlSem.SelectedItem.Value);
            BindSectionDetail(Batch, Branch, Sem, collegecode);
        }
        catch (Exception ex)
        {
            lblMainErr.Text = ex.ToString();
        }
    }

    private void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            string DegCode = "0";
            string Batch = "0";
            string Branch = "0";
            string Sem = "0";
            ddlDegree.Items.Clear();
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
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
            }
            if (ddlDegree.Items.Count > 0)
                DegCode = Convert.ToString(ddlDegree.SelectedItem.Value);
            if (ddlBatch.Items.Count > 0)
                Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
            if (ddlBranch.Items.Count > 0)
                Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
            BindBranch(singleuser, group_user, DegCode, collegecode, usercode);
            BindSem(Branch, Batch, collegecode);
            if (ddlSem.Items.Count > 0)
                Sem = Convert.ToString(ddlSem.SelectedItem.Value);
            BindSectionDetail(Batch, Branch, Sem, collegecode);
        }
        catch (Exception ex)
        {
            lblMainErr.Text = ex.ToString();
        }
    }

    private void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = "0";
            string Batch = "0";
            string Branch = "0";
            string Sem = "0";
            if (ddlDegree.Items.Count > 0)
                course_id = Convert.ToString(ddlDegree.SelectedItem.Value);
            ddlBranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
            if (ddlBatch.Items.Count > 0)
                Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
            if (ddlBranch.Items.Count > 0)
                Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
            BindSem(Branch, Batch, collegecode);
            if (ddlSem.Items.Count > 0)
                Sem = Convert.ToString(ddlSem.SelectedItem.Value);
            BindSectionDetail(Batch, Branch, Sem, collegecode);
        }
        catch (Exception ex)
        {
            lblMainErr.Text = "Please Select the Degree";
        }
    }

    private void BindSectionDetail(string strbatch, string strbranch, string strSem, string Collcode)
    {
        try
        {
            strbatch = "0";
            if (ddlBatch.Items.Count > 0)
                strbatch = ddlBatch.SelectedValue.ToString();
            strbranch = "0";
            if (ddlBranch.Items.Count > 0)
                strbranch = ddlBranch.SelectedValue.ToString();
            strSem = "0";
            if (ddlSem.Items.Count > 0)
                strSem = ddlSem.SelectedItem.Value.ToString();
            ddlSec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("select distinct TT_sec from TT_ClassTimeTable T,TT_ClassTimeTabledet TT Where T.TT_ClassPK=TT.TT_ClassFK and TT_degCode='" + strbranch + "' and TT_batchyear='" + strbatch + "' and TT_sem='" + strSem + "' and TT_ColCode='" + Collcode + "'", "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "TT_sec";
                ddlSec.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblMainErr.Text = ex.ToString();
        }
    }

    private void BindSem(string strbranch, string strbatchyear, string collegecode)
    {
        try
        {
            strbatchyear = "0";
            if (ddlBatch.Items.Count > 0)
                strbatchyear = ddlBatch.SelectedValue.ToString();
            strbranch = "0";
            if (ddlBranch.Items.Count > 0)
                strbranch = ddlBranch.SelectedValue.ToString();
            ddlSem.Items.Clear();
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
                        ddlSem.Items.Add(i.ToString());
                    else if (first_year == true && i != 2)
                        ddlSem.Items.Add(i.ToString());
                }
            }
        }
        catch (Exception ex) { lblMainErr.Text = ex.ToString(); }
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        string DegCode = "0";
        string Batch = "0";
        string Branch = "0";
        string Sem = "0";
        collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        BindBatch();
        BindDegree(singleuser, group_user, collegecode, usercode);
        if (ddlDegree.Items.Count > 0)
            DegCode = Convert.ToString(ddlDegree.SelectedItem.Value);
        BindBranch(singleuser, group_user, DegCode, collegecode, usercode);
        if (ddlBatch.Items.Count > 0)
            Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
        if (ddlBranch.Items.Count > 0)
            Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
        BindSem(Branch, Batch, collegecode);
        if (ddlSem.Items.Count > 0)
            Sem = Convert.ToString(ddlSem.SelectedItem.Value);
        BindSectionDetail(Batch, Branch, Sem, collegecode);
        loadcolumns(sender, e);
    }

    protected void ddlBatch_Change(object sender, EventArgs e)
    {
        string Batch = "0";
        string Branch = "0";
        string Sem = "0";
        if (ddlBatch.Items.Count > 0)
            Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
        if (ddlBranch.Items.Count > 0)
            Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
        BindSem(Branch, Batch, collegecode);
        if (ddlSem.Items.Count > 0)
            Sem = Convert.ToString(ddlSem.SelectedItem.Value);
        BindSectionDetail(Batch, Branch, Sem, collegecode);
    }

    protected void ddlDegree_Change(object sender, EventArgs e)
    {
        string DegCode = "0";
        string Batch = "0";
        string Branch = "0";
        string Sem = "0";
        if (ddlDegree.Items.Count > 0)
            DegCode = Convert.ToString(ddlDegree.SelectedItem.Value);
        if (ddlBatch.Items.Count > 0)
            Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
        if (ddlBranch.Items.Count > 0)
            Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
        BindBranch(singleuser, group_user, DegCode, collegecode, usercode);
        BindSem(Branch, Batch, collegecode);
        if (ddlSem.Items.Count > 0)
            Sem = Convert.ToString(ddlSem.SelectedItem.Value);
        BindSectionDetail(Batch, Branch, Sem, collegecode);
    }

    protected void ddlBranch_Change(object sender, EventArgs e)
    {
        string Batch = "0";
        string Branch = "0";
        string Sem = "0";
        if (ddlBatch.Items.Count > 0)
            Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
        if (ddlBranch.Items.Count > 0)
            Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
        BindSem(Branch, Batch, collegecode);
        if (ddlSem.Items.Count > 0)
            Sem = Convert.ToString(ddlSem.SelectedItem.Value);
        BindSectionDetail(Batch, Branch, Sem, collegecode);
    }

    protected void ddlSem_Change(object sender, EventArgs e)
    {
        string Batch = "0";
        string Branch = "0";
        string Sem = "0";
        if (ddlBatch.Items.Count > 0)
            Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
        if (ddlBranch.Items.Count > 0)
            Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
        if (ddlSem.Items.Count > 0)
            Sem = Convert.ToString(ddlSem.SelectedItem.Value);
        BindSectionDetail(Batch, Branch, Sem, collegecode);
    }

    protected void radSemWise_Change(object sender, EventArgs e)
    {
        tdlbFrm.Visible = false;
        txtFrmDt.Visible = false;
        lblToDt.Visible = false;
        txtToDt.Visible = false;
    }

    protected void radDayWise_Change(object sender, EventArgs e)
    {
        tdlbFrm.Visible = true;
        txtFrmDt.Visible = true;
        lblToDt.Visible = true;
        txtToDt.Visible = true;
        txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        class_tt_dic.Clear();
        class_tt_det_dic.Clear();
        loadcolumns(sender, e);
        if (ddlBatch.Items.Count == 0)
        {
            lblMainErr.Visible = true;
            lblMainErr.Text = "Please Select any Batch Year!";
            grdClass_TT.Visible = false;
            grdClassDet_TT.Visible = false;
            btnComPrint.Visible = false;
            return;
        }
        if (ddlDegree.Items.Count == 0)
        {
            lblMainErr.Visible = true;
            lblMainErr.Text = "Please Select any Degree!";
            grdClass_TT.Visible = false;
            grdClassDet_TT.Visible = false;
            btnComPrint.Visible = false;
            return;
        }
        if (ddlBranch.Items.Count == 0)
        {
            lblMainErr.Visible = true;
            lblMainErr.Text = "Please Select any Branch!";
            grdClass_TT.Visible = false;
            grdClassDet_TT.Visible = false;
            btnComPrint.Visible = false;
            return;
        }
        if (ddlSem.Items.Count == 0)
        {
            lblMainErr.Visible = true;
            lblMainErr.Text = "Please Select any Semester!";
            grdClass_TT.Visible = false;
            grdClassDet_TT.Visible = false;
            btnComPrint.Visible = false;
            return;
        }
        bindClassTT();
    }

    private void bindClassTT()
    {
        try
        {
            DataSet dsGetSchOrd = new DataSet();
            DataSet dsBind = new DataSet();
            DataView dvBind = new DataView();
            DataTable dtStfTT = new DataTable();
            DataRow drStfTT;
            int noofDays = 0;
            string SchOrd = "";
            string GetSchOrd = "select distinct schOrder,nodays from PeriodAttndSchedule p,BellSchedule b,syllabus_master sy where b.Degree_Code =sy.degree_code and b.batch_year =sy.Batch_Year and b.semester =sy.semester and p.degree_code =b.Degree_Code and p.semester =b.semester";
            dsGetSchOrd.Clear();
            dsGetSchOrd = d2.select_method_wo_parameter(GetSchOrd, "Text");
            if (dsGetSchOrd.Tables.Count > 0 && dsGetSchOrd.Tables[0].Rows.Count > 0)
            {
                Int32.TryParse(Convert.ToString(dsGetSchOrd.Tables[0].Rows[0]["nodays"]), out noofDays);
                SchOrd = Convert.ToString(dsGetSchOrd.Tables[0].Rows[0]["schOrder"]);
                if (noofDays > 0)
                {
                    string GetPeriod = "select Period1,Convert(varchar(5),start_time,108) as start_time,Convert(varchar(5),end_time,108) as end_time from BellSchedule  where Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "' and batch_year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' and semester='" + Convert.ToString(ddlSem.SelectedItem.Text) + "' order by start_time,end_time";
                    dsBind.Clear();
                    dsBind = d2.select_method_wo_parameter(GetPeriod, "Text");
                    if (dsBind.Tables.Count > 0 && dsBind.Tables[0].Rows.Count > 0)
                    {
                        dtStfTT.Columns.Add("Day/Period");
                        for (int ttcol = 0; ttcol < dsBind.Tables[0].Rows.Count; ttcol++)
                        {
                            dtStfTT.Columns.Add(Convert.ToString(dsBind.Tables[0].Rows[ttcol]["start_time"]) + "-" + Convert.ToString(dsBind.Tables[0].Rows[ttcol]["end_time"]));
                        }
                        bool IsNotExist = false;
                        if (SchOrd.Trim() == "1")
                        {
                            drStfTT = dtStfTT.NewRow();
                            for (int ttcol = 0; ttcol < dsBind.Tables[0].Rows.Count; ttcol++)
                            {
                                drStfTT[ttcol + 1] = Convert.ToString(dsBind.Tables[0].Rows[ttcol]["Period1"]);
                            }
                            dtStfTT.Rows.Add(drStfTT);
                            dtStfTT.Rows.Add("Monday");
                            dtStfTT.Rows.Add("Tuesday");
                            dtStfTT.Rows.Add("Wednesday");
                            dtStfTT.Rows.Add("Thursday");
                            dtStfTT.Rows.Add("Friday");
                            dtStfTT.Rows.Add("Saturday");
                            dtStfTT.Rows.Add("Sunday");
                            if (noofDays < dtStfTT.Rows.Count)
                                dtStfTT.Rows.Remove(dtStfTT.Rows[dtStfTT.Rows.Count - (dtStfTT.Rows.Count - noofDays) + 1]);
                        }
                        else if (SchOrd.Trim() == "0")
                        {
                            drStfTT = dtStfTT.NewRow();
                            for (int ttcol = 0; ttcol < dsBind.Tables[0].Rows.Count; ttcol++)
                            {
                                drStfTT[ttcol + 1] = Convert.ToString(dsBind.Tables[0].Rows[ttcol]["Period1"]);
                            }
                            dtStfTT.Rows.Add("Day1");
                            dtStfTT.Rows.Add("Day2");
                            dtStfTT.Rows.Add("Day3");
                            dtStfTT.Rows.Add("Day4");
                            dtStfTT.Rows.Add("Day5");
                            dtStfTT.Rows.Add("Day6");
                            dtStfTT.Rows.Add("Day7");
                            if (noofDays < dtStfTT.Rows.Count)
                                dtStfTT.Rows.Remove(dtStfTT.Rows[dtStfTT.Rows.Count - (dtStfTT.Rows.Count - noofDays) + 1]);
                        }
                        else
                        {
                            IsNotExist = true;
                        }
                        if (IsNotExist == false)
                        {
                            btnComPrint.Visible = true;
                            lblMainErr.Visible = false;
                            grdClass_TT.Visible = true;
                            grdClass_TT.DataSource = dtStfTT;
                            grdClass_TT.DataBind();
                            bindGrdValues(SchOrd, noofDays, dtStfTT);
                            bindColor();
                        }
                        else
                        {
                            btnComPrint.Visible = false;
                            grdClass_TT.Visible = false;
                            grdClassDet_TT.Visible = false;
                            lblMainErr.Visible = true;
                            lblMainErr.Text = "Day Order Not Available!";
                        }
                    }
                    else
                    {
                        btnComPrint.Visible = false;
                        grdClass_TT.Visible = false;
                        grdClassDet_TT.Visible = false;
                        lblMainErr.Visible = true;
                        lblMainErr.Text = "No Record(s) Found!";
                    }
                }
                else
                {
                    btnComPrint.Visible = false;
                    grdClass_TT.Visible = false;
                    grdClassDet_TT.Visible = false;
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "Days Not Available!";
                }
            }
            else
            {
                btnComPrint.Visible = false;
                grdClass_TT.Visible = false;
                grdClassDet_TT.Visible = false;
                lblMainErr.Visible = true;
                lblMainErr.Text = "No Record(s) Found!";
            }
        }
        catch { }
    }

    private void bindHasColumns()
    {
        dicDbCol.Clear();
        dicDbCol.Add("SUBJECT CODE", "subject_code");
        dicDbCol.Add("SUBJECT NAME", "subject_name");
        dicDbCol.Add("STAFF CODE", "TT_staffcode");
        dicDbCol.Add("STAFF NAME", "staff_name");
        dicDbCol.Add("ROOM NAME", "Room_Name");
    }

    private void bindGrdValues(string SchOrder, int NoofDays, DataTable myDataTable)
    {
        try
        {
            bindHasColumns();
            string SelDayOrd = "";
            if (SchOrder.Trim() == "1")
                SelDayOrd = " Select TT_Day_DayorderPK,Daydiscription from TT_Day_Dayorder where Daytype='0'";
            else if (SchOrder.Trim() == "0")
                SelDayOrd = " Select TT_Day_DayorderPK,Daydiscription from TT_Day_Dayorder where Daytype='1'";
            else
            {
                btnComPrint.Visible = false;
                grdClass_TT.Visible = false;
                grdClassDet_TT.Visible = false;
                lblMainErr.Visible = true;
                lblMainErr.Text = "Day Order is InValid!";
                return;
            }
            SelDayOrd = SelDayOrd + " select distinct TT_subno,TT_staffcode,TT_Hour,TT_Day,TT_Room,s.subject_name,s.subject_code,SM.staff_name,R.Room_Name from TT_ClassTimeTable T,TT_ClassTimeTabledet TT,Subject S,StaffMaster SM,Room_detail R Where T.TT_ClassPK=TT.TT_ClassFK and S.subject_no=TT_subno and SM.staff_code=TT_staffCode and TT_room=R.RoomPk and TT_degCode='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "' and TT_batchyear='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' and TT_sem='" + Convert.ToString(ddlSem.SelectedItem.Text) + "' and TT_ColCode='" + collegecode + "'";
            if (ddlSec.Items.Count > 0)
                SelDayOrd = SelDayOrd + " and TT_Sec='" + Convert.ToString(ddlSec.SelectedItem.Text) + "'";
            SelDayOrd = SelDayOrd + " order by TT_Day,TT_Hour";
            SelDayOrd = SelDayOrd + " select distinct TT_staffcode,s.subject_name,s.subject_code,SM.staff_name from TT_ClassTimeTable T,TT_ClassTimeTabledet TT,Subject S,StaffMaster SM,Room_detail R Where T.TT_ClassPK=TT.TT_ClassFK and S.subject_no=TT_subno and SM.staff_code=TT_staffCode and TT_room=R.RoomPk and TT_degCode='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "' and TT_batchyear='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' and TT_sem='" + Convert.ToString(ddlSem.SelectedItem.Text) + "' and TT_ColCode='" + collegecode + "'";
            if (ddlSec.Items.Count > 0)
                SelDayOrd = SelDayOrd + " and TT_Sec='" + Convert.ToString(ddlSec.SelectedItem.Text) + "'";
            DataSet dsDayOrd = new DataSet();
            DataView dvDayOrd = new DataView();
            DataView dvVal = new DataView();
            dsDayOrd = d2.select_method_wo_parameter(SelDayOrd, "Text");
            if (dsDayOrd.Tables.Count > 0 && dsDayOrd.Tables[0].Rows.Count > 0)
            {
                bool IsDayExist = true;
                int headerColumnCount = grdClass_TT.HeaderRow.Cells.Count;
                int index = 0;
                for (int ro = 1; ro < grdClass_TT.Rows.Count; ro++)
                {
                    dsDayOrd.Tables[0].DefaultView.RowFilter = " Daydiscription='" + Convert.ToString(grdClass_TT.Rows[ro].Cells[0].Text) + "'";
                    dvDayOrd = dsDayOrd.Tables[0].DefaultView;
                    if (dvDayOrd.Count > 0)
                    {
                        string DayFK = Convert.ToString(dvDayOrd[0]["TT_Day_DayorderPK"]);
                        if (!String.IsNullOrEmpty(DayFK.Trim()) && DayFK.Trim() != "0")
                        {
                            for (int co = 1; co < headerColumnCount; co++)
                            {
                                string ColHour = Convert.ToString(grdClass_TT.Rows[0].Cells[co].Text);
                                if (!String.IsNullOrEmpty(ColHour) && ColHour.Trim() != "0")
                                {
                                    if (dsDayOrd.Tables[1].Rows.Count > 0)
                                    {
                                        string myGetVal = ""; string getcolorval = "";
                                        int myHour = 0;
                                        Int32.TryParse(ColHour, out myHour);
                                        if (myHour > 0)
                                        {
                                            dsDayOrd.Tables[1].DefaultView.RowFilter = " TT_Day='" + DayFK.Trim() + "' and TT_Hour='" + myHour + "'";
                                            dvVal = dsDayOrd.Tables[1].DefaultView;
                                            if (dvVal.Count > 0)
                                            {
                                                for (int ik = 0; ik < dvVal.Count; ik++)
                                                {
                                                    string GetVal = ""; string colorvalue = "";
                                                    for (int colOrd = 0; colOrd < cblcolumnorder.Items.Count; colOrd++)
                                                    {
                                                        if (cblcolumnorder.Items[colOrd].Selected == true)
                                                        {
                                                            if (GetVal.Trim() == "")
                                                                GetVal = Convert.ToString(dvVal[ik]["" + dicDbCol[cblcolumnorder.Items[colOrd].Text]]).Trim() + "";
                                                            else
                                                                GetVal = GetVal + "$" + Convert.ToString(dvVal[ik]["" + dicDbCol[cblcolumnorder.Items[colOrd].Text]]).Trim() + "";
                                                        }
                                                        if (colOrd == 0 || colOrd == 2)
                                                        {
                                                            if (colorvalue.Trim() == "")
                                                                colorvalue = Convert.ToString(dvVal[ik]["" + dicDbCol[cblcolumnorder.Items[colOrd].Text]]).Trim() + "";
                                                            else
                                                                colorvalue = colorvalue + "$" + Convert.ToString(dvVal[ik]["" + dicDbCol[cblcolumnorder.Items[colOrd].Text]]).Trim() + "";
                                                        }
                                                    }

                                                    if (myGetVal.Trim() == "")
                                                        myGetVal = GetVal;
                                                    else
                                                        myGetVal = myGetVal + ";\n" + GetVal;

                                                    if (getcolorval.Trim() == "")
                                                        getcolorval = colorvalue.Trim();
                                                    else
                                                        getcolorval = getcolorval + ";\n" + colorvalue.Trim();
                                                }
                                                myDataTable.Rows[ro][co] = myGetVal;
                                                if (!class_tt_dic.ContainsKey(getcolorval.Trim()))
                                                {
                                                    index++;
                                                    string bgcolor = getColor(index);
                                                    class_tt_dic.Add(getcolorval.Trim(), bgcolor);
                                                }
                                                if (!class_tt_det_dic.ContainsKey(myGetVal.Trim()))
                                                {
                                                    class_tt_det_dic.Add(myGetVal.Trim(), getcolorval.Trim());
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    IsDayExist = false;
                                }
                            }
                        }
                        else
                        {
                            IsDayExist = false;
                        }
                    }
                    else
                    {
                        IsDayExist = false;
                    }
                }
                if (IsDayExist == true)
                {
                    btnComPrint.Visible = true;
                    grdClass_TT.Visible = true;
                    grdClass_TT.DataSource = myDataTable;
                    grdClass_TT.DataBind();
                    bindDetGrd(SchOrder, NoofDays, dsDayOrd);
                    lblMainErr.Visible = false;
                }
                else
                {
                    btnComPrint.Visible = false;
                    grdClass_TT.Visible = false;
                    grdClassDet_TT.Visible = false;
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "Day Order is InValid!";
                }
            }
            else
            {
                btnComPrint.Visible = false;
                grdClass_TT.Visible = false;
                grdClassDet_TT.Visible = false;
                lblMainErr.Visible = true;
                lblMainErr.Text = "Day Order Not Available!";
            }
        }
        catch { }
    }

    private void LoadDates()
    {
        dicDays.Clear();
        dicDays.Add("Mon", "Monday");
        dicDays.Add("Tue", "Tuesday");
        dicDays.Add("Wed", "Wednesday");
        dicDays.Add("Thu", "Thursday");
        dicDays.Add("Fri", "Friday");
        dicDays.Add("Sat", "Saturday");
        dicDays.Add("Sun", "Sunday");
    }

    private void bindDetGrd(string mySchOrd, int noDays, DataSet dsDetVal)
    {
        try
        {
            LoadDates();
            DataView dvGetDay = new DataView();
            DataView dvGetVal = new DataView();
            DataView dvFinVal = new DataView();
            DataTable dtDet = new DataTable();
            Dictionary<string, string> dicRoom = new Dictionary<string, string>();
            DataRow drDet;
            dtDet.Columns.Add("Staff Code");
            dtDet.Columns.Add("Staff Name");
            dtDet.Columns.Add("Subject Code");
            dtDet.Columns.Add("Subject Name");

            if (mySchOrd.Trim() == "1")
            {
                dtDet.Columns.Add("Mon");
                dtDet.Columns.Add("Tue");
                dtDet.Columns.Add("Wed");
                dtDet.Columns.Add("Thu");
                dtDet.Columns.Add("Fri");
                dtDet.Columns.Add("Sat");
                dtDet.Columns.Add("Sun");
                if (noDays < (dtDet.Columns.Count - 4))
                    dtDet.Columns.Remove(dtDet.Columns[(dtDet.Columns.Count - (dtDet.Columns.Count - noDays)) + 4]);
            }
            else if (mySchOrd.Trim() == "0")
            {
                dtDet.Columns.Add("Day1");
                dtDet.Columns.Add("Day2");
                dtDet.Columns.Add("Day3");
                dtDet.Columns.Add("Day4");
                dtDet.Columns.Add("Day5");
                dtDet.Columns.Add("Day6");
                dtDet.Columns.Add("Day7");
                if (noDays < (dtDet.Columns.Count - 4))
                    dtDet.Columns.Remove(dtDet.Columns[(dtDet.Columns.Count - (dtDet.Columns.Count - noDays)) + 4]);
            }

            if (dsDetVal.Tables.Count > 0 && dsDetVal.Tables[0].Rows.Count > 0 && dsDetVal.Tables[1].Rows.Count > 0 && dsDetVal.Tables[2].Rows.Count > 0)
            {
                bool EntryVal = false;
                for (int dsRow = 0; dsRow < dsDetVal.Tables[2].Rows.Count; dsRow++)
                {
                    bool myEntryVal = false;
                    string Staf_Code = Convert.ToString(dsDetVal.Tables[2].Rows[dsRow]["TT_staffcode"]);
                    string Staf_Name = Convert.ToString(dsDetVal.Tables[2].Rows[dsRow]["staff_name"]);
                    string subj_Code = Convert.ToString(dsDetVal.Tables[2].Rows[dsRow]["subject_code"]);
                    string subj_Name = Convert.ToString(dsDetVal.Tables[2].Rows[dsRow]["subject_name"]);

                    drDet = dtDet.NewRow();
                    drDet[0] = Staf_Code.Trim();
                    drDet[1] = Staf_Name.Trim();
                    drDet[2] = subj_Code.Trim();
                    drDet[3] = subj_Name.Trim();

                    int ColIdx = 4;
                    dsDetVal.Tables[1].DefaultView.RowFilter = " TT_staffcode='" + Staf_Code + "' and staff_name='" + Staf_Name + "' and subject_code='" + subj_Code + "' and subject_name='" + subj_Name + "'";
                    dvGetVal = dsDetVal.Tables[1].DefaultView;
                    if (dvGetVal.Count > 0)
                    {
                        DataTable dtdvGetVal = dvGetVal.ToTable();
                        for (int iCol = ColIdx; iCol < dtDet.Columns.Count; iCol++)
                        {
                            dicRoom.Clear();
                            string GetVal = "";
                            string Date = Convert.ToString(dicDays[Convert.ToString(dtDet.Columns[iCol].ColumnName)]);
                            dsDetVal.Tables[0].DefaultView.RowFilter = " Daydiscription='" + Date + "'";
                            dvGetDay = dsDetVal.Tables[0].DefaultView;
                            if (dvGetDay.Count > 0)
                            {
                                string DayFk = Convert.ToString(dvGetDay[0]["TT_Day_DayorderPK"]);
                                if (dtdvGetVal.Rows.Count > 0)
                                {
                                    dtdvGetVal.DefaultView.RowFilter = " TT_Day='" + DayFk + "'";
                                    dvFinVal = dtdvGetVal.DefaultView;
                                    if (dvFinVal.Count > 0)
                                    {
                                        for (int Finval = 0; Finval < dvFinVal.Count; Finval++)
                                        {
                                            if (dicRoom.ContainsKey(Convert.ToString(dvFinVal[Finval]["Room_Name"])))
                                            {
                                                string GetDicVal = Convert.ToString(dicRoom[Convert.ToString(dvFinVal[Finval]["Room_Name"])]);
                                                GetDicVal = GetDicVal + "," + Convert.ToString(dvFinVal[Finval]["TT_Hour"]);
                                                dicRoom.Remove(Convert.ToString(dvFinVal[Finval]["Room_Name"]));
                                                dicRoom.Add(Convert.ToString(dvFinVal[Finval]["Room_Name"]), GetDicVal);
                                            }
                                            else
                                            {
                                                dicRoom.Add(Convert.ToString(dvFinVal[Finval]["Room_Name"]), Convert.ToString(dvFinVal[Finval]["TT_Hour"]));
                                            }
                                        }
                                        if (dicRoom.Count > 0)
                                        {
                                            foreach (KeyValuePair<string, string> myDict in dicRoom)
                                            {
                                                if (GetVal.Trim() == "")
                                                    GetVal = Convert.ToString(myDict.Value + "-" + myDict.Key);
                                                else
                                                    GetVal = GetVal + ";" + Convert.ToString(myDict.Value + "-" + myDict.Key);
                                            }
                                        }
                                        if (GetVal.Trim() != "")
                                        {
                                            drDet[iCol] = GetVal;
                                            EntryVal = true;
                                            myEntryVal = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (myEntryVal == true)
                    {
                        dtDet.Rows.Add(drDet);
                    }
                }
                if (EntryVal == true)
                {
                    grdClassDet_TT.Visible = true;
                    grdClassDet_TT.DataSource = dtDet;
                    grdClassDet_TT.DataBind();
                }
                else
                {
                    grdClassDet_TT.Visible = false;
                }
            }
            else
            {
                grdClassDet_TT.Visible = false;
            }
        }
        catch { }
    }

    private void bindColor()
    {
        try
        {
            if (grdClass_TT.Rows.Count > 0)
            {
                for (int ro = 0; ro < grdClass_TT.Rows.Count; ro++)
                {
                    if (ro == 0)
                    {
                        grdClass_TT.Rows[ro].Font.Bold = true;
                        grdClass_TT.Rows[ro].Font.Name = "Book Antiqua";
                        grdClass_TT.Rows[ro].Font.Size = FontUnit.Medium;
                        grdClass_TT.Rows[ro].HorizontalAlign = HorizontalAlign.Center;
                        grdClass_TT.Rows[ro].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    }
                    else
                    {
                        grdClass_TT.Rows[ro].Cells[0].Font.Bold = true;
                        grdClass_TT.Rows[ro].Cells[0].Font.Name = "Book Antiqua";
                        grdClass_TT.Rows[ro].Cells[0].Font.Size = FontUnit.Medium;
                        grdClass_TT.Rows[ro].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdClass_TT.Rows[ro].Cells[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    }
                }
            }
        }
        catch { }
    }

    public bool columncount()
    {
        bool colorder = false;
        try
        {
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == true)
                    colorder = true;
            }
        }
        catch { }
        return colorder;
    }

    public void loadcolumns(object sender, EventArgs e)
    {
        try
        {
            string columnvalue = "";
            int clsupdate = 0;
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='TT_Class_ColOrder' and  user_code='" + usercode + "' and college_code='" + collegecode + "'";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (columncount() == true)
            {
                if (cblcolumnorder.Items.Count > 0)
                {
                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                    {
                        if (cblcolumnorder.Items[i].Selected == true)
                        {
                            if (columnvalue == "")
                                columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                            else
                                columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0 && dscol.Tables[0].Rows.Count > 0)
            {
                for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                {
                    string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                    string[] valuesplit = value.Split(',');
                    if (valuesplit.Length > 0)
                    {
                        for (int k = 0; k < valuesplit.Length; k++)
                        {
                            if (columnvalue == "")
                                columnvalue = Convert.ToString(valuesplit[k]);
                            else
                                columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                    if (columnvalue == "")
                        columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                    else
                        columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='TT_Class_ColOrder' and college_code='" + collegecode + "' and user_Code='" + usercode + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='TT_Class_ColOrder' and user_code='" + usercode + "' and college_code='" + collegecode + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('TT_Class_ColOrder','" + columnvalue + "','" + usercode + "','" + collegecode + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='TT_Class_ColOrder' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
                DataSet dscolor = new DataSet();
                dscolor.Clear();
                dscolor = d2.select_method_wo_parameter(sel, "Text");
                if (dscolor.Tables.Count > 0)
                {
                    int count = 0;
                    if (dscolor.Tables[0].Rows.Count > 0)
                    {
                        string value = Convert.ToString(dscolor.Tables[0].Rows[0]["LinkValue"]);
                        string[] value1 = value.Split(',');
                        if (value1.Length > 0)
                        {
                            for (int i = 0; i < value1.Length; i++)
                            {
                                string val = value1[i].ToString();
                                for (int k = 0; k < cblcolumnorder.Items.Count; k++)
                                {
                                    if (val == cblcolumnorder.Items[k].Value)
                                    {
                                        cblcolumnorder.Items[k].Selected = true;
                                        count++;
                                    }
                                }
                            }
                            if (count == cblcolumnorder.Items.Count)
                                CheckBox_column.Checked = true;
                            else
                                CheckBox_column.Checked = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { }
    }

    protected void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                tborder.Text = "";
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                        colname12 = Convert.ToString(cblcolumnorder.Items[i].Text) + "(" + (j).ToString() + ")";
                    else
                        colname12 = colname12 + "," + Convert.ToString(cblcolumnorder.Items[i].Text) + "(" + (j).ToString() + ")";
                }
                tborder.Text = colname12;
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                }
                tborder.Text = "";
                tborder.Visible = false;
            }
            tborder.Visible = true;
        }
        catch (Exception ex) { }
    }

    protected void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        cblcolumnorder.ClearSelection();
        CheckBox_column.Checked = false;
        lnk_columnorder.Visible = false;
        tborder.Text = "";
        tborder.Visible = true;
    }

    protected void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);

            int SelCount = 0;
            lnk_columnorder.Visible = true;
            string colname12 = "";
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == true)
                {
                    SelCount += 1;
                    if (colname12 == "")
                        colname12 = Convert.ToString(cblcolumnorder.Items[i].Text) + "(" + (SelCount).ToString() + ")";
                    else
                        colname12 = colname12 + "," + Convert.ToString(cblcolumnorder.Items[i].Text) + "(" + (SelCount).ToString() + ")";
                }
            }
            tborder.Text = colname12;
            if (SelCount == 7)
                CheckBox_column.Checked = true;
            if (SelCount == 0)
                lnk_columnorder.Visible = false;
            tborder.Visible = true;
        }
        catch (Exception ex) { }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

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
        lbl.Add(lblColl);
        fields.Add(0);

        lbl.Add(lblDeg);
        fields.Add(2);

        lbl.Add(lblBranch);
        fields.Add(3);

        lbl.Add(lblSem);
        fields.Add(4);

        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    private string getColor(int index)
    {
        List<string> clrList = NewStringColors();
        return clrList[index];
    }
    private List<string> NewStringColors()
    {
        List<string> clrList = new List<string>();
        clrList.Add("#FEB739");
        clrList.Add("#FF6863");
        clrList.Add("#55D2FF");
        clrList.Add("#C6C6C6");
        clrList.Add("#C5C47B");
        clrList.Add("#CDDC39");
        clrList.Add("#B5E496");
        clrList.Add("#AFDEF8");
        clrList.Add("#F9C4CE");
        clrList.Add("#8EA39A");
        clrList.Add("#7283D1");
        clrList.Add("#06D995");
        clrList.Add("#4CAF50");
        clrList.Add("#57BC30");
        clrList.Add("#8BC34A");
        clrList.Add("#FFCCCC");
        clrList.Add("#FF9800");
        clrList.Add("#00BCD4");
        clrList.Add("#009688");
        clrList.Add("#FF033B");
        clrList.Add("#FF5722");
        clrList.Add("#795548");
        clrList.Add("#9E9E9E");
        clrList.Add("#607D8B");
        clrList.Add("#03A9F4");
        clrList.Add("#E91E63");
        clrList.Add("#CDDC39");
        clrList.Add("#F06292");
        clrList.Add("#3F51B5");
        clrList.Add("#FFC107");
        clrList.Add("#CC0066");
        clrList.Add("#CCCC99");
        clrList.Add("#00CCCC");
        clrList.Add("#FF33CC");
        clrList.Add("#CCFF00");
        clrList.Add("#CCCCCC");
        clrList.Add("#FFCC99");
        clrList.Add("#0099FF");
        clrList.Add("#FF6699");
        clrList.Add("#CCFF99");
        clrList.Add("#CCCCFF");
        clrList.Add("#99CC66");
        clrList.Add("#99FFCC");
        clrList.Add("#FFCC00");
        clrList.Add("#FFCC33");
        clrList.Add("#99CCCC");
        clrList.Add("#673AB7");
        clrList.Add("#CCFFCC");
        return clrList;
    }
    protected void grdClass_TT_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int col = 1; col < e.Row.Cells.Count; col++)
            {
                string value = e.Row.Cells[col].Text;
                if (class_tt_det_dic.ContainsKey(value))
                {
                    string staffcodeandsubject = Convert.ToString(class_tt_det_dic[value]);
                    if (class_tt_dic.ContainsKey(staffcodeandsubject))
                    {
                        string cellcolor = Convert.ToString(class_tt_dic[staffcodeandsubject]);
                        e.Row.Cells[col].BackColor = ColorTranslator.FromHtml(cellcolor);
                        string[] multiplesubject = staffcodeandsubject.Split(new string[] { ";\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (multiplesubject.Length > 1)
                        {
                            foreach (string subjectcode in multiplesubject)
                            {
                                if (!multiple_dic.ContainsKey(Convert.ToString(subjectcode).Trim()))
                                {
                                    multiple_dic.Add(Convert.ToString(subjectcode).Trim(), cellcolor);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    protected void grdClassDet_TT_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string staffcode = e.Row.Cells[0].Text.Trim();
            string subjectcode = e.Row.Cells[2].Text.Trim();
            if (class_tt_dic.ContainsKey(subjectcode + "$" + staffcode))
            {
                string cellcolor = Convert.ToString(class_tt_dic[subjectcode + "$" + staffcode]);
                e.Row.BackColor = ColorTranslator.FromHtml(cellcolor);
            }
            else
            {
                if (multiple_dic.ContainsKey(subjectcode + "$" + staffcode))
                {
                    string cellcolor = Convert.ToString(multiple_dic[subjectcode + "$" + staffcode]);
                    e.Row.BackColor = ColorTranslator.FromHtml(cellcolor);
                }
            }
        }
    }
}
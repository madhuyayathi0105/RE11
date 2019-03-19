using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Text;

public partial class Staff_Time_Table : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    static string clgcode = string.Empty;
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
            bindcollege();
            collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
            clgcode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
            binddept();
            designation();
            stafftype();
            bindStaff();
            tdStfCode.Visible = true;
            tdStfName.Visible = false;
            tdStfCodeAuto.Visible = true;
            tdStfNameAuto.Visible = false;
            txtFrmDt.Attributes.Add("readonly", "readonly");
            txtToDt.Attributes.Add("readonly", "readonly");
            if (!String.IsNullOrEmpty(strstaffcode) && strstaffcode.Trim() != "0")
            {
                ddlcollege.Enabled = false;
                txt_dept.Enabled = false;
                txtDesig.Enabled = false;
                txtStfType.Enabled = false;
                ddlStfName.Enabled = false;
                ddlSearchOption.Enabled = false;
                txt_scode.Enabled = false;
                txt_sname.Enabled = false;
            }
            loadcolumns(sender, e);
        }
        collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        clgcode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        lblMainErr.Visible = false;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like '" + prefixText + "%' and college_code='" + clgcode + "'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> stfName = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '" + prefixText + "%' and college_code='" + clgcode + "'";
        stfName = ws.Getname(query);
        return stfName;
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

    private void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            ds.Clear();

            string group_user = "";
            string cmd = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + usercode + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";

            }
            ds = d2.select_method_wo_parameter(cmd, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds.Tables[0];
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "dept_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = "Department (" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
                else
                {
                    txt_dept.Text = "--Select--";
                    cb_dept.Checked = false;
                }
            }
        }
        catch { }
    }

    private void designation()
    {
        try
        {
            ds.Clear();
            cblDesig.Items.Clear();
            txtDesig.Text = "--Select--";
            cbDesig.Checked = false;
            string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + collegecode + "' order by desig_name";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblDesig.DataSource = ds;
                cblDesig.DataTextField = "desig_name";
                cblDesig.DataValueField = "desig_code";
                cblDesig.DataBind();
                if (cblDesig.Items.Count > 0)
                {
                    for (int i = 0; i < cblDesig.Items.Count; i++)
                    {
                        cblDesig.Items[i].Selected = true;
                    }
                    txtDesig.Text = "Designation (" + cblDesig.Items.Count + ")";
                    cbDesig.Checked = true;
                }
            }
        }
        catch { }
    }

    private void stafftype()
    {
        try
        {
            ds.Clear();
            cblStfType.Items.Clear();
            txtStfType.Text = "--Select--";
            cbStfType.Checked = false;
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collegecode + "' order by stftype";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblStfType.DataSource = ds;
                cblStfType.DataTextField = "stftype";
                cblStfType.DataBind();
                if (cblStfType.Items.Count > 0)
                {
                    for (int i = 0; i < cblStfType.Items.Count; i++)
                    {
                        cblStfType.Items[i].Selected = true;
                    }
                    txtStfType.Text = "StaffType (" + cblStfType.Items.Count + ")";
                    cbStfType.Checked = true;
                }
            }
        }
        catch { }
    }

    private void bindStaff()
    {
        try
        {
            ds.Clear();
            ddlStfName.Items.Clear();
            string SelQ = "select sm.staff_code,(sm.staff_code+' - '+sm.staff_name) as Staff_Name from staffmaster sm,stafftrans st,staff_appl_master sa where sm.staff_code=st.staff_code and sm.appl_no=sa.appl_no and sm.resign='0' and sm.settled='0' and ISNULL(sm.Discontinue,'0')='0' and st.latestrec='1' and sm.college_code='" + collegecode + "' order by len(sm.staff_code),sm.staff_Code";
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlStfName.DataSource = ds;
                ddlStfName.DataTextField = "Staff_Name";
                ddlStfName.DataValueField = "staff_code";
                ddlStfName.DataBind();
                ddlStfName.Items.Insert(0, "Select");
            }
            else
            {
                ddlStfName.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        clgcode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        binddept();
        designation();
        stafftype();
        bindStaff();
        tdStfCode.Visible = true;
        tdStfName.Visible = false;
        tdStfCodeAuto.Visible = true;
        tdStfNameAuto.Visible = false;
        loadcolumns(sender, e);
    }

    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
    }

    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
    }

    protected void cbDesig_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbDesig, cblDesig, txtDesig, "Designation");
    }

    protected void cblDesig_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbDesig, cblDesig, txtDesig, "Designation");
    }

    protected void cbStfType_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbStfType, cblStfType, txtStfType, "StaffType");
    }

    protected void cblStfType_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbStfType, cblStfType, txtStfType, "StaffType");
    }

    protected void txt_scode_Change(object sender, EventArgs e)
    {
        txt_sname.Text = "";
    }

    protected void txt_sname_Change(object sender, EventArgs e)
    {
        txt_scode.Text = "";
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

    protected void ddlSearchOption_Change(object sender, EventArgs e)
    {
        if (ddlSearchOption.SelectedIndex == 0)
        {
            tdStfCode.Visible = true;
            tdStfName.Visible = false;
            tdStfCodeAuto.Visible = true;
            tdStfNameAuto.Visible = false;
        }
        else
        {
            tdStfCode.Visible = false;
            tdStfName.Visible = true;
            tdStfCodeAuto.Visible = false;
            tdStfNameAuto.Visible = true;
        }
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
            string selcol = "select LinkValue from New_InsSettings where LinkName='TT_Staff_ColOrder' and  user_code='" + usercode + "' and college_code='" + collegecode + "'";
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
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='TT_Staff_ColOrder' and college_code='" + collegecode + "' and user_Code='" + usercode + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='TT_Staff_ColOrder' and user_code='" + usercode + "' and college_code='" + collegecode + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('TT_Staff_ColOrder','" + columnvalue + "','" + usercode + "','" + collegecode + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='TT_Staff_ColOrder' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
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

    protected void btnGo_Click(object sender, EventArgs e)
    {
        class_tt_dic.Clear();
        class_tt_det_dic.Clear();
        loadcolumns(sender, e);
        string SelStaff = "";
        string StaffName = "";
        if (String.IsNullOrEmpty(strstaffcode.Trim()) || strstaffcode.Trim() == "0")
        {
            string DeptCode = GetSelectedItemsValue(cbl_dept);
            string DesigCode = GetSelectedItemsValue(cblDesig);
            string stfType = GetSelectedItemsText(cblStfType);
            if (!String.IsNullOrEmpty(txt_scode.Text.Trim()) && txt_scode.Text.Trim() != "0")
                SelStaff = Convert.ToString(txt_scode.Text.Trim());
            else if (!String.IsNullOrEmpty(txt_sname.Text.Trim()) && txt_sname.Text.Trim() != "0")
                StaffName = Convert.ToString(txt_sname.Text.Trim());
            else
            {
                if (ddlStfName.SelectedItem.Text.Trim() != "Select" && ddlStfName.SelectedIndex != 0)
                    SelStaff = Convert.ToString(ddlStfName.SelectedItem.Value).Trim();
                else
                    SelStaff = "0";
            }

            if ((String.IsNullOrEmpty(SelStaff.Trim()) || SelStaff.Trim() == "0") && (String.IsNullOrEmpty(StaffName) || StaffName == "0") && String.IsNullOrEmpty(DeptCode.Trim()) || DeptCode.Trim() == "0")
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select any Department!";
                grdStf_TT.Visible = false;
                btnComPrint.Visible = false;
                grdStfDet_TT.Visible = false;
                return;
            }
            if ((String.IsNullOrEmpty(SelStaff.Trim()) || SelStaff.Trim() == "0") && (String.IsNullOrEmpty(StaffName) || StaffName == "0") && String.IsNullOrEmpty(DesigCode.Trim()) || DesigCode.Trim() == "0")
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select any Designation!";
                grdStf_TT.Visible = false;
                btnComPrint.Visible = false;
                grdStfDet_TT.Visible = false;
                return;
            }
            if ((String.IsNullOrEmpty(SelStaff.Trim()) || SelStaff.Trim() == "0") && (String.IsNullOrEmpty(StaffName) || StaffName == "0") && String.IsNullOrEmpty(stfType.Trim()) || stfType.Trim() == "0")
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select any Staff Type!";
                grdStf_TT.Visible = false;
                grdStfDet_TT.Visible = false;
                btnComPrint.Visible = false;
                return;
            }
            if ((String.IsNullOrEmpty(SelStaff.Trim()) || SelStaff.Trim() == "0") && (String.IsNullOrEmpty(StaffName) || StaffName == "0"))
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select any Staff!";
                grdStf_TT.Visible = false;
                grdStfDet_TT.Visible = false;
                btnComPrint.Visible = false;
                return;
            }
        }
        else
        {
            SelStaff = strstaffcode.Trim();
        }
        bindStaffTT(SelStaff, StaffName);
    }

    private void bindStaffTT(string StaffCode, string Staff_Name)
    {
        try
        {
            DataSet dsGetSchOrd = new DataSet();
            DataSet dsBind = new DataSet();
            DataView dvBind = new DataView();
            DataTable dtStfTT = new DataTable();
            DataRow drStfTT;
            string SchOrd = "";
            int noofDays = 0;
            string GetSchOrd = "select distinct schOrder,nodays from PeriodAttndSchedule p,BellSchedule b,syllabus_master sy where b.Degree_Code =sy.degree_code and b.batch_year =sy.Batch_Year and b.semester =sy.semester and p.degree_code =b.Degree_Code and p.semester =b.semester";
            dsGetSchOrd.Clear();
            dsGetSchOrd = d2.select_method_wo_parameter(GetSchOrd, "Text");
            if (dsGetSchOrd.Tables.Count > 0 && dsGetSchOrd.Tables[0].Rows.Count > 0)
            {
                SchOrd = Convert.ToString(dsGetSchOrd.Tables[0].Rows[0]["schOrder"]);
                Int32.TryParse(Convert.ToString(dsGetSchOrd.Tables[0].Rows[0]["nodays"]), out noofDays);
                if ((SchOrd.Trim() == "0" || SchOrd.Trim() == "1") && noofDays > 0)
                {
                    string GetPeriod = "select distinct schOrder,nodays,holiday,Period1,Convert(varchar(5),start_time,108) as start_time,Convert(varchar(5),end_time,108) as end_time from PeriodAttndSchedule p,BellSchedule b,syllabus_master sy where b.Degree_Code =sy.degree_code and b.batch_year =sy.Batch_Year and b.semester =sy.semester and p.degree_code =b.Degree_Code and p.semester =b.semester order by start_time,end_time";
                    GetPeriod = GetPeriod + " select TT_Day_DayorderPK,Daydiscription from TT_Day_Dayorder";
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
                            lblMainErr.Visible = false;
                            grdStf_TT.Visible = true;
                            btnComPrint.Visible = true;
                            grdStf_TT.DataSource = dtStfTT;
                            grdStf_TT.DataBind();
                            bindGrdValues(SchOrd, dtStfTT, StaffCode, Staff_Name, noofDays);
                            bindColor();
                        }
                        else
                        {
                            btnComPrint.Visible = false;
                            grdStf_TT.Visible = false;
                            grdStfDet_TT.Visible = false;
                            lblMainErr.Visible = true;
                            lblMainErr.Text = "Day Order Not Available!";
                        }
                    }
                    else
                    {
                        btnComPrint.Visible = false;
                        grdStf_TT.Visible = false;
                        grdStfDet_TT.Visible = false;
                        lblMainErr.Visible = true;
                        lblMainErr.Text = "No Record(s) Found!";
                    }
                }
                else
                {
                    btnComPrint.Visible = false;
                    grdStf_TT.Visible = false;
                    grdStfDet_TT.Visible = false;
                    lblMainErr.Visible = true;
                    if (String.IsNullOrEmpty(SchOrd) || SchOrd.Trim() == "")
                        lblMainErr.Text = "Please Set Schedule Order!";
                    else if (noofDays <= 0)
                        lblMainErr.Text = "Days Not Available!";
                    else
                        lblMainErr.Text = "No Record(s) Found!";
                }
            }
            else
            {
                btnComPrint.Visible = false;
                grdStf_TT.Visible = false;
                grdStfDet_TT.Visible = false;
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
        dicDbCol.Add("DEGREE", "");
        dicDbCol.Add("BATCH", "TT_batchyear");
        dicDbCol.Add("SEMESTER", "TT_sem");
        dicDbCol.Add("SECTION", "TT_sec");
        dicDbCol.Add("ROOM NAME", "Room_Name");
    }

    private void bindGrdValues(string SchOrder, DataTable myDataTable, string Staf_Code, string Staf_Name, int noofDays)
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
                grdStf_TT.Visible = false;
                grdStfDet_TT.Visible = false;
                lblMainErr.Visible = true;
                lblMainErr.Text = "Day Order is InValid!";
                return;
            }
            SelDayOrd = SelDayOrd + " select distinct TT_staffcode,SM.Staff_Name,TT_subno,TT_Hour,TT_Day,TT_Room,s.subject_name,s.subject_code,R.Room_Name,deg.Dept_Code,T.TT_sem,t.TT_sec,T.TT_batchyear from TT_ClassTimeTable T,TT_ClassTimeTabledet TT,Subject S,StaffMaster SM,Room_detail R,Degree deg Where T.TT_ClassPK=TT.TT_ClassFK and S.subject_no=TT_subno and SM.staff_code=TT_staffCode and TT_room=R.RoomPk and deg.Degree_Code=TT_degCode";
            if (!String.IsNullOrEmpty(Staf_Code) && Staf_Code != "0")
                SelDayOrd = SelDayOrd + " and TT_staffcode='" + Staf_Code + "'";
            else if (!String.IsNullOrEmpty(Staf_Name) && Staf_Name != "0")
                SelDayOrd = SelDayOrd + " and staff_name='" + Staf_Name + "'";
            SelDayOrd = SelDayOrd + " order by TT_Day,TT_Hour";
            SelDayOrd = SelDayOrd + " select Dept_Code,Dept_Name from Department";
            SelDayOrd = SelDayOrd + " select distinct TT_staffcode,SM.staff_name,s.subject_code,s.subject_name,deg.Dept_Code,TT_sem,TT_sec,TT_batchyear from TT_ClassTimeTable T,TT_ClassTimeTabledet TT,Subject S,StaffMaster SM,Room_detail R,Degree deg Where T.TT_ClassPK=TT.TT_ClassFK and S.subject_no=TT_subno and SM.staff_code=TT_staffCode and TT_room=R.RoomPk and deg.Degree_Code=TT_degCode";
            if (!String.IsNullOrEmpty(Staf_Code) && Staf_Code != "0")
                SelDayOrd = SelDayOrd + " and TT_staffcode='" + Staf_Code + "'";
            else if (!String.IsNullOrEmpty(Staf_Name) && Staf_Name != "0")
                SelDayOrd = SelDayOrd + " and staff_name='" + Staf_Name + "'";
            SelDayOrd = SelDayOrd + " order by subject_code";
            DataSet dsDayOrd = new DataSet();
            DataView dvDayOrd = new DataView();
            DataView dvVal = new DataView();
            DataView dvDegName = new DataView();
            dsDayOrd.Clear();
            dsDayOrd = d2.select_method_wo_parameter(SelDayOrd, "Text"); int index = 0;
            if (dsDayOrd.Tables.Count > 0 && dsDayOrd.Tables[0].Rows.Count > 0)
            {
                bool IsDayExist = true;
                int headerColumnCount = grdStf_TT.HeaderRow.Cells.Count;
                for (int ro = 1; ro < grdStf_TT.Rows.Count; ro++)
                {
                    dsDayOrd.Tables[0].DefaultView.RowFilter = " Daydiscription='" + Convert.ToString(grdStf_TT.Rows[ro].Cells[0].Text) + "'";
                    dvDayOrd = dsDayOrd.Tables[0].DefaultView;
                    if (dvDayOrd.Count > 0)
                    {
                        string DayFK = Convert.ToString(dvDayOrd[0]["TT_Day_DayorderPK"]);
                        if (!String.IsNullOrEmpty(DayFK.Trim()) && DayFK.Trim() != "0")
                        {
                            for (int co = 1; co < headerColumnCount; co++)
                            {
                                string ColHour = Convert.ToString(grdStf_TT.Rows[0].Cells[co].Text);
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
                                                            if (cblcolumnorder.Items[colOrd].Text.Trim() == "DEGREE")
                                                            {
                                                                if (dsDayOrd.Tables[2].Rows.Count > 0)
                                                                {
                                                                    dsDayOrd.Tables[2].DefaultView.RowFilter = " Dept_Code='" + Convert.ToString(dvVal[ik]["dept_code"]) + "'";
                                                                    dvDegName = dsDayOrd.Tables[2].DefaultView;
                                                                    if (dvDegName.Count > 0)
                                                                    {
                                                                        if (GetVal.Trim() == "")
                                                                            GetVal = Convert.ToString(dvDegName[0]["Dept_Name"]);
                                                                        else
                                                                            GetVal = GetVal + "$" + Convert.ToString(dvDegName[0]["Dept_Name"]);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (GetVal.Trim() == "")
                                                                    GetVal = Convert.ToString(dvVal[ik]["" + dicDbCol[cblcolumnorder.Items[colOrd].Text]]).Trim() + "";
                                                                else
                                                                    GetVal = GetVal + "$" + Convert.ToString(dvVal[ik]["" + dicDbCol[cblcolumnorder.Items[colOrd].Text]]).Trim() + "";
                                                            }
                                                        }
                                                        if (colOrd == 0)
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
                                                        getcolorval = colorvalue;
                                                    else
                                                        getcolorval = getcolorval + ";\n" + colorvalue;
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
                    grdStf_TT.Visible = true;
                    grdStf_TT.DataSource = myDataTable;
                    grdStf_TT.DataBind();
                    bindGrdDet(SchOrder, noofDays, dsDayOrd);
                    lblMainErr.Visible = false;
                }
                else
                {
                    btnComPrint.Visible = false;
                    grdStf_TT.Visible = false;
                    grdStfDet_TT.Visible = false;
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "Day Order is InValid!";
                }
            }
            else
            {
                btnComPrint.Visible = false;
                grdStf_TT.Visible = false;
                grdStfDet_TT.Visible = false;
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

    private void bindGrdDet(string mySchOrd, int myNoofDays, DataSet myDataSet)
    {
        try
        {
            LoadDates();
            DataView dvGetVal = new DataView();
            DataView dvGetDay = new DataView();
            DataView dvFinVal = new DataView();
            DataTable dtStfDet = new DataTable();
            Dictionary<string, string> dicRoom = new Dictionary<string, string>();
            DataRow drStfDet;
            dtStfDet.Columns.Add("Staff Code");
            dtStfDet.Columns.Add("Staff Name");
            dtStfDet.Columns.Add("Subject Code");
            dtStfDet.Columns.Add("Subject Name");

            if (mySchOrd.Trim() == "1")
            {
                dtStfDet.Columns.Add("Mon");
                dtStfDet.Columns.Add("Tue");
                dtStfDet.Columns.Add("Wed");
                dtStfDet.Columns.Add("Thu");
                dtStfDet.Columns.Add("Fri");
                dtStfDet.Columns.Add("Sat");
                dtStfDet.Columns.Add("Sun");
                if (myNoofDays < (dtStfDet.Columns.Count - 4))
                    dtStfDet.Columns.Remove(dtStfDet.Columns[(dtStfDet.Columns.Count - (dtStfDet.Columns.Count - myNoofDays)) + 4]);
            }
            else if (mySchOrd.Trim() == "0")
            {
                dtStfDet.Columns.Add("Day1");
                dtStfDet.Columns.Add("Day2");
                dtStfDet.Columns.Add("Day3");
                dtStfDet.Columns.Add("Day4");
                dtStfDet.Columns.Add("Day5");
                dtStfDet.Columns.Add("Day6");
                dtStfDet.Columns.Add("Day7");
                if (myNoofDays < (dtStfDet.Columns.Count - 4))
                    dtStfDet.Columns.Remove(dtStfDet.Columns[(dtStfDet.Columns.Count - (dtStfDet.Columns.Count - myNoofDays)) + 4]);
            }

            if (myDataSet.Tables.Count > 0 && myDataSet.Tables[0].Rows.Count > 0 && myDataSet.Tables[1].Rows.Count > 0 && myDataSet.Tables[2].Rows.Count > 0 && myDataSet.Tables[3].Rows.Count > 0)
            {
                bool EntryVal = false;
                for (int dsRow = 0; dsRow < myDataSet.Tables[3].Rows.Count; dsRow++)
                {
                    bool myEntryVal = false;
                    string Staf_Code = Convert.ToString(myDataSet.Tables[3].Rows[dsRow]["TT_staffcode"]);
                    string Staf_Name = Convert.ToString(myDataSet.Tables[3].Rows[dsRow]["staff_name"]);
                    string subj_Code = Convert.ToString(myDataSet.Tables[3].Rows[dsRow]["subject_code"]);
                    string subj_Name = Convert.ToString(myDataSet.Tables[3].Rows[dsRow]["subject_name"]);
                    string dept_Code = Convert.ToString(myDataSet.Tables[3].Rows[dsRow]["Dept_Code"]);
                    string Sem = Convert.ToString(myDataSet.Tables[3].Rows[dsRow]["TT_sem"]);
                    string Sec = Convert.ToString(myDataSet.Tables[3].Rows[dsRow]["TT_sec"]);
                    string Batch_Year = Convert.ToString(myDataSet.Tables[3].Rows[dsRow]["TT_batchyear"]);

                    drStfDet = dtStfDet.NewRow();
                    drStfDet[0] = Staf_Code.Trim();
                    drStfDet[1] = Staf_Name.Trim();
                    drStfDet[2] = subj_Code.Trim();
                    drStfDet[3] = subj_Name.Trim();

                    int ColIdx = 4;
                    myDataSet.Tables[1].DefaultView.RowFilter = " TT_staffcode='" + Staf_Code + "' and staff_name='" + Staf_Name + "' and subject_code='" + subj_Code + "' and subject_name='" + subj_Name + "' and Dept_Code='" + dept_Code + "' and TT_sem='" + Sem + "' and TT_sec='" + Sec + "' and TT_batchyear='" + Batch_Year + "'";
                    dvGetVal = myDataSet.Tables[1].DefaultView;
                    if (dvGetVal.Count > 0)
                    {
                        DataTable dtdvGetVal = dvGetVal.ToTable();
                        for (int iCol = ColIdx; iCol < dtStfDet.Columns.Count; iCol++)
                        {
                            dicRoom.Clear();
                            string GetVal = "";
                            string Date = Convert.ToString(dicDays[Convert.ToString(dtStfDet.Columns[iCol].ColumnName)]);
                            myDataSet.Tables[0].DefaultView.RowFilter = " Daydiscription='" + Date + "'";
                            dvGetDay = myDataSet.Tables[0].DefaultView;
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
                                            drStfDet[iCol] = GetVal;
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
                        dtStfDet.Rows.Add(drStfDet);
                    }
                }
                if (EntryVal == true)
                {
                    grdStfDet_TT.Visible = true;
                    grdStfDet_TT.DataSource = dtStfDet;
                    grdStfDet_TT.DataBind();
                }
                else
                {
                    grdStfDet_TT.Visible = false;
                }
            }
            else
            {
                grdStfDet_TT.Visible = false;
            }
        }
        catch { }
    }

    private void bindColor()
    {
        try
        {
            if (grdStf_TT.Rows.Count > 0)
            {
                for (int ro = 0; ro < grdStf_TT.Rows.Count; ro++)
                {
                    if (ro == 0)
                    {
                        grdStf_TT.Rows[ro].Font.Bold = true;
                        grdStf_TT.Rows[ro].Font.Name = "Book Antiqua";
                        grdStf_TT.Rows[ro].Font.Size = FontUnit.Medium;
                        grdStf_TT.Rows[ro].HorizontalAlign = HorizontalAlign.Center;
                        grdStf_TT.Rows[ro].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    }
                    else
                    {
                        grdStf_TT.Rows[ro].Cells[0].Font.Bold = true;
                        grdStf_TT.Rows[ro].Cells[0].Font.Name = "Book Antiqua";
                        grdStf_TT.Rows[ro].Cells[0].Font.Size = FontUnit.Medium;
                        grdStf_TT.Rows[ro].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdStf_TT.Rows[ro].Cells[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    }
                }
            }
        }
        catch { }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + " (" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + " (" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }

    private string GetSelectedItemsText(CheckBoxList cblColumn)
    {
        StringBuilder sbAppend = new StringBuilder();
        try
        {
            for (int j = 0; j < cblColumn.Items.Count; j++)
            {
                if (cblColumn.Items[j].Selected == true)
                {
                    if (sbAppend.Length == 0)
                        sbAppend.Append(Convert.ToString(cblColumn.Items[j].Text));
                    else
                        sbAppend.Append("','" + Convert.ToString(cblColumn.Items[j].Text));
                }
            }
        }
        catch { sbAppend.Clear(); }
        return sbAppend.ToString();
    }

    private string GetSelectedItemsValue(CheckBoxList cblColumn)
    {
        StringBuilder sbAppend = new StringBuilder();
        try
        {
            for (int j = 0; j < cblColumn.Items.Count; j++)
            {
                if (cblColumn.Items[j].Selected == true)
                {
                    if (sbAppend.Length == 0)
                        sbAppend.Append(Convert.ToString(cblColumn.Items[j].Value));
                    else
                        sbAppend.Append("','" + Convert.ToString(cblColumn.Items[j].Value));
                }
            }
        }
        catch { sbAppend.Clear(); }
        return sbAppend.ToString();
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
    protected void grdStf_TT_OnRowDataBound(object sender, GridViewRowEventArgs e)
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
                            if (!multiple_dic.ContainsKey(Convert.ToString(multiplesubject[0]).Trim()))
                            {
                                multiple_dic.Add(Convert.ToString(multiplesubject[0]).Trim(), cellcolor);
                            }
                            if (!multiple_dic.ContainsKey(Convert.ToString(multiplesubject[1]).Trim()))
                            {
                                multiple_dic.Add(Convert.ToString(multiplesubject[1]).Trim(), cellcolor);
                            }
                        }
                    }
                }
            }
        }
    }
    protected void grdStfDet_TT_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string subjectcode = e.Row.Cells[2].Text;
            if (class_tt_dic.ContainsKey(subjectcode.Trim()))
            {
                string cellcolor = Convert.ToString(class_tt_dic[subjectcode]);
                e.Row.BackColor = ColorTranslator.FromHtml(cellcolor);
            }
            else
            {
                if (multiple_dic.ContainsKey(subjectcode))
                {
                    string cellcolor = Convert.ToString(multiple_dic[subjectcode]);
                    e.Row.BackColor = ColorTranslator.FromHtml(cellcolor);
                }
            }
        }
    }
}
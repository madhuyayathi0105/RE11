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

public partial class AttendanceMOD_TT_StaffWorkload : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods Rs = new ReuasableMethods();
    string collegecode = string.Empty;
    static string clgcode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string strstaffcode = "";
    Hashtable hat = new Hashtable();
    Dictionary<string, string> dicDbCol = new Dictionary<string, string>();
    Dictionary<string, string> dicDays = new Dictionary<string, string>();

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
            string DeptCode = Rs.GetSelectedItemsValueAsString(cbl_dept);
            string DesigCode = Rs.GetSelectedItemsValueAsString(cblDesig);
            string staffType = Rs.GetSelectedItemsValueAsString(cblStfType);
            ds.Clear();
            ddlStfName.Items.Clear();
            string SelQ = "select sm.staff_code,(sm.staff_code+' - '+sm.staff_name) as Staff_Name from staffmaster sm,stafftrans st,staff_appl_master sa where sm.staff_code=st.staff_code and sm.appl_no=sa.appl_no and sm.resign='0' and sm.settled='0' and ISNULL(sm.Discontinue,'0')='0' and st.latestrec='1' and sm.college_code='" + collegecode + "' ";
            if (DeptCode.Trim() != "")
            {
                SelQ += " and st.dept_code in ('" + DeptCode + "')";
            }
            if (DesigCode.Trim() != "")
            {
                SelQ += " and st.desig_code in ('" + DesigCode + "')";
            }
            if (staffType.Trim() != "")
            {
                SelQ += " and st.stftype in ('" + staffType + "')";
            }

            SelQ += " order by len(sm.staff_code),sm.staff_Code";

            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlStfName.DataSource = ds;
                ddlStfName.DataTextField = "Staff_Name";
                ddlStfName.DataValueField = "staff_code";
                ddlStfName.DataBind();
                ddlStfName.Items.Insert(0, "All");
            }
            else
            {
                ddlStfName.Items.Insert(0, "All");
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

    }

    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
        bindStaff();
    }

    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
        bindStaff();
    }

    protected void cbDesig_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbDesig, cblDesig, txtDesig, "Designation");
        bindStaff();
    }

    protected void cblDesig_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbDesig, cblDesig, txtDesig, "Designation");
        bindStaff();
    }

    protected void cbStfType_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbStfType, cblStfType, txtStfType, "StaffType");
        bindStaff();
    }

    protected void cblStfType_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbStfType, cblStfType, txtStfType, "StaffType");
        bindStaff();
    }

    protected void txt_scode_Change(object sender, EventArgs e)
    {
        txt_sname.Text = "";
    }

    protected void txt_sname_Change(object sender, EventArgs e)
    {
        txt_scode.Text = "";
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

    protected void btnGo_Click(object sender, EventArgs e)
    {
        bindStaffTT();
    }

    private void bindStaffTT()
    {
        try
        {
            string DeptCode = Rs.GetSelectedItemsValueAsString(cbl_dept);
            string DesigCode = Rs.GetSelectedItemsValueAsString(cblDesig);
            string staffType = Rs.GetSelectedItemsValueAsString(cblStfType);

            DataTable DStaff = new DataTable();
            DataTable DtDay = new DataTable();
            DataView Dv = new DataView();
            DataView dvTheory = new DataView();
            DataView dvLab = new DataView();
            ArrayList AddDAyArray = new ArrayList();
            DataRow Dnew;
            DStaff.Columns.Add("S.No");
            DStaff.Columns.Add("Staff Code");
            DStaff.Columns.Add("Staff Name");
            if (chkDeptDes.Checked == true)
            {
                DStaff.Columns.Add("Department");
                DStaff.Columns.Add("Designation");
            }
            string Selecquery = "select count(distinct TT_Hour) as Total,TT_subno,TT_Day,TT_staffcode,s.staff_Name,h.dept_name,desig.desig_name from TT_ClassTimeTabledet TT,StaffMaster s,Stafftrans T,hrdept_master h,desig_master desig where desig.desig_code=T.desig_code and h.dept_code=T.Dept_code and s.staff_Code=TT_staffcode and T.staff_code=s.staff_code and T.staff_code =TT.TT_staffcode ";
            if (DeptCode.Trim() != "")
            {
                Selecquery += " and t.dept_code in ('" + DeptCode + "')";
            }
            if (DesigCode.Trim() != "")
            {
                Selecquery += " and t.desig_code in ('" + DesigCode + "')";
            }
            if (staffType.Trim() != "")
            {
                Selecquery += " and t.stftype in ('" + staffType + "')";
            }
            if (ddlStfName.SelectedItem.Text != "All")
            {
                Selecquery += " and TT_staffcode='" + ddlStfName.SelectedItem.Value + "'";
            }
            if (txt_scode.Text != "")
            {
                Selecquery += " and TT_staffcode='" + txt_scode.Text + "'";
            }
            Selecquery += " group by TT_Day,TT_staffcode,s.staff_Name,h.dept_name,desig.desig_name,TT_subno order by TT_staffcode,TT_Day";
            Selecquery += " select TT_Day_DayorderPK,DayDiscription,DayType from TT_Day_Dayorder where DayType='0' order by TT_Day_DayorderPK";
            Selecquery += " select count(TT_Hour) as Total,TT_subno,TT_staffcode,s.staff_Name from TT_ClassTimeTabledet TT,StaffMaster s,Stafftrans T,Subject Su,Sub_Sem SS where Su.subType_no=ss.subType_no and  Su.subject_no=TT.TT_subno and s.staff_Code=TT_staffcode and T.staff_code=s.staff_code and T.staff_code =TT.TT_staffcode and ss.lab='0' and ss.ElectivePap='0' group by TT_staffcode,s.staff_Name,TT_subno order by TT_staffcode";
            Selecquery += " select count(TT_Hour) as Total,TT_subno,TT_staffcode,s.staff_Name from TT_ClassTimeTabledet TT,StaffMaster s,Stafftrans T,Subject Su,Sub_Sem SS where Su.subType_no=ss.subType_no and  Su.subject_no=TT.TT_subno and s.staff_Code=TT_staffcode and T.staff_code=s.staff_code and T.staff_code =TT.TT_staffcode and ss.lab='1' group by TT_staffcode,s.staff_Name,TT_subno order by TT_staffcode";
            Selecquery += " select count(distinct TT_Hour) as Total,TT_subno,TT_staffcode,s.staff_Name from TT_ClassTimeTabledet TT,StaffMaster s,Stafftrans T,Subject Su,Sub_Sem SS where Su.subType_no=ss.subType_no and  Su.subject_no=TT.TT_subno and s.staff_Code=TT_staffcode and T.staff_code=s.staff_code and T.staff_code =TT.TT_staffcode and ss.lab='0' and ss.ElectivePap='1' group by TT_staffcode,s.staff_Name,TT_subno order by TT_staffcode";

            Selecquery += " select TT_subNo,TT_sub_Groupid,Pair,Noofhrsperweek from TT_Combined_subject T,Subject s where T.TT_subno=s.subject_no ";

            Selecquery += " select distinct TT_subno,TT_Staffcode,lab,TT_ClassFK,TT_Sec,ElectivePap from TT_ClassTimeTable TT, TT_ClassTimeTabledet T,Subject S,Sub_sem SS where T.TT_subno=s.subject_no and s.subType_no=ss.subType_no and TT.TT_ClassPK=T.TT_ClassFK"; //and ss.lab='0'

            ds.Clear();
            ds = d2.select_method_wo_parameter(Selecquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable DvNewTheroy = ds.Tables[5].DefaultView.ToTable();
                DtDay = ds.Tables[0].DefaultView.ToTable(true, "TT_Day");
                if (DtDay.Rows.Count > 0)
                {
                    for (int Row = 0; Row < DtDay.Rows.Count; Row++)
                    {
                        string DayValue = string.Empty;
                        ds.Tables[1].DefaultView.RowFilter = "TT_Day_DayorderPK='" + Convert.ToString(DtDay.Rows[Row]["TT_Day"]) + "'";
                        Dv = ds.Tables[1].DefaultView;
                        if (Dv.Count > 0)
                        {
                            DayValue = Convert.ToString(Dv[0]["DayDiscription"]);
                            if (chkIncDay.Checked == true)
                                DStaff.Columns.Add(DayValue);
                            if (!AddDAyArray.Contains(Convert.ToString(DtDay.Rows[Row]["TT_Day"])))
                            {
                                AddDAyArray.Add(Convert.ToString(DtDay.Rows[Row]["TT_Day"]));
                            }
                        }
                    }
                    DStaff.Columns.Add("Theory");
                    DStaff.Columns.Add("Lab");
                    DStaff.Columns.Add("Total");
                }
                DtDay = ds.Tables[0].DefaultView.ToTable(true, "TT_staffcode", "staff_Name", "dept_name", "desig_name");
                if (DtDay.Rows.Count > 0)
                {
                    DataView dv = DtDay.DefaultView;
                    dv.Sort = "staff_Name asc";
                    DtDay = dv.ToTable();
                    for (int Staf = 0; Staf < DtDay.Rows.Count; Staf++)
                    {
                        string StfCode = Convert.ToString(DtDay.Rows[Staf]["TT_staffcode"]);
                        string StName = Convert.ToString(DtDay.Rows[Staf]["staff_Name"]);
                        string Dept_Name = Convert.ToString(DtDay.Rows[Staf]["dept_name"]);
                        string Desig_Name = Convert.ToString(DtDay.Rows[Staf]["desig_name"]);
                        Dnew = DStaff.NewRow();
                        Dnew[0] = Convert.ToInt32(Staf + 1);
                        Dnew[1] = Convert.ToString(StfCode);
                        Dnew[2] = Convert.ToString(StName);
                        int Index = 2;
                        if (chkDeptDes.Checked == true)
                        {
                            Dnew[3] = Convert.ToString(Dept_Name);
                            Dnew[4] = Convert.ToString(Desig_Name);
                            Index = 4;
                        }
                        int TotalCount = 0;
                        DataTable AddData = new DataTable();
                        if (AddDAyArray.Count > 0)
                        {
                            for (int Dy = 0; Dy < AddDAyArray.Count; Dy++)
                            {
                                if (chkIncDay.Checked == true)
                                    Index++;
                                ds.Tables[0].DefaultView.RowFilter = "TT_Day='" + Convert.ToString(AddDAyArray[Dy]) + "' and TT_staffcode='" + StfCode + "'";
                                Dv = ds.Tables[0].DefaultView;
                                AddData = Dv.ToTable();
                                if (Dv.Count > 0)
                                {
                                    if (chkIncDay.Checked == true)
                                        Dnew[Index] = Convert.ToInt32(AddData.Compute("Sum(Total)", ""));
                                    TotalCount += Convert.ToInt32(AddData.Compute("Sum(Total)", ""));
                                }
                            }
                            ArrayList AddString = new ArrayList();
                            ArrayList AddStringNEw = new ArrayList();
                            ArrayList AddElective = new ArrayList();
                            ArrayList AddClass = new ArrayList();
                            ds.Tables[6].DefaultView.RowFilter = "TT_StaffCode='" + StfCode + "'";
                            dvTheory = ds.Tables[6].DefaultView;
                            if (dvTheory.Count > 0)
                            {
                                for (int i = 0; i < dvTheory.Count; i++)
                                {
                                    AddString.Add(Convert.ToString(dvTheory[i]["TT_subno"]) + "*" + Convert.ToString(dvTheory[i]["lab"]) + "*" + Convert.ToString(dvTheory[i]["TT_sec"]) + "*" + Convert.ToString(dvTheory[i]["ElectivePap"]));
                                }
                            }
                            int SubCountAddValue = 0;
                            int LabSubCountAddValue = 0;
                            if (AddString.Count > 0)
                            {
                                for (int AddStr = 0; AddStr < AddString.Count; AddStr++)
                                {
                                    string SubjectNo = Convert.ToString(AddString[AddStr]).Split('*')[0];
                                    string LAb = Convert.ToString(AddString[AddStr]).Split('*')[1];
                                    string TT_Sec = Convert.ToString(AddString[AddStr]).Split('*')[2];
                                    string ElectivePap = Convert.ToString(AddString[AddStr]).Split('*')[3];
                                    if (ElectivePap.Trim() != "1" && ElectivePap.Trim() != "True")
                                    {
                                        if (!AddStringNEw.Contains(SubjectNo + "*" + TT_Sec)) //|| !AddClass.Contains(TTClassPK)
                                        {
                                            ds.Tables[5].DefaultView.RowFilter = "TT_Subno in (" + SubjectNo + ")";
                                            dvTheory = ds.Tables[5].DefaultView;
                                            if (dvTheory.Count > 0)
                                            {
                                                string Pair = Convert.ToString(dvTheory[0]["Pair"]);
                                                string TT_SubGroupID = Convert.ToString(dvTheory[0]["TT_sub_Groupid"]);
                                                int NoofHrsPerWeek = Convert.ToInt32(dvTheory[0]["Noofhrsperweek"]);

                                                DvNewTheroy.DefaultView.RowFilter = "Pair='" + Pair + "' and TT_sub_Groupid='" + TT_SubGroupID + "'";
                                                dvTheory = DvNewTheroy.DefaultView;
                                                if (dvTheory.Count > 0)
                                                {
                                                    if (LAb.Trim() == "False")
                                                    {
                                                        SubCountAddValue += NoofHrsPerWeek;
                                                    }
                                                    else if (LAb.Trim() == "True")
                                                    {
                                                        LabSubCountAddValue += NoofHrsPerWeek;
                                                    }
                                                    for (int AddSt = 0; AddSt < dvTheory.Count; AddSt++)
                                                    {
                                                        if (!AddStringNEw.Contains(dvTheory[AddSt]["TT_subNo"].ToString() + "*" + TT_Sec))
                                                        {
                                                            AddStringNEw.Add(dvTheory[AddSt]["TT_subNo"].ToString() + "*" + TT_Sec);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!AddElective.Contains(SubjectNo)) //|| !AddClass.Contains(TTClassPK)
                                        {
                                            ds.Tables[5].DefaultView.RowFilter = "TT_Subno in (" + SubjectNo + ")";
                                            dvTheory = ds.Tables[5].DefaultView;
                                            if (dvTheory.Count > 0)
                                            {
                                                string Pair = Convert.ToString(dvTheory[0]["Pair"]);
                                                string TT_SubGroupID = Convert.ToString(dvTheory[0]["TT_sub_Groupid"]);
                                                int NoofHrsPerWeek = Convert.ToInt32(dvTheory[0]["Noofhrsperweek"]);

                                                DvNewTheroy.DefaultView.RowFilter = "Pair='" + Pair + "' and TT_sub_Groupid='" + TT_SubGroupID + "'";
                                                dvTheory = DvNewTheroy.DefaultView;
                                                if (dvTheory.Count > 0)
                                                {
                                                    if (LAb.Trim() == "False")
                                                    {
                                                        SubCountAddValue += NoofHrsPerWeek;
                                                    }
                                                    else if (LAb.Trim() == "True")
                                                    {
                                                        LabSubCountAddValue += NoofHrsPerWeek;
                                                    }
                                                    for (int AddSt = 0; AddSt < dvTheory.Count; AddSt++)
                                                    {
                                                        if (!AddElective.Contains(dvTheory[AddSt]["TT_subNo"].ToString()))
                                                        {
                                                            AddElective.Add(dvTheory[AddSt]["TT_subNo"].ToString());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }

                            int Total = 0;
                            ds.Tables[2].DefaultView.RowFilter = "TT_StaffCode='" + StfCode + "'";
                            dvTheory = ds.Tables[2].DefaultView;
                            AddData = dvTheory.ToTable();

                            if (dvTheory.Count > 0)
                                Total += Convert.ToInt32(AddData.Compute("Sum(Total)", ""));
                            else
                                Total += 0;

                            ds.Tables[4].DefaultView.RowFilter = "TT_StaffCode='" + StfCode + "'";
                            dvTheory = ds.Tables[4].DefaultView;
                            AddData = dvTheory.ToTable();

                            if (dvTheory.Count > 0)
                                Total += Convert.ToInt32(AddData.Compute("Sum(Total)", ""));
                            else
                                Total += 0;

                            if (Total != 0 && SubCountAddValue != 0)
                            {
                                Total -= SubCountAddValue;
                            }

                            Dnew[Index + 1] = Total;

                            ds.Tables[3].DefaultView.RowFilter = "TT_StaffCode='" + StfCode + "'";
                            dvLab = ds.Tables[3].DefaultView;
                            AddData = dvLab.ToTable();
                            if (dvLab.Count > 0)
                            {
                                int Labcount = Convert.ToInt32(AddData.Compute("Sum(Total)", "")) - LabSubCountAddValue;
                                Dnew[Index + 2] = Labcount;
                            }
                            else
                                Dnew[Index + 2] = "";

                            if (TotalCount != 0)
                            {
                                TotalCount -= (SubCountAddValue + LabSubCountAddValue);
                            }
                            Dnew[Index + 3] = TotalCount;
                        }
                        DStaff.Rows.Add(Dnew);
                    }
                }
                //  DStaff.DefaultView.Sort = "len(Staff Code) asc";

                if (DStaff.Rows.Count > 0)
                {
                    grdStf_TT.DataSource = DStaff;
                    grdStf_TT.DataBind();
                    grdStf_TT.Visible = true;
                    btnExport.Visible = true;
                    pnlContents.Visible = true;
                    printCollegeDet();
                    columnCount();
                }
                else
                {
                    grdStf_TT.Visible = false;
                    btnExport.Visible = false;
                    pnlContents.Visible = false;
                    Response.Write("<script>alert('No Record Found')</script>");
                }
            }
        }
        catch { }
    }

    protected void OnrowDataBoun(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int Count = e.Row.Cells.Count;
            if (Count > 0)
            {
                if (chkDeptDes.Checked == true)
                {
                    for (int r = 0; r < Count; r++)
                    {
                        if (r != 2 && r != 3 && r != 4)
                        {
                            e.Row.Cells[r].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                else
                {
                    for (int r = 0; r < Count; r++)
                    {
                        if (r != 2)
                        {
                            e.Row.Cells[r].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
        }
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
                grdStf_TT.Visible = false;

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
            SelDayOrd = SelDayOrd + " select distinct TT_staffcode,SM.staff_name,s.subject_code,s.subject_name,R.Room_Name,deg.Dept_Code,TT_sem,TT_sec,TT_batchyear from TT_ClassTimeTable T,TT_ClassTimeTabledet TT,Subject S,StaffMaster SM,Room_detail R,Degree deg Where T.TT_ClassPK=TT.TT_ClassFK and S.subject_no=TT_subno and SM.staff_code=TT_staffCode and TT_room=R.RoomPk and deg.Degree_Code=TT_degCode";
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
            dsDayOrd = d2.select_method_wo_parameter(SelDayOrd, "Text");
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
                                        string myGetVal = "";
                                        int myHour = 0;
                                        Int32.TryParse(ColHour, out myHour);
                                        if (myHour > 0)
                                        {
                                            dsDayOrd.Tables[1].DefaultView.RowFilter = " TT_Day='" + DayFK.Trim() + "' and TT_Hour='" + myHour + "'";
                                            dvVal = dsDayOrd.Tables[1].DefaultView;
                                            if (dvVal.Count > 0)
                                            {

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
                    grdStf_TT.Visible = true;
                    grdStf_TT.DataSource = myDataTable;
                    grdStf_TT.DataBind();
                    lblMainErr.Visible = false;
                }
                else
                {
                    grdStf_TT.Visible = false;

                    lblMainErr.Visible = true;
                    lblMainErr.Text = "Day Order is InValid!";
                }
            }
            else
            {
                grdStf_TT.Visible = false;

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

    protected void columnCount()
    {
        try
        {
            int Cnt = grdStf_TT.Rows[1].Cells.Count;
            if (Cnt > 10)
                btnExport.Text = "Print A3 Format";
            else
                btnExport.Text = "Print A4 Format";
        }
        catch { }
    }

    protected void printCollegeDet()
    {
        try
        {
            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + ddlcollege.SelectedItem.Value + " ";

            string collegename = "";
            string add1 = "";
            string add2 = "";
            string add3 = "";
            string univ = "";
            string feedet = "";
            ds = d2.select_method_wo_parameter(colquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                add1 += " " + add2;
                spCollege.InnerText = collegename;
                spAffBy.InnerText = add1;
                spController.InnerText = add3;
                spSeating.InnerText = univ;
                // spDateSession.InnerText = "PRE-PRIMARY COMPARTMENT";
                sprptnamedt.InnerText = "Time Table Staff Work Load";
                spdate.InnerText = DateTime.Now.ToString("dd.MM.yyyy");
                //spdate.InnerText = "STUDENTS ATTENDANCE CONSOLIDATION--" + academicyear + "";
            }
        }
        catch { }
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
}
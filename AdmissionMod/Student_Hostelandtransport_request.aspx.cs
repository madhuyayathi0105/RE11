using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
public partial class AdmissionMod_Student_Hostelandtransport_request : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string UserCode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        UserCode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            bindCollege();
            bindBatch();
            bindEdulevel();
            bindCourse();
            settings();
            //bindHostel();
            bindroomtype();
            Bindstage();
            Bindroute();
            txt_date.Attributes.Add("readonly", "readonly");
            txt_date.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            ddl_filledtype.SelectedIndex = ddl_filledtype.Items.IndexOf(ddl_filledtype.Items.FindByValue("2"));
        }
    }
    protected void settings()
    {
        DataSet settings_ds = new DataSet();
        string settings = " SELECT LinkValue FROM New_InsSettings WHERE LinkName='RoomDetailIncludebatch' AND college_code='" + ddlCollege.SelectedValue + "'";
        //and user_code='" + UserCode + "'
        settings += " select LinkValue from New_InsSettings where LinkName='ONLY HOSTELFEE OR TRANSPORTFEE' and college_code='" + ddlCollege.SelectedValue + "'";
        //and user_code='" + UserCode + "'
        settings_ds = d2.select_method_wo_parameter(settings, "text");
        if (settings_ds.Tables.Count > 0)
        {
            if (settings_ds.Tables[0].Rows.Count > 0)
                ViewState["includebatch"] = Convert.ToString(settings_ds.Tables[0].Rows[0]["LinkValue"]);
            if (settings_ds.Tables[1].Rows.Count > 0)
                ViewState["hostelandtransportsettings"] = Convert.ToString(settings_ds.Tables[1].Rows[0]["LinkValue"]);
        }
    }
    public void bindCollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollegebaseonrights(UserCode, 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch
        {
        }
    }
    public void bindBatch()
    {
        try
        {
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch
        {
        }
    }
    public void bindEdulevel()
    {
        try
        {
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct Edu_level from Course where college_code=" + ddlCollege.SelectedValue + " order by Edu_level desc", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlEduLev.DataSource = ds;
                ddlEduLev.DataTextField = "Edu_level";
                ddlEduLev.DataValueField = "Edu_level";
                ddlEduLev.DataBind();
            }
        }
        catch
        {
        }
    }
    public void bindCourse()
    {
        try
        {
            if (ddlEduLev.Items.Count > 0)
            {
                ds.Clear();
                ds = d2.select_method_wo_parameter("select distinct course_id,Course_Name from Course where Edu_Level='" + ddlEduLev.SelectedItem.Value + "' and college_code=" + ddlCollege.SelectedValue + " order by course_id", "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcourse.DataSource = ds;
                    ddlcourse.DataTextField = "Course_Name";
                    ddlcourse.DataValueField = "course_id";
                    ddlcourse.DataBind();
                }
            }
        }
        catch
        {
        }
    }
    public void bindHostel(int gendertype)
    {
        ds.Clear();
        ddl_hostel.Items.Clear();
        string roomquery = "select HostelName,CONVERT(varchar,HostelMasterPK)+'$'+CONVERT(varchar, HostelBuildingFK)as hostelandbuildingcode from HM_HostelMaster where HostelType='" + gendertype + "'";
        ds = d2.select_method_wo_parameter(roomquery, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_hostel.DataSource = ds.Tables[0];
            ddl_hostel.DataTextField = "HostelName";
            ddl_hostel.DataValueField = "hostelandbuildingcode";
            ddl_hostel.DataBind();
        }
        //ddl_hostel.Items.Insert(0, "Select");
    }
    public void bindroomtype()
    {
        ds.Clear();
        ddl_roomtype.Items.Clear();
        string roomquery = "select distinct Room_type from Room_Detail where College_Code='" + Convert.ToString(ddlCollege.SelectedItem.Value) + "' order by Room_type";
        ds = d2.select_method_wo_parameter(roomquery, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_roomtype.DataSource = ds.Tables[0];
            ddl_roomtype.DataTextField = "Room_type";
            ddl_roomtype.DataValueField = "Room_type";
            ddl_roomtype.DataBind();
        }
        ddl_roomtype.Items.Insert(0, "Select");
    }
    public void bindbuilding()
    {
        ddl_buildingname.Items.Clear();
        if (ddl_roomtype.Items.Count > 0)
        {
            if (ddl_roomtype.SelectedIndex != 0)
            {
                ds.Clear();
                if (ddl_hostel.Items.Count > 0)//if (ddl_hostel.SelectedValue != "Select")
                {
                    string roomquery = "select distinct code,bm.Building_Name from Building_Master bm,Room_Detail rd where bm.Building_Name=rd.Building_Name and rd.Room_type='" + Convert.ToString(ddl_roomtype.SelectedItem.Value) + "' and code in(" + ddl_hostel.SelectedValue.Split('$')[1] + ")";
                    ds = d2.select_method_wo_parameter(roomquery, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_buildingname.DataSource = ds.Tables[0];
                        ddl_buildingname.DataTextField = "Building_Name";
                        ddl_buildingname.DataValueField = "code";
                        ddl_buildingname.DataBind();
                    }
                    bindroomname();
                }
            }
        }
    }
    private void bindroomname()
    {
        roomno_datalist.Visible = false;
        if (ddl_roomtype.Items.Count > 0 && ddl_buildingname.Items.Count > 0)
        {
            if (ddl_roomtype.SelectedIndex != 0)
            {
                string filtertype = "";
                if (ddl_filledtype.SelectedIndex == 1)
                    filtertype = " and isnull(students_allowed,0)=ISNULL(Avl_Student,0)";
                else if (ddl_filledtype.SelectedIndex == 2)
                    filtertype = " and isnull(students_allowed,0)<>ISNULL(Avl_Student,0)";
                string includebatch = "";
                if (Convert.ToString(ViewState["includebatch"]) == "1")
                    includebatch = " and batchYear='" + Convert.ToString(ddlbatch.SelectedItem.Value) + "'";

                string roomquery = "select Room_Name+'('+convert(varchar,isnull(students_allowed,0)) +'-'+ convert(varchar,isnull(Avl_Student,0))+')' as Room_Name,roompk,case when isnull(students_allowed,0)=ISNULL(Avl_Student,0) then '1' when isnull(students_allowed,0)<>ISNULL(Avl_Student,0) then '0' end checkval from room_detail where Building_Name='" + Convert.ToString(ddl_buildingname.SelectedItem.Text) + "' and Room_type='" + Convert.ToString(ddl_roomtype.SelectedItem.Value) + "' " + includebatch + "" + filtertype + " order by LEN(room_name) asc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(roomquery, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    roomno_datalist.DataSource = ds.Tables[0];
                    roomno_datalist.DataBind();
                    roomno_datalist.Visible = true;
                }
                else
                {
                    roomno_datalist.DataSource = ds.Tables[0];
                    roomno_datalist.DataBind();
                    roomno_datalist.Visible = true;
                }
            }
        }
        else
        {
            roomno_datalist.Visible = false;
        }
    }
    private void Bindstage()
    {
        ds.Clear(); ddl_stage.Items.Clear();
        if (ddl_stage.SelectedValue != "Select")
        {
            string roomquery = "select Stage_id,Stage_Name from Stage_Master order by Stage_Name ";
            ds = d2.select_method_wo_parameter(roomquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_stage.DataSource = ds.Tables[0];
                ddl_stage.DataTextField = "Stage_Name";
                ddl_stage.DataValueField = "Stage_id";
                ddl_stage.DataBind();
            }
        }
    }
    private void Bindroute()
    {
        ds.Clear(); ddl_route.Items.Clear();
        if (ddl_stage.Items.Count > 0)
        {
            string roomquery = "select distinct Route_ID,Veh_ID from RouteMaster where Stage_Name='" + ddl_stage.SelectedItem.Value + "' ";
            ds = d2.select_method_wo_parameter(roomquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_route.DataSource = ds.Tables[0];
                ddl_route.DataTextField = "Route_ID";
                ddl_route.DataValueField = "Veh_ID";
                ddl_route.DataBind();
                if (ddl_route.Items.Count > 0)
                {
                    string avilable = d2.GetFunction(" select 'Alloted '+convert(varchar(50),(COUNT(distinct r.App_No)))+'-Available '+convert(varchar, v.firstyearstudent)studentcount from vehicle_master v left join Registration r on v.Veh_ID=r.VehID where v.Veh_ID='" + ddl_route.SelectedItem.Value + "' and route='" + ddl_route.SelectedItem.Text + "' group by v.Veh_ID,v.firstyearstudent ");
                    string[] avilableVal = avilable.Split('-');
                    if (avilableVal.Length == 2)
                    {
                        string resVal = "<span style='color:red; font-weight:bold;'>" + avilableVal[0] + "</span>-<span style='color:green; font-weight:bold;'>" + avilableVal[1] + "</span>";
                        availablevehicalseat.InnerHtml = resVal;
                    }
                    else
                    {
                        availablevehicalseat.InnerHtml = string.Empty;
                    }
                }
            }
        }
    }
    protected void ddl_roomtype_selectedindexchanged(object sendder, EventArgs e)
    {
        bindbuilding();
        bindroomname();
    }
    protected void ddlEduLev_selectedindexchanged(object sender, EventArgs e)
    {
        bindCourse();
    }
    protected void ddl_building_selectedindexchanged(object sender, EventArgs e)
    {
        bindroomname();
    }
    protected void ddl_filledtype_selectedindexchanged(object sender, EventArgs e)
    {
        bindroomname();
    }
    protected void ddl_hostel_selectedindexchanged(object sender, EventArgs e)
    {
        bindbuilding();
        bindroomtype();
        bindroomname();
    }
    protected void ddl_stage_selectedindexchanged(object sender, EventArgs e)
    {
        Bindroute();
    }
    protected void ddl_route_selectedindexchanged(object sender, EventArgs e)
    {
        bindavailableseat();
    }
    protected void bindavailableseat()
    {
        if (ddl_route.Items.Count > 0)
        {
            string avilable = d2.GetFunction(" select 'Alloted '+convert(varchar(50),(COUNT(distinct r.App_No)))+'-Available '+convert(varchar, v.firstyearstudent)studentcount from vehicle_master v left join Registration r on v.Veh_ID=r.VehID where v.Veh_ID='" + ddl_route.SelectedItem.Value + "' and route='" + ddl_route.SelectedItem.Text + "' group by v.Veh_ID,v.firstyearstudent ");
            string[] avilableVal = avilable.Split('-');
            if (avilableVal.Length == 2)
            {
                string resVal = "<span style='color:red; font-weight:bold;'>" + avilableVal[0] + "</span>-<span style='color:green; font-weight:bold;'>" + avilableVal[1] + "</span>";
                availablevehicalseat.InnerHtml = resVal;
            }
            else
            {
                availablevehicalseat.InnerHtml = string.Empty;
            }
        }
    }
    protected void roomno_datalist_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        string value = Convert.ToString((e.Item.FindControl("lbl_checkcolor") as Label).Text);
        if (value == "1")
        {
            ((System.Web.UI.WebControls.CheckBox)(e.Item.FindControl("cb_room_dv"))).Enabled = false;
            ((System.Web.UI.WebControls.Label)(e.Item.FindControl("lbl_roomname"))).ForeColor = Color.Red;
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText, string contextKey)
    {
        string Flitervalues = contextKey;
        string[] Flitervalue = Flitervalues.Split('$');
        string collegecode = Convert.ToString(Flitervalue[0]);
        string batchyear = Convert.ToString(Flitervalue[1]);
        string edulevel = Convert.ToString(Flitervalue[2]);
        string courseid = Convert.ToString(Flitervalue[3]);
        //string tapselect = Convert.ToString(Flitervalue[4]);
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (prefixText.Trim() != "")
        {
            string query = "  select app_formno from applyn a,Registration r, course c,Degree d,Department dt where a.app_no=r.App_No and a.college_code=d.college_code and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and a.degree_code=d.Degree_Code and r.App_No not in(select app_no from HT_HostelRegistration) and  a.college_code='" + collegecode + "' and r.batch_year='" + batchyear + "' and c.Edu_Level='" + edulevel + "' and c.Course_Id='" + courseid + "' and IsConfirm='1' and ISNULL(selection_status,0)=1 and ISNULL(admission_status,0)=1 and app_formno like '" + prefixText + "%' ";
            name = ws.Getname(query);
        }
        return name;
    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        alert_pop.Visible = false;
        txt_applicationno.Focus();
    }
    public void rdbtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtype.SelectedItem.Value == "0")
        {
            transport_div.Visible = false;
            //if (Convert.ToString(ViewState["hostelandtransportsettings"]).Split(',')[0] == "1") 
            //room_div.Visible = false;
            //else 
            room_div.Visible = true;
            btn_register.Visible = true;
            verification_div.Visible = true;
        }
        if (rdbtype.SelectedItem.Value == "1")
        {
            transport_div.Visible = true;
            room_div.Visible = false;
            btn_register.Visible = true;
            verification_div.Visible = true;
        }
    }
    protected void btn_submit_click(object sender, EventArgs e)
    {
        try
        {
            verification_div.Visible = false;
            if (txt_applicationno.Text.Trim() != "")
            {
                string studpersonaldet = " select a.app_no,app_formno,a.stud_name,CONVERT(varchar(10),dob,103)dob,case when sex='0' then 'Male' when sex='1' then 'Female' when sex='2' then 'Transgender' end sex,Student_Mobile,parent_name,case when sex=0 then '1' when sex=1 then '2' when sex=2 then 'Transgender' end as gender,isnull(r.Stud_Type,'') as Stud_Type  from applyn a,Registration r  where a.app_no=r.app_no and app_formno='" + txt_applicationno.Text + "'";
                ds = d2.select_method_wo_parameter(studpersonaldet, "text");
                #region student details
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string StudType = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Type"]);
                    if (StudType.Trim() == "")
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            app_no_span.InnerHtml = Convert.ToString(dr["app_no"]);
                            applicantno_span.InnerHtml = ": " + Convert.ToString(dr["app_formno"]);
                            applicantname_span.InnerHtml = ": " + Convert.ToString(dr["stud_name"]);
                            dob_span.InnerHtml = ": " + Convert.ToString(dr["dob"]);
                            gender_span.InnerHtml = ": " + Convert.ToString(dr["sex"]);
                            studmobileno_span.InnerHtml = ": " + Convert.ToString(dr["Student_Mobile"]);
                            fathername_span.InnerHtml = ": " + Convert.ToString(dr["parent_name"]);
                            verification_div.Visible = true;
                        }
                        int gender = 0;
                        int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["gender"]), out gender);
                        bindHostel(gender);
                        rdbtype_SelectedIndexChanged(sender, e);
                        bindavailableseat();
                    }
                    else
                    {
                        lbl_alert.Text = "Already Student Register Hostel or Transport Allotment";
                        alert_pop.Visible = true;
                    }
                }
                else
                {
                    lbl_alert.Text = "Please Check Application Number";
                    alert_pop.Visible = true;
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "Student_Hostelandtransport_request");
        }
    }
    protected void clear()
    {
        applicantno_span.InnerHtml = ": ";
        applicantname_span.InnerHtml = ": ";
        dob_span.InnerHtml = ": ";
        gender_span.InnerHtml = ": ";
        studmobileno_span.InnerHtml = ": ";
        fathername_span.InnerHtml = ": ";
        app_no_span.InnerHtml = "";
    }
    protected void btn_register_click(object sender, EventArgs e)
    {
        try
        {
            bool insert = false;
            string checkdatalistselect = "";
            if (txt_applicationno.Text.Trim() != "")
            {
                if (rdbtype.SelectedItem.Value == "0")
                {
                    if (Convert.ToString(ViewState["hostelandtransportsettings"]).Split(',')[0] == "1")
                    {
                        string Sex = d2.GetFunction("select sex from applyn where app_no ='" + app_no_span.InnerHtml + "'");
                        string Total = string.Empty;
                        int Tot = 0;
                        int AllotTot = 0;
                        if (Sex == "0")
                        {
                            Total = d2.GetFunction("select COUNT(r.app_no) as Total from Registration r,applyn a  where r.Stud_Type  ='Hostler' and a.app_no =r.App_No and a.sex ='0' ");
                            Tot = 1300;
                        }
                        else
                        {
                            Total = d2.GetFunction("select COUNT(r.app_no) as Total from Registration r,applyn a  where r.Stud_Type  ='Hostler' and a.app_no =r.App_No and a.sex ='1' ");
                            Tot = 650;
                        }
                        int.TryParse(Convert.ToString(Total), out AllotTot);
                        if (AllotTot < Tot)
                        {
                            #region Only Hostelfee allot
                            ds.Clear();
                            string settingquery = " select LinkValue from New_InsSettings where LinkName='Hostel_Admission_Form_Fee' and LEFT(LinkValue,charindex('$',linkvalue)-1)='1' AND college_code ='" + Convert.ToString(ddlCollege.SelectedItem.Value) + "' and user_code='" + Session["usercode"].ToString() + "' and isnull(LinkValue,'')<>''";
                            settingquery += " select LinkValue from New_InsSettings where LinkName='Fee Yearwise' AND college_code ='" + Convert.ToString(ddlCollege.SelectedItem.Value) + "' and user_code='" + Session["usercode"].ToString() + "' and isnull(LinkValue,'')<>''";
                            ds = d2.select_method_wo_parameter(settingquery, "text");
                            string linkvalue = "0";
                            if (ds.Tables[1].Rows.Count > 0 && ds.Tables.Count > 0)
                            {
                                if (ds.Tables[1].Rows.Count > 0)
                                    linkvalue = Convert.ToString(ds.Tables[1].Rows[0]["LinkValue"]);
                                string type = "";
                                if (linkvalue.Trim() == "1")
                                    type = "1 Year";
                                if (linkvalue.Trim() == "0")
                                    type = "1 Semester";
                                string feecatagory = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + type + "' and college_code='" + Convert.ToString(ddlCollege.SelectedItem.Value) + "'");
                                string getfinid = d2.getCurrentFinanceYear(UserCode, Convert.ToString(ddlCollege.SelectedItem.Value));

                                if (getfinid.Trim() != "" && getfinid.Trim() != "0" && feecatagory.Trim() != "" && feecatagory.Trim() != "0")
                                {
                                    ArrayList addlederArray = new ArrayList();
                                    addlederArray.Add("2$11$100");
                                    addlederArray.Add("2$12$2000");
                                    addlederArray.Add("2$13$8000");

                                    for (int intd = 0; intd < addlederArray.Count; intd++)
                                    {
                                        string Feeallotheader = ""; string Feeallotledger = ""; double feeamount = 0;
                                        string[] admissionformfee = Convert.ToString(addlederArray[intd]).Split('$');//1$9,10$500
                                        if (admissionformfee.Length > 1)
                                        {
                                            Feeallotheader = Convert.ToString(admissionformfee[0]);
                                            Feeallotledger = Convert.ToString(admissionformfee[1]);
                                            double.TryParse(Convert.ToString(admissionformfee[2]), out feeamount);
                                        }
                                        if (feeamount != 0)
                                        {
                                            if (feecatagory != "" && feecatagory != "0")
                                            {
                                                if (Feeallotheader != "0" && Feeallotledger != "0")
                                                {
                                                    string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + Feeallotledger + "') and HeaderFK in('" + Feeallotheader + "') and FeeCategory in('" + feecatagory + "')  and App_No in('" + app_no_span.InnerHtml + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + feeamount + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + feeamount + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + feeamount + "' where LedgerFK in('" + Feeallotledger + "') and HeaderFK in('" + Feeallotheader + "') and FeeCategory in('" + feecatagory + "') and App_No in('" + app_no_span.InnerHtml + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount, DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no_span.InnerHtml + ",'" + Feeallotledger + "','" + Feeallotheader + "','" + feeamount + "','0','0','0','" + feeamount + "','0','0','','0','" + feecatagory + "','','0','','0','0','" + feeamount + "','" + getfinid + "')";
                                                    insupdquery += "update registration set Stud_Type ='Hostler'  where app_no='" + app_no_span.InnerHtml + "'";
                                                    int a = d2.update_method_wo_parameter(insupdquery, "text");
                                                    if (a != 0)
                                                        insert = true;
                                                }
                                                else
                                                {
                                                    lbl_alert.Text = "Kindly Allot The Hostel Header and Ledger";
                                                    alert_pop.Visible = true;
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                lbl_alert.Text = "Kindly Set Fee catagory";
                                                alert_pop.Visible = true;
                                                return;
                                            }
                                        }
                                    }
                                    if (insert)
                                    {
                                        alert_pop.Visible = true;
                                        lbl_alert.Text = "Saved Successfully";
                                        clearDetails();
                                    }
                                }
                                else
                                {
                                    lbl_alert.Text = "Kindly Set Financial Year or Fee catagory settings";
                                    alert_pop.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            lbl_alert.Text = "No Seats Available";
                            alert_pop.Visible = true;
                        }
                            #endregion
                    }
                    else
                    {
                        #region Hostel
                        checkdatalistselect = checkdatalistreturnroompk();
                        string[] checkdet = checkdatalistselect.Split('$');
                        if (checkdet.Length == 3)
                        {
                            if (ddl_buildingname.Items.Count > 0)
                            {
                                if (ddl_hostel.Items.Count > 0) //if (ddl_hostel.SelectedItem.Value != "Select")
                                {
                                    if (Convert.ToString(checkdet[0]) == "1")
                                    {
                                        string floorpk = ""; string floorname = "";
                                        string roompk = Convert.ToString(checkdet[1]);
                                        string roomname = Convert.ToString(checkdet[2]);
                                        string q1 = " select f.Floor_Name,f.Floorpk from Room_Detail rd,Floor_Master f where rd.Building_Name=f.Building_Name and rd.Floor_Name=f.Floor_Name and f.Building_Name='" + Convert.ToString(ddl_buildingname.SelectedItem.Text) + "' and rd.Room_Name='" + roomname + "' and rd.Room_type='" + Convert.ToString(ddl_roomtype.SelectedItem.Text) + "'";
                                        q1 += " select convert(varchar, hosteladmfeeheaderfk)+'$'+CONVERT(varchar, hosteladmfeeledgerfk)headerandledger from HM_HostelMaster where hostelmasterpk='" + Convert.ToString(ddl_hostel.SelectedValue.Split('$')[0]) + "' ";
                                        q1 += " select convert(varchar(18), Room_Cost)+'$'+CONVERT(varchar(10), Rent_Type)Roomcostandrenttype from RoomCost_Master where college_code='" + Convert.ToString(ddlCollege.SelectedItem.Value) + "' and Room_Type='" + Convert.ToString(ddl_roomtype.SelectedItem.Text) + "'";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(q1, "text");
                                        if (ds.Tables != null)
                                        {
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                floorpk = Convert.ToString(ds.Tables[0].Rows[0]["Floorpk"]);
                                                floorname = Convert.ToString(ds.Tables[0].Rows[0]["Floor_Name"]);
                                            }
                                            if (floorpk.Trim() != "" && roompk.Trim() != "" && Convert.ToString(ddl_buildingname.SelectedItem.Value) != "")
                                            {
                                                #region Insert
                                                string[] ay = txt_date.Text.Split('/');
                                                string admit_date = ay[1].ToString() + "/" + ay[0].ToString() + "/" + ay[2].ToString();
                                                string q = "select students_allowed,Avl_Student from Room_Detail where Room_Type='" + Convert.ToString(ddl_roomtype.SelectedItem.Text) + "' and Floor_Name='" + floorname + "' and Room_Name='" + roomname + "' and Building_Name='" + Convert.ToString(ddl_buildingname.SelectedItem.Text) + "'";
                                                DataSet availabledet = new DataSet();
                                                availabledet = d2.select_method_wo_parameter(q, "text");
                                                double comp1 = 0; double comp2 = 0;
                                                double.TryParse(Convert.ToString(availabledet.Tables[0].Rows[0]["students_allowed"].ToString()), out comp1);
                                                double.TryParse(Convert.ToString(availabledet.Tables[0].Rows[0]["Avl_Student"]), out comp2);
                                                int h = 0;
                                                if (comp1 >= comp2 && comp1 != comp2)
                                                {
                                                    string hostelquery = " update Room_Detail set Avl_Student= isnull(Avl_Student,0) + 1 where Room_Type='" + Convert.ToString(ddl_roomtype.SelectedItem.Text) + "' and Floor_Name='" + floorname + "' and Room_Name='" + roomname + "' and Building_Name='" + Convert.ToString(ddl_buildingname.SelectedItem.Text) + "'";
                                                    hostelquery += " if not exists(select app_no from HT_HostelRegistration where app_no='" + app_no_span.InnerHtml + "') insert into HT_HostelRegistration(MemType,APP_No,HostelAdmDate,BuildingFK, FloorFK,RoomFK,StudMessType,IsDiscontinued, DiscontinueDate, HostelMasterFK,collegecode)values(1,'" + app_no_span.InnerHtml + "','" + admit_date + "','" + Convert.ToString(ddl_buildingname.SelectedItem.Value) + "','" + floorpk + "','" + roompk + "','0','0','','" + ddl_hostel.SelectedValue.Split('$')[0] + "','" + ddlCollege.SelectedItem.Value + "')";
                                                    hostelquery += " update Registration set Stud_Type='Hostler' where App_No='" + app_no_span.InnerHtml + "'";
                                                    h = d2.update_method_wo_parameter(hostelquery, "Text");
                                                    #region Hostel Feeallot
                                                    string Hostelfee = d2.GetFunction("select value from Master_Settings where settings ='HostelFeeAllot' and usercode ='" + UserCode + "'");
                                                    if (Hostelfee == "1")
                                                    {
                                                        string Hostelheader = "";
                                                        string Hostelledger = "";
                                                        if (ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            string[] headerandledger = Convert.ToString(ds.Tables[1].Rows[0]["headerandledger"]).Split('$');
                                                            if (headerandledger.Length == 2)
                                                            {
                                                                Hostelheader = Convert.ToString(headerandledger[0]);
                                                                Hostelledger = Convert.ToString(headerandledger[1]);
                                                            }
                                                        }
                                                        string roomcost = ""; string renttype = "";
                                                        if (ds.Tables[2].Rows.Count > 0)
                                                        {
                                                            string[] Roomcostandrenttype = Convert.ToString(ds.Tables[2].Rows[0]
            ["Roomcostandrenttype"]).Split('$');
                                                            if (Roomcostandrenttype.Length == 2)
                                                            {
                                                                roomcost = Convert.ToString(Roomcostandrenttype[0]);
                                                                renttype = Convert.ToString(Roomcostandrenttype[1]);
                                                            }
                                                        }
                                                        string val = "";
                                                        if (renttype == "2")
                                                            val = "1 Year";
                                                        else
                                                            val = "1 Semester";
                                                        string catagory = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + val + "' and college_code='" + Convert.ToString(ddlCollege.SelectedItem.Value) + "'");
                                                        string getfinid = d2.getCurrentFinanceYear(UserCode, Convert.ToString(ddlCollege.SelectedItem.Value));
                                                        if (getfinid.Trim() == "" || getfinid.Trim() == "0")
                                                        {
                                                            lbl_alert.Text = "Set Financial year settings";
                                                            alert_pop.Visible = true;
                                                            return;
                                                        }
                                                        if (catagory != "" && catagory != "0")
                                                        {
                                                            if (Hostelheader != "0" && Hostelledger != "0" && roomcost != "0" && roomcost != "")
                                                            {
                                                                string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + Hostelledger + "') and HeaderFK in('" + Hostelheader + "') and FeeCategory in('" + catagory + "')  and App_No in('" + app_no_span.InnerHtml + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + roomcost + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + roomcost + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + roomcost + "' where LedgerFK in('" + Hostelledger + "') and HeaderFK in('" + Hostelheader + "') and FeeCategory in('" + catagory + "') and App_No in('" + app_no_span.InnerHtml + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount, DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no_span.InnerHtml + ",'" + Hostelledger + "','" + Hostelheader + "','" + roomcost + "','0','0','0','" + roomcost + "','0','0','','0','" + catagory + "','','0','','0','0','" + roomcost + "','" + getfinid + "')";
                                                                int a = d2.update_method_wo_parameter(insupdquery, "text");
                                                            }
                                                            else
                                                            {
                                                                lbl_alert.Text = "Kindly Allot The Fees Or Hostel Header and Ledger";
                                                                alert_pop.Visible = true;
                                                                return;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            lbl_alert.Text = "Kindly Set Fee catagory";
                                                            alert_pop.Visible = true;
                                                            return;
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    alert_pop.Visible = true;
                                                    lbl_alert.Text = "Room Filled Please Select Another Room Name";
                                                }
                                                #endregion
                                                if (h != 0)
                                                {
                                                    alert_pop.Visible = true;
                                                    lbl_alert.Text = "Saved Successfully";
                                                    bindroomname();
                                                    clearDetails();
                                                }
                                            }
                                            else
                                            {
                                                lbl_alert.Text = "Room Details Missing";
                                                alert_pop.Visible = true;
                                            }
                                        }
                                        else
                                        {
                                            lbl_alert.Text = "Please Run I Patch";
                                            alert_pop.Visible = true;
                                        }
                                    }
                                    else
                                    {
                                        lbl_alert.Text = "Please select one room name";
                                        alert_pop.Visible = true;
                                    }
                                }
                                else
                                {
                                    lbl_alert.Text = "Please select Hostel Name";
                                    alert_pop.Visible = true;
                                }
                            }
                            else
                            {
                                lbl_alert.Text = "Please select Building Name";
                                alert_pop.Visible = true;
                            }
                        }
                        else
                        {
                            lbl_alert.Text = "Please select room name";
                            alert_pop.Visible = true;
                        }
                        #endregion
                    }
                }
                else if (rdbtype.SelectedItem.Value == "1")
                {
                    if (Convert.ToString(ViewState["hostelandtransportsettings"]).Split(',')[1] == "1")
                    {
                        #region Transport fee allot only boarding point
                        string set = " select value from Master_Settings where settings in('TransportFeeAllot') and usercode ='" + UserCode + "'";
                        set += " select LinkValue from New_InsSettings where LinkName='TransportLedgerValue' and user_code ='" + UserCode + "'";
                        set += " select value  from Master_Settings where settings ='TransportFeeAllotmentSettings' and usercode ='" + UserCode + "'";
                        //set += " select convert(varchar(50),(COUNT(distinct r.App_No)))Alloted,convert(varchar, v.firstyearstudent)Available from vehicle_master v left join Registration r on v.Veh_ID=r.VehID where v.Veh_ID='" + ddl_route.SelectedItem.Value + "' and v.Route='" + ddl_route.SelectedItem.Text + "' group by v.Veh_ID,v.firstyearstudent";
                        DataSet setting = d2.select_method_wo_parameter(set, "text");
                        string getfinid = d2.getCurrentFinanceYear(UserCode, Convert.ToString(ddlCollege.SelectedItem.Value));
                        if (getfinid.Trim() != "" && getfinid.Trim() != "0")
                        {
                            if (setting.Tables[0].Rows.Count > 0 && setting.Tables[1].Rows.Count > 0 && setting.Tables[2].Rows.Count > 0 && setting.Tables != null)
                            {
                                string boarding = ddl_stage.SelectedItem.Value;
                                string Total = d2.GetFunction("select COUNT(app_no) as Total from Registration where Boarding ='" + boarding + "'");
                                int Tot = 0;
                                int.TryParse(Convert.ToString(Total), out Tot);
                                if (Tot < 10)
                                {
                                    if (setting.Tables[0].Rows.Count > 0)
                                    {
                                        string transfee = Convert.ToString(setting.Tables[0].Rows[0]["value"]);
                                        string[] valtranc = transfee.Split('/');
                                        if (valtranc[0] == "1")
                                        {
                                            string cost = ""; string header = "";
                                            string value = ""; string ledger = ""; string type = "";
                                            if (setting.Tables[1].Rows.Count > 0)
                                            {
                                                string ledhead = Convert.ToString(setting.Tables[1].Rows[0]["LinkValue"]);
                                                string[] spl = ledhead.Split(',');
                                                header = spl[0];
                                                ledger = spl[1];

                                                if (setting.Tables[2].Rows.Count > 0)
                                                {
                                                    string Transportsettings = Convert.ToString(setting.Tables[2].Rows[0]["value"]);
                                                    if (Transportsettings.Trim() != "" && Transportsettings.Trim() != "0")
                                                    {
                                                        string[] transtype = Transportsettings.Split('-');
                                                        string paytype = "";
                                                        if (transtype[0] == "1")
                                                            paytype = " and payType ='Semester'";
                                                        else if (transtype[0] == "2")
                                                            paytype = " and payType ='Yearly'";
                                                        else
                                                            paytype = " and payType ='Monthly'";
                                                        string values = d2.GetFunction(" select convert(varchar, isnull(cost,0))+'$'+convert(varchar,isnull(payType,0)) from feeinfo where StrtPlace ='" + boarding + "' " + paytype + " ");
                                                        string[] costandpaytype = values.Split('$');
                                                        if (costandpaytype.Length == 2)
                                                        {
                                                            cost = Convert.ToString(costandpaytype[0]);
                                                            value = Convert.ToString(costandpaytype[1]);
                                                            type = "Semester";
                                                        }
                                                        string val = "";
                                                        if (value == "Yearly")
                                                        {
                                                            val = "1 Year";
                                                        }
                                                        else if (value == "Semester")
                                                        {
                                                            val = "1 Semester";
                                                        }
                                                        else if (value == "Monthly")
                                                        {
                                                            string settingquery = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + UserCode + "' and college_code ='" + ddlCollege.SelectedItem.Value + "'");
                                                            if (settingquery.Trim() != "")
                                                            {
                                                                if (settingquery == "0")
                                                                {
                                                                    val = "1 Semester";
                                                                }
                                                                else if (settingquery == "1")
                                                                {
                                                                    val = "1 Year";
                                                                }
                                                            }
                                                        }
                                                        if (val.Trim() != "")
                                                        {
                                                            string textcode = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + val + "' and college_code='" + ddlCollege.SelectedItem.Value + "'");
                                                            if (transtype[0] != "3")
                                                            {
                                                                #region year and semesterwise
                                                                if (textcode != "" && textcode != "0")
                                                                {
                                                                    if (header != "0" && ledger != "0" && cost != "0")
                                                                    {
                                                                        string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "')  and App_No in('" + app_no_span.InnerHtml + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + cost + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + cost + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + cost + "' where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "') and App_No in('" + app_no_span.InnerHtml + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout, DeductReason,FromGovtAmt, TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no_span.InnerHtml + ",'" + ledger + "','" + header + "','" + cost + "','0','0','0','" + cost + "','0','0','','0','" + textcode + "','','0','','0','0','" + cost + "','" + getfinid + "')";
                                                                        insupdquery += "update registration set Boarding='" + boarding + "',Stud_Type='Day Scholar' where app_no='" + app_no_span.InnerHtml + "'";
                                                                        int u = d2.update_method_wo_parameter(insupdquery, "text");
                                                                        if (u != 0)
                                                                            insert = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        lbl_alert.Text = "Kindly Allot The Fees";
                                                                        alert_pop.Visible = true;
                                                                        return;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    lbl_alert.Text = "Kindly Allot The Feecatagory";
                                                                    alert_pop.Visible = true;
                                                                    return;
                                                                }
                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                #region Monthwise
                                                                double calcost = 0;
                                                                string mnthamt = "";
                                                                string[] yearcal = transtype[1].Split(';');
                                                                string[] monthcal = yearcal[0].Split(',');
                                                                for (int u = 0; u < monthcal.Length; u++)
                                                                {
                                                                    string year = yearcal[1];
                                                                    if (mnthamt == "")
                                                                        mnthamt = monthcal[u] + ":" + year + ":" + cost;
                                                                    else
                                                                        mnthamt = mnthamt + "," + monthcal[u] + ":" + year + ":" + cost;
                                                                    calcost = calcost + Convert.ToDouble(cost);
                                                                }
                                                                string querystu1 = " if exists (select * from FT_FeeAllot where App_No ='" + app_no_span.InnerHtml + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "' ) update FT_FeeAllot set FeeAmount='" + calcost + "',TotalAmount ='" + calcost + "' ,BalAmount ='" + calcost + "', FeeAmountMonthly='" + mnthamt + "'  where App_No ='" + app_no_span.InnerHtml + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "'  else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt,FeeAmountMonthly)  values ('" + app_no_span.InnerHtml + "','" + ledger + "','" + header + "','" + getfinid + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + calcost + "','" + textcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "',0,0,'" + calcost + "','" + calcost + "','1','1',0,0,'" + mnthamt + "')";
                                                                querystu1 += "update registration set Boarding='" + boarding + "' where app_no='" + app_no_span.InnerHtml + "'";
                                                                int uh = d2.update_method_wo_parameter(querystu1, "text");
                                                                if (uh != 0)
                                                                    insert = true;
                                                                string allotpk = d2.GetFunction(" select FeeAllotPK from FT_FeeAllot  where App_No ='" + app_no_span.InnerHtml + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "'");
                                                                if (allotpk != "")
                                                                {
                                                                    for (int u = 0; u < monthcal.Length; u++)
                                                                    {
                                                                        string year = yearcal[1];
                                                                        string InsertQ = "if exists(select * from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + monthcal[u] + "' and AllotYear='" + year + "' and FinYearFK='" + getfinid + "')update FT_FeeallotMonthly set AllotAmount=AllotAmount+'" + cost + "',BalAmount=BalAmount+'" + cost + "' where FeeAllotPK='" + allotpk + "' and AllotMonth='" + monthcal[u] + "' and AllotYear='" + year + "' and FinYearFK='" + getfinid + "' else insert into FT_FeeallotMonthly (FeeAllotPK,AllotMonth,AllotYear,AllotAmount,FinYearFK,BalAmount) values('" + allotpk + "','" + monthcal[u] + "','" + year + "','" + cost + "','" + getfinid + "','" + cost + "')";
                                                                        int ins = d2.update_method_wo_parameter(InsertQ, "Text");
                                                                    }
                                                                }
                                                                #endregion
                                                            }
                                                            //string appnumb = Convert.ToString(app_no_span.InnerHtml);
                                                            //travelAllotment(appnumb, type);
                                                        }
                                                        else
                                                        {
                                                            lbl_alert.Text = "Please set Boarding point fees settings";
                                                            alert_pop.Visible = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lbl_alert.Text = "Please set Transport Fee Allotment settings";
                                                        alert_pop.Visible = true;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lbl_alert.Text = "Please set Transport FeeAllot settings";
                                            alert_pop.Visible = true;
                                        }
                                    }
                                }
                                else
                                {
                                    lbl_alert.Text = "No Seat Available";
                                    alert_pop.Visible = true;
                                }
                                //}
                                //else
                                //{
                                //    lbl_alert.Text = "Seat Availablity Full";
                                //    alert_pop.Visible = true;
                                //    return;
                                //}
                            }
                            else
                            {
                                alert_pop.Visible = true;
                                lbl_alert.Text = "Please set Transport FeeAllot settings";
                                if (setting.Tables[3].Rows.Count == 0)
                                {
                                    alert_pop.Visible = true;
                                    lbl_alert.Text = "Please allot firstyear seat";
                                }
                            }
                        }
                        else
                        {
                            alert_pop.Visible = true;
                            lbl_alert.Text = "Please set Financial year settings";
                        }
                        if (insert == true)
                        {
                            alert_pop.Visible = true;
                            lbl_alert.Text = "Transport Fee Alloted Successfully"; bindavailableseat();
                            clearDetails();
                            txt_applicationno.Text = "";
                        }
                        #endregion
                    }
                    else
                    {
                        #region Transport fee allot
                        string set = " select value from Master_Settings where settings in('TransportFeeAllot') and usercode ='" + UserCode + "'";
                        set += " select LinkValue from New_InsSettings where LinkName='TransportLedgerValue' and user_code ='" + UserCode + "'";
                        set += " select value  from Master_Settings where settings ='TransportFeeAllotmentSettings' and usercode ='" + UserCode + "'";
                        set += " select convert(varchar(50),(COUNT(distinct r.App_No)))Alloted,convert(varchar, v.firstyearstudent)Available from vehicle_master v left join Registration r on v.Veh_ID=r.VehID where v.Veh_ID='" + ddl_route.SelectedItem.Value + "' and v.Route='" + ddl_route.SelectedItem.Text + "' group by v.Veh_ID,v.firstyearstudent";
                        DataSet setting = d2.select_method_wo_parameter(set, "text");
                        string getfinid = d2.getCurrentFinanceYear(UserCode, Convert.ToString(ddlCollege.SelectedItem.Value));
                        if (getfinid.Trim() != "" && getfinid.Trim() != "0")
                        {
                            if (setting.Tables[0].Rows.Count > 0 && setting.Tables[1].Rows.Count > 0 && setting.Tables[2].Rows.Count > 0 && setting.Tables[3].Rows.Count > 0 && setting.Tables != null)
                            {
                                double available = 0; double allot = 0;
                                if (setting.Tables[3].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(setting.Tables[3].Rows[0]["Available"]), out available);
                                    double.TryParse(Convert.ToString(setting.Tables[3].Rows[0]["Alloted"]), out allot);
                                }
                                if (available >= allot && available != allot)
                                {
                                    if (setting.Tables[0].Rows.Count > 0)
                                    {
                                        string transfee = Convert.ToString(setting.Tables[0].Rows[0]["value"]);
                                        string[] valtranc = transfee.Split('/');
                                        if (valtranc[0] == "1")
                                        {
                                            string cost = ""; string header = "";
                                            string value = ""; string ledger = ""; string type = "";
                                            if (setting.Tables[1].Rows.Count > 0)
                                            {
                                                string ledhead = Convert.ToString(setting.Tables[1].Rows[0]["LinkValue"]);
                                                string[] spl = ledhead.Split(',');
                                                header = spl[0];
                                                ledger = spl[1];
                                                string boarding = ddl_stage.SelectedItem.Value;
                                                if (setting.Tables[2].Rows.Count > 0)
                                                {
                                                    string Transportsettings = Convert.ToString(setting.Tables[2].Rows[0]["value"]);
                                                    if (Transportsettings.Trim() != "" && Transportsettings.Trim() != "0")
                                                    {
                                                        string[] transtype = Transportsettings.Split('-');
                                                        string paytype = "";
                                                        if (transtype[0] == "1")
                                                            paytype = " and payType ='Semester'";
                                                        else if (transtype[0] == "2")
                                                            paytype = " and payType ='Yearly'";
                                                        else
                                                            paytype = " and payType ='Monthly'";
                                                        string values = d2.GetFunction(" select convert(varchar, isnull(cost,0))+'$'+convert(varchar,isnull(payType,0)) from feeinfo where StrtPlace ='" + boarding + "' " + paytype + " ");
                                                        string[] costandpaytype = values.Split('$');
                                                        if (costandpaytype.Length == 2)
                                                        {
                                                            cost = Convert.ToString(costandpaytype[0]);
                                                            value = Convert.ToString(costandpaytype[1]);
                                                            type = "Semester";
                                                        }
                                                        string val = "";
                                                        if (value == "Yearly")
                                                        {
                                                            val = "1 Year";
                                                        }
                                                        else if (value == "Semester")
                                                        {
                                                            val = "1 Semester";
                                                        }
                                                        else if (value == "Monthly")
                                                        {
                                                            string settingquery = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + UserCode + "' and college_code ='" + ddlCollege.SelectedItem.Value + "'");
                                                            if (settingquery.Trim() != "")
                                                            {
                                                                if (settingquery == "0")
                                                                {
                                                                    val = "1 Semester";
                                                                }
                                                                else if (settingquery == "1")
                                                                {
                                                                    val = "1 Year";
                                                                }
                                                            }
                                                        }
                                                        if (val.Trim() != "")
                                                        {
                                                            string textcode = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + val + "' and college_code='" + ddlCollege.SelectedItem.Value + "'");
                                                            if (transtype[0] != "3")
                                                            {
                                                                #region year and semesterwise
                                                                if (textcode != "" && textcode != "0")
                                                                {
                                                                    if (header != "0" && ledger != "0" && cost != "0")
                                                                    {
                                                                        string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "')  and App_No in('" + app_no_span.InnerHtml + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + cost + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + cost + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + cost + "' where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "') and App_No in('" + app_no_span.InnerHtml + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout, DeductReason,FromGovtAmt, TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no_span.InnerHtml + ",'" + ledger + "','" + header + "','" + cost + "','0','0','0','" + cost + "','0','0','','0','" + textcode + "','','0','','0','0','" + cost + "','" + getfinid + "')";
                                                                        insupdquery += "update registration set Boarding='" + boarding + "' where app_no='" + app_no_span.InnerHtml + "'";
                                                                        int u = d2.update_method_wo_parameter(insupdquery, "text");
                                                                        if (u != 0)
                                                                            insert = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        lbl_alert.Text = "Kindly Allot The Fees";
                                                                        alert_pop.Visible = true;
                                                                        return;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    lbl_alert.Text = "Kindly Allot The Feecatagory";
                                                                    alert_pop.Visible = true;
                                                                    return;
                                                                }
                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                #region Monthwise
                                                                double calcost = 0;
                                                                string mnthamt = "";
                                                                string[] yearcal = transtype[1].Split(';');
                                                                string[] monthcal = yearcal[0].Split(',');
                                                                for (int u = 0; u < monthcal.Length; u++)
                                                                {
                                                                    string year = yearcal[1];
                                                                    if (mnthamt == "")
                                                                        mnthamt = monthcal[u] + ":" + year + ":" + cost;
                                                                    else
                                                                        mnthamt = mnthamt + "," + monthcal[u] + ":" + year + ":" + cost;
                                                                    calcost = calcost + Convert.ToDouble(cost);
                                                                }
                                                                string querystu1 = " if exists (select * from FT_FeeAllot where App_No ='" + app_no_span.InnerHtml + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "' ) update FT_FeeAllot set FeeAmount='" + calcost + "',TotalAmount ='" + calcost + "' ,BalAmount ='" + calcost + "', FeeAmountMonthly='" + mnthamt + "'  where App_No ='" + app_no_span.InnerHtml + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "'  else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt,FeeAmountMonthly)  values ('" + app_no_span.InnerHtml + "','" + ledger + "','" + header + "','" + getfinid + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + calcost + "','" + textcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "',0,0,'" + calcost + "','" + calcost + "','1','1',0,0,'" + mnthamt + "')";
                                                                querystu1 += "update registration set Boarding='" + boarding + "' where app_no='" + app_no_span.InnerHtml + "'";
                                                                int uh = d2.update_method_wo_parameter(querystu1, "text");
                                                                if (uh != 0)
                                                                    insert = true;
                                                                string allotpk = d2.GetFunction(" select FeeAllotPK from FT_FeeAllot  where App_No ='" + app_no_span.InnerHtml + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "'");
                                                                if (allotpk != "")
                                                                {
                                                                    for (int u = 0; u < monthcal.Length; u++)
                                                                    {
                                                                        string year = yearcal[1];
                                                                        string InsertQ = "if exists(select * from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + monthcal[u] + "' and AllotYear='" + year + "' and FinYearFK='" + getfinid + "')update FT_FeeallotMonthly set AllotAmount=AllotAmount+'" + cost + "',BalAmount=BalAmount+'" + cost + "' where FeeAllotPK='" + allotpk + "' and AllotMonth='" + monthcal[u] + "' and AllotYear='" + year + "' and FinYearFK='" + getfinid + "' else insert into FT_FeeallotMonthly (FeeAllotPK,AllotMonth,AllotYear,AllotAmount,FinYearFK,BalAmount) values('" + allotpk + "','" + monthcal[u] + "','" + year + "','" + cost + "','" + getfinid + "','" + cost + "')";
                                                                        int ins = d2.update_method_wo_parameter(InsertQ, "Text");
                                                                    }
                                                                }
                                                                #endregion
                                                            }
                                                            string appnumb = Convert.ToString(app_no_span.InnerHtml);
                                                            travelAllotment(appnumb, type);
                                                        }
                                                        else
                                                        {
                                                            lbl_alert.Text = "Please set Boarding point fees settings";
                                                            alert_pop.Visible = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lbl_alert.Text = "Please set Transport Fee Allotment settings";
                                                        alert_pop.Visible = true;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lbl_alert.Text = "Please set Transport FeeAllot settings";
                                            alert_pop.Visible = true;
                                        }
                                    }
                                }
                                else
                                {
                                    lbl_alert.Text = "Seat Availablity Full";
                                    alert_pop.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                alert_pop.Visible = true;
                                lbl_alert.Text = "Please set Transport FeeAllot settings";
                                if (setting.Tables[3].Rows.Count == 0)
                                {
                                    alert_pop.Visible = true;
                                    lbl_alert.Text = "Please allot firstyear seat";
                                }
                            }
                        }
                        else
                        {
                            alert_pop.Visible = true;
                            lbl_alert.Text = "Please set Financial year settings";
                        }
                        if (insert == true)
                        {
                            alert_pop.Visible = true;
                            lbl_alert.Text = "Transport Fee Alloted Successfully"; bindavailableseat();
                            clearDetails();
                            txt_applicationno.Text = "";
                        }
                        #endregion
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, ddlCollege.SelectedValue, "Student_Hostelandtransport_request");
        }
    }
    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
    }
    public void travelAllotment(string appnumber, string type)
    {
        try
        {
            string sqlcmd = "";
            string routeid = "";
            string Dep_Time = "";
            string Arr_Time = "";
            string addrouteid = "";
            double duration = 0;
            string routid_dur = "";
            string Stage_id = "";
            string Veh_ID = "";
            string veh_ids = "";
            sqlcmd = " (select distinct v.Veh_ID,r.Route_ID,s.Stage_Name,Stage_id,Arr_Time,Dep_Time,Stages,TotalNo_Seat,nofstudents,nofStaffs from vehicle_master v,routemaster r,stage_master s where v.veh_id=r.veh_id and v.route=r.route_id and convert(varchar(50),s.Stage_id)=(r.Stage_Name) and college_code like'%" + ddlCollege.SelectedItem.Value + "%' and s.stage_name='" + Convert.ToString(ddl_stage.SelectedItem.Text) + "' and sess='M') UNION (select distinct v.Veh_ID,r.Route_ID,s.Stage_Name,Stage_id,Arr_Time,Dep_Time,Stages, TotalNo_Seat,nofstudents,nofStaffs from vehicle_master v,routemaster r,stage_master s where v.veh_id=r.veh_id and v.route=r.route_id and convert(varchar(50),s.Stage_id)=(r.Stage_Name) and (college_code is null or college_code='' or college_code not like'%" + ddlCollege.SelectedItem.Value + "%') and s.stage_name='" + Convert.ToString(ddl_stage.SelectedItem.Text) + "' and sess='M')";
            ds = d2.select_method_wo_parameter(sqlcmd, "Text");
            Dictionary<int, double> routee = new Dictionary<int, double>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                for (int y = 0; y < ds.Tables[0].Rows.Count; y++)
                {
                    routeid = Convert.ToString(ds.Tables[0].Rows[y]["Route_ID"]);
                    Dep_Time = d2.GetFunction("select Arr_Time  from routemaster where Route_ID='" + routeid + "' and sess='M' and (Dep_Time like 'Hal%')");
                    Arr_Time = d2.GetFunction("select Dep_Time  from routemaster where Route_ID='" + routeid + "' and sess='M' and (Arr_Time like 'Ha%')");
                    duration = Convert.ToDouble(Dep_Time) - Convert.ToDouble(Arr_Time);
                    Stage_id = Convert.ToString(ds.Tables[0].Rows[y]["Stage_id"]);
                    Veh_ID = Convert.ToString(ds.Tables[0].Rows[y]["Veh_ID"]);
                    if (addrouteid == "")
                    {
                        addrouteid = Convert.ToString(duration);
                        routid_dur = routeid;
                        veh_ids = Veh_ID;
                    }
                    else
                    {
                        if (Convert.ToDouble(addrouteid) > duration)
                        {
                            addrouteid = Convert.ToString(duration);
                            routid_dur = routeid;
                        }
                    }
                }
                string querystu = "update registration set Bus_RouteID='" + routid_dur + "',Boarding='" + Stage_id + "',VehID='" + veh_ids + "',Trans_PayType='" + type + "',Traveller_Date = '" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no='" + appnumber + "'";
                int u = d2.update_method_wo_parameter(querystu, "text");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, ddlCollege.SelectedItem.Value, "Student_Hostelandtransport_request");
        }
    }
    protected string checkdatalistreturnroompk()
    {
        string checkval = ""; string roompk = "";
        int count = 0;
        foreach (DataListItem gvrow in roomno_datalist.Items)
        {
            CheckBox chkSelect = (gvrow.FindControl("cb_room_dv") as CheckBox);
            if (chkSelect.Checked)
            {
                count++;
                Label room_pk = (gvrow.FindControl("lbl_roompk") as Label);
                Label room_name = (gvrow.FindControl("lbl_roomname") as Label);
                string[] roomname = room_name.Text.Split('(');
                roompk = room_pk.Text + '$' + roomname[0];
            }
        }
        checkval = count + "$" + roompk;
        return checkval;
    }
    //Added by Idhris 30-05-2017
    protected void btn_clear_click(object sender, EventArgs e)
    {
        clearDetails();
    }
    private void clearDetails()
    {
        //Personal Details
        applicantno_span.InnerHtml = ":";
        dob_span.InnerHtml = ":";
        studmobileno_span.InnerHtml = ":";
        applicantname_span.InnerHtml = ":";
        gender_span.InnerHtml = ":";
        fathername_span.InnerHtml = ":";
        //Other tab details
        rdbtype.SelectedIndex = 0;
        rdbtype_SelectedIndexChanged(new object(), new EventArgs());
        if (ddl_hostel.Items.Count > 0) ddl_hostel.SelectedIndex = 0;
        bindbuilding();
        bindroomtype();
        bindroomname();
        verification_div.Visible = false;
    }
}
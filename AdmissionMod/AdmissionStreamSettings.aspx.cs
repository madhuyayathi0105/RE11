/*
 * 
 * Author : Mohamed Idhris Sheik Dawood
 * Date created : 26-05-2017
 * 
 * */

using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Linq;
using InsproDataAccess;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

public partial class AdmissionStreamSettings : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
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
            txtCommDate.Attributes.Add("readonly", "readonly");
            txtCommDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            bindCollege();
            bindBatch();
            bindEdulevel();
            bindCourse();
            bindStream();
            bindCategory();
            loadValues();
            Bindheaderledger();
        }
    }
    //Base screen controls loaders
    private void bindCollege()
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
    private void bindBatch()
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
    private void bindEdulevel()
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
    private void bindCourse()
    {
        try
        {
            cbl_Session.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct course_id,Course_Name from Course where college_code=" + ddlCollege.SelectedValue + " and edu_level='" + ddlEduLev.SelectedValue + "' order by course_id", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Session.DataSource = ds;
                cbl_Session.DataTextField = "Course_Name";
                cbl_Session.DataValueField = "course_id";
                cbl_Session.DataBind();
            }
        }
        catch
        {

        }
    }
    private void bindStream()
    {
        try
        {
            ddlStream.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("SELECT TextVal,TextCode FROM TextValTable WHERE TextCriteria='ADMst' AND college_code='" + ddlCollege.SelectedValue + "' order by TextVal,TextCode ", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlStream.DataSource = ds;
                ddlStream.DataTextField = "TextVal";
                ddlStream.DataValueField = "TextCode";
                ddlStream.DataBind();
            }
        }
        catch
        {

        }
    }
    private void bindCategory()
    {
        try
        {
            ddlCategory.Items.Clear();
            DataSet dsStudRankCrit = d2.select_method_wo_parameter("select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudRankCriteria' and collegeCode ='" + ddlCollege.SelectedValue + "' ", "Text");
            if (dsStudRankCrit.Tables.Count > 0 && dsStudRankCrit.Tables[0].Rows.Count > 0)
            {
                ddlCategory.DataSource = dsStudRankCrit;
                ddlCategory.DataTextField = "MasterValue";
                ddlCategory.DataValueField = "MasterCode";
                ddlCategory.DataBind();
            }
        }
        catch { }
    }
    //Base screen controls events
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBatch();
        bindEdulevel();
        bindCourse();
        bindStream();
        bindCategory();
        loadValues();
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEdulevel();
        bindCourse();
        bindStream();
    }
    protected void ddlEdulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindCourse();
        bindStream();
    }
    protected void ddlcourse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindStream();
    }
    private void loadValues()
    {
        try
        {
            string[] resVal = dirAcc.selectScalarString("SELECT LinkValue FROM New_InsSettings WHERE LinkName='ADMISSIONCOURSESELECTIONSETTINGS' AND college_code='" + ddlCollege.SelectedValue + "'").Split('$');

            if (resVal.Length == 6)
            {
                string collegeCode = resVal[0];
                string batchYear = resVal[1];
                string eduLevel = resVal[2];
                string courseCode = resVal[3];
                string streamCode = resVal[4];
                string criteriaCode = resVal[5];
                ddlbatch.SelectedIndex = ddlbatch.Items.IndexOf(ddlbatch.Items.FindByValue(batchYear));
                bindEdulevel();
                ddlEduLev.SelectedIndex = ddlEduLev.Items.IndexOf(ddlEduLev.Items.FindByValue(eduLevel));
                bindCourse();
                string[] splitArray = courseCode.Split(',');
                if (splitArray.Length > 0)
                {
                    if (cbl_Session.Items.Count > 0)
                    {
                        for (int intcbl = 0; intcbl < cbl_Session.Items.Count; intcbl++)
                        {
                            for (int intsplit = 0; intsplit < splitArray.Length; intsplit++)
                            {
                                if (splitArray[intsplit].Trim() != "")
                                {
                                    if (cbl_Session.Items[intcbl].Value == splitArray[intsplit])
                                    {
                                        cbl_Session.Items[intcbl].Selected = true;
                                    }
                                }
                            }
                        }

                    }
                    txtSession.Text = "Course (" + splitArray.Length + ")";
                }
                // ddlcourse.SelectedIndex = ddlcourse.Items.IndexOf(ddlcourse.Items.FindByValue(courseCode));
                bindStream();
                ddlStream.SelectedIndex = ddlStream.Items.IndexOf(ddlStream.Items.FindByValue(streamCode));
                bindCategory();
                ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(criteriaCode));

                string includebatch = d2.GetFunction("SELECT LinkValue FROM New_InsSettings WHERE LinkName='RoomDetailIncludebatch' AND college_code='" + collegeCode + "' and user_code='" + UserCode + "'");
                if (includebatch == "1")
                    cb_includebatch.Checked = true;
                else
                    cb_includebatch.Checked = false;
                cb_hostelfees.Checked = false;
                cb_transportfees.Checked = false;
                string[] hostelfeesettings = dirAcc.selectScalarString("select LinkValue from New_InsSettings where LinkName='ONLY HOSTELFEE OR TRANSPORTFEE' and college_code='" + collegeCode + "'").Split(',');
                if (hostelfeesettings.Length == 2)
                {
                    if (Convert.ToString(hostelfeesettings[0]) == "1")
                        cb_hostelfees.Checked = true;
                    if (Convert.ToString(hostelfeesettings[1]) == "1")
                        cb_transportfees.Checked = true;
                }

                string ListRegistration = dirAcc.selectScalarString("select LinkValue from New_InsSettings where LinkName='ShowListRegistration' and college_code='" + collegeCode + "'");

                if (Convert.ToString(ListRegistration) == "1")
                    cb_ListRegister.Checked = true;



                string commDateTime = dirAcc.selectScalarString("select LinkValue from New_InsSettings where college_code='" + collegeCode + "' and LinkName='CommenceDateAndTime'");
                string[] commDateTimes = commDateTime.Split(',');
                if (commDateTimes.Length == 2)
                {
                    string commDate = commDateTimes[0];
                    txtCommDate.Text = commDate;
                    string[] commTime = commDateTimes[1].Split(' ');
                    if (commTime.Length == 2)
                    {
                        string ampm = commTime[1].Trim().ToUpper();
                        ddlCommAmPm.SelectedIndex = ddlCommAmPm.Items.IndexOf(ddlCommAmPm.Items.FindByText(ampm));
                        string[] hrsmins = commTime[0].Split(':');
                        if (hrsmins.Length == 2)
                        {
                            ddlComHrs.SelectedIndex = ddlComHrs.Items.IndexOf(ddlComHrs.Items.FindByText(hrsmins[0]));
                            ddlCommMin.SelectedIndex = ddlCommMin.Items.IndexOf(ddlCommMin.Items.FindByText(hrsmins[1]));
                        }
                    }
                }
            }

        }
        catch { }
    }
    //Base screen save
    protected void btnDaySlotSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string batchYear = Convert.ToString(ddlbatch.SelectedValue);
            string eduLevel = Convert.ToString(ddlEduLev.SelectedValue);
            //string courseCode = Convert.ToString(ddlcourse.SelectedValue);
            string courseCode = rs.GetSelectedItemsValue(cbl_Session);
            string streamCode = Convert.ToString(ddlStream.SelectedValue);
            string criteriaCode = Convert.ToString(ddlCategory.SelectedValue);
            string includebatch = "0";
            if (cb_includebatch.Checked)
                includebatch = "1";
            if (collegeCode != string.Empty && batchYear != string.Empty && eduLevel != string.Empty && courseCode != string.Empty && streamCode != string.Empty)
            {
                string saveVal = collegeCode + "$" + batchYear + "$" + eduLevel + "$" + courseCode + "$" + streamCode + "$" + criteriaCode;
                string insUpdQ = " IF EXISTS (SELECT LinkValue FROM New_InsSettings WHERE LinkName='ADMISSIONCOURSESELECTIONSETTINGS' AND college_code='" + collegeCode + "') UPDATE New_InsSettings SET LinkValue='" + saveVal + "' WHERE  LinkName='ADMISSIONCOURSESELECTIONSETTINGS' AND college_code='" + collegeCode + "' INSERT INTO New_InsSettings (LinkName,LinkValue,college_code) VALUES ('ADMISSIONCOURSESELECTIONSETTINGS','" + saveVal + "','" + collegeCode + "') ";
                insUpdQ += " if exists (select linkname from New_InsSettings where user_code='" + UserCode + "' and college_code='" + collegeCode + "' and LinkName='RoomDetailIncludebatch') update New_InsSettings set LinkValue='" + includebatch + "' where user_code='" + UserCode + "' and college_code='" + collegeCode + "' and LinkName='RoomDetailIncludebatch' else    insert into New_InsSettings (LinkName,LinkValue,user_code,college_code) values('RoomDetailIncludebatch','" + includebatch + "','" + UserCode + "','" + collegeCode + "') ";
                string commDate = txtCommDate.Text;
                string commTime = ddlComHrs.SelectedValue + ":" + ddlCommMin.SelectedValue + " " + ddlCommAmPm.SelectedValue;
                insUpdQ += " if exists (select linkname from New_InsSettings where college_code='" + collegeCode + "' and LinkName='CommenceDateAndTime') update New_InsSettings set LinkValue='" + (commDate + "," + commTime) + "' where  college_code='" + collegeCode + "' and LinkName='CommenceDateAndTime' else    insert into New_InsSettings (LinkName,LinkValue,user_code,college_code) values('CommenceDateAndTime','" + (commDate + "," + commTime) + "','" + UserCode + "','" + collegeCode + "') ";
                string hosteltransportvalue = (cb_hostelfees.Checked == true) ? "1" : "0";
                hosteltransportvalue += (cb_transportfees.Checked == true) ? ",1" : ",0";
                insUpdQ += " if exists (SELECT * FROM New_InsSettings WHERE LinkName='ONLY HOSTELFEE OR TRANSPORTFEE' AND college_code='" + collegeCode + "' and user_code='" + UserCode + "')UPDATE New_InsSettings SET LinkValue='" + hosteltransportvalue + "' WHERE LinkName='ONLY HOSTELFEE OR TRANSPORTFEE' AND college_code='" + collegeCode + "' and user_code='" + UserCode + "' ELSE INSERT INTO New_InsSettings (LinkName,LinkValue,user_code,college_code)VALUES('ONLY HOSTELFEE OR TRANSPORTFEE','" + hosteltransportvalue + "','" + UserCode + "','" + collegeCode + "')";
                string ShwolistRigester = (cb_ListRegister.Checked == true) ? "1" : "0";

                insUpdQ += " if exists (SELECT * FROM New_InsSettings WHERE LinkName='ShowListRegistration' AND college_code='" + collegeCode + "' and user_code='" + UserCode + "')UPDATE New_InsSettings SET LinkValue='" + ShwolistRigester + "' WHERE LinkName='ShowListRegistration' AND college_code='" + collegeCode + "' and user_code='" + UserCode + "' ELSE INSERT INTO New_InsSettings (LinkName,LinkValue,user_code,college_code)VALUES('ShowListRegistration','" + ShwolistRigester + "','" + UserCode + "','" + collegeCode + "')";

                dirAcc.updateData(insUpdQ);
                lbl_alert.Text = "Saved Successfully";
            }
            else
            {
                lbl_alert.Text = "Please check inputs";
            }
        }
        catch
        {
            lbl_alert.Text = "Please try later";
        }
        imgdiv2.Visible = true;
    }
    protected void btnRegSignUpload_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (fuRegSign.HasFile)
            {
                string fileExt = Path.GetExtension(fuRegSign.FileName).ToUpper();
                List<string> lstFileExt = new List<string>();
                lstFileExt.Add(".JPG");
                lstFileExt.Add(".JPEG");

                if (lstFileExt.Contains(fileExt))
                {
                    int length = fuRegSign.PostedFile.ContentLength;
                    byte[] pic = new byte[length];

                    fuRegSign.PostedFile.InputStream.Read(pic, 0, length);

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    try
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand("update collinfo set registrarSign=@photo where college_code='" + ddlCollege.SelectedValue + "'", con);
                        com.Parameters.AddWithValue("@photo", pic);
                        com.ExecuteNonQuery();
                    }
                    finally
                    {
                        con.Close();
                    }

                    lbl_alert.Text = "Uploaded Successfully";
                }
                else
                {
                    lbl_alert.Text = "Please select (.jpeg or .jpg) file";
                }
            }
            else
            {
                lbl_alert.Text = "Please select file";
            }
        }
        catch { lbl_alert.Text = "Please try later"; }
        imgdiv2.Visible = true;
    }
    //Alert Close
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void cb_Session_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_Session, cb_Session, txtSession, "Course");
        // btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void cbl_Session_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_Session, cb_Session, txtSession, "Course");
        //btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }

    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }

    protected void btn_hostelfeesave_click(object sender, EventArgs e)//barath 03.06.17
    {
        if (ddl_admissionH.Items.Count > 0 && ddl_admissionL.Items.Count > 0)
        {
            string app_gensettings = "";
            if (cb_hosteladmissionformfee.Checked == true)
                app_gensettings = "1";
            else
                app_gensettings = "0";
            string hosteladmissionformfeeset = app_gensettings + "$" + Convert.ToString(ddl_admissionH.SelectedItem.Value + "," + ddl_admissionL.SelectedItem.Value + "$" + txt_admissionfee.Text);

            //string insqry1 = "if exists (select * from New_InsSettings where LinkName='Hostel_Admission_Form_Fee' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + hosteladmissionformfeeset + "' where LinkName='Hostel_Admission_Form_Fee' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Hostel_Admission_Form_Fee','" + hosteladmissionformfeeset + "','" + selected_usercode + "','" + ddlcollege.SelectedItem.Value + "')";
            //int ins = d2.update_method_wo_parameter(insqry1, "Text");
            //if (ins != 0)
            //{
            //    imgAlert.Visible = true;
            //    lbl_alert.Text = "Saved successfully";
            //}
        }
    }

    public void Bindheaderledger()
    {
        try
        {
            string straccheadquery = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H WHERE CollegeCode = " + ddlCollege.SelectedValue + "";
            DataSet ds = d2.select_method_wo_parameter(straccheadquery, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                ddl_admissionH.DataSource = ds;
                ddl_admissionH.DataTextField = "HeaderName";
                ddl_admissionH.DataValueField = "HeaderPK";
                ddl_admissionH.DataBind();
                if (ddl_admissionH.Items.Count > 0)
                {
                    string strquer = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L WHERE  l.LedgerMode=0   AND L.CollegeCode = " + ddlCollege.SelectedValue + " and L.HeaderFK in (" + Convert.ToString(ddl_admissionH.SelectedItem.Value) + ")";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(strquer, "text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_admissionL.DataSource = ds;
                        ddl_admissionL.DataTextField = "ledgername";
                        ddl_admissionL.DataValueField = "ledgerpk";
                        ddl_admissionL.DataBind();
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void ddl_admissionH_SelectedIndexChanged(object sender, EventArgs e)
    {
        //barath 29.03.17
        if (ddl_admissionH.Items.Count > 0)
        {
            string strquer = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L WHERE  l.LedgerMode=0   AND L.CollegeCode = " + ddlCollege.SelectedValue + " and L.HeaderFK in (" + Convert.ToString(ddl_admissionH.SelectedItem.Value) + ")";
            DataSet ds1 = new DataSet();
            ds1 = d2.select_method_wo_parameter(strquer, "text");
            ddl_admissionL.Items.Clear();
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                ddl_admissionL.DataSource = ds1;
                ddl_admissionL.DataTextField = "ledgername";
                ddl_admissionL.DataValueField = "ledgerpk";
                ddl_admissionL.DataBind();
            }
        }
    }
}
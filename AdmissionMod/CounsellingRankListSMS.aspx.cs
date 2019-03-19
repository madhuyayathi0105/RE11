using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Linq;
using InsproDataAccess;
using System.Collections.Generic;

public partial class AdmissionMod_CounsellingRankListSMS : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
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
            bindStream();

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
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct course_id,Course_Name from Course where college_code=" + ddlCollege.SelectedValue + " and edu_level='" + ddlEduLev.SelectedValue + "' order by course_id", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcourse.DataSource = ds;
                ddlcourse.DataTextField = "Course_Name";
                ddlcourse.DataValueField = "course_id";
                ddlcourse.DataBind();
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

    //Common Functions
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
    //Base screen controls events
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBatch();
        bindEdulevel();
        bindCourse();
        bindStream();

        //btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEdulevel();
        bindCourse();
        bindStream();

        //btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlEdulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindCourse();
        bindStream();

        //btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlcourse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindStream();

        // btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlStream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlDate_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    //Send Sms for slot allocation
    private void SendSms(string userCode, string collegeCode, List<Int64> lstAppNo, string message)
    {
        try
        {
            string user_id = string.Empty;
            string SenderID = string.Empty;
            string Password = string.Empty;

            string ssr = "select * from Track_Value where college_code='" + collegeCode + "'";
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(ssr, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]).Trim();
            }

            if (user_id != string.Empty)
            {
                string getval = d2.GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {

                    SenderID = spret[0].ToString();
                    Password = spret[0].ToString();

                }
                foreach (Int64 app in lstAppNo)
                {
                    string appformNo = dirAcc.selectScalarString("SELECT app_formno FROM applyn where app_no='" + app + "'");
                    string Mobile_no = dirAcc.selectScalarString("SELECT Student_Mobile FROM applyn where app_no='" + app + "'");
                    string Msg = "Your slot for SASTRA University admission counselling has been alloted. ";

                    d2.send_sms(user_id.Trim(), collegeCode, userCode, Mobile_no, Msg, "1");
                }
            }
        }
        catch
        {
        }
    }

    protected void btnBaseGo_Click(object sender, EventArgs e)
    {
        try
        {
            bindGoDetails();
        }
        catch
        {
        }
    }

    public void bindGoDetails()
    {
        try
        {
            string Query = "select SlotDate,SlotTime,count(ss.ST_App_No ) as count,CONVERT(varchar(10),SlotDate,103) as date,criteriaCode ,(select Mastervalue from CO_MasterValues where MasterCode=criteriaCode) as criteriavalue from ST_StudentSession ss,ST_DaySlot st,ST_RankListSlot sr,applyn a where a.app_no=ss.ST_App_No and a.batch_year =st.BatchYear and st.ST_DaySlotPK =sr.ST_DaySlotFk and sr.ST_RankListPk =ss.ST_RankListFk and sr.streamCode ='" + ddlStream.SelectedValue + "' and a.college_code ='" + ddlCollege.SelectedValue + "' and a.batch_year ='" + ddlbatch.SelectedValue + "'  and a.courseID ='" + ddlcourse.SelectedValue + "'  group by SlotDate,SlotTime,criteriaCode order by SlotDate ";

            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gridDaySlots.DataSource = ds;
                gridDaySlots.DataBind();
                gridDaySlots.Visible = true;
            }
        }
        catch
        {

        }

    }

    protected void btnsmssend_Click(object sender, EventArgs e)
    {
        try
        {
            if (gridDaySlots.Rows.Count > 0)
            {
                for (int intGrid = 0; intGrid < gridDaySlots.Rows.Count; intGrid++)
                {
                    if ((gridDaySlots.Rows[intGrid].FindControl("chkSel") as CheckBox).Checked == true)
                    {
                        DateTime date = Convert.ToDateTime((gridDaySlots.Rows[intGrid].FindControl("hdnDate") as HiddenField).Value);
                        string TimeSlot = (gridDaySlots.Rows[intGrid].FindControl("hdnSlotPk") as HiddenField).Value;
                        string Category = (gridDaySlots.Rows[intGrid].FindControl("hdnCategory") as HiddenField).Value;

                        string Query = "select app_formno,Student_Mobile,stud_name,sex from ST_StudentSession ss,ST_DaySlot st,ST_RankListSlot sr,applyn a where a.app_no=ss.ST_App_No and a.batch_year =st.BatchYear and st.ST_DaySlotPK =sr.ST_DaySlotFk and sr.ST_RankListPk =ss.ST_RankListFk and sr.streamCode ='" + ddlStream.SelectedValue + "' and a.college_code ='" + ddlCollege.SelectedValue + "' and a.batch_year ='" + ddlbatch.SelectedValue + "' and a.courseID ='" + ddlcourse.SelectedValue + "' and st.SlotDate='" + date.ToString("MM/dd/yyyy") + "' and st.SlotTime ='" + TimeSlot + "' and sr.criteriaCode ='" + Category + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(Query, "Text");
                        string ssr = d2.GetFunction("select SMS_User_ID from Track_Value where college_code='" + ddlCollege.SelectedValue + "'");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int introw = 0; introw < ds.Tables[0].Rows.Count; introw++)
                            {
                                string studName = Convert.ToString(ds.Tables[0].Rows[introw]["stud_name"]);
                                string studentMobile = Convert.ToString(ds.Tables[0].Rows[introw]["Student_Mobile"]);
                                string Applicationo = Convert.ToString(ds.Tables[0].Rows[introw]["app_formno"]);
                                string Gender = Convert.ToString(ds.Tables[0].Rows[introw]["sex"]);
                                if (Gender == "0" || Gender == "False")
                                {
                                    Gender = "Mr.";
                                }
                                else
                                {
                                    Gender = "Ms.";

                                }
                                string Msg = " Dear " + Gender + " " + studName + ", You have been shortlisted for counselling. Kindly check SASTRA University website for further information.";
                                if (ssr.Trim() != "" && ssr.Trim() != "0")
                                {
                                    int nofosmssend = d2.send_sms(ssr, ddlCollege.SelectedValue, UserCode, studentMobile, Msg, "0");
                                }

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
}
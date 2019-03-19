using System;
using System.Data;
using InsproDataAccess;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections.Generic;
using System.Web.UI;
using System.Collections;

public partial class AdmissionMod_CBSCRegistration : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    DAccess2 d2 = new DAccess2();
    Hashtable ht = new Hashtable();
    Dictionary<string, string> dicDbCol = new Dictionary<string, string>();
    Dictionary<string, string> dicDays = new Dictionary<string, string>();
    Dictionary<string, string> class_tt_dic = new Dictionary<string, string>();
    Dictionary<string, string> class_tt_det_dic = new Dictionary<string, string>();
    Dictionary<string, string> multiple_dic = new Dictionary<string, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            errorspan.Visible = false;
            Button1.Visible = true;
            ViewState["CollegeCode"] = null;
            ViewState["AppNo"] = null;
            txt_date.Attributes.Add("readyonly", "readonly");
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbltime.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        errorspan.Attributes.Add("style", "Color:Red;");
    }
    protected void tmr_Click(object sender, EventArgs e)
    {
        try
        {
            lbltime.Text = System.DateTime.Now.ToString("HH:mm:ss");
        }
        catch
        {

        }
    }
    protected void btn_submit_OnClick(object sender, EventArgs e)
    {
        try
        {
            clearDetails();
            ht.Clear();
            string applicationno = txt_applicationno.Text;
            string application = applicationno.Replace("'", "''");
            //  string dob = ddlmonth.SelectedItem.Value + "/" + ddldate.SelectedItem.Value + "/" + ddlyear.SelectedItem.Value;
            string[] dob = txt_Password.Text.Split('/');

            //string dateofbirth = dob[1] + "/" + dob[0] + "/" + dob[2];
            string checkCode = " select app_no ,isnull(ISOTP,'0') as OTP,student_Mobile,college_code from applyn where ISNULL(IsConfirm,0)=1 and app_formno='" + application + "'";
            //and DATEDIFF(MINUTE,ISNULL(LogDateTime,''),GETDATE ())>0
            //ht.Add("@AppNo", application);
            //ht.Add("@date", dateofbirth);
            DataSet dnew = d2.select_method_wo_parameter(checkCode, "Text");
            DataTable dtOTP = dnew.Tables[0];
            string app_no = string.Empty;
            if (dtOTP.Rows.Count == 0)
            {
                errorspan.Visible = true;
                errorspan.InnerHtml = "Invalid Application Number or Date of Birth or Please Try Later";
            }
            else if (dtOTP.Rows.Count > 0)
            {
                errorspan.Visible = false;
                string OTP = Convert.ToString(dtOTP.Rows[0]["OTP"]);
                string MobileNo = Convert.ToString(dtOTP.Rows[0]["student_Mobile"]);
                string CollegeCode = Convert.ToString(dtOTP.Rows[0]["college_code"]);
                app_no = Convert.ToString(dtOTP.Rows[0]["app_no"]);
                ViewState["CollegeCode"] = CollegeCode.ToString();
                ViewState["AppNo"] = app_no.ToString();
                //if (OTP.Trim() != "0" && OTP.Trim() != "False")
                //{
                if (checkLoginTime(app_no))
                {
                    errorspan.Visible = false;
                    showTimeTable(app_no);
                }
                else
                {
                    errorspan.Visible = true;
                    errorspan.InnerHtml = "Login restricted";
                }
                //}
                //else
                //{
                //    submitdiv.Visible = true;
                //    Button1.Visible = false;
                //    if (MobileNo.Trim() != "0" || MobileNo.Trim() != "")
                //    {
                //        txt_Mobile.Text = MobileNo.ToString();
                //    }
                //}
            }

        }
        catch
        {
            errorspan.Visible = true;
            errorspan.InnerHtml = "Please try later";
        }
    }

    protected void btnOtp_Click(object sender, EventArgs e)
    {
        try
        {
            string Number = RandomFunction();
            string Msg = "Your OTP for CBCS Registration : " + Number + "";
            string MobileNo = txt_Mobile.Text.ToString();
            string datetime = DateTime.Now.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");
            string Query = "update applyn set OTPNumber='" + Number + "' , Student_Mobile ='" + MobileNo + "',OTPDateTime='" + datetime + "' where app_no ='" + Convert.ToString(ViewState["AppNo"]) + "'";
            int upd = d2.update_method_wo_parameter(Query, "Text");
            bool check = SendSms("30", Convert.ToString(ViewState["CollegeCode"]), Msg, MobileNo);
            if (check == true)
            {
                errorspan.Visible = true;
                errorspan.Attributes.Add("style", "Color:Green;");
                errorspan.InnerHtml = "OTP Number Sent your register mobile number.";
            }
            else
            {
                errorspan.Visible = true;
                errorspan.InnerHtml = "Please Try Later";
            }
        }
        catch
        {

        }
    }

    public string RandomFunction()
    {
        string Ran = string.Empty;
        Random rnd = new Random();
        int month = rnd.Next(1, 9999999);
        Ran = Convert.ToString(month);
        return Ran;
    }
    private bool SendSms(string userCode, string collegeCode, string message, string Mobile_no)
    {
        bool checkflage = false;
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
                int sec = d2.send_sms(user_id.Trim(), collegeCode, userCode, Mobile_no, message, "1");
                if (sec > 0)
                {
                    checkflage = true;
                }

            }


        }
        catch
        {
            checkflage = false;
        }
        return checkflage;
    }

    private void showTimeTable(string appNo)
    {
        try
        {
            string selQ = "SELECT R.Stud_Name,A.app_formno, R.Roll_No,r.Reg_No,R.Roll_Admit, R.Batch_Year, R.degree_code,(C.Course_Name+' '+DT.Dept_Name) AS BRANCH,r.Current_Semester,r.college_code,isnull(r.Sections,'') as Sections FROM applyn A,Registration R,Degree D, Department DT, Course C WHERE R.App_No=A.app_no AND R.degree_code=D.Degree_Code AND D.Dept_Code=DT.Dept_Code AND D.Course_Id =C.Course_Id AND R.App_No='" + appNo + "'";
            DataTable dtStudDet = dirAcc.selectDataTable(selQ);
            if (dtStudDet.Rows.Count > 0)
            {
                string studName = Convert.ToString(dtStudDet.Rows[0]["Stud_Name"]);
                string appFormNo = Convert.ToString(dtStudDet.Rows[0]["app_formno"]);
                string regNo = Convert.ToString(dtStudDet.Rows[0]["Reg_No"]);
                string branch = Convert.ToString(dtStudDet.Rows[0]["BRANCH"]);
                string degCode = Convert.ToString(dtStudDet.Rows[0]["degree_code"]);
                string batch = Convert.ToString(dtStudDet.Rows[0]["Batch_Year"]);
                string colCode = Convert.ToString(dtStudDet.Rows[0]["college_code"]);
                string curSem = Convert.ToString(dtStudDet.Rows[0]["Current_Semester"]);
                string section = Convert.ToString(dtStudDet.Rows[0]["Sections"]).Trim();

                lblStudName.Text = studName;
                lblAppFormNo.Text = regNo;
                lblAppNo.Text = appNo;
                lblBranchDisp.Text = branch;
                lblBranch.Text = degCode;
                lblBatch.Text = batch;
                lblCollegeCode.Text = colCode;
                lblSem.Text = curSem;
                lblsection.Text = section;
                // string selTTSec = "  and ct.TT_sec='" + section + "'";
                string selTTSec = section;
                if (string.IsNullOrEmpty(section))
                {
                    selTTSec = string.Empty;
                }

                BindClassTTNew(batch, degCode, colCode, curSem, selTTSec);

                bool showElective = checkElectiveTime(appNo);
                if (showElective)
                {
                    Label lblTTPk = (Label)gridFnl.Rows[0].FindControl("lblTTPk");
                    bindElectiveSubjects(lblTTPk.Text, batch, degCode, colCode, curSem, selTTSec);
                    divElective.Visible = true;
                }
                else
                {
                    divElective.Visible = false;
                }
                string datetime = DateTime.Now.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");
                string Query = "update applyn set LogDateTime='" + datetime + "' where app_no ='" + appNo + "'";
                int upd = d2.update_method_wo_parameter(Query, "Text");
                ttSelectionDiv.Visible = true;
                popupEntryChk.Visible = false;
            }
            else
            {
                errorspan.Visible = true;
                errorspan.InnerHtml = "Invalid Application Number or Date of Birth";
            }
        }
        catch { }
    }
    private void clearDetails()
    {
        ttSelectionDiv.Visible = false;

        lblStudName.Text = string.Empty;
        lblAppFormNo.Text = string.Empty;
        lblAppNo.Text = string.Empty;
        lblBranchDisp.Text = string.Empty;
        lblBranch.Text = string.Empty;
        lblBatch.Text = string.Empty;
        lblCollegeCode.Text = string.Empty;
        lblSem.Text = string.Empty;
    }
    protected void imgLogout_OnClick(object sender, EventArgs e)
    {

        Response.Redirect("CBSCRegistration.aspx", false);

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
    private void bindColor(GridView gridClassTT)
    {
        try
        {
            if (gridClassTT.Rows.Count > 0)
            {
                for (int ro = 0; ro < gridClassTT.Rows.Count; ro++)
                {
                    if (ro == 0)
                    {
                        gridClassTT.Rows[ro].Font.Bold = true;
                        gridClassTT.Rows[ro].Font.Name = "Book Antiqua";
                        //gridClassTT.Rows[ro].Font.Size = FontUnit.Medium;
                        gridClassTT.Rows[ro].HorizontalAlign = HorizontalAlign.Center;
                        gridClassTT.Rows[ro].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    }
                    else
                    {
                        gridClassTT.Rows[ro].Cells[0].Font.Bold = true;
                        gridClassTT.Rows[ro].Cells[0].Font.Name = "Book Antiqua";
                        //gridClassTT.Rows[ro].Cells[0].Font.Size = FontUnit.Medium;
                        gridClassTT.Rows[ro].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        gridClassTT.Rows[ro].Cells[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    }
                }
            }
        }
        catch { }
    }
    private string getColor(int index)
    {
        List<string> clrList = NewStringColors();
        return clrList[index];
    }
    private List<string> NewStringColors()
    {
        List<string> clrList = new List<string>();
        clrList.Add("#d5d8dc");
        clrList.Add("#edbb99");
        clrList.Add("#76d7c4");
        clrList.Add("#e6b0aa");
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
    //Bind Time Table 
    protected void gridFnl_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    string batchYr = lblBatch.Text.Trim();
        //    string collegeCode = lblCollegeCode.Text.Trim();
        //    string curSem = lblSem.Text.Trim();
        //    string degCode = lblBranch.Text.Trim();

        //    GridView gridClassTT = (GridView)e.Row.FindControl("grdClass_TT");
        //    GridView gridClassDetTT = (GridView)e.Row.FindControl("grdClassDet_TT");
        //    Label lblTTSec = (Label)e.Row.FindControl("lblTTSec");

        //    getbindClassTT(batchYr, degCode, collegeCode, curSem, lblTTSec.Text, gridClassTT, gridClassDetTT);
        //    //getbindClassTT("2014", "54", "13", "6", lblTTSec.Text, gridClassTT, gridClassDetTT);

        //}
    }
    private void getbindClassTT(string batchYr, string degCode, string collegeCode, string curSem, string section, GridView gridClassTT, GridView gridClassDetTT)
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
                    string GetPeriod = "select Period1,Convert(varchar(5),start_time,108) as start_time,Convert(varchar(5),end_time,108) as end_time from BellSchedule  where Degree_Code='" + degCode + "' and batch_year='" + batchYr + "' and semester='" + curSem + "' order by start_time,end_time";
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
                            gridClassTT.Visible = true;
                            gridClassTT.DataSource = dtStfTT;
                            gridClassTT.DataBind();
                            getbindGrdValues(SchOrd, noofDays, dtStfTT, batchYr, degCode, collegeCode, curSem, section, gridClassTT, gridClassDetTT);
                            bindColor(gridClassTT);
                        }
                    }
                }
            }
        }
        catch { }
    }
    private void getbindGrdValues(string SchOrder, int NoofDays, DataTable myDataTable, string batchYr, string degCode, string collegeCode, string curSem, string section, GridView gridClassTT, GridView gridClassDetTT)
    {
        try
        {
            if (dicDbCol.Count == 0)
            {
                dicDbCol.Add("SUBJECT CODE", "subject_code");
                dicDbCol.Add("SUBJECT NAME", "subject_name");
                dicDbCol.Add("STAFF CODE", "TT_staffcode");
                dicDbCol.Add("STAFF NAME", "staff_name");
                dicDbCol.Add("ROOM NAME", "Room_Name");
            }

            string SelDayOrd = "";
            if (SchOrder.Trim() == "1")
                SelDayOrd = " Select TT_Day_DayorderPK,Daydiscription from TT_Day_Dayorder where Daytype='0'";
            else if (SchOrder.Trim() == "0")
                SelDayOrd = " Select TT_Day_DayorderPK,Daydiscription from TT_Day_Dayorder where Daytype='1'";
            else
            {
                return;
            }
            SelDayOrd = SelDayOrd + " select distinct TT_subno,TT_staffcode,TT_Hour,TT_Day,TT_Room,s.subject_name,s.subject_code,SM.staff_name,R.Room_Name from TT_ClassTimeTable T,TT_ClassTimeTabledet TT,Subject S,StaffMaster SM,Room_detail R Where T.TT_ClassPK=TT.TT_ClassFK and S.subject_no=TT_subno and SM.staff_code=TT_staffCode and TT_room=R.RoomPk and TT_degCode='" + degCode + "' and TT_batchyear='" + batchYr + "' and TT_sem='" + curSem + "' and TT_ColCode='" + collegeCode + "'";

            SelDayOrd = SelDayOrd + " and TT_Sec='" + section + "'";

            SelDayOrd = SelDayOrd + " order by TT_Day,TT_Hour";
            SelDayOrd = SelDayOrd + " select distinct TT_staffcode,s.subject_name,s.subject_code,SM.staff_name from TT_ClassTimeTable T,TT_ClassTimeTabledet TT,Subject S,StaffMaster SM,Room_detail R Where T.TT_ClassPK=TT.TT_ClassFK and S.subject_no=TT_subno and SM.staff_code=TT_staffCode and TT_room=R.RoomPk and TT_degCode='" + degCode + "' and TT_batchyear='" + batchYr + "' and TT_sem='" + curSem + "' and TT_ColCode='" + collegeCode + "'";

            SelDayOrd = SelDayOrd + " and TT_Sec='" + section + "'";

            DataSet dsDayOrd = new DataSet();
            DataView dvDayOrd = new DataView();
            DataView dvVal = new DataView();
            dsDayOrd = d2.select_method_wo_parameter(SelDayOrd, "Text");
            if (dsDayOrd.Tables.Count > 0 && dsDayOrd.Tables[0].Rows.Count > 0)
            {
                bool IsDayExist = true;
                int headerColumnCount = gridClassTT.HeaderRow.Cells.Count;
                int index = 0;
                for (int ro = 1; ro < gridClassTT.Rows.Count; ro++)
                {
                    dsDayOrd.Tables[0].DefaultView.RowFilter = " Daydiscription='" + Convert.ToString(gridClassTT.Rows[ro].Cells[0].Text) + "'";
                    dvDayOrd = dsDayOrd.Tables[0].DefaultView;
                    if (dvDayOrd.Count > 0)
                    {
                        string DayFK = Convert.ToString(dvDayOrd[0]["TT_Day_DayorderPK"]);
                        if (!String.IsNullOrEmpty(DayFK.Trim()) && DayFK.Trim() != "0")
                        {
                            for (int co = 1; co < headerColumnCount; co++)
                            {
                                string ColHour = Convert.ToString(gridClassTT.Rows[0].Cells[co].Text);
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
                    gridClassTT.Visible = true;
                    gridClassTT.DataSource = myDataTable;
                    gridClassTT.DataBind();
                    getbindDetGrd(SchOrder, NoofDays, dsDayOrd, gridClassDetTT);
                }
            }
        }
        catch { }
    }
    private void getbindDetGrd(string mySchOrd, int noDays, DataSet dsDetVal, GridView gridClassDetTT)
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
                    gridClassDetTT.Visible = true;
                    gridClassDetTT.DataSource = dtDet;
                    gridClassDetTT.DataBind();
                }
                else
                {
                    gridClassDetTT.Visible = false;
                }
            }
            else
            {
                gridClassDetTT.Visible = false;
            }
        }
        catch { }
    }
    private void BindClassTTNew(string batchYr, string degCode, string collegeCode, string curSem, string selSec)
    {
        //DataTable dtTotalSec = dirAcc.selectDataTable("select distinct TT_ClassPK,TT_name,TT_sec,(isnull(sd.studentCount,0)-(select COUNT(r.app_no) from Registration r where r.Batch_Year='" + batchYr + "' and r.degree_code='" + degCode + "' and r.Sections=TT_sec) ) studentCount  from TT_classTimetable ct, TT_ClassTimetableDet cdt,sectionDetails sd where ct.TT_ClassPK = cdt.TT_ClassFk  and sd.batchYear=TT_batchyear and sd.degreeCode=TT_degCode  and sd.sectionName=TT_sec  and TT_degCode='" + degCode + "' and TT_batchyear='" + batchYr + "' and TT_colCode='" + collegeCode + "' and TT_sem='" + curSem + "' and TT_lastRec='1'  " + selSec + (string.IsNullOrEmpty(selSec) ? "  and (isnull(sd.studentCount,0)-(select COUNT(r.app_no) from Registration r where r.Batch_Year='" + batchYr + "' and r.degree_code='" + degCode + "' and r.Sections=TT_sec) )>0 " : string.Empty));

        string SelectQury = " select COUNT(ISNULL(r.sections,0)) as Total ,sections,degree_code,Batch_Year,college_code from Registration r where r.Batch_Year =" + batchYr + " and degree_code ='" + degCode + "' and r.college_code ='" + collegeCode + "' and r.Sections<>'' and r.Sections is not null and Current_Semester ='" + curSem + "' ";
        if (selSec.Trim() != "")
        {
            SelectQury += " and sections='" + selSec + "'";
        }
        SelectQury += " group by Sections,degree_code,Batch_Year,college_code ";
        SelectQury += " select degreeCode,sectionName,studentCount,batchYear from sectionDetails where  batchyear ='" + batchYr + "' and Degreecode='" + degCode + "' ";
        if (selSec.Trim() != "")
        {
            SelectQury += " and sectionName='" + selSec + "'";
        }

        DataSet dsshow = dirAcc.selectDataSet(SelectQury);
        DataTable dtTotalSec = new DataTable();
        dtTotalSec = dsshow.Tables[1];
        if (dtTotalSec.Rows.Count > 0 && dsshow.Tables.Count > 0)
        {
            DataTable dtTT = new DataTable();
            dtTT.Columns.Add("TTName");
            dtTT.Columns.Add("TTPk");
            dtTT.Columns.Add("TTSec");
            dtTT.Columns.Add("TTMaxRem");
            dtTT.Columns.Add("degree");
            dtTT.Columns.Add("batch");
            dtTT.Columns.Add("collegecode");
            dtTT.Columns.Add("sem");
            foreach (DataRow drTotSec in dtTotalSec.Rows)
            {
                DataRow drTT = dtTT.NewRow();
                drTT["TTName"] = "Time Table " + drTotSec["sectionName"];
                drTT["TTPk"] = Convert.ToInt32(drTotSec["studentCount"]);
                drTT["TTSec"] = drTotSec["sectionName"];
                int Count = Convert.ToInt32(drTotSec["studentCount"]);
                int Studcount = 0;
                dsshow.Tables[0].DefaultView.RowFilter = "sections='" + drTotSec["sectionName"] + "' and degree_code='" + drTotSec["degreeCode"] + "' and Batch_Year='" + drTotSec["batchYear"] + "'";
                DataView dvshow = dsshow.Tables[0].DefaultView;
                if (dvshow.Count > 0)
                {
                    int.TryParse(Convert.ToString(dvshow[0]["Total"]), out Studcount);
                }
                int Remaining = Count - Studcount;
                if (Remaining < 0)
                {
                    Remaining = 0;
                }
                drTT["TTMaxRem"] = (Remaining);
                drTT["degree"] = (drTotSec["degreeCode"]);
                drTT["batch"] = (drTotSec["batchYear"]);
                drTT["collegecode"] = (collegeCode);
                drTT["sem"] = (curSem);

                dtTT.Rows.Add(drTT);
            }

            gridFnl.Visible = true;
            gridFnl.DataSource = dtTT;
            gridFnl.DataBind();
            if (gridFnl.Rows.Count > 0)
            {
                if (checkTimeTableTime(lblAppNo.Text.Trim()) && selSec.Trim() == "")
                {
                    gridFnl.Columns[6].Visible = true;
                }
                else
                {
                    gridFnl.Columns[6].Visible = false;
                    // btnSave.Visible = true;
                }
            }
        }
    }
    //Save selected Time Table
    protected void btnSaveTT_OnClick(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            Button btnSaveTT = (Button)sender;
            string rowIndxS = btnSaveTT.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int selIndx = Convert.ToInt32(rowIndxS) - 2;

            Label lblTTSec = (Label)gridFnl.Rows[selIndx].FindControl("lblTTSec");
            Label lblTTPk = (Label)gridFnl.Rows[selIndx].FindControl("lblTTPk");

            Label lblTotal = (Label)gridFnl.Rows[selIndx].FindControl("TTSec");

            Label lblbatch = (Label)gridFnl.Rows[selIndx].FindControl("lblbatch");
            Label lblDegree = (Label)gridFnl.Rows[selIndx].FindControl("lbldegree");

            Label lblsem = (Label)gridFnl.Rows[selIndx].FindControl("lblsem");
            Label lblcollege = (Label)gridFnl.Rows[selIndx].FindControl("lblcollege");

            string appNo = lblAppNo.Text.Trim();

            string TTClassPk = lblTTPk.Text.Trim();

            ht.Add("@batchYear", lblbatch.Text);
            ht.Add("@degreeCode", lblDegree.Text);
            ht.Add("@collegeCode", lblcollege.Text);
            ht.Add("@semester", lblsem.Text);
            ht.Add("@section", lblTTSec.Text);

            string SelectQury = " select COUNT(ISNULL(r.sections,0)) as Total ,sections,degree_code,Batch_Year,college_code from Registration r where r.Batch_Year =" + lblbatch.Text + " and degree_code ='" + lblDegree.Text + "' and r.college_code ='" + lblcollege.Text + "' and r.Sections<>'' and r.Sections is not null and Current_Semester ='" + lblsem.Text + "' ";
            if (lblTTSec.Text.Trim() != "")
            {
                SelectQury += " and Sections='" + lblTTSec.Text + "'";
            }
            SelectQury += " group by Sections,degree_code,Batch_Year,college_code ";
            DataSet dbnew = d2.select_method("SectionCountCheck", ht, "sp");
            int Tot = 0;
            if (dbnew.Tables.Count > 0 && dbnew.Tables[0].Rows.Count > 0)
            {
                int.TryParse(Convert.ToString(dbnew.Tables[0].Rows[0]["Total"]), out Tot);
            }
            // dirAcc.selectScalarInt(SelectQury);
            if (Convert.ToInt32(TTClassPk) > Tot)
            {
                string updTTClass = "update registration set Sections='" + lblTTSec.Text.Trim() + "' where app_no='" + appNo + "' and isnull(Sections,'')=''";
                int upd = dirAcc.updateData(updTTClass);
                if (upd > 0)
                {
                    //DataTable dtStudDet = dirAcc.selectDataTable("select roll_no,Current_Semester from registration where app_No='" + appNo + "'");
                    //string rollNo = string.Empty;
                    //string curSem = string.Empty;

                    //if (dtStudDet.Rows.Count > 0)
                    //{
                    //    rollNo = Convert.ToString(dtStudDet.Rows[0]["roll_no"]);
                    //    curSem = Convert.ToString(dtStudDet.Rows[0]["Current_Semester"]);

                    //    string selQ = "select distinct TT_staffcode,TT_subno,s.subType_no,isnull(ss.Lab,'0') as Lab  from TT_classTimetable ct, TT_ClassTimetableDet cdt, sub_sem ss,subject s,staffmaster sm where ct.TT_ClassPK = cdt.TT_ClassFk and TT_staffcode=sm.staff_code and ss.subType_no=s.subType_no and s.subject_no= TT_subno and ISNULL(ss.ElectivePap,'0')='0' and ct.TT_ClassPK=" + TTClassPk + " ";
                    //    DataTable dtSubjDet = dirAcc.selectDataTable(selQ);
                    //    if (dtSubjDet.Rows.Count > 0)
                    //    {
                    //        foreach (DataRow drSubje in dtSubjDet.Rows)
                    //        {
                    //            string TT_staffcode = Convert.ToString(drSubje["TT_staffcode"]);
                    //            string TT_subno = Convert.ToString(drSubje["TT_subno"]);
                    //            string subType_no = Convert.ToString(drSubje["subType_no"]);
                    //            string isLab = Convert.ToString(drSubje["Lab"]).ToLower();

                    //            string insUpdQ = "If exists (select id from subjectChooser where roll_no='" + rollNo + "' and semester='" + curSem + "' and subject_no='" + TT_subno + "') update subjectChooser set subtype_no='" + subType_no + "' , staffcode='" + TT_staffcode + "'  where roll_no='" + rollNo + "' and semester='" + curSem + "' and subject_no='" + TT_subno + "'  else insert into subjectChooser (semester,roll_no,subject_no,subtype_no,staffcode) values ('" + curSem + "','" + rollNo + "','" + TT_subno + "','" + subType_no + "','" + TT_staffcode + "')";

                    //            if (isLab == "true")
                    //            {
                    //                insUpdQ = "If exists (select id from subjectChooser where roll_no='" + rollNo + "' and semester='" + curSem + "' and subject_no='" + TT_subno + "' and staffcode='" + TT_staffcode + "' ) update subjectChooser set subtype_no='" + subType_no + "' , staffcode='" + TT_staffcode + "'  where roll_no='" + rollNo + "' and semester='" + curSem + "' and subject_no='" + TT_subno + "' and staffcode='" + TT_staffcode + "'  else insert into subjectChooser (semester,roll_no,subject_no,subtype_no,staffcode) values ('" + curSem + "','" + rollNo + "','" + TT_subno + "','" + subType_no + "','" + TT_staffcode + "')";
                    //            }

                    //            dirAcc.updateData(insUpdQ);
                    //        }
                    //    }
                    //}
                    int count = dirAcc.selectScalarInt("select ElectiveCount from Ndegree where batch_year ='" + lblBatch.Text + "' and Degree_code ='" + lblBranch.Text + "'");
                    if (count > 0)
                    {
                        btnSave.Visible = true;
                        showTimeTable(appNo);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Section Selected Choose Electives')", true);
                    }
                    else
                    {
                        btnSave.Visible = false;
                        showTimeTable(appNo);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Section Selected Sucessfully')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Already selected')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Selected Section Already Filled')", true);
            }

        }
        catch { ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please try later')", true); }
    }
    //Eelective selection
    protected void btnViewTT_OnClick(object sender, EventArgs e)
    {
        try
        {
            Button btnViewTT = (Button)sender;
            string rowIndxS = btnViewTT.UniqueID.ToString().Split('$')[1].Replace("ctl", string.Empty);
            int selIndx = Convert.ToInt32(rowIndxS) - 2;

            Label lblTTPk = (Label)gridFnl.Rows[selIndx].FindControl("lblTTPk");
            Label lblTTSec = (Label)gridFnl.Rows[selIndx].FindControl("lblTTSec");

            divViewTT.Visible = true;

            string appNo = lblAppNo.Text.Trim();
            showTimeTableSingle(appNo, lblTTSec.Text.Trim());

        }
        catch { }
    }
    protected void imgViewTT_OnClick(object sender, EventArgs e)
    {
        divViewTT.Visible = false;
    }
    private void bindElectiveSubjects(string electivePk, string batch, string degree, string collegeCode, string sem, string section)
    {
        try
        {
            // btnSave.Visible = false;
            gridElective.Visible = false;

            DataTable dtElect = new DataTable();
            dtElect.Columns.Add("subType_no");
            dtElect.Columns.Add("pp");
            dtElect.Columns.Add("subject_type");
            dtElect.Columns.Add("TT_subno");
            dtElect.Columns.Add("subject_code");
            dtElect.Columns.Add("subject_name");
            dtElect.Columns.Add("TT_staffcode");
            dtElect.Columns.Add("staff_name");
            dtElect.Columns.Add("StudCount");
            string appNo = lblAppNo.Text.Trim();
            string semester = lblSem.Text.Trim();
            string colCode = lblCollegeCode.Text;
            DataRow dr;
            string Query = string.Empty;

            Hashtable hash = new Hashtable();
            ArrayList addArray = new ArrayList();
            Query = "  select s.practicalPair,sc.subject_no,sc.staffcode from subjectChooser sc,subject s,Registration r,sub_sem ss where ss.subtype_no=s.subtype_no and ss.subtype_no=sc.subtype_no and sc.subject_no =s.subject_no and ElectivePap ='1' and r.Roll_No =sc.roll_no and sc.semester ='" + semester + "' and r.App_No ='" + appNo + "'";
            DataSet dsadd = dirAcc.selectDataSet(Query);
            if (dsadd.Tables[0].Rows.Count > 0)
            {
                for (int inDs = 0; inDs < dsadd.Tables[0].Rows.Count; inDs++)
                {
                    if (!hash.ContainsKey(Convert.ToString(dsadd.Tables[0].Rows[inDs]["subject_no"])))
                    {
                        hash.Add(Convert.ToString(dsadd.Tables[0].Rows[inDs]["subject_no"]) + "," + Convert.ToString(dsadd.Tables[0].Rows[inDs]["staffcode"]), Convert.ToString(dsadd.Tables[0].Rows[inDs]["practicalPair"]));
                    }
                    if (!addArray.Contains(Convert.ToString(dsadd.Tables[0].Rows[inDs]["practicalPair"])))
                    {
                        addArray.Add(Convert.ToString(dsadd.Tables[0].Rows[inDs]["practicalPair"]));
                    }
                }
            }

            int Elective = dirAcc.selectScalarInt("  select ISNULL(ElectiveSelection,0) as ElectiveSelection  from Ndegree where Degree_code ='" + degree + "' and batch_year ='" + batch + "' and college_code ='" + colCode + "' ");
            //dtElect = dirAcc.selectDataTable("select distinct TT_ClassFk,TT_staffcode,TT_subno,sm.staff_name,s.subject_name,s.subject_code,s.subType_no,isnull(s.practicalPair,0) as pp,ss.subject_type  from TT_classTimetable ct, TT_ClassTimetableDet cdt, sub_sem ss,subject s,staffmaster sm where ct.TT_ClassPK = cdt.TT_ClassFk and TT_staffcode=sm.staff_code and ss.subType_no=s.subType_no and s.subject_no= TT_subno and ISNULL(ss.ElectivePap,'0')='1' and TT_ClassFk='" + electivePk + "' order by subType_no,TT_subno ");
            if (Elective == 0)
            {
                Query = " select distinct '' TT_ClassFk ,e.staffCode as TT_staffcode,s.subject_no as TT_subno,sm.staff_name,s.subject_name,s.subject_code,s.subType_no,isnull(s.practicalPair,0) as pp,ss.subject_type,e.studentCount as StudCount from electiveSubjectDetails e,syllabus_master sy,sub_sem ss,subject s,staffmaster sm where sy.syll_code =ss.syll_code and sy.syll_code =s.syll_code and ss.subType_no =s.subType_no and e.subjectNo =s.subject_no  and sm.staff_code =e.staffCode and sy.Batch_Year ='" + batch + "' and sy.degree_code ='" + degree + "' and sy.semester ='" + sem + "' and ISNULL(IsSectionWise,'0')='0' order by pp ";
                Query += "  select staffcode,sc.subject_no,sy.degree_code,sy.Batch_Year,sy.semester,COUNT(sc.roll_no) as Total from Registration r,subjectChooser sc,subject s,sub_sem ss,syllabus_master sy where sy.syll_code =s.syll_code and ss.syll_code =sy.syll_code and s.subType_no =ss.subType_no and s.subject_no =sc.subject_no and r.Roll_No=sc.roll_no and r.degree_code =sy.degree_code and r.Batch_Year =sy.Batch_Year and sy.Batch_Year ='" + batch + "' and sy.degree_code ='" + degree + "' and sy.semester ='" + sem + "'  group by staffcode,sc.subject_no,sy.degree_code,sy.Batch_Year,sy.semester";
            }
            if (Elective == 1)
            {
                Query = "select distinct '' TT_ClassFk ,e.staffCode as TT_staffcode,s.subject_no as TT_subno,sm.staff_name,s.subject_name,s.subject_code,s.subType_no,isnull(s.practicalPair,0) as pp,ss.subject_type,sum(e.studentCount) as StudCount from electiveSubjectDetails e,syllabus_master sy,sub_sem ss,subject s,staffmaster sm where sy.syll_code =ss.syll_code and sy.syll_code =s.syll_code and ss.subType_no =s.subType_no and e.subjectNo =s.subject_no and sm.staff_code =e.staffCode and sy.Batch_Year ='" + batch + "' and sy.degree_code ='" + degree + "' and sy.semester ='" + sem + "' and ISNULL(IsSectionWise,'0')='1'";
                if (section.Trim() != "")
                {
                    Query += " and e. SectionName ='" + section + "'";
                }
                Query += " group by e.staffCode,s.subject_no,sm.staff_name,s.subject_name,s.subject_code,s.subType_no,ss.subject_type,s.practicalPair order by pp";
                Query += "  select staffcode,sc.subject_no,sy.degree_code,sy.Batch_Year,sy.semester,COUNT(sc.roll_no) as Total from Registration r,subjectChooser sc,subject s,sub_sem ss,syllabus_master sy where sy.syll_code =s.syll_code and ss.syll_code =sy.syll_code and s.subType_no =ss.subType_no and s.subject_no =sc.subject_no and r.Roll_No=sc.roll_no and r.degree_code =sy.degree_code and r.Batch_Year =sy.Batch_Year and sy.Batch_Year ='" + batch + "' and sy.degree_code ='" + degree + "' and sy.semester ='" + sem + "'";
                if (section.Trim() != "")
                {
                    Query += " and r. Sections ='" + section + "'";
                }
                Query += " group by staffcode,sc.subject_no,sy.degree_code,sy.Batch_Year,sy.semester";
            }

            DataSet dsElective = dirAcc.selectDataSet(Query);
            if (dsElective.Tables.Count > 0)
            {
                for (int intdsElective = 0; intdsElective < dsElective.Tables[0].Rows.Count; intdsElective++)
                {
                    dr = dtElect.NewRow();
                    string PP = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["pp"]);
                    if (addArray.Contains(PP))
                    {
                        if (hash.Contains(Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["TT_subno"]) + "," + Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["TT_staffcode"])))
                        {
                            dr["subType_no"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["subType_no"]);
                            dr["pp"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["pp"]);
                            dr["subject_type"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["subject_type"]);
                            dr["TT_subno"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["TT_subno"]);
                            dr["subject_code"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["subject_code"]);
                            dr["subject_name"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["subject_name"]);
                            dr["TT_staffcode"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["TT_staffcode"]);
                            dr["staff_name"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["staff_name"]);

                            int ActACount = 0;
                            int SectCount = 0;
                            int RemainCount = 0;
                            int.TryParse(Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["StudCount"]), out ActACount);
                            dsElective.Tables[1].DefaultView.RowFilter = "staffcode='" + Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["TT_staffcode"]) + "' and subject_no='" + Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["TT_subno"]) + "'";
                            DataView dsEle = dsElective.Tables[1].DefaultView;
                            if (dsEle.Count > 0)
                            {
                                int.TryParse(Convert.ToString(dsEle[0]["Total"]), out SectCount);
                            }
                            RemainCount = ActACount - SectCount;
                            dr["StudCount"] = RemainCount;

                            dtElect.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        dr["subType_no"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["subType_no"]);
                        dr["pp"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["pp"]);
                        dr["subject_type"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["subject_type"]);
                        dr["TT_subno"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["TT_subno"]);
                        dr["subject_code"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["subject_code"]);
                        dr["subject_name"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["subject_name"]);
                        dr["TT_staffcode"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["TT_staffcode"]);
                        dr["staff_name"] = Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["staff_name"]);

                        int ActACount = 0;
                        int SectCount = 0;
                        int RemainCount = 0;
                        int.TryParse(Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["StudCount"]), out ActACount);
                        dsElective.Tables[1].DefaultView.RowFilter = "staffcode='" + Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["TT_staffcode"]) + "' and subject_no='" + Convert.ToString(dsElective.Tables[0].Rows[intdsElective]["TT_subno"]) + "'";
                        DataView dsEle = dsElective.Tables[1].DefaultView;
                        if (dsEle.Count > 0)
                        {
                            int.TryParse(Convert.ToString(dsEle[0]["Total"]), out SectCount);
                        }
                        RemainCount = ActACount - SectCount;
                        dr["StudCount"] = RemainCount;

                        dtElect.Rows.Add(dr);
                    }

                }

                if (dtElect.Rows.Count > 0)
                {
                    //btnSave.Visible = true;
                    gridElective.DataSource = dtElect;
                    gridElective.DataBind();
                    gridElective.Visible = true;
                }
            }

        }
        catch
        {
            btnSave.Visible = false;
            gridElective.Visible = false;
        }
    }
    protected void gridElective_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            ArrayList arrColor = new ArrayList();
            foreach (GridViewRow gRow in gridElective.Rows)
            {
                Label lblSubTypeNo = (Label)gRow.FindControl("lblSubTypeNo");
                Label lblPP = (Label)gRow.FindControl("lblPP");
                if (!arrColor.Contains(lblSubTypeNo.Text + "-" + lblPP.Text))
                {
                    arrColor.Add((lblSubTypeNo.Text + "-" + lblPP.Text));
                }
                gRow.BackColor = ColorTranslator.FromHtml(getColor(arrColor.Count - 1));

                Label lblRemain = (Label)gRow.FindControl("lblRemain");
                int rem = 0; int.TryParse(lblRemain.Text, out rem);
                if (rem <= 0)
                {
                    CheckBox chkSel = (CheckBox)gRow.FindControl("chkSel");
                    chkSel.Enabled = false;
                }
            }
            string rollNo = string.Empty;
            string curSem = string.Empty;
            string appNo = lblAppNo.Text.Trim();
            string semester = lblSem.Text.Trim();
            ArrayList AddArray = new ArrayList();
            DataTable dtStudDet = dirAcc.selectDataTable("select roll_no,Current_Semester from registration where app_No='" + appNo + "'");
            if (dtStudDet.Rows.Count > 0)
            {
                rollNo = Convert.ToString(dtStudDet.Rows[0]["roll_no"]);
                curSem = Convert.ToString(dtStudDet.Rows[0]["Current_Semester"]);
                string Query = "  select distinct s.practicalPair from subjectChooser sc,subject s,Registration r where sc.subject_no =s.subject_no and r.Roll_No =sc.roll_no and sc.semester ='" + semester + "' and r.App_No ='" + appNo + "'";
                DataSet dsadd = dirAcc.selectDataSet(Query);
                if (dsadd.Tables[0].Rows.Count > 0)
                {
                    for (int inDs = 0; inDs < dsadd.Tables[0].Rows.Count; inDs++)
                    {
                        AddArray.Add(Convert.ToString(dsadd.Tables[0].Rows[inDs]["practicalPair"]));
                    }

                }
                int ElectiveCount = 0;
                DataTable dtElectSaveCheck = dirAcc.selectDataTable("select  distinct subject_no,subtype_no,staffcode  from subjectChooser where roll_no='" + rollNo + "' and semester='" + curSem + "' ");
                if (dtElectSaveCheck.Rows.Count > 0)
                {

                    foreach (GridViewRow gRow in gridElective.Rows)
                    {
                        int index = gRow.RowIndex;
                        CheckBox chkSel = (CheckBox)gRow.FindControl("chkSel");
                        Label lblSubTypeNo = (Label)gRow.FindControl("lblSubTypeNo");
                        Label lblSubNo = (Label)gRow.FindControl("lblSubNo");
                        Label lblStaffCode = (Label)gRow.FindControl("lblStaffCode");
                        Label lblPP = (Label)gRow.FindControl("lblPP");

                        dtElectSaveCheck.DefaultView.RowFilter = "subject_no='" + lblSubNo.Text + "' and subtype_no='" + lblSubTypeNo.Text + "' and staffcode='" + lblStaffCode.Text + "'";
                        DataTable dtEleSav = dtElectSaveCheck.DefaultView.ToTable();

                        if (dtEleSav.Rows.Count > 0)
                        {
                            chkSel.Checked = true;
                            chkSel.Enabled = false;
                            ElectiveCount++;
                            // btnSave.Visible = false;
                            gridElective.Rows[index].Visible = true;
                        }
                        else
                        {
                            //if (AddArray.Contains(lblPP.Text))
                            //{
                            //    gridElective.Rows[index].Visible = false;
                            //}
                            //else
                            //{
                            //    gridElective.Rows[index].Visible = true;
                            //}
                        }
                    }

                    //else
                    //{
                    //    btnSave.Visible = true;
                    //}
                }
                int count = dirAcc.selectScalarInt("select ElectiveCount from Ndegree where batch_year ='" + lblBatch.Text + "' and Degree_code ='" + lblBranch.Text + "'");
                if (ElectiveCount == count)
                {
                    btnSave.Visible = false;
                }
                else if (ElectiveCount < count && lblsection.Text.Trim() != "")
                {
                    btnSave.Visible = true;
                }
            }

            try
            {
                for (int i = gridElective.Rows.Count - 1; i > 0; i--)
                {
                    GridViewRow row = gridElective.Rows[i];
                    GridViewRow previousRow = gridElective.Rows[i - 1];
                    for (int j = 2; j <= 4; j++)
                    {
                        bool validation = false;
                        switch (j)
                        {
                            case 2:
                                {
                                    Label lnlname = (Label)row.FindControl("lblSubTypeName");
                                    Label lnlname1 = (Label)previousRow.FindControl("lblSubTypeName");
                                    if (lnlname.Text == lnlname1.Text)
                                    {
                                        validation = true;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    Label lnlname = (Label)row.FindControl("lblSubCode");
                                    Label lnlname1 = (Label)previousRow.FindControl("lblSubCode");
                                    if (lnlname.Text == lnlname1.Text)
                                    {
                                        validation = true;
                                    }
                                }
                                break;
                            case 4:
                                {
                                    Label lnlname = (Label)row.FindControl("lblSubName");
                                    Label lnlname1 = (Label)previousRow.FindControl("lblSubName");
                                    if (lnlname.Text == lnlname1.Text)
                                    {
                                        validation = true;
                                    }
                                }
                                break;
                        }


                        if (validation)
                        {
                            if (previousRow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousRow.Cells[j].RowSpan = 2;
                                }
                                else
                                {
                                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
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
            btnSave.Visible = false;
        }
    }
    protected void btnSaveElective_Click(object sender, EventArgs e)
    {
        try
        {
            string rollNo = string.Empty;
            string curSem = string.Empty;
            string appNo = lblAppNo.Text.Trim();
            DataTable dtStudDet = dirAcc.selectDataTable("select roll_no,Current_Semester from registration where app_No='" + appNo + "'");
            if (dtStudDet.Rows.Count > 0)
            {
                rollNo = Convert.ToString(dtStudDet.Rows[0]["roll_no"]);
                curSem = Convert.ToString(dtStudDet.Rows[0]["Current_Semester"]);
                int getcode = dirAcc.selectScalarInt("select degree_code  from Registration where roll_no ='" + rollNo + "'");
                bool chkSelected = false;
                bool duplicateSubject = false;
                bool sameElectGroup = false;

                ArrayList arrElectiveGroup = new ArrayList();
                ArrayList arrMaxElectiveGroup = new ArrayList();
                Dictionary<string, string> dicSubStaff = new Dictionary<string, string>();
                foreach (GridViewRow gRow in gridElective.Rows)
                {
                    CheckBox chkSel = (CheckBox)gRow.FindControl("chkSel");
                    Label lblSubTypeNo = (Label)gRow.FindControl("lblSubTypeNo");
                    Label lblPP = (Label)gRow.FindControl("lblPP");
                    if (chkSel.Checked)
                    {

                        Label lblSubNo = (Label)gRow.FindControl("lblSubNo");
                        Label lblSubCode = (Label)gRow.FindControl("lblSubCode");
                        Label lblStaffCode = (Label)gRow.FindControl("lblStaffCode");

                        if (!dicSubStaff.ContainsKey(lblSubNo.Text))
                        {
                            dicSubStaff.Add(lblSubNo.Text, lblStaffCode.Text);
                        }
                        else
                        {
                            duplicateSubject = true;
                        }
                        if (!arrElectiveGroup.Contains((lblSubTypeNo.Text + "-" + lblPP.Text)))
                        {
                            arrElectiveGroup.Add((lblSubTypeNo.Text + "-" + lblPP.Text));
                        }
                        else
                        {
                            sameElectGroup = true;
                        }

                        chkSelected = true;
                    }

                    if (!arrMaxElectiveGroup.Contains((lblSubTypeNo.Text + "-" + lblPP.Text)))
                    {
                        arrMaxElectiveGroup.Add((lblSubTypeNo.Text + "-" + lblPP.Text));
                    }
                    //if (arrMaxElectiveGroup.Count > 2)
                    //{
                    //    count = 2;
                    //}
                    //else
                    //{
                    //    count = arrMaxElectiveGroup.Count;
                    //}
                }
                int count = dirAcc.selectScalarInt("select ElectiveCount from Ndegree where batch_year ='" + lblBatch.Text + "' and Degree_code ='" + lblBranch.Text + "'");
                bool SaveCheck = false;
                int Countvalue = 0;
                int Elective = dirAcc.selectScalarInt("  select ISNULL(ElectiveSelection,0) as ElectiveSelection  from Ndegree where Degree_code ='" + lblBranch.Text + "' and batch_year ='" + lblBatch.Text + "' and college_code ='" + lblCollegeCode.Text + "' ");
                if (chkSelected && !duplicateSubject && !sameElectGroup && (arrElectiveGroup.Count == count))
                {
                    foreach (GridViewRow gRow in gridElective.Rows)
                    {
                        CheckBox chkSel = (CheckBox)gRow.FindControl("chkSel");
                        if (chkSel.Checked)
                        {
                            Label lblSubNo = (Label)gRow.FindControl("lblSubNo");
                            Label lblSubCode = (Label)gRow.FindControl("lblSubCode");
                            Label lblStaffCode = (Label)gRow.FindControl("lblStaffCode");
                            Label lblSubTypeNo = (Label)gRow.FindControl("lblSubTypeNo");
                            Label lblPP = (Label)gRow.FindControl("lblPP");
                            int Tot = 0;
                            ht.Clear();
                            if (Elective == 0)
                            {
                                Tot = dirAcc.selectScalarInt(" select (studentCount-(select count(roll_no)as total from subjectChooser sc where sc.subject_no='" + lblSubNo.Text + "' and sc.staffCode ='" + lblStaffCode.Text + "')) as Total  from electiveSubjectDetails where ISNULL (IsSectionWise ,'0')='0' and (studentCount-(select count(roll_no)as total from subjectChooser sc where sc.subject_no='" + lblSubNo.Text + "' and sc.staffCode ='" + lblStaffCode.Text + "')) >0 and staffCode ='" + lblStaffCode.Text + "' and subjectNo ='" + lblSubNo.Text + "'");
                            }
                            else if (Elective == 1)
                            {
                                string SQl = " select (studentCount-(select count(sc.roll_no)as total from subjectChooser sc,Registration r where r.Roll_No =sc.roll_no ";
                                if (lblsection.Text.Trim() != "")
                                {
                                    SQl += " and r.Sections ='" + lblsection.Text + "'";
                                }
                                SQl += " and sc.subject_no='" + lblSubNo.Text + "' and sc.staffCode ='" + lblStaffCode.Text + "')) as Total  from electiveSubjectDetails where ISNULL (IsSectionWise ,'0')='1' and (studentCount-(select count(sc.roll_no)as total from subjectChooser sc,Registration r where r.Roll_No =sc.roll_no ";
                                if (lblsection.Text.Trim() != "")
                                {
                                    SQl += " and r.Sections ='" + lblsection.Text + "'";
                                }
                                SQl += " and sc.subject_no='" + lblSubNo.Text + "' and sc.staffCode ='" + lblStaffCode.Text + "')) >0 and staffCode ='" + lblStaffCode.Text + "' and subjectNo ='" + lblSubNo.Text + "'";
                                if (lblsection.Text.Trim() != "")
                                {
                                    SQl += " and SectionName='" + lblsection.Text + "'";
                                }

                                Tot = dirAcc.selectScalarInt(SQl);
                            }
                            //Update Elective subjects
                            if (Tot > 0)
                            {
                                ht.Add("@rollNo", rollNo);
                                ht.Add("@semester", curSem);
                                ht.Add("@subjectno", lblSubNo.Text);
                                ht.Add("@subjectType", lblSubTypeNo.Text);
                                ht.Add("@staffcode", lblStaffCode.Text);

                                string insUpdQ = "If exists (select id from subjectChooser where roll_no='" + rollNo + "' and semester='" + curSem + "' and subject_no='" + lblSubNo.Text + "') update subjectChooser set subtype_no='" + lblSubTypeNo.Text + "' , staffcode='" + lblStaffCode.Text + "'  where roll_no='" + rollNo + "' and semester='" + curSem + "' and subject_no='" + lblSubNo.Text + "'  else insert into subjectChooser (semester,roll_no,subject_no,subtype_no,staffcode) values ('" + curSem + "','" + rollNo + "','" + lblSubNo.Text + "','" + lblSubTypeNo.Text + "','" + lblStaffCode.Text + "')";

                                // dirAcc.updateData(insUpdQ);
                                int val = d2.update_method_with_parameter("ElectiveIns", ht, "sp");
                                SaveCheck = true;
                                //Countvalue++;
                            }
                            else
                            {
                                Countvalue++;
                            }
                        }
                    }
                    if (SaveCheck == true)
                    {
                        if (Countvalue > 0)
                        {
                            btnSave.Visible = true;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('You have still not select " + Countvalue + " Elective group Subjects')", true);
                        }
                        else
                        {
                            btnSave.Visible = false;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                        }
                        showTimeTable(appNo);
                    }
                }
                else
                {

                    if (!chkSelected)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select subjects')", true);
                    }
                    else if (duplicateSubject)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Same subject selected more than once')", true);
                    }
                    else if (sameElectGroup)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Only one subject can be chosed from each Elective group')", true);
                    }
                    else
                    {
                        int Remain = (count - arrElectiveGroup.Count);
                        if (Remain < 0)
                        {
                            Remain = count;
                        }
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select " + (Remain) + " subjects')", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please logout and try later')", true);
            }

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Try Later')", true);
        }
    }
    private void showTimeTableSingle(string appNo, string section)
    {
        try
        {
            string selQ = "SELECT R.Stud_Name,A.app_formno, R.Roll_No,r.Reg_No,R.Roll_Admit, R.Batch_Year, R.degree_code,(C.Course_Name+' '+DT.Dept_Name) AS BRANCH,r.Current_Semester,r.college_code,isnull(r.Sections,'') as Sections FROM applyn A,Registration R,Degree D, Department DT, Course C WHERE R.App_No=A.app_no AND R.degree_code=D.Degree_Code AND D.Dept_Code=DT.Dept_Code AND D.Course_Id =C.Course_Id AND R.App_No='" + appNo + "'";
            DataTable dtStudDet = dirAcc.selectDataTable(selQ);
            if (dtStudDet.Rows.Count > 0)
            {
                string studName = Convert.ToString(dtStudDet.Rows[0]["Stud_Name"]);
                string appFormNo = Convert.ToString(dtStudDet.Rows[0]["app_formno"]);
                string regNo = Convert.ToString(dtStudDet.Rows[0]["Reg_No"]);
                string branch = Convert.ToString(dtStudDet.Rows[0]["BRANCH"]);
                string degCode = Convert.ToString(dtStudDet.Rows[0]["degree_code"]);
                string batch = Convert.ToString(dtStudDet.Rows[0]["Batch_Year"]);
                string colCode = Convert.ToString(dtStudDet.Rows[0]["college_code"]);
                string curSem = Convert.ToString(dtStudDet.Rows[0]["Current_Semester"]);
                //string section = Convert.ToString(dtStudDet.Rows[0]["Sections"]).Trim();

                lblStudName.Text = studName;
                lblAppFormNo.Text = regNo;
                lblAppNo.Text = appNo;
                lblBranchDisp.Text = branch;
                lblBranch.Text = degCode;
                lblBatch.Text = batch;
                lblCollegeCode.Text = colCode;
                lblSem.Text = curSem;

                string selTTSec = "  and ct.TT_sec='" + section + "'";
                if (string.IsNullOrEmpty(section))
                {
                    selTTSec = string.Empty;
                }

                getbindClassTT(batch, degCode, colCode, curSem, section, grdClass_TT, grdClassDet_TT);
            }
        }
        catch { }
    }
    protected void tmrTTStat_OnTick(object sender, EventArgs e)
    {
        try
        {
            //showTimeTable(lblAppNo.Text.Trim());
        }
        catch { }
    }
    protected void btnConfirm_OnClick(object sender, EventArgs e)
    {
        try
        {
            string datetime = DateTime.Now.ToString("MM/dd/yyyy");
            string OTP = Convert.ToString(txt_OTP.Text);
            string TextMobile = Convert.ToString(txt_Mobile.Text);
            string checkCode = " select app_no ,isnull(ISOTP,'0') as OTP, convert(varchar(10), OTPDateTime,108) as OTPDateTime ,DATEDIFF(MINUTE,OTPDateTime,GETDATE ()) from applyn where OTPNumber='" + OTP + "' and student_Mobile='" + TextMobile + "' and app_no='" + ViewState["AppNo"].ToString() + "' and CONVERT(varchar(10),OTPDateTime,101)='" + datetime + "' and  DATEDIFF(MINUTE,OTPDateTime,GETDATE ())<5";
            DataTable dtOTP = dirAcc.selectDataTable(checkCode);
            string app_no = string.Empty;
            if (dtOTP.Rows.Count == 0)
            {
                errorspan.Visible = true;
                errorspan.InnerHtml = "Invalid OTP Number or Mobile Number";
            }
            else
            {
                if (dtOTP.Rows.Count > 0)
                {
                    string date = Convert.ToString(dtOTP.Rows[0]["OTPDateTime"]);
                    app_no = Convert.ToString(dtOTP.Rows[0]["app_no"]);
                    string Query = "update applyn set ISOTP='1' where OTPNumber ='" + OTP + "' and app_no='" + app_no + "'";
                    int upd = d2.update_method_wo_parameter(Query, "Text");
                    if (checkLoginTime(app_no))
                    {
                        errorspan.Visible = false;
                        showTimeTable(app_no);
                    }
                    else
                    {
                        errorspan.Visible = true;
                        errorspan.InnerHtml = "Login restricted";
                    }
                }
            }

        }
        catch
        {

        }
    }
    private bool checkLoginTime(string appNo)
    {
        bool IsLoginOk = false;
        try
        {
            string selQ = "select TT_SetPk from Registration r,TT_SelectionSettings s where r.batch_year=s.batchyear and r.college_code=s.collegeCode and r.degree_code=s.degreeCode and r.app_no='" + appNo + "' and '" + DateTime.Now + "' between s.LoginTimeFrom and s.LoginTimeTo";
            int res = dirAcc.selectScalarInt(selQ);
            if (res > 0)
            {
                IsLoginOk = true;
            }
        }
        catch { IsLoginOk = false; }
        IsLoginOk = true;
        return IsLoginOk;
    }
    private bool checkTimeTableTime(string appNo)
    {
        bool IsTTOk = false;
        try
        {
            string selQ = "select TT_SetPk from Registration r,TT_SelectionSettings s where r.batch_year=s.batchyear and r.college_code=s.collegeCode and r.degree_code=s.degreeCode and r.app_no='" + appNo + "' and '" + DateTime.Now + "' between s.TTSelectTimeFrom and s.TTSelectTimeTo";
            int res = dirAcc.selectScalarInt(selQ);
            if (res > 0)
            {
                IsTTOk = true;
            }
        }
        catch { IsTTOk = false; }
        IsTTOk = true;
        return IsTTOk;
    }
    private bool checkElectiveTime(string appNo)
    {
        bool IsETOk = false;
        try
        {
            string selQ = "select TT_SetPk from Registration r,TT_SelectionSettings s where r.batch_year=s.batchyear and r.college_code=s.collegeCode and r.degree_code=s.degreeCode and r.app_no='" + appNo + "' and '" + DateTime.Now + "' between s.ElectiveSelectFrom and s.ElectiveSelectTo";
            int res = dirAcc.selectScalarInt(selQ);
            if (res > 0)
            {
                IsETOk = true;
            }
            IsETOk = true;
        }
        catch { IsETOk = false; }
        return IsETOk;
    }
}